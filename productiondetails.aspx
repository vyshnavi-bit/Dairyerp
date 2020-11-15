<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="productiondetails.aspx.cs" Inherits="productiondetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            Clearvalues();
            get_curdproduction_details();
            $('#btn_addDept').click(function () {
                $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
                $('#trbiproducts').css('display', 'none');
                $('#diveditdata').css('display', 'none');
                $('#td_product').show();
                $('#td_producttype').show();
                $('#divFillScreen').hide();
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
            $('#btn_close').click(function () {
                $('#Inwardsilo_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#diveditdata').css('display', 'none');
                $('#div_Deptdata').show();
                Clearvalues();
            });
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
        $(function () {
            var today = new Date();
            var dd = today.getDate() - 1;
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
            $('#txt_getdatadate').val(yyyy + '-' + mm + '-' + dd);
            get_curdproduction_details();
        });
        function addcurdproductiondetails() {
            $('#Inwardsilo_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            $('#trbiproducts').css('display', 'none');
            $('#diveditdata').css('display', 'none');
            $('#td_product').show();
            $('#td_producttype').show();
            $('#divFillScreen').hide();
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
            var data = document.getElementById('slct_biproducts');
            var length = data.options.length;
            document.getElementById('slct_biproducts').options.length = null;
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
        function onproductchange() {
            var producttype = document.getElementById('ddlproducttype').value;
            if (producttype == "1") {
                $('#biproduct').css('display', 'block');
                $('#divFillScreen').hide();
                get_biprodct_products_details();
            }
            else {
                $('#divFillScreen').show();
                fillproductdetails();
                $('#biproduct').css('display', 'none');
            }
        }

        function ontypechange() {
            var biproducttype = document.getElementById('slct_biproducts').value;
            if (biproducttype == "93") {
                $('#divFillScreen').show();
                fillproductdetails();
            }
            else {
                $('#divFillScreen').show();
                fillproductdetails();
            }
            var data = { 'op': 'get_biproductob_details', 'producttype': biproducttype };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('txt_biob').value = msg[i].quantity;
                        }
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
        var branchidw = '<%=Session["Branch_ID"] %>';
        function fillproductdetails() {
            var producttype = document.getElementById('ddlproducttype').value;
            var biproducttype = document.getElementById('slct_biproducts').value;
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        if (producttype == "1") {
                            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" style="font-weight: 700;">Production(kgs)</th><th scope="col" id="tfat" style="font-weight: 700;">FAT</th><th scope="col" id="tsnf" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">Sales</th></tr></thead></tbody>';
                        }
                        else {
                            if (branchidw == "26" || branchidw == "115") {
                                results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" id="trecive" style="font-weight: 700;">Receive Milk(Qty)</th><th scope="col" id="tfat" style="font-weight: 700;">FAT</th><th scope="col" id="tsnf" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">Production (kgs)</th><th scope="col" style="font-weight: 700;">Sales</th><th scope="col" style="font-weight: 700;">Loss Qty</th><th scope="col" style="font-weight: 700;">Curd Sale Rate</th><th scope="col" style="font-weight: 700;">Cutting Qty</th><th scope="col" style="font-weight: 700;">Damage Qty</th><th scope="col" style="font-weight: 700;">Return Qty</th><th scope="col" style="font-weight: 700;">Closing Balance</th></tr></thead></tbody>';
                            }
                            else {
                                results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" id="trecive" style="font-weight: 700;">Receive Milk(Qty)</th><th scope="col" id="tfat" style="font-weight: 700;">FAT</th><th scope="col" id="tsnf" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">Production (kgs)</th><th scope="col" style="font-weight: 700;">Sales</th><th scope="col" style="font-weight: 700;">Loss Qty</th><th scope="col" style="font-weight: 700;">Curd Sale Rate</th></tr></thead></tbody>';
                            }
                        }
                        for (var i = 0; i < msg.length; i++) {
                            if (producttype == "1") {
                                if (msg[i].departmentid == "1" && msg[i].batchid == "16") {
                                    if (biproducttype == "93" && msg[i].biproductsshortname == "P") {
                                        results += '<tr>';
                                        results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                        results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                        results += '<td><input id="txt_ob" readonly class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                        results += '<td><input id="txt_production" class="form-control" onkeypress="return isFloat(event);" value="0" type="text" name="vendorcode"placeholder="Enter Production(kgs)"></td>';
                                        results += '<td style="display:none"><input id="txt_reciveqty" onkeypress="return isFloat(event);" class="form-control" value="0" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                        results += '<td><input id="txt_fat" class="form-control" value="3" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter fat"></td>';
                                        results += '<td><input id="txt_snf" class="form-control" value="9.5" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                                        results += '<td><input id="txt_sales" class="form-control" value="0" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Sales"></td>';
                                        results += '<td style="display:none"><input id="txt_salesrate" onkeypress="return isFloat(event);" class="form-control" value="0" type="text" name="vendorcode"placeholder="Sales Rate"></td>';
                                        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                    }
                                    if (biproducttype == "92" && msg[i].biproductsshortname == "K") {
                                        results += '<tr>';
                                        results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                        results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                        results += '<td><input id="txt_ob" readonly class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                        results += '<td><input id="txt_production" class="form-control" onkeypress="return isFloat(event);" value="0" type="text" name="vendorcode"placeholder="Enter Production(kgs)"></td>';
                                        results += '<td style="display:none"><input id="txt_reciveqty" onkeypress="return isFloat(event);" class="form-control" value="0" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                        results += '<td><input id="txt_fat" class="form-control" value="3" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter fat"></td>';
                                        results += '<td><input id="txt_snf" class="form-control" value="9.5" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                                        results += '<td><input id="txt_sales" class="form-control" value="0" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Sales"></td>';
                                        results += '<td style="display:none"><input id="txt_salesrate" onkeypress="return isFloat(event);" class="form-control" value="0" type="text" name="vendorcode"placeholder="Sales Rate"></td>';
                                        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                    }
                                    if (biproducttype == "1211" && msg[i].biproductsshortname == "PS") {
                                        results += '<tr>';
                                        results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                        results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                        results += '<td><input id="txt_ob" readonly class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                        results += '<td><input id="txt_production" class="form-control" onkeypress="return isFloat(event);" value="0" type="text" name="vendorcode"placeholder="Enter Production(kgs)"></td>';
                                        results += '<td style="display:none"><input id="txt_reciveqty" onkeypress="return isFloat(event);" class="form-control" value="0" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                        results += '<td><input id="txt_fat" class="form-control" value="3" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter fat"></td>';
                                        results += '<td><input id="txt_snf" class="form-control" value="9.5" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                                        results += '<td><input id="txt_sales" class="form-control" value="0" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Sales"></td>';
                                        results += '<td style="display:none"><input id="txt_salesrate" onkeypress="return isFloat(event);" class="form-control" value="0" type="text" name="vendorcode"placeholder="Sales Rate"></td>';
                                        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                    }
                                    if (biproducttype == "1213" && msg[i].biproductsshortname == "BS") {
                                        results += '<tr>';
                                        results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                        results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                        results += '<td><input id="txt_ob" readonly class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                        results += '<td><input id="txt_production" class="form-control" onkeypress="return isFloat(event);" value="0" type="text" name="vendorcode"placeholder="Enter Production(kgs)"></td>';
                                        results += '<td style="display:none"><input id="txt_reciveqty" onkeypress="return isFloat(event);" class="form-control" value="0" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                        results += '<td><input id="txt_fat" class="form-control" value="3" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter fat"></td>';
                                        results += '<td><input id="txt_snf" class="form-control" value="9.5" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                                        results += '<td><input id="txt_sales" class="form-control" value="0" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Sales"></td>';
                                        results += '<td style="display:none"><input id="txt_salesrate" onkeypress="return isFloat(event);" class="form-control" value="0" type="text" name="vendorcode"placeholder="Sales Rate"></td>';
                                        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                    }
                                }
                            }
                            else {
                                if (msg[i].departmentid == "1" && msg[i].batchid != "16" && msg[i].batchid != "13") {
                                    results += '<tr>';
                                    results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                    results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                    results += '<td><input id="txt_ob" readonly class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                    results += '<td><input id="txt_reciveqty" class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                    results += '<td><input id="txt_fat" class="form-control" value="3" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter fat"></td>';
                                    results += '<td><input id="txt_snf" class="form-control" value="9.5" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                                    results += '<td><input id="txt_production" class="form-control" onkeyup="total_closingbalance(this);" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter Production(kgs)"></td>';
                                    results += '<td><input id="txt_sales" class="form-control" onkeyup="total_closingbalance(this);" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Sales"></td>';
                                    results += '<td><input id="txt_lossqty" class="form-control" onkeyup="total_closingbalance(this);" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Loss Qty"></td>';
                                    results += '<td><input id="txt_salesrate" class="form-control" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Sales Rate"></td>';
                                    if (branchidw == "26" || branchidw == "115") {
                                        results += '<td><input id="txt_cuttingqty" class="form-control" value="0" onkeyup="total_closingbalance(this);" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Cutting Qty"></td>';
                                        results += '<td><input id="txt_damageqty" class="form-control" value="0" onkeyup="total_closingbalance(this);" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Damage Qty"></td>';
                                        results += '<td><input id="txt_returnqty" class="form-control" value="0" onkeyup="total_closingbalance(this);" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Return Qty"></td>';
                                        results += '<td><input id="txt_closingbals" class="form-control" type="text" name="vendorcode"placeholder="Closing Balance" readonly></td>';
                                    }
                                    results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                }
                            }
                        }
                        results += '</table></div>';
                        $("#divFillScreen").html(results);
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
        function total_closingbalance(qtyid) {
            if (qtyid.value != "") {
                var ob = 0;
                ob = $(qtyid).closest("tr").find('#txt_ob').val();
                if (ob == "" || ob == null) {
                    ob = 0;
                }
                var production = 0;
                production = $(qtyid).closest("tr").find('#txt_production').val();
                if (production == "" || production == null) {
                    production = 0;
                }
                var sales = 0;
                sales = $(qtyid).closest("tr").find('#txt_sales').val();
                if (sales == "" || sales == null) {
                    sales = 0;
                }
                var lossqty = 0;
                lossqty = $(qtyid).closest("tr").find('#txt_lossqty').val();
                if (lossqty == "" || lossqty == null) {
                    lossqty = 0;
                }
                var cuttingqty = 0;
                cuttingqty = $(qtyid).closest("tr").find('#txt_cuttingqty').val();
                if (cuttingqty == "" || cuttingqty == null) {
                    cuttingqty = 0;
                }
                var returnqty = 0;
                returnqty = $(qtyid).closest("tr").find('#txt_returnqty').val();
                if (returnqty == "" || returnqty == null) {
                    returnqty = 0;
                }
                var damageqty = 0;
                damageqty = $(qtyid).closest("tr").find('#txt_damageqty').val();
                if (damageqty == "" || damageqty == null) {
                    damageqty = 0;
                }
                var closingbal = 0;
                closingbal = parseFloat(ob) + parseFloat(production) - parseFloat(sales) - parseFloat(lossqty) - parseFloat(cuttingqty) + parseFloat(returnqty) - parseFloat(damageqty);
                $(qtyid).closest("tr").find('#txt_closingbals').val(closingbal);
            }
        }
        function save_production_details_click() {
            var btnvalue = document.getElementById('save_batchdetails').innerHTML;
            var biproducttype = "";
            if (btnvalue == "Save") {
                biproducttype = document.getElementById('slct_biproducts').value;
            }
            else {
                biproducttype = biproductsshortname;
            }
            var producttype = document.getElementById('ddlproducttype').value;
            var remarks = document.getElementById('txt_Remarks').value;
            var date = document.getElementById('txt_date').value;
            if (date == "") {
                alert("Please select date");
                return false;
            }
            var sno = document.getElementById('lbl_sno').value;
            var pqty = document.getElementById('lbl_pqty').value;
            var rows = $("#table_shift_wise_details tr:gt(0)");
            var curd_production_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_production').val() != "") {
                    curd_production_details.push({ productid: $(this).find('#hdn_productid').val(), recive_qty: $(this).find('#txt_reciveqty').val(), fat: $(this).find('#txt_fat').val(), snf: $(this).find('#txt_snf').val(), production: $(this).find('#txt_production').val(),
                        sales: $(this).find('#txt_sales').val(), lossqty: $(this).find('#txt_lossqty').val(), curdsalerate: $(this).find('#txt_salesrate').val(), cuttingqty: $(this).find('#txt_cuttingqty').val(), returnqty: $(this).find('#txt_returnqty').val(), damageqty: $(this).find('#txt_damageqty').val()
                    });
                }
            });
            if (curd_production_details.length == 0) {
                alert("Please enter receive quantity");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_curd_production_click', 'curd_production_details': curd_production_details, 'remarks': remarks, 'btnvalue': btnvalue, 'sno': sno, 'pqty': pqty, 'date': date, 'biproducttype': biproducttype, 'producttype': producttype };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            Clearvalues();
                            get_curdproduction_details();
                            $('#Inwardsilo_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                            $('#diveditdata').css('display', 'none');
                            $('#div_Deptdata').show();
                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                CallHandlerUsingJson(data, s, e);
            }
        }
        function ctotal(qtyid) {
            if (qtyid.value == "") {
            }
            else {
                var ob = document.getElementById('txt_ob').value;
                if (ob == "") {
                    alert("Please enter O/B");
                    return false;
                }
                var recipts = qtyid.value;
                var creats = parseFloat(recipts) / 12;
                var sum = parseFloat(ob) + parseFloat(recipts);
                document.getElementById('txt_total').value = sum;
                document.getElementById('txt_creats').value = creats;
            }
        }
        function closingtotal(qtyid) {
            if (qtyid.value == "") {
            }
            else {
                var total = document.getElementById('txt_total').value;
                if (total == "") {
                    alert("Please enter total");
                    return false;
                }
                var recipts = qtyid.value;
                var sum = parseFloat(total) - parseFloat(recipts);
                document.getElementById('txt_closing').value = sum;
            }
        }

        function productchange() {
            var productid = document.getElementById('slct_product').value;
            var data = { 'op': 'get_productqty_details', 'productid': productid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('txt_ob').value = msg[i].quantity;
                        }
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
        function get_curdproduction_details() {
            var getdatadate = document.getElementById('txt_getdatadate').value;
            var data = { 'op': 'get_curdproduction_details', 'getdatadate': getdatadate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcurdproduction(msg);
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
        function fillcurdproduction(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">FAT</th><th scope="col" style="font-weight: bold;">SNF</th><th scope="col" style="font-weight: bold;">O/B</th><th scope="col" style="font-weight: bold;">Production</th><th scope="col" style="font-weight: bold;">Total</th><th scope="col" style="font-weight: bold;">Date</th><th style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" style="display:none" class="1" style="text-align:center;">' + msg[i].branchname + '</td>';
                results += '<td data-title="Capacity" class="2" >' + msg[i].productname + '</td>';
                results += '<td  class="3" >' + msg[i].fat + '</td>';
                results += '<td  class="4" >' + msg[i].snf + '</td>';
                results += '<td  class="5" >' + msg[i].ob + '</td>';
                results += '<td  class="6" >' + msg[i].production + '</td>';
                results += '<td  class="7" >' + msg[i].total + '</td>';
                results += '<td  class="8" >' + msg[i].doe + '</td>';
                results += '<td style="display:none" class="9" >' + msg[i].sales + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].quantity + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].remarks + '</td>';
                results += '<td style="display:none" class="13">' + msg[i].sno + '</td>';
                results += '<td style="display:none" class="15">' + msg[i].datetime + '</td>';
                results += '<td style="display:none" class="14">' + msg[i].biproductsshortname + '</td>';
                results += '<td style="display:none" class="20">' + msg[i].lossqty + '</td>';
                results += '<td style="display:none" class="21">' + msg[i].cuttingqty + '</td>';
                results += '<td style="display:none" class="22">' + msg[i].returnqty + '</td>';
                results += '<td style="display:none" class="23">' + msg[i].damageqty + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        var biproductsshortname = "";
        function getme(thisid) {
            var branchname = $(thisid).parent().parent().children('.1').html();
            var productname = $(thisid).parent().parent().children('.2').html();
            var fat = $(thisid).parent().parent().children('.3').html();
            var snf = $(thisid).parent().parent().children('.4').html();
            var ob = $(thisid).parent().parent().children('.5').html();
            var production = $(thisid).parent().parent().children('.6').html();
            var total = $(thisid).parent().parent().children('.7').html();
            var doe = $(thisid).parent().parent().children('.15').html();
            var sales = $(thisid).parent().parent().children('.9').html();
            var quantity = $(thisid).parent().parent().children('.10').html();
            var productid = $(thisid).parent().parent().children('.11').html();
            var remarks = $(thisid).parent().parent().children('.12').html();
            var sno = $(thisid).parent().parent().children('.13').html();
            biproductsshortname = $(thisid).parent().parent().children('.14').html();
            var lossqty = $(thisid).parent().parent().children('.20').html();
            var cuttingqty = $(thisid).parent().parent().children('.21').html();
            var returnqty = $(thisid).parent().parent().children('.22').html();
            var damageqty = $(thisid).parent().parent().children('.23').html();

            document.getElementById('txt_date').value = doe;
            if (biproductsshortname == "P") {
                biproductsshortname = "93";
            }
            else if (biproductsshortname == "K") {
                biproductsshortname = "92";
            }
            else if (biproductsshortname == "PS") {
                biproductsshortname = "1211";
            }
            else if (biproductsshortname == "BS") {
                biproductsshortname = "1213";
            }
            else if (biproductsshortname == "") {
                biproductsshortname = "";
            }
            //document.getElementById('slct_biproducts').value = biproductsshortname;
            document.getElementById('txt_Remarks').value = remarks;

            document.getElementById('lbl_sno').value = sno;
            document.getElementById('save_batchdetails').innerHTML = "Modify";

            if (biproductsshortname == "92" || biproductsshortname == "93" || biproductsshortname == "1211" || biproductsshortname == "1213") {
                $("#divFillScreen").show();
                var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" style="font-weight: 700;">Production(kgs)</th><th scope="col" id="tfat" style="font-weight: 700;">FAT</th><th scope="col" id="tsnf" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">Sales</th></tr></thead></tbody>';
                results += '<tr>';
                results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + sno + '</span></th>';
                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + productname + '</span></th>';
                results += '<td><input id="txt_ob" readonly class="form-control" onkeypress="return isFloat(event);" value="' + ob + '" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                results += '<td><input id="txt_production" class="form-control" onkeypress="return isFloat(event);" value="' + production + '" type="text" name="vendorcode"placeholder="Enter Production(kgs)"></td>';
                results += '<td><input id="txt_fat" class="form-control" onkeypress="return isFloat(event);" value="' + fat + '" type="text" name="vendorcode" placeholder="Enter fat"></td>';
                results += '<td><input id="txt_snf" class="form-control" onkeypress="return isFloat(event);" value="' + snf + '" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                results += '<td><input id="txt_sales" class="form-control" onkeypress="return isFloat(event);" value="' + sales + '" type="text" name="vendorcode"placeholder="Sales"></td>';
                results += '<td style="display:none"><input id="txt_reciveqty" onkeypress="return isFloat(event);" class="form-control" value="0" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + productid + '"></td></tr>';
                results += '</table></div>';
                $("#divFillScreen").html(results);
            }
            else {
                $("#divFillScreen").show();
                var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                //results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" id="trecive" style="font-weight: 700;">Receive Milk(Qty)</th><th scope="col" id="tfat" style="font-weight: 700;">FAT</th><th scope="col" id="tsnf" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">Production(kgs)</th><th scope="col" style="font-weight: 700;">Sales</th><th scope="col" style="font-weight: 700;">Loss</th><th scope="col" style="font-weight: 700;">Cutting Qty</th><th scope="col" style="font-weight: 700;">Return Qty</th></tr></thead></tbody>';
                if (branchidw == "26" || branchidw == "115") {
                    results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" id="trecive" style="font-weight: 700;">Receive Milk(Qty)</th><th scope="col" id="tfat" style="font-weight: 700;">FAT</th><th scope="col" id="tsnf" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">Production (kgs)</th><th scope="col" style="font-weight: 700;">Sales</th><th scope="col" style="font-weight: 700;">Loss Qty</th><th scope="col" style="font-weight: 700;">Cutting Qty</th><th scope="col" style="font-weight: 700;">Damage Qty</th><th scope="col" style="font-weight: 700;">Return Qty</th></tr></thead></tbody>';
                }
                else {
                    results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" id="trecive" style="font-weight: 700;">Receive Milk(Qty)</th><th scope="col" id="tfat" style="font-weight: 700;">FAT</th><th scope="col" id="tsnf" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">Production (kgs)</th><th scope="col" style="font-weight: 700;">Sales</th><th scope="col" style="font-weight: 700;">Loss Qty</th></tr></thead></tbody>';
                }
                results += '<tr>';
                results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + sno + '</span></th>';
                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + productname + '</span></th>';
                results += '<td><input id="txt_ob" readonly class="form-control" onkeypress="return isFloat(event);" value="' + ob + '" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                results += '<td><input id="txt_reciveqty" class="form-control" onkeypress="return isFloat(event);" value="' + quantity + '" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                results += '<td><input id="txt_fat" class="form-control" onkeypress="return isFloat(event);" value="' + fat + '" type="text" name="vendorcode"placeholder="Enter fat"></td>';
                results += '<td><input id="txt_snf" class="form-control" onkeypress="return isFloat(event);" value="' + snf + '" type="text" name="vendorcode"placeholder="Enter snf"></td>';
                results += '<td><input id="txt_production" class="form-control" onkeypress="return isFloat(event);" value="' + production + '" type="text" name="vendorcode"placeholder="Enter Production(kgs)"></td>';
                results += '<td><input id="txt_sales" class="form-control" onkeypress="return isFloat(event);" value="' + sales + '" type="text" name="vendorcode"placeholder="Sales"></td>';
                results += '<td><input id="txt_lossqty" class="form-control" onkeypress="return isFloat(event);" value="' + lossqty + '" type="text" name="vendorcode"placeholder="Loss Qty"></td>';
                if (branchidw == "26" || branchidw == "115") {
                    results += '<td><input id="txt_cuttingqty" class="form-control" value="' + cuttingqty + '" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Cutting Qty"></td>';
                    results += '<td><input id="txt_damageqty" class="form-control" value="' + damageqty + '" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Cutting Qty"></td>';
                    results += '<td><input id="txt_returnqty" class="form-control" value="' + returnqty + '" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Cutting Qty"></td>';
                }
                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + productid + '"></td></tr>';
                results += '</table></div>';
                $("#divFillScreen").html(results);
            }
            $('#divFillScreen').show();
            $('#showlogs').hide();
            $('#div_Deptdata').hide();
            $('#Inwardsilo_fillform').show();
            $('#td_product').hide();
            $('#td_producttype').hide();
        }
        function Clearvalues() {
            document.getElementById('txt_date').value = "";
            document.getElementById('ddlproducttype').selectedIndex = 0;
            document.getElementById('slct_biproducts').selectedIndex = 0;
            document.getElementById('txt_biob').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('lbl_sno').value = "";
            document.getElementById('save_batchdetails').innerHTML = "Save";
            get_curdproduction_details();
            $('#Inwardsilo_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#diveditdata').css('display', 'none');
            $('#div_Deptdata').show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Production Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Curd</a></li>
            <li><a href="#">Production Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Production Details
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
                            <input id="txt_getdatadate" class="form-control" type="date" name="vendorcode" />
                        </td>
                         <td style="width: 5px;">
                          </td>
                        <td>
                         <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="get_curdproduction_details()"></span> <span onclick="get_curdproduction_details()">Generate</span>
                          </div>
                          </div>
                        </td>
                            <td style="padding-left: 650px;">
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addcurdproductiondetails()"></span> <span onclick="addcurdproductiondetails()">Add Details</span>
                          </div>
                          </div>
                            </td>
                    </tr>
                </table>
                </div>
                <div>
                <div id="div_Deptdata">
                </div>
                </div>
                <div id='Inwardsilo_fillform' style="display: none;">
                    <div style="padding-left: 20%;">
                        <table align="center">
                            <tr>
                                <td>
                                    <label>
                                        Date <span style="color: red;">*</span></label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                        placeholder="Enter Date">
                                </td>
                                <td>
                                </td>
                                <td id="td_product">
                                    <label>
                                        Product Type<span style="color: red;">*</span></label>
                                </td>
                                <td id="td_producttype">
                                    <select id="ddlproducttype" class="form-control" onchange="onproductchange()">
                                        <option value="0">Select Products</option>
                                        <option value="1">BI-PRODUCTS</option>
                                        <option value="2">CURD & BUTTER MILK</option>
                                    </select>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="biproduct" style="padding-top: 15px; padding-left: 20%; display: none;">
                        <table align="center">
                            <tr id="td_bitype">
                                <td>
                                    <label>
                                        Type<span style="color: red;">*</span></label>
                                </td>
                                <td style="width: 212px;">
                                    <select id="slct_biproducts" class="form-control" onchange="ontypechange()">
                                        <option value="0" selected value disabled>Select Bi-Product</option>
                                        <%--<option value="93">PANEER</option>
                                        <option value="92">KHOVA LOOSE</option>--%>
                                    </select>
                                </td>
                                <td>
                                    <label>
                                        O/B<span style="color: red;">*</span></label>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_biob" class="form-control" name="vendorcode"
                                        placeholder="O/B">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divFillScreen">
                    </div>
                    <div style="padding-left:350px;">
                        <table align="center">
                            <tr>
                                <td>
                                    <label>
                                        Remarks</label>
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
                            <tr hidden>
                                <td>
                                    <label id="lbl_pqty">
                                    </label>
                                </td>
                            </tr>
                            </table>
                        <div  style="padding-left: 10%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_batchdetails1" onclick="save_production_details_click()"></span><span id="save_batchdetails" onclick="save_production_details_click()">Save</span>
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
                </div>
                <div id="diveditdata" style="display:none;">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
