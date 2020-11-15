<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="ProductMaster.aspx.cs" Inherits="ProductMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
th
{
    text-align:center;
}
</style>
<script type="text/javascript">
    $(function () {
        get_SiloDepartments();
        $('#btn_addDept').click(function () {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            forclearall();
            get_SiloDepartments();
        });
        $('#btn_close').click(function () {
            $('#fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_Deptdata').show();
            forclearall();
        });
        get_Batchs();
        get_product_details();
    });
    function addproducts() {
        $('#fillform').css('display', 'block');
        $('#showlogs').css('display', 'none');
        $('#div_Deptdata').hide();
        forclearall();
        get_SiloDepartments();
    }
    function closeproducts() {
        $('#fillform').css('display', 'none');
        $('#showlogs').css('display', 'block');
        $('#div_Deptdata').show();
        forclearall();
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
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function filldetails(msg) {
        var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Batch Name</th><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">Product Code</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color:' + COLOR[l] + '">';
            //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
            results += '<td scope="row" class="1" style="display:none">' + msg[i].batch + '</td>';
            results += '<td data-title="brandstatus" class="43"><span class="" style="color: cadetblue;"></span> ' + msg[i].batch + '</td>';
            results += '<td data-title="Capacity" class="2" style="display:none">' + msg[i].productname + '</td>';
            results += '<td data-title="brandstatus" class="41"><span class="" style="color: cadetblue;"></span> ' + msg[i].productname + '</td>';
            results += '<td data-title="Capacity" class="7" style="display:none">' + msg[i].productcode + '</td>';
            results += '<td data-title="brandstatus" class="42"><span class="" style="color: cadetblue;"></span> ' + msg[i].productcode + '</td>';
            results += '<td style="display:none" class="6" style="text-align:center;">' + msg[i].departmentname + '</td>';
            results += '<td style="display:none" class="3">' + msg[i].productid + '</td>';
            results += '<td style="display:none" class="4">' + msg[i].batchid + '</td>';
            results += '<td style="display:none" class="5">' + msg[i].departmentid + '</td>';
            results += '<td style="display:none" class="10">' + msg[i].packetsize + '</td>';
            results += '<td style="display:none" class="11">' + msg[i].filimrate + '</td>';
            results += '<td style="display:none" class="12">' + msg[i].fat + '</td>';
            results += '<td style="display:none" class="13">' + msg[i].snf + '</td>';
            results += '<td style="display:none" class="14">' + msg[i].clr + '</td>';
            results += '<td style="display:none" class="15">' + msg[i].biproductsshortname + '</td>';
            results += '<td style="display:none" class="16">' + msg[i].price + '</td>';
            results += '<td style="display:none" class="17">' + msg[i].categorycode + '</td>';

            results += '<td style="display:none" class="30">' + msg[i].hsnsaccode + '</td>';
            results += '<td style="display:none" class="31">' + msg[i].igstcode + '</td>';
            results += '<td style="display:none" class="32">' + msg[i].cgstcode + '</td>';
            results += '<td style="display:none" class="33">' + msg[i].sgstcode + '</td>';
            results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
            l = l + 1;
            if (l == 4) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_Deptdata").html(results);
    }
    function getme(thisid) {

        var batch = $(thisid).parent().parent().children('.1').html();
        var productname = $(thisid).parent().parent().children('.2').html();
        var productid = $(thisid).parent().parent().children('.3').html();
        var batchid = $(thisid).parent().parent().children('.4').html();
        var departmentid = $(thisid).parent().parent().children('.5').html();
        var productcode = $(thisid).parent().parent().children('.7').html();

        var packetsize = $(thisid).parent().parent().children('.10').html();
        var filmrate = $(thisid).parent().parent().children('.11').html();
        var fat = $(thisid).parent().parent().children('.12').html();
        var snf = $(thisid).parent().parent().children('.13').html();
        var clr = $(thisid).parent().parent().children('.14').html();
        var biproductshortname = $(thisid).parent().parent().children('.15').html();
        var price = $(thisid).parent().parent().children('.16').html();
        var categorycode = $(thisid).parent().parent().children('.17').html();

        var hsnsaccode = $(thisid).parent().parent().children('.30').html();
        var igstcode = $(thisid).parent().parent().children('.31').html();
        var cgstcode = $(thisid).parent().parent().children('.32').html();
        var sgstcode = $(thisid).parent().parent().children('.33').html();

        document.getElementById('slct_Department').value = departmentid;
        document.getElementById('slct_Batch').value = batchid;
        document.getElementById('txtproduct').value = productname;
        document.getElementById('txtproductcode').value = productcode;

        document.getElementById('txt_packetsize').value = packetsize;
        document.getElementById('txt_filmrate').value = filmrate;
        document.getElementById('txt_fat').value = fat;
        document.getElementById('txt_snf').value = snf;
        document.getElementById('txt_clr').value = clr;
        document.getElementById('txt_biproductshortname').value = biproductshortname;
        document.getElementById('txt_price').value = price;
        document.getElementById('txt_categorycode').value = categorycode;
        document.getElementById('lbl_sno').value = productid;

        document.getElementById('txt_hsnsaccode').value = hsnsaccode;
        document.getElementById('txt_igst').value = igstcode;
        document.getElementById('txt_cgst').value = cgstcode;
        document.getElementById('txt_sgst').value = sgstcode;
        document.getElementById('btn_save').innerHTML = "Modify";
        $("#div_Deptdata").hide();
        $("#fillform").show();
        $('#showlogs').hide();
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
    function get_Batchs() {
        var data = { 'op': 'get_batch_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillBatchs(msg);
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
    function fillBatchs(msg) {
        var data = document.getElementById('slct_Batch');
        var length = data.options.length;
        document.getElementById('slct_Batch').options.length = null;
        var opt = document.createElement('option');
        opt.innerHTML = "Select Batch";
        opt.value = "Select Batch";
        opt.setAttribute("selected", "selected");
        opt.setAttribute("disabled", "disabled");
        opt.setAttribute("class", "dispalynone");
        data.appendChild(opt);
        for (var i = 0; i < msg.length; i++) {
            if (msg[i].batchtype != null) {
                var option = document.createElement('option');
                option.innerHTML = msg[i].batchtype;
                option.value = msg[i].batchid;
                data.appendChild(option);
            }
        }
    }

    //Function for only no
    $(document).ready(function () {
        $("#txt_phoneno").keydown(function (event) {
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
    function validateEmail(email) {
        var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
        if (reg.test(email)) {
            return true;
        }
        else {
            return false;
        }
    }
    function get_SiloDepartments() {
        var data = { 'op': 'get_SiloDepartments_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    filldepartments(msg);
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
    function filldepartments(msg) {
        var data = document.getElementById('slct_Department');
        var length = data.options.length;
        document.getElementById('slct_Department').options.length = null;
        var opt = document.createElement('option');
        opt.innerHTML = "Select Department";
        opt.value = "Select Department";
        opt.setAttribute("selected", "selected");
        opt.setAttribute("disabled", "disabled");
        opt.setAttribute("class", "dispalynone");
        data.appendChild(opt);
        for (var i = 0; i < msg.length; i++) {
            if (msg[i].DeportmentName != null) {
                var option = document.createElement('option');
                option.innerHTML = msg[i].DeportmentName;
                option.value = msg[i].SiloDeportmentId;
                data.appendChild(option);
            }
        }
    }
    function save_product_master_click() {
        var batch = document.getElementById('slct_Batch').value;
        var dept = document.getElementById('slct_Department').value;
        var product = document.getElementById('txtproduct').value;
        var productcode = document.getElementById('txtproductcode').value;

        var packetsize = document.getElementById('txt_packetsize').value;
        var filmrate = document.getElementById('txt_filmrate').value;
        var fat = document.getElementById('txt_fat').value;
        var snf = document.getElementById('txt_snf').value;
        var clr = document.getElementById('txt_clr').value;
        var biproductshortname = document.getElementById('txt_biproductshortname').value;
        var price = document.getElementById('txt_price').value;
        var categorycode = document.getElementById('txt_categorycode').value;

        var productid = document.getElementById('lbl_sno').value;
        var btnval = document.getElementById('btn_save').innerHTML;
        if (dept == "" || dept == "Select Department") {
            alert("Select Department");
            $("#slct_Department").focus();
            return false;
        }
        if (batch == "" || batch == "Select Batch") {
            alert("Select Batch Type");
            $("#slct_Batch").focus();
            return false;
        }
        if (product == "") {
            alert("Enter Product Name");
            $("#txtproduct").focus();
            return false;
        }
        if (productcode == "" || productcode == "0") {
            alert("Enter Product Code");
            $("#txtproductcode").focus();
            return false;
        }
        var hsnsaccode = document.getElementById('txt_hsnsaccode').value;
        var igstcode = document.getElementById('txt_igst').value;
        var cgstcode = document.getElementById('txt_cgst').value;
        var sgstcode = document.getElementById('txt_sgst').value;

        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var data = { 'op': 'save_product_master_click', 'batch': batch, 'dept': dept, 'product': product, 'btnval': btnval, 'productid': productid, 'productcode': productcode,
                'packetsize': packetsize, 'filmrate': filmrate, 'fat': fat, 'snf': snf, 'clr': clr, 'biproductshortname': biproductshortname, 'price': price,
                'categorycode': categorycode, 'hsnsaccode': hsnsaccode, 'igstcode': igstcode, 'cgstcode': cgstcode, 'sgstcode': sgstcode
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        forclearall();
                        get_product_details();
                        $('#div_Deptdata').show();
                        $('#fillform').css('display', 'none');
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
    function forclearall() {
        document.getElementById('slct_Batch').selectedIndex = 0;
        document.getElementById('slct_Department').selectedIndex = 0;
        document.getElementById('txtproduct').value = "";
        document.getElementById('lbl_sno').value = "";
        document.getElementById('txtproductcode').value = "";
        document.getElementById('txt_packetsize').value = "";
        document.getElementById('txt_filmrate').value = "";
        document.getElementById('txt_fat').value = "";
        document.getElementById('txt_snf').value = "";
        document.getElementById('txt_clr').value = "";
        document.getElementById('txt_biproductshortname').value = "";
        document.getElementById('txt_price').value = "";
        document.getElementById('txt_categorycode').value = "";
        document.getElementById('btn_save').innerHTML = "Save";

        document.getElementById('txt_hsnsaccode').value = "";
        document.getElementById('txt_igst').value = "0";
        document.getElementById('txt_cgst').value = "0";
        document.getElementById('txt_sgst').value = "0";
    }
    function getdetails() {
        var igst = document.getElementById('txt_igst').value;
        var igsts = (igst / 2);
        igsts = parseFloat(igsts).toFixed(2);
        document.getElementById('txt_cgst').value = igsts;
        document.getElementById('txt_sgst').value = igsts;
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
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            Product Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Product Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Product Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                    <%--<input id="btn_addDept" type="button" name="submit" value='Add Product' onclick="addproducts();" class="btn btn-success" />--%>
                     <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addproducts()"></span> <span onclick="addproducts()">Add Product</span>
                          </div>
                          </div>
                            </td>
                         </tr>
                      </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='fillform' style="display: none;" align="center">
                    <table align="center" >
                        <tr>
                            <th>
                            </th>
                        </tr>
                        <tr>
                            <td  style="height: 40px;">
                              <label>
                                    Department<span style="color: red;">*</span></label>
                            
                                <select id="slct_Department" class="form-control">
                                </select>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td  style="height: 40px;">
                              <label>
                                    Batch Type<span style="color: red;">*</span></label>
                           
                                <select id="slct_Batch" class="form-control">
                                    <option selected disabled value="Select SILO No">Select Batch Type</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    Product Name
                                </label>
                                <span style="color: red;">*</span>
                           
                                <input type="text" id="txtproduct" class="form-control" name="vendorcode"
                                    placeholder="Enter Product Name">
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td style="height: 40px;">
                                <label>
                                   SAP Product Code
                                </label>
                                <span style="color: red;">*</span>
                            
                                <input type="text"  id="txtproductcode" class="form-control" name="vendorcode"
                                    placeholder="Enter Product Code">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    Packet Size
                                </label>
                                <span style="color: red;">*</span>
                            
                                <input type="text"  id="txt_packetsize" class="form-control" name="packetsize"
                                    placeholder="Enter Packet Size" onkeypress="return isFloat(event);">
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td style="height: 40px;">
                                <label>
                                   Film Rate
                                </label>
                                <span style="color: red;">*</span>
                           
                                <input type="text" id="txt_filmrate" class="form-control" name="filmrate"
                                    placeholder="Enter Film Rate"  onkeypress="return isFloat(event);"  >
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    FAT
                                </label>
                                <span style="color: red;">*</span>
                            
                                <input type="text"  id="txt_fat" class="form-control" name="fat"
                                    placeholder="Enter Fat" onkeypress="return isFloat(event);" >
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td style="height: 40px;">
                                <label>
                                   SNF
                                </label>
                                <span style="color: red;">*</span>
                            
                                <input type="text" id="txt_snf" class="form-control" name="snf"
                                    placeholder="Enter Snf" onkeypress="return isFloat(event);" >
                            </td>
                        </tr>
                         <tr>
                            <td style="height: 40px;">
                                <label>
                                    CLR
                                </label>
                                <span style="color: red;">*</span>
                            
                                <input type="text"  id="txt_clr" class="form-control" name="fat"
                                    placeholder="Enter Clr" onkeypress="return isFloat(event);" >
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td style="height: 40px;" hidden>
                                <label>
                                   Bi-Product Short Name
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td hidden>
                                <input type="text"  id="txt_biproductshortname" class="form-control" name="snf"
                                    placeholder="Enter Bi-Product Shortname">
                            </td>
                            <td style="height: 40px;">
                                <label>
                                    Price
                                </label>
                                <span style="color: red;">*</span>
                            
                                <input type="text"  id="txt_price" class="form-control" name="fat"
                                    placeholder="Enter Price" onkeypress="return isFloat(event);">
                            </td>
                             </tr>
                             <tr>
                            <td style="height: 40px;">
                                <label>
                                   Category Code
                                </label>
                                <span style="color: red;">*</span>
                            
                                <input type="text" id="txt_categorycode" class="form-control" name="snf"
                                    placeholder="Enter Category Code">
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td style="height: 40px;">
                                <label>
                                    HSN/SAC Code
                                </label>
                                <span style="color: red;">*</span>
                                <input type="text"  id="txt_hsnsaccode" class="form-control" name="hsnsaccode"
                                    placeholder="Enter  HSN/SAC Code">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                   IGST
                                </label>
                                <span style="color: red;">*</span>
                                <input type="text"  id="txt_igst" class="form-control" onkeyup="getdetails();" name="igst"
                                    placeholder="Enter IGST" onkeypress="return isFloat(event);" value="0">
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td style="height: 40px;">
                                <label>
                                    CGST
                                </label>
                                <span style="color: red;">*</span>
                                <input type="text"  id="txt_cgst" class="form-control" name="cgst"
                                    placeholder="Enter  CGST" readonly value="0" />
                            </td>
                        </tr>
                         <tr>
                            <td style="height: 40px;">
                                <label>
                                   SGST
                                </label>
                                <span style="color: red;">*</span>
                                <input type="text"  id="txt_sgst" class="form-control" name="sgst"
                                    placeholder="Enter SGST" readonly value="0" />
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                        </table>
                        <table>
                       <%-- <tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input type="button"  name="submit" class="btn btn-success"
                                    id="btn_save" value='Save' onclick="save_product_master_click()" />  <input id='btn_close'
                                        type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>--%>
                    </table>
                    <div  style="padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="save_product_master_click()"></span><span id="btn_save" onclick="save_product_master_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="closeproducts()"></span><span id='btn_close' onclick="closeproducts()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

