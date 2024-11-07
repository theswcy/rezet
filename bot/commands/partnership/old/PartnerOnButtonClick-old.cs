using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using Rezet;




public class PartnerDashboard
{
    public static async Task PD_PrimaryButtons(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // EXIT BUTTON:
            if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PAexit")
            {
                await e.Message.DeleteAsync();
            }
            // TUTORIAL BUTTON:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PAtutorial")
            {
                await e.Interaction.DeferAsync();
                var embed = new DiscordEmbedBuilder()
                {
                    Description =
                            "## Tutorial!",
                    Color = new DiscordColor("7e67ff")
                };
                embed.AddField(
                    "<:rezet_shine:1147373071737573446> Basics:",
                    "> - `Role`: Cargo que será dado aos **representantes** de parceria mencionados no convite de parcerias." +
                    "\n> - - [ `Certifique-se de que o cargo não possua permissões especiais.` ]" +
                    "\n> - `Ping`: Cargo de **ping/menção** de uma nova parceria feita." +
                    "\n> - `Channel`: Canal que será feito a detecção de novas parcerias!" +
                    "\n> - `Log channel`: Canal dedicado aos registros das parcerias."
                );
                embed.AddField(
                    "<:rezet_shine:1147373071737573446> Ranking:",
                    "> - Após a ativação da função, os **responsáveis pelas parcerias** serão recompensados com pontos no ranking, cada parceria feita equivale a **1 ponto**." +
                    "\n> -  Comunidades que não possuem a função **ranking** ativada, não terão seus **dados de parcerias** registrados, não ganharão **XP** e nem **patentes novas**." +
                    "\n> -  Você pode desativar essa função sem se preocupar com a **perda dos pontos**, mas caso queira apagar todos os pontos dos usuários, selecione a opção **Reset** na **dashboard**." +
                    "\n> - <:rezet_exclamation:1164417019303702570> [ `Não é possível resetar a quantidade total de parcerias feitas na comunidade manualmente!` ]"
                );
                var embed2 = new DiscordEmbedBuilder()
                {
                    Color = new DiscordColor("7e67ff")
                };
                embed2.AddField(
                    "<:rezet_shine:1147373071737573446> Embed dashboard:",
                    $"> - `Variables`: Menu de variáveis disponíveis. As variáveis são **parâmetros especiais** usados para mostrar atributos ou valores em uma embed! **Exemplo**: o `@[staff.mention]` menciona o funcionário, {e.Interaction.User.Mention}." +
                    "\n> - `Add embed`: Cria uma nova embed que será salva no banco de dados, possibilitanto você de criar e salvar vários tipos de embeds." +
                    "\n> - `Set embed`: Seleciona uma embed que será usada na mensagem resposta do convite de parceria." +
                    "\n> - `Delete embed`: Permite você deletar uma embed." +
                    "\n> - `Preview embed`: Visualize as embeds guardadas no banco de dados!"
                );
                embed2.AddField(
                    "<:rezet_shine:1147373071737573446> Embed builder:",
                    "> -# Nota: parâmetros com esse ícone ( <:rezet_exclamation:1164417019303702570> ) são parâmetros que não permitem **menções**, ao contrário de ( <:rezet_dgreen:1147164307889586238> ) que permitem todas as variáveis e ( <:rezet_dred:1147164215837208686> ) que **não permitem** variáveis.\n⠀" +
                    "\n> - <:rezet_exclamation:1164417019303702570> `Title`: Título da embed. ( max: 256 caracteres )" +
                    "\n> - <:rezet_dgreen:1147164307889586238> `Description`: Descrição da embed. ( max: 4096 caracteres )" +
                    "\n> - <:rezet_exclamation:1164417019303702570> `Footer`: Rodapé da embed. ( max: 2048 caracteres )" +
                    "\n> - <:rezet_dred:1147164215837208686> `Author`: Autor da embed, uma mini imagem da comunidade seguido do nome da comunidade." +
                    "\n> - <:rezet_dred:1147164215837208686> `Image`: Imagem da embed, é permitido somentes URLs **HTTPS**." +
                    "\n> - <:rezet_dred:1147164215837208686> `Thumbnail`: Thumbanil ícone da embed, é permitido somentes URLs **HTTPS**."
                );
                var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");




                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Bip bup bip!")
                        .AddEmbed(embed)
                        .AddEmbed(embed2)
                        .AddComponents(button));
            }
            // UNACTIVATE FUNCTION BUTTON:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PAuna")
            {
                await e.Interaction.DeferAsync();



                var Guild = e.Interaction.Guild;
                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.option", 0);
#pragma warning disable CS8602
                await collection.UpdateOneAsync(shard, update);



                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Coma poeira! A função **partnership** foi desativada!")
                );
                await e.Message.DeleteAsync();
            }
            // ACTIVATE RANKING BUTTON:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_ALB")
            {
                await e.Interaction.DeferAsync();



                var Guild = e.Interaction.Guild;
                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                // DATABASE:
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.leaderboard.option", 1);
#pragma warning disable CS8602
                await collection.UpdateOneAsync(shard, update);



                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Beleza! O módulo **ranking** da função **partnership** foi ativado, agora todos os usuários responsáveis pelas parcerias terão seus pontos contados. Use o comando `/partnership dashboard` novamente!")
                );
                await e.Message.DeleteAsync();
            }
            // ACTIVATE PARTNERSHIP FUNCTION BUTTON:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_APFB")
            {
                await e.Interaction.DeferAsync();
                var shard = Program._databaseService?.GetShard(e.Interaction.Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {e.Interaction.Guild.Name} / {e.Interaction.Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.option", 1);
                await collection.UpdateOneAsync(shard, update);



                var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Nice, a função **partnership** foi ativada!\nQuer adicionar ou modificar alguma embed? Use o comando `/partnership dashboard`!")
                        .AddComponents(button)
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static async Task PD_SelectMenuEmbeds(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // PARTNERSHIP EMBED OPTIONS:
            if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PEOptions")
            {
                var selectedOption = e.Values[0];

                // VARIABLES:
                if (selectedOption == "variables")
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
                        "\n> `@[user.points]`: Total de pontos do funcionário. ( Apenas se o módulo **ranking** estiver ativada. )"
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
                else if (selectedOption == "add")
                {
                    var modal = new DiscordInteractionResponseBuilder()
                    .WithTitle("Embed Builder")
                    .WithCustomId($"{e.Interaction.User.Id}_EBModalName")
                    .AddComponents(new TextInputComponent(
                            "Name:", "name_input", "The name of the embed to save", style: TextInputStyle.Short, max_length: 10
                    ));
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
                }
                // SET EMBED:
                else if (selectedOption == "set")
                {
                    await e.Interaction.DeferAsync();
                    var Guild = e.Guild;



                    var shard = Program._databaseService?.GetShard(Guild, 1);
                    if (shard == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                        Console.ResetColor();
                        return;
                    }



                    var GetPartnerConfigs = shard.GetValue($"{Guild.Id}")?
                            .AsBsonDocument.GetValue("partner")?
                            .AsBsonDocument.GetValue("configs")?
                            .AsBsonDocument.GetValue("embeds")?.AsBsonDocument;
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
                else if (selectedOption == "delete")
                {
                    await e.Interaction.DeferAsync();
                    var Guild = e.Guild;



                    var shard = Program._databaseService?.GetShard(Guild, 1);
                    if (shard == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                        Console.ResetColor();
                        return;
                    }



                    var GetPartnerConfigs = shard.GetValue($"{Guild.Id}")?
                            .AsBsonDocument.GetValue("partner")?
                            .AsBsonDocument.GetValue("configs")?
                            .AsBsonDocument.GetValue("embeds")?.AsBsonDocument;
                    var SelectOptions = new List<DiscordSelectComponentOption>();
                    foreach (var configElement in GetPartnerConfigs.Elements)
                    {
                        var ConfigName = configElement.Name;
                        var customEmoji = new DiscordComponentEmoji("📂");
                        SelectOptions.Add(new DiscordSelectComponentOption(ConfigName, ConfigName, emoji: customEmoji));
                    }
                    var SelectMenu = new DiscordSelectComponent($"{e.Interaction.User.Id}_EDDelete", "Select the embed to delete!", SelectOptions, false);



                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! Selecione a embed!")
                            .AddComponents(SelectMenu)
                            .AddComponents(button));
                }
                // PREVIEW EMBED:
                else if (selectedOption == "preview")
                {
                    await e.Interaction.DeferAsync();
                    var Guild = e.Interaction.Guild;



                    var shard = Program._databaseService?.GetShard(Guild, 1);
                    var value = shard.GetValue($"{Guild.Id}")?
                                .AsBsonDocument.GetValue("partner")?
                                .AsBsonDocument.GetValue("selected").ToString();
                    if (shard == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                        Console.ResetColor();
                        return;
                    }



                    var SelectedEmbed = shard.GetValue($"{Guild.Id}")?
                            .AsBsonDocument.GetValue("partner")?
                            .AsBsonDocument.GetValue("configs")?
                            .AsBsonDocument.GetValue("embeds")?
                            .AsBsonDocument.GetValue(value.ToString());



#pragma warning disable CS8604
                    var embed = new DiscordEmbedBuilder()
                    {
                        Title = SwitchOrResponse.SwitchVariables(
                            SelectedEmbed.AsBsonDocument.GetValue("title").ToString(), e.Interaction
                        ),
                        Description = SwitchOrResponse.SwitchVariables(
                            SelectedEmbed.AsBsonDocument.GetValue("description").ToString(), e.Interaction
                        ),
                        Color = new DiscordColor(SelectedEmbed.AsBsonDocument.GetValue("color").ToString())
                    };
                    embed.WithFooter(
                        SwitchOrResponse.SwitchVariables(
                            SelectedEmbed.AsBsonDocument.GetValue("footer").ToString(), e.Interaction
                        )
                    );
                    if (!string.IsNullOrEmpty(SelectedEmbed.AsBsonDocument.GetValue("image").ToString()))
                    {
                        embed.WithImageUrl(SelectedEmbed.AsBsonDocument.GetValue("image").ToString());
                    }
                    if (SelectedEmbed.AsBsonDocument.GetValue("thumb") != 0)
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
                    if (SelectedEmbed.AsBsonDocument.GetValue("author") != 0)
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
            // SET EMBED:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_EDSet")
            {
                await e.Interaction.DeferAsync();
                var value = e.Values[0];
                var Guild = e.Interaction.Guild;



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                if (shard.GetValue($"{Guild.Id}")?
                        .AsBsonDocument.GetValue("partner")?
                        .AsBsonDocument.GetValue("selected") == value)
                {
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("<:rezet_dred:1147164215837208686> Essa embed já está selecionada!")
                    );
                    await Task.Delay(3000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                    return;
                }



                var SelectedEmbed = shard.GetValue($"{Guild.Id}")?
                        .AsBsonDocument.GetValue("partner")?
                        .AsBsonDocument.GetValue("configs")?
                        .AsBsonDocument.GetValue("embeds")?
                        .AsBsonDocument.GetValue(value.ToString());
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.selected", value);
                await collection.UpdateOneAsync(shard, update);



#pragma warning disable CS8604
                var embed = new DiscordEmbedBuilder()
                {
                    Title = SwitchOrResponse.SwitchVariables(
                        SelectedEmbed.AsBsonDocument.GetValue("title").ToString(), e.Interaction
                    ),
                    Description = SwitchOrResponse.SwitchVariables(
                        SelectedEmbed.AsBsonDocument.GetValue("description").ToString(), e.Interaction
                    ),
                    Color = new DiscordColor(SelectedEmbed.AsBsonDocument.GetValue("color").ToString())
                };
                embed.WithFooter(
                    SwitchOrResponse.SwitchVariables(
                        SelectedEmbed.AsBsonDocument.GetValue("footer").ToString(), e.Interaction
                    )
                );
                if (!string.IsNullOrEmpty(SelectedEmbed.AsBsonDocument.GetValue("image").ToString()))
                {
                    embed.WithImageUrl(SelectedEmbed.AsBsonDocument.GetValue("image").ToString());
                }
                if (SelectedEmbed.AsBsonDocument.GetValue("thumb") != 0)
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
                if (SelectedEmbed.AsBsonDocument.GetValue("author") != 0)
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
                    .WithContent($"Bip bup bip! A embed **{value}** foi selecionada com sucesso! Prévia da embed abaixo:")
                    .AddEmbed(embed)
                    .AddComponents(button)
                );
            }
            // DELETE EMBED:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_EDDelete")
            {
                await e.Interaction.DeferAsync();
                var value = e.Values[0];
                var Guild = e.Interaction.Guild;



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }



                if (shard.GetValue($"{Guild.Id}")?
                        .AsBsonDocument.GetValue("partner")?
                        .AsBsonDocument.GetValue("selected") == value)
                {
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("<:rezet_dred:1147164215837208686> Você não pode deletar uma embed em uso!")
                    );
                    await Task.Delay(3000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                    return;
                }
                if (value == "default")
                {
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("<:rezet_dred:1147164215837208686> Você não pode deletar a embed pedrão!")
                    );
                    await Task.Delay(3000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                    return;
                }




                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Unset($"{Guild.Id}.partner.configs.embeds.{value}");
                await collection.UpdateOneAsync(shard, update);



                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent($"Okay! A embed **{value}** foi **deletada**!")
                );
                await Task.Delay(3000);
                await e.Interaction.DeleteOriginalResponseAsync();
            }
            // PARTNERSHIP RANKING OPTIONS:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PERanking")
            {
                // RESET RANKING:
                if (e.Values[0] == "reset")
                {
                    await e.Interaction.DeferAsync();
                    var Guild = e.Interaction.Guild;



                    var shard = Program._databaseService?.GetShard(Guild, 1);
                    if (shard == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                        Console.ResetColor();
                        return;
                    }
                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.leaderboard.ranking", new BsonDocument { });
                    await collection.UpdateOneAsync(shard, update);



                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! O ranking foi limpo!")
                    );
                    await Task.Delay(3000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                }
                // UNACTIVATE RANKING:
                else if (e.Values[0] == "unactivate")
                {
                    await e.Interaction.DeferAsync();
                    var Guild = e.Interaction.Guild;



                    var shard = Program._databaseService?.GetShard(Guild, 1);
                    if (shard == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                        Console.ResetColor();
                        return;
                    }
                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.leaderboard.option", 0);
                    await collection.UpdateOneAsync(shard, update);



                    await e.Message.DeleteAsync();
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip! O módulo **ranked** da função **partnership** foi desativado! Use o comando `/partnership dashboard` novamente.")
                    );
                    await Task.Delay(3000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                }
                // PATENTS:
                else if (e.Values[0] == "patents")
                {
                    await e.Interaction.DeferAsync();


                    var embed = new DiscordEmbedBuilder()
                    {
                        Color = new DiscordColor("7e67ff")
                    };
                    embed.AddField(
                        "<:rezet_shine:1147373071737573446> Patents:",
                        "> <:rezet_sss_elite:1290676886556377112> **SSS-Elite**⠀/⠀`10000`" +
                        "\n> <:rezet_ss_elite:1290676831544017046> **SS-Elite**⠀/⠀`5000`" +
                        "\n> <:rezet_s_elite:1290676775826886696> **S-Elite**⠀/⠀`2500`" +
                        "\n> <:rezet_a_elite:1290676720537567373> **A-Elite**⠀/⠀`1000`" +
                        "\n> <:rezet_b_elite:1290676630527934567> **B-Elite**⠀/⠀`500`" +
                        "\n> <:rezet_c_elite:1290676392870023190> **C-Elite**⠀/⠀`0`"
                    );
                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");



                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed)
                            .AddComponents(button)
                    );
                }
            }



            // PARTNERSHIP MORE OPTIONS:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_PEConfigs")
            {
                // DELETE ON EVERYONE:
                if (e.Values[0] == "everyone")
                {
                    await e.Interaction.DeferAsync();
                    var value = e.Values[0];
                    var Guild = e.Interaction.Guild;



                    var shard = Program._databaseService?.GetShard(Guild, 1);
                    if (shard == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                        Console.ResetColor();
                        return;
                    }



                    var embed = new DiscordEmbedBuilder()
                    {
                        Description = "<:rezet_exclamation:1164417019303702570> **Atenção**: Essa é um módulo que possui ferramentas de **risco**, selecione um dos modos abaixo pensando **3 vezes** ou mais!",
                        Color = new DiscordColor("ff0000")
                    };
                    embed.AddField(
                        "<:rezet_settings1:1147163366561955932> Mode 1 ( righly recommended )",
                        "> Deletar mensagens de parceria com menções **everyone/here**."
                    );
                    embed.AddField(
                        "<:rezet_settings1:1147163366561955932> Mode 2",
                        "> Deletar mensagens de parceria com menções **everyone/here** e **expulsar** o **representante** no convite."
                    );
                    embed.AddField(
                        "<:rezet_settings1:1147163366561955932> Mode 3",
                        "> Deletar mensagens de parceria com menções **everyone/here** e **banir** o **representante** no convite."
                    );



                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");
                    var button1 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_OSMode1", "Mode 1");
                    var button2 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_OSMode2", "Mode 2");
                    var button3 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_OSMode3", "Mode 3");
                    var button4 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_OSMode0", "Unactivate");



                    if (shard[Guild.Id.ToString()]["partner"]["anti-eh"] == 0)
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed)
                            .AddComponents(button, button1, button2, button3)
                        );
                    }
                    else
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed)
                            .AddComponents(button, button1, button2, button3, button4)
                        );
                    }
                }
                // DELETE ON QUIT:
                else if (e.Values[0] == "delete")
                {
                    await e.Interaction.DeferAsync();
                    var value = e.Values[0];
                    var Guild = e.Interaction.Guild;



                    var shard = Program._databaseService?.GetShard(Guild, 1);
                    if (shard == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                        Console.ResetColor();
                        return;
                    }



                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");
                    var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                    if (shard[Guild.Id.ToString()]["partner"]["anti-qi"] == 0)
                    {
                        var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.anti-qi", 1);
                        await collection.UpdateOneAsync(shard, update);


                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Bip bup bip! Agora, os parceiros que sairem terão seus convites removidos do canal de parcerias!")
                                .AddComponents(button)
                        );
                    }
                    else
                    {
                        var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.anti-qi", 0);
                        await collection.UpdateOneAsync(shard, update);


                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Bip bup bip! Módulo desativado!")
                        );
                        await Task.Delay(2000);
                        await e.Interaction.DeleteOriginalResponseAsync();
                    }
                }
            }
            // MODE 0:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode0")
            {
                await e.Interaction.DeferAsync();
                var Guild = e.Interaction.Guild;



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.anti-eh", 0);
                await collection.UpdateOneAsync(shard, update);



                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Bip bup bip! Módulo desativado!")
                );
                await Task.Delay(3000);
                await e.Interaction.DeleteOriginalResponseAsync();
            }
            // MODE 1:
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode1")
            {
                await e.Interaction.DeferAsync();
                var Guild = e.Interaction.Guild;



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.anti-eh", 1);
                await collection.UpdateOneAsync(shard, update);



                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Bip bup bip! Módulo ativado no **Mode 1**!")
                );
                await Task.Delay(3000);
                await e.Interaction.DeleteOriginalResponseAsync();
            }
            // MODE 2
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode2")
            {
                await e.Interaction.DeferAsync();
                var Guild = e.Interaction.Guild;



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.anti-eh", 2);
                await collection.UpdateOneAsync(shard, update);



                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Bip bup bip! Módulo ativado no **Mode 2**!")
                );
                await Task.Delay(3000);
                await e.Interaction.DeleteOriginalResponseAsync();
            }
            // MODE 3
            else if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_OSMode3")
            {
                await e.Interaction.DeferAsync();
                var Guild = e.Interaction.Guild;



                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.anti-eh", 3);
                await collection.UpdateOneAsync(shard, update);



                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent("Bip bup bip! Módulo ativado no **Mode 3**!")
                );
                await Task.Delay(3000);
                await e.Interaction.DeleteOriginalResponseAsync();
            }



            // EMBED BUILDER:
            else if (e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString() + "_EBMN_"))
            {
                var t = e.Interaction.Data.CustomId.Split('_');



                var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Embed builder")
                .WithCustomId($"{e.Interaction.User.Id}_EBMB_{t[2]}")
                .AddComponents(new TextInputComponent(
                    "Title:", "title_input", "The title of the embed.", style: TextInputStyle.Short
                ))
                .AddComponents(new TextInputComponent(
                    "Description", "desc_input", "The description of the embed.", style: TextInputStyle.Paragraph
                ))
                .AddComponents(new TextInputComponent(
                    "Footer", "foot_input", "The footer of the embed.", style: TextInputStyle.Short
                ))
                .AddComponents(new TextInputComponent(
                    "Color HEX", "color_input", "The color of the embed. [ Only HEX colors, Ex: #7e67ff ]", style: TextInputStyle.Short
                ))
                .AddComponents(new TextInputComponent(
                    "Image", "image_input", "The image of the embed. [ Only HTTPS ]", style: TextInputStyle.Paragraph, required: false
                ));
                await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
            }
            // EMBED BUILDER DELETE:
            else if (e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString() + "_PDDelet"))
            {
                await e.Interaction.DeferAsync();



                var content = e.Interaction.Data.CustomId.Split('_');
                var Guild = e.Interaction.Guild;
                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update.Unset($"{Guild.Id}.partner.configs.embeds.{content[2]}");
                await collection.UpdateOneAsync(shard, update);



                var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent($"Okay! A embed **{content[2]}** foi deletada.")
                        .AddComponents(button)
                );
                var u = await e.Interaction.Channel.GetMessagesAsync(2);
                await e.Interaction.Channel.DeleteMessageAsync(u[1]);
            }
            // EMBED BUILDER ACTIVATE THUMB:
            else if (e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString() + "_PDACTT"))
            {
                await e.Interaction.DeferAsync();
                var p = e.Interaction.Data.CustomId.Split('_');
                var t = p[2];




                var Guild = e.Interaction.Guild;
                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");



                if (shard.GetValue($"{Guild.Id}")
                        .AsBsonDocument.GetValue("partner")?
                        .AsBsonDocument.GetValue("configs")?
                        .AsBsonDocument.GetValue("embeds")?
                        .AsBsonDocument.GetValue(t)?
                        .AsBsonDocument.GetValue("thumb") == 0)
                {
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.configs.embeds.{t}.thumb", 1);
                    await collection.UpdateOneAsync(shard, update);
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Okay! A **thumbnail** da embed foi **ativada**!")
                    );
                    await Task.Delay(4000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                }
                else
                {
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.configs.embeds.{t}.thumb", 0);
                    await collection.UpdateOneAsync(shard, update);
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Okay! A **thumbnail** da embed foi **desativada**!")
                    );
                    await Task.Delay(4000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                }
            }
            // EMBED BUILDER ACTIVATE AUTHOR:
            else if (e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString() + "_PDACTA"))
            {
                await e.Interaction.DeferAsync();
                var p = e.Interaction.Data.CustomId.Split('_');
                var t = p[2];




                var Guild = e.Interaction.Guild;
                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");



                if (shard.GetValue($"{Guild.Id}")
                        .AsBsonDocument.GetValue("partner")?
                        .AsBsonDocument.GetValue("configs")?
                        .AsBsonDocument.GetValue("embeds")?
                        .AsBsonDocument.GetValue(t)?
                        .AsBsonDocument.GetValue("author") == 0)
                {
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.configs.embeds.{t}.author", 1);
                    await collection.UpdateOneAsync(shard, update);
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Okay! O **author** da embed foi **ativada**!")
                    );
                    await Task.Delay(4000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                }
                else
                {
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.configs.embeds.{t}.author", 0);
                    await collection.UpdateOneAsync(shard, update);
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Okay! o **author** da embed foi **desativada**!")
                    );
                    await Task.Delay(4000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static async Task PD_ModalSubmit(DiscordClient sender, ModalSubmitEventArgs e)
    {
        try
        {
            // NAME EMBED BUILDER:
            if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_EBModalName")
            {
                await e.Interaction.DeferAsync();
                var content = e.Values["name_input"];




                var shard = Program._databaseService?.GetShard(e.Interaction.Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {e.Interaction.Guild.Name} / {e.Interaction.Guild.Id})");
                    Console.ResetColor();
                    return;
                }



                var embed = new DiscordEmbedBuilder()
                {
                    Description = "O nome foi salvo com sucesso, quer prosseguir?",
                    Color = new DiscordColor("7e67ff")
                };



                var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                var button1 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_EBMN_{content}", "Build embed");


                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent(content)
                        .AddEmbed(embed)
                        .AddComponents(button, button1)
                );
            }
            // EMBED BUILDER:
            else if (e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString() + "_EBMB"))
            {
                await e.Interaction.DeferAsync();
                var p = e.Interaction.Data.CustomId.Split('_');
                var t = p[2];



                var title = e.Values["title_input"];
                var description = e.Values["desc_input"];
                var footer = e.Values["foot_input"];
                var color = e.Values["color_input"];
                var image = e.Values["image_input"];



                var embed2 = new DiscordEmbedBuilder()
                {
                    Description = "Nice! A embed foi criada e salva no meu **banco de dados**!",
                    Color = new DiscordColor("7e67ff")
                };
                var embed = new DiscordEmbedBuilder()
                {
                    Title = SwitchOrResponse.SwitchVariables(title, e.Interaction),
                    Description = SwitchOrResponse.SwitchVariables(description, e.Interaction),
                };




                // VERIFY COLOR:
                if (!e.Values["color_input"].StartsWith('#'))
                {
                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder()
                    .WithContent($"<:rezet_dred:1147164215837208686> A cor **HEX** precisa começar com `#`!"));
                    await Task.Delay(5000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                    return;
                }
                if (!uint.TryParse(e.Values["color_input"].AsSpan(1), System.Globalization.NumberStyles.HexNumber, null, out uint Color))
                {
                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder()
                    .WithContent($"<:rezet_dred:1147164215837208686> A cor **HEX** fornecida é inválida! Por favor, forneça uma cor válida.\n- Siga o exemplo: `#c67bff`"));
                    await Task.Delay(5000);
                    await e.Interaction.DeleteOriginalResponseAsync();
                    return;
                }
                // VERIFY IMAGE:
                if (!string.IsNullOrEmpty(e.Values["image_input"]))
                {
                    if (e.Values["image_input"].Substring(0, 5) != "https")
                    {
                        await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder()
                            .WithContent($"<:rezet_dred:1147164215837208686> A imagem fornecida é inválida!")
                        );
                        await Task.Delay(5000);
                        await e.Interaction.DeleteOriginalResponseAsync();
                        return;
                    }
                }
                embed.WithColor(new DiscordColor(color.ToString()));



                
                // VERIFY FOOTER:
                if (string.IsNullOrEmpty(e.Values["foot_input"]))
                {
    
                }
                else
                {
                    embed.WithFooter(
                        SwitchOrResponse.SwitchVariables(footer, e.Interaction)
                    );
                }




                // VERIFY IMAGE:
                if (!string.IsNullOrEmpty(image))
                {
                    if (image.Substring(0, 5) != "https")
                    {
                        await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder()
                        .WithContent($"<:rezet_dred:1147164215837208686> A imagem fornecida é inválida!"));
                        return;
                    }
                    else
                    {
                        embed.WithImageUrl(image.ToString());
                    }
                }




                var Guild = e.Interaction.Guild;
                var shard = Program._databaseService?.GetShard(Guild, 1);
                if (shard == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"X [  GUILD PARTNER  ] Failed to acess guild ( {Guild.Name} / {Guild.Id})");
                    Console.ResetColor();
                    return;
                }
                var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
                var content = new BsonDocument
                            {
                                { "title", title },
                                { "description", description },
                                { "footer", footer },
                                { "color", color },
                                { "image", image },
                                { "thumb", 0 },
                                { "author", 0 }
                            };
                var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.configs.embeds.{t}", content);
#pragma warning disable CS8602
                await collection.UpdateOneAsync(shard, update);




                var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                var button1 = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PDDelet_{t}", "Delete embed");
                var button2 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_PDACTT_{t}", "Thumbnail [ Guild icon ]");
                var button3 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_PDACTA_{t}", "Author [ Guild icon / name ]");




                await e.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder()
                        .WithContent($"{t}")
                        .AddEmbed(embed2)
                        .AddEmbed(embed)
                        .AddComponents(button, button1, button2, button3)
                );
                var y = await e.Interaction.Channel.GetMessagesAsync(2);
                await e.Interaction.Channel.DeleteMessageAsync(y[1]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}