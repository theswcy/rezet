using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.VisualBasic;
using RezetSharp;



public class ToModerationBasic_prefix : BaseCommandModule
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
    private static readonly List<string> Keys =
    [
        "minuto", "minutos", "minute", "minutes",
        "hora", "horas", "hour", "hours",
        "dia", "dias", "days", "day",
    ];





    [Command("ban")]
    [Aliases("vasco")]
    public async Task Ban(CommandContext ctx,
        string Member,
        string? Reason = null,
        string? Delete = null
    )
    {
        try
        {
            await ctx.TriggerTypingAsync();
            await CheckPermi_Prefix.CheckMemberPermissions(ctx, 7);

            await CheckPermi_Prefix.CheckBotPermissions(ctx, 7);



            var Guild = ctx.Guild;
            if (Member.StartsWith("<@&") || Member.StartsWith("<#"))
            {
                await ctx.RespondAsync("Eu só posso banir usuários!");
                return;
            }
            if (Member.StartsWith("<@") && Member.EndsWith('>'))
            {
                var mention = Member.Trim('<', '@', '1', '>');
                if (ulong.TryParse(mention, out var userId))
                {
                    var UserToBan = await Guild.GetMemberAsync(userId);
                    await CheckPermi_Prefix.CheckMemberPermissions(ctx, 10, UserToBan);
                    await CheckPermi_Prefix.CheckBotPermissions(ctx, 10, UserToBan);
                    if (UserToBan.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
                    {
                        await ctx.RespondAsync("Calma ai! Eu não posso me banir!");
                        return;
                    }
                    await UserToBan.BanAsync(
                        Delete != null ? 7 : 0,
                        Reason
                    );
                    await ctx.RespondAsync($"Bang! O usuário {UserToBan.Mention} [ `{UserToBan.Id}` ] foi jogar no Vasco!");
                }
            }
            else if (ulong.TryParse(Member, out var userId))
            {
                var UserToBan = await Guild.GetMemberAsync(userId);
                await CheckPermi_Prefix.CheckMemberPermissions(ctx, 10, UserToBan);
                await CheckPermi_Prefix.CheckBotPermissions(ctx, 10, UserToBan);
                if (UserToBan.Id == EngineV8X.RezetRazor?.CurrentUser.Id)
                {
                    await ctx.RespondAsync("Calma ai! Eu não posso me banir!");
                    return;
                }
                await UserToBan.BanAsync(
                    Delete != null ? 7 : 0,
                    Reason
                );
                await ctx.RespondAsync($"Bang! O usuário {UserToBan.Mention} [ `{UserToBan.Id}` ] foi jogar no Vasco!");
            }
        }
        catch (Exception ex)
        {
            await ctx.RespondAsync($"Falha ao executar o comando, verifique minhas permissões!");
            Console.WriteLine($"    ➜  Prefix Command: ban\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }




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