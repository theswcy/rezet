using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;
using DSharpPlus.SlashCommands;




#pragma warning disable CS8602
public class ModeratorDashboard
{
    public static async Task MD_Primary(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // AUTOMATIC ACTIONS:
            if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_AAOpti")
            {
                var Guild = e.Guild;
                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (e.Values[0] == "autorole")
                {
                    await e.Interaction.DeferAsync();
                    var RoleDict = shard
                            [$"{Guild.Id}"]
                            ["moderation"]
                            ["auto_actions"]
                            ["auto_role"]
                            .AsBsonDocument.ToDictionary(
                                elem => elem.Name,
                                elem => elem.Value.AsInt64
                            );


                    var embed = new DiscordEmbedBuilder()
                    {
                        Color = new DiscordColor("#7e67ff")
                    };
                    var desc = "## <:rezet_shine:1147373071737573446> Autorole!\nEsses aqui são os cargos que eu darei quando algum usuário novo entrar na comunidade!\n";




                    foreach (var entry in RoleDict)
                    {
                        var r = Guild.GetRole((ulong)entry.Value);
                        var botMember = await e.Guild.GetMemberAsync(Program.Rezet.CurrentUser.Id);
                        var highestBotRole = botMember.Roles.OrderByDescending(r => r.Position).FirstOrDefault();

                        // PERMISSIONS:
                        if (r.Permissions.HasPermission(Permissions.Administrator))
                        {
                            desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Administrator`.\n";
                        }
                        else if (r.Permissions.HasPermission(Permissions.ManageGuild))
                        {
                            desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Manage Guild`.\n";
                        }
                        else if (r.Permissions.HasPermission(Permissions.ManageChannels))
                        {
                            desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Manage Channels`.\n";
                        }
                        else if (r.Permissions.HasPermission(Permissions.ManageRoles))
                        {
                            desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Manage Roles`.\n";
                        }
                        else if (r.Permissions.HasPermission(Permissions.ManageWebhooks))
                        {
                            desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Manage Webhooks`.\n";
                        }
                        else if (r.Permissions.HasPermission(Permissions.BanMembers))
                        {
                            desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Ban Members`.\n";
                        }
                        else if (r.Permissions.HasPermission(Permissions.KickMembers))
                        {
                            desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Kick Members`.\n";
                        }
                        else if (r.Permissions.HasPermission(Permissions.MuteMembers))
                        {
                            desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Mute Members`.\n";
                        }


                        // HIGHEST ROLE:
                        else if (r.Position > highestBotRole.Position)
                        {
                            desc += $"<:rezet_dred:1147164215837208686> {r.Mention}\n> Cargo bloqueado!\n> O cargo é maior que o meu maior cargo atual!\n";
                        }
                        else if (r.Position == highestBotRole.Position)
                        {
                            desc += $"<:rezet_dred:1147164215837208686> {r.Mention}\n> Cargo bloqueado!\n> O cargo é o mesmo que o meu maior cargo atual!\n";
                        }


                        // CONTINUE:
                        else
                        {
                            desc += $"<:rezet_dgreen:1147164307889586238> {r.Mention}\n> Nenhum risco detectado!\n";
                        }
                    }
                    embed.WithDescription(desc);
                    var button = new DiscordButtonComponent(ButtonStyle.Primary, $"{e.Interaction.User.Id}_PAexit", "Exit", emoji: new DiscordComponentEmoji(id: 1308125206883078225));



                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("BIp bup bip!")
                            .AddEmbed(embed)
                            .AddComponents(button)
                    );
                }
                else if (e.Values[0] == "autoping")
                {

                }
                else if (e.Values[0] == "modlogs")
                {
                    await e.Interaction.DeferAsync();
                    var LogsDict = shard
                            [$"{Guild.Id}"]
                            ["moderation"]
                            ["mod_logs"]
                            .AsBsonDocument.ToDictionary(
                                elem => elem.Name,
                                elem => elem.Value
                            );
                    var embed = new DiscordEmbedBuilder()
                    {
                        Color = new DiscordColor("#7e67ff")
                    };
                    var desc = "## <:rezet_shine:1147373071737573446> Modding Logs!\nCanais dedicados aos registros da moderação do servidor!\n";



                    foreach (var entry in LogsDict)
                    {
                        if (entry.Value != BsonNull.Value)
                        {
                            switch (entry.Key)
                            {
                                case "messages_channel":
                                    desc += $"<:rezet_3_act:1189936284379119726> Messages Management:\n> **Channel**: <#{(ulong)entry.Value.AsInt64}>\n> `Messages Delete`\n> `Messages Modify`\n";
                                    break;
                                case "moderation_channel":
                                    desc += $"<:rezet_3_act:1189936284379119726> Moderation Actions:\n> **Channel**: <#{(ulong)entry.Value.AsInt64}>\n> `Member Kick`\n> `Member Ban / Unban`\n> `Member Timeout / Untimeout`\n> `Member Warn / Unwarn / Warn Modify`\n";
                                    break;
                                case "modified_roles":
                                    desc += $"<:rezet_3_act:1189936284379119726> Roles Management:\n> **Channel**: <#{(ulong)entry.Value.AsInt64}>\n> `Role Add`\n> `Role Remove`\n> `Role Create`\n> `Role Modify`\n> `Role Delete`\n";
                                    break;
                                case "modified_channels":
                                    desc += $"<:rezet_3_act:1189936284379119726> Channels Management:\n> **Channel**: <#{(ulong)entry.Value.AsInt64}>\n> `Channel Create`\n> `Channel Delete`\n> `Channel Modify`\n";
                                    break;
                                case "manage_guild":
                                    desc += $"<:rezet_3_act:1189936284379119726> Guild Management:\n> **Channel**: <#{(ulong)entry.Value.AsInt64}>\n> `Guild Name Change`\n> `Guild Description Change`\n> `Guild Icon Change`\n> `Guild Splash Change`\n> `Guild Banner Change`\n> `Guild Bot Add`\n> `Guild Bot Remove`\n";
                                    break;
                            }
                        }
                        else
                        {
                            switch (entry.Key)
                            {
                                case "messages_channel":
                                    desc += $"<:rezet_3_nact:1189936390113341601> Messages Management:\n> `Uncativated`.\n";
                                    break;
                                case "moderation_channel":
                                    desc += $"<:rezet_3_nact:1189936390113341601> Moderation Actions:\n> `Uncativated`.\n";
                                    break;
                                case "modified_roles":
                                    desc += $"<:rezet_3_nact:1189936390113341601> Roles Management:\n> `Uncativated`.\n";
                                    break;
                                case "modified_channels":
                                    desc += $"<:rezet_3_nact:1189936390113341601> Channels Management:\n> `Uncativated`.\n";
                                    break;
                                case "manage_guild":
                                    desc += $"<:rezet_3_nact:1189936390113341601> Guild Management:\n> `Uncativated`.\n";
                                    break;
                            }
                        }
                    }
                    embed.WithDescription(desc);
                    var button = new DiscordButtonComponent(ButtonStyle.Primary, $"{e.Interaction.User.Id}_PAexit", "Exit", emoji: new DiscordComponentEmoji(id: 1308125206883078225));
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed)
                            .AddComponents(button)
                    );
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}