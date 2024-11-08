using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;




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
                    await e.Interaction.DeferAsync(ephemeral: true);



                    var shard = Program._databaseService?.GetShard(e.Guild, 1);
                    if (!shard[$"{e.Guild.Id}"]["partner"]["leaderboard"]["ranking"].AsBsonDocument.Any())
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Hey! Não há nenhum usuário no ranking!")
                                .AsEphemeral(true)
                        );
                        return;
                    }



                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.leaderboard.ranking", new BsonDocument { });
                    await collection.UpdateOneAsync(shard, update);



                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! O ranking foi limpo!")
                            .AsEphemeral(true)
                    );
                }
                // UNACTIVATE RANKING:
                else if (e.Values[0] == "unactivate")
                {
                    await e.Interaction.DeferAsync();



                    var shard = Program._databaseService?.GetShard(e.Guild, 1);
                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.leaderboard.option", 0);
                    await collection.UpdateOneAsync(shard, update);



                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! O módulo **ranked** da função **partnership** foi desativado!")
                    );
                    await e.Message.DeleteAsync();
                }
                // PATENTS RANKING:
                else if (e.Values[0] == "patents")
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
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}