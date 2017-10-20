using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeploymentDAO;
using Ucsmy.Framework;
using Ucsmy.Framework.Common;
using DeploymentModel;
using System.IO;

namespace DeploymentService
{
    public class DeployService
    {
        public static ResponseResult PackageUnZip(string packageId)
        {
            var rs = new ResponseResult() { IsSucess = true };

            try
            {
                var package = QueryService.CTBDeployment.Find<D_ProjectPackage>(t => t.PackageID == packageId);
                if (package.IsNull())
                {
                    rs.IsSucess = false;
                    rs.Message = "参数错误,部署包不存在";
                    return rs;
                }
                var savePath = GetDepoymentUnZipPath(package);


                if (!Directory.Exists("PackageUnZipPath".ValueOfAppSettings()))
                {
                    Directory.CreateDirectory("PackageUnZipPath".ValueOfAppSettings());
                }
                if (Directory.Exists(savePath))
                {
                    Directory.Delete(savePath);
                }

                DeploymentUtility.FileScanCommon.UnZipFile(package.PackagePath, savePath);
                rs.Message = "解压成功";

            }
            catch (Exception ex)
            {
                rs.IsSucess = false;
                rs.Message = ex.GetInnerMessage();
            }

            return rs;
        }

        private static string GetDepoymentUnZipPath(D_ProjectPackage package)
        {
            var savePath = "PackageUnZipPath".ValueOfAppSettings() + "/{0}_{1}".FormatWith(package.PackageID, package.Version);
            return savePath;
        }

        public static ResponseResult CreateDeployTask(string packageId)
        {
            var rs = new ResponseResult() { IsSucess = true };



            var package = QueryService.CTBDeployment.Find<D_ProjectPackage>(t => t.PackageID == packageId);
            if (package.IsNull())
            {
                rs.IsSucess = false;
                rs.Message = "参数错误,部署包不存在";
                return rs;
            }

            var project = QueryService.CTBDeployment.Find<D_Project>(t => t.ProjectID == package.ProjectID);
            var servers = QueryService.CTBDeployment.FindList<D_ProjectCodeServer>(t => t.ProjectID == package.ProjectID);

            if (!servers.HasAny())
            {
                rs.IsSucess = false;
                rs.Message = "不存在代码服务器";
                return rs;
            }

            var deployTasks = new List<D_DeployTask>();

            foreach (var server in servers)
            {
                deployTasks.Add(new D_DeployTask()
                {
                    CreateTime = DateTime.Now,
                    TaskID = Guid.NewGuid().ToString(),
                    DeploymentPath = server.Path,
                    DeploymentType = server.DeploymentType,
                    PackageID = package.PackageID,
                    FtpLoginPwd = server.FtpLoginPwd,
                    FtpLoginUser = server.FtpLoginUser,
                    PackageName = package.PackageName,
                    PackageVersion = package.Version,
                    ProejctID = package.ProjectID,
                    ProjectName = project.ProjectName,
                    ProejctCode = project.ProjectCode,
                    ServerID = server.ServerID,
                    ServerIndex = server.ServerIndex,
                    Status = (int)EnumType.DeployTaskStatus.新建,
                });
            }

            try
            {
                using (var uow = UnitService.CTBDeployment)
                {
                    deployTasks.ForEach(t => uow.Create(t));
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                rs.IsSucess = false;
                rs.Message = ex.GetInnerMessage();
            }

            return rs;
        }

        public static ResponseResult ExcuteDeployTask(string DeployTaskId)
        {

            var rs = new ResponseResult() { IsSucess = true };
            try
            {
                var task = QueryService.CTBDeployment.Find<D_DeployTask>(t => t.TaskID == DeployTaskId && t.Status == (int)EnumType.DeployTaskStatus.新建);
                if (task.IsNull())
                {
                    rs.IsSucess = false;
                    rs.Message = "参数错误,布署Task不存在";
                    return rs;
                }
                var package = QueryService.CTBDeployment.Find<D_ProjectPackage>(t => t.PackageID == task.PackageID);
                if (package.IsNull())
                {
                    rs.IsSucess = false;
                    rs.Message = "参数错误,部署包不存在";
                    return rs;
                }
                var fromPath = GetDepoymentUnZipPath(package);
                var toPath = task.DeploymentPath;

                DeploymentUtility.FileScanCommon.FolderCopy(fromPath, toPath);
                rs.Message = "布署成功";
            }
            catch (Exception ex)
            {
                rs.IsSucess = false;
                rs.Message = ex.GetInnerMessage();
            }
            return rs;
        }


    }
}
