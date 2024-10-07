using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Rezet;



public class PrefixPrimary : BaseCommandModule
{
    [Command("rzt-database:all")]
    public async Task All(CommandContext ctx)
    {
        if (ctx.User.Id != 461618792464646145) { return; }
        #pragma warning disable CS8602
        try
        {
            var Guilds = Program.Rezet?.Guilds;
            foreach (var Guild in Guilds)
            {
                await InsertGuildDB.CreateGuild(Guild.Value);
                await ctx.Channel.SendMessageAsync(
                    $"<:rezet_dgreen:1147164307889586238> A guild [ **{Guild.Value.Name}** | `{Guild.Value.Id}` ] foi adicionada ao banco de dados!"
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }




    [Command("rzt-database:add")]
    public async Task Add(CommandContext ctx, string id)
    {
        if (ctx.User.Id != 461618792464646145) { return; }
        try
        {
            var Guild = await Program.Rezet.GetGuildAsync(ulong.Parse(id));
            await InsertGuildDB.CreateGuild(Guild);
            await ctx.RespondAsync($"Nice! A guild [ **{Guild.Name}** | `{Guild.Id}` ] foi adicionada ao banco de dados!");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }




    [Command("rzt-database:del")]
    public async Task Del(CommandContext ctx, string id)
    {
        if (ctx.User.Id != 461618792464646145) { return; }
        try
        {
            var Guild = await Program.Rezet.GetGuildAsync(ulong.Parse(id));
            await DeleteGuildDB.DeleteGuild(ctx.Guild);
            await ctx.RespondAsync($"Nice! A guild [ **{Guild.Name}** | `{Guild.Id}` ] foi removida do banco de dados!");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}