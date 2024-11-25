using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;
using MongoDB.Driver;
using RezetSharp;



#pragma warning disable CS8602
[SlashCommandGroup("autorole", "Autorole Setup.")]
public class AutoRoleSettings : ApplicationCommandModule
{
    [SlashCommand("activate", "🎟️ | Activate an automatic join role!")]
    public static async Task Add(InteractionContext ctx,
        [Option("role", "The role that will be given to the new member!")] DiscordRole Role
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;



            // PERMISSIONS:
            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);
            var botMember = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);
            var highestBotRole = botMember.Roles.OrderByDescending(r => r.Position).FirstOrDefault();
            if (Role.Position >= highestBotRole.Position)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Eu não posso atribuir um cargo maior que o meu maior cargo atual!"));
                return;
            }
            var highestMemberRole = ctx.Member.Roles.OrderByDescending(r => r.Position).FirstOrDefault();
            if (Role.Position >= highestMemberRole.Position)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Você não pode selecionar um cargo maior que o seu maior cargo atual!"));
                return;
            }



            var shard = EngineV1.HerrscherRazor.GetHerrscherDocument(Guild);
            if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"] != BsonNull.Value)
            {
                if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"].AsBsonDocument.Contains($"{Role.Id}"))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Ops! O cargo `{Role.Name} [ {Role.Id} ]` já está adicionado a função **Autorole**.")
                    );
                    return;
                }
                var GetAllRoles = shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"].AsBsonDocument.Count();
                if (GetAllRoles >= 10)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Ops! O número máximo de cargos automáticos permitidos é **10**!")
                    );
                    return;
                }



                var collection = EngineV1.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_role.{Role.Id}", Role.Id);
                await collection.UpdateOneAsync(shard, update);


                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Nice! O cargo `@{Role.Name} [ {Role.Id} ]` foi adicionado a função **Autorole**.")
                );
                return;
            }
            else
            {
                var t = new BsonDocument
                {
                    { $"{Role.Id}", (long)Role.Id }
                };
                var collection = EngineV1.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_role", t);
                await collection.UpdateOneAsync(shard, update);


                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Nice! O cargo `{Role.Name} [ {Role.Id} ]` foi adicionado a função **Autorole**.")
                );
            }
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando.\n\n> `{ex.Message}`")
            );
            return;
        }
    }



    [SlashCommand("uncativate", "🎟️ | Uncativate an automatic role.")]
    public static async Task Remove(InteractionContext ctx,
        [Option("role", "The role that will be removed.")] DiscordRole Role
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;



            // PERMISSIONS:
            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);



            var shard = EngineV1.HerrscherRazor.GetHerrscherDocument(Guild);
            if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"] == BsonNull.Value)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Ops! Não há nenhum cargo adicionado a função **Autorole**.")
                );
                return;
            }
            else if (!shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"].AsBsonDocument.Contains($"{Role.Id}"))
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Ops! Não há nenhum cargo `{Role.Name} [ {Role.Id} ]` adicionado a função **Autorole**.")
                );
                return;
            }
            else if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"].AsBsonDocument.Count() == 1)
            {
                var collection = EngineV1.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_role", BsonNull.Value);
                await collection.UpdateOneAsync(shard, update);


                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Nice! O cargo `{Role.Name} [ {Role.Id} ]` foi removido da função **Autorole**.")
                );
            }
            else
            {
                var collection = EngineV1.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Unset($"{Guild.Id}.moderation.auto_actions.auto_role.{Role.Id}");
                await collection.UpdateOneAsync(shard, update);


                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Nice! O cargo `{Role.Name} [ {Role.Id} ]` foi removido da função **Autorole**.")
                );
            }
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando.\n\n> `{ex.Message}`")
            );
            return;
        }
    }



    [SlashCommand("clear", "🎟️ | Remove all automatic roles!")]
    public static async Task Clear(InteractionContext ctx)
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;



            // PERMISSIONS:
            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);



            var shard = EngineV1.HerrscherRazor.GetHerrscherDocument(Guild);
            if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"] == BsonNull.Value)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Não há nenhum canal com a função **Autorole**!")
                );
                return;
            }



            var collection = EngineV1.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.auto_actions.auto_role", BsonNull.Value);
            await collection.UpdateOneAsync(shard, update);


            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Nice! Todos os cargos foram removidos da função **Autorole**.")
            );
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando.\n\n> `{ex.Message}`")
            );
            return;
        }
    }
}