<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Vendor.aspx.cs" Inherits="Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <link href="css/formstable.css" rel="stylesheet" type="text/css" />
    <link href="css/custom.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
        $(function () {
            get_vendor_details();
            get_state_details();
            $('#btn_addvendor').click(function () {
                $('#fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_vendordata').hide();
                forclearall();
                get_state_details();
            });
            $('#btn_close').click(function () {
                $('#fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_vendordata').show();
                forclearall();
            });
        });
        function addvendordetails() {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_vendordata').hide();
            forclearall();
            get_state_details();
        }
        function closevendors() {
            $('#fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_vendordata').show();
            forclearall();
            get_state_details();
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
        function validate_email(txt_email) {
            var emailid = document.getElementById('txt_email');
            with (emailid) {
                apos = value.indexOf("@");
                dotpos = value.lastIndexOf(".");
                if (value == '') {
                    alert('Field can not be blank');
                    $('#txt_email').focus();
                }
                else if (apos < 1 || dotpos - apos < 2) {
                    alert('Please enter correct email');
                    $('#txt_email').focus();
                    return false;
                }
            }
        }
        function ValidateNos() {
            var phoneNo = document.getElementById('txt_phoneno');
            if (phoneNo.value == "" || phoneNo.value == null) {
                alert("Please enter your Mobile No.");
                $('#txt_phoneno').focus();
                return false;
            }
            if (phoneNo.value.length < 10 || phoneNo.value.length > 10) {
                alert("Mobile No. is not valid, Please Enter 10  Digit Mobile No.");
                $('#txt_phoneno').focus();
                return false;
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
        function for_save_edit_vendor_click() {
            var vendorcode = document.getElementById('txt_vendorcode').value;
            var vendorname = document.getElementById('txt_vendorname').value;
            if (vendorname == "") {
                alert("Please Enter vendor name");
                $("#txt_vendorname").focus();
                return false;
            }
            var addr = document.getElementById('txt_address').value;
            if (addr == "") {
                alert("Please Enter Address");
                $("#txt_address").focus();
                return false;
            }
            var email = document.getElementById('txt_email').value;
            var phoneno = document.getElementById('txt_phoneno').value;
            if (phoneno == "") {
                alert("Please Enter Phone number");
                $("#txt_phoneno").focus();
                return false;
            }
            var kms = document.getElementById('txt_kms').value;
            var exptime = document.getElementById('txt_time').value;
            var cmb_vendortype = document.getElementById('cmb_vendortype').value;
            var branchtype = document.getElementById('ddlbranchtype').value;
            var btnval = document.getElementById('btn_save').innerHTML;

            var sno = document.getElementById('lbl_sno').innerHTML;
            var ledgertype = document.getElementById('txt_ledgertype').value;
            var overhead = document.getElementById('txtoh').value;
            var sapcode = document.getElementById('txtsapcode').value;
            var saleledgertype = document.getElementById('txtsalesledgertype').value;

            var tallyoverheadsales = document.getElementById('txt_tallyoverheadsales').value;
            var ledgercodesales = document.getElementById('txt_ledgercodesales').value;
            var sapvendorcode = document.getElementById('txt_sapvendorcode').value;
            var purchaseoverheadcode = document.getElementById('txt_purchaseoverheadcode').value;
            var salesohcode = document.getElementById('txt_salesohcode').value;

            var coustmername = document.getElementById('txt_coustmername').value;
            var sapcoustmercode = document.getElementById('txt_sapcoustmercode').value;
            var purchasetype = document.getElementById('txt_purchasetype').value;
            var vendortypes = document.getElementById('txt_vendortype').value;
            var gstnnumber = document.getElementById('txt_gstnnumber').value;

            var registertype = document.getElementById('slct_registertype').value;
            var satatename = document.getElementById('slct_satatename').value;
            if (satatename == "" || satatename == "Select State Name") {
                alert("Please Select State Name");
                $("#slct_satatename").focus();
                return false;
            }
            var triplogs_array = [];
            $('#tbl_trip_locations> tbody > tr').each(function () {
                var ddltype = $(this).find('select[name*="ddltype"] :selected').val();
                var Rateon = $(this).find('select[name*="Rateon"] :selected').val();
                var CalculationOn = $(this).find('select[name*="CalculationOn"] :selected').val();
                var RateOnCost = $(this).find('[name="RateOnCost"]').val();
                var OverHeadOn = $(this).find('select[name*="OverHeadOn"] :selected').val();
                var OverHeadCost = $(this).find('[name="OverHeadCost"]').val();
                var MSTDSNF = $(this).find('[name="MSTDSNF"]').val();
                var PSTDSNF = $(this).find('[name="PSTDSNF"]').val();
                var SNFPlus = $(this).find('select[name*="SNFPlus"] :selected').val();
                var MSNFPlusCost = $(this).find('[name="MSNFPlusCost"]').val();
                var PSNFPlusCost = $(this).find('[name="PSNFPlusCost"]').val();
                var TransportOn = $(this).find('select[name*="TransportOn"] :selected').val();
                var KmCost = $(this).find('[name="KmCost"]').val();
                var Transport = $(this).find('[name="Transport"]').val();
                var rowindex = $(this).index();
                var rank = (parseInt(rowindex) + 1).toString();

                var FATPlus = $(this).find('select[name*="FATPlus"] :selected').val();
                var MFATPlusCost = $(this).find('[name="MFATPlusCost"]').val();
                var PFATPlusCost = $(this).find('[name="PFATPlusCost"]').val();
                var MSTDFAT = $(this).find('[name="MSTDFAT"]').val();
                var PSTDFAT = $(this).find('[name="PSTDFAT"]').val();
                triplogs_array.push({ 'ddltype': ddltype, 'Rateon': Rateon, 'CalculationOn': CalculationOn, 'RateOnCost': RateOnCost,
                    'OverHeadOn': OverHeadOn, 'OverHeadCost': OverHeadCost, 'MSTDSNF': MSTDSNF, 'PSTDSNF': PSTDSNF, 'SNFPlus': SNFPlus,
                    'MSNFPlusCost': MSNFPlusCost, 'PSNFPlusCost': PSNFPlusCost, 'TransportOn': TransportOn, 'KmCost': KmCost,
                    'Transport': Transport, 'rank': rank, 'FATPlus': FATPlus, 'MFATPlusCost': MFATPlusCost, 'PFATPlusCost': PFATPlusCost, 'MSTDFAT': MSTDFAT, 'PSTDFAT': PSTDFAT
                });
            });
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var Data = { 'op': 'save_edit_Vendor_click', 'vendorcode': vendorcode, 'vendorname': vendorname,
                    'addr': addr, 'email': email, 'phoneno': phoneno, 'kms': kms, 'exptime': exptime, 'Vendortype': cmb_vendortype, 'sno': vendorsno, 'btnval': btnval, 'ledgertype': ledgertype, 'saleledgertype': saleledgertype, 'overhead': overhead, 'branchtype': branchtype, 'sapcode': sapcode, 'Vendor_subarray': triplogs_array,
                    'tallyoverheadsales': tallyoverheadsales, 'ledgercodesales': ledgercodesales, 'sapvendorcode': sapvendorcode, 'purchaseoverheadcode': purchaseoverheadcode, 'salesohcode': salesohcode,
                    'coustmername': coustmername, 'sapcoustmercode': sapcoustmercode, 'purchasetype': purchasetype, 'vendortypes': vendortypes, 'gstnnumber': gstnnumber,
                    'registertype': registertype, 'satatename': satatename
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        forclearall();
                        get_vendor_details();
                        $('#fillform').css('display', 'none');
                        $('#showlogs').css('display', 'block');
                        $('#div_vendordata').show();
                    }
                }
                var e = function (x, h, e) {
                };
                CallHandlerUsingJson(Data, s, e);
            }
        }
        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            d = encodeURIComponent(d);
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
        function forclearall() {
            document.getElementById('txt_vendorcode').value = "";
            document.getElementById('txt_vendorname').value = "";
            document.getElementById('txt_address').value = "";
            document.getElementById('txt_email').value = "";
            document.getElementById('txt_phoneno').value = "";
            document.getElementById('txt_kms').value = "";
            document.getElementById('txt_time').value = "";
            document.getElementById('txtoh').value = "";
            document.getElementById('txt_ledgertype').value = "";
            document.getElementById('btn_save').innerHTML = "Save";
            document.getElementById('cmb_vendortype').selectedIndex = 0;
            document.getElementById('ddlbranchtype').selectedIndex = 0;
            document.getElementById('txtsalesledgertype').value = "";
            document.getElementById('txtsapcode').value = "";
            document.getElementById('txt_tallyoverheadsales').value = "";
            document.getElementById('txt_ledgercodesales').value = "";
            document.getElementById('txt_sapvendorcode').value = "";
            document.getElementById('txt_purchaseoverheadcode').value = "";
            document.getElementById('txt_salesohcode').value = "";
            document.getElementById('txt_coustmername').value = "";
            document.getElementById('txt_sapcoustmercode').value = "";
            document.getElementById('txt_purchasetype').value = "";
            document.getElementById('txt_vendortype').selectedIndex = 0;
            document.getElementById('txt_gstnnumber').value = "";
            document.getElementById('slct_registertype').selectedIndex = 0;
            document.getElementById('slct_satatename').selectedIndex = 0;
            $('#tbl_trip_locations>tbody> tr').empty();
        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
      
        function get_vendor_details() {
            var data = { 'op': 'get_Vendor_details' };
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
        var Vendor_sub_list = [];
        function filldetails(msg) {
            var results = '<div  id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Vendor Name</th><th scope="col" style="font-weight: bold;">VendorCode</th><th scope="col" style="font-weight: bold;">MobileNumber</th><th scope="col" style="font-weight: bold;">Email</th><th scope="col" style="font-weight: bold;">Distance</th><th scope="col" style="font-weight: bold;">ExpTime</th><th scope="col" style="font-weight: bold;">BranchType</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var Vendor = msg[0].vendordetails;
            Vendor_sub_list = msg[0].vendorsubdetails;
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < Vendor.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getVendorvalues(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<th scope="row" class="2" style="display:none;">' + Vendor[i].vendorname + '</th>';
                results += '<td data-title="brandstatus" class="40"><span class="" style="color: cadetblue;"></span>' + Vendor[i].vendorname + '</td>';
                results += '<td data-title="Vendor Code" class="1">' + Vendor[i].vendorcode + '</td>';
                results += '<td data-title="Mobno" style="display:none;" class="3">' + Vendor[i].phoneno + '</td>';
                results += '<td data-title="brandstatus" class="41"><span class="glyphicon glyphicon-phone-alt" style="color: cadetblue;">&nbsp</span>' + Vendor[i].phoneno + '</td>';
                results += '<td data-title="Email" class="4" style="display:none;">' + Vendor[i].email + '</td>';
                results += '<td data-title="brandstatus" class="42"><span class="fa fa-envelope" style="color: cadetblue;">&nbsp</span>' + Vendor[i].email + '</td>';
                results += '<td data-title="kms" class="5" style="display:none;">' + Vendor[i].kms + '</td>';
                results += '<td data-title="brandstatus" class="42"><span class="glyphicon glyphicon-road" style="color: cadetblue;">&nbsp</span>' + Vendor[i].kms + '</td>';
                results += '<td data-title="Address" class="6" style="display:none;">' + Vendor[i].addr + '</td>';
                results += '<td data-title="Address" class="8" style="display:none;">' + Vendor[i].exptime + '</td>';
                results += '<td data-title="brandstatus" class="43"><span class="fa fa-calendar-times-o" style="color: cadetblue;">&nbsp</span>' + Vendor[i].exptime + '</td>';
                results += '<td data-title="Address" style="display:none;" class="9">' + Vendor[i].branchtype + '</td>';
                results += '<td data-title="brandstatus" class="44"><span class="" style="color: cadetblue;"></span> ' + Vendor[i].branchtype + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="10">' + Vendor[i].Vendortype + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="11">' + Vendor[i].overhead + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="12">' + Vendor[i].ledgertype + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="14">' + Vendor[i].saleledgertype + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="13">' + Vendor[i].sapcode + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="20">' + Vendor[i].tallyoverheadsales + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="21">' + Vendor[i].ledgercodesales + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="22">' + Vendor[i].sapvendorcode + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="23">' + Vendor[i].purchaseoverheadcode + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="24">' + Vendor[i].salesohcode + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="25">' + Vendor[i].coustmername + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="26">' + Vendor[i].sapcoustmercode + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="27">' + Vendor[i].purchasetype + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="28">' + Vendor[i].vendortypes + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="33">' + Vendor[i].gstnnumber + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="34">' + Vendor[i].registertype + '</td>';
                results += '<td data-title="Address"  style="display:none;" class="35">' + Vendor[i].satatename + '</td>';
                results += '<td data-title="Address" class="7" style="display:none;">' + Vendor[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getVendorvalues(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
        }
        var vendorsno = 0;
        function getVendorvalues(thisid) {
            scrollTo(0, 0);
            vendorsno = $(thisid).parent().parent().children('.7').html();
            var vendorcode = $(thisid).parent().parent().children('.1').html();
            vendorcode = replaceHtmlEntites(vendorcode);
            var vendorname = $(thisid).parent().parent().children('.2').html();
            vendorname = replaceHtmlEntites(vendorname);
            var mobno = $(thisid).parent().parent().children('.3').html();
            mobno = replaceHtmlEntites(mobno);
            var email = $(thisid).parent().parent().children('.4').html();
            var kms = $(thisid).parent().parent().children('.5').html();
            var address = $(thisid).parent().parent().children('.6').html();
            var exptime = $(thisid).parent().parent().children('.8').html();
            var branchtype = $(thisid).parent().parent().children('.9').html();
            var Vendortype = $(thisid).parent().parent().children('.10').html();
            var tallyoh = $(thisid).parent().parent().children('.11').html();
            var ledgertype = $(thisid).parent().parent().children('.12').html();
            var sapcode = $(thisid).parent().parent().children('.13').html();
            var saleledgertype = $(thisid).parent().parent().children('.14').html();
            var tallyoverheadsales = $(thisid).parent().parent().children('.20').html();
            var ledgercodesales = $(thisid).parent().parent().children('.21').html();
            var sapvendorcode = $(thisid).parent().parent().children('.22').html();
            var purchaseoverheadcode = $(thisid).parent().parent().children('.23').html();
            var salesohcode = $(thisid).parent().parent().children('.24').html();
            var coustmername = $(thisid).parent().parent().children('.25').html();
            var sapcoustmercode = $(thisid).parent().parent().children('.26').html();
            var purchasetype = $(thisid).parent().parent().children('.27').html();
            var vendortype = $(thisid).parent().parent().children('.28').html();
            var gstnnumber = $(thisid).parent().parent().children('.33').html();
            var registertype = $(thisid).parent().parent().children('.34').html();
            var satatename = $(thisid).parent().parent().children('.35').html();

            document.getElementById('txt_vendorcode').value = vendorcode;
            document.getElementById('txt_vendorname').value = vendorname;
            document.getElementById('txt_address').value = address;
            document.getElementById('txt_email').value = email;
            document.getElementById('txt_phoneno').value = mobno;
            document.getElementById('txt_kms').value = kms;
            document.getElementById('txt_time').value = exptime;
            document.getElementById('ddlbranchtype').value = branchtype;
            document.getElementById('cmb_vendortype').value = Vendortype;
            document.getElementById('txt_ledgertype').value = ledgertype;
            document.getElementById('txtsalesledgertype').value = saleledgertype;
            document.getElementById('txtsapcode').value = sapcode;
            document.getElementById('txtoh').value = tallyoh;
            document.getElementById('txt_tallyoverheadsales').value = tallyoverheadsales;
            document.getElementById('txt_ledgercodesales').value = ledgercodesales;
            document.getElementById('txt_sapvendorcode').value = sapvendorcode;
            document.getElementById('txt_purchaseoverheadcode').value = purchaseoverheadcode;
            document.getElementById('txt_salesohcode').value = salesohcode;
            document.getElementById('txt_coustmername').value = coustmername;
            document.getElementById('txt_sapcoustmercode').value = sapcoustmercode;
            document.getElementById('txt_purchasetype').value = purchasetype;
            document.getElementById('txt_vendortype').value = vendortype;
            document.getElementById('txt_gstnnumber').value = gstnnumber;
            document.getElementById('slct_registertype').value = registertype;
            document.getElementById('slct_satatename').value = satatename;
            document.getElementById('btn_save').innerHTML = "Modify";

            var table = document.getElementById("tbl_trip_locations");
            for (var i = table.rows.length - 1; i > 0; i--) {
                table.deleteRow(i);
            }
            for (var i = 0; i < Vendor_sub_list.length; i++) {
                if (vendorsno == Vendor_sub_list[i].sno) {
                    $("#tbl_trip_locations").append('<tr><td data-title="From"><select class="form-control" name="ddltype" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option><option  value="Skim">Skim</option><option  value="Condensed">Condensed</option></select></td>' +
              '<td data-title="Rate On"><select class="form-control" name="Rateon" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="TS">TS</option><option  value="KGFAT">KGFAT</option><option  value="PerLtr">PerLtr</option><option  value="PerKg">PerKg</option></select></td>' +
                  '<td data-title="Calculation On"><select class="form-control" name="CalculationOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="Cost"><input class="form-control" type="text" placeholder="Cost" name="RateOnCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Over Head On"><select class="form-control" name="OverHeadOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="Over Head Cost"><input class="form-control" type="text" placeholder="Over Head Cost" name="OverHeadCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="STD FAT"><input class="form-control" name="MSTDFAT" type="text" placeholder="-STD FAT" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" name="PSTDFAT" value="0" type="text" placeholder="+STD FAT" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                   '<td data-title="FAT +/- On"><select class="form-control" name="FATPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="FAT - Cost"><input class="form-control" type="text" placeholder="-FAT Cost" name="MFATPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+FAT Cost" name="PFATPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="STD SNF"><input class="form-control" name="MSTDSNF" type="text" placeholder="-STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" name="PSTDSNF" type="text" placeholder="+STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="SNF +/- On"><select class="form-control" name="SNFPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="SNF - Cost"><input class="form-control" type="text" placeholder="-SNF Cost" name="MSNFPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+SNF Cost" name="PSNFPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Transport On"><select class="form-control" name="TransportOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option><option  value="Kms">Kms</option></select></td>' +
                '<td data-title="Cost"><input class="form-control" name="KmCost" type="text" placeholder="Cost" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Transport"><input class="form-control" name="Transport" type="text" placeholder="Transport" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                       '<td><i style="font-size:15px;cursor:pointer;" class="fa fa-remove fa-2x" onclick="removetopic(this);"></i></td></tr>');
                }
            }
            for (var i = 0; i < Vendor_sub_list.length; i++) {
                if (vendorsno == Vendor_sub_list[i].sno) {
                    var milktype = "";
                    $('#tbl_trip_locations> tbody > tr').each(function () {
                        if (milktype != Vendor_sub_list[i].ddltype) {
                            $(this).find('select[name*="ddltype"]').val(Vendor_sub_list[i].ddltype);
                            $(this).find('select[name*="Rateon"]').val(Vendor_sub_list[i].Rateon);
                            $(this).find('select[name*="CalculationOn"]').val(Vendor_sub_list[i].CalculationOn);
                            $(this).find('[name="RateOnCost"]').val(Vendor_sub_list[i].RateOnCost);
                            $(this).find('select[name*="OverHeadOn"]').val(Vendor_sub_list[i].OverHeadOn);
                            $(this).find('[name="OverHeadCost"]').val(Vendor_sub_list[i].OverHeadCost);
                            $(this).find('[name="MSTDFAT"]').val(Vendor_sub_list[i].MSTDFAT);
                            $(this).find('[name="PSTDFAT"]').val(Vendor_sub_list[i].PSTDFAT);
                            $(this).find('select[name*="FATPlus"]').val(Vendor_sub_list[i].FATPlus);
                            $(this).find('[name="MFATPlusCost"]').val(Vendor_sub_list[i].MFATPlusCost);
                            $(this).find('[name="PFATPlusCost"]').val(Vendor_sub_list[i].PFATPlusCost);
                            var MSTDSNF = 0;
                            $(this).find('[name="MSTDSNF"]').val(Vendor_sub_list[i].MSTDSNF);
                            $(this).find('[name="PSTDSNF"]').val(Vendor_sub_list[i].PSTDSNF);
                            $(this).find('select[name*="SNFPlus"]').val(Vendor_sub_list[i].SNFPlus);
                            $(this).find('[name="MSNFPlusCost"]').val(Vendor_sub_list[i].MSNFPlusCost);
                            $(this).find('[name="PSNFPlusCost"]').val(Vendor_sub_list[i].PSNFPlusCost);
                            $(this).find('select[name*="TransportOn"]').val(Vendor_sub_list[i].TransportOn);
                            $(this).find('[name="KmCost"]').val(Vendor_sub_list[i].Transportcost);
                            $(this).find('[name="Transport"]').val(Vendor_sub_list[i].Transport);
                            milktype = Vendor_sub_list[i].ddltype;
                        }
                    });
                }
            }
            $("#div_vendordata").hide();
            $("#fillform").show();
            $('#showlogs').hide();
        }
        var nme = 0;
        function add_location_log() {
            var rowcount = $('#tbl_trip_locations tbody tr').length;
            if (rowcount > 0) {
                var lastrow = $('#tbl_trip_locations tbody tr:last');
                var from_locof_last = $(lastrow).find('[name=From_location] :selected').text();
                var KMS = $(lastrow).find('[name=kms]').val();
                var firstrow = $('#tbl_trip_locations tbody tr:first');
                var from_locof_first_val = $(firstrow).find('[name=From_location] :selected').val();
                if ($(lastrow).find('[name=datetime_log]').val() == "") {
                    alert("Eneter Proper trip Datetime");
                    $(lastrow).find('[name=datetime_log]').focus();
                    return;
                }
                if (from_locof_last != "Location" && KMS != "") {
                    //$(lastrow).find('[name=From_location]').attr('disabled', 'disabled');
                    //$(lastrow).find('[name=To_location]').attr('disabled', 'disabled');
                    $("#tbl_trip_locations").append('<tr><td data-title="From"><select class="form-control" name="ddltype" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option><option  value="Skim">Skim</option><option  value="Condensed">Condensed</option></select></td>' +
              '<td data-title="Rate On"><select class="form-control" name="Rateon" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="TS">TS</option><option  value="KGFAT">KGFAT</option><option  value="PerLtr">PerLtr</option><option  value="PerKg">PerKg</option></select></td>' +
                  '<td data-title="Calculation On"><select class="form-control" name="CalculationOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="Cost"><input class="form-control" type="text" placeholder="Cost" name="RateOnCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Over Head On"><select class="form-control" name="OverHeadOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="Over Head Cost"><input class="form-control" type="text" placeholder="Over Head Cost" name="OverHeadCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="STD FAT"><input class="form-control" name="MSTDFAT" type="text" placeholder="-STD FAT" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" value="0" name="PSTDFAT" type="text" placeholder="+STD FAT" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                  '<td data-title="FAT +/- On"><select class="form-control" name="FATPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="FAT - Cost"><input class="form-control" type="text" placeholder="-FAT Cost" name="MFATPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+FAT Cost" name="PFATPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +

                '<td data-title="STD SNF"><input class="form-control" name="MSTDSNF" type="text" placeholder="-STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" name="PSTDSNF" type="text" placeholder="+STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="SNF +/- On"><select class="form-control" name="SNFPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="SNF - Cost"><input class="form-control" type="text" placeholder="-SNF Cost" name="MSNFPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+SNF Cost" name="PSNFPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Transport On"><select class="form-control" name="TransportOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option><option  value="Kms">Kms</option></select></td>' +
                '<td data-title="Cost"><input class="form-control" name="KmCost" type="text" placeholder="Cost" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Transport"><input class="form-control" name="Transport" type="text" placeholder="Transport" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td><i style="font-size:15px;cursor:pointer;" class="fa fa-remove fa-2x" onclick="removetopic(this);"></i></td></tr>');
                    //                    only_no_trips();
                }
                else {
                    alert("Please Select Proper Location and KMS");
                    $(lastrow).find('[name=From_location]').focus();
                }
                var lastrow_after = $('#tbl_trip_locations tbody tr:last');
                var to_locof_last_val_after = $(lastrow).find('[name=From_location] :selected').val();
            }
            else {
                $("#tbl_trip_locations").append('<tr><td data-title="From"><select class="form-control" name="ddltype" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option><option  value="Skim">Skim</option><option  value="Condensed">Condensed</option></select></td>' +
                '<td data-title="Rate On"><select class="form-control" name="Rateon" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="TS">TS</option><option  value="KGFAT">KGFAT</option><option  value="PerLtr">PerLtr</option><option  value="PerKg">PerKg</option></select></td>' +
                  '<td data-title="Calculation On"><select class="form-control" name="CalculationOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="Cost"><input class="form-control" type="text" placeholder="Cost" name="RateOnCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Over Head On"><select class="form-control" name="OverHeadOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="Over Head Cost"><input class="form-control" type="text" placeholder="Over Head Cost" name="OverHeadCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="STD FAT"><input class="form-control" name="MSTDFAT" type="text" placeholder="-STD FAT" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" value="0" name="PSTDFAT" type="text" placeholder="+STD FAT" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                   '<td data-title="FAT +/- On"><select class="form-control" name="FATPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="FAT - Cost"><input class="form-control" type="text" placeholder="-FAT Cost" name="MFATPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+FAT Cost" name="PFATPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +

                '<td data-title="STD SNF"><input class="form-control" name="MSTDSNF" type="text" value="0" placeholder="-STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" value="0" name="PSTDSNF" type="text" placeholder="+STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="SNF +/- On"><select class="form-control" name="SNFPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>' +
                '<td data-title="SNF - Cost"><input class="form-control" type="text" placeholder="-SNF Cost" name="MSNFPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+SNF Cost" name="PSNFPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Transport On"><select class="form-control" name="TransportOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option><option  value="Kms">Kms</option></select></td>' +
                '<td data-title="Cost"><input class="form-control" name="KmCost" type="text" placeholder="Cost" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td data-title="Transport"><input class="form-control" name="Transport" type="text" placeholder="Transport" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>' +
                '<td><i style="font-size:15px;cursor:pointer;" class="fa fa-remove fa-2x" onclick="removetopic(this);"></i></td></tr>');
                nme++;
                //                only_no_trips();
            }
        }
        function removetopic(thisid) {
            $(thisid).closest('tr').remove();
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
        function get_state_details() {
            var data = { 'op': 'get_state_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillstatedetails(msg);
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
        function fillstatedetails(msg) {
            var data = document.getElementById('slct_satatename');
            var length = data.options.length;
            document.getElementById('slct_satatename').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select State Name";
            opt.value = "Select State Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].sno != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].statename;
                    option.value = msg[i].sno;
                    data.appendChild(option);
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Vendor Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vendor Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vendor Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                    <%--<input id="btn_addvendor" type="button" name="submit" value='Add Vendor' class="btn btn-success" />--%>
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addvendordetails()"></span> <span onclick="addvendordetails()">Add Vendor</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_vendordata">
                </div>
                <div id='fillform' style="display: none;">
                    <div>
                        <div  style="text-align: -webkit-center;padding-bottom: 5%;">
                        <table cellpadding="1px" align="center;" style="width:50%">
                            <tr>
                                <th colspan="2" align="center">
                                </th>
                            </tr>
                            <tr>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Code</label> <span style="color: red;">*</span>
                                
                                    <input type="text" maxlength="45" id="txt_vendorcode" class="form-control" name="vendorcode"
                                        placeholder="Enter Vendor Code"><label id="lbl_vencode_error_msg" class="errormessage">*
                                            Please Enter Vendor Code</label>
                                </td>
                            
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Name</label> <span style="color: red;">*</span>
                                
                                    <input type="text" maxlength="55" id="txt_vendorname" class="form-control" name="vendorcode"
                                        placeholder="Enter Vendor Name"><label id="lbl_vennme_error_msg" class="errormessage">*
                                            Please Enter Vendor Name</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Type</label> <span style="color: red;">*</span>
                                
                                    <select id="cmb_vendortype" class="form-control">
                                        <option value="Both">Both</option>
                                        <option value="Vendor">Vendor</option>
                                        <option value="Client">Client</option>
                                    </select>
                                </td>
                           
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Branch Type</label> <span style="color: red;">*</span>
                                
                                    <select id="ddlbranchtype" class="form-control">
                                        <option>Inter Branch</option>
                                        <option>Other Branch</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Address</label><span style="color: red;">*</span>
                               
                                    <input type="text" name="vendorcode" id="txt_address" class="form-control" placeholder="Enter Address">
                                </td>
                            
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Email</label>
                               
                                    <input type="text" name="vendorcode" id="txt_email" class="form-control"  onchange="validate_email();"
                                        placeholder="Enter Email ID"><label id="lbl_email_error_msg" class="errormessage">*
                                            Please Enter Proper Email ID</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Phone Number</label> <span style="color: red;">*</span>
                                
                                    <input type="text" name="vendorcode" id="txt_phoneno" onkeypress="return isNumber(event);" class="form-control" maxlength="12" onchange="ValidateNos();"
                                        placeholder="Enter Phone Number">
                                </td>
                           
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Distance(Kms)</label><span style="color: red;">*</span>
                               
                                    <input type="text" name="vendorcode" id="txt_kms" class="form-control" maxlength="10"
                                        placeholder="Enter Distance(Kms)">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Expected Time(In Mins)</label><span style="color: red;">*</span>
                                
                                    <input type="text" name="vendorcode" id="txt_time" class="form-control"  onkeypress="return isFloat(event);"   maxlength="10"
                                        placeholder="Enter Expected Time(In Mins)">
                                </td>
                            
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Tally Ledger Type(purchase)</label><span style="color: red;">*</span>
                                
                                    <input type="text" name="vendorcode" id="txt_ledgertype" class="form-control" 
                                        placeholder="Enter Ledger Type">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Tally Over Head</label><span style="color: red;">*</span>
                                
                                    <input type="text" name="vendorcode" id="txtoh" class="form-control" 
                                        placeholder="Enter Over Head">
                                </td>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Ledger Code(Purchase)</label><span style="color: red;">*</span>
                               
                                    <input type="text" name="vendorcode" id="txtsapcode" class="form-control" 
                                        placeholder="Enter Ledger Code(Purchase)">
                                </td>
                            </tr>
                            <tr>
                            <td style="height: 10%;padding-left: 5%;">
                                    <label>Tally Ledger Type(Sales)</label><span style="color: red;">*</span>
                               
                                    <input type="text" name="vendorcode" id="txtsalesledgertype" class="form-control" 
                                        placeholder="Enter Ledger Type">
                                </td>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Tally Over Head(Sales)</label><span style="color: red;">*</span>
                               
                                    <input type="text" name="vendorcode" id="txt_tallyoverheadsales" class="form-control" 
                                        placeholder="Enter Tally Over Head Sales">
                                </td>
                            </tr>
                            <tr>
                            <td style="height: 10%;padding-left: 5%;">
                                    <label>Ledger Code(Sales)</label><span style="color: red;">*</span>
                              
                                    <input type="text" name="vendorcode" id="txt_ledgercodesales" class="form-control" 
                                        placeholder="Enter Ledger Code Sales">
                                </td>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label class="txtspan">SAP Vendor Code</label><span style="color: red;">*</span>
                                
                                    <input type="text" name="vendorcode" id="txt_sapvendorcode" class="form-control" 
                                        placeholder="Enter Sap Vendor Code">
                                </td>
                            </tr>
                             <tr>
                            <td style="height: 10%;padding-left: 5%;">
                                    <label>Purchase Over Head Code</label><span style="color: red;">*</span>
                                
                                    <input type="text" name="vendorcode" id="txt_purchaseoverheadcode" class="form-control" 
                                        placeholder="Enter Purchase Over Head Code">
                                </td>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>Sales Over Head Code</label><span style="color: red;">*</span>
                               
                                    <input type="text" name="vendorcode" id="txt_salesohcode" class="form-control" 
                                        placeholder="Enter Sales Over Head Code">
                                </td>
                            </tr>
                             <tr>
                            <td style="height: 10%;padding-left: 5%;">
                                    <label>Customer Name</label><span style="color: red;">*</span>
                                
                                    <input type="text" name="vendorcode" id="txt_coustmername" class="form-control" 
                                        placeholder="Enter Customer Name">
                                </td>
                                <td style="height: 10%;padding-left: 5%;">
                                    <label>SAP Customer Code</label><span style="color: red;">*</span>

                                    <input type="text" name="vendorcode" id="txt_sapcoustmercode" class="form-control" 
                                        placeholder="Enter Sap Customer Code">
                                </td>
                            </tr>
                             <tr>
                           <td style="height: 10%;padding-left: 5%;">
                                    <label>Purchase Type</label><span style="color: red;">*</span>
                                
                                    <input type="text" name="vendorcode" id="txt_purchasetype" class="form-control" 
                                        placeholder="Enter Purchase Type">
                                </td>
                                 <td style="height: 10%;padding-left: 5%;">
                                    <label>Type</label> <span style="color: red;">*</span>
                             
                                    <select id="txt_vendortype" class="form-control">
                                        <option>own</option>
                                        <option>other</option>
                                    </select>
                                </td>
                            </tr>
                             <tr>
                             <td style="height: 10%;padding-left: 5%;">
                                    <label>GSTIN Number</label> <span style="color: red;">*</span>
                                 <input type="text" name="vendorcode" id="txt_gstnnumber" class="form-control" 
                                        placeholder="Enter GSTIN Number">
                                </td>
                            
                             <td style="height: 10%;padding-left: 5%;">
                                    <label>Registration Type</label><span style="color: red;">*</span>
                                    <select   id="slct_registertype" class="form-control" >
                                        <option selected value disabled value="">-Select Registration Type-</option>
                                        <option value="Casual Taxable Person">Casual Taxable Person</option>
                                        <option value="Composition Levy">Composition Levy</option>
                                        <option value="Government Department or PSU">Government Department or PSU</option>
                                        <option value="Non Resident Taxable Person">Non Resident Taxable Person</option>
                                        <option value="Regular/TDS/ISD">Regular/TDS/ISD</option>
                                        <option value="UN Agency or Embassy">UN Agency or Embassy</option>
                                    </select>
                                </td>
                                </tr>
                            <tr>
                                 <td style="height: 10%;padding-left: 5%;">
                                    <label>State Name</label> <span style="color: red;">*</span>
                                 <select   id="slct_satatename" class="form-control">
                                 <option selected value disabled value="">Select State Name</option>
                                  </select>
                                </td>
                            </tr>
                        </table>
                        </div>
                        <div class="box box-danger">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vendor TS Rate Details</h3>
                            </div>
                            <table id="tbl_trip_locations" class="table table-bordered table-hover dataTable no-footer"
                                role="grid" aria-describedby="example2_info">
                                <thead>
                                    <tr>
                                        <th scope="col">
                                            Milk Type
                                        </th>
                                        <th scope="col">
                                            Rate On
                                        </th>
                                        <th scope="col">
                                            Calculation On
                                        </th>
                                        <th scope="col">
                                            Cost
                                        </th>
                                        <th scope="col">
                                            Over Head On
                                        </th>
                                        <th scope="col">
                                            Over Head Cost
                                        </th>
                                        <th scope="col">
                                            STD FAT
                                        </th>
                                         <th scope="col">
                                            FAT +/- On
                                        </th>
                                        <th scope="col">
                                            FAT +/- Cost
                                        </th>
                                         <th scope="col">
                                            STD SNF
                                        </th>
                                        <th scope="col">
                                            SNF +/- On
                                        </th>
                                        <th scope="col">
                                            SNF +/- Cost
                                        </th>
                                        <th scope="col">
                                            Transport On
                                        </th>
                                        <th scope="col">
                                            Cost
                                        </th>
                                        <th scope="col">
                                            Transport
                                        </th>
                                        <th scope="col">
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                      <%--  <input id="btn_addlocation" type="button" class="btn btn-default" value="Add new row"
                                            onclick="add_location_log()" />--%>
                                        <div class="input-group">
                                            <div class="input-group-addon">
                                            <span  class="glyphicon glyphicon-plus-sign" onclick="add_location_log()"></span> <span onclick="add_location_log()">Add New Row</span>
                                        </div>
                                        </div>
                                        </td>
                                </tr>
                            </table>
                            </div>
                            <div  style="padding-left: 35%;padding-top: 5%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="for_save_edit_vendor_click()"></span><span id="btn_save" onclick="for_save_edit_vendor_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="closevendors()"></span><span id='btn_close' onclick="closevendors()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                            <div style="padding-left:275px;">
                            <table align="center">
                                <tr hidden>
                                    <td>
                                        <label id="lbl_sno">
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                       <%--     <input type="button" class="btn btn-success" name="submit" class="btn btn-primary"
                                                id="btn_save" value='Save' onclick="for_save_edit_vendor_click()" />
                                            <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close' />--%>
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
