using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace Hungry.Services.Infrastructure.Cache
{
    public class BufferedRedisCache : IBufferedCache
    {
        private int bufferSize;
        private IRedisClient redisClient;

        public BufferedRedisCache(int bufferSize, IRedisClient redisClient)
        {
            this.BufferSize = bufferSize;
            this.redisClient = redisClient;
        }

        public int BufferSize
        {
            get { return this.bufferSize; }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("BufferSize should be positive");
                }

                this.bufferSize = value;
            }
        }

        public void Add(string key, string value)
        {
            redisClient.PushItemToList(key, value);
            redisClient.TrimList(key, 0, this.bufferSize - 1);
        }

        public List<string> Get(string key, int count, int page)
        {
            var start = count * page;
            var end = start + count;
            return redisClient.GetRangeFromList(key, start, end);
        }
    }
}