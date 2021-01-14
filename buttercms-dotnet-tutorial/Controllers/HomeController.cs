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
        public class Homepage
        {
            public string seo_title { get; set; }
            public string headline { get; set; }
            public string hero_image { get; set; }
            public string call_to_action { get; set; }
            public List<CustomerLogo> customer_logos { get; set; }
        }

        public class CustomerLogo
        {
            public string logo_image { get; set; }
        }

        [Route("/some")]
        public async Task<ActionResult> Some()
        {
            var butterClient = new ButterCMSClient("7409d6a1280930a7271d31c985de5337ee174085");

            PageResponse<Homepage> home = butterClient.RetrievePage<Homepage>("*", "homepage");

            var viewModel = new HomeViewModel();
            viewModel.SeoTitle = home.Data.Fields.seo_title;
            viewModel.Headline = home.Data.Fields.headline;
            viewModel.HeroImage = home.Data.Fields.hero_image;
            viewModel.CallToAction = home.Data.Fields.call_to_action;
            viewModel.CustomerLogos = home.Data.Fields.customer_logos;
            
            return View(viewModel);
        }
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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
