using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;




#pragma warning disable CS8602
public class PartnershipOthersConfigs
{
    public static async Task PartnershipMoreOptions(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        // ========== PARTNERSHIP MORE OPTIONS
        if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PEConfigs")
        {
            // DELETE ON EVERYONE:
            if (e.Values[0] == "everyone")
            {
                await e.Interaction.DeferAsync();



                var shard = Program._databaseService?.GetShard(e.Guild, 1);



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



                if (shard[$"{e.Guild.Id}"]["partner"]["anti-eh"] == 0)
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
            // DELETE ON QUIT:
            else if (e.Values[0] == "delete")
            {
                await e.Interaction.DeferAsync();



                var shard = Program._databaseService?.GetShard(e.Guild, 1);



                var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                if (shard[$"{e.Guild.Id}"]["partner"]["anti-qi"] == 0)
                {
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-qi", 1);
                    await collection.UpdateOneAsync(shard, update);


                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! Agora, os parceiros que sairem terão seus convites removidos do canal de parcerias!")
                            .AddComponents(button)
                    );
                }
                else
                {
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-qi", 0);
                    await collection.UpdateOneAsync(shard, update);

                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! Módulo desativado!")
                    );
                    await Task.Delay(2000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                }
            }
        }




        // MODE 0:
        else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode0")
        {
            var shard = Program._databaseService?.GetShard(e.Guild, 1);
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-eh", 0);
            await collection.UpdateOneAsync(shard, update);



            await e.Message.ModifyAsync(
                builder: new DiscordMessageBuilder()
                    .WithContent("Bip bup bip! Módulo desativado!"),
                suppressEmbeds: true
            );
            await Task.Delay(2000);
            await e.Message.DeleteAsync();
        }
        // MODE 1:
        else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode1")
        {
            var shard = Program._databaseService?.GetShard(e.Guild, 1);
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-eh", 1);
            await collection.UpdateOneAsync(shard, update);



            await e.Message.ModifyAsync(
                builder: new DiscordMessageBuilder()
                    .WithContent("Bip bup bip! Módulo ativado no **Mode 1**!"),
                suppressEmbeds: true
            );
            await Task.Delay(3000);
            await e.Message.DeleteAsync();
        }
        // MODE 2:
        else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode2")
        {
            var shard = Program._databaseService?.GetShard(e.Guild, 1);
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-eh", 2);
            await collection.UpdateOneAsync(shard, update);



            await e.Message.ModifyAsync(
                builder: new DiscordMessageBuilder()
                    .WithContent("Bip bup bip! Módulo ativado no **Mode 2**!"),
                suppressEmbeds: true
            );
            await Task.Delay(3000);
            await e.Message.DeleteAsync();
        }
        // MODE 3:
        else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode3")
        {
            var shard = Program._databaseService?.GetShard(e.Guild, 1);
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.anti-eh", 3);
            await collection.UpdateOneAsync(shard, update);



            await e.Message.ModifyAsync(
                builder: new DiscordMessageBuilder()
                    .WithContent("Bip bup bip! Módulo ativado no **Mode 3**!"),
                suppressEmbeds: true
            );
            await Task.Delay(3000);
            await e.Message.DeleteAsync();
        }
    }
}