using System;
using System.Collections.Generic;
using System.Linq;
using ButterCMS;
using ButterCMS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using buttercms_dotnet_tutorial.Configuration;

namespace buttercms_dotnet_tutorial.Controllers
{
    public class BaseController : Controller
    {
        public IWebHostEnvironment HostingEnvironment { get; }
        public ButterCmsOptions SiteOptions { get; }
        public ButterCMSClient Client { get; }
        public IMemoryCache Cache { get; }
        public UrlOptions UrlOptions { get; }

        public BaseController(IWebHostEnvironment hostingEnvironment, IOptions<UrlOptions> urlOptions, IOptions<ButterCmsOptions> siteOptions, ButterCMSClient client, IMemoryCache cache)
        {
            HostingEnvironment = hostingEnvironment;
            SiteOptions = siteOptions.Value;
            Client = client;
            Cache = cache;
            UrlOptions = urlOptions.Value;
        }

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewBag.BaseUrl = UrlOptions.BaseUrl;

            ViewBag.Author = Cache.GetOrCreate($"author|by-slug|{SiteOptions.PrimaryAuthorSlug}", entry =>
            {
                entry.Value = Client.RetrieveAuthor(SiteOptions.PrimaryAuthorSlug, false);
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddDays(1);
                return (Author)entry.Value;
            });
            ViewBag.Categories = Cache.GetOrCreate("categories|all", entry =>
            {
                entry.Value = Client.ListCategories(true).Where(x => x.RecentPosts.Any()).ToList();
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddDays(1);
                return (List<Category>)entry.Value;
            });
            ViewBag.Tags = Cache.GetOrCreate("tags|all", entry =>
            {
                entry.Value = Client.ListTags(true).Where(x => x.RecentPosts.Any()).ToList();
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddDays(1);
                return (List<Tag>)entry.Value;
            });
        }
    }
}