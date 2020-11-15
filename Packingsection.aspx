<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Packingsection.aspx.cs" Inherits="Packingsection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(function () {
        $('#showlogs').css('display', 'block');
        $('#div_getpackingdata').css('display', 'block');
        $('#div_pckdetails').css('display', 'none');
        $('#divFillScreen').hide();
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
        $('#txt_datetime').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        get_Batchs();
        get_pakingentry_details();
        $('#btn_addpak').click(function () {
            $('#showlogs').css('display', 'none');
            $('#div_getpackingdata').css('display', 'none');
            $('#div_pckdetails').css('display', 'block');
            $('#divFillScreen').hide();
        });
        $('#btn_close').click(function () {
            $('#showlogs').css('display', 'block');
            $('#div_getpackingdata').css('display', 'block');
            $('#div_pckdetails').css('display', 'none');
            $('#divFillScreen').hide();
        });
        $('#btn_peclose').click(function () {
            $('#diveditdata').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_getpackingdata').css('display', 'block');
            $('#div_pckdetails').css('display', 'none');
            $('#divFillScreen').hide();
        });
    });
    function addpackingsection() {
        get_Batchs();
        $('#showlogs').css('display', 'none');
        $('#div_getpackingdata').css('display', 'none');
        $('#div_pckdetails').css('display', 'block');
        $('#divFillScreen').hide();
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
        $('#txt_datetime').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
    }
    $(function () {
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
        $('#txt_getdatadate').val(yyyy + '-' + mm + '-' + dd);
        get_pakingentry_details();
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
        var data = document.getElementById('slct_Batch');
        var length = data.options.length;
        document.getElementById('slct_Batch').options.length = null;
        var opt = document.createElement('option');
        opt.innerHTML = "Select Batch";
        opt.value = "Select Batch";
        opt.setAttribute("selected", "selected");
        opt.setAttribute("disabled", "disabled");
        opt.setAttribute("class", "dispalynone");
        data.appendChild(opt);
        for (var i = 0; i < msg.length; i++) {
            if (msg[i].departmentid == "5") {
                if (msg[i].batchtype != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].batchtype;
                    option.value = msg[i].batchid;
                    data.appendChild(option);
                }
            }
        }
    }
    function fillproductdetails() {
        var batch = document.getElementById('slct_Batch').value;
        if (batch == "" || batch == "Select Batch") {
            alert("Please select batch Type");
            return false;
        }
        var data = { 'op': 'get_Batch_Product_details', 'batch': batch };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    $("#divFillScreen").show();
                    var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                    results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Quantity</th><th scope="col" style="font-weight: 700;">Received Film</th><th scope="col" style="font-weight: 700;">Consumption Film</th><th scope="col" style="font-weight: 700;">Return Film</th><th scope="col" style="font-weight: 700;">Wastage Film</th><th scope="col" style="font-weight: 700;">Cutting Film</th></tr></thead></tbody>';
                    for (var i = 0; i < msg.length; i++) {
                        results += '<tr>';
                        results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                        results += '<th><span id="txt_SiloName" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].productname + '</span></th>';
                        results += '<td><input id="txt_qtykgs" class="form-control" value="" type="number" name="vendorcode"placeholder="Enter qty ltrs"></td>';
                        results += '<td><input id="txt_recivedfilm" class="form-control" value="" type="number" name="vendorcode"placeholder="Enter recived film"></td>';
                        results += '<td><input id="txt_Consumption" class="form-control" value="" type="number" name="vendorcode"placeholder="Enter Consumption film"></td>';
                        results += '<td><input id="txt_Return" class="form-control" value="" type="number" name="vendorcode"placeholder="Enter Return film"></td>';
                        results += '<td><input id="txt_wastage" class="form-control" value="" type="number" name="vendorcode"placeholder="Enter wastage film"></td>';
                        results += '<td><input id="txt_cutting" class="form-control" value="" type="number" name="vendorcode"placeholder="Enter cutting film"></td>';
                        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="number" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                    }
                    results += '</table></div>';
                    $("#divFillScreen").html(results);
                }
                else {
                    $("#divFillScreen").hide();
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function save_packing_section_click() {
        var batch = document.getElementById('slct_Batch').value;
        var sno = document.getElementById('lbl_sno').innerHTML;
        var btnvalue = document.getElementById('save_batchdetails').innerHTML;
        var date = document.getElementById('txt_datetime').value;
        if (batch == "" || batch == "Select Batch") {
            alert("Please select batch Type");
            return false;
        }
        var rows = $("#table_shift_wise_details tr:gt(0)");
        var pkg_batch_wise_details = new Array();
        $(rows).each(function (i, obj) {
            if ($(this).find('#txt_qtykgs').val() != "") {
                pkg_batch_wise_details.push({ productid: $(this).find('#hdn_productid').val(), qty_kgs: $(this).find('#txt_qtykgs').val(), recivedfilm: $(this).find('#txt_recivedfilm').val(), Consumptionfilm: $(this).find('#txt_Consumption').val(), Returnfilm: $(this).find('#txt_Return').val(), wastagefilm: $(this).find('#txt_wastage').val(), cuttingfilm: $(this).find('#txt_cutting').val() });
            }
        });
        if (pkg_batch_wise_details.length == 0) {
            alert("Please enter new quantity");
            return false;
        }
        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var data = { 'op': 'save_packing_section_click', 'batch': batch, 'sno': sno, 'btnvalue': btnvalue, 'date': date, 'pkg_batch_wise_details': pkg_batch_wise_details };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        clearvalue();
                        get_pakingentry_details();
                        $('#diveditdata').css('display', 'none');
                        $('#showlogs').css('display', 'block');
                        $('#div_getpackingdata').css('display', 'block');
                        $('#div_pckdetails').css('display', 'none');
                        $('#divFillScreen').hide();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
    }
    function Clearvalues() {
        document.getElementById('slct_Batch').selectedIndex = 0;
        document.getElementById('slct_product').selectedIndex = 0;
        document.getElementById('ddlsource').selectedIndex = 0;
        document.getElementById('txt_qtyltrs').value = "";
        document.getElementById('txt_fat').value = "";
        document.getElementById('txt_snf').value = "";
        document.getElementById('txt_clr').value = "";
        document.getElementById('txt_receivedfilm').value = "";
        document.getElementById('txt_consumption').value = "";
        document.getElementById('txt_returnfilm').value = "";
        document.getElementById('txt_cuttingfilm').value = "";
        document.getElementById('txt_Wastage').value = "";
    }
    function LRChange(qtyid) {
        if (qtyid.value == "") {
        }
        else {
            ////////For SNF ////////////////
            clr = parseFloat(qtyid.value).toFixed(3);
            var fat = 0;
            fat = document.getElementById('txt_fat').value;
            fat = parseFloat(fat).toFixed(3);
            var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
            document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);
        }
    }
    function ConsumptionfilmChange(qtyid) {
        if (qtyid.value == "") {
        }
        else {
            var receivedfilm = document.getElementById('txt_receivedfilm').value;
            var consumption = document.getElementById('txt_consumption').value;


            if (receivedfilm == "") {
                alert("Please enter Received Film");
                return false;
            }
            if (consumption == "") {
                alert("Please enter consumption Film");
                return false;
            }
            var returnfilm = qtyid.value;
            var sum = parseFloat(returnfilm) + parseFloat(consumption);
            var Return = 0;
            Return = receivedfilm - sum;
            Return = parseFloat(Return).toFixed(1);
            document.getElementById('txt_Wastage').value = Return;
        }
    }
    function get_pakingentry_details() {
        var getdatadate = document.getElementById('txt_getdatadate').value;
        var data = { 'op': 'get_pakingentry_details', 'getdatadate': getdatadate };
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
        var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">Quantity(ltrs)</th><th scope="col" style="font-weight: bold;">Received Film</th><th scope="col" style="font-weight: bold;">Cutting Film</th><th scope="col" style="font-weight: bold;">Wastage Film</th><th scope="col" style="font-weight: bold;">Return Film</th><th scope="col" style="font-weight: bold;">Consumption Film</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color:' + COLOR[l] + '">';
            //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
            results += '<td data-title="Capacity" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
            results += '<td  class="2" style="text-align:center;">' + msg[i].qtyltrs + '</td>';
            results += '<td  class="3" style="text-align:center;">' + msg[i].receivedfilm + '</td>';
            results += '<td  class="4" style="text-align:center;">' + msg[i].cuttingfilm + '</td>';
            results += '<td  class="5" style="text-align:center;">' + msg[i].wastagefilm + '</td>';
            results += '<td  class="6" style="text-align:center;">' + msg[i].returnfilm + '</td>';
            results += '<td  class="7" style="text-align:center;">' + msg[i].consumptionfilm + '</td>';
            results += '<td style="display:none" class="8">' + msg[i].fat + '</td>';
            results += '<td style="display:none" class="9">' + msg[i].snf + '</td>';
            results += '<td style="display:none" class="10">' + msg[i].clr + '</td>';
            results += '<td style="display:none" class="11">' + msg[i].productid + '</td>';
            results += '<td style="display:none" class="12">' + msg[i].batchid + '</td>';
            results += '<td style="display:none" class="20">' + msg[i].datetime + '</td>';
            results += '<td  class="13" style="display:none">' + msg[i].date + '</td>';
            results += '<td style="display:none" class="14">' + msg[i].sno + '</td>';
            results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
            l = l + 1;
            if (l == 4) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_getpackingdata").html(results);
    }
    function getme(thisid) {
        var productname = $(thisid).parent().parent().children('.1').html();
        var qtyltrs = $(thisid).parent().parent().children('.2').html();
        var receivedfilm = $(thisid).parent().parent().children('.3').html();
        var cuttingfilm = $(thisid).parent().parent().children('.4').html();
        var wastagefilm = $(thisid).parent().parent().children('.5').html();
        var returnfilm = $(thisid).parent().parent().children('.6').html();
        var consumptionfilm = $(thisid).parent().parent().children('.7').html();
        var fat = $(thisid).parent().parent().children('.8').html();
        var snf = $(thisid).parent().parent().children('.9').html();
        var clr = $(thisid).parent().parent().children('.10').html();
        var productid = $(thisid).parent().parent().children('.11').html();
        var batchid = $(thisid).parent().parent().children('.12').html();
        var date = $(thisid).parent().parent().children('.13').html();
        var sno = $(thisid).parent().parent().children('.14').html();
        var datetime = $(thisid).parent().parent().children('.20').html();
        $("#divFillScreen").show();
        var i = 1;
        var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Quantity</th><th scope="col" style="font-weight: 700;">Received Film</th><th scope="col" style="font-weight: 700;">Consumption Film</th><th scope="col" style="font-weight: 700;">Return Film</th><th scope="col" style="font-weight: 700;">Wastage Film</th><th scope="col" style="font-weight: 700;">Cutting Film</th></tr></thead></tbody>';
        results += '<tr>';
        results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + i + '</span></th>';
        results += '<th><span id="txt_SiloName" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + productname + '</span></th>';
        results += '<td><input id="txt_qtykgs" class="form-control" value="' + qtyltrs + '" type="number" name="vendorcode"placeholder="Enter qty ltrs"></td>';
        results += '<td><input id="txt_recivedfilm" class="form-control" value="' + receivedfilm + '" type="number" name="vendorcode"placeholder="Enter recived film"></td>';
        results += '<td><input id="txt_Consumption" class="form-control" value="' + consumptionfilm + '" type="number" name="vendorcode"placeholder="Enter Consumption film"></td>';
        results += '<td><input id="txt_Return" class="form-control" value="' + returnfilm + '" type="number" name="vendorcode"placeholder="Enter Return film"></td>';
        results += '<td><input id="txt_wastage" class="form-control" value="' + wastagefilm + '" type="number" name="vendorcode"placeholder="Enter wastage film"></td>';
        results += '<td><input id="txt_cutting" class="form-control" value="' + cuttingfilm + '" type="number" name="vendorcode"placeholder="Enter cutting film"></td>';
        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="number" name="vendorcode" value="' + productid + '"></td></tr>';
        results += '</table></div>';
        $("#divFillScreen").html(results);
        document.getElementById('save_batchdetails').innerHTML = "Modify";
        document.getElementById('slct_Batch').value = batchid;
        $('#showlogs').css('display', 'none');
        $('#div_getpackingdata').css('display', 'none');
        $('#div_pckdetails').css('display', 'block');
        document.getElementById('lbl_sno').innerHTML = sno;
        document.getElementById('txt_datetime').value = datetime;
    }
    function clearvalue() {
        document.getElementById('lbl_sno').innerHTML = "";
        document.getElementById('txt_datetime').value = "";
        document.getElementById('slct_Batch').selectedIndex = 0;
        document.getElementById('save_batchdetails').innerHTML = "Save";
        $('#showlogs').css('display', 'block');
        $('#div_getpackingdata').css('display', 'block');
        $('#div_pckdetails').css('display', 'none');
        $('#divFillScreen').hide();
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
           Packing Material<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Packing</a></li>
            <li><a href="#"> Packing Material</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
        <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Packing Material
                </h3>
            </div>
            <div>
            <div class="box-body">
                <div id="showlogs">
                    <%--<input id="btn_addpak" type="button" name="submit" value='Add Details'
                        class="btn btn-success" />--%>
                        <table>
                    <tr>
                    <td>
                          </td>
                        <td>
                            <label>
                                Date </label>
                        </td>
                        <td style="width: 5px;">
                          </td>
                        <td>
                            <input id="txt_getdatadate" class="form-control" type="date" name="vendorcode" />
                        </td>
                         <td style="width: 5px;">
                          </td>
                        <td>
                        <%--<input id="txt_getgenerate" type="button" name="submit" value='Generate'
                        class="btn btn-primary" onclick="get_pakingentry_details();"/>--%>
                        <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="get_pakingentry_details()"></span> <span onclick="get_pakingentry_details()">Generate</span>
                          </div>
                          </div>
                        </td>
                        <%--<td style="width: 400px;">
                          </td>
                        <td>
                         <input id="btn_addpak" type="button" name="submit" value='Add Details'
                        class="btn btn-success" />
                        </td>--%>
                            <td style="padding-left: 80%;">
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addpackingsection()"></span> <span onclick="addpackingsection()">Add Details</span>
                          </div>
                          </div>
                            </td>
                    </tr>
                </table>
                </div>
                <div id="div_getpackingdata">
                </div>
                <div style="display:none;" id="div_pckdetails">
             <div style="padding-left: 410PX;" >
                <table>
                    <tr>
                        <td>
                            <label>
                                Date <span style="color: red;">*</span></label>
                            <input id="txt_datetime" class="form-control" type="datetime-local"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Batch Type<span style="color: red;">*</span></label>
                            <select id="slct_Batch" class="form-control" onchange="fillproductdetails();">
                                <option selected disabled value="Select SILO No">Select Batch Type</option>
                            </select>
                            <label id="lbl_silo" class="errormessage">
                                * Please Batch Type</label>
                        </td>
                    </tr>
                    <tr hidden>
                        <label id="lbl_sno">
                        </label>
                    </tr>
                </table>
            </div>
            <div class="box-body">
                <div id="divFillScreen">
                </div>
            </div>
           <%-- <div style="padding-left: 410PX;">
                <input id='save_batchdetails' type="button" class="btn btn-success" name="submit"
                    value='Save' onclick="save_packing_section_click()" />
                     <input id='btn_close' type="button" class="btn btn-danger" name="submit"
                    value='Close' onclick="clearvalue();" />
            </div>--%>
             <div  style="padding-left: 38%;padding-top: 0%;">
                <table>
                <tr>
                <td>
                    <div class="input-group">
                        <div class="input-group-addon">
                        <span class="glyphicon glyphicon-ok" id="save_batchdetails1" onclick="save_packing_section_click()"></span><span id="save_batchdetails" onclick="save_packing_section_click()">Save</span>
                    </div>
                    </div>
                    </td>
                    <td style="width:10px;"></td>
                    <td>
                        <div class="input-group">
                        <div class="input-group-close">
                        <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="clearvalue()"></span><span id='btn_close' onclick="clearvalue()">Close</span>
                    </div>
                    </div>
                    </td>
                    </tr>
                    </table>
                </div>
            </div>
                </div>
            </div>
            <div   class="box-body">
            <div id="diveditdata" style="display:none;">
                </div>
            </div>
            </div>
    </section>
</asp:Content>