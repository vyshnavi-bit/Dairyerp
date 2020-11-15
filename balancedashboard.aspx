<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="balancedashboard.aspx.cs" Inherits="balancedashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            get_receivebles_details();
            get_payble_details();
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


        function get_receivebles_details() {
            var data = { 'op': 'get_receivebles_details' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        document.getElementById("lbldate").innerHTML = msg[i].doe;
                        if (msg[i].vendortype == "Vendor" && msg[i].branchtype != "Inter Branch") {
                            var tablerowcnt = document.getElementById("tbl_vendor_list").rows.length;
                            if (msg[i].amount > 0) {
                                $('#tbl_vendor_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vendorname + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs" style="font-size:16px;">' + msg[i].amount + '</span></span></th></tr>');
                                j++;
                            }
                            else {
                                $('#tbl_vendor_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vendorname + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-red"><span class="clsqtykgs" style="font-size:16px;">' + msg[i].amount + '</span></span></th></tr>');
                                j++;
                            }
                        }
                    }
                    var tot = "Total";
                    $('#tbl_vendor_list').append('<tr><td data-title="categorysno"></td><th scope="Category Name" style="font-size:16px;">' + tot + '</th><th scope="Category Name" ><span class="badge bg-yellow"><span id="ltrclass" style="font-size:16px;"></span></span></th><td data-title="IsTransport" ></td></tr>');
                    GetTotalCal();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function get_payble_details() {
            var data = { 'op': 'get_receivebles_details' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        if (msg[i].vendortype == "Client") {
                            var tablerowcnt = document.getElementById("tbl_client_list").rows.length;
                            if (msg[i].amount > 0) {
                                $('#tbl_client_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vendorname + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs" style="font-size:16px;">' + msg[i].amount + '</span></span></th></tr>');
                                j++;
                            }
                            else {
                                $('#tbl_client_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vendorname + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-red"><span class="clsqtykgs" style="font-size:16px;">' + msg[i].amount + '</span></span></th></tr>');
                                j++;
                            }

                        }
                    }
                    var tot = "Total";
                    $('#tbl_client_list').append('<tr><td data-title="categorysno"></td><th scope="Category Name" >' + tot + '</th><th scope="Category Name" ><span class="badge bg-yellow"><span id="ltrsclass" style="font-size:16px;"></span></span></th><td data-title="IsTransport" ></td></tr>');
                    GetbTotalCal();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function GetTotalCal() {
            var totalkgs = 0;
            $('.clsqtykgs').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalkgs += parseFloat(qtyclass);
                }

            });
            document.getElementById('ltrclass').innerHTML = parseFloat(totalkgs).toFixed(2);
        }


        function GetbTotalCal() {
            var totalltr = 0;
            $('.clsqtykgs').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalltr += parseFloat(qtyclass);
                }
            });
            //document.getElementById('Kgssclass').innerHTML = parseFloat(totalltr).toFixed(2);
            document.getElementById('ltrsclass').innerHTML = parseFloat(totalltr).toFixed(2);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <div style="padding-left: 390px; font-size: large; color: red;">
            <table>
                <tr>
                    <td>
                        As On Date :
                    </td>
                    <td>
                        <label id="lbldate">
                        </label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-md-6">
            <!-- AREA CHART -->
            <div class="box box-success">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Payables Details</h3>
                    <div class="box-tools pull-right">
                        <button class="btn btn-box-tool" data-widget="collapse">
                            <i class="fa fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="box-body">
                    <div class="box-body no-padding">
                        <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info"
                            id="tbl_vendor_list">
                            <tr>
                                <th>
                                    #
                                </th>
                                <th>
                                    Name
                                </th>
                                <th>
                                    Amount
                                </th>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <!-- AREA CHART -->
            <div class="box box-success">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Receivebles Details</h3>
                    <div class="box-tools pull-right">
                        <button class="btn btn-box-tool" data-widget="collapse">
                            <i class="fa fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="box-body">
                    <div class="box-body no-padding">
                        <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info"
                            id="tbl_client_list">
                            <tr>
                                <th>
                                    #
                                </th>
                                <th>
                                    Name
                                </th>
                                <th>
                                    Amount
                                </th>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
