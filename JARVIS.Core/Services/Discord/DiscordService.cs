﻿using System;
//using Discord.Rest;

namespace JARVIS.Core.Services.Discord
{
    public class DiscordService : IService
    {
        //DiscordRestConfig Config;
        //DiscordRestClient Client;
        //RestGuild Guild;

        public string GetName()
        {
            return "Discord";
        }

        public void Start()
        {
            //DiscordClient c = new DiscordClient();
            //Config = new DiscordRestConfig();
            //Config.
            //      ]
            //      ]var config = new DiscordRestConfig
            //       {
            //           RestClientProvider = url =>
            //           {
            //               _cache.SetUrl(url);
            //               return _cache;
            //           }
            //       };
            //Config.RestClientProvider = new Discord.Net.Rest.RestClientProvider();
            //)

            //Client = new DiscordRestClient(Config);

            //Client.LoginAsync(Discord.TokenType.Bot, Server.Config.Get("Discord.Token")).Wait();

            ////MigrateAsync().Wait();
            //Guild = Client.GetGuildAsync(ulong.Parse(Server.Config.Get("Discord.GuildID"))).Result;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }

}