using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Diagnostics;
using System.Reflection;
using Rezet;
using MongoDB.Bson;




[SlashCommandGroup("a", "Rezet Gets")]
public class RezetGets : ApplicationCommandModule
{
    [SlashCommand("about", "✨ | About me!")]
    public static async Task AboutCommand(InteractionContext ctx)
    {
        await ctx.DeferAsync();
        

        var proc = Process.GetCurrentProcess();
        var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
        var fileSize = fileInfo.Length / 1024.0 / 1024.0;


        var embed = new DiscordEmbedBuilder
        {
            Description = (
                "- Heyoo, eu sou a **Rezet**, uma poderosa aplicação multi-**funções** com a simples tarefa de suprir todas as necessidades que sua comunidade precisa!\n⠀"
            ),
            Color = new DiscordColor("#7e67ff")
        };
        embed.WithAuthor(
            "Public Beta!", iconUrl: ctx.Client.CurrentUser.AvatarUrl
        );
        embed.AddField(
            "<:rezet_shine:1147368423475658882> Development:",
            "> **My version**: `Sharp 1.0 / PTB R-00022`" +
            "\n> **Language**: `CSharp. [ C# ]`" +
            "\n> **Library**: `D#+ [ DSharpPlus ]`"  +
            "\n> **Framework**: `.NET 8.0`" +
            $"\n> **Operating in**: `{ctx.Client.Guilds.Count} communities`"
            );
        var t = proc.PrivateMemorySize64 / 1024 / 1024;
        var ss = Program._databaseService?.database?.GetCollection<BsonDocument>("Guilds");
#pragma warning disable CS8602
        var stats = ss.Database.RunCommand<BsonDocument>(new BsonDocument { { "dbStats", 1 } });
        var st = Convert.ToDouble(stats["dataSize"]) / (1024 * 1024);
        embed.AddField(
            "<:rezet_shine:1147373071737573446> Components:",
            $"> **PING**: `{ctx.Client.Ping}ms`" +
            $"\n> **RAM**: `{t}MB / 2GB`" +
            $"\n> **SSD**: `{fileSize:F2}MB / 2GB`" +
            $"\n> **DB**: `{st:F2}MB / 512MB`"
            );
        embed.WithImageUrl("https://media.discordapp.net/attachments/1111358828282388532/1172043117294264350/IMG_20231109_020343.png");
        embed.WithThumbnail(Program.Rezet?.CurrentUser.AvatarUrl);



        await ctx.EditResponseAsync(
            new DiscordWebhookBuilder()
            .WithContent("Bip bup bip!")
            .AddEmbed(embed)
        );
    }
}