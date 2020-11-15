<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="closingdetails.aspx.cs" Inherits="closingdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <style type="text/css">
        input[type=text]::-webkit-inner-spin-button, input[type=text]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
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
            $('#txt_batchdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#txt_swdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#div_shiftwiseclose').css('display', 'block');
            $('#div_batchwiseclose').css('display', 'none');
            $('#div_silowiseclosing').css('display', 'none');
            $('#div_departmentwiseclose').css('display', 'none');
            $('#div_butterclosingdetails').css('display', 'none');
            get_Silos();
            get_Shift_details();
            $('#btn_addDept').click(function () {
                $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
                get_SiloDepartments();
            });
            $('#btn_close').click(function () {
                $('#Inwardsilo_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                Clearvalues();
            });
        });
        function adddepartment() {
            $('#Inwardsilo_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            get_SiloDepartments();
        }
        function showshiftwiseclosing() {
            $('#div_shiftwiseclose').css('display', 'block');
            $('#div_batchwiseclose').css('display', 'none');
            $('#div_silowiseclosing').css('display', 'none');
            $('#div_departmentwiseclose').css('display', 'none');
            $('#div_butterclosingdetails').css('display', 'none');
            get_Silos();
            get_Shift_details();
            btn_cancel_click_shift();
        }
        function showbatchwiseclosing() {
            $('#div_shiftwiseclose').css('display', 'none');
            $('#div_batchwiseclose').css('display', 'block');
            $('#div_silowiseclosing').css('display', 'none');
            $('#div_departmentwiseclose').css('display', 'none');
            $('#div_butterclosingdetails').css('display', 'none');
            get_batchs();
            get_Silosbatch();
            btn_cancel_click_batch();
        }
        //        function showsilowiseclosing() {
        //            $('#div_shiftwiseclose').css('display', 'none');
        //            $('#div_batchwiseclose').css('display', 'none');
        //            $('#div_silowiseclosing').css('display', 'block');
        //                    $('#div_departmentwiseclose').css('display', 'none');
        //        $('#div_butterclosingdetails').css('display', 'none');
        //            get_Silos_silo();
        //            btn_cancel_click_silo();
        //        }
        function showdepartmentwiseclosing() {
            $('#div_shiftwiseclose').css('display', 'none');
            $('#div_batchwiseclose').css('display', 'none');
            $('#div_silowiseclosing').css('display', 'none');
            $('#div_departmentwiseclose').css('display', 'block');
            $('#div_butterclosingdetails').css('display', 'none');
            get_department_wise_qty_details();
        }
        function showbutterclosingdetails() {
            $('#div_shiftwiseclose').css('display', 'none');
            $('#div_batchwiseclose').css('display', 'none');
            $('#div_silowiseclosing').css('display', 'none');
            $('#div_departmentwiseclose').css('display', 'none');
            $('#div_butterclosingdetails').css('display', 'block');
            get_butterproduction_details();
            Clearvaluesbutterclosing();
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
        function get_Silos() {
            var data = { 'op': 'get_Silomonitor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Silo Name</th><th scope="col">Opening Quaninty</th><th scope="col">Fat</th><th scope="col">Snf</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr>';
                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                            results += '<th><span id="txt_SiloName" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].SiloName + '</span></th>';
                            results += '<td><input id="txt_qtykgs" class="form-control" value="' + msg[i].Quantity + '" type="text" name="vendorcode" onkeypress="return isFloat(event);" placeholder="Enter qty kgs"></td>';
                            results += '<td  style="display:none"><input id="txt_qtyltrs" value="0" class="form-control" type="text" name="vendorcode"placeholder="Enter qty " onkeypress="return isFloat(event);"></td>';
                            results += '<td><input id="txt_fat" class="form-control" type="text" name="vendorcode"placeholder="Enter fat" onkeypress="return isFloat(event);"></td>';
                            results += '<td><input id="txt_snf" class="form-control" type="text" name="vendorcode"placeholder="Enter snf" onkeypress="return isFloat(event);"></td>';
                            results += '<td style="display:none" class="8"><input id="hdn_siloid" class="form-control" type="text" name="vendorcode" value="' + msg[i].SiloId + '"></td></tr>';
                        }
                        results += '</table></div>';
                        $("#divFillScreen").html(results);
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
        function btn_cancel_click_shift() {
            document.getElementById("slct_shift").selectedIndex = 0;
            document.getElementById("txt_qtyltrs").value = "";
            document.getElementById("txt_fat").value = "";
            document.getElementById("txt_snf").value = "";
            document.getElementById("txt_swdate").value = "";
            get_Silos();
        }
        function btnsave_shiftclosing_click() {
            var date = document.getElementById('txt_swdate').value;
            if (date == "") {
                alert("Please select Date");
                $('#txt_swdate').focus();
                return false;
            }
            var shiftid = document.getElementById('slct_shift').value;
            if (shiftid == "Select Shift type") {
                alert("Please select Shift type");
                return false;
            }
            var rows = $("#table_shift_wise_details tr:gt(0)");
            var silo_shift_wise_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_qtykgs').val() != "") {
                    silo_shift_wise_details.push({ siloid: $(this).find('#hdn_siloid').val(), shiftid: shiftid, qty_kgs: $(this).find('#txt_qtykgs').val(), qty_ltrs: $(this).find('#txt_qtyltrs').val(), fat: $(this).find('#txt_fat').val(), snf: $(this).find('#txt_snf').val() });
                }
            });
            if (silo_shift_wise_details.length == 0) {
                alert("Please enter new quantity");
                return false;
            }
            var confi = confirm("Do you want to save  Shift closing Details ?");
            if (confi) {
                var Data = { 'op': 'btnsave_shiftclosing_click', 'Shift_Wise_Details': silo_shift_wise_details, 'shiftid': shiftid, 'date': date };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        btn_cancel_click_shift();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                CallHandlerUsingJson(Data, s, e);
            }
        }

        function get_Shift_details() {
            var data = { 'op': 'get_Shift_details' };
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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldetails(msg) {
            var data = document.getElementById('slct_shift');
            var length = data.options.length;
            document.getElementById('slct_shift').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Shift type";
            opt.value = "Select Shift type";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].shiftname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].shiftname;
                    option.value = msg[i].shiftid;
                    data.appendChild(option);
                }
            }
        }
        function get_batchs() {
            var data = { 'op': 'get_batch_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbatchdetails(msg);
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
        function fillbatchdetails(msg) {
            $('.batchfillclsdetails').each(function () {
                var batch = $(this);
                batch[0].options.length = null;
                var opt = document.createElement('option');
                opt.innerHTML = "Select Batch";
                opt.value = "Select Batch";
                opt.setAttribute("selected", "selected");
                opt.setAttribute("disabled", "disabled");
                opt.setAttribute("class", "dispalynone");
                batch[0].appendChild(opt);
                for (var i = 0; i < msg.length; i++) {
                    if (msg[i].batchtype != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].batchtype;
                        option.value = msg[i].batchid;
                        batch[0].appendChild(option);
                    }
                }
            });
        }
        function get_Silosbatch() {
            var data = { 'op': 'get_Silomonitor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        get_batchs();
                        var results = '<div  style="overflow:auto;"><table id="table_batch_wiseclose_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Silo Name</th><th scope="col">Batch Type</th><th scope="col">Openingqty</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr>';
                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                            results += '<th><span id="txt_SiloName" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].SiloName + '</span></th>';
                            results += '<td><select id="ddlbatchtype" class="batchfillclsdetails form-control" ></select></td>';
                            //results += '<td class="5"><input id="txt_fromdate" class="fromdatecls" type="date" name="vendorcode"placeholder="Enter Date"></td>';
                            results += '<td><input id="txt_qtykgs" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].Quantity + '" type="text" name="vendorcode"placeholder="Enter qty kgs"></td>';
                            //                            results += '<td><input id="txt_qtyltrs" class="form-control" type="text" name="vendorcode"placeholder="Enter qty ltrs"></td>';
                            //                            results += '<td><input id="txt_fat" class="form-control" type="text" name="vendorcode"placeholder="Enter fat"></td>';
                            //                            results += '<td><input id="txt_snf" class="form-control" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                            results += '<td style="display:none" class="8"><input id="hdn_siloid" class="form-control" type="text" name="vendorcode" value="' + msg[i].SiloId + '"></td></tr>';
                        }
                        results += '</table></div>';
                        $("#div_batchget").html(results);
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
                        $('.fromdatecls').val(yyyy + '-' + mm + '-' + dd);
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
        function btn_cancel_click_batch() {

        }
        function btnsave_batchclosing_click() {
            var date = document.getElementById('txt_batchdate').value;
            if (date == "") {
                alert("Plese Select Date");
                return false;
            }
            var rows = $("#table_batch_wiseclose_details tr:gt(0)");
            var batch_wise_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_qtykgs').val() != "") {
                    batch_wise_details.push({ siloid: $(this).find('#hdn_siloid').val(), qty_kgs: $(this).find('#txt_qtykgs').val(), batchid: $(this).find('#ddlbatchtype').val() });
                }
            });
            if (batch_wise_details.length == 0) {
                alert("Please enter new quantity");
                return false;
            }
            var confi = confirm("Do you want to save  Batch closing Details ?");
            if (confi) {
                var Data = { 'op': 'btnsave_batchclosing_click', 'Batch_Wise_Details': batch_wise_details, 'date': date };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        btn_cancel_click_batch();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                CallHandlerUsingJson(Data, s, e);
            }
        }
        function get_Silos_silo() {
            var data = { 'op': 'get_Silomonitor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div    style="overflow:auto;"><table id="table_silo_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Silo Name</th><th scope="col">Quantity</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr>';
                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                            results += '<th><span id="txt_SiloName" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].SiloName + '</span></th>';
                            results += '<td><input id="txt_qtykgs" class="form-control" value="' + msg[i].Quantity + '" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter qty kgs"></td>';
                            results += '<td><input id="txt_fat" class="form-control" value="" type="text" onkeypress="return isFloat(event);" name="vendorcode"placeholder="Enter FAT"></td>';
                            results += '<td><input id="txt_snf" class="form-control" value="" type="text" onkeypress="return isFloat(event);" name="vendorcode"placeholder="Enter SNF"></td>';
                            results += '<td><input id="txt_clr" class="form-control" value="" type="text" onkeypress="return isFloat(event);" name="vendorcode"placeholder="Enter CLR"></td>';
                            results += '<td style="display:none" class="8"><input id="hdn_siloid" class="form-control" type="text" name="vendorcode" value="' + msg[i].SiloId + '"></td></tr>';
                        }
                        results += '</table></div>';
                        $("#div_siloget").html(results);
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
        function btn_cancel_click_silo() {
            document.getElementById("txt_fat").value = "";
            document.getElementById("txt_snf").value = "";
            get_Silos_silo();
        }
        function btnsave_siloclosing_click() {
            var rows = $("#table_silo_wise_details tr:gt(0)");
            var silo_wise_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_qtykgs').val() != "") {
                    silo_wise_details.push({ siloid: $(this).find('#hdn_siloid').val(), qty_kgs: $(this).find('#txt_qtykgs').val(), fat: $(this).find('#txt_fat').val(), snf: $(this).find('#txt_snf').val(), clr: $(this).find('#txt_clr').val() });
                }
            });
            if (silo_wise_details.length == 0) {
                alert("Please enter new quantity");
                return false;
            }
            var confi = confirm("Do you want to save  silo closing Details ?");
            if (confi) {
                var Data = { 'op': 'btnsave_siloclosing_click', 'silo_wise_details': silo_wise_details };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        get_Silos();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                CallHandlerUsingJson(Data, s, e);
            }
        }
        function save_departmentqtydetails_click() {
            var Department = document.getElementById('slct_Department').value;
            var Qtykgs = document.getElementById('txt_qtykgs').value;
            var Qtyltrs = document.getElementById('txt_qtyltrs').value;
            var BatchName = document.getElementById('txt_batchname').value;
            var Closingqty = document.getElementById('txt_closingqty').value;
            var flag = false;
            if (Department == "Select Department") {
                $("#lbl_department").show();
                flag = true;
            }
            if (Qtykgs == "") {
                $("#lbl_qty").show();
                flag = true;
            }
            if (BatchName == "") {
                $("#lbl_batchname").show();
                flag = true;
            }
            if (Closingqty == "") {
                $("#lbl_cqty").show();
                flag = true;
            }
            if (flag) {
                return;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_department_wise_qty_details_click', 'Department': Department, 'Qtykgs': Qtykgs, 'Qtyltrs': Qtyltrs, 'BatchName': BatchName, 'Closingqty': Closingqty };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            get_department_wise_qty_details();
                            Clearvalues();
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
            var data = { 'op': 'get_processingdeptqty', 'Department': Department };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('txt_qtykgs').value = msg[i].usingquantitykgs;

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

        function get_department_wise_qty_details() {
            var data = { 'op': 'get_department_wise_qty_details' };
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
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="text-align: center !important;">Department Name</th><th scope="col">Using Quantity(kgs)</th><th scope="col">Batch Name</th><th scope="col">Closing Quantity</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {

                results += '<tr><td scope="row" class="1">' + msg[i].departmentname + '</td>';
                results += '<td data-title="Code" class="2">' + msg[i].usingquantitykgs + '</td>';
                results += '<td data-title="Code" class="2">' + msg[i].batchname + '</td>';
                results += '<td data-title="Code" class="3">' + msg[i].closingqtyltrs + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function Clearvalues() {
            document.getElementById('slct_Department').selectedIndex = 0;
            document.getElementById('txt_qtykgs').value = "";
            document.getElementById('txt_qtyltrs').value = "";
            document.getElementById('txt_batchname').value = "";
            document.getElementById('txt_closingqty').value = "";
            $("#lbl_date").hide();
            $("#lbl_department").hide();
            $("#lbl_silo").hide();
            $("#lbl_clr").hide();
        }
        function get_butterproduction_details() {
            var data = { 'op': 'get_butterproduction_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div style="overflow:auto;"><table id="table_butter_close_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Silo Name</th><th scope="col">Openingqty</th><th scope="col">FAT</th><th scope="col">SNF</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr>';
                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                            results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].productname + '</span></th>';
                            results += '<td><input id="txt_qtykgs" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter qty kgs"></td>';
                            results += '<td><input id="txt_fat" class="form-control" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter fat"></td>';
                            results += '<td><input id="txt_snf" class="form-control" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                            results += '<td style="display:none" class="8"><input id="hdn_productsno" class="form-control" type="text" name="vendorcode" value="' + msg[i].sno + '"></td></tr>';
                        }
                        results += '</table></div>';
                        $("#div_getclosingbutter").html(results);
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
        function btnsave_butterclosing_click() {
            var rows = $("#table_butter_close_details tr:gt(0)");
            var butter_closing_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_qtykgs').val() != "") {
                    butter_closing_details.push({ productsno: $(this).find('#hdn_productsno').val(), qty_kgs: $(this).find('#txt_qtykgs').val(), fat: $(this).find('#txt_fat').val(), snf: $(this).find('#txt_snf').val() });
                }
            });
            if (butter_closing_details.length == 0) {
                alert("Please enter new quantity");
                return false;
            }
            var confi = confirm("Do you want to save  butter closing Details ?");
            if (confi) {
                var Data = { 'op': 'btnsave_butterclosing_click', 'butter_closing_details': butter_closing_details };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        Clearvaluesbutterclosing();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                CallHandlerUsingJson(Data, s, e);
            }
        }
        function Clearvaluesbutterclosing() {
            document.getElementById('txt_fat').value = "";
            document.getElementById('txt_snf').value = "";
            get_butterproduction_details();
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
            <i class="fa fa-files-o" aria-hidden="true"></i>Closing Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Closing Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showshiftwiseclosing()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Shift Wise Closing</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showbatchwiseclosing()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Batch Wise Closing</a></li>
                        <%--<li id="Li1" class=""><a data-toggle="tab" href="#" onclick="showsilowiseclosing()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Silo Wise Closing Details</a></li>
                        <li id="Li2" class=""><a data-toggle="tab" href="#" onclick="showdepartmentwiseclosing()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Department Wise Quantity Details</a></li>--%>
                        <li id="Li1" class=""><a data-toggle="tab" href="#" onclick="showbutterclosingdetails()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Butter Closing Details</a></li>
                </ul>
            </div>
       <div id="div_shiftwiseclose">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Shift Wise SILO Closing Balance
                    Details
                </h3>
            </div>
            <div class="box-body">
                <div style="text-align: center; padding-top: 10px; padding-bottom: 10px; padding-left:300px;">
                    <table align="center">
                        <tr>
                            <td>
                            <label>Date</label> 
                            </td>
                            <td>
                                <input id="txt_swdate" class="form-control"   type="datetime-local" />
                            </td>

                            <td>
                            <label>   Shift Type</label> 
                            </td>
                            <td>
                                <select id="slct_shift" class="form-control" >
                                    <option selected disabled value="Select Shift Name">Select Shift Name</option>
                                </select>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divFillScreen">
                </div>
                <div style="text-align: -webkit-center;">
                <table>
                    <%--<tr>
                        <td>
                            <input type="button" id="Button1" value="Save" onclick="btnsave_shiftclosing_click();"
                                class="btn btn-success" />
                        </td>
                        </tr>--%>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="btnsave_shiftclosing_click()"></span><span id="btn_save" onclick="btnsave_shiftclosing_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                </table>
                </div>
            </div>
        </div>
       </div>
       <div id="div_batchwiseclose">
       <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Batch Wise Closing Balance
                    Details
                </h3>
            </div>
            <div class="box-body">
            <div style="text-align: center; padding-top: 10px; padding-bottom: 10px; padding-left:250px;">
                <table align="center">
                        <tr>
                            <td>
                            <label>Date : </label> 
                            </td>
                            <td>
                                <input id="txt_batchdate" type="datetime-local" class="form-control"  />
                            </td>
                        </tr>
                    </table>
            </div>
                <div id="div_batchget">
                </div>
                <div style="text-align: -webkit-center;">
                <%--<table align="center">
                    <tr>
                        <td>
                            <input type="button" id="Button2" value="Save" onclick="btnsave_batchclosing_click();"
                                class="btn btn-success" />
                        </td>
                        </tr>
                </table>--%>
                <table>
                <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="Span1" onclick="btnsave_batchclosing_click()"></span><span id="Span2" onclick="btnsave_batchclosing_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                </table>
                </div>
            </div>
        </div>
       </div>
       <div id="div_silowiseclosing">
       <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Silo Wise Closing Balance
                    Details
                </h3>
            </div>
            <div class="box-body">
                <div id="div_siloget">
                </div>
                <table align="center">
                    <tr>
                        <td>
                            <input type="button" id="Button3" value="Save" onclick="btnsave_siloclosing_click();"
                                class="btn btn-success" />
                        </td>
                        </tr>
                </table>
            </div>
        </div>
       </div>
       <div id="div_departmentwiseclose">
       <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Department Wise Quantity Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;" >
                   <%-- <input id="btn_addDept" type="button" name="submit" value='Add Department Wise Quantity Details'
                        class="btn btn-success" />--%>
                         <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="adddepartment()"></span> <span onclick="adddepartment()">Add Quantity</span>
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
                                    Department</label>
                                <select id="slct_Department" class="form-control" onchange="get_deptqty();">
                                    <option selected disabled value="Select Department">Select Department</option>
                                </select>
                                <label id="lbl_department" class="errormessage">
                                    * Please select Department</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Quantity(kgs)<span style="color: red;">*</span></label>
                                <input id="txt_qtykgs" type="text" class="form-control" name="vendorcode" placeholder="Enter Qty in Kgs">
                                <label id="lbl_qty" class="errormessage">
                                    * Required Quantity</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Quantity(ltrs)<span style="color: red;">*</span></label>
                                <input id="txt_qtyltrs" type="text" class="form-control" name="vendorcode" onchange="return checkquantity();"
                                    placeholder="Enter Qty in ltrs">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Batch Name<span style="color: red;">*</span></label>
                                <input id="txt_batchname" type="text" class="form-control" name="vendorcode" placeholder="Enter Batch Name">
                                <label id="lbl_batchname" class="errormessage">
                                    * Required BatchName</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Closing Quantity(ltrs)<span style="color: red;">*</span></label>
                                <input id="txt_closingqty" type="text" class="form-control" name="vendorcode" placeholder="Enter Closing Quantity">
                                <label id="lbl_cqty" class="errormessage">
                                    * Required Closing Quantity</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input id='save_departmentqtydetails' type="button" class="btn btn-success" name="submit"
                                    value='Save' onclick="save_departmentqtydetails_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
       </div>
       <div id="div_butterclosingdetails">

       <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Butter Closing Details
                </h3>
            </div>
            <div class="box-body">
                <div id="div_getclosingbutter">
                </div>
                <table align="center">
                <tr hidden>
                <td>
                <input type="text" id="lbl_buttersno" value="Save"  class="btn btn-success" />
                </td>
                </tr>
                    <tr>
                        <td style="padding-left: 92%;">
                            <%--<input type="button" id="Button4" value="Save" onclick="btnsave_butterclosing_click();"
                                class="btn btn-success" />--%>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="Span3" onclick="btnsave_butterclosing_click()"></span><span id="Span4" onclick="btnsave_butterclosing_click()">Save</span>
                            </div>
                            </div>
                        </td>
                        </tr>
                </table>
            </div>
        </div>
       </div>
      </div>
    </section>
</asp:Content>
