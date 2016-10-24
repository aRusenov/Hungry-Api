using Hungry.Data;
using Hungry.Services.Infrastructure.Cache;
using ServiceStack.Redis;
using System.Web.Http;

namespace Hungry.Services.Controllers
{
    public class BaseController : ApiController
    {
        public BaseController(IHungryData data, IBufferedCache cache)
        {
            this.HungryData = data;
            this.Cache = cache;
        }

        public IHungryData HungryData { get; private set; }

        public IBufferedCache Cache { get; private set; }
    }
}