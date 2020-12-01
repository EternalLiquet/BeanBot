![.NET Core Master and Deploy Checks](https://github.com/EternalLiquet/BeanBot/workflows/.NET%20Core%20Master%20and%20Deploy%20Checks/badge.svg?branch=master)
# Bean Bot
This is the code behind the discord bot for the Illinois Livers server

One of the the primary intentions of this bot is to provide the Illinois Livers server with fun commands to play with.  
The secondary intent of this bot is to provide a gacha style game based on cards on schoolido.lu's API.  
And finally, the final (current) intent of this bot is to provide more sophistication to meetup information and RSVPing  
  
Of course, this can be used for any server.  
  
Discord link to the Illinois Server: https://discord.gg/a9hbx9S  
  
# Bean Bot Setup Instructions
  
## Necessary Installations
To run Bean Bot, you will need ASP.NET Core runtime 5.0 found here: https://dotnet.microsoft.com/download/dotnet/5.0
Optionally, if you have Windows 10 Pro, you can also download Docker for Desktop here: https://www.docker.com/products/docker-desktop
  
## Setup Steps
First, you will need to create a bot on Discord. You can do that with the following guide: https://discord.foxbot.me/docs/guides/getting_started/first-bot.html  
Once you have a bot token, you can invite your bot to the server. 
  
After that, you will need to create a file named hostSettings.json in the Config folder. 
The file should have the following:
```
{
  "environment": "Development",
  "LOG_LEVEL": "DEBUG",
  "DISCORD_BOT_TOKEN": "<INSERT BOT TOKEN HERE>",
}
```

Afterwards, run the program either by:

     Using Visual Studio and hitting the play button (If you do not have Docker Desktop, the play button must say "Bean Bot" and not "Docker")

or


    • Navigate to the folder containing the .sln in the base of the repo.  
    • Run command `dotnet build --configuration Debug`  
    • This will build the solution in Debug mode. Navigate to the BeanBot/bin/Debug/net5.0 folder.   
    • Run command `dotnet BeanBot.dll`

If you have configured your settings correctly, the bot should connect to your server, enjoy!