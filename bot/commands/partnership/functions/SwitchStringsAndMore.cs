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
                        .Replace("@[partner.name]", "Example Community")
                        .Replace("@[rep.name]", "Rep Name")
                        .Replace("@[rep.id]", "1234567890.")
                        .Replace("@[rep.mention]", "@rep.");
        return txt2;
    }
    public static string SwitchVariables2(string txt, MessageCreateEventArgs ctx, DiscordUser rep, int? points = null, int? ranking = null, string? pguild = null)
    {
        var txt2 = txt.Replace("@[staff.mention]", ctx.Author.Mention)
                        .Replace("@[staff.name]", ctx.Author.Username)
                        .Replace("@[staff.id]", ctx.Author.Id.ToString())
                        .Replace("@[staff.points]", $"{points ?? 10}")
                        .Replace("@[staff.rank]", $"{ranking ?? 99}º")

                        .Replace("@[guild.name]", ctx.Guild.Name)
                        .Replace("@[guild.owner]", ctx.Guild.Owner.DisplayName)
                        .Replace("@[guild.id]", ctx.Guild.Id.ToString())

                        .Replace("@[partner.name]", $"{pguild ?? "Example Community"}")
                        .Replace("@[rep.name]", $"{rep.Username}")
                        .Replace("@[rep.id]", $"{rep.Id}")
                        .Replace("@[rep.mention]", $"{rep.Mention}");
        return txt2;
    }
}