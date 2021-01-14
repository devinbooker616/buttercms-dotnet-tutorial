using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buttercms_dotnet_tutorial.Models
{
    public class CaseStudiesViewModel
    {
        public List<CaseStudyViewModel> CaseStudies { get; set; }
        public int? PreviousPageNumber { get; set; }
        public int? NextPageNumber { get; set; }
        public int PagesCount { get; set; }

    }
}
