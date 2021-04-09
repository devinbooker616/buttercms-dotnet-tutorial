using System;
using System.Threading.Tasks;
using ButterCMS;
using ButterCMS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using buttercms_dotnet_tutorial.Models;

namespace buttercms_dotnet_tutorial.Controllers
{
    public class BlogController : Controller
    {
        private ButterCMSClient Client;

        private static string _apiToken = "YOUR KEY";

        public BlogController()
        {
            Client = new ButterCMSClient(_apiToken);
        }

        
        [Route("blog")]
        [Route("blog/p/{page}")]
        public async Task<ActionResult> ListAllPosts(int page = 1)
        {
            var response = await Client.ListPostsAsync(page, 10);
            ViewBag.Posts = response.Data;
            ViewBag.NextPage = response.Meta.NextPage;
            ViewBag.PreviousPage = response.Meta.PreviousPage;
            return View();
        }

        [Route("blog/{slug}")]
        public async Task<ActionResult> ShowPost(string slug)
        {
            var response = await Client.RetrievePostAsync(slug);
            ViewBag.Post = response.Data;
            return View("PostDetail");
        }
    }
}