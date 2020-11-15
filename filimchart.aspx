<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="filimchart.aspx.cs" Inherits="filimchart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="JSF/jquery.blockUI.js" type="text/javascript"></script>
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
            $('#txt_fromdate').val(yyyy + '-' + mm + '-' + dd);
            $('#txt_todate').val(yyyy + '-' + mm + '-' + dd);
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

        function generate_filimchart() {
            var fromdate = document.getElementById('txt_fromdate').value;
            var todate = document.getElementById('txt_todate').value;
            var data = { 'op': 'filimchartdetails', 'date': fromdate };
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
            $("#chart").empty();
            newXarray = [];
            var wastage = databind[0].wastage;
            for (var i = 0; i < wastage.length; i++) {
                document.getElementById('lblwastagefilim').innerHTML = wastage[0];
                document.getElementById('lblcuttingfilim').innerHTML = wastage[1];
                document.getElementById('lbloverallwastage').innerHTML = wastage[2];
            }
            var Amount = databind[0].Amount;
            var RouteName = databind[0].RouteName;
            for (var i = 0; i < RouteName.length; i++) {
                newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]) });
            }
            $("#chart").kendoChart({
                title: {
                    position: "bottom",
                    text: "Filim Consumption Report",
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            <i class="fa fa-pie-chart" aria-hidden="true"></i>Film Summary<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Chart Reports</a></li>
            <li><a href="#">Film Summary Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-body">
                <div id='Inwardsilo_fillform'>
                    <table>
                        <tr>
                            <td style="display:none;">
                                <label>
                                    From Date<span style="color: red;">*</span></label>
                                <input id="txt_fromdate" class="form-control" type="date" name="vendorcode" placeholder="Enter fromDate">
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                     Date<span style="color: red;">*</span></label>
                                <input id="txt_todate" class="form-control" type="date" name="vendorcode" placeholder="Enter toDate">
                            </td>
                            <td style="width: 10px;">
                            </td>
                            <td>
                                <input id='btn_generate' type="button" class="btn btn-success" name="submit" value='Genarate'
                                    onclick="generate_filimchart()" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
            <div id="example" class="k-content absConf">
                <div class="chart-wrapper" style="margin: auto;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 70%;">
                                <div id="chart">
                                </div>
                            </td>
                            <td>
                                Total Wastage Film(%) :
                                <label id="lblwastagefilim">
                                </label>
                                <br />
                                Total Cutting Film(%) :
                                <label id="lblcuttingfilim">
                                </label>
                                <br />
                                Over All Film Watsge(%) :
                                <label id="lbloverallwastage">
                                </label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
