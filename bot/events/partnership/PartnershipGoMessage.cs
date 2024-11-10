using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Bson;
using Rezet;
using MongoDB.Driver;
using System.Text.RegularExpressions;



#pragma warning disable CS8602
public static class PartnershipGoMessage
{
    // ========== START:
    public static async Task StartThePartnership(MessageCreateEventArgs e, BsonDocument shard)
    {
        try
        {
            if (e.Message.Content.Contains("@everyone") || e.Message.Content.Contains("@here"))
            {
                if (shard[$"{e.Guild.Id}"]["partner"]["anti-eh"] == 1)
                {
                    await IfMentions(e, shard, 1);
                    return;
                }
                else if (shard[$"{e.Guild.Id}"]["partner"]["anti-eh"] == 2)
                {
                    await IfMentions(e, shard, 2);
                    return;
                }
                else if (shard[$"{e.Guild.Id}"]["partner"]["anti-eh"] == 3)
                {
                    await IfMentions(e, shard, 3);
                    return;
                }
            }
            else
            {
                await Go(e, shard);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }





    // ========== IF MENTIONS @EVERYONE - @HERE:
    public static async Task IfMentions(MessageCreateEventArgs e, BsonDocument shard, int OPT)
    {
        try
        {
            if (OPT == 1)
            {
                await e.Message.RespondAsync(
                    "Oops! Os convites de parceria não podem mencionar **everyone** e/ou **here**!"
                );



                if (shard[$"{e.Guild.Id}"]["partner"]["log"] != BsonNull.Value)
                {
                    var ch = e.Guild.GetChannel((ulong)shard[$"{e.Guild.Id}"]["partner"]["log"].AsInt64);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Description = $"O usuário {e.Author.Mention} [ `{e.Author.Id}` ] enviou um convite de parcerias que continha menções **everyone** e/ou **here**! O convite foi detelado.",
                        Color = new DiscordColor("7e67ff")
                    };
                    embed.WithTimestamp(e.Message.Timestamp);
                    var embed2 = new DiscordEmbedBuilder()
                    {
                        Description = $"Message:\n```{e.Message.Content}```",
                        Color = new DiscordColor("7e67ff")
                    };
                    await ch.SendMessageAsync(
                        new DiscordMessageBuilder()
                            .WithContent("Oh...")
                            .AddEmbed(embed)
                            .AddEmbed(embed2)
                    );
                }
                await e.Message.DeleteAsync();
                return;
            }
            else if (OPT == 2)
            {
                var rep = e.MentionedUsers;
                foreach (var i in rep)
                {
                    var p = await e.Guild.GetMemberAsync(i.Id);
                    await p.RemoveAsync("Móduto de parcerias anti menção everyone/here ativado: Mode 2.");
                }
                await e.Message.RespondAsync(
                    new DiscordMessageBuilder()
                        .WithContent("Oops! Os Convites de parceria não podem mencionar **everyone** e **here**!")
                );
                if (shard[$"{e.Guild.Id}"]["partner"]["log"] != BsonNull.Value)
                {
                    var ch = e.Guild.GetChannel((ulong)shard[$"{e.Guild.Id}"]["partner"]["log"].AsInt64);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Description = $"O usuário {e.Author.Mention} [ `{e.Author.Id}` ] enviou um convite de parcerias que continha menções **everyone** e/ou **here**!",
                        Color = new DiscordColor("7e67ff")
                    };
                    embed.WithTimestamp(e.Message.Timestamp);
                    var embed2 = new DiscordEmbedBuilder()
                    {
                        Description = $"Message:\n```{e.Message.Content}```",
                        Color = new DiscordColor("7e67ff")
                    };
                    await ch.SendMessageAsync(
                        new DiscordMessageBuilder()
                            .WithContent("Oh...")
                            .AddEmbed(embed)
                            .AddEmbed(embed2)
                    );
                }
                await e.Message.DeleteAsync();
                return;
            }
            else if (OPT == 3)
            {
                var rep = e.MentionedUsers;
                foreach (var i in rep)
                {
                    var p = await e.Guild.GetMemberAsync(i.Id);
                    await p.BanAsync(reason: "Móduto de parcerias anti menção everyone/here ativado: Mode 3.");
                }
                await e.Message.RespondAsync(
                    new DiscordMessageBuilder()
                        .WithContent("Oops! Os Convites de parceria não podem mencionar **everyone** e **here**!")
                );
                if (shard[$"{e.Guild.Id}"]["partner"]["log"] != BsonNull.Value)
                {
                    var ch = e.Guild.GetChannel((ulong)shard[$"{e.Guild.Id}"]["partner"]["log"].AsInt64);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Description = $"O usuário {e.Author.Mention} [ `{e.Author.Id}` ] enviou um convite de parcerias que continha menções **everyone** e/ou **here**!",
                        Color = new DiscordColor("7e67ff")
                    };
                    embed.WithTimestamp(e.Message.Timestamp);
                    var embed2 = new DiscordEmbedBuilder()
                    {
                        Description = $"Message:\n```{e.Message.Content}```",
                        Color = new DiscordColor("7e67ff")
                    };
                    await ch.SendMessageAsync(
                        new DiscordMessageBuilder()
                            .WithContent("Oh...")
                            .AddEmbed(embed)
                            .AddEmbed(embed2)
                    );
                }
                await e.Message.DeleteAsync();
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    // ========== GO MESSAGE:
    public static async Task Go(MessageCreateEventArgs e, BsonDocument shard)
    {
        try
        {
            var ConfigsOptions = shard[$"{e.Guild.Id}"]["partner"]["configs"]["options"];
            var Role = e.Guild.GetRole((ulong)ConfigsOptions["role"].ToInt64());
            var Ping = e.Guild.GetRole((ulong)ConfigsOptions["ping"].ToInt64());
            var SelectedEmbedName = shard[$"{e.Guild.Id}"]["partner"]["selected"];
            var embedConfigs = shard[$"{e.Guild.Id}"]["partner"]["configs"]["embeds"][SelectedEmbedName.ToString()];
            var points = 0;
            var leaderboard = shard[$"{e.Guild.Id}"]["partner"]["leaderboard"];




            // ========== RANKING UPDATE:
            Random rnd = new();
            var ranking = 0;
            if (leaderboard["option"] != 0)
            {
                if (leaderboard["ranking"] != BsonNull.Value)
                {
                    if (leaderboard["ranking"].AsBsonDocument.Contains($"{e.Author.Id}"))
                    {
                        int pta = (int)shard[$"{e.Guild.Id}"]["partner"]["leaderboard"]["ranking"][$"{e.Author.Id}"];
                        var k = shard[$"{e.Guild.Id}"]["partner"]["leaderboard"]["ranking"].AsBsonDocument.ToDictionary(elem => elem.Name, elem => elem.Value);
                        var kt = k.OrderByDescending(x => x.Value);
                        foreach (var entry in kt)
                        {
                            ranking++;
                            if (ulong.Parse(entry.Key) == e.Author.Id) { break; }
                        }
                        var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update
                            .Inc($"{e.Guild.Id}.partner.leaderboard.ranking.{e.Author.Id}", 1)
                            .Inc($"{e.Guild.Id}.partner.ps", 1)
                            .Inc($"{e.Guild.Id}.partner.xp", rnd.Next(2, 5));
                        await collection.UpdateOneAsync(shard, update);
                        points = pta++;
                    }
                    else
                    {
                        var k = shard[$"{e.Guild.Id}"]["partner"]["leaderboard"]["ranking"].AsBsonDocument.ToDictionary(elem => elem.Name, elem => elem.Value);
                        var kt = k.OrderByDescending(x => x.Value);
                        foreach (var entry in kt)
                        {
                            ranking++;
                            if (ulong.Parse(entry.Key) == e.Author.Id) { break; }
                        }
                        var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update
                            .Set($"{e.Guild.Id}.partner.leaderboard.ranking.{e.Author.Id}", 1)
                            .Inc($"{e.Guild.Id}.partner.ps", 1)
                            .Inc($"{e.Guild.Id}.partner.xp", rnd.Next(2, 5));
                        await collection.UpdateOneAsync(shard, update);
                        points = 1;
                    }
                }
                else
                {
                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update
                        .Set($"{e.Guild.Id}.partner.leaderboard.ranking.{e.Author.Id}", 1)
                        .Inc($"{e.Guild.Id}.partner.ps", 1)
                        .Inc($"{e.Guild.Id}.partner.xp", rnd.Next(2, 5));
                    await collection.UpdateOneAsync(shard, update);
                    points = 1;
                }
            }




            // ========== EMBED BUILDER:
            var embed = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(embedConfigs["color"].ToString())
            };
            var BuilderMessage = new DiscordMessageBuilder();
            if (e.Message.MentionedUsers.Any())
            {
                if (e.Message.Content.Contains("discord.gg/") || e.Message.Content.Contains("discord.com/invite/"))
                {
                    var inviteRegex = new Regex(@"discord(?:app\.com\/invite|\.gg)\/([\w-]+)", RegexOptions.IgnoreCase);
                    var match = inviteRegex.Match(e.Message.Content);
                    var g = await Program.Rezet.GetInviteByCodeAsync(match.Groups[1].Value);
                    if (!g.IsRevoked)
                    {
                        embed.WithTitle(
                            SwitchOrResponse.SwitchVariables2(
                                $"{embedConfigs["title"]}", e, e.Message.MentionedUsers[0], points, ranking, g.Guild.Name
                            )
                        );
                        embed.WithDescription(
                            SwitchOrResponse.SwitchVariables2(
                                $"{embedConfigs["description"]}", e, e.Message.MentionedUsers[0], points, ranking, g.Guild.Name
                            )
                        );
                        if (!embedConfigs["footer"].IsInt32)
                        {
                            embed.WithFooter(
                                text: SwitchOrResponse.SwitchVariables2(
                                    $"{embedConfigs["footer"]}", e, e.Message.MentionedUsers[0], points, ranking, g.Guild.Name
                                )
                            );
                        }
                        var buttonLink = new DiscordLinkButtonComponent(
                            $"https://discord.gg/{match.Groups[1].Value}", "Entrar no servidor", false
                        );
                        BuilderMessage.AddComponents(buttonLink);
                    }
                    else
                    {
                        var u = await e.Message.RespondAsync("Hey! O convite fornecido não é válido!");
                        await Task.Delay(2000); await u.DeleteAsync();
                        return;
                    }
                }
                else
                {
                    var u = await e.Message.RespondAsync("Hey! O convite de parcerias deve estar no formato `discord.gg/` ou `discord.com/invite/`!");
                    await Task.Delay(3000); await u.DeleteAsync();
                    return;
                }
                var rep = e.Message.MentionedUsers;
                foreach (var i in rep)
                {
                    var p = await e.Guild.GetMemberAsync(i.Id);
                    await p.GrantRoleAsync(Role);
                }
            }
            else
            {
                if (e.Message.Content.Contains("discord.gg/") || e.Message.Content.Contains("discord.com/invite/"))
                {
                    var inviteRegex = new Regex(@"discord(?:app\.com\/invite|\.gg)\/([\w-]+)", RegexOptions.IgnoreCase);
                    var match = inviteRegex.Match(e.Message.Content);
                    var g = await Program.Rezet.GetInviteByCodeAsync(match.Groups[1].Value);
                    if (!g.IsRevoked)
                    {
                        embed.WithTitle(
                            SwitchOrResponse.SwitchVariables2(
                                $"{embedConfigs["title"]}", e, e.Author, points, ranking, g.Guild.Name
                            )
                        );
                        embed.WithDescription(
                            SwitchOrResponse.SwitchVariables2(
                                $"{embedConfigs["description"]}", e, e.Author, points, ranking, g.Guild.Name
                            )
                        );
                        if (!embedConfigs["footer"].IsInt32)
                        {
                            embed.WithFooter(
                                text: SwitchOrResponse.SwitchVariables2(
                                    $"{embedConfigs["footer"]}", e, e.Author, points, ranking, g.Guild.Name
                                )
                            );
                        }
                        var buttonLink = new DiscordLinkButtonComponent(
                            $"https://discord.gg/{match.Groups[1].Value}", "Entrar no servidor", false
                        );
                        BuilderMessage.AddComponents(buttonLink);
                    }
                    else
                    {
                        var u = await e.Message.RespondAsync("Hey! O convite fornecido não é válido!");
                        await Task.Delay(2000); await u.DeleteAsync();
                        return;
                    }
                }
                else
                {
                    var u = await e.Message.RespondAsync("Hey! O convite de parcerias deve estar no formato `discord.gg/` ou `discord.com/invite/`!");
                    await Task.Delay(3000); await u.DeleteAsync();
                    return;
                }
            }




            // ========== VERIFY IMAGE:
            if (!embedConfigs["image"].IsInt32)
            {
                embed.WithImageUrl(embedConfigs["image"].ToString());
            }



            // ========== VERIFY THUMBNAIL:
            if (embedConfigs["thumb"].IsInt32)
            {
                if (embedConfigs["thumb"] == 1 && e.Guild.IconUrl != null) { embed.WithThumbnail(e.Guild.IconUrl); }
            }
            else if (embedConfigs["thumb"].IsString)
            {
                embed.WithThumbnail(embedConfigs["thumb"].AsString);
            }



            // ========== VERIFY AUTHOR:
            if (embedConfigs["author"] != 0)
            {
                if (e.Guild.IconUrl != null) { embed.WithAuthor(name: e.Guild.Name, iconUrl: e.Guild.IconUrl); }
            }



            await e.Message.RespondAsync(
                builder: BuilderMessage.AddEmbed(embed).WithContent($"✨ {Role.Mention}")
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}