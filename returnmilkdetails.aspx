<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="returnmilkdetails.aspx.cs" Inherits="returnmilkdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
        $('#txt_rtndate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        $('#div_returnmilk').css('display', 'block');
        $('#div_returnmilkapprove').css('display', 'none');
        get_returnmilk_details();
        get_SiloDepartments();
        get_Silos();
        $('#btn_addDept').click(function () {
            $('#Inwardsilo_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            get_SiloDepartments();
            get_Silos();

        });
        $('#btn_close').click(function () {
            $('#Inwardsilo_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_Deptdata').show();
            Clearvalues();
        });
    });
    function addreturnmilkdetails() {
        $('#Inwardsilo_fillform').css('display', 'block');
        $('#showlogs').css('display', 'none');
        $('#div_Deptdata').hide();
        get_SiloDepartments();
        get_Silos();
    }
    function showreturnmilk() {
        $('#div_returnmilk').css('display', 'block');
        $('#div_returnmilkapprove').css('display', 'none');
        get_returnmilk_details();
    }
    function showreturnmilkapproval() {
        $('#div_returnmilkapprove').css('display', 'block');
        $('#div_returnmilk').css('display', 'none');
        get_returnmilk_details_approve();
    }
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
    function Clearvalues() {
        document.getElementById('slct_Department').selectedIndex = 0;
        document.getElementById('txt_qtykgs').value = "";
        document.getElementById('slct_Source_Name').selectedIndex = 0;
        document.getElementById('txt_qtyltrs').value = "";
        document.getElementById('txt_fat').value = "";
        document.getElementById('txt_snf').value = "";
        document.getElementById('txt_clr').value = "";
        document.getElementById('lbl_sno').value = "";
        document.getElementById('save_departmentqtydetails').value = "Save";
        $("#lbl_department").hide();
        $("#lbl_silo").hide();

    }
    function get_Silos() {

        var data = { 'op': 'get_Silo_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillfSilos(msg);
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
    function fillfSilos(msg) {
        var data = document.getElementById('slct_Source_Name');
        var length = data.options.length;
        document.getElementById('slct_Source_Name').options.length = null;
        var opt = document.createElement('option');
        opt.innerHTML = "Select Silo";
        opt.value = "Select Silo";
        opt.setAttribute("selected", "selected");
        opt.setAttribute("disabled", "disabled");
        opt.setAttribute("class", "dispalynone");
        data.appendChild(opt);
        for (var i = 0; i < msg.length; i++) {
            if (msg[i].SiloName != null) {
                var option = document.createElement('option');
                option.innerHTML = msg[i].SiloName;
                option.value = msg[i].SiloId;
                data.appendChild(option);
            }
        }
    }


    function get_SiloDepartments() {
        var data = { 'op': 'get_SiloDepartments_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    filldepartments(msg);
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
    function filldepartments(msg) {
        var data = document.getElementById('slct_Department');
        var length = data.options.length;
        document.getElementById('slct_Department').options.length = null;
        var opt = document.createElement('option');
        opt.innerHTML = "Select Department";
        opt.value = "Select Department";
        opt.setAttribute("selected", "selected");
        opt.setAttribute("disabled", "disabled");
        opt.setAttribute("class", "dispalynone");
        data.appendChild(opt);
        for (var i = 0; i < msg.length; i++) {
            if (msg[i].DeportmentName != null) {
                var option = document.createElement('option');
                option.innerHTML = msg[i].DeportmentName;
                option.value = msg[i].SiloDeportmentId;
                data.appendChild(option);
            }
        }
    }
    function get_deptqty() {
        var Department = document.getElementById('slct_Department').selectedIndex;
        if (Department == 3) {
            alert("Return not allowed");
            document.getElementById('slct_Department').selectedIndex = 0;
            return;
        }
    }

    function get_returnmilk_details() {
        var data = { 'op': 'get_returnmilk_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillmilkdetails(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function fillmilkdetails(msg) {
        var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;"></th><th scope="col" style="font-weight: bold;">Department Name</th><th scope="col" style="font-weight: bold;">Silo Name</th><th scope="col" style="font-weight: bold;">Quantity(Kgs)</th><th scope="col" style="font-weight: bold;">Quantity(ltrs)</th><th scope="col" style="font-weight: bold;">Return MilkType</th><th scope="col" style="font-weight: bold;">Status</th><th scope="col" style="font-weight: bold;">Date</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color:' + COLOR[l] + '">';
            results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
            results += '<td scope="row" class="1">' + msg[i].departmentname + '</td>';
            results += '<td data-title="Code" class="2">' + msg[i].siloname + '</td>';
            results += '<td data-title="Code" class="3">' + msg[i].qty_kg + '</td>';
            results += '<td data-title="Code" class="4">' + msg[i].qty_ltr + '</td>';
            results += '<td data-title="Code" class="5">' + msg[i].milktype + '</td>';
            if (msg[i].status == "P") {
                var statustype = "Pending";
                results += '<td data-title="Code" class="6">' + statustype + '</td>';
            }
            else {
                var statustype = "Approved";
                results += '<td data-title="Code"  class="7">' + statustype + '</td>';
            }
            results += '<td data-title="Code" class="8">' + msg[i].doe + '</td>';

            results += '<td data-title="Code" style="display:none;" class="10">' + msg[i].departmentid + '</td>';
            results += '<td data-title="Code" style="display:none;" class="11">' + msg[i].siloid + '</td>';

            results += '<td data-title="Code" style="display:none;" class="12">' + msg[i].fat + '</td>';
            results += '<td data-title="Code" style="display:none;" class="13">' + msg[i].snf + '</td>';
            results += '<td data-title="Code" style="display:none;" class="14">' + msg[i].clr + '</td>';

            results += '<td style="display:none;" class="9">' + msg[i].returnmilkid + '</td></tr>';
            l = l + 1;
            if (l == 4) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_Deptdata").html(results);
    }
    function getme(thisid) {
        scrollTo(0, 0);
        var departmentname = $(thisid).parent().parent().children('.1').html();
        var siloname = $(thisid).parent().parent().children('.2').html();
        var qty_kg = $(thisid).parent().parent().children('.3').html();
        var qty_ltr = $(thisid).parent().parent().children('.4').html();
        var milktype = $(thisid).parent().parent().children('.5').html();
        var departmentid = $(thisid).parent().parent().children('.10').html();
        var siloid = $(thisid).parent().parent().children('.11').html();

        var fat = $(thisid).parent().parent().children('.12').html();
        var snf = $(thisid).parent().parent().children('.13').html();
        var clr = $(thisid).parent().parent().children('.14').html();
        var returnmilkid = $(thisid).parent().parent().children('.9').html();
        var doe = $(thisid).parent().parent().children('.8').html();

        document.getElementById('slct_Department').value = departmentid;
        document.getElementById('txt_qtykgs').value = qty_kg;
        document.getElementById('slct_Source_Name').value = siloid;
        document.getElementById('slct_milktype').value = milktype;
        document.getElementById('txt_qtyltrs').value = qty_ltr;
        document.getElementById('txt_fat').value = fat;
        document.getElementById('txt_snf').value = snf;
        document.getElementById('txt_clr').value = clr;
        document.getElementById('lbl_sno').value = returnmilkid;
        document.getElementById('txt_rtndate').value = doe;
        document.getElementById('save_departmentqtydetails').value = "Modify";


        $('#Inwardsilo_fillform').css('display', 'block');
        $('#showlogs').css('display', 'none');
        $('#div_Deptdata').hide();
    }
    function LtrsChange(qtyid) {
        var qtyltr = 0;
        qtyltr = parseFloat(qtyid.value).toFixed(3);
        var clr = document.getElementById('txt_clr').value;
        if (clr == "") {
            clr = 30;
        }
        clr = parseFloat(clr).toFixed(3);
        var qtyltrkgs = 0;
        var modclr = (clr / 1000) + 1;
        modclr = parseFloat(modclr).toFixed(5);
        qtyltrkgs = qtyltr / modclr;
        qtyltrkgs = parseFloat(qtyltrkgs).toFixed(0);
        document.getElementById('txt_qtyltrs').value = qtyltrkgs;
    }
    function LRChange(qtyid) {
        var branchid = '<%=Session["Branch_ID"] %>';
        if (qtyid.value == "") {
            var qty = 0;
            var qtykg = 0;
            qtykg = document.getElementById('txt_qtykgs').value;
            qtykg = parseFloat(qtykg).toFixed(3);
            var qtykgsltr = 0;
            var clr = 0;
            clr = parseFloat(qty).toFixed(3);
            var modclr = (clr / 1000) + 1;
            modclr = parseFloat(modclr).toFixed(3);
            qtykgsltr = qtykg / modclr;
            qtykgsltr = parseFloat(qtykgsltr).toFixed(2);
            if (branchid == 26 || branchid == 115) {

            }
            else {
                document.getElementById('txt_qtyltrs').value = qtykgsltr;
            }
            //  $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);
        }
        else {
            var qtykg = 0;
            qtykg = document.getElementById('txt_qtykgs').value;
            if (qtykg == "") {
                qtykg = parseFloat(qtykg).toFixed(3);
                var qtykgsltr = 0;
                var clr = 0;
                clr = parseFloat(qtyid.value).toFixed(3);
                var modclr = (clr / 1000) + 1;
                modclr = parseFloat(modclr).toFixed(5);
                qtykgsltr = qtykg / modclr;
                qtykgsltr = parseFloat(qtykgsltr).toFixed(0);
                if (branchid == 26 || branchid == 115) {

                }
                else {
                    document.getElementById('txt_qtyltrs').value = qtykgsltr;
                }
            }
            else {
                var qtyltr = document.getElementById('txt_qtyltrs').value;
                qtyltr = parseFloat(qtyltr).toFixed(3);
                var qtyltrkgs = 0;
                var clr = 0;
                clr = parseFloat(qtyid.value).toFixed(3);
                var modclr = (clr / 1000) + 1;
                modclr = parseFloat(modclr).toFixed(5);
                qtyltrkgs = qtyltr * modclr;
                qtyltrkgs = parseFloat(qtyltrkgs).toFixed(0);
                if (branchid == 26 || branchid == 115) {

                }
                else {
                    document.getElementById('txt_qtykgs').value = qtyltrkgs;
                }
            }

            ////////For SNF ////////////////

            var fat = 0;
            fat = document.getElementById('txt_fat').value;
            fat = parseFloat(fat).toFixed(3);
            var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
            document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);
        }
    }
    function save_returnmilkdetails_click() {
        var date = document.getElementById('txt_rtndate').value;
        if (date == "") {
            alert("Please Select Date");
            $('#txt_rtndate').focus();
            return false;
        }
        var Department = document.getElementById('slct_Department').value;
        if (Department == "" || Department == "Select Department") {
            alert("Please enter department name");
            $('#slct_Department').focus();
            return false;
        }
        var silo = document.getElementById('slct_Source_Name').value;
        if (silo == "" ||silo=="Select Silo") {
            alert("Please select silo");
            $('#slct_Source_Name').focus();
            return false;
        }
        var Qtykgs = document.getElementById('txt_qtykgs').value;
        if (Qtykgs == "") {
            alert("Please enter Qty in Kg's");
            $('#txt_qtykgs').focus();
            return false;
        }
        var qtyltrs = document.getElementById('txt_qtyltrs').value;
        var fat = document.getElementById('txt_fat').value;
        if (fat == "") {
            alert("Please enter fat");
            $('#txt_fat').focus();
            return false;
        }
        var snf = document.getElementById('txt_snf').value;
        var clr = document.getElementById('txt_clr').value;
        if (clr == "") {
            alert("Please enter clr");
            $('#txt_clr').focus();
            return false;
        }
        var milktype = document.getElementById('slct_milktype').value;
        if (milktype == "" || milktype == "Select Milk Type") {
            alert("Please enter milk type");
            $('#slct_milktype').focus();
            return false;
        }
        var sno = document.getElementById('lbl_sno').value;
        var btnval = document.getElementById('save_departmentqtydetails').value;
        
        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var data = { 'op': 'save_returnmilkdetails_click', 'date': date, 'btnval': btnval, 'Department': Department, 'sno': sno, 'Qtykgs': Qtykgs,
                'silo': silo, 'qtyltrs': qtyltrs, 'fat': fat, 'snf': snf, 'clr': clr, 'milktype': milktype
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        get_returnmilk_details();
                        Clearvalues();
                        alert(msg);
                        $('#div_Deptdata').show();
                        $('#Inwardsilo_fillform').css('display', 'none');
                        $('#showlogs').css('display', 'block');
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
    function get_returnmilk_details_approve() {
        var data = { 'op': 'get_returnmilk_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillmilkdetailsapprove(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function fillmilkdetailsapprove(msg) {
        var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Department Name</th><th scope="col" style="font-weight: bold;">Silo Name</th><th scope="col" style="font-weight: bold;">Quantity(Kgs)</th><th scope="col" style="font-weight: bold;">Quantity(ltrs)</th><th scope="col" style="font-weight: bold;">Return MilkType</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;">Status</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            if (msg[i].status == "P") {
                var statustype = "Pending";
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1">' + msg[i].departmentname + '</td>';
                results += '<td data-title="Code" class="2">' + msg[i].siloname + '</td>';
                results += '<td data-title="Code" class="3">' + msg[i].qty_kg + '</td>';
                results += '<td data-title="Code" class="4">' + msg[i].qty_ltr + '</td>';
                results += '<td data-title="Code" class="5">' + msg[i].milktype + '</td>';
                results += '<td  class="14">' + msg[i].doe + '</td>';
                results += '<td data-title="Code" class="6">' + statustype + '</td>';
                results += '<td style="display:none;" class="7">' + msg[i].siloid + '</td>';
                results += '<td style="display:none;" class="8">' + msg[i].returnmilkid + '</td>';
                results += '<td style="display:none;" class="10">' + msg[i].departmentid + '</td>';
                results += '<td style="display:none;" class="11">' + msg[i].fat + '</td>';
                results += '<td style="display:none;" class="12">' + msg[i].snf + '</td>';
                results += '<td style="display:none;" class="13">' + msg[i].clr + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Approve!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 apprvcls"  onclick="getmeapprove(this)"><span class="glyphicon glyphicon-thumbs-up" style="top: 0px !important;"></span></button></td></tr>';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getmeapprove(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                //'</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
        }
        results += '</table></div>';
        $("#div_approveget").html(results);
    }
    function getmeapprove(thisid) {
        var qtyltrs = $(thisid).parent().parent().children('.4').html();
        var siloid = $(thisid).parent().parent().children('.7').html();
        var returnmilkid = $(thisid).parent().parent().children('.8').html();

        var departmentname = $(thisid).parent().parent().children('.1').html();
        var siloname = $(thisid).parent().parent().children('.2').html();
        var qty_kg = $(thisid).parent().parent().children('.3').html();
        var milktype = $(thisid).parent().parent().children('.5').html();
        var statustype = $(thisid).parent().parent().children('.6').html();
        var departmentid = $(thisid).parent().parent().children('.10').html();
        var fat = $(thisid).parent().parent().children('.11').html();
        var snf = $(thisid).parent().parent().children('.12').html();
        var clr = $(thisid).parent().parent().children('.13').html();
        var doe = $(thisid).parent().parent().children('.14').html();

        var data = { 'op': 'approve_returnmilk_details', 'returnmilkid': returnmilkid, 'siloid': siloid, 'qtyltrs': qtyltrs, 'departmentname': departmentname, 'siloname': siloname, 'qty_kg': qty_kg, 'milktype': milktype, 'departmentid': departmentid, 'fat': fat, 'snf': snf, 'clr': clr, 'doe': doe };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    get_returnmilk_details_approve(msg);
                    alert(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function isFloat(evt) {
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        else {
            //if dot sign entered more than once then don't allow to enter dot sign again. 46 is the code for dot sign
            var parts = evt.srcElement.value.split('.');
            if (parts.length > 1 && charCode == 46) {
                return false;
            }
            return true;
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            <i class="fa fa-files-o" aria-hidden="true"></i>Return Milk Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Return Milk Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showreturnmilk()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Return Milk Details</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showreturnmilkapproval()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Return Milk Approve Details</a></li>
                </ul>
            </div>
            <div id="div_returnmilk">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Return Milk Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                    <%--<input id="btn_addDept" type="button" name="submit" value='Add Return Milk Details'
                        class="btn btn-success" />--%>
                        <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addreturnmilkdetails()"></span> <span onclick="addreturnmilkdetails()">Add Return Milk</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='Inwardsilo_fillform' style="display: none; padding-left:250px;">
                    <table align="center">
                        <tr>
                        <td>
                                <label>
                                    Date</label>
                                <input id="txt_rtndate" type="datetime-local" class="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Department</label>
                                <select id="slct_Department" class="form-control" onchange="get_deptqty();">
                                    <option selected disabled value="Select Department">Select Department</option>
                                </select>
                                <label id="lbl_department" class="errormessage" style="color: red !important;">
                                    * Please select Department</label>
                            </td>
                                                        <td style="width:10px;"></td>

                            <td>
                                <label>
                                    SILO Name</label>
                                <select id="slct_Source_Name" class="form-control">
                                    <option selected disabled value="Select SILO Name">Select SILO Name</option>
                                </select>
                                <label id="lbl_silo" class="errormessage" style="color: red !important;">
                                    * Please select Silo</label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <label>
                                    Qty(Kgs)</label>
                                <input id="txt_qtykgs" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);"  placeholder="Enter Qty in Kgs" onkeyup="LtrsChange(this)">
                                <label id="lbl_Qtykgs" class="errormessage" style="color: red !important;">
                                    * Please enter Quantity</label>
                            </td>
                            <td></td>
                            <td>
                                <label>
                                    Qty(ltrs)</label>
                                <input id="txt_qtyltrs" type="text" class="form-control" name="vendorcode" readonly placeholder="Enter Qty in ltrs" >
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    FAT</label>
                                <input id="txt_fat" type="text" class="form-control" name="vendorcode" placeholder="FAT" onkeypress="return isFloat(event);">
                            </td>
                            <td></td>
                            <td>
                                <label>
                                    SNF</label>
                                <input id="txt_snf" type="text" class="form-control" name="vendorcode" placeholder="SNF" readonly onkeypress="return isFloat(event);"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    CLR</label>
                                <input id="txt_clr" type="text" class="form-control" name="vendorcode" placeholder="SNF" onkeypress="return isFloat(event);"
                                    onkeyup="LRChange(this)" />
                                <label id="lbl_clr" class="errormessage" style="color: red !important;">
                                    * Please enter CLR</label>
                            </td>
                             <td></td>
                            <td>
                                <label>
                                    Return MilkType</label>
                                 <select id="slct_milktype" class="form-control">
                                    <option selected disabled value="Select Milk Type">Select Milk Type</option>
                                    <option value="cuttingmilk">Cutting Milk</option>
                                    <option value="mixedmilk">Mixed Milk</option>
                                    <option value="returnmilk">Return Milk</option>

                                </select>
                                 <label id="lbl_milktype" class="errormessage" style="color: red !important;">
                                    * Please enter MilkType</label>
                            </td>
                        </tr>
                        <tr>
                            <label id="lbl_sno"></label>
                        </tr>
                        </table>
                        <div style="padding-left: 126px;padding-top: 5px;">
                        <table>
                        <tr>
                            <td>
                                <input id='save_departmentqtydetails' type="button" class="btn btn-success" name="submit"
                                    value='Save' onclick="save_returnmilkdetails_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>
                         </table>
                        </div>
                   
                </div>
            </div>
        </div>
            </div>
            <div id="div_returnmilkapprove">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Return Milk Pending Details
                </h3>
            </div>
            <div class="box-body">
                
                <div id="div_approveget">
                </div>
                
            </div>
        </div>
     </div>
    </div>
    </section>
</asp:Content>

