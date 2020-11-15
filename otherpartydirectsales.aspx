<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="otherpartydirectsales.aspx.cs" Inherits="otherpartydirectsales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(function () {
            $('#lidop').click(function () {
                $('#divpurchase').css('display', 'block');
                $('#divsales').css('display', 'none');
                Getmilkdetails();
                get_Vendor_details();
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
            $('#lidos').click(function () {
                $('#divpurchase').css('display', 'none');
                $('#divsales').css('display', 'block');
                Getsalesmilkdetails();
                get_Vendor_details();
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
            Getmilkdetails();
            get_Vendor_details();
        });

        //Function for only no
        $(document).ready(function () {
            $("#txt_dcno,#txt_Inwardno").keydown(function (event) {
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
        function get_Vendor_details() {
            var data = { 'op': 'get_Vendor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var Vendor = msg[0].vendordetails;
                        fillSource(Vendor);
                        fillvendor(Vendor);
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
            var data = document.getElementById('slct_Source_Name');
            var length = data.options.length;
            document.getElementById('slct_Source_Name').options.length = null;
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
        function fillvendor(msg) {
            var data = document.getElementById('slct_vender_Name');
            var length = data.options.length;
            document.getElementById('slct_vender_Name').options.length = null;
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
        function save_direct_purchase_click() {
            var dcno = document.getElementById('txt_dcno').value;
            var Inwardno = document.getElementById('txt_Inwardno').value;
            var vehicleNo = document.getElementById('txt_vehicleNo').value;
            var inwarddate = document.getElementById('txt_date').value;
            var fatcalon = "";
            var qco = document.getElementById('txt_qco').value;
            var Remarks = document.getElementById('txt_Remarks').value;
            var Chemist = document.getElementById('txt_Chemist').value;
            var SourceID = document.getElementById('slct_Source_Name').value;
            var btnvalue = document.getElementById('save_milktransactions').innerHTML;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var party_dcno = document.getElementById('txt_party_dcno').value;
            var transportvalue = document.getElementById('txt_transportvalue').value;
            if (inwarddate == "") {
                alert("Please Select Date");
                $("#txt_date").focus();
                return false;
            }
            if (dcno == "") {
                alert("Enter dc no");
                $("#txt_dcno").focus();
                return false;
            }
            if (Inwardno == "") {
                alert("Enter inward no");
                $("#txt_Inwardno").focus();
                return false;
            }
            if (vehicleNo == "") {
                alert("Enter vehicle no");
                $("#txt_vehicleNo").focus();
                return false;
            }
            if (SourceID == "" || SourceID == "Select Vendor Name") {
                alert("Select Vendor name");
                $("#slct_Source_Name").focus();
                return false;
            }
            if (Chemist == "") {
                alert("Enter Chemist name");
                $("#txt_Chemist").focus();
                return false;
            }
            if (qco == "") {
                alert("Enter qco name");
                $("#txt_qco").focus();
                return false;
            }
            var MilkfatDetailsMilkarray = [];
            $('#tbl_milk_details> tbody > tr').each(function () {
                var milktype = $(this).find('select[name*="milktype"] :selected').val();
                var CellName = $(this).find('[name="CellName"]').val();
                var Qtykg = $(this).find('[name="Qtykg"]').val();
                var Qtyltr = $(this).find('[name="Qtyltr"]').val();
                var fat = $(this).find('[name="FAT"]').val();
                var snf = $(this).find('[name="SNF"]').val();
                var temp = $(this).find('[name="TEMP"]').val();
                var acidity = $(this).find('[name="Acidity"]').val();
                var clr = $(this).find('[name="CLR"]').val();
                var cob = $(this).find('select[name*="COB"] :selected').val(); // $(this).find('[name="COB"]').val();
                var ot = $(this).find('[name="OT"]').val();
                var hs = $(this).find('[name="HS"]').val();
                var phosps = $(this).find('select[name*="Phosps"] :selected').val(); // $(this).find('[name="Phosps"]').val();
                var alcohol = $(this).find('[name="Alcohol"]').val();
                var neutralizers = $(this).find('[name="Neutralizers"]').val();
                var MBRT = $(this).find('[name="MBRT"]').val();
                if (Qtykg == "") {
                }
                else {
                    MilkfatDetailsMilkarray.push({ 'milktype': milktype, 'CellName': CellName, 'Qtykg': Qtykg, 'Qtyltr': Qtyltr, 'fat': fat, 'snf': snf, 'temp': temp,
                        'acidity': acidity, 'clr': clr, 'cob': cob, 'ot': ot, 'hs': hs, 'phosps': phosps, 'alcohol': alcohol, 'neutralizers': neutralizers, 'mbrt': MBRT
                    });
                }
            });
            if (MilkfatDetailsMilkarray.length == "0") {
                alert("Please enter fat,snf details");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_direct_purchase_click', 'partydcno': party_dcno, 'dcno': dcno, 'inwardno': Inwardno, 'vehicleno': vehicleNo, 'dispdate': inwarddate,
                    'fatcalon': fatcalon, 'qco': qco, 'remarks': Remarks, 'chemist': Chemist, 'sourceid': SourceID, 'sno': sno, 'btnvalue': btnvalue,
                    'MilkfatDetailsMilkarray': MilkfatDetailsMilkarray, 'transportvalue': transportvalue
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        purchaseclearvalues();
                        Getmilkdetails();
                        get_Vendor_details();
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
        function purchaseclearvalues() {
            document.getElementById('txt_dcno').value = "";
            document.getElementById('txt_Inwardno').value = "";
            document.getElementById('txt_vehicleNo').value = "";
            document.getElementById('txt_date').value = "";
            document.getElementById('txt_qco').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('txt_Chemist').value = "";
            document.getElementById('txt_transportvalue').value = "";
            document.getElementById('slct_Source_Name').selectedIndex = 0;
            Getmilkdetails();
        }

        function save_direct_sales_click() {
            var sdate = document.getElementById('txt_sdate').value;
            var sparty_transno = document.getElementById('txt_stransno').value;
            var sInwardno = document.getElementById('txt_sinwardno').value;
            var sparty_dcno = document.getElementById('txt_sdcno').value;
            var svehicleNo = document.getElementById('txt_svehicleno').value;
            var sqco = document.getElementById('txt_sqco').value;
            var sRemarks = document.getElementById('txt_sremarks').value;
            var sChemist = document.getElementById('txt_schemist').value;
            var sSourceID = document.getElementById('slct_vender_Name').value;
            var btnvalue = document.getElementById('btn_save').innerHTML;
            var transportvalue = document.getElementById('txt_stransportvalue').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
            if (sdate == "") {
                alert("Please Select Date");
                $("#txt_sdate").focus();
                return false;
            }
            if (sparty_transno == "") {
                alert("Enter Party Transaction No");
                $("#txt_stransno").focus();
                return false;
            }
            if (sparty_dcno == "") {
                alert("Enter dc no");
                $("#txt_sdcno").focus();
                return false;
            }
            if (sInwardno == "") {
                alert("Enter inward no");
                $("#txt_sinwardno").focus();
                return false;
            }
            if (svehicleNo == "") {
                alert("Enter vehicle no");
                $("#txt_svehicleno").focus();
                return false;
            }
            if (sSourceID == "" || sSourceID == "Select Vendor Name") {
                alert("Select plant name");
                $("#slct_vender_Name").focus();
                return false;
            }
            if (sChemist == "") {
                alert("Enter Chemist name");
                $("#txt_schemist").focus();
                return false;
            }
            if (sqco == "") {
                alert("Enter qco name");
                $("#txt_sqco").focus();
                return false;
            }
            var MilksaleDetailsMilkarray = [];
            $('#tbl_milk_salesdetails> tbody > tr').each(function () {
                var milktype = $(this).find('select[name*="smilktype"] :selected').val();
                var CellName = $(this).find('[name="sCellName"]').val();
                var Qtykg = $(this).find('[name="sQtykg"]').val();
                var Qtyltr = $(this).find('[name="sQtyltr"]').val();
                var fat = $(this).find('[name="sFAT"]').val();
                var snf = $(this).find('[name="sSNF"]').val();
                var temp = $(this).find('[name="sTEMP"]').val();
                var acidity = $(this).find('[name="sAcidity"]').val();
                var clr = $(this).find('[name="sCLR"]').val();
                var cob = $(this).find('select[name*="sCOB"] :selected').val(); // $(this).find('[name="COB"]').val();
                var ot = $(this).find('[name="sOT"]').val();
                var hs = $(this).find('[name="sHS"]').val();
                var phosps = $(this).find('select[name*="sPhosps"] :selected').val(); // $(this).find('[name="Phosps"]').val();
                var alcohol = $(this).find('[name="sAlcohol"]').val();
                var neutralizers = $(this).find('[name="sNeutralizers"]').val();
                var MBRT = $(this).find('[name="sMBRT"]').val();
                if (Qtykg == "") {
                }
                else {
                    MilksaleDetailsMilkarray.push({ 'milktype': milktype, 'CellName': CellName, 'Qtykg': Qtykg, 'Qtyltr': Qtyltr, 'fat': fat, 'snf': snf, 'temp': temp,
                        'acidity': acidity, 'clr': clr, 'cob': cob, 'ot': ot, 'hs': hs, 'phosps': phosps, 'alcohol': alcohol, 'neutralizers': neutralizers, 'mbrt': MBRT
                    });
                }
            });
            if (MilksaleDetailsMilkarray.length == "0") {
                alert("Please enter fat,snf details");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_direct_sales_click', 'partydcno': sparty_dcno, 'partytransno': sparty_transno, 'inwardno': sInwardno, 'vehicleno': svehicleNo, 'dispdate': sdate,
                    'qco': sqco, 'remarks': sRemarks, 'chemist': sChemist, 'sourceid': sSourceID, 'sno': sno, 'btnvalue': btnvalue,
                    'MilksaleDetailsMilkarray': MilksaleDetailsMilkarray, 'transportvalue': transportvalue
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        salesclearvalues();
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
        function salesclearvalues() {
            document.getElementById('txt_sdate').value = "";
            document.getElementById('txt_stransno').value = "";
            document.getElementById('txt_sinwardno').value = "";
            document.getElementById('txt_sdcno').value = "";
            document.getElementById('txt_sqco').value = "";
            document.getElementById('txt_sremarks').value = "";
            document.getElementById('txt_schemist').value = "";
            document.getElementById('txt_stransportvalue').value = "";
            document.getElementById('slct_vender_Name').selectedIndex = 0;
            Getsalesmilkdetails();
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
        function LRChange(qtyid) {
            if (qtyid.value == "") {
                var qty = 0;
                var qtykg = 0;
                qtykg = $(qtyid).closest("tr").find('#txtkg').val();
                qtykg = parseFloat(qtykg).toFixed(3);
                var qtykgsltr = 0;
                var clr = 0;
                clr = parseFloat(qty).toFixed(3);
                var modclr = (clr / 1000) + 1;
                modclr = parseFloat(modclr).toFixed(3);
                qtykgsltr = qtykg / modclr;
                qtykgsltr = parseFloat(qtykgsltr).toFixed(2);
                $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);
            }
            else {
                var qtykg = 0;
                qtykg = $(qtyid).closest("tr").find('#txtkg').val();
                qtykg = parseFloat(qtykg).toFixed(3);
                var qtykgsltr = 0;
                var clr = 0;
                clr = parseFloat(qtyid.value).toFixed(3);
                var modclr = (clr / 1000) + 1;
                modclr = parseFloat(modclr).toFixed(5);
                qtykgsltr = qtykg / modclr;
                qtykgsltr = parseFloat(qtykgsltr).toFixed(0);
                $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);



                ////////For SNF ////////////////

                var fat = 0;
                fat = $(qtyid).closest("tr").find('#txtfat').val();
                fat = parseFloat(fat).toFixed(3);
                var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
                $(qtyid).closest("tr").find('#txtsnf').val(snfvalue);

            }
        }
        function Getmilkdetails() {
            var cellnames = "F Cell,M Cell,B Cell";
            var names = cellnames.split(',');
            var results = '<div    style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">MilkType</th><th scope="col">Cell Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th><th scope="col">TEMP</th><th scope="col">Acidity</th><th scope="col">COB</th><th scope="col">OT</th><th scope="col">HS</th><th scope="col">Phosps</th><th scope="col">Alcohol%</th><th scope="col">MBRT</th><th scope="col">Neutralizers</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < names.length; i++) {
                results += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results += '<th data-title="From"><select class="form-control" name="milktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + names[i] + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="Qty(kgs)" name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" disabled="disabled" placeholder="Qty(Ltrs)" id="txtltr" name="Qtyltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="FAT" name="FAT" id="txtfat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="SNF" name="SNF" value="" id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="CLR"  onkeyup="LRChange(this);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Temp" style="text-align:center;" class="5"><input class="form-control"onkeypress="return isFloat(event);" type="text" placeholder="TEMP" name="TEMP" value="" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Acidity" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Acidity" name="Acidity"  id="txtacidity"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="COB" style="text-align:center;width:65px;" class="5"><select class="form-control" name="COB" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                results += '<td data-title="OT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="OT" name="OT" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="HS" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="HS" onkeypress="return isFloat(event);" name="HS" id="txths"   value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Phosps" style="text-align:center;width:65px;" class="5"><select class="form-control" name="Phosps" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                results += '<td data-title="Alcohol" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Alcohol%" id="txtalcohol" onkeypress="return isFloat(event);" name="Alcohol" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="MBRT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="MBRT" name="MBRT" id="txtmbrt" onkeypress="return isFloat(event);" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Neutralizers" style="text-align:center;" class="5"><select class="form-control" name="Neutralizers" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td></tr>';
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
        }
        function Getsalesmilkdetails() {
            var cellnames = "F Cell,M Cell,B Cell";
            var names = cellnames.split(',');
            var results = '<div    style="overflow:auto;"><table id="tbl_milk_salesdetails" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">MilkType</th><th scope="col">Cell Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th><th scope="col">TEMP</th><th scope="col">Acidity</th><th scope="col">COB</th><th scope="col">OT</th><th scope="col">HS</th><th scope="col">Phosps</th><th scope="col">Alcohol%</th><th scope="col">MBRT</th><th scope="col">Neutralizers</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < names.length; i++) {
                results += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results += '<th data-title="From"><select class="form-control" name="smilktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="sCellName" value="' + names[i] + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty(kgs)" onkeypress="return isFloat(event);" name="sQtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" disabled="disabled" onkeypress="return isFloat(event);" placeholder="Qty(Ltrs)" id="txtltr" name="sQtyltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="FAT" onkeypress="return isFloat(event);" name="sFAT" id="txtfat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF" onkeypress="return isFloat(event);" name="sSNF" value="" id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="CLR"  onkeypress="return isFloat(event);" onkeyup="LRChange(this);" id="txtclr" name="sCLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Temp" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="TEMP"  onkeypress="return isFloat(event);" name="sTEMP" value="" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Acidity" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Acidity"  name="sAcidity"  id="txtacidity"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="COB" style="text-align:center;width:65px;" class="5"><select class="form-control" name="sCOB" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                results += '<td data-title="OT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="OT" name="sOT" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="HS" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="HS" name="sHS" id="txths" onkeypress="return isFloat(event);"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Phosps" style="text-align:center;width:65px;" class="5"><select class="form-control" name="sPhosps" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                results += '<td data-title="Alcohol" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Alcohol%" id="txtalcohol" onkeypress="return isFloat(event);" name="sAlcohol" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="MBRT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="MBRT" name="sMBRT" id="txtmbrt" onkeypress="return isFloat(event);" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Neutralizers" style="text-align:center;" class="5"><select class="form-control" name="sNeutralizers" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td></tr>';
            }
            results += '</table></div>';
            $("#divsalesdata").html(results);
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

        function get_inward_details() {
            var data = { 'op': 'get_inward_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillinwordmilkdetails(msg);
                        get_Vendor_details();

                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var transactiondetails = [];
        function fillinwordmilkdetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Date</th></th><th scope="col">DCNo</th><th scope="col">InwardNo</th><th scope="col">VehicleNo</th><th scope="col">Vendor Name</th><th scope="col">Chemist</th><th scope="col">QCO</th><th scope="col">Remarks</th></tr></thead></tbody>';
            var transactionsubdetails = msg[0].transactionsubdetails;
            transactiondetails = msg[0].transactiondetails;
            for (var i = 0; i < transactionsubdetails.length; i++) {
                results += '<tr><td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-success" value="Edit" /></td>';
                results += '<td class="11">' + transactionsubdetails[i].doe + '</td>';
                results += '<th scope="row" class="1" style="text-align:center;">' + transactionsubdetails[i].dcno + '</th>';
                results += '<td data-title="Capacity" class="2">' + transactionsubdetails[i].inwardno + '</td>';
                results += '<td data-title="Capacity" class="3">' + transactionsubdetails[i].vehicleno + '</td>';
                results += '<td data-title="Capacity" class="3">' + transactionsubdetails[i].vendorname + '</td>';
                results += '<td data-title="Capacity" class="10">' + transactionsubdetails[i].chemist + '</td>';
                results += '<td data-title="Capacity" class="8">' + transactionsubdetails[i].qco + '</td>';
                results += '<td data-title="Capacity" class="9">' + transactionsubdetails[i].remarks + '</td>';
                results += '<td style="display:none" class="4">' + transactionsubdetails[i].sectionid + '</td>';
                results += '<td  style="display:none" class="13">' + transactionsubdetails[i].partydcno + '</td>';
                results += '<td  style="display:none" class="5">' + transactionsubdetails[i].transportcost + '</td>';
                results += '<td  style="display:none" class="6">' + transactionsubdetails[i].transport + '</td>';
                results += '<td style="display:none" class="7">' + transactionsubdetails[i].rateon + '</td>';
                results += '<td style="display:none" class="12">' + transactionsubdetails[i].milktransactonno + '</td>';
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }

        function getme(thisid) {
            var dcno = $(thisid).parent().parent().children('.1').html();
            var sno = $(thisid).parent().parent().children('.12').html();
            var inwardno = $(thisid).parent().parent().children('.2').html();
            var vehicalno = $(thisid).parent().parent().children('.3').html();
            var sectionid = $(thisid).parent().parent().children('.4').html();
            var chemist = $(thisid).parent().parent().children('.10').html();
            var Remarks = $(thisid).parent().parent().children('.9').html();
            var Qco = $(thisid).parent().parent().children('.8').html();
            var date = $(thisid).parent().parent().children('.11').html();
            var partydcno = $(thisid).parent().parent().children('.13').html();
            document.getElementById('txt_dcno').value = dcno;
            document.getElementById('txt_Inwardno').value = inwardno;
            document.getElementById('txt_vehicleNo').value = vehicalno;
            document.getElementById('txt_date').value = date;
            document.getElementById('txt_qco').value = Qco;
            document.getElementById('txt_Remarks').value = Remarks;
            document.getElementById('txt_Chemist').value = chemist;
            document.getElementById('txt_party_dcno').value = partydcno;
            document.getElementById('slct_Source_Name').value = sectionid;
            document.getElementById('lbl_sno').innerHTML = sno;
            document.getElementById('save_milktransactions').innerHTML = "Modify";

            var results = '<div    style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">MilkType</th><th scope="col">Cell Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th><th scope="col">TEMP</th><th scope="col">Acidity</th><th scope="col">COB</th><th scope="col">OT</th><th scope="col">HS</th><th scope="col">Phosps</th><th scope="col">Alcohol%</th><th scope="col">MBRT</th><th scope="col">Neutralizers</th><th scope="col">SealNo</th></tr></thead></tbody>';

            for (var j = 0; j < transactiondetails.length; j++) {
                if (sno == transactiondetails[j].sno) {
                    results += '<td data-title="Sno" class="2">' + j + '</td>';
                    results += '<th data-title="From"><select class="form-control" name="milktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                    results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + transactiondetails[j].CellName + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                    results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty(kgs)" onkeypress="return isFloat(event);" name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Qty" style="text-align:center;"  class="3"><input class="form-control" type="text" disabled="disabled" placeholder="Qty(Ltrs)" onkeypress="return isFloat(event);" name="Qtyltr" id="txtltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="FAT" name="FAT" value="" onkeypress="return isFloat(event);" id="txtfat" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF" name="SNF" value="" onkeypress="return isFloat(event);" id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="CLR" onkeyup="LRChange(this);" onkeypress="return isFloat(event);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Temp" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="TEMP"  name="TEMP" value="" onkeypress="return isFloat(event);" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Acidity" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Acidity" name="Acidity" id="txtacidity" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="COB" style="text-align:center;width:65px;" class="5"><select class="form-control" name="COB" style="width:100%;  font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                    results += '<td data-title="OT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="OT" name="OT"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="HS" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="HS" name="HS" onkeypress="return isFloat(event);" value="" id="txths" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Phosps" style="text-align:center;width:65px;" class="5"><select class="form-control" name="Phosps" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                    results += '<td data-title="Alcohol" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Alcohol%" name="Alcohol" onkeypress="return isFloat(event);" id="txtalcohol" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="MBRT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="MBRT" name="MBRT" value="" onkeypress="return isFloat(event);" id="txtmbrt" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Neutralizers" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Neutralizers" name="Neutralizers" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Sealno" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Sealno" name="Sealno" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
                }
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
            for (var i = 0; i < transactiondetails.length; i++) {
                if (sno == transactiondetails[i].sno) {
                    $('#tbl_milk_details> tbody > tr').each(function () {
                        var cell = $(this).find('[name="CellName"]').val();
                        if (cell == transactiondetails[i].CellName) {
                            $(this).find('select[name*="milktype"] :selected').val(transactiondetails[i].milktype);
                            $(this).find('[name="CellName"]').val(transactiondetails[i].CellName);
                            $(this).find('[name="Qtykg"]').val(transactiondetails[i].qtykgs);
                            $(this).find('[name="Qtyltr"]').val(transactiondetails[i].qtyltr);
                            $(this).find('[name="FAT"]').val(transactiondetails[i].fat);
                            $(this).find('[name="SNF"]').val(transactiondetails[i].snf);
                            $(this).find('[name="TEMP"]').val(transactiondetails[i].temp);
                            $(this).find('[name="Acidity"]').val(transactiondetails[i].acidity);
                            $(this).find('[name="CLR"]').val(transactiondetails[i].clr);
                            $(this).find('select[name*="COB"] :selected').val(transactiondetails[i].cob1);
                            $(this).find('[name="OT"]').val(transactiondetails[i].ot);
                            $(this).find('[name="HS"]').val(transactiondetails[i].hs);
                            $(this).find('select[name*="Phosps"] :selected').val(transactiondetails[i].phosps1);
                            $(this).find('[name="Alcohol"]').val(transactiondetails[i].alcohol);
                            $(this).find('[name="Neutralizers"]').val(transactiondetails[i].neutralizers);
                            $(this).find('[name="MBRT"]').val(transactiondetails[i].mbrt);
                            $(this).find('[name="Sealno"]').val(transactiondetails[i].sealno);
                        }
                    });
                }
            }
            $("#div_Deptdata").hide();
            $("#vehmaster_fillform").show();
            $('#showlogs').hide();
        }
        function Onchage_Source_Name() {
            var dcno = document.getElementById('txt_dcno').value;
            if (dcno == "") {
                document.getElementById('slct_Source_Name').selectedIndex = 0;
                alert("Please enter Party Trans No");
                return false;
            }
            var data = { 'op': 'get_Vehicleno_details', 'dcno': dcno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        document.getElementById('txt_vehicleNo').value = msg;
                    }
                }
                else {
                    document.getElementById('txt_vehicleNo').value = "";
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Onchage_vender_Name() {
            var dcno = document.getElementById('txt_sdcno').value;
            if (dcno == "") {
                document.getElementById('slct_vender_Name').selectedIndex = 0;
                alert("Please enter party trans no");
                return false;
            }
            var data = { 'op': 'get_Vehicleno_details', 'dcno': dcno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        document.getElementById('txt_svehicleno').value = msg;
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Others Direct Sales  <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Others Direct Sales </a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
        <div class="navigation">
                <ul style="padding-left:350px;">
                    <li id="lidop"><a href="#" class="active"><span id="spandp">Direct Sales Purchase</span></a></li>
                    <li>&nbsp</li>
                    <li id="lidos"><a href="#"><span id="spands">Direct Sales Sales</span></a></li>
                </ul>
                </div>
            <div class="box-header with-border">
               
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Others Direct Sales Entry
                </h3>

            </div>
            <div class="box-body" id="divpurchase">
                <div id='vehmaster_fillform'>
                    <div style="padding-left: 100px;">
                        <table align="center">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px;">
                                    <label>
                                        DC Datetime</label>
                                    <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                        placeholder="Enter Date">
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td>
                                    <label>
                                        Party Trans No</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_dcno" type="text" class="form-control" name="vendorcode" placeholder="DC No">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Party DC No</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_party_dcno" type="text" class="form-control" name="vendorcode" placeholder="Party DC No">
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td style="height: 40px;">
                                    <label>
                                        Inward No</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_Inwardno" type="text" class="form-control" name="vendorcode" placeholder="Inward No">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Vendor Name</label>
                                    <select id="slct_Source_Name" class="form-control" onchange="Onchage_Source_Name();">
                                        <option selected disabled value="Select Vehicle No">Select Vendor Name</option>
                                    </select>
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td style="height: 40px;">
                                    <label>
                                        Vehicle No</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_vehicleNo" type="text" class="form-control" name="vendorcode" placeholder="Vehicle No">
                                </td>
                                <%-- <td>
                        Type Of Milk
                    </td>
                    <td>
                        <select id="slct_milk_type" class="form-control">
                            <option selected disabled value="Select Type Of Milk">Select Type Of Milk</option>
                            <option>Buffalo</option>
                            <option>Cow</option>
                        </select>
                    </td>--%>
                            <td style="width: 6px;">
                                </td>
                                <td style="height: 40px;">
                                    <label>
                                        Transport Value</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_transportvalue" type="text" class="form-control" name="transportvalue" placeholder="transportvalue">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </table>
                    </div>
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk Details For Purchase</h3>
                        </div>
                        <div id="div_vendordata">
                        </div>
                    </div>
                    <div style="text-align: center;">
                        <div style="padding-left: 200px;">
                            <table align="center">
                                <tr>
                                    <td style="height: 40px;">
                                        <label>
                                            Remarks</label>
                                        <textarea rows="3" cols="45" id="txt_Remarks" class="form-control" maxlength="200"
                                            placeholder="Enter Remarks"></textarea>
                                        <%--   <textarea id="txt_Remarks" class="form-control" rows="5" cols="45" placeholder="Remarks" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;">
                                        <label>
                                            Chemist</label>
                                        <input id="txt_Chemist" type="text" class="form-control" name="vendorcode" placeholder="Chemist" />
                                    </td>
                                    <td style="width: 6px;">
                                    </td>
                                    <td>
                                        <label>
                                            QCO</label>
                                        <input id="txt_qco" type="text" class="form-control" name="vendorcode" placeholder="QCO" />
                                    </td>
                                </tr>
                                <tr hidden>
                                    <td>
                                        <label id="lbl_sno">
                                        </label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <%--<input id='save_milktransactions' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_direct_purchase_click()" />
                        <input id='close_vehmaster' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="purchaseclearvalues()" />
                        <input id='btnPrint' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />--%>
                        <div  style="padding-left: 40%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_milktransactions1" onclick="save_direct_purchase_click()"></span><span id="save_milktransactions" onclick="save_direct_purchase_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='close_vehmaster1' onclick="purchaseclearvalues()"></span><span id='close_vehmaster' onclick="purchaseclearvalues()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                </div>
                    </div>
                </div>
            <div class="box-body" id="divsales" style="display:none;">
                <div id='vehmaster_fillforms'>
                    <div style="padding-left: 100px;">
                        <table align="center">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px;">
                                    <label>
                                        DC Datetime</label>
                                    <input id="txt_sdate" class="form-control" type="datetime-local" name="vendorcode"
                                        placeholder="Enter Date">
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td>
                                    <label>
                                        Party Trans No</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_stransno" type="text" class="form-control" name="vendorcode" placeholder="DC No">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Party DC No</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_sdcno" type="text" class="form-control" name="vendorcode" placeholder="Party DC No">
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td style="height: 40px;">
                                    <label>
                                        Inward No</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_sinwardno" type="text" class="form-control" name="vendorcode" placeholder="Inward No">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Vendor Name</label>
                                    <select id="slct_vender_Name" class="form-control" onchange="Onchage_vender_Name();">
                                        <option selected disabled value="Select Vehicle No">Select Vendor Name</option>
                                    </select>
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td style="height: 40px;">
                                    <label>
                                        Vehicle No</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_svehicleno" type="text" class="form-control" name="vendorcode" placeholder="Vehicle No">
                                </td>
                                <%-- <td>
                        Type Of Milk
                    </td>
                    <td>
                        <select id="slct_milk_type" class="form-control">
                            <option selected disabled value="Select Type Of Milk">Select Type Of Milk</option>
                            <option>Buffalo</option>
                            <option>Cow</option>
                        </select>
                    </td>--%>
                    <td style="width: 6px;">
                                </td>
                                <td style="height: 40px;">
                                    <label>
                                       Transport value</label>
                                    <span style="color: red;">*</span>
                                    <input id="txt_stransportvalue" type="text" class="form-control" name="transportvalue" placeholder="transportvalue">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </table>
                    </div>
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk Details For Sales</h3>
                        </div>
                        <div id="divsalesdata">
                        </div>
                    </div>
                    <div style="text-align: center;">
                        <div style="padding-left: 200px;">
                            <table align="center">
                                <tr>
                                    <td style="height: 40px;">
                                        <label>
                                            Remarks</label>
                                        <textarea rows="3" cols="45" id="txt_sremarks" class="form-control" maxlength="200"
                                            placeholder="Enter Remarks"></textarea>
                                        <%--   <textarea id="txt_Remarks" class="form-control" rows="5" cols="45" placeholder="Remarks" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;">
                                        <label>
                                            Chemist</label>
                                        <input id="txt_schemist" type="text" class="form-control" name="vendorcode" placeholder="Chemist" />
                                    </td>
                                    <td style="width: 6px;">
                                    </td>
                                    <td>
                                        <label>
                                            QCO</label>
                                        <input id="txt_sqco" type="text" class="form-control" name="vendorcode" placeholder="QCO" />
                                    </td>
                                </tr>
                                <tr hidden>
                                    <td>
                                        <label id="lbl_ssno">
                                        </label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                       <%-- <input id='btn_save' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_direct_sales_click()" />
                        <input id='btn_clear' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearvalues()" />
                        <input id='btn_print' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />--%>
                        <div  style="padding-left: 40%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="save_direct_sales_click()"></span><span id="btn_save" onclick="save_direct_sales_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="clearvalues()"></span><span id='btn_close' onclick="clearvalues()">Close</span>
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
    </section>
</asp:Content>
