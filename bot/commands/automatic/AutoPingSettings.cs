using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;
using MongoDB.Driver;
using Rezet;



#pragma warning disable CS8602
[SlashCommandGroup("autoping", "Autoping setup")]
public class AutoPingSettings : ApplicationCommandModule
{
    [SlashCommand("add", "🔔 | Add an automatic message ping!")]
    public static async Task Add(InteractionContext ctx,
        [Option("channel", "The channel that the ping will be sent.")] DiscordChannel Channel,
        [Option("ping", "The ping that will used.")] DiscordRole Ping,
        [Option("message", "The message that will be sent with the ping.")] string Message
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;
            var shard = Program._databaseService?.GetShard(Guild, 1);
            if (shard == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                Console.ResetColor();
                return;
            }


            await CheckPermi.CheckMemberPermissions(ctx, 3);
            await CheckPermi.CheckBotPermissions(ctx, 3);


            if (Message.Length > 100)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Máximo **100** caracteres na mensagem!")
                );
                return;
            }
            if (Message.Length < 1)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Mínimo **1** caracteres na mensagem!")
                );
                return;
            }


            if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] != BsonNull.Value)
            {
                if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"].AsBsonDocument.Contains($"{Channel.Id}"))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Ops! O canal {Channel.Mention} já possui um **Autoping** ativado.")
                    );
                    return;
                }
                var GetAllRoles = shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"].AsBsonDocument.Count();
                if (GetAllRoles >= 10)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Ops! O número máximo de canais com pings automáticos permitidos é **10**!")
                    );
                    return;
                }



                var tropical = new BsonDocument
                {
                    { "ping", (long)Ping.Id },
                    { "message", Message }
                };
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_ping.{Channel.Id}", tropical);
                await collection.UpdateOneAsync(shard, update);



                var embed = new DiscordEmbedBuilder()
                {
                    Color = new DiscordColor("#7e67ff")
                };
                embed.AddField(
                    "<:rezet_settings1:1147163366561955932> Configurations:",
                    $"> Ping: @{Ping.Mention} | `{Ping.Id}`\n> Message:\n> ```{Message}```"
                );



                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Nice! O canal {Channel.Mention} recebeu a função **Autoping**.")
                        .AddEmbed(embed)
                );
            }
            else
            {
                var tropical = new BsonDocument
                {
                    { $"{Channel.Id}", new BsonDocument
                        {
                            { "ping", (long)Ping.Id },
                            { "message", Message }
                        }
                    }
                };
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_ping", tropical);
                await collection.UpdateOneAsync(shard, update);



                var embed = new DiscordEmbedBuilder()
                {
                    Color = new DiscordColor("#7e67ff")
                };
                embed.AddField(
                    "<:rezet_settings1:1147163366561955932> Configurations:",
                    $"> Ping: @{Ping.Mention} | `{Ping.Id}`\n> Message:\n> ```{Message}```"
                );



                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Nice! O canal {Channel.Mention} recebeu a função **Autoping**.")
                        .AddEmbed(embed)
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }




    [SlashCommand("remove", "🔔 | Remove an automatic message ping save.")]
    public static async Task Remove(InteractionContext ctx,
        [Option("channel", "The channel that will be removed.")] DiscordChannel Channel
    )
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        var Guild = ctx.Guild;
        var shard = Program._databaseService?.GetShard(Guild, 1);
        if (shard == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
            Console.ResetColor();
            return;
        }


        await CheckPermi.CheckMemberPermissions(ctx, 3);
        await CheckPermi.CheckBotPermissions(ctx, 3);


        if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] == BsonNull.Value)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent("Ops! Não há nenhum canal com a função **Autoping**.")
            );
            return;
        }
        else if (!shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"].AsBsonDocument.Contains($"{Channel.Id}"))
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Ops! O canal {Channel.Mention} não possui a função **Autoping**.")
            );
            return;
        }
        else if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"].AsBsonDocument.Count() == 1)
        {
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_ping", BsonNull.Value);
            await collection.UpdateOneAsync(shard, update);


            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Nice! O canal {Channel.Mention} teve a função **Autoping** removida.")
            );
        }
        else
        {
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Unset($"{Guild.Id}.moderation.auto_actions.auto_ping.{Channel.Id}");
            await collection.UpdateOneAsync(shard, update);


            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Nice! O canal {Channel.Mention} teve a função **Autoping** removida.")
            );
        }
    }




    [SlashCommand("clear", "🔔 | Clear all automatic message ping save.")]
    public static async Task Clear(InteractionContext ctx)
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;
            var shard = Program._databaseService?.GetShard(Guild, 1);
            if (shard == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                Console.ResetColor();
                return;
            }



            // PERMISSIONS:
            await CheckPermi.CheckMemberPermissions(ctx, 3);
            await CheckPermi.CheckBotPermissions(ctx, 3);
            if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] == BsonNull.Value)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Não há nenhum canal com a função **Autoping**!")
                );
                return;
            }



            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_ping", BsonNull.Value);
            await collection.UpdateOneAsync(shard, update);


            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Nice! Todos os cargos foram removidos da função **Autorole**.")
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}