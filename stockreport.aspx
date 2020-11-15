<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="stockreport.aspx.cs" Inherits="stockreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#head1').css('display', 'none');
            $('#head2').css('display', 'none');
            $('#head3').css('display', 'none');
            $('#head4').css('display', 'none');
            $('#head5').css('display', 'none');
            $('#head6').css('display', 'none');
            $('#head7').css('display', 'none');
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
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd);
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
        function get_stockdetails_click() {
            $('#head1').css('display', 'block');
            $('#head2').css('display', 'block');
            $('#head3').css('display', 'block');
            $('#head4').css('display', 'block');
            $('#head5').css('display', 'block');
            $('#head6').css('display', 'block');
            $('#head7').css('display', 'block');
            get_curdproduction();
            get_gheeproduction();
            get_butterproduction();
            get_creamproduction();
            get_smpproduction();
            get_biproductproduction();
            get_other_biproducts_stock_details();
        }
        function get_curdproduction() {
            var doe = document.getElementById('txt_date').value;
            var data = { 'op': 'get_curdstockdetails', 'doe': doe };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcurdproductiondetails(msg);
                    }
                    else {
                        msg = 0;
                        fillcurdproductiondetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillcurdproductiondetails(msg) {
         var branchname = '<%=Session["Branch_ID"] %>';
         if (branchname == "26" || branchname == "115") {
             var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
             results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Opening Stock</th><th scope="col">Recive Milk Qty</th><th scope="col">Production</th><th scope="col">Sales</th><th scope="col">Loss Qty</th><th scope="col">Damage Qty</th><th scope="col">Cutting Qty</th><th scope="col">Return Qty</th><th scope="col">Closing Balance</th><th></th></tr></thead></tbody>';
             for (var i = 0; i < msg.length; i++) {
                 results += '<tr>';
                 results += '<td scope="row" class="1" style="text-align:center;">' + parseFloat(msg[i].OpeningStock).toFixed(2) + '</td>';
                 results += '<td data-title="Capacity" class="2">' + parseFloat(msg[i].recivemilkqty).toFixed(2) + '</td>';
                 results += '<td data-title="Capacity" class="2">' + parseFloat(msg[i].production).toFixed(2) + '</td>';
                 results += '<td data-title="Capacity" class="2">' + parseFloat(msg[i].sales).toFixed(2) + '</td>';
                 results += '<td data-title="Capacity" class="2">' + parseFloat(msg[i].lossqty).toFixed(2) + '</td>';
                 results += '<td data-title="Capacity" class="2">' + parseFloat(msg[i].damageqty).toFixed(2) + '</td>';
                 results += '<td data-title="Capacity" class="2">' + parseFloat(msg[i].cuttingqty).toFixed(2) + '</td>';
                 results += '<td data-title="Capacity" class="2">' + parseFloat(msg[i].returnqty).toFixed(2) + '</td>';
                 var OpeningStock = parseFloat(msg[i].OpeningStock);
                 var production = parseFloat(msg[i].production);
                 var sales = parseFloat(msg[i].sales);
                 var lossqty = parseFloat(msg[i].lossqty);
                 var damageqty = parseFloat(msg[i].damageqty);
                 var cuttingqty = parseFloat(msg[i].cuttingqty);
                 var returnqty = parseFloat(msg[i].returnqty);
                 var add = OpeningStock + production + returnqty;
                 var sub = sales + lossqty + damageqty + cuttingqty;
                 results += '<td data-title="Capacity" class="2">' + parseFloat((add - sub)).toFixed(2) + '</td>';
                 results += '<td><input id="btn_poplate" type="button"  data-toggle="modal" data-target="#myModal" onclick="getcurdstock(this)" name="submit" class="btn btn-primary" value="View"/></td></tr>';
             }
             results += '</table></div>';
             $("#div_Deptdata").html(results);
         }
         else {
             var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
             results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Opening Stock</th><th scope="col">Recive Milk Qty</th><th scope="col">Production</th><th scope="col">Sales</th><th scope="col">Closing Balance</th><th></th></tr></thead></tbody>';
             for (var i = 0; i < msg.length; i++) {
                 results += '<tr>';
                 results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].OpeningStock + '</td>';
                 results += '<td data-title="Capacity" class="2">' + msg[i].recivemilkqty + '</td>';
                 results += '<td data-title="Capacity" class="2">' + msg[i].production + '</td>';
                 results += '<td data-title="Capacity" class="2">' + msg[i].sales + '</td>';
                 results += '<td data-title="Capacity" class="2">' + msg[i].curdclosingbalance + '</td>';
                 results += '<td><input id="btn_poplate" type="button"  data-toggle="modal" data-target="#myModal" onclick="getcurdstock(this)" name="submit" class="btn btn-primary" value="View"/></td></tr>';
             }
             results += '</table></div>';
             $("#div_Deptdata").html(results);
         }
        }
        //divcurdstock
        function getcurdstock(thisid) {
            var doe = document.getElementById('txt_date').value;
            var data = { 'op': 'get_productwisecurdstockdetails', 'doe': doe };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcurdstock(msg);
                    }
                    else {
                        msg = 0;
                        fillcurdstock(msg);
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
        function fillcurdstock(msg) {
            var branchname = '<%=Session["Branch_ID"] %>';
            if (branchname == "26" || branchname == "115") {
                var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Product Name</th><th scope="col">Opening Stock</th><th scope="col">Recive Milk Qty</th><th scope="col">Production</th><th scope="col">Sales</th><th scope="col">Loss Qty</th><th scope="col">Damage Qty</th><th scope="col">Cutting Qty</th><th scope="col">Return Qty</th><th scope="col">Closing Balance</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    var OpeningStock = parseFloat(msg[i].OpeningStock);
                    var production = parseFloat(msg[i].production);
                    var sales = parseFloat(msg[i].sales);
                    var lossqty = parseFloat(msg[i].lossqty);
                    var damageqty = parseFloat(msg[i].damageqty);
                    var cuttingqty = parseFloat(msg[i].cuttingqty);
                    var returnqty = parseFloat(msg[i].returnqty);
                    var add = OpeningStock + production + returnqty;
                    var sub = sales + lossqty + damageqty + cuttingqty;
                    var tot = add - sub;
                    if (OpeningStock > 0 || production > 0) {
                        results += '<tr>';
                        results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                        results += '<td class="1">' + parseFloat(msg[i].OpeningStock).toFixed(2) + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].recivemilkqty).toFixed(2) + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].production).toFixed(2) + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].sales).toFixed(2) + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].lossqty).toFixed(2) + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].damageqty).toFixed(2) + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].cuttingqty).toFixed(2) + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].returnqty).toFixed(2) + '</td>';
                        results += '<td class="3">' + parseFloat(tot).toFixed(2) + '</td></tr>';
                    }
                }
                results += '</table></div>';
                $("#divcurdstock").html(results);
            }
            else {
                var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Product Name</th><th scope="col">Opening Stock</th><th scope="col">Recive Milk Qty</th><th scope="col">Production</th><th scope="col">Sales</th><th scope="col">Closing Balance</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr>';
                    results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                    results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].OpeningStock + '</td>';
                    results += '<td data-title="Capacity" class="2">' + msg[i].recivemilkqty + '</td>';
                    results += '<td data-title="Capacity" class="2">' + msg[i].production + '</td>';
                    results += '<td data-title="Capacity" class="2">' + msg[i].sales + '</td>';
                    results += '<td class="3">' + msg[i].curdclosingbalance + '</td></tr>';
                }
                results += '</table></div>';
                $("#divcurdstock").html(results);
            }
        }
        function get_gheeproduction() {
            var doe = document.getElementById('txt_date').value;
            var data = { 'op': 'get_gheestockdetails', 'doe': doe };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillgheeproductiondetails(msg);
                    }
                    else {
                        msg = 0;
                        fillgheeproductiondetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillgheeproductiondetails(msg) {
            var branchname = '<%=Session["Branch_ID"] %>';
            if (branchname == "26" || branchname == "115") {
                var results = '<div  style="overflow:auto;"><table  style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Product Name</th><th scope="col">Opening Stock</th><th scope="col">Recive Cream Qty</th><th scope="col">Production</th><th scope="col">From KCC</th><th scope="col">Marcket Return</th><th scope="col">Oter Return</th><th scope="col">Sales</th><th scope="col">CON/CUT</th><th scope="col">Closing Balance</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr>';
                    results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                    results += '<td scope="row" class="1" style="text-align:center;"><div style="text-align: right;">' + parseFloat(msg[i].gheeOpeningStock).toFixed(4) + '</div></td>';
                    results += '<td class="2"><div style="text-align: right;">' + parseFloat(msg[i].gheerecivemilkqty).toFixed(4) + '</div></td>';
                    results += '<td class="2"><div style="text-align: right;">' + parseFloat(msg[i].gheeproduction).toFixed(4) + '</div></td>';
                    results += '<td class="2"><div style="text-align: right;">' + parseFloat(msg[i].gheefromkcc).toFixed(4) + '</div></td>';
                    results += '<td class="2"><div style="text-align: right;">' + parseFloat(msg[i].gheemarcketreturn).toFixed(4) + '</td>';
                    results += '<td class="2"><div style="text-align: right;">' + parseFloat(msg[i].gheeoterreturn).toFixed(4) + '</div></td>';
                    results += '<td class="2"><div style="text-align: right;">' + parseFloat(msg[i].gheecutting).toFixed(4) + '</div></td>';
                    results += '<td class="2"><div style="text-align: right;">' + parseFloat(msg[i].gheesales).toFixed(4) + '</div></td>';
                    var gheeOpeningStock = parseFloat(msg[i].gheeOpeningStock);
                    var gheeproduction = parseFloat(msg[i].gheeproduction);
                    var gheefromkcc = parseFloat(msg[i].gheefromkcc);
                    var gheemarcketreturn = parseFloat(msg[i].gheemarcketreturn);
                    var gheeoterreturn = parseFloat(msg[i].gheeoterreturn);
                    var gheecutting = parseFloat(msg[i].gheecutting);
                    var gheesales = parseFloat(msg[i].gheesales);
                    var add = gheeOpeningStock + gheeproduction + gheefromkcc + gheemarcketreturn + gheeoterreturn;
                    var sub = gheecutting + gheesales;
                    results += '<td class="3"><div style="text-align: right;">' + parseFloat((add - sub)).toFixed(4) + '</div></td></tr>';
                }
                results += '</table></div>';
                $("#div_gheedata").html(results);
            }
            else {
                var results = '<div  style="overflow:auto;"><table  style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Product Name</th><th scope="col">Opening Stock</th><th scope="col">Recive Cream Qty</th><th scope="col">Production</th><th scope="col">Sales</th><th scope="col">Closing Balance</th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr>';
                    results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                    results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].gheeOpeningStock + '</td>';
                    results += '<td data-title="Capacity" class="2">' + msg[i].gheerecivemilkqty + '</td>';
                    results += '<td data-title="Capacity" class="2">' + msg[i].gheeproduction + '</td>';
                    results += '<td data-title="Capacity" class="2">' + msg[i].gheesales + '</td>';
                    results += '<td class="3">' + msg[i].gheeclosingbalance + '</td></tr>';
                }
                results += '</table></div>';
                $("#div_gheedata").html(results);
            }
        }
        function get_butterproduction() {
            var doe = document.getElementById('txt_date').value;
            var data = { 'op': 'get_butterstockdetails', 'doe': doe };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbutterproductiondetails(msg);
                    }
                    else {
                        msg = 0;
                        fillbutterproductiondetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillbutterproductiondetails(msg) {
            var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Opening Stock</th><th scope="col">Recive Cream Qty</th><th scope="col">Recive Cream Fat</th><th scope="col">Production</th><th scope="col">Sales</th><th scope="col">Dispatch To Ghee</th><th scope="col">Closing Balance</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].butteropening + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].creamrecive + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].creamfat + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].butterproduction + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].buttersales + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].butterdesp + '</td>';
                results += '<td class="3">' + msg[i].butterclosing + '</td></tr>'; 
            }
            results += '</table></div>';
            $("#div_butterdata").html(results);
        }

        function get_creamproduction() {
            var doe = document.getElementById('txt_date').value;
            var data = { 'op': 'get_creamstockdetails', 'doe': doe };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcreamproductiondetails(msg);
                    }
                    else {
                        msg = 0;
                        fillcreamproductiondetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillcreamproductiondetails(msg) {
            var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Opening Stock</th><th scope="col">Production Qty</th><th scope="col"> Fat</th><th scope="col">Avg FAT</th><th scope="col">Closing Balance</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].creamopening + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].creamproduction + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].creampfat + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].creamavgfat + '</td>';
                results += '<td class="3">' + msg[i].creamclosing + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_creamdata").html(results);
        }
        function get_smpproduction() {
            var doe = document.getElementById('txt_date').value;
            var data = { 'op': 'get_smpstockdetails', 'doe': doe };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillsmpproductiondetails(msg);
                    }
                    else {
                        msg = 0;
                        fillsmpproductiondetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillsmpproductiondetails(msg) {
            var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Opening Stock</th><th scope="col">No Of Bags</th><th scope="col">Recived Qty</th><th scope="col"> Consumption</th><th scope="col"> Stock Transfor</th><th scope="col">Closing Balance</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].smpopening + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].noofbags + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].smpproduction + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].smpconsumption + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].stocktransfor + '</td>'; 
                results += '<td class="3">' + msg[i].smpclosing + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_smpdata").html(results);
        }
        function get_biproductproduction() {
            var doe = document.getElementById('txt_date').value;
            var data = { 'op': 'get_biproduct_stock_details', 'doe': doe };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbiproductsproductiondetails(msg);
                    }
                    else {
                        msg = 0;
                        fillbiproductsproductiondetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillbiproductsproductiondetails(msg) {
            var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Product Name</th><th scope="col">Opening Stock</th><th scope="col">Recived Qty</th><th scope="col"> Production</th><th scope="col">Sale</th><th scope="col">Closing Balance</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].biproductopening + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].biproductrecive + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].biproductproduction + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].biproductsale + '</td>';
                results += '<td class="3">' + msg[i].biproductclosing + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_biproductsdata").html(results);
        }
        function CallPrint() {
            var divToPrint = document.getElementById("divprint");
            var newWin = window.open('', 'Print-Window', 'width=100%,height=100%,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function CallPrint1() {
            var divToPrint = document.getElementById("youModal");
            var newWin = window.open('', 'Print-Window', 'width=100%,height=100%,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        var replaceHtmlEntites = (function () {
            var translate_re = /&(nbsp|amp|quot|lt|gt);/g;
            var translate = {
                "nbsp": " ",
                "amp": "&",
                "quot": "\"",
                "lt": "<",
                "gt": ">"
            };
            return function (s) {
                return (s.replace(translate_re, function (match, entity) {
                    return translate[entity];
                }));
            }
        })();
        function get_other_biproducts_stock_details() {
            var todate = document.getElementById('txt_date').value;
            if (todate == "") {
                alert("Select To Date");
                return false;
            }
            var data = { 'op': 'get_other_biproducts_stock_details', 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillobprepdetails(msg);
                    }
                    else {
                        msg = 0;
                        fillobprepdetails(msg);
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
            var results = '<div  style="overflow:auto;"><table style="background-color: antiquewhite;" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">OB</th><th scope="col" style="font-weight: bold;">Raw Material</th><th scope="col" style="font-weight: bold;">HandlingLoss</th><th scope="col" style="font-weight: bold;">Prooduction</th><th scope="col" style="font-weight: bold;">Despatch</th><th scope="col" style="font-weight: bold;">CB</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td data-title="Code" class="7">' + msg[i].ob + '</td>';
                results += '<td data-title="Code" class="8">' + msg[i].recivedqty + '</td>';
                results += '<td data-title="Code" class="9">' + msg[i].lossqty + '</td>';
                results += '<td data-title="Code" class="5">' + msg[i].productionqty + '</td>';
                results += '<td data-title="Code" class="10">' + msg[i].sales + '</td>';
                results += '<td data-title="Code" class="8">' + msg[i].cb + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_obpdetails").html(results);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Stock Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Stock Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Stock Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <table align="center" style="width: 30%;">
                        <tr>
                            <td>
                                <label>
                                    Date<span style="color: red;">*</span></label>
                                <input id="txt_date" class="form-control" type="date" name="vendorcode" />
                            </td>
                            <td style="width: 3px;">
                            </td>
                            <td>
                                <br />
                                <input id='get_stockdetails' type="button" class="btn btn-success" name="submit"
                                    value='Get' onclick="get_stockdetails_click()" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divprint" style="width: 100%;">
                    <div id="head1">
                        <h3 class="box-title">
                            Curd Stock Details
                        </h3>
                    </div>
                    <div id="div_Deptdata" style="width: 100%;">
                    </div>
                    <div id="head2">
                        <h3 class="box-title">
                            GHEE Stock Details
                        </h3>
                    </div>
                    <div id="div_gheedata">
                    </div>
                    <div id="head3">
                        <h3 class="box-title">
                            Butter Stock Details
                        </h3>
                    </div>
                    <div id="div_butterdata">
                    </div>
                    <div id="head4">
                        <h3 class="box-title">
                            Cream Stock Details
                        </h3>
                    </div>
                    <div id="div_creamdata">
                    </div>
                    <div id="head5">
                        <h3 class="box-title">
                            SMP Stock Details
                        </h3>
                    </div>
                    <div id="div_smpdata">
                    </div>
                    <div id="head6">
                        <h3 class="box-title">
                           BI-Products Stock Details
                        </h3>
                    </div>
                    <div id="div_biproductsdata">
                    </div>
                    <div id="head7">
                        <h3 class="box-title">
                           Other BI-Products Stock Details
                        </h3>
                    </div>
                    <div id="div_obpdetails">
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
                <br />
            <div align="center">
             <input id='btnprint' type="button" class="btn btn-success" name="submit" value='Print' onclick="CallPrint()" />
            </div>
            </div>
        </div>
    </section>
    <div id="myModal" class="modal fade" role="dialog" >
            <div class="modal-dialog" style="width: 1000px;">
                <!-- Modal content-->
                <div class="modal-content" style="width:1000px; " >
                    <div id="youModal"> 
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                &times;</button>
                            <h4 class="modal-title">
                                Stock Details</h4>
                        </div>
                        <div class="modal-body">
                           <table align="center">
                            <tr>
                                <td colspan="4">
                                    <div id="divcurdstock">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="Button1" type="button" class="btn btn-success"  onclick="CallPrint1()">
                            Print</button>
                        <button id="btnaclose" type="button" class="btn btn-warning" data-dismiss="modal">
                            Close</button>
                    </div>
                </div>
            </div>
    </div>
</asp:Content>
