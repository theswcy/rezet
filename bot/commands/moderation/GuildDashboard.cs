using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using Rezet;
using MongoDB.Bson;
using System.Collections;
using RezetSharp;



[SlashCommandGroup("moderator", "To moderators")]
public class Moderators : ApplicationCommandModule
{
    [SlashCommand("dashboard", "🧰 | Dashboard das configurações de moderação.")]
    public static async Task Dashboard(InteractionContext ctx)
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = ctx.Guild;


            await CheckPermi.CheckMemberPermissions(ctx, 2);
            await CheckPermi.CheckBotPermissions(ctx, 2);



            var embed = new DiscordEmbedBuilder
            {
                Description = $"## <:rezet_shine:1147368423475658882> Moderation Dashboard!\nBem vindo(a) a **dashboard** de moderação!\n⠀",
                Color = new DiscordColor("#7e67ff")
            };
            if (Guild.IconUrl != null) { embed.WithThumbnail(Guild.IconUrl); }
            var button = new DiscordButtonComponent(ButtonStyle.Primary, $"{ctx.User.Id}_PAexit", "Exit", emoji: new DiscordComponentEmoji(id: 1308125206883078225));




#pragma warning disable CS8602
            embed.AddField(
                "<:rezet_settings1:1147163366561955932> Community",
                $"> {(Guild.RulesChannel != null ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Canal de regras**: {(Guild.RulesChannel != null ? Guild.RulesChannel.Mention : "`Uncativated`.")}" +
                $"\n> {(Guild.SystemChannel != null ? "<:rezet_3_act:1189936284379119726>": "<:rezet_3_nact:1189936390113341601>")} **Canal padrão**: {(Guild.SystemChannel != null ? Guild.SystemChannel.Mention : "`Uncativated`.")}" +
                $"\n> {(Guild.MfaLevel == MfaLevel.Enabled ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **2FA interno**. {(Guild.MfaLevel == MfaLevel.Enabled ? "`Activated`." : "`Uncativated`. <:rezet_exclamation:1164417019303702570> `Ativação recomendada`.")}" +
                $"\n> <:rezet_verified:1147163979022610472> **Verificação**: `{Guild.VerificationLevel}`."
            );




            var emoji = new DiscordComponentEmoji("🪛");
            var AutomaticActions = new List<DiscordSelectComponentOption>
            {
                new("View autoroles", "autorole", "View roles in autorole function.", emoji: emoji),
                new("View autopings", "autoping", "View channels with autoping function.", emoji: emoji),
                new("View mod logs channel", "modlogs", "View channels with mod logs function.", emoji: emoji)
            };




            var Herrscher = EngineV8X.HerrscherRazor?.GetHerrscherDocument(Guild);
            // SHOW THE MODERATION CONFIGS:
            var GuildSecurity = Herrscher[$"{Guild.Id}"]["moderation"]["security"];
            embed.AddField(
                "<:rezet_shine:1147373071737573446> Security [ Em breve! ]",
                $"> {(GuildSecurity["sharp-mode"] != false ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Sharp-mode**." +
                $"\n> {(GuildSecurity["anti-invites"] != BsonNull.Value ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Anti-invites**." +
                $"\n> {(GuildSecurity["anti-raid"] != BsonNull.Value ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Anti-raid**." +
                $"\n> {(GuildSecurity["anti-selfbot"] != BsonNull.Value ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Anti-selfbot**." +
                $"\n> {(GuildSecurity["blackout"] != BsonNull.Value ? "<:rezet_3_act:1189936284379119726>" : "<:rezet_3_nact:1189936390113341601>")} **Blackout**."
            );
            // SHOW THE AUTOROLE CONFIGS:
            if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_role"] != BsonNull.Value)
            {
                var RolesDict = Herrscher
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
                    "<:rezet_3_act:1189936284379119726> Autorole",
                    $"> `{cc}` **Cargos na função Autorole**."
                );
            }
            else
            {
                embed.AddField(
                    "<:rezet_3_nact:1189936390113341601> Autorole",
                    "> **Nenhum cargo adicionado a função Autorole**."
                );
                AutomaticActions.RemoveAll(action => action.Value == "autorole");
            }



            // SHOW THE AUTOPING CONFIGS:
            if (Herrscher[$"{Guild.Id}"]["moderation"]["auto_actions"]["auto_ping"] != BsonNull.Value)
            {
                var PingDict = Herrscher
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
                    "<:rezet_3_act:1189936284379119726> Autoping",
                    $"> `{r}`**Canais com a função Autoping**."
                );
            }
            else
            {
                embed.AddField(
                    "<:rezet_3_nact:1189936390113341601> Autoping",
                    "> **Nenhum canal com a função autoping ativada**."
                );
                AutomaticActions.RemoveAll(action => action.Value == "autoping");
            }



            // SHOW THE MODERATION LOGS CONFIGS:
            var ModLogs = Herrscher
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
                    "<:rezet_3_act:1189936284379119726> Mod Logs",
                    $"> `{t}` **Canais de registro da moderação**."
                );
            }
            else
            {
                embed.AddField(
                    "<:rezet_3_nact:1189936390113341601> Mod Logs",
                    "> **Nenhum canal possui a função de registros**."
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
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
                );
                Console.WriteLine($"    ➜  Slash Command: /moderator dashboard\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                return;
            }
    }
}