using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Deployment.Web.Models
{
    public class ProjectCodeServerListVM
    {
            public class iListItem
            { 
                public string ServerID { get; set; }
                public int ServerIndex { get; set; }
                public int DeploymentType { get; set; }
                public string ProjectID { get; set; }
                public string ProjectCode { get; set; }
                public string ProjectName { get; set; }
                public string Path { get; set; }
                public string FtpLoginUser { get; set; }
                public string FtpLoginPwd { get; set; }
                public string Remark { get; set; }
                public DateTime CreateTime { get; set; }
                public string CreateUser { get; set; }
            }
            
    }
}