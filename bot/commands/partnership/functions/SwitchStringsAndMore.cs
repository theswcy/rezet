using DSharpPlus.Entities;
using DSharpPlus.EventArgs;



public static class SwitchOrResponse
{
    public static string SwitchVariables(string txt, DiscordInteraction ctx)
    {
        var txt2 = txt.Replace("@[staff.mention]", ctx.User.Mention)
                        .Replace("@[staff.name]", ctx.User.Username)
                        .Replace("@[staff.id]", ctx.User.Id.ToString())
                        .Replace("@[staff.points]", "10")
                        // ========== NEW:
                        .Replace("@[staff.rank]", "1º")

                        .Replace("@[guild.name]", ctx.Guild.Name)
                        .Replace("@[guild.owner]", ctx.Guild.Owner.DisplayName)
                        .Replace("@[guild.id]", ctx.Guild.Id.ToString())

                        // ========== NEW:
                        .Replace("@[partner.name]", "Name of the partner guild.")
                        .Replace("@[rep.name]", "Name of the partner rep.")
                        .Replace("@[rep.id]", "Id of the rep.")
                        .Replace("@[rep.mention]", "Mention of the rep.");
        return txt2;
    }
    public static string SwitchVariables2(string txt, MessageCreateEventArgs ctx, int? points = null)
    {
        var txt2 = txt.Replace("@[staff.mention]", ctx.Author.Mention)
                        .Replace("@[staff.name]", ctx.Author.Username)
                        .Replace("@[staff.id]", ctx.Author.Id.ToString())
                        .Replace("@[staff.points]", $"{points ?? 10}")

                        .Replace("@[guild.name]", ctx.Guild.Name)
                        .Replace("@[guild.owner]", ctx.Guild.Owner.DisplayName)
                        .Replace("@[guild.id]", ctx.Guild.Id.ToString());
        return txt2;
    }
}