<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="productionplaning.aspx.cs" Inherits="productionplaning" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btn_addDept').click(function () {
                $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
                get_Batchs();
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
                $('#txt_date').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
                get_cowdetailschart();
                get_buffalodetailschart();
                get_silopositions();
                get_ToSilos();
            });

            $('#btn_close').click(function () {
                $('#Inwardsilo_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                $('#div_planingsmp').css('display', 'none');
                $('#div_planingcream').css('display', 'none');
                $('#div_planingbutter').css('display', 'none');
                $('#div_planing').css('display', 'none');
                $('#div_batchusage').css('display', 'none');

                $('#div_showbatchusage').css('display', 'none');
                $('#div_showfilm').css('display', 'none');
                $('#div_showbutter').css('display', 'none');
                $('#div_showcream').css('display', 'none');
                $('#div_plan').css('display', 'none');
                Clearvalues();
                $('#div_showcream').css('display', 'none');
                $('#div_showbutter').css('display', 'none');
                $('#div_showfilm').css('display', 'none');
            });
            // get_batchentrydetails();
            //get_ToSilos();
            get_Batchs();
            get_cowdetailschart();
            get_buffalodetailschart();
            production_planing_smp();
            production_planing_cream();
            production_planing_butter();
            get_silopositions();

            get_ToSilos();

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
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#txt_cowbuffdate').val(yyyy + '-' + mm + '-' + dd);
        });
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
            $('#txt_cowbuffdate').val(yyyy + '-' + mm + '-' + dd);
            // get_batchentrydetails();
        });

        function get_silopositions() {
            var data = { 'op': 'get_silopositions' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        silopositions(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function silopositions(msg) {
            var milkresults = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable"  role="grid" aria-describedby="example2_info">';
            milkresults += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Silo</th><th scope="col" style="font-weight: bold;">Batch Type</th><th scope="col" style="font-weight: bold;">Qty</th><th scope="col" style="font-weight: bold;">Fat</th><th scope="col" style="font-weight: bold;">Snf</th><th scope="col" style="font-weight: bold;">Clr</th><th scope="col" style="font-weight: bold;">Status</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#cfe2e0", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].status == "Ready For Processing") {
                    milkresults += '<tr style="background-color:' + COLOR[0] + '">';
                }
                else {
                    milkresults += '<tr>';
                }

                milkresults += '<td scope="row" class="1">' + msg[i].silo + '</td>';
                milkresults += '<td scope="row" class="1">' + msg[i].batch + '</td>';
                milkresults += '<td data-title="Code" class="2">' + msg[i].qtykg + '</td>';
                milkresults += '<td data-title="Code" class="3">' + msg[i].fat + '</td>';
                milkresults += '<td data-title="Code" class="4">' + msg[i].snf + '</td>';
                milkresults += '<td data-title="Code" class="5">' + msg[i].clr + '</td>';
                milkresults += '<td data-title="Code" class="5">' + msg[i].status + '</td>';
                milkresults += '</tr>';
            }
            milkresults += '</table></div>';
            $("#divsiloqty").html(milkresults);
        }

        function get_cowdetailschart() {
            var cowbuffdate = document.getElementById("txt_getdatadate").value;
            var data = { 'op': 'get_cowmilkdetailschart', 'cowbuffdate': cowbuffdate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        cowdetail(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var Qtykg = [];
        var milkresults;
        function cowdetail(msg) {
            var Qtykg = msg[0].qtykg;
            var Qtyltr = msg[0].qtyltr;
            var snf = msg[0].snf;
            var fat = msg[0].fat;
            var kgfat = msg[0].kgfat;
            var kgsnf = msg[0].kgsnf;
            milkresults = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable"  role="grid" aria-describedby="example2_info">';
            milkresults += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Milk Type</th><th scope="col" style="font-weight: bold;">Qty(kgs)</th><th scope="col" style="font-weight: bold;">Qty(ltrs)</th><th scope="col" style="font-weight: bold;">Avg Fat</th><th scope="col" style="font-weight: bold;">Avg Snf</th><th scope="col" style="font-weight: bold;">KG Fat</th><th scope="col" style="font-weight: bold;">KGSnf</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var milktype = "Cow";
            for (var i = 0; i < msg.length; i++) {
                milkresults += '<tr style="background-color:' + COLOR[l] + '">';
                milkresults += '<td scope="row" class="1">' + milktype + '</td>';
                milkresults += '<td data-title="Code" class="2">' + Qtykg + '</td>';
                milkresults += '<td data-title="Code" class="3">' + Qtyltr + '</td>';
                milkresults += '<td data-title="Code" class="4">' + fat + '</td>';
                milkresults += '<td data-title="Code" class="5">' + snf + '</td>';
                milkresults += '<td data-title="Code" class="20">' + kgfat + '</td>';
                milkresults += '<td data-title="Code" class="5">' + kgsnf + '</td>';
                milkresults += '</tr>';
            }
            milkresults += '</table></div>';
            $("#divmilkdtlsbind").html(milkresults);
        }
        function get_buffalodetailschart() {
            var cowbuffdate = document.getElementById("txt_getdatadate").value;
            var data = { 'op': 'get_buffalomilkdetailschart', 'cowbuffdate': cowbuffdate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        buffalodetail(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var Qtyltr = [];
        function buffalodetail(msg) {
            var Qtykg = msg[0].qtykg;
            var Qtyltr = msg[0].qtyltr;
            var snf = msg[0].snf;
            var fat = msg[0].fat;
            var kgfat = msg[0].kgfat;
            var kgsnf = msg[0].kgsnf;
            var milktype = "Buffalow";
            var milkresults = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable"  role="grid" aria-describedby="example2_info">';
            for (var i = 0; i < msg.length; i++) {
                milkresults += '<tr>';
                milkresults += '<td scope="row" class="1">' + milktype + '</td>';
                milkresults += '<td data-title="Code" class="2">' + Qtykg + '</td>';
                milkresults += '<td data-title="Code" class="3">' + Qtyltr + '</td>';
                milkresults += '<td data-title="Code" class="4">' + fat + '</td>';
                milkresults += '<td data-title="Code" class="5">' + snf + '</td>';
                milkresults += '<td data-title="Code" class="20">' + kgfat + '</td>';
                milkresults += '<td data-title="Code" class="5">' + kgsnf + '</td>';
                milkresults += '</tr>';
            }
            milkresults += '</table></div>';
            $("#divbuffmilkdtlsbind").html(milkresults);
        }

        function addbatches() {
            $('#Inwardsilo_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            $('#div_planing').css('display', 'none');
            $('#div_showfilm').css('display', 'none');
            get_Batchs();
            //            get_batchentrydetails();
            //            get_ToSilos();
            get_Batchs();
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
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            production_planing_smp();
            production_planing_cream();
            production_planing_butter();
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
            document.getElementById('slct_Batch').selectedIndex = 0;
            document.getElementById('ddltype').selectedIndex = 0;
            document.getElementById('ddlsource').selectedIndex = 0;
            document.getElementById('ddlDestinationSilo').selectedIndex = 0;
            document.getElementById('txt_qtykgs').value = "";
            document.getElementById('txt_qtyltrs').value = "";
            document.getElementById('txt_fat').value = "";
            document.getElementById('txt_snf').value = "";
            document.getElementById('txt_clr').value = "";
            document.getElementById('txt_smp').value = "";
            document.getElementById('txt_dcno').value = "";
            document.getElementById('save_batchdetails').innerHTML = "Save";
            document.getElementById('txt_rate').value = "";
            $('#div_showfilm').css('display', 'none');
            $('#div_showcream').css('display', 'none');
            $('#div_showbutter').css('display', 'none');
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
        function checkcapacity() {
            var type = document.getElementById('ddltype').value;
            var siloid = document.getElementById('ddlsource').value;
            if (type == "From SILO") {
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
        }
        function fillcapacity(msg) {
            var btnval = document.getElementById('save_batchdetails').innerHTML;
            if (btnval == "Save") {
                for (var i = 0; i < msg.length; i++) {
                    var silocapacity = msg[i].Capacity;
                    var qtycltrs = parseInt(document.getElementById('txt_qtyltrs').value);
                    var caty = parseInt(silocapacity);
                    if (qtycltrs > caty) {
                        alert("Please enter less than the from silo Quantity.");
                        document.getElementById('txt_qtyltrs').value = "";
                        document.getElementById('txt_qtykgs').value = "";
                        $("#txt_qtyltrs").focus();
                    }
                }
            }
        }
        function getsiloqtysource() {
            var siloname = document.getElementById('ddlsource').value;
            var data = { 'op': 'get_siloqty_details', 'siloname': siloname };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('lbl_silosourceqty').value = msg[i].quantity;
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
            qtyltrkgs = qtyltr * modclr;
            qtyltrkgs = parseFloat(qtyltrkgs).toFixed(0);
            document.getElementById('txt_qtykgs').value = qtyltrkgs;

            var fat = document.getElementById('txt_fat').value;
            var snf = document.getElementById('txt_snf').value;

            var kgfat = (qtyltrkgs * fat) / 100;
            var kgsnf = (qtyltrkgs * snf) / 100;
            document.getElementById('txtkgfat').value = parseFloat(kgfat).toFixed(2);
            document.getElementById('txtkgsnf').value = parseFloat(kgsnf).toFixed(2);
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
                    document.getElementById('txt_qtykgs').value = qtyltrkgs;
                }

                ////////For SNF ////////////////
                var type = document.getElementById('ddltype').value;
                var fatchange = document.getElementById('txt_fat').value;
                if (fatchange > 10) {
                    var fat = 0;
                    fat = document.getElementById('txt_fat').value;
                    fat = parseFloat(fat).toFixed(3);
                    var snfvalue = (100 - fat) / 11;
                    document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);
                }
                else {
                    var fat = 0;
                    fat = document.getElementById('txt_fat').value;
                    fat = parseFloat(fat).toFixed(3);
                    var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
                    document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);
                }
            }
        }
        function ddlsourceonchange() {
            var type = document.getElementById('ddltype').value;
            if (type == "From SILO") {
                getsiloqtysource();
                document.getElementById('txt_snf').readOnly = true;
                var siloid = document.getElementById('ddlsource').value;
                var date = document.getElementById('txt_date').value;
                document.getElementById('txt_rate').value = "0";
                var data = { 'op': 'get_intrasilo_perltrrate_details', 'siloid': siloid, 'date': date };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            for (var i = 0; i < msg.length; i++) {
                                //var ltrcost = msg[i].perltr;
                                document.getElementById('txt_rate').value = parseFloat(msg).toFixed(2);
                            }
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
            if (type == "Cream") {
                document.getElementById('txt_snf').readOnly = true;
            }
            else {
                document.getElementById('txt_snf').readOnly = true;
            }
            if (type == "From CC") {
                var vendor = document.getElementById('ddlsource').value;
                var data = { 'op': 'get_vendorperltrrate_details', 'vendor': vendor };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            for (var i = 0; i < msg.length; i++) {
                                var ltrcost = msg[i].perltr;
                                document.getElementById('txt_rate').value = ltrcost;
                            }
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
        }

        function ddltypeonchange() {
            var type = document.getElementById('ddltype').value;
            if (type == "From SILO" || type == "Cream") {
                get_Silos();
                $('#div_dcnoshow').hide();
                $('#td_creamopbal').show();
                var productid = "1217";
                var data = { 'op': 'get_productqty_details', 'productid': productid };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            for (var i = 0; i < msg.length; i++) {
                                document.getElementById('txt_creamopbal').value = msg[i].quantity;
                            }
                        }
                        else {
                            document.getElementById('txt_creamopbal').value = "0";
                        }
                    }
                    else {
                        document.getElementById('txt_creamopbal').value = "0";
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
            if (type == "From CC") {
                get_Vendor_details();
                $('#div_dcnoshow').show();
                $('#td_creamopbal').hide();
            }
            if (type == "Return Milk" || type == "Mixed Milk" || type == "Cutting Milk" || type == "RCM Water" || type == "Condensed milk") {
                get_Proces_dept_details();
                $('#div_dcnoshow').hide();
                $('#td_creamopbal').hide();
            }
        }
        function get_Proces_dept_details() {
            var data = { 'op': 'get_processingdepartment_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillProcessingDept(msg);
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

        function fillProcessingDept(msg) {
            var data = document.getElementById('ddlsource');
            var length = data.options.length;
            document.getElementById('ddlsource').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Dept Name";
            opt.value = "Select Dept Name";
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
        function get_Vendor_details() {
            var data = { 'op': 'get_Vendor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var Vendor = msg[0].vendordetails;
                        fillSource(Vendor);
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

        function fillSource(msg) {
            var data = document.getElementById('ddlsource');
            var length = data.options.length;
            document.getElementById('ddlsource').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vendor Name";
            opt.value = "Select Vendor Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].vendorname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].vendorname;
                    option.value = msg[i].sno;
                    data.appendChild(option);
                }
            }
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
            var data = document.getElementById('ddlsource');
            var length = data.options.length;
            document.getElementById('ddlsource').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select SILO";
            opt.value = "Select SILO";
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
        function get_ToSilos() {
            var data = { 'op': 'get_Silo_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillToSilos(msg);
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
        function fillToSilos(msg) {
            var data = document.getElementById('ddlDestinationSilo');
            var length = data.options.length;
            document.getElementById('ddlDestinationSilo').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select SILO";
            opt.value = "Select SILO";
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
        function getsiloqty() {
            var siloname = document.getElementById('ddlDestinationSilo').value;
            var data = { 'op': 'get_silopositions' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].siloid == siloname) {
                                document.getElementById('txtfrmsiloqty').value = msg[i].qtykg;
                                document.getElementById('txtfrmsilofat').value = msg[i].fat;
                                document.getElementById('txtfrmsilosnf').value = msg[i].snf;
                            }
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
        //Function for only no
        function isNumberKey(evt, obj) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            var value = obj.value;
            var dotcontains = value.indexOf(".") != -1;
            if (dotcontains)
                if (charCode == 46) return false;
            if (charCode == 46) return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function save_batchentrydetails_click() {
            var doe = document.getElementById('txt_date').value;
            if (doe == "") {
                alert("Please Select Date");
                return false;
            }
            var batch = document.getElementById('slct_Batch').value;
            if (batch == "" || batch == "Select Batch") {
                alert("Please Select Batch Name");
                return false;
            }
            var type = document.getElementById('ddltype').value;
            var dcno = document.getElementById('txt_dcno').value;
            if (type == "From CC" && dcno == "") {
                alert("Please Enter Transaction  No");
                return false;
            }
            if (type == "From CC" && dcno == "0") {
                alert("Please Enter Correct Transaction No");
                return false;
            }
            var source = document.getElementById('ddlsource').value;
            if (type == "From CC" && source == "Select Vendor Name") {
                alert("Please Select Vendor Name");
                return false;
            }
            if (type == "From CC" && source == "") {
                alert("Please Select Vendor Name");
                return false;
            }
            if (type == "From SILO" && source == "Select SILO") {
                alert("Please Select Silo Name");
                $("#ddlsource").focus();
                return false;
            }
            var destinationsilo = document.getElementById('ddlDestinationSilo').value;
            if (destinationsilo == "Select SILO") {
                alert("Please Select To Silo Name");
                $("#ddlDestinationSilo").focus();
                return false;
            }
            var Qtykgs = document.getElementById('txt_qtykgs').value;
            var Qtyltrs = document.getElementById('txt_qtyltrs').value;
            if (Qtyltrs == "") {
                alert("Please Enter Qty ltrs");
                $("#txt_qtyltrs").focus();
                return false;
            }
            var fat = document.getElementById('txt_fat').value;
            if (fat == "") {
                alert("Please Enter fat");
                $("#txt_fat").focus();
                return false;
            }
            var snf = document.getElementById('txt_snf').value;
            var clr = document.getElementById('txt_clr').value;
            if (clr == "") {
                alert("Please Enter clr");
                $("#txt_clr").focus();
                return false;
            }
            var rate = document.getElementById('txt_rate').value;
            var siloqty = document.getElementById('lbl_siloqty').value;
            var btnval = document.getElementById('save_batchdetails').innerHTML;
            var smp = document.getElementById('txt_smp').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var oldqty = document.getElementById('lbl_qtyold').innerHTML;
            var creamopbal = document.getElementById('txt_creamopbal').value;
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_batchentrydetails_click', 'doe': doe, 'rate': rate, 'type': type, 'source': source, 'destinationsilo': destinationsilo, 'clr': clr,
                    'batch': batch, 'fat': fat, 'snf': snf, 'Qtykgs': Qtykgs, 'Qtyltrs': Qtyltrs, 'btnval': btnval, 'smp': smp, 'siloqty': siloqty, 'sno': sno, 'dcno': dcno, 'creamopbal': creamopbal
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_batchentrydetails();
                            Clearvalues();
                            $('#div_Deptdata').show();
                            $('#Inwardsilo_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                            $('#div_planingsmp').css('display', 'none');
                            $('#div_planingcream').css('display', 'none');
                            $('#div_planingbutter').css('display', 'none');
                            $('#div_batchusage').css('display', 'none');
                            $('#div_showcream').css('display', 'none');
                            $('#div_showbutter').css('display', 'none');
                            $('#div_showfilm').css('display', 'none');
                            $('#div_showbatchusage').css('display', 'none');
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

        function batchwisefatsnfdetails() {
            var batch = document.getElementById('slct_Batch').value;
            if (batch == "" || batch == "Select Batch") {
                alert("Please Select Batch Name");
                document.getElementById('txt_qtyltrs').value = "";
                document.getElementById('txt_qtykgs').value = "";
                return false;
            }
            var data = { 'op': 'batchwisefatsnfdetails', 'batch': batch };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        document.getElementById('txt_fat').value = msg[0].fat;
                        document.getElementById('txt_snf').value = msg[0].snf;
                        document.getElementById('txt_clr').value = msg[0].clr;
                    }
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        // Film Consumption
        function production_planing_cost() {
            var batch = document.getElementById('slct_Batch').value;
            if (batch == "" || batch == "Select Batch") {
                alert("Please Select Batch Name");
                document.getElementById('txt_qtyltrs').value = "";
                document.getElementById('txt_qtykgs').value = "";
                return false;
            }
            var ltrs = document.getElementById('txt_qtyltrs').value;
            var data = { 'op': 'production_planing_cost', 'batch': batch, 'ltrs': ltrs };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillplaning(msg);
                    }
                    else {
                        $('#div_showfilm').css('display', 'none');
                        $('#div_planing').css('display', 'none');
                    }
                }
                else {
                    $('#div_showfilm').css('display', 'none');
                    $('#div_planing').css('display', 'none');
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillplaning(msg) {
            $('#div_showfilm').css('display', 'block');
            $('#div_planing').css('display', 'block');
            var results = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable"  role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">No Of Packets</th><th scope="col" style="font-weight: bold;">Film Consumption</th><th scope="col" style="font-weight: bold;">Rate Per Liters</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Code" class="2">' + msg[i].productname + '</td>';
                results += '<td data-title="Code" class="4"><div style="float:right;">' + parseFloat(msg[i].packs).toFixed(2) + '</div></td>';
                results += '<td data-title="Code" class="3"><div style="float:right;">' + parseFloat(msg[i].filmconsumpction).toFixed(2) + '</div></td>';
                results += '<td data-title="Code" class="4"><div style="float:right;">' + parseFloat(msg[i].perltrcost).toFixed(2) + '</div></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_planing").html(results);
        }
        //SMP
        function production_planing_smp() {
            var data = { 'op': 'production_planing_smp' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillplaningsmp(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillplaningsmp(msg) {
            $('#div_showskim').css('display', 'block');
            $('#divmilkdtls').css('display', 'block');
            $('#div_planingsmp').css('display', 'block');
            var results = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable"  role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">Stock</th><th scope="col" style="font-weight: bold;">Avg Usage</th><th scope="col" style="font-weight: bold;">Remaining Days To Use</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#cfe2e0", "#f3f5f7", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                var avgusge = 50;
                var qty = parseFloat(msg[i].qty) || 0;
                var totdays = qty / avgusge;
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Code" class="2">' + msg[i].productname + '</td>';
                results += '<td data-title="Code" class="3" ><div style="float:right;">' + parseFloat(qty).toFixed(2) + '</div></td>';
                results += '<td data-title="Code" class="4" ><div style="float:right;">' + parseFloat(avgusge).toFixed(2) + '</div></td>';
                results += '<td data-title="Code" class="5" ><div style="float:right;">' + parseFloat(totdays).toFixed(2) + '</div></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_planingsmp").html(results);
        }
        //cream
        function production_planing_cream() {
            var data = { 'op': 'get_gheecream_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillplaningcream(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillplaningcream(msg) {
            $('#div_showcream').css('display', 'block');
            $('#div_planingcream').css('display', 'block');
            var results = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable"  role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Cream Type</th><th scope="col" style="font-weight: bold;">Quantity</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#cfe2e0", "#f3f5f7", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                var creamtype = parseFloat(msg[i].creamtype) || 0;
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Code" class="2">' + msg[i].productname + '</td>';
                results += '<td data-title="Code" class="3" ><div style="float:right;">' + parseFloat(creamtype).toFixed(2) + '</div></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_planingcream").html(results);
        }
        //butter
        function production_planing_butter() {
            var data = { 'op': 'get_buttercream_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillplaningbutter(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillplaningbutter(msg) {
            $('#div_showbutter').css('display', 'block');
            $('#div_planingbutter').css('display', 'block');
            var results = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable"  role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Cream Type</th><th scope="col" style="font-weight: bold;">Quantity</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#cfe2e0", "#f3f5f7", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                var creamtype = parseFloat(msg[i].creamtype) || 0;
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Code" class="2">' + msg[i].productname + '</td>';
                results += '<td data-title="Code" class="3" ><div style="float:right;">' + parseFloat(creamtype).toFixed(2) + '</div></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_planingbutter").html(results);
        }


        function get_ToSilos() {
            var data = { 'op': 'get_Silo_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillToSilos(msg);
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
        function fillToSilos(msg) {
            var data = document.getElementById('ddlDestinationSilo');
            var length = data.options.length;
            document.getElementById('ddlDestinationSilo').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select SILO";
            opt.value = "Select SILO";
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









        //avg fat
        function get_batchpreparation_details() {
            var ltrs = document.getElementById('txt_qtyltrs').value;
            var fat = document.getElementById('txt_fat').value;
            var snf = document.getElementById('txt_snf').value;
            var batch = document.getElementById('slct_Batch').value;
            var data = { 'op': 'get_batchpreparation_details', 'ltrs': ltrs, 'fat': fat, 'snf': snf, 'batch': batch };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbchprp(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillbchprp(msg) {
            $('#div_showbatchusage').css('display', 'block');
            $('#div_batchusage').css('display', 'block');
            var results = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable"  role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Type</th><th scope="col" style="font-weight: bold;">Fat</th><th scope="col" style="font-weight: bold;">Snf</th><th scope="col" style="font-weight: bold;">KgFat</th><th scope="col" style="font-weight: bold;">KgSnf</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#cfe2e0", "#f3f5f7", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                var fat = parseFloat(msg[i].fat) || 0;
                var snf = parseFloat(msg[i].snf) || 0;
                var kgfat = parseFloat(msg[i].kgfat) || 0;
                var kgsnf = parseFloat(msg[i].kgsnf) || 0;

                var creamkgfat = "";
                var creamkgsnf = "";

                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Code" class="2">' + msg[i].productname + '</td>';
                results += '<td data-title="Code" class="2"><div style="float:right;">' + parseFloat(fat).toFixed(2) + '</div></td>';
                results += '<td data-title="Code" class="2"><div style="float:right;">' + parseFloat(snf).toFixed(2) + '</div></td>';
                results += '<td data-title="Code" class="2"><div style="float:right;">' + parseFloat(kgfat).toFixed(2) + '</div></td>';
                results += '<td data-title="Code" class="2"><div style="float:right;">' + parseFloat(kgsnf).toFixed(2) + '</div></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_batchusage").html(results);
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
        var batcharray = [];
        function addbtn_click() {
            var qty = document.getElementById('txtfrmsiloqty').value;
            var fat = document.getElementById('txtfrmsilofat').value;
            var snf = document.getElementById('txtfrmsilosnf').value;
            var silo = document.getElementById('ddlDestinationSilo').value;


            var Checkexist = false;
            $('.AccountClass').each(function (i, obj) {
                var IName = $(this).text();
                if (IName == silo) {
                    alert("Account Already Added");
                    Checkexist = true;
                }
            });
            if (Checkexist == true) {
                return;
            }
            batcharray.push({ 'silo': silo, 'Qtykg': qty, 'fat': fat, 'snf': snf });
            if (batcharray.length == "0") {
                alert("Please enter quantity in kg's, fat, snf details");
                return false;
            }
            else {
                var results = '<div  style="overflow:auto;"><table id="tableCashFormdetails" class="responsive-table">';
                results += '<thead><tr><th scope="col"></th><th scope="col">Silo Name</th><th scope="col">Quantity</th><th scope="col">Fat</th><th scope="col">Snf</th><th></th></tr></thead></tbody>';
                for (var i = 0; i < batcharray.length; i++) {
                    results += '<tr><td></td>';
                    results += '<td scope="row" class="1" style="text-align:center;"><span id="txtAccount" style="font-size: 16px; color: Red;  font-weight: bold;" class="AccountClass"><b>' + batcharray[i].silo + '</b></span></td>';
                    results += '<td class="2"><span id="txtamount" style="font-size: 16px; color: green; font-weight: bold;"class="AmountClass"><b>' + batcharray[i].Qtykg + '</b></span></td>';
                    results += '<td class="2"><span id="txtfat" style="font-size: 16px; color: green; font-weight: bold;"class="AmountClass"><b>' + batcharray[i].fat + '</b></span></td>';
                    results += '<td class="2"><span id="txtsnf" style="font-size: 16px; color: green; font-weight: bold;"class="AmountClass"><b>' + batcharray[i].snf + '</b></span></td>';
                    results += '<td  class="6"> <img src="images/Odclose.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Edit Qty"/> </td></tr>';
                }
                results += '</table></div>';
                $("#divsilobatch").html(results);
            }
        }
        function RowDeletingClick(Account) {
            batcharray = [];
            var snf = "";
            var fat = "";
            var qty = "";
            var Account = $(Account).closest("tr").find("#txtAccount").text();
            var Amount = "";
            var rows = $("#tableCashFormdetails tr:gt(0)");
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtamount').text() != "") {
                    silo = $(this).find('#txtAccount').text();
                    qty = $(this).find('#txtamount').text();
                    fat = $(this).find('#txtfat').text();
                    snf = $(this).find('#txtsnf').text();
                    if (Account == silo) {
                    }
                    else {
                        batcharray.push({ 'silo': silo, 'Qtykg': qty, 'fat': fat, 'snf': snf });
                    }
                }
            });
            var results = '<div  style="overflow:auto;"><table id="tableCashFormdetails" class="responsive-table">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Silo Name</th><th scope="col">Quantity</th><th scope="col">Fat</th><th scope="col">Snf</th><th></th></tr></thead></tbody>';
            for (var i = 0; i < batcharray.length; i++) {
                results += '<tr><td></td>';
                results += '<td scope="row" class="1" style="text-align:center;"><span id="txtAccount" style="font-size: 16px; color: Red;  font-weight: bold;" class="AccountClass"><b>' + batcharray[i].silo + '</b></span></td>';
                results += '<td class="2"><span id="txtamount" style="font-size: 16px; color: green; font-weight: bold;"class="AmountClass"><b>' + batcharray[i].Qtykg + '</b></span></td>';
                results += '<td class="2"><span id="txtfat" style="font-size: 16px; color: green; font-weight: bold;"class="AmountClass"><b>' + batcharray[i].fat + '</b></span></td>';
                results += '<td class="2"><span id="txtsnf" style="font-size: 16px; color: green; font-weight: bold;"class="AmountClass"><b>' + batcharray[i].snf + '</b></span></td>';
                results += '<td  class="6"> <img src="images/Odclose.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Edit Qty"/> </td></tr>';
            }
            results += '</table></div>';
            $("#divsilobatch").html(results);
        }

    </script>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
           <i class="fa fa-cogs" aria-hidden="true"></i> Production Planning<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Production Preparation</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Planning Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="display:none;">
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
                        <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="get_batchentrydetails()"></span> <span onclick="get_batchentrydetails()">Generate</span>
                          </div>
                          </div>
                        </td>
                        <td  style="padding-left:200px !important">
                            <td>
                            </td>
                            <td>

                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addbatches()"></span> <span onclick="addbatches()">Add Batch</span>
                          </div>
                          </div>
                            </td>
                    </tr>
                </table>
               
                </div>
                <div id="div_Deptdata">
                </div>

                <div class="row">
            <div class="col-md-6" style="width:40% !important;">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            </h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button class="btn btn-box-tool" data-widget="remove">
                                <i class="fa fa-times"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="chart">
                            <div id="Inwardsilo_fillform">
                             <table align="center" style="">
                        <tr>
                            <td>
                                <label>
                                    Date<span style="color: red;">*</span></label>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                placeholder="Enter Date">
                            </td>
                            <td style="width:3px;"></td>
                             <td>
                                <label>
                                    Batch Type<span style="color: red;">*</span></label>
                                <select id="slct_Batch" class="form-control" onchange="batchwisefatsnfdetails();">
                                    <option selected disabled value="Select SILO No">Select Batch Type</option>
                                </select>
                                <label id="lbl_silo" class="errormessage">
                                    * Please Batch Type</label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <label>
                                    FAT<span style="color: red;">*</span></label>
                                <input id="txt_fat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter FAT">
                                <label id="lbl_fat" class="errormessage">
                                    * Please enter fat</label>
                            </td>
                            <td  style="width: 3px;"></td>
                            <td>
                                <label>
                                    SNF</label>
                                <input id="txt_snf" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="SNF"/>
                                <label id="lbl_snf" class="errormessage">
                                    * Please enter snf</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Qty(Kgs)</label>
                                <input id="txt_qtykgs" type="text" class="form-control" name="vendorcode" placeholder="Enter Qty in Kgs" readonly  onkeypress="return isFloat(event);">
                                <label id="lbl_Qtykgs" class="errormessage">
                                    * Please enter Quantity(kgs)</label>
                            </td>
                            <td  style="width: 3px;"></td>
                            <td>
                                <label>
                                    Qty(ltrs)<span style="color: red;">*</span></label>
                                <input id="txt_qtyltrs" type="text" class="form-control" name="vendorcode" placeholder="Enter Qty in ltrs" onkeypress="return isFloat(event);" onchange="LtrsChange(this);production_planing_cost();checkcapacity();" >
                                <label id="lbl_Qtyltrs" class="errormessage">
                                    * Please enter Quantity(ltrs)</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    CLR<span style="color: red;">*</span></label>
                                <input id="txt_clr" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);" placeholder="Enter CLR" 
                                    onkeyup="LRChange(this)"  onchange="get_batchpreparation_details();" />
                                <label id="lbl_clr" class="errormessage">
                                    * Please enter CLR</label>
                            </td>
                              <td  style="width: 3px;"></td>
                            <td>
                                <label>
                                   Kgfat<span style="color: red;">*</span></label>
                                 <input id="txtkgfat" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);" placeholder="Enter kgfat" />
                            </td>
                        </tr>
                        <tr>
                        <td>
                        <label>
                                    Kgsnf<span style="color: red;">*</span></label>
                                <input id="txtkgsnf" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Kg snf"/>
                                <label id="lbl_smp" class="errormessage">
                                    * Please enter kgsnf</label>
                        </td>
                        </tr>

                        <tr>
                        <td>&nbsp</td>
                        </tr>


                        <tr>
                             <td>
                                <label>
                                    From Silo<span style="color: red;">*</span></label>
                                <select id="ddlDestinationSilo" class="form-control" onchange="getsiloqty();">
                                </select>
                            </td>
                            <td  style="width: 3px;"></td>
                            <td>
                                <label>
                                    Qty(Kgs)</label>
                                <input id="txtfrmsiloqty" type="text" class="form-control" name="vendorcode" placeholder="Enter Qty in Kgs" readonly  onkeypress="return isFloat(event);">
                                <label id="Label3" class="errormessage">
                                    * Please enter Quantity(kgs)</label>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <label>
                                    FAT<span style="color: red;">*</span></label>
                                <input id="txtfrmsilofat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" readonly placeholder="Enter FAT">
                                <label id="Label1" class="errormessage">
                                    * Please enter fat</label>
                            </td>
                            <td  style="width: 3px;"></td>
                            <td>
                                <label>
                                    SNF</label>
                                <input id="txtfrmsilosnf" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" readonly placeholder="SNF"/>
                                <label id="Label2" class="errormessage">
                                    * Please enter snf</label>
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_siloqty"></label>
                                 <input id="txt_cowbuffdate"  type="date" name="vendorcode"/> 
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_silosourceqty"></label>
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
                                <label id="lbl_qtyold">
                                </label>
                            </td>
                        </tr>
                        </table>
                      
                    <div  style="padding-left: 30%;padding-top: 0%;">
                        <table>
                        <tr>
                        <td></td>  <br />
                        </tr>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-plus" id="save_batchdetails1" onclick="addbtn_click()"></span><span id="save_batchdetails" onclick="addbtn_click()">Add</span>
                            </div>
                            </div>
                            </td>

                            </tr>
                             </table> <br />
                    </div>
                    
                    <div>
                    <table>
                    <tr>
                    <td>
                   
                     <div id="divsilobatch"></div>
                    </td>
                    </tr>
                    </table>
                    </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
                <!-- DONUT CHART -->
                <!-- /.box -->
            </div>
            <!-- /.col (LEFT) -->
            <div class="col-md-6" style="width:60% !important;">
                <!-- LINE CHART -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button class="btn btn-box-tool" data-widget="remove">
                                <i class="fa fa-times"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="chart">
                            <div id="div_plan">

                               <div id="divmilkdtls">
                                <div id="divsiloqty">
                        </div>
                        <div id="divmilkdtlsbind">
                        </div>
                        <div id="divbuffmilkdtlsbind">
                        </div>
                       
                    </div>

                     <div id="div_showskim">
                        <div id="div_planingsmp">
                        </div>
                    </div>

                 

                     <div  id="div_showcream">
                        <div id="div_planingcream">
                        </div>
                    </div>
                    <div  id="div_showbutter">
                        <div id="div_planingbutter">
                        </div>
                    </div>
                </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
                <!-- BAR CHART -->
                <!-- /.box -->
            </div>
            <!-- /.col (RIGHT) -->
        </div>
















                <div style="width:100%;">
                
            <div class="box-body">
            <div style="display:none;width:100%;" id="div_showfilm">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;"></i>Film Consumption
                        </h3>
                        <div id="div_planing">
                        </div>
                    </div>
            <div style="display:none;width:100%;" id="div_showbatchusage">
                <h3 class="box-title">
                    <i style="padding-right: 5px;"></i>Batch Prepare
                </h3>
                <div id="div_batchusage">
                </div>
            </div>
          </div>
        </div>
    </section>
</asp:content>
