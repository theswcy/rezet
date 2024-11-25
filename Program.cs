// using System.Text.Json;
// using DSharpPlus;
// using DSharpPlus.Entities;
// using DSharpPlus.Interactivity.Extensions;
// using DSharpPlus.CommandsNext;
// using Rezet.Commands;
// using Rezet.Events;



// #pragma warning disable CS8602
// #pragma warning disable CS8604
// namespace TRezet
// {
//     class Program
//     {
//         public static DiscordClient?Rezet;
//         public static CommandsNextExtension? commands;
//         public static DatabaseService? _databaseService;
//         private static readonly TimeSpan StatusUpdateInterval = TimeSpan.FromMinutes(10);
//         static async Task Main()
//         {
//             try
//             {
//                 Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("\n\n\n       ⚯  Builder Started!"); Console.ResetColor();
//                 var configPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "obj", "config.json"); var configJson = await File.ReadAllTextAsync(configPath); var config = JsonSerializer.Deserialize<Config>(configJson);


//                 // ========== CLIENT:
//                 var rzt = new DiscordConfiguration
//                 {
//                     Token = config?.Token,
//                     TokenType = TokenType.Bot,
//                     Intents = DiscordIntents.All,
//                     ReconnectIndefinitely = true,
//                     GatewayCompressionLevel = GatewayCompressionLevel.Payload,
//                     MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Error
//                 };
//                 Rezet = new DiscordClient(rzt);
//                 commands = Rezet.UseCommandsNext(new CommandsNextConfiguration { StringPrefixes = ["-r "], CaseSensitive = false, EnableDms = false, EnableMentionPrefix = true });
//                 var interactivity = Rezet.UseInteractivity(new DSharpPlus.Interactivity.InteractivityConfiguration() { Timeout = TimeSpan.FromMinutes(5) });
//                 DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
//                 Console.Write("    ➜  ");
//                 Console.ForegroundColor = ConsoleColor.Blue;
//                 Console.Write($"  {y}  |  REZET  ⚯   Client created.\n");
//                 Console.ResetColor();


//                 // ========== DATABASE CONFIGS:
//                 var connectionString = config?.Keydb;
//                 var databaseName = "extensions";
//                 _databaseService = new DatabaseService(connectionString, databaseName);


//                 // ========== START REZET:
//                 CommandHandler.RegisterCommands(Rezet);
//                 EventsHandler.RegisterEvents(Rezet);


//                 await Rezet.ConnectAsync();
//                 Rezet.Ready += Client_Ready;
//                 Console.ResetColor();
//                 Console.Write("    ➜  ");
//                 Console.ForegroundColor = ConsoleColor.Blue;
//                 Console.Write($"  {y}  |  REZET  ⚯   {Rezet.CurrentUser.Username} is ready!!!!!!!!!!.\n");
//                 Console.ResetColor();
//                 Console.Write("    ➜  ");
//                 Console.ForegroundColor = ConsoleColor.Cyan;
//                 Console.Write($"  {y}  |  SHARP  ⚯   Version: Sharp 1.5\n");
//                 Console.ForegroundColor = ConsoleColor.White;
//                 Console.Write("       ⚯  Builder Finished!\n\n");
//                 Console.ForegroundColor = ConsoleColor.White;
//                 await Task.Delay(-1);
//             }
//             catch (Exception ex)
//             {
//                 DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");

//                 Console.ForegroundColor = ConsoleColor.White;
//                 Console.Write("    ➜  ");
//                 Console.ForegroundColor = ConsoleColor.DarkYellow;
//                 Console.Write($"{y}  | Compiler Error!\n\n      ╭  {ex.GetType()}  /  by: {ex.Source}\n{ex.StackTrace}\n\n");
//                 Console.ForegroundColor = ConsoleColor.White;
//                 Console.Write("       ⚯  Error:\n");
//                 Console.ForegroundColor = ConsoleColor.DarkRed;
//                 Console.Write($"      {ex.Message}\n\n");
//                 Console.ForegroundColor = ConsoleColor.White;
//                 return;
//             }
//         }


//         private static async Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args) { await UpdateStatusLoop(); }
//         private static async Task UpdateStatusLoop()
//         {
//             while (true)
//             {
//                 var serverCount = Rezet?.Guilds.Count;
//                 var activity = new DiscordActivity($"Heyo! In {serverCount} communities!", ActivityType.Playing); var userStatus = UserStatus.DoNotDisturb;
//                 await Rezet.UpdateStatusAsync(activity, userStatus); await Task.Delay(StatusUpdateInterval);
//             }
//         }
//     }
//     class Config { public string? Token { get; set; } public string? Keydb { get; set; } }
// }



using System;
namespace CS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await RezetSharp.EngineV1.AwaysOnCore();
        }
    }
}


