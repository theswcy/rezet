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
            await ctx.RespondAsync("okay!");
            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(ctx.Guild);
            var collection = EngineV8X.HerrscherRazor._database?.GetCollection<BsonDocument>("guilds");


            int o = 0;
            foreach (var Entry in EngineV8X.RezetRazor.Guilds.Values)
            {
                if (Herrscher.AsBsonDocument.Contains($"{Entry.Id}"))
                {
                    o++;
                    DateTime now = DateTime.Now; var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
                    Console.ResetColor();
                    Console.Write("    ➜  ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"  {y}  |  SHARP  ⚯   [ #{o} ] Guild {Entry.Name} [ {Entry.Id} ] Updated.\n");
                    Console.ResetColor();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}