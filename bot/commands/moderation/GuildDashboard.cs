using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;



[SlashCommandGroup("moderator", "To moderators")]
public class Moderators : ApplicationCommandModule
{
    [SlashCommand("dashboard", "🧰 | Moderation dashboard.")]
    public static async Task Dashboard(InteractionContext ctx)
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