<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="productwiseproduction.aspx.cs" Inherits="productwiseproduction" %>

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

        function get_product_details() {
            var data = { 'op': 'get_product_details' };
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
            var departmentid = document.getElementById('slct_dept_type').value;
            var data = document.getElementById('slct_product');
            var length = data.options.length;
            document.getElementById('slct_product').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select branch";
            opt.value = "Select branch";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (departmentid == msg[i].departmentid) {
                    if (msg[i].productname != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].productname;
                        option.value = msg[i].productid;
                        data.appendChild(option);
                    }
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
        function generate_linechart() {
            var deptid = document.getElementById('slct_dept_type').value;
            var productid = document.getElementById('slct_product').value;
            var fromdate = document.getElementById('txt_fromdate').value;
            var todate = document.getElementById('txt_todate').value;
            var data = { 'op': 'get_productwiseproductiondetails', 'productid': productid, 'fromdate': fromdate, 'todate': todate, 'deptid': deptid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg.length != "433") {
                            $('#chart').css('display', 'block');
                            createChart(msg);
                        }
                        else {
                            alert("There Is No Production");
                            $('#chart').css('display', 'none');

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
                for (var i = 0; i < DeliveryQty.length; i++) {
                    newYarray.push({ 'data': DeliveryQty[i].split(','), 'name': Status[i] });
                }
            }

            var agent = document.getElementById("slct_product");
            var agentname = agent.options[agent.selectedIndex].text;
            var textname = agentname + " Production";
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
           <i class="fa fa-line-chart" aria-hidden="true"></i> Product Production<small>Details</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">Product Production Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-body">
                <div id='productionfillform'>
                    <table>
                        <tr>
                            <%--<td>
                                <label>
                                    Department<span style="color: red;">*</span></label>
                                <select id="slct_dept_type" class="form-control" onchange="batchtype_change();">
                                    
                                </select>
                                </td>--%>
                            <td>
                                <label>
                                    Department<span style="color: red;">*</span></label>
                                <select id="slct_dept_type" class="form-control" onchange="get_product_details()">
                                    <option selected disabled value="Select Department">Select Department</option>
                                    <option value="1">CurdBlock</option>
                                    <option value="2">Packing</option>
                                    <option value="3">Ghee</option>
                                </select>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Product Name<span style="color: red;">*</span></label>
                                <select id="slct_product" class="form-control">
                                </select>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    From Date<span style="color: red;">*</span></label>
                                <input id="txt_fromdate" class="form-control" type="date" placeholder="Enter fromDate">
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    To Date<span style="color: red;">*</span></label>
                                <input id="txt_todate" class="form-control" type="date" placeholder="Enter toDate">
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <input id='btn_generate' type="button" class="btn-success" name="submit" value='Genarate'
                                    onclick="generate_linechart()" />
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
