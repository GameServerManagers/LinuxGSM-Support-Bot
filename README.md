# LGSM-SupportBot
Discord support bot for LinuxGSM

# Adding Triggers
The file: [triggers.json](SupportBot/triggers.json) contains all the triggers the bot will respond to.

If you need to edit or update one of them, please open a pull request with the change.

# Trigger format
The trigger format is done in JSON to make it easier to update and review.
```json
{
   "Name": "0x202",
   "Starters": [
      {
         "Type": "simple",
         "Value": "0x202"
      }
   ],
   "Answer": "https://docs.linuxgsm.com/support/faq#write-error-no-space-left-on-device"
}
 ```
 
 Each trigger can have multiple starters, at this time there are two supported types of starters, `simple` and `regex`
 
 ## Simple Starter
 The simple starter will search each message to see if it contains the specified text, this must be done in lower case as the entire message is cast to lower.
 
 ## Regex Starter
 The regex start will search each message for a match of the regex, if a single match is found the answer will be triggered. 
 
 Note .NET regex is used so no `Nested quantifier +`
 
# What happens if multiple triggers are hit?
The bot will simply collect each answer, and send a single message with all of them along with a notice that multiple potential issues were found.
