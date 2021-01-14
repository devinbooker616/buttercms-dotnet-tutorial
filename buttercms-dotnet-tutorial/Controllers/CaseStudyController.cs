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
            public string facebook_open_graph_title { get; set; }
            public string seo_title { get; set; }
            public string headline { get; set; }
            public string testimonial { get; set; }
            public string customer_logo { get; set; }
        }
        [Route("customers/")]
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10)
        {
            var butterClient = new ButterCMSClient("7409d6a1280930a7271d31c985de5337ee174085");

            var parameterDict = new Dictionary<string, string>()
            {
                {"page", page.ToString()},
                {"page_size", pageSize.ToString()}
            };

            PagesResponse<CaseStudyPage> caseStudyPages = butterClient.ListPages<CaseStudyPage>("*", parameterDict);

            var viewModel = new CaseStudiesViewModel();
            viewModel.PreviousPageNumber = caseStudyPages.Meta.PreviousPage;
            viewModel.NextPageNumber = caseStudyPages.Meta.NextPage;
            viewModel.PagesCount = caseStudyPages.Meta.Count;

            viewModel.CaseStudies = new List<CaseStudyViewModel>();
            foreach (Page<CaseStudyPage> caseStudy in caseStudyPages.Data)
            {
                CaseStudyViewModel caseStudyViewModel = new CaseStudyViewModel();

                caseStudyViewModel.FacebookOGTitle = caseStudy.Fields.facebook_open_graph_title;
                caseStudyViewModel.SeoTitle = caseStudy.Fields.seo_title;
                caseStudyViewModel.Headline = caseStudy.Fields.headline;
                caseStudyViewModel.Testimonial = caseStudy.Fields.testimonial;
                caseStudyViewModel.CustomerLogo = caseStudy.Fields.customer_logo;

                viewModel.CaseStudies.Add(caseStudyViewModel);
            }

            return View(viewModel);
        }


        [Route("customers/{slug}")]
        public async Task<ActionResult> ShowCaseStudy(string slug)
       
        {
            var butterClient = new ButterCMSClient("7409d6a1280930a7271d31c985de5337ee174085");

            PageResponse<CaseStudyPage> caseStudy = await butterClient.RetrievePageAsync<CaseStudyPage>("*", slug);

            var viewModel = new CaseStudyViewModel();
            viewModel.FacebookOGTitle = caseStudy.Data.Fields.facebook_open_graph_title;
            viewModel.SeoTitle = caseStudy.Data.Fields.seo_title;
            viewModel.Headline = caseStudy.Data.Fields.headline;
            viewModel.Testimonial = caseStudy.Data.Fields.testimonial;
            viewModel.CustomerLogo = caseStudy.Data.Fields.customer_logo;

            return View(viewModel);
        }
        
    }
}