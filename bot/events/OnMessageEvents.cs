using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;




#pragma warning disable CS8602
public static class OnMessageEvents
{
    // GENERAL:
    public static async Task OnMessageComponents(DiscordClient sender, MessageCreateEventArgs e)
    {
        try
        {
            var Guild = e.Guild;
            var shard = Program._databaseService?.GetShard(Guild, 1);




            // FOR AUTOPING:
            if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] != BsonNull.Value)
            {
                if (e.Author.IsBot) { return; }
                await ForAutoping(sender, e, shard);
            }



            // FOR PARTNERSHIP:
            if (shard[$"{Guild.Id}"]["partner"]["option"] != 0)
            {
                if (e.Author.IsBot) { return; }
                if ((ulong)shard[$"{Guild.Id}"]["partner"]["configs"]["options"]["channel"].ToInt64() != e.Channel.Id) { return; }
                await PartnershipGoMessage.StartThePartnership(e, shard);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Author.Username} ( {e.Author.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }



    // AUTOPING:
    public static async Task ForAutoping(DiscordClient sender, MessageCreateEventArgs e, BsonDocument shard)
    {
        try
        {
            var Guild = e.Guild;
            var PingDict = shard
                            [$"{Guild.Id}"]
                            ["moderation"]
                            ["auto_actions"]
                            ["auto_ping"]
                            .AsBsonDocument;
            foreach (var entry in PingDict.Elements)
            {
                if (e.Channel.Id == ulong.Parse(entry.Name))
                {
                    var pingData = entry.Value.AsBsonDocument;
                    var ping = Guild.GetRole((ulong)pingData["ping"].AsInt64);


                    await e.Message.RespondAsync(
                        $"{ping.Mention} | {pingData["message"].AsString}"
                    );
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Author.Username} ( {e.Author.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}