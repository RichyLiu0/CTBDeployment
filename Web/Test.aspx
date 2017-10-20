<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%
                //1解压
                //var rs = DeploymentService.DeployService.PackageUnZip("B6D1B3FC-06E2-432C-AC15-7A67CAFB65EA");
                //Response.Write(rs.ToJson());

                //2创建部署Task
                //var rs = DeploymentService.DeployService.CreateDeployTask("B6D1B3FC-06E2-432C-AC15-7A67CAFB65EA");
                //Response.Write(rs.ToJson());
                
                
               //3执行Task
               //  var rs = DeploymentService.DeployService.ExcuteDeployTask("32b42177-9d9a-4e36-b410-57a834c93a6b");
               // Response.Write(rs.ToJson());
                var rs = DeploymentService.DeployService.ExcuteDeployTask("89332250-af2d-4986-8755-289c8c696131");
                Response.Write(rs.ToJson());
                 
                
            %>
        </div>
    </form>
</body>
</html>
