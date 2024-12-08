using DSharpPlus;
using DSharpPlus.EventArgs;
using MongoDB.Bson;
using RezetSharp;
using RezetSharp.LuminyCache;




#pragma warning disable CS8629
#pragma warning disable CS8602
public static class OnMessageEvents
{
    // GENERAL:
    public static async Task OnMessageComponents(DiscordClient sender, MessageCreateEventArgs e)
    {
        try
        {
            var LumiCacheForPartnership = new CacheTier1_ForPartnership();
            var LumiCacheForAutoping = new CacheTier1_ForAutoping();
            // ========== FOR PARTNERSHIP:
            if (LumiCacheForPartnership.GetGuild(e.Guild.Id) != null)
            {
                if (LumiCacheForPartnership.GetGuild(e.Guild.Id).Value == e.Channel.Id)
                {
                    var Guild = e.Guild;
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);


                    if (Herrscher[$"{Guild.Id}"]["partner"]["option"] != 0)
                    {
                        if (e.Author.IsBot) { return; }
                        await PartnershipGoMessage.StartThePartnership(e, Herrscher);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                var Guild = e.Guild;
                var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);


                if (Herrscher[$"{Guild.Id}"]["partner"]["option"] != 0)
                {
                    if (e.Author.IsBot) { return; }
                    if ((ulong)Herrscher[$"{Guild.Id}"]["partner"]["configs"]["options"]["channel"].ToInt64() != e.Channel.Id) { return; }
                    await PartnershipGoMessage.StartThePartnership(e, Herrscher);
                    LumiCacheForPartnership.SaveGuild(e.Guild.Id, (ulong)Herrscher[$"{e.Guild.Id}"]["partner"]["configs"]["options"]["channel"].AsInt64);
                }
            }
            // ========== FOR AUTOPING:
            // if (LumiCacheForAutoping.GetGuild(e.Guild.Id) != null)
            // {
            //     if (LumiCacheForAutoping.GetGuild(e.Guild.Id).Value == e.Channel.Id)
            //     {
            //         var Guild = e.Guild;
            //         var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);


            //         if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] != BsonNull.Value)
            //         {
            //             if (e.Author.IsBot) { return; }
            //             await ForAutoping(sender, e, Herrscher);
            //         }
            //     }
            //     else
            //     {
            //         return;
            //     }
            // }
            // else
            // {
            //     var Guild = e.Guild;
            //     var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);


            //     if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] != BsonNull.Value)
            //     {
            //         if (e.Author.IsBot) { return; }
            //         await ForAutoping(sender, e, Herrscher);
            //     }
            // }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Event: On Message\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Message.Author.Username} ( {e.Message.Author.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
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
            Console.WriteLine($"    ➜  Event: For Autoping\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Message.Author.Username} ( {e.Message.Author.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}