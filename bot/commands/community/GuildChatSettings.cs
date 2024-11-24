using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using MongoDB.Driver.Linq;
using System.Threading.Channels;



#pragma warning disable CS8602
[SlashCommandGroup("chat", "Chat Settings")]
public class CommunityChats : ApplicationCommandModule
{
    [SlashCommand("clear", "💭 | Apagar mensagens!")]
    public static async Task Clear(InteractionContext ctx,
        [Option("amount", "Quantidade de mensagens. [ max: 100 ]")] long amount,
        [Option("channel", "Deletar mensagens de um canal específico.")] DiscordChannel? channel = null,
        [Option("member", "Deletar mensagens de um membro específico..")] DiscordUser? member = null
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



            await CheckPermi.CheckMemberPermissions(ctx, 9);
            await CheckPermi.CheckBotPermissions(ctx, 9);




            if (amount > 100)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Máximo **100** mensagens!"));
                return;
            }
            if (amount == 1)
            {
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder()
                        .WithContent($"Mínimo **2** mensagens!"));
                return;
            }




            try
            {
                if (channel != null)
                {
                    await CheckChannelType.CheckType(ctx, 1, channel);
                    await CheckChannelPermissions.CheckMemberPermissions(ctx, 10, channel);
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
                    await CheckChannelType.CheckType(ctx, 1, ctx.Channel);
                    await CheckChannelPermissions.CheckMemberPermissions(ctx, 10, ctx.Channel);
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
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ➜  In: {ctx.Guild.Name} ( {ctx.Guild.Id} )  /  {ex.GetType()}\n    ➜  Used by: {ctx.User.Username} ( {ctx.User.Id} )\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
        }
    }
}