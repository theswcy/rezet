using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;



[SlashCommandGroup("moderation", "To moderation.")]
public class ToModerationBasic_slash : ApplicationCommandModule
{
    [SlashCommand("ban", "💉 | Ban someone.")]
    public static async Task Ban(InteractionContext ctx,
        [Option("member", "Select the member to ban. [ for ID: @rezet ban <id> ]")] DiscordUser User,
        [Option("reason", "The reason of the ban.")] string? Reason = null
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




    [SlashCommand("unban", "💉 | Unban someone")]
    public static async Task Unban(InteractionContext ctx,
        [Option("id", "The ID of the member")] string ID,
        [Option("reason", "The reason of the unban")] string? Reason = null
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




    [SlashCommand("timeout", "💉 | Timeout someone.")]
    public static async Task Timeoout(InteractionContext ctx,
        [Option("member", "Select the member to timeout.")] DiscordUser User,
        [Option("time", "The time of the timeout.")] long Time,
        [Option("reason", "The reason fo the timeout.")] string? Reason = null
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




    [SlashCommand("untimeout", "💉 | Untimeout someone.")]
    public static async Task Unmute(InteractionContext ctx,
        [Option("member", "Select the member to untimeout.")] DiscordUser User,
        [Option("reason", "The reason of the unimeout.")] string? Reason = null
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




    [SlashCommand("kick", "💉 | Kick someone.")]
    public static async Task Kick(InteractionContext ctx,
        [Option("member", "Select the member to kick.")] DiscordUser User,
        [Option("reason", "The reason of the kick.")] string? Reason = null
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