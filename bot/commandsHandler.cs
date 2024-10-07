using DSharpPlus;
using DSharpPlus.SlashCommands;
using Rezet;

namespace Rezet.Commands
{
    public static class CommandHandler
    {
        #pragma warning disable CS8602
        public static void RegisterCommands(DiscordClient client)
        {
            var prefix = Program.commands;

            // PRIMARY:
            prefix.RegisterCommands<PrefixPrimary>();





            var slash = client.UseSlashCommands();
            slash.RegisterCommands<RezetGets>();

            // COMMUNITY:
            slash.RegisterCommands<CommunityCommands>();
            slash.RegisterCommands<CommunityRole>();
            slash.RegisterCommands<CommunityChats>();

            // PARTNERSHIP:
            slash.RegisterCommands<PartnershipCommands>();
            slash.RegisterCommands<CommunityPartnerMore>();




            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"O [  {y}  |  REZET  ] All commands synchronized!");
            Console.ResetColor();
        }
    }
}
