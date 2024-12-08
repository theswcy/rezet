using DSharpPlus;
using DSharpPlus.EventArgs;
using RezetSharp;



#pragma warning disable CS8602
public class AwaysOnCore
{
    public static async Task OnSocketOpened(DiscordClient sender, EventArgs e)
    {
        DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"    ➜  {y}  |  EngineV8X Started\n       Socket Opened! 🔥");
        Console.ResetColor();
        await Task.CompletedTask;
    }
    public static async Task OnSocketClosed(DiscordClient sender, SocketCloseEventArgs e)
    {
            DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            int RetryCount = 0;
            while (RetryCount < 15)
            {
                Console.WriteLine($"    ➜  {y}  |  EngineV8X Error\n       Socket Connection Closed:\n       {e.CloseMessage}");
                try
                {
                    await EngineV8X.EngineStart();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  {y}  |  EngineV8X Error\n       Error in EngineV8X restart:\n       {ex.Message}\n       {e.CloseMessage}");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
            Console.WriteLine($"    ➜  {y}  |  EngineV8X Error\n       The engine will not restart:\n\n       {e.CloseMessage}");
    }
    public static async Task OnSocketErrored(DiscordClient sender, SocketErrorEventArgs e)
    {
        EngineV8X.RezetRazor.SocketErrored += async (sender, e) =>
        {
            DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.WriteLine($"    ➜  {y}  |  EngineV8X Error\n       Socket Connection Errored:\n       {e.Exception.Message}");
            await Task.CompletedTask;
        };
        await Task.CompletedTask;
    }
}