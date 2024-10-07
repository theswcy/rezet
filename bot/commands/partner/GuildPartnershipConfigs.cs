using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;
using MongoDB.Driver;
using Rezet;



[SlashCommandGroup("partnership", "Partnership's commands")]
public class PartnershipCommands : ApplicationCommandModule
{
    // Comando finalizado!

    [SlashCommand("dashboard", "🎋 | Partnership's dashboard!")]
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



            var shard = Program._databaseService?.GetShard(Guild, 1);




#pragma warning disable CS8602
            if (shard[$"{Guild.Id}"]["partner"]["option"].AsInt32 == 0)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("<:rezet_dred:1147164215837208686> Essa comunidade não possui a função **Partnership** ativada! Use o comando `/partnership setup` para ativar.")
                );
                return;
            }
            else
            {
                var emoji = new DiscordComponentEmoji("🪛");
                var emoji2 = new DiscordComponentEmoji("📘");
                var emoji3 = new DiscordComponentEmoji("📗");
                var options1 = new[]
                {
                    new DiscordSelectComponentOption("Variables", "variables", "See available variables!", emoji: emoji2),
                    new DiscordSelectComponentOption("Add embed", "add", "Add a new embed!", emoji: emoji),
                    new DiscordSelectComponentOption("Set embed", "set", "Set an embed.", emoji: emoji),
                    new DiscordSelectComponentOption("Delete embed", "delete", "Delete an embed.", emoji: emoji),
                    new DiscordSelectComponentOption("Preview embed", "preview", "The preview of the selected embed.", emoji: emoji3)
                };
                var options2 = new[]
                {
                    new DiscordSelectComponentOption("Reset ranking", "reset", "Reset the ranking's score.", emoji: emoji),
                    new DiscordSelectComponentOption("Unactivate local ranked", "unactivate", "Unactivate the local ranked.", emoji: emoji),
                    new DiscordSelectComponentOption("Ranking patents", "patents", "The ranking's patents.", emoji: emoji2)
                };
                var options3 = new[]
                {
                    new DiscordSelectComponentOption("Delete on everyone/here", "everyone", "Delete on mention everyone/here.", emoji: emoji),
                    new DiscordSelectComponentOption("Delete on partner quit", "delete", "Delete if the partner quit.", emoji: emoji)
                };
                var selectMenu1 = new DiscordSelectComponent($"{ctx.User.Id}_PEOptions", "Embed options", options1);
                var selectMenu2 = new DiscordSelectComponent($"{ctx.User.Id}_PERanking", "Ranking options", options2);
                var selectMenu3 = new DiscordSelectComponent($"{ctx.User.Id}_PEConfigs", "Others configurations", options3);
                var buttons = new DiscordComponent[]
                {
                    new DiscordButtonComponent(ButtonStyle.Danger, $"{ctx.User.Id}_PAexit", "Exit"),
                    new DiscordButtonComponent(ButtonStyle.Secondary, $"{ctx.User.Id}_PAtutorial", "Tutorial"),
                    new DiscordButtonComponent(ButtonStyle.Secondary, $"{ctx.User.Id}_PAuna", "Unactivate function")
                };



                var ch = "<:rezet_3_nact:1189936390113341601> **Logs**: Unactivated.";
                if (shard[$"{Guild.Id}"]["partner"]["log"] != BsonNull.Value)
                {
                    var channel = Guild.GetChannel((ulong)shard[$"{Guild.Id}"]["partner"]["log"].ToInt64());
                    ch = $"<:rezet_3_act:1189936284379119726> **Logs**: {channel.Mention}";
                }
                var xr = "<:rezet_3_nact:1189936390113341601> **On everyone/here**: Unactivated.";
                if (shard[$"{Guild.Id}"]["partner"]["anti-eh"] != 0)
                {
                    xr = $"<:rezet_3_act:1189936284379119726> **On everyone/here**: Mode {shard[$"{Guild.Id}"]["partner"]["anti-eh"]}.";
                }
                var tr = "<:rezet_3_nact:1189936390113341601> **On partner quit**: Unactivated.";
                if (shard[$"{Guild.Id}"]["partner"]["anti-qi"] != 0)
                {
                    tr = $"<:rezet_3_act:1189936284379119726> **On partner quit**: Activated.";
                }



                var rbxp = "<:rezet_c_elite:1290676392870023190> `C-Elite`";
                if (shard[$"{Guild.Id}"]["partner"]["xp"] > 10000) { rbxp = "<:rezet_sss_elite:1290676886556377112> `SSS-Elite`"; }
                else if (shard[$"{Guild.Id}"]["partner"]["xp"] > 5000) { rbxp = "<:rezet_ss_elite:1290676831544017046> `SS-Elite`"; }
                else if (shard[$"{Guild.Id}"]["partner"]["xp"] > 2500) { rbxp = "<:rezet_s_elite:1290676775826886696> `S-Elite`"; }
                else if (shard[$"{Guild.Id}"]["partner"]["xp"] > 1000) { rbxp = "<:rezet_a_elite:1290676720537567373> `A-Elite`"; }
                else if (shard[$"{Guild.Id}"]["partner"]["xp"] >= 500) { rbxp = "<:rezet_b_elite:1290676630527934567>`B-Elite`"; }
                var embed = new DiscordEmbedBuilder()
                {
                    Description =
                        "## Partnership Dashboard!" +
                        "\nBem vindo(a) a **dashboard** da função **partnership**.",
                    Color = new DiscordColor("7e67ff")
                };
                var Opti = shard[$"{Guild.Id}"]["partner"]["configs"]["options"];
                embed.AddField(
                    "<:rezet_settings1:1147163366561955932> Configurations:",
                    $"> <:rezet_3_act:1189936284379119726> **Role**: <@&{Opti["role"]}>" +
                    $"\n> <:rezet_3_act:1189936284379119726> **Ping**: <@&{Opti["ping"]}>" +
                    $"\n> <:rezet_3_act:1189936284379119726> **Channel**: <#{Opti["channel"]}>" +
                    $"\n> {ch}" +
                    $"\n> {xr}" +
                    $"\n> {tr}"
                );
                embed.AddField(
                    "<:rezet_shine:1147368423475658882> Partnership status:",
                    $"> **Partnerships**: `{shard[$"{Guild.Id}"]["partner"]["ps"]}`" +
                    $"\n> **Partner XP**: `{shard[$"{Guild.Id}"]["partner"]["xp"]}`" +
                    $"\n> **Ranking**: {rbxp}"
                );



                if (shard[$"{Guild.Id}"]["partner"]["leaderboard"]["option"] == 1)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed)
                            .AddComponents(buttons[0], buttons[1], buttons[2])
                            .AddComponents(selectMenu1)
                            .AddComponents(selectMenu2)
                            .AddComponents(selectMenu3)
                    );
                    return;
                }
                else
                {
                    var buttonALB = new DiscordComponent[]
                    {
                        new DiscordButtonComponent(ButtonStyle.Success, $"{ctx.User.Id}_ALB", "Activate local ranked")
                    };
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed)
                            .AddComponents(buttons[0], buttons[1], buttons[2], buttonALB[0])
                            .AddComponents(selectMenu1)
                            .AddComponents(selectMenu3)
                    );
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }









    // Comando finalizado!

    [SlashCommand("setup", "🎋 | Setup partnership function")]
    public static async Task Activate(InteractionContext ctx,
        [Option("channel", "The partnership's channel.")] DiscordChannel channel,
        [Option("ping", "The partnership's ping.")] DiscordRole ping,
        [Option("role", "The partner role.")] DiscordRole role,
        [Option("logs", "The partnership's logs channel.")] DiscordChannel? logs = null
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
                    .WithContent($"<:rezet_dred:1147164215837208686> Eu não tenho permissão para enviar mensagens no canal {channel.Mention}!"));
                return;
            }




            var botMember = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);
            var highestBotRole = botMember.Roles.OrderByDescending(r => r.Position).FirstOrDefault();
            if (role.Position >= highestBotRole?.Position)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent($"<:rezet_dred:1147164215837208686> Eu não posso atribuir um cargo maior que o meu maior cargo atual!"));
                return;
            }




            // DATABASE:
            var shard = Program._databaseService?.GetShard(Guild, 1);
            if (shard == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                Console.ResetColor();
                return;
            }
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
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
                        .Set($"{Guild.Id}.partner.log", (long)logs.Id);
                await collection.UpdateOneAsync(shard, update);
            }
            else
            {
                var update = Builders<BsonDocument>.Update
                        .Set($"{Guild.Id}.partner.configs.options", tropical)
                        .Set($"{Guild.Id}.partner.log", BsonNull.Value);
                await collection.UpdateOneAsync(shard, update);
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
                "<:rezet_settings1:1147163366561955932> Configurations:",
                $"> <:rezet_3_act:1189936284379119726> **Channel**: {channel.Mention}" +
                $"\n> <:rezet_3_act:1189936284379119726> **Role**: {role.Mention}" +
                $"\n> <:rezet_3_act:1189936284379119726> **Ping**: {ping.Mention}" +
                $"\n> {t}"
            );
            var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{ctx.User.Id}_PAexit", "Close");




            if (shard.GetValue($"{Guild.Id}")?.AsBsonDocument.GetValue("partner")?.AsBsonDocument.GetValue("option") == 0)
            {
                var button2 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{ctx.User.Id}_APFB", "Activate partnership function");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("Bip bup bip!")
                    .AddEmbed(embed)
                    .AddComponents(button, button2)
                );
            }
            else
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("Bip bup bip!")
                    .AddEmbed(embed)
                    .AddComponents(button)
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}