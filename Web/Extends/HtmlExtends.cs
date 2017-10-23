using Deployment.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
 


namespace Deployment.Web 
{
    public static class HtmlExtends
    {
        public static IHtmlString UploadFile(this HtmlHelper helper, UploadFileModel model)
        {
            return PartialExtensions.Partial(helper, "/Views/Shared/UploadFile.cshtml", model);
        }
    }
}