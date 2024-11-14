using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Bson;
using MongoDB.Driver;
using Rezet;



#pragma warning disable CS8602
public static class DeleteGuildDB
{
    public static async Task OnGuildDelete(DiscordClient client, GuildDeleteEventArgs e)
    {
        try
        {
            await DeleteGuild(e.Guild);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
    public static async Task DeleteGuild(DiscordGuild e)
    {
        try
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            var shard = Program._databaseService?.GetShard(e, 1);
            if (shard == null)
            {
                Console.WriteLine($"    ⌬  {y}  |  GUILDE REMOVE\n    ➜  Failed to acess guild: {e.Name} / {e.Id}\n\n\n");
                return;
            }
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Unset($"{e.Id}");
            var result = await collection.UpdateOneAsync(shard, update);

            if (result.ModifiedCount > 0)
            {
                Console.WriteLine($"    ⌬  {y}  |  GUILD REMOVE\n    ➜  Removed guild: {e.Name} / {e.Id}\n\n\n");
            }
            else
            {
                Console.WriteLine($"    ⌬  {y}  |  GUILD REMOVE\n    ➜  Failed to remove guild {e.Name} / {e.Id}\n\n\n");
            }
        }
        catch (Exception ex)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.WriteLine($"    ⌬  {y}  |  GUILD CREATE\n    ➜  Error adding guild to database.\n    ➜  Error: {ex.Message}\n{ex.StackTrace}\n\n\n");
        }
    }
}