<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="biproductsproduction.aspx.cs" Inherits="biproductsproduction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btn_addDept').click(function () {
                $('#biprdsp_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_getbidata').hide();
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
            });

            $('#btn_close').click(function () {
                $('#biprdsp_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_getbidata').show();
                $('#div_editbiproducts').hide();
            });
            $('#btn_eclose').click(function () {
                $('#biprdsp_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_getbidata').show();
                $('#div_editbiproducts').hide();
            });
            get_curdbiproductdetails();
            get_biprodct_products_details();
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
        });
        function addbiproducts() {
            $('#biprdsp_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_getbidata').hide();
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
            document.getElementById('ddltype').selectedIndex = 0;
            document.getElementById('txt_date').value = "";
            document.getElementById('txt_bipob').value = "";
            document.getElementById('txt_recivemilk').value = "";
            document.getElementById('txt_recivefat').value = "";
            document.getElementById('txt_snf').value = "";
            document.getElementById('txt_production').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('save_batchdetails').innerHTML = "Save";
            $('#biprdsp_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_getbidata').show();
            $('#div_editbiproducts').hide();
        }
//        function productchange() {
//            var productid = document.getElementById('ddltype').value;
//            var data = { 'op': 'get_productqty_details', 'productid': productid };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg.length > 0) {
//                        for (var i = 0; i < msg.length; i++) {
//                            document.getElementById('txt_bipob').value = msg[i].quantity;
//                        }
//                    }
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
        //        }
        var msgarray = [];
        function get_biprodct_products_details() {
            var data = { 'op': 'get_biprodct_products_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillproducts(msg);
                        msgarray = msg;
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
        function fillproducts(msg) {
            var data = document.getElementById('ddltype');
            var length = data.options.length;
            document.getElementById('ddltype').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Product Name";
            opt.value = "Select Product Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].productname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].productname;
                    option.value = msg[i].productid;
                    data.appendChild(option);
                }
            }
        }
        function productchange() {
            var productid = document.getElementById('ddltype').value;
            for (var i = 0; i < msgarray.length; i++) {
                if (msgarray[i].productid == productid) {
                    document.getElementById('txt_bipob').value = msgarray[i].quantity;
                }
            }
        }
        function save_curdbiproductdetails_click() {
            var flag = false;
            var doe = document.getElementById('txt_date').value;
            var ob = document.getElementById('txt_bipob').value;
            var producttype = document.getElementById('ddltype').value;
            if (producttype == "") {
                alert("Enter Skim product type");
                flag = true;
            }
            var milkreciveqty = document.getElementById('txt_recivemilk').value;
            if (milkreciveqty == "") {
                alert("Enter Milk Recive Qty");
                flag = true;
            }
            var milkrecivefat = document.getElementById('txt_recivefat').value;
            if (milkrecivefat == "") {
                alert("Enter Milk Recive Fat");
                flag = true;
            }
            var milkrecivesnf = document.getElementById('txt_snf').value;
            var production = document.getElementById('txt_production').value;
            if (production == "") {
                alert("Enter production");
                flag = true;
            }
            var remarks = document.getElementById('txt_Remarks').value;
            var btnval = document.getElementById('save_batchdetails').innerHTML;
            var sno = document.getElementById('lbl_sno').value;
            if (flag) {
                return;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_curdbiproductdetails_click', 'doe': doe, 'milkreciveqty': milkreciveqty, 'milkrecivefat': milkrecivefat, 'milkrecivesnf': milkrecivesnf, 'production': production, 'producttype': producttype, 'btnval': btnval, 'sno': sno, 'remarks': remarks, 'ob': ob };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_curdbiproductdetails();
                            Clearvalues();
                            $('#div_getbidata').show();
                            $('#biprdsp_fillform').css('display', 'none');
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
        function get_curdbiproductdetails() {
            var data = { 'op': 'get_curdbiproductdetails' };
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
            var results = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Id</th><th scope="col" style="font-weight: bold;">Milk Recive Qty</th><th scope="col" style="font-weight: bold;">Milk Recive FAT</th><th scope="col" style="font-weight: bold;">Milk Recive SNF</th><th scope="col" style="font-weight: bold;">Production</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1">' + msg[i].productname + '</td>';
                results += '<td scope="row" class="2">' + msg[i].milkreciveqty + '</td>';
                results += '<td data-title="Code" class="3">' + msg[i].milkrecivefat + '</td>';
                results += '<td data-title="Code" class="4">' + msg[i].milkrecivesnf + '</td>';
                results += '<td data-title="Code" class="5">' + msg[i].production + '</td>';
                results += '<td data-title="Code" class="6">' + msg[i].doe + '</td>';
                results += '<td data-title="Code" style="display:none;"   class="8">' + msg[i].ob + '</td>';
                results += '<td data-title="Code" style="display:none;"   class="9">' + msg[i].datetime + '</td>';
                results += '<td data-title="Code" style="display:none;"   class="10">' + msg[i].remarks + '</td>';
                results += '<td data-title="Code" style="display:none;"   class="11">' + msg[i].productid + '</td>';
                results += '<td data-title="Code" style="display:none;" class="7">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_getbidata").html(results);
        }
        function getme(thisid) {
            var productname = $(thisid).parent().parent().children('.1').html();
            var milkreciveqty = $(thisid).parent().parent().children('.2').html();
            var milkrecivefat = $(thisid).parent().parent().children('.3').html();
            var milkrecivesnf = $(thisid).parent().parent().children('.4').html();
            var production = $(thisid).parent().parent().children('.5').html();
            var doe = $(thisid).parent().parent().children('.6').html();
            var sno = $(thisid).parent().parent().children('.7').html();
            var ob = $(thisid).parent().parent().children('.8').html();
            var datetime = $(thisid).parent().parent().children('.9').html();
            var remarks = $(thisid).parent().parent().children('.10').html();
            var productid = $(thisid).parent().parent().children('.11').html();

            document.getElementById('txt_date').value = doe;
            document.getElementById('ddltype').value = productid;
            document.getElementById('txt_bipob').value = ob;
            document.getElementById('txt_recivemilk').value = milkreciveqty;
            document.getElementById('txt_recivefat').value = milkrecivefat;
            document.getElementById('txt_snf').value = milkrecivesnf;
            document.getElementById('txt_production').value = production;
            document.getElementById('txt_Remarks').value = remarks;
            document.getElementById('lbl_sno').value = sno;
            document.getElementById('save_batchdetails').innerHTML = "Modify";

            $('#biprdsp_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_getbidata').hide();
        }
        function fatChange(fat) {
            var sectiontype = document.getElementById('ddlcreamtype').value;
            if (sectiontype == "Ghee") {
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
        function sectionchange() {
            var sectiontype = document.getElementById('ddlcreamtype').value;
            if (sectiontype == "Ghee") {
                $('#butterfat').css('display', 'none');
            }
            else {
                $('#butterfat').css('display', 'block');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Curd BI-Products <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Curd BI-Products</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Curd BI-Products Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                    <%--<input id="btn_addDept" type="button" name="submit" value='Add Details'
                        class="btn btn-success" />--%>
                         <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addbiproducts()"></span> <span onclick="addbiproducts()">Add Details</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_getbidata">
                </div>
                <div id='biprdsp_fillform' style="display: none; padding-left: 235px;">
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Date</label>
                                <input id="txt_date" type="date" class="form-control" name="vendorcode" placeholder="Enter Date" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Type<span style="color: red;">*</span></label>
                                <select id="ddltype" class="form-control" onchange="productchange();">
                                   <%-- <option selected value disabled>Select Bi-Product</option>
                                    <option value="93">PANEER</option>
                                    <option value="92">KHOVA LOOSE</option>--%>
                                </select>
                            </td>
                             <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                    Opening Balance</label>
                                <input id="txt_bipob" type="text" class="form-control" name="ob" readonly />
                            </td>
                            </tr>
                            <tr>
                            <td>
                                <label>
                                    Milk Recived Qty<span style="color: red;">*</span></label>
                                <input id="txt_recivemilk" class="form-control" type="text" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Milk Recived Qty" />
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                    Milk Recived Fat</label>
                                <input id="txt_recivefat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Milk Recived Fat" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Milk Recived SNF<span style="color: red;">*</span></label>
                                <input id="txt_snf" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);" placeholder="Enter Milk Recived SNF" />
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                    Production<span style="color: red;">*</span></label>
                                <input id="txt_production" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);" placeholder="Enter Production Qty" />
                            </td>
                        </tr>
                        <tr>
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
                       <%-- <div style="padding-left: inherit;">
                        <table>
                        <tr>
                            <td>
                                <input id='save_batchdetails' type="button" class="btn btn-success" name="submit"
                                    value='Save' onclick="save_curdbiproductdetails_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="Clearvalues()" />
                            </td>
                        </tr>
                     </table>
                    </div>--%>
                    <div  style="padding-left: 10%;padding-top: 0%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_batchdetails1" onclick="save_curdbiproductdetails_click()"></span><span id="save_batchdetails" onclick="save_curdbiproductdetails_click()">Save</span>
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
                </div>
                <div id="div_editbiproducts" style="display:none;aligen:center">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
