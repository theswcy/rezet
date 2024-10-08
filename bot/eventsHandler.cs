using DSharpPlus;


namespace Rezet.Events
{
    public static class EventsHandler
    {
        public static void RegisterEvents(DiscordClient client)
        {
            // GUILD CREATE & DELETE:
            client.GuildCreated += InsertGuildDB.OnGuildCreated;
            client.GuildDeleted += DeleteGuildDB.OnGuildDelete;



            // PARTNERSHIP EVENTS & INTERACTIONS:
            client.ComponentInteractionCreated += PartnerDashboard.PD_PrimaryButtons;
            client.ComponentInteractionCreated += PartnerDashboard.PD_SelectMenuEmbeds;
            client.ModalSubmitted += PartnerDashboard.PD_ModalSubmit;



            // MESSAGE EVENTS:
            client.MessageCreated += OnMessageEvents.OnMessageComponents;
            client.GuildMemberRemoved += OnJoinOrQuit.OnQuit;



            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"O [  {y}  |  REZET  ] All events synchronized!");
            Console.ResetColor();
        }
    }
}