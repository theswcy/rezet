using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;




// ANOTAÇÕES:
//   > Apenas os tipos principais:

// RESUMO:
//        > (  1 ) IS EVERYONE.
//        > (  1 ) IS BOOSTER.
//        > (  1 ) IS MANAGED.




public static class CheckRoleType
{
    public static async Task CheckType(InteractionContext ctx, int Type, DiscordRole Role)
    {
        switch (Type)
        {
            case 1: // EVERYONE:
                if (Role.Id == ctx.Guild.EveryoneRole.Id)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"`{Role.Name}` não é um cargo válido!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 2: // BOOSTER:
                if (Role.Tags.IsPremiumSubscriber)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"`{Role.Name}` é um cargo de **boost** e não pode ser gerenciado!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
            case 3: // MANAGED:
                if (Role.IsManaged)
                {
                    await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"`{Role.Name}` é um cargo de **integração** e não pode ser gerenciado!")
                    );
                    throw new UnauthorizedAccessException();
                }
                break;
        }
    }
}