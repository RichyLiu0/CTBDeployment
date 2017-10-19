<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
    <link href="styles/common.css" rel="stylesheet" type="text/css" />
    <link href="styles/topLeft.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <style type="text/css">
        .topMenuItem
        {
            padding: 5px 15px 5px 15px;
            font-size: 16px;
            font-weight: bold;
            border: solid 1px black;
            cursor: pointer;
            background-color: #f0f8fa;
        }
    </style>
    <script type="text/javascript">
        function opennewtab(menu) {
            window.parent.document.getElementsByTagName("frame")["ifleftmenu"].contentWindow.opentab(menu);
        }
        function MenuGroupClick(menus, btn) {
            top.MainSetMenuGroup(menus);
            if (btn) {
                $(".topMenuItem").css("background-color", "#f0f8fa");
                $(".topMenuItem").css("color", "Black");
                $(btn).css("background-color", "#36a3e7");
                $(btn).css("color", "White");
            }
        }
    </script>
</head>
<body>
    <div class="top">
        <div class="logo">
            <%--<img alt="" src="/images/logo1.png" />--%>
          
        </div>
        <div class="topBotton">
        </div>
        <div class="topInfo" style="margin-right: 0px;">
    
        </div>

    </div>
    <div class="topLine">
    </div>
</body>
</html> 
