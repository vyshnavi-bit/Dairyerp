<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="milkagedashboard.aspx.cs" Inherits="milkagedashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            get_milkagedashbord_details();
            get_Buffalomilkagedashbord_details();
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


        function get_milkagedashbord_details() {
            var data = { 'op': 'get_milkagedashbord_details' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    var l = 0;
                    var COLOR = ["", "#f3f5f7"];
                    for (var i = 0; i < msg.length; i++) {
                        if (msg[i].milktype == "1") {
                            var tablerowcnt = document.getElementById("tbl_Cow_list").rows.length;
                            $('#tbl_Cow_list').append('<tr style="background-color:' + COLOR[l] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].branchname + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs">' + msg[i].qtykgs + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].qtyltrs + '</span></span></th></tr>');
                            j++;
                            l = l + 1;
                            if (l == 2) {
                                l = 0;
                            }
                        }
                    }
                    var tot = "Total";
                    $('#tbl_Cow_list').append('<tr><td data-title="categorysno"></td><th scope="Category Name">' + tot + '</th><th scope="Category Name" ><span class="badge bg-yellow"><span id="ltrclass"></span></span></th><th scope="Category Name"><span class="badge bg-yellow"><span id="Kgsclass"></span></span></th></tr>');
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

        function get_Buffalomilkagedashbord_details() {
            var data = { 'op': 'get_milkagedashbord_details' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    var l = 0;
                    var COLOR = ["", "#f3f5f7"];
                    for (var i = 0; i < msg.length; i++) {
                        if (msg[i].milktype == "2") {
                            var tablerowcnt = document.getElementById("tbl_Buffalo_list").rows.length;
                            $('#tbl_Buffalo_list').append('<tr style="background-color:' + COLOR[l] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].branchname + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgss">' + msg[i].qtykgs + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltrs">' + msg[i].qtyltrs + '</span></span></th></tr>');
                            j++;
                            l = l + 1;
                            if (l == 2) {
                                l = 0;
                            }
                        }
                    }
                    var tot = "Total";
                    $('#tbl_Buffalo_list').append('<tr><td data-title="categorysno"></td><th scope="Category Name">' + tot + '</th><th scope="Category Name" ><span class="badge bg-yellow"><span id="ltrsclass"></span></span></th><th scope="Category Name"><span class="badge bg-yellow"><span id="Kgssclass"></span></span></th></tr>');
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
            var totalltr = 0;
            $('.clsqtyltr').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalltr += parseFloat(qtyclass);
                }

            });
            var totalkgs = 0;
            $('.clsqtykgs').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalkgs += parseFloat(qtyclass);
                }

            });
            document.getElementById('Kgsclass').innerHTML = parseFloat(totalltr).toFixed(2);
            document.getElementById('ltrclass').innerHTML = parseFloat(totalkgs).toFixed(2);
        }


        function GetbTotalCal() {
            var totalltr = 0;
            $('.clsqtyltrs').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalltr += parseFloat(qtyclass);
                }

            });
            var totalkgs = 0;
            $('.clsqtykgss').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalkgs += parseFloat(qtyclass);
                }

            });
            document.getElementById('Kgssclass').innerHTML = parseFloat(totalltr).toFixed(2);
            document.getElementById('ltrsclass').innerHTML = parseFloat(totalkgs).toFixed(2);
        }



        function fillpakingentrydetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Branch Name</th><th scope="col">Quantity(ltrs)</th><th scope="col">Quantity(kgs)</th><th scope="col">Fat</th><th scope="col">Snf</th><th scope="col">Kgfat</th><th scope="col">Kgsnf</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].branchname + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].qtyltrs + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].qtykgs + '</td>';
                results += '<td  class="7" style="text-align:center;">' + msg[i].fat + '</td>';
                results += '<td  class="8" style="text-align:center;">' + msg[i].snf + '</td>';
                results += '<td  class="9" style="text-align:center;">' + msg[i].kgfat + '</td>';
                results += '<td  class="9" style="text-align:center;">' + msg[i].kgsnf + '</td>';
                results += '<td  class="10">' + msg[i].milkage + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
            <div class="col-md-6">
                <!-- AREA CHART -->
                <div class="box box-success">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                             <i style="padding-right: 5px;" class="fa fa-cog"></i>  Cow Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" >
                            <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" id="tbl_Cow_list">
                                <tr style="background-color:#5d6677; color: whitesmoke;">
                                    <th >
                                        #
                                    </th>
                                    <th>
                                         CC Name
                                    </th>
                                    <th>
                                      Qty(Kgs)
                                    </th>
                                    <th>
                                       Qty(Ltr)
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
                             <i style="padding-right: 5px;" class="fa fa-cog"></i>  Buffalo Milk Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" >
                            <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" id="tbl_Buffalo_list">
                                <tr style="background-color:#5d6677; color: whitesmoke;">
                                    <th >
                                        #
                                    </th>
                                    <th>
                                         CC Name
                                    </th>
                                    <th>
                                      Qty(Kgs)
                                    </th>
                                    <th>
                                       Qty(Ltr)
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
    </section>
</asp:Content>

