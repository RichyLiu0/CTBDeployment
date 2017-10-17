using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ucsmy.Framework.DAL;
using DeploymentDAO;
using Ucsmy.Framework.Common;
using System.Dynamic;

namespace Deployment.Web.Controllers
{
    public class ProjectController : Controller
    {
        //
        // GET: /Project/

        public ActionResult Index()
        {

            var request = new Ucsmy.Framework.PageRequest<D_Project>()
                   {
                       PageIndex = "PageIndex".ValueOfQuery().TryInt(0),
                       PageSize = "PageSize".ValueOfQuery().TryInt(10),
                       OrderBy = t => t.OrderBy(to => to.CreateTime)
                   };

            var ProjectID = "ProjectID".ValueOfQuery("");
            if (ProjectID != "")
            {
                request.AddFilter(t => t.ProjectID == ProjectID);
                //request.AddFilter(" AND ProjectID =@ProjectID").P("ProjectID", ProjectID);
            }

            var ProjectName = "ProjectName".ValueOfQuery("");
            if (ProjectName != "")
            {
                request.AddFilter(t => t.ProjectName == ProjectName);
            }

            var ProjectCode = "ProjectCode".ValueOfQuery("");
            if (ProjectCode != "")
            {
                request.AddFilter(t => t.ProjectCode == ProjectCode);
            }

            var minCreateTime = "minCreateTime".ValueOfQuery("1990-01-01").TryDateTime().Value;
            if (minCreateTime != "1990-01-01".TryDateTime().Value)
            {
                request.AddFilter(t => t.CreateTime >=minCreateTime);
            }

            var maxCreateTime = "maxCreateTime".ValueOfQuery("1990-01-01").TryDateTime().Value;
            if (maxCreateTime != "1990-01-01".TryDateTime().Value)
            {
                request.AddFilter(t => t.CreateTime <= maxCreateTime);
            }   
            var list = DeploymentDAO.UnitService.CTBDeployment.PageList<D_Project>(request);
            return View(list);
        }


    }
}
