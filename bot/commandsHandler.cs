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
            prefix.RegisterCommands<ToModerationBasic_prefix>(); // !! EM TESTES





            var slash = client.UseSlashCommands(null);
            slash.RegisterCommands<RezetGets>(null);

            // COMMUNITY:
            slash.RegisterCommands<CommunityCommands>(null);
            slash.RegisterCommands<CommunityRole>(null);
            slash.RegisterCommands<CommunityChats>(null);

            // PARTNERSHIP:
            slash.RegisterCommands<PartnershipCommands>(null);
            slash.RegisterCommands<CommunityPartnerMore>(null);

            // MODERATION:
            slash.RegisterCommands<ToModerationBasic_slash>(null);




            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"O [  {y}  |  REZET  ] All commands synchronized!");
            Console.ResetColor();
        }
    }
}
