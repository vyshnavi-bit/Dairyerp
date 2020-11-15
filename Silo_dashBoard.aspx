<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Silo_dashBoard.aspx.cs" Inherits="Silo_dashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    </style>
    <script type="text/javascript">
        $(function () {
            Siloquantity_details();
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
    <section class="content">
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
    </section>
</asp:Content>
