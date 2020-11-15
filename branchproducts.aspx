<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="branchproducts.aspx.cs" Inherits="branchproducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(function () {
        get_product_details();
        get_branch_products();
        $('#btn_addprod').click(function () {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_proddata').hide();
            get_product_details();
        });
        $('#btn_close').click(function () {
            $('#fillform').css('display', 'none');
            $('#showlogs').css('display', 'none');
            $('#div_proddata').show();
        });
    });
    function get_product_details() {
        var data = { 'op': 'get_product_Master_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillproducts(msg);
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
        var data = document.getElementById('slct_productname');
        var length = data.options.length;
        document.getElementById('slct_productname').options.length = null;
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
    function save_branch_products() {
        var productname = document.getElementById('lbl_sno').value;
        var quantity = document.getElementById('txt_quantity').value;
        var vat = document.getElementById('txt_vat').value;
        var price = document.getElementById('txt_price').value;
        var btnval = document.getElementById('btn_save').value;
        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var data = { 'op': 'save_branch_products', 'productname': productname, 'quantity': quantity, 'price': price, 'vat': vat, 'btnval': btnval
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        get_product_details();
                        $('#div_proddata').show();
                        $('#fillform').css('display', 'none');
                        $('#showlogs').css('display', 'none');
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
        document.getElementById('slct_productname').value = "";
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
        document.getElementById('txt_quantity').value = "";
        document.getElementById('txt_vat').value = "";
        document.getElementById('txt_price').value = "";
        document.getElementById('btn_save').value = "Save";
    }
    function get_branch_products() {
        var data = { 'op': 'get_branch_products' };
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
        var results = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col"></th><th scope="col">Batch Name</th><th scope="col">Product Name</th><th scope="col">Product Code</th></tr></thead></tbody>';
        for (var i = 0; i < msg.length; i++) {
            results += '<tr><td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-success" value="Choose" /></td>';
            results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].batch + '</td>';
            results += '<td data-title="Capacity" class="2" style="text-align:center;">' + msg[i].productname + '</td>';
            results += '<td data-title="Capacity" class="7" style="text-align:center;">' + msg[i].productcode + '</td>';
            results += '<td style="display:none" class="6" style="text-align:center;">' + msg[i].departmentname + '</td>';
            results += '<td style="display:none" class="3">' + msg[i].productid + '</td>';
            results += '<td style="display:none" class="5">' + msg[i].departmentid + '</td>';
            results += '<td style="display:none" class="10">' + msg[i].packetsize + '</td>';
            results += '<td style="display:none" class="11">' + msg[i].filimrate + '</td>';
            results += '<td style="display:none" class="12">' + msg[i].fat + '</td>';
            results += '<td style="display:none" class="13">' + msg[i].snf + '</td>';
            results += '<td style="display:none" class="14">' + msg[i].clr + '</td>';
            results += '<td style="display:none" class="15">' + msg[i].biproductsshortname + '</td>';
            results += '<td style="display:none" class="16">' + msg[i].price + '</td>';
            results += '<td style="display:none" class="17">' + msg[i].categorycode + '</td>';
            results += '<td style="display:none" class="18">' + msg[i].quantity + '</td>';
            results += '<td style="display:none" class="4">' + msg[i].batchid + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_proddata").html(results);
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
        var quantity = $(thisid).parent().parent().children('.18').html();

        document.getElementById('slct_productname').value = productid;

        document.getElementById('txt_packetsize').value = packetsize;
        document.getElementById('txt_filmrate').value = filmrate;
        document.getElementById('txt_fat').value = fat;
        document.getElementById('txt_snf').value = snf;
        document.getElementById('txt_clr').value = clr;
        document.getElementById('txt_price').value = price;
        document.getElementById('txt_quantity').value = quantity;

        document.getElementById('lbl_sno').value = productid;
        document.getElementById('btn_save').value = "Modify";
        $('#div_proddata').hide();
        $('#fillform').show();
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            Branch Products<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Branch Products</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Branch Product Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center" style="display: none;">
                    <input id="btn_addprod" type="button" name="submit" value='Add Product' class="btn btn-success" />
                </div>
                <div id="div_proddata">
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
                                     Product Name<span style="color: red;">*</span></label>
                            </td>
                            <td>
                                <select id="slct_productname" class="form-control" >
                                    <option selected disabled value="Select  Product Name">Select Product Name</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    Packet Size
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_packetsize" class="form-control" name="packetsize"
                                    placeholder="Enter Packet Size" readonly/>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td style="height: 40px;">
                                <label>
                                   Film Rate
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_filmrate" class="form-control" name="filmrate"
                                    placeholder="Enter Film Rate" readonly />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    FAT
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_fat" class="form-control" name="fat"
                                    placeholder="Enter Fat" readonly/>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td style="height: 40px;">
                                <label>
                                   SNF
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_snf" class="form-control" name="snf"
                                    placeholder="Enter Snf" readonly />
                            </td>
                        </tr>
                         <tr>
                            <td style="height: 40px;">
                                <label>
                                    CLR
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_clr" class="form-control" name="fat"
                                    placeholder="Enter Clr" readonly />
                            </td>
                             <td style="width: 5px;">
                             </td>
                             <td style="height: 40px;">
                                <label>
                                    Price
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_price" class="form-control" name="fat"
                                    placeholder="Enter Price">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    Quantity
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="number" maxlength="45" id="txt_quantity" class="form-control" name="quantity"
                                    placeholder="Enter Quantity" />
                            </td>
                            </tr>
                            <tr>
                            <td style="height: 40px;">
                                <label>
                                   VAT
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_vat" class="form-control" name="vat"
                                    placeholder="Enter Vat" />
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
                        <tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input type="button"  name="submit" class="btn btn-success"
                                    id="btn_save" value='Save' onclick="save_branch_products()" />  <input id='btn_close'
                                        type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

