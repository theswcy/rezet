using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;




// ANOTAÇÕES:
//   > Apenas as principais permissões dos canais serão checadas.

// RESUMO:
//   > Check de permissões para os membros e para a Rezet:
//        > (  1 ) VIEW CHANNEL.

//        > (  2 ) MANAGE CHANNEL.
//        > (  3 ) MANAGE WEBHOOKS.

//        > (  4 ) CREATE INVITE.

//        > (  5 ) SEND MESSAGES.
//        > (  6 ) EMBED LINKS.
//        > (  7 ) ATTACH FILES.
//        > (  8 ) ADD REACTIONS.
//        > (  9 ) USE EXTERNAL EMOJIS.

//        > ( 10 ) MANAGE MESSAGES.

//        > ( 11 ) USE APPLICATION COMMANDS.



public static class CheckChannelPermissions
{
    // ========== FOR THE MEMBER:
    public static async Task CheckMemberPermissions(InteractionContext ctx, int Permission, DiscordChannel Channel)
    {
        var PermsIn = Channel.PermissionsFor(ctx.Member);
        switch (Permission)
        {
            // ========== CHANNEL GENERAL PERMISSIONS:
            case 1: // VIEW CHANNEL.
                if (!PermsIn.HasPermission(Permissions.AccessChannels))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **ver** o canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 2: // MANAGE CHANNEL:
                if (!PermsIn.HasPermission(Permissions.ManageChannels))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **gerênciar** o canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 3: // MANAGE PERMISSIONS:
                if (!PermsIn.HasPermission(Permissions.ManageWebhooks))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **gerênciar webhooks** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 4: // CREATE INVITES:
                if (!PermsIn.HasPermission(Permissions.CreateInstantInvite))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **criar convites** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 5: // SEND MESSAGES:
                if (!PermsIn.HasPermission(Permissions.SendMessages))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **enviar mensagens** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 6: // SEND EMBED LINKS:
                if (!PermsIn.HasPermission(Permissions.EmbedLinks))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **enviar links com embed** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 7: // ATTACH FILES:
                if (!PermsIn.HasPermission(Permissions.AttachFiles))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **enviar arquivos** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 8: // ADD REACTIONS:
                if (!PermsIn.HasPermission(Permissions.AddReactions))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **adicionar reações** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 9: // USE EXTERNAL EMOJIS:
                if (!PermsIn.HasPermission(Permissions.UseExternalEmojis))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **usar emojis externos** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 10: // MANAGE MESSAGES:
                if (!PermsIn.HasPermission(Permissions.ManageMessages))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **gerênciar mensagens** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 11: // USE APPLICATION COMMANDS:
                if (!PermsIn.HasPermission(Permissions.UseApplicationCommands))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Você não tem permissão para **usar comandos** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
        }
    }
    // ========== FOR REZET:
    public static async Task CheckRezetPermissions(InteractionContext ctx, int Permission, DiscordChannel Channel)
    {
        var f = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);
        var PermsIn = Channel.PermissionsFor(f);
        switch (Permission)
        {
            // ========== CHANNEL GENERAL PERMISSIONS:
            case 1: // VIEW CHANNEL.
                if (!PermsIn.HasPermission(Permissions.AccessChannels))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **ver** o canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 2: // MANAGE CHANNEL:
                if (!PermsIn.HasPermission(Permissions.ManageChannels))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **gerênciar** o canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 3: // MANAGE PERMISSIONS:
                if (!PermsIn.HasPermission(Permissions.ManageWebhooks))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **gerênciar webhooks** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 4: // CREATE INVITES:
                if (!PermsIn.HasPermission(Permissions.CreateInstantInvite))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **criar convites** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 5: // SEND MESSAGES:
                if (!PermsIn.HasPermission(Permissions.SendMessages))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **enviar mensagens** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 6: // SEND EMBED LINKS:
                if (!PermsIn.HasPermission(Permissions.EmbedLinks))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **enviar links com embed** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 7: // ATTACH FILES:
                if (!PermsIn.HasPermission(Permissions.AttachFiles))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **enviar arquivos** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 8: // ADD REACTIONS:
                if (!PermsIn.HasPermission(Permissions.AddReactions))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **adicionar reações** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 9: // USE EXTERNAL EMOJIS:
                if (!PermsIn.HasPermission(Permissions.UseExternalEmojis))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **usar emojis externos** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 10: // MANAGE MESSAGES:
                if (!PermsIn.HasPermission(Permissions.ManageMessages))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **gerênciar mensagens** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 11: // USE APPLICATION COMMANDS:
                if (!PermsIn.HasPermission(Permissions.UseApplicationCommands))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Eu não tenho permissão para **usar comandos** no canal {Channel.Mention}!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
        }
    }
}