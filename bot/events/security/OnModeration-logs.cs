using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;
using RezetSharp;




#pragma warning disable CS8602
#pragma warning disable CS1998
public static class OnModeration_logs
{
    // ON MESSAGES:
    public class LoggingMessages
    {
        public static async Task Delete(DiscordClient sender, MessageDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Deleted Message\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Modify(DiscordClient sender, MessageUpdateEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Modify Message\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
    }



    // ON MODERATION ACTIONS:
    public class LoggingModerations
    {
        public static async Task Ban(DiscordClient sender, GuildBanAddEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Ban Add\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Unban(DiscordClient sender, GuildBanRemoveEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Ban Remove\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Kick(DiscordClient sender, GuildMemberRemoveEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Kick\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Timeout(DiscordClient sender, GuildMemberUpdateEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Timeout Add\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Untimeout(DiscordClient sender, GuildMemberUpdateEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Timeout Remove\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task WarnAdd(InteractionContext ctx)
        {

        }
        public static async Task WarnRemove(InteractionContext ctx)
        {

        }
        public static async Task WarnModify(InteractionContext ctx)
        {

        }
    }



    // ON ROLES ADD, REMOVE, CREATE, MODIFY AND DELETE:
    public class LoggingRoles
    {
        public static async Task Add(DiscordClient sender, GuildMemberUpdateEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Role Add\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Remove(DiscordClient sender, GuildMemberUpdateEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Role Remove\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Create(DiscordClient sender, GuildRoleCreateEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Role Create\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Delete(DiscordClient sender, GuildRoleDeleteEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Role Delete\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Modify(DiscordClient sender, GuildRoleUpdateEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Deleted Message\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
    }



    // ON CHANNEL CREATE, DELETE AND MODIFY:
    public class LoggingChannels
    {
        public static async Task Create(DiscordClient sender, ChannelCreateEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Channel Create\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Delete(DiscordClient sender, ChannelDeleteEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Deleted Message\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
        public static async Task Modify(DiscordClient sender, ChannelUpdateEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ➜  Logging: Deleted Message\n    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
            }
        }
    }



    // ON GUILD MANAGE:
    public class LoggingGuild
    {
        public static async Task Rename(DiscordClient sender, GuildUpdateEventArgs e)
        {
            
        }
        public static async Task Description(DiscordClient sender, GuildUpdateEventArgs e)
        {

        }
        public static async Task Icon(DiscordClient sender, GuildUpdateEventArgs e)
        {

        }
        public static async Task Splash(DiscordClient sender, GuildUpdateEventArgs e)
        {

        }
        public static async Task Banner(DiscordClient sender, GuildUpdateEventArgs e)
        {

        }
        public static async Task BotAdd(DiscordClient sender, GuildUpdateEventArgs e)
        {

        }
        public static async Task BotRemove(DiscordClient sender, GuildUpdateEventArgs e)
        {
            
        }
    }
}