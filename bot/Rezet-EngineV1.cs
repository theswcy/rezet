using System.Text.Json;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.CommandsNext;
using Rezet.Commands;
using Rezet.Events;




#pragma warning disable CS8604
#pragma warning disable CS8602
namespace RezetSharp
{
    // ========== REZET ENGINE V1.0
    class EngineV1
    {
        // ========== IGNITES:
        public static DiscordClient? RezetRazor;
        public static CommandsNextExtension? CommandsRazor;
        public static HerrscherService? HerrscherRazor;
        private static readonly TimeSpan StatusUpdateInterval = TimeSpan.FromMinutes(1);




        // ========== AOCORE:
        public static async Task AwaysOnCore()
        {
            await EngineStart();
            RezetRazor.SocketOpened += async (sender, e) =>
            {
                DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
                Console.WriteLine($"    ➜  {y}  |  EngineV1 Started\n       Socket Opened!");
                await Task.CompletedTask;
            };
            RezetRazor.SocketClosed += async (sender, e) =>
            {
                DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
                int RetryCount = 0;
                while (RetryCount < 10)
                {
                    Console.WriteLine($"    ➜  {y}  |  EngineV1 Error\n       Socket Connection Closed:\n       {e.CloseMessage}");
                    try
                    {
                        await EngineStart();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  {y}  |  EngineV1 Error\n       Error in EngineV1 restart:\n       {ex.Message}\n       {e.CloseMessage}");
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }
                }
                Console.WriteLine($"    ➜  {y}  |  EngineV1 Error\n       The engine will not restart:\n\n       {e.CloseMessage}");
            };
            RezetRazor.SocketErrored += async (sender, e) => 
            {
                DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
                Console.WriteLine($"    ➜  {y}  |  EngineV1 Error\n       Socket Connection Errored:\n       {e.Exception.Message}");
                await Task.CompletedTask;
            };
        }




        // ========== STATUS UPDATE:
        private static async Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            await UpdateStatusLoop();
        }
        private static async Task UpdateStatusLoop()
        {
            while (true)
            {
                var serverCount = RezetRazor?.Guilds.Count;
                var activity = new DiscordActivity($"Heyo! In {serverCount} communities!", ActivityType.Playing); var userStatus = UserStatus.DoNotDisturb;
                await RezetRazor.UpdateStatusAsync(activity, userStatus); await Task.Delay(StatusUpdateInterval);
            }
        }




        // ========== ENGINE START:
        private static async Task EngineStart()
        {
            try
            {
                // ========== GET THE TOKEN:
                Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("\n\n\n       ⚯  Engine V1 Started!"); Console.ResetColor();
                var configPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "obj", "config.json");
                var configJson = await File.ReadAllTextAsync(configPath);
                var config = JsonSerializer.Deserialize<Config>(configJson);



                // ========== CLIENT BUILDER:
                var rzt = new DiscordConfiguration
                {
                    Token = config?.Token,
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.All,
                    ReconnectIndefinitely = true,
                    GatewayCompressionLevel = GatewayCompressionLevel.Payload,
                    MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Error
                };
                RezetRazor = new DiscordClient(rzt);
                CommandsRazor = RezetRazor.UseCommandsNext(
                    new CommandsNextConfiguration
                    {
                        StringPrefixes = ["-r "],
                        CaseSensitive = false,
                        EnableDms = false,
                        EnableMentionPrefix = true
                    }
                );
                var Interactivity = RezetRazor.UseInteractivity(
                    new DSharpPlus.Interactivity.InteractivityConfiguration()
                    {
                        Timeout = TimeSpan.FromMinutes(5)
                    }
                );
                DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
                Console.Write("    ➜  ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"  {y}  |  REZET  ⚯   Client created.\n");
                Console.ResetColor();




                // ========== HERRSCHER CONFIGURATION:
                var ConnectionString = config?.Keydb;
                var HerrscherExtension = "extensions";
                HerrscherRazor = new HerrscherService(ConnectionString, HerrscherExtension);




                // ========== REGISTER COMMANDS AND EVENTS:
                CommandHandler.RegisterCommands(RezetRazor);
                EventsHandler.RegisterEvents(RezetRazor);




                // ========== START!
                await RezetRazor.ConnectAsync();
                RezetRazor.Ready += Client_Ready;
                Console.ResetColor();
                Console.Write("    ➜  ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"  {y}  |  REZET  ⚯   {RezetRazor.CurrentUser.Username} is ready!!!!!!!!!!.\n");
                Console.ResetColor();
                Console.Write("    ➜  ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"  {y}  |  SHARP  ⚯   Version: Sharp 1.5\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"       ⚯  Engine V1 working!\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("    ➜  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{y}  | Compiler Error!\n\n      ╭  {ex.GetType()}  /  by: {ex.Source}\n{ex.StackTrace}\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("       ⚯  Error:\n");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"      {ex.Message}\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
        }
    }
    class Config { public string? Token { get; set; } public string? Keydb { get; set; } }
}