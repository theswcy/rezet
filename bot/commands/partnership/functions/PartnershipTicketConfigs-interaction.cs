using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;
using System.Text.RegularExpressions;



#pragma warning disable CS8604
#pragma warning disable CS8602
public class PartnershipTicketConfigs
{
    // ========== EDIT TICKET CONFIGURATIONS:
    public static async Task EditTicketDashboard(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // ========== EDIT TICKET DASHBORD:
            if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_PTICptio")
            {
                // EDIT TICKET SELECT BUTTONS:
                if (e.Values[0] == "1_edit")
                {
                    await e.Interaction.DeferAsync();
                    var shard = Program._databaseService?.GetShard(e.Guild, 1);
                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");



                    var embedBuilder = new DiscordEmbedBuilder()
                    {
                        Description = "## Ticket Builder!\nConfigurações dos ticket!\n⠀\n> **Dica**: quer **setar** uma ou mais embeds no ticket? Basta colocar o nome da embed salva no modal! Exemplo:\n> ```embed_1 embed_2 embed_3```\n⠀",
                        Color = new DiscordColor("#7e67ff")
                    };
                    var SelectedTickets = "";
                    var SelectedTickets1 = shard[$"{e.Guild.Id}"]["partner"]["ticket"]["embed_1"].AsString;
                    if (SelectedTickets1.Contains('+'))
                    {
                        var t = SelectedTickets1.Split('+');
                        int y = 0;
                        foreach (var entry in t)
                        {
                            SelectedTickets += $"> `{y + 1}.` - **{t[0]}**\n";
                            y++;
                        }
                    }
                    else { SelectedTickets = $"> `1.` - {SelectedTickets1}"; }
                    embedBuilder.AddField(
                        "<:rezet_settings1:1147163366561955932> Selected Embeds:",
                        SelectedTickets
                    );



                    var emoji5 = new DiscordComponentEmoji("🔖");
                    var options1 = new[]
                    {
                        new DiscordSelectComponentOption("Add embed", "5_add", "Add an embed.", emoji: emoji5),
                        new DiscordSelectComponentOption("Edit embed", "5_edit", "Edit an embed.", emoji: emoji5),
                        new DiscordSelectComponentOption("Set embed", "5_set", "Set an embed.", emoji: emoji5),
                        new DiscordSelectComponentOption("Delete embed", "5_del", "Delete an embed.", emoji: emoji5),
                        new DiscordSelectComponentOption("Preview embed", "5_pre", "Preview of the embed.", emoji: emoji5),
                    };
                    var selectMenu1 = new DiscordSelectComponent($"{e.Interaction.User.Id}_PTICmod", "Ticket Embed Options", options1);
                    var options2 = new[]
                    {
                        new DiscordSelectComponentOption("Edit response", "5_edit", "Edit the response embed.", emoji: emoji5),
                        new DiscordSelectComponentOption("Preview response", "5_prev", "Edit the response embed.", emoji: emoji5),
                    };
                    var selectMenu2 = new DiscordSelectComponent($"{e.Interaction.User.Id}_PTICmod2", "Ticket Response Options", options2);



                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! Configurações do ticket!")
                            .AddEmbed(embedBuilder)
                            .AddComponents(selectMenu1)
                            .AddComponents(selectMenu2)
                    );
                }
            }
            // ========== EDIT TICKET BUILDER
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    // ========== EDIT TICKET REAL TIME - BUTTON:
    // ========== EDIT TICKET REAL TIME - MODAL:
    // ========== EDIT TICKET MODAL:
}