﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="otherpartydirectsalesvocher.aspx.cs" Inherits="otherpartydirectsalesvocher" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    </script>
    <script type="text/javascript">
        function exportfn() {
            window.location = "exporttoxl.aspx";
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
                    Direct Sales voucher<small>Preview</small>
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
                    <li><a href="#">Direct Sales Voucher Report</a></li>
                </ol>
            </section>
            <section class="content">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i> Tanker Direct Sales Details
                        </h3>
                    </div>
                    <div class="box-body">
                        <div align="center">
                            <table>
                                <tr>
                                <td>
                                        <asp:Label ID="Label6" runat="server" Text="Label">Branch Type</asp:Label>&nbsp;
                                        <asp:DropDownList ID="ddlbranchType" runat="server" CssClass="form-control" >
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Inter Branches</asp:ListItem>
                                            <asp:ListItem>Other Branches</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Label">Type</asp:Label>&nbsp;
                                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="True">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Branch Wise</asp:ListItem>
                                            <asp:ListItem>Vehicle Wise</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Panel ID="hideVehicles" runat="server" Visible="false">
                                            <asp:Label ID="Label1" runat="server" Text="Label">Branch Name</asp:Label>&nbsp;
                                            <asp:DropDownList ID="ddlbranches" runat="server" CssClass="ddlclass">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Label">Milk Type</asp:Label>&nbsp;
                                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Buffalo</asp:ListItem>
                                            <asp:ListItem>Cow</asp:ListItem>
                                            <asp:ListItem>Skim</asp:ListItem>
                                            <asp:ListItem>Condensed</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Label">From Date</asp:Label>&nbsp;
                                        <asp:TextBox ID="dtp_FromDate" runat="server" CssClass="txtClass"></asp:TextBox>
                                        <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="dtp_FromDate" Format="dd-MM-yyyy HH:mm">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Label">To Date</asp:Label>&nbsp;
                                        <asp:TextBox ID="dtp_Todate" runat="server" CssClass="txtClass">
                                        </asp:TextBox>
                                        <asp:CalendarExtender ID="enddate_CalendarExtender2" runat="server" Enabled="True"
                                            TargetControlID="dtp_Todate" Format="dd-MM-yyyy HH:mm">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="Button2" runat="server" Text="GENERATE" CssClass="btn btn-success"/><br />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl.aspx">Export to XL</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="hidepanel" runat="server" Visible='false'>
                                <div id="divPrint">
                                    <div style="width: 100%;">
                                        <div>
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
                                        </div>
                                        <div>
                                            <asp:Panel ID="pnlcow" runat="server" Visible="false">
                                                <asp:GridView ID="grdcow" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
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
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


