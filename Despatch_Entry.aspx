<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Despatch_Entry.aspx.cs" Inherits="Despatch_Entry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="JSF/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#btn_addDept').click(function () {
                $('#vehmaster_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
                clearvalues();
                Getmilkdetails();
                get_Vehicle_Master_details();
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

            $('#close_details').click(function () {
                $('#vehmaster_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                clearvalues();
            });
            getinwordvalues();
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
        function adddespatchentry() {
            $('#vehmaster_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            Getmilkdetails();
            get_Vehicle_Master_details();
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
        function get_Vehicle_Master_details() {
            var data = { 'op': 'get_Vehicle_Master_details' }; // get_Vehicle_fleet_details
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillSource(msg);
                    }
                    else {
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

        function fillSource(msg) {
            var data = document.getElementById('slct_VehicleNo');
            var length = data.options.length;
            document.getElementById('slct_VehicleNo').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vehicle No";
            opt.value = "Select Vehicle No";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].vehicleno != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].vehicleno;
                    option.value = msg[i].sno;
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
        function save_despatch_entry_click() {
            var vehicleNo = $("#slct_VehicleNo :selected").text();
            var vehicleid = document.getElementById('slct_VehicleNo').value;
            var inwarddate = document.getElementById('txt_date').value;
            var qco = document.getElementById('txt_qco').value;
            var Remarks = document.getElementById('txt_Remarks').value;
            var Chemist = document.getElementById('txt_Chemist').value;
            var btnvalue = document.getElementById('save_milktransactions').innerHTML;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var ddlsalesType = document.getElementById('ddlType').value;

            var fatcalon = "";
//            if (inwarddate == "") {
//                alert("please select date");
//                return false;
//            }
            if (vehicleNo == "" || vehicleNo == "Select Vehicle No") {
                alert("Enter vehicle no");
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
                var milktype = $(this).find('select[name*="milktype"] :selected').val();
                var CellName = $(this).find('[name="CellName"]').val();
                var Qtykg = $(this).find('[name="Qtykg"]').val();
                var Qtyltr = $(this).find('[name="Qtyltr"]').val();
                var fat = $(this).find('[name="FAT"]').val();
                var snf = $(this).find('[name="SNF"]').val();
                var temp = $(this).find('[name="TEMP"]').val();
                var acidity = $(this).find('[name="Acidity"]').val();
                var clr = $(this).find('[name="CLR"]').val();
                var cob = $(this).find('select[name*="COB"] :selected').val();
                var ot = $(this).find('[name="OT"]').val();
                var hs = $(this).find('[name="HS"]').val();
                var phosps = $(this).find('select[name*="Phosps"] :selected').val();
                var alcohol = $(this).find('[name="Alcohol"]').val();
                var neutralizers = $(this).find('[name="Neutralizers"]').val();
                var MBRT = $(this).find('[name="MBRT"]').val();
                var Sealno = $(this).find('[name="Sealno"]').val();
                if (Qtykg == "") {
                }
                else {
                    MilkfatDetailsMilkarray.push({ 'milktype': milktype, 'CellName': CellName, 'Qtykg': Qtykg, 'Qtyltr': Qtyltr, 'fat': fat, 'snf': snf, 'temp': temp,
                        'acidity': acidity, 'clr': clr, 'cob': cob, 'ot': ot, 'hs': hs, 'phosps': phosps, 'alcohol': alcohol, 'neutralizers': neutralizers, 'mbrt': MBRT, 'sealno': Sealno
                    });
                }
            });
            if (MilkfatDetailsMilkarray.length == "0") {
                alert("Please Enter Quantity in Kg's ");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_despatch_entry_click', 'vehicleno': vehicleNo, 'dispdate': inwarddate, 'salesType': ddlsalesType, 'vehicleid': vehicleid,
                    'qco': qco, 'remarks': Remarks, 'fatcalon': fatcalon, 'chemist': Chemist, 'btnvalue': btnvalue, 'sno': sno, 'MilkfatDetailsMilkarray': MilkfatDetailsMilkarray
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg == "OK") {
                            alert("Dispatch details  Successfully Inserted");
                            clearvalues();
                            getinwordvalues();
                            $('#div_Deptdata').show();
                            $('#vehmaster_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                        }
                        else if (msg == "update") {
                            alert("Dispatch details Successfully Modified");
                            clearvalues();
                            getinwordvalues();
                            $('#div_Deptdata').show();
                            $('#vehmaster_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                        }
                        else {
                            alert("Please Raise The DC With Currect Information(qty ltrs is empty)");
                            clearvalues();
                            getinwordvalues();
                            $('#div_Deptdata').show();
                            $('#vehmaster_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
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
        function clearvalues() {
            document.getElementById('slct_VehicleNo').selectedIndex = 0;
            document.getElementById('txt_date').value = "";
            document.getElementById('txt_qco').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('txt_Chemist').value = "";
            document.getElementById('save_milktransactions').innerHTML = "Save";
            $('#vehmaster_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_Deptdata').show();
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
        function getinwordvalues() {
            var data = { 'op': 'get_InwordMilkTransaction_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails(msg);
                        get_Vehicle_Master_details();

                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var dispatch_subdetails = [];
        function filldetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            //results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;><th scope="col">DCNo</th><th scope="col">ccid</th><th scope="col">entryby</th><th scope="col">vehicalno</th><th scope="col">transtype</th><th scope="col">chemist</th><th scope="col"></th></tr></thead></tbody>';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">DCNo</th><th scope="col" style="font-weight: bold;">CCId</th><th scope="col" style="font-weight: bold;">Entry By</th><th scope="col" style="font-weight: bold;">Vehicle Number</th><th scope="col" style="font-weight: bold;">Transaction Type</th><th scope="col" style="font-weight: bold;">Chemist</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var dispathdetails = msg[0].Dispathentrydetails;
            dispatch_subdetails = msg[0].Dispathsubdetails;
            for (var i = 0; i < dispathdetails.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-success" value="Edit" /></td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + dispathdetails[i].dcno + '</td>';
                results += '<td data-title="Capacity" class="2">' + dispathdetails[i].ccid + '</td>';
                results += '<td data-title="Capacity" class="3">' + dispathdetails[i].entryby + '</td>';
                results += '<td data-title="Capacity" class="4">' + dispathdetails[i].vehicalno + '</td>';
                results += '<td data-title="Capacity" class="5">' + dispathdetails[i].transtype + '</td>';
                results += '<td data-title="Capacity" class="6">' + dispathdetails[i].Chemist + '</td>';
                results += '<td style="display:none" class="7">' + dispathdetails[i].Remarks + '</td>';
                results += '<td style="display:none" class="8">' + dispathdetails[i].date + '</td>';
                results += '<td style="display:none" class="9">' + dispathdetails[i].Qco + '</td>';
                results += '<td style="display:none" class="10">' + dispathdetails[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function getme(thisid) {
            var sno = $(thisid).parent().parent().children('.10').html();
            var vehicalno = $(thisid).parent().parent().children('.4').html();
            var chemist = $(thisid).parent().parent().children('.6').html();
            var entryby = $(thisid).parent().parent().children('.3').html();
            var Remarks = $(thisid).parent().parent().children('.7').html();
            var Qco = $(thisid).parent().parent().children('.9').html();
            var date = $(thisid).parent().parent().children('.8').html();
            document.getElementById('lbl_sno').innerHTML = sno;
            document.getElementById('slct_VehicleNo').value = vehicalno;
            document.getElementById('txt_date').value = date;
            document.getElementById('txt_qco').value = Qco;
            document.getElementById('txt_Remarks').value = Remarks;
            document.getElementById('txt_Chemist').value = chemist;
            document.getElementById('save_milktransactions').innerHTML = "Modify";
            var results = '<div    style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">MilkType</th><th scope="col">Cell Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th><th scope="col">TEMP</th><th scope="col">Acidity</th><th scope="col">COB</th><th scope="col">OT</th><th scope="col">HS</th><th scope="col">Phosps</th><th scope="col">Alcohol%</th><th scope="col">MBRT</th><th scope="col">Neutralizers</th><th scope="col">SealNo</th></tr></thead></tbody>';
            for (var j = 0; j < dispatch_subdetails.length; j++) {
                if (sno == dispatch_subdetails[j].desprefno) {
                    results += '<td data-title="Sno" class="2">' + j + '</td>';
                    results += '<th data-title="From"><select class="form-control" name="milktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                    results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + dispatch_subdetails[j].cellname + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
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
            for (var i = 0; i < dispatch_subdetails.length; i++) {
                if (sno == dispatch_subdetails[i].desprefno) {
                    $('#tbl_milk_details> tbody > tr').each(function () {
                        var cell = $(this).find('[name="CellName"]').val();
                        if (cell == dispatch_subdetails[i].cellname) {
                            $(this).find('select[name*="milktype"] :selected').val(dispatch_subdetails[i].milktype);
                            $(this).find('[name="CellName"]').val(dispatch_subdetails[i].cellname);
                            $(this).find('[name="Qtykg"]').val(dispatch_subdetails[i].qtykgs);
                            $(this).find('[name="Qtyltr"]').val(dispatch_subdetails[i].qtyltr);
                            $(this).find('[name="FAT"]').val(dispatch_subdetails[i].fat);
                            $(this).find('[name="SNF"]').val(dispatch_subdetails[i].snf);
                            $(this).find('[name="TEMP"]').val(dispatch_subdetails[i].temp);
                            $(this).find('[name="Acidity"]').val();
                            $(this).find('[name="CLR"]').val(dispatch_subdetails[i].clr);
                            $(this).find('select[name*="COB"] :selected').val(dispatch_subdetails[i].cob1);
                            $(this).find('[name="OT"]').val(dispatch_subdetails[i].ot);
                            $(this).find('[name="HS"]').val(dispatch_subdetails[i].hs);
                            $(this).find('select[name*="Phosps"] :selected').val(dispatch_subdetails[i].phosps1);
                            $(this).find('[name="Alcohol"]').val(dispatch_subdetails[i].alcohol);
                            $(this).find('[name="Neutralizers"]').val();
                            $(this).find('[name="MBRT"]').val(dispatch_subdetails[i].mbrt);
                            $(this).find('[name="Sealno"]').val(dispatch_subdetails[i].sealno);
                        }
                    });
                }
            }
            $("#div_Deptdata").hide();
            $("#vehmaster_fillform").show();
            $('#showlogs').hide();
        }
        function Getmilkdetails() {
            var cellnames = "F Cell,M Cell,B Cell";
            var names = cellnames.split(',');
            var results = '<div    style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">MilkType</th><th scope="col">Cell Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th><th scope="col">TEMP</th><th scope="col">Acidity</th><th scope="col">COB</th><th scope="col">OT</th><th scope="col">HS</th><th scope="col">Phosps</th><th scope="col">Alcohol%</th><th scope="col">MBRT</th><th scope="col">Neutralizers</th><th scope="col">SealNo</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < names.length; i++) {
                results += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results += '<th data-title="From"><select class="form-control" name="milktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + names[i] + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty(kgs)" onkeypress="return isFloat(event);"  name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Qty" style="text-align:center;"  class="3"><input class="form-control" type="text" disabled="disabled" onkeypress="return isFloat(event);" placeholder="Qty(Ltrs)"  name="Qtyltr" id="txtltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="FAT" name="FAT" value="" id="txtfat" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF" onkeypress="return isFloat(event);" name="SNF" value=""  id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="CLR" onkeypress="return isFloat(event);" onkeyup="LRChange(this);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Temp" style="text-align:center;" class="5"><input class="form-control" type="number" placeholder="TEMP"  name="TEMP" value="" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Acidity" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Acidity" name="Acidity" id="txtacidity" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="COB" style="text-align:center;width:65px;" class="5"><select class="form-control" name="COB" style="width:100%;  font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                results += '<td data-title="OT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="OT" name="OT" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="HS" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="HS" name="HS" value="" onkeypress="return isFloat(event);" id="txths" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Phosps" style="text-align:center;width:65px;" class="5"><select class="form-control" name="Phosps" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-ve">-ve</option><option  value="+ve">+ve</option></select></td>';
                results += '<td data-title="Alcohol" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Alcohol%" onkeypress="return isFloat(event);" name="Alcohol"  id="txtalcohol" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="MBRT" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="MBRT" name="MBRT" onkeypress="return isFloat(event);" value="" id="txtmbrt" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Neutralizers" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Neutralizers" name="Neutralizers" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Sealno" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Sealno" name="Sealno" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
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
        //Function for only no
        $(document).ready(function () {
            $("#txtkg,#txtkg,#txtfat,#txtsnf,#txtclr,#txttemp,#txtacidity,#txths,#txtalcohol,#txtmbrt").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 || event.keyCode == 183 ||
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
          <i class="fa fa-truck fa-2x" aria-hidden="true"></i>  Despatch Entry<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Despatch Entry</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Despatch Entry
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                    <%--<input id="btn_addDept" type="button" name="submit" value='Add Despatch Entry' class="btn btn-success" />--%>
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="adddespatchentry()"></span> <span onclick="adddespatchentry()">Add Despatch</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='vehmaster_fillform' style="display: none;">
                    <div style="padding-left:253px;">
                    <table align="center">
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td style="height: 40px;">
                                <label>
                                    Datetime</label>
                            </td>
                            <td>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Vehicle No</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                                <select id="slct_VehicleNo" class="form-control">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Destination</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlType" class="form-control">
                                    <option>SVDS PUNABAKA</option>
                                    <option>SVF WYRA</option>
                                    <option>KUPPAM</option>
                                    <option>SANGAM</option>
                                    <option>NVL DAIRY</option>
                                    <option>HYDERABAD PLANT</option>
                                </select>
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
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk Details</h3>
                        </div>
                        <div id="div_vendordata">
                        </div>
                    </div>
                    <div style="text-align: center;">
                    <div style="padding-left:253px;">
                        <table align="center">
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Remarks</label>
                                </td>
                                <td colspan="6">
                                    <textarea rows="3" cols="45" id="txt_Remarks" class="form-control" maxlength="200"
                                        placeholder="Enter Remarks"></textarea>
                                    <%--   <textarea id="txt_Remarks" class="form-control" rows="5" cols="45" placeholder="Remarks" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Chemist</label>
                                </td>
                                <td>
                                    <input id="txt_Chemist" type="text" class="form-control" name="vendorcode" placeholder="Chemist" />
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td>
                                    <label>
                                        QCO</label>
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
                        </table>
                        </div>
                        <%--<input id='save_milktransactions' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_despatch_entry_click()" />
                        <input id='close_details' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearvalues()" />
                        <input id='btnPrint' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />--%>
                        <div style="text-align: -webkit-center;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_milktransactions1" onclick="save_despatch_entry_click()"></span><span id="save_milktransactions" onclick="save_despatch_entry_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='close_details1' onclick="clearvalues()"></span><span id='close_details' onclick="clearvalues()">Close</span>
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
    </section>
</asp:Content>
