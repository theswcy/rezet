using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using RezetSharp;
using MongoDB.Bson;
using MongoDB.Driver;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.SlashCommands;



#pragma warning disable CS8602
public class CommunityRankingPrefix : BaseCommandModule
{
    [Command("plr")]
    [Aliases("plrank")]
    public async Task Local(CommandContext ctx)
    {
        try
        {
            await ctx.TriggerTypingAsync();
            var Guild = ctx.Guild;
            var Author = ctx.Member;



            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);
            if (Herrscher[Guild.Id.ToString()]["partner"]["option"].AsInt32 == 0)
            {
                await ctx.RespondAsync("Essa comunidade não possui a função **Partnership ranked** ativada!");
                return;
            }
            if (Herrscher[Guild.Id.ToString()]["partner"]["leaderboard"]["option"].AsInt32 == 0)
            {
                await ctx.RespondAsync("Essa comunidade não possui a função **Partnership ranked** ativada!");
                return;
            }



            var rankingDict = Herrscher[Guild.Id.ToString()]
                        ["partner"]
                        ["leaderboard"]
                        ["ranking"]
                        .AsBsonDocument.ToDictionary(
                            elem => elem.Name,
                            elem => elem.Value.AsInt32
                        );
            if (rankingDict.Count == 0)
            {
                await ctx.RespondAsync("Não há usuários no ranking!");
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
                };

                ranking += $"> {emoji}⠀`{position:D2}. -⠀{score:D4}`⠀<@{userId}>\n";
                position++;
            }
            var embed = new DiscordEmbedBuilder()
            {
                Description = $"**Local leaderboard!**\n`Position⠀⠀⠀Score⠀⠀⠀⠀Name`\n{ranking}",
                Color = new DiscordColor("7e67ff")
            };
            embed.WithAuthor(Guild.Name, iconUrl: Guild.IconUrl);
            var button = new DiscordButtonComponent(ButtonStyle.Primary, $"{ctx.User.Id}_PAexit", "Close", emoji: new DiscordComponentEmoji(id: 1308125206883078225));



            await ctx.RespondAsync(
                builder: new DiscordMessageBuilder()
                    .WithContent("Bip bup bip!")
                    .AddEmbed(embed)
                    .AddComponents(button)
            );
        }
        catch (Exception ex)
        {
            await ctx.RespondAsync($"Falha ao executar o comando, verifique minhas permissões!");
            Console.WriteLine($"    ➜  Prefix Command: plr\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }



    [Command("pgr")]
    [Aliases("pgrank")]
    public async Task Global(CommandContext ctx)
    {
        try
        {
            await ctx.TriggerTypingAsync();
            var Guild = ctx.Guild;
            var Author = ctx.Member;



            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);



            var guildRanking = new List<(ulong GuildId, string Name, dynamic Invite, int Score, string rxp)>();
            var f = ctx.User.AvatarUrl;
            foreach (var element in Herrscher.Elements)
            {
                if (element.Name == "_id") continue;
                var guildData = element.Value.AsBsonDocument;
                if (guildData["partner"]["option"] != 0)
                {
                    var g = await EngineV8X.RezetRazor.GetGuildAsync(ulong.Parse(element.Name));



                    ulong guildId = ulong.Parse(element.Name);
                    var GuildInvite = guildData["partner"]["leaderboard"]["invite"];
                    if (GuildInvite == BsonNull.Value) { GuildInvite = $"{g.Name}"; } else { GuildInvite = $"[{g.Name}](https://discord.gg/{GuildInvite})"; }
                    int GuildScore = guildData["partner"]["ps"].AsInt32;

                    if (GuildScore > 0)
                    {
                        string rbxp = "<:rezet_c_elite:1290676392870023190>";
                        if (guildData["partner"]["xp"] > 10000) { rbxp = "<:rezet_sss_elite:1290676886556377112>"; }
                        else if (guildData["partner"]["xp"] > 5000) { rbxp = "<:rezet_ss_elite:1290676831544017046>"; }
                        else if (guildData["partner"]["xp"] > 2500) { rbxp = "<:rezet_s_elite:1290676775826886696>"; }
                        else if (guildData["partner"]["xp"] > 1000) { rbxp = "<:rezet_a_elite:1290676720537567373>"; }
                        else if (guildData["partner"]["xp"] >= 500) { rbxp = "<:rezet_b_elite:1290676630527934567>"; }


                        guildRanking.Add((guildId, g.Name, GuildInvite, GuildScore, rbxp));
                    }
                }
            }
            var rankedGuild = guildRanking.OrderByDescending(g => g.Score).Take(20).ToList();



            var firtsGuild = await ctx.Client.GetGuildAsync(rankedGuild[0].GuildId);
            if (firtsGuild.IconUrl != null) { f = firtsGuild.IconUrl; }



            var rankingMessage = "";
            for (int i = 0; i < rankedGuild.Count; i++)
            {
                rankingMessage += $"> {rankedGuild[i].rxp}⠀`{i + 1:D2}.⠀-⠀{rankedGuild[i].Score}` ⠀**{rankedGuild[i].Invite}**\n";
            }
            var embed = new DiscordEmbedBuilder()
            {
                Description = $"**Global Partnership ranked!**\nRanking **global** das comunidades com mais **parcerias**!\n\n`Position⠀⠀⠀Score⠀⠀⠀⠀Name`\n{rankingMessage}⠀",
                Color = new DiscordColor("7e67ff")
            };
            embed.WithFooter(
                "Powered by Rezet Sharp!",
                EngineV8X.RezetRazor.CurrentUser.AvatarUrl
            );
            embed.WithAuthor(
                $"Top 1: {rankedGuild[0].Name}",
                iconUrl: f
            );
            var button = new DiscordButtonComponent(ButtonStyle.Primary, $"{ctx.User.Id}_PAexit", "Close", emoji: new DiscordComponentEmoji(id: 1308125206883078225));



            await ctx.RespondAsync(
                builder: new DiscordMessageBuilder()
                    .WithContent("Bip bup bip!")
                    .AddEmbed(embed)
                    .AddComponents(button)
            );
        }
        catch (Exception ex)
        {
            await ctx.RespondAsync($"Falha ao executar o comando, verifique minhas permissões!");
            Console.WriteLine($"    ➜  Prefix Command: pgr\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }
}