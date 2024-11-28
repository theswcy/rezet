using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;



#pragma warning disable CS8602
[SlashCommandGroup("role", "Role Settings")]
public class CommunityRole : ApplicationCommandModule
{
    [SlashCommand("add", "📗 | Adicionar um cargo a um usuário.")]
    public static async Task Add(InteractionContext ctx,
        [Option("role", "Selecione o cargo que será adicionado.")] DiscordRole role,
        [Option("member", "Selecione o membro que receberá o cargo")] DiscordUser member,
        [Option("reason", "Motivo do membro ter recebido o cargo.")] string? Reason = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Author = ctx.Member;
            DiscordMember Member = await ctx.Guild.GetMemberAsync(member.Id);



            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);
            await CheckRoleType.CheckType(ctx, 1, role);
            await CheckRoleType.CheckType(ctx, 2, role);
            await CheckRoleType.CheckType(ctx, 3, role);




            var botMember = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);
            var highestBotRole = botMember.Roles.OrderByDescending(r => r.Position).FirstOrDefault();
            if (role.Position >= highestBotRole.Position)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Eu não posso atribuir um cargo maior que o meu maior cargo atual!"));
                return;
            }
            var highestMemberRole = ctx.Member.Roles.OrderByDescending(r => r.Position).FirstOrDefault();
            if (role.Position >= highestMemberRole.Position)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Você não pode atribuir um cargo maior que o seu maior cargo atual!"));
                return;
            }



            if (member.Id == Author.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Você não pode atribuir um cargo a si mesmo!"));
                return;
            }
            if (Member.Roles.Any(r => r.Id == role.Id))
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"O membro seleconado ja possui este cargo!"));
                return;
            }




            try
            {
                await Member.GrantRoleAsync(role, Reason);

                var embed = new DiscordEmbedBuilder
                {
                    Description = $"O cargo **[ {role.Name} ]** foi adiconado com sucesso ao usuário **[ {member.Mention} ]**!",
                    Color = new DiscordColor("#60ed7c")
                };

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Bip bup bip!")
                        .AddEmbed(embed));
                return;
            }
            catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando.\n\n> `{ex.Message}`"));
                return;
            }
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando.\n\n> `{ex.Message}`"));
            return;
        }

    }










    [SlashCommand("remove", "📗 | Remover um cargo de um usuário.")]
    public static async Task Remove(InteractionContext ctx,
        [Option("role", "Selecione o cargo que será removido.")] DiscordRole role,
        [Option("member", "Selecione o membro que perderá o cargo.")] DiscordUser member,
        [Option("reason", "Motivo do membro ter perdido o cargo.")] string? Reason = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Author = ctx.Member;
            DiscordMember Member = await ctx.Guild.GetMemberAsync(member.Id);




            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);
            await CheckRoleType.CheckType(ctx, 1, role);
            await CheckRoleType.CheckType(ctx, 2, role);
            await CheckRoleType.CheckType(ctx, 3, role);




            var botMember = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);
            var highestBotRole = botMember.Roles.OrderByDescending(r => r.Position).FirstOrDefault();
            if (role.Position >= highestBotRole.Position)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Eu não posso atribuir um cargo maior que o meu maior cargo atual!"));
                return;
            }
            var highestMemberRole = ctx.Member.Roles.OrderByDescending(r => r.Position).FirstOrDefault();
            if (role.Position >= highestMemberRole.Position)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Você não pode atribuir um cargo maior que o seu maior cargo atual!"));
                return;
            }




            if (member.Id == Author.Id)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Você não pode remover um cargo de si mesmo!"));
                return;
            }
            if (!Member.Roles.Any(r => r.Id == role.Id))
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"O membro seleconado não possui este cargo!"));
                return;
            }



            try
            {
                await Member.RevokeRoleAsync(role, Reason);

                var embed = new DiscordEmbedBuilder
                {
                    Description = $"O cargo **[ {role.Name} ]** foi removido com sucesso do usuário **[ {member.Mention} ]**!",
                    Color = new DiscordColor("#60ed7c")
                };

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Bip bup bip!")
                        .AddEmbed(embed));
                return;
            }
            catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando.\n\n> `{ex.Message}`"));
                return;
            }
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando.\n\n> `{ex.Message}`"));
            return;
        }
    }









    [SlashCommand("info", "📗 | Mostrar as informações de um cargo!")]
    public static async Task Info(InteractionContext ctx,
        [Option("role", "Selecione o cargo.")] DiscordRole role
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);



            var embed = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(role.Color.ToString() ?? "2a2d30")
            };
            if (role.IconUrl != null) { embed.WithThumbnail(role.IconUrl); }
            embed.AddField(
               "<:rezet_creditcard:1147341888538542132> Basics",
               $"> **Name**: `{role.Name}`" +
               $"\n> **ID**: `{role.Id}`" +
               $"\n> **Color**: `{role.Color.ToString() ?? "None"}`" +
               $"\n> **Members**: {ctx.Guild.Members.Values.Count(m => m.Roles.Contains(role))}"
            );
            embed.AddField(
               "<:rezet_settings1:1147163366561955932> Configurations",
               $"> {(role.IsMentionable ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Mentionable**." +
               $"\n> {(role.IsHoisted ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Hoisted**." +
               $"\n> {(role.IsManaged ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **By application**."
            );
            embed.AddField(
                "<:rezet_shine:1147373071737573446> Permissions",
                $"> {(role.Permissions.HasPermission(Permissions.Administrator) ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Administrator**." +
                $"\n> {(role.Permissions.HasPermission(Permissions.ManageRoles) ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Manage Server**." +
                $"\n> {(role.Permissions.HasPermission(Permissions.ManageRoles) ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Manage Channels.**." +
                $"\n> {(role.Permissions.HasPermission(Permissions.ManageRoles) ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Manage Roles**." +
                $"\n> {(role.Permissions.HasPermission(Permissions.ManageWebhooks) ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Manage Webhooks**."
            );



            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent("Bip bup bip!")
                    .AddEmbed(embed));
            return;
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando.\n\n> `{ex.Message}`"));
            return;
        }
    }









    [SlashCommand("create", "📗 | Criar um cargo!")]
    public static async Task Create(InteractionContext ctx,
        [Option("name", "Nome do cargo.")] string name,
        [Option("member", "Adicionar o cargo a um membro.")] DiscordUser? user = null,
        [Option("color", "Cor do cargo. [ ex: #FF0000 ]")] string? colorhex = null,
        [Option("perms", "Adicionar permissões ao cargo.")]
            [Choice("basics", 1)]
            [Choice("moderation", 2)]
            [Choice("manager", 3)]
            long perms = 0,
        [Option("time", "Deletar depois de um tempo.")]
            [Choice("1 minute", 1)]
            [Choice("5 minutes", 5)]
            [Choice("10 minutes", 10)]
            [Choice("30 minutes", 30)]
            [Choice("1 hour", 100)]
            [Choice("2 hours", 200)]
            [Choice("6 hours", 600)]
            [Choice("12 hours", 1200)]
            [Choice("1 day", 2400)]
            [Choice("2 days", 4800)]
            long? time = 0
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;
            var Author = ctx.Member;




            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);




            if (colorhex != null)
            {
                if (!colorhex.StartsWith("#"))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"A cor **HEX** precisa começar com `#`!"));
                    return;
                }
                if (!uint.TryParse(colorhex.Substring(1), System.Globalization.NumberStyles.HexNumber, null, out uint color))
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"A cor **HEX** fornecida é inválida! Por favor, forneça uma cor válida.\n- Siga o exemplo: `#c67bff`"));
                    return;
                }
            }




            try
            {
                var Color = colorhex != null ? colorhex.TrimStart('#') : "ffffff";
                if (perms == 0)
                {
                    var Role = await Guild.CreateRoleAsync(name, color: new DiscordColor(Color), permissions: Permissions.None);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Description =
                        $"Novo cargo criado com sucessso: {Role.Mention} !",
                        Color = new DiscordColor(Color)
                    };
                    if (user != null)
                    {
                        if (user.Id == ctx.User.Id)
                        {
                            await ctx.EditResponseAsync(
                                new DiscordWebhookBuilder()
                                    .WithContent($"Você não pode atribuir um cargo a si mesmo!"));
                            return;
                        }
                        DiscordMember Member = await ctx.Guild.GetMemberAsync(user.Id);
                        if (Member != null)
                        {
                            await Member.GrantRoleAsync(Role); embed.AddField("<:rezet_creditcard:1147341888538542132> Added on", $"{Member.Mention}");
                        }
                    }
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Bip bup bip!")
                            .AddEmbed(embed));
                    if (time != 0) { await DeleteRole(1, time, Author, Role); }
                    return;
                }
                else if (perms == 1)
                {
                    var Role = await Guild.CreateRoleAsync(name, color: new DiscordColor(Color),
                        permissions:
                            Permissions.AddReactions |
                            Permissions.UseExternalEmojis |
                            Permissions.UseExternalStickers |
                            Permissions.EmbedLinks |
                            Permissions.AttachFiles |
                            Permissions.UseApplicationCommands);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Description =
                        $"Novo cargo criado com sucessso: {Role.Mention} !",
                        Color = new DiscordColor(Color)
                    };
                    if (user != null)
                    {
                        if (user.Id == ctx.User.Id)
                        {
                            await ctx.EditResponseAsync(
                                new DiscordWebhookBuilder()
                                    .WithContent($"Você não pode atribuir um cargo a si mesmo!"));
                            return;
                        }
                        DiscordMember Member = await ctx.Guild.GetMemberAsync(user.Id);
                        if (Member != null)
                        {
                            await Member.GrantRoleAsync(Role); embed.AddField("<:rezet_creditcard:1147341888538542132> Added on", $"> {Member.Mention}");
                        }
                    }
                    embed.AddField(
                        "<:rezet_settings1:1147163366561955932> Configurations",
                        "> <:rezet_3_act:1189936284379119726> **Add Reactions**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Use External Emojis**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Use External Stickers**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Use Application Commands**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Send Embed Links**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Send Files**."
                    );
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed));
                    if (time != 0) { await DeleteRole(1, time, Author, Role); }
                    return;
                }
                else if (perms == 2)
                {
                    var Role = await Guild.CreateRoleAsync(name, color: new DiscordColor(Color),
                        permissions:
                            Permissions.KickMembers |
                            Permissions.MuteMembers |
                            Permissions.BanMembers |
                            Permissions.ManageNicknames |
                            Permissions.ManageMessages |
                            Permissions.ViewAuditLog);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Description =
                        $"Novo cargo criado com sucessso: {Role.Mention} !",
                        Color = new DiscordColor(Color)
                    };
                    if (user != null)
                    {
                        DiscordMember Member = await ctx.Guild.GetMemberAsync(user.Id);
                        if (Member != null)
                        {
                            await Member.GrantRoleAsync(Role); embed.AddField("<:rezet_creditcard:1147341888538542132> Added on", $"> {Member.Mention}");
                        }
                    }
                    embed.AddField(
                        "<:rezet_settings1:1147163366561955932> Configurations",
                        "> <:rezet_3_act:1189936284379119726> **Kick Members**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Mute Members**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Ban Members**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Manage Nicknames**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Manage Messages**." +
                        "\n> <:rezet_3_act:1189936284379119726> **View Aduti Logs**."
                    );
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed));
                    if (time != 0) { await DeleteRole(1, time, Author, Role); }
                    return;
                }
                else if (perms == 3)
                {
                    var Role = await Guild.CreateRoleAsync(name, color: new DiscordColor(Color),
                        permissions:
                            Permissions.ManageChannels |
                            Permissions.ManageEmojis |
                            Permissions.ManageRoles |
                            Permissions.ManageEvents |
                            Permissions.ViewAuditLog);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Description =
                        $"Novo cargo criado com sucessso: {Role.Mention} !",
                        Color = new DiscordColor(Color)
                    };
                    if (user != null)
                    {
                        DiscordMember Member = await ctx.Guild.GetMemberAsync(user.Id);
                        if (Member != null)
                        {
                            await Member.GrantRoleAsync(Role); embed.AddField("<:rezet_creditcard:1147341888538542132> Added on", $"> {Member.Mention}");
                        }
                    }
                    embed.AddField(
                        "<:rezet_settings1:1147163366561955932> Configurations",
                        "> <:rezet_3_act:1189936284379119726> **Manage Channels**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Manage Emojis**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Manage Roles**." +
                        "\n> <:rezet_3_act:1189936284379119726> **Manage Events**." +
                        "\n> <:rezet_3_act:1189936284379119726> **View Aduti Logs**."
                    );
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed));
                    if (time != 0) { await DeleteRole(1, time, Author, Role); }
                    return;
                }
                else
                {
                    var Role = await Guild.CreateRoleAsync(name, color: new DiscordColor(Color));
                    var embed = new DiscordEmbedBuilder()
                    {
                        Description =
                        $"Novo cargo criado com sucessso: {Role.Mention} !",
                        Color = new DiscordColor(Color)
                    };
                    if (user != null)
                    {
                        DiscordMember Member = await ctx.Guild.GetMemberAsync(user.Id);
                        if (Member != null)
                        {
                            await Member.GrantRoleAsync(Role); embed.AddField("<:rezet_creditcard:1147341888538542132> Added on", $"> {Member.Mention}");
                        }
                    }
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed));
                        if (time != 0) { await DeleteRole(1, time, Author, Role); }
                    return;
                }
            }
            catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Erro: `{ex}`"));
                return;
            }
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Erro: `{ex}`"));
            return;
        }
    }
    public static async Task DeleteRole(long f, long? Time, DiscordMember Member, DiscordRole Role)
    {
        try
        {
            // DELETE:
            if (f == 1)
            {
                switch (Time)
                {
                    // OFF;
                    case 0:
                        return;

                    // 1 MINUTE:
                    case 1:
                        await Task.Delay(60000);
                        await Role.DeleteAsync();
                        break;

                    // 5 MINUTES:
                    case 5:
                        await Task.Delay(300000);
                        await Role.DeleteAsync();
                        break;

                    // 10 MINUTES:
                    case 10:
                        await Task.Delay(600000);
                        await Role.DeleteAsync();
                        break;

                    // 30 MINUTES:
                    case 30:
                        await Task.Delay(1800000);
                        await Role.DeleteAsync();
                        break;

                    // 1 HOUR:
                    case 100:
                        await Task.Delay(3600000);
                        await Role.DeleteAsync();
                        break;

                    // 2 HOURS:
                    case 200:
                        await Task.Delay(7200000);
                        await Role.DeleteAsync();
                        break;

                    // 6 HOURS:
                    case 600:
                        await Task.Delay(21600000);
                        await Role.DeleteAsync();
                        break;

                    // 12 HOURS:
                    case 1200:
                        await Task.Delay(43200000);
                        await Role.DeleteAsync();
                        break;

                    // 1 DAY:
                    case 2400:
                        await Task.Delay(86400000);
                        await Role.DeleteAsync();
                        break;

                    // 2 DAYS:
                    case 4800:
                        await Task.Delay(172800000);
                        await Role.DeleteAsync();
                        break;
                }
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
}