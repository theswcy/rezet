using DSharpPlus.EventArgs;
using DSharpPlus;
using MongoDB.Driver;
using MongoDB.Bson;
using RezetSharp;
using DSharpPlus.Entities;




#pragma warning disable CS8602
public class PartnershipDashboardPrimaryButtons
{
    public static async Task PrimaryButtons(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // DELETE MESSAGE:
            if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_PAexit")
            {
                try
                {
                    await e.Message.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Delete Message Button\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ACTIVATE PARTNERSHIP FUNCTION:
            else if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_APFB")
            {
                try
                {
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.option", 1);
                    await collection.UpdateOneAsync(Herrscher, update);


                    await e.Message.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent("Nice, a função **partnership** foi ativada!"),
                        suppressEmbeds: true
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Setup Activate\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // UNACTIVATE PARTNERSHIP FUNCTION:
            else if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_PAuna")
            {
                try
                {
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.option", 0);
                    await collection.UpdateOneAsync(Herrscher, update);


                    await e.Message.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent("Coma poeira! A função **partnership** foi desativada!"),
                        suppressEmbeds: true
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Unactivate Function\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ACTIVATE RANKING:
            else if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_ALB")
            {
                try
                {
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.leaderboard.option", 1);
                    await collection.UpdateOneAsync(Herrscher, update);


                    await e.Message.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent("Beleza, dashboard reiniciada! O módulo **ranking** da função **partnership** foi ativado, agora todos os usuários responsáveis pelas parcerias terão seus pontos contados."),
                            suppressEmbeds: true
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Dashboard Activate Ranking\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Dashboard\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}