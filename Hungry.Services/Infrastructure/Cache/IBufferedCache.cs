using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hungry.Services.Infrastructure.Cache
{
    public interface IBufferedCache
    {
        int BufferSize { get; }

        void Add(string key, string value);

        List<string> Get(string key, int count, int page);
    }
}