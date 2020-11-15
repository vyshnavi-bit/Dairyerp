<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Inward_dc_varify.aspx.cs"
    Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <link rel="icon" href="images/vyshnavilogo.png" type="image/x-icon" title="Dairy ERP" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>Dairy ERP</title>
    <meta charset="UTF-8" />
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/font-awesome.min.css" rel="stylesheet">
    <link href="css/animate.css" rel="stylesheet">
    <link href="css/main.css" rel="stylesheet">
    <script src="js/jquery.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="JSF/jquery.min.js"></script>
    <script src="JSF/jquery-ui.js" type="text/javascript"></script>
    <link href="JSF/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="JSF/jquery.blockUI.js" type="text/javascript"></script>
    <link href="css/formstable.css" rel="stylesheet" type="text/css" />
    <link href="css/custom.css" rel="stylesheet" type="text/css" />
    <link href="css/skel.css" rel="stylesheet" type="text/css" />
    <link href="css/fleetStyles.css" rel="stylesheet" type="text/css" />
    <script src="bootstrap/js/Barscanner.js" type="text/javascript"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <script type="text/javascript">
        $(function () {
            var myVar = setInterval(function () { checkInternet() }, 2000);
        });
    </script>
    <script type="text/javascript">
        function checkInternet() {
            var online = window.navigator.onLine;
        }
    </script>
    <script type="text/javascript">
        $(function () {
            document.getElementById('item').value = "";
            $("#item").focus();
        });
        function GetDespatchDetails_click() {
            var refdcno = document.getElementById('item').value;
            if (refdcno == "") {
                alert("Enter Ref DC No");
                document.getElementById('item').value = "";
                return false;
            }
            var data = { 'op': 'GetDespatchDetails_click', 'refdcno': refdcno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "No dc were found") {
                            document.getElementById('item').value = "";
                            alert(msg);
                            return false;
                        }
                        if (msg == "Dc alredy verified") {
                            document.getElementById('item').value = "";
                            alert(msg);
                            return false;

                        }
                        var milkdetails = msg[0].MilkDetailslst;
                        document.getElementById('spndcno').innerHTML = milkdetails[0].dcno;
                        document.getElementById('spnccname').innerHTML = milkdetails[0].plantname;
                        document.getElementById('spndate').innerHTML = milkdetails[0].dispdate;
                        document.getElementById('spnvehicleno').innerHTML = milkdetails[0].vehicleno;
                        document.getElementById('spnchemist').innerHTML = milkdetails[0].chemist;
                        fillDespatchDetails(msg);
                        var vsno = milkdetails[0].vsno;
                        var sno = milkdetails[0].sno;
                        var bcode = vsno + "/" + sno;
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
        function fillDespatchDetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="responsive-table">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Milk Type</th><th scope="col">Cell Name</th><th scope="col">Qty Kgs</th><th scope="col">Qty ltrs</th><th scope="col">Fat</th><th scope="col">Snf</th><th scope="col">CLR</th><th scope="col">COB</th><th scope="col">HS</th><th scope="col">Phosps</th><th scope="col">Alcohol</th><th scope="col">TEMP</th></tr></thead></tbody>';
            var MilkfatDetails = msg[0].MilkfatDetailslst;
            var j = 1;
            for (var i = 0; i < MilkfatDetails.length; i++) {
                results += '<tr><td>' + j + '</td>';
                results += '<td data-title="Vendor Code" class="1">' + MilkfatDetails[i].milktype + '</td>';
                results += '<th scope="row" class="2" style="text-align:center;">' + MilkfatDetails[i].CellName + '</th>';
                results += '<td data-title="Mobno" class="3">' + MilkfatDetails[i].Qtykg + '</td>';
                results += '<td data-title="Email" class="4">' + MilkfatDetails[i].Qtyltr + '</td>';
                results += '<td data-title="Pan" class="5">' + MilkfatDetails[i].fat + '</td>';
                results += '<td data-title="Address" class="6">' + MilkfatDetails[i].snf + '</td>';
                results += '<td data-title="Address" class="6">' + MilkfatDetails[i].clr + '</td>';
                results += '<td data-title="Address" class="6">' + MilkfatDetails[i].cob + '</td>';
                results += '<td data-title="Address" class="6">' + MilkfatDetails[i].hs + '</td>';
                results += '<td data-title="Address" class="6">' + MilkfatDetails[i].phosps + '</td>';
                results += '<td data-title="Address" class="6">' + MilkfatDetails[i].alcohol + '</td>';
                results += '<td data-title="Address" class="7">' + MilkfatDetails[i].temp + '</td></tr>';
                var vsno = MilkfatDetails[i].vsno;
                var sno = MilkfatDetails[i].sno;
                var CellName = MilkfatDetails[i].CellName;
                if (CellName == "F Cell") {
                    CellName = "F1";
                }
                if (CellName == "M Cell") {
                    CellName = "M2";
                }
                if (CellName == "B Cell") {
                    CellName = "B3";
                }
                var bcode = vsno + "/" + sno + "/" + CellName;
                if (j == 1) {
                    generateBarcode(bcode);
                }
                if (j == 2) {
                    generateBarcode1(bcode);
                }
                if (j == 3) {
                    generateBarcode2(bcode);
                }
                j++;
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
        }
        function Get_Varify_DespatchDetails_click() {
            var refdcno = document.getElementById('item').value;
            if (refdcno == "") {
                alert("Enter Ref DC No");
                return false;
            }
            var data = { 'op': 'Get_Varify_DespatchDetails_click', 'refdcno': refdcno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('item').value = "";
                    document.getElementById('spndcno').innerHTML = "";
                    document.getElementById('spnccname').innerHTML = "";
                    document.getElementById('spndate').innerHTML = "";
                    document.getElementById('spnvehicleno').innerHTML = "";
                    document.getElementById('spnchemist').innerHTML = "";
                    clearvalues();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function clearvalues() {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Milk Type</th><th scope="col">Cell Name</th><th scope="col">Qty Kgs</th><th scope="col">Qty ltrs</th><th scope="col">Fat</th><th scope="col">Snf</th><th scope="col">CLR</th><th scope="col">COB</th><th scope="col">HS</th><th scope="col">Phosps</th><th scope="col">Alcohol</th><th scope="col">TEMP</th></tr></thead></tbody>';
            var MilkfatDetails = [];
            var j = 1;
            for (var i = 0; i < MilkfatDetails.length; i++) {
                results += '<tr><td>' + j + '</td>';
                results += '<td data-title="Vendor Code" class="1">' + MilkfatDetails[i].milktype + '</td>';
                results += '<th scope="row" class="2" style="text-align:center;">' + MilkfatDetails[i].CellName + '</th>';
                results += '<td data-title="Mobno" class="3">' + MilkfatDetails[i].Qtykg + '</td>';
                results += '<td data-title="Email" class="4">' + MilkfatDetails[i].Qtyltr + '</td>';
                results += '<td data-title="Pan" class="5">' + MilkfatDetails[i].fat + '</td>';
                results += '<td data-title="Address" class="6">' + MilkfatDetails[i].snf + '</td>';
                results += '<td data-title="Address" class="7">' + MilkfatDetails[i].clr + '</td>';
                results += '<td data-title="Address" class="8">' + MilkfatDetails[i].cob + '</td>';
                results += '<td data-title="Address" class="9">' + MilkfatDetails[i].hs + '</td>';
                results += '<td data-title="Address" class="10">' + MilkfatDetails[i].phosps + '</td>';
                results += '<td data-title="Address" class="11">' + MilkfatDetails[i].alcohol + '</td>';
                results += '<td data-title="Address" class="12">' + MilkfatDetails[i].temp + '</td></tr>';
                j++;
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
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
    </script>
    <link href="http://www.jqueryscript.net/css/jquerysctipttop.css" rel="stylesheet"
        type="text/css">
    <script src="http://code.jquery.com/jquery-latest.min.js"></script>
    <script src="Barcode/jquery-barcode.js" type="text/javascript"></script>
    <script type="text/javascript">
        function generateBarcode(bcode) {
            var Beforevalue = "SVDS/" + bcode + "/";
            var value = Beforevalue;
            var btype = $("input[name=btype]:checked").val();
            var renderer = $("input[name=renderer]:checked").val();
            var quietZone = false;
            if ($("#quietzone").is(':checked') || $("#quietzone").attr('checked')) {
                quietZone = true;
            }
            var settings = {
                output: renderer,
                bgColor: $("#bgColor").val(),
                color: $("#color").val(),
                barWidth: $("#barWidth").val(),
                barHeight: $("#barHeight").val(),
                moduleSize: $("#moduleSize").val(),
                posX: $("#posX").val(),
                posY: $("#posY").val(),
                addQuietZone: $("#quietZoneSize").val()
            };
            if ($("#rectangular").is(':checked') || $("#rectangular").attr('checked')) {
                value = { code: value, rect: true };
            }
            if (renderer == 'canvas') {
                clearCanvas();
                $("#barcodeTarget").hide();
                $("#canvasTarget").show().barcode(value, btype, settings);
            } else {
                $("#canvasTarget").hide();
                $("#barcodeTarget").html("").show().barcode(value, btype, settings);
            }
        }

        function showConfig1D() {
            $('.config .barcode1D').show();
            $('.config .barcode2D').hide();
        }

        function showConfig2D() {
            $('.config .barcode1D').hide();
            $('.config .barcode2D').show();
        }

        function clearCanvas() {
            var canvas = $('#canvasTarget').get(0);
            var ctx = canvas.getContext('2d');
            ctx.lineWidth = 1;
            ctx.lineCap = 'butt';
            ctx.fillStyle = '#FFFFFF';
            ctx.strokeStyle = '#000000';
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.strokeRect(0, 0, canvas.width, canvas.height);
        }

        $(function () {
            $('input[name=btype]').click(function () {
                if ($(this).attr('id') == 'datamatrix') showConfig2D(); else showConfig1D();
            });
            $('input[name=renderer]').click(function () {
                if ($(this).attr('id') == 'canvas') $('#miscCanvas').show(); else $('#miscCanvas').hide();
            });
            //        generateBarcode();
        });
    </script>

    <script type="text/javascript">
        function generateBarcode1(bcode) {
            var Beforevalue = "SVDS/" + bcode + "/";
            var value = Beforevalue;
            var btype = $("input[name=ctype]:checked").val();
            var renderer = $("input[name=renderer]:checked").val();
            var quietZone = false;
            if ($("#quietzone").is(':checked') || $("#quietzone").attr('checked')) {
                quietZone = true;
            }
            var settings = {
                output: renderer,
                bgColor: $("#bgColor").val(),
                color: $("#color").val(),
                barWidth: $("#barWidth").val(),
                barHeight: $("#barHeight").val(),
                moduleSize: $("#moduleSize").val(),
                posX: $("#posX").val(),
                posY: $("#posY").val(),
                addQuietZone: $("#quietZoneSize").val()
            };
            if ($("#rectangular").is(':checked') || $("#rectangular").attr('checked')) {
                value = { code: value, rect: true };
            }
            if (renderer == 'canvas') {
                clearCanvas();
                $("#barcodeTarget3").hide();
                $("#canvasTarget3").show().barcode(value, btype, settings);
            } else {
                $("#canvasTarget3").hide();
                $("#barcodeTarget3").html("").show().barcode(value, btype, settings);
            }
        }

        function showConfig1D() {
            $('.config .barcode1D').show();
            $('.config .barcode2D').hide();
        }

        function showConfig2D() {
            $('.config .barcode1D').hide();
            $('.config .barcode2D').show();
        }

        function clearCanvas() {
            var canvas = $('#canvasTarget3').get(0);
            var ctx = canvas.getContext('2d');
            ctx.lineWidth = 1;
            ctx.lineCap = 'butt';
            ctx.fillStyle = '#FFFFFF';
            ctx.strokeStyle = '#000000';
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.strokeRect(0, 0, canvas.width, canvas.height);
        }

        $(function () {
            $('input[name=ctype]').click(function () {
                if ($(this).attr('id') == 'datamatrix') showConfig2D(); else showConfig1D();
            });
            $('input[name=renderer]').click(function () {
                if ($(this).attr('id') == 'canvas') $('#miscCanvas').show(); else $('#miscCanvas').hide();
            });
            //        generateBarcode();
        });
    </script>

    <script type="text/javascript">
        function generateBarcode2(bcode) {
            var Beforevalue = "SVDS/" + bcode + "/";
            var value = Beforevalue;
            var btype = $("input[name=mtype]:checked").val();
            var renderer = $("input[name=renderer]:checked").val();
            var quietZone = false;
            if ($("#quietzone").is(':checked') || $("#quietzone").attr('checked')) {
                quietZone = true;
            }
            var settings = {
                output: renderer,
                bgColor: $("#bgColor").val(),
                color: $("#color").val(),
                barWidth: $("#barWidth").val(),
                barHeight: $("#barHeight").val(),
                moduleSize: $("#moduleSize").val(),
                posX: $("#posX").val(),
                posY: $("#posY").val(),
                addQuietZone: $("#quietZoneSize").val()
            };
            if ($("#rectangular").is(':checked') || $("#rectangular").attr('checked')) {
                value = { code: value, rect: true };
            }
            if (renderer == 'canvas') {
                clearCanvas();
                $("#barcodeTarget2").hide();
                $("#canvasTarget2").show().barcode(value, btype, settings);
            } else {
                $("#canvasTarget2").hide();
                $("#barcodeTarget2").html("").show().barcode(value, btype, settings);
            }
        }

        function showConfig1D() {
            $('.config .barcode1D').show();
            $('.config .barcode2D').hide();
        }

        function showConfig2D() {
            $('.config .barcode1D').hide();
            $('.config .barcode2D').show();
        }

        function clearCanvas() {
            var canvas = $('#canvasTarget2').get(0);
            var ctx = canvas.getContext('2d');
            ctx.lineWidth = 1;
            ctx.lineCap = 'butt';
            ctx.fillStyle = '#FFFFFF';
            ctx.strokeStyle = '#000000';
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.strokeRect(0, 0, canvas.width, canvas.height);
        }

        $(function () {
            $('input[name=mtype]').click(function () {
                if ($(this).attr('id') == 'datamatrix') showConfig2D(); else showConfig1D();
            });
            $('input[name=renderer]').click(function () {
                if ($(this).attr('id') == 'canvas') $('#miscCanvas').show(); else $('#miscCanvas').hide();
            });
            //        generateBarcode();
        });
    </script>
</head>
<body data-color="grey" class="flat">
<input type="radio" name="btype" id="Radio1" checked="checked" value="code128" style="display:none;">
<input type="radio" name="mtype" id="Radio2" checked="checked" value="code128" style="display:none;">
<input type="radio" name="ctype" id="Radio3" checked="checked" value="code128" style="display:none;">
    <div id="wrapper" class="minibar">
    </div>
    <div id="register_container" class="sales clearfix">
        <div id="content-header" class="hidden-print sales_header_container">
            <h1 class="headigs">
                <span id="ajax-loader"></span>
            </h1>
        </div>
        <div class="clear">
        </div>
        <!--Left small box-->
        <section class="content-header">
            <h1 style="background-color: aliceblue; text-align: center;">
                Tanker Inward DC Verify <small>Preview</small>
            </h1>
            <div style="width: 100%;">
                <div style="width: 50%; float: left;padding-left:5%;">
                    <a href="samplesentry.aspx">Sampleno Entry</a>
                </div>
                <div style="width: 50%; float: right;text-align: right;padding-right:5%;">
                    <a href="LogOut.aspx">Log Out</a>
                </div>
            </div>
        </section>
        <section class="content">
            <div id='vehmaster_fillform' style="border: 1px solid #d5d5d5; margin-left: 18px;
                margin-top: 10px; margin-right: 5px; height: 55px;">
                <table align="center">
                    <tr>
                        <td style="padding-top: 12px;">
                            <input type="text" name="item" value="" id="item" class="input-xlarge" style="width: 111%;"
                                onchange="GetDespatchDetails_click();" accesskey="k" placeholder="Enter DC No or scan barcode"
                                autocomplete="off" />
                        </td>
                    </tr>
                </table>
                <!-- Right small box  -->
                <div style="padding-top: 30px;">
                    <table align="center" style="width: 50%;">
                        <tr>
                            <td>
                                Dc No:
                            </td>
                            <td>
                                <span id="spndcno" style="font-size: 14px; color: Red; font-weight: bold;"></span>
                            </td>
                            <td>
                                Vendor Name:
                            </td>
                            <td>
                                <span id="spnccname" style="font-size: 14px; color: Red; font-weight: bold;"></span>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr>
                            <td>
                                Date:
                            </td>
                            <td>
                                <span id="spndate" style="font-size: 14px; color: Red; font-weight: bold;"></span>
                            </td>
                            <td>
                                Vehicle No:
                            </td>
                            <td>
                                <span id="spnvehicleno" style="font-size: 14px; color: Red; font-weight: bold;">
                                </span>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr>
                            <td>
                                Chemist:
                            </td>
                            <td>
                                <span id="spnchemist" style="font-size: 14px; color: Red; font-weight: bold;"></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_vendordata">
                </div>
                <div>
                    <table align="center">
                        <tr>
                            <td>
                                <input id='Button1' type="button" class="btn btn-success" name="submit" value='Verify'
                                    onclick="Get_Varify_DespatchDetails_click()" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table align="center">
                        <tr>
                            <td>
                                <div style="width: 30%; float: right; padding-top:9px;">
                                 <div id="barcodeTarget" class="barcodeTarget" style="font-size:14px !important;">
                                 </div>
                                <canvas id="canvasTarget" width="150" height="150">
                                </canvas>
                        <br />
                    </div>
                            </td>
                     
                        <td>
                                <div style="width: 30%; float: right; padding-top:9px;">
                                 <div id="barcodeTarget2" class="barcodeTarget" style="font-size:14px !important;">
                                 </div>
                                <canvas id="canvasTarget2" width="150" height="150">
                                </canvas>
                        <br />
                    </div>
                            </td>
                        
                        <td>
                                <div style="width: 30%; float: right; padding-top:9px;">
                                 <div id="barcodeTarget3" class="barcodeTarget" style="font-size:14px !important;">
                                 </div>
                                <canvas id="canvasTarget3" width="150" height="150">
                                </canvas>
                        <br />
                    </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="row ">
                    <script type="text/javascript" language="javascript">
                        $(document).keydown(function (event) {
                            var mycode = event.keyCode;
                            if (mycode == 113) {
                                document.getElementById('item').value = "";
                                $("#item").focus();
                            }
                        });
                    </script>
                </div>
            </div>
        </section>
    </div>
</body>
</html>
