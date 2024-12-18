using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;
using MongoDB.Driver;
using RezetSharp;



#pragma warning disable CS8602
[SlashCommandGroup("modding", "Modding logs.")]
public class ModdingLogs : ApplicationCommandModule
{
    [SlashCommandGroup("logs", "Modding logs command.")]
    public class ModdingLogsCommands : ApplicationCommandModule
    {
        [SlashCommand("add", "📜 | Adicionar um canal de logs.")]
        public static async Task Setup(InteractionContext ctx,
            [Option("config", "Configurações que serão registradas.")]
                [Choice("Message delete / modify.", 1)]
                [Choice("Moderation actions.", 2)]
                [Choice("Roles add / remove / create / modify / delete.", 3)]
                [Choice("Channels create / delete / modify.", 4)]
                [Choice("Guild management.", 5)]
                long Config,
            [Option("channel", "Canal que será usado para enviar os registros.")] DiscordChannel Channel
        )
        {
            try
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                var Guild = ctx.Guild;



                // PERMISSIONS:
                await CheckPermi.CheckMemberPermissions(ctx, 5);
                await CheckPermi.CheckBotPermissions(ctx, 5);
                await CheckPermi.CheckMemberPermissions(ctx, 3);
                await CheckPermi.CheckBotPermissions(ctx, 3);
                await CheckChannelType.CheckType(ctx, 1, Channel);
                await CheckChannelPermissions.CheckMemberPermissions(ctx, 1, Channel);
                await CheckChannelPermissions.CheckMemberPermissions(ctx, 2, Channel);
                await CheckChannelPermissions.CheckMemberPermissions(ctx, 5, Channel);
                await CheckChannelPermissions.CheckRezetPermissions(ctx, 1, Channel);
                await CheckChannelPermissions.CheckRezetPermissions(ctx, 5, Channel);



                var ConfigLog = "";
                var r = "";
                var y = "";
                // MESSAGES:
                switch (Config)
                {
                    case 1:
                        ConfigLog = "messages_channel";
                        r = "Messages delete / modify";
                        y = "`Messages Delete`\n`Messages Modify`";
                        break;
                    case 2:
                        ConfigLog = "moderation_channel";
                        r = "Moderation actions";
                        y = "`Member Kick`\n`Member Ban / Unban`\n`Member Timeout / Untimeout`\n`Member Warn / Unwarn / Warn Modify`";
                        break;
                    case 3:
                        ConfigLog = "modified_roles";
                        r = "Roles add / remove / create / modify / delete";
                        y = "`Role Add`\n`Role Remove`\n`Role Create`\n`Role Modify`\n`Role Delete`";
                        break;
                    case 4:
                        ConfigLog = "modified_channels";
                        r = "Channels create / delete / modify";
                        y = "`Channel Create`\n`Channel Delete`\n`Channel Modify`";
                        break;
                    case 5:
                        ConfigLog = "manage_guild";
                        r = "Guild management";
                        y = "`Guild Name Change`\n`Guild Description Change`\n`Guild Icon Change`\n`Guild Splash Change`\n`Guild Banner Change`\n`Guild Bot Add`\n`Guild Bot Remove`";
                        break;
                }





                // NEW BUILD:
                var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);
                var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                if (Herrscher[$"{Guild.Id}"]["moderation"]["mod_logs"][$"{ConfigLog}"] != BsonNull.Value)
                {
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.mod_logs.{ConfigLog}", Channel.Id);
                    await collection.UpdateOneAsync(Herrscher, update);


                    var ch2 = Guild.GetChannel((ulong)Herrscher[$"{Guild.Id}"]["moderation"]["mod_logs"][$"{ConfigLog}"].AsInt64);
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Bip bup bip!\n> Configuration: `{r}`\n> {ch2.Mention} -> {Channel.Mention} ")
                    );
                    await Channel.SendMessageAsync(
                        $"Hey, {ctx.User.Mention} ! Agora esse canal registrará os seguintes eventos:\n>>> {y}"
                    );
                    return;
                }
                // OLD BUILD:
                else
                {
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.mod_logs.{ConfigLog}", Channel.Id);
                    await collection.UpdateOneAsync(Herrscher, update);
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Bip bup bip!\n> Configuration: `{r}`\n> In: {Channel.Mention} ")
                    );
                    await Channel.SendMessageAsync(
                        $"Hey, {ctx.User.Mention} ! Agora esse canal registrará os seguintes eventos:\n>>> {y}"
                    );
                    return;
                }
            }
            catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
                );
                Console.WriteLine($"    ➜  Slash Command: /modding logs add\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                return;
            }
        }





        [SlashCommand("unactivate", "📜 | Desativar uma função de logs.")]
        public static async Task Unactivate(InteractionContext ctx,
            [Option("config", "The configuration type that will be uncativated.")]
                [Choice("Message delete / modify.", 1)]
                [Choice("Moderation actions.", 2)]
                [Choice("Roles add / remove / create / modify / delete.", 3)]
                [Choice("Channels create / delete / modify.", 4)]
                [Choice("Guild management.", 5)]
                long Config
        )
        {
            try
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                var Guild = ctx.Guild;



                // PERMISSIONS:
                await CheckPermi.CheckMemberPermissions(ctx, 5);
                await CheckPermi.CheckBotPermissions(ctx, 5);
                await CheckPermi.CheckMemberPermissions(ctx, 3);
                await CheckPermi.CheckBotPermissions(ctx, 3);




                var ConfigLog = "";
                var r = "";
                // MESSAGES:
                switch (Config)
                {
                    case 1:
                        ConfigLog = "messages_channel";
                        r = "Messages delete / modify";
                        break;
                    case 2:
                        ConfigLog = "moderation_channel";
                        r = "Moderation actions";
                        break;
                    case 3:
                        ConfigLog = "modified_roles";
                        r = "Roles add / remove / create / modify / delete";
                        break;
                    case 4:
                        ConfigLog = "modified_channels";
                        r = "Channels create / delete / modify";
                        break;
                    case 5:
                        ConfigLog = "manage_guild";
                        r = "Guild management";
                        break;
                }



                var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);
                var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                if (Herrscher[$"{Guild.Id}"]["moderation"]["mod_logs"][$"{ConfigLog}"] == BsonNull.Value)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Ops! O Modding Logs `{r}` não está ativado!")
                    );
                    return;
                }
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.moderation.mod_logs.{ConfigLog}", BsonNull.Value);
                await collection.UpdateOneAsync(Herrscher, update);



                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Okay! O Modding Logs `{r}` foi desativado.")
                );
            }
            catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
                );
                Console.WriteLine($"    ➜  Slash Command: /modding logs unactivate\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                return;
            }
        }
    }
}