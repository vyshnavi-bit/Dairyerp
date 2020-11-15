<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="cipandsmpdetails.aspx.cs" Inherits="cipandsmpdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#div_cipdetails').css('display', 'block');
            $('#div_smpdetails').css('display', 'none');
            $('#div_powerdetails').css('display', 'none');
            $('#btn_addDept').click(function () {
                $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_CIPdata').hide();
                get_Silos_cip();
            });
            $('#btn_close').click(function () {
                $('#Inwardsilo_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_CIPdata').show();
                Clearvalues();
            });
            get_Silos();
            get_cipcleaning_details_cip();
            $('#btn_addsmp').click(function () {
                $('#Inwardsilo_fillform_smp').css('display', 'block');
                $('#showlogs_smp').css('display', 'none');
                $('#div_smpget').hide();
                $('#btn_addsmp').hide();
            });
            $('#btn_closesmp').click(function () {
                $('#Inwardsilo_fillform_smp').css('display', 'none');
                $('#showlogs_smp').css('display', 'block');
                $('#btn_addsmp').show();
                $('#div_smpget').show();
                clearvalues_smp();
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
            $('#txt_startingtimepower').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#txt_endingdatepower').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#txt_startingtimepower').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#txt_endingdatepower').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            $('#txt_datesmp').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        });
        function Clearvalues(){
        $('#Inwardsilo_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_CIPdata').show();
                Clearvalues();
        }
        function addsmpdetails(){
        $('#Inwardsilo_fillform_smp').css('display', 'block');
                $('#showlogs_smp').css('display', 'none');
                $('#div_smpget').hide();
        }
        function addcipdetails(){
        $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_CIPdata').hide();
                get_Silos_cip();
        }
        function showcipdetails() {
            $('#div_cipdetails').css('display', 'block');
            $('#div_smpdetails').css('display', 'none');
            $('#div_powerdetails').css('display', 'none'); 
            get_Silos_cip();
            get_cipcleaning_details_cip();
        }
        function showsmpdetails() {
            $('#div_cipdetails').css('display', 'none');
            $('#div_smpdetails').css('display', 'block');
            $('#showlogs_smp').show();
            $('#div_powerdetails').css('display', 'none'); 
            get_smp_details();
        }
        function showpowerdetails(){
            $('#div_cipdetails').css('display', 'none');
            $('#div_smpdetails').css('display', 'none');
            $('#div_powerdetails').css('display', 'block'); 
            get_Silos();
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

        function get_Silos_cip() {
            var data = { 'op': 'get_Silo_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillfSilos_cip(msg);
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
        function fillfSilos_cip(msg) {
            var data = document.getElementById('slct_Source_Name');
            var length = data.options.length;
            document.getElementById('slct_Source_Name').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Silo";
            opt.value = "Select Silo";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].SiloName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].SiloName;
                    option.value = msg[i].SiloId;
                    data.appendChild(option);
                }
            }
        }

        function get_cipcleaning_details_cip() {
        var data = { 'op': 'get_cipcleaning_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillcipdatacleaningdetails(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function fillcipdatacleaningdetails(msg) {
        var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable"  role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">SiloName</th><th scope="col" style="font-weight: bold;">Chemical</th><th scope="col" style="font-weight: bold;">Temperature</th><th scope="col" style="font-weight: bold;">StartingTime</th><th scope="col" style="font-weight: bold;">EndingTime</th><th scope="col" style="font-weight: bold;">Quantity</th></tr></thead></tbody>';
        var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
        results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Capacity" class="2">' + msg[i].siloname + '</td>';
                results += '<td  class="3" >' + msg[i].chemical + '</td>';
                results += '<td  class="4" >' + msg[i].tempurature + '</td>';
                results += '<td  class="5" >' + msg[i].startingtime + '</td>';
                results += '<td  class="6" >' + msg[i].endingtime + '</td>';
                results += '<td class="10">' + msg[i].quantity + '</td>';
                results += '<td style="display:none" class="7">' + msg[i].actualstreangth + '</td>';
                results += '<td style="display:none" class="7">' + msg[i].siloid + '</td>';
                results += '<td style="display:none" class="8">' + msg[i].remarks + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].sno + '</td></tr>';
                 l = l + 1;
                if (l == 4) {
                    l = 0;
                }
        }
        results += '</table></div>';
        $("#div_CIPdata").html(results);
    }
        function getmecip(thisid) {
            var sno = $(thisid).parent().parent().children('.9').html();
            var productid = $(thisid).parent().parent().children('.7').html();
            var remarks = $(thisid).parent().parent().children('.8').html();
            var cutting = $(thisid).parent().parent().children('.5').html();
            var rootname = $(thisid).parent().parent().children('.4').html();
            var dispatchquantity = $(thisid).parent().parent().children('.3').html();
            var quantity = $(thisid).parent().parent().children('.10').html();
            document.getElementById('slct_product').value = productid;
            document.getElementById('txt_qty').value = quantity;
            document.getElementById('txt_dqty').value = dispatchquantity;
            document.getElementById('txt_root').value = rootname;
            document.getElementById('txt_Remarks').value = remarks;
            document.getElementById('txt_cutting').value = cutting;
            document.getElementById('lbl_sno').value = sno;
            document.getElementById('save_batchdetails').innerHTML = "Modify";
            $("#div_CIPdata").hide();
            $("#Inwardsilo_fillform").show();
            $('#showlogs').hide();
        }
        function save_cipcleaningdetails_click() {
            var siloid = document.getElementById('slct_Source_Name').value;
            var chemical = document.getElementById('slct_chemical').value;
            var temperature = document.getElementById('txt_temperature').value;
            var qty = document.getElementById('txt_qty').value;
            var starttime = document.getElementById('txt_starttime').value;
            var endingtime = document.getElementById('txt_endingtime').value;
            var date = document.getElementById('txtdate').value;
            var actualstrength = document.getElementById('txt_actualstrength').value;
            var remarks = document.getElementById('txt_Remarks').value;
            var btnvalue = document.getElementById('save_batchdetails').innerHTML;
            var sno = document.getElementById('lbl_sno').value;
            if (siloid == "" || siloid == "Select Silo") {
                alert("Please select Silo");
                 $("#slct_Source_Name").focus();
                return false; 
            }
            if (chemical == "") {
                alert("Select Chemical Type");
                $("#slct_chemical").focus();
                return false;
            }
            if (temperature == "") {
                alert("Enter temperature");
                 $("#txt_temperature").focus();
                return false;
            }
            if (qty == "") {
                alert("Enter qty");
                $("#txt_qty").focus();
                return false;
            }
             if (starttime == "") {
                alert("Enter starttime");
                return false;
            }
             if (endingtime == "") {
                alert("Enter endingtime");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_cipcleaningdetails_click',  'siloid':siloid, 'chemical':chemical, 'temperature':temperature, 'qty':qty, 'starttime':starttime, 'endingtime':endingtime, 'actualstrength':actualstrength, 'remarks':remarks, 'date':date, 'btnvalue': btnvalue, 'sno': sno, };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            Clearvalues();
                            get_cipcleaning_details_cip();
                            $('#Inwardsilo_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                            $('#div_CIPdata').show();
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
            document.getElementById('txt_qty').value = "";
            document.getElementById('slct_chemical').selectedIndex="";
            document.getElementById('txt_temperature').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('txt_actualstrength').value = "";
        }
        
    function validate(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]|\./;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        }
        function save_smp_silotransaction_click() {
            var date = document.getElementById('txt_datesmp').value;
            var Qtykgs = document.getElementById('txt_qtykgssmp').value;
            var ob = document.getElementById('txt_obsmp').value;
            var recived = document.getElementById('txt_recivedsmp').value;
            var stocktransfor = document.getElementById('txt_stocktransforsmp').value;
            var fat = document.getElementById('txt_fatsmp').value;
            var snf = document.getElementById('txt_snfsmp').value;
            var sno = document.getElementById('lbl_snosmp').innerHTML;
            var btnval = document.getElementById('save_smpsilo').innerHTML;
            var flag = false;
            if (date == "") {
                alert("please enter date");
                return false;
            }
            if (ob == "") {
                alert("please enter opening balance");
                return false;
            }
            if (recived == "") {
                alert("please enter received qty");
                return false;
            }
            if (Qtykgs == "") {
                alert("please enter consumption qty");
                return false;
            }
            if (fat == "") {
                alert("please enter fat");
                return false;
            }
            if (snf == "") {
                alert("please enter snf");
                return false;
            }
            if (flag) {
                return;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_smp_silotransaction_click', 'date': date, 'ob': ob, 'recived': recived, 'fat': fat, 'snf': snf, 'Qtykgs': Qtykgs, 'stocktransfor':stocktransfor, 'btnval': btnval, 'sno': sno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_smp_details();
                            clearvalues_smp();
                            $('#div_smpget').show();
                            $('#btn_addsmp').show();
                            $('#Inwardsilo_fillform_smp').css('display', 'none');
                            $('#showlogs_smp').css('display', 'block');
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
        function clearvalues_smp() {
            document.getElementById('txt_datesmp').value = "";
            document.getElementById('txt_qtykgssmp').value = "";
            document.getElementById('txt_fatsmp').value = "";
            document.getElementById('txt_snfsmp').value = "";
            document.getElementById('save_smpsilo').innerHTML= "Save";
            transsno = 0;
            $('#Inwardsilo_fillform_smp').css('display', 'none');
                $('#showlogs_smp').css('display', 'block');
                $('#btn_addsmp').show();
                $('#div_smpget').show();
        }
        function get_smp_details() {
            var data = { 'op': 'get_smp_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillsmpdetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillsmpdetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Quantity(kgs)</th><th style="font-weight: bold;">StockTransfer</th><th scope="col" style="font-weight: bold;">FAT</th><th scope="col" style="font-weight: bold;">SNF</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getmesmp(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td data-title="Code" class="5">' + msg[i].OutwordQuantitykgs + '</td>';
                results += '<td data-title="Code" class="10">' + msg[i].stocktransfor + '</td>';
                results += '<td data-title="Code" class="7">' + msg[i].fat + '</td>';
                results += '<td data-title="Code" class="8">' + msg[i].snf + '</td>';
                results += '<td data-title="Code" class="9">' + msg[i].doe + '</td>';
                results += '<td data-title="Code"  style="display:none;" class="12">' + msg[i].datetime + '</td>';
                results += '<td data-title="Code" style="display:none;" class="11">' + msg[i].transno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getmesmp(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_smpget").html(results);
        }
        var transsno = 0;
        function getmesmp(thisid) {
            transsno = $(thisid).parent().parent().children('.11').html();
            var OutwordQuantitykgs = $(thisid).parent().parent().children('.5').html();
            var fat = $(thisid).parent().parent().children('.7').html();
            var snf = $(thisid).parent().parent().children('.8').html();
            var date = $(thisid).parent().parent().children('.9').html();
            var datetime = $(thisid).parent().parent().children('.12').html();
            document.getElementById('txt_datesmp').value = datetime;
            document.getElementById('txt_qtykgssmp').value = OutwordQuantitykgs;
            document.getElementById('txt_fatsmp').value = fat;
            document.getElementById('txt_snfsmp').value = snf;
            document.getElementById('lbl_snosmp').innerHTML = transsno;
            document.getElementById('save_smpsilo').innerHTML = "Modify";
            $('#Inwardsilo_fillform_smp').css('display', 'block');
            $('#showlogs_smp').css('display', 'none');
            $('#div_smpget').hide();
            $('#btn_addsmp').hide();
        }

        var PaymentType = "";
        function ddlchange(Payment) {
            PaymentType = Payment.options[Payment.selectedIndex].text;
            if (PaymentType == "MilkPosteurizer") {
                $('.divoilpower').css('display', 'none');
                $('.divmilkpower').css('display', 'none');
                $('.divtemppower').css('display', 'table-row');
                $('.divsilopower').css('display', 'table-row');
                get_Silos();
            }
            if (PaymentType == "Homoginizer") {
                $('.divoilpower').css('display', 'table-row');
                $('.divmilkpower').css('display', 'none');
                $('.divtemppower').css('display', 'none');
                $('.divsilopower').css('display', 'none');
                
            }
            if (PaymentType == "Separator") {
                $('.divoilpower').css('display', 'none');
                $('.divmilkpower').css('display', 'table-row');
                $('.divtemppower').css('display', 'none');
                $('.divsilopower').css('display', 'none');
            }
        }

        function get_Silos() {
            var data = { 'op': 'get_Silo_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillfSilos(msg);
                        filltoSilos(msg);
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
        function fillfSilos(msg) {
            var data = document.getElementById('ddlfromsilopower');
            var length = data.options.length;
            document.getElementById('ddlfromsilopower').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Silo";
            opt.value = "Select Silo";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].SiloName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].SiloName;
                    option.value = msg[i].SiloId;
                    data.appendChild(option);
                }
            }
        }
        function filltoSilos(msg) {
            var data = document.getElementById('ddltosilopower');
            var length = data.options.length;
            document.getElementById('ddltosilopower').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Silo";
            opt.value = "Select Silo";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].SiloName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].SiloName;
                    option.value = msg[i].SiloId;
                    data.appendChild(option);
                }
            }
        }

        function save_processing_consumption_click() {
            var transtype = document.getElementById('ddltranstypepower').value;
            var startingtime = document.getElementById('txt_startingtimepower').value;
            var endingtime = document.getElementById('txt_endingdatepower').value;
            var remarks = document.getElementById('txt_remarkspower').value;
            var tosilo = "";
            var fromsilo = "";
            var cctemp = "";
            var chillingtemp = "";
            if (transtype == "" || transtype == "Select Type") {
                alert("Enter Power Type");
                return false;
            }
            if (transtype == "MilkPosteurizer") {
                 tosilo = document.getElementById('ddltosilopower').value;
                 fromsilo = document.getElementById('ddlfromsilopower').value;
                 cctemp = document.getElementById('txt_ccptemppower').value;
                 chillingtemp = document.getElementById('txt_chillingtemppower').value;
                if (startingtime == "") {
                    alert("Enter startingtime");
                    return false;
                }
                if (endingtime == "") {
                    alert("Enter endingtime");
                    return false;
                }
                if (cctemp == "") {
                    alert("Enter cc temp");
                    return false;
                }
                if (chillingtemp == "") {
                    alert("Enter chilling temp");
                    return false;
                }
            }
            var txtoilpresure = "";
            var txthomopresure = "";
            if (transtype == "Homoginizer") {
                txtoilpresure = document.getElementById('txt_oilpresurepower').value;
                txthomopresure = document.getElementById('txt_homopresurepower').value;
                if (txtoilpresure == "") {
                    alert("Enter oil presure");
                    return false;
                }
                if (txthomopresure == "") {
                    alert("Enter homo presure");
                    return false;
                }
            }
            var milktype = "";
            if (transtype == "Separator") {
                milktype = document.getElementById('txt_typeofmilkpower').value;
                if (milktype == "") {
                    alert("Enter Milktype");
                    return false;
                }
            }
            var data = { 'op': 'save_processing_powercollection_click', 'transtype': transtype, 'startingtime': startingtime, 'endingtime': endingtime, 'cctemp': cctemp, 'chillingtemp': chillingtemp, 'fromsilo': fromsilo, 'tosilo': tosilo, 'txtoilpresure': txtoilpresure, 'txthomopresure': txthomopresure, 'milktype': milktype, 'remarks': remarks };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    forclearall_power();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall_power() {
            //document.getElementById('ddltranstypepower').value = "";
            document.getElementById('txt_startingtimepower').value = "";
            document.getElementById('txt_endingdatepower').value = "";
            document.getElementById('txt_remarkspower').value = "";
            document.getElementById('txt_oilpresurepower').value = "";
            document.getElementById('txt_homopresurepower').value = "";
            document.getElementById('txt_ccptemppower').value = "";
            document.getElementById('txt_chillingtemppower').value = "";
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
            <i class="fa fa-files-o" aria-hidden="true"></i>CIP & SMP Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">CIP & SMP Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showcipdetails()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;CIP Cleaning Details</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showsmpdetails()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;SMP Details</a></li>
                       <%-- <li id="Li1" class=""><a data-toggle="tab" href="#" onclick="showpowerdetails()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Processing Section Power Using Details</a></li>--%>
                </ul>
            </div>
            <div id="div_cipdetails">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>CIP Cleaning Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                    <%--<input id="btn_addDept" type="button" name="submit" value='Add CIP Cleaning Details'
                        class="btn btn-success" />--%>
                        <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addcipdetails()"></span> <span onclick="addcipdetails()">Add CIP Details</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_CIPdata">
                </div>
                <div id='Inwardsilo_fillform' style="display: none; padding-left:250px;">
                    <table align="center">
                     <tr>
                        <td>
                                <label>
                                    Date <span style="color: red;">*</span></label>
                                <input id="txtdate" type="date" class="form-control" name="vendorcode" placeholder="Enter Starttime">
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Silo <span style="color: red;">*</span></label>
                                <select id="slct_Source_Name" class="form-control">
                                </select>
                                <label id="Label3" class="errormessage">
                                    * Select Silo Type</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Chemical Type<span style="color: red;">*</span></label>
                                <select id="slct_chemical" type="text" class="form-control" name="vendorcode" >
                                <option selected value disabled>Select Chemical Type</option>
                                <option>CAUSTIC</option>
                                <option>ACID</option>
                                <option>NITRIC ACID</option>
                                <option>HOT WATER</option>
                                <option>TAP WATER</option>
                                <option>RAW WATER</option>
                                </select>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Temperature<span style="color: red;">*</span></label>
                                <input id="txt_temperature" type="text" class="form-control" name="vendorcode" placeholder="Enter Temperature">
                                <label id="lbl_receipts" class="errormessage">
                                    * Please enter Temperature</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Quantity <span style="color: red;">*</span></label>
                                <input id="txt_qty" type="text" class="form-control" onkeypress="validate(event);" name="vendorcode" placeholder="Enter Quantity">
                                <label id="lbl_total" class="errormessage">
                                    * Please enter Quantity</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Start Time<span style="color: red;">*</span></label>
                                <input id="txt_starttime" type="datetime-local" class="form-control" name="vendorcode" placeholder="Enter Starttime">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Ending Time<span style="color: red;">*</span></label>
                                <input id="txt_endingtime" type="datetime-local" class="form-control" name="vendorcode" placeholder="Enter Endingtime">
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Actual Strength <span style="color: red;">*</span></label>
                                <input id="txt_actualstrength" type="text" class="form-control" name="vendorcode"
                                    placeholder="Enter Actual Strength">
                                <label id="Label1" class="errormessage">
                                    * Please enter Actual Strength</label>
                            </td>
                        </tr>
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
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <div style="padding-left: 200px;">
                    <table align="center">
                        <tr>
                            <%--<td>
                                <input id='save_batchdetails' type="button" class="btn btn-success" name="submit"
                                    value='Save' onclick="save_cipcleaningdetails_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="Clearvalues()" />
                            </td>--%>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_batchdetails1" onclick="save_cipcleaningdetails_click()"></span><span id="save_batchdetails" onclick="save_cipcleaningdetails_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="Clearvalues()"></span><span id='btn_close' onclick="Clearvalues()">Clear</span>
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
            <div id="div_smpdetails">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>SMP Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs_smp" style="display: none;text-align: -webkit-right;">
                    <%--<input id="btn_addsmp" type="button" name="submit" value='Add SMP' class="btn btn-success" />--%>
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addsmpdetails()"></span> <span onclick="addsmpdetails()">Add SMP</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_smpget">
                </div>
                <div id='Inwardsilo_fillform_smp' style="display: none;width:100%;" align="center">
                    <table >
                        <tr>
                            <td>
                                <label>
                                    Datetime<span style="color: red;">*</span></label>
                                <input id="txt_datesmp" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date" />
                            </td>
                            <td style="width:10px;"></td>
                           <td>
                                <label>
                                   O/B</label>
                                <input id="txt_obsmp" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);"  placeholder="Enter Opening Balance" />
                            </td>
                            </tr>
                        <tr>
                        <td>
                                <label>
                                   Received Qty(Kgs)</label>
                                <input id="txt_recivedsmp" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);"  placeholder="Enter Qty in Kgs" />
                              
                            </td>
                            <td></td>
                            <td>
                                <label>
                                   Consumption Qty(Kgs)</label>
                                <input id="txt_qtykgssmp" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);"  placeholder="Enter Qty in Kgs" />
                            </td>
                             <td></td>
                            <td>
                                <label>
                                   Stock Transfer Qty(Kgs)</label>
                                <input id="txt_stocktransforsmp" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);"  placeholder="Enter Qty in Kgs" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    FAT<span style="color: red;">*</span></label>
                                <input id="txt_fatsmp" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);" placeholder="Enter FAT">
                            </td>
                            <td></td>
                            <td>
                                <label>
                                    SNF</label>
                                <input id="txt_snfsmp" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);" placeholder="SNF"/>
                            </td>
                        </tr>
                        
                         <tr hidden>
                            <td>
                                <label id="lbl_snosmp">
                                </label>
                            </td>
                        </tr>
                        </table>
                        <div style="padding-top: 20px;">
                        <table>
                        <%--<tr>
                            <td>
                                <input id='save_smpsilo' type="button" class="btn btn-success" name="submit" value='Save'
                                    onclick="save_smp_silotransaction_click()" />
                                <input id='btn_closesmp' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="clearvalues_smp()" />
                            </td>
                        </tr>--%>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_smpsilo1" onclick="save_smp_silotransaction_click()"></span><span id="save_smpsilo" onclick="save_smp_silotransaction_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_closesmp1' onclick="clearvalues_smp()"></span><span id='btn_closesmp' onclick="clearvalues_smp()">Close</span>
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
            <div id="div_powerdetails" style="display:none;">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Processing Section Power using
                    Details
                </h3>
            </div>
            <div class="box-body">
                <table  align="center">
                <tr>
                        <td style="height: 40px;">
                            <span>Power Type</span>
                        </td>
                        <td>
                            <select id="ddltranstypepower" class="form-control" onchange="ddlchange(this)">
                                <option>Select Type</option>
                                <option>MilkPosteurizer</option>
                                <option>Homoginizer</option>
                                <option>Separator</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 50px;">
                            <label>
                                Starting Time</label>
                            <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input id="txt_startingtimepower" class="form-control" type="datetime-local" name="vendorcode"
                                placeholder="Enter startingtime">
                        </td>
                        <td style="height: 50px;">
                            <label>
                                Stoping Time</label>
                            <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input id="txt_endingdatepower" class="form-control" type="datetime-local" name="vendorcode"
                                placeholder="Enter endingdate">
                        </td>
                    </tr>
                    <tr class="divtemppower" style="display:none;">
                        <td style="height: 50px;">
                            <label>
                                CCP Tmpurature</label>
                        </td>
                        <td>
                            <input type="text" maxlength="45" id="txt_ccptemppower" class="form-control" name="vendorcode"
                                placeholder="Enter Amount">
                        </td>
                    
                        <td style="height: 50px;">
                            <label>
                                Chilling Temprature</label>
                            <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input type="text" maxlength="45" id="txt_chillingtemppower" class="form-control" name="vendorcode"
                                placeholder="Enter Amount">
                        </td>
                    </tr>
                    <tr class="divsilopower" style="display:none;">
                        <td style="height: 50px;">
                            <label>
                                From Silo</label>
                            <span style="color: red;">*</span>
                        </td>
                        <td>
                            <select id="ddlfromsilopower" class="form-control">
                            </select>
                        </td>
                   
                        <td style="height: 50px;">
                            <label>
                                To Silo</label>
                            <span style="color: red;">*</span>
                        </td>
                        <td>
                            <select id="ddltosilopower" class="form-control">
                            </select>
                        </td>
                    </tr>
                    <tr class="divoilpower" style="display:none;">
                        <td style="height: 50px;">
                            <label>
                                Oil Pressure</label>
                            <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input type="text" maxlength="45" id="txt_oilpresurepower" class="form-control" name="vendorcode"
                                placeholder="Enter  Oil Pressure">
                        </td>
                   
                        <td style="height: 50px;">
                            <label>
                                Homoginizer Pressure</label>
                            <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input type="text" maxlength="45" id="txt_homopresurepower" class="form-control" name="vendorcode"
                                placeholder="Enter Homoginizer Pressure">
                        </td>
                    </tr>
                    <tr class="divmilkpower" style="display:none;">
                        <td style="height: 50px;">
                            <label>
                                Type Of Milk</label>
                            <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input type="text" maxlength="45" id="txt_typeofmilkpower" class="form-control" name="vendorcode"
                                placeholder="Enter Milk">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 40px;">
                            <span>Remarks</span>
                        </td>
                        <td>
                            <textarea rows="5" cols="45" id="txt_remarkspower" class="ddlsize" maxlength="2000" placeholder="Enter Remarks"></textarea>
                        </td>
                    </tr>
                    <tr hidden>
                        <td>
                            <label id="lbl_snopower">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="height: 40px;">
                            <input type="button" class="btn btn-success" name="submit" id="btn_savepower" value='Save'
                                onclick="save_processing_consumption_click()" />
                            <input id='btn_closepower' type="button" class="btn btn-danger" name="Close" value='Close' />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        </div>
        </div>
    </section>
</asp:Content>
