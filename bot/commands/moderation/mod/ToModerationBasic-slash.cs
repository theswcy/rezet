using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using System;



[SlashCommandGroup("moderation", "To moderation.")]
public class ToModerationBasic_slash : ApplicationCommandModule
{
    [SlashCommand("ban", "💉 | Ban someone.")]
    public static async Task Ban(InteractionContext ctx,
        [Option("member", "Select the member to ban. [ for ID: @rezet ban <id> ]")] DiscordUser User,
        [Option("reason", "The reason of the ban.")] string? Reason = null,
        [Option("messages", "Delete messages of the banned member.")]
            [Choice("yes", "Delete the messages.")]
            [Choice("no", "Don'd delete the messages.")]
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




    [SlashCommand("unban", "💉 | Unban someone")]
    public static async Task Unban(InteractionContext ctx,
        [Option("id", "The ID of the member")] string ID,
        [Option("reason", "The reason of the unban")] string? Reason = null
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




    [SlashCommand("timeout", "💉 | Timeout someone.")]
    public static async Task Timeoout(InteractionContext ctx,
        [Option("member", "Select the member to timeout.")] DiscordUser User,
        [Option("time", "The time of the timeout.")]
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
        [Option("reason", "The reason fo the timeout.")] string? Reason = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(User.Id);

            await CheckPermi.CheckMemberPermissions(ctx, 8);
            await CheckPermi.CheckBotPermissions(ctx, 8);


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




    [SlashCommand("untimeout", "💉 | Untimeout someone.")]
    public static async Task Unmute(InteractionContext ctx,
        [Option("member", "Select the member to untimeout.")] DiscordUser User,
        [Option("reason", "The reason of the unimeout.")] string? Reason = null
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




    [SlashCommand("kick", "💉 | Kick someone.")]
    public static async Task Kick(InteractionContext ctx,
        [Option("member", "Select the member to kick.")] DiscordUser User,
        [Option("reason", "The reason of the kick.")] string? Reason = null
    )
    {
        try
        {
             await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var Guild = ctx.Guild;
            var Member = await Guild.GetMemberAsync(User.Id);

            await CheckPermi.CheckMemberPermissions(ctx, 6);
            await CheckPermi.CheckBotPermissions(ctx, 6);


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




    [SlashCommand("warn", "💉 | Warn someone.")]
    public static async Task Warn(InteractionContext ctx,
        [Option("member", "Select the member to warn.")] DiscordUser User,
        [Option("reason", "The reaosn of the warn.")] string? Reason = null
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




    [SlashCommand("unwarn", "💉 | Unwarn someone.")]
    public static async Task Unwarn(InteractionContext ctx,
        [Option("member", "Select the member to unwarn")] DiscordUser User,
        [Option("warn_id", "The if of the warn.")] string WarnID,
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




    [SlashCommand("warns", "💉 | View the warns of someone.")]
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