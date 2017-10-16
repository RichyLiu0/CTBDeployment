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

            var list = DeploymentDAO.UnitService.CTBDeployment.PageList<D_Project>(request);
            return View(list);
        }


    }
}
