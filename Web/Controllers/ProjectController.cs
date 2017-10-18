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
    public class ProjectController : Controller
    {
        //
        // GET: /Project/

        public ActionResult Index()
        {

            var request = new Ucsmy.Framework.PageRequest<D_Project>()
                   {
                       PageIndex = "pageindex".ValueOfQuery().TryInt(1),
                       PageSize = "pagesize".ValueOfQuery().TryInt(10),
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
                request.AddFilter(t => t.CreateTime >= minCreateTime);
            }

            var maxCreateTime = "maxCreateTime".ValueOfQuery("1990-01-01").TryDateTime().Value;
            if (maxCreateTime != "1990-01-01".TryDateTime().Value)
            {
                request.AddFilter(t => t.CreateTime <= maxCreateTime);
            }
            var list = DeploymentDAO.UnitService.CTBDeployment.PageList<D_Project>(request);
            return View(list);
        }


        public ActionResult Create()
        {
            ViewBag.Action = "Create";
            return View("~/Views/Project/Edit.cshtml", new D_Project());
        }

        [HttpPost]
        public ActionResult CreateSubmit()
        {
            var project = new D_Project();
            project.FillWithForm();
            project.ProjectID = Guid.NewGuid().ToString();
            project.CreateUser = "admin";
            project.CreateTime = DateTime.Now;

            var rs = ProjectService.Create(project);
            Response.Write(rs.ToJson());
            return View("~/Views/Shared/ResponseHandle.cshtml");
        }

        public ActionResult Edit(string ProjectID)
        {
            ViewBag.Action = "Edit";
            var project = QueryService.CTBDeployment.Find<D_Project>(t => t.ProjectID == ProjectID);

            if (project.IsNull())
            {
                Response.Write("参数错误");
                return View("~/Views/Shared/ResponseHandle.cshtml");
            }

            return View("~/Views/Project/Edit.cshtml", project);
        }


        [HttpPost]
        public ActionResult EditSubmit()
        {
            var project = new D_Project();
            project.FillWithForm();
            project.CreateUser = "admin";
            project.CreateTime = DateTime.Now;

            var rs = ProjectService.Update(project);
            Response.Write(rs.ToJson());
            return View("~/Views/Shared/ResponseHandle.cshtml");
        }
    }
}
