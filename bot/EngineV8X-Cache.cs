using System;
using System.Runtime.Caching;
using MongoDB.Bson;

namespace RezetSharp.LuminyCache
{
    public class CacheTier1_ForPartnership
    {
        private readonly MemoryCache _cache;



        public CacheTier1_ForPartnership()
        {
            _cache = MemoryCache.Default;
        }



        public void SaveGuild(ulong Key, ulong Channel)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(24)
            };
            _cache.Set(Key.ToString(), Channel, policy);
            Console.WriteLine("Cache salvo");
        }



        public ulong? GetGuild(ulong Key)
        {
            Console.WriteLine("Cache obtido");
            return _cache.Get(Key.ToString()) as ulong?;
        }



        public void RemoveGuild(ulong Key)
        {
            Console.WriteLine("Cache removido");
            _cache.Remove(Key.ToString());
        }
    }





    public class CacheTier1_ForAutoping
    {
        private readonly MemoryCache _cache;



        public CacheTier1_ForAutoping()
        {
            _cache = MemoryCache.Default;
        }



        public void SaveGuild(ulong Key, ulong Channel)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(24)
            };
            _cache.Set(Key.ToString(), Channel, policy);
        }



        public ulong? GetGuild(ulong Key)
        {
            return _cache.Get(Key.ToString()) as ulong?;
        }



        public void RemoveGuild(ulong Key)
        {
            _cache.Remove(Key.ToString());
        }
    }
}
