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
