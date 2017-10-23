using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Deployment.Web.Models
{
    public class UploadFileModel
    {
        public string FileId { get; set; }
        public string Control { get; set; }
        public string Action { get; set; }
        public string JSCallBack { get; set; }
    }
}