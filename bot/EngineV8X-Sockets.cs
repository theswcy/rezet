using DSharpPlus;
using DSharpPlus.EventArgs;




namespace RezetSharp.AOCore
{
    public class AwaysOnCore
    {
        private static DateTime _lastHeartBeat = DateTime.Now;




        public static async Task OnHeartBeated(DiscordClient sender, HeartbeatEventArgs e)
        {
            _lastHeartBeat = DateTime.Now;
            Console.WriteLine($"    ➜    {_lastHeartBeat}  |  ⚡ EngineV8X AOCore Heartbeat\n    ⚯   🧬 Lantency: {e.Ping}ms");
            await Task.CompletedTask;
        }
        public static void CheckZombieConnection(object state)
        {
            var DiscordHeart = (DiscordClient)state;
            var timeSinceLastHeartbeat = DateTime.Now - _lastHeartBeat;
            if (timeSinceLastHeartbeat.TotalSeconds > 120)
            {
                Console.WriteLine($"    ➜    {_lastHeartBeat}  |  ⚡ EngineV8X AOCore Heartbeat\n     ⚯   🔴 Zombie connection detected! Triyng to reconnect...");
                Task.Run(async () =>
                {
                    try
                    {
                        await DiscordHeart.ConnectAsync();
                        Console.WriteLine($"    ➜    {_lastHeartBeat}  |  ⚡ EngineV8X AOCore Heartbeat\n     ⚯   🟢 Client connected!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜    {_lastHeartBeat}  |  ⚡ EngineV8X AOCore Heartbeat\n     ⚯   🔴 Zombie connection win.\n{ex.Message}");
                    }
                });
            }
        }




        public static async Task OnSocketOpened(DiscordClient sender, EventArgs e)
        {
            DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"    ➜    {y}  ・  ⚡ EngineV8X AOCore Started\n    ⚯   🔥 Socket Opened!");
            Console.ResetColor();
            await Task.CompletedTask;
        }




        public static async Task OnSocketClosed(DiscordClient sender, SocketCloseEventArgs e)
        {
            DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            int RetryCount = 0;
            while (RetryCount < 15)
            {
                Console.WriteLine($"    ➜    {y}  ・  ⚡ EngineV8X AOCore Error\n    ⚯   🟠 Socket Connection Closed\n    ➜  {e.CloseMessage}");
                try
                {
                    await sender.ConnectAsync();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜    {y}  ・  ⚡ EngineV8X AOCore Error\n    ⚯   🔴 Error on client reconnect, retrying...\n    ➜  {ex.Message}\n\n{e.CloseMessage}");
                    RetryCount++;
                    await Task.Delay(TimeSpan.FromSeconds(7));
                }
            }
            Console.WriteLine($"    ➜    {y}  ・  EngineV8X AOCore Error\n    ⚯   🔴 The client will not reconnect\n\n{e.CloseMessage}");
        }




        public static async Task OnSocketErrored(DiscordClient sender, SocketErrorEventArgs e)
        {
            DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.WriteLine($"    ➜    {y}  ・  ⚡ EngineV8X AOCore Error\n    ⚯   🔴 Socket Connection Errored\n       {e.Exception.Message}");
            await Task.CompletedTask;
        }
    }
}