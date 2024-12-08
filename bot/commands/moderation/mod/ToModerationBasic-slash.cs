using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;
using RezetSharp;
using MongoDB.Driver;



public class GenKey
{
    public static string Key()
    {
        // 4 NUMBERS:
        string Numbers = new string(
            Enumerable.Range(0, 4)
            .Select(_ => (char)('0' + new Random().Next(0, 10)))
            .ToArray()
        );

        // 4 LETTERS:
        const string LE = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string Letters = new string(
            Enumerable.Range(0, 4)
            .Select(_ => LE[new Random().Next(LE.Length)])
            .ToArray()
        );

        return $"{Numbers}-{Letters}";
    }
}




[SlashCommandGroup("mod", "To moderation.")]
public class ToModerationBasic_slash : ApplicationCommandModule
{
    [SlashCommand("ban", "💉 | Banir um membro.")]
    public static async Task Ban(InteractionContext ctx,
        [Option("member", "Selecione o membro. [ for ID: @rezet ban <id> ]")] DiscordUser User,
        [Option("reason", "Motivo do banimento.")] string Reason,
        [Option("messages", "Deletar mensagens do membro banido?")]
            [Choice("yes", "Deletar as mensagens.")]
            string? Delete = null
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(User.Id);


            await CheckPermi.CheckMemberPermissions(ctx, 7);
            await CheckPermi.CheckMemberPermissions(ctx, 10, Member);
            await CheckPermi.CheckBotPermissions(ctx, 7);
            await CheckPermi.CheckBotPermissions(ctx, 10, Member);
            if (Member.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Calma ai! Eu não posso me banir!")
                );
                return;
            }


            await Member.BanAsync(
                Delete != null ? 7 : 0,
                Reason
            );
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Bang! O usuário {Member.Mention} [ `{Member.Id}` ] foi jogar no Vasco!")
            );
        }
        catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
                );
                Console.WriteLine($"    ➜  Slash Command: /mod ban\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                return;
            }
    }




#pragma warning disable CS8602
    [SlashCommand("unban", "💉 | Remover o banimento de um usuário.")]
    public static async Task Unban(InteractionContext ctx,
        [Option("id", "ID do membro.")] string ID,
        [Option("reason", "Motivo da remoção do banimento")] string Reason
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;


            try { await EngineV8X.RezetRazor.GetUserAsync(ulong.Parse(ID)); }
            catch (Exception)
            {
                await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent("O ID fornecido é inválido!")
                );
                return;
            }
            var Member = await EngineV8X.RezetRazor.GetUserAsync(ulong.Parse(ID));


            await CheckPermi.CheckMemberPermissions(ctx, 7);
            await CheckPermi.CheckBotPermissions(ctx, 7);
            if (Member.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Calma ai! Eu não posso me desbanir!")
                );
                return;
            }


            await ctx.Guild.UnbanMemberAsync(Member, Reason);
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Bip bup bip! O usuário {Member.Mention} [ `{Member.Id}` ] saiu do Vasco!")
            );
        }
        catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
                );
                Console.WriteLine($"    ➜  Slash Command: /mod unban\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                return;
            }
    }




    [SlashCommand("timeout", "💉 | Mutar um membro.")]
    public static async Task Timeoout(InteractionContext ctx,
        [Option("member", "Selecione o membro.")] DiscordUser User,
        [Option("time", "Tempo do mute.")]
            [Choice("10 minutes", 10)]
            [Choice("30 minutes", 30)]
            [Choice("1 hours", 60)]
            [Choice("2 hours", 120)]
            [Choice("6 hours", 360)]
            [Choice("12 hours", 720)]
            [Choice("1 day", 1440)]
            [Choice("2 days", 1440)]
            [Choice("7 days", 1440)]
            long Time,
        [Option("reason", "Motivo do mute.")] string Reason
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(User.Id);


            await CheckPermi.CheckMemberPermissions(ctx, 8);
            await CheckPermi.CheckMemberPermissions(ctx, 10, Member);
            await CheckPermi.CheckBotPermissions(ctx, 8);
            await CheckPermi.CheckBotPermissions(ctx, 10, Member);
            if (Member.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Calma ai! Eu não posso me mutar!")
                );
                return;
            }


            TimeSpan timeoutDuration = TimeSpan.Zero;
            timeoutDuration = TimeSpan.FromMinutes(Time);
            var endTime = DateTime.UtcNow.Add(timeoutDuration);
            DateTimeOffset muteEndTime = DateTimeOffset.UtcNow.Add(timeoutDuration);
            long unixTimestamp = muteEndTime.ToUnixTimeSeconds();


            await Member.TimeoutAsync(endTime, Reason);
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Bang! O usuário {Member.Mention} [ `{Member.Id}` ] foi mutado! Duração: **<t:{unixTimestamp}:R>**.")
            );
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /mod timeout\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




    [SlashCommand("untimeout", "💉 | Remover o mute de um membro.")]
    public static async Task Unmute(InteractionContext ctx,
        [Option("member", "Selecione o membro.")] DiscordUser User,
        [Option("reason", "Motivo da remoção do mute.")] string Reason
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(User.Id);


            await CheckPermi.CheckMemberPermissions(ctx, 8);
            await CheckPermi.CheckBotPermissions(ctx, 8);


            await Member.TimeoutAsync(null, Reason);
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Bip bup bip! O usuário {Member.Mention} [ `{Member.Id}` ] foi desmutado!")
            );
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /mod untimeout\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




    [SlashCommand("kick", "💉 | Expulsar.")]
    public static async Task Kick(InteractionContext ctx,
        [Option("member", "Selecione o membro.")] DiscordUser User,
        [Option("reason", "Motivo da expulsão.")] string Reason
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(User.Id);


            await CheckPermi.CheckMemberPermissions(ctx, 6);
            await CheckPermi.CheckMemberPermissions(ctx, 10, Member);
            await CheckPermi.CheckBotPermissions(ctx, 6);
            await CheckPermi.CheckBotPermissions(ctx, 10, Member);
            if (Member.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Calma ai! Eu não posso me chutar, bocó!")
                );
                return;
            }


            await Member.RemoveAsync(Reason);
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Bang! O usuário {Member.Mention} [ `{Member.Id}` ] foi chutado!")
            );
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /mod kick\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




    [SlashCommand("warn", "💉 | Aplicar uma advertência a um membro.")]
    public static async Task Warn(InteractionContext ctx,
        [Option("member", "Selecione o membro para dar uma advertência.")] DiscordUser User,
        [Option("reason", "TMotivo da advertência.")] string Reason
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;
            var Herrscher = EngineV8X.HerrscherRazor?.GetHerrscherDocument(Guild);
            var Member = await Guild.GetMemberAsync(User.Id);


            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckMemberPermissions(ctx, 10, Member);
            await CheckPermi.CheckBotPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 10, Member);
            if (User.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Calma ai! Eu não posso me dar warn!")
                );
                return;
            }
            if (User.Id == ctx.User.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Patrão, você não pode aplicar uma advertência em si mesmo!")
                );
                return;
            }
            if (Reason.Length > 200)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Brother, o motivo pode ter no máximo **200 caracteres**.")
                );
            }


            string Key = GenKey.Key();
            long TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            int cases = (int)Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["count"] + 1;
            var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");


            if (Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"] != BsonNull.Value)
            {
                var update = Builders<BsonDocument>.Update
                .Inc($"{Guild.Id}.moderation.warns.count", 1)
                .Set($"{Guild.Id}.moderation.warns.cases.{Key}",
                    new BsonDocument {
                        { "user_id", (long)Member.Id },
                        { "reason", Reason },
                        { "case", cases },
                        { "author_id", (long)ctx.User.Id },
                        { "date", TimeStamp }
                    }
                );
                await collection.UpdateOneAsync(Herrscher, update);
            }
            else
            {
                var w = new BsonDocument
                {
                    {
                        Key, new BsonDocument
                        {
                            { "user_id", (long)Member.Id },
                            { "reason", Reason },
                            { "case", cases },
                            { "author_id", (long)ctx.User.Id },
                            { "date", TimeStamp }
                        }
                    }
                };
                var update = Builders<BsonDocument>.Update
                .Inc($"{Guild.Id}.moderation.warns.count", 1)
                .Set($"{Guild.Id}.moderation.warns.cases", w);
                await collection.UpdateOneAsync(Herrscher, update);
            }


            var embed = new DiscordEmbedBuilder()
            {
                Description = $"## Case: `#{cases}`!",
                Color = new DiscordColor("#ff3a3a")
            };
            embed.WithFooter("Powered by: Rezet Sharp!", EngineV8X.RezetRazor.CurrentUser.AvatarUrl);
            if (Member.AvatarUrl != null) { embed.WithThumbnail(Member.AvatarUrl); }
            embed.AddField(
                "<:rezet_creditcard:1147341888538542132> Informações:",
                $"> **User**: {Member.Mention} - [ `{Member.Id}` ]" +
                $"\n> **Warn by**: {ctx.User.Mention} - [ `{ctx.User.Id}` ]" +
                $"\n> **ID**: `{Key}`" +
                $"\n> **Date**: <t:{TimeStamp}>"
            );
            embed.AddField(
                "<:rezet_channels:1308125117875752961> Motivo:",
                $"> {Reason}"
            );


            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent("Bip bip... Bang!")
                    .AddEmbed(embed)
            );
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /mod warn\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




    [SlashCommand("warn-remove", "💉 | Remover advertência de um membro.")]
    public static async Task Unwarn(InteractionContext ctx,
        [Option("warn_id", "ID da advertência.")] string WarnID,
        [Option("reason", "TMotivo da remoção da advertência.")] string Reason
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;
            var Herrscher = EngineV8X.HerrscherRazor?.GetHerrscherDocument(Guild);


            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);


            if (Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"] == BsonNull.Value)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Não há nenhuma advertência guardada em meu banco de dados!")
                );
                return;
            }
            if (!Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"].AsBsonDocument.Contains(WarnID))
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"ops! A advertência `{WarnID}` não existe no meu banco de dados!")
                );
                return;
            }
            if (Reason.Length > 200)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Brother, o motivo pode ter no máximo **200 caracteres**.")
                );
            }


            var CaseInformations = Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"][WarnID];
            if ((ulong)CaseInformations["user_id"].AsInt64 == ctx.User.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Alô? Você não pode remover uma advertência de você mesmo, isso é feio (jã não basta tua cara feiosa?)!")
                );
                return;
            }
            var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update
                .Unset($"{Guild.Id}.moderation.warns.cases.{WarnID}");
            await collection.UpdateOneAsync(Herrscher, update);


            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"A advertência `{WarnID}` do caso `#{CaseInformations["case"]}` pertencente a <@{CaseInformations["user_id"]}> foi removida!\n\n> Motivo da remoção: {Reason}")
            );
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /mod warn-remove\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




    [SlashCommand("warn-modify", "💉 | Editar advertência de um membro.")]
    public static async Task WarnModify(InteractionContext ctx,
        [Option("warn_id", "ID da advertência.")] string WarnID,
        [Option("reason", "Novo motivo da advertência.")] string Reason
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;
            var Herrscher = EngineV8X.HerrscherRazor?.GetHerrscherDocument(Guild);


            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);


            if (Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"] == BsonNull.Value)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Não há nenhuma advertência guardada em meu banco de dados!")
                );
                return;
            }
            if (!Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"].AsBsonDocument.Contains(WarnID))
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"ops! A advertência `{WarnID}` não existe no meu banco de dados!")
                );
                return;
            }
            if (Reason.Length > 200)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Brother, o motivo pode ter no máximo **200 caracteres**.")
                );
            }


            var CaseInformations = Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"][WarnID];
            if ((ulong)CaseInformations["user_id"].AsInt64 == ctx.User.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Alô? Você não pode modificar uma advertência sua, isso é trapaça!")
                );
                return;
            }
            var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update
                .Set($"{Guild.Id}.moderation.warns.cases.{WarnID}.reason", $"{Reason}\n-# (Modificado)");
            await collection.UpdateOneAsync(Herrscher, update);


            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"A advertência `{WarnID}` do caso `#{CaseInformations["case"]}` pertencente a <@{CaseInformations["user_id"]}> foi modificada!\n> Novo motivo:\n> `{Reason}`")
            );
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /mod warn-modify\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




    [SlashCommand("warns", "💉 | Ver as advertências de um usuário.")]
    public static async Task Warns(InteractionContext ctx,
        [Option("member", "Selecione o membro ver as advertências.")] DiscordUser User
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;
            var Herrscher = EngineV8X.HerrscherRazor?.GetHerrscherDocument(Guild);
            var Member = await Guild.GetMemberAsync(User.Id);


            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);
            if (User.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Calma ai! Eu não sou delinquente, eu não tenho warns em meu nome!!!!! :(")
                );
                return;
            }
            if (Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"] == BsonNull.Value)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Não há nenhuma advertência guardada em meu banco de dados!")
                );
                return;
            }


            var embed = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor("#7e67ff")
            };
            embed.WithFooter("Powered by: Rezet Sharp!", EngineV8X.RezetRazor.CurrentUser.AvatarUrl);
            if (User.AvatarUrl != null) { embed.WithThumbnail(User.AvatarUrl); }
            var desc = $"## <:rezet_plant:1308125160577962004> Warns!\n- Esses aqui são todos os casos pertencente a {User.Mention}.\n\n";
            var WarnsDict = Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"]
                .AsBsonDocument.ToDictionary(elem => elem.Name);
            int u = 0;
            foreach (var Entry in WarnsDict)
            {
                var EntryKey = Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"][Entry.Key].AsBsonDocument;
                if (EntryKey["user_id"] == (long)User.Id)
                {
                    desc += $"> **Case** `#{EntryKey["case"]}` / `{Entry.Key}`\n> <t:{EntryKey["date"]}>\n\n";
                    u++;
                }
            }


            if (u > 0)
            {
                embed.WithDescription(desc);
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Bip bup bip!")
                        .AddEmbed(embed)
                );
                return;
            }
            else
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Brother, o usuário {User.Mention} ainda não recebeu nenhuma advertência!")
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
            Console.WriteLine($"    ➜  Slash Command: /mod warns\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




    [SlashCommand("warn-view", "💉 | Visualizar uma advertência específica.")]
    public static async Task WarnView(InteractionContext ctx,
        [Option("warn_id", "ID da advertência.")] string WarnID
    )
    {
        try
        {
            await ctx.DeferAsync();


            var Guild = ctx.Guild;
            var Herrscher = EngineV8X.HerrscherRazor?.GetHerrscherDocument(Guild);


            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);


            if (Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"] == BsonNull.Value)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Não há nenhuma advertência guardada em meu banco de dados!")
                );
            }
            if (!Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"].AsBsonDocument.Contains(WarnID))
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"ops! A advertência `{WarnID}` não existe no meu banco de dados!")
                );
                return;
            }


            var CaseInformations = Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["cases"][WarnID];
            var User = await EngineV8X.RezetRazor.GetUserAsync((ulong)CaseInformations["user_id"].AsInt64);
            var Author = await EngineV8X.RezetRazor.GetUserAsync((ulong)CaseInformations["author_id"].AsInt64);
            var embed = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor("#7e67ff"),
                Description = $"## <:rezet_plant:1308125160577962004> Case: `#{CaseInformations["case"]}`!"
            };
            embed.WithFooter("Powered by: Rezet Sharp!", EngineV8X.RezetRazor.CurrentUser.AvatarUrl);
            if (User.AvatarUrl != null) { embed.WithThumbnail(User.AvatarUrl); }
            embed.AddField(
                "<:rezet_creditcard:1147341888538542132> Informações:",
                $"> **Case**: `#{CaseInformations["case"]}`" +
                $"\n> **User**: {User.Mention} - [ `{User.Id}` ]" +
                $"\n> **Warn by**: {Author.Mention} - [ `{Author.Id}` ]" +
                $"\n> **Date**: <t:{CaseInformations["date"]}>"
            );
            embed.AddField(
                "<:rezet_channels:1308125117875752961> Motivo:",
                $"> {CaseInformations["reason"]}"
            );


            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent("Bip bup bip!")
                    .AddEmbed(embed)
            );

        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
            );
            Console.WriteLine($"    ➜  Slash Command: /mod warn-view\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }
}