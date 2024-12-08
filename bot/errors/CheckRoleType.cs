using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;



// ANOTAÇÕES:
//   > Apenas os tipos principais:

// RESUMO:
//        > (  1 ) IS EVERYONE.
//        > (  2 ) IS BOOSTER.
//        > (  3 ) IS MANAGED.




#pragma warning disable CS8602
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
                if (ctx.Guild.Roles.Values.FirstOrDefault(r => r.Tags.IsPremiumSubscriber) != null)
                {
                    if (Role.Id == ctx.Guild.Roles.Values.FirstOrDefault(r => r.Tags.IsPremiumSubscriber).Id)
                    {
                        await ctx.EditResponseAsync(
                        new DiscordWebhookBuilder()
                            .WithContent($"`{Role.Name}` é um cargo de **boost** e não pode ser gerenciado!")
                        );
                        throw new UnauthorizedAccessException();
                    }
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
