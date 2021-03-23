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
    public class CaseStudyController : Controller
    {
        public class CaseStudyPage
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

            var butterClient = new ButterCMSClient("YOUR KEY");

            var parameterDict = new Dictionary<string, string>()
            {
                {"page", page.ToString()},
                {"page_size", pageSize.ToString()},

            };

            var paramterDict = new Dictionary<string, string>();
            
            PagesResponse<CaseStudyPage> caseStudyPages = butterClient.ListPages<CaseStudyPage>("customer_case_study", parameterDict);

            var viewModel = new CaseStudiesViewModel();
            viewModel.PreviousPageNumber = caseStudyPages.Meta.PreviousPage;
            viewModel.NextPageNumber = caseStudyPages.Meta.NextPage;
            viewModel.PagesCount = caseStudyPages.Meta.Count;

            viewModel.CaseStudies = new List<CaseStudyViewModel>();
            PageResponse<CaseStudyPage> myPage = butterClient.RetrievePage<CaseStudyPage>("*", "sample-page", parameterDict);
            CaseStudyViewModel caseStudyViewModel = new CaseStudyViewModel();
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
            var butterClient = new ButterCMSClient("YOUR KEY");

            PageResponse<CaseStudyPage> caseStudy = await butterClient.RetrievePageAsync<CaseStudyPage>("*", slug);

            var viewModel = new CaseStudyViewModel();
            viewModel.Readme = caseStudy.Data.Fields.readme;
            viewModel.Seo = caseStudy.Data.Fields.seo;
            viewModel.twitterCard = caseStudy.Data.Fields.twitter_card;
            viewModel.openGraph = caseStudy.Data.Fields.open_graph;
            

            return View(viewModel);
        }

    }
}