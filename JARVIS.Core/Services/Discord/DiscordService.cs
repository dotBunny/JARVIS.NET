using JARVIS.Core.Protocols.OAuth2;
using JARVIS.Shared;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading;
using System;

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
        bool _connected;

        DiscordSocketClient _client;
        IServiceCollection _map = new ServiceCollection();
        CommandService _commands = new CommandService();

        public DiscordService()
        {
            // Initialize Settings
            Enabled = Server.Config.GetBool(SettingsEnabledKey);
            if (Enabled)
            {
               LoadSettings();
            }
            _commands.Log += DiscordLog;
        }

        void LoadSettings()
        {

            _botToken = Server.Config.Get(SettingsBotTokenKey);
            _botUsername = Server.Config.Get(SettingsBotUsernameKey);
            _guildID = Server.Config.Get(SettingsGuildIDKey);
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

            _client = new DiscordSocketClient();
            _client.Log += DiscordLog;
           
            // Let us know if things disconnect
            _client.Disconnected += (arg) =>
            {
                if (arg != null)
                {
                    Log.Message("Discord", arg.Message);
                }
                return Task.CompletedTask;
            };

            // Let us know if things are ready.
            _client.Ready += () =>
            {
                Log.Message("Discord", "Connected!");
                return Task.CompletedTask;
            };

            MainAsync().GetAwaiter();
        }

        public async Task MainAsync()
        {
            await InitCommands();
            await _client.LoginAsync(TokenType.Bot, SettingsBotTokenKey);
            await _client.StartAsync();
           // await Task.Delay(Timeout.Infinite);
        }

        async Task InitCommands()
        {
            // Repeat this for all the service classes
            // and other dependencies that your commands might need.
            //_map.AddSingleton(new SomeServiceClass());

            // When all your required services are in the collection, build the container.
            // Tip: There's an overload taking in a 'validateScopes' bool to make sure
            // you haven't made any mistakes in your dependency graph.
            //_services = _map.BuildServiceProvider();

            // Either search the program and add all Module classes that can be found.
            // Module classes MUST be marked 'public' or they will be ignored.
            //await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            //// Or add Modules manually if you prefer to be a little more explicit:
            //await _commands.AddModuleAsync<SomeModule>();
            //// Note that the first one is 'Modules' (plural) and the second is 'Module' (singular).

            //// Subscribe a handler to see if a message invokes a command.
            _client.MessageReceived += HandleCommandAsync;
        }

        public void Stop()
        {
            if (_connected )
            {
                _client.LogoutAsync();
                _client.StopAsync();    
            }

            _client.Dispose();
        }

        public void Tick()
        {
        }



        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Bail out if it's a System Message.
            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            // We don't want the bot to respond to itself or other bots.
            // NOTE: Selfbots should invert this first check and remove the second
            // as they should ONLY be allowed to respond to messages from the same account.
            if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot) return;

            // Create a number to track where the prefix ends and the command begins
            int pos = 0;
            // Replace the '!' with whatever character
            // you want to prefix your commands with.
            // Uncomment the second half if you also want
            // commands to be invoked by mentioning the bot instead.
            if (msg.HasCharPrefix('!', ref pos) /* || msg.HasMentionPrefix(_client.CurrentUser, ref pos) */)
            {
                if ( msg.Content == "!ping") {
                    await msg.Channel.SendMessageAsync("Pong!");
                }

                //// Create a Command Context.
                //var context = new SocketCommandContext(_client, msg);

                //// Execute the command. (result does not indicate a return value, 
                //// rather an object stating if the command executed successfully).
                //var result = await _commands.ExecuteAsync(context, pos, _services);

                //// Uncomment the following lines if you want the bot
                //// to send a message if it failed (not advised for most situations).
                ////if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                ////    await msg.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        Task DiscordLog(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Log.Fatal("Discord", message.Source + ": " + message.Message + message.Exception);
                    break;
                case LogSeverity.Warning:
                    Log.Error("Discord", message.Source + ": " + message.Message + message.Exception);
                    break;
                case LogSeverity.Info:
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Log.Message("Discord", message.Source + ": " + message.Message + message.Exception);
                    break;
            }
            return Task.CompletedTask;
        }
    }

}
