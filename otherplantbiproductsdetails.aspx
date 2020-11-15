<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="otherplantbiproductsdetails.aspx.cs" Inherits="otherplantbiproductsdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#div_odetails').css('display', 'block');
            $('#div_otherdetailsreport').css('display', 'none');
            $('#div_othbiprod').css('display', 'none');
            $('#div_othbiprodrep').css('display', 'none');
            fillgheeproducts();
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
            $('#txt_odate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#txt_orfromdate').val(yyyy + '-' + mm + '-' + dd);
            $('#txt_ortodate').val(yyyy + '-' + mm + '-' + dd);
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
        function showotherproducts() {
            $('#div_odetails').css('display', 'block');
            $('#div_otherdetailsreport').css('display', 'none');
            $('#div_othbiprod').css('display', 'none');
            $('#div_othbiprodrep').css('display', 'none');
        }
        function showotherproductsreport() {
            $('#div_odetails').css('display', 'none');
            $('#div_otherdetailsreport').css('display', 'block');
            $('#div_othbiprod').css('display', 'none');
            $('#div_othbiprodrep').css('display', 'none');
        }
        function showotherbiproducts() {
            $('#div_odetails').css('display', 'none');
            $('#div_otherdetailsreport').css('display', 'none');
            $('#div_othbiprod').css('display', 'block');
            $('#div_othbiprodrep').css('display', 'none');
            get_product_details();
            get_otherbiproducts_details();
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
            $('#txt_obidate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        }
        function showotherbiproductsreport() {
            $('#div_odetails').css('display', 'none');
            $('#div_otherdetailsreport').css('display', 'none');
            $('#div_othbiprod').css('display', 'none');
            $('#div_othbiprodrep').css('display', 'block');
            get_product_details();
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
        function fillgheeproducts() {
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div    style="overflow:auto;"><table id="table_sales_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Product Name</th><th scope="col">Quantity(O/B)</th><th scope="col">Inward</th><th scope="col">Outward</th></tr></thead></tbody>';
                        var k = 1;
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].departmentid == "16" && msg[i].batchid == "30") {
                                results += '<tr>';
                                results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + (k++) + '</span></th>';
                                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                results += '<td><input id="txt_openingbalance" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter qty" readonly></td>';
                                results += '<td><input id="txt_production" class="form-control" onkeypress="return isFloat(event);" value="0" type="text" name="vendorcode"placeholder="Enter Inward"></td>';
                                results += '<td><input id="txt_sales" class="form-control" value="0" onkeypress="return isFloat(event);"  type="text" name="vendorcode"placeholder="Enter Outward"></td>';
                                //results += '<td><input id="txt_closingbalance" class="form-control" value="0" type="text" name="vendorcode"placeholder="Enter Sales"></td>';
                                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                            }
                        }
                        results += '</table></div>';
                        $("#div_getdetails").html(results);
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
        function save_otherbiproducts_sales_click() {
            var date = document.getElementById('txt_odate').value;
            if (date == "") {
                alert("Please Enter Date");
                return false;
            }
            var sno = document.getElementById('lbl_osno').value;
            var remarks = document.getElementById('txt_oremarks').value;
            var btnvalue = document.getElementById('btnproducts').innerHTML;
            var rows = $("#table_sales_wise_details tr:gt(0)");
            var obip_closing_details = new Array();
            //         $(rows).each(function (i, obj) {
            //             if ($(this).find('#txt_dispatchqty').val() != 0) {
            //                 obip_closing_details.push({ productid: $(this).find('#hdn_productid').val(), openingbalance: $(this).find('#txt_openingbalance').val(), production: $(this).find('#txt_production').val(), sales: $(this).find('#txt_sales').val() });
            //             }
            //         });
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_production').val() == "0") {
                }
                else {
                    obip_closing_details.push({ productid: $(this).find('#hdn_productid').val(), openingbalance: $(this).find('#txt_openingbalance').val(), production: $(this).find('#txt_production').val(), sales: $(this).find('#txt_sales').val() });
                }
            });
            if (obip_closing_details.length == 0) {
                alert("Please Enter Inward & Outward");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_otherbiproducts_sales_click', 'obip_closing_details': obip_closing_details, 'date': date, 'remarks': remarks, 'btnvalue': btnvalue, 'sno': sno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
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
        function get_otherproducts_details_click() {
            var fromdate = document.getElementById('txt_orfromdate').value;
            var todate = document.getElementById('txt_ortodate').value;
            var data = { 'op': 'get_otherproducts_details_click', 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillproductiondetails(msg);
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
        function fillproductiondetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">SNO</th><th scope="col">DATE</th><th scope="col">PRODUCTNAME</th><th scope="col">OPENINGBALANCE</th><th scope="col">INWARD</th><th scope="col">ISSUE</th><th scope="col">CLOSINGBALANCE</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td class="3" style="text-align:center;">' + msg[i].sno + '</td>';
                results += '<td class="3" style="text-align:center;">' + msg[i].date + '</td>';
                results += '<td class="3" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].openingbalance + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].production + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].sales + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].closingbalance + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_getotherreport").html(results);
        }
        //
        function save_otherbiproducts_details() {
            var date = document.getElementById('txt_obidate').value;
            if (date == "") {
                alert("Select Date.");
                return false;
            }
            var openingbalance = document.getElementById('txt_obiopeningbalance').value;
            if (openingbalance == "") {
                alert("Enter Opening Balance.");
                return false;
            }
            var rawmetarial = document.getElementById('txt_obirawmetarial').value;
            if (rawmetarial == "") {
                alert("Enter Raw MEterial Qty.");
                return false;
            }
            var production = document.getElementById('txt_obiproductionqty').value;
            if (production == "") {
                alert("Enter Production Qty.");
                return false;
            }
            var despatch = document.getElementById('txt_obidespatchqty').value;
            if (despatch == "") {
                alert("Enter despatch.");
                return false;
            }
            var stocktransfer = document.getElementById('txt_obistocktransferqty').value;
            if (stocktransfer == "") {
                alert("Enter Stock Transfer.");
                return false;
            }
            var closingbalance = document.getElementById('txt_oboclosingbalance').value;
            if (closingbalance == "") {
                alert("Enter closing balance.");
                return false;
            }
            var remarks = document.getElementById('txt_obiremarks').value;
            var btnval = document.getElementById('btn_obisave').innerHTML;
            var sno = document.getElementById('hdn_obisno').value;
            var productname = document.getElementById('slct_obiproductname').value;
            if (productname == "") {
                alert("Select Product Name.");
                return false;
            }
            var handlingloss = document.getElementById('txt_obphandlingloss').value;
            if (handlingloss == "") {
                alert("Enter handling loss.");
                return false;
            }
            var suger = document.getElementById('txt_obisuger').value;
            var ghee = document.getElementById('txt_obighee').value;
            var smp = document.getElementById('txt_obismp').value;
            var other = document.getElementById('txt_obiother').value;
            var data = { 'op': 'save_otherbiproducts_details', 'date': date, 'openingbalance': openingbalance, 'rawmetarial': rawmetarial, 'production': production, 'despatch': despatch,
                'stocktransfer': stocktransfer, 'closingbalance': closingbalance, 'remarks': remarks, 'btnval': btnval, 'sno': sno, 'productname': productname, 'handlingloss': handlingloss,
                'suger': suger, 'ghee': ghee, 'smp': smp, 'other': other
            };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        get_otherbiproducts_details();
                        otherbiproducts_clear();
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
        function get_product_details() {
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails(msg);
                        filldetails_rep(msg);
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
            var data = document.getElementById('slct_obiproductname');
            var length = data.options.length;
            document.getElementById('slct_obiproductname').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Product Name";
            opt.value = "";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].productname != null) {
                    if (msg[i].batchid == "32") {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].productname;
                        option.value = msg[i].productid;
                        data.appendChild(option);
                    }
                }
            }
        }
        function get_otherbiproducts_details() {
            var data = { 'op': 'get_otherbiproducts_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillobpdetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillobpdetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Date</th><th style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">OB</th><th scope="col" style="font-weight: bold;">Raw Material</th><th scope="col" style="font-weight: bold;">HandlingLoss</th><th scope="col" style="font-weight: bold;">Prooduction</th><th scope="col" style="font-weight: bold;">Despatch</th><th scope="col" style="font-weight: bold;">Stock Transfer</th><th scope="col" style="font-weight: bold;">CB</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Code" class="5">' + msg[i].doe + '</td>';
                results += '<td data-title="Code" class="10">' + msg[i].productname + '</td>';
                results += '<td data-title="Code" class="7">' + msg[i].ob + '</td>';
                results += '<td data-title="Code" class="8">' + msg[i].recivedqty + '</td>';
                results += '<td data-title="Code" class="9">' + msg[i].lossqty + '</td>';
                results += '<td data-title="Code" class="5">' + msg[i].productionqty + '</td>';
                results += '<td data-title="Code" class="10">' + msg[i].sales + '</td>';
                results += '<td data-title="Code" class="7">' + msg[i].issue + '</td>';
                results += '<td data-title="Code" class="8">' + msg[i].cb + '</td>';
                results += '<td data-title="Code"  style="display:none;" class="12">' + msg[i].datetime + '</td>';
                results += '<td data-title="Code"  style="display:none;" class="12">' + msg[i].sno + '</td>';
                results += '<td data-title="Code" style="display:none;" class="11">' + msg[i].productid + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_getobip").html(results);
        }
        function otherbiproducts_clear() {
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
            $('#txt_obidate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            document.getElementById('txt_obiopeningbalance').value = "";
            document.getElementById('txt_obirawmetarial').value = "";
            document.getElementById('txt_obiproductionqty').value = "";
            document.getElementById('txt_obidespatchqty').value = "";
            document.getElementById('txt_obistocktransferqty').value = "";
            document.getElementById('txt_oboclosingbalance').value = "";
            document.getElementById('txt_obiremarks').value = "";
            document.getElementById('btn_obisave').innerHTML = "Save";
            document.getElementById('hdn_obisno').value = "";
            document.getElementById('slct_obiproductname').selectedIndex = 0;
            document.getElementById('txt_obphandlingloss').value = "";
        }
        function fill_opening_balance() {
            var productname = document.getElementById('slct_obiproductname').value;
            var data = { 'op': 'fill_opening_balance', 'productname': productname };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        document.getElementById('txt_obiopeningbalance').value = msg;
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function change_productionqty() {
            var rawmetaril = document.getElementById('txt_obirawmetarial').value;
            var handlingloss = document.getElementById('txt_obphandlingloss').value;
            rawmetaril = parseFloat(rawmetaril) || 0;
            handlingloss = parseFloat(handlingloss) || 0;
            document.getElementById('txt_obiproductionqty').value = rawmetaril - handlingloss;
        }
        function change_cloasingbalance() {
            var ob = document.getElementById('txt_obiopeningbalance').value;
            var prod = document.getElementById('txt_obiproductionqty').value;
            var des = document.getElementById('txt_obidespatchqty').value;
            var stocktrans = document.getElementById('txt_obistocktransferqty').value;
            ob = parseFloat(ob) || 0;
            prod = parseFloat(prod) || 0;
            des = parseFloat(des) || 0;
            stocktrans = parseFloat(stocktrans) || 0;
            document.getElementById('txt_oboclosingbalance').value = (ob + prod) - (des + stocktrans);
        }
        function fill_total_raw_material() {
            var suger = document.getElementById('txt_obisuger').value;
            var ghee = document.getElementById('txt_obighee').value;
            var smp = document.getElementById('txt_obismp').value;
            var other = document.getElementById('txt_obiother').value;
            suger = parseFloat(suger) || 0;
            ghee = parseFloat(ghee) || 0;
            smp = parseFloat(smp) || 0;
            other = parseFloat(other) || 0;
            document.getElementById('txt_obirawmetarial').value = suger + ghee + smp + other;
        }
        //
        function filldetails_rep(msg) {
            var data = document.getElementById('slct_productname');
            var length = data.options.length;
            document.getElementById('slct_productname').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Product Name";
            opt.value = "";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].productname != null) {
                    if (msg[i].batchid == "32") {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].productname;
                        option.value = msg[i].productid;
                        data.appendChild(option);
                    }
                }
            }
        }
        function get_other_biproducts_report_details() {
            var productname = document.getElementById('slct_productname').value;
            if (productname == "") {
                alert("Select Product Name");
                return false;
            }
            var fromdate = document.getElementById('txt_fromdate').value;
            if (fromdate == "") {
                alert("Select From Date");
                return false;
            }
            var todate = document.getElementById('txt_todate').value;
            if (todate == "") {
                alert("Select To Date");
                return false;
            }
            var data = { 'op': 'get_other_biproducts_report_details', 'fromdate': fromdate, 'todate': todate, 'productname': productname };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillobprepdetails(msg);
                        document.getElementById('spn_fromdate').innerHTML = document.getElementById('txt_fromdate').value;
                        document.getElementById('spn_todate').innerHTML = document.getElementById('txt_todate').value;
                    }
                    else {
                        msg = 0;
                        fillobprepdetails(msg);
                        document.getElementById('spn_fromdate').innerHTML = document.getElementById('txt_fromdate').value;
                        document.getElementById('spn_todate').innerHTML = document.getElementById('txt_todate').value;
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillobprepdetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Date</th><th style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">OB</th><th scope="col" style="font-weight: bold;">Raw Material</th><th scope="col" style="font-weight: bold;">HandlingLoss</th><th scope="col" style="font-weight: bold;">Prooduction</th><th scope="col" style="font-weight: bold;">Despatch</th><th scope="col" style="font-weight: bold;">Stock Transfer</th><th scope="col" style="font-weight: bold;">CB</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var tprodqty = 0;
            var tsales = 0;
            var tisuue = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Code" class="5">' + msg[i].doe + '</td>';
                results += '<td data-title="Code" class="10">' + msg[i].productname + '</td>';
                results += '<td data-title="Code" class="7">' + parseFloat(msg[i].ob).toFixed(2) + '</td>';
                results += '<td data-title="Code" class="8">' + parseFloat(msg[i].recivedqty).toFixed(2) + '</td>';
                results += '<td data-title="Code" class="9">' + parseFloat(msg[i].lossqty).toFixed(2) + '</td>';
                results += '<td data-title="Code" class="5">' + parseFloat(msg[i].productionqty).toFixed(2) + '</td>';
                results += '<td data-title="Code" class="10">' + parseFloat(msg[i].sales).toFixed(2) + '</td>';
                results += '<td data-title="Code" class="7">' + parseFloat(msg[i].issue).toFixed(2) + '</td>';
                results += '<td data-title="Code" class="8">' + parseFloat(msg[i].cb).toFixed(2) + '</td>';
                results += '<td data-title="Code"  style="display:none;" class="12">' + msg[i].datetime + '</td>';
                results += '<td data-title="Code"  style="display:none;" class="12">' + msg[i].sno + '</td>';
                results += '<td data-title="Code" style="display:none;" class="11">' + msg[i].productid + '</td></tr>';
                tprodqty += parseFloat(msg[i].productionqty);
                tsales += parseFloat(msg[i].sales);
                tisuue += parseFloat(msg[i].issue);
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '<tr>';
            results += '<td data-title="Code" class="5"></td>';
            results += '<td data-title="Code" class="10">Total</td>';
            results += '<td data-title="Code" class="7"></td>';
            results += '<td data-title="Code" class="8"></td>';
            results += '<td data-title="Code" class="9"></td>';
            results += '<td data-title="Code" class="5">' + parseFloat(tprodqty).toFixed(2) + '</td>';
            results += '<td data-title="Code" class="10">' + parseFloat(tsales).toFixed(2) + '</td>';
            results += '<td data-title="Code" class="7">' + parseFloat(tisuue).toFixed(2) + '</td>';
            results += '<td data-title="Code" class="8"></td></tr>';
            results += '</table></div>';
            $("#div_getdet").html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
           Other Products Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#">Other Products Details</a></li>
        </ol>
    </section>
    <section class="content">
     <div class="box box-info">
            <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showotherproducts()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Other Products</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showotherproductsreport()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Other Products Report</a></li>
                    <li id="Li1" class=""><a data-toggle="tab" href="#" onclick="showotherbiproducts()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Other Bi-Products</a></li>
                    <li id="Li2" class=""><a data-toggle="tab" href="#" onclick="showotherbiproductsreport()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Other Bi-Products Report</a></li>
                </ul>
            </div>
            <div id="div_odetails" style="display:none;">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Other Products Details</h3>
                </div>
                <div class="box-body">
                <div style="padding-left: 40%;">
                    <table>
                        <tr>
                        <td>
                            <label>
                                Date : <span style="color: red;">*</span></label>
                        </td>
                        <td>
                            <input id="txt_odate" class="form-control" type="datetime-local" name="vendorcode" />
                        </td>
                        </tr>
                    </table>
                </div>
                <div id="div_getdetails">
                </div>
                <div  align="center">
                    <table>
                        <tr hidden>
                            <td>
                                <label id="lbl_osno">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Remarks</label>
                                <textarea rows="3" cols="45" id="txt_oremarks" class="form-control" maxlength="200"
                                    placeholder="Enter Remarks"></textarea>
                            </td>
                        </tr>
                        </table>
                        <br />
                        <%--<table>
                        <tr>
                            <td>
                                <input id='btnproducts' type="button" class="btn btn-success" name="submit" value='Save'
                                    onclick="save_otherbiproducts_sales_click()" />
                                <input id='btnclear' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="otherproductsclearclearvalues()" />
                            </td>
                        </tr>
                    </table>--%>
                    <div  style="padding-left: 4%;">
                    <table>
                    <tr>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                            <span class="glyphicon glyphicon-ok" id="btnproducts1" onclick="save_otherbiproducts_sales_click()"></span><span id="btnproducts" onclick="save_otherbiproducts_sales_click()">Save</span>
                        </div>
                        </div>
                        </td>
                        <td style="width:10px;"></td>
                        <td>
                            <div class="input-group">
                            <div class="input-group-close">
                            <span class="glyphicon glyphicon-remove" id='btnclear1' onclick="otherproductsclearclearvalues()"></span><span id='btnclear' onclick="otherproductsclearclearvalues()">Close</span>
                        </div>
                        </div>
                        </td>
                        </tr>
                        </table>
                    </div>
                </div>
                </div>
                </div>
            <div id="div_otherdetailsreport" style="display:none;">
            <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Other Products Report</h3>
                    </div>
                    <div >
                    <div class="box-body">
                <div align="center">
                    <table>
                        <tr>
                        <td>
                            <label>
                                From Date : <span style="color: red;">*</span></label>
                        </td>
                        <td>
                            <input id="txt_orfromdate" class="form-control" type="date"/>
                        </td>
                        <td style="width: 6px;">
                            </td>
                        <td>
                            <label>
                                To Date : <span style="color: red;">*</span></label>
                        </td>
                        <td>
                            <input id="txt_ortodate" class="form-control" type="date" />
                        </td>
                        <td style="width: 6px;">
                            </td>
                            <td>
                                <input id='Button1' type="button" class="btn btn-success" name="submit" value='Generate'
                                    onclick="get_otherproducts_details_click()" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_getotherreport">
                </div>
                <br />
                    <table style="width: 100%;">
                    <tr>
                        <td style="width: 25%;">
                            <span style="font-weight: bold; font-size: 12px;">INCHARGE SIGNATURE</span>
                        </td>
                        <td style="width: 25%;">
                            <span style="font-weight: bold; font-size: 12px;">AUTHORISED SIGNATURE</span>
                        </td>
                        <td style="width: 25%;">
                            <span style="font-weight: bold; font-size: 12px;">PREPARED BY</span>
                        </td>
                    </tr>
                </table>
                </div>
                </div>
            </div>
            <div id="div_othbiprod" style="display:none;">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Other Bi-Products
                        </h3>
                    </div>
                    <div class="box-body">
                      <div style="text-align: -webkit-center;">
                        <table>
                            <tr>
                                <td>
                                    <label>Date : 
                                    </label>
                                </td>
                                <td style="height:40px;">
                                    <input id="txt_obidate" class="form-control" type="datetime-local"  />
                                </td>
                            <td style="width:3%;"></td>
                                <td>
                                    <label>Product Name : 
                                    </label>
                                </td>
                                <td style="height:40px;">
                                    <select id="slct_obiproductname" class="form-control" onchange="fill_opening_balance();">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Opening Balance : 
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_obiopeningbalance" class="form-control" type="text" readonly />
                                </td>
                            <td style="width:3%;"></td>
                               <td>
                                    <label>Suger : 
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_obisuger" class="form-control" type="text" value="0" onchange="fill_total_raw_material()" />
                                </td>
                            </tr>
                            <tr>
                               <td>
                                    <label>Ghee : 
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_obighee" class="form-control" type="text" value="0" onchange="fill_total_raw_material()" />
                                </td>
                              <td style="width:3%;"></td>
                               <td>
                                    <label>SMP : 
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_obismp" class="form-control" type="text" value="0" onchange="fill_total_raw_material()"  />
                                </td>
                             </tr>
                            <tr>
                               <td>
                                    <label>Other : 
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_obiother" class="form-control" type="text" value="0" onchange="fill_total_raw_material()"  />
                                </td>
                            <td style="width:3%;"></td>
                               <td>
                                    <label>Total Raw Material : 
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_obirawmetarial" class="form-control" type="text" readonly/>
                                </td>
                             </tr>
                            <tr>
                                <td>
                                    <label>Production Qty : 
                                    </label>
                                </td>
                                <td style="height:40px;">
                                    <input id="txt_obiproductionqty" class="form-control" type="text" readonly />
                                </td>
                             <td style="width:3%;"></td>
                                <td>
                                    <label>Handling Loss : 
                                    </label>
                                </td>
                                <td style="height:40px;">
                                    <input id="txt_obphandlingloss" class="form-control" type="text" onchange="change_productionqty();"/>
                                </td>
                           </tr>
                           <tr>
                                <td>
                                    <label>Despatch Qty : 
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_obidespatchqty" class="form-control" type="text" onchange="change_cloasingbalance();" value="0" />
                                </td>
                             <td style="width:3%;"></td>
                               <td>
                                    <label>Stock Transfer Qty : 
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_obistocktransferqty" class="form-control" type="text" onchange="change_cloasingbalance();" value="0" />
                                </td>
                          </tr>
                          <tr>
                                <td>
                                    <label>Closing Balance : 
                                    </label>
                                </td>
                                <td style="height:40px;">
                                    <input id="txt_oboclosingbalance" class="form-control" type="text"  readonly/>
                                </td>
                            <td style="width:3%;"></td>
                                <td>
                                    <label>Remarks : 
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_obiremarks" class="form-control" type="text" />
                                </td>
                            </tr>
                            <tr>
                            <td>
                            <input id="hdn_obisno" hidden />
                            </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                        <div  style="text-align: -webkit-center;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="Span1" onclick="save_otherbiproducts_details()"></span><span id="btn_obisave" onclick="save_otherbiproducts_details()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='Span3' onclick="otherbiproducts_clear()"></span><span id='Span4' onclick="otherbiproducts_clear()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                    <br />
                    <div id="div_getobip">
                    </div>
                </div>
            </div>
            <div id="div_othbiprodrep" style="display:none;">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Other Bi-Products Report
                        </h3>
                    </div>
                    <div class="box-body">
                    <div style="text-align: -webkit-center;">
                        <table>
                            <tr>
                                 <td>
                                    <label>Product Name
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <select id="slct_productname" class="form-control"></select>
                                </td>
                            <td style="width:3%;"></td>
                                <td>
                                    <label>From Date
                                    </label>
                                </td>
                                <td style="height:40px;"> 
                                    <input id="txt_fromdate" class="form-control" type="date"  />
                                </td>
                            <td style="width:3%;"></td>
                                <td>
                                    <label>To Date
                                    </label>
                                </td>
                                <td style="height:40px;">
                                    <input id="txt_todate" class="form-control" type="date"  />
                                </td>
                            <td style="width:3%;"></td>
                                <td>
                                    <input class="btn btn-primary" type="button" value="Generate"  id="btn_generate" onclick="get_other_biproducts_report_details();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div align="center">
                     <table>
                        <tr>
                            <td>
                                 <label style="font-weight: 800 !important;font-size: 26px !important;" >Sri Vyshnavi Dairy Specialities Pvt Ltd</label>
                            </td>
                        </tr>
                        <br />
                        <tr>
                            <td style="text-align: center;">
                                 <span style="font-size: 18px; font-weight: bold; color: #0252aa;" id="Span2">Other Plant Deatils</span>
                            </td>
                        </tr>
                        <br />
                        <tr>
                            <td style="text-align: center;">
                                 <span style="font-size: 18px; font-weight: bold; color: #0252aa;">From Date : </span>
                                 <span style="font-size: 18px; font-weight: bold; color: red;text-align: center;padding-left: 4%;" id="spn_fromdate"></span>
                                 <span style="font-size: 18px; font-weight: bold; color: #0252aa;">To Date : </span>
                                 <span style="font-size: 18px; font-weight: bold; color: red;text-align: center;padding-left: 4%;" id="spn_todate"></span>
                         </td>
                      </tr>
                      <br />
                    </table>
                   </div>
                   <br />
                    <div id="div_getdet">
                    </div>
                    </div>
                     </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
