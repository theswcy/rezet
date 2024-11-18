using DSharpPlus.EventArgs;
using DSharpPlus;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;
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
                await e.Message.DeleteAsync();
            }
            // ACTIVATE PARTNERSHIP FUNCTION:
            else if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_APFB")
            {
                var shard = Program._databaseService?.GetShard(e.Interaction.Guild, 1);
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.option", 1);
                await collection.UpdateOneAsync(shard, update);


                await e.Message.ModifyAsync(
                    builder: new DiscordMessageBuilder()
                        .WithContent("Nice, a função **partnership** foi ativada!"),
                    suppressEmbeds: true
                );
            }
            // UNACTIVATE PARTNERSHIP FUNCTION:
            else if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_PAuna")
            {
                var shard = Program._databaseService?.GetShard(e.Guild, 1);
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.option", 0);
                await collection.UpdateOneAsync(shard, update);


                await e.Message.ModifyAsync(
                    builder: new DiscordMessageBuilder()
                        .WithContent("Coma poeira! A função **partnership** foi desativada!"),
                    suppressEmbeds: true
                );
            }
            // ACTIVATE RANKING:
            else if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_ALB")
            {
                var shard = Program._databaseService?.GetShard(e.Guild, 1);
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.leaderboard.option", 1);
                await collection.UpdateOneAsync(shard, update);


                await e.Message.ModifyAsync(
                    builder: new DiscordMessageBuilder()
                        .WithContent("Beleza, dashboard reiniciada! O módulo **ranking** da função **partnership** foi ativado, agora todos os usuários responsáveis pelas parcerias terão seus pontos contados."),
                        suppressEmbeds: true
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}