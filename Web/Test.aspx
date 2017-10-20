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
                var rs = DeploymentService.DeployService.ExcuteDeployTask("70962f45-8131-45e4-818c-579282910ff1");
                Response.Write(rs.ToJson());
               
                 
                
            %>
        </div>
    </form>
</body>
</html>
