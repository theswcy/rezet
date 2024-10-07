using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;




public static class OnMessageEvents
{
    // GENERAL:
    public static async Task OnMessageComponents(DiscordClient sender, MessageCreateEventArgs e)
    {
        try
        {
            var Guild = e.Guild;
            var shard = Program._databaseService?.GetShard(Guild, 1);
            if (shard == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                Console.ResetColor();
                return;
            }


            
            // ON EVERYONE:
            if (shard[Guild.Id.ToString()]["partner"]["option"] != 0)
            {
                if (e.Author.IsBot) { return; }
                if ((ulong)shard[Guild.Id.ToString()]["partner"]["configs"]["options"]["channel"].ToInt64() != e.Channel.Id) { return; }
                if (e.Message.Content.Contains("@everyone") || e.Message.Content.Contains("@here"))
                {
                    if (shard[Guild.Id.ToString()]["partner"]["anti-eh"] == 1)
                    {
                        await e.Message.RespondAsync(
                            "Oops! Os convites de parceria não podem mencionar **everyone** e/ou **here**!"
                        );
                        if (shard[Guild.Id.ToString()]["partner"]["log"] != BsonNull.Value)
                        {
                            var ch = Guild.GetChannel((ulong)shard[Guild.Id.ToString()]["partner"]["log"].AsInt64);
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
                    else if (shard[Guild.Id.ToString()]["partner"]["anti-eh"] == 2)
                    {
                        var rep = e.MentionedUsers;
                        foreach (var i in rep)
                        {
                            var p = await Guild.GetMemberAsync(i.Id);
                            await p.RemoveAsync();
                        }
                        await e.Message.RespondAsync(
                            new DiscordMessageBuilder()
                                .WithContent("Oops! Os Convites de parceria não podem mencionar **everyone** e **here**!")
                        );
                        if (shard[Guild.Id.ToString()]["partner"]["log"] != BsonNull.Value)
                        {
                            var ch = Guild.GetChannel((ulong)shard[Guild.Id.ToString()]["partner"]["log"].AsInt64);
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
                    else if (shard[Guild.Id.ToString()]["partner"]["anti-eh"] == 3)
                    {
                        var rep = e.MentionedUsers;
                        foreach (var i in rep)
                        {
                            var p = await Guild.GetMemberAsync(i.Id);
                            await p.BanAsync();
                        }
                        await e.Message.RespondAsync(
                            new DiscordMessageBuilder()
                                .WithContent("Oops! Os Convites de parceria não podem mencionar **everyone** e **here**!")
                        );
                        if (shard[Guild.Id.ToString()]["partner"]["log"] != BsonNull.Value)
                        {
                            var ch = Guild.GetChannel((ulong)shard[Guild.Id.ToString()]["partner"]["log"].AsInt64);
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
                await ForPartnership(sender, e, shard);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }




    // PARTNERSHIP:
    public static async Task ForPartnership(DiscordClient sender, MessageCreateEventArgs e, BsonDocument shard)
    {
        try
        {
            var Guild = e.Guild;
            var ConfigsOptions = shard[Guild.Id.ToString()]["partner"]["configs"]["options"];

#pragma warning disable CS8602
            var Role = e.Guild.GetRole((ulong)ConfigsOptions["role"].ToInt64());
            var Ping = e.Guild.GetRole((ulong)ConfigsOptions["ping"].ToInt64());



            var SelectedEmbedName = shard[Guild.Id.ToString()]["partner"]["selected"];
            var embedConfigs = shard[Guild.Id.ToString()]["partner"]["configs"]["embeds"][SelectedEmbedName.ToString()];
            var points = 0;
            var leaderboard = shard[Guild.Id.ToString()]["partner"]["leaderboard"];
            Random rnd = new();
            if (leaderboard["option"] != 0)
            {
                if (leaderboard["ranking"] != BsonNull.Value)
                {
                    if (leaderboard["ranking"].AsBsonDocument.Contains(e.Author.Id.ToString()))
                    {
                        int pta = (int)shard[$"{Guild.Id}"]["partner"]["leaderboard"]["ranking"][e.Author.Id.ToString()];
                        var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update
                            .Inc($"{Guild.Id}.partner.leaderboard.ranking.{e.Author.Id}", 1)
                            .Inc($"{Guild.Id}.partner.ps", 1)
                            .Inc($"{Guild.Id}.partner.xp", rnd.Next(2, 5));
                        await collection.UpdateOneAsync(shard, update);
                        points = pta++;
                    }
                    else
                    {
                        var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update
                            .Set($"{Guild.Id}.partner.leaderboard.ranking.{e.Author.Id}", 1)
                            .Inc($"{Guild.Id}.partner.ps", 1)
                            .Inc($"{Guild.Id}.partner.xp", rnd.Next(2, 5));
                        await collection.UpdateOneAsync(shard, update);
                        points = 1;
                    }
                }
                else
                {
                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update
                        .Set($"{Guild.Id}.partner.leaderboard.ranking.{e.Author.Id}", 1)
                        .Inc($"{Guild.Id}.partner.ps", 1)
                        .Inc($"{Guild.Id}.partner.xp", rnd.Next(2, 5));
                    await collection.UpdateOneAsync(shard, update);
                    points = 1;
                }
            }




#pragma warning disable CS8604
            var embed = new DiscordEmbedBuilder()
            {
                Title = SwitchOrResponse.SwitchVariables2(
                    embedConfigs["title"].ToString(), e, points
                ),
                Description = SwitchOrResponse.SwitchVariables2(
                    embedConfigs["description"].ToString(), e, points
                ),
                Color = new DiscordColor(embedConfigs["color"].ToString())
            };
            embed.WithFooter(
                SwitchOrResponse.SwitchVariables2(
                    embedConfigs["footer"].ToString(), e, points
                )
            );
            if (!string.IsNullOrEmpty(embedConfigs["image"].ToString()))
            {
                embed.WithImageUrl(embedConfigs["image"].ToString());
            }
            if (embedConfigs.AsBsonDocument["thumb"] != 0)
            {
                embed.WithThumbnail(Guild.IconUrl);
            }
            if (embedConfigs.AsBsonDocument["author"] != 0)
            {
                embed.WithAuthor(
                    Guild.Name, iconUrl: Guild.IconUrl
                );
            }



            var rep = e.Message.MentionedUsers;
            foreach (var i in rep)
            {
                var p = await Guild.GetMemberAsync(i.Id);
                await p.GrantRoleAsync(Role);
            }
            await e.Message.RespondAsync(
                $"<:rezet_star:1173977797971165256> Bip bup bip! {Ping.Mention}",
                embed
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}