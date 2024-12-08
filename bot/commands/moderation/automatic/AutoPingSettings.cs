using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;
using MongoDB.Driver;
using RezetSharp;



#pragma warning disable CS8602
[SlashCommandGroup("autoping", "Autoping setup")]
public class AutoPingSettings : ApplicationCommandModule
{
    [SlashCommand("add", "🔔 | Adicionar ping automático!")]
    public static async Task Add(InteractionContext ctx,
        [Option("channel", "Canal em que os pings serão enviados.")] DiscordChannel Channel,
        [Option("ping", "Ping que será usado.")] DiscordRole Ping,
        [Option("message", "Mensagem que será usada o ping.")] string Message
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;



            await CheckPermi.CheckMemberPermissions(ctx, 3);
            await CheckPermi.CheckBotPermissions(ctx, 3);
            await CheckChannelType.CheckType(ctx, 1, Channel);
            await CheckChannelPermissions.CheckMemberPermissions(ctx, 1, Channel);
            await CheckChannelPermissions.CheckMemberPermissions(ctx, 2, Channel);
            await CheckChannelPermissions.CheckMemberPermissions(ctx, 5, Channel);
            await CheckChannelPermissions.CheckRezetPermissions(ctx, 1, Channel);
            await CheckChannelPermissions.CheckRezetPermissions(ctx, 5, Channel);



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



            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);
            if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] != BsonNull.Value)
            {
                if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"].AsBsonDocument.Contains($"{Channel.Id}"))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Ops! O canal {Channel.Mention} já possui um **Autoping** ativado.")
                    );
                    return;
                }
                var GetAllRoles = Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"].AsBsonDocument.Count();
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
                var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_ping.{Channel.Id}", tropical);
                await collection.UpdateOneAsync(Herrscher, update);



                var embed = new DiscordEmbedBuilder()
                {
                    Color = new DiscordColor("#7e67ff")
                };
                embed.AddField(
                    "<:rezet_settings1:1147163366561955932> Configurations:",
                    $"> Ping: {Ping.Mention} | `{Ping.Id}`\n> Message:\n> ```{Message}```"
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
                var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_ping", tropical);
                await collection.UpdateOneAsync(Herrscher, update);



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
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /autoping add\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




    [SlashCommand("remove", "🔔 | Remover ping automático de um canal.")]
    public static async Task Remove(InteractionContext ctx,
        [Option("channel", "Canal que téra o ping automático removido.")] DiscordChannel Channel
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;



            await CheckPermi.CheckMemberPermissions(ctx, 3);
            await CheckPermi.CheckBotPermissions(ctx, 3);
            await CheckChannelType.CheckType(ctx, 1, Channel);
            await CheckChannelPermissions.CheckMemberPermissions(ctx, 1, Channel);
            await CheckChannelPermissions.CheckMemberPermissions(ctx, 2, Channel);
            await CheckChannelPermissions.CheckMemberPermissions(ctx, 5, Channel);
            await CheckChannelPermissions.CheckRezetPermissions(ctx, 1, Channel);
            await CheckChannelPermissions.CheckRezetPermissions(ctx, 5, Channel);




            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);
            if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] == BsonNull.Value)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Ops! Não há nenhum canal com a função **Autoping**.")
                );
                return;
            }
            else if (!Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"].AsBsonDocument.Contains($"{Channel.Id}"))
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Ops! O canal {Channel.Mention} não possui a função **Autoping**.")
                );
                return;
            }
            else if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"].AsBsonDocument.Count() == 1)
            {
                var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_ping", BsonNull.Value);
                await collection.UpdateOneAsync(Herrscher, update);


                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Nice! O canal {Channel.Mention} teve a função **Autoping** removida.")
                );
            }
            else
            {
                var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Unset($"{Guild.Id}.moderation.auto_actions.auto_ping.{Channel.Id}");
                await collection.UpdateOneAsync(Herrscher, update);


                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Nice! O canal {Channel.Mention} teve a função **Autoping** removida.")
                );
            }
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /autoping remove\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




    [SlashCommand("clear", "🔔 | Remover todos os pings automáticos.")]
    public static async Task Clear(InteractionContext ctx)
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;



            // PERMISSIONS:
            await CheckPermi.CheckMemberPermissions(ctx, 3);
            await CheckPermi.CheckBotPermissions(ctx, 3);
            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);
            if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] == BsonNull.Value)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Não há nenhum canal com a função **Autoping**!")
                );
                return;
            }



            var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_ping", BsonNull.Value);
            await collection.UpdateOneAsync(Herrscher, update);


            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Nice! Todos os cargos foram removidos da função **Autorole**.")
            );
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /autoping clear\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }
}