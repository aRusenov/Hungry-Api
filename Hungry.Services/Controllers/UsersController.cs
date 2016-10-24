using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hungry.Data;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Hungry.Models;
using EntityFramework.Extensions;
using Hungry.Services.Infrastructure.Cache;
using ServiceStack.Redis;
using Hungry.Services.Models.Binding;
using System.Diagnostics;
using System.IO;

namespace Hungry.Services.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IHungryData data, IBufferedCache cache) 
            : base(data, cache)
        {
        }

        [Authorize]
        [HttpGet, Route("api/users/feed")]
        public IHttpActionResult GetFeed([FromUri]GetFeedBindingModel model)
        {
            var user = this.HungryData.Users.Find(this.User.Identity.GetUserId());
            if (user == null)
            {
                return this.Unauthorized();
            }

            if (model == null)
            {
                return this.BadRequest("Invalid model");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            int[] activitiesSourceIds;
            var start = model.Count * model.Page;
            if (start < Cache.BufferSize)
            {
                // Pull activities from cache
                var activityIds = Cache.Get(user.Id, model.Count, model.Page).Select(int.Parse).ToArray();

                activitiesSourceIds = HungryData.Activities.All()
                    .Where(a => activityIds.Contains(a.Id))
                    .Select(a => a.SourceId)
                    .ToArray();
            }
            else
            {
                // Fallback to querying DB
                activitiesSourceIds = HungryData.Activities.All()
                    .Where(a => a.User.Subscribers.Any(s => s.SubscriberId == user.Id))
                    .OrderByDescending(a => a.CreatedAt)
                    .Skip(start)
                    .Take(model.Count)
                    .Select(a => a.SourceId)
                    .ToArray();
            }

            // Perform 1 query per activity type
            var recipes = HungryData.Recipes.All()
                .Where(r => activitiesSourceIds.Contains(r.Id))
                .OrderBy(r => r.CreatedAt)
                .Select(r => new
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .ToList();

            return this.Ok(recipes);
        }

        [Authorize]
        [HttpGet]
        public IHttpActionResult GetSubscriptions()
        {
            var user = this.HungryData.Users.Find(this.User.Identity.GetUserId());
            if (user == null)
            {
                return this.Unauthorized();
            }

            var subscriptions = HungryData.Subscriptions.All()
                .Where(s => s.SubscriberId == user.Id)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new
                {
                    UserId = s.User.Id,
                    Username = s.User.UserName
                })
                .ToList();

            return this.Ok(subscriptions);
        }

        [Authorize]
        [HttpPost, Route("api/users/subscribe/{userId}")]
        public IHttpActionResult Subscribe(string userId)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var subscriber = this.HungryData.Users.Find(currentUserId);
            if (subscriber == null)
            {
                return this.Unauthorized();
            }

            var user = this.HungryData.Users.Find(userId);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            if (subscriber.Id == user.Id)
            {
                return this.BadRequest("Cannot subscribe to self");
            }

            if (IsSubscribedTo(subscriber, user))
            {
                return this.BadRequest("Already subscribed");
            }

            HungryData.Subscriptions.Add(new Subscription
            {
                UserId = user.Id,
                SubscriberId = subscriber.Id,
                CreatedAt = DateTime.Now
            });

            HungryData.SaveChanges();

            return this.Ok();
        }

        private bool IsSubscribedTo(User subscriber, User target)
        {
            return this.HungryData.Users.All()
                            .Where(u => u.Id == target.Id && u.Subscribers.Any(s => s.SubscriberId == subscriber.Id))
                            .Count() > 0;
        }

        [Authorize]
        [HttpPost, Route("api/users/unsubscribe/{userId}")]
        public IHttpActionResult Unubscribe(string userId)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var subscriber = this.HungryData.Users.Find(currentUserId);
            var target = this.HungryData.Users.Find(userId);
            if (target == null)
            {
                return this.BadRequest("Invalid user");
            }
            
            if (!IsSubscribedTo(subscriber, target))
            {
                return this.BadRequest("Not subscribed");
            }

            HungryData.Subscriptions.All()
                .Where(u => u.UserId == userId && u.SubscriberId == currentUserId)
                .Delete();

            return this.Ok();
        }
    }
}