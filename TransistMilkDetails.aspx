<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="TransistMilkDetails.aspx.cs" Inherits="TransistMilkDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            GetInTrasistMilkDeatails();
            Get_weighMilk_Deatails();
            GetlabDeatails();
            GetunlodingDeatails();
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
        function GetunlodingDeatails() {
            var table = document.getElementById("tbl_unload_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Get_unload_Deatails' };
            var s = function (msg) {
                if (msg) {
                    if (msg) {
                        var j = 1;
                        var l = 0;
                        var COLOR = ["", "#f3f5f7"];
                        for (var i = 0; i < msg.length; i++) {
                            var tablerowcnt = document.getElementById("tbl_Intrasist_list").rows.length;
                            $('#tbl_unload_list').append('<tr style="background-color:' + COLOR[l] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehciecleno + '</th><th scope="Category Name">' + msg[i].milktype + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs">' + msg[i].fat + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].snf + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].dcdate + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].dcdtime + '</span></span></th><td data-title="IsTransport" ></td></tr>');
                            j++;
                            l = l + 1;
                            if (l == 2) {
                                l = 0;
                            }
                        }
                    }
                    else {
                    }
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Get_weighMilk_Deatails() {
            var table = document.getElementById("tbl_weighMilk_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Get_weighMilk_Deatails' };
            var s = function (msg) {
                if (msg) {
                    if (msg) {
                        var j = 1;
                        var l = 0;
                        var COLOR = ["", "#f3f5f7"];
                        for (var i = 0; i < msg.length; i++) {
                            var tablerowcnt = document.getElementById("tbl_Intrasist_list").rows.length;
                            $('#tbl_weighMilk_list').append('<tr style="background-color:' + COLOR[l] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehciecleno + '</th><th scope="Category Name">' + msg[i].milktype + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="wclsqtykgs">' + msg[i].QtyKgs + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="wclsqtyltr">' + msg[i].QtyLtr + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].dcdate + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].dcdtime + '</span></span></th><td data-title="IsTransport" ></td></tr>');
                            j++;
                            l = l + 1;
                            if (l == 2) {
                                l = 0;
                            }
                        }
                        var tot = "Total";
                        $('#tbl_weighMilk_list').append('<tr><td data-title="categorysno"></td><th scope="Category Name"></th><th scope="Category Name">' + tot + '</th><th scope="Category Name" ><span class="badge bg-yellow"><span id="wltrclass"></span></span></th><th scope="Category Name"><span class="badge bg-yellow"><span id="wKgsclass"></span></span></th><td data-title="IsTransport" ></td></tr>');
                        GetTotalCal1();
                    }
                    else {
                    }
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function GetInTrasistMilkDeatails() {
            var table = document.getElementById("tbl_Intrasist_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Get_InTrasistMilk_Deatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    var l = 0;
                    var COLOR = ["", "#f3f5f7"];
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_Intrasist_list").rows.length;
                        $('#tbl_Intrasist_list').append('<tr style="background-color:' + COLOR[l] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vendorname + '</th><th scope="Category Name">' + msg[i].vehciecleno + '</th><th scope="Category Name">' + msg[i].milktype + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs">' + msg[i].QtyKgs + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].QtyLtr + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].dcdate + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].dcdtime + '</span></span></th><td data-title="IsTransport" ></td></tr>');
                        j++;
                        l = l + 1;
                        if (l == 2) {
                            l = 0;
                        }
                    }
                    var tot = "Total";
                    $('#tbl_Intrasist_list').append('<tr><td data-title="categorysno"></td><th scope="Category Name"></th><th scope="Category Name"></th><th scope="Category Name">' + tot + '</th><th scope="Category Name" ><span class="badge bg-yellow"><span id="ltrclass"></span></span></th><th scope="Category Name"><span class="badge bg-yellow"><span id="Kgsclass"></span></span></th><td data-title="IsTransport" ></td></tr>');
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

        function GetlabDeatails() {
            var table = document.getElementById("tbl_sampleinward_taken");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Get_labdcMilk_Deatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    var l = 0;
                    var COLOR = ["", "#f3f5f7"];
                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_Intrasist_list").rows.length;
                        $('#tbl_sampleinward_taken').append('<tr style="background-color:' + COLOR[l] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vehciecleno + '</th><th scope="Category Name">' + msg[i].milktype + '</th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].dcdate + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].dcdtime + '</span></span></th><th scope="Category Name"></th><td data-title="IsTransport" ></td></tr>');
                        j++;
                        l = l + 1;
                        if (l == 2) {
                            l = 0;
                        }
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

       // <span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].weight + '</span></span>

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
        function GetTotalCal1() {
            var totalltr = 0;
            $('.wclsqtyltr').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalltr += parseFloat(qtyclass);
                }

            });
            var totalkgs = 0;
            $('.wclsqtykgs').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalkgs += parseFloat(qtyclass);
                }

            });
            document.getElementById('wKgsclass').innerHTML = parseFloat(totalltr).toFixed(2);
            document.getElementById('wltrclass').innerHTML = parseFloat(totalkgs).toFixed(2);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Milk Details <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <%--   <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Charts</a></li>
            <li class="active">ChartJS</li>--%>
        </ol>
    </section>
    <!-- Main content -->
    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk InTrasist Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height: 300px; overflow-y: scroll;">
                            <table class="table" id="tbl_Intrasist_list">
                                <tr style="background-color:#5d6677; color: whitesmoke;">
                                    <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Name
                                    </th>
                                    <th>
                                        Vehicle No
                                    </th>
                                    <th>
                                        Milk Type
                                    </th>
                                    <th>
                                        Qty(kg)
                                    </th>
                                    <th>
                                        Qty(Ltr)
                                    </th>
                                    <th>
                                        Date
                                    </th>
                                    <th>
                                        Time
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Sample Not Taken Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <!-- /.box-header -->
                        <div class="box-body no-padding" style="height: 300px; overflow-y: scroll;">
                            <table class="table" id="tbl_sampleinward_taken">
                                <tr style="background-color:#5d6677; color: whitesmoke;">
                                  <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Vehicle No
                                    </th>
                                    <th>
                                        dcno
                                    </th>
                                     <th>
                                        Date
                                    </th>
                                    <th>
                                        Time
                                    </th>
                                    <%--<th>
                                        weight
                                    </th>--%>
                                </tr>
                            </table>
                        </div>
                        <!-- /.box-body -->
                        <!-- /.box -->
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <!-- LINE CHART -->
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Weighing InComplete Details
                        </h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height: 300px; overflow-y: scroll;">
                            <table class="table" id="tbl_weighMilk_list">
                                <tr style="background-color:#5d6677; color: whitesmoke;">
                                  <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Vehicle No
                                    </th>
                                    <th>
                                        Milk Type
                                    </th>
                                    <th>
                                        Qty(kg)
                                    </th>
                                    <th>
                                        Qty(Ltr)
                                    </th>
                                     <th>
                                        Date
                                    </th>
                                    <th>
                                        Time
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="box box-success">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Unloading InComplete Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height: 300px; overflow-y: scroll;">
                            <table class="table" id="tbl_unload_list">
                                <tr style="background-color:#5d6677; color: whitesmoke;">
                                  <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Vehicle No
                                    </th>
                                    <th>
                                        DC No
                                    </th>
                                    <th>
                                         FAT
                                    </th>
                                    <th>
                                        SNF
                                    </th>
                                     <th>
                                        Date
                                    </th>
                                    <th>
                                        Time
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
