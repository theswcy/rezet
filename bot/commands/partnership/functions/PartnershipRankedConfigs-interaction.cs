using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using RezetSharp;
using System.Text.RegularExpressions;




#pragma warning disable CS8602
public class PartnershipRanked
{
    public static async Task PartnerRankingSelectMenu(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // PARTNERSHIP RANKING OPTIONS:
            if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_PERanking")
            {
                // RESET RANKING:
                if (e.Values[0] == "reset")
                {
                    try
                    {
                        await e.Interaction.DeferAsync(ephemeral: true);



                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                        if (!Herrscher[$"{e.Guild.Id}"]["partner"]["leaderboard"]["ranking"].AsBsonDocument.Any())
                        {
                            await e.Interaction.CreateFollowupMessageAsync(
                                new DiscordFollowupMessageBuilder()
                                    .WithContent("Hey! Não há nenhum usuário no ranking!")
                                    .AsEphemeral(true)
                            );
                            return;
                        }



                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.leaderboard.ranking", new BsonDocument { });
                        await collection.UpdateOneAsync(Herrscher, update);



                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Bip bup bip! O ranking foi limpo!")
                                .AsEphemeral(true)
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Ranking Reset\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // UNACTIVATE RANKING:
                else if (e.Values[0] == "unactivate")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();



                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.leaderboard.option", 0);
                        await collection.UpdateOneAsync(Herrscher, update);



                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Bip bup bip! O módulo **ranked** da função **partnership** foi desativado!")
                        );
                        await e.Message.DeleteAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Ranking Unactivate\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // PATENTS RANKING:
                else if (e.Values[0] == "patents")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();


                        var embed = new DiscordEmbedBuilder()
                        {
                            Color = new DiscordColor("7e67ff")
                        };
                        embed.AddField(
                            "<:rezet_shine:1147373071737573446> Patents:",
                            "> <:rezet_sss_elite:1290676886556377112> `SSS-Elite⠀-⠀  10000`" +
                            "\n> <:rezet_ss_elite:1290676831544017046> `SS-Elite⠀ -⠀  5000`" +
                            "\n> <:rezet_s_elite:1290676775826886696> `S-Elite⠀  -⠀  2500`" +
                            "\n> <:rezet_a_elite:1290676720537567373> `A-Elite⠀  -⠀  1000`" +
                            "\n> <:rezet_b_elite:1290676630527934567> `B-Elite⠀  -⠀  500`" +
                            "\n> <:rezet_c_elite:1290676392870023190> `C-Elite⠀  -⠀  0`"
                        );
                        var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");



                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Bip bup bip!")
                                .AddEmbed(embed)
                                .AddComponents(button)
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Ranking Patents\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // INVITE LINK
                else if (e.Values[0] == "invite")
                {
                    try
                    {
                        var modal = new DiscordInteractionResponseBuilder()
                        .WithTitle("Guild Link")
                        .WithCustomId($"{e.Interaction.User.Id}_RCIL")
                        .AddComponents(
                            new TextInputComponent(
                                "Link:", "invite_input", "Only HTTPS!", style: TextInputStyle.Paragraph
                            )
                        );
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Ranking Change Invite\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Ranking Select Menu\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
    public static async Task PartnerrankingModifyInvite(DiscordClient sender, ModalSubmitEventArgs e)
    {
        try
        {
            if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}_RCIL")
            {
                await e.Interaction.DeferAsync(true);
                var content = e.Values["invite_input"];
                var inviteRegex = new Regex(@"discord(?:app\.com\/invite|\.gg)\/([\w-]+)", RegexOptions.IgnoreCase);
                var match = inviteRegex.Match(content);
                if (!match.Success)
                {
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Ops! o Convite fornecido é inválido!")
                    );
                    return;
                }
                var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.leaderboard.invite", match.Groups[1].Value);
                await collection.UpdateOneAsync(Herrscher, update);



                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Nice! O link de convite foi atualizado com sucesso!")
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Ranking Change Invite\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}