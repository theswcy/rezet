using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;


namespace Rezet.Events
{
    public static class EventsHandler
    {
        public static void RegisterEvents(DiscordClient client)
        {
            // PRIMARY:
            client.ComponentInteractionCreated += OnCommandSenderError;
            // GUILD CREATE & DELETE:
            client.GuildCreated += InsertGuildDB.OnGuildCreated;
            client.GuildDeleted += DeleteGuildDB.OnGuildDelete;



            // PARTNERSHIP EVENTS & INTERACTIONS:
            // client.ComponentInteractionCreated += PartnerDashboard.PD_PrimaryButtons;
            // client.ComponentInteractionCreated += PartnerDashboard.PD_SelectMenuEmbeds;
            // client.ModalSubmitted += PartnerDashboard.PD_ModalSubmit;
            // NEW:
            client.ComponentInteractionCreated += PartnershipDashboardPrimaryButtons.PrimaryButtons;
            client.ComponentInteractionCreated += PartnershipEmbedConfigs.DashboardSelectMenu;
            client.ComponentInteractionCreated += PartnershipEmbedConfigs.PartershipEmbedBuilder;
            client.ModalSubmitted += PartnershipEmbedConfigs.PartnershipEmbedBulderModal;



            // MESSAGE EVENTS:
            client.MessageCreated += OnMessageEvents.OnMessageComponents;
            client.GuildMemberRemoved += OnJoinOrQuit.OnQuitPARTNER;


            // AUTOMATIC:
            client.GuildMemberAdded += OnJoinOrQuit.OnJoinAUTOROLE;


            // MODERATOR:
            client.ComponentInteractionCreated += ModeratorDashboard.MD_Primary;



            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"O [  {y}  |  REZET  ] All events synchronized!");
            Console.ResetColor();
        }

        public static async Task OnCommandSenderError(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            try
            {
                if (!e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString()))
                {
                    await e.Interaction.DeferAsync(ephemeral: true);
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .AsEphemeral(true)
                            .WithContent("Ops, você não pode interferir nos comandos dos outros!")
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}