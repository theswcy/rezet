using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;




// ANOTAÇÕES:
//   > Apenas os tipos principas.

// RESUMO:
//   > Check do tipo do canal:
//        > (  1 ) IS TEXT.
//        > (  2 ) IS CATEGORY.
//        > (  3 ) IS GROUP.
//        > (  4 ) IS PRIVATE.
//        > (  5 ) IS FÓRUM.
//        > (  6 ) IS STAGE.
//        > (  7 ) IS VOICE.



public static class CheckChannelType
{
    public static async Task CheckType(InteractionContext ctx, int Type, DiscordChannel Channel)
    {
        switch(Type)
        {
            case 1: // TEXT:
                if (Channel.Type != ChannelType.Text || Channel.Type != ChannelType.News)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"{Channel.Mention} não é um canal de **texto**!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 2: // CATEGORY:
                if (Channel.Type != ChannelType.Category)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"O{Channel.Mention} não é uma **categoria**!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 3: // GROUP:
                if (Channel.Type != ChannelType.Group)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"{Channel.Mention} não é um canal de **grupo privado**!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 4: // IS PRIVATE:
                if (Channel.Type != ChannelType.Private)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"Meus comandos não funcionam em canais de **mensagens diretas**!!!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 5: // FORUM:
                if (Channel.Type == ChannelType.GuildForum)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"{Channel.Mention} não é um canal de **fórum**!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 6: // STAGE:
                if (Channel.Type != ChannelType.Stage)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"{Channel.Mention} não é um canal **palco**!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 7: // VOICE:
                if (Channel.Type != ChannelType.Voice)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"{Channel.Mention} não é um canal de **vóz**!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
        }
    }
}