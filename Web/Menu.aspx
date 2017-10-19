<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="Styles/common.css" rel="stylesheet" type="text/css" />
    <link href="Styles/main.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/indexevent.js?v=3" type="text/javascript"></script>
    <style type="text/css">
        .menuSH {
            color: red;
            font-size: 20px;
            font-weight: bold;
            cursor: pointer;
        }
    </style>
    <script type="text/javascript">

        function SetMenuGroup(menus) {
            $("[menugroup]").hide().each(function () {
                var item = $(this);
                if (menus.indexOf(item.attr("menugroup")) > -1) {
                    item.show();
                }
            });
        }

    </script>
</head>
<body>
    <div class="leftSidebar" id="leftSidebar">
        <h3>

            <span id="lbltime"></span>
        </h3>
        <div class="navBg">
            <h2>
                <span>功能菜单</span></h2>
            <ul class="leftNav" id="menu_left_ucsmy">

                <li menugroup=",项目管理,">
                    <label class="yj">
                        <a href="javascript:;">项目管理</a></label>
                    <ul class="two">

                        <li class="first end"><a target="showframe" nav="/Project/Index">
                            <span>项目管理</span></a> </li>
                    </ul>
                </li>
                
            </ul>
        </div>

    </div>
    <div class="rightSidebar" id="rightSidebar">
        <div class="labelBox">

            <div class="label Toggle_1" id="Toggletitle">
                <ul>
                </ul>
            </div>

        </div>
    </div>
</body>
</html>

