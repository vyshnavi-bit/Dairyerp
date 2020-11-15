<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="butterproductiondetails.aspx.cs" Inherits="butterproductiondetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            get_buttercreambinding_details();
            $('#lidop').click(function () {
                $('#butter_fillform').css('display', 'block');
                $('#div_production').css('display', 'none');
                $('#divproductionsales').css('display', 'none');
                $('#div_creambind').show();
                $('#div_editbutterprod').hide();
                $('#div_editcream').hide();
                get_buttercreambinding_details();
            });
            $('#btn_ceclear').click(function () {
                $('#butter_fillform').css('display', 'block');
                $('#div_production').css('display', 'none');
                $('#divproductionsales').css('display', 'none');
                $('#div_creambind').show();
                $('#div_editcream').hide();
                $('#div_editbutterprod').hide();
                get_buttercreambinding_details();
                CeClearvalues();
            });
            $('#lidos').click(function () {
                $('#butter_fillform').css('display', 'none');
                $('#div_production').css('display', 'block');
                $('#divproductionsales').css('display', 'none');
                $('#div_getbpdata').show();
                $('#div_editbutterprod').hide();
                $('#div_editcream').hide();
                get_butterproductionbinding_details();
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
            });
            $('#btn_ebpclose').click(function () {
                $('#butter_fillform').css('display', 'none');
                $('#div_production').css('display', 'block');
                $('#divproductionsales').css('display', 'none');
                $('#div_getbpdata').show();
                $('#div_creambind').hide();
                $('#div_editcream').hide();
                $('#div_editbutterprod').hide();
                get_butterproductionbinding_details();
            });
            $('#lisales').click(function () {
                $('#butter_fillform').css('display', 'none');
                $('#div_production').css('display', 'none');
                $('#divproductionsales').css('display', 'block');
                $('#div_editbutterprod').hide();
                $('#div_editcream').hide();
                fillbutterproductsales();
                get_buttersales_details();
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
        // Cream 
        function get_buttercreambinding_details() {
            var data = { 'op': 'get_buttercreambinding_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcreamdetails(msg);
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
        function fillcreamdetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Cream Type</th><th scope="col" style="font-weight: bold;">O/B</th><th scope="col" style="font-weight: bold;">O/B FAT</th><th scope="col" style="font-weight: bold;">Cream Received(qty)</th><th scope="col" style="font-weight: bold;">Cream Received(fat)</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getmecream(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1">' + msg[i].creamtype + '</td>';
                results += '<td  class="2">' + msg[i].ob + '</td>';
                results += '<td  class="3" >' + msg[i].obfat + '</td>';
                results += '<td  class="4" >' + msg[i].recivequnatity + '</td>';
                results += '<td  class="5" >' + msg[i].recivefat + '</td>';
                results += '<td  class="6" >' + msg[i].doe + '</td>';
                results += '<td  class="8" style="display:none" >' + msg[i].totalcreamqty + '</td>';
                results += '<td  class="9" style="display:none" >' + msg[i].avgfat + '</td>';
                results += '<td  class="12" style="display:none">' + msg[i].datetime + '</td>';
                results += '<td style="display:none" class="7">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmecream(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_creambind").html(results);
        }
        function getmecream(thisid) {
            var creamtype = $(thisid).parent().parent().children('.1').html();
            var ob = $(thisid).parent().parent().children('.2').html();
            var obfat = $(thisid).parent().parent().children('.3').html();
            var recivequnatity = $(thisid).parent().parent().children('.4').html();
            var recivefat = $(thisid).parent().parent().children('.5').html();
            var doe = $(thisid).parent().parent().children('.12').html();
            var sno = $(thisid).parent().parent().children('.7').html();
            var totalcreamqty = $(thisid).parent().parent().children('.8').html();
            var avgfat = $(thisid).parent().parent().children('.9').html();

            document.getElementById('txt_date').value = doe;
            document.getElementById('ddlcreamtype').value = creamtype;
            document.getElementById('txt_ob').value = ob;
            document.getElementById('txt_recive').value = recivequnatity;
            document.getElementById('txt_recivedfat').value = recivefat;
            document.getElementById('txt_creamfat').value = obfat;
            document.getElementById('txt_total').value = totalcreamqty;
            document.getElementById('txt_avgfat').value = avgfat;
            document.getElementById('lbl_sno').value = sno;
            document.getElementById('btn_creamsave').innerHTML = "Modify";


            $("#div_editcream").hide();
            $('#butter_fillform').show();
            $('#div_production').hide();
            $('#divproductionsales').hide();
            $('#div_creambind').hide();
        }
        function save_buttercream_production_click() {
            var date = document.getElementById('txt_date').value;
            if (date == "") {
                alert("Please select date");
                $('#txt_date').focus();
                return false;
            }
            var creamtype = document.getElementById('ddlcreamtype').value;
            if (creamtype == "" || creamtype == "Select Cream Type") {
                alert("Please Select Cream Type");
                $('#ddlcreamtype').focus();
                return false;
            }
            var ob = document.getElementById('txt_ob').value;
            var recive = document.getElementById('txt_recive').value;
            if (recive == "") {
                alert("Please enter Cream Received quantity");
                $('#txt_recive').focus();
                return false;
            }
            var recivedfat = document.getElementById('txt_recivedfat').value;
            if (recivedfat == "") {
                alert("Please enter Cream Received fat");
                $('#txt_recivedfat').focus();
                return false;
            }
            var creamfat = document.getElementById('txt_creamfat').value;
            if (creamfat == "") {
                alert("Please enter cream fat");
                $('#txt_creamfat').focus();
                return false;
            }
            var total = document.getElementById('txt_total').value;
            var avgfat = document.getElementById('txt_avgfat').value;
            var btnvalue = document.getElementById('btn_creamsave').innerHTML;
            var sno = document.getElementById('lbl_sno').value;
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_buttercream_production_click', 'creamtype': creamtype, 'ob': ob, 'recive': recive, 'recivedfat': recivedfat, 'creamfat': creamfat, 'total': total, 'avgfat': avgfat, 'doe': date, 'btnvalue': btnvalue, 'sno': sno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            Clearvalues();
                            get_buttercreambinding_details();
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
            document.getElementById('btn_creamsave').innerHTML = "Save";
            $('#butter_fillform').css('display', 'block');
            $('#div_production').css('display', 'none');
            $('#divproductionsales').css('display', 'none');
            $('#div_creambind').show();
            $('#div_editcream').hide();
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

        function fatchange(fatval) {
            var Recive = document.getElementById('txt_recive').value;
            var ob = document.getElementById('txt_ob').value;
            var totalcreamqty = document.getElementById('txt_total').value;
            var recivedfat = document.getElementById('txt_recivedfat').value;
            if (Recive == "") {
                alert("Please enter Receive");
                return false;
            }
            var recipts = fatval.value;
            var x = parseFloat(recipts) / 100;
            var y = parseFloat(x) * parseFloat(ob);

            var xx = parseFloat(recivedfat) / 100;
            var yy = parseFloat(xx) * parseFloat(Recive);

            var zz = parseFloat(y) + parseFloat(yy);

            var a = parseFloat(totalcreamqty) / 100;
            var ab = parseFloat(zz) / parseFloat(a);
            document.getElementById('txt_avgfat').value = parseFloat(ab).toFixed(2);
        }

        function creamchange() {
            var productid = document.getElementById('ddlcreamtype').value;
            var data = { 'op': 'get_buttercream_details', 'productid': productid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].creamtype == "0") {
                                document.getElementById('txt_ob').value = "0";
                            }
                            else {
                                document.getElementById('txt_ob').value = msg[i].creamtype;
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
                var convertionqty = document.getElementById('txt_qty').value;
                var recipts = qtyid.value;
                var sum = parseFloat(convertionqty) * parseFloat(recipts);
                var pqty = parseFloat(sum / 100).toFixed(2);
                $(qtyid).closest("tr").find('#txt_production').val(pqty);
            }
        }
        // Butter Production
        function get_butterproductionbinding_details() {
            var data = { 'op': 'get_butterproductionbinding_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbutterproductiondetails(msg);
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
        function fillbutterproductiondetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">ProductName</th><th scope="col" style="font-weight: bold;">OpeningBalance</th><th scope="col" style="font-weight: bold;">Conversion(Quantity)</th><th scope="col" style="font-weight: bold;">ConvertionFat</th><th scope="col" style="font-weight: bold;">Production(qty)</th><th scope="col" style="font-weight: bold;">ClosingBalance</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getmebutterproduction(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td class="2" style="text-align:center;">' + msg[i].openingbalance + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].convertionquantity + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].convertionfat + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].productionqty + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].closingbalance + '</td>';
                results += '<td  class="7" style="text-align:center;">' + msg[i].doe + '</td>';
                results += '<td style="display:none" class="8">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].creamtype + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].remarks + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].type + '</td>';
                results += '<td style="display:none" class="14">' + msg[i].datetime + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmebutterproduction(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_getbpdata").html(results);
        }
        function getmebutterproduction(thisid) {
            var productname = $(thisid).parent().parent().children('.1').html();
            var openingbalance = $(thisid).parent().parent().children('.2').html();
            var convertionquantity = $(thisid).parent().parent().children('.3').html();
            var convertionfat = $(thisid).parent().parent().children('.4').html();
            var productionqty = $(thisid).parent().parent().children('.5').html();
            var closingbalance = $(thisid).parent().parent().children('.6').html();
            var doe = $(thisid).parent().parent().children('.14').html();
            var productid = $(thisid).parent().parent().children('.8').html();
            var sno = $(thisid).parent().parent().children('.9').html();
            var remarks = $(thisid).parent().parent().children('.11').html();
            var type = $(thisid).parent().parent().children('.12').html();


            document.getElementById('txt_pdate').value = doe;
            document.getElementById('ddltype').value = type;
            document.getElementById('txt_qty').value = openingbalance;
            document.getElementById('lbl_psno').value = sno;
            document.getElementById('txt_pRemarks').value = remarks;
            document.getElementById('btnproduction').innerHTML = "Modify";


            $("#divFillScreen").show();
            var results = '<div    style="overflow:auto;"><table id="table_Ghee_production_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" style="font-weight: 700;">Conversion Qty</th><th scope="col" style="font-weight: 700;">Conversion Fat</th><th scope="col" style="font-weight: 700;">Production Quantity</th></tr></thead></tbody>';
            results += '<tr>';
            results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + sno + '</span></th>';
            results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + productname + '</span></th>';
            results += '<td><input id="txt_OB" class="form-control" onkeypress="return isFloat(event);" value="' + openingbalance + '" type="text"  name="vendorcode"placeholder="Enter OB qty" readonly></td>';
            results += '<td><input id="txt_cqty"  class="form-control" onkeypress="return isFloat(event);" value="' + convertionquantity + '" type="text" name="vendorcode"placeholder="Enter Conversion Qty"></td>';
            results += '<td><input id="txt_cfat"  class="form-control" onkeypress="return isFloat(event);" value="' + convertionfat + '" type="text" name="vendorcode"placeholder="Enter Conversion Fat"></td>';
            results += '<td><input id="txt_production"  class="form-control" onkeypress="return isFloat(event);" value="' + productionqty + '" type="text" name="vendorcode"placeholder="Enter production"></td>';
            results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + productid + '"></td></tr>';
            results += '</table></div>';
            $("#divFillScreen").html(results);

            $("#td_type").hide();
            $("#td_qty").hide();
            $("#div_editbutterprod").hide();
            $("#div_getbpdata").hide();
            $("#div_editcream").hide();
            $('#butter_fillform').hide();
            $('#div_production').show();
            $('#divproductionsales').hide();
            $('#div_creambind').hide();
        }
        var GheeDetails = [];
        function fillbutterproductdetails() {
            var type = document.getElementById('ddltype').value;
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        GheeDetails = msg;
                        if (type == 96) {
                            var results = '<div    style="overflow:auto;"><table id="table_Ghee_production_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" style="font-weight: 700;">Conversion Qty</th><th scope="col" style="font-weight: 700;">Conversion Fat</th><th scope="col" style="font-weight: 700;">Production Quantity</th></tr></thead></tbody>';
                            for (var i = 0; i < msg.length; i++) {
                                if (msg[i].departmentid == "10") {
                                    if (msg[i].productid != "96") {
                                        results += '<tr>';
                                        results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                        results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                        results += '<td><input id="txt_OB" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter OB qty" readonly></td>';
                                        results += '<td><input id="txt_cqty"  class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter Conversion Qty"></td>';
                                        results += '<td><input id="txt_cfat"  class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter Conversion Fat"></td>';
                                        results += '<td><input id="txt_production"  class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Enter production"></td>';
                                        results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                                    }
                                }
                            }
                        }
                        results += '</table></div>';
                        $("#divFillScreen").html(results);
                        $('#divFillScreen').css('display', 'block');
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
        function save_butter_production_click() {
            var type = document.getElementById('ddltype').value;
            var qty = document.getElementById('txt_qty').value;
            var remarks = document.getElementById('txt_pRemarks').value;
            var date = document.getElementById('txt_pdate').value;
            if (date == "") {
                alert("Please select date");
                $('#txt_pdate').focus();
                return false;
            }
            var btnvalue = document.getElementById('btnproduction').innerHTML;
            var sno = document.getElementById('lbl_psno').value;
            var rows = $("#table_Ghee_production_details tr:gt(0)");
            var ghee_production_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_production').val() == "" | $(this).find('#txt_production').val() == null) {
                }
                else {
                    ghee_production_details.push({ productid: $(this).find('#hdn_productid').val(), OB: $(this).find('#txt_OB').val(), productionqty: $(this).find('#txt_production').val(), convertionqty: $(this).find('#txt_cqty').val(), convertionfat: $(this).find('#txt_cfat').val() });
                }
            });
            if (ghee_production_details.length == 0) {
                alert("Please enter opening balance");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_butter_production_click', 'qty': qty, 'type': type, 'ghee_production_details': ghee_production_details, 'type': type, 'date': date, 'remarks': remarks, 'btnvalue': btnvalue, 'sno': sno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            pclearvalues();
                            $('#butter_fillform').css('display', 'none');
                            $('#div_production').css('display', 'block');
                            $('#divproductionsales').css('display', 'none');
                            $('#div_getbpdata').show();
                            $('#div_editbutterprod').hide();
                            $('#div_editcream').hide();
                            get_butterproductionbinding_details();
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
        function pclearvalues() {
            document.getElementById('txt_qty').value = "";
            document.getElementById('ddltype').selectedIndex = 0;
            document.getElementById('txt_pRemarks').value = "";
            document.getElementById('txt_pdate').value = "";
            document.getElementById('btnproduction').innerHTML = "Save";
            document.getElementById('lbl_psno').value = "";
            $('#butter_fillform').css('display', 'none');
            $('#div_production').css('display', 'block');
            $('#divproductionsales').css('display', 'none');
            $('#div_getbpdata').show();
            $('#div_editbutterprod').hide();
            $('#div_editcream').hide();
            $("#td_type").show();
            $("#td_qty").show();
            $('#divFillScreen').css('display', 'none');
            get_butterproductionbinding_details();
        }
        //Butter Sales
        function fillbutterproductsales() {
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div    style="overflow:auto;"><table id="table_sales_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">O/B</th><th scope="col" style="font-weight: 700;">Dispatch To Ghee</th><th scope="col" style="font-weight: 700;">DispatchToGheeFat</th><th scope="col" style="font-weight: 700;">Sales</th><th scope="col" style="font-weight: 700;">Butter Milk QTY</th><th scope="col" style="font-weight: 700;">Butter Milk FAT</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].departmentid == "10") {
                                results += '<tr>';
                                results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                results += '<td><input id="txt_OB" class="form-control" onkeypress="return isFloat(event);" value="' + msg[i].quantity + '" type="text" name="vendorcode"placeholder="Enter OB qty"></td>';
                                results += '<td><input id="txt_dipatchtoghee" class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode" placeholder="Enter dispatch qty"></td>';
                                results += '<td><input id="txt_dipatchtogheefat" class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode"placeholder="Dipatch to Ghee Fat"></td>';
                                results += '<td><input id="txt_sales" class="form-control" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter Sales qty"></td>';
                                results += '<td><input id="txt_buttermilkquantity" class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode" placeholder="Buttermilk qty"></td>';
                                results += '<td><input id="txt_buttermilkfat" class="form-control" onkeypress="return isFloat(event);" value="" type="text" name="vendorcode" placeholder="Buttermilk Fat"></td>';
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
        function save_butter_sales_click() {
            var remarks = document.getElementById('txt_sRemarks').value;
            var date = document.getElementById('txt_sdate').value;
            var btnvalue = document.getElementById('btnsales').innerHTML;
            var sno = document.getElementById('lbl_ssno').value;
            if (date == "") {
                alert("Please Select Date");
                $('#txt_sdate').focus();
                return false;
            }
            var rows = $("#table_sales_wise_details tr:gt(0)");
            var ghee_sales_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_dispatchqty').val() != "") {
                    ghee_sales_details.push({ productid: $(this).find('#hdn_productid').val(), Sales: $(this).find('#txt_sales').val(), dispatchtoghee: $(this).find('#txt_dipatchtoghee').val(), dispatchtogheefat: $(this).find('#txt_dipatchtogheefat').val(), buttermilkquantity: $(this).find('#txt_buttermilkquantity').val(), buttermilkfat: $(this).find('#txt_buttermilkfat').val() });
                }
            });
            if (ghee_sales_details.length == 0) {
                alert("Please enter opening balance");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_butter_sales_click', 'ghee_sales_details': ghee_sales_details, 'date': date, 'remarks': remarks, 'btnvalue': btnvalue, 'sno': sno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            salesclearvalues();
                            get_buttersales_details();
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
            var type = document.getElementById('ddltype').value;
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            if (type == msg[i].productid) {
                                if (msg[i].quantity == "") {
                                    document.getElementById('txt_qty').value = 0;
                                    fillbutterproductdetails();
                                }
                                else {
                                    document.getElementById('txt_qty').value = msg[i].quantity;
                                    fillbutterproductdetails();
                                }
                            }
                        }
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
        function get_buttersales_details() {
            var data = { 'op': 'get_buttersales_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillsalesdetails(msg);
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
        function fillsalesdetails(msg) {
            var results = '<div    style="overflow:auto;"><table id="table_sales_wise_details" class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">ProductName</th><th scope="col" style="font-weight: bold;">Sale Quantity</th><th scope="col" style="font-weight: bold;">Dispatch To Ghee</th><th scope="col" style="font-weight: bold;">Butter Milk QTY</th><th scope="col" style="font-weight: bold;">Remarks</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getmesales(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td class="2" style="text-align:center;">' + msg[i].salesquantity + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].dispatchtoghee + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].buttermilkquantity + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].remarks + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].doe + '</td>';
                results += '<td  class="7" style="display:none" >' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="8">' + msg[i].buttermilkfat + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].dispgheefat + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="14">' + msg[i].datetime + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmesales(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_getsaledetails").html(results);
        }
        function getmesales(thisid) {
            var productname = $(thisid).parent().parent().children('.1').html();
            var salesquantity = $(thisid).parent().parent().children('.2').html();
            var dispatchtoghee = $(thisid).parent().parent().children('.3').html();
            var buttermilkquantity = $(thisid).parent().parent().children('.4').html();
            var remarks = $(thisid).parent().parent().children('.5').html();
            var doe = $(thisid).parent().parent().children('.14').html();
            var productid = $(thisid).parent().parent().children('.7').html();
            var buttermilkfat = $(thisid).parent().parent().children('.8').html();
            var dispgheefat = $(thisid).parent().parent().children('.9').html();
            var productid = $(thisid).parent().parent().children('.10').html();
            var sno = $(thisid).parent().parent().children('.11').html();

            document.getElementById('txt_sdate').value = doe;
            document.getElementById('lbl_ssno').value = sno;
            document.getElementById('txt_sRemarks').value = remarks;
            document.getElementById('btnsales').innerHTML = "Modify";

            $("#divsales").show();
            var results = '<div    style="overflow:auto;"><table id="table_sales_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Dispatch To Ghee</th><th scope="col" style="font-weight: 700;">DispatchToGheeFat</th><th scope="col" style="font-weight: 700;">Sales</th><th scope="col" style="font-weight: 700;">Butter Milk QTY</th><th scope="col" style="font-weight: 700;">Butter Milk FAT</th></tr></thead></tbody>';
            results += '<tr>';
            results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + sno + '</span></th>';
            results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + productname + '</span></th>';
            results += '<td style="display:none"><input id="txt_OB" hidden class="form-control" value="" type="text" name="vendorcode"placeholder="Enter OB qty"></td>';
            results += '<td><input id="txt_dipatchtoghee" class="form-control" onkeypress="return isFloat(event);" value="' + dispatchtoghee + '" type="text" name="vendorcode" placeholder="Enter dispatch qty"></td>';
            results += '<td><input id="txt_dipatchtogheefat" class="form-control" onkeypress="return isFloat(event);" value="' + dispgheefat + '" type="text" name="vendorcode"placeholder="Dipatch to Ghee Fat"></td>';
            results += '<td><input id="txt_sales" class="form-control" onkeypress="return isFloat(event);" value="' + salesquantity + '" type="text" name="vendorcode"placeholder="Enter Sales qty"></td>';
            results += '<td><input id="txt_buttermilkquantity" class="form-control" onkeypress="return isFloat(event);" value="' + buttermilkquantity + '" type="text" name="vendorcode" placeholder="Buttermilk qty"></td>';
            results += '<td><input id="txt_buttermilkfat" class="form-control" onkeypress="return isFloat(event);" value="' + dispgheefat + '" type="text" name="vendorcode" placeholder="Buttermilk Fat"></td>';
            results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + productid + '"></td></tr>';
            results += '</table></div>';
            $("#divsales").html(results);

            $("#div_editbutterprod").hide();
            $("#div_getbpdata").hide();
            $("#div_editcream").hide();
            $('#butter_fillform').hide();
            $('#div_production').hide();
            $('#divproductionsales').show();
            $('#div_creambind').hide();
            $('#div_getsaledetails').hide();
        }
        function salesclearvalues() {
            document.getElementById('txt_sdate').value = "";
            document.getElementById('btnsales').innerHTML = "Save";
            $('#butter_fillform').css('display', 'none');
            $('#div_production').css('display', 'none');
            $('#divproductionsales').css('display', 'block');
            $('#div_editbutterprod').hide();
            $('#div_editcream').hide();
            $('#div_getsaledetails').show();
            fillbutterproductsales();
            get_buttersales_details();
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
            Butter Production Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Curd</a></li>
            <li><a href="#">Butter Production Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="navigation">
                <ul style="padding-left: 350px;">
                    <li id="lidop"><a href="#" class="active"><span id="spandp">Cream Details</span></a></li>
                    <li>&nbsp</li>
                    <li id="lidos"><a href="#"><span id="spands">Butter Production Details</span></a></li>
                    <li>&nbsp</li>
                    <li id="lisales"><a href="#"><span id="span1">Butter Sales Details</span></a></li>
                </ul>
            </div>
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Butter Production Details
                </h3>
            </div>
            <div class="box-body">
                <div id='butter_fillform'>
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Butter Cream Details</h3>
                        </div>
                        <div id="div_vendordata">
                        </div>
                    </div>
                    <div style="padding-left: 140px;">
                        <table align="center" style="width: 65%;">
                            <tr>
                                <td>
                                    <label>
                                        Date<span style="color: red;">*</span></label>
                                    <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                        placeholder="Enter Date">
                                </td>
                                <td>
                                </td>
                                <td id="td_creamtype">
                                    <label>
                                        Cream Type<span style="color: red;">*</span></label>
                                    <select id="ddlcreamtype" class="form-control"  onchange="creamchange()">
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
                                    <input id="txt_ob" type="text" class="form-control" onkeypress="return isFloat(event);" readonly name="vendorcode" placeholder="Enter o/b" />
                                    <label id="lbl_phe" class="errormessage">
                                        * Please enter O/B</label>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        Butter Cream Received Quantity<span style="color: red;">*</span></label>
                                    <input id="txt_recive" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Received Quantity"
                                        onkeyup="totalcreamqty(this)">
                                </td>
                                <td style="padding: 1px;">
                                    <label>
                                       Butter Cream Received Fat<span style="color: red;"></span></label>
                                    <input id="txt_recivedfat" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);" placeholder="Enter  Cream Received Fat" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        O/B Fat</label>
                                    <input id="txt_creamfat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter fat"
                                        onkeyup="fatchange(this)" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <label>
                                        Total Cream Quantity<span style="color: red;">*</span></label>
                                    <input id="txt_total" type="text" class="form-control" name="vendorcode" placeholder="Enter  Quantity"
                                        readonly />
                                </td>
                                &nbsp&nbsp&nbsp
                                <td style="padding: 1px;">
                                    <label>
                                        AVG Fat<span style="color: red;"></span></label>
                                    <input id="txt_avgfat" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter  Quantity"
                                        readonly />
                                </td>
                            </tr>
                            <tr style="display:none;">
                            <td>
                            <input id="lbl_sno" type="text" />
                            </td>
                            </tr>
                        </table>
                    </div>
                   <%-- <div style="padding-left: 140px;">
                        <table align="center">
                            <tr>
                                <td>
                                    <input id='btn_creamsave' type="button" class="btn btn-success" name="submit" value='Save'
                                        onclick="save_buttercream_production_click()" />
                                    <input id='btn_creamclear' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                        onclick="Clearvalues()" />
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                     <div  style="padding-left: 38%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_creamsave1" onclick="save_buttercream_production_click()"></span><span id="btn_creamsave" onclick="save_buttercream_production_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_creamclear1' onclick="Clearvalues()"></span><span id='btn_creamclear' onclick="Clearvalues()">Clear</span>
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
                <div id="div_editcream" style="display:none;">
                </div>
                <div id="div_production" style="display: none;">
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Butter Production Details</h3>
                        </div>
                    </div>
                    <div style="padding-left: 22%;">
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
                                <td id="td_type">
                                    <label>
                                        Source<span style="color: red;">*</span></label>
                                    <select id="ddltype" class="form-control" onchange="ddltypeonchange();">
                                        <option selected disabled value="Select Type">Select Type</option>
                                        <option  value="96">Cream</option>
                                    </select>
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td id="td_qty">
                                    <label>
                                        Qty<span style="color: red;">*</span></label>
                                    <input id="txt_qty" type="text" class="form-control" name="vendorcode" placeholder="Enter Qty" readonly/>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divFillScreen">
                    </div>
                    <div style="padding-left: 140px;">
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
                            <%--<tr>
                                <td>
                                    <input id='btnproduction' type="button" class="btn btn-success" name="submit" value='Save'
                                        onclick="save_butter_production_click()" />
                                    <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                        onclick="pclearvalues()" />
                                </td>
                            </tr>--%>
                        </table>
                        <div  style="padding-left: 36%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btnproduction1" onclick="save_butter_production_click()"></span><span id="btnproduction" onclick="save_butter_production_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="pclearvalues()"></span><span id='btn_close' onclick="pclearvalues()">Clear</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                    <div id="div_getbpdata">
                    </div>
                </div>
                <div id="div_editbutterprod" style="display: none;">
                </div>
                <div id="divproductionsales" style="display: none;">
                    <div class="box box-danger">
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
                    <div style="padding-left: 140px;">
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
                            <%--<tr>
                                <td>
                                    <input id='btnsales' type="button" class="btn btn-success" name="submit" value='Save'
                                        onclick="save_butter_sales_click()" />
                                    <input id='btnclear' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                        onclick="salesclearvalues()" />
                                </td>
                            </tr>--%>
                        </table>
                        <div  style="padding-left: 10%;padding-top: 5%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btnsales1" onclick="save_butter_sales_click()"></span><span id="btnsales" onclick="save_butter_sales_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btnclear1' onclick="salesclearvalues()"></span><span id='btnclear' onclick="salesclearvalues()">Clear</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                    <div id="div_getsaledetails">
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>