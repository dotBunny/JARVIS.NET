using System;
using Grapevine.Interfaces.Server;
using Discord.Rest;

namespace JARVIS.Core.Services.Discord
{
    public class DiscordService : IService
    {
        public const string ScopeAuthentication = "discord-authenticate";       

        // Settings Reference Keys
        const string SettingsEnabledKey = "Discord.Enabled";
        const string SettingsGuildIDKey = "Discord.GuildID";
        const string SettingsTokenKey = "Discord.Token";

        // Settings Values (pulled from DB)
        public bool Enabled { get; private set; }
        string _guildID;
        string _token;

        public DiscordService()
        {
            // Initialize Settings
            Enabled = Server.Config.GetBool(SettingsEnabledKey);
            if (Enabled)
            {
                _guildID = Server.Config.Get(SettingsGuildIDKey);
                _token = Server.Config.Get(SettingsTokenKey);
            }
        }

        //DiscordRestConfig Config;
        //DiscordRestClient Client;
        //RestGuild Guild;

        public string GetName()
        {
            return "Discord";
        }

        public void HandleCallbackAsync(IHttpRequest request)
        {

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

        public void Tick()
        {

        }
    }

}
