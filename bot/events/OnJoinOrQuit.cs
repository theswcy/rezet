using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Bson;
using Rezet;




public static class OnJoinOrQuit
{
    // AUTOROLE:
    public static async Task OnJoinAUTOROLE(DiscordClient sender, GuildMemberAddEventArgs e)
    {
        try
        {
            var Guild = e.Guild;
            var shard = Program._databaseService?.GetShard(Guild, 1);
            var Member = e.Member;



#pragma warning disable CS8602
            if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"] != BsonNull.Value)
            {
                var RolesDict = shard
                                [$"{Guild.Id}"]
                                ["moderation"]
                                ["auto_actions"]
                                ["auto_role"]
                                .AsBsonDocument.ToDictionary(
                                    elem => elem.Name,
                                    elem => elem.Value.AsInt64
                                );
                foreach (var entry in RolesDict)
                {
                    var r = Guild.GetRole((ulong)entry.Value);
                    await Member.GrantRoleAsync(r, "Função Autorole.");
                }
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }



    // PARTNERSHIP:
    public static async Task OnQuitPARTNER(DiscordClient sender, GuildMemberRemoveEventArgs e)
    {
        try
        {
            var Guild = e.Guild;
            var shard = Program._databaseService?.GetShard(Guild, 1);
            var Member = e.Member;





            if (shard[$"{Guild.Id}"]["partner"]["anti-qi"] == 1)
            {
                var ch2 = e.Guild.GetChannel((ulong)shard[$"{Guild.Id}"]["partner"]["configs"]["options"]["channel"].AsInt64);
                var messages = await ch2.GetMessagesAsync(100);
                var msg = messages.Where(msg => msg.MentionedUsers.Any(u => u.Id == Member.Id));
                if (msg.Count() != 0)
                {
                    if (shard[$"{Guild.Id}"]["partner"]["log"] != BsonNull.Value)
                    {
                        var ch = e.Guild.GetChannel((ulong)shard[$"{Guild.Id}"]["partner"]["log"].AsInt64);
                        foreach (var message in msg)
                        {
                            await message.DeleteAsync();
                        }
                        var embed = new DiscordEmbedBuilder()
                        {
                            Description = $"Um **parceiro** {Member.Mention} **saiu da comunidade** e teve seus convites **deletados**.",
                            Color = new DiscordColor("7e67ff")
                        };
                        if (Member.AvatarUrl != null) { embed.WithAuthor($"{Member.DisplayName}", iconUrl: Member.AvatarUrl); }



                        await ch.SendMessageAsync(
                            "Bip bup bip!",
                            embed: embed
                        );
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}