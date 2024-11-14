using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using Rezet;



#pragma warning disable CS8602
[SlashCommandGroup("community", "Community commands")]
public class CommunityCommands : ApplicationCommandModule
{
    [SlashCommand("informations", "📘 | About community.")]
    public static async Task Informations(InteractionContext ctx)
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);



            var Guild = await Program.Rezet.GetGuildAsync(ctx.Guild.Id);
            await Guild.RequestMembersAsync();
            var Members = await Guild.GetAllMembersAsync();
            var Channels = await Guild.GetChannelsAsync();
            var Roles = Guild.Roles;



            var BoostBadge = "<:rezet_globo:1147178426927681646>";
            if (Guild.PremiumSubscriptionCount >= 1){BoostBadge = "<:rezet_sb_0:1171817483779457186>";}
            else if (Guild.PremiumSubscriptionCount >= 2){BoostBadge = "<:rezet_sb_1:1171817556319936583>";}
            else if (Guild.PremiumSubscriptionCount >= 7){BoostBadge = "<:rezet_sb_2:1171817619762991204>";}
            else if (Guild.PremiumSubscriptionCount >= 14){BoostBadge = "<:rezet_sb_3:1171817673852715008>";}
            

            
            var embed = new DiscordEmbedBuilder
            {
                Description = 
                    $"## {BoostBadge} {Guild.Name}\n" +
                    $"- {Guild.Description ?? "<:rezet_dred:1147164215837208686> Essa comunidade não possui descrição."}\n⠀",
                Color = new DiscordColor("#7e67ff")
            };
            if (Guild.IconUrl != null){embed.WithThumbnail(Guild.IconUrl);}
            embed.AddField(
                "<:rezet_creditcard:1147341888538542132> Basics",
                $"> **Owner**: {Guild.Owner.DisplayName} | `{Guild.OwnerId}`" +
                $"\n> **Type**: `Community`."
            );


            
            int onlineMembers = Members.Count(m => m.Presence != null && 
                (m.Presence.Status == UserStatus.Online || 
                 m.Presence.Status == UserStatus.Idle || 
                 m.Presence.Status == UserStatus.DoNotDisturb));
            embed.AddField(
                "<:rezet_globo:1147178426927681646> Members",
                $"> <:rezet_dgreen:1147164307889586238> **Online**: {onlineMembers}." +
                $"\n> <:rezet_dred:1147164215837208686> **Offline**: {Members.Count - onlineMembers}." +
                $"\n> <:rezet_shine:1147373071737573446> **Bots**: {Members.Count(m => m.IsBot)}." +
                $"\n> <:rezet_globo:1147178426927681646> **Total**: {Members.Count}."
            );


            
            embed.AddField(
                "<:rezet_connect:1147907330378309652> Channels",
                $"> **Text**: {Channels.Count(c => c.Type == ChannelType.Text)}." + 
                $"\n> **Voice** {Channels.Count(c => c.Type == ChannelType.Voice)}." +
                $"\n> **Forum**: {Channels.Count(c => c.Type == ChannelType.GuildForum)}." +
                $"\n> **Announcement**: {Channels.Count(c => c.Type == ChannelType.News)}." +
                $"\n> **Category**: {Channels.Count(c => c.Type == ChannelType.Category)}."
            );



            embed.AddField(
                "<:rezet_share:1147165266887856209> Roles",
                $"> **Roles**: {Roles.Count}"
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






    [SlashCommand("description", "📘 | Change the community's description!")]
    public async Task description(InteractionContext ctx,
    [Option("description", "The new description for the community!")] string description
    )
    {
        try
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            var Guild = await Program.Rezet.GetGuildAsync(ctx.Guild.Id);
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
                    .WithContent($"Ops! Não consegui fazer as alterações na descrição da comunidade. Verifique minhas permissões!"));
        }
    }
}