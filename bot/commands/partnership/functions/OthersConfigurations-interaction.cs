using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using RezetSharp;




#pragma warning disable CS8602
public class PartnershipOthersConfigs
{
    public static async Task PartnershipMoreOptions(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // ========== PARTNERSHIP MORE OPTIONS
            if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PEConfigs")
            {
                // DELETE ON EVERYONE:
                if (e.Values[0] == "everyone")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();



                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);



                        var embed = new DiscordEmbedBuilder()
                        {
                            Description = "<:rezet_exclamation:1164417019303702570> **Atenção**: Essa é um módulo que possui ferramentas de **risco**, selecione um dos modos abaixo pensando **3 vezes** ou mais!",
                            Color = new DiscordColor("ff0000")
                        };
                        embed.AddField(
                            "<:rezet_settings1:1147163366561955932> Mode 1 ( righly recommended )",
                            "> Deletar mensagens de parceria com menções **everyone/here**."
                        );
                        embed.AddField(
                            "<:rezet_settings1:1147163366561955932> Mode 2",
                            "> Deletar mensagens de parceria com menções **everyone/here** e **expulsar** o **representante** no convite."
                        );
                        embed.AddField(
                            "<:rezet_settings1:1147163366561955932> Mode 3",
                            "> Deletar mensagens de parceria com menções **everyone/here** e **banir** o **representante** no convite."
                        );



                        var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");
                        var button1 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_OSMode1", "Mode 1");
                        var button2 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_OSMode2", "Mode 2");
                        var button3 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_OSMode3", "Mode 3");
                        var button4 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_OSMode0", "Unactivate");



                        if (Herrscher[$"{e.Guild.Id}"]["partner"]["anti-eh"] == 0)
                        {
                            await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Bip bup bip!")
                                .AddEmbed(embed)
                                .AddComponents(button, button1, button2, button3)
                            );
                        }
                        else
                        {
                            await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Bip bup bip!")
                                .AddEmbed(embed)
                                .AddComponents(button, button1, button2, button3, button4)
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership More Options - Delete On Everyone Choice\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // DELETE ON QUIT:
                else if (e.Values[0] == "delete")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();



                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);



                        var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");
                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        if (Herrscher[$"{e.Guild.Id}"]["partner"]["anti-qi"] == 0)
                        {
                            var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-qi", 1);
                            await collection.UpdateOneAsync(Herrscher, update);


                            await e.Interaction.CreateFollowupMessageAsync(
                                new DiscordFollowupMessageBuilder()
                                    .WithContent("Bip bup bip! Agora, os parceiros que sairem terão seus convites removidos do canal de parcerias!")
                                    .AddComponents(button)
                            );
                        }
                        else
                        {
                            var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-qi", 0);
                            await collection.UpdateOneAsync(Herrscher, update);

                            await e.Interaction.CreateFollowupMessageAsync(
                                new DiscordFollowupMessageBuilder()
                                    .WithContent("Bip bup bip! Módulo desativado!")
                            );
                            await Task.Delay(2000);
                            await e.Interaction.DeleteOriginalResponseAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership More Options - Delete On Quit Choice\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
            }




            // MODE 0:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode0")
            {
                try
                {
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-eh", 0);
                    await collection.UpdateOneAsync(Herrscher, update);



                    await e.Message.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent("Bip bup bip! Módulo desativado!"),
                        suppressEmbeds: true
                    );
                    await Task.Delay(2000);
                    await e.Message.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership More Options - Delete On Everyone Choice 0\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // MODE 1:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode1")
            {
                try
                {
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-eh", 1);
                    await collection.UpdateOneAsync(Herrscher, update);



                    await e.Message.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent("Bip bup bip! Módulo ativado no **Mode 1**!"),
                        suppressEmbeds: true
                    );
                    await Task.Delay(3000);
                    await e.Message.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership More Options - Delete On Everyone Choice 1\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // MODE 2:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode2")
            {
                try
                {
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-eh", 2);
                    await collection.UpdateOneAsync(Herrscher, update);



                    await e.Message.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent("Bip bup bip! Módulo ativado no **Mode 2**!"),
                        suppressEmbeds: true
                    );
                    await Task.Delay(3000);
                    await e.Message.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership More Options - Delete On Everyone Choice 2\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // MODE 3:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode3")
            {
                try
                {
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-eh", 3);
                    await collection.UpdateOneAsync(Herrscher, update);



                    await e.Message.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent("Bip bup bip! Módulo ativado no **Mode 3**!"),
                        suppressEmbeds: true
                    );
                    await Task.Delay(3000);
                    await e.Message.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership More Options - Delete On Everyone Choice 3\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership More Options\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}