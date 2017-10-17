using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Deployment.Web.Models
{
    public class PageModel
    {
        public int TotalCount {get;set;}
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}