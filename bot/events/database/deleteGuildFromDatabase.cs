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
            Console.WriteLine(ex);
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  {y}  |  GUILDE REMOVE  ] Failed to acess guild ( {e.Name} / {e.Id} ): The guild has no database.");
                Console.ResetColor();
                return;
            }
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Unset($"{e.Id}");
            var result = await collection.UpdateOneAsync(shard, update);

            if (result.ModifiedCount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"O [  {y}  |  GUILD REMOVE  ] Removed guild ( {e.Name} )");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  {y}  |  GUILD REMOVE  ] Failed to remove guild ( {e.Name} / {e.Id} ) in database.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"X [  {y}  |  GUILD DELETE  ] Error removing guild from databse: [ {ex.Message} ]");
            Console.ResetColor();
        }
    }
}