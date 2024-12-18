using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using RezetSharp;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;



#pragma warning disable CS8602
public class CommunityCommands_prefix : BaseCommandModule
{
    [Command("servidor")]
    [Aliases("server", "sv", "guild", "g", "community")]
    public async Task GuildCommands(CommandContext ctx, string CMD)
    {
        try
        {
            if (CMD == "stats")
            {
                var Members = await ctx.Guild.GetAllMembersAsync();
                double onlineMembers = Members.Count(m => m.Presence != null && 
                (m.Presence.Status == UserStatus.Online || 
                 m.Presence.Status == UserStatus.Idle || 
                 m.Presence.Status == UserStatus.DoNotDisturb));

                var embed = new DiscordEmbedBuilder()
                {
                    Description = "## <:rezet_plant:1308125160577962004> Guild Stats!",
                    Color = new DiscordColor("#7e67ff")
                };
                int OnMe = (int)Math.Round(onlineMembers / Members.Count * 100); 
                int OfMe = (int)Math.Round((Members.Count - onlineMembers) / Members.Count  * 100);
                embed.AddField(
                    "<:rezet_settings1:1147163366561955932> Porcentagem:",
                    $"> <:rezet_globo:1147178426927681646> **Total**: `{Members.Count}`" +
                    $"\n> <:rezet_dgreen:1147164307889586238> **Online**: `{onlineMembers}` ╺╸ **{OnMe}%**" +
                    $"\n> <:rezet_dred:1147164215837208686> **Offline**: `{Members.Count - onlineMembers}` ╺╸ **{OfMe}%**"
                );
                double per = onlineMembers / Members.Count * 100;
                int GraphCount = (int)Math.Round(per / 10);
                string ONM = "<:rezet_dgreen:1147164307889586238>";
                string OFM = "<:rezet_dred:1147164215837208686>";
                string Graph = "";
                for (int i = 0; i < 10; i++)
                {
                    if (i < GraphCount) {Graph += ONM; }
                    else { Graph += OFM; }
                }
                embed.AddField(
                    "<:rezet_channels:1308125117875752961> Gráfico:",
                    $"\n> {Graph}"
                );



                await ctx.RespondAsync(
                    builder: new DiscordMessageBuilder().WithContent("Bip bup bip").AddEmbed(embed)
                );
            }
        }
        catch (Exception ex)
        {
            await ctx.RespondAsync($"Falha ao executar o comando, verifique as configurações do servidor ou contate o meu suporte!");
            Console.WriteLine($"    ➜  Prefix Command: ban\n    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            return;
        }
    }
}






[SlashCommandGroup("community", "Community commands")]
public class CommunityCommands : ApplicationCommandModule
{
    [SlashCommand("informations", "📘 | Sobre a comunidade.")]
    public static async Task Informations(InteractionContext ctx)
    {
        try
        {
            await ctx.DeferAsync();



            var Guild = await EngineV8X.RezetRazor.GetGuildAsync(ctx.Guild.Id);
            await Guild.RequestMembersAsync();
            var Members = await Guild.GetAllMembersAsync();
            var Channels = await Guild.GetChannelsAsync();
            var Roles = Guild.Roles;


 
            int bl = 0;
            var bc = Guild.PremiumSubscriptionCount;
            string BoostBadge = "<:rezet_globo:1147178426927681646>";
            if (bc == 1) { BoostBadge = "<:rezet_sb_0:1171817483779457186>"; }
            else if (bc >= 2 && bc < 7) { BoostBadge = "<:rezet_sb_1:1171817556319936583>"; bl = 1; }
            else if (bc >= 7 && bc < 14) { BoostBadge = "<:rezet_sb_2:1171817619762991204>"; bl = 2; }
            else if (bc >= 14) { BoostBadge = "<:rezet_sb_3:1171817673852715008>"; bl = 3; }
            

            
            var embed = new DiscordEmbedBuilder
            {
                Description = 
                    $"## {BoostBadge} {Guild.Name}\n" +
                    $"- {Guild.Description ?? "<:rezet_dred:1147164215837208686> Essa comunidade não possui descrição."}\n⠀",
                Color = new DiscordColor("#7e67ff")
            };
            if (Guild.IconUrl != null) { embed.WithThumbnail(Guild.IconUrl); }
            if (Guild.SplashUrl != null) { embed.WithImageUrl(Guild.SplashUrl); }
            else if (Guild.BannerUrl != null) { embed.WithImageUrl(Guild.BannerUrl); }
            embed.AddField(
                "<:rezet_creditcard:1147341888538542132> Basics",
                $"> **Owner**: {Guild.Owner.DisplayName} | `{Guild.OwnerId}`" +
                $"\n> **Type**: `Community`"
            );
            embed.AddField(
                "<:rezet_colornitrod:1164416963121008701> Boost",
                $"> **Boost role**: {(Guild.Roles.Values.FirstOrDefault(r => r.Tags.IsPremiumSubscriber) != null ? Guild.Roles.Values.FirstOrDefault(r => r.Tags.IsPremiumSubscriber).Mention : "**Uncativated**.")}" +
                $"\n> **Boosts**: `{Guild.PremiumSubscriptionCount}`" +
                $"\n> **Level**: `{bl}`"
            );


            
            int onlineMembers = Members.Count(m => m.Presence != null && 
                (m.Presence.Status == UserStatus.Online || 
                 m.Presence.Status == UserStatus.Idle || 
                 m.Presence.Status == UserStatus.DoNotDisturb));
            embed.AddField(
                "<:rezet_globo:1147178426927681646> Members",
                $"> <:rezet_dgreen:1147164307889586238> `{onlineMembers}` **Online**." +
                $"\n> <:rezet_dred:1147164215837208686> `{Members.Count - onlineMembers}` **Offline**." +
                $"\n> <:rezet_shine:1147373071737573446> `{Members.Count(m => m.IsBot)}` **Bots**." +
                $"\n> <:rezet_globo:1147178426927681646> `{Members.Count}` **Total**."
            );


            
            embed.AddField(
                "<:rezet_connect:1147907330378309652> Channels",
                $"> `{Channels.Count(c => c.Type == ChannelType.Text)}` **Text**." + 
                $"\n> `{Channels.Count(c => c.Type == ChannelType.Voice)}` **Voice**." +
                $"\n> `{Channels.Count(c => c.Type == ChannelType.GuildForum)}` **Forum**." +
                $"\n> `{Channels.Count(c => c.Type == ChannelType.Stage)}` **Stage**." +
                $"\n> `{Channels.Count(c => c.Type == ChannelType.Category)}` **Category**." +
                $"\n> `{Channels.Count(c => c.Type == ChannelType.PublicThread)}` **Public Threads**." +
                $"\n> `{Channels.Count(c => c.Type == ChannelType.PrivateThread)}` **Private Threads**." +
                $"\n> `{Channels.Count(c => c.Type == ChannelType.News)}` **Announcement**." 
            );


            int z = 0;
            foreach (var entry in Roles) { if (entry.Value.IsManaged) {z++;} }
            embed.AddField(
                "<:rezet_share:1147165266887856209> Roles",
                $"> `{Roles.Count}` **Roles**." +
                $"\n> `{z}` **Managed**."
            );



            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent("Bip bup bip!")
                    .AddEmbed(embed));
        }
        catch (Exception ex)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando.\n\n> `{ex.Message}`"));
        }
    }






    [SlashCommand("description", "📘 | Alterar descrição da comunidade!")]
    public async Task description(InteractionContext ctx,
    [Option("description", "Nova descrição!")] string description
    )
    {
        try
        {
            await ctx.DeferAsync();
            var Guild = await EngineV8X.RezetRazor.GetGuildAsync(ctx.Guild.Id);
            var Author = ctx.Member;




            await CheckPermi.CheckMemberPermissions(ctx, 2);
            await CheckPermi.CheckBotPermissions(ctx, 2);




            if (description.Length > 120)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent($"Máximo `120` caracteres!"));
                return;
            }



            
            try
            {
                await Guild.ModifyAsync(p => p.Description = description);

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Nice! A descrição da comunidade foi alterada com sucesso!\nNova descrição:\n> `{description}`"));
            }
            catch (Exception)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Ops! Não consegui fazer as alterações na descrição da comunidade. Verifique minhas permissões!"));
            }
        }
        catch (Exception)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"Ops! Não consegui fazer as alterações na descrição da comunidade. Verifique as configurações!"));
        }
    }
}