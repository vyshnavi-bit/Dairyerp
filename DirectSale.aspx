<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="DirectSale.aspx.cs" Inherits="DirectSale" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            get_dispatchdc_details();
            $('#btn_addDept').click(function () {
                $('#vehmaster_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
                clearvalues();
                Getmilkdetails();
                get_Client_details();
                //  get_Vehicle_Master_details();
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
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                clearvalues();
            });
            //            getinwordvalues();
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
        function adddirectsale() {
            $('#vehmaster_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            clearvalues();
            Getmilkdetails();
            get_Client_details();
            //  get_Vehicle_Master_details();
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
            var data = { 'op': 'get_Vehicle_Master_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillVehicles(msg);
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
        function fillVehicles(msg) {
            var data = document.getElementById('txt_vno');
            var length = data.options.length;
            document.getElementById('txt_vno').options.length = null;
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
        function get_Client_details() {
            var data = { 'op': 'get_direct_sale_Vendor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var Vendor = msg[0].vendordetails;
                        fillSource(Vendor);
                        fillDestination(Vendor);
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
        function fillDestination(msg) {
            var data = document.getElementById('slct_dest_Name');
            var length = data.options.length;
            document.getElementById('slct_dest_Name').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vendor Name";
            opt.value = "Select Vendor Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].vendorname != null) {
                    if (msg[i].vendorname != "Vendor") {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].vendorname;
                        option.value = msg[i].sno;
                        data.appendChild(option);
                    }
                }
            }
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
        function get_dispatchdc_details() {
            var data = { 'op': 'get_divdirectsale_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldivdirectsaledetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldivdirectsaledetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col">Date</th></th><th scope="col">DCNo</th><th scope="col">From CC Name</th><th scope="col">To CC Name</th><th scope="col">Cell Name</th><th scope="col">Milk Type</th><th scope="col">Fat</th><th scope="col">Snf</th><th scope="col">Clr</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '"><td>' + msg[i].doe + '</td>';
                results += '<td scope="row" class="1" >' + msg[i].dcno + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].fromcc + '</td>';
                results += '<td data-title="Capacity" class="3">' + msg[i].tocc + '</td>';
                results += '<td data-title="Capacity" class="3">' + msg[i].cell + '</td>';
                results += '<td data-title="Capacity" class="3">' + msg[i].milktype + '</td>';
                results += '<td data-title="Capacity" class="3">' + msg[i].fat + '</td>';
                results += '<td data-title="Capacity" class="3">' + msg[i].snf + '</td>';
                results += '<td class="12">' + msg[i].clr + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function save_despatch_entry_click() {
            var inwarddate = document.getElementById('txt_date').value;
            var refdcNo = document.getElementById('txt_dcNo').value;
            if (refdcNo == "0") {
                var newdcNo = document.getElementById('txt_newdcno').value;
            }
            var vehicleNo = document.getElementById('txt_vno').value;
            var Remarks = document.getElementById('txt_Remarks').value;
            var fromSourceID = document.getElementById('slct_Source_Name').value;
            var toSourceID = document.getElementById('slct_dest_Name').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var btnvalue = document.getElementById('save_milktransactions').innerHTML;
            var fatcalon = "";
            if (inwarddate == "") {
                alert("please select date");
                return false;
            }
            if (vehicleNo == "" || vehicleNo == "Select Vehicle No") {
                alert("Select Vehicle No");
                return false;
            }
            if (toSourceID == "" || toSourceID == "Select Vendor Name") {
                alert("Select Vendor Name");
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
                //                var temp = $(this).find('[name="TEMP"]').val();
                //                var acidity = $(this).find('[name="Acidity"]').val();
                var clr = $(this).find('[name="CLR"]').val();
                //                var cob = $(this).find('select[name*="COB"] :selected').val();
                //                var ot = $(this).find('[name="OT"]').val();
                //                var hs = $(this).find('[name="HS"]').val();
                //                var phosps = $(this).find('select[name*="Phosps"] :selected').val();
                //                var alcohol = $(this).find('[name="Alcohol"]').val();
                //                var neutralizers = $(this).find('[name="Neutralizers"]').val();
                //                var MBRT = $(this).find('[name="MBRT"]').val();
                //                var Sealno = $(this).find('[name="Sealno"]').val();
                if (Qtykg == "") {
                }
                else {
                    MilkfatDetailsMilkarray.push({ 'milktype': milktype, 'CellName': CellName, 'Qtykg': Qtykg, 'Qtyltr': Qtyltr, 'fat': fat, 'snf': snf, 'clr': clr });
                }
            });
            if (MilkfatDetailsMilkarray.length == "0") {
                alert("Please enter quaninty in kg's, fat, snf details");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_milk_direct_sale_click', 'vehicleno': vehicleNo, 'dcno': refdcNo, 'newdcNo':newdcNo, 'dispdate': inwarddate, 'remarks': Remarks, 'fromsourceid': fromSourceID, 'tosourceid': toSourceID, 'btnvalue': btnvalue, 'sno': sno, 'MilkfatDetailsMilkarray': MilkfatDetailsMilkarray
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        clearvalues();
                        $('#div_Deptdata').show();
                        $('#vehmaster_fillform').css('display', 'none');
                        $('#showlogs').css('display', 'block');
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
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('slct_Source_Name').selectedIndex = 0;
            document.getElementById('txt_vno').selectedIndex = 0;
            document.getElementById('save_milktransactions').innerHTML = "Save";
            document.getElementById('txt_dcNo').value = "";
            document.getElementById('txt_vno').value = "";
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
                snfvalue = parseFloat(snfvalue).toFixed(3);
                $(qtyid).closest("tr").find('#txtsnf').val(snfvalue);
            }
        }
        function Getmilkdetails() {
            var cellnames = "F Cell,M Cell,B Cell";
            var names = cellnames.split(',');
            var results = '<div    style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">MilkType</th><th scope="col">Cell Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < names.length; i++) {
                results += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results += '<th data-title="From"><select class="form-control" name="milktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + names[i] + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="Qty(kgs)" name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Qty" style="text-align:center;"  class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" disabled="disabled" placeholder="Qty(Ltrs)"  name="Qtyltr" id="txtltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="FAT" name="FAT" value="" id="txtfat" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" onkeypress="return isFloat(event);" type="text"  placeholder="SNF" name="SNF" value=""  id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="CLR" onkeyup="LRChange(this);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
            var results1 = '<div    style="overflow:auto;"><table id="tbl_milk_vendor_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results1 += '<thead><tr><th scope="col">Sno</th><th scope="col">MilkType</th><th scope="col">Cell Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < names.length; i++) {
                results1 += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results1 += '<th data-title="From"><select class="form-control" name="milktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                results1 += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + names[i] + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results1 += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="Qty(kgs)" name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results1 += '<td data-title="Qty" style="text-align:center;"  class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" disabled="disabled" placeholder="Qty(Ltrs)"  name="Qtyltr" id="txtltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results1 += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="FAT" name="FAT" value="" id="txtfat" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results1 += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="SNF" name="SNF" value=""  id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results1 += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" onkeypress="return isFloat(event);" type="text" placeholder="CLR" onkeyup="LRChange(this);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
            }
            results1 += '</table></div>';
            $("#div_milkdata").html(results1);
            
        }
        function Get_DespatchDetails_click() {
            var dcNo = document.getElementById('txt_dcNo').value;
            if (dcNo == "") {
                alert("Please enter dc no");
                return false;
            }
            var data = { 'op': 'get_dcno_details', 'dcno': dcNo };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (dcNo == "0") {
                            document.getElementById('txt_vno').value = "";
                            $('#txt_vno').prop('readonly', false);
                            $('.newdcno').css('display', 'table-row');
                        }
                        else {
                            $('#txt_vno').prop('readonly', true);
                            filldispatchdetails(msg);
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
        var dispatch_subdetails = [];
        function filldispatchdetails(msg) {
            dispatch_subdetails = msg[0].Dispathsubdetails;
            var dispathdetails = msg[0].Dispathentrydetails;
            var dcno;
            var vehicleno;
            var vender;
            for (var i = 0; i < dispathdetails.length; i++) {
                dcno = dispathdetails[i].sno;
                vehicleno = dispathdetails[i].vehicalno;
                vender = dispathdetails[i].ccid;
            }
            document.getElementById('txt_vno').value = vehicleno;
            document.getElementById('slct_Source_Name').value = vender;
            if (dispatch_subdetails.length > 0) {
                var results = '<div    style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">MilkType</th><th scope="col">Cell Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th></tr></thead></tbody>';
                for (var j = 0; j < dispatch_subdetails.length; j++) {
                    if (dcno == dispatch_subdetails[j].desprefno) {
                        results += '<td data-title="Sno" class="2">' + j + '</td>';
                        results += '<th data-title="From"><select class="form-control milktype" name="milktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                        results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + dispatch_subdetails[j].cellname + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                        results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty(kgs)" onkeypress="return isFloat(event);" name="Qtykg" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                        results += '<td data-title="Qty" style="text-align:center;"  class="3"><input class="form-control" type="text" disabled="disabled" placeholder="Qty(Ltrs)" onkeypress="return isFloat(event);"  name="Qtyltr" id="txtltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                        results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="FAT" name="FAT" value="" onkeypress="return isFloat(event);" id="txtfat" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                        results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF" name="SNF" value="" onkeypress="return isFloat(event);" id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                        results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="CLR" onkeyup="LRChange(this);" onkeypress="return isFloat(event);" id="txtclr" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
                    }
                }
                results += '</table></div>';
                $("#div_vendordata").html(results);
                for (var i = 0; i < dispatch_subdetails.length; i++) {
                    if (dcno == dispatch_subdetails[i].desprefno) {
                        $('#tbl_milk_details> tbody > tr').each(function () {
                            var cell = $(this).find('[name="CellName"]').val();
                            if (cell == dispatch_subdetails[i].cellname) {
                                $('.milktype').append("").val(dispatch_subdetails[i].milktype);     //fill dropdown vales
                                //$('.milktype').prepend("").val(dispatch_subdetails[i].milktype); 
                                //$(this).find('select[name*="milktype"] :selected').val(dispatch_subdetails[i].milktype);
                                $(this).find('[name="CellName"]').val(dispatch_subdetails[i].cellname);
                                $(this).find('[name="Qtykg"]').val(dispatch_subdetails[i].qtykgs);
                                $(this).find('[name="Qtyltr"]').val(dispatch_subdetails[i].qtyltr);
                                $(this).find('[name="FAT"]').val(dispatch_subdetails[i].fat);
                                $(this).find('[name="SNF"]').val(dispatch_subdetails[i].snf);
                                $(this).find('[name="CLR"]').val(dispatch_subdetails[i].clr);
                            }
                        });
                    }
                }
            }
            var results1 = '<div    style="overflow:auto;"><table id="tbl_milk_vendor_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results1 += '<thead><tr><th scope="col">Sno</th><th scope="col">MilkType</th><th scope="col">Cell Name</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th></tr></thead></tbody>';
            for (var j = 0; j < dispatch_subdetails.length; j++) {
                if (dcno == dispatch_subdetails[j].desprefno) {
                    results1 += '<td data-title="Sno" class="2">' + j + '</td>';
                    results1 += '<th data-title="From"><select class="form-control milktypes" name="milktype" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ><option  value="Buffalo">Buffalo</option><option  value="Cow">Cow</option></select></td>';
                    results1 += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + dispatch_subdetails[j].cellname + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                    results1 += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" onkeypress="return isFloat(event);" id="txtkg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results1 += '<td data-title="Qty" style="text-align:center;"  class="3"><input class="form-control" type="text" disabled="disabled" placeholder="Qty(Ltrs)" onkeypress="return isFloat(event);"  name="Qtyltr" id="txtltr" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results1 += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="FAT" name="FAT" value="" id="txtfat" onkeypress="return isFloat(event);" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results1 += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF" name="SNF" value=""  id="txtsnf" onkeypress="return isFloat(event);" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results1 += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="CLR" onkeyup="LRChange(this);" id="txtclr" onkeypress="return isFloat(event);" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
                }
            }
            results1 += '</table></div>';
            $("#div_milkdata").html(results1);
            for (var i = 0; i < dispatch_subdetails.length; i++) {
                if (dcno == dispatch_subdetails[i].desprefno) {
                    $('#tbl_milk_vendor_details> tbody > tr').each(function () {
                        var cell = $(this).find('[name="CellName"]').val();
                        if (cell == dispatch_subdetails[i].cellname) {
                            $('.milktypes').append("").val(dispatch_subdetails[i].milktype);      //fill dropdown vales
                            //$(this).find('select[name*="milktype"] :selected').val(dispatch_subdetails[i].milktype);
                            $(this).find('[name="CellName"]').val(dispatch_subdetails[i].cellname);
                            $(this).find('[name="Qtykg"]').val(dispatch_subdetails[i].qtykgs);
                            $(this).find('[name="Qtyltr"]').val(dispatch_subdetails[i].qtyltr);
                            $(this).find('[name="FAT"]').val(dispatch_subdetails[i].fat);
                            $(this).find('[name="SNF"]').val(dispatch_subdetails[i].snf);
                            $(this).find('[name="CLR"]').val(dispatch_subdetails[i].clr);
                        }
                    });
                }
            }
            $("#div_Deptdata").hide();
            $("#vehmaster_fillform").show();
            $('#showlogs').hide();
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
            Direct Sale<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Direct Sale</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Direct Sale Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                    <%--<input id="btn_addDept" type="button" name="submit" value='Add Direct Sale' class="btn btn-success" />--%>
                     <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="adddirectsale()"></span> <span  id="btn_addDept" onclick="adddirectsale()">Add Direct Sale</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='vehmaster_fillform' style="display: none;">
                <div align="center">
                    <table >
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px;">
                                <label>
                                    Datetime</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                   Ref DC No
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_dcNo" class="form-control" name="vendorcode"
                                    placeholder="Enter Ref DC No"><label id="lbl_code_error_msg" class="errormessage">* Please
                                        Enter Ref DC No</label>
                            </td>
                            <td style="width:5px;"></td>
                            <td>
                                <%--<input id='Button1' type="button" class="btn btn-success" name="submit" value='Get Details'
                                    onclick="Get_DespatchDetails_click()" />--%>
                                    <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="Get_DespatchDetails_click()"></span> <span  id="Span1" onclick="Get_DespatchDetails_click()">Get Details</span>
                          </div>
                          </div>
                            </td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    From Vendor</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                                <select id="slct_Source_Name" class="form-control">
                                    <option selected disabled value="Select Vendor Name">Select Vendor Name</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    Vehicle No</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_vno" class="form-control" name="vendorcode"
                                    placeholder="Enter Vehicle No" readonly="true">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    To Vendor</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                                <select id="slct_dest_Name" class="form-control">
                                    <option selected disabled value="Select Vendor Name">Select Vendor Name</option>
                                </select>
                            </td>
                        </tr>

                        <tr class="newdcno" style="display:none;">
                            <td>
                                <label>
                                    DC NO</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                               <input type="text" maxlength="45" id="txt_newdcno" class="form-control" name="vendorcode"
                                    placeholder="Enter DCNO">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                    </div>
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk Details</h3>
                        </div>
                        <div id="div_milkdata">
                        </div>
                    </div>
                    <div class="box box-success">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk Acknowledge Details</h3>
                        </div>
                        <div id="div_vendordata">
                        </div>
                    </div>
                    <div style="text-align: center;">
                        <table align="center">
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Remarks</label>
                                </td>
                                <td colspan="6">
                                    <textarea rows="3" cols="45" id="txt_Remarks" class="form-control" maxlength="200"
                                        placeholder="Enter Remarks"></textarea>
                                        <br />
                                    <%--   <textarea id="txt_Remarks" class="form-control" rows="5" cols="45" placeholder="Remarks" />--%>
                                </td>
                            </tr>
                            <tr hidden>
                                <td>
                                    <label id="lbl_sno">
                                    </label>
                                </td>
                            </tr>
                        </table>
                        <%--<input id='save_milktransactions' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_despatch_entry_click()" />
                        <input id='close_vehmaster' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearvalues()" />
                        <input id='btnPrint' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />--%>
                        <div  style="padding-left: 44%;padding-top:0%;">
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
                                <span class="glyphicon glyphicon-remove" id='close_vehmaster1' onclick="clearvalues()"></span><span id='close_vehmaster' onclick="clearvalues()">Close</span>
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
