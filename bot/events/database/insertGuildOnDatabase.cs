using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Bson;
using MongoDB.Driver;
using Rezet;


#pragma warning disable CS8602
public static class InsertGuildDB
{
    public static async Task OnGuildCreated(DiscordClient sender, GuildCreateEventArgs e)
    {
        try
        {
            await CreateGuild(e.Guild);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static async Task CreateGuild(DiscordGuild e)
    {
        try
        {
            var guildStructure = new BsonDocument
            {
                { "guild_name", e.Name },
                { "type", "community" },
                { "lang", "pt-br" },
                { "moderation", new BsonDocument
                    {
                        { "sub_owner", BsonNull.Value },
                        { "key_pass", BsonNull.Value },
                        { "mod_logs", new BsonDocument
                            {
                                // MODERATION LOGS
                                { "message_deleted_channel", BsonNull.Value },
                                { "banned_user_channel", BsonNull.Value },
                                { "kick_user_channel", BsonNull.Value },
                                { "mute_user_channel", BsonNull.Value },
                                { "role_add_user_channel", BsonNull.Value },

                                // GUILD MANAGER LOGS
                                { "change_icon_channel", BsonNull.Value },
                                { "change_banner_channel", BsonNull.Value },
                                { "change_splash_channel", BsonNull.Value },
                                { "change_name_channel", BsonNull.Value },

                                // ROLES AND CHANNELS:
                                { "modified_role_channel", BsonNull.Value },
                                { "modified_chat_channel", BsonNull.Value },
                                { "modified_category_channel", BsonNull.Value },
                            }
                        },
                        { "security", new BsonDocument
                            {
                                // SHARP MODE:
                                { "sharp-mode", false },
                                // BASICS:
                                { "anti-invites", BsonNull.Value },
                                { "anti-raid", BsonNull.Value },
                                { "anti-selfbot", BsonNull.Value },
                                { "blackout", BsonNull.Value }
                            }
                        },
                        { "auto_actions", new BsonDocument
                            {
                                { "auto_role", BsonNull.Value },
                                { "auto_ping", BsonNull.Value }
                            }
                        },
                        { "verify", new BsonDocument
                            {
                                { "option", 0 }
                                // ========== OPTIONS:
                                // 0 -> off.
                                // 1 -> simple.
                                // 2 -> complex.
                            }
                        }
                    }
                },
                { "partner", new BsonDocument
                    {
                        { "option", 0 },
                        // ========== OPTIONS:
                        // 0 -> off.
                        // 1 -> on.
                        { "ps", 0 },
                        { "xp" , 0 },
                        { "selected", "default" },
                        { "log", 0 },
                        { "anti-eh", 0 },
                        { "anti-qi", 0 },
                        { "configs", new BsonDocument
                            {
                                { "options", new BsonDocument
                                    {
                                        { "role", BsonNull.Value },
                                        { "ping", BsonNull.Value },
                                        { "channel", BsonNull.Value },
                                    }
                                },
                                { "embeds", new BsonDocument
                                    {
                                        { "default", new BsonDocument
                                            {
                                                { "title", "Partnership!" },
                                                { "description", "Uma nova parceria foi feita com a nossa comunidade **@[guild.name]**!" },
                                                { "footer", "Managed by Rezet!" },
                                                { "color", "#7e67ff" },
                                                { "image", "https://media.discordapp.net/attachments/1111358828282388532/1172043117294264350/IMG_20231109_020343.png" },
                                                { "thumb", 0 },
                                                { "author", 0 }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        { "leaderboard", new BsonDocument
                            {
                                { "option", 0 },
                                { "ranking", new BsonDocument {} }
                            }
                        }
                    }
                }
            };



            // ========== INSERT GUILD DATABASE:
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            var shard = Program._databaseService?.GetShard(e, 2);
            if (shard == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  {y}  |  GUILD CREATE ] Failed to acess guild ( {e.Name} / {e.Id} )");
                Console.ResetColor();
                return;
            }
            var collection = Program._databaseService?.database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{e.Id}", guildStructure);
            var result = await collection.UpdateOneAsync(shard, update);

            if (result.ModifiedCount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"O [  {y}  |  GUILD CREATE  ] Added new guild ( {e.Name} )");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"X [  {y}  |  GUILD CREATE  ] Failed to add guild ( {e.Name} / {e.Id} ) in database.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"X [  {y}  |  GUILD CREATE  ] Error adding guild to database: [ {ex.Message} ]");
            Console.ResetColor();
        }
    }
}