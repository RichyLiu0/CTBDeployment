using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ucsmy.Framework.DAL;
using DeploymentDAO;
using Ucsmy.Framework.Common;
using System.Dynamic;
using Deployment.Web.Models;
using DeploymentService;

namespace Deployment.Web.Controllers
{
    public class ProjectCodeServerController : Controller
    {
        //
        // GET: /Project/

        public ActionResult Index()
        {

            var request = new Ucsmy.Framework.PageRequest<D_ProjectCodeServer>()
                   {
                       PageIndex = "pageindex".ValueOfQuery().TryInt(1) - 1,
                       PageSize = "pagesize".ValueOfQuery().TryInt(10),
                       OrderBy = t => t.OrderBy(to => to.CreateTime)
                   };

            var ProjectID = "ProjectID".ValueOfQuery("");
            if (ProjectID != "")
            {
                request.AddFilter(t => t.ProjectID == ProjectID);
            }

            var ServerIndex = "ServerIndex".ValueOfQuery("").TryInt(-1).Value;
            if (ServerIndex != -1)
            {
                request.AddFilter(t => t.ServerIndex == ServerIndex);
            }
            var list = DeploymentDAO.UnitService.CTBDeployment.PageList<D_ProjectCodeServer>(request);
            return View(list);
        }


        public ActionResult Create()
        {
            ViewBag.Action = "Create";
            return View("~/Views/ProjectCodeServer/Edit.cshtml", new D_ProjectCodeServer() { ProjectID = "ProjectID".ValueOfQuery() });
        }

        [HttpPost]
        public ActionResult CreateSubmit()
        {
            var project = new D_ProjectCodeServer();
            project.FillWithForm();
            project.ServerID = Guid.NewGuid().ToString();
            project.CreateUser = "admin";
            project.CreateTime = DateTime.Now;

            var rs = ProjectService.AddProjectCodeServer(project);
            Response.Write(rs.ToJson());
            return View("~/Views/Shared/ResponseHandle.cshtml");
        }

        //    public ActionResult Edit(string ProjectID)
        //    {
        //        ViewBag.Action = "Edit";
        //        var project = QueryService.CTBDeployment.Find<D_Project>(t => t.ProjectID == ProjectID);

        //        if (project.IsNull())
        //        {
        //            Response.Write("参数错误");
        //            return View("~/Views/Shared/ResponseHandle.cshtml");
        //        }

        //        return View("~/Views/Project/Edit.cshtml", project);
        //    }


        //    [HttpPost]
        //    public ActionResult EditSubmit()
        //    {
        //        var project = new D_Project();
        //        project.FillWithForm();
        //        project.CreateUser = "admin";
        //        project.CreateTime = DateTime.Now;

        //        var rs = ProjectService.Update(project);
        //        Response.Write(rs.ToJson());
        //        return View("~/Views/Shared/ResponseHandle.cshtml");
        //    }
    }
}
