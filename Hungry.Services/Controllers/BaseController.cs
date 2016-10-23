using Hungry.Data;
using Hungry.Services.Infrastructure.Cache;
using ServiceStack.Redis;
using System.Web.Http;

namespace Hungry.Services.Controllers
{
    public class BaseController : ApiController
    {
        protected IHungryData hungryData;
        protected IBufferedCache cache;

        public BaseController() : this(
            new HungryData(new HungryContext()),
            new BufferedRedisCache(3, new RedisClient())
            )
        {
        }

        public BaseController(IHungryData data, IBufferedCache cache)
        {
            this.hungryData = data;
            this.cache = cache;
        }
    }
}