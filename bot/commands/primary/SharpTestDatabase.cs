using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Rezet;
using RezetSharp;



#pragma warning disable CS8602
public class SharpTestDatabase : BaseCommandModule
{
    [Command("sharp-1")]
    public async Task SharpToTest(CommandContext ctx)
    {
        try
        {
            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(ctx.Guild);
            var collection = EngineV8X.HerrscherRazor._database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update
                .Set($"{ctx.Guild.Id}.partner.ticket.count", 0);
            await collection.UpdateOneAsync(Herrscher, update);
            await ctx.RespondAsync("okay!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}