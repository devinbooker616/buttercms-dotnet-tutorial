using System;
using System.Threading.Tasks;
using ButterCMS;
using ButterCMS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using buttercms_dotnet_tutorial.Configuration;
using buttercms_dotnet_tutorial.Models;

namespace buttercms_dotnet_tutorial.Controllers
{
    public class BlogController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public BlogController(IWebHostEnvironment hostingEnvironment, IOptions<UrlOptions> urlOptions, IOptions<ButterCmsOptions> siteOptions, ButterCMSClient client, IMemoryCache cache) : base(hostingEnvironment, urlOptions, siteOptions, client, cache)
        {
        }

        [Route("")]
        [Route("p/{page}")]
        [Route("blog")]
        [Route("blog/p/{page}")]
        [ResponseCache(CacheProfileName = "2days")]
        public async Task<IActionResult> ListAllPosts(int page = 1)
        {
            var postsPerPage = 10;

            var response = await Cache.GetOrCreateAsync($"posts|all|{postsPerPage}|{page}", async entry =>
            {
                entry.Value = (await Client.ListPostsAsync(page, postsPerPage));
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddDays(2);
                return (PostsResponse)entry.Value;
            });
            
            var model = new BlogListViewModel
            {
                Posts = response.Data,
                Count = response.Meta.Count,
                NextPage = response.Meta.NextPage,
                CurrentPage = page,
                PreviousPage = response.Meta.PreviousPage,
                TotalPages = Convert.ToInt32(Math.Floor(decimal.Divide(response.Meta.Count, postsPerPage)))
            };

            return View(model);
        }

        [Route("blog/{slug}")]
        [ResponseCache(CacheProfileName = "7days")]
        public async Task<ActionResult> PostDetail(string slug)
        {
            var response = await Cache.GetOrCreateAsync($"post|by-slug|{slug}", async entry =>
            {
                entry.Value = await Client.RetrievePostAsync(slug);
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddDays(7);
                return (PostResponse)entry.Value;
            });
            
            return View(response.Data);
        }
    }
}