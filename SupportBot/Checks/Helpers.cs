﻿// ***********************************************************************
// Assembly         : SupportBot
// Author           : Nathan Pipes
// Created          : 02-23-2021
//
// Last Modified By : Nathan Pipes
// Last Modified On : 09-08-2021
// ***********************************************************************
// <copyright file="Helpers.cs" company="NPipes">
//     Copyright (c) NPipes. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SupportBot.Checks.Modal;

namespace SupportBot.Checks
{
    /// <summary>
    /// Taken from the wonderful https://github.com/codingfreak/cfUtils library
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// Checks using the Steam Public API what is currently running on the specified server
        /// </summary>
        /// <param name="address">The hostname or IP of the server to check</param>
        /// <returns>List of servers running, or an error message</returns>
        public static async Task<string> CheckSteam(string address)
        {
            try
            {
                var sb = new StringBuilder();

                if (!Helpers.CheckIpValid(address))
                {
                    var entries = await Dns.GetHostAddressesAsync(address);
                    if (entries.Length > 1)
                    {
                        sb.Append(
                            "Multiple addresses found for this host, only the first is checked, please specify the IP you want to check next time.\n");
                    }

                    //Set the address to the resolved IP.
                    address = entries[0].ToString();
                }

                using var webClient = new WebClient();
                //We are not using Async here as this will cause the method to hang the entire Task.
                var result = JsonSerializer.Deserialize<SteamApiResponse>(
                    webClient.DownloadString(
                        $"https://api.steampowered.com/ISteamApps/GetServersAtAddress/v0001?addr={address}"));

                if (result == null || !result.response.success)
                {
                    return await Task.FromResult("Steam API resulted in a failure. Try again later.");
                }

                var totalServers = result.response.servers.Length;

                sb.Append("Steam can see the following:\n");

                foreach (var item in result.response.servers)
                {
                    sb.Append(
                        $"**{item.gamedir}**\nApp ID: {item.appid}\nIs Secure: {item.secure}\nIs Lan:{item.lan}\nGame Port:{item.gameport}\nSpec Port:{item.specport}\n");
                }

                sb.Append($"Total Servers: {totalServers}");

                var response = sb.ToString();
                return await Task.FromResult(response);
            }
            catch (Exception)
            {
                // ignored
            }

            return await Task.FromResult("Unable to check with steam, sorry about that.");
        }

        /// <summary>
        /// Tries to open a network connection to a specific port retrieving the result.
        /// </summary>
        /// <param name="host">IP-Address or host name to check for the port.</param>
        /// <param name="port">The port-number to check.</param>
        /// <param name="timeoutSeconds">The timeoutSeconds in seconds to wait for a reply. Defaults to 2 because 1 second is
        /// mostly too short for .NET.</param>
        /// <param name="useUdp"><c>true</c> if a UDP port should be checked.</param>
        /// <returns>The result of the operation.</returns>
        public static PortState GetPortState(string host, int port, int timeoutSeconds = 2, bool useUdp = false)
        {
            var outerResult = PortState.Unknown;
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
            var token = tokenSource.Token;
            try
            {
                // use a task to enable outer cancellation regardless of the asyncResult-timeoutSeconds which isn't working very well
                outerResult = Task.Run(
                    () =>
                    {
                        var result = PortState.Unknown;
                        if (!useUdp)
                        {
                            // Use TCP
                            var client = new TcpClient();
                            try
                            {
                                var asyncResult = client.BeginConnect(host, port, null, null);
                                var waitHandle = asyncResult.AsyncWaitHandle;
                                try
                                {
                                    if (asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeoutSeconds),
                                        false))
                                    {
                                        // The result was positive
                                        if (!asyncResult.IsCompleted)
                                        {
                                            result = PortState.TimedOut;
                                        }
                                        else
                                        {
                                            result = client.Connected ? PortState.Open : PortState.Closed;
                                        }
                                    }

                                    // ensure the ending-call
                                    client.EndConnect(asyncResult);
                                }
                                finally
                                {
                                    // Ensure to close the wait handle.
                                    waitHandle.Close();
                                }
                            }
                            catch (SocketException sockEx)
                            {
                                // see https://msdn.microsoft.com/en-us/library/ms740668.aspx for a list of all states
                                result = sockEx.NativeErrorCode switch
                                {
                                    10060 => PortState.TimedOut,
                                    10061 => PortState.Refused,
                                    _ => result
                                };
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                            finally
                            {
                                // wait handle didn't came back in time
                                client.Close();
                            }
                        }
                        else
                        {
                            // Use UDP
                            var client = new UdpClient();
                            try
                            {
                                client.Connect(host, port);
                                var asyncResult = client.BeginReceive(
                                    _ => { },
                                    null);
                                asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeoutSeconds), false);
                                if (!asyncResult.IsCompleted)
                                {
                                    return PortState.TimedOut;
                                }

                                result = PortState.Open;
                            }
                            catch (SocketException sockEx)
                            {
                                result = sockEx.NativeErrorCode switch
                                {
                                    // see https://msdn.microsoft.com/en-us/library/ms740668.aspx for a list of all states
                                    10060 => PortState.TimedOut,
                                    10061 => PortState.Refused,
                                    _ => result
                                };
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                            finally
                            {
                                // wait handle didn't came back in time
                                client.Close();
                            }
                        }

                        return result;
                    },
                    token).ContinueWith(
                    t =>
                    {
                        if (t.IsCanceled || token.IsCancellationRequested)
                        {
                            return PortState.TimedOut;
                        }

                        return t.Result;
                    },
                    token).Result;
            }
            catch (AggregateException aex)
            {
                var flatten = aex.Flatten();
                if (flatten.InnerException is TaskCanceledException)
                {
                    outerResult = PortState.TimedOut;
                }
            }
            catch
            {
                // empty catch
            }

            return outerResult;
        }

        /// <summary>
        /// Checks if the IP address valid.
        /// </summary>
        /// <param name="strIp">The IP address to check</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CheckIpValid(string strIp)
        {
            return !string.IsNullOrEmpty(strIp) && IPAddress.TryParse(strIp, out _);
        }
    }

    /// <summary>
    /// Defines all the possible port states
    /// </summary>
    public enum PortState
    {
        /// <summary>
        /// Unknown port state
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The port is open
        /// </summary>
        Open = 1,

        /// <summary>
        /// The port is closed
        /// </summary>
        Closed = 2,

        /// <summary>
        /// Timeout trying to reach the port
        /// </summary>
        TimedOut = 3,

        /// <summary>
        /// Connection refused.
        /// </summary>
        Refused = 4
    }
}