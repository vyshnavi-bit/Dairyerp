<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="returnmilkchart.aspx.cs" Inherits="returnmilkchart" %>

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
            $('#txt_frmdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#txt_todate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
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

        function generate_returnmilkchart() {
            var milktype = document.getElementById('slct_milktype').value;
            var fromdate = document.getElementById('txt_frmdate').value;
            var todate = document.getElementById('txt_todate').value;
            var data = { 'op': 'generate_returnmilkchart', 'milktype': milktype, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        createChart(msg);
                    }
                    else {
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
                var aqty = databind[0].quantity;
                var splitqty = aqty[1].split(',');
                var avgqty = splitqty[0];
                for (var i = 0; i < DeliveryQty.length; i++) {
                    newYarray.push({ 'data': DeliveryQty[i].split(','), 'name': Status[i] });
                }
            }
            //                var BranchName = [];
            //                var InwardDate = databind[0].Date;
            //                var inwardQty = databind[0].quantity;
            //                var Status = databind[0].Status;
            //                                newXarray = InwardDate.split(',');
            //                                for (var i = 0; i < inwardQty.length; i++) {
            //                                    newYarray.push({ 'data': inwardQty[i].split(','), 'name': Status[i] });
            //                                }
            var agent = document.getElementById("slct_milktype");
            var agentname = agent.options[agent.selectedIndex].text;
            var textname = agentname + " vs Avg " + agentname + " --> " + avgqty;
            $("#chart").kendoChart({
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<section class="content-header">
        <h1>
            <i class="fa fa-line-chart" aria-hidden="true"></i>Return Milk Summary<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Chart Reports</a></li>
            <li><a href="#">Return Milk Summary Details</a></li>
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
                                    Type<span style="color: red;">*</span></label>
                                <select id="slct_milktype" class="form-control">
                                   <option selected disabled value="Select Batch Type">Select Milk Type</option>
                                    <option>Return Milk</option>
                                    <option>Mixed Milk</option>
                                    <option>Cutting Milk</option>
                                    <option>Condensed milk</option>
                                </select>
                            </td>
                            <td style="width:10px;"></td>
                           <td>
                                <label>
                                  From Date<span style="color: red;">*</span></label>
                                <input id="txt_frmdate" class="form-control" type="datetime-local" name="vendorcode"
                                placeholder="Enter Date">
                            </td>
                            <td style="width:10px;"></td>
                           <td>
                                <label>
                                   To Date<span style="color: red;">*</span></label>
                                <input id="txt_todate" class="form-control" type="datetime-local" name="vendorcode"
                                placeholder="Enter Date">
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <input id='btn_generate' type="button" class="btn btn-success" name="submit" value='Genarate'
                                    onclick="generate_returnmilkchart()" />
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

