﻿using System.Text.Json;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.CommandsNext;
using Rezet.Commands;
using Rezet.Events;


#pragma warning disable CS8602
#pragma warning disable CS8604
namespace Rezet
{
    class Program
    {
        public static DiscordClient? Rezet; public static CommandsNextExtension? commands;
        public static DatabaseService? _databaseService; private static readonly TimeSpan StatusUpdateInterval = TimeSpan.FromMinutes(10);
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("START ==================================================="); Console.ResetColor();
            var configPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "obj", "config.json"); var configJson = await File.ReadAllTextAsync(configPath); var config = JsonSerializer.Deserialize<Config>(configJson);


            // ========== CLIENT:
            var rzt = new DiscordConfiguration
            {
                Token = config?.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                ReconnectIndefinitely = true,
                GatewayCompressionLevel = GatewayCompressionLevel.Payload,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Error
            };
            Rezet = new DiscordClient(rzt);
            commands = Rezet.UseCommandsNext(new CommandsNextConfiguration { StringPrefixes = ["-r "], CaseSensitive = false, EnableDms = false, EnableMentionPrefix = true });
            var interactivity = Rezet.UseInteractivity(new DSharpPlus.Interactivity.InteractivityConfiguration() { Timeout = TimeSpan.FromMinutes(5) });
            DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"O [  {y}  |  REZET  ] Client created.");
            Console.ResetColor();


            // ========== DATABASE CONFIGS:
            var connectionString = config?.Keydb;
            var databaseName = "extensions";
            _databaseService = new DatabaseService(connectionString, databaseName);


            // ========== START REZET:
            CommandHandler.RegisterCommands(Rezet);
            EventsHandler.RegisterEvents(Rezet);


            await Rezet.ConnectAsync();
            Rezet.Ready += Client_Ready;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"O [  {y}  |  REZET  ] {Rezet.CurrentUser.Username} is ready!!!!!!!!!!.");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"V [  {y}  ]  SHARP  ] Version: Sharp 1.0 | PTB R-0023");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("CNTRL ===================================================");
            Console.ResetColor();
            await Task.Delay(-1);
        }


        private static async Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args) { await UpdateStatusLoop(); }
        private static async Task UpdateStatusLoop()
        {
            while (true)
            {
                var serverCount = Rezet?.Guilds.Count;
                var activity = new DiscordActivity($"Heyo! In {serverCount} communities! [ IN TEST ]", ActivityType.Playing); var userStatus = UserStatus.DoNotDisturb;
                await Rezet.UpdateStatusAsync(activity, userStatus); await Task.Delay(StatusUpdateInterval);
            }
        }
    }
    class Config { public string? Token { get; set; } public string? Keydb { get; set; } }
}