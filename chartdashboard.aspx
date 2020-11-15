<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="chartdashboard.aspx.cs" Inherits="chartdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="JSF/jquery.blockUI.js" type="text/javascript"></script>
    <script src="Charts/amcharts.js" type="text/javascript"></script>
    <script src="Charts/serial.js" type="text/javascript"></script>
    <link href="Charts/style.css" rel="stylesheet" type="text/css" />
    <script src="Charts/light.js" type="text/javascript"></script>
    <style type="text/css">
        #chartdiv1
        {
            width: 1000px;
            height: 500px;
            font-size: 11px;
            position: absolute;
        }
        #mychart
        {
            width: 1000px;
            height: 500px;
            font-size: 11px;
            position: absolute;
        }
        #chartdiv3
        {
            width: 10px;
            height: 50px;
        }
        #chartdiv4
        {
            width: 10px;
            height: 50px;
        }
        #chartdiv5
        {
            width: 10px;
            height: 50px;
        }
        label
        {
            font-size: 35px !important;
            font-weight: 600 !important;
            text-transform: capitalize !important;
            color: white !important;
            line-height: 1.8 !important;
        }
        
        /*change the label effects on quantity */
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
            $('#txt_cowbuffdate').val(yyyy + '-' + mm + '-' + dd);
            var branchid = '<%=Session["Branch_ID"] %>';
            if (branchid == "26") {
                Siloquantity_details();
                generate_vendorwisecowmilk();
                generate_vendorwisebuffalomilk();
                get_cowdetailschart();
                get_buffalodetailschart();
                get_vendorlinechart_details();
            }
            else {
                Siloquantity_details();
                generate_vendorwisecowmilk();
                generate_vendorwisebuffalomilk();
                generate_filimchart();
                generate_returnmilkbarchart();
                get_cowdetailschart();
                get_buffalodetailschart();
                get_vendorlinechart_details();
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
        function get_vendorlinechart_details() {
            var date = document.getElementById("txt_cowbuffdate").value;
            var data = { 'op': 'get_vendorlinechart_details', 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        createlineChart(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var datainXSeries = 0;
        var datainYSeries = 0;
        var newXarray = [];
        var newYarray = [];
        function createlineChart(databind) {
            var newYarray = [];
            var newXarray = [];
            if (databind.length > 0) {
                for (var k = 0; k < databind.length; k++) {
                    var BranchName = [];
                    var vendorname = databind[k].vendorname;
                    var quantity = databind[k].quantity;
                    var Status = databind[k].status;
                    newXarray = vendorname.split(',');
                    for (var i = 0; i < quantity.length; i++) {
                        newYarray.push({ 'data': quantity[i].split(','), 'name': Status[i] });
                    }
                }
            }
            var textname = "Inter Branches Milk Transaction Details";
            $("#interactive").kendoChart({
                title: {
                    text: textname,
                    color: "#006600",
                    font: "bold italic 18px Arial,Helvetica,sans-serif"
                },
                legend: {
                    position: "bottom"
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: newYarray,
                valueAxis: {
                    labels: {
                        format: "{0}"
                    },
                    line: {
                        visible: false
                    },
                    axisCrossingValue: -10
                },
                categoryAxis: {
                    categories: newXarray,
                    //                        categories: [2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: 65
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value #"
                }
            });
        }
        function get_cowdetailschart() {
            var cowbuffdate = document.getElementById("txt_cowbuffdate").value;
            var data = { 'op': 'get_cowdetailschart', 'cowbuffdate': cowbuffdate };
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
        function cowdetail(msg) {
            var Qtykg = msg[0].qtykg;
            var Qtyltr = msg[0].qtyltr;
            var snf = msg[0].snf;
            var fat = msg[0].fat;
            var kgfat = msg[0].kgfat;
            var kgsnf = msg[0].kgsnf;

            document.getElementById('lblcowqtykgs').innerHTML = Qtykg;
            document.getElementById('lblcowqtyltrs').innerHTML = Qtyltr;
            document.getElementById('lblcowAvgfat').innerHTML = fat;
            document.getElementById('lblcowAvgsnf').innerHTML = snf;
            document.getElementById('lblcowkgfat').innerHTML = kgfat;
            document.getElementById('lblcowkgsnf').innerHTML = kgsnf;
        }
        function get_buffalodetailschart() {
            var cowbuffdate = document.getElementById("txt_cowbuffdate").value;
            var data = { 'op': 'get_buffalodetailschart', 'cowbuffdate': cowbuffdate };
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

            document.getElementById('lblbuffkgs').innerHTML = Qtykg;
            document.getElementById('lblbuffltrs').innerHTML = Qtyltr;
            document.getElementById('lblbuffavgfat').innerHTML = fat;
            document.getElementById('lblbuffavgsnf').innerHTML = snf;
            document.getElementById('lblbuffkgfat').innerHTML = kgfat;
            document.getElementById('lblbuffkgsnf').innerHTML = kgsnf;
        }

        function generate_filimchart() {
            var date = document.getElementById("txt_cowbuffdate").value;
            var data = { 'op': 'filimchartdetails', 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        createChart(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var newXarray = [];
        var wastagearray = [];
        function createChart(databind) {
            $("#divfilim").empty();
            newXarray = [];
            var wastage = databind[0].wastage;
            if (wastage == undefined) {

            }
            else {
                for (var i = 0; i < wastage.length; i++) {
                    document.getElementById('lblwastagefilim').innerHTML = wastage[0];
                    document.getElementById('lblcuttingfilim').innerHTML = wastage[1];
                    document.getElementById('lbloverallwastage').innerHTML = wastage[2];
                }
            }
            var Amount = databind[0].Amount;
            var RouteName = databind[0].RouteName;
            if (RouteName != undefined) {
                for (var i = 0; i < RouteName.length; i++) {
                    newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]) });
                }
            }
            $("#divfilim").kendoChart({
                title: {
                    position: "bottom",
                    text: "",
                    color: "#006600",
                    font: "bold italic 18px Arial,Helvetica,sans-serif"
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: "#= category #: #= value#"
                    }
                },
                dataSource: {
                    data: newXarray
                },
                series: [{
                    type: "pie",
                    field: "value",
                    categoryField: "category"
                }],
                seriesColors: ["#3275a8", "#FF7F50", "#A52A2A", "#c71585", "#00FF00"],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        }
        function generate_vendorwisecowmilk() {
            var branchtype = "Inter Branch";
            var milktype = "Cow";
            var status = "Daily";
            var date = document.getElementById("txt_cowbuffdate").value;
            var data = { 'op': 'piechartvalues', 'milktype': milktype, 'status': status, 'branchtype': branchtype, 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        createvendorwisecowmilkChart(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var newXarray = [];
        function createvendorwisecowmilkChart(databind) {
            $("#divdailycowmilk").empty();
            newXarray = [];
            var Amount = databind[0].Amount;
            var RouteName = databind[0].RouteName;
            if (RouteName.length > 0) {
                for (var i = 0; i < RouteName.length; i++) {
                    newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]) });
                }
            }
            $("#divdailycowmilk").kendoChart({
                title: {
                    position: "bottom",
                    text: "Inter Branches Daily Milk Summary",
                    color: "#006600",
                    font: "bold italic 18px Arial,Helvetica,sans-serif"
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: "#= category #: #= value#"
                    }
                },
                dataSource: {
                    data: newXarray
                },
                series: [{
                    type: "pie",
                    field: "value",
                    categoryField: "category"
                }],
                seriesColors: ["#3275a8", "#267ed4", "#068c35", "#808080", "#FFA500", "#A52A2A", "#FF7F50", "#00FF00", "#808000", "#0041C2", "#800517", "#1C1715", "#F1E4C9", "#D82AF1", "#17B2B7"],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        }
        function generate_vendorwisebuffalomilk() {
            var branchtype = "Inter Branch";
            var milktype = "Buffalo";
            var status = "Daily";
            var date = document.getElementById("txt_cowbuffdate").value;
            var data = { 'op': 'piechartvalues', 'milktype': milktype, 'status': status, 'branchtype': branchtype, 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        createvendorwisebuffalomilkChart(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var newXarray = [];
        function createvendorwisebuffalomilkChart(databind) {
            $("#divdailybuffalomilkChart").empty();
            newXarray = [];
            var Amount = databind[0].Amount;
            var RouteName = databind[0].RouteName;
            for (var i = 0; i < RouteName.length; i++) {
                newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]) });
            }
            $("#divdailybuffalomilkChart").kendoChart({
                title: {
                    position: "bottom",
                    text: "Inter Branches Daily Milk Summary",
                    color: "#006600",
                    font: "bold italic 18px Arial,Helvetica,sans-serif"
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: "#= category #: #= value#"
                    }
                },
                dataSource: {
                    data: newXarray
                },
                series: [{
                    type: "pie",
                    field: "value",
                    categoryField: "category"
                }],
                seriesColors: ["#3275a8", "#267ed4", "#068c35", "#808080", "#FFA500", "#A52A2A", "#FF7F50", "#00FF00", "#808000", "#0041C2", "#800517", "#1C1715", "#F1E4C9", "#D82AF1", "#17B2B7"],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        }
        function generate_returnmilkbarchart() {
            var date = document.getElementById("txt_cowbuffdate").value;
            var data = { 'op': 'generate_returnmilkbarchart', 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        createbarChart(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var newXarray = [];
        function createbarChart(databind) {
            $("#divbarchart").empty();
            newXarray = [];
            var quantity = databind[0].quantity;
            var milktype = databind[0].milktype;
            for (var i = 0; i < milktype.length; i++) {
                newXarray.push({ "category": milktype[i], "value": parseFloat(quantity[i]) });
            }
            $("#divbarchart").kendoChart({
                title: {
                    position: "bottom",
                    text: "Return Milk Details",
                    color: "#006600",
                    font: "bold italic 18px Arial,Helvetica,sans-serif"
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: "#= value #"
                    }
                },
                dataSource: {
                    data: newXarray
                },
                series: [{
                    type: "column",
                    field: "value"
                }],
                categoryAxis: {
                    field: "category"
                },
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        }
        function get_cow_vendor_details_chart() {
            var date = document.getElementById("txt_cowbuffdate").value;
            var data = { 'op': 'get_cow_vendor_details_chart', 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $('#divMainAddNewRowCow').css('display', 'none');
                        fillcowvendordetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillcowvendordetails(msg) {
            $('#divMainAddNewRowCow').css('display', 'block');
            var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th scope="col">SNO</th><th scope="col">Vendor Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">KGFAT</th><th scope="col">KGSNF</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td scope="row" class="1" style="text-align:center;">' + (i + 1) + '</td>';
                results += '<td scope="row" class="2" style="text-align:center;">' + msg[i].vendorname + '</td>';
                results += '<td data-title="Capacity" class="totqtykgsclss">' + msg[i].qtykg + '</td>';
                results += '<td data-title="Capacity" class="totlqtyltrsclss">' + msg[i].qtyltr + '</td>';
                results += '<td data-title="Capacity" class="5">' + msg[i].fat + '</td>';
                results += '<td data-title="Capacity" class="6">' + msg[i].snf + '</td>';
                results += '<td data-title="Capacity" class="7">' + msg[i].kgfat + '</td>';
                results += '<td class="8">' + msg[i].kgsnf + '</td></tr>';
            }
            results += '<tr><th scope="row" class="1" style="text-align:center;"></th>';
            results += '<td data-title="brandstatus" class="badge bg-yellow">Total</td>';
            results += '<td data-title="brandstatus" class="3"><span id="totalkgsclss" class="badge bg-yellow"></span></td>';
            results += '<td data-title="brandstatus" class="4"><span id="totalltrsclss" class="badge bg-yellow"></span></td>';
            results += '<td data-title="brandstatus" class="5"></td>';
            results += '<td data-title="brandstatus" class="6"></td>';
            results += '<td data-title="brandstatus" class="7"></td>';
            results += '<td data-title="brandstatus" class="8"></td></tr>';
            results += '</table></div>';
            $("#div_cowvendordetails").html(results);
            GetkgstotalclsCalcow();
            GetltrstotalclsCalcow();
        }
        function GetkgstotalclsCalcow() {
            var totamount = 0;
            $('.totqtykgsclss').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totamount += parseFloat(qtyclass);
                }
            });
            document.getElementById('totalkgsclss').innerHTML = parseFloat(totamount).toFixed(2);
        }
        function GetltrstotalclsCalcow() {
            var totamount = 0;
            $('.totlqtyltrsclss').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totamount += parseFloat(qtyclass);
                }
            });
            document.getElementById('totalltrsclss').innerHTML = parseFloat(totamount).toFixed(2);
        }
        function CloseClick_cowvendor_det_close() {
            $('#divMainAddNewRowCow').css('display', 'none');
        }
        function get_buffalo_vendor_details_chart() {
            var date = document.getElementById("txt_cowbuffdate").value;
            var data = { 'op': 'get_buffalo_vendor_details_chart', 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $("#divMainAddNewRowBuffalo").css('diaplay', 'none');
                        fillbuffalovendordetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillbuffalovendordetails(msg) {
            $('#divMainAddNewRowBuffalo').css('display', 'block');
            var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th scope="col">SNO</th><th scope="col">Vendor Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">KGFAT</th><th scope="col">KGSNF</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td scope="row" class="1" style="text-align:center;">' + (i + 1) + '</td>';
                results += '<td scope="row" class="2" style="text-align:center;">' + msg[i].vendorname + '</td>';
                results += '<td data-title="Capacity" class="totqtykgscls">' + msg[i].qtykg + '</td>';
                results += '<td data-title="Capacity" class="totlqtyltrscls">' + msg[i].qtyltr + '</td>';
                results += '<td data-title="Capacity" class="5">' + msg[i].fat + '</td>';
                results += '<td data-title="Capacity" class="6">' + msg[i].snf + '</td>';
                results += '<td data-title="Capacity" class="7">' + msg[i].kgfat + '</td>';
                results += '<td class="8">' + msg[i].kgsnf + '</td></tr>';
            }
            results += '<tr><th scope="row" class="1" style="text-align:center;"></th>';
            results += '<td data-title="brandstatus" class="badge bg-yellow">Total</td>';
            results += '<td data-title="brandstatus" class="3"><span id="totalkgscls" class="badge bg-yellow"></span></td>';
            results += '<td data-title="brandstatus" class="4"><span id="totalltrscls" class="badge bg-yellow"></span></td>';
            results += '<td data-title="brandstatus" class="5"></td>';
            results += '<td data-title="brandstatus" class="6"></td>';
            results += '<td data-title="brandstatus" class="7"></td>';
            results += '<td data-title="brandstatus" class="8"></td></tr>';
            results += '</table></div>';
            $("#div_buffalovendordetails").html(results);
            GetkgstotalclsCalbuff();
            GetltrstotalclsCalbuff();
        }
        function CloseClick_buffalovendor_det_close() {
            $('#divMainAddNewRowBuffalo').css('display', 'none');
        }
        function GetkgstotalclsCalbuff() {
            var totamount = 0;
            $('.totqtykgscls').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totamount += parseFloat(qtyclass);
                }
            });
            document.getElementById('totalkgscls').innerHTML = parseFloat(totamount).toFixed(2);
        }
        function GetltrstotalclsCalbuff() {
            var totamount = 0;
            $('.totlqtyltrscls').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totamount += parseFloat(qtyclass);
                }
            });
            document.getElementById('totalltrscls').innerHTML = parseFloat(totamount).toFixed(2);
        }

        // Silo Dash Board

        function Siloquantity_details() {
            var data = { 'op': 'Siloquantity_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails(msg);
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
        function filldetails(msg) {
            var newYarray = [];
            for (var i = 0; i < msg.length; i++) {
                var newdiv = document.createElement('div');
                var divid = "chartdiv" + (i + 1);
                newdiv.id = divid;
                newdiv.style.width = "1800px";
                newdiv.style.height = "500px";
                newdiv.style.marginLeft = "-300px";
                // newdiv.style.marginright = "100px";
                newdiv.style.position = "absolute";

                var category = msg[i].SiloName;
                var value1 = msg[i].Quantity;
                var value2 = msg[i].Capacity;
                var value3 = msg[i].DeportmentName;
                newYarray.push({ 'category': category, 'value2': value2, 'value1': value1, 'value3': value3 });
                var chart1 = AmCharts.makeChart(newdiv, {
                    "theme": "light",
                    "type": "serial",
                    "depth3D": 100,
                    "angle": 30,
                    "autoMargins": false,
                    "marginBottom": 100,
                    "marginLeft": 350,
                    "marginRight": 300,
                    "dataProvider": newYarray,
                    "valueAxes": [{
                        "stackType": "100%",
                        "gridAlpha": 0
                    }],
                    "graphs": [{
                        "type": "column",
                        "topRadius": 1,
                        "columnWidth": 1,
                        "showOnAxis": true,
                        "lineThickness": 2,
                        "lineAlpha": 0.5,
                        "lineColor": "#FFFFFF",
                        "fillColors": "#8d003b",
                        "fillAlphas": 0.8,
                        "valueField": "value1"
                    }, {
                        "type": "column",
                        "topRadius": 1,
                        "columnWidth": 1,
                        "showOnAxis": true,
                        "lineThickness": 2,
                        "lineAlpha": 0.5,
                        "lineColor": "#cdcdcd",
                        "fillColors": "#cdcdcd",
                        "fillAlphas": 0.5,
                        "valueField": "value2"
                    }],

                    "categoryField": "category",
                    "categoryAxis": {
                        "axisAlpha": 0,
                        "labelOffset": 40,
                        "gridAlpha": 0
                    },
                    "export": {
                        "enabled": true
                    }
                });
            }
            $('#mychart').append(newdiv);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        $('body').addClass('hold-transition sidebar-mini skin-green-light sidebar-collapse');
    </script>
    <section class="content">
    <div>
        <div class="row">
            <div class="col-xs-12">
                <!-- interactive chart -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-bar-chart-o"></i>
                        <h3 class="box-title">
                            Tanker Inward Details</h3>
                         &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                         <input id="txt_cowbuffdate"  type="date" name="vendorcode" onblur="get_cowdetailschart();get_buffalodetailschart();get_vendorlinechart_details();generate_vendorwisecowmilk();generate_vendorwisebuffalomilk();generate_returnmilkbarchart();generate_filimchart();"/> 
                         
                        <div class="box-tools pull-right">
                        <a href="Dashboard.aspx?Dash Board=DashBoard">Click Here To View InTrasist DashBoard&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</a>
                            <div class="box-tools pull-right">
                                <button class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button class="btn btn-box-tool" data-widget="remove">
                                    <i class="fa fa-times"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    
                    <div class="box-body">
                    <div class="row" >
                        <div class="col-lg-3 col-xs-6" style="width: 10% !important;">
                                <!-- small box -->
                                <img src="images/newimg/cow.png" alt="" height="100px;"/>
                            </div>
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_cow_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-aqua">
                                    <div class="inner">
                                             <h3>
                                             <label id="lblcowqtykgs"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            Qty(Kgs)</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-bag"></i>
                                    </div>
                                    <a href="#" class="small-box-footer" > <i class="fa fa-arrow-circle-right" onclick="get_cow_vendor_details_chart();"  ></i>
                                    </a>
                                </div>
                            </div>

        <!-- ./Cow Popup -->

        <div id="divMainAddNewRowCow" class="pickupclass" style="text-align: center; height: 100%;
                width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                background: rgba(192, 192, 192, 0.7);">
                <div id="div2" style="border: 5px solid #A0A0A0; position: absolute; top: 8%;
                    background-color: White; left: 10%; right: 10%; width: 80%; height: 100%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    border-radius: 10px 10px 10px 10px;">
                    <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                        id="table1" class="mainText3" border="1">
                        <tr>
                            <td colspan="2">
                                <div id="div_cowvendordetails" style="background-color: antiquewhite;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" class="btn btn-danger" id="btn_prod_det_close" value="Close" onclick="CloseClick_cowvendor_det_close();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_vendor_det_close" style="width: 35px; top: 7.5%; right: 10%; position: absolute;
                    z-index: 99999; cursor: pointer;">
                    <img src="images/PopClose.png" onclick="CloseClick_cowvendor_det_close();" />
                </div>
            </div>

                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_cow_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-green">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblcowqtyltrs"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            Qty(ltrs)</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-stats-bars"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right" onclick="get_cow_vendor_details_chart();" ></i>
                                    </a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_cow_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-yellow">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblcowkgfat"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            KgFat</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-person-add"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_cow_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-navy">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblcowkgsnf"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            KgSnf</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_cow_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-teal">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblcowAvgfat"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            AvgFat</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-person-add"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_cow_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-red">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblcowAvgsnf"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            AvgSnf</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                        <div class="col-lg-3 col-xs-6" style="width: 10% !important;">
                                <!-- small box -->
                                <img src="images/newimg/buffalo.png" alt="" height="100px;"/>
                            </div>
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_buffalo_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-red">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblbuffkgs"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            Qty(Kgs)</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-bag"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right" onclick="get_buffalo_vendor_details_chart();"></i>
                                    </a>
                                </div>
                            </div>

         <div id="divMainAddNewRowBuffalo" class="pickupclass" style="text-align: center; height: 100%;
                width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                background: rgba(192, 192, 192, 0.7);">
                <div id="div3" style="border: 5px solid #A0A0A0; position: absolute; top: 8%;
                    background-color: White; left: 10%; right: 10%; width: 80%; height: 100%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    border-radius: 10px 10px 10px 10px;">
                    <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                        id="table2" class="mainText3" border="1">
                        <tr>
                            <td colspan="2">
                                <div id="div_buffalovendordetails">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" class="btn btn-danger" id="Button1" value="Close" onclick="CloseClick_buffalovendor_det_close();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div5" style="width: 35px; top: 7.5%; right: 10%; position: absolute;
                    z-index: 99999; cursor: pointer;">
                    <img src="images/PopClose.png" onclick="CloseClick_buffalovendor_det_close();" />
                </div>
            </div>

                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_buffalo_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-navy">
                                    <div class="inner">
                                        <h3>
                                           <label id="lblbuffltrs"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            Qty(ltrs)</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-stats-bars"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right" onclick="get_buffalo_vendor_details_chart();" ></i>
                                    </a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_buffalo_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-teal">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblbuffkgfat"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            KgFat</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-person-add"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_buffalo_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-yellow">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblbuffkgsnf"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            KgSnf</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_buffalo_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-green">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblbuffavgfat"  style="font-weight: 500;"></label></h3>
                                        <p>
                                           AvgFat</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-person-add"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"> <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="width: 15% !important;" onclick="get_buffalo_vendor_details_chart();">
                                <!-- small box -->
                                <div class="small-box bg-aqua">
                                    <div class="inner">
                                        <h3>
                                            <label id="lblbuffavgsnf"  style="font-weight: 500;"></label></h3>
                                        <p>
                                            AvgSnf</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"><i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                        
                    </div>
                    <!-- /.box-body-->
                </div>
                <!-- /.box -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            Inter Branches Cow Milk</h3>
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
                            <div id="divdailycowmilk">
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
            <div class="col-md-6">
                <!-- LINE CHART -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            Inter Branches Buffalo Milk</h3>
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
                            <div id="divdailybuffalomilkChart">
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
        <div class="row">
            <div class="col-xs-12">
                <!-- interactive chart -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-bar-chart-o"></i>
                        <h3 class="box-title">
                            Inter Branches Milk Details</h3>
                        <div class="box-tools pull-right">
                            <div class="box-tools pull-right">
                                <button class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button class="btn btn-box-tool" data-widget="remove">
                                    <i class="fa fa-times"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div id="interactive" style="height: 300px;">
                        </div>
                    </div>
                    <!-- /.box-body-->
                </div>
                <!-- /.box -->
            </div>
        </div>

         <!-- Silo Dash Board -->
         <div class="row">
            <div class="col-md-6" style="width: 100%;">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i class="fa fa-cog fa-spin fa-1x fa-fw"></i>Silo Wise Milk Details</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div style="width: 100%; position: relative;">
                        <div id="mychart">
                        </div>
                    </div>
                </div>
            </div>
        </div>
         <!--- Silo Dash Board End  --->

        <div class="row" style="margin-top: 550px;">
            <div class="col-md-6">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            Film Details</h3>
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
                            <div id="divfilim">
                            </div>
                            Total Wastage Film(%) :
                            <label id="lblwastagefilim" style="font-size: 15px !important;color: black !important;">
                            </label>
                            <br />
                            Total Cutting Film(%) :
                            <label id="lblcuttingfilim" style="font-size: 15px !important;color: black !important;">
                            </label>
                            <br />
                            Over All Film Watsge(%) :
                            <label id="lbloverallwastage" style="font-size: 15px !important;color: black !important;">
                            </label>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-success">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            Return Milk Details</h3>
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
                            <div id="divbarchart">
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
            </div>
        </div>
       </div>
    </section>
</asp:Content>
