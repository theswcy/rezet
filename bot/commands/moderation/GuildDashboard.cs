using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using Rezet;
using MongoDB.Bson;
using System.Collections;



[SlashCommandGroup("moderator", "To moderators")]
public class Moderators : ApplicationCommandModule
{
    [SlashCommand("dashboard", "🧰 | Moderation dashboard.")]
    public static async Task Dashboard(InteractionContext ctx)
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;
            var shard = Program._databaseService?.GetShard(Guild, 1);
            if (shard == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                Console.ResetColor();
                return;
            }


            await CheckPermi.CheckMemberPermissions(ctx, 2);
            await CheckPermi.CheckBotPermissions(ctx, 2);



            var embed = new DiscordEmbedBuilder
            {
                Description = $"## <:rezet_shine:1147368423475658882> Moderation Dashboard!\nBem vindo(a) a **dashboard** de moderação!\n⠀",
                Color = new DiscordColor("#7e67ff")
            };
            if (Guild.IconUrl != null) { embed.WithThumbnail(Guild.IconUrl); }
            var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{ctx.User.Id}_PAexit", "Exit");




            var emoji = new DiscordComponentEmoji("🪛");
            var AutomaticActions = new List<DiscordSelectComponentOption>
            {
                new("View autoroles", "autorole", "View roles in autorole function.", emoji: emoji),
                new("View autopings", "autoping", "View channels with autoping function.", emoji: emoji),
                new("View mod logs channel", "modlogs", "View channels with mod logs function.", emoji: emoji)
            };
            // SHOW THE AUTOROLE CONFIGS:
            if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"] != BsonNull.Value)
            {
                var RolesDict = shard
                [$"{Guild.Id}"]
                ["moderation"]
                ["auto_actions"]
                ["auto_role"]
                .AsBsonDocument;


                int cc = 0;
                foreach (var entry in RolesDict)
                {
                    cc++;
                }
                embed.AddField(
                    "<:rezet_3_act:1189936284379119726> Autorole:",
                    $"> Cargos na função Autorole: **{cc}**."
                );
            }
            else
            {
                embed.AddField(
                    "<:rezet_3_nact:1189936390113341601> Autorole:",
                    "> Nenhum cargo adicionado a função **Autorole**."
                );
                AutomaticActions.RemoveAll(action => action.Value == "autorole");
            }



            // SHOW THE AUTOPING CONFIGS:
            if (shard[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] != BsonNull.Value)
            {
                var PingDict = shard
                [$"{Guild.Id}"]
                ["moderation"]
                ["auto_actions"]
                ["auto_ping"]
                .AsBsonDocument;
                int r = 0;
                foreach (var entry in PingDict)
                {
                    r++;
                }
                embed.AddField(
                    "<:rezet_3_act:1189936284379119726> Autoping:",
                    $"> Canais com a função Autoping: **{r}**."
                );
            }
            else
            {
                embed.AddField(
                    "<:rezet_3_nact:1189936390113341601> Autoping:",
                    "> Nenhum canal com a função autoping ativada."
                );
                AutomaticActions.RemoveAll(action => action.Value == "autoping");
            }



            // SHOW THE MODERATION LOGS CONFIGS:
            var ModLogs = shard
                [$"{Guild.Id}"]
                ["moderation"]
                ["mod_logs"]
                .AsBsonDocument.ToDictionary(
                    elem => elem.Name,
                    elem => elem.Value
                );
            int t = 0;
            foreach (var entry in ModLogs)
            {
                if (entry.Value != BsonNull.Value)
                {
                    t++;
                }
            }
            if (t != 0)
            {
                embed.AddField(
                    "<:rezet_3_act:1189936284379119726> Mod Logs:",
                    $"> Canais de registro da moderação: **{t}**."
                );
            }
            else
            {
                embed.AddField(
                    "<:rezet_3_nact:1189936390113341601> Mod Logs:",
                    "> Nenhum canal possui a função de registros."
                );
                AutomaticActions.RemoveAll(action => action.Value == "modlogs");
            }



            var Builder = new DiscordWebhookBuilder()
                    .WithContent("Bip bup bip!")
                    .AddEmbed(embed)
                    .AddComponents(button);
            if (AutomaticActions.Count > 0)
            {
                var AASelect = new DiscordSelectComponent($"{ctx.User.Id}_AAOpti", "Automatic Actions", AutomaticActions);
                Builder.AddComponents(AASelect);
            }



            await ctx.EditResponseAsync(Builder);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}