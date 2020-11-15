<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="SiloMonitor.aspx.cs" Inherits="SiloMonitor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var branchid = '<%=Session["Branch_ID"] %>';
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
            $('#txt_istdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            get_Silomonitor_details();
            $('#divsilomonitor').css('display', 'block');
            $('#divintrasilo').css('display', 'none');
            $('#divoutwardsilo').css('display', 'none');
            $('#btn_outclose').click(function () {
                $('#outwardsilo_fillform').css('display', 'none');
                $('#outshowlogs').css('display', 'block');
                $('#div_outdata').show();
                outClearvalues();
            });
        });
        function addoutward() {
            $('#outwardsilo_fillform').css('display', 'block');
            $('#outshowlogs').css('display', 'none');
            $('#div_outdata').hide();
        }
        function clearoutwarddet() {
            $('#outwardsilo_fillform').css('display', 'none');
            $('#outshowlogs').css('display', 'block');
            $('#div_outdata').show();
            outClearvalues();
        }
        function showmonitor() {
            $('#divsilomonitor').css('display', 'block');
            $('#divintrasilo').css('display', 'none');
            $('#divoutwardsilo').css('display', 'none');
        }
        function showintra() {
            $('#divsilomonitor').css('display', 'none');
            $('#divintrasilo').css('display', 'block');
            $('#divoutwardsilo').css('display', 'none');
            get_intraSilos();
            get_toSilos();
            get_intrasilo_transaction_details();
        }
        function showoutward() {
            $('#divsilomonitor').css('display', 'none');
            $('#divintrasilo').css('display', 'none');
            $('#divoutwardsilo').css('display', 'block');
            $('#outwardsilo_fillform').css('display', 'none');
            $('#outshowlogs').css('display', 'block');
            $('#div_outdata').show();
            get_outward_silo_transaction();
            get_outSilos();
            get_SiloDepartments();
            get_outBatchs();
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
            get_outward_silo_transaction();
        });
        function get_Silos() {
            var data = { 'op': 'get_Silo_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillSilos(msg);
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
        function fillSilos(msg) {
            var data = document.getElementById('slct_Silo_Name');
            var length = data.options.length;
            document.getElementById('slct_Silo_Name').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Silo Name";
            opt.value = "Select Silo Name";
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
            $("#txt_phoneno,#txt_servtax").keydown(function (event) {
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
        //------------>Prevent Backspace<--------------------//
        $(document).unbind('keydown').bind('keydown', function (event) {
            var doPrevent = false;
            if (event.keyCode === 8) {
                var d = event.srcElement || event.target;
                if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD'))
            || d.tagName.toUpperCase() === 'TEXTAREA') {
                    doPrevent = d.readOnly || d.disabled;
                } else {
                    doPrevent = true;
                }
            }

            if (doPrevent) {
                event.preventDefault();
            }
        });
        // silo monitor //
        function for_save_SiloMonitor() {
            var Quantity = document.getElementById('txt_Quantity').value;
            var SiloId = document.getElementById('slct_Silo_Name').value;
            // var sno = document.getElementById('lbl_sno').innerHTML;
            var btnval = document.getElementById('btn_save').value;
            var flag = false;
            if (SiloId == "Select Silo Name") {
                $("#lbl_SiloName_error_msg").show();
                flag = true;
            }
            if (Quantity == "") {
                $("#lbl_Quantity_error_msg").show();
                flag = true;
            }
            if (flag) {
                return;
            }
            var data = { 'op': 'for_save_Silomonitor', 'SiloId': SiloId, 'Quantity': Quantity, 'btnval': btnval };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("Successfully added Silo quantity");
                            forclearall();
                            get_Silomonitor_details();
                            $('#div_Deptdata').show();
                            $('#fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                        }
                        else {
                            alert(msg);
                        }
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall() {
            document.getElementById('txt_Quantity').value = "";
            document.getElementById('slct_Silo_Name').selectedIndex = 0;
            document.getElementById('btn_save').value = "Save";
            $("#lbl_Quantity_error_msg").hide();
            $("#lbl_SiloName_error_msg").hide();
        }

        function get_Silomonitor_details() {
            var data = { 'op': 'get_Silomonitor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Branch Name</th><th scope="col" style="font-weight: bold;">Silo Name</th><th scope="col" style="font-weight: bold;">Quantity</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1" style="display:none;">' + msg[i].branchname + '</td>';
                results += '<td data-title="brandstatus" class="40" style="font-weight: 600;"><span class="" style="color: cadetblue;"></span>' + msg[i].branchname + '</td>';
                results += '<td data-title="Code" style="display:none;" class="2">' + msg[i].SiloName + '</td>';
                results += '<td data-title="brandstatus" class="40"><span class="fa fa-circle-o" style="color: cadetblue;"></span>' + msg[i].SiloName + '</td>';
                results += '<td data-title="brandstatus" class="40"><span style="color: cadetblue;"></span>' + msg[i].Quantity + '</td>';
                results += '<td data-title="Code" style="display:none;" class="3">' + msg[i].Quantity + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }

        var replaceHtmlEntites = (function () {
            var translate_re = /&(nbsp|amp|quot|lt|gt);/g;
            var translate = {
                "nbsp": " ",
                "amp": "&",
                "quot": "\"",
                "lt": "<",
                "gt": ">"
            };
            return function (s) {
                return (s.replace(translate_re, function (match, entity) {
                    return translate[entity];
                }));
            }
        })();
        // end silomonotor//

        // intra silo //
        function LRChange(qtyid) {
            if (branchid == 26 || branchid == 115) {
                var clr = 0;
                clr = parseFloat(qtyid.value).toFixed(3);
                var fat = 0;
                fat = document.getElementById('txt_fat').value;
                fat = parseFloat(fat).toFixed(3);
                var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
                document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);
            }
            else {
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
                    document.getElementById('txt_qtyltrs').value = qtykgsltr;
                    //  $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);
                }
                else {
                    var qtykg = 0;
                    qtykg = document.getElementById('txt_qtykgs').value;
                    qtykg = parseFloat(qtykg).toFixed(3);
                    var qtykgsltr = 0;
                    var clr = 0;
                    clr = parseFloat(qtyid.value).toFixed(3);
                    var modclr = (clr / 1000) + 1;
                    modclr = parseFloat(modclr).toFixed(5);
                    qtykgsltr = qtykg / modclr;
                    qtykgsltr = parseFloat(qtykgsltr).toFixed(0);
                    document.getElementById('txt_qtyltrs').value = qtykgsltr;

                    ////////For SNF ////////////////

                    var fat = 0;
                    fat = document.getElementById('txt_fat').value;
                    fat = parseFloat(fat).toFixed(3);
                    var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
                    document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);
                }
            }
        }
        function forintraclearall() {
            document.getElementById('slct_tsilo').selectedIndex = 0;
            document.getElementById('slct_fsilo').selectedIndex = 0;
            document.getElementById('slct_purpose').selectedIndex = 0;
            document.getElementById('slct_batchtype').selectedIndex = 0;
            document.getElementById('txt_istdate').value = "";
            document.getElementById('txt_qtykgs').value = "";
            document.getElementById('txt_qtyltrs').value = "";
            document.getElementById('txt_fat').value = "";
            document.getElementById('txt_snf').value = "";
            document.getElementById('txt_clr').value = "";
            document.getElementById('txt_costltrs').value = "";
            get_intrasilo_transaction_details();
            $("#lbl_fsilo_error_msg").hide();
            $("#lbl_tsilo_error_msg").hide();
            $("#lbl_Quantitykgs_error_msg").hide();
            $("#lbl_Quantityltrs_error_msg").hide();
            $("#lbl_fat_error_msg").hide();
            $("#lbl_snf_error_msg").hide();
            $("#lbl_costltrs_error_msg").hide();
        }

        function get_intraSilos() {
            var data = { 'op': 'get_Silo_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillintrafSilos(msg);

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
        function fillintrafSilos(msg) {
            var data = document.getElementById('slct_fsilo');
            var length = data.options.length;
            document.getElementById('slct_fsilo').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Silo Name";
            opt.value = "Select Silo Name";
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
        function get_toSilos() {
            var data = { 'op': 'get_Silo_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filltSilos(msg)
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
        function filltSilos(msg) {
            var fromsilovalue = document.getElementById('slct_fsilo').value;
            var data = document.getElementById('slct_tsilo');
            var length = data.options.length;
            document.getElementById('slct_tsilo').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Silo Name";
            opt.value = "Select Silo Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].SiloId != fromsilovalue) {
                    if (msg[i].SiloName != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].SiloName;
                        option.value = msg[i].SiloId;
                        data.appendChild(option);
                    }
                }

            }
        }

        function get_intratransactions_details() {
            var data = { 'op': 'get_intratransactions_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function save_intrasilo_milktransactions_click() {
            var istdate = document.getElementById('txt_istdate').value;
            if (istdate == "") {
                alert("Please Select Date");
                $("#txt_istdate").focus();
                return false;
            }

            var FromSiloId = document.getElementById('slct_fsilo').value;
            if (FromSiloId == "" || FromSiloId == "Select Silo Name") {
                alert("Please Select From Silo");
                $("#slct_fsilo").focus();
                return false;
            }
            var ToSiloId = document.getElementById('slct_tsilo').value;
            if (ToSiloId == "" || ToSiloId == "Select Silo Name") {
                alert("Please Select To Silo");
                $("#ToSiloId").focus();
                return false;
            }
            var Quantityltrs = document.getElementById('txt_qtyltrs').value;
            if (Quantityltrs == "") {
                alert("Please Enter Quantity ltrs");
                $("#txt_qtyltrs").focus();
                return false;
            }
            var Quantitykgs = document.getElementById('txt_qtykgs').value;
            if (Quantitykgs == "") {
                alert("Please Enter Quantity kgs");
                $("#txt_qtykgs").focus();
                return false;
            }
            var fat = document.getElementById('txt_fat').value;
            if (fat == "") {
                alert("Please Enter fat");
                $("#txt_fat").focus();
                return false;
            }
            var clr = document.getElementById('txt_clr').value;
            if (clr == "") {
                alert("Please Enter clr");
                $("#txt_clr").focus();
                return false;
            }
            var snf = document.getElementById('txt_snf').value;
            if (snf == "") {
                alert("Please Enter snf");
                $("#txt_snf").focus();
                return false;
            }
            var costltrs = document.getElementById('txt_costltrs').value;
            var purposetype = document.getElementById('slct_purpose').value;
            if (purposetype == "" || purposetype == "Select Purpose Type") {
                alert("Please Select Purpose");
                $("#slct_purpose").focus();
                return false;
            }
            var batchid = "";
            if (purposetype == "Btach Preparation") {
                batchid = document.getElementById('slct_batchtype').value;
                if (batchid == "" || batchid == "Select Batch") {
                    alert("Please Select Batch");
                    $("#slct_batchtype").focus();
                    return false;
                }
            }
            var smp = document.getElementById('txt_insmp').value;
            var sno = document.getElementById('lbl_intrasno').innerHTML;
            var btnval = document.getElementById('save_silotransactions').innerHTML;
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'for_save_Silointratransaction', 'istdate': istdate, 'FromSiloId': FromSiloId, 'ToSiloId': ToSiloId, 'Quantitykgs': Quantitykgs, 'Quantityltrs': Quantityltrs, 'fat': fat, 'snf': snf,
                    'clr': clr, 'costltrs': costltrs, 'purposetype': purposetype, 'batchid': batchid, 'smp': smp, 'sno': sno, 'btnval': btnval
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert("Successfully Transfer Silo quantity");
                            forintraclearall();
                            get_intrasilo_transaction_details();
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
        function get_intrasilo_transaction_details() {
            var data = { 'op': 'get_intrasilo_transaction_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillintatrans(msg);
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
        function fillintatrans(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;">From Silo</th><th scope="col" style="font-weight: bold;">To Silo</th><th scope="col" style="font-weight: bold;">Qty(ltrs)</th><th scope="col" style="font-weight: bold;">Qty(kgs)</th><th scope="col" style="font-weight: bold;">FAT</th><th scope="col" style="font-weight: bold;">SNF</th><th scope="col" style="font-weight: bold;">CLR</th><th scope="col" style="font-weight: bold;">SMP</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td  class="1" style="display:none;">' + msg[i].sno + '</td>';
                results += '<td  class="2" >' + msg[i].doe + '</td>';
                results += '<td  class="3" style="display:none;">' + msg[i].fromsiloid + '</td>';
                results += '<td  class="4" style="display:none;">' + msg[i].tosiloid + '</td>';
                results += '<td  class="5">' + msg[i].fromsilo + '</td>';
                results += '<td  class="6">' + msg[i].tosilo + '</td>';
                results += '<td  class="7">' + msg[i].qty_ltrs + '</td>';

                results += '<td  class="8" >' + msg[i].qty_kgs + '</td>';
                results += '<td  class="9" style="display:none;">' + msg[i].costperltr + '</td>';
                results += '<td  class="10">' + msg[i].fat + '</td>';
                results += '<td  class="11">' + msg[i].snf + '</td>';
                results += '<td  class="12" >' + msg[i].clr + '</td>';
                results += '<td  class="13">' + msg[i].smp + '</td>';
                results += '<td  class="14" style="display:none;">' + msg[i].datetime + '</td>';

                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" onclick="getmeintra(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_intrasiloget").html(results);
        }
        function getmeintra(thisid) {
            scrollTo(0, 0);
            var sno = $(thisid).parent().parent().children('.1').html();
            var doe = $(thisid).parent().parent().children('.2').html();
            var fromsiloid = $(thisid).parent().parent().children('.3').html();
            var tosiloid = $(thisid).parent().parent().children('.4').html();
            var fromsilo = $(thisid).parent().parent().children('.5').html();
            var tosilo = $(thisid).parent().parent().children('.6').html();
            var qty_ltrs = $(thisid).parent().parent().children('.7').html();
            var qty_kgs = $(thisid).parent().parent().children('.8').html();
            var costperltr = $(thisid).parent().parent().children('.9').html();
            var fat = $(thisid).parent().parent().children('.10').html();
            var snf = $(thisid).parent().parent().children('.11').html();
            var clr = $(thisid).parent().parent().children('.12').html();
            var smp = $(thisid).parent().parent().children('.13').html();
            var datetime = $(thisid).parent().parent().children('.14').html();

            document.getElementById('txt_istdate').value = datetime;
            document.getElementById('slct_fsilo').value = fromsiloid;
            document.getElementById('slct_tsilo').value = tosiloid;
            document.getElementById('slct_purpose').value = "Btach Preparation";
            ddlpurposechange();
            document.getElementById('txt_qtykgs').value = qty_kgs;
            document.getElementById('txt_qtyltrs').value = qty_ltrs;
            document.getElementById('txt_costltrs').value = costperltr;
            document.getElementById('txt_fat').value = fat;
            document.getElementById('txt_snf').value = snf;
            document.getElementById('txt_clr').value = clr;
            document.getElementById('txt_insmp').value = smp;
            document.getElementById('lbl_intrasno').innerHTML = sno;
            document.getElementById('save_silotransactions').innerHTML = "Modify";
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
        function LtrsChange(qtyid) {
            qtyltr = parseFloat(qtyid.value).toFixed(3);
            var clr = document.getElementById('txt_clr').value;
            if (clr == "") {
                clr = 30;
            }
            clr = parseFloat(clr).toFixed(3);
            var qtyltrkgs = 0;
            var modclr = (clr / 1000) + 1;
            modclr = parseFloat(modclr).toFixed(5);
            qtyltrkgs = qtyltr * modclr;
            qtyltrkgs = parseFloat(qtyltrkgs).toFixed(0);
            document.getElementById('txt_qtykgs').value = qtyltrkgs;
        }
        var silocapacity;
        function checkcapacity() {
            var siloid = document.getElementById('slct_fsilo').value;
            var data = { 'op': 'get_Silo_capacity', 'siloid': siloid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcapacity(msg);
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
        function fillcapacity(msg) {
            var btnval = document.getElementById('save_silotransactions').innerHTML;
            if (btnval == "Save") {
                for (var i = 0; i < msg.length; i++) {
                    silocapacity = msg[i].Capacity;
                    var qtycltrs = parseFloat(document.getElementById('txt_qtyltrs').value);
                    var caty = parseFloat(silocapacity);
                    if (qtycltrs > caty) {
                        alert("Please enter less than the from silo Quantity.");
                        document.getElementById('txt_qtyltrs').value = "";
                        document.getElementById('txt_qtykgs').value = "";
                        $("#txt_qtyltrs").focus();
                    }
                }
            }
        }
        function ddlfromsilo() {
            get_toSilos();
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
            var data = document.getElementById('slct_batchtype');
            var length = data.options.length;
            document.getElementById('slct_batchtype').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Batch";
            opt.value = "Select Batch";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].departmentid == "5" || msg[i].departmentid == "11") {
                    if (msg[i].batchtype != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].batchtype;
                        option.value = msg[i].batchid;
                        data.appendChild(option);
                    }
                }
            }
        }
        function ddlpurposechange() {
            var purposetype = document.getElementById('slct_purpose').value;
            if (purposetype == "Btach Preparation") {
                $("#tdpurposetype").show();
                get_Batchs();
            }
            else {
                $("#tdpurposetype").hide();
                document.getElementById('slct_batchtype').value = "";
            }
        }
        //end intra silo//

        // out ward silo//
        function outClearvalues() {
            //document.getElementById('txt_date').value = "";
            document.getElementById('slct_Department').selectedIndex = 0;
            document.getElementById('slct_Source_Name').selectedIndex = 0;
            document.getElementById('txt_outqtykgs').value = "";
            document.getElementById('txt_outqtyltrs').value = "";
            document.getElementById('save_Outwardtransactions').innerHTML = "Save";
            document.getElementById('txt_outfat').value = "";
            document.getElementById('txt_outsnf').value = "";
            document.getElementById('txt_outclr').value = "";
            transsno = 0;
            //  document.getElementById('txt_fat').value = "";
            //  document.getElementById('txt_snf').value = "";
            // $("#lbl_date").hide();
            $("#lbl_department").hide();
            $("#lbl_silo").hide();
            // $("#lbl_clr").hide();


        }

        function outLtrsChange(qtyid) {
            var qtyltr = 0;
            qtyltr = parseFloat(qtyid.value).toFixed(3);
            var clr = document.getElementById('txt_outclr').value;
            if (clr == "") {
                clr = 30;
            }
            clr = parseFloat(clr).toFixed(3);
            var qtyltrkgs = 0;
            var modclr = (clr / 1000) + 1;
            modclr = parseFloat(modclr).toFixed(5);
            qtyltrkgs = qtyltr * modclr;
            qtyltrkgs = parseFloat(qtyltrkgs).toFixed(0);
            document.getElementById('txt_outqtykgs').value = qtyltrkgs;
        }

        function fatChange(qtyid) {
            var fat = 0;
            var clr = 0;
            var department = document.getElementById('slct_Department').value;
            if (department == "3" || department == "10") {
                fat = document.getElementById('txt_outfat').value;
                fat = parseFloat(fat).toFixed(3);
                var snfvalue = (100 - fat) / 11;
                document.getElementById('txt_outsnf').value = parseFloat(snfvalue).toFixed(2);
            }
        }
        function outLRChange(qtyid) {
            if (qtyid.value == "") {
                var qty = 0;
                var qtykg = 0;
                qtykg = document.getElementById('txt_outqtykgs').value;
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
                    document.getElementById('txt_outqtyltrs').value = qtykgsltr;
                }
                //  $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);
            }
            else {
                var qtykg = 0;
                qtykg = document.getElementById('txt_outqtykgs').value;
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
                        document.getElementById('txt_outqtyltrs').value = qtykgsltr;
                    }
                }
                else {
                    var qtyltr = document.getElementById('txt_outqtyltrs').value;
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
                        document.getElementById('txt_outqtykgs').value = qtyltrkgs;
                    }
                }

                ////////For SNF ////////////////
                var department = document.getElementById('slct_Department').value;
                if (department == "3" || department == "5") {
                    fat = document.getElementById('txt_outfat').value;
                    fat = parseFloat(fat).toFixed(3);
                    var snfvalue = (100 - fat) / 11;
                    document.getElementById('txt_outsnf').value = parseFloat(snfvalue).toFixed(2);
                }
                else {
                    var fat = 0;
                    fat = document.getElementById('txt_outfat').value;
                    fat = parseFloat(fat).toFixed(3);
                    var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
                    document.getElementById('txt_outsnf').value = parseFloat(snfvalue).toFixed(2);
                }
            }
        }
        function get_outSilos() {
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

        function out_checkquantity() {
            var siloid = document.getElementById('slct_Source_Name').value;
            var data = { 'op': 'silo_Quantitycheck_transaction', 'siloid': siloid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        checkdetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function checkdetails(msg) {
            var btnval = document.getElementById('save_Outwardtransactions').innerHTML;
            if (btnval == "Save") {
                var siloquantity = "";
                var Qtykgs = document.getElementById('txt_outqtyltrs').value;
                for (var i = 0; i < msg.length; i++) {
                    siloquantity = msg[i].OutwordQuantitykgs;
                }
                if (parseFloat(Qtykgs) > parseFloat(siloquantity)) {
                    alert("Please enter less than the from silo Quantity.");
                    document.getElementById('txt_outqtykgs').value = "";
                    document.getElementById('txt_outqtyltrs').value = "";
                    $("#txt_outqtyltrs").focus();
                }
                else {

                }
            }
        }

        function get_outBatchs() {
            var data = { 'op': 'get_batch_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filloutBatchs(msg);
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
        function filloutBatchs(msg) {
            var data = document.getElementById('slct_product');
            var length = data.options.length;
            document.getElementById('slct_product').options.length = null;
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

        function get_outward_silo_transaction() {
            var getdatadate = document.getElementById('txt_getdatadate').value;
            var data = { 'op': 'get_outward_silo_transaction', 'getdatadate': getdatadate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillosilodetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillosilodetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Silo Name</th><th scope="col" style="font-weight: bold;">Dept Name</th><th scope="col" style="font-weight: bold;">Qty(kgs)</th><th scope="col" style="font-weight: bold;">Qty(Ltrs)</th><th scope="col" style="font-weight: bold;">FAT</th><th scope="col" style="font-weight: bold;">SNF</th><th scope="col" style="font-weight: bold;">CLR</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="outgetme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1" style="display:none;">' + msg[i].branchname + '</td>';
                results += '<td data-title="Code" class="2">' + msg[i].SiloName + '</td>';
                results += '<td data-title="Code" class="3">' + msg[i].Deportment + '</td>';
                results += '<td data-title="Code" class="4">' + msg[i].OutwordQuantitykgs + '</td>';
                results += '<td data-title="Code"  class="8">' + msg[i].OutwordQuantityltrs + '</td>';
                results += '<td data-title="Code"  class="9">' + msg[i].fat + '</td>';
                results += '<td data-title="Code"  class="10">' + msg[i].snf + '</td>';
                results += '<td data-title="Code" class="11">' + msg[i].clr + '</td>';
                results += '<td data-title="Code" class="12">' + msg[i].doe + '</td>';
                results += '<td data-title="Code" style="display:none;" class="5">' + msg[i].departmentid + '</td>';
                results += '<td data-title="Code" style="display:none;" class="22">' + msg[i].productid + '</td>';
                results += '<td data-title="Code" style="display:none;" class="7">' + msg[i].SiloId + '</td>';
                results += '<td data-title="Code" style="display:none;" class="20">' + msg[i].datetime + '</td>';
                results += '<td data-title="Code" style="display:none;" class="6">' + msg[i].transno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="outgetme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_outdata").html(results);
        }
        var transsno = 0;
        function outgetme(thisid) {
            transsno = $(thisid).parent().parent().children('.6').html();
            var branchname = $(thisid).parent().parent().children('.1').html();
            var SiloName = $(thisid).parent().parent().children('.2').html();
            var Deportment = $(thisid).parent().parent().children('.3').html();
            var OutwordQuantitykgs = $(thisid).parent().parent().children('.4').html();
            var departmentid = $(thisid).parent().parent().children('.5').html();
            var SiloId = $(thisid).parent().parent().children('.7').html();
            var OutwordQuantityltrs = $(thisid).parent().parent().children('.8').html();
            var fat = $(thisid).parent().parent().children('.9').html();
            var snf = $(thisid).parent().parent().children('.10').html();
            var clr = $(thisid).parent().parent().children('.11').html();
            var date = $(thisid).parent().parent().children('.20').html();
            var productid = $(thisid).parent().parent().children('.22').html();

            document.getElementById('txt_date').value = date;
            document.getElementById('slct_Source_Name').value = SiloId;
            document.getElementById('slct_Department').value = departmentid;
            document.getElementById('txt_outqtykgs').value = OutwordQuantitykgs;
            document.getElementById('txt_outqtyltrs').value = OutwordQuantityltrs;
            document.getElementById('txt_outfat').value = fat;
            document.getElementById('txt_outsnf').value = snf;
            document.getElementById('txt_outclr').value = clr;
            document.getElementById('slct_product').value = productid;
            document.getElementById('save_Outwardtransactions').innerHTML = "Modify";
            $('#outwardsilo_fillform').css('display', 'block');
            $('#outshowlogs').css('display', 'none');
            $('#div_outdata').hide();
        }
        function save_outword_silotransaction_click() {
            var date = document.getElementById('txt_date').value;
            if (date == "") {
                alert("Please Select date");
                $("#txt_date").focus();
                return false;
            }
            var Siloname = document.getElementById('slct_Source_Name').value;
            if (Siloname == "" || Siloname == "Select Silo") {
                alert("Please Select Silo Name");
                $("#slct_Source_Name").focus();
                return false;
            }
            var Department = document.getElementById('slct_Department').value;
            if (Department == "" || Department == "Select Department") {
                alert("Please select Department");
                $("#slct_Department").focus();
                return false;
            }
            var product = document.getElementById('slct_product').value;
            var Qtykgs = document.getElementById('txt_outqtykgs').value;

            var Qtyltrs = document.getElementById('txt_outqtyltrs').value;
            if (Qtyltrs == "") {
                alert("Please Enter Qty ltrs");
                $("#txt_outqtyltrs").focus();
                return false;
            }
            var fat = document.getElementById('txt_outfat').value;
            var snf = document.getElementById('txt_outsnf').value;
            var clr = document.getElementById('txt_outclr').value;
            var btnval = document.getElementById('save_Outwardtransactions').innerHTML;
            var obcream = document.getElementById('txt_obcream').value;
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_outword_silo_transaction_click', 'Department': Department, 'product': product, 'Siloname': Siloname, 'Qtykgs': Qtykgs, 'Qtyltrs': Qtyltrs,
                    'btnval': btnval, 'transsno': transsno, 'fat': fat, 'snf': snf, 'clr': clr, 'date': date, 'obcream': obcream
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_outward_silo_transaction();
                            outClearvalues();
                            $('#div_outdata').show();
                            $('#outwardsilo_fillform').css('display', 'none');
                            $('#outshowlogs').css('display', 'block');
                            $('#td_opbalance').hide();
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
        function change_creamdetails() {
            var deptname = document.getElementById('slct_Department').value;
            var batchname = document.getElementById('slct_product').value;
            if (deptname == "3" || deptname == "10") {
                $('#td_opbalance').show();
                var productid = "1217";
                var data = { 'op': 'get_productqty_details', 'productid': productid };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            for (var i = 0; i < msg.length; i++) {
                                document.getElementById('txt_obcream').value = msg[i].quantity;
                            }
                        }
                        else {
                            document.getElementById('txt_obcream').value = "0";
                        }
                    }
                    else {
                        document.getElementById('txt_obcream').value = "0";
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
            else {
                $('#td_opbalance').hide();
            }
        }
        function change_batchdetails() {
            var deptname = document.getElementById('slct_Department').value;
            var batchname = document.getElementById('slct_product').value;
            if (deptname == "4") {
                if (batchname == "12" || batchname == "38" || batchname == "39") {
                    $('#td_opbalance').show();
                    var productid = "1217";
                    var data = { 'op': 'get_productqty_details', 'productid': productid };
                    var s = function (msg) {
                        if (msg) {
                            if (msg.length > 0) {
                                for (var i = 0; i < msg.length; i++) {
                                    document.getElementById('txt_obcream').value = msg[i].quantity;
                                }
                            }
                            else {
                                document.getElementById('txt_obcream').value = "0";
                            }
                        }
                        else {
                            document.getElementById('txt_obcream').value = "0";
                        }
                    };
                    var e = function (x, h, e) {
                    };
                    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                    callHandler(data, s, e);
                }
                else {
                    $('#td_opbalance').hide();
                }
            }
        }
        //        function checkoutwardsilocapacity() {
        //            var siloid = document.getElementById('slct_Source_Name').value;
        //            var data = { 'op': 'get_Silo_capacity', 'siloid': siloid };
        //            var s = function (msg) {
        //                if (msg) {
        //                    if (msg.length > 0) {
        //                        fillcapacitys(msg);
        //                    }
        //                    else {
        //                    }
        //                }
        //                else {
        //                }
        //            };
        //            var e = function (x, h, e) {
        //            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        //            callHandler(data, s, e);
        //        }
        //        function fillcapacitys(msg) {
        //            for (var i = 0; i < msg.length; i++) {
        //                silocapacity = msg[i].Capacity;
        //                var qtycltrs = parseInt(document.getElementById('txt_outqtyltrs').value);
        //                var caty = parseInt(silocapacity);
        //                if (qtycltrs > caty) {
        //                    alert("Entry quantity is more than the silo capacity");
        //                    document.getElementById('txt_outqtyltrs').value = "";
        //                    document.getElementById('txt_outqtykgs').value = "";
        //                    return;
        //                }
        //            }
        //        }
        //end out ward silo//
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            <i class="fa fa-television" aria-hidden="true"></i>SILO Operations<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">SILO Operations</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showmonitor()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Silo Monitor Details</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showintra()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Intra Silo Transaction</a></li>
                        <li id="Li1" class=""><a data-toggle="tab" href="#" onclick="showoutward()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Outward Silo</a></li>
                </ul>
            </div>
            <div id="divsilomonitor">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i> SILO Monitor Details
                        </h3>
                    </div>
                    <div class="box-body">
                        <%--<div id="showlogs" align="center">
                            <input id="btn_addDept" type="button" name="submit" value='Silo Monitor Quantity'
                                class="btn btn-success" />
                        </div>--%>
                        <div id="div_Deptdata">
                        </div>
                        <%--<div id='fillform' style="display: none;">
                            <table cellpadding="1px" align="center" style="width: 60%;">
                                <tr>
                                    <th colspan="2" align="center">
                                    </th>
                                </tr>
                                <tr>
                                    <td style="height: 40px;">
                                        <label>
                                            Silo Name</label>
                                        <span style="color: red;">*</span>
                                    </td>
                                    <td>
                                        <select id="slct_Silo_Name" class="form-control">
                                            <option selected disabled value="Select Silo Name">Select Silo Name</option>
                                        </select>
                                        <label id="lbl_SiloName_error_msg" class="errormessage">
                                            * Please Enter Siloname</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 40px;">
                                        <label>
                                            Quantity
                                        </label>
                                        <span style="color: red;">*</span>
                                    </td>
                                    <td>
                                        <input type="text" maxlength="45" id="txt_Quantity" class="form-control" name="vendorcode"
                                            placeholder="Enter Quantity"><label id="lbl_Quantity_error_msg" class="errormessage">*
                                                Please Enter Quantity</label>
                                    </td>
                                </tr>
                                <tr hidden>
                                    <td>
                                        <label id="lbl_sno">
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" style="height: 40px;">
                                        <input type="button" name="submit" class="btn btn-success" id="btn_save" value='Save'
                                            onclick="for_save_SiloMonitor()" />
                                        <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close' />
                                    </td>
                                </tr>
                            </table>
                        </div>--%>
                    </div>
                </div>
            </div>

            <div id="divintrasilo">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Intra SILO Transaction Details
                </h3>
            </div>
            <div class="box-body">
                <div id='Inwardsilo_fillform' style="padding-left: 325px;">
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                   Date</label>
                                <input id="txt_istdate" type="datetime-local" class="form-control">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    From SILO<span style="color: red;">*</span></label>
                                <select id="slct_fsilo" class="form-control" onchange="ddlfromsilo();">
                                    <option selected disabled value="Select SILO No">Select SILO Name</option>
                                </select>
                                <label id="lbl_fsilo_error_msg" class="errormessage">
                                    * Please Enter From Silo</label>
                            </td>
                            <td style="width:3px;"></td>
                            <td>
                                <label>
                                    To SILO<span style="color: red;">*</span></label>
                                <select id="slct_tsilo" class="form-control">
                                    <option selected disabled value="Select SILO No">Select SILO Name</option>
                                </select>
                                <label id="lbl_tsilo_error_msg" class="errormessage">
                                    * Please Enter To Silo</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Purpose<span style="color: red;">*</span></label>
                                <select id="slct_purpose" class="form-control" onchange="ddlpurposechange();">
                                    <option value="Select Purpose Type">Select Purpose Type</option>
                                    <option value="Btach Preparation">Batch Preparation</option>
                                    <option value="Other">Other</option>
                                </select>
                                <label id="Label1" class="errormessage">
                                    * Please Enter Purpose</label>
                            </td>
                            <td style="width:3px;"></td>
                            <td id="tdpurposetype">
                                <label>
                                    Batch Type<span style="color: red;">*</span></label>
                                <select id="slct_batchtype" class="form-control">
                                    <option selected disabled value="Select SILO No">Select Purpose Type</option>
                                </select>
                                <label id="Label2" class="errormessage">
                                    * Please Enter To Silo</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Qty(Kgs)</label>
                                <input id="txt_qtykgs" type="text" class="form-control" name="vendorcode" placeholder="Enter Qty in Kgs" readonly>
                                <label id="lbl_Quantitykgs_error_msg" class="errormessage">
                                    * Please Enter Quantity(kgs)</label>
                            </td>
                            <td style="width:3px;"></td>
                            <td>
                                <label>
                                    Qty(ltrs)<span style="color: red;">*</span></label>
                                <input id="txt_qtyltrs" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Qty in ltrs" onchange="LtrsChange(this);checkcapacity();">
                                <label id="lbl_Quantityltrs_error_msg" class="errormessage">
                                    * Please Enter Quantity(ltrs)</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Cost per(ltrs)<span style="color: red;">*</span></label>
                                <input id="txt_costltrs" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter ltrs cost">
                                <label id="lbl_costltrs_error_msg" class="errormessage">
                                    * Please Enter cost per(ltrs)</label>
                            </td>
                            <td style="width:3px;"></td>
                            <td>
                                <label>
                                    FAT<span style="color: red;">*</span></label>
                                <input id="txt_fat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="FAT">
                                <label id="lbl_fat_error_msg" class="errormessage">
                                    * Please Enter fat</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    SNF</label>
                                <input id="txt_snf" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="SNF" readonly />
                                <label id="lbl_snf_error_msg" class="errormessage">
                                    * Please Enter snf</label>
                            </td>
                            <td style="width:3px;"></td>
                            <td>
                                <label>
                                    CLR<span style="color: red;">*</span></label>
                                <input id="txt_clr" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="CLR"
                                    onkeyup="LRChange(this)" />
                                <label id="lbl_clr" class="errormessage">
                                    * Please Enter CLR</label>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <label>
                                    SMP</label>
                                <input id="txt_insmp" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="SMP" />
                                <label id="Label4" class="errormessage">
                                    * Please Enter SMP</label>
                            </td>
                        </tr>
                           <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <div  style="padding-left: 10%;padding-top: 0%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_silotransactions1" onclick="save_intrasilo_milktransactions_click()"></span><span id="save_silotransactions" onclick="save_intrasilo_milktransactions_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='close_vehmaster1' onclick="forintraclearall()"></span><span id='close_vehmaster' onclick="forintraclearall()">Clear</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                </div>
                <div>
                    <label id="lbl_intrasno" style="display:none;">
                    </label>
                </div>
                <br />
            </div>
            <div id="div_intrasiloget" style="padding: 2%;">
            </div>
        </div>
            </div>
            <div id="divoutwardsilo">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Outward SILO
                </h3>
            </div>
            <div class="box-body">
                <div id="outshowlogs">
                    <table>
                    <tr>
                    <td>
                          </td>
                        <td>
                            <label>
                                Date</label>
                        </td>
                        <td style="width: 5px;">
                          </td>
                        <td>
                            <input id="txt_getdatadate" class="form-control" type="date" name="vendorcode" />
                        </td>
                         <td style="width: 5px;">
                          </td>
                        <td>
                        <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-refresh" id="txt_getgenerate1" onclick="get_outward_silo_transaction()"></span><span id="txt_getgenerate" onclick="get_outward_silo_transaction()">Generate</span>
                            </div>
                            </div>
                        </td>
                        <td style="padding-left: 621px;">
                            <td>
                            </td>
                            <td>

                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addoutward()"></span> <span onclick="addoutward()">Add OutWard SILO</span>
                          </div>
                          </div>
                            </td>
                    </tr>
                </table>
                </div>
                <div id="div_outdata">
                </div>
                <div id='outwardsilo_fillform' style="display: none; padding-left:250px;">
                    <table align="center">
                    <tr>
                    <td colspan="2"><label>
                                    Date <span style="color: red;">*</span></label>
                                 
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                        </td>  
                    </tr>
                        <tr>
                            <td>
                                <label>
                                    SILO Name<span style="color: red;">*</span></label>
                                <select id="slct_Source_Name" class="form-control">
                                    <option selected disabled value="Select SILO Name">Select SILO Name</option>
                                </select>
                                <label id="lbl_silo" class="errormessage">
                                    * Please select Silo</label>
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                    Department<span style="color: red;">*</span></label>
                                <select id="slct_Department" class="form-control" onchange="change_creamdetails();">
                                    <option selected disabled value="Select Department">Select Department</option>
                                </select>
                                <label id="lbl_department" class="errormessage">
                                    * Please select Department</label>
                            </td>
                             <td style="width: 3px;">
                             </td>
                            <td style="display:none;" id="td_opbalance">
                                <label>
                                    O/B Cream</label>
                                <input id="txt_obcream" type="text" class="form-control" readonly>
                            </td>
                        </tr>
                        <tr>
                        <td>
                            <label>
                                    Batch<span style="color: red;">*</span></label>
                                <select id="slct_product" class="form-control"  onchange="change_batchdetails()">
                                    <option selected disabled value="Select Product">Select Batch</option>
                                </select>
                                <label id="lbl_product" class="errormessage">
                                    * Please select Batch</label>
                        </td>
                         <td style="width: 3px;">
                            <td>
                                <label>
                                    Qty(Kgs)</label>
                                <input id="txt_outqtykgs" type="text" class="form-control" name="vendorcode" readonly
                                    placeholder="Enter Qty in Kgs" >
                                <label id="lbl_qty" class="errormessage">
                                    * Required Quantity</label>
                            </td>
                        </tr>
                        <tr>
                         <td>
                                <label>
                                    Qty(ltrs)<span style="color: red;">*</span></label>
                                <input id="txt_outqtyltrs" type="text" class="form-control" name="vendorcode" onchange="outLtrsChange(this);out_checkquantity();" placeholder="Enter Qty in ltrs">
                            </td>
                              <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                    FAT<span style="color: red;">*</span></label>
                                <input id="txt_outfat" type="text" class="form-control" name="vendorcode" placeholder="FAT" onkeyup="fatChange(this)">
                                <label id="lbl_fat" class="errormessage">
                                    * Please enter fat</label>
                            </td>
                          
                            
                        </tr>
                        <tr>
                        <td>
                                <label>
                                    SNF</label>
                                <input id="txt_outsnf" type="text" class="form-control" name="vendorcode" placeholder="SNF" readonly/>
                                <label id="lbl_snf" class="errormessage">
                                    * Please enter snf</label>
                            </td>
                              <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                    CLR<span style="color: red;">*</span></label>
                                <input id="txt_outclr" type="text" class="form-control" name="vendorcode" placeholder="CLR"
                                    onkeyup="outLRChange(this)" />
                                <label id="Label3" class="errormessage">
                                    * Please enter CLR</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <div  style="padding-left: 10%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_Outwardtransactions1" onclick="save_outword_silotransaction_click()"></span><span id="save_Outwardtransactions" onclick="save_outword_silotransaction_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_outclose1' onclick="clearoutwarddet()"></span><span id='btn_outclose' onclick="clearoutwarddet()">Close</span>
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
