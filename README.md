
# Setup
## Create a new project

* Open Visual Studio and click ```Create a new project```
* Click on ```ASP.Net Core Web Application```
* Name the project and solution
* Then click ```create```

## Install

In Visual Studio, open the Package Manager Console and run:
```Install-Package ButterCMS```

# Configuration
Go to the ```Startup.cs``` file that comes included with all projects once created and copy this code:
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ButterCMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;


namespace buttercms_dotnet_tutorial
{
    public class Startup
    {
        public IWebHostEnvironment HostingEnvironment { get; }

        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Config = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
          

            services.AddSingleton<IWebHostEnvironment>(HostingEnvironment);

            services.AddOptions();
            //services.Configure<UrlOptions>(Configuration.GetSection("UrlOptions"));
            //services.Configure<ButterCmsOptions>(Configuration.GetSection("ButterCMSOptions"));

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

```
Now we are ready to move onto the rest of the project.
# Landing Page
Before we actually get started on building the main functions of ```ButterCMS``` we should make a landing page for the site so all the other pages as well as relevant documentation can be accessed all from there. 

Considering a ```Home``` folder as well as controller should already be made, all we have to do is alter some code and HTML. First let’s go to ```HomeController.cs``` and copy this code: 

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using buttercms_dotnet_tutorial.Models;
using ButterCMS;
using ButterCMS.Models;


namespace buttercms_dotnet_tutorial.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        
    }
}
```

With controller done we can edit the ```Index.cshtml``` file in the ```Home``` folder:
```cshtml
@model HomeViewModel
    <h3>ButterCMS. Headless CMS you'll melt over</h3>
    <div class="card bg-gray-50 mb-5 rounded-lg index-cards">
        <div class="card-body bg-gray-50 mb-5 rounded-lg">
            <h1 class="card-title"><strong>Blog Engine</strong></h1>
            <h1 class="card-title text-primary" style="color: "><strong>You've got better things to do than building another blog.</strong></h1>
            <a class="btn btn-primary" asp-area="" asp-controller="Blog" asp-action="ListAllPosts">Preview integration</a>
            <a class="btn btn-outline-dark" href="https://buttercms.com/features/#flexiblecontentmodeling-blog-engine">Learn More</a>
        </div>
    </div>
    <div class="card bg-gray-50 mb-5 rounded-lg index-cards">
        <div class="card-body bg-gray-50 mb-5 rounded-lg index-cards">
            <h1 class="card-title"><strong>Pages</strong></h1>
            <h1 class="card-title text-primary"><strong>Build SEO landing pages, knowledge base, news articles, and more by using Page Types.</strong></h1>
            <a class="btn btn-primary" asp-controller="CaseStudy" asp-action="Index">Preview integration</a>
            <a class="btn btn-outline-dark" href="https://buttercms.com/features/#flexiblecontentmodeling-page-types">Learn More</a>
        </div>
    </div>
```

Now we can get started with ButterCMS pages

# Pages
# Pages structure 
Before making the logic for Pages make sure your page strucutre (which it should already be with the sample page) is set up and that you already have at least one made.

## Pages Models
In the ```Models``` folder you'll want to create ```PageViewModel.cs``` and it will look like this:
```csharp 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buttercms_dotnet_tutorial.Models
{
    public class PageViewModel
    {
        public string Readme { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }

        public Dictionary<string, string> Seo { get; set; }

        public Dictionary<string, string> twitterCard { get; set; }

        public Dictionary<string, string> openGraph { get; set; }

    }
}
```

amd then make ```PagesViewModel.cs``` in the ```Models``` folder which will look like this: 
```csharp 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buttercms_dotnet_tutorial.Models
{
    public class PagesViewModel
    {
        public List<PageViewModel> CaseStudies { get; set; }
        public int? PreviousPageNumber { get; set; }
        public int? NextPageNumber { get; set; }
        public int PagesCount { get; set; }
    }
}
```
Now with the ```Models``` made, we can start on the controller. 

## Pages Controller
Go into the ```Controllers``` folder and add a file called ```PageController.cs``` and the file will contain this code:
```csharp 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ButterCMS;
using Newtonsoft.Json;
using ButterCMS.Models;
using buttercms_dotnet_tutorial.Models;

namespace buttercms_dotnet_tutorial.Controllers
{
    public class PageController : Controller
    {
        public class Page
        {
           
            public string readme { get; set; }
            
            public string slug { get; set; }
            
    
           public Dictionary<string, string> seo { get; set; }

           public Dictionary<string, string> twitter_card { get; set; }

           public Dictionary<string, string>  open_graph { get; set; }

            

        }


        [Route("pages/")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {

            var butterClient = new ButterCMSClient("YOUR KEY");

            var parameterDict = new Dictionary<string, string>()
            {
                {"page", page.ToString()},
                {"page_size", pageSize.ToString()},

            };

            var paramterDict = new Dictionary<string, string>();
            
            PagesResponse<Page> caseStudyPages = butterClient.ListPages<Page>("sample-page", parameterDict);

            var viewModel = new PagesViewModel();

            viewModel.CaseStudies = new List<PageViewModel>();
            PageResponse<Page> myPage = butterClient.RetrievePage<Page>("*", "sample-page", parameterDict);
            PageViewModel caseStudyViewModel = new PageViewModel();
            caseStudyViewModel.Readme = myPage.Data.Fields.readme;
            caseStudyViewModel.Seo = myPage.Data.Fields.seo;
            caseStudyViewModel.twitterCard = myPage.Data.Fields.twitter_card;
            caseStudyViewModel.openGraph = myPage.Data.Fields.open_graph;
            caseStudyViewModel.Slug = myPage.Data.Slug;
            viewModel.CaseStudies.Add(caseStudyViewModel);
           
            return View(viewModel);
        }


        [Route("pages/{slug}")]
        public async Task<ActionResult> ShowCaseStudy(string slug)

        {
            var butterClient = new ButterCMSClient("YOUR KEY");

            PageResponse<Page> caseStudy = await butterClient.RetrievePageAsync<Page>("*", slug);

            var viewModel = new PageViewModel();
            viewModel.Readme = caseStudy.Data.Fields.readme;
            viewModel.Seo = caseStudy.Data.Fields.seo;
            viewModel.twitterCard = caseStudy.Data.Fields.twitter_card;
            viewModel.openGraph = caseStudy.Data.Fields.open_graph;
            

            return View(viewModel);
        }

    }
}
```
With the ```Controllers``` and ```Models``` made all that is left to do is add files to the ```Views``` folder. In the ```Views``` folder we’re gonna add another folder and in that folder will be two files. 

## Pages Views
First, in ```Views```, make a folder called ```Page``` then add ```Index.cshtml```. The contents of ```Index.cshtml``` will look as such:
 
 ```Index.cshtml```:
```cshtml 
@model PagesViewModel

@{
    ViewData["Title"] = "Index";
}

<style>
    @@media (min-width: 768px){
    .mobile-pages {
        padding-left: 50% !important;
    }
}
</style>


<h2 class="text-2xl md:text-4xl font-bold tracking-tight md:tracking-tighter leading-tight px-4 mb-20 mt-8">Case studies</h2>
@foreach (var pages in Model.CaseStudies)
{
<div class="container mx-auto px-5">

        <div class="grid grid-flow-col auto-rows-max gap-4 mb-5 row">
            <div style="position: relative; width: 300px; height: 300px;">
                <a href="/customers">
                    <div style="display: block; overflow: hidden; position: absolute; inset: 0px; box-sizing: border-box; margin: 0px; border: solid 1px black;">
                        @*<img alt="@pages.Headline" src="@pages.CustomerLogo" decoding="async" class="rounded-lg" sizes="(max-width: 640px) 640px, (max-width: 750px) 750px, (max-width: 828px) 828px, (max-width: 1080px) 1080px, (max-width: 1200px) 1200px, (max-width: 1920px) 1920px, (max-width: 2048px) 2048px, 3840px" style="visibility: visible; position: absolute; inset: 0px; box-sizing: border-box; padding: 0px; border: none; margin: auto; display: block; width: 0px; height: 0px; min-width: 100%; max-width: 100%; min-height: 100%; max-height: 100%; object-fit: cover;">*@
                    </div>
                </a>
            </div>
            <div class="col-8 mobile-pages">
                <h3 class="text-3xl mb-3 mt-3 leading-snug">
                    <a class="hover:underline" href="/customers/@pages.Slug">Sample Page</a>
                </h3>
                Study date: <time datetime="2020-08-30T00:00:00">August	30, 2020</time>
                <div>
                    Reviewed by customer
                </div>
            </div>
        </div>
    </div>

}
```
Now we can make the ```ShowCaseStudy.cshtml``` file which will contain this: 
```cshtml 

@{
    ViewData["Title"] = "ShowCaseStudy";

}

@model PageViewModel



<p class="mt-5">@Html.Raw(Model.Readme)</p>
<h1>SEO</h1>
@foreach (var item in Model.Seo)
{

    <ul>
        <h1><label>@Html.DisplayFor(modelItem => item.Key)</label></h1>
        <li>@Html.DisplayFor(modelItem => item.Value);</li>
    </ul>
}

<h1>Open Graph</h1>
@foreach (var item in Model.openGraph)
{
    <ul>
        <h1><label>@Html.DisplayFor(modelItem => item.Key)</label></h1>
        @if (item.Value.Contains("http"))
        {
            <li><img src="@Html.DisplayFor(modelItem => item.Value)" /></li>
        }
        else
        {
            @Html.DisplayFor(modelItem => item.Value)
        }


    </ul>
}

<h1>Twitter Card</h1>
@foreach (var item in Model.twitterCard)
{
    <ul>
        <h1><label>@Html.DisplayFor(modelItem => item.Key)</label></h1>
        @if (item.Value.Contains("http"))
        {
            <li><img src="@Html.DisplayFor(modelItem => item.Value)" /></li>
        }
        else
        {
            @Html.DisplayFor(modelItem => item.Value)
        }


    </ul>
}


```
Now with all the code for ```Pages``` implemented correctly, you should be able to display all of your ButterCMS ```Pages``` using the ButterCMS API data. 

With ```Pages``` done we can move on to implementing ButterCMS ```Blogs```.

# Blog

```Blogs``` will follow a very similar pattern to ```Pages```  where we make a ```Blogs Models```, a ```Blogs Controller```, and ```Blogs Views```.

## Blog Models
So let's start with ```Models```. First you're going to go into the ```Models``` folder and then make a file called ```BlogListViewModel``` which will look like this: 
```csharp 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buttercms_dotnet_tutorial.Models
{
    public class BlogListViewModel
    {
        public IEnumerable<ButterCMS.Models.Post> Posts { get; set; }
        public int Count { get; set; }
        public int? NextPage { get; set; }
        public int CurrentPage { get; set; }
        public int? PreviousPage { get; set; }
        public int TotalPages { get; set; }
    }
}
```
With that done we can move onto the ```Controller```

## Blog Controller 

Now for the last part, we're gonna make make the ```BlogController```. Make a file called ```BlogController.cs``` and in the file put: 
```csharp 
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
```
## Blog Views

Now with that done we can move onto the actual blog. First let’s make the ```Blog``` folder in the ```Views``` folder. In the folder add ```ListAllPosts.cshtml``` which is gonna look like this:
```cshmtl
<h2>Posts</h2>

@foreach (var post in ViewBag.Posts)
{
    <a href="/blog/@Uri.EscapeDataString(post.Slug)">@post.Title</a>
    @Html.Raw("by")
    <a href="/author/@Uri.EscapeDataString(post.Author.Slug)">@post.Author.FirstName @post.Author.LastName</a>
    <br />
}

@if (ViewBag.PreviousPage != null)
{
    <a href="/blog/p/@ViewBag.PreviousPage">Prev</a>
}

@if (ViewBag.NextPage != null)
{
    <a href="/blog/p/@ViewBag.NextPage">Next</a>
}
```
Now that the posts are displaying in a neat vertical list we can make the page that displays the whole blog. Make a file called ```PostDetail.cshtml``` and the following code will be in there: 
```cshtml

<h2>@ViewBag.Post.Title</h2>

Published @ViewBag.Post.Published.ToString("d/M/yyyy")
@if (@ViewBag.Post.Categories.Count > 0)
{
    @Html.Raw("in")
}
@foreach (var category in @ViewBag.Post.Categories)
{
    <a href="/category/@Uri.EscapeDataString(category.Slug)">@category.Name</a>
}

<br />

<a href="/author/@Uri.EscapeDataString(ViewBag.Post.Author.Slug)">
    @ViewBag.Post.Author.FirstName @ViewBag.Post.Author.LastName
</a>

<br />

<div>
    @Html.Raw(ViewBag.Post.Body)
</div>
```

# Finishing Notes

And with al  that done you should have a landing page, blog page, and a page that showcases the ability of ```Pages```. 
For any more assistance go to https://buttercms.com/docs/api-client/dotnet for more information on ButterCMS's .Net capabilities. 
