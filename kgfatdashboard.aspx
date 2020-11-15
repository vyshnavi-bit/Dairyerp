<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="kgfatdashboard.aspx.cs" Inherits="kgfatdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            GetfatsnfDeatails();
            GetbuffatsnfDeatails();
            GetcowfatsnfDeatails();
            GetsilofatsnfDeatails();
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

        function GetfatsnfDeatails() {
            var table = document.getElementById("tbl_totalfatsnf_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Get_fat_snf_Deatails' };
            var s = function (msg) {
                if (msg) {
                    if (msg) {
                        var j = 1;
                        for (var i = 0; i < msg.length; i++) {
                            var tablerowcnt = document.getElementById("tbl_totalfatsnf_list").rows.length;
                            $('#tbl_totalfatsnf_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name"></th><th scope="Category Name"><span class="badge bg-yellow">' + msg[i].kgfat + '</span></th><th scope="Category Name"><span class="badge bg-yellow">' + msg[i].kgfat + '</span></th><td data-title="IsTransport" ></td></tr>');
                            j++;
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

        function GetcowfatsnfDeatails() {
            var table = document.getElementById("tbl_cowfatsnf_details");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Get_fat_snf_Deatails' };
            var s = function (msg) {
                if (msg) {
                    if (msg) {
                        var j = 1;
                        for (var i = 0; i < msg.length; i++) {
                            var tablerowcnt = document.getElementById("tbl_cowfatsnf_details").rows.length;
                            $('#tbl_cowfatsnf_details').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name"></th><th scope="Category Name"><span class="badge bg-yellow">' + msg[i].kgcowfat + '</span></th><th scope="Category Name"><span class="badge bg-yellow">' + msg[i].kgcowsnf + '</span></th><td data-title="IsTransport" ></td></tr>');
                            j++;
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

        function GetbuffatsnfDeatails() {
            var table = document.getElementById("tbl_buffalofatsnf_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Get_fat_snf_Deatails' };
            var s = function (msg) {
                if (msg) {
                    if (msg) {
                        var j = 1;
                        for (var i = 0; i < msg.length; i++) {
                            var tablerowcnt = document.getElementById("tbl_buffalofatsnf_list").rows.length;
                            $('#tbl_buffalofatsnf_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name"></th><th scope="Category Name"><span class="badge bg-yellow">' + msg[i].kgbuffat + '</span></th><th scope="Category Name"><span class="badge bg-yellow">' + msg[i].kgbufsnf + '</span></th><td data-title="IsTransport" ></td></tr>');
                            j++;
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

        function GetsilofatsnfDeatails() {
            var table = document.getElementById("tbl_silosnffat_list");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            var data = { 'op': 'Get_silo_wise_fat_snf_Deatails' };
            var s = function (msg) {
                if (msg) {
                    if (msg) {
                        var j = 1;
                        for (var i = 0; i < msg.length; i++) {
                            var tablerowcnt = document.getElementById("tbl_silosnffat_list").rows.length;
                            $('#tbl_silosnffat_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name"><span class="badge bg-yellow">' + msg[i].SiloName + '</span></th><th scope="Category Name"><span class="badge bg-yellow">' + msg[i].ltrsnf + '</span></th><th scope="Category Name"><span class="badge bg-yellow">' + msg[i].ltrfat + '</span></th><td data-title="IsTransport" ></td></tr>');
                            j++;
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
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            FAT AND SNF Details <small>Preview</small>
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
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>TOTAL Fat AND Snf Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height: 100px; overflow-y: scroll;">
                            <table class="table" id="tbl_totalfatsnf_list">
                                <tr>
                                    <th style="width: 10px">
                                        #
                                    </th>
                                     <th>
                                       
                                    </th>
                                    <th>
                                        totalfat
                                    </th>
                                    <th>
                                        totalsnf
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Total Cow fat and snf Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <!-- /.box-header -->
                        <div class="box-body no-padding" style="height: 100px; overflow-y: scroll;">
                            <table class="table" id="tbl_cowfatsnf_details">
                                <tr>
                                 <th style="width: 10px">
                                        #
                                    </th>
                                     <th>
                                       
                                    </th>
                                    <th>
                                        fat
                                    </th>
                                    <th>
                                        snf
                                    </th>
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
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Total Buffalo fat and snf Details
                        </h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height: 100px; overflow-y: scroll;">
                            <table class="table" id="tbl_buffalofatsnf_list">
                                <tr>
                                  <th style="width: 10px">
                                        #
                                    </th>
                                     <th>
                                       
                                    </th>
                                    <th>
                                        fat
                                    </th>
                                    <th>
                                        snf
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="box box-success">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Silo Wise FAT AND SNF Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" style="height: 300px; overflow-y: scroll;">
                            <table class="table" id="tbl_silosnffat_list">
                                <tr>
                                  <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        Silo Name
                                    </th>
                                    <th>
                                        LTR FAT
                                    </th>
                                    <th>
                                       LTR SNF
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
