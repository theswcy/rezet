using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Rezet;



#pragma warning disable CS8602
public class SharpTestDatabase : BaseCommandModule
{
    [Command("sharp-1")]
    public async Task SharpToTest(CommandContext ctx)
    {
        try
        {
            var shard = Program._databaseService?.GetShard(ctx.Guild, 1);
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update
                .Set($"{ctx.Guild.Id}.partner.ticket.count", 0);
            await collection.UpdateOneAsync(shard, update);
            await ctx.RespondAsync("okay!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}