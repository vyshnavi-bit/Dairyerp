<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="auditqltytest.aspx.cs" Inherits="auditqltytest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(function () {
        $('#div_batchwise').css('display', 'block');
        get_batchqualitytesting_details();
       
        $('#batch_btn_addDept').click(function () {
            $('#batch_fillform').css('display', 'block');
            $('#batch_showlogs').css('display', 'none');
            $('#div_batchgetdata').hide();
            
        });
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
        $('#txt_batchdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        Get_batch_details();
        $('#close_batchqltytest').click(function () {
            $('#batch_fillform').css('display', 'none');
            $('#batch_showlogs').css('display', 'block');
            $('#div_batchgetdata').show();
            batch_clearvalues();
            get_batchqualitytesting_details();
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
            $('#txt_batchdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        });
    });
    function addcreadetails() {
        $('#cream_fillform').css('display', 'block');
        $('#showlogs_cream').css('display', 'none');
        $('#div_creamdetailsdata').hide();
        Getcreamdetails();
        get_cream_qualitytesting_details();
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
        $('#txt_cdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
    }
    function addbatchtesting() {
        $('#batch_fillform').css('display', 'block');
        $('#batch_showlogs').css('display', 'none');
        $('#div_batchgetdata').hide();
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
        $('#txt_batchdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
    }
    function addgheedetails() {
        $('#ghee_fillform').css('display', 'block');
        $('#ghee_showlogs').css('display', 'none');
        $('#div_gheedata').hide();
        get_gheeproduction_details();
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
        $('#txt_gheedate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
    }
    function addsilowisetesing() {
        $('#silodetails_fillform').css('display', 'block');
        $('#silo_showlogs').css('display', 'none');
        $('#div_getsilodata').hide();
        get_Silos();
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
        $('#txt_silodate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
    }
    function addcurdpacketdetails() {
        $('#curpacket_fillform').css('display', 'block');
        $('#curd_showlogs').css('display', 'none');
        $('#div_getcurddata').hide();
        get_curdproduction_details();
        get_product_details();
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
        $('#txt_curddate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
    }
    function showbatchwisedetails() {
        $('#div_batchwise').css('display', 'block');
        $('#div_silowise').css('display', 'none');
        $('#div_curdsection').css('display', 'none');
        $('#div_gheesection').css('display', 'none');
        $('#div_creamsec').css('display', 'none');
        get_batchqualitytesting_details();
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
    //Function for only no
    $(document).ready(function () {
        $("#txt_dcno,#txt_Inwardno").keydown(function (event) {
            // Allow: backspace, delete, tab, escape, and enter
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });
    });
    function validate(evt) {
        var theEvent = evt || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        var regex = /[0-9]|\./;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
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
    //Batch

    function save_batch_wise_qualitytesting_click() {
        var date = document.getElementById('txt_batchdate').value;
        var qco = document.getElementById('txt_batchqco').value;
        var Remarks = document.getElementById('txt_batchremarks').value;
        var Chemist = document.getElementById('txt_batchchemist').value;
        var SourceID = "46";
        var btnvalue = document.getElementById('save_batchqltytest').innerHTML;
        var sample = document.getElementById('txt_batchsampleno').value;
        var sno = document.getElementById('lbl_batchsno').innerHTML;
        if (date == "") {
            alert("Please Select Date");
            return false;
        }
        if (SourceID == "" || SourceID == "Select Batch") {
            alert("Select Batch name");
            return false;
        }
        if (Chemist == "") {
            alert("Enter Chemist name");
            return false;
        }
        if (qco == "") {
            alert("Enter qco name");
            return false;
        }
        var MilkqualityDetailsarray = [];
        $('#tbl_batchwiseqlty_testing_details> tbody > tr').each(function () {
            var Qtykg = $(this).find('[name="Qtykg"]').val();
            var Qtyltr = $(this).find('[name="Qtyltr"]').val();
            var fat = $(this).find('[name="FAT"]').val();
            var snf = $(this).find('[name="SNF"]').val();
            var temp = $(this).find('[name="TEMP"]').val();
            var acidity = $(this).find('[name="Acidity"]').val();
            var clr = $(this).find('[name="CLR"]').val();
            var cob = $(this).find('select[name*="COB"] :selected').val(); // $(this).find('[name="COB"]').val();
            var ot = $(this).find('[name="OT"]').val();
            var hs = $(this).find('[name="HS"]').val();
            var phosps = $(this).find('select[name*="Phosps"] :selected').val(); // $(this).find('[name="Phosps"]').val();
            var alcohol = $(this).find('[name="Alcohol"]').val();
            var neutralizers = $(this).find('[name="Neutralizers"]').val();
            var MBRT = $(this).find('[name="MBRT"]').val();
            if (Qtykg == "") {
            }
            else {
                MilkqualityDetailsarray.push({ 'Qtykg': Qtykg, 'Qtyltr': Qtyltr, 'fat': fat, 'snf': snf, 'temp': temp,
                    'acidity': acidity, 'clr': clr, 'cob': cob, 'ot': ot, 'hs': hs, 'phosps': phosps, 'alcohol': alcohol, 'neutralizers': neutralizers, 'mbrt': MBRT
                });
            }
        });
        if (MilkqualityDetailsarray.length == "0") {
            alert("Please enter fat,snf details");
            return false;
        }
        var confi = confirm("Do you want to Save Batch Wise Quality Testing Details ?");
        if (confi) {
            var data = { 'op': 'save_batch_wise_qualitytesting_click', 'qco': qco, 'date': date, 'remarks': Remarks, 'chemist': Chemist, 'sourceid': SourceID, 'sample': sample, 'sno': sno, 'btnvalue': btnvalue, 'MilkqualityDetailsarray': MilkqualityDetailsarray
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#batch_fillform').css('display', 'none');
                    $('#batch_showlogs').css('display', 'block');
                    $('#div_batchgetdata').show();
                    batch_clearvalues();
                    get_batchqualitytesting_details();
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
    function batch_clearvalues() {
        document.getElementById('txt_batchdate').value = "";
        document.getElementById('txt_batchqco').value = "";
        document.getElementById('txt_batchremarks').value = "";
        document.getElementById('txt_batchchemist').value = "";
        document.getElementById('save_batchqltytest').innerHTML = "Save";
        $('#batch_fillform').css('display', 'none');
        $('#batch_showlogs').css('display', 'block');
        $('#div_batchgetdata').show();
    }
    function LRChange(qtyid) {
        if (qtyid.value == "") {
            var qty = 0;
            var qtykg = 0;
            qtykg = $(qtyid).closest("tr").find('#txtkg').val();
            qtykg = parseFloat(qtykg).toFixed(3);
            var qtykgsltr = 0;
            var clr = 0;
            clr = parseFloat(qty).toFixed(3);
            var modclr = (clr / 1000) + 1;
            modclr = parseFloat(modclr).toFixed(3);
            qtykgsltr = qtykg / modclr;
            qtykgsltr = parseFloat(qtykgsltr).toFixed(2);
            $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);
        }
        else {
            var qtykg = 0;
            qtykg = $(qtyid).closest("tr").find('#txtkg').val();
            qtykg = parseFloat(qtykg).toFixed(3);
            var qtykgsltr = 0;
            var clr = 0;
            clr = parseFloat(qtyid.value).toFixed(3);
            var modclr = (clr / 1000) + 1;
            modclr = parseFloat(modclr).toFixed(5);
            qtykgsltr = qtykg / modclr;
            qtykgsltr = parseFloat(qtykgsltr).toFixed(0);
            $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);
            ////////For SNF ////////////////
            var fat = 0;
            fat = $(qtyid).closest("tr").find('#txtfat').val();
            fat = parseFloat(fat).toFixed(3);
            var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
            $(qtyid).closest("tr").find('#txtsnf').val(snfvalue);
        }
    }
    function Get_batch_details() {
        var names = 10;
        var results = '<div    style="overflow:auto;"><table id="tbl_batchwiseqlty_testing_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Qty(Kgs)</th><th scope="col" style="font-weight: 700;">Qty(Ltrs)</th><th scope="col" style="font-weight: 700;">FAT</th><th scope="col" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">CLR</th><th scope="col" style="font-weight: 700;">Temperature</th><th scope="col" style="font-weight: 700;">Acidity</th><th scope="col" style="font-weight: 700;">COB</th><th scope="col" style="font-weight: 700;">OT</th><th scope="col" style="font-weight: 700;">HS</th><th scope="col" style="font-weight: 700;">Phosps</th><th scope="col" style="font-weight: 700;">Alcohol%</th><th scope="col" style="font-weight: 700;">MBRT</th><th scope="col" style="font-weight: 700;">Neutralizers</th></tr></thead></tbody>';
        var j = 1;
        for (var i = 0; i < 1; i++) {
            results += '<td data-title="Sno" class="2">' + (i + 1) + '</td>';
            results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty(kgs)" onkeypress="return isFloat(event);" name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" disabled="disabled" placeholder="Qty(Ltrs)" id="txtltr" name="Qtyltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="FAT" onkeypress="return isFloat(event);" name="FAT" id="txtfat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF" onkeypress="return isFloat(event);" name="SNF" value="" id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="CLR" onkeypress="return isFloat(event);" onkeyup="LRChange(this);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Temp" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="TEMP" onkeypress="return isFloat(event);" name="TEMP" value="" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Acidity" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Acidity" name="Acidity"  id="txtacidity"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="COB" style="text-align:center;width:65px;" class="5"><select class="form-control" name="COB" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
            results += '<td data-title="OT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="OT" name="OT" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="HS" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="HS" name="HS" onkeypress="validate(event);" id="txths"   value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Phosps" style="text-align:center;width:65px;" class="5"><select class="form-control" name="Phosps" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
            results += '<td data-title="Alcohol" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Alcohol%" onkeypress="return isFloat(event);" id="txtalcohol" name="Alcohol" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="MBRT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="MBRT"  onkeypress="return isFloat(event);" name="MBRT" id="txtmbrt" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Neutralizers" style="text-align:center;" class="5"><select class="form-control" name="Neutralizers" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td></tr>';
        }
        results += '</table></div>';
        $("#div_batchdata").html(results);
    }
    function get_batchqualitytesting_details() {
        var data = { 'op': 'get_auditqualitytesting_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillbatchdetails(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function fillbatchdetails(msg) {
        var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Date</th></th><th scope="col" style="font-weight: bold;">Batch Name</th></th><th scope="col" style="font-weight: bold;">Qty(kgs)</th><th scope="col" style="font-weight: bold;">FAT</th><th scope="col" style="font-weight: bold;">SNF</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color:' + COLOR[l] + '">';
            //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getVendorvalues(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
            results += '<td class="11">' + msg[i].doe + '</td>';
            results += '<td data-title="Capacity" class="10">' + msg[i].batchname + '</td>';
            results += '<td data-title="Capacity" class="10">' + msg[i].OutwordQuantitykgs + '</td>';
            results += '<td data-title="Capacity" class="8">' + msg[i].fat + '</td>';
            results += '<td data-title="Capacity" class="9">' + msg[i].snf + '</td>';
            results += '<td style="display:none" class="4">' + msg[i].sectionid + '</td>';
            results += '<td style="display:none" class="12">' + msg[i].transno + '</td></tr>';
            l = l + 1;
            if (l == 4) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_batchgetdata").html(results);
    }
    //Silos
    function get_Silos() {
        var data = { 'op': 'get_Silo_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    Get_silo_details(msg);
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
    function save_silo_wise_qualitytesting_click() {
        var date = document.getElementById('txt_silodate').value;
        var qco = document.getElementById('txt_siloqco').value;
        var Remarks = document.getElementById('txt_siloremarks').value;
        var Chemist = document.getElementById('txt_silochemist').value;
        // var SourceID = document.getElementById('slct_Source_Name').value;
        var btnvalue = document.getElementById('save_silotestdetails').innerHTML;
        var sample = document.getElementById('txt_silosample').value;
        var sno = document.getElementById('lbl_silosno').innerHTML;
        if (date == "") {
            alert("Please Select Date");
            return false;
        }
        //            if (SourceID == "" || SourceID == "Select Batch") {
        //                alert("Select plant name");
        //                return false;
        //            }
        if (Chemist == "") {
            alert("Enter Chemist name");
            return false;
        }
        if (qco == "") {
            alert("Enter qco name");
            return false;
        }
        var MilksiloqualityDetailsarray = [];
        $('#tbl_milk_details> tbody > tr').each(function () {
            var Qtykg = $(this).find('[name="Qtykg"]').val();
            var Qtyltr = $(this).find('[name="Qtyltr"]').val();
            var fat = $(this).find('[name="FAT"]').val();
            var snf = $(this).find('[name="SNF"]').val();
            var temp = $(this).find('[name="TEMP"]').val();
            var acidity = $(this).find('[name="Acidity"]').val();
            var clr = $(this).find('[name="CLR"]').val();
            var cob = $(this).find('select[name*="COB"] :selected').val(); // $(this).find('[name="COB"]').val();
            var ot = $(this).find('[name="OT"]').val();
            var hs = $(this).find('[name="HS"]').val();
            var phosps = $(this).find('select[name*="Phosps"] :selected').val(); // $(this).find('[name="Phosps"]').val();
            var alcohol = $(this).find('[name="Alcohol"]').val();
            var neutralizers = $(this).find('[name="Neutralizers"]').val();
            var MBRT = $(this).find('[name="MBRT"]').val();
            var siloid = $(this).find('[name="siloid"]').val();
            if (Qtykg == "") {
            }
            else {
                MilksiloqualityDetailsarray.push({ 'Qtykg': Qtykg, 'Qtyltr': Qtyltr, 'fat': fat, 'snf': snf, 'temp': temp,
                    'acidity': acidity, 'clr': clr, 'cob': cob, 'ot': ot, 'hs': hs, 'phosps': phosps, 'alcohol': alcohol, 'neutralizers': neutralizers, 'mbrt': MBRT, 'siloid': siloid
                });
            }
        });
        if (MilksiloqualityDetailsarray.length == "0") {
            alert("Please enter quantity in kg's, fat, snf details");
            return false;
        }
        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var data = { 'op': 'save_silo_wise_qualitytesting_click', 'qco': qco, 'date': date, 'remarks': Remarks, 'chemist': Chemist, 'sample': sample, 'sno': sno, 'btnvalue': btnvalue, 'MilksiloqualityDetailsarray': MilksiloqualityDetailsarray
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#silodetails_fillform').css('display', 'none');
                    $('#silo_showlogs').css('display', 'block');
                    $('#div_getsilodata').show();
                    clearvalues_silo();
                    get_siloqualitytesting_details();
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
    function clearvalues_silo() {
        document.getElementById('txt_silodate').value = "";
        document.getElementById('txt_siloqco').value = "";
        document.getElementById('txt_siloremarks').value = "";
        document.getElementById('txt_silochemist').value = "";
        $('#silodetails_fillform').css('display', 'none');
        $('#silo_showlogs').css('display', 'block');
        $('#div_getsilodata').show();
    }
    function LRChange_silo(qtyid) {
        if (qtyid.value == "") {
            var qty = 0;
            var qtykg = 0;
            qtykg = $(qtyid).closest("tr").find('#txtkg').val();
            qtykg = parseFloat(qtykg).toFixed(3);
            var qtykgsltr = 0;
            var clr = 0;
            clr = parseFloat(qty).toFixed(3);
            var modclr = (clr / 1000) + 1;
            modclr = parseFloat(modclr).toFixed(3);
            qtykgsltr = qtykg / modclr;
            qtykgsltr = parseFloat(qtykgsltr).toFixed(2);
            $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);
        }
        else {
            var qtykg = 0;
            qtykg = $(qtyid).closest("tr").find('#txtkg').val();
            qtykg = parseFloat(qtykg).toFixed(3);
            var qtykgsltr = 0;
            var clr = 0;
            clr = parseFloat(qtyid.value).toFixed(3);
            var modclr = (clr / 1000) + 1;
            modclr = parseFloat(modclr).toFixed(5);
            qtykgsltr = qtykg / modclr;
            qtykgsltr = parseFloat(qtykgsltr).toFixed(0);
            $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);
            ////////For SNF ////////////////
            var fat = 0;
            fat = $(qtyid).closest("tr").find('#txtfat').val();
            fat = parseFloat(fat).toFixed(3);
            var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
            $(qtyid).closest("tr").find('#txtsnf').val(snfvalue);

        }
    }
    function Get_silo_details(msg) {
        var names = 10;
        var results = '<div    style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Silo Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th><th scope="col">TEMP</th><th scope="col">Acidity</th><th scope="col">COB</th><th scope="col">OT</th><th scope="col">HS</th><th scope="col">Phosps</th><th scope="col">Alcohol%</th><th scope="col">MBRT</th><th scope="col">Neutralizers</th></tr></thead></tbody>';
        var j = 1;
        for (var i = 0; i < msg.length; i++) {
            results += '<td data-title="Sno" class="2">' + (i + 1) + '</td>';
            results += '<td data-title="Silo" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Silo" name="siloqty" id="txtsilo" value="' + msg[i].SiloName + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty(kgs)"  onkeypress="return isFloat(event);" name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" disabled="disabled" placeholder="Qty(Ltrs)" id="txtltr" name="Qtyltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="FAT"  onkeypress="return isFloat(event);" name="FAT" id="txtfat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF"  onkeypress="return isFloat(event);" name="SNF" value="" id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="CLR"  onkeypress="return isFloat(event);"  onkeyup="LRChange_silo(this);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Temp" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="TEMP"  onkeypress="return isFloat(event);" name="TEMP" value="" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Acidity" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Acidity"  onkeypress="return isFloat(event);" name="Acidity"  id="txtacidity"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="COB" style="text-align:center;width:65px;" class="5"><select class="form-control" name="COB" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
            results += '<td data-title="OT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="OT" name="OT" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="HS" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="HS" name="HS" id="txths"   value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Phosps" style="text-align:center;width:65px;" class="5"><select class="form-control" name="Phosps" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
            results += '<td data-title="Alcohol" style="text-align:center;" class="5"><input class="form-control" type="text" onkeypress="return isFloat(event);" placeholder="Alcohol%" id="txtalcohol" name="Alcohol" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="MBRT" style="text-align:center;" class="5"><input class="form-control"  type="text" onkeypress="return isFloat(event);" placeholder="MBRT" name="MBRT" id="txtmbrt" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Neutralizers" style="text-align:center;" class="5"><select class="form-control" name="Neutralizers" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
            results += '<td style="display:none;" data-title="sioid" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="siloid" name="siloid" id="txtsiloid" value="' + msg[i].SiloId + '"/></td></tr>';
        }
        results += '</table></div>';
        $("#div_silodata").html(results);
    }

    function get_siloqualitytesting_details() {
        var data = { 'op': 'get_siloqualitytesting_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillsilodetails(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function fillsilodetails(msg) {
        var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;">Silo Name</th><th scope="col" style="font-weight: bold;">Quantity(kgs)</th><th scope="col" style="font-weight: bold;">Quantity(ltrs)</th><th scope="col" style="font-weight: bold;">FAT</th><th scope="col" style="font-weight: bold;">SNF</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color:' + COLOR[l] + '">';
            results += '<td class="11">' + msg[i].doe + '</td>';
            results += '<td class="4">' + msg[i].SiloName + '</td>';
            results += '<td data-title="Capacity" class="10">' + msg[i].Quantity + '</td>';
            results += '<td data-title="Capacity" class="10">' + msg[i].Quantity + '</td>';
            results += '<td data-title="Capacity" class="8">' + msg[i].fat + '</td>';
            results += '<td data-title="Capacity" class="9">' + msg[i].snf + '</td>';
            results += '<td style="display:none" class="12">' + msg[i].transno + '</td></tr>';
            l = l + 1;
            if (l == 4) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_getsilodata").html(results);
    }
    function get_product_details() {
        var data = { 'op': 'get_product_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    //                        fillcurdproducts(msg);
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
    // Curd
    function save_curd_packetwise_qualitytesting_click() {
        var date = document.getElementById('txt_curddate').value;
        var qco = document.getElementById('txt_curdqco').value;
        var Remarks = document.getElementById('txt_curdremarks').value;
        var Chemist = document.getElementById('txt_curdchemist').value;
        var btnvalue = document.getElementById('curd_btnvalue').innerHTML;
        var sample = document.getElementById('txt_curdsample').value;
        var sno = document.getElementById('lbl_curdsno').innerHTML;
        if (date == "") {
            alert("Please Select Date");
            return false;
        }
        if (sample == "") {
            alert("Please Enter Sample No");
            return false;
        }
        var curdpktqualityarray = [];
        $('#tbl_curdpacket_details> tbody > tr').each(function () {
            var date = $(this).find('[name="Date"]').val();
            var packsize = $(this).find('select[name="packetsize"] :selected').val();
            var productid = $(this).find('[name="pid"]').val();
            var batchno = $(this).find('[name="BatchNo"]').val();
            var mrp = $(this).find('[name="Mrp"]').val();
            var structure = $(this).find('[name="Structure"]').val();
            var ot = $(this).find('[name="OT"]').val();
            var acidity = $(this).find('[name="Acidity"]').val();
            var temp = $(this).find('[name="TEMP"]').val();
            var usedbydate = $(this).find('[name="USEBYDATE"]').val();
            if (mrp == "") {
            }
            else {

                curdpktqualityarray.push({ 'date': date, 'packetsize': packsize, 'productid': productid, 'batchno': batchno, 'mrp': mrp, 'structure': structure, 'ot': ot, 'acidity': acidity, 'temp': temp, 'usedbydate': usedbydate
                });
            }
        });
        if (curdpktqualityarray.length == "0") {
            alert("Please enter packetsize");
            return false;
        }
        var confi = confirm("Do you want to Save Curd Packet Quality Testing Details ?");
        if (confi) {
            var data = { 'op': 'save_curd_packetwise_qualitytesting_click', 'qco': qco, 'Chemist': Chemist, 'date': date, 'sample': sample, 'remarks': Remarks, 'sno': sno, 'btnvalue': btnvalue, 'curdpktqualityarray': curdpktqualityarray
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#curpacket_fillform').css('display', 'none');
                    $('#curd_showlogs').css('display', 'block');
                    $('#div_getcurddata').show();
                    clearvalues_curddetails();
                    get_curdproductqualitytesting_details();
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
    function clearvalues_curddetails() {
        document.getElementById('txt_curddate').value = "";
        document.getElementById('txt_curdqco').value = "";
        document.getElementById('txt_curdremarks').value = "";
        document.getElementById('txt_curdchemist').value = "";
        document.getElementById('txt_curdsample').value = "";
        document.getElementById('curd_btnvalue').innerHTML = "Save";
        get_curdproduction_details();
        $('#curpacket_fillform').css('display', 'none');
        $('#curd_showlogs').css('display', 'block');
        $('#div_getcurddata').show();
    }
    function get_curdproduction_details() {
        var data = { 'op': 'lab_curdproduction_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    Getcurddetails(msg);
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
    function Getcurddetails(msg) {
        var names = 10;
        var results = '<div    style="overflow:auto;"><table id="tbl_curdpacket_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Product Name</th><th scope="col">Pack Size</th><th scope="col">BatchNo</th><th scope="col">Used By Date</th><th scope="col">Mrp</th><th scope="col">Structure</th><th scope="col">OT</th><th scope="col">Temp</th><th scope="col">Acidity%</th></tr></thead></tbody>';
        var j = 1;
        for (var i = 0; i < msg.length; i++) {
            results += '<td data-title="Sno" class="0">' + (i + 1) + '</td>';
            results += '<td data-title="Product" style="text-align:center;" class="1"><input class="form-control" type="text"  placeholder="PacketSize" id="txtproductname" readonly name="packetsize" value="' + msg[i].productname + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="PacketSize" style="text-align:center;" class="2"><select class="form-control" type="text"  placeholder="PacketSize" id="txtpacketsize" name="packetsize" value="' + msg[i].productid + '" style="font-size:12px;padding: 0px 5px;height:30px; width: 70px"><option  value="100 ML">100 ML</option><option  value="175 ML">175 ML</option><option  value="180 ML">180 ML</option><option  value="200 ML">200 ML</option><option  value="500 ML">500 ML</option><option  value="450 GRMS">450 GRMS</option><option  value="5 KG">5 KG</option><option  value="10 KG">10 KG</option><option  value="40 KG">40 KG</option><option  value="20 KG">20 KG</option><option  value="10 KG BUCKET">10 KG BUCKET</option><option  value="CURD MILK-AMARARAAJA">CURD MILK-AMARARAAJA</option><option  value="STD CURD BUCKETS">STD CURD BUCKETS</option></select></td>';
            results += '<td data-title="BatchNo" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="BatchNo" name="BatchNo" id="txtBatchNo" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="USEBYDATE" style="text-align:center;" class="4"><input class="form-control" type="date" placeholder="Usebydate" name="USEBYDATE" value="" id="txtusedbydate" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Mrp" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Mrp" name="Mrp" value="" id="txtMrp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Structure" id="txstructure"  class="6"><input class="form-control" type="text" placeholder="Structure"   id="txtStructure" name="Structure" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="OT" style="text-align:center;" class="7"><input class="form-control" type="text" placeholder="OT" name="OT" value="" id="txtOT" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Temp" style="text-align:center;" class="8"><input class="form-control" type="text" placeholder="Temp" name="TEMP"  id="txtTemp"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Acidity" style="text-align:center;" class="9"><input class="form-control" type="text" placeholder="Acidity" name="Acidity"  id="txtAcidity"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td style="display:none" data-title="pid" style="text-align:center;" class="10"><input class="form-control" type="text" placeholder="pid" name="pid" id="txtpid" value="' + msg[i].productid + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
        }
        results += '</table></div>';
        $("#div_curdpacket").html(results);
    }
    function get_curdproductqualitytesting_details() {
        var data = { 'op': 'get_curd_packetwise_qualitytesting_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillcurdqualitydetails(msg);
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
    function fillcurdqualitydetails(msg) {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">Packet Size</th><th scope="col" style="font-weight: bold;">Sample Number</th><th scope="col" style="font-weight: bold;">Used By Date</th><th scope="col" style="font-weight: bold;">Temperature</th><th scope="col" style="font-weight: bold;">MRP</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color:' + COLOR[l] + '">';
            results += '<td scope="row" class="1" >' + msg[i].productname + '</td>';
            results += '<td scope="row" class="2" >' + msg[i].packetsize + '</td>';
            results += '<td scope="row" class="3" >' + msg[i].sampleno + '</td>';
            results += '<td scope="row" class="4" >' + msg[i].usedbydate + '</td>';
            results += '<td scope="row" class="5" >' + msg[i].ptemp + '</td>';
            results += '<td scope="row" class="6" >' + msg[i].pmrp + '</td>';
            results += '<td style="display:none" class="7">' + msg[i].productid + '</td>';
            results += '<td style="display:none" class="8">' + msg[i].sno + '</td></tr>';
            l = l + 1;
            if (l == 4) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_getcurddata").html(results);
    }

    //Ghee
    function save_ghee_wise_qualitytesting_click() {
        var date = document.getElementById('txt_gheedate').value;
        var qco = document.getElementById('txt_gheeqco').value;
        var Remarks = document.getElementById('txt_gheeremarks').value;
        var Chemist = document.getElementById('txt_gheechemist').value;
        var sampleno = document.getElementById('txt_gheesample').value;
        var btnvalue = document.getElementById('save_gheetransactions').innerHTML;
        var sno = document.getElementById('lbl_gheesno').innerHTML;

        if (date == "") {
            alert("Please Select Date");
            return false;
        }
        if (Chemist == "") {
            alert("Enter Chemist name");
            return false;
        }
        if (sampleno == "") {
            alert("Enter Sample No");
            return false;
        }

        var gheequalityDetailsarray = [];
        $('#tbl_ghee_details> tbody > tr').each(function () {
            var date = $(this).find('[name="DATE"]').val();
            var packsize = $(this).find('select[name="packsize"] :selected').val();
            var productid = $(this).find('[name="pid"]').val();
            var batchno = $(this).find('[name="BATCHNO"]').val();
            var usebydate = $(this).find('[name="USEBYDATE"]').val();
            var mrp = $(this).find('[name="MRP"]').val();
            var structure = $(this).find('[name="STRUCTURE"]').val();
            var ot = $(this).find('[name="OT"]').val();
            var temp = $(this).find('[name="TEMP"]').val();
            var acidity = $(this).find('[name="ACIDITY"]').val();
            var remarks = $(this).find('[name="REMARKS"]').val();

            if (mrp == "") {
            }
            else {
                gheequalityDetailsarray.push({ 'date': date, 'productid': productid, 'packsize': packsize, 'batchno': batchno, 'usebydate': usebydate, 'mrp': mrp,
                    'structure': structure, 'ot': ot, 'temp': temp, 'acidity': acidity, 'remarks': remarks
                });
            }
        });
        if (gheequalityDetailsarray.length == "0") {
            alert("Please enter packsize,batchno details");
            return false;
        }
        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var data = { 'op': 'save_ghee_wise_qualitytesting_click', 'sampleno': sampleno, 'qco': qco, 'date': date, 'remarks': Remarks, 'chemist': Chemist, 'sno': sno, 'btnvalue': btnvalue, 'gheequalityDetailsarray': gheequalityDetailsarray
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#ghee_fillform').css('display', 'none');
                    $('#ghee_showlogs').css('display', 'block');
                    $('#div_gheedata').show();
                    clearvalues_ghee();
                    get_gheeproductqualitytesting_details();
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
    function clearvalues_ghee() {
        document.getElementById('txt_gheedate').value = "";
        document.getElementById('txt_gheeqco').value = "";
        document.getElementById('txt_gheeremarks').value = "";
        document.getElementById('txt_gheechemist').value = "";
        $('#ghee_fillform').css('display', 'none');
        $('#ghee_showlogs').css('display', 'block');
        $('#div_gheedata').show();
    }
    function get_gheeproduction_details() {
        var data = { 'op': 'get_gheeproduction_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    Getgheedetails(msg);
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
    function Getgheedetails(msg) {
        var names = 11;
        var results = '<div    style="overflow:auto;"><table id="tbl_ghee_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Packet Size</th><th scope="col" style="font-weight: 700;">Batch Number</th><th scope="col" style="font-weight: 700;">Manufacture Date</th><th scope="col" style="font-weight: 700;">Mrp</th><th scope="col" style="font-weight: 700;">AppeaRance</th><th scope="col" style="font-weight: 700;">OT</th><th scope="col" style="font-weight: 700;">Temperature</th><th scope="col" style="font-weight: 700;">Acidity</th><th scope="col">Remarks</th></tr></thead></tbody>';
        var j = 1;
        for (var i = 0; i < msg.length; i++) {
            results += '<td data-title="Sno" class="0">' + (i + 1) + '</td>';
            results += '<td data-title="Product" style="text-align:center;" class="1"><input class="form-control" type="text"  placeholder="Product Name" id="txtproductname" name="productname" value="' + msg[i].productname + '" style="font-size:12px;padding: 0px 5px;height:30px;"></td>';
            results += '<td data-title="packsize" style="text-align:center;" class="2"><select class="form-control" type="text" placeholder="Packet Size" name="packsize" id="pcktsize" value="" style="font-size:12px;padding: 0px 5px;height:30px;"><option  value="50 ML">Loose</option><option  value="50 ML">50 ML</option><option  value="100 ML">100 ML</option><option  value="180 ML">180 ML</option><option  value="200 ML">200 ML</option><option  value="500 ML">500 ML</option><option  value="450 GRMS">450 GRMS</option><option  value="1000 ML">1000 ML</option><option  value="1 KG">1 KG</option><option  value="5 KG TIN">5 KG TIN</option><option  value="15 KG TIN">15 KG TIN</option></select></td>';
            results += '<td data-title="BATCHNO" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Batchno" name="BATCHNO" id="txtfat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="USEBYDATE" style="text-align:center;" class="4"><input class="form-control" type="date" placeholder="Usebydate" name="USEBYDATE" value="" id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="MRP" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Mrp" id="txtclr" name="MRP" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="STRUCTURE" style="text-align:center;" class="6 "><input class="form-control" type="text" placeholder="Structure" name="STRUCTURE" value="" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="OT" style="text-align:center;" class="7"><input class="form-control" type="text" placeholder="OT" name="OT"  id="txtacidity"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="TEMP" style="text-align:center;width:65px;" class="8"><input class="form-control" name="TEMP" placeholder="Temp" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ></td>';
            results += '<td data-title="ACIDITY" style="text-align:center;" class="9"><input class="form-control" type="text" placeholder="Acidity" name="ACIDITY" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="REMARKS" style="text-align:center;" class="10"><input class="form-control" name="REMARKS"  value="" placeholder="Remarks" style="width:100%; "font-size:12px;padding: 0px 5px;height:30px;" ></td>';
            results += '<td style="display:none" data-title="pid" style="text-align:center;" class="11"><input class="form-control" type="text" placeholder="pid" name="pid" id="txtpid" value="' + msg[i].productid + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
        }
        results += '</table></div>';
        $("#div_gheedetails").html(results);
    }
    function getme(thisid) {

    }
    function get_gheeproductqualitytesting_details() {
        var data = { 'op': 'get_gheeproductqualitytesting_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillgheequalitydetails(msg);
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
    function fillgheequalitydetails(msg) {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">Packet Size</th><th scope="col" style="font-weight: bold;">Sample No</th><th scope="col" style="font-weight: bold;">Manufacture Date</th><th scope="col" style="font-weight: bold;">Temperature</th><th scope="col">MRP</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color:' + COLOR[l] + '">';
            results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
            results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].packetsize + '</td>';
            results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].sampleno + '</td>';
            results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].usedbydate + '</td>';
            results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].ptemp + '</td>';
            results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].pmrp + '</td>';
            results += '<td style="display:none" class="2">' + msg[i].productid + '</td>';
            results += '<td style="display:none" class="3">' + msg[i].sno + '</td></tr>';
            l = l + 1;
            if (l == 4) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_gheedata").html(results);
    }

    // Cream 

    function Getcreamdetails() {
        var names = 10;
        var results = '<div    style="overflow:auto;"><table id="tbl_cream_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Qty(Kgs)</th><th scope="col" style="font-weight: 700;">FAT</th><th scope="col" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">Temperature</th><th scope="col" style="font-weight: 700;">Acidity</th></tr></thead></tbody>';
        var j = 1;
        for (var i = 0; i < 1; i++) {
            results += '<td data-title="Sno" class="2">' + (i + 1) + '</td>';
            results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="number" placeholder="Qty(kgs)" name="Qtykg" id="txt_kg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="fat" style="text-align:center;" class="3"><input class="form-control" type="number" placeholder="FAT" name="FAT" id="txt_fat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="snf" style="text-align:center;" class="3"><input class="form-control" type="number" placeholder="SNF" name="SNF" id="txt_snf" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="temprature" style="text-align:center;" class="3"><input class="form-control" type="number" placeholder="Temperature" name="TEMPRATURE" id="txt_temprature" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="acidity" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="ACIDITY" name="Acidity" id="txt_acidity" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
        }
        results += '</table></div>';
        $("#div_creamdata").html(results);
    }
    function save_cream_qualitytesting_click() {
        var date = document.getElementById('txt_cdate').value;
        var creamtype = document.getElementById('slct_creamtype').value;
        var remarks = document.getElementById('txt_cRemarks').value;
        var chemist = document.getElementById('txt_cChemist').value;
        var qco = document.getElementById('txt_cqco').value;
        var sno = document.getElementById('lbl_csno').value;
        var btnvalue = document.getElementById('save_creamtest').innerHTML;
        var rows = $("#tbl_cream_details tr:gt(0)");
        var creamdetails = new Array();
        $(rows).each(function (i, obj) {
            if ($(this).find('#txt_fat').val() != "") {
                creamdetails.push({ Qtykg: $(this).find('#txt_kg').val(), fat: $(this).find('#txt_fat').val(), snf: $(this).find('#txt_snf').val(), temprature: $(this).find('#txt_temprature').val(), acidity: $(this).find('#txt_acidity').val() });
            }
        });
        if (creamdetails.length == 0) {
            alert("Please Enter FAT and SNF balance");
            return false;
        }
        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var data = { 'op': 'save_cream_qualitytesting_click', 'creamdetails': creamdetails, 'date': date, 'creamtype': creamtype, 'remarks': remarks, 'chemist': chemist, 'qco': qco, 'btnvalue': btnvalue, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        clearvalues_cream();
                        $('#cream_fillform').css('display', 'none');
                        $('#showlogs_cream').css('display', 'block');
                        $('#div_creamdetailsdata').show();
                        get_cream_qualitytesting_details();
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
    function clearvalues_cream() {
        document.getElementById('txt_cdate').value = "";
        document.getElementById('slct_creamtype').selectedIndex = "";
        document.getElementById('txt_cRemarks').value = "";
        document.getElementById('txt_cChemist').value = "";
        document.getElementById('txt_cqco').value = "";
        document.getElementById('save_creamtest').innerHTML = "Save";
        $('#cream_fillform').css('display', 'none');
        $('#showlogs_cream').css('display', 'block');
        $('#div_creamdetailsdata').show();
    }
    function get_cream_qualitytesting_details() {
        var data = { 'op': 'get_cream_qualitytesting_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillcreamdetails(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function fillcreamdetails(msg) {
        var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Cream Type</th></th><th scope="col" style="font-weight: bold;">Qty(kgs)</th><th scope="col" style="font-weight: bold;">Fat</th><th scope="col" style="font-weight: bold;">Snf</th><th scope="col" style="font-weight: bold;">Date</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color:' + COLOR[l] + '">';
            results += '<td   class="10" >' + msg[i].creamtype + '</td>';
            results += '<td   class="10">' + msg[i].qtykgs + '</td>';
            results += '<td  class="8">' + msg[i].fat + '</td>';
            results += '<td   class="9">' + msg[i].snf + '</td>';
            results += '<td  class="4">' + msg[i].doe + '</td>';
            results += '<td style="display:none" class="12">' + msg[i].sno + '</td>';
            l = l + 1;
            if (l == 4) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_creamdetailsdata").html(results);
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            <i class="fa fa-files-o" aria-hidden="true"></i>Quality Testing<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Lab</a></li>
            <li><a href="#">Quality Testing Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_documents" class="active"><a data-toggle="tab" href="#" onclick="showbatchwisedetails()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Sample No wise Wise</a></li>
                </ul>
            </div>
            <div id="div_batchwise">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Sample Wise Quality Testing Entry
                </h3>
            </div>
            <div class="box-body">
                <div id="batch_showlogs" style="text-align: -webkit-right;">
                    <%--<input id="batch_btn_addDept" type="button" name="submit" value='Add Quality Testing' class="btn btn-success" />--%>
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addbatchtesting()"></span> <span onclick="addbatchtesting()">Add Sample Testing</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_batchgetdata">
                </div>
                <div id='batch_fillform' style="display: none;">
                <div style="padding-left:200px;">
                    <table align="center">
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                   Date time</label>
                            </td>
                            <td>
                                <input id="txt_batchdate" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date" />
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Sample No</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input id="txt_batchsampleno" type="text" class="form-control" name="vendorcode" placeholder="Sample No" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                    </div>
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Details</h3>
                        </div>
                        <div id="div_batchdata">
                        </div>
                    </div>
                    <div style="text-align: center;">
                    <div style="padding-left:200px;">
                        <table align="center">
                            <tr>
                                <td style="height: 40px;">
                                    <label>
                                        Remarks</label>
                                </td>
                                <td colspan="6">
                                    <textarea rows="3" cols="45" id="txt_batchremarks" class="form-control" maxlength="200"
                                        placeholder="Enter Remarks"></textarea>
                                    <%--   <textarea id="txt_batchremarks" class="form-control" rows="5" cols="45" placeholder="Remarks" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Chemist</label>
                                </td>
                                <td>
                                    <input id="txt_batchchemist" type="text" class="form-control" name="vendorcode" placeholder="Chemist" />
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td>
                                    <label>
                                        QCO</label>
                                </td>
                                <td>
                                    <input id="txt_batchqco" type="text" class="form-control" name="vendorcode" placeholder="QCO" />
                                </td>
                            </tr>
                            <tr hidden>
                                <td>
                                    <label id="lbl_batchsno">
                                    </label>
                                </td>
                            </tr>
                           
                        </table>
                        </div>
                        <%--<input id='save_batchqltytest' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_batch_wise_qualitytesting_click()" />
                        <input id='close_batchqltytest' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="batch_clearvalues()" />
                        <input id='batch_btnprint' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />--%>
                        <div  style="padding-left: 35%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_batchqltytest1" onclick="save_batch_wise_qualitytesting_click()"></span><span id="save_batchqltytest" onclick="save_batch_wise_qualitytesting_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='close_batchqltytest1' onclick="batch_clearvalues()"></span><span id='close_batchqltytest' onclick="batch_clearvalues()">Close</span>
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
            </div>
    </section>
</asp:Content>

