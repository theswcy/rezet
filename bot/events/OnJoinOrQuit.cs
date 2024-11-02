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
            if (shard == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                Console.ResetColor();
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
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
            if (shard == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                Console.ResetColor();
                return;
            }





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
            Console.WriteLine(ex);
        }
    }
}