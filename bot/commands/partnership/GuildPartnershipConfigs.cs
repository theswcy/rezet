using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;
using MongoDB.Driver;
using RezetSharp;
using RezetSharp.LuminyCache;



[SlashCommandGroup("partnership", "Partnership's commands")]
public class PartnershipCommands : ApplicationCommandModule
{
    [SlashCommand("dashboard", "🎋 | Dashboard das configurações de parcerias!")]
    public static async Task Dashboard(InteractionContext ctx)
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;
            var Author = ctx.Member;



            // MANAGE CHANNELS.
            await CheckPermi.CheckMemberPermissions(ctx, 3);
            await CheckPermi.CheckBotPermissions(ctx, 3);



            var Herrscher = EngineV8X.HerrscherRazor?.GetHerrscherDocument(Guild);




#pragma warning disable CS8602
            if (Herrscher[$"{Guild.Id}"]["partner"]["option"].AsInt32 == 0)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("Essa comunidade não possui a função **Partnership** ativada! Use o comando `/partnership setup` para ativar.")
                );
                return;
            }
            else
            {
                var emoji = new DiscordComponentEmoji("🪛");
                var emoji2 = new DiscordComponentEmoji("📘");
                var emoji3 = new DiscordComponentEmoji("📗");
                var emoji4 = new DiscordComponentEmoji("📝");
                var options1 = new[]
                {
                    new DiscordSelectComponentOption("Variables", "variables", "See available variables!", emoji: emoji2),
                    new DiscordSelectComponentOption("Add embed", "add", "Add a new embed!", emoji: emoji),
                    new DiscordSelectComponentOption("Edit embed", "edit", "Edit an embed.", emoji: emoji),
                    new DiscordSelectComponentOption("Set embed", "set", "Set an embed.", emoji: emoji),
                    new DiscordSelectComponentOption("Delete embed", "delete", "Delete an embed.", emoji: emoji),
                    new DiscordSelectComponentOption("Preview embed", "preview", "The preview of the selected embed.", emoji: emoji3)
                };
                var options2 = new[]
                {
                    new DiscordSelectComponentOption("Reset ranking", "reset", "Reset the ranking's score.", emoji: emoji),
                    new DiscordSelectComponentOption("Unactivate local ranked", "unactivate", "Unactivate the local ranked.", emoji: emoji),
                    new DiscordSelectComponentOption("Ranking patents", "patents", "The ranking's patents.", emoji: emoji2),
                    new DiscordSelectComponentOption("Invite Link", "invite", "Modify the guild invite.", emoji: emoji4)
                };
                var options3 = new[]
                {
                    new DiscordSelectComponentOption("Delete on everyone/here", "everyone", "Delete on mention everyone/here.", emoji: emoji),
                    new DiscordSelectComponentOption("Delete on partner quit", "delete", "Delete if the partner quit.", emoji: emoji)
                };
                var selectMenu1 = new DiscordSelectComponent($"{ctx.User.Id}_PEOptions", "Embed Options", options1);
                var selectMenu2 = new DiscordSelectComponent($"{ctx.User.Id}_PERanking", "Ranking Options", options2);
                var selectMenu3 = new DiscordSelectComponent($"{ctx.User.Id}_PEConfigs", "Others Configurations", options3);
                var buttons = new DiscordComponent[]
                {
                    new DiscordButtonComponent(ButtonStyle.Primary, $"{ctx.User.Id}_PAexit", "Close", emoji: new DiscordComponentEmoji(id: 1308125206883078225)),
                    new DiscordButtonComponent(ButtonStyle.Secondary, $"{ctx.User.Id}_PAuna", "Unactivate Function", emoji: new DiscordComponentEmoji(id: 1308125081339432971))
                };



                var ch = "<:rezet_3_nact:1189936390113341601> **Logs**: Unactivated.";
                if (Herrscher[$"{Guild.Id}"]["partner"]["log"] != BsonNull.Value)
                {
                    var channel = Guild.GetChannel((ulong)Herrscher[$"{Guild.Id}"]["partner"]["log"].ToInt64());
                    ch = $"<:rezet_3_act:1189936284379119726> **Logs**: {channel.Mention}";
                }
                var xr = "<:rezet_3_nact:1189936390113341601> **On everyone/here**: Unactivated.";
                if (Herrscher[$"{Guild.Id}"]["partner"]["anti-eh"] != 0)
                {
                    xr = $"<:rezet_3_act:1189936284379119726> **On everyone/here**: Mode {Herrscher[$"{Guild.Id}"]["partner"]["anti-eh"]}.";
                }
                var tr = "<:rezet_3_nact:1189936390113341601> **On partner quit**: Unactivated.";
                if (Herrscher[$"{Guild.Id}"]["partner"]["anti-qi"] != 0)
                {
                    tr = $"<:rezet_3_act:1189936284379119726> **On partner quit**: Activated.";
                }



                var rbxp = "<:rezet_c_elite:1290676392870023190> `C-Elite`";
                if (Herrscher[$"{Guild.Id}"]["partner"]["xp"] > 10000) { rbxp = "<:rezet_sss_elite:1290676886556377112> `SSS-Elite`"; }
                else if (Herrscher[$"{Guild.Id}"]["partner"]["xp"] > 5000) { rbxp = "<:rezet_ss_elite:1290676831544017046> `SS-Elite`"; }
                else if (Herrscher[$"{Guild.Id}"]["partner"]["xp"] > 2500) { rbxp = "<:rezet_s_elite:1290676775826886696> `S-Elite`"; }
                else if (Herrscher[$"{Guild.Id}"]["partner"]["xp"] > 1000) { rbxp = "<:rezet_a_elite:1290676720537567373> `A-Elite`"; }
                else if (Herrscher[$"{Guild.Id}"]["partner"]["xp"] >= 500) { rbxp = "<:rezet_b_elite:1290676630527934567>`B-Elite`"; }
                var embed = new DiscordEmbedBuilder()
                {
                    Description =
                        "## <:rezet_plant:1308125160577962004> Partnership __Dashboard__!" +
                        "\nBem vindo(a) a **dashboard** da função **partnership**.",
                    Color = new DiscordColor("7e67ff")
                };
                var Opti = Herrscher[$"{Guild.Id}"]["partner"]["configs"]["options"];
                embed.AddField(
                    "<:rezet_channels:1308125117875752961> Setup:",
                    $"> <:rezet_3_act:1189936284379119726> **Role**: <@&{Opti["role"]}>" +
                    $"\n> <:rezet_3_act:1189936284379119726> **Ping**: <@&{Opti["ping"]}>" +
                    $"\n> <:rezet_3_act:1189936284379119726> **Channel**: <#{Opti["channel"]}>" +
                    $"\n> {ch}" +
                    $"\n> {xr}" +
                    $"\n> {tr}"
                );
                var o = "Unactivated.";
                if (Herrscher[$"{Guild.Id}"]["partner"]["leaderboard"]["invite"] != BsonNull.Value)
                {
                    try
                    {
                        var inv = await EngineV8X.RezetRazor.GetInviteByCodeAsync(Herrscher[$"{Guild.Id}"]["partner"]["leaderboard"]["invite"].AsString);
                        if (inv.Guild.Id != Guild.Id)
                        {
                            o = $"https://discord.gg/{Herrscher[$"{Guild.Id}"]["partner"]["leaderboard"]["invite"].AsString}\n> [ <:rezet_exclamation:1164417019303702570> este convite pertence a outro servidor! ]";
                        }
                        else
                        {
                            o = $"https://discord.gg/{Herrscher[$"{Guild.Id}"]["partner"]["leaderboard"]["invite"].AsString}";
                        }
                    }
                    catch (Exception)
                    {
                        o = $"https://discord.gg/{Herrscher[$"{Guild.Id}"]["partner"]["leaderboard"]["invite"].AsString}";
                    }
                }
                if (Herrscher[$"{Guild.Id}"]["partner"]["leaderboard"]["option"] == 1)
                {
                    embed.AddField(
                        "<:rezet_channels:1308125117875752961> Ranked:",
                        $"> **Partnerships**: `{Herrscher[$"{Guild.Id}"]["partner"]["ps"]}`" +
                        $"\n> **Partner XP**: `{Herrscher[$"{Guild.Id}"]["partner"]["xp"]}`" +
                        $"\n> **Ranking**: {rbxp}" +
                        $"\n> **Invite**: {o}"
                    );
                }
                else
                {
                    embed.AddField(
                        "<:rezet_channels:1308125117875752961> Ranked:",
                        $"> <:rezet_3_nact:1189936390113341601> **Unactivated**."
                    );
                }




                var DashboardBuilder = new DiscordWebhookBuilder();
                if (Herrscher[$"{Guild.Id}"]["partner"]["leaderboard"]["option"] == 1)
                {
                    DashboardBuilder
                        .AddComponents(buttons[0], buttons[1])
                        .AddComponents(selectMenu1)
                        .AddComponents(selectMenu2)
                        .AddComponents(selectMenu3);
                }
                else
                {
                    var buttonALB = new DiscordButtonComponent(ButtonStyle.Primary, $"{ctx.User.Id}_ALB", "Activate local ranked", emoji: new DiscordComponentEmoji(id: 1308125039534674033));
                    DashboardBuilder
                        .AddComponents(buttons[0],buttonALB , buttons[1])
                        .AddComponents(selectMenu1)
                        .AddComponents(selectMenu3);
                }



                if (Herrscher[$"{Guild.Id}"]["partner"]["ticket"]["option"] == 1)
                {
                    embed.AddField(
                        "<:rezet_3_act:1189936284379119726> Tickets:",
                        "> **Automatic**: coming soon." +
                        $"\n> **Tickets Total**: `{Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["count"].AsInt32}`" +
                        $"\n> **Ticket Channel**: <#{Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["configs"]["channel"].AsInt64}>" +
                        $"\n> **Ticket Category**: <#{Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["configs"]["category"].AsInt64}>" +
                        $"\n> **Support Role**: <@&{Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["configs"]["support"].AsInt64}>"
                    );



                    var emoji5 = new DiscordComponentEmoji("🔖");
                    var emoji6 = new DiscordComponentEmoji("🔔");
                    var options4 = new[]
                    {
                        // new DiscordSelectComponentOption("View ticket", "1_view", "View ticket.", emoji: emoji5),
                        // new DiscordSelectComponentOption("Add ticket", "1_add", "Add ticket", emoji: emoji5),
                        // new DiscordSelectComponentOption("Edit ticket", "1_edit", "Edit ticket.", emoji: emoji5),
                        new DiscordSelectComponentOption("Tickets Builder", "1_edit", "Builder a ticket.", emoji: emoji5),
                        new DiscordSelectComponentOption("Ticket's button", "1_butt", "Ticket's button.", emoji: emoji5),
                        // new DiscordSelectComponentOption("View ticket message", "1_view", "View the ticket's message", emoji: emoji6),
                        // new DiscordSelectComponentOption("Edit ticket message", "1_edit", "Edit the ticket's message.", emoji: emoji6)
                    };
                    var selectMenu4 = new DiscordSelectComponent($"{ctx.User.Id}_PTICptio", "Ticket Options", options4);

                    DashboardBuilder.AddComponents(selectMenu4);
                }
                else
                {
                    embed.AddField(
                        "<:rezet_3_nact:1189936390113341601> Tickets:",
                        "> Unactivated."
                    );



                    var buttonTicket = new DiscordButtonComponent(ButtonStyle.Success, $"{ctx.User.Id}_TicketACT", "Activate ticket");
                    DashboardBuilder.AddComponents(buttonTicket);
                }



                await ctx.EditResponseAsync(
                    DashboardBuilder
                        .WithContent("Bip bup bip!")
                        .AddEmbed(embed)
                );
            }
        }
        catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
                );
                Console.WriteLine($"    ➜  Slash Command: /partnership dashboard\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                return;
            }
    }






    [SlashCommand("setup", "🎋 | Configurar função de parcerias.")]
    public static async Task Activate(InteractionContext ctx,
        [Option("channel", "Canal de parcerias.")] DiscordChannel channel,
        [Option("ping", "Ping de notificação de novas parcerias.")] DiscordRole ping,
        [Option("role", "Cargo que será dado aos novos parceiros.")] DiscordRole role,
        [Option("logs", "Canal de logs de parcerias (recomendado configurar).")] DiscordChannel? logs = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;
            var Author = ctx.Member;
            var Channel = channel;




            // MANAGE CHANNELS - MANAGE ROLES.
            await CheckPermi.CheckMemberPermissions(ctx, 3);
            await CheckPermi.CheckBotPermissions(ctx, 3);
            await CheckPermi.CheckMemberPermissions(ctx, 4);
            await CheckPermi.CheckBotPermissions(ctx, 4);




            if (!channel.Guild.CurrentMember.Permissions.HasPermission(Permissions.SendMessages))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent($"Eu não tenho permissão para enviar mensagens no canal {channel.Mention}!"));
                return;
            }




            var botMember = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);
            var highestBotRole = botMember.Roles.OrderByDescending(r => r.Position).FirstOrDefault();
            if (role.Position >= highestBotRole?.Position)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent($"Eu não posso atribuir um cargo maior que o meu maior cargo atual!"));
                return;
            }




            // DATABASE AND CACHE:
            var LumiCache = new CacheTier1_ForPartnership();
            if (LumiCache.GetGuild(ctx.Guild.Id) != null)
            {
                LumiCache.RemoveGuild(ctx.Guild.Id);
                LumiCache.SaveGuild(ctx.Guild.Id, Channel.Id);
            }
            else
            {
                LumiCache.SaveGuild(ctx.Guild.Id, Channel.Id);
            }
            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);
            var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
            var tropical = new BsonDocument
                    {
                        { "role", (long)role.Id },
                        { "ping", (long)ping.Id },
                        { "channel", (long)channel.Id }
                    };
            if (logs != null)
            {
#pragma warning disable CS8602
                var update = Builders<BsonDocument>.Update
                        .Set($"{Guild.Id}.partner.configs.options", tropical)
                        .Set($"{Guild.Id}.partner.option", 1)
                        .Set($"{Guild.Id}.partner.log", (long)logs.Id);
                await collection.UpdateOneAsync(Herrscher, update);
            }
            else
            {
                var update = Builders<BsonDocument>.Update
                        .Set($"{Guild.Id}.partner.configs.options", tropical)
                        .Set($"{Guild.Id}.partner.option", 1)
                        .Set($"{Guild.Id}.partner.log", BsonNull.Value);
                await collection.UpdateOneAsync(Herrscher, update);
            }





            var embed = new DiscordEmbedBuilder()
            {
                Description =
                    "Nice! Função **partnership** modificada!\n"
                    ,
                Color = new DiscordColor("7e67ff")
            };
            var t = "<:rezet_3_nact:1189936390113341601> **Logs**: Unactivated.";
            if (logs != null) { t = $"<:rezet_3_act:1189936284379119726> **Logs**: {logs.Mention}"; }
            embed.AddField(
                "<:rezet_channels:1308125117875752961> Setup:",
                $"> <:rezet_3_act:1189936284379119726> **Channel**: {channel.Mention}" +
                $"\n> <:rezet_3_act:1189936284379119726> **Role**: {role.Mention}" +
                $"\n> <:rezet_3_act:1189936284379119726> **Ping**: {ping.Mention}" +
                $"\n> {t}"
            );
            var button = new DiscordButtonComponent(ButtonStyle.Primary, $"{ctx.User.Id}_PAexit", "Close", emoji: new DiscordComponentEmoji(id: 1308125206883078225));





            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent("Bip bup bip!")
                .AddEmbed(embed)
                .AddComponents(button)
            );
        }
        catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
                );
                Console.WriteLine($"    ➜  Slash Command: /partnership setup\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                return;
            }
    }
}