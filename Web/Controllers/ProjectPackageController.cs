using DeploymentDAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deployment.Web.Controllers
{
    public class ProjectPackageController : Controller
    {
        //
        // GET: /ProjectPackage/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string projectId)
        {
            ViewBag.Action = "Create";

            return View("~/Views/ProjectPackage/Edit.cshtml", new D_ProjectPackage() { PackageID = projectId });
        }
        public JsonResult FileUpLoad()
        {

            var file = this.Request.Files[0];
            if (file == null)
                return Json(new FileUpLoadResult() { IsOk = false, Message = "文件不存在" });
            if (!file.FileName.EndsWith(".zip"))
                return Json(new FileUpLoadResult() { IsOk = false, Message = "请上传.zip类型的文件" });
            else
            {
                var pathName = this.Server.MapPath("/App_Data/UploadFiles") + "\\" + file.FileName;
                file.SaveAs(pathName);
                return Json(new FileUpLoadResult() { IsOk = true, Message = "文件上传成功", FileName = file.FileName, FilePath = pathName });
            }

        }

    }
    public class FileUpLoadResult
    {
        public bool IsOk { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
