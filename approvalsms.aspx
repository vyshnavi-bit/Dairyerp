<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="approvalsms.aspx.cs" Inherits="approvalsms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btn_addDept').click(function () {
                $('#vehmaster_fillform').css('display', 'block');
                $('#div_Deptdata').hide();
                clearvalues();
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
            $('#close_vehmaster').click(function () {
                $('#vehmaster_fillform').css('display', 'none');
                $('#div_Deptdata').show();
                clearvalues();
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

            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txt_entrydate').val(today);
        });
        function clearvalues() {
            $('#vehmaster_fillform').css('display', 'none');
            $('#div_Deptdata').show();
            clearvalues();
        }
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
        function btn_approvalsms_click() {
            var dcno = document.getElementById('txt_dcno').value;
            var Inwardno = document.getElementById('txt_Inwardno').value;
            var vehicleNo = document.getElementById('txt_vehicleNo').value;
            var inwarddate = document.getElementById('txt_date').value;
            var fatcalon = "";
            var qco = document.getElementById('txt_qco').value;
            var Remarks = document.getElementById('txt_Remarks').value;
            var Chemist = document.getElementById('txt_Chemist').value;
            var SourceID = document.getElementById('slct_Source_Name').value;
            var btnvalue = document.getElementById('save_milktransactions').value;
            var sno = document.getElementById('lbl_tno').innerHTML;
            var pdcno = document.getElementById('lbl_pdcno').value;
            if (inwarddate == "") {
                alert("Please Select Date");
                return false;
            }
            if (dcno == "") {
                alert("Enter dc no");
                return false;
            }
            if (Inwardno == "") {
                alert("Enter inward no");
                return false;
            }
            if (vehicleNo == "") {
                alert("Enter vehicle no");
                return false;
            }
            if (SourceID == "" || SourceID == "Select Vendor Name") {
                alert("Select plant name");
                return false;
            }
            if (Chemist == "") {
                alert("Enter Chemist name");
                return false;
            }
            if (qco == "") {
                alert("Enter qco name");
                return false;
            }
            var MilkfatDetailsMilkarray = [];
            $('#tbl_milk_details> tbody > tr').each(function () {
                var milktype = $(this).find('[name="milktype"]').val();
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
            var confi = confirm("Do you want to Send SMS ?");
            if (confi) {
                var data = { 'op': 'save_approvalsms_click', 'dcno': dcno, 'inwardno': Inwardno, 'vehicleno': vehicleNo, 'dispdate': inwarddate,
                    'fatcalon': fatcalon, 'qco': qco, 'remarks': Remarks, 'chemist': Chemist, 'sourceid': SourceID, 'sno': sno, 'btnvalue': btnvalue, 'pdcno': pdcno, 'MilkfatDetailsMilkarray': MilkfatDetailsMilkarray
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        $('#vehmaster_fillform').css('display', 'none');
                        $('#div_Deptdata').show();
                        clearvalues();
                        Getmilkdetails();
                        get_Vendor_details();
                        get_approval_sms_details();

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
        function clearvalues() {
            document.getElementById('txt_dcno').value = "";
            document.getElementById('txt_Inwardno').value = "";
            document.getElementById('txt_vehicleNo').value = "";
            document.getElementById('txt_date').value = "";
            document.getElementById('txt_qco').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('txt_Chemist').value = "";
            document.getElementById('slct_Source_Name').selectedIndex = 0;
            //            document.getElementById('slct_milk_type').selectedIndex = 0;
            Getmilkdetails();
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
            var results = '<div  style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">MilkType</th><th scope="col">Cell Name</th><th scope="col" style="font-weight: 700;">Qty(Kgs)</th><th scope="col" style="font-weight: 700;">Qty(Ltrs)</th><th scope="col" style="font-weight: 700;">FAT</th><th scope="col" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">CLR</th><th scope="col" style="font-weight: 700;">TEMP</th><th scope="col" style="font-weight: 700;">Acidity</th><th scope="col" style="font-weight: 700;">COB</th><th scope="col" style="font-weight: 700;">OT</th><th scope="col" style="font-weight: 700;">HS</th><th scope="col" style="font-weight: 700;">Phosps</th><th scope="col" style="font-weight: 700;">Alcohol%</th><th scope="col" style="font-weight: 700;">MBRT</th><th scope="col" style="font-weight: 700;">Neutralizers</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < names.length; i++) {
                results += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results += '<th data-title="From"><select class="form-control" name="milktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + names[i] + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty(kgs)" name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" disabled="disabled" placeholder="Qty(Ltrs)" id="txtltr" name="Qtyltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="FAT" name="FAT" id="txtfat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF" name="SNF" value="" id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="CLR"  onkeyup="LRChange(this);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Temp" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="TEMP" name="TEMP" value="" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Acidity" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Acidity" name="Acidity"  id="txtacidity"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="COB" style="text-align:center;width:65px;" class="5"><select class="form-control" name="COB" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                results += '<td data-title="OT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="OT" name="OT" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="HS" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="HS" name="HS" id="txths"   value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Phosps" style="text-align:center;width:65px;" class="5"><select class="form-control" name="Phosps" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                results += '<td data-title="Alcohol" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Alcohol%" id="txtalcohol" name="Alcohol" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="MBRT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="MBRT" name="MBRT" id="txtmbrt" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Neutralizers" style="text-align:center;" class="5"><select class="form-control" name="Neutralizers" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td></tr>';
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
        }

        function get_approval_sms_details() {
            var entrydate = document.getElementById('txt_entrydate').value;
            var data = { 'op': 'get_approval_sms_details', 'entrydate': entrydate };
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
        var transactionsubdetails = [];
        function fillinwordmilkdetails(msg) {
            var results = '<div  style="overflow:auto;"><table  class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Date</th></th><th scope="col" style="font-weight: bold;">DCNo</th><th scope="col" style="font-weight: bold;">InwardNo</th><th scope="col" style="font-weight: bold;">VehicleNo</th><th scope="col" style="font-weight: bold;">Vendor Name</th><th scope="col" style="font-weight: bold;">Chemist</th><th scope="col" style="font-weight: bold;">QCO</th><th scope="col" style="font-weight: bold;">Remarks</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var transactiondetails = msg[0].transactiondetails;
            transactionsubdetails = msg[0].transactionsubdetails;
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < transactiondetails.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td class="11">' + transactiondetails[i].doe + '</td>';
                results += '<th scope="row" class="1" style="text-align:center;">' + transactiondetails[i].dcno + '</th>';
                results += '<td data-title="Capacity" class="2">' + transactiondetails[i].inwardno + '</td>';
                results += '<td data-title="Capacity" class="3">' + transactiondetails[i].vehicleno + '</td>';
                results += '<td data-title="Capacity" class="3">' + transactiondetails[i].vendorname + '</td>';
                results += '<td data-title="Capacity" class="10">' + transactiondetails[i].chemist + '</td>';
                results += '<td data-title="Capacity" class="8">' + transactiondetails[i].qco + '</td>';
                results += '<td data-title="Capacity" class="9">' + transactiondetails[i].remarks + '</td>';
                results += '<td style="display:none" class="4">' + transactiondetails[i].sectionid + '</td>';
                results += '<td  style="display:none" class="5">' + transactiondetails[i].transportcost + '</td>';
                results += '<td  style="display:none" class="6">' + transactiondetails[i].transport + '</td>';
                results += '<td style="display:none" class="7">' + transactiondetails[i].rateon + '</td>';
                results += '<td style="display:none" class="12">' + transactiondetails[i].milktransactonno + '</td>';
                results += '<td style="display:none" class="13">' + transactiondetails[i].partydcno + '</td>';
                results += '<td style="display:none" class="15">' + transactiondetails[i].datetime + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Approve!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 apprvcls"  onclick="getme(this)"><span class="glyphicon glyphicon-thumbs-up" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
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
            var date = $(thisid).parent().parent().children('.15').html();
            var milktransactonno = $(thisid).parent().parent().children('.12').html();
            var partydcno = $(thisid).parent().parent().children('.13').html();


            document.getElementById('txt_dcno').value = dcno;
            document.getElementById('txt_Inwardno').value = inwardno;
            document.getElementById('txt_vehicleNo').value = vehicalno;
            document.getElementById('txt_date').value = date;
            document.getElementById('txt_qco').value = Qco;
            document.getElementById('txt_Remarks').value = Remarks;
            document.getElementById('txt_Chemist').value = chemist;

            document.getElementById('lbl_pdcno').value = partydcno;

            document.getElementById('slct_Source_Name').value = sectionid;
            document.getElementById('lbl_sno').innerHTML = dcno;
            document.getElementById('save_milktransactions').value = "Send verified Sms";
            var results = '<div  style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">MilkType</th><th scope="col" style="font-weight: 700;">Cell Name</th><th scope="col" style="font-weight: 700;">Qty(Kgs)</th><th scope="col" style="font-weight: 700;">Qty(Ltrs)</th><th scope="col" style="font-weight: 700;">FAT</th><th scope="col" style="font-weight: 700;">SNF</th><th scope="col" style="font-weight: 700;">CLR</th><th scope="col" style="font-weight: 700;">TEMP</th><th scope="col" style="font-weight: 700;">Acidity</th><th scope="col" style="font-weight: 700;">COB</th><th scope="col" style="font-weight: 700;">OT</th><th scope="col" style="font-weight: 700;">HS</th><th scope="col" style="font-weight: 700;">Phosps</th><th scope="col" style="font-weight: 700;">Alcohol%</th><th scope="col" style="font-weight: 700;">MBRT</th><th scope="col" style="font-weight: 700;">Neutralizers</th><th scope="col" style="font-weight: 700;">SealNo</th></tr></thead></tbody>';
            for (var j = 0; j < transactionsubdetails.length; j++) {
                if (dcno == transactionsubdetails[j].dcno) {
                    results += '<td data-title="Sno" class="2">' + j + '</td>';
                    results += '<th data-title="From"><input class="form-control" disabled="disabled" type="text" placeholder="milktype" name="milktype" value="' + transactionsubdetails[j].milktype + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + transactionsubdetails[j].CellName + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                    results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Qty" style="text-align:center;"  class="3"><input class="form-control" type="text" disabled="disabled" placeholder="Qty(Ltrs)"  name="Qtyltr" id="txtltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="FAT" name="FAT" value="" id="txtfat" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF" name="SNF" value=""  id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="CLR" onkeyup="LRChange(this);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Temp" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="TEMP" name="TEMP" value="" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Acidity" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Acidity" name="Acidity" id="txtacidity" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="COB" style="text-align:center;width:65px;" class="5"><select class="form-control" name="COB" style="width:100%;  font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                    results += '<td data-title="OT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="OT" name="OT" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="HS" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="HS" name="HS" value="" id="txths" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Phosps" style="text-align:center;width:65px;" class="5"><select class="form-control" name="Phosps" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                    results += '<td data-title="Alcohol" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Alcohol%" name="Alcohol"  id="txtalcohol" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="MBRT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="MBRT" name="MBRT" value="" id="txtmbrt" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Neutralizers" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Neutralizers" name="Neutralizers" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="Sealno" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Sealno" name="Sealno" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
                }
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
            for (var i = 0; i < transactionsubdetails.length; i++) {
                if (dcno == transactionsubdetails[i].dcno && inwardno == transactionsubdetails[i].inwardno) {
                    $('#tbl_milk_details> tbody > tr').each(function () {
                        var cell = $(this).find('[name="CellName"]').val();
                        var tno = transactionsubdetails[i].sno;
                        if (cell == transactionsubdetails[i].CellName) {
                            $(this).find('select[name*="milktype"] :selected').val(transactionsubdetails[i].milktype);
                            $(this).find('[name="CellName"]').val(transactionsubdetails[i].CellName);
                            $(this).find('[name="Qtykg"]').val(transactionsubdetails[i].qtykgs);
                            $(this).find('[name="Qtyltr"]').val(transactionsubdetails[i].qtyltr);
                            $(this).find('[name="FAT"]').val(transactionsubdetails[i].fat);
                            $(this).find('[name="SNF"]').val(transactionsubdetails[i].snf);
                            $(this).find('[name="TEMP"]').val(transactionsubdetails[i].temp);
                            $(this).find('[name="Acidity"]').val(transactionsubdetails[i].acidity);
                            $(this).find('[name="CLR"]').val(transactionsubdetails[i].clr);
                            $(this).find('select[name*="COB"] :selected').val(transactionsubdetails[i].cob1);
                            $(this).find('[name="OT"]').val(transactionsubdetails[i].ot);
                            $(this).find('[name="HS"]').val(transactionsubdetails[i].hs);
                            $(this).find('select[name*="Phosps"] :selected').val(transactionsubdetails[i].phosps1);
                            $(this).find('[name="Alcohol"]').val(transactionsubdetails[i].alcohol);
                            $(this).find('[name="Neutralizers"]').val(transactionsubdetails[i].neutralizers);
                            $(this).find('[name="MBRT"]').val(transactionsubdetails[i].mbrt);
                            $(this).find('[name="Sealno"]').val(transactionsubdetails[i].sealno);
                            document.getElementById('lbl_tno').innerHTML = tno;
                        }
                    });
                }
            }
            $("#div_Deptdata").hide();
            $("#vehmaster_fillform").show();

        }

        //Function for only no
        $(document).ready(function () {
            $("#txtkg,#txtkg,#txtfat,#txtsnf,#txtclr,#txttemp,#txtacidity,#txths,#txtalcohol,#txtmbrt").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Approval SMS <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Approval SMS</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Approval SMS Details
                </h3>
            </div>
            
            <div class="box-body">
            <div align="center">
              <table>
              <tr>
                  <td>
                      <label>
                        DC  Date</label>
                  </td>
                  <td>
                      <input id="txt_entrydate" class="form-control" type="date" name="vendorcode" placeholder="Select Date">
                  </td>
                  <td style="width:5px;"></td>
                  <td>
                      <%--<input id='Button1' type="button" class="btn btn-primary" name="submit" value='Generate'
                          onclick="get_approval_sms_details()" />--%>
                            <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="get_approval_sms_details()"></span> <span onclick="get_approval_sms_details()">Generate</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                  </td>
              </tr>
          </table>
            </div>
                <div id="div_Deptdata">
                </div>
                <div id='vehmaster_fillform' style="display: none;">
                    <table >
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td  style="height:40px;">
                               <label> Datetime</label>
                               </td>
                               <td>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                            <td style="width:6px;">                         
                            </td>
                            <td>
                                <label>Party Trans No</label><span style="color: red;">*</span>
                                   </td>
                               <td>
                                <input id="txt_dcno" type="text" class="form-control" name="vendorcode" placeholder="DC No">
                            </td>
                        </tr>
                        <tr>
                            <td  style="height:40px;">
                                <label>Inward No</label><span style="color: red;">*</span>
                                   </td>
                               <td>
                                <input id="txt_Inwardno" type="text" class="form-control" name="vendorcode" placeholder="Inward No">
                            </td>
                            <td style="width:6px;">                         
                            </td>
                            <td>
                               <label> Vendor Name</label>
                                  </td>
                               <td>
                                <select id="slct_Source_Name" class="form-control">
                                    <option selected disabled value="Select Vehicle No">Select Vendor Name</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td  style="height:40px;">
                                <label>Vehicle No</label><span style="color: red;">*</span>
                                   </td>
                               <td>
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
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk Details</h3>
                        </div>
                        <div id="div_vendordata">
                        </div>
                    </div>
                    <div style="text-align: center;">
                        <table align="center" style="width: 64%;">
                            <tr>
                                <td  style="height:40px;">
                                <label>    Remarks</label>
                                   </td>
                               <td>
                                    <textarea rows="3" cols="45" id="txt_Remarks" class="form-control" maxlength="200"
                                        placeholder="Enter Remarks"></textarea>
                                    <%--   <textarea id="txt_Remarks" class="form-control" rows="5" cols="45" placeholder="Remarks" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td  style="height:50px;">
                                 <label>   Chemist</label>
                                    </td>
                               <td>
                                    <input id="txt_Chemist" type="text" class="form-control" name="vendorcode" placeholder="Chemist" />
                                </td>
                                <td style="width:6px;">                         
                            </td>
                                <td>
                                  <label>  QCO</label>
                                     </td>
                               <td>
                                    <input id="txt_qco" type="text" class="form-control" name="vendorcode" placeholder="QCO" />
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
                                    <label id="lbl_tno">
                                    </label>
                                </td>
                            </tr>
                            <tr hidden>
                            <td>
                            <label id="lbl_pdcno">
                            </label>
                            </td>
                            </tr>
                        </table>
                      <%--  <input id='save_milktransactions' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="btn_approvalsms_click()" />
                        <input id='close_vehmaster' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearvalues()" />
                        <input id='btnPrint' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />--%>
                        <div  style="padding-left: 37%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_milktransactions1" onclick="btn_approvalsms_click()"></span><span id="save_milktransactions" onclick="btn_approvalsms_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='close_vehmaster1' onclick="clearvalues()"></span><span id='close_vehmaster' onclick="clearvalues()">Clear</span>
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

