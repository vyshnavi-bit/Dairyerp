<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);
        });
    </script>
    <script type="text/javascript">
        $(function () {
            GetInTrasistMilkDeatails();
            Get_Buffalo_Deatails();
            Get_Cow_Deatails();
            //            Get_fat_snf_Deatails();
            var Branch = '<%=Session["Branch_ID"] %>';
            if (Branch == "1") {
                $('#div_milkdetcow').css('display', 'block');
                $('#div_milkdetbuff').css('display', 'block');
            }
            else {
                $('#div_milkdetcow').css('display', 'none');
                $('#div_milkdetbuff').css('display', 'none');
            }
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
//        function Get_fat_snf_Deatails() {
//            var table = document.getElementById("tbl_fat_snf_list");
//            for (var i = table.rows.length - 1; i > 0; i--) {
//                table.deleteRow(i);
//            }
//            var data = { 'op': 'Get_fat_snf_Deatails' };
//            var s = function (msg) {
//                if (msg) {
//                    var j = 1;
//                    for (var i = 0; i < msg.length; i++) {
//                        var tablerowcnt = document.getElementById("tbl_fat_snf_list").rows.length;
//                        $('#tbl_fat_snf_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].kgfat + '</span></th><th scope="Category Name"><span class="badge bg-green">' + msg[i].kgsnf + '</span></th></tr>');
//                        j++;
//                    }
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
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
                        $('#tbl_Intrasist_list').append('<tr style="background-color:' + COLOR[l] + '"><td data-title="categorysno">' + j + '</td><th scope="Category Name">' + msg[i].vendorname + '</th><th scope="Category Name">' + msg[i].dctime + '</th><th scope="Category Name">' + msg[i].distance + '</th><th scope="Category Name">' + msg[i].exptime + '</th><th scope="Category Name">' + msg[i].vehciecleno + '</th><th scope="Category Name">' + msg[i].milktype + '</th><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span class="clsqtykgs">' + msg[i].QtyKgs + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span class="clsqtyltr">' + msg[i].QtyLtr + '</span></span></th><th scope="Category Name">' + msg[i].fat + '</th><th scope="Category Name">' + msg[i].snf + '</th><th scope="Category Name">' + msg[i].kgfat + '</th><th scope="Category Name">' + msg[i].kgsnf + '</th></tr>');
                        j++;
                        l = l + 1;
                        if (l == 2) {
                            l = 0;
                        }
                    }
                    var tot = "Total";
                    $('#tbl_Intrasist_list').append('<tr><td data-title="categorysno"></td><th scope="Category Name"></th><th scope="Category Name"></th><th scope="Category Name"></th><th scope="Category Name"></th><th scope="Category Name"></th><th scope="Category Name">' + tot + '</th><th scope="Category Name" ><span class="badge bg-yellow"><span id="ltrclass"></span></span></th><th scope="Category Name"><span class="badge bg-yellow"><span id="Kgsclass"></span></span></th><td data-title="IsTransport" ></td></tr>');
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
        function Get_Buffalo_Deatails() {
            var table1 = document.getElementById("tbl_Buffalo_list");
            for (var i = table1.rows.length - 1; i > 0; i--) {
                table1.deleteRow(i);
            }
            var table2 = document.getElementById("tbl_Cow_list");
            for (var i = table2.rows.length - 1; i > 0; i--) {
                table2.deleteRow(i);
            }
            var data = { 'op': 'Get_Buffalo_Deatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        var milktype = msg[i].milktype;
                        var tablerowcnt = document.getElementById("tbl_Buffalo_list").rows.length;
                        $('#tbl_Buffalo_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span >' + msg[i].QtyKgs + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span >' + msg[i].QtyLtr + '</span></span></th><td><span  class="badge bg-green">' + msg[i].fat + '</span></td><td><span  class="badge bg-green">' + msg[i].snf + '</span></td><td><span  class="badge bg-green">' + msg[i].kgfat + '</span></td><td><span  class="badge bg-green">' + msg[i].kgsnf + '</span></td></tr>');
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
        function Get_Cow_Deatails() {
            var table1 = document.getElementById("tbl_Buffalo_list");
            for (var i = table1.rows.length - 1; i > 0; i--) {
                table1.deleteRow(i);
            }
            var table2 = document.getElementById("tbl_Cow_list");
            for (var i = table2.rows.length - 1; i > 0; i--) {
                table2.deleteRow(i);
            }
            var data = { 'op': 'Get_Cow_Deatails' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    for (var i = 0; i < msg.length; i++) {
                        var milktype = msg[i].milktype;
                        var tablerowcnt = document.getElementById("tbl_Cow_list").rows.length;
                        $('#tbl_Cow_list').append('<tr><td data-title="categorysno">' + j + '</td><th scope="Category Name" ><span id="spnkgs" class="badge bg-green"><span >' + msg[i].QtyKgs + '</span></span></th><th scope="Category Name"><span id="spbltr"  class="badge bg-green"><span >' + msg[i].QtyLtr + '</span></span></th><td><span  class="badge bg-green">' + msg[i].fat + '</span></td><td><span  class="badge bg-green">' + msg[i].snf + '</span></td><td><span  class="badge bg-green">' + msg[i].kgfat + '</span></td><td><span  class="badge bg-green">' + msg[i].kgsnf + '</span></td></tr>');
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
        function GetTotalCal() {
            document.getElementById('Kgsclass').innerHTML = "";
            document.getElementById('ltrclass').innerHTML = "";
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Main content -->
    <section class="content">
        <div class="row">
            <div class="col-md-6" style="width: 100%;">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i class="fa fa-cog fa-spin fa-1x fa-fw"></i>   Intrasist Milk Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding">
                            <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" id="tbl_Intrasist_list">
                                <tr style="background-color:#5d6677; color: whitesmoke;">
                                    <th style="width: 10px">
                                        #
                                    </th>
                                    <th>
                                        CC Name
                                    </th>
                                    <th>
                                        DC Time
                                    </th>
                                     <th>
                                        Kms
                                    </th>
                                     <th>
                                        Expected Time
                                    </th>
                                    <th>
                                        Vehicle No
                                    </th>
                                    <th>
                                        Milk Type
                                    </th>
                                    <th>
                                        Qty(Kgs)
                                    </th>
                                    <th>
                                        Qty(Ltr)
                                    </th>
                                     <th>
                                       Fat
                                    </th>
                                    <th>
                                       Snf
                                    </th>
                                     <th>
                                       KG Fat
                                    </th>
                                    <th>
                                      KG Snf
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6" style=="display:block;" id="div_milkdetbuff">
                <!-- AREA CHART -->
                <div class="box box-success">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <img src="images/newimg/buffalo.png" alt="" height="50px;"/> Buffalo Milk Details</h3>
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
                                        Qty(Kgs)
                                    </th>
                                    <th>
                                       Qty(Ltr)
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
            <div class="col-md-6" style=="display:block;" id="div_milkdetcow">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                              <img src="images/newimg/cow.png" alt="" height="50px;"/>  Cow Milk Details
                        </h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding" >
                            <table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" id="tbl_Cow_list">
                                <tr>
                                      <th >
                                        #
                                    </th>
                                    <th>
                                        Qty(Kgs)
                                    </th>
                                    <th>
                                       Qty(Ltr)
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
        </div>
    </section>
</asp:Content>
