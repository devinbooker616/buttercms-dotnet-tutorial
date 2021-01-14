using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buttercms_dotnet_tutorial.Configuration
{
    public class ButterCmsOptions
    {
        public string ApiKey { get; set; } = "7409d6a1280930a7271d31c985de5337ee174085";
        public int BlogPostsPerPage { get; set; }
        public string PrimaryAuthorSlug { get; set; }
    }
}
