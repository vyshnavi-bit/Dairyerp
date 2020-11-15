<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="importxl.aspx.cs" Inherits="importxl" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="div_importdeduction">
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
        <div class="box-header with-border">
            <h3 class="box-title">
                <i style="padding-right: 5px;" class="fa fa-cog"></i>Import Data
                Details
            </h3>
        </div>
        <div class="box-body">
            <table>
                <tr>
                   
                    <td>
                        <asp:FileUpload ID="FileUploadToServer" runat="server" Style="height: 25px; font-size: 16px;" />
                    </td>
                    <td style="width: 5px;">
                    </td>
                    <td>
                        <asp:Button ID="btnImport" runat="server" Text="Import" class="btn btn-primary" OnClick="btnImport_Click" />
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text=" NOTE : Date formet must be MM/DD/YYYY"
                            Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <asp:UpdatePanel ID="updPanel" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grvExcelData" runat="server">
                        <HeaderStyle BackColor="#df5015" Font-Bold="true" ForeColor="White" />
                    </asp:GridView>
                    </dr>
                    <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" />
                    <br />
                    <br />
                    <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label><br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl.aspx">Export to XL</asp:HyperLink>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label><br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
