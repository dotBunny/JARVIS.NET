using JARVIS.Core.Protocols.OAuth2;
using JARVIS.Shared;
using Microsoft.Extensions.DependencyInjection;

using Discord;

namespace JARVIS.Core.Services.Discord
{
    public class DiscordService : IService
    {
        public const string ScopeAuthentication = "discord-authenticate";

        // Settings Reference Keys
        const string SettingsClientIDKey = "Discord.ClientID";
        const string SettingsClientSecretKey = "Discord.ClientSecret";
        const string SettingsEnabledKey = "Discord.Enabled";
        const string SettingsGuildIDKey = "Discord.GuildID";
        const string SettingsTokenKey = "Discord.Token";
        const string SettingsBotTokenKey = "Discord.BotToken";
        const string SettingsBotUsernameKey = "Discord.BotUsername";


        // Settings Values (pulled from DB)
        public bool Enabled { get; private set; }


        string _botUsername;
        string _botToken;
        string _guildID;

        OAuth2Provider OAuth2 = new OAuth2Provider();
        //IDiscordClient Client = new DiscordClientExtensions.

        public DiscordService()
        {
            // Initialize Settings
            Enabled = Server.Config.GetBool(SettingsEnabledKey);
            if (Enabled)
            {
               LoadSettings();
            }
        }

        void LoadSettings()
        {

            _botToken = Server.Config.Get(SettingsBotTokenKey);
            _botUsername = Server.Config.Get(SettingsBotUsernameKey);
            _guildID = Server.Config.Get(SettingsGuildIDKey);

            OAuth2 = new OAuth2Provider(GetName(), 
                                        Server.Config.Get(SettingsClientIDKey), 
                                        Server.Config.Get(SettingsClientSecretKey),
                                        "bot messages.read connections rpc guilds identify guilds.join webhook.incoming rpc.notifications.read gdm.join email rpc.api",
                                        "https://discordapp.com/api/oauth2/authorize?response_type=code&permissions=8",
                                        "https://discordapp.com/api/oauth2/authorize?response_type=token",
                                        "https://discordapp.com/api/oauth2/authorize?response_type=token",
                                        ScopeAuthentication);

            OAuth2.OnComplete += OAuth2_OnComplete;
        }


        public string GetName()
        {
            return "Discord";
        }


        public void Start()
        {
            if (!Enabled)
            {
                Log.Message("Discord", "Unable to start as service is disabled.");
                return;
            }
           
            if (!OAuth2.IsValid() && Server.Services.GetService<Socket.SocketService>().AuthenticatedUserCount > 0)
            {
                OAuth2.Login();
            }
        }

        public void Stop()
        {
           OAuth2.Reset();
        }

        public void Tick()
        {
            if (!Enabled) return;
        }

        void OAuth2_OnComplete()
        {
            
        }
    }

}
