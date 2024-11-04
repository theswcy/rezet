using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;




#pragma warning disable CS8602
public class ModeratorDashboard
{
    public static async Task MD_Primary(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        // AUTOMATIC ACTIONS:
        if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_AAOpti")
        {
            var Guild = e.Guild;
            var shard = Program._databaseService?.GetShard(Guild, 1);
            if (shard == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                Console.ResetColor();
                return;
            }
            if (e.Values[0] == "autorole")
            {
                await e.Interaction.DeferAsync();
                var Member = await e.Guild.GetMemberAsync(e.User.Id);
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
                var desc = "## <:rezet_shine:1147373071737573446> Autorole!\nEsses aqui são os cargos que eu darei quando algum usuário novo entrar na comunidade!\n⠀\n";




                foreach (var entry in RoleDict)
                {
                    var r = Guild.GetRole((ulong)entry.Value);
                    var botMember = await e.Guild.GetMemberAsync(Program.Rezet.CurrentUser.Id);
                    var highestBotRole = botMember.Roles.OrderByDescending(r => r.Position).FirstOrDefault();

                    // PERMISSIONS:
                    if (r.Permissions.HasPermission(Permissions.Administrator))
                    {
                        desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Administrator`.\n⠀\n";
                    }
                    else if (r.Permissions.HasPermission(Permissions.ManageGuild))
                    {
                        desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Manage Guild`.\n⠀\n";
                    }
                    else if (r.Permissions.HasPermission(Permissions.ManageChannels))
                    {
                        desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Manage Channels`.\n⠀\n";
                    }
                    else if (r.Permissions.HasPermission(Permissions.ManageRoles))
                    {
                        desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Manage Roles`.\n⠀\n";
                    }
                    else if (r.Permissions.HasPermission(Permissions.ManageWebhooks))
                    {
                        desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Manage Webhooks`.\n⠀\n";
                    }
                    else if (r.Permissions.HasPermission(Permissions.BanMembers))
                    {
                        desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Ban Members`.\n⠀\n";
                    }
                    else if (r.Permissions.HasPermission(Permissions.KickMembers))
                    {
                        desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Kick Members`.\n⠀\n";
                    }
                    else if (r.Permissions.HasPermission(Permissions.MuteMembers))
                    {
                        desc += $"<:rezet_exclamation:1164417019303702570> {r.Mention}\n> Risco detectado!\n> O cargo possui a permissão `Mute Members`.\n⠀\n";
                    }


                    // HIGHEST ROLE:
                    else if (r.Position > highestBotRole.Position)
                    {
                        desc += $"<:rezet_dred:1147164215837208686> {r.Mention}\n> Cargo bloqueado!\n> O cargo é maior que o meu maior cargo atual!\n⠀\n";
                    }
                    else if (r.Position == highestBotRole.Position)
                    {
                        desc += $"<:rezet_dred:1147164215837208686> {r.Mention}\n> Cargo bloqueado!\n> O cargo é o mesmo que o meu maior cargo atual!\n⠀\n";
                    }


                    // CONTINUE:
                    else
                    {
                        desc += $"<:rezet_dgreen:1147164307889586238> {r.Mention}\n> Nenhum risco detectado!\n⠀\n";
                    }
                }
                embed.WithDescription(desc);
                var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");



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

            }
        }
    }
}