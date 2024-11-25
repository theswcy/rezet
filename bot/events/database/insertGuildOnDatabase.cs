using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Bson;
using MongoDB.Driver;
using RezetSharp;


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
            Console.WriteLine($"    ➜  In: {e.Guild.Name} ( {e.Guild.Id} )  /  {ex.GetType()}\n    ➜  Error: {ex.Message}\n       {ex.StackTrace}\n\n\n");
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
                                { "messages_channel", BsonNull.Value },
                                { "moderation_channel", BsonNull.Value },

                                // GUILD MANAGER LOGS
                                { "manage_guild", BsonNull.Value },

                                // ROLES AND CHANNELS:
                                { "modified_roles", BsonNull.Value },
                                { "modified_channels", BsonNull.Value }
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
                        // ---------- START TEST NEW:
                        { "warns", new BsonDocument 
                            {
                                { "type_w", BsonNull.Value },
                                { "type_u", BsonNull.Value }
                            }
                        },
                        // ---------- END TEST NEW.
                    }
                },
                { "features", new BsonDocument
                    {
                        { "welcome", new BsonDocument
                            {
                                { "channel", BsonNull.Value },
                                { "delete", 0 },
                                { "embed", new BsonDocument
                                    {
                                        { "title", "Welcome new member!" },
                                        { "description", "Welcome to the community @[guild.name], @[member.mention]!" },
                                        { "image", 0 },
                                        { "thumbnail", 0 },
                                        { "author", 0 }
                                    }
                                }
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
                                { "invite", BsonNull.Value },
                                { "ranking", new BsonDocument {} }
                            }
                        },
                        { "ticket", new BsonDocument
                            {
                                { "option", 0 },
                                { "automatic", 0 },
                                { "embed_1", "default" },
                                { "embed_2", "default" },
                                { "configs", new BsonDocument
                                    {
                                        { "channel", BsonNull.Value },
                                        { "category", BsonNull.Value },
                                        { "support", BsonNull.Value },
                                        { "invite", BsonNull.Value }
                                    }
                                },
                                { "req_embeds", new BsonDocument
                                    {
                                        { "default", new BsonDocument
                                            {
                                                { "title", "Partnership Ticket!" }, // CAN BE NULL!
                                                { "description", "Quer fazer uma parceria com a nossa comunidade? Abra um ticket clicando no botão abaixo!" },
                                                { "footer", "Managed by Rezet!" },
                                                { "color", "#7e67ff" },
                                                { "image", "https://media.discordapp.net/attachments/1111358828282388532/1172043117294264350/IMG_20231109_020343.png" },
                                                { "thumb", 0 },
                                                { "author", 0 },
                                                { "button", new BsonDocument
                                                    {
                                                        { "color", "gray" },
                                                        { "text", "Solicitar parceria!" }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                { "tic_embeds", new BsonDocument
                                    {
                                        { "default", new BsonDocument
                                            {
                                                { "title", "Partnership Ticket!" }, // CAN BE NULL!
                                                { "description", "Aguarde um de nossos funcionários atender você!" },
                                                { "footer", "Managed by Rezet!" },
                                                { "color", "#7e67ff" },
                                                { "image", "https://media.discordapp.net/attachments/1111358828282388532/1172043117294264350/IMG_20231109_020343.png" },
                                                { "thumb", 0 },
                                                { "author", 0 },
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };



            // ========== INSERT GUILD DATABASE:
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            var shard = EngineV1.HerrscherRazor.GetHerrscherDocument(e);
            if (shard == null)
            {
                Console.WriteLine($"    ⌬  {y}  |  GUILD CREATE\n    ➜  Failed to acess guild: {e.Name} / {e.Id}\n\n\n");
                return;
            }
            var collection = EngineV1.HerrscherRazor?._database?.GetCollection<BsonDocument>("guilds");
            var update = Builders<BsonDocument>.Update.Set($"{e.Id}", guildStructure);
            var result = await collection.UpdateOneAsync(shard, update);

            if (result.ModifiedCount > 0)
            {
                Console.WriteLine($"    ⌬  {y}  |  GUILD CREATE\n    ➜  Added new guild: {e.Name} / {e.Id}\n\n\n");
            }
            else
            {
                Console.WriteLine($"    ⌬  {y}  |  GUILD CREATE\n    ➜  Failed to add guild: {e.Name} / {e.Id}\n\n\n");
            }
        }
        catch (Exception ex)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("dd/MM/yyyy - HH:mm:ss");
            Console.WriteLine($"    ⌬  {y}  |  GUILD CREATE\n    ➜  Error adding guild to database.\n    ➜  Error: {ex.Message}\n{ex.StackTrace}\n\n\n");
        }
    }
}