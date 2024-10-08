using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using Rezet;
using MongoDB.Bson;
using MongoDB.Driver;



[SlashCommandGroup("partner", "Community Partner.")]
public class CommunityPartnerMore : ApplicationCommandModule
{
    [SlashCommandGroup("ranking", "ranking configs.")]
    public class RankingConigs : ApplicationCommandModule
    {
        [SlashCommand("local", "🎋 | The local user ranking's leaderboard!")]
        public static async Task Local(InteractionContext ctx)
        {
            try
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                var Guild = ctx.Guild;
                var Author = ctx.Member;



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }




                if (shard[Guild.Id.ToString()]["partner"]["option"].AsInt32 == 0)
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .WithContent("<:rezet_dred:1147164215837208686> Essa comunidade não possui a função **Partnership** ativada!")
                    );
                    return;
                }
                if (shard[Guild.Id.ToString()]["partner"]["leaderboard"]["option"].AsInt32 == 0)
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .WithContent("<:rezet_dred:1147164215837208686> Essa comunidade não possui a função **Partnership local ranked** ativada!")
                    );
                    return;
                }



                var rankingDict = shard[Guild.Id.ToString()]
                        ["partner"]
                        ["leaderboard"]
                        ["ranking"]
                        .AsBsonDocument.ToDictionary(
                            elem => elem.Name,
                            elem => elem.Value.AsInt32
                        );
                if (rankingDict.Count == 0)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("<:rezet_dred:1147164215837208686> Não há usuários no ranking!")
                    );
                    return;
                }
                var orderedRanking = rankingDict.OrderByDescending(x => x.Value);



                string ranking = "";
                int position = 1;
                foreach (var entry in orderedRanking)
                {
                    string userId = entry.Key;
                    int score = entry.Value;
                    var emoji = "🐬";
                    if (position == 1)
                    {
                        emoji = "🦈";
#pragma warning disable CS8602
                    };

                    ranking += $"> {emoji}⠀`{position:D2}. -⠀{score:D4}`⠀<@{userId}>\n";
                    position++;
                }
                var embed = new DiscordEmbedBuilder()
                {
                    Description = $"**Local leaderboard!**\n`⠀⠀##⠀⠀⠀⠀⠀Score⠀⠀⠀⠀⠀Name`\n{ranking}",
                    Color = new DiscordColor("7e67ff")
                };
                embed.WithAuthor(Guild.Name, iconUrl: Guild.IconUrl);
                var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{ctx.User.Id}_PAexit", "Exit");



                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Bip bup bip!")
                        .AddEmbed(embed)
                        .AddComponents(button)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }





        [SlashCommand("global", "🎋 | The global community ranking's leaderboard")]
        public static async Task Global(InteractionContext ctx)
        {
            try
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                var Guild = ctx.Guild;
                var Author = ctx.Member;



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }



#pragma warning disable CS8600 
                var guildRanking = new List<(ulong GuildId, string Name, int Score, string rxp)>();
                var f = ctx.User.AvatarUrl;
                foreach (var element in shard.Elements)
                {
                    if (element.Name == "_id") continue;

                    var guildData = element.Value.AsBsonDocument;
                    ulong guildId = ulong.Parse(element.Name);
                    string GuildName = guildData["guild_name"].AsString;
                    int GuildScore = guildData["partner"]["ps"].AsInt32;


                    string rbxp = "<:rezet_c_elite:1290676392870023190>";
                    if (guildData["partner"]["xp"] > 10000) { rbxp = "<:rezet_sss_elite:1290676886556377112>"; }
                    else if (guildData["partner"]["xp"] > 5000) { rbxp = "<:rezet_ss_elite:1290676831544017046>"; }
                    else if (guildData["partner"]["xp"] > 2500) { rbxp = "<:rezet_s_elite:1290676775826886696>"; }
                    else if (guildData["partner"]["xp"] > 1000) { rbxp = "<:rezet_a_elite:1290676720537567373>"; }
                    else if (guildData["partner"]["xp"] >= 500) { rbxp = "<:rezet_b_elite:1290676630527934567>"; }


                    guildRanking.Add((guildId, GuildName, GuildScore, rbxp));
                }
                var rankedGuild = guildRanking.OrderByDescending(g => g.Score).Take(20).ToList();



                var firtsGuild = await ctx.Client.GetGuildAsync(rankedGuild[0].GuildId);
                if (firtsGuild.IconUrl != null) { f = firtsGuild.IconUrl; }



                var rankingMessage = "";
                for (int i = 0; i < rankedGuild.Count; i++)
                {
                    rankingMessage += $"> {rankedGuild[i].rxp}⠀`{i + 1:D2}.⠀-⠀{rankedGuild[i].Score}` ⠀**{rankedGuild[i].Name}**\n";
                }
                var embed = new DiscordEmbedBuilder()
                {
                    Description = $"**Global Partnership ranked!**\nRanking **global** das comunidades com mais **parcerias**!\n\n{rankingMessage}⠀",
                    Color = new DiscordColor("7e67ff")
                };
                embed.WithFooter(
                    "Powered by Rezet Sharp!",
                    Program.Rezet.CurrentUser.AvatarUrl
                );
                embed.WithAuthor(
                    $"Top 1: {rankedGuild[0].Name}",
                    iconUrl: f
                );
                var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{ctx.User.Id}_PAexit", "Exit");



                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Bip bup bip!")
                        .AddEmbed(embed)
                        .AddComponents(button)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }






    [SlashCommandGroup("points", "Cotrol points")]
    public class Points : ApplicationCommandModule
    {
        [SlashCommand("add", "🎋 | Add partnership points to a member!")]
        public static async Task AddPoints(InteractionContext ctx,
            [Option("user", "The user.")] DiscordUser user,
            [Option("amount", "Points to add.")] long amount
        )
        {
            try
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                var Guild = ctx.Guild;
                var Author = ctx.Member;



                // MANAGE CHANNELS.
                await CheckPermi.CheckMemberPermissions(ctx, 3);
                await CheckPermi.CheckBotPermissions(ctx, 3);


                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard[$"{Guild.Id}"]["partner"]["log"] != BsonNull.Value)
                {
                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Inc($"{Guild.Id}.partner.leaderboard.ranking.{user.Id}", (uint)amount);
                    await collection.UpdateOneAsync(shard, update);

                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"O usuário {user.Mention} recebeu **{amount} pontos**!")
                    );
                    return;
                }
                else
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("<:rezet_dred:1147164215837208686> O canal de registro de parcerias precisa está ativado")
                    );
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }





        [SlashCommand("remove", "🎋 | Remove partnership points from a member.")]
        public static async Task RemovePoints(InteractionContext ctx,
            [Option("user", "The user.")] DiscordUser user,
            [Option("amount", "Points to remove.")] long amount
        )
        {
            try
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                var Guild = ctx.Guild;
                var Author = ctx.Member;



                // MANAGE CHANNELS.
                await CheckPermi.CheckMemberPermissions(ctx, 3);
                await CheckPermi.CheckBotPermissions(ctx, 3);



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (!shard[$"{Guild.Id}"]["partner"]["leaderboard"]["ranking"].AsBsonDocument.Contains(user.Id.ToString()))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"<:rezet_dred:1147164215837208686> O usuário {user.Mention} não está no ranking!")
                    );
                    return;
                }
                long p = shard[$"{Guild.Id}"]["partner"]["leaderboard"]["ranking"][$"{user.Id}"].AsInt32;
                if (amount >= p) { p = 0; } else { p -= amount; if (p <= 0 ) { p = 0; } }
                if (shard[$"{Guild.Id}"]["partner"]["log"] != BsonNull.Value)
                {
                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.leaderboard.ranking.{user.Id}", (uint)p);
                    await collection.UpdateOneAsync(shard, update);

                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"O usuário {user.Mention} perdeu **{amount} pontos**!")
                    );
                    return;
                }
                else
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("<:rezet_dred:1147164215837208686> O canal de registro de parcerias precisa está ativado")
                    );
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
