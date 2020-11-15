<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="creamsaparation.aspx.cs" Inherits="creamsaparation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            //level type show edit opction
            var leveltype = '<%= Session["leveltype"] %>';
            if (leveltype == "Admin" || leveltype == "MAdmin") {
                $('#div_creamdata').show();
            }
            else {
                $('#div_creamdata').hide();
            }
            $('#btn_addDept').click(function () {
                get_productqty_details();
                $('#cream_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_creamdata').hide();
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
                $('#txt_getdatadate').val(yyyy + '-' + mm + '-' + dd);
            });
            $('#btn_close').click(function () {
                $('#cream_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                //level type show edit opction
                var leveltype = '<%= Session["leveltype"] %>';
                if (leveltype == "Admin" || leveltype == "MAdmin") {
                    $('#div_creamdata').show();
                }
                else {
                    $('#div_creamdata').hide();
                }
            });
            get_creamsaparation_details();
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
            $('#txt_getdatadate').val(yyyy + '-' + mm + '-' + dd);
        });
        function addcreamsaparation() {
            get_productqty_details();
            $('#cream_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_creamdata').hide();
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
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd);
            $('#txt_getdatadate').val(yyyy + '-' + mm + '-' + dd);
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
            document.getElementById('txt_date').value = "";
            document.getElementById('ddlcreamtype').value = "";
            document.getElementById('txt_butterfat').value = "";
            document.getElementById('txt_recivemilk').value = "";
            document.getElementById('txt_creamquantity').value = "";

            document.getElementById('txt_recivefat').value = "";
            document.getElementById('txt_skimmilkqty').value = "";
            document.getElementById('txtproductionqty').value = "";
            document.getElementById('txt_productionfat').value = "";
            document.getElementById('txt_totalcreamproductionfat').value = "";
            document.getElementById('txt_skimmilkfat').value = "";
            document.getElementById('txt_skimmilkfat').value = "";

            document.getElementById('txt_totalskimmilkfat').value = "";
            document.getElementById('txtdifference').value = "";

            document.getElementById('txt_Remarks').value = "";
            document.getElementById('lbl_sno').innerHTML = "";
            document.getElementById('save_batchdetails').innerHTML = "Save";
            $('#cream_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            //level type show edit opction
            var leveltype = '<%= Session["leveltype"] %>';
            if (leveltype == "Admin" || leveltype == "MAdmin") {
                $('#div_creamdata').show();
            }
            else {
                $('#div_creamdata').hide();
            }
        }
        function save_creamsaparationdetails_click() {
            var doe = document.getElementById('txt_date').value;
            var opcreamqty = document.getElementById('txt_opcreamqty').value;
            var opcreamfat = document.getElementById('txt_opcreamfat').value;
            if (opcreamfat == "") {
                alert("Enter OB Cream FAT");
                $("#txt_opcreamfat").focus();
                return false;
            }
            var section = document.getElementById('ddlcreamtype').value;
            var butterfat = document.getElementById('txt_butterfat').value;
            var milkreciveqty = document.getElementById('txt_recivemilk').value;
            var milkrecivefat = document.getElementById('txt_recivefat').value;
            var creamqty = document.getElementById('txt_creamquantity').value;
            var skimmilkqty = document.getElementById('txt_skimmilkqty').value;
            var skimmilkfat = document.getElementById('txt_skimmilkfat').value;

            var productionqty = document.getElementById('txtproductionqty').value;
            var productionfat = document.getElementById('txt_productionfat').value;
            var totalcreamproductionfat = document.getElementById('txt_totalcreamproductionfat').value;
            var totalskimmilkfat = document.getElementById('txt_totalskimmilkfat').value;

            var remarks = document.getElementById('txt_Remarks').value;
            var btnval = document.getElementById('save_batchdetails').innerHTML;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var flag = false;
            if (milkreciveqty == "") {
                alert("Enter Milk Receive Qty");
                $("#txt_recivemilk").focus();
                return false;
            }
            if (milkrecivefat == "") {
                alert("Enter Milk Receive Fat");
                $("#txt_recivefat").focus();
                return false;
            }

            if (skimmilkqty == "") {
                alert("Enter Skim Milk Qty");
                $("#txt_skimmilkqty").focus();
                return false;
            }
            if (skimmilkfat == "") {
                alert("Enter Skim Milk fAT");
                $("#txt_skimmilkfat").focus();
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_creamsaparation_click', 'doe': doe, 'milkreciveqty': milkreciveqty, 'milkrecivefat': milkrecivefat, 'creamqty': creamqty, 'skimmilkqty': skimmilkqty, 'skimmilkfat': skimmilkfat,
                    'section': section, 'butterfat': butterfat, 'btnval': btnval, 'sno': sno, 'productionqty': productionqty, 'productionfat': productionfat, 'totalcreamproductionfat': totalcreamproductionfat,
                    'totalskimmilkfat': totalskimmilkfat, 'opcreamqty': opcreamqty, 'opcreamfat': opcreamfat
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_creamsaparation_details();
                            Clearvalues();
                            $('#div_creamdata').show();
                            $('#cream_fillform').css('display', 'none');
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
        function get_creamsaparation_details() {
            var getdate = document.getElementById('txt_getdatadate').value;
            var data = { 'op': 'get_creamsaparation_details', 'getdate': getdate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcreamsaparationdetails(msg);
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
        function fillcreamsaparationdetails(msg) {
            var results = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr  style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col">Section</th><th scope="col">Milk Receive Qty</th><th scope="col">Milk Receive FAT</th><th scope="col">Cream Qty</th><th scope="col">Skim Milk Qty</th><th scope="col">Skim Milk FAT</th><th scope="col" style="text-align: center !important;">Date</th><th scope="col" style="text-align: center !important;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1">' + msg[i].section + '</td>';
                results += '<td class="2">' + msg[i].milkreciveqty + '</td>';
                results += '<td  class="3">' + msg[i].milkrecivefat + '</td>';
                results += '<td  class="4">' + msg[i].creamqty + '</td>';
                results += '<td  class="5">' + msg[i].skimmilkqty + '</td>';
                results += '<td  class="6">' + msg[i].skimmilkfat + '</td>';
                results += '<td  class="7">' + msg[i].Date + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                results += '<td  style="display:none;" class="8">' + msg[i].butterfat + '</td>';
                results += '<td  style="display:none;" class="9">' + msg[i].productionqty + '</td>';
                results += '<td  style="display:none;" class="10">' + msg[i].productionfat + '</td>';
                results += '<td  style="display:none;" class="11">' + msg[i].totalcreamproductionfat + '</td>';
                results += '<td  style="display:none;" class="12">' + msg[i].totalskimmilkfat + '</td>';
                results += '<td  style="display:none;" class="14">' + msg[i].openingbalance + '</td>';
                results += '<td  style="display:none;" class="15">' + msg[i].openingfat + '</td>';
                results += '<td  style="display:none;" class="13">' + msg[i].sno + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_creamdata").html(results);
        }
        function getme(thisid) {
            $('#cream_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_creamdata').hide();
            var section = $(thisid).parent().parent().children('.1').html();
            var milkreciveqty = $(thisid).parent().parent().children('.2').html();
            var milkrecivefat = $(thisid).parent().parent().children('.3').html();
            var creamqty = $(thisid).parent().parent().children('.4').html();
            var skimmilkqty = $(thisid).parent().parent().children('.5').html();
            var skimmilkfat = $(thisid).parent().parent().children('.6').html();
            var Date = $(thisid).parent().parent().children('.7').html();
            var butterfat = $(thisid).parent().parent().children('.8').html();
            var productionqty = $(thisid).parent().parent().children('.9').html();
            var productionfat = $(thisid).parent().parent().children('.10').html();
            var totalcreamproductionfat = $(thisid).parent().parent().children('.11').html();
            var totalskimmilkfat = $(thisid).parent().parent().children('.12').html();
            var sno = $(thisid).parent().parent().children('.13').html();
            var ob = $(thisid).parent().parent().children('.14').html();
            var obfat = $(thisid).parent().parent().children('.15').html();

            document.getElementById('txt_date').value = Date;
            document.getElementById('ddlcreamtype').value = section;
            document.getElementById('txt_butterfat').value = butterfat;
            document.getElementById('txt_recivemilk').value = milkreciveqty;
            document.getElementById('txt_recivefat').value = milkrecivefat;
            document.getElementById('txtproductionqty').value = productionqty;
            document.getElementById('txt_creamquantity').value = creamqty;
            document.getElementById('txt_productionfat').value = productionfat;
            document.getElementById('txt_totalcreamproductionfat').value = totalcreamproductionfat;
            document.getElementById('txt_skimmilkqty').value = skimmilkqty;
            document.getElementById('txt_skimmilkfat').value = skimmilkfat;
            document.getElementById('txt_totalskimmilkfat').value = totalskimmilkfat;
            document.getElementById('lbl_sno').innerHTML = sno;
            document.getElementById('save_batchdetails').innerHTML = "Modify";
            document.getElementById('txt_opcreamqty').value = ob;
            document.getElementById('txt_opcreamfat').value = obfat;
        }
        function fatChange(fat) {
            var sectiontype = document.getElementById('ddlcreamtype').value;
            if (sectiontype == "Ghee" || sectiontype == "Processing") {
                var racivefat = 0;
                racivefat = parseFloat(fat.value).toFixed(3);
                var recivemilk = 0;
                recivemilk = document.getElementById('txt_recivemilk').value;
                recivemilk = parseFloat(recivemilk).toFixed(3);
                var creamqty = (recivemilk * racivefat) / 100;
                document.getElementById('txt_creamquantity').value = creamqty;
            }
            else {
                var butterfat = document.getElementById('txt_butterfat').value;
                var racivefat = 0;
                racivefat = parseFloat(fat.value).toFixed(3);
                var recivemilk = 0;
                recivemilk = document.getElementById('txt_recivemilk').value;
                recivemilk = parseFloat(recivemilk).toFixed(3);
                var creamqty = (recivemilk * racivefat);
                var tcreamqty = (creamqty / butterfat / 100) * 100;
                document.getElementById('txt_creamquantity').value = parseFloat(tcreamqty).toFixed(3); ;
            }
        }
        function creamproductionqtyChange(qty) {
            var recivemilk = 0;
            recivemilk = document.getElementById('txt_recivemilk').value;
            var productionqty = 0;
            productionqty = document.getElementById('txtproductionqty').value;
            var productionloss = 2;
            productionloss = recivemilk * 2 / 100;
            var sum = parseFloat(productionqty) + parseFloat(productionloss);
            var production = parseFloat(recivemilk) - parseFloat(sum);
            document.getElementById('txt_skimmilkqty').value = production;
        }
        function creamproductionfatChange(qty) {
            var productionqty = 0;
            productionqty = document.getElementById('txtproductionqty').value;
            var fat = 0;
            fat = document.getElementById('txt_productionfat').value;
            var production = productionqty * fat / 100;
            var productionfat = parseFloat(production);
            document.getElementById('txt_totalcreamproductionfat').value = productionfat;
        }
        function skimmilkfatChange(qty) {
            var skimmilkqty = 0;
            skimmilkqty = document.getElementById('txt_skimmilkqty').value;
            var skimmilkfat = 0;
            skimmilkfat = document.getElementById('txt_skimmilkfat').value;
            var production = skimmilkqty * skimmilkfat / 100;
            var productionfat = parseFloat(production);
            document.getElementById('txt_totalskimmilkfat').value = productionfat;
            var totalcreamproductionfat = document.getElementById('txt_totalcreamproductionfat').value;
            var diff = parseFloat(totalcreamproductionfat) + parseFloat(productionfat);
            var creamqty = document.getElementById('txt_creamquantity').value;
            document.getElementById('txtdifference').value = parseFloat(creamqty) - parseFloat(diff);
        }
        function sectionchange() {
            var sectiontype = document.getElementById('ddlcreamtype').value;
            if (sectiontype == "Ghee" || sectiontype == "Processing") {
                $('#butterfat').css('display', 'none');
            }
            else {
                $('#butterfat').css('display', 'block');
            }
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
        function get_productqty_details() {
            var productid = "1217";
            var data = { 'op': 'get_productqty_details', 'productid': productid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('txt_opcreamqty').value = msg[i].quantity;
                        }
                    }
                    else {
                        document.getElementById('txt_opcreamqty').value = "0";
                    }
                }
                else {
                    document.getElementById('txt_opcreamqty').value = "0";
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function show_creamseparation_details() {
            $('#div_creamdet').css('display', 'block');
            $('#div_cremclosing').css('display', 'none');
        }
        function show_creamclosing_details() {
            $('#div_creamdet').css('display', 'none');
            $('#div_cremclosing').css('display', 'block');
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
            $('#txt_ccdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            get_cream_opening_details();
        }
        function get_cream_opening_details() {
            var data = { 'op': 'get_cream_opening_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var k = 0;
                        var results = '<div style="overflow:auto;"><table id="table_cream_close_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Silo Name</th><th scope="col">Openingqty</th><th scope="col">FAT</th><th scope="col">SNF</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr>';
                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + (k + 1) + '</span></th>';
                            results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].productname + '</span></th>';
                            results += '<td><input id="txt_qtykgs" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter qty kgs"></td>';
                            results += '<td><input id="txt_fat" class="form-control" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter fat"></td>';
                            results += '<td><input id="txt_snf" class="form-control" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                            results += '<td style="display:none" class="8"><input id="hdn_productsno" class="form-control" type="text" name="vendorcode" value="' + msg[i].sno + '"></td></tr>';
                        }
                        results += '</table></div>';
                        $("#div_getcreamclos").html(results);
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
        function save_cream_closing_details() {
            var date = document.getElementById('txt_ccdate').value;
            if (date == "") {
                alert("Plese Select Date");
                return false;
            }
            var rows = $("#table_cream_close_details tr:gt(0)");
            var cream_closing_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_qtykgs').val() != "") {
                    cream_closing_details.push({ productsno: $(this).find('#hdn_productsno').val(), qty_kgs: $(this).find('#txt_qtykgs').val(), fat: $(this).find('#txt_fat').val(), snf: $(this).find('#txt_snf').val() });
                }
            });
            if (cream_closing_details.length == 0) {
                alert("Please enter new quantity");
                return false;
            }
            var confi = confirm("Do you want to save  cream closing details ?");
            if (confi) {
                var Data = { 'op': 'save_cream_closing_details', 'cream_closing_details': cream_closing_details, 'date': date };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
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
        function clear_closing() {
            get_cream_opening_details();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Cream Seperation <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Cream Seperation</a></li>
        </ol>
    </section>
    <section class="content">
       <div class="box box-info">
         <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="show_creamseparation_details()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Cream Seperation</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="show_creamclosing_details()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Cream Closing</a></li>
                </ul>
            </div>
        <div id="div_creamdet" style="display:block;">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Cream Seperation Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs">
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
                            <input id="txt_getdatadate" class="form-control" type="date" name="date" />
                        </td>
                        <td>
                        <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="get_creamsaparation_details()"></span> <span onclick="get_creamsaparation_details()">Generate</span>
                          </div>
                          </div>
                        </td>
                            <td>
                            </td>
                            <td  style="padding-left: 66%;">
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addcreamsaparation()"></span> <span onclick="addcreamsaparation()">Add Cream</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                </div>
                <div id="div_creamdata">
                </div>
                <div id='cream_fillform' style="display: none; padding-left: 235px;">
                    <table align="center">
                    <tr>
                     <td>
                                <label>
                                    Date</label>
                                <input id="txt_date" type="date" class="form-control" name="vendorcode" placeholder="Enter Date">
                            </td>
                           
                    </tr>
                        <tr>
                            <td>
                                <label>
                                    Opening Cream Qty<span style="color: red;">*</span></label>
                                <input id="txt_opcreamqty" class="form-control" type="text" onkeypress="return isFloat(event);" readonly>
                            </td>
                             <td style="width: 3px;">
                            </td>
                             <td>
                                <label>
                                   Opening Cream Fat<span style="color: red;">*</span></label>
                                <input id="txt_opcreamfat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter OB Cream Fat" >
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Section Type<span style="color: red;">*</span></label>
                                <select id="ddlcreamtype" class="form-control" onchange="sectionchange();">
                                    <option selected value disabled>Select Section Type</option>
                                    <option>Ghee</option>
                                    <option>Butter</option>
                                    <option>Processing</option>
                                </select>
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td id="butterfat" style="display:none;">
                                <label>
                                    Butter Fat<span style="color: red;">*</span></label>
                                <input id="txt_butterfat" class="form-control" type="text" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter FAT" >
                            </td>
                        </tr>
                        <tr>
                           
                            <td>
                                <label>
                                    Milk Received Qty<span style="color: red;">*</span></label>
                                <input id="txt_recivemilk" class="form-control" type="text" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Milk Received Qty">
                            </td>
                             <td style="width: 3px;">
                            </td>
                             <td>
                                <label>
                                    Milk Received Fat</label>
                                <input id="txt_recivefat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Milk Received Fat" 
                                    onkeyup="fatChange(this)">
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                   Total Cream Quantity<span style="color: red;">*</span></label>
                                <input id="txt_creamquantity" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode"
                                    readonly>
                            </td>
                        </tr>
                        <tr>
                           <td>
                                <label>
                                   Cream Production Quantity<span style="color: red;">*</span></label>
                                <input id="txtproductionqty" type="text" class="form-control" placeholder="Enter Cream Production Quantity" onkeypress="return isFloat(event);" name="vendorcode" onkeyup="creamproductionqtyChange(this)" />
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                   Cream Production Fat<span style="color: red;">*</span></label>
                                <input id="txt_productionfat" type="text" class="form-control" placeholder="Enter Cream Production Fat" onkeypress="return isFloat(event);" name="vendorcode" onkeyup="creamproductionfatChange(this)"/>
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                   Total Cream Production Fat<span style="color: red;">*</span></label>
                                <input id="txt_totalcreamproductionfat" type="text" onkeypress="return isFloat(event);" class="form-control" name="vendorcode" readonly/>
                            </td>
                        </tr>
                        <tr>
                         <td>
                                <label>
                                    Skim Milk Qty<span style="color: red;">*</span></label>
                                <input id="txt_skimmilkqty" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Skim Milk Qty">
                            </td>
                           <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                    Skim Milk Fat<span style="color: red;">*</span></label>
                                <input id="txt_skimmilkfat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Skim Milk Fat" onkeyup="skimmilkfatChange(this)">
                            </td>
                             <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                   Total Skim Milk Production Fat<span style="color: red;">*</span></label>
                                <input id="txt_totalskimmilkfat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" readonly />
                            </td>
                        </tr>
                        <tr>
                         <td>
                                <label>
                                    Difference<span style="color: red;">*</span></label>
                                <input id="txtdifference" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Skim Milk Qty">
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td colspan="2">
                                <label>
                                    Remarks
                                </label>
                                <textarea rows="3" cols="45" id="txt_Remarks" class="form-control" maxlength="200"
                                    placeholder="Enter Remarks"></textarea>
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                        </tr>
                    </table>
                    <div  style="padding-left: 10%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_batchdetails1" onclick="save_creamsaparationdetails_click()"></span><span id="save_batchdetails" onclick="save_creamsaparationdetails_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="Clearvalues()"></span><span id='btn_close' onclick="Clearvalues()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    <br />
                </div>
            </div>
        </div>
        </div>
        <div id="div_cremclosing" style="display:none;">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Cream Closing Details
                </h3>
            </div>
            <div class="box-body">
                <div>
                    <table>
                        <tr>
                            <td>
                                <label>Date : </label> 
                            </td>
                            <td>
                                <input id="txt_ccdate" type="datetime-local" class="form-control"  />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_getcreamclos">
                </div>
                <div>
                   <table align="center">
                    <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="Span3" onclick="save_cream_closing_details()"></span><span id="Span4" onclick="save_cream_closing_details()">Save</span>
                            </div>
                            </div>
                        </td>
                     </tr>
                </table>
                </div>
            </div>
        </div>
        </div>
      </div> 
    </section>
</asp:Content>
