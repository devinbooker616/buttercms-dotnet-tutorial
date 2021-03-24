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


        [Route("customers/")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {

            var butterClient = new ButterCMSClient("7409d6a1280930a7271d31c985de5337ee174085");

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


        [Route("customers/{slug}")]
        public async Task<ActionResult> ShowCaseStudy(string slug)

        {
            var butterClient = new ButterCMSClient("7409d6a1280930a7271d31c985de5337ee174085");

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