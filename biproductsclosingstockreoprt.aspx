<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="biproductsclosingstockreoprt.aspx.cs" Inherits="biproductsclosingstockreoprt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
 $(function () {
            $('#head1').css('display', 'none');
            var today = new Date();
            var dd = today.getDate() - 1;
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var hrs = today.getHours();
            var mnts = today.getMinutes();
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
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
        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            d = d.replace(/&/g, '\uFF06');
            d = d.replace(/#/g, '\uFF03');
            d = d.replace(/\+/g, '\uFF0B');
            d = d.replace(/\=/g, '\uFF1D');
            $.ajax({
                type: "GET",
                url: "FleetManagementHandler.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        function get_biproductproduction() {
            var doe = document.getElementById('txt_date').value;
            var data = { 'op': 'get_biproduct_stock_details', 'doe': doe };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbiproductsproductiondetails(msg);
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillbiproductsproductiondetails(msg) {
            var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Product Name</th><th scope="col">Opening Stock</th><th scope="col">Recived Qty</th><th scope="col"> Production</th><th scope="col">Sale</th><th scope="col">Closing Balance</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].biproductopening + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].biproductrecive + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].biproductproduction + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].biproductsale + '</td>';
                results += '<td class="3">' + msg[i].biproductclosing + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_biproductsdata").html(results);
        }
        function CallPrint() {
            var divToPrint = document.getElementById("divprint");
            var newWin = window.open('', 'Print-Window', 'width=100%,height=100%,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        var replaceHtmlEntites = (function () {
            var translate_re = /&(nbsp|amp|quot|lt|gt);/g;
            var translate = {
                "nbsp": " ",
                "amp": "&",
                "quot": "\"",
                "lt": "<",
                "gt": ">"
            };
            return function (s) {
                return (s.replace(translate_re, function (match, entity) {
                    return translate[entity];
                }));
            }
        })();
        function get_stockdetails_click() {
            $('#head1').css('display', 'block');
            get_biproductproduction();
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            Bi-Products Stock Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#"> Bi-Products Stock Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Bi-Products Stock Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <table align="center" style="width: 30%;">
                        <tr>
                            <td>
                                <label>
                                    Date<span style="color: red;">*</span></label>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode" />
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td>
                                <br />
                                <input id='get_stockdetails' type="button" class="btn btn-success" name="submit"
                                    value='Get' onclick="get_stockdetails_click()" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <div id="head1">
                        <h3 class="box-title">
                            Bi-Products Stock Details
                        </h3>
                    </div>
                    <div id="div_biproductsdata" style="width: 100%;">
                    </div>
                    <br />
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
            </div>
    </section>
</asp:Content>

