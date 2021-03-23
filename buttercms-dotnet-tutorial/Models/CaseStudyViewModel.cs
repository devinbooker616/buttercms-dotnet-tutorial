using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buttercms_dotnet_tutorial.Models
{
    public class CaseStudyViewModel
    {
        public string Readme { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }

        public Dictionary<string, string> Seo { get; set; }

        public Dictionary<string, string> twitterCard { get; set; }

        public Dictionary<string, string> openGraph { get; set; }

    }
}
