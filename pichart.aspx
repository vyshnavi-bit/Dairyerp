<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="pichart.aspx.cs" Inherits="pichart" %>

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

        function generate_branchwiselinechart() {
            var branchtype = document.getElementById('slct_branchtype').value;
            var milktype = document.getElementById('slct_milk_type').value;
            var status = document.getElementById('slct_status').value;
            var date = document.getElementById('txt_fromdate').value;
            var data = { 'op': 'piechartvalues', 'milktype': milktype, 'status': status, 'branchtype': branchtype, 'date': date };
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
        function createChart(databind) {
            $("#chart").empty();
            newXarray = [];
            var Amount = databind[0].Amount;
            var RouteName = databind[0].RouteName;
            for (var i = 0; i < RouteName.length; i++) {
                newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]) });
            }
            $("#chart").kendoChart({
                title: {
                    position: "bottom",
                    text: "Vendor Wise Summary Report",
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<section class="content-header">
        <h1>
          <i class="fa fa-pie-chart" aria-hidden="true"></i>Vendor Wise Milk Summary<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Chart Reports</a></li>
            <li><a href="#">Vendor Wise Milk Summary Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-body">
                <div id='Inwardsilo_fillform'>
                    <table>
                        <tr>
                         <td>
                                <label>
                                    MILK Type<span style="color: red;">*</span></label>
                                <select id="slct_branchtype" class="form-control">
                                   <option selected disabled value="Select Batch Type">Select Batch Type</option>
                                    <option>Inter Branch</option>
                                    <option>Other Branch</option>
                                </select>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <label>
                                    MILK Type<span style="color: red;">*</span></label>
                                <select id="slct_milk_type" class="form-control">
                                    <option selected disabled value="Select Batch Type">Select Milk Type</option>
                                    <option>Cow</option>
                                    <option>Buffalo</option>
                                </select>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <label>
                                    Status<span style="color: red;">*</span></label>
                                <select id="slct_status" class="form-control">
                                    <option>Daily</option>
                                    <option>Monthly</option>
                                </select>
                            </td>

                            <td style="width:10px;"></td>
                            <td>
                                <input id='btn_generate' type="button" class="btn btn-success" name="submit" value='Genarate'
                                    onclick="generate_branchwiselinechart()" />
                            </td>
                            <td hidden>
                             <input id="txt_fromdate" class="form-control" type="date" placeholder="Enter fromDate" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
            <div id="example" class="k-content absConf">
                <div class="chart-wrapper" style="margin: auto;">
                    <div id="chart">
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

