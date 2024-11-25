using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using RezetSharp;
using System.Text.RegularExpressions;



#pragma warning disable CS8604
#pragma warning disable CS8602
public class PartnershipTicketConfigs
{
    // ========== MANAGE TICKET CONFIGURATIONS:
    public static async Task EditTicketDashboard(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // ========== MANAGE TICKET DASHBORD:
            if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_PTICptio")
            {
                // TICKET BUILDER SELECT BUTTONS:
                if (e.Values[0] == "1_edit")
                {
                    await e.Interaction.DeferAsync();
                    var shard = EngineV1.HerrscherRazor.GetHerrscherDocument(e.Guild);
                    var collection = EngineV1.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");



                    var embedBuilder = new DiscordEmbedBuilder()
                    {
                        Description = 
                            "## <:rezet_plant:1308125160577962004> Partnership Ticket __Builder__!",
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
                        "<:rezet_channels:1308125117875752961> Selected Embeds:",
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
                // EDIT TICKET BUTTON:
                else if (e.Values[0] == "1_butt")
                {
                    await e.Interaction.DeferAsync();


                    
                    // BUTTON SHARD:
                    var shard = EngineV1.HerrscherRazor.GetHerrscherDocument(e.Guild);
                    var collection = EngineV1.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var ButtonSettings = shard[$"{e.Guild.Id}"]["partner"]["ticket"]["button"].AsBsonDocument;



                    var BStyle = ButtonStyle.Primary;
                    if (ButtonSettings["color"] == "gray") { BStyle = ButtonStyle.Secondary; }
                    else if (ButtonSettings["color"] == "red") { BStyle = ButtonStyle.Danger; }
                    else if (ButtonSettings["color"] == "green") { BStyle = ButtonStyle.Success; }
                    var Button = new DiscordButtonComponent(BStyle, "example_button", $"{ButtonSettings["text"]}");



                    var emoji1 = new DiscordComponentEmoji("📝");
                    var BlueColor = new DiscordComponentEmoji("🔵");
                    var GrayColor = new DiscordComponentEmoji("⚪");
                    var RedColor = new DiscordComponentEmoji("🔴");
                    var GreenColor = new DiscordComponentEmoji("🟢");
                    var options1 = new[]
                    {
                        new DiscordSelectComponentOption("Edit text", "4_text", "Edit the button text.", emoji: emoji1),
                        new DiscordSelectComponentOption("Color blue", "4_blue", "Set blue color.", emoji: BlueColor),
                        new DiscordSelectComponentOption("Color gray", "4_gray", "Set gray color.", emoji: GrayColor),
                        new DiscordSelectComponentOption("Color red", "4_red", "Set red color.", emoji: RedColor),
                        new DiscordSelectComponentOption("Color green", "4_green", "Set green color.", emoji: GreenColor),
                    };
                    var selectMenu1 = new DiscordSelectComponent($"{e.Interaction.User.Id}_PTBO", "Ticket Button Options", options1);



                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Preview do botão abaixo:")
                            .AddComponents(Button)
                            .AddComponents(selectMenu1)
                    );
                }
            }
            // ========== EDIT TICKET BUILDER
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
    // ========== EDIT TICKET REAL TIME - BUTTON:
    // ========== EDIT TICKET REAL TIME - MODAL:
    // ========== EDIT TICKET MODAL:
}