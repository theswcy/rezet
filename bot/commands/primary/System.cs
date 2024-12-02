using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Diagnostics;
using System.Reflection;
using MongoDB.Bson;
using RezetSharp;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;




#pragma warning disable CS8604
#pragma warning disable CS8602
public class RezetGets_prefix : BaseCommandModule
{
    [Command("ping")]
    [Aliases("latency")]
    public async Task MySy(CommandContext ctx)
    {
        try
        {
            await ctx.RespondAsync(
                $"My latency: `{ctx.Client.Ping}ms`"
            );   
        }
        catch (Exception ex)
        {
            await ctx.RespondAsync("Falha ao verificar ping...");
            Console.WriteLine(ex);
        }
    }
}
[SlashCommandGroup("my", "My systems.")]
public class RezetSystems : ApplicationCommandModule
{
    [SlashCommand("system", "✨ | Rezet Systems.")]
    public static async Task MySystems(InteractionContext ctx)
    {
        await ctx.DeferAsync();



        // ========== INTERNAL FILES:
        var proc = Process.GetCurrentProcess();
        var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
        var fileSize = fileInfo.Length / 1024.0 / 1024.0;



        // ========== EMBEDS BUILDERS:
        // MACHIME:
        var MachineInformations = new DiscordEmbedBuilder()
        {
            Description = "## <a:rezet_loading:1147722017806762045> Machine!",
            Color = new DiscordColor("#7e67ff")
        };
        MachineInformations.AddField(
            "<:rezet_settings1:1147163366561955932> Developemt:",
            "> **My version**: `Sharp 1.5`" +
            "\n> **Language**: `C# 12`" +
            "\n> **Library**: `D#+`" +
            "\n> **Engine**: `.NET 8.0`"
        );
        var t = proc.PrivateMemorySize64 / 1024 / 1024;
        var ss = EngineV1.HerrscherRazor?._database?.GetCollection<BsonDocument>("Guilds");
        var stats = ss.Database.RunCommand<BsonDocument>(new BsonDocument { { "dbStats", 1 } });
        var st = Convert.ToDouble(stats["dataSize"]) / (1024 * 1024);
        var pingEmoji = "<:rezet_1_goodping:1147907217593479179>";
        if (ctx.Client.Ping > 40) { pingEmoji = "<:rezet_1_idelping:1147907282844274879>"; }
        else if (ctx.Client.Ping > 60) { pingEmoji = "<:rezet_1_badping:1147907161507246254>"; }
        MachineInformations.AddField(
            "<:rezet_settings1:1147163366561955932> Machine:",
            $"> {pingEmoji} **Ping**: `{ctx.Client.Ping}ms`" +
            $"\n> <:memory:1184559679406354432> **RAM**: `{t}MB / 4096MB`" +
            $"\n> <:ssd:1184559769697136730> **SSD**: `{fileSize:F2}MB`" +
            $"\n> <:database:1184560409005522965> **DB**: `{st:F2}MB / 512MB`"
        );
        MachineInformations.WithImageUrl("https://media.discordapp.net/attachments/1111358828282388532/1172043117294264350/IMG_20231109_020343.png");
        MachineInformations.WithThumbnail(EngineV1.RezetRazor.CurrentUser.AvatarUrl);




        // SHARDS:
        var ShardedClientInformations = new DiscordEmbedBuilder()
        {
            Description = "## <a:rezet_loading:1147722017806762045> Shards!",
            Color = new DiscordColor("#7e67ff")
        };
        int MembersCount = 0;
        foreach (var entry in ctx.Client.Guilds.Values)
        {
            MembersCount += entry.MemberCount;
        }
        ShardedClientInformations.AddField(
            "<:rezet_box:1147164112091086920> Single Shard Client:",
            $"> **Servers**: `{ctx.Client.Guilds.Count}`" +
            $"\n> **Members**: `{MembersCount}`"
        );
        ShardedClientInformations.WithImageUrl("https://media.discordapp.net/attachments/1031214679915233380/1082297086290178048/IMG_20230306_104146.png");
        // foreach (var shard in Program.ShardedRezet.ShardClients)
        // {
        //     var shardId = shard.Key;
        //     var guildCount = shard.Value.Guilds.Count;
        //     var shardPing = shard.Value.Ping;

        //     var pingEmoji1 = "<:rezet_1_goodping:1147907217593479179>";
        //     if (shardPing > 40) { pingEmoji1 = "<:rezet_1_idelping:1147907282844274879>"; }
        //     else if (shardPing > 60) { pingEmoji1 = "<:rezet_1_badping:1147907161507246254>"; }

        //     int shardUsers = 0;
        //     foreach (var s in shard.Value.Guilds)
        //     {
        //         shardUsers += s.Value.MemberCount;
        //     }



        //     ShardedClientInformations.AddField(
        //         $"<:rezet_shine:1147368423475658882> Andromeda {shardId}:",
        //         $"> <:rezet_box:1147164112091086920> **Servers**: `{guildCount}`" +
        //         $"\n: <:rezet_Bugs:1148010355147153489> **Users**:`{shardUsers:N0}`" +
        //         $"\n> {pingEmoji1} **Ping**: `{shardPing}ms`",
        //         true
        //     );
        // }




        // DATABASE:
        var ShardedDatabaseInformations = new DiscordEmbedBuilder()
        {
            Description = "## <a:rezet_loading:1147722017806762045> Database!",
            Color = new DiscordColor("#7e67ff")
        };
        var Herrscher = EngineV1.HerrscherRazor.GetHerrscherDocument(ctx.Guild);
        var GuildsInHerrscher = Herrscher.AsBsonDocument.ToDictionary();
        ShardedDatabaseInformations.AddField(
            "<:rezet_share:1147165266887856209> Herrscher 0",
            $"> **Servers**: `{GuildsInHerrscher.Count}/1000`"
        );
        ShardedDatabaseInformations.WithImageUrl("https://media.discordapp.net/attachments/1031214679915233380/1082297086290178048/IMG_20230306_104146.png");
        




        // BUILDER:
        await ctx.EditResponseAsync(
            new DiscordWebhookBuilder()
                .WithContent("Bip bup bip!")
                .AddEmbed(MachineInformations)
                .AddEmbed(ShardedClientInformations)
                .AddEmbed(ShardedDatabaseInformations)
        );
    }
}