<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <!-- Basics -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Login</title>
     <link rel="icon" href="Images/vyshnavilogo.PNG" type="image/x-icon" title="Vyshnavi" />
    <!-- CSS -->
    <link href="css/login/animate.css?v=125" rel="stylesheet" type="text/css" />
    <link href="css/login/reset.css?v=125" rel="stylesheet" type="text/css" />
     <link href="css/login/styles.css?v=126" rel="stylesheet" type="text/css" />
      <style>
            body
            {
                background: url(Images/Planterp.jpg) no-repeat fixed;
                margin: 0px;
                -webkit-background-size: cover;
                -moz-background-size: cover;
                -o-background-size: cover;
                background-size: cover;
            }
            .alertBox
            {
                position: absolute;
                top: 100px;
                left: 50%;
                width: 500px;
                margin-left: -250px;
                background-color: #fff;
                border: 1px solid #ccc;
                border-radius: 4px;
                box-sizing: border-box;
                padding: 4px 8px;
                text-align:center;
            }
            .modalBackground
            {
                background-color: Black;
                filter: alpha(opacity=90);
                opacity: 0.8;
            }
            .modalPopup
            {
                background-color: #FFFFFF;
                border-width: 3px;
                border-style: solid;
                border-color: black;
                padding-top: 10px;
                padding-left: 10px;
                width: 300px;
                height: 140px;
            }
    </style>
</head>
<body>
    <!-- Begin Page Content -->
    <div id="container">
        <form id="Form1" runat="server">
       <label for="name">
            Username:</label>
      <asp:TextBox ID="Usernme_txt" runat="server" placeholder="Username" type="name" CssClass="txtpassword"></asp:TextBox>
      <label for="username">
            Password:</label>
     <asp:TextBox ID="Pass_pas" runat="server" placeholder="Password" type="password" CssClass="txtpassword"></asp:TextBox>
     <asp:Label ID="lbl_passwords" runat="server" Visible="false"></asp:Label>
     <asp:Label ID="lbl_username" runat="server" Visible="false"></asp:Label>
     <asp:Label ID="lbl_messsge" runat="server" Visible="false"></asp:Label>
        <a href="#" style="font-family: Monospace; font-size: 14px; margin-left: 45%;">Forgot
            your password?</a>
        <div id="lower">
            <asp:Button ID="login_btn" Text="Login" runat="server" OnClick="login_click" class="small" />
        </div>
        <div >
            <asp:Label Text="" runat="server" ID="lbl_validation" ForeColor="Blue" Font-Bold="true" Font-Size="18px"></asp:Label>
            <asp:Panel ID="hdnpnl_logoutsession" runat="server" Visible="false" >
                <%--<asp:Button ID="btn_logoutsession" Text="Kill All Sessions" runat="server" OnClick="sessionsclick_click" class="small" style="margin-top: 0px;background-color: #03a9f4;" />--%>
           </asp:Panel>
           <!--Alert Box-->
                <div runat="server" id="AlertBox" class="alertBox" Visible="false">
                <div runat="server" id="AlertBoxMessage"></div>
                    <asp:Label Text="" runat="server" ID="lbl_validations" ForeColor="Blue" Font-Bold="true" Font-Size="18px" style="padding-top: 3%" ></asp:Label>
                    <table>
                    <tr>
                    <td style="padding-top: 10%;padding-bottom: 4%;">
                <asp:Panel ID="Panel1" runat="server" >
                    <asp:Button ID="btn_logoutsession" Text="Kill All Sessions" runat="server" OnClick="sessionsclick_click" class="small" style="background-color: #03a9f4;" />
                  </asp:Panel>
                  </td>
                  <td>
                  <asp:Panel ID="Panel2" runat="server" style="padding-left: 17%;" >
                    <asp:Button ID="Button1" Text="Close" runat="server" OnClick="sessionsclick_Close" class="small" style="background-color: #03a9f4;" />
                </asp:Panel>
                </td></tr></table>
            </div>
        </div>
       </form>
    </div>
</body>
</html>
