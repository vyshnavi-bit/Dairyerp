<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="packingdashboard.aspx.cs" Inherits="packingdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            get_pakingfilm_details();
            get_ghee_Deatails();
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


        function get_pakingfilm_details() {
            var data = { 'op': 'get_pakingfilm_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillpakingentrydetails(msg);
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
        function fillpakingentrydetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color:#5d6677; color: whitesmoke;"><th scope="col">Batch Name</th><th scope="col">Quantity(ltrs)</th><th scope="col">Received Film</th><th scope="col">Cutting Film</th><th scope="col">Wastage Film</th><th scope="col">Return Film</th><th scope="col">Consumption Film</th></tr></thead></tbody>';
            var l = 0;
            var COLOR = ["", "#f3f5f7"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].batch + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].qtyltrs + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].receivedfilm + '</td>';
                results += '<td  class="7" style="text-align:center;">' + msg[i].cuttingfilm + '</td>';
                results += '<td  class="8" style="text-align:center;">' + msg[i].wastagefilm + '</td>';
                results += '<td  class="9" style="text-align:center;">' + msg[i].returnfilm + '</td>';
                results += '<td  class="10">' + msg[i].consumptionfilm + '</td></tr>';
                l = l + 1;
                if (l == 2) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }


        function get_ghee_Deatails() {
            var table1 = document.getElementById("tbl_Buffalo_list");
            for (var i = table1.rows.length - 1; i > 0; i--) {
                table1.deleteRow(i);
            }
            var data = { 'op': 'get_ghee_Deatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        var milktype = msg[i].milktype;
                        var tablerowcnt = document.getElementById("tbl_Buffalo_list").rows.length;
                        $('#tbl_Buffalo_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs">' + msg[i].milktype + '</span></span></th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs">' + msg[i].QtyKgs + '</span></span></th><td><span  class="badge bg-green">' + msg[i].fat + '</span></td><td><span  class="badge bg-green">' + msg[i].snf + '</span></td><td><span  class="badge bg-green">' + msg[i].kgfat + '</span></td><td><span  class="badge bg-green">' + msg[i].kgsnf + '</span></td></tr>');
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

        function get_curd_Deatails() {
            var table1 = document.getElementById("tbl_Buffalo_list");
            for (var i = table1.rows.length - 1; i > 0; i--) {
                table1.deleteRow(i);
            }
            var data = { 'op': 'get_ghee_Deatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        var milktype = msg[i].milktype;
                        var tablerowcnt = document.getElementById("tbl_Buffalo_list").rows.length;
                        $('#tbl_Buffalo_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs">' + msg[i].milktype + '</span></span></th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs">' + msg[i].QtyKgs + '</span></span></th><td><span  class="badge bg-green">' + msg[i].fat + '</span></td><td><span  class="badge bg-green">' + msg[i].snf + '</span></td><td><span  class="badge bg-green">' + msg[i].kgfat + '</span></td><td><span  class="badge bg-green">' + msg[i].kgsnf + '</span></td></tr>');
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">

    <div class="col-md-6" style="width: 100%;">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>   Packing Entry Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding">
                            <div id="div_Deptdata">
                </div>
                        </div>
                    </div>
                </div>
            </div>
        
        <div class="col-md-6" style="display:none;">
                <!-- AREA CHART -->
                <div class="box box-success">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                             <i style="padding-right: 5px;" class="fa fa-cog"></i>  Ghee Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" >
                            <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" id="tbl_Buffalo_list">
                                <tr>
                                    <th >
                                        #
                                    </th>
                                    <th>
                                         Recived Cream Qty
                                    </th>
                                    <th>
                                      Qty(Kgs)
                                    </th>
                                    <th>
                                       FAT
                                    </th>
                                    <th>
                                       SNF
                                    </th>
                                     <th>
                                       KG FAT
                                    </th>
                                    <th>
                                      KG SNF
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
    </section>
</asp:Content>
