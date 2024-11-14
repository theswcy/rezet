using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using MongoDB.Driver.Linq;
using System.Threading.Channels;



#pragma warning disable CS8602
[SlashCommandGroup("chat", "Chat Settings")]
public class CommunityChats : ApplicationCommandModule
{
    [SlashCommand("clear", "💭 | Clear the chat!")]
    public static async Task Clear(InteractionContext ctx,
        [Option("amount", "Amount of messages. [ max: 100 ]")] long amount,
        [Option("channel", "In a specific chat.")] DiscordChannel? channel = null,
        [Option("member", "From a specific member.")] DiscordUser? member = null
    )
    {
        try
        {
            await ctx.CreateResponseAsync(
                InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AsEphemeral(true)
            );
            var Guild = ctx.Guild;
            var Author = ctx.Member;



            if (!channel.IsCategory)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent("Ops! Você deve selecionar um canal de **texto**!")
                );
                return;
            }




            await CheckPermi.CheckMemberPermissions(ctx, 9);
            await CheckPermi.CheckBotPermissions(ctx, 9);




            if (amount > 100)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent($"Máximo **100** mensagens!"));
                return;
            }
            if (amount == 1)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent($"Mínimo **2** mensagens!"));
                return;
            }




            try
            {
                if (channel != null)
                {
                    if (member != null)
                    {
                        var messages = await channel.GetMessagesAsync(100);
                        var userMessages = messages.Where(m => m.Author.Id == member.Id).Take((int)amount).ToList();
                        await channel.DeleteMessagesAsync(userMessages);
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                            .WithContent($"**{(int)amount}** mensagens do membro {member.Mention} foram deletadas no canal {channel.Mention}!"));
                    }
                    else
                    {
                        var messages = await channel.GetMessagesAsync((int)amount);
                        await channel.DeleteMessagesAsync(messages);
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                            .WithContent($"**{(int)amount}** mensagens foram deletadas no canal {channel.Mention}!"));
                    }
                }
                else
                {
                    if (member != null)
                    {
                        var messages = await ctx.Channel.GetMessagesAsync(100);
                        var userMessages = messages.Where(m => m.Author.Id == member.Id).Take((int)amount).ToList();
                        await ctx.Channel.DeleteMessagesAsync(userMessages);
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                            .WithContent($"**{(int)amount}** mensagens do membro {member.Mention} foram deletadas neste canal!"));
                    }
                    else
                    {
                        var messages = await ctx.Channel.GetMessagesAsync((int)amount);
                        await ctx.Channel.DeleteMessagesAsync(messages);
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                            .WithContent($"**{(int)amount}** mensagens foram deletadas neste canal!"));
                    }
                }
            }
            catch (Exception)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent($"Falha ao executar o comando. Verifique minhas permissões!"));
                return;
            }
        }
        catch (Exception)
        {
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent($"Falha ao executar o comando. Verifique minhas permissões!"));
            return;
        }
    }
}