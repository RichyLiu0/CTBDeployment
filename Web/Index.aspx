<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>部署管理</title>
    <script type="text/javascript">
        function MainSetMenuGroup(menus) {
            window.frames["ifleftmenu"].SetMenuGroup(menus)
        }
        function MainInitMenuGroup(menus) {
            window.frames["iftopmenu"].InitMenuGroup(menus)
        }
    </script>
</head>
<frameset rows="80,*" frameborder="no" framespacing="0">
  <frame src="Top.aspx" noresize="noresize" scrolling="no" id="iftopmenu" name="iftopmenu"  />
    <frame src="Menu.aspx?menugroup=c" noresize="noresize" scrolling="no" id="ifleftmenu" name='ifleftmenu'/>
    
</frameset>
<noframes>
</noframes>
</html>
