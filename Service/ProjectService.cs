using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeploymentDAO;
using Ucsmy.Framework;
using Ucsmy.Framework.Common;


namespace DeploymentService
{
    public class ProjectService
    {
        public static ResponseResult Create(D_Project project)
        {
            var rs = new ResponseResult() { IsSucess = true };


            if (project.ProjectCode.IsNullOrEmpty())
            {
                rs.IsSucess = false;
                rs.Message = "项目编号不能为空";
                return rs;
            }
            if (project.ProjectName.IsNullOrEmpty())
            {
                rs.IsSucess = false;
                rs.Message = "项目名称不能为空";
                return rs;
            }


            if (DeploymentDAO.QueryService.CTBDeployment.Exist<D_Project>(t => t.ProjectCode == project.ProjectCode))
            {
                rs.IsSucess = false;
                rs.Message = "项目编号已存在";
                return rs;
            }



            using (var uow = DeploymentDAO.UnitService.CTBDeployment)
            {
                uow.Create(project);
                uow.Commit();
            }

            rs.IsSucess = true;
            rs.Message = "保存成功";

            return rs;
        }


        public static ResponseResult Update(D_Project project)
        {
            var rs = new ResponseResult() { IsSucess = true };


            if (project.ProjectCode.IsNullOrEmpty())
            {
                rs.IsSucess = false;
                rs.Message = "项目编号不能为空";
                return rs;
            }
            if (project.ProjectName.IsNullOrEmpty())
            {
                rs.IsSucess = false;
                rs.Message = "项目名称不能为空";
                return rs;
            }


            var updateProject = QueryService.CTBDeployment.Find<D_Project>(t => t.ProjectID == project.ProjectID);

            if (updateProject.IsNull())
            {
                rs.IsSucess = false;
                rs.Message = "项目不存在";
                return rs;
            }

            using (var uow = DeploymentDAO.UnitService.CTBDeployment)
            {
                updateProject.ProjectName = project.ProjectName;
                updateProject.Remark = project.Remark;
                uow.Update(project);
                uow.Commit();
            }

            rs.IsSucess = true;
            rs.Message = "保存成功";

            return rs;
        }
    }
}
