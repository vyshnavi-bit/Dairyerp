<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="silowiseediting.aspx.cs" Inherits="silowiseediting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            var today = new Date();
            var dd = today.getDate();
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
            $('#txt_sdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#txt_chdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
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

        function siloclosingdetails() {
            get_Batchs();
            var date = document.getElementById('txt_sdate').value;
            var data = { 'op': 'get_siloclosing_details', 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div    style="overflow:auto;"><table id="table_sales_wise_details" class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Silo Name</th><th scope="col" style="font-weight: bold;">Batch Name</th><th scope="col" style="font-weight: bold;">Quantity</th><th scope="col" style="font-weight: bold;">fat</th><th scope="col" style="font-weight: bold;">snf</th><th scope="col" style="font-weight: bold;">clr</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
                        var k = 1;
                        var l = 0;
                        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr style="background-color:' + COLOR[l] + '">';
                            // results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                            results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].SiloName + '</td>';
                            results += '<td data-title="Code" class="8">' + msg[i].batch + '</td>';
                            results += '<td data-title="Code" style="display:none;" class="9">' + msg[i].batchid + '</td>';
                            results += '<td data-title="Code" class="2">' + msg[i].Quantity + '</td>';
                            results += '<td data-title="Code" class="3">' + msg[i].fat + '</td>';
                            results += '<td data-title="Code" class="4">' + msg[i].snf + '</td>';
                            results += '<td data-title="Code" class="5">' + msg[i].clr + '</td>';
                            results += '<td data-title="Code" style="display:none;" class="6">' + msg[i].sno + '</td>';
                            results += '<td data-title="Code" style="display:none;" class="7">' + msg[i].SiloId + '</td>';
                            results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                            l = l + 1;
                            if (l == 4) {
                                l = 0;
                            }
                        }
                        results += '</table></div>';
                        $("#divsales").html(results);
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
        function get_Batchs() {
            var data = { 'op': 'get_batch_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillBatchs(msg);
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
        function fillBatchs(msg) {
            var data = document.getElementById('slct_batchname');
            var length = data.options.length;
            document.getElementById('slct_batchname').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Batch";
            opt.value = "Select Batch";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].batchtype != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].batchtype;
                    option.value = msg[i].batchid;
                    data.appendChild(option);
                }
            }
        }
        function getme(thisid) {
            var siloname = $(thisid).parent().parent().children('.1').html();
            var qtykgs = $(thisid).parent().parent().children('.2').html();
            var fat = $(thisid).parent().parent().children('.3').html();
            var snf = $(thisid).parent().parent().children('.4').html();
            var clr = $(thisid).parent().parent().children('.5').html();
            var sno = $(thisid).parent().parent().children('.6').html();
            var siloid = $(thisid).parent().parent().children('.7').html();
            var batch = $(thisid).parent().parent().children('.8').html();
            var batchid = $(thisid).parent().parent().children('.9').html();

            document.getElementById('txtsiloname').value = siloname;
            document.getElementById('txtqty').value = qtykgs;
            document.getElementById('txtfat').value = fat;
            document.getElementById('txtsnf').value = snf;
            document.getElementById('txtclr').value = clr;
            document.getElementById('lbl_sno').value = sno;
            document.getElementById('lbl_siloid').value = siloid;
            document.getElementById('slct_batchname').value = batchid;
            var datee = document.getElementById('txt_sdate').value;
            document.getElementById('txt_chdate').value = datee;
            $("#divsales").hide();
            $("#fillform").show();
            if (batchid == "12" || batchid == "31") {
                $("#tr_costperltr").show();
            }
        }
        function btnclear() {
            document.getElementById('txtsiloname').value = "";
            document.getElementById('txtqty').value = "";
            document.getElementById('txtfat').value = "";
            document.getElementById('txtsnf').value = "";
            document.getElementById('txtclr').value = "";
            $("#divsales").show();
            $("#fillform").hide();
            $("#tr_costperltr").hide();
        }
        function for_save_edit_siloclosing() {
            var date = document.getElementById('txt_sdate').value;
            var qty = document.getElementById('txtqty').value;
            var fat = document.getElementById('txtfat').value;
            var snf = document.getElementById('txtsnf').value;
            var clr = document.getElementById('txtclr').value;
            var sno = document.getElementById('lbl_sno').value;
            var siloid = document.getElementById('lbl_siloid').value;
            var cdate = document.getElementById('txt_chdate').value;
            var costperltr = document.getElementById('hdn_costperltr').value;
            var batchid = document.getElementById('slct_batchname').value;
            var btnvalue = document.getElementById('btn_save').innerHTML;
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'modify_silo_closing_details', 'cdate': cdate, 'date': date, 'qty': qty, 'fat': fat, 'snf': snf, 'clr': clr, 'sno': sno,
                    'siloid': siloid, 'btnvalue': btnvalue, 'batchid': batchid, 'costperltr': costperltr
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            siloclosingdetails();
                            $("#divsales").show();
                            $("#fillform").hide();
                            //pclearvalues();
                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Edit Silo Closing Details<small></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#">Edit Silo Closing Details</a></li>
        </ol>
    </section>
    <section class="content">
         <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Edit Silo Closing Details</h3>
                        </div>
                    
            <div class="box-body">
                <div id="divproductionsales">
                    <div style="padding-left: 40%;">
                        <table>
                            <tr>
                            <td>
                                <label>
                                    Date<span style="color: red;">*</span></label>
                                <input id="txt_sdate" class="form-control" type="datetime-local" name="vendorcode"
                                    style="width: 200px;" placeholder="Enter Date">
                                    </td>
                                    <td style="width:2%;"></td>
                                    <td>
                                     <label>
                                    &nbsp<span style="color: red;"></span></label>
                                    <table>
                                    <tr>
                                    <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                        <span  class="glyphicon glyphicon-refresh" onclick="siloclosingdetails()"></span> <span onclick="siloclosingdetails()">Generate</span>
                                    </div>
                                    </div>
                            </td>
                     </tr>
                     </table>
                     </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div id="divsales">
                    </div>
                    <div id='fillform' style="display: none; padding-left:20%;">
                        <table align="center" style="width: 70%;">
                            <tr>
                                <th>
                                </th>
                            </tr>
                            <tr>
                                <td style="height: 40px; PADDING-LEFT: 2%;">
                                    <label>
                                        Silo Name<span style="color: red;">*</span></label>
                                    <input type="text" maxlength="45" id="txtsiloname" class="form-control" name="vendorcode"  readonly/>
                                </td>
                                <td style="height: 40px; PADDING-LEFT: 2%;">
                                    <label>
                                        Quantity
                                    </label>
                                    <span style="color: red;">*</span>
                                    <input type="text" maxlength="45" id="txtqty" class="form-control" name="vendorcode" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px; PADDING-LEFT: 2%;">
                                    <label>
                                        fat
                                    </label>
                                    <span style="color: red;">*</span>
                                    <input type="text" maxlength="45" id="txtfat" class="form-control" name="vendorcode" />
                                </td>
                                <td style="height: 40px; PADDING-LEFT: 2%;">
                                    <label>
                                        snf
                                    </label>
                                    <span style="color: red;">*</span>
                                    <input type="text" maxlength="45" id="txtsnf" class="form-control" name="vendorcode" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px; PADDING-LEFT: 2%;">
                                    <label>
                                        clr
                                    </label>
                                    <span style="color: red;">*</span>
                                    <input type="text" maxlength="45" id="txtclr" class="form-control" name="vendorcode" />
                                </td>
                                <td style="height: 40px; PADDING-LEFT: 2%;">
                                    <label>
                                        Date
                                    </label>
                                    <span style="color: red;">*</span>
                                    <input type="datetime-local" id="txt_chdate" class="form-control" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px; PADDING-LEFT: 2%;">
                                    <label>
                                        Batch Name
                                    </label>
                                    <span style="color: red;">*</span>
                                    <select   id="slct_batchname" class="form-control"   />
                                </td>
                                <td id="tr_costperltr" style="display:none;height: 40px; PADDING-LEFT: 2%;">
                                   <label>
                                        Cost Per Liter
                                    </label>
                                    <span style="color: red;">*</span>
                                    <input   id="hdn_costperltr" class="form-control" />
                                </td>
                            </tr>
                            <tr hidden>
                                <td>
                                    <label id="lbl_sno">
                                    </label>
                                </td>
                            </tr>
                             <tr hidden>
                                <td>
                                    <label id="lbl_siloid">
                                    </label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2" align="center" style="height: 40px;">
                                    <input type="button" name="submit" class="btn btn-success" id="btn_save" value='Modify'
                                        onclick="for_save_edit_siloclosing()" />
                                    <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close' onclick="btnclear()" />
                                </td>
                            </tr>--%>
                        </table>
                        <div  style="padding-left: 26%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="for_save_edit_siloclosing()"></span><span id="btn_save" onclick="for_save_edit_siloclosing()">Modify</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="btnclear()"></span><span id='btn_close' onclick="btnclear()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
