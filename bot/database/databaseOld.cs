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
            Console.ResetColor();
            Console.Write("    ➜  ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"  {y}  |  DBASE  ⚯   Herrscher connected to MongoDB.\n");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ResetColor();
            Console.Write("    ➜  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"  {y}  |  DBASE  ⚯   Error connecting to MongoDB: [ {ex.Message} ]\n");
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
    public BsonDocument? GetHerrscher(DiscordGuild Guild, long Option)
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
                var SelectedHerrscher = collection.Find(filter).FirstOrDefault();



                if (Option == 1) // RETURN HERRSCHER WITH GUILD.
                {
                    if (SelectedHerrscher != null && SelectedHerrscher.Contains(Guild.Id.ToString()))
                    {
                        return SelectedHerrscher;
                    }
                    else
                    {
                        Console.WriteLine($"    ➜  {y}  |  Database Shard\n       The guild {Guild.Name} / {Guild.Id} don't have a database in shard {shard}.");
                    }
                }
                else if (Option == 2) // RETURN SHARD WITHOUT GUILD TO ADD.
                {
                    if (SelectedHerrscher != null && SelectedHerrscher.Contains(Guild.Id.ToString()) == false)
                    {
                        if (SelectedHerrscher.ElementCount >= 1000)
                        {
                            Console.WriteLine($"    ➜  {y}  |  Database Shard\n       The shard {shard} is full!");
                        }
                        else
                        {
                            return SelectedHerrscher;
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
            Console.WriteLine($"    ➜  {y}  |  Database Shard\n       Error in shard select:\n       {ex.Message}");
            return null;
        }
    }
}