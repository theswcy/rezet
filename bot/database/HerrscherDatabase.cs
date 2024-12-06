using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Driver;



// ========== HERRSCHER DATABASE SYSTEM V1.0
public class HerrscherService
{
    public readonly IMongoDatabase? _database;



    // ========== HERRSCHER CONNECT:
    public HerrscherService(string ConnectionString, string HerrscherExtension)
    {
        // ========== DATA TRY:
        try
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            var client = new MongoClient(ConnectionString);
            // FIND HERRSCHER EXTENSION BY NAME:
            _database = client.GetDatabase(HerrscherExtension);




            Console.ResetColor();
            Console.Write("    ➜  ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"  {y}  |  DBASE  ⚯   Herrscher connected from MongoDB.\n");
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



    // ========== GET ONE HERRSCHER DOCUMENT:
    public BsonDocument? GetHerrscherDocument(DiscordGuild Guild)
    {
        int x = 1; // HERRSCHERS TOTAL.
        // ========== TRY RETURN DATA:
        try
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            for (int HerrscherNumber = 0; HerrscherNumber < x; HerrscherNumber++)
            {
                // GET HERRSCHER EXTENSIONS:
                var HerrscherExtension = _database?.GetCollection<BsonDocument>("guilds");
                var HerrscherFilter = Builders<BsonDocument>.Filter.Eq("_id", $"shard_{HerrscherNumber}");
                var SelectedHerrscher = HerrscherExtension.Find(HerrscherFilter).FirstOrDefault();



                // RETURN THE HERRSCHER DOCUMENT IF CONTAIS GUILD:
                if (SelectedHerrscher != null && SelectedHerrscher.Contains($"{Guild.Id}"))
                {
                    return SelectedHerrscher;
                }
                else
                {
                    Console.WriteLine($"    ➜  {y}  |  Database Herrscher\n       The guild {Guild.Name} / {Guild.Id} don't have a database in Herrscher {HerrscherNumber}.");
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.WriteLine($"    ➜  {y}  |  Database Herrscher\n       Error in Herrscher select:\n       {ex.Message}");
            return null;
        }
    }



    // ========== GER HERRSCHER DOCUMENT TO ADD GUILD:
    public BsonDocument? GetHerrscherToAdd(DiscordGuild Guild)
    {
        int x = 1;
        try
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            for (int HerrscherNumber = 0; HerrscherNumber < x; HerrscherNumber++)
            {
                // GET HERRSCHER EXTENSION:
                var HerrscherExtension = _database?.GetCollection<BsonDocument>("guilds");
                var HerrscherFilter = Builders<BsonDocument>.Filter.Eq("_id", $"shard_{HerrscherNumber}");
                var SelectedHerrscher = HerrscherExtension.Find(HerrscherFilter).FirstOrDefault();



                // RETURN; THE HERRSCHER EXTENSION:
                if (SelectedHerrscher != null && !SelectedHerrscher.Contains($"{Guild.Id}"))
                {
                    if (SelectedHerrscher.ElementCount < 1000)
                    {
                        return SelectedHerrscher;
                    }
                    else
                    {
                        Console.WriteLine($"    ➜  {y}  |  Database Herrscher\n       The Herrscher {HerrscherNumber} is full!");
                    }
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.WriteLine($"    ➜  {y}  |  Database Herrscher\n       Error in Herrscher select:\n       {ex.Message}");
            return null;
        }
    }
}