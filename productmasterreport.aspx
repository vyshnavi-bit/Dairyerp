<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="productmasterreport.aspx.cs" Inherits="productmasterreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">
     $(function () {
         get_allproduct_details();
     });
        function callHandler(d, s, e) {
            $.ajax({
                url: 'FleetManagementHandler.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        function get_allproduct_details() {
            var data = { 'op': 'get_allproduct_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillproductiondetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillproductiondetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">PRODUCTID</th><th scope="col">PRODUCTNAME</th><th scope="col">PRODUCTCODE</th><th scope="col">CATEGORYCODE</th><th scope="col">PRICE</th><th scope="col">DEPARTMENT</th><th scope="col">BATCH</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td class="3" style="text-align:center;">' + msg[i].productid + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].productcode + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].categorycode + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].price + '</td>';
                results += '<td class="3" style="text-align:center;">' + msg[i].departmentname + '</td>';
                results += '<td class="3" style="text-align:center;">' + msg[i].batch + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_getdata").html(results);
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            Product Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Product Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Product Details
                </h3>
            </div>
            <div class="box-body">
            <br />
            <br />
                <div id="div_getdata">
                </div>
                <br />
                     <table style="width: 100%;">
                        <tr>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 12px;">INCHARGE SIGNATURE</span>
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
    </section>
</asp:Content>
