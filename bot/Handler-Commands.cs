using DSharpPlus;
using DSharpPlus.SlashCommands;
using Rezet;
using RezetSharp;





namespace Rezet.Commands
{
    public static class CommandHandler
    {
        #pragma warning disable CS8602
        public static void RegisterCommands(DiscordClient client)
        {
            var prefix = EngineV1.CommandsRazor;
            var slash = client.UseSlashCommands(null);
            // slash.RegisterCommands<RezetGets>(null);
            slash.RegisterCommands<RezetSystems>(null);
            prefix.RegisterCommands<RezetGets_prefix>();


            // COMMUNITY:
            slash.RegisterCommands<CommunityCommands>(null);
            slash.RegisterCommands<CommunityRole>(null);
            slash.RegisterCommands<CommunityChats>(null);
            prefix.RegisterCommands<CommunityCommands_prefix>();


            // PARTNERSHIP:
            slash.RegisterCommands<PartnershipCommands>(null);
            slash.RegisterCommands<CommunityPartnerMore>(null);
            slash.RegisterCommands<PartnershipTickets>(null);


            // MODERATION:
            slash.RegisterCommands<ToModerationBasic_slash>(null);
            slash.RegisterCommands<ModdingLogs>(null);


            // AUTOMATIC:
            slash.RegisterCommands<AutoRoleSettings>(null);
            slash.RegisterCommands<AutoPingSettings>(null);
            slash.RegisterCommands<Moderators>(null);




            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ResetColor();
            Console.Write("    ➜  ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"  {y}  |  REZET  ⚯   All slash-commands synchronized!\n");
            Console.ResetColor();
            Console.ResetColor();
            Console.Write("    ➜  ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"  {y}  |  REZET  ⚯   All prefix-commands synchronized!\n");
            Console.ResetColor();
        }
    }
}
