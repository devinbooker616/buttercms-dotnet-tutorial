using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static buttercms_dotnet_tutorial.Controllers.HomeController;

namespace buttercms_dotnet_tutorial.Models
{
    public class HomeViewModel
    {
            public string SeoTitle { get; set; }
            public string Headline { get; set; }
            public string HeroImage { get; set; }
            public string CallToAction { get; set; }
            public List<CustomerLogo> CustomerLogos { get; set; }
    }
}
