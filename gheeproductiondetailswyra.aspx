<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="gheeproductiondetailswyra.aspx.cs" Inherits="gheeproductiondetailswyra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#lidop').click(function () {
                $('#cream_fillform').css('display', 'block');
                $('#div_production').css('display', 'none');
                $('#divproductionsales').css('display', 'none');
                $('#div_creambind').show();
                $('#div_editproduction').hide();
                $('div_salesedit').hide();
                $('#div_editcream').hide();
            });
            $('#lidos').click(function () {
                $('#cream_fillform').css('display', 'none');
                $('#div_production').css('display', 'block');
                $('#divproductionsales').css('display', 'none');
                $('#div_editgheeproduct').show();
                $('#div_editproduction').hide();
                $('div_salesedit').hide();
                $('#div_editcream').hide();
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
                $('#txt_pdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
                get_gheeproduction_details();
            });
            $('#lisales').click(function () {
                $('#cream_fillform').css('display', 'none');
                $('#div_production').css('display', 'none');
                $('#divproductionsales').css('display', 'block');
                $('#div_editsales').show();
                $('#div_editproduction').hide();
                $('div_salesedit').hide();
                $('#div_editcream').hide();
                get_gheesales_details();
                fillgheeproductsales();
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
                $('#txt_sdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            });
            Clearvalues();
            get_gheecreambinding_details();
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
            $('#btn_ceclose').click(function () {
                $('#cream_fillform').show();
                $('#div_creambind').show();
                $('#div_production').hide();
                $('#divproductionsales').hide();
                $('#div_editcream').hide();
                $('#div_editproduction').hide();
                $('div_salesedit').hide();
            });
            $('#btn_peclose').click(function () {
                $('#div_editproduction').hide();
                $('#cream_fillform').hide();
                $('#div_creambind').hide();
                $('#div_production').show();
                $('#div_editgheeproduct').show();
                $('#divproductionsales').hide();
                $('#div_editcream').hide();
                $('div_salesedit').hide();
            });
            $('#btn_seclose').click(function () {
                $('#div_editproduction').hide();
                $('#cream_fillform').hide();
                $('#div_creambind').hide();
                $('#div_production').hide();
                $('#div_editgheeproduct').hide();
                $('#divproductionsales').hide();
                $('#div_editcream').hide();
                $('#cream_fillform').css('display', 'none');
                $('#div_production').css('display', 'none');
                $('#divproductionsales').css('display', 'block');
                $('#div_editsales').show();
                $('#div_salesedit').hide();
            });
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
        // cream 
        function get_product_details() {
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcurdproducts(msg);
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
        function fillcurdproducts(msg) {
            var data = document.getElementById('slct_product');
            var length = data.options.length;
            document.getElementById('slct_product').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Product";
            opt.value = "Select Product";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].departmentid == "3") {
                    if (msg[i].productname != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].productname;
                        option.value = msg[i].productid;
                        data.appendChild(option);
                    }
                }
            }
        }
        function get_gheecreambinding_details() {
            var data = { 'op': 'get_gheecreambinding_details_wyra' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillgheecreamdetails(msg);
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
        function fillgheecreamdetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr  role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Cream Type</th><th scope="col" style="font-weight: bold;">O/B</th><th scope="col" style="font-weight: bold;">O/B FAT</th><th scope="col" style="font-weight: bold;">Cream Received(qty)</th><th scope="col" style="font-weight: bold;">Cream Received(fat)</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getmecream(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].creamtype + '</td>';
                results += '<td data-title="Capacity" class="2" style="text-align:center;">' + msg[i].ob + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].obfat + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].recivequnatity + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].recivefat + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].doe + '</td>';
                results += '<td style="display:none" class="7">' + msg[i].sno + '</td>';
                results += '<td style="display:none" class="8">' + msg[i].avgfat + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].datetime + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].totalcreamqty + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].receivedtype + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].receivedfrom + '</td>';
                results += '<td style="display:none" class="13">' + msg[i].recivesnf + '</td>';
                results += '<td style="display:none" class="14">' + msg[i].snf + '</td>';
                results += '<td style="display:none" class="15">' + msg[i].avgsnf + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmecream(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_creambind").html(results);
        }
        var receivedtype = "";
        function getmecream(thisid) {
            var creamtype = $(thisid).parent().parent().children('.1').html();
            var ob = $(thisid).parent().parent().children('.2').html();
            var obfat = $(thisid).parent().parent().children('.3').html();
            var recivequnatity = $(thisid).parent().parent().children('.4').html();
            var recivefat = $(thisid).parent().parent().children('.5').html();
            var datetime = $(thisid).parent().parent().children('.10').html();
            var sno = $(thisid).parent().parent().children('.7').html();
            var avgfat = $(thisid).parent().parent().children('.8').html();
            var totalcreamqty = $(thisid).parent().parent().children('.9').html();
            receivedtype = $(thisid).parent().parent().children('.11').html();
            var receivedfrom = $(thisid).parent().parent().children('.12').html();
            var recivesnf = $(thisid).parent().parent().children('.13').html();
            var snf = $(thisid).parent().parent().children('.14').html();
            var avgsnf = $(thisid).parent().parent().children('.15').html();
            document.getElementById('slct_crmreceivedtype').value = receivedtype;
            onchange_receivedfrom();
            document.getElementById('slct_crmreceivedfrom').value = receivedfrom;
            document.getElementById('txt_date').value = datetime;
            document.getElementById('ddlcreamtype').value = creamtype;
            document.getElementById('txt_ob').value = ob;
            document.getElementById('txt_creamfat').value = obfat;
            document.getElementById('txt_recive').value = recivequnatity;
            document.getElementById('txt_recivedfat').value = recivefat;
            document.getElementById('txt_total').value = totalcreamqty;
            document.getElementById('txt_avgfat').value = avgfat;
            document.getElementById('hdn_creamsno').value = sno;
            document.getElementById('hdn_oldreciveqty').value = recivequnatity;
            document.getElementById('txt_obcreamsnf').value = snf;
            document.getElementById('txt_recivedsnf').value = recivesnf;
            document.getElementById('txt_avgsnf').value = avgsnf;
            document.getElementById('btn_creamsave').innerHTML = "Modify";

            $("#cream_fillform").show();
            $("#div_editcream").hide();
            $("#div_creambind").hide();
            $('#div_production').hide();
            $('#divproductionsales').hide();
        }
        function save_cream_production_click() {
            var creamtype = document.getElementById('ddlcreamtype').value;
            var ob = document.getElementById('txt_ob').value;
            var date = document.getElementById('txt_date').value;
            var recive = document.getElementById('txt_recive').value;
            var recivedfat = document.getElementById('txt_recivedfat').value;
            var creamfat = document.getElementById('txt_creamfat').value;
            var total = document.getElementById('txt_total').value;
            var avgfat = document.getElementById('txt_avgfat').value;
            var obcreamsnf = document.getElementById('txt_obcreamsnf').value;
            var recivedsnf = document.getElementById('txt_recivedsnf').value;
            if (recivedsnf == "") {
                alert("Please enter Cream received SNF");
                $("#txt_recivedsnf").focus();
                return false;
            }
            var avgsnf = document.getElementById('txt_avgsnf').value;
            var oldreciveqty = document.getElementById('hdn_oldreciveqty').value;
            if (date == "") {
                alert("Please select date");
                $("#txt_date").focus();
                return false;
            }
            if (creamtype == "" || creamtype == "Select Cream Type") {
                alert("Please Select Cream Type");
                $("#ddlcreamtype").focus();
                return false;
            }
            if (ob == "") {
                alert("Please enter ob");
                $("#txt_ob").focus();
                return false;
            }
            if (recive == "") {
                alert("Please enter Cream received quantity");
                $("#txt_recive").focus();
                return false;
            }
            if (recivedfat == "") {
                alert("Please enter Cream received fat");
                $("#txt_recivedfat").focus();
                return false;
            }
            if (creamfat == "") {
                alert("Please enter O/B fat");
                $("#txt_creamfat").focus();
                return false;
            }
            var btnvalue = document.getElementById('btn_creamsave').innerHTML;
            var sno = document.getElementById('hdn_creamsno').value;
            var receivedtype = document.getElementById('slct_crmreceivedtype').value;
            if (receivedtype == "") {
                alert("Please Select Received Type");
                $("#slct_crmreceivedtype").focus();
                return false;
            }
            var receivedfrom = document.getElementById('slct_crmreceivedfrom').value;
            if (receivedfrom == "Select Vendor Name" || receivedfrom == "Select Branch Name") {
                alert("Please Select Received Type");
                $("#slct_crmreceivedtype").focus();
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_cream_production_click', 'creamtype': creamtype, 'ob': ob, 'recive': recive, 'recivedfat': recivedfat, 'creamfat': creamfat, 'total': total,
                    'avgfat': avgfat, 'doe': date, 'btnvalue': btnvalue, 'sno': sno, 'oldreciveqty': oldreciveqty, 'receivedtype': receivedtype, 'receivedfrom': receivedfrom,
                    'obcreamsnf': obcreamsnf, 'recivedsnf': recivedsnf, 'avgsnf': avgsnf
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_gheecreambinding_details();
                            Clearvalues();
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
        }
        function Clearvalues() {
            document.getElementById('txt_date').value = "";
            document.getElementById('txt_ob').value = "";
            document.getElementById('txt_recive').value = "";
            document.getElementById('txt_recivedfat').value = "";
            document.getElementById('txt_creamfat').value = "";
            document.getElementById('txt_total').value = "";
            document.getElementById('txt_avgfat').value = "";
            document.getElementById('txt_obcreamsnf').value = "";
            document.getElementById('txt_recivedsnf').value = "";
            document.getElementById('txt_avgsnf').value = "";
            document.getElementById('ddlcreamtype').selectedIndex = 0;
            document.getElementById('btn_creamsave').innerHTML = "Save";
            $('#cream_fillform').css('display', 'block');
            $('#div_production').css('display', 'none');
            $('#divproductionsales').css('display', 'none');
            $('#div_creambind').show();
            $('#div_editproduction').hide();
            $('div_salesedit').hide();
            $('#div_editcream').hide();
            document.getElementById('slct_crmreceivedtype').selectedIndex = 0;
            document.getElementById("slct_crmreceivedfrom").options.length = 0;
            $('#td_receivedfrom').hide();
        }
        function totalcreamqty(tqtyid) {
            var ob = document.getElementById('txt_ob').value;
            if (ob == "") {
                alert("Please enter O/B");
                return false;
            }
            var recipts = tqtyid.value;
            var sum = parseFloat(ob) + parseFloat(recipts);
            document.getElementById('txt_total').value = sum;
        }
        function closingtotal(qtyid) {
            if (qtyid.value == "") {
            }
            else {
                var convertionqty = document.getElementById('txt_convertionqty').value;
                if (convertionqty == "") {
                    alert("Please enter convertionqty");
                    return false;
                }
                var recipts = qtyid.value;
                var sum = parseFloat(convertionqty) * parseFloat(recipts);
                document.getElementById('txt_productionqty').value = parseFloat(sum / 100).toFixed(2);
            }
        }

        function salestotal(qtyid) {
            if (qtyid.value == "") {
            }
            else {
                var pob = document.getElementById('txt_pob').value;
                var productionqty = document.getElementById('txt_productionqty').value;
                if (productionqty == "") {
                    alert("Please enter productionqty");
                    return false;
                }
                if (pob == "") {
                    alert("Please enter O/B");
                    return false;
                }
                var recipts = qtyid.value;
                var sum = parseFloat(pob) + parseFloat(productionqty);
                var pcb = parseFloat(sum) - parseFloat(recipts);
                document.getElementById('txt_pcb').value = pcb;
            }
        }
        function fatchange() {
            var Recive = document.getElementById('txt_recive').value;
            var ob = document.getElementById('txt_ob').value;
            var totalcreamqty = document.getElementById('txt_total').value;
            var recivedfat = document.getElementById('txt_recivedfat').value;
            if (Recive == "") {
                alert("Please enter Received");
                return false;
            }
            var recipts = document.getElementById('txt_creamfat').value;
            var x = parseFloat(recipts) / 100;
            var y = parseFloat(x) * parseFloat(ob);

            var xx = parseFloat(recivedfat) / 100;
            var yy = parseFloat(xx) * parseFloat(Recive);

            var zz = parseFloat(y) + parseFloat(yy);

            var a = parseFloat(totalcreamqty) / 100;
            var ab = parseFloat(zz) / parseFloat(a);
            document.getElementById('txt_avgfat').value = parseFloat(ab).toFixed(2);
        }
        function snfchange() {
            var Recive = document.getElementById('txt_recive').value;
            var ob = document.getElementById('txt_ob').value;
            var totalcreamqty = document.getElementById('txt_total').value;
            var recivedfat = document.getElementById('txt_recivedsnf').value;
            if (Recive == "") {
                alert("Please enter Received");
                return false;
            }
            var recipts = document.getElementById('txt_obcreamsnf').value;
            var x = parseFloat(recipts) / 100;
            var y = parseFloat(x) * parseFloat(ob);

            var xx = parseFloat(recivedfat) / 100;
            var yy = parseFloat(xx) * parseFloat(Recive);

            var zz = parseFloat(y) + parseFloat(yy);

            var a = parseFloat(totalcreamqty) / 100;
            var ab = parseFloat(zz) / parseFloat(a);
            document.getElementById('txt_avgsnf').value = parseFloat(ab).toFixed(2);
        }
        function creamchange() {
            var productid = document.getElementById('ddlcreamtype').value;
            var data = { 'op': 'get_gheecream_details', 'productid': productid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].creamtype == "0") {
                                document.getElementById('txt_ob').value = "0";
                                document.getElementById('txt_creamfat').value = "0";
                                document.getElementById('txt_obcreamsnf').value = "0";
                            }
                            else {
                                document.getElementById('txt_ob').value = msg[i].creamtype;
                                document.getElementById('txt_creamfat').value = msg[i].fat;
                                document.getElementById('txt_obcreamsnf').value = msg[i].snf;
                            }
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
        var GheeDetails = [];
        function productchange() {
            var productid = document.getElementById('slct_product').value;
            var data = { 'op': 'get_productqty_details', 'productid': productid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('txt_pob').value = msg[i].quantity;
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
        function fatChange(qtyid) {
            if (qtyid.value == "") {
            }
            else {
                var convertionqty = document.getElementById('txt_rqty').value;
                var recipts = qtyid.value;
                var sum = parseFloat(convertionqty) * parseFloat(recipts);
                var pqty = parseFloat(sum / 100).toFixed(2);
                $(qtyid).closest("tr").find('#txt_production').val(pqty);
            }
        }
        function lossChange(qtyid) {
            if (qtyid.value == "") {
            }
            else {
                var productionqty = document.getElementById('txt_production').value;
                var lossqty = qtyid.value;
                var sum = parseFloat(productionqty) - parseFloat(lossqty);
                var qty = parseFloat(sum).toFixed(2);
                $(qtyid).closest("tr").find('#txt_greciveqty').val(qty);
            }
        }
        function onchange_receivedfrom() {
            receivedtype = document.getElementById('slct_crmreceivedtype').value;
            if (receivedtype == "Plant") {
                $('#td_receivedfrom').show();
                get_Branch_details_wyra();
            }
            else if (receivedtype == "Chilling Center") {
                $('#td_receivedfrom').show();
                get_Vendor_details();
            }
            else {
                $('#td_receivedfrom').hide();
            }
        }
        function get_Vendor_details() {
            var data = { 'op': 'get_Vendor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var Vendor = msg[0].vendordetails;
                        fillSource(Vendor);
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
        function fillSource(msg) {
            var data = document.getElementById('slct_crmreceivedfrom');
            var length = data.options.length;
            document.getElementById('slct_crmreceivedfrom').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vendor Name";
            opt.value = "Select Vendor Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].vendorname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].vendorname;
                    option.value = msg[i].sno;
                    data.appendChild(option);
                }
            }
        }
        function get_Branch_details_wyra() {
            var data = { 'op': 'get_Branch_details_wyra' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbranchSource(msg);
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
        function fillbranchSource(msg) {
            var data = document.getElementById('slct_crmreceivedfrom');
            var length = data.options.length;
            document.getElementById('slct_crmreceivedfrom').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Branch Name";
            opt.value = "Select Branch Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].branchName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].branchName;
                    option.value = msg[i].Sno;
                    data.appendChild(option);
                }
            }
        }
        // Ghee Production
        function fillgheeproductdetails() {
            var type = document.getElementById('ddltype').value;
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        GheeDetails = msg;
                        if (type == 10) {
                            var results = '<div    style="overflow:auto;"><table id="table_Ghee_production_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Qty(O/B)</th><th scope="col" style="font-weight: 700;">Production Quantity</th></tr></thead></tbody>';
                            for (var i = 0; i < msg.length; i++) {
                                if (msg[i].departmentid == "3" && msg[i].batchid == "9") {
                                    if (msg[i].productid == 10 || msg[i].productid == 90 || msg[i].productid == 164 || msg[i].productid == 165 || msg[i].productid == 166) {
                                    }
                                    else {
                                        results += '<tr>';
                                        results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                        results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                        results += '<td><input id="txt_OB" readonly class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter qty"></td>';
                                        results += '<td><input id="txt_production"  class="form-control" onkeypress="return isFloat(event);" value="0" type="text" name="vendorcode"placeholder="Enter production"></td>';
                                        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                    }
                                }
                            }
                        }
                        else if (type == 165) {
                            var results = '<div    style="overflow:auto;"><table id="table_Ghee_production_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Qty(O/B)</th><th scope="col" style="font-weight: 700;">Production Quantity</th></tr></thead></tbody>';
                            for (var i = 0; i < msg.length; i++) {
                                if (msg[i].departmentid == "3" && msg[i].batchid == "10") {
                                    if (msg[i].productid == 10 || msg[i].productid == 90 || msg[i].productid == 164 || msg[i].productid == 165 || msg[i].productid == 166) {
                                    }
                                    else {
                                        results += '<tr>';
                                        results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                        results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                        results += '<td><input id="txt_OB" readonly class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter qty"></td>';
                                        results += '<td><input id="txt_production"  class="form-control" onkeypress="return isFloat(event);" value="0" type="text" name="vendorcode"placeholder="Enter production"></td>';
                                        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                    }
                                }
                            }
                        }
                        else if (type == 166) {
                            var results = '<div    style="overflow:auto;"><table id="table_Ghee_production_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Qty(O/B)</th><th scope="col" style="font-weight: 700;">Production Quantity</th></tr></thead></tbody>';
                            for (var i = 0; i < msg.length; i++) {
                                if (msg[i].departmentid == "3" && msg[i].batchid == "35") {
                                    if (msg[i].productid == 10 || msg[i].productid == 90 || msg[i].productid == 164 || msg[i].productid == 165 || msg[i].productid == 166) {
                                    }
                                    else {
                                        results += '<tr>';
                                        results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                        results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                        results += '<td><input id="txt_OB"  class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter qty"></td>';
                                        results += '<td><input id="txt_production"  class="form-control" onkeypress="return isFloat(event);" value="0" type="text" name="vendorcode"placeholder="Enter production"></td>';
                                        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                    }
                                }
                            }
                        }
                        else {
                            var results = '<div    style="overflow:auto;"><table id="table_Ghee_production_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Qty (O/B)</th><th scope="col" style="font-weight: 700;">Conversion Qty</th><th scope="col" style="font-weight: 700;">Conversion FAT</th><th scope="col" style="font-weight: 700;">Conversion SNF</th><th scope="col" style="font-weight: 700;">Production Qty(kgs)</th><th scope="col" style="font-weight: 700;">Loss Qty(kgs)</th><th scope="col" style="font-weight: 700;">Received Qty</th></tr></thead></tbody>';
                            for (var i = 0; i < msg.length; i++) {
                                if (msg[i].departmentid == "3") {
                                    var productid = 10;
                                    if (type == "90") {
                                        productid = 10;
                                        results += '<tr>';
                                        if (productid == msg[i].productid) {
                                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                            results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                            results += '<td><input id="txt_OB" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" ></td>';
                                            //results += '<td><input id="txt_pobfat" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].obfat + '" type="text" ></td>';
                                            //results += '<td><input id="txt_pobsnf" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].obsnf + '" type="text" ></td>';
                                            results += '<td><input id="txt_rqty" class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter qty"></td>';
                                            results += '<td><input id="txt_convertionfat" class="form-control" onkeypress="return isFloat(event);" value="" type="text" onkeyup="fatChange(this);" name="vendorcode"placeholder="Enter Fat"></td>';
                                            results += '<td><input id="txt_convertionsnf" class="form-control" onkeypress="return isFloat(event);" value="" type="text" placeholder="Enter SNF"></td>';
                                            results += '<td><input id="txt_production"  class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter production"></td>';
                                            results += '<td><input id="txt_loss"  class="form-control" value="" onkeypress="return isFloat(event);" type="text" onkeyup="lossChange(this);" name="vendorcode"placeholder="Enter Loss"></td>';
                                            results += '<td><input id="txt_greciveqty" readonly  class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter Recive"></td>';
                                            results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                        }
                                    }
                                    else {
                                        results += '<tr>';
                                        if ("165" == msg[i].productid || "166" == msg[i].productid) {
                                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                            results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                            results += '<td><input id="txt_OB" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter qty"></td>';
                                            //results += '<td><input id="txt_pobfat" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].obfat + '" type="text" ></td>';
                                            //results += '<td><input id="txt_pobsnf" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].obsnf + '" type="text" ></td>';
                                            results += '<td><input id="txt_rqty" class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter qty"></td>';
                                            results += '<td><input id="txt_convertionfat" class="form-control" onkeypress="return isFloat(event);" value="" type="text" onkeyup="fatChange(this);" name="vendorcode"placeholder="Enter FAT"></td>';
                                            results += '<td><input id="txt_convertionsnf" class="form-control" onkeypress="return isFloat(event);" value="" type="text" placeholder="Enter SNF"></td>';
                                            results += '<td><input id="txt_production"  class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter production"></td>';
                                            results += '<td><input id="txt_loss"  class="form-control" value="" onkeypress="return isFloat(event);" type="text" onkeyup="lossChange(this);" name="vendorcode"placeholder="Enter Loss"></td>';
                                            results += '<td><input id="txt_greciveqty" readonly  class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter Recive"></td>';
                                            results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                        }
                                    }
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
        function save_ghee_production_click() {
            var type = document.getElementById('ddltype').value;
            var qty = document.getElementById('txt_qty').value;
            var prodobcremfat = document.getElementById('txt_prodobcremfat').value;
            var prodobcremsnf = document.getElementById('txt_prodobcremsnf').value;
            var remarks = document.getElementById('txt_pRemarks').value;
            var date = document.getElementById('txt_pdate').value;
            if (date == "") {
                alert("Please select date");
                return false;
            }
            var btnvalue = document.getElementById('btnproduction').innerHTML;
            var sno = document.getElementById('lbl_psno').value;
            var rows = $("#table_Ghee_production_details tr:gt(0)");
            var ghee_production_details = new Array();
            $(rows).each(function (i, obj) {
                if (($(this).find('#txt_production').val() == "0" && $(this).find('#txt_OB').val() == "0") || $(this).find('#txt_production').val() == undefined) {
                }
                else {
                    ghee_production_details.push({ productid: $(this).find('#hdn_productid').val(), OB: $(this).find('#txt_OB').val(),
                        convertionqty: $(this).find('#txt_rqty').val(), convertionfat: $(this).find('#txt_convertionfat').val(), convertionsnf: $(this).find('#txt_convertionsnf').val(),
                        productionqty: $(this).find('#txt_production').val(), lossqty: $(this).find('#txt_loss').val(), greciveqty: $(this).find('#txt_greciveqty').val()
                    });
                }
            });
            if (ghee_production_details.length == 0) {
                alert("Please Enter Production ");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_ghee_production_click', 'qty': qty, 'type': type, 'ghee_production_details': ghee_production_details, 'type': type, 'date': date, 'remarks': remarks, 'btnvalue': btnvalue, 'sno': sno, 'prodobcremfat': prodobcremfat, 'prodobcremsnf': prodobcremsnf };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_gheeproduction_details();
                            pclearvalues();
                            $('#div_editproduction').hide();
                            $('#cream_fillform').hide();
                            $('#div_creambind').hide();
                            $('#div_production').show();
                            $('#div_editgheeproduct').show();
                            $('#divproductionsales').hide();
                            $('#div_editcream').hide();
                            $('#div_salesedit').hide();
                            $('#divFillScreen').hide();
                            $('#td_source').show();
                            $('#td_pcqty').show();
                            $('#td_pcobfat').show();
                            $('#td_pcobsnf').show();
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
        function get_gheeproduction_details() {
            var data = { 'op': 'get_gheeproduction_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillgheeproductiondetails(msg);
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
        function fillgheeproductiondetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col"  style="font-weight: bold;">Product Name</th><th scope="col"  style="font-weight: bold;"> O/B</th><th scope="col"  style="font-weight: bold;">Production(Qty)</th><th scope="col"  style="font-weight: bold;">Date</th><th scope="col"  style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getmeproduction(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1" style="display:none" style="text-align:center;">' + msg[i].branchname + '</td>';
                results += '<td data-title="Capacity" class="2" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].openingbalance + '</td>';
                results += '<td  class="4" style="display:none" style="text-align:center;">' + msg[i].recivequnatity + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].productionqty + '</td>';
                results += '<td  class="6" style="display:none" style="text-align:center;">' + msg[i].convertionfat + '</td>';
                results += '<td  class="7" style="display:none" style="text-align:center;">' + msg[i].closingbalance + '</td>';
                results += '<td  class="8" style="text-align:center;">' + msg[i].doe + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].remarks + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].lossqty + '</td>';
                results += '<td style="display:none" class="13">' + msg[i].type + '</td>';
                results += '<td style="display:none" class="14">' + msg[i].datetime + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].sno + '</td>';
                results += '<td style="display:none" class="20">' + msg[i].snf + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeproduction(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_editgheeproduct").html(results);
        }
        function getmeproduction(thisid) {
            var branchname = $(thisid).parent().parent().children('.1').html();
            var productname = $(thisid).parent().parent().children('.2').html();
            var openingbalance = $(thisid).parent().parent().children('.3').html();
            var recivequnatity = $(thisid).parent().parent().children('.4').html();
            var productionqty = $(thisid).parent().parent().children('.5').html();
            var convertionfat = $(thisid).parent().parent().children('.6').html();
            var closingbalance = $(thisid).parent().parent().children('.7').html();
            var doe = $(thisid).parent().parent().children('.14').html();
            var productid = $(thisid).parent().parent().children('.9').html();
            var remarks = $(thisid).parent().parent().children('.10').html();
            var sno = $(thisid).parent().parent().children('.11').html();
            var lossqty = $(thisid).parent().parent().children('.12').html();
            var type = $(thisid).parent().parent().children('.13').html();
            var convertionsnf = $(thisid).parent().parent().children('.20').html();

            document.getElementById('txt_pdate').value = doe;
            document.getElementById('txt_qty').value = openingbalance;
            document.getElementById('lbl_psno').value = sno;
            document.getElementById('ddltype').value = type;
            document.getElementById('txt_pRemarks').value = remarks;
            document.getElementById('btnproduction').innerHTML = "Modify";

            if (productid == "10" || productid == "165" || productid == "166") {
                $("#divFillScreen").show();
                var results = '<div    style="overflow:auto;"><table id="table_Ghee_production_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Qty(O/B)</th><th scope="col" style="font-weight: 700;">Conversion Qty</th><th scope="col" style="font-weight: 700;">Conversion fat</th><th scope="col" style="font-weight: 700;">Conversion snf</th><th scope="col" style="font-weight: 700;">Production Qty(kgs)</th><th scope="col" style="font-weight: 700;">Loss Qty(kgs)</th><th scope="col" style="font-weight: 700;">ReceivedQty</th></tr></thead></tbody>';
                results += '<tr>';
                results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + sno + '</span></th>';
                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + productname + '</span></th>';
                results += '<td><input id="txt_OB" readonly class="form-control" onkeypress="return isFloat(event);" value="' + openingbalance + '" type="text" name="vendorcode"placeholder="Enter qty"></td>';
                results += '<td><input id="txt_rqty" class="form-control" onkeypress="return isFloat(event);" value="' + recivequnatity + '" type="text" name="vendorcode"placeholder="Enter qty"></td>';
                results += '<td><input id="txt_convertionfat" onkeypress="return isFloat(event);" class="form-control" value="' + convertionfat + '" type="text" onkeyup="fatChange(this);" name="vendorcode"placeholder="Enter FAT"></td>';
                results += '<td><input id="txt_convertionsnf" onkeypress="return isFloat(event);" class="form-control" value="' + convertionsnf + '" type="text"  name="vendorcode"placeholder="Enter SNF"></td>';
                results += '<td><input id="txt_production" onkeypress="return isFloat(event);" class="form-control" value="' + productionqty + '" type="text" name="vendorcode"placeholder="Enter production"></td>';
                results += '<td><input id="txt_loss" onkeypress="return isFloat(event);"  class="form-control" value="' + lossqty + '" type="text" onkeyup="lossChange(this);" name="vendorcode"placeholder="Enter Loss"></td>';
                results += '<td><input id="txt_greciveqty" onkeypress="return isFloat(event);" readonly  class="form-control" value="' + recivequnatity + '" type="text" name="vendorcode"placeholder="Enter Recive"></td>';
                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + productid + '"></td></tr>';
                results += '</table></div>';
                $("#divFillScreen").html(results);
            }
            else if (productid != "10" || productid != "90" || productid != "164" || productid != "165" || productid != "166") {
                $("#divFillScreen").show();
                var results = '<div    style="overflow:auto;"><table id="table_Ghee_production_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Qty(O/B)</th><th scope="col" style="font-weight: 700;">Production Quantity</th></tr></thead></tbody>';
                results += '<tr>';
                results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + sno + '</span></th>';
                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + productname + '</span></th>';
                results += '<td><input id="txt_OB" readonly onkeypress="return isFloat(event);" class="form-control" value="' + openingbalance + '" type="text" name="vendorcode"placeholder="Enter qty"></td>';
                results += '<td><input id="txt_production" onkeypress="return isFloat(event);"  class="form-control" value="' + productionqty + '" type="text" name="vendorcode"placeholder="Enter production"></td>';
                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + productid + '"></td></tr>';
                results += '</table></div>';
                $("#divFillScreen").html(results);
            }
            $('#div_production').show();
            $('#div_editproduction').hide();
            $("#div_editcream").hide();
            $("#cream_fillform").hide();
            $("#div_creambind").hide();
            $('#divproductionsales').hide();
            $('#div_editgheeproduct').hide();
            $('#td_source').hide();
            $('#td_pcqty').hide();
            $('#td_pcobfat').hide();
            $('#td_pcobsnf').hide();
        }
        function pclearvalues() {
            document.getElementById('txt_pdate').value = "";
            document.getElementById('txt_pRemarks').value = "";
            document.getElementById('txt_qty').value = "";
            document.getElementById('lbl_psno').value = "";
            document.getElementById('btnproduction').innerHTML = "Save";
            $('#div_editproduction').hide();
            $('#cream_fillform').hide();
            $('#div_creambind').hide();
            $('#div_production').show();
            $('#div_editgheeproduct').show();
            $('#divproductionsales').hide();
            $('#div_editcream').hide();
            $('#div_salesedit').hide();
            $('#divFillScreen').hide();
            $('#td_source').show();
            $('#td_pcqty').show();
            $('#td_pcobfat').show();
            $('#td_pcobsnf').show();
            fillgheeproductdetails();
        }
        // Ghee Sales
        function fillgheeproductsales() {
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div    style="overflow:auto;"><table id="table_sales_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;" >Product Name</th><th scope="col" style="font-weight: 700;">O/B Qty</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" style="font-weight: 700;">Frm KCC</th><th scope="col" style="font-weight: 700;">Marcket Return</th><th scope="col" style="font-weight: 700;">Other Return</th><th scope="col" style="font-weight: 700;">Sales</th><th scope="col" style="font-weight: 700;">CON/CUT</th><th scope="col" style="font-weight: 700;">C/B</th><th scope="col" style="font-weight: 700;">C/B Qty</th></tr></thead></tbody>';
                        var k = 1;
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].departmentid == "3") {
                                results += '<tr>';
                                results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + (k++) + '</span></th>';
                                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                results += '<td><input id="txt_ob" readonly onkeypress="return isFloat(event);" class="form-control" value="' + msg[i].quantity + '" type="text" name="vendorcode"></td>';
                                results += '<td><input id="txt_obpak" readonly onkeypress="return isFloat(event);" class="form-control" value="' + msg[i].packets + '" type="text" name="vendorcode"></td>';
                                results += '<td><input id="txt_frmkcc" onkeyup="change_packs(this);" onkeypress="return isFloat(event);" class="form-control" value="" type="text" name="vendorcode" placeholder="Enter Frm KCC"></td>';
                                results += '<td><input id="txt_mrkreturn" onkeyup="change_packs(this);" onkeypress="return isFloat(event);" class="form-control" value="" type="text" name="vendorcode" placeholder="Enter marcket return"></td>';
                                results += '<td><input id="txt_othreturn" onkeyup="change_packs(this);" onkeypress="return isFloat(event);" class="form-control" value="" type="text" name="vendorcode" placeholder="Enter other return"></td>';

                                results += '<td><input id="txt_salespck" onkeyup="change_packs(this);" onkeypress="return isFloat(event);" onkeyup="salesChange_packs(this);" class="form-control" value="" type="text" name="vendorcode" placeholder="Enter dispatch qty in Packets"></td>';
                                results += '<td><input id="txt_cutting" onkeyup="change_packs(this);" onkeypress="return isFloat(event);"  class="form-control" value="" type="text" name="vendorcode" placeholder="Enter dispatch qty in Packets"></td>';
                                results += '<td style="display:none" ><input id="txt_sales" onkeypress="return isFloat(event);" onkeyup="salesChange(this);" readonly class="form-control" value="" type="text" name="vendorcode" placeholder="Enter dispatch qty"></td>';
                                results += '<td><input id="txt_cbpak" readonly onkeypress="return isFloat(event);"  class="form-control" value="' + msg[i].packets + '" type="text" name="vendorcode" ></td>';
                                results += '<td><input id="txt_cb" readonly onkeypress="return isFloat(event);"  class="form-control" value="' + msg[i].quantity + '" type="text" name="vendorcode"></td>';
                                results += '<td style="display:none" class="8"><input id="hdn_packetsize" class="form-control" type="text" name="vendorcode" value="' + msg[i].packetsize + '"></td>';
                                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                            }
                        }
                        results += '</table></div>';
                        $("#divsales").html(results);
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
        function salesChange(qtyid) {
            var sale = parseFloat(qtyid.value).toFixed(3);
            var saleqty = $(qtyid).closest("tr").find('#txt_sales').val();
            var ob = $(qtyid).closest("tr").find('#txt_ob').val();
            saleqty = parseFloat(saleqty) || 0;
            ob = parseFloat(ob) || 0;
            if (saleqty == 'NaN') {
                saleqty = 0;
            }
            if (ob < saleqty) {
                alert("Enter Sale value is Less then the Opening Balance");
                $(qtyid).closest("tr").find('#txt_sales').val("0");
                return;
            }
        }
        function salesChange_packs(qtyid) {
            var packetsize = $(qtyid).closest("tr").find('#hdn_packetsize').val();
            var salespck = $(qtyid).closest("tr").find('#txt_salespck').val();
            var totqty = (parseFloat(packetsize) * parseFloat(salespck));
            $(qtyid).closest("tr").find('#txt_sales').val(totqty);
        }
        function change_packs(qtyid) {
            var packetsize = $(qtyid).closest("tr").find('#hdn_packetsize').val();
            if (packetsize == "" || packetsize == "null" || packetsize == null) {
                packetsize = 0;
            }
            var obpak = $(qtyid).closest("tr").find('#txt_obpak').val();
            if (obpak == "" || obpak == "null" || obpak == null) {
                obpak = 0;
            }
            var frmkcc = $(qtyid).closest("tr").find('#txt_frmkcc').val();
            if (frmkcc == "" || frmkcc == "null" || frmkcc == null) {
                frmkcc = 0;
            }
            var mrkreturn = $(qtyid).closest("tr").find('#txt_mrkreturn').val();
            if (mrkreturn == "" || mrkreturn == "null" || mrkreturn == null) {
                mrkreturn = 0;
            }
            var othreturn = $(qtyid).closest("tr").find('#txt_othreturn').val();
            if (othreturn == "" || othreturn == "null" || othreturn == null) {
                othreturn = 0;
            }
            var salespck = $(qtyid).closest("tr").find('#txt_salespck').val();
            if (salespck == "" || salespck == "null" || salespck == null) {
                salespck = 0;
            }
            var cutting = $(qtyid).closest("tr").find('#txt_cutting').val();
            if (cutting == "" || cutting == "null" || cutting == null) {
                cutting = 0;
            }
            var add = parseFloat(obpak) + parseFloat(frmkcc) + parseFloat(mrkreturn) + parseFloat(othreturn);
            var sub = parseFloat(salespck) + parseFloat(cutting);
            var cbpak = parseFloat(add) - parseFloat(sub);
            $(qtyid).closest("tr").find('#txt_cbpak').val(cbpak);
            var cb = parseFloat(cbpak) * parseFloat(packetsize);
            cb = parseFloat(cb).toFixed(4);
            $(qtyid).closest("tr").find('#txt_cb').val(cb);
        }
        function save_ghee_sales_click() {
            var remarks = document.getElementById('txt_sRemarks').value;
            var date = document.getElementById('txt_sdate').value;
            if (date == "") {
                alert("Please select date");
                return false;
            }
            var btnvalue = document.getElementById('btnsales').innerHTML;
            var sno = document.getElementById('lbl_ssno').value;
            var rows = $("#table_sales_wise_details tr:gt(0)");
            var ghee_sales_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_dispatchqty').val() != "") {
                    ghee_sales_details.push({ productid: $(this).find('#hdn_productid').val()
                    , ob: $(this).find('#txt_ob').val()
                    , obpak: $(this).find('#txt_obpak').val()
                    , frmkcc: $(this).find('#txt_frmkcc').val()
                    , mrkreturn: $(this).find('#txt_mrkreturn').val()
                    , othreturn: $(this).find('#txt_othreturn').val()
                    , salespck: $(this).find('#txt_salespck').val()
                    , cutting: $(this).find('#txt_cutting').val()
                    , cbpak: $(this).find('#txt_cbpak').val()
                    , cb: $(this).find('#txt_cb').val()
                    , packetsize: $(this).find('#hdn_packetsize').val()
                    });
                }
            });
            if (ghee_sales_details.length == 0) {
                alert("Please enter opening balance");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_ghee_sales_click_wyra', 'ghee_sales_details': ghee_sales_details, 'date': date, 'remarks': remarks, 'btnvalue': btnvalue, 'sno': sno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            salesclearvalues();
                            get_gheesales_details();
                            $('#cream_fillform').css('display', 'none');
                            $('#div_production').css('display', 'none');
                            $('#divproductionsales').css('display', 'block');
                            $('#div_editsales').show();
                            $('#div_editproduction').hide();
                            $('div_salesedit').hide();
                            $('#div_editcream').hide();
                            fillgheeproductsales();
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
        function ddltypeonchange() {
            var productid = document.getElementById('ddltype').value;
            var data = { 'op': 'get_productqty_details', 'productid': productid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('txt_qty').value = msg[i].quantity;
                            document.getElementById('txt_prodobcremfat').value = msg[i].fat;
                            document.getElementById('txt_prodobcremsnf').value = msg[i].snf;
                            fillgheeproductdetails();
                        }
                    }
                    else {
                        document.getElementById('txt_qty').value = "0";
                        document.getElementById('txt_prodobcremfat').value = "0";
                        document.getElementById('txt_prodobcremsnf').value = "0";
                        fillgheeproductdetails();
                    }
                }
                else {
                    document.getElementById('txt_qty').value = "0";
                    document.getElementById('txt_prodobcremfat').value = "0";
                    document.getElementById('txt_prodobcremsnf').value = "0";
                    fillgheeproductdetails();
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        //        function ddltypeonchange() {
        //            var type = document.getElementById('ddltype').value;
        //            if (GheeDetails.length > 0) {
        //                for (var i = 0; i < GheeDetails.length; i++) {
        //                    if (type == GheeDetails[i].productid) {
        //                        if (GheeDetails[i].quantity == "") {
        //                            document.getElementById('txt_qty').value = 0;
        //                        }
        //                        else {
        //                            document.getElementById('txt_qty').value = GheeDetails[i].quantity;
        //                        }
        //                    }
        //                }
        //            }
        //            else {
        //                document.getElementById('txt_qty').value = 0;
        //            }
        //            fillgheeproductdetails();
        //        }
        function get_gheesales_details() {
            var data = { 'op': 'get_gheesales_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillgheesalesdetails(msg);
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
        function fillgheesalesdetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">Sales</th><th scope="col" style="font-weight: bold;">Date</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getmesales(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td  class="2" style="text-align:center;">' + msg[i].salesquantity + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].doe + '</td>';
                results += '<td  class="4" style="display:none" >' + msg[i].productid + '</td>';
                results += '<td  class="6" style="display:none" >' + msg[i].remarks + '</td>';
                results += '<td  class="8" style="display:none" >' + msg[i].datetime + '</td>';
                results += '<td style="display:none" class="5">' + msg[i].sno + '</td>';
                //                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" onclick="getmesales(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_editsales").html(results);
        }
        function getmesales(thisid) {
            var productname = $(thisid).parent().parent().children('.1').html();
            var salesquantity = $(thisid).parent().parent().children('.2').html();
            var doe = $(thisid).parent().parent().children('.8').html();
            var productid = $(thisid).parent().parent().children('.4').html();
            var sno = $(thisid).parent().parent().children('.5').html();
            var remarks = $(thisid).parent().parent().children('.6').html();

            document.getElementById('txt_sdate').value = doe;
            document.getElementById('lbl_ssno').value = sno;
            document.getElementById('txt_sRemarks').value = remarks;
            document.getElementById('btnsales').innerHTML = "Modify";

            $("#divsales").show();
            var results = '<div    style="overflow:auto;"><table id="table_sales_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Product Name</th><th scope="col">Sales</th></tr></thead></tbody>';
            results += '<tr>';
            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + sno + '</span></th>';
            results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + productname + '</span></th>';
            results += '<td><input id="txt_sales" class="form-control" onkeypress="return isFloat(event);" value="' + salesquantity + '" type="text" onkeyup="salesChange(this);" name="vendorcode"placeholder="Enter dispatch qty"></td>';
            results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + productid + '"></td></tr>';
            results += '</table></div>';
            $("#divsales").html(results);

            $('#div_editsales').hide();
            $("#div_editcream").hide();
            $("#cream_fillform").hide();
            $("#div_creambind").hide();
            $('#div_production').hide();
            $('#divproductionsales').show();
        }
        function salesclearvalues() {
            document.getElementById('txt_sdate').value = "";
            document.getElementById('lbl_ssno').value = "";
            document.getElementById('btnsales').innerHTML = "Save";
            document.getElementById('txt_sRemarks').value = "";
            $('#cream_fillform').css('display', 'none');
            $('#div_production').css('display', 'none');
            $('#divproductionsales').css('display', 'block');
            $('#div_editsales').show();
            $('#div_editproduction').hide();
            $('div_salesedit').hide();
            $('#div_editcream').hide();
            fillgheeproductsales();
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Ghee Production Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Curd</a></li>
            <li><a href="#">Ghee Production Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="navigation">
                <ul style="padding-left: 350px;">
                    <li id="lidop"><a href="#" class="active"><span id="spandp">Cream Details</span></a></li>
                    <li>&nbsp</li>
                    <li id="lidos"><a href="#"><span id="spands">Production Details</span></a></li>
                    <li>&nbsp</li>
                    <li id="lisales"><a href="#"><span id="span1">Stock Summary Details</span></a></li>
                </ul>
            </div>
            <div class="box-body">
                <div id='cream_fillform'>
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Cream Production Details</h3>
                        </div>
                        </div>
                        <div id="div_vendordata">
                        </div>
                    <div align="center">
                        <table align="center" >
                            <tr>
                                <td>
                                    <label>
                                        Date<span style="color: red;">*</span></label>
                                    <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                        placeholder="Enter Date">
                                </td>
                                <td>
                                </td>
                                <td>
                                    <label>
                                        Cream Type<span style="color: red;">*</span></label>
                                    <select id="ddlcreamtype" class="form-control" onchange="creamchange()">
                                        <option>Select Cream Type</option>
                                        <option>Cow</option>
                                        <option>Buffalo</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        O/B</label>
                                    <input id="txt_ob" type="text" class="form-control" readonly name="vendorcode" />
                                    <label id="lbl_phe" class="errormessage">
                                        * Please enter O/B</label>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                 <td>
                                    <label>
                                        O/B Fat</label>
                                    <input id="txt_creamfat" type="text" class="form-control" readonly name="vendorcode" placeholder="Enter fat"  onkeypress="return isFloat(event);"/>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                 <td>
                                    <label>
                                        O/B SNF</label>
                                    <input id="txt_obcreamsnf" type="text" class="form-control" readonly  placeholder="Enter SNF"  onkeypress="return isFloat(event);"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Cream Received Quantity<span style="color: red;">*</span></label>
                                    <input id="txt_recive" type="text" class="form-control" name="vendorcode" placeholder="Enter ReceivedQuantity"
                                        onkeyup="totalcreamqty(this)"  onkeypress="return isFloat(event);" />
                                </td>
                             <td style="width: 5px;">
                                </td>
                                <td >
                                    <label>
                                        Cream Received Fat<span style="color: red;"></span></label>
                                    <input id="txt_recivedfat" type="text" class="form-control" name="vendorcode" placeholder="Enter  Cream ReceivedFat"   onkeyup="fatchange()" 
                                        onkeypress="return isFloat(event);"/>
                                </td>
                               <td style="width: 5px;">
                                </td>
                                <td >
                                    <label>
                                        Cream Received SNF<span style="color: red;"></span></label>
                                    <input id="txt_recivedsnf" type="text" class="form-control" name="vendorcode" placeholder="Enter  Cream Received SNF" onkeyup="snfchange()"
                                        onkeypress="return isFloat(event);"/>
                                </td>
                            <tr>
                            </tr>
                                <td>
                                    <label>
                                        Total Cream Quantity<span style="color: red;">*</span></label>
                                    <input id="txt_total" type="text" class="form-control" name="vendorcode" 
                                        readonly />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        AVG Fat<span style="color: red;"></span></label>
                                    <input id="txt_avgfat" type="text" class="form-control" name="vendorcode" 
                                        readonly />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        AVG SNF<span style="color: red;"></span></label>
                                    <input id="txt_avgsnf" type="text" class="form-control" name="vendorcode" 
                                        readonly />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Received Type<span style="color: red;">*</span></label>
                                    <select id="slct_crmreceivedtype" class="form-control" onchange="onchange_receivedfrom()">
                                        <option>From KCC</option>
                                        <option>Plant</option>
                                        <option>Chilling Center</option>
                                    </select>
                                </td>
                                <td>
                                </td>
                                <td id="td_receivedfrom" hidden>
                                    <label>
                                        Received From<span style="color: red;">*</span></label>
                                    <select id="slct_crmreceivedfrom" class="form-control" >
                                    </select>
                                </td>
                            </tr>
                            <tr hidden>
                             <td >
                                 <input id="hdn_creamsno" type="text" class="form-control"  />
                                 <input id="hdn_oldreciveqty" type="text" class="form-control"  />
                                 </td>
                            </tr>
                        </table>
                    </div>
                     <div  style="padding-left: 35%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_creamsave1" onclick="save_cream_production_click()"></span><span id="btn_creamsave" onclick="save_cream_production_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_creamclear1' onclick="Clearvalues()"></span><span id='btn_creamclear' onclick="Clearvalues()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    <br />
                    <div id="div_creambind">
                    </div>
                </div>
                
                <div id="div_editcream" style="display:none;align:center;">
                </div>
                <div id="div_production" style="display: none;">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Production Details</h3>
                        </div>
                    </div>
                    <div align="center">
                        <table align="center">
                            <tr>
                                <td>
                                    <label>
                                        Date<span style="color: red;">*</span></label>
                                    <input id="txt_pdate" class="form-control" type="datetime-local" name="vendorcode"
                                        style="width: 200px;" placeholder="Enter Date">
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td id="td_source">
                                    <label>
                                        Source<span style="color: red;">*</span></label>
                                    <select id="ddltype" class="form-control" onchange="ddltypeonchange();" >
                                        <option selected disabled value="Select Type">Select Type</option>
                                        <option  value="90">Cow Cream</option>
                                        <option value="10">COW GHEE LOOSE</option>
                                        <option value="164">Buffalo</option>
                                        <option value="165">BUFF GHEE LOOSE</option>
                                        <option value="166">EXTRA BUFF GHEE LOOSE</option>
                                    </select>
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td id="td_pcqty">
                                    <label>
                                        O/B<span style="color: red;">*</span></label>
                                    <input id="txt_qty" type="text" class="form-control" name="vendorcode" readonly placeholder="Enter Qty" />
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td id="td_pcobfat">
                                    <label>
                                        O/B Cream Fat<span style="color: red;">*</span></label>
                                    <input id="txt_prodobcremfat" type="text" readonly class="form-control" />
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td id="td_pcobsnf">
                                    <label>
                                        O/B Cream SNF<span style="color: red;">*</span></label>
                                    <input id="txt_prodobcremsnf" type="text" readonly class="form-control" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divFillScreen">
                    </div>
                    <div style="padding-left: 325px;">
                        <table align="center">
                            <tr hidden>
                                <td>
                                    <label id="lbl_psno">
                                    </label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Remarks</label>
                                    <textarea rows="3" cols="45" id="txt_pRemarks" class="form-control" maxlength="200"
                                        placeholder="Enter Remarks"></textarea>
                                </td>
                            </tr>
                            </table>
                            <%--<div style="padding-left: 125px;padding-top: 20px;">
                            <table>
                            <tr>
                                <td>
                                    <input id='btnproduction' type="button" class="btn btn-success" name="submit" value='Save'
                                        onclick="save_ghee_production_click()" />
                                    <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                        onclick="pclearvalues()" />
                                </td>
                            </tr>
                        </table>
                        </div>--%>
                         <div  style="padding-left: 6%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btnproduction1" onclick="save_ghee_production_click()"></span><span id="btnproduction" onclick="save_ghee_production_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="pclearvalues()"></span><span id='btn_close' onclick="pclearvalues()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                    <div id="div_editgheeproduct">
                    </div>
                    </div>
                <div id="div_editproduction" style="display:none;align:center;">
                    </div>
                <div id="divproductionsales" style="display: none;">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>sales Details</h3>
                        </div>
                    </div>
                    <div style="padding-left: 40%;">
                        <table>
                            <tr>
                                <label>
                                    Date<span style="color: red;">*</span></label>
                                <input id="txt_sdate" class="form-control" type="datetime-local" name="vendorcode"
                                    style="width: 200px;" placeholder="Enter Date">
                            </tr>
                        </table>
                    </div>
                    <div id="divsales">
                    </div>
                    <div style="padding-left: 380px;">
                        <table align="center">
                            <tr hidden>
                                <td>
                                    <label id="lbl_ssno">
                                    </label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Remarks</label>
                                    <textarea rows="3" cols="45" id="txt_sRemarks" class="form-control" maxlength="200"
                                        placeholder="Enter Remarks"></textarea>
                                </td>
                            </tr>
                            </table>
                            <%--<div style="padding-left: 120px;padding-top: 20px;">
                              <table>
                                <tr>
                                    <td>
                                        <input id='btnsales' type="button" class="btn btn-success" name="submit" value='Save'
                                            onclick="save_ghee_sales_click()" />
                                        <input id='btnclear' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                            onclick="salesclearvalues()" />
                                    </td>
                                </tr>
                            </table>
                        </div>--%>
                        <div  style="padding-left: 6%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btnsales1" onclick="save_ghee_sales_click()"></span><span id="btnsales" onclick="save_ghee_sales_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btnclear1' onclick="salesclearvalues()"></span><span id='btnclear' onclick="salesclearvalues()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                    <div id="div_editsales">
                    </div>
                </div>
                <div id="div_salesedit" style="display:none">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
