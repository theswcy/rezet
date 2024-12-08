using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using RezetSharp;




#pragma warning disable CS8602
[SlashCommandGroup("partners", "Ticket setup.")]
public class PartnershipTickets : ApplicationCommandModule
{
    [SlashCommandGroup("ticket", "Ticket setup.")]
    public class Tickets : ApplicationCommandModule
    {
        [SlashCommand("setup", "🔖 | Configurar e enviar ticket de parcerias.")]
        public static async Task TicketSetup(InteractionContext ctx,
            [Option("channel", "The channel of the ticket.")] DiscordChannel Channel,
            [Option("category", "The ID of the category for the tickets. [ ⚠️ ONLY categories!! ]")] DiscordChannel Category,
            [Option("role", "The support role for the ticket.")] DiscordRole SupportRole,
            [Option("logs", "The logs channel of the ticket.")] DiscordChannel LogsChannel
        )
        {
            try
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);




                // MANAGE CHANNELS - MANAGE ROLES:
                await CheckPermi.CheckMemberPermissions(ctx, 3);
                await CheckPermi.CheckBotPermissions(ctx, 3);
                await CheckPermi.CheckMemberPermissions(ctx, 4);
                await CheckPermi.CheckBotPermissions(ctx, 4);




                if (Channel.IsCategory)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Ei! na opção **channel** você precisa selecionar um **canal** de texto!")
                    );
                    return;
                }
                if (!Category.IsCategory)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Ei! na opção **category** você precisa selecionar uma **categoria**, não um canal de texto!")
                    );
                    return;
                }
                if (LogsChannel.IsCategory)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent("Ei! na opção **logs** você precisa selecionar um **canal** de texto!")
                    );
                    return;
                }



                var Herrscher = EngineV8X.HerrscherRazor.GetHerrscherDocument(ctx.Guild);
                var SelectedEmbed = Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["embed_1"].AsString;



                // BUTTON BUILDER:
                var p = ButtonStyle.Primary;
                if (Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["button"]["color"].AsString == "gray")
                {
                    p = ButtonStyle.Secondary;
                }
                else if (Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["button"]["color"].AsString == "green")
                {
                    p = ButtonStyle.Success;
                }
                else if (Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["button"]["color"].AsString == "red")
                {
                    p = ButtonStyle.Danger;
                }
                var button = new DiscordButtonComponent(p, $"{ctx.Guild.Id}_TicketB", $"{Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["button"]["text"].AsString}");





                // TICKET EMBED BUILDER:
                var TicketBuilder = new DiscordMessageBuilder();
                TicketBuilder.AddComponents(button);
                if (SelectedEmbed.Contains('+'))
                {
                    var SeEmbed = SelectedEmbed.Split('+');
                    int de = 0;
                    foreach (var EmbedEntry in SeEmbed)
                    {
                        string NativeEmbed = SeEmbed[de];
                        var EmbedBox = Herrscher[$"{ctx.Guild.Id}"]["partner"]["req_embeds"][NativeEmbed].AsBsonDocument;



                        var EmbedBuilder = new DiscordEmbedBuilder();
                        int ty = 0;
                        EmbedBuilder.WithColor(new DiscordColor(EmbedBox["color"].AsString));
                        if (EmbedBox["title"].IsString) { EmbedBuilder.WithTitle(EmbedBox["title"].AsString); ty++; }
                        if (EmbedBox["description"].IsString) { EmbedBuilder.WithDescription(EmbedBox["description"].AsString); ty++; }
                        if (EmbedBox["footer"].IsString) { EmbedBuilder.WithFooter(EmbedBox["footer"].AsString); ty++; }
                        if (EmbedBox["image"].IsString) { EmbedBuilder.WithImageUrl(EmbedBox["image"].AsString); ty++; }
                        if (EmbedBox["thumb"].IsInt32)
                        {
                            if (EmbedBox["thumb"].AsInt32 == 1)
                            {
                                if (ctx.Guild.IconUrl != null) { EmbedBuilder.WithThumbnail(ctx.Guild.IconUrl); ty++; }
                            }
                        }
                        else if (EmbedBox["thumb"].IsString) { EmbedBuilder.WithThumbnail(EmbedBox["thumb"].AsString); ty++; }
                        if (EmbedBox["author"].AsInt32 == 1)
                        {
                            if (ctx.Guild.IconUrl != null)
                            {
                                EmbedBuilder.WithAuthor(name: ctx.Guild.Name, iconUrl: ctx.Guild.IconUrl); ty++;
                            }
                        }



                        if (ty > 1) { TicketBuilder.AddEmbed(EmbedBuilder); }
                        de++;
                    }
                }
                else
                {
                    var EmbedBox = Herrscher[$"{ctx.Guild.Id}"]["partner"]["ticket"]["req_embeds"][SelectedEmbed].AsBsonDocument;



                    var EmbedBuilder = new DiscordEmbedBuilder();
                    int ty = 0;
                    EmbedBuilder.WithColor(new DiscordColor(EmbedBox["color"].AsString));
                    if (EmbedBox["title"].IsString) { EmbedBuilder.WithTitle(EmbedBox["title"].AsString); ty++; }
                    if (EmbedBox["description"].IsString) { EmbedBuilder.WithDescription(EmbedBox["description"].AsString); ty++; }
                    if (EmbedBox["footer"].IsString) { EmbedBuilder.WithFooter(EmbedBox["footer"].AsString); ty++; }
                    if (EmbedBox["image"].IsString) { EmbedBuilder.WithImageUrl(EmbedBox["image"].AsString); ty++; }
                    if (EmbedBox["thumb"].IsInt32)
                    {
                        if (EmbedBox["thumb"].AsInt32 == 1)
                        {
                            if (ctx.Guild.IconUrl != null) { EmbedBuilder.WithThumbnail(ctx.Guild.IconUrl); ty++; }
                        }
                    }
                    else if (EmbedBox["thumb"].IsString) { EmbedBuilder.WithThumbnail(EmbedBox["thumb"].AsString); ty++; }
                    if (EmbedBox["author"].AsInt32 == 1)
                    {
                        if (ctx.Guild.IconUrl != null)
                        {
                            EmbedBuilder.WithAuthor(name: ctx.Guild.Name, iconUrl: ctx.Guild.IconUrl); ty++;
                        }
                    }



                    if (ty > 1) { TicketBuilder.AddEmbed(EmbedBuilder); }
                }



                // UPDATE DATABASE:
                var collection = EngineV8X.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
                var update = Builders<BsonDocument>.Update
                    .Set($"{ctx.Guild.Id}.partner.ticket.configs.channel", Channel.Id)
                    .Set($"{ctx.Guild.Id}.partner.ticket.configs.support", SupportRole.Id)
                    .Set($"{ctx.Guild.Id}.partner.ticket.configs.category", Category.Id)
                    .Set($"{ctx.Guild.Id}.partner.ticket.configs.logs", LogsChannel.Id);
                await collection.UpdateOneAsync(Herrscher, update);



                // COMMAND EMBED BUILDER:
                await Channel.SendMessageAsync(
                    builder: TicketBuilder
                );
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Jóia, o **ticket** foi enviado com sucesso no canal {Channel.Mention}.")
                );
            }
            catch (Exception ex)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Falha ao executar o comando, verifique minhas permissões!")
                );
                Console.WriteLine($"    ➜  Slash Command: /partners ticket setup\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
                return;
            }
        }
    }
}