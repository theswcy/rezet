using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;




// ANOTAÇÕES:
//   > Apenas as principais permissões vão ser checadas.

// RESUMO:
//   > Check de permissões para os membros e para a Rezet:
//        > (  1 ) ADMIN.
//        > (  2 ) MANAGE GUILD.
//        > (  3 ) MANAGE CHANNELS.
//        > (  4 ) MANAGE ROLES.
//        > (  5 ) VIEL AUDIT LOGS.
//        > (  6 ) KICK MEMBERS.
//        > (  7 ) BAN MEMBERS.
//        > (  8 ) TIMEOUT MEMBERS.
//        > (  9 ) MANAGE MESSAGES.
//        > ( 10 ) POSITION:



#pragma warning disable CS8602
public static class CheckPermi_Prefix
{
    // ========== FOR THE MEMBER:
    public static async Task CheckMemberPermissions(CommandContext ctx, int permission, DiscordMember? Member = null)
    {
        switch (permission)
        {
            // ========== GUILD GENERAL PERMISSIONS:
            case 1: // ADMINISTRATOR:
                if (!ctx.Member.Permissions.HasPermission(Permissions.Administrator))
                {
                    await ctx.RespondAsync("Você não tem a permissão `ADMINISTRATOR` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 2: // MANAGE GUILD:
                if (!ctx.Member.Permissions.HasPermission(Permissions.ManageGuild))
                {
                    await ctx.RespondAsync("Você não tem a permissão `MANAGE GUILD` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 3: // MANAGE CHANNELS:
                if (!ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
                {
                    await ctx.RespondAsync("Você não tem a permissão `MANAGE CHANNELS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 4: // MANAGE ROLES:
                if (!ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
                {
                    await ctx.RespondAsync("Você não tem a permissão `MANAGE ROLES` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 5: // VIEL AUDIT LOGS:
                if (!ctx.Member.Permissions.HasPermission(Permissions.ViewAuditLog))
                {
                    await ctx.RespondAsync("Você não tem a permissão `VIEW AUDIT LOGS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;





            // ========== MODERATIONS PERMISSIONS:
            case 6: // KICK MEMBERS:
                if (!ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
                {
                    await ctx.RespondAsync("Você não tem a permissão `KICK MEMBERS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 7: // BAN MEMBERS:
                if (!ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
                {
                    await ctx.RespondAsync("Você não tem a permissão `BAN MEMBERS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 8: // TIMEOUT MEMBERS:
                if (!ctx.Member.Permissions.HasPermission(Permissions.MuteMembers))
                {
                    await ctx.RespondAsync("Você não tem a permissão `TIMEOUT MEMBERS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 9: // MANAGE MESSAGES:
                if (!ctx.Member.Permissions.HasPermission(Permissions.ManageMessages))
                {
                    await ctx.RespondAsync("Você não tem a permissão `MANAGE MESSAGES` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 10: // POSITION:
#pragma warning disable CS8602
                if (ctx.Member.Hierarchy <= Member.Hierarchy)
                {
                    await ctx.RespondAsync("Eu não posso punir um usuário com a posição maior que a sua!");
                    throw new UnauthorizedAccessException();
                }
                break;
        }
    }




    // ========== FOR THE REZET:
    public static async Task CheckBotPermissions(CommandContext ctx, int permission, DiscordMember? Member = null)
    {
        switch (permission)
        {
            // ========== GUILD GENERAL PERMISSIONS:
            case 1: // ADMINISTRATOR:
                if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.Administrator))
                {
                    await ctx.RespondAsync("eu não possuo a permissão `ADMINISTRATOR` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 2: // MANAGE GUILD:
                if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.ManageGuild))
                {
                    await ctx.RespondAsync("eu não possuo a permissão `MANAGE GUILD` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 3: // MANAGE CHANNELS:
                if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.ManageChannels))
                {
                    await ctx.RespondAsync("eu não possuo a permissão `MANAGE CHANNELS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 4: // MANAGE ROLES:
                if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.ManageRoles))
                {
                    await ctx.RespondAsync("eu não possuo a permissão `MANAGE ROLES` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 5: // VIEL AUDIT LOGS:
                if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.ViewAuditLog))
                {
                    await ctx.RespondAsync("eu não possuo a permissão `VIEW AUDIT LOGS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;





            // ========== MODERATIONS PERMISSIONS:
            case 6: // KICK MEMBERS:
                if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.KickMembers))
                {
                    await ctx.RespondAsync("eu não possuo a permissão `KICK MEMBERS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 7: // BAN MEMBERS:
                if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.BanMembers))
                {
                    await ctx.RespondAsync("eu não possuo a permissão `BAN MEMBERS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 8: // TIMEOUT MEMBERS:
                if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.MuteMembers))
                {
                    await ctx.RespondAsync("eu não possuo a permissão `TIMEOUT MEMBERS` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 9: // MANAGE MESSAGES:
                if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.ManageMessages))
                {
                    await ctx.RespondAsync("eu não possuo a permissão `MANAGE MESSAGES` para usar esse comando!");
                    throw new UnauthorizedAccessException();
                }
                break;
            case 10: // POSITION:
#pragma warning disable CS8602
                if (ctx.Member.Hierarchy <= Member.Hierarchy)
                {
                    await ctx.RespondAsync("Eu não posso punir um usuário com a posição maior que a minha!");
                    throw new UnauthorizedAccessException();
                }
                break;
        }
    }
}
