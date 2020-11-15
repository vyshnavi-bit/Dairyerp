<%@ Page Language="C#" AutoEventWireup="true" CodeFile="otp.aspx.cs" Inherits="otp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title></title>
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <!-- Theme style -->
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css">
    <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
    <script src="http://code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <%--<script src="js/jquery.autocomplete.min.js" type="text/javascript"></script>--%>
    <style>
        .bcolor
        {
            background: #e0e0e0;
        }
        .fbcolor
        {
            background: "#0073b7";
        }
        .dheight
        {
            height: 590px;
        }
        .frcolor
        {
            color: Red;
            font-size: large;
            font-weight: bold;
        }
        .fgcolor
        {
            color: Green;
            font-size: large;
            font-weight: bold;
        }
        .fsize
        {
            font-size: large;
            font-weight: bold;
        }
        .toppad
        {
            padding-top: 25px;
        }
        .toppad1
        {
            padding-top: 15px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);
        });
    </script>
    <script type="text/jscript">
        function save() {
            var retval = 0;
            var otpval = document.getElementById("txt_otppassword").value;
            PageMethods.Save(otpval, message1);
        }
        function message1(msg) {
            if (msg == 'Transaction Completed Sccessfully...') {
                alert(msg);
                document.getElementById("txt_otppassword").value = "";
                window.open('Dashboard.aspx', '_self');
            }
            else if (msg == 'Please,Check the OTP Data...') {
                alert(msg);
                document.getElementById("txt_otppassword").value = "";
            }
            else if (msg == 'Timeout Error...') {
                alert(msg);
                window.open('Login.aspx', '_self');
            }
            else if (msg == 'Please try again...') {
                alert(msg);
                window.open('Login.aspx', '_self');
            }
        }
        function Resents() {
            var mno = 0;
            PageMethods.Resent(mno, message2);
        }
        function message2(msg) {
            alert(msg);
        }

        function setFocusToTextBox() {
            document.getElementById("txt_otppassword").focus();
        }
        var timeleft = 180;
        var downloadTimer = setInterval(function () {
            timeleft--;
            document.getElementById("countdowntimer").textContent = timeleft;
            if (timeleft <= 0) {
                clearInterval(downloadTimer)
            }
            if (timeleft == 0)
            {
                window.open('Login.aspx', '_self');
            }
        }, 1000);
    </script>
    <script type="text/javascript">
        window.onload = window.history.forward(0);  
    </script>
</head>
<body onload='setFocusToTextBox()' class="bcolor">
    <form id="form1" runat="server">
    <div class="dheight">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePageMethods="true" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdateProgress ID="UpdateProgress" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 1px; width: 1px; top: 0;
                    right: 0; left: 0; z-index: 9999999; background-color: Gray; opacity: 0.7;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="images/Loading.gif" AlternateText="Loading ..."
                        ToolTip="Loading ..." Style="padding: 1px; position: fixed; top: 45%; left: 45%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div align="center" style="padding-top: 6%;">
                    <table width="100%">
                        <tr>
                            <td width="2%" align="right">
                                <img src="Images/Vyshnavilogo.png" style="height: 75px; width: 111px;" />
                            </td>
                            <td width="85%" style="text-align: center; padding-right: 380px;">
                                <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="22px" ForeColor="#ff1493"
                                    Text="SRI VYSHNAVI DAIRY SPECIALITIES PVT LIMITED"></asp:Label>
                                <br />
                                <span style="font-size: 20px; font-weight: bold; color: #0252aa;">OTP Authentication</span>
                            </td>
                            <td width="10%">
                            </td>
                        </tr>
                        <tr>
                            <td width="30%">
                            </td>
                            <td width="60%">
                                <table width="100%">
                                    <tr>
                                        <td class="fsize" colspan="3" style="padding-top: 5%;">
                                            Please enter the One Time Password (OTP) Which is sent to your registered mobile
                                            number.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="toppad" width="25%" style="font-size: large; font-weight: bold;">
                                            Module :
                                        </td>
                                        <td class="toppad" width="25%">
                                            <asp:Label ID="Lbl_Approvaltype" runat="server" Font-Bold="true" Text=""></asp:Label>
                                        </td>
                                        <td width="50%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="toppad1" width="25%" style="font-size: large; font-weight: bold;">
                                            Date :
                                        </td>
                                        <td class="toppad1" width="25%">
                                            <asp:Label ID="Lbl_Date" runat="server" Font-Bold="true" Text=""></asp:Label>
                                        </td>
                                        <td width="50%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="50%" colspan="2" align="center" class="fgcolor" style="padding-top: 5%;">
                                            Successfully sent the One Time Password to your Registered Mobile Number.
                                        </td>
                                        <td width="50%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="25%" class="toppad" style="font-size: large; font-weight: bold;">
                                            One Time Password :
                                        </td>
                                        <td width="50%" class="toppad">
                                            <input type="password" id="txt_otppassword" value="" /><input type="button" id="btn_Resendotp"
                                                value="Resend OTP" onclick="Resents();" style="background-color: #0073b7; height: 25px;
                                                width: 100px; color: white; font-weight: bold;" />
                                        </td>
                                        <td width="25%">
                                        </td>
                            </td>
                        </tr>
                        <tr>
                            <td width="25%" align="right" class="toppad1">
                                <input type="button" id="btn_Save" value="Submit" onclick="save();" style="background-color: #0073b7;
                                    height: 30px; width: 70px; color: white; font-weight: bold;" />
                            </td>
                            <td width="25%" class="toppad1">
                                <input type="button" class="btn btn-primary" id="btn_close" value="Cancel" onclick="cancledeatilse();"
                                    style="background-color: #0073b7; height: 30px; width: 70px; color: white; font-weight: bold;" />
                            </td>
                            <td width="50%">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 3%; padding-left: 10%;" colspan="3" class="frcolor">
                                <p>
                                    This page will automatically timeout after <span id="countdowntimer" style="color:Green;">180</span> seconds.</p> 
                                <br />
                            </td>
                        </tr>
                    </table>
                    </td>
                    <td width="10%">
                    </td>
                    </tr> </table>
                </div>
            </ContentTemplate>
            <%-- <Triggers> 
            </Triggers>--%>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
