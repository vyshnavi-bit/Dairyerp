<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="xmlexport.aspx.cs" Inherits="xmlexport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Packing Entry Details
                </h3>
            </div>
            <div class="box-body">
                
                <div id="div_Deptdata">
                </div>
                <div id='Inwardsilo_fillform'>
                    <table align="center">
                        <tr>
                            <td>
                              <asp:Button ID="btnexport" runat="server" OnClick="btnexport_click" Text="export" />
                              
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

