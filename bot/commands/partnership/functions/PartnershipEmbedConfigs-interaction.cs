using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;




#pragma warning disable CS8604
#pragma warning disable CS8602
public class PartnershipEmbedConfigs
{
    public static async Task DashboardSelectMenu(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // PARTNERSHIP EMBED OPTIONS:
            if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_PEOptions")
            {
                // VARIABLES:
                if (e.Values[0] == "variables")
                {
                    await e.Interaction.DeferAsync();
                    var embed = new DiscordEmbedBuilder()
                    {
                        Description = "-# Nota: As variáveis ainda estão em testes, podem ser atualizadas a qualquer momento!",
                        Color = new DiscordColor("7e67ff")
                    };
                    embed.AddField(
                        "<:rezet_shine:1147368423475658882> Staff variables",
                        "> `@[staff.name]`: Nome do funcionário." +
                        "\n> `@[staff.mention]`: Menção do funcionário. ( Disponível apenas na descrição da embed )" +
                        "\n> `@[staff.id]`: Id do funcionário." +
                        "\n> `@[staff.points]`: Total de pontos do funcionário. ( Apenas se o módulo **ranking** estiver ativada. )"
                    );
                    embed.AddField(
                        "<:rezet_shine:1147368423475658882> Guild variables",
                        "> `@[guild.name]`: Nome da comunidade." +
                        "\n> `@[guild.id]`: Id da comunidade." +
                        "\n> `@[guild.owner]`: Nome do **dono** da comunidade."
                    );
                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");



                    await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Bip bup bip!")
                        .AddEmbed(embed)
                        .AddComponents(button));
                }
                // ADD EMBED:
                else if (e.Values[0] == "add")
                {
                    var modal = new DiscordInteractionResponseBuilder()
                    .WithTitle("Embed Builder")
                    .WithCustomId($"{e.Interaction.User.Id}_EBModalName")
                    .AddComponents(new TextInputComponent(
                            "Name:", "name_input", "The name of the embed to save", style: TextInputStyle.Short, max_length: 10
                    ));
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
                }
                // EDIT EMBED:
                else if (e.Values[0] == "edit")
                {
                    await e.Interaction.DeferAsync();


                    var shard = Program._databaseService?.GetShard(e.Guild, 1);
                    var GetPartnerConfigs = shard[$"{e.Guild.Id}"]
                            ["partner"]
                            ["configs"]
                            ["embeds"]
                            .AsBsonDocument;
                    var SelectOptions = new List<DiscordSelectComponentOption>();
                    if (GetPartnerConfigs.Count() == 1)
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Ops, nenhuma embed personalizada foi adicionada ao banco de dados!")
                        );
                        return;
                    }
                    foreach (var configElement in GetPartnerConfigs.Elements)
                    {
                        var customEmoji = new DiscordComponentEmoji("📂");
                        if (configElement.Name != "default")
                        {
                            SelectOptions.Add(new DiscordSelectComponentOption(configElement.Name, configElement.Name, emoji: customEmoji));
                        }
                    }
                    var SelectMenu = new DiscordSelectComponent($"{e.Interaction.User.Id}_EDEdit", "Select the embed to edit!", SelectOptions, false);
                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");


                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! Selecione a embed!")
                            .AddComponents(SelectMenu)
                            .AddComponents(button)
                    );
                }
                // SET EMBED:
                else if (e.Values[0] == "set")
                {
                    await e.Interaction.DeferAsync();


                    var shard = Program._databaseService?.GetShard(e.Guild, 1);
                    var GetPartnerConfigs = shard[$"{e.Guild.Id}"]
                            ["partner"]
                            ["configs"]
                            ["embeds"].AsBsonDocument;
                    var SelectOptions = new List<DiscordSelectComponentOption>();
                    foreach (var configElement in GetPartnerConfigs.Elements)
                    {
                        var ConfigName = configElement.Name;
                        var customEmoji = new DiscordComponentEmoji("📂");
                        SelectOptions.Add(new DiscordSelectComponentOption(ConfigName, ConfigName, emoji: customEmoji));
                    }
                    var SelectMenu = new DiscordSelectComponent($"{e.Interaction.User.Id}_EDSet", "Select the embed to set!", SelectOptions, false);


                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! Selecione a embed!")
                            .AddComponents(SelectMenu)
                            .AddComponents(button)
                    );
                }
                // DELETE EMBED:
                else if (e.Values[0] == "delete")
                {
                    await e.Interaction.DeferAsync();


                    var shard = Program._databaseService?.GetShard(e.Guild, 1);



                    var GetPartnerConfigs = shard[$"{e.Guild.Id}"]
                            ["partner"]
                            ["configs"]
                            ["embeds"].AsBsonDocument;
                    var SelectOptions = new List<DiscordSelectComponentOption>();
                    if (GetPartnerConfigs.Count() == 1)
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Ops, nenhuma embed personalizada foi adicionada ao banco de dados!")
                        );
                        return;
                    }
                    foreach (var configElement in GetPartnerConfigs.Elements)
                    {
                        var customEmoji = new DiscordComponentEmoji("📂");
                        if (configElement.Name != "default")
                        {
                            SelectOptions.Add(new DiscordSelectComponentOption(configElement.Name, configElement.Name, emoji: customEmoji));
                        }
                    }
                    var SelectMenu = new DiscordSelectComponent($"{e.Interaction.User.Id}_EDDelete", "Select the embed to delete!", SelectOptions, false);



                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! Selecione a embed!")
                            .AddComponents(SelectMenu)
                            .AddComponents(button));
                }
                // IMPORT EMBED:
                else if (e.Values[0] == "import")
                {

                }
                // PREVIEW EMBED:
                else if (e.Values[0] == "preview")
                {
                    await e.Interaction.DeferAsync();
                    var Guild = e.Interaction.Guild;



                    var shard = Program._databaseService?.GetShard(Guild, 1);
                    var value = shard[$"{Guild.Id}"]
                                ["partner"]
                                ["selected"].ToString();
                    if (shard == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                        Console.ResetColor();
                        return;
                    }



                    var SelectedEmbed = shard[$"{Guild.Id}"]
                            ["partner"]
                            ["configs"]
                            ["embeds"]
                            [value.ToString()];



                    var embed = new DiscordEmbedBuilder()
                    {
                        Title = SwitchOrResponse.SwitchVariables(
                            SelectedEmbed["title"].ToString(), e.Interaction
                        ),
                        Description = SwitchOrResponse.SwitchVariables(
                            SelectedEmbed["description"].ToString(), e.Interaction
                        ),
                        Color = new DiscordColor(SelectedEmbed["color"].ToString())
                    };
                    embed.WithFooter(
                        SwitchOrResponse.SwitchVariables(
                            SelectedEmbed["footer"].ToString(), e.Interaction
                        )
                    );
                    if (!string.IsNullOrEmpty(SelectedEmbed["image"].ToString()))
                    {
                        embed.WithImageUrl(SelectedEmbed["image"].ToString());
                    }
                    if (SelectedEmbed["thumb"] != 0)
                    {
                        if (Guild.IconUrl != null)
                        {
                            embed.WithThumbnail(Guild.IconUrl);
                        }
                        else
                        {
                            embed.WithThumbnail(Program.Rezet.CurrentUser.AvatarUrl);
                        }
                    }
                    if (SelectedEmbed["author"] != 0)
                    {
                        if (Guild.IconUrl != null)
                        {
                            embed.WithAuthor(Guild.Name, iconUrl: Guild.IconUrl);
                        }
                        else
                        {
                            embed.WithAuthor(Guild.Name);
                        }
                    }



                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                        .WithContent($"Bip bup bip!")
                        .AddEmbed(embed)
                        .AddComponents(button)
                    );
                }
            }
            // ERROR:
            else if (!e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString()))
            {
                await e.Interaction.DeferAsync(ephemeral: true);
                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .AsEphemeral(true)
                        .WithContent("Ops, você não pode interferir nos comandos dos outros!")
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}