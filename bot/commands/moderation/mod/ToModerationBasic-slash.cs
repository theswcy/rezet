using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Rezet;
using MongoDB.Bson;
using RezetSharp;



[SlashCommandGroup("moderation", "To moderation.")]
public class ToModerationBasic_slash : ApplicationCommandModule
{
    [SlashCommand("ban", "💉 | BBanir um membro.")]
    public static async Task Ban(InteractionContext ctx,
        [Option("member", "Selecione o membro. [ for ID: @rezet ban <id> ]")] DiscordUser User,
        [Option("reason", "Motivo do banimento.")] string? Reason = null,
        [Option("messages", "Deletar mensagens do membro banido?")]
            [Choice("yes", "Deletar as mensagens.")]
            string? Delete = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(User.Id);


            await CheckPermi.CheckMemberPermissions(ctx, 7);
            await CheckPermi.CheckBotPermissions(ctx, 7);
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
            Console.WriteLine(ex);
        }
    }




    [SlashCommand("unban", "💉 | Remover o banimento de um usuário.")]
    public static async Task Unban(InteractionContext ctx,
        [Option("id", "ID do membro.")] string ID,
        [Option("reason", "Motivo da remoção do banimento")] string? Reason = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(ulong.Parse(ID));

            await CheckPermi.CheckMemberPermissions(ctx, 7);
            await CheckPermi.CheckBotPermissions(ctx, 7);

            await Member.UnbanAsync(Reason);
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Bip bup bip! O usuário {Member.Mention} [ `{Member.Id}` ] saiu do Vasco!")
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
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
        [Option("reason", "Motivo do mute.")] string? Reason = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(User.Id);


            await CheckPermi.CheckMemberPermissions(ctx, 8);
            await CheckPermi.CheckBotPermissions(ctx, 8);
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
            Console.WriteLine(ex);
        }
    }




    [SlashCommand("untimeout", "💉 | Remover o mute de um membro.")]
    public static async Task Unmute(InteractionContext ctx,
        [Option("member", "Selecione o membro.")] DiscordUser User,
        [Option("reason", "Motivo da remoção do mute.")] string? Reason = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

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
            Console.WriteLine(ex);
        }
    }




    [SlashCommand("kick", "💉 | Expulsar.")]
    public static async Task Kick(InteractionContext ctx,
        [Option("member", "Selecione o membro.")] DiscordUser User,
        [Option("reason", "Motivo da expulsão.")] string? Reason = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(User.Id);


            await CheckPermi.CheckMemberPermissions(ctx, 6);
            await CheckPermi.CheckBotPermissions(ctx, 6);
            if (Member.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Calma ai! Eu não posso me chutar!")
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
            Console.WriteLine(ex);
        }
    }




    [SlashCommand("warn", "💉 | Aplicar uma advertência a um membro.")]
    public static async Task Warn(InteractionContext ctx,
        [Option("member", "Select the member to warn.")] DiscordUser User,
        [Option("reason", "The reaosn of the warn.")] string? Reason = null
    )
    {
        try
        {
            await CheckPermi.CheckMemberPermissions(ctx, 8);
            await CheckPermi.CheckMemberPermissions(ctx, 8);
            if (User.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Calma ai! Eu não posso me dar warn!")
                );
                return;
            }
            var Guild = ctx.Guild;
            var Herrscher = EngineV8X.HerrscherRazor?.GetHerrscherDocument(Guild);



#pragma warning disable CS8602
            if (Herrscher[$"{Guild.Id}"]["moderation"]["warns"]["type_u"].AsBsonDocument.Contains($"{User.Id}"))
            {
                var h = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");

            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }




    [SlashCommand("unwarn", "💉 | Remover advertência de um membro.")]
    public static async Task Unwarn(InteractionContext ctx,
        [Option("warn_id", "The id of the warn.")] string WarnID,
        [Option("reason", "The reaosn of the unwarn.")] string? Reason = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }




    [SlashCommand("warn-modify", "💉 | Editar advertência de um membro.")]
    public static async Task WarnModify(InteractionContext ctx,
        [Option("warn", "Select the to modify.")] string WarnID,
        [Option("reason", "The warn reason.")] string Reason
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }




    [SlashCommand("warns", "💉 | Ver as advertências de um usuário.")]
    public static async Task Unwarn(InteractionContext ctx,
        [Option("member", "Select the member to view warns.")] DiscordUser User
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}