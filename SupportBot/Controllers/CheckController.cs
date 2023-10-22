using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportBot.Modules;

namespace SupportBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        [HttpGet("gamedig")]
        public async Task<IActionResult> GamedigAsync(string type, string address)
        {
            var result = await Helpers.GameDig(type, address);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpGet("steam")]
        public async Task<IActionResult> SteamAsync(string address)
        {
            var result = await Helpers.CheckSteam(address);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }


        [HttpGet("port")]
        public IActionResult Port(string address, int port, bool useUdp)
        {
            return Ok(Enum.GetName(Helpers.GetPortState(address, port, 2, useUdp)));
        }
    }
}
