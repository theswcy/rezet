using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;



[SlashCommandGroup("autorole", "Autorole Setup.")]
public class AutoRoleSettings : ApplicationCommandModule
{
    [SlashCommand("add", "💼 | Add an automatic role!")]
    public static async Task AutoRoleAdd(InteractionContext ctx,
        [Option("role", "The role that will be given to the new member!")] DiscordRole Role
    )
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }



    [SlashCommand("remove", "💼 | Remove an automatic role.")]
    public static async Task AutoRoleRemove(InteractionContext ctx,
        [Option("role", "The role that will be removed.")] DiscordRole Role
    )
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }



    [SlashCommand("clear", "💼 | Remove all automatic roles!")]
    public static async Task AutoRoleClear(InteractionContext ctx)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}