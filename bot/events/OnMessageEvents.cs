using DSharpPlus;
using DSharpPlus.EventArgs;
using MongoDB.Bson;
using RezetSharp;




#pragma warning disable CS8602
public static class OnMessageEvents
{
    // GENERAL:
    public static async Task OnMessageComponents(DiscordClient sender, MessageCreateEventArgs e)
    {
        try
        {
            var Guild = e.Guild;
            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);




            // FOR AUTOPING:
            if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] != BsonNull.Value)
            {
                if (e.Author.IsBot) { return; }
                await ForAutoping(sender, e, Herrscher);
            }



            // FOR PARTNERSHIP:
            if (Herrscher[$"{Guild.Id}"]["partner"]["option"] != 0)
            {
                if (e.Author.IsBot) { return; }
                if ((ulong)Herrscher[$"{Guild.Id}"]["partner"]["configs"]["options"]["channel"].ToInt64() != e.Channel.Id) { return; }
                await PartnershipGoMessage.StartThePartnership(e, Herrscher);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Author.Username} ( {e.Author.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }



    // AUTOPING:
    public static async Task ForAutoping(DiscordClient sender, MessageCreateEventArgs e, BsonDocument Herrscher)
    {
        try
        {
            var Guild = e.Guild;
            var PingDict = Herrscher
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