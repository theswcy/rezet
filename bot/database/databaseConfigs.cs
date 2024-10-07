using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Driver;


#pragma warning disable CS8603
public class DatabaseService
{
    public readonly IMongoDatabase? database;

    // ========== DATABASE BUILD START:
    public DatabaseService(string connectionString, string databaseName)
    {
        // DATA TRY:
        try
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            var client = new MongoClient(connectionString);
            // FIND DATABASE BY NAME:
            database = client.GetDatabase(databaseName);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"O [  {y}  |  DATABASE  ] Connected to MongoDB.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"X [  {y}  |  DATABASE  ] Error connecting to MongoDB: [ {ex.Message} ]");
            Console.ResetColor();
        }
    }



    // ========== GET COLLECTION:
    // public IMongoCollection<T> GetCollection<T>(string collectionName)
    // {
    //     try
    //     {
    //         var collection = database?.GetCollection<T>(collectionName);
    //         Console.WriteLine($"O [  DATABASE  ] Accessed class: [ {collectionName} ]");
    //         return collection;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"X [  DATABASE  ] Error accessing collection: [ {ex.Message} ]");
    //         return null;
    //     }
    // }
    // ========== GET DOCUMENT AND SHARD:
    public BsonDocument? GetShard(DiscordGuild Guild, long Option)
    {
        try
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            long x = 1;
            for (long shard = 0; shard < x; shard++)
            {
                var collection = database?.GetCollection<BsonDocument>("guilds");
                var filter = Builders<BsonDocument>.Filter.Eq("_id", $"shard_{shard}");
                var SelectedShard = collection.Find(filter).FirstOrDefault();



                if (Option == 1) // RETURN SHARD WITH GUILD.
                {
                    if (SelectedShard != null && SelectedShard.Contains(Guild.Id.ToString()))
                    {
                        return SelectedShard;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"X [  {y}  |  DATABASE  ] The guild ( {Guild.Name} / {Guild.Id} ) don't have a database in shard {shard}.");
                        Console.ResetColor();
                    }
                }
                else if (Option == 2) // RETURN SHARD WITHOUT GUILD TO ADD.
                {
                    if (SelectedShard != null && SelectedShard.Contains(Guild.Id.ToString()) == false)
                    {
                        if (SelectedShard.ElementCount >= 100)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"X [  {y}  |  DATABASE  ] The shard {shard} is full.");
                            Console.ResetColor();
                        }
                        else
                        {
                            return SelectedShard;
                        }
                    }
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"X [  {y}  |  DATABASE  ] Error in shard select: [ {ex.Message} ]");
            Console.ResetColor();
            return null;
        }
    }
}