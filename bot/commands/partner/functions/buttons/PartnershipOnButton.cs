using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;



public class PartnershipOnButtonClick
{
    public static async Task Button_PSActivate(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PSActivate")
            {
                var Guild = e.Interaction.Guild;
                var t = await e.Message.RespondAsync("Bip bup bip, a função **partnership** foi ativada! Use o comando `/partnership dashboard` novamente.");



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.option", 1);



                await Task.Delay(500);
                await e.Message.DeleteAsync();
                await Task.Delay(10000);
                await t.DeleteAsync();
            }
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PSNotActivate")
            {
                await e.Message.DeleteAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}