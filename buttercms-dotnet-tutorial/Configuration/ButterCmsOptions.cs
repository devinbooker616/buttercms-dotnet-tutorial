using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buttercms_dotnet_tutorial.Configuration
{
    public class ButterCmsOptions
    {
        public string ApiKey { get; set; } = "YOUR KEY";
        public int BlogPostsPerPage { get; set; }
        public string PrimaryAuthorSlug { get; set; }
    }
}
