<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="branchsummarychart.aspx.cs" Inherits="branchsummarychart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="JSF/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

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
            var milktype = document.getElementById('slct_milk_type').value;
            var status = document.getElementById('slct_status').value;
            var data = { 'op': 'generate_branchwiselinechart', 'milktype': milktype, 'status': status };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        createChart(msg);
                      //  createpieChart(msg);
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
        function createChart(databind) {
            var newYarray = [];
            var newXarray = [];
            for (var k = 0; k < databind.length; k++) {
                var BranchName = [];
                var IndentDate = databind[k].Date;
                var DeliveryQty = databind[k].quantity;
                var Status = databind[k].status;
                newXarray = IndentDate.split(',');
                for (var i = 0; i < DeliveryQty.length; i++) {
                    newYarray.push({ 'data': DeliveryQty[i].split(','), 'name': Status[i] });
                }
            }
            $("#chart").kendoChart({
                title: {
                    text: "Vendor wise summary report",
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<section class="content-header">
        <h1>
           <i class="fa fa-line-chart" aria-hidden="true"></i> Vendor Wise Summary<small>Details</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">Vendor Wise Summary Details</a></li>
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

            <div id="Div1" class="k-content absConf">
                <div class="chart-wrapper" style="margin: auto;">
                    <div id="piechart">
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
