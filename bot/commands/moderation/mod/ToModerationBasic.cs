using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;



public class ToModerationBasic : BaseCommandModule
{
    private static readonly List<string> Keys =
    [
        "minuto", "minutos", "minute", "minutes",
        "hora", "horas", "hour", "hours",
        "dia", "dias", "days", "day",
    ];




    [Command("mute")]
    [Aliases("timeout")]
    public async Task Mute(CommandContext ctx, DiscordMember Member, int time, string opt, [RemainingText] string? reason = null)
    {
        try
        {
            opt = opt.TrimEnd('.', '!', '?');
            TimeSpan timeoutDuration = TimeSpan.Zero;
            if (Keys.Contains(opt))
            {
                if (opt.StartsWith('m'))
                {
                    timeoutDuration = TimeSpan.FromMinutes(time);
                    opt = "minutos";
                }
                else if (opt.StartsWith('h'))
                {
                    timeoutDuration = TimeSpan.FromHours(time);
                    opt = "horas";
                }
                else if (opt.StartsWith('d'))
                {
                    timeoutDuration = TimeSpan.FromDays(time);
                    opt = "dias";
                }

            }
            if (timeoutDuration.TotalDays > 7)
            {
                await ctx.RespondAsync("No máximo 7 dias!");
                return;
            }



            var endTime = DateTime.UtcNow.Add(timeoutDuration);
            await Member.TimeoutAsync(endTime, reason);
            if (reason != null) { reason = $"\n> Motivo: `{reason}`"; }
            await ctx.RespondAsync(
                $"O membro {Member.Mention} foi silenciado!\n> Duração: `{timeoutDuration}`{reason}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}