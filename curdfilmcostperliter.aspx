﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="curdfilmcostperliter.aspx.cs" Inherits="curdfilmcostperliter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="js/date.format.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .container
        {
            max-width: 100%;
        }
        th
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function PopupOpen() {
            $("#dialog").dialog({
                title: "View Details",
                buttons: {
                    Ok: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
            $('#dialog').css('display', 'block');
        }
        function popupCloseClick() {
            $('#dialog').css('display', 'none');
        }
    </script>
    <script type="text/javascript">
        function exportfn() {
            window.location = "exporttoxl.aspx";
        }

        //------------>Prevent Backspace<--------------------//
        $(document).unbind('keydown').bind('keydown', function (event) {
            var doPrevent = false;
            if (event.keyCode === 8) {
                var d = event.srcElement || event.target;
                if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD'))
            || d.tagName.toUpperCase() === 'TEXTAREA') {
                    doPrevent = d.readOnly || d.disabled;
                } else {
                    doPrevent = true;
                }
            }

            if (doPrevent) {
                event.preventDefault();
            }
        });
    </script>
    <script type ="text/javascript">
        function openWin() {
            window.open("http://google.co.in")
        }  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600">
    </asp:ToolkitScriptManager>
    <asp:UpdateProgress ID="updateProgress1" runat="server">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                <br />
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                    AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: absolute;
                    top: 35%; left: 40%;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>
            <section class="content-header">
                <h1>
                    Curd Packing Report<small>Preview</small>
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
                    <li><a href="#">Curd Packing Report</a></li>
                </ol>
            </section>
            <section class="content">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Curd Packing Report Details
                        </h3>
                    </div>
                    <div class="box-body">
                        <div align="center" >
                        <table>
                            <tr>
                                 <td style="padding-left: 3px !important;"></td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Label">From Date</asp:Label>&nbsp;
                                        <asp:TextBox ID="dtp_FromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="dtp_FromDate" Format="dd-MM-yyyy HH:mm">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="padding-left: 3px !important;"></td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Label">To Date</asp:Label>&nbsp;
                                        <asp:TextBox ID="dtp_Todate" runat="server" CssClass="form-control">
                                        </asp:TextBox>
                                        <asp:CalendarExtender ID="enddate_CalendarExtender2" runat="server" Enabled="True"
                                            TargetControlID="dtp_Todate" Format="dd-MM-yyyy HH:mm">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="padding-left: 3px !important;"></td>
                                    <td>
                                        <asp:Button ID="Button2" runat="server" Text="GENERATE" CssClass="btn btn-success" OnClick="btn_Generate_Click" /><br />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl.aspx">Export to XL</asp:HyperLink>
                                    </td>
                                    <%--<td>
                                        <asp:LinkButton ID="LinkButton1" OnClientClick="openWin()" runat="server">LinkButton</asp:LinkButton>
                                    </td>--%>
                                </tr>
                            </table>
                            <asp:Panel ID="hidepanel" runat="server" Visible='false'>
                                <div id="divPrint">
                                    <div style="width: 100%;">
                                        <div style="width: 13%; float: left;">
                                            <img src="Images/Vyshnavilogo.png" alt="Vyshnavi" width="120px" height="82px" />
                                        </div>
                                        <div align="center">
                                            <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="20px" ForeColor="#0252aa"
                                                Text=""></asp:Label>
                                            <br />
                                            <asp:Label ID="lblAddress" runat="server" Font-Bold="true" Font-Size="12px" ForeColor="#0252aa"
                                                Text=""></asp:Label>
                                            <br />
                                            <span style="font-size: 18px; font-weight: bold; color: #0252aa;">Film Consumption Report</span><br />
                                            <asp:Panel ID="pnlVendor" runat="server" Visible="false">
                                                <span style="font-weight: bold;">Vendor Name:</span>
                                                <asp:Label ID="lblVendorName" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#0252aa"
                                                    Text=""></asp:Label>
                                            </asp:Panel>
                                        </div>
                                        <table style="width: 80%">
                                            <tr>
                                                <td>
                                                    From Date
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    To date:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltodate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <div >
                                        <asp:Panel ID="panel_day" runat="server" Visible="false">
                                            <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="gridcls" 
                                                GridLines="Both" Font-Bold="true">
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                                <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                                    Font-Names="Raavi" Font-Size="Small" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                                <AlternatingRowStyle HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            </asp:GridView>
                                            </asp:Panel>
                                        </div>
                                        <br />
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 25%;">
                                                    <span style="font-weight: bold; font-size: 12px;">INCHARGE SIGNATURE</span>
                                                </td>
                                                <td style="width: 25%;">
                                                    <span style="font-weight: bold; font-size: 12px;">ACCOUNTS DEPARTMENT</span>
                                                </td>
                                                <td style="width: 25%;">
                                                    <span style="font-weight: bold; font-size: 12px;">AUTHORISED SIGNATURE</span>
                                                </td>
                                                <td style="width: 25%;">
                                                    <span style="font-weight: bold; font-size: 12px;">PREPARED BY</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <br />
                                <asp:Button ID="btnPrint" runat="Server" CssClass="btn btn-success" OnClientClick="javascript:CallPrint('divPrint');"
                                    Text="Print" />
                            </asp:Panel>
                            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                        </div>
                    </div>
                    
                    <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                </div>
                </div>
                <div id="dialog" style="display: none;" >
                    <asp:GridView ID="GrdProducts" runat="server" ForeColor="White" Width="260%" CssClass="gridcls"
                        GridLines="Both" Font-Bold="true">
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                            Font-Names="Raavi" Font-Size="Small" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                    </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
