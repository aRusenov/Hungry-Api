﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hungry.Data;
using Hungry.Models;
using Hungry.Services.Infrastructure.Upload;
using Hungry.Services.Models.Binding;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Hungry.Services.Infrastructure.Cache;
using Hungry.Services.Infrastructure.Attributes;

namespace Hungry.Services.Controllers
{
    public class RecipesController : BaseController
    {
        public RecipesController(IHungryData data, IBufferedCache cache, IImageUploadService uploadService) 
            : base(data, cache)
        {
            this.UploadService = uploadService;
        }

        public IImageUploadService UploadService { get; private set; }

        [HttpGet]
        public IHttpActionResult Get(GetRecipesBindingModel model)
        {
            var recipes = this.HungryData.Recipes.All()
                .Select(r => new
                {
                    Id = r.Id,
                    Title = r.Title,
                    PreviewImageUrl = r.PreviewImageUrl,
                    Author = new
                    {
                        Id = r.Author.Id,
                        Name = r.Author.UserName
                    }
                })
                .ToList();

            return this.Ok(recipes);
        }

        [Authorize]
        [HttpPost, MultipartRequest]
        public async Task<IHttpActionResult> Add()
        {
            var user = this.HungryData.Users.Find(this.User.Identity.GetUserId());
            if (user == null)
            {
                return this.Unauthorized();
            }
            
            string root = HttpContext.Current.Server.MapPath("~/temp/uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            string filePath = null;
            try
            {
                var result = await this.Request.Content.ReadAsMultipartAsync(provider);
                if (provider.FileData.Count == 0)
                {
                    return this.BadRequest("Recipe preview image is required");
                }

                var json = provider.FormData["data"];
                var model = JsonConvert.DeserializeObject<AddRecipeBindingModel>(json);

                if (model == null)
                {
                    return this.BadRequest("Invalid model");
                }

                this.Validate(model);
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                Array.Sort(model.Steps, (s1, s2) => s1.Order.CompareTo(s2.Order));
                for (int i = 1; i <= model.Steps.Length; i++)
                {
                    if (model.Steps[i - 1].Order != i)
                    {
                        return this.BadRequest("Recipe steps should have a unique order from [1...N]");
                    }
                }

                filePath = Path.Combine(root, provider.FileData[0].LocalFileName);
                var previewImageUrl = await UploadService.UploadAsync(filePath);
                var recipe = this.CreateRecipe(user, model, previewImageUrl);
                HungryData.Recipes.Add(recipe);
                HungryData.SaveChanges();

                var activity = new Activity
                {
                    SourceId = recipe.Id,
                    Type = ActivityType.NewRecipe,
                    User = user,
                    CreatedAt = DateTime.Now
                };

                HungryData.Activities.Add(activity);
                HungryData.SaveChanges();

                var subscribers = HungryData.Users.All()
                    .Where(u => u.Id == user.Id)
                    .Select(u => u.Subscribers.Select(s => s.SubscriberId))
                    .FirstOrDefault();

                var activityId = activity.Id.ToString();
                foreach (var sub in subscribers)
                {
                    Cache.Add(sub, activityId);
                }
            }
            finally
            {
                if (filePath != null)
                {
                    File.Delete(filePath);
                }
            }

            return this.Ok();
        }

        private Recipe CreateRecipe(User user, AddRecipeBindingModel model, string previewImageUrl)
        {
            var recipe = new Recipe
            {
                Description = model.Description,
                Title = model.Name,
                Preparation = TimeSpan.FromMinutes(45),
                PreviewImageUrl = previewImageUrl,
                AuthorId = user.Id,
                CreatedAt = DateTime.Now
            };

            foreach (var step in model.Steps)
            {
                recipe.Steps.Add(new RecipeStep
                {
                    Order = step.Order,
                    Content = step.Description
                });
            }

            foreach (var ingredient in model.Ingredients)
            {
                var ingr = HungryData.Ingredients.All()
                    .Where(i => i.Name == ingredient.Name)
                    .FirstOrDefault();
                if (ingr == null)
                {
                    ingr = new Ingredient { Name = ingredient.Name };
                }

                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    Recipe = recipe,
                    Ingredient = ingr,
                    Measurement = ingredient.Measurement,
                    Quantity = ingredient.Quantity,
                });
            }

            return recipe;
        }

        [Authorize]
        [HttpDelete, Route("api/recipes/delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var user = this.HungryData.Users.Find(this.User.Identity.GetUserId());
            if (user == null)
            {
                return this.Unauthorized();
            }

            var recipe = this.HungryData.Recipes.Find(id);
            if (recipe.AuthorId != user.Id)
            {
                return this.BadRequest("Only recipe author can delete recipe");
            }

            this.HungryData.Recipes.Delete(recipe);
            this.HungryData.SaveChanges();

            return this.Ok();
        }
    }
}