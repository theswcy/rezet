using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using RezetSharp;




#pragma warning disable CS8604
#pragma warning disable CS8602
public class PartnershipEmbedConfigs
{
    // ========== PRIMARY:
    public static async Task DashboardSelectMenu(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // ========== PARTNERSHIP EMBED OPTIONS:
            if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_PEOptions")
            {
                // VARIABLES:
                if (e.Values[0] == "variables")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();
                        var embed = new DiscordEmbedBuilder()
                        {
                            Description = "-# Nota: As variáveis ainda estão em testes, podem ser atualizadas a qualquer momento!",
                            Color = new DiscordColor("7e67ff")
                        };
                        embed.AddField(
                            "<:rezet_shine:1147368423475658882> Staff:",
                            "> `@[staff.name]`: Nome do funcionário." +
                            "\n> `@[staff.mention]`: Menção do funcionário. ( Disponível apenas na descrição da embed )" +
                            "\n> `@[staff.id]`: Id do funcionário." +
                            "\n>  ( <:rezet_exclamation:1164417019303702570> Variáveis abaixo apenas se o módulo **ranking** estiver ativado. )" +
                            "\n> `@[staff.points]`: Total de pontos do funcionário." +
                            "\n> `@[staff.rank]`: Total de pontos do funcionário."
                        );
                        embed.AddField(
                            "<:rezet_shine:1147368423475658882> Guild:",
                            "> `@[guild.name]`: Nome da comunidade." +
                            "\n> `@[guild.id]`: Id da comunidade." +
                            "\n> `@[guild.owner]`: Nome do **dono** da comunidade."
                        );
                        embed.AddField(
                            "<:rezet_shine:1147368423475658882> Partner:",
                            "> `@[partner.name]`: Nome da comunidade parceira." +
                            "\n> `@[rep.name]`: Nome do representante." +
                            "\n> `@[rep.id]`: Id do representante." +
                            "\n> `@[rep.mention]`: Mencionar o representante."
                        );
                        var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Exit");



                        await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Bip bup bip!")
                            .AddEmbed(embed)
                            .AddComponents(button));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Variables\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // ADD EMBED:
                else if (e.Values[0] == "add")
                {
                    try
                    {
                        var modal = new DiscordInteractionResponseBuilder()
                        .WithTitle("Embed Builder")
                        .WithCustomId($"{e.Interaction.User.Id}_EBModalName")
                        .AddComponents(new TextInputComponent(
                                "Name:", "name_input", "The name of the embed to save", style: TextInputStyle.Short, max_length: 10
                        ));
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Add Embed\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // EDIT EMBED:
                else if (e.Values[0] == "edit")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();


                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                        var SelectedEmbed = Herrscher[$"{e.Guild.Id}"]
                                ["partner"]
                                ["configs"]
                                ["embeds"]
                                .AsBsonDocument;
                        var SelectOptions = new List<DiscordSelectComponentOption>();
                        if (SelectedEmbed.Count() == 1)
                        {
                            await e.Interaction.CreateFollowupMessageAsync(
                                new DiscordFollowupMessageBuilder()
                                    .WithContent("Ops, nenhuma embed personalizada foi adicionada ao banco de dados!")
                            );
                            return;
                        }
                        foreach (var configElement in SelectedEmbed.Elements)
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
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Edit Embed\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // SET EMBED:
                else if (e.Values[0] == "set")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();


                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                        var SelectedEmbed = Herrscher[$"{e.Guild.Id}"]
                                ["partner"]
                                ["configs"]
                                ["embeds"].AsBsonDocument;
                        var SelectOptions = new List<DiscordSelectComponentOption>();
                        foreach (var configElement in SelectedEmbed.Elements)
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
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  partnership Set Embed\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // DELETE EMBED:
                else if (e.Values[0] == "delete")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();


                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);



                        var SelectedEmbed = Herrscher[$"{e.Guild.Id}"]
                                ["partner"]
                                ["configs"]
                                ["embeds"].AsBsonDocument;
                        var SelectOptions = new List<DiscordSelectComponentOption>();
                        if (SelectedEmbed.Count() == 1)
                        {
                            await e.Interaction.CreateFollowupMessageAsync(
                                new DiscordFollowupMessageBuilder()
                                    .WithContent("Ops, nenhuma embed personalizada foi adicionada ao banco de dados!")
                            );
                            return;
                        }
                        foreach (var configElement in SelectedEmbed.Elements)
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
                                .AsEphemeral(true)
                                .AddComponents(SelectMenu)
                                .AddComponents(button));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Delete Embed\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // PREVIEW EMBED:
                else if (e.Values[0] == "preview")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();



                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                        var value = Herrscher[$"{e.Guild.Id}"]
                                    ["partner"]
                                    ["selected"].ToString();



                        var SelectedEmbed = Herrscher[$"{e.Guild.Id}"]
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



                        // VERIFY THUMBNAIL:
                        if (SelectedEmbed["thumb"].IsInt32)
                        {
                            if (SelectedEmbed["thumb"] == 1 && e.Guild.IconUrl != null) { embed.WithThumbnail(e.Guild.IconUrl); }
                        }
                        else if (SelectedEmbed["thumb"].IsString)
                        {
                            embed.WithThumbnail(SelectedEmbed["thumb"].AsString);
                        }



                        // VERIFY FOOTER:
                        if (!SelectedEmbed["footer"].IsInt32)
                        {
                            embed.WithFooter(SelectedEmbed["footer"].AsString);
                        }



                        // VERIFY AUTHOR:
                        if (!SelectedEmbed["author"].IsInt32)
                        {
                            if (e.Guild.IconUrl != null) { embed.WithAuthor(name: e.Guild.Name, iconUrl: e.Guild.IconUrl); }
                        }



                        // VERIFY IMAGE:
                        if (!SelectedEmbed["image"].IsString)
                        {
                            embed.WithImageUrl(SelectedEmbed["image"].AsString);
                        }



                        var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                            .WithContent($"Bip bup bip!")
                            .AddEmbed(embed)
                            .AddComponents(button)
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Preview Embed\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
            }
            // ========== EDIT EMBED:
            else if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_EDEdit")
            {
                await PartnershipEmbedEdit(sender, e, e.Values[0]);
                await Task.Delay(120000);
                var msgs = await e.Interaction.Channel.GetMessagesAsync(30);
                var msgg = await e.Interaction.Channel.GetMessageAsync(msgs.FirstOrDefault(m => m.Content.Contains(e.Values[0]) && m.Author.Id == EngineV8X.RezetRazor.CurrentUser.Id).Id);
                try
                {
                    await msgg.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent("Editor de embed fechado!"),
                        suppressEmbeds: true
                    );
                }
                catch (Exception)
                {
                    return;
                }
            }
            // ========== SET EMBED:
            else if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_EDSet")
            {
                try
                {
                    await e.Interaction.DeferAsync(ephemeral: true);
                    var value = e.Values[0];



                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                    if (Herrscher[$"{e.Guild.Id}"]
                            ["partner"]
                            ["selected"] == value)
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Essa embed já está selecionada!")
                                .AsEphemeral(true)
                        );
                        return;
                    }



                    var SelectedEmbed = Herrscher[$"{e.Guild.Id}"]
                            ["partner"]
                            ["configs"]
                            ["embeds"]
                            [value.ToString()];
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Guild.Id}.partner.selected", value);
                    await collection.UpdateOneAsync(Herrscher, update);



                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                        .WithContent($"Bip bup bip! A embed **{value}** foi selecionada com sucesso!")
                        .AsEphemeral(true)
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Set Embed\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== DELETE EMBED:
            else if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_EDDelete")
            {
                try
                {
                    await e.Interaction.DeferAsync(ephemeral: true);
                    var value = e.Values[0];



                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);



                    if (Herrscher[$"{e.Guild.Id}"]
                            ["partner"]
                            ["selected"] == value)
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("<Você não pode deletar uma embed em uso!")
                                .AsEphemeral(true)
                        );
                        return;
                    }
                    if (value == "default")
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Você não pode deletar a embed pedrão!")
                                .AsEphemeral(true)
                        );
                        return;
                    }




                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Unset($"{e.Guild.Id}.partner.configs.embeds.{value}");
                    await collection.UpdateOneAsync(Herrscher, update);



                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent($"Okay! A embed **{value}** foi **deletada**!")
                            .AsEphemeral(true)
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Delete Embed\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Dashboard Select Menu\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
    // ========== BUILDER INTERACTION:
    public static async Task PartershipEmbedBuilder(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // ========== EMBED BULDER:
            if (e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString() + "_EBMN_"))
            {
                try
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
                        "Footer", "foot_input", "The footer of the embed.", style: TextInputStyle.Short, required: false
                    ))
                    .AddComponents(new TextInputComponent(
                        "Color HEX", "color_input", "The color of the embed. [ Only HEX colors, Ex: #7e67ff ]", style: TextInputStyle.Short
                    ))
                    .AddComponents(new TextInputComponent(
                        "Image", "image_input", "The image of the embed. [ Only HTTPS ]", style: TextInputStyle.Paragraph, required: false
                    ));
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Embed Builder Modal Send\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== EMBED BUILDER DELETE:
            else if (e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString() + "_PDDelet"))
            {
                try
                {
                    var content = e.Interaction.Data.CustomId.Split('_');
                    var Guild = e.Interaction.Guild;
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Unset($"{Guild.Id}.partner.configs.embeds.{content[2]}");
                    await collection.UpdateOneAsync(Herrscher, update);



                    var button = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PAexit", "Close");
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent($"Okay! A embed **{content[2]}** foi deletada.")
                    );
                    var u = await e.Interaction.Channel.GetMessagesAsync(2);
                    await e.Interaction.Channel.DeleteMessageAsync(u[1]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Embed Builder Delete\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== EMBED BUILDER EDIT:
            else if (e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString() + "_PDEditT_"))
            {
                var t = e.Interaction.Data.CustomId.Split('_');
                await PartnershipEmbedEdit(sender, e, t[2]);
                await Task.Delay(120000);
                var msgs = await e.Interaction.Channel.GetMessagesAsync(30);
                var msgg = await e.Interaction.Channel.GetMessageAsync(msgs.FirstOrDefault(m => m.Content.Contains(t[2]) && m.Author.Id == EngineV8X.RezetRazor.CurrentUser.Id).Id);
                try
                {
                    await msgg.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent("Editor de embed fechado!"),
                        suppressEmbeds: true
                    );
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Embed Builder Create\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
    // ========== BUILDER MODAL:
    public static async Task PartnershipEmbedBulderModal(DiscordClient sender, ModalSubmitEventArgs e)
    {
        try
        {
            // ========== EMBED BUILDER NAME:
            if (e.Interaction.Data.CustomId == e.Interaction.User.Id.ToString() + "_EBModalName")
            {
                try
                {
                    await e.Interaction.DeferAsync();
                    var content = e.Values["name_input"];
                    if (content.Length > 7)
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent("Ops! O nome da embed deve ter no máximo **7 caracteres**!")
                        );
                        return;
                    }




                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);



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
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Embed Builder Name\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== EMBED BUILDER MODAL:
            else if (e.Interaction.Data.CustomId.Contains(e.Interaction.User.Id.ToString() + "_EBMB"))
            {
                try
                {
                    await e.Interaction.DeferAsync();
                    var p = e.Interaction.Data.CustomId.Split('_');
                    var t = p[2];



                    var title = e.Values["title_input"];
                    var description = e.Values["desc_input"];
                    var footer = e.Values["foot_input"];
                    dynamic footer2 = 0;
                    var color = e.Values["color_input"];
                    var image = e.Values["image_input"];
                    dynamic image2 = 0;



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
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent($"A cor **HEX** precisa começar com `#`!")
                                .AsEphemeral(true)
                            );
                        return;
                    }
                    if (!uint.TryParse(e.Values["color_input"].AsSpan(1), System.Globalization.NumberStyles.HexNumber, null, out uint Color))
                    {
                        await e.Interaction.CreateFollowupMessageAsync(
                            new DiscordFollowupMessageBuilder()
                                .WithContent($"A cor **HEX** fornecida é inválida! Por favor, forneça uma cor válida.\n- Siga o exemplo: `#c67bff`")
                                .AsEphemeral(true)
                            );
                        return;
                    }
                    embed.WithColor(new DiscordColor(color.ToString()));




                    // VERIFY FOOTER:
                    if (string.IsNullOrEmpty(e.Values["foot_input"]))
                    {
                        footer2 = 0;
                    }
                    else
                    {
                        embed.WithFooter(
                            SwitchOrResponse.SwitchVariables(footer, e.Interaction)
                        );
                        footer2 = footer;
                    }




                    // VERIFY IMAGE:
                    if (!string.IsNullOrEmpty(image))
                    {
                        if (image[..5] != "https")
                        {
                            await e.Interaction.CreateFollowupMessageAsync(
                                new DiscordFollowupMessageBuilder()
                                    .WithContent($"A imagem fornecida é inválida!")
                                    .AsEphemeral(true)
                                );
                            return;
                        }
                        else
                        {
                            embed.WithImageUrl(image.ToString());
                            image2 = image;
                        }
                    }
                    else
                    {
                        image2 = 0;
                    }




                    var Guild = e.Interaction.Guild;
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var content = new BsonDocument
                            {
                                { "title", title },
                                { "description", description },
                                { "footer", footer2 },
                                { "color", color },
                                { "image", image2 },
                                { "thumb", 0 },
                                { "author", 0 }
                            };
                    var update = Builders<BsonDocument>.Update.Set($"{Guild.Id}.partner.configs.embeds.{t}", content);
#pragma warning disable CS8602
                    await collection.UpdateOneAsync(Herrscher, update);




                    var button = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_PAexit", "Close");
                    var button1 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_PDEditT_{t}", "Edit embed");
                    var button2 = new DiscordButtonComponent(ButtonStyle.Danger, $"{e.Interaction.User.Id}_PDDelet_{t}", "Delete embed");




                    // await e.Interaction.CreateFollowupMessageAsync(
                    //     new DiscordFollowupMessageBuilder()
                    //         .WithContent($"{t}")
                    //         .AddEmbed(embed2)
                    //         .AddEmbed(embed)
                    //         .AddComponents(button, button1, button2)
                    // );



                    var msgs = await e.Interaction.Channel.GetMessagesAsync(30);
                    var msgg = await e.Interaction.Channel.GetMessageAsync(msgs.FirstOrDefault(m => m.Content.Contains(t) && m.Author.Id == EngineV8X.RezetRazor.CurrentUser.Id).Id);
                    await msgg.ModifyAsync(
                        builder: new DiscordMessageBuilder()
                            .WithContent($"{t}")
                            .AddEmbed(embed2)
                            .AddEmbed(embed)
                            .AddComponents(button, button1, button2)
                    );
                    var u = await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Embed criada!")
                    );
                    await Task.Delay(2000);
                    await u.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Moderator Dashboard Select Menu\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Embed Builder Modal\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
    // ========== EMBED EDIT:
    public static async Task PartnershipEmbedEdit(DiscordClient sender, ComponentInteractionCreateEventArgs e, string name)
    {
        try
        {
            await e.Interaction.DeferAsync();
            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
            var SelectedEmbed = Herrscher[$"{e.Guild.Id}"]
                    ["partner"]
                    ["configs"]
                    ["embeds"]
                    [name]
                    .AsBsonDocument;



            var embed = new DiscordEmbedBuilder()
            {
                Title = $"{SwitchOrResponse.SwitchVariables(SelectedEmbed["title"].AsString, e.Interaction)}",
                Description = $"{SwitchOrResponse.SwitchVariables(SelectedEmbed["description"].AsString, e.Interaction)}",
                Color = new DiscordColor(SelectedEmbed["color"].ToString())
            };



            // VERIFY THUMBNAIL:
            if (SelectedEmbed["thumb"].IsInt32)
            {
                if (SelectedEmbed["thumb"] == 1 && e.Guild.IconUrl != null) { embed.WithThumbnail(e.Guild.IconUrl); }
            }
            else if (SelectedEmbed["thumb"].IsString)
            {
                embed.WithThumbnail(SelectedEmbed["thumb"].AsString);
            }



            // VERIFY FOOTER:
            if (!SelectedEmbed["footer"].IsInt32)
            {
                embed.WithFooter(SwitchOrResponse.SwitchVariables(SelectedEmbed["footer"].AsString, e.Interaction));
            }



            // VERIFY AUTHOR:
            if (SelectedEmbed["author"] != 0)
            {
                if (e.Guild.IconUrl != null) { embed.WithAuthor(name: e.Guild.Name, iconUrl: e.Guild.IconUrl); }
            }



            // VERIFY IMAGE:
            if (SelectedEmbed["image"].IsString)
            {
                embed.WithImageUrl(SelectedEmbed["image"].AsString);
            }



            // BUILDER SELECT MENU:
            var emoji = new DiscordComponentEmoji("📝");
            var OptionsToEditEmbed = new[]
                {
                    new DiscordSelectComponentOption("Title", "title", "Edit the title.", emoji: emoji),
                    new DiscordSelectComponentOption("Description", "desc", "Edit the description.", emoji: emoji),
                    new DiscordSelectComponentOption("Footer", "footer", "Edit the footer.", emoji: emoji),
                    new DiscordSelectComponentOption("Author", "author", "Add an author.", emoji: emoji),
                };
            var OptionsToEditEmbed1 = new[]
                {
                    new DiscordSelectComponentOption("Color", "color", "Edit the color.", emoji: emoji),
                    new DiscordSelectComponentOption("Image", "image", "Edit the Image.", emoji: emoji),
                    new DiscordSelectComponentOption("Thumbnail", "thumb", "Edit the thumbnail.", emoji: emoji),
                };
            var OptionsToEditEmbedSelectMenu1 = new DiscordSelectComponent($"{e.Interaction.User.Id}_EB2_{name}", "More options to edit the Embed!", OptionsToEditEmbed1);
            var OptionsToEditEmbedSelectMenu = new DiscordSelectComponent($"{e.Interaction.User.Id}_EB_{name}", "Options to edit the Embed!", OptionsToEditEmbed);




            await e.Message.ModifyAsync(
                builder: new DiscordMessageBuilder()
                    .WithContent($"{name}")
                    .AddEmbed(embed)
                    .AddComponents(OptionsToEditEmbedSelectMenu)
                    .AddComponents(OptionsToEditEmbedSelectMenu1)
            );
            await e.Interaction.DeleteOriginalResponseAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Embed Edit\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
    // ========== EMBED EDIT FOR MODAL:
    public static async Task PartnershipEmbedEditForModal(DiscordClient sender, ModalSubmitEventArgs e, string name)
    {
        try
        {
            var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
            var SelectedEmbed = Herrscher[$"{e.Interaction.Guild.Id}"]
                    ["partner"]
                    ["configs"]
                    ["embeds"]
                    [name]
                    .AsBsonDocument;



            var embed = new DiscordEmbedBuilder()
            {
                Title = $"{SwitchOrResponse.SwitchVariables(SelectedEmbed["title"].AsString, e.Interaction)}",
                Description = $"{SwitchOrResponse.SwitchVariables(SelectedEmbed["description"].AsString, e.Interaction)}",
                Color = new DiscordColor(SelectedEmbed["color"].ToString())
            };



            // VERIFY THUMBNAIL:
            if (SelectedEmbed["thumb"].IsInt32)
            {
                if (SelectedEmbed["thumb"] == 1 && e.Interaction.Guild.IconUrl != null) { embed.WithThumbnail(e.Interaction.Guild.IconUrl); }
            }
            else if (SelectedEmbed["thumb"].IsString)
            {
                embed.WithThumbnail(SelectedEmbed["thumb"].AsString);
            }



            // VERIFY FOOTER:
            if (!SelectedEmbed["footer"].IsInt32)
            {
                embed.WithFooter(SwitchOrResponse.SwitchVariables(SelectedEmbed["footer"].AsString, e.Interaction));
            }



            // VERIFY AUTHOR:
            if (SelectedEmbed["author"] != 0)
            {
                if (e.Interaction.Guild.IconUrl != null) { embed.WithAuthor(name: e.Interaction.Guild.Name, iconUrl: e.Interaction.Guild.IconUrl); }
            }



            // VERIFY IMAGE:
            if (SelectedEmbed["image"].IsString)
            {
                embed.WithImageUrl(SelectedEmbed["image"].AsString);
            }



            // BUILDER SELECT MENU:
            var emoji = new DiscordComponentEmoji("📝");
            var OptionsToEditEmbed = new[]
                {
                    new DiscordSelectComponentOption("Title", "title", "Edit the title.", emoji: emoji),
                    new DiscordSelectComponentOption("Description", "desc", "Edit the description.", emoji: emoji),
                    new DiscordSelectComponentOption("Footer", "footer", "Edit the footer.", emoji: emoji),
                    new DiscordSelectComponentOption("Author", "author", "Add an author.", emoji: emoji),
                };
            var OptionsToEditEmbed1 = new[]
                {
                    new DiscordSelectComponentOption("Color", "color", "Edit the color.", emoji: emoji),
                    new DiscordSelectComponentOption("Image", "image", "Edit the Image.", emoji: emoji),
                    new DiscordSelectComponentOption("Thumbnail", "thumb", "Edit the thumbnail.", emoji: emoji),
                };
            var OptionsToEditEmbedSelectMenu1 = new DiscordSelectComponent($"{e.Interaction.User.Id}_EB2_{name}", "More options to edit the Embed!", OptionsToEditEmbed1);
            var OptionsToEditEmbedSelectMenu = new DiscordSelectComponent($"{e.Interaction.User.Id}_EB_{name}", "Options to edit the Embed!", OptionsToEditEmbed);




            var msgs = await e.Interaction.Channel.GetMessagesAsync(30);
            var msgg = await e.Interaction.Channel.GetMessageAsync(msgs.FirstOrDefault(m => m.Content.Contains(name) && m.Author.Id == EngineV8X.RezetRazor.CurrentUser.Id).Id);
            await msgg.ModifyAsync(
                builder: new DiscordMessageBuilder()
                    .WithContent($"{name}")
                    .AddEmbed(embed)
                    .AddComponents(OptionsToEditEmbedSelectMenu)
                    .AddComponents(OptionsToEditEmbedSelectMenu1)
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Embed Edit For Modal\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
    // ========== EMBED EDIT FUNCTIONS:
    public static async Task PartnershipEmbedEditGo(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        try
        {
            // ========== SAVE THE EMBED:
            if (e.Interaction.Data.CustomId == $"{e.Interaction.User.Id}" + "_PAS")
            {
                try
                {
                    await e.Interaction.DeferAsync(true);
                    await e.Message.DeleteAsync();
                    await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Embed salva!")
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Embed Save\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }




            // ========== EMBED EDIT - BUILDER 1:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EB_"))
            {
                // TITLE:
                if (e.Values[0] == "title")
                {
                    try
                    {
                        var t = e.Interaction.Data.CustomId.Split('_');

                        var modal = new DiscordInteractionResponseBuilder()
                        .WithTitle($"Edit: [ {t[2]} ]")
                        .WithCustomId($"{e.Interaction.User.Id}_EE-T_{t[2]}")
                        .AddComponents(new TextInputComponent(
                            "Title:", "title_input", "The embed's title.", style: TextInputStyle.Short
                        ));
                        await e.Interaction.CreateResponseAsync(
                            InteractionResponseType.Modal, modal
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Edit Embed Title\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // DESCRIPTION:
                else if (e.Values[0] == "desc")
                {
                    try
                    {
                        var t = e.Interaction.Data.CustomId.Split('_');

                        var modal = new DiscordInteractionResponseBuilder()
                        .WithTitle($"Edit: [ {t[2]} ]")
                        .WithCustomId($"{e.Interaction.User.Id}_EE-D_{t[2]}")
                        .AddComponents(new TextInputComponent(
                            "Description:", "desc_input", "The embed's description.", style: TextInputStyle.Paragraph
                        ));
                        await e.Interaction.CreateResponseAsync(
                            InteractionResponseType.Modal, modal
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Edit Embed Description\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // FOOTER:
                else if (e.Values[0] == "footer")
                {
                    try
                    {
                        var t = e.Interaction.Data.CustomId.Split('_');

                        var modal = new DiscordInteractionResponseBuilder()
                        .WithTitle($"Edit: [ {t[2]} ]")
                        .WithCustomId($"{e.Interaction.User.Id}_EE-F_{t[2]}")
                        .AddComponents(new TextInputComponent(
                            "Footer:", "footer_input", "The embed's footer.", style: TextInputStyle.Short, required: false
                        ));
                        await e.Interaction.CreateResponseAsync(
                            InteractionResponseType.Modal, modal
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Edit Embed Footer\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // AUTHOR:
                else if (e.Values[0] == "author")
                {
                    try
                    {
                        var t = e.Interaction.Data.CustomId.Split('_');
                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        if (Herrscher[$"{e.Guild.Id}"]["partner"]["configs"]["embeds"][$"{t[2]}"]["author"] == 0)
                        {
                            var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t[2]}.author", 1);
                            await collection.UpdateOneAsync(Herrscher, update);
                            await PartnershipEmbedEdit(sender, e, t[2]);
                        }
                        else
                        {
                            var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t[2]}.author", 0);
                            await collection.UpdateOneAsync(Herrscher, update);
                            await PartnershipEmbedEdit(sender, e, t[2]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Edit Embed Author\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
            }
            // ========== EMBED EDIT - BUILDER 2:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EB2_"))
            {
                // COLOR:
                if (e.Values[0] == "color")
                {
                    try
                    {
                        var t = e.Interaction.Data.CustomId.Split('_');


                        var modal = new DiscordInteractionResponseBuilder()
                        .WithTitle($"Edit: [ {t[2]} ]")
                        .WithCustomId($"{e.Interaction.User.Id}_EE-C_{t[2]}")
                        .AddComponents(new TextInputComponent(
                            "Color:", "color_input", "The embed's color.", style: TextInputStyle.Short
                        ));
                        await e.Interaction.CreateResponseAsync(
                            InteractionResponseType.Modal, modal
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Edit Embed Color\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // IMAGE:
                else if (e.Values[0] == "image")
                {
                    try
                    {
                        var t = e.Interaction.Data.CustomId.Split('_');


                        var modal = new DiscordInteractionResponseBuilder()
                        .WithTitle($"Edit: [ {t[2]} ]")
                        .WithCustomId($"{e.Interaction.User.Id}_EE-I_{t[2]}")
                        .AddComponents(new TextInputComponent(
                            "Image:", "image_input", "The embed's image.", style: TextInputStyle.Paragraph, required: false
                        ));
                        await e.Interaction.CreateResponseAsync(
                            InteractionResponseType.Modal, modal
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Edit Embed Image\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
                // THUMBNAIL:
                else if (e.Values[0] == "thumb")
                {
                    try
                    {
                        await e.Interaction.DeferAsync();
                        var t = e.Interaction.Data.CustomId.Split('_');
                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Guild);
                        var y = Herrscher[$"{e.Guild.Id}"]["partner"]["configs"]["embeds"][$"{t[2]}"]["thumb"];


                        var button1 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_EE-1_{t[2]}", "Unactivate");
                        var button2 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_EE-2_{t[2]}", "Use community icon");
                        var button3 = new DiscordButtonComponent(ButtonStyle.Secondary, $"{e.Interaction.User.Id}_EE-3_{t[2]}", "Personalize");
                        if (y.IsString)
                        {
                            await e.Interaction.CreateFollowupMessageAsync(
                                new DiscordFollowupMessageBuilder()
                                    .WithContent("Hoyo! Selecione uma opção.")
                                    .AddComponents(button1, button2, button3)
                            );
                        }
                        else if (y.IsInt32)
                        {
                            if (y == 1)
                            {
                                await e.Interaction.CreateFollowupMessageAsync(
                                new DiscordFollowupMessageBuilder()
                                    .WithContent("Hoyo! Selecione uma opção.")
                                    .AddComponents(button1, button2, button3)
                                );
                            }
                            else
                            {
                                await e.Interaction.CreateFollowupMessageAsync(
                                new DiscordFollowupMessageBuilder()
                                    .WithContent("Hoyo! Selecione uma opção.")
                                    .AddComponents(button2, button3)
                                );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ➜  Partnership Embed Edit Thumbnail\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                    }
                }
            }




            // ========== EMBED EDIT THUMBNAIL - UNACTIVATE:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EE-1_"))
            {
                try
                {
                    var t = e.Interaction.Data.CustomId.Split('_');
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t[2]}.thumb", 0);
                    await collection.UpdateOneAsync(Herrscher, update);
                    await PartnershipEmbedEdit(sender, e, t[2]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Edit Embed Thumbnail Unactivate\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== EMBED EDIT THUMBNAIL - USE COMMUNITY ICON:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EE-2_"))
            {
                try
                {
                    var t = e.Interaction.Data.CustomId.Split('_');
                    if (e.Guild.IconUrl == null)
                    {
                        await e.Interaction.DeferAsync(true);
                        await e.Interaction.EditOriginalResponseAsync(
                            new DiscordWebhookBuilder()
                                .WithContent("Ei! A comunidade não tem icon.")
                        );
                        return;
                    }
                    else
                    {
                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t[2]}.thumb", 1);
                        await collection.UpdateOneAsync(Herrscher, update);
                        await PartnershipEmbedEdit(sender, e, t[2]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Edit Embed Thumbnail - Use Community\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== EMBED EDIT THUMBNAIL - PERSONALIZE:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EE-3_"))
            {
                try
                {
                    var t = e.Interaction.Data.CustomId.Split('_');


                    var modal = new DiscordInteractionResponseBuilder()
                        .WithTitle($"Edit: [ {t[2]} ]")
                        .WithCustomId($"{e.Interaction.User.Id}_EE-Y_{t[2]}")
                        .AddComponents(new TextInputComponent(
                            "Thumbnail:", "thumb_input", "Only HTTPS.", style: TextInputStyle.Paragraph
                        ));
                    await e.Interaction.CreateResponseAsync(
                        InteractionResponseType.Modal, modal
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Embed Edit Thumbnail - Personalize\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Edit Embed Functions\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.User.Username} ( {e.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
    // ========== EMBED EDIT MODAL:
    public static async Task PartnershipEmbedEditModal(DiscordClient sender, ModalSubmitEventArgs e)
    {
        try
        {
            // ========== EMBED EDIT - TITLE:
            if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EE-T_"))
            {
                try
                {
                    await e.Interaction.DeferAsync();
                    var j = await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Modificando...")
                    );
                    var p = e.Interaction.Data.CustomId.Split('_');
                    var t = p[2];



                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t}.title", e.Values["title_input"]);
                    await collection.UpdateOneAsync(Herrscher, update);
                    await PartnershipEmbedEditForModal(sender, e, t);



                    await j.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Edit Embed Title - Modal\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== EMBED EDIT - DESCRIPTION:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EE-D_"))
            {
                try
                {
                    await e.Interaction.DeferAsync();
                    var j = await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Modificando...")
                    );
                    var p = e.Interaction.Data.CustomId.Split('_');
                    var t = p[2];



                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t}.description", e.Values["desc_input"]);
                    await collection.UpdateOneAsync(Herrscher, update);
                    await PartnershipEmbedEditForModal(sender, e, t);



                    await j.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Edit Embed Description - Modal\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== EMBED EDIT - FOOTER:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EE-F_"))
            {
                try
                {
                    await e.Interaction.DeferAsync();
                    var j = await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Modificando...")
                    );
                    var p = e.Interaction.Data.CustomId.Split('_');
                    var t = p[2];



                    if (!string.IsNullOrEmpty(e.Values["footer_input"]))
                    {
                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t}.footer", e.Values["footer_input"]);
                        await collection.UpdateOneAsync(Herrscher, update);
                    }
                    else
                    {
                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t}.footer", 0);
                        await collection.UpdateOneAsync(Herrscher, update);
                    }
                    await PartnershipEmbedEditForModal(sender, e, t);



                    await j.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Edit Embed Footer - Modal\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== EMBED EDIT - COLOR:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EE-C_"))
            {
                try
                {
                    await e.Interaction.DeferAsync();
                    var j = await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Modificando...")
                    );
                    if (!e.Values["color_input"].StartsWith('#'))
                    {
                        await j.ModifyAsync(
                            builder: new DiscordMessageBuilder()
                                .WithContent($"A cor **HEX** precisa começar com `#`!")
                            );
                        return;
                    }
                    if (!uint.TryParse(e.Values["color_input"].AsSpan(1), System.Globalization.NumberStyles.HexNumber, null, out uint Color))
                    {
                        await j.ModifyAsync(
                            builder: new DiscordMessageBuilder()
                                .WithContent($"A cor **HEX** fornecida é inválida! Por favor, forneça uma cor válida.\n- Siga o exemplo: `#c67bff`")
                            );
                        return;
                    }



                    var p = e.Interaction.Data.CustomId.Split('_');
                    var t = p[2];
                    var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                    var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                    var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t}.color", e.Values["color_input"]);
                    await collection.UpdateOneAsync(Herrscher, update);
                    await PartnershipEmbedEditForModal(sender, e, t);



                    await j.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Edit Embed Color - Modal\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // =========== EMBED EDIT - IMAGE:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EE-I_"))
            {
                try
                {
                    var p = e.Interaction.Data.CustomId.Split('_');
                    var t = p[2];
                    await e.Interaction.DeferAsync();
                    var j = await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Modificando...")
                    );



                    if (!string.IsNullOrEmpty(e.Values["image_input"]))
                    {
                        if (e.Values["image_input"][..5] != "https")
                        {
                            await j.ModifyAsync(
                                builder: new DiscordMessageBuilder()
                                    .WithContent($"A imagem fornecida é inválida!")
                            );
                            return;
                        }
                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t}.image", e.Values["image_input"]);
                        await collection.UpdateOneAsync(Herrscher, update);
                        await PartnershipEmbedEditForModal(sender, e, t);



                        await j.DeleteAsync();
                    }
                    else
                    {
                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t}.image", 0);
                        await collection.UpdateOneAsync(Herrscher, update);
                        await PartnershipEmbedEditForModal(sender, e, t);



                        await j.DeleteAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Edit Embed Image - Modal\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
            // ========== EMBED EDIT - THUMBNAIL:
            else if (e.Interaction.Data.CustomId.Contains($"{e.Interaction.User.Id}_EE-Y_"))
            {
                try
                {
                    var p = e.Interaction.Data.CustomId.Split('_');
                    var t = p[2];
                    await e.Interaction.DeferAsync();
                    var j = await e.Interaction.CreateFollowupMessageAsync(
                        new DiscordFollowupMessageBuilder()
                            .WithContent("Modificando...")
                    );



                    if (!string.IsNullOrEmpty(e.Values["thumb_input"]))
                    {
                        if (e.Values["thumb_input"][..5] != "https")
                        {
                            await j.ModifyAsync(
                                builder: new DiscordMessageBuilder()
                                    .WithContent($"A imagem fornecida é inválida!")
                            );
                            return;
                        }
                        var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(e.Interaction.Guild);
                        var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                        var update = Builders<BsonDocument>.Update.Set($"{e.Interaction.Guild.Id}.partner.configs.embeds.{t}.thumb", e.Values["thumb_input"]);
                        await collection.UpdateOneAsync(Herrscher, update);
                        await PartnershipEmbedEditForModal(sender, e, t);



                        var msgs = await e.Interaction.Channel.GetMessagesAsync(30);
                        var msgg = await e.Interaction.Channel.GetMessageAsync(msgs.FirstOrDefault(m => m.Content.Contains("Hoyo!") && m.Author.Id == EngineV8X.RezetRazor.CurrentUser.Id).Id);
                        await msgg.DeleteAsync();



                        await j.DeleteAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ➜  Partnership Edit Embed Thumbnail - Modal\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  Partnership Edit Embed - Modal\n    ➜  In: {e.Interaction.Guild.Name} ( {e.Interaction.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {e.Interaction.User.Username} ( {e.Interaction.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}