<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="batchwisequalitytesting.aspx.cs" Inherits="batchwisequalitytesting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btn_addBatch').click(function () {
                $('#batch_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_batchdata').hide();
                clearvalues();
                Getmilkdetails();
                get_Batchs();
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

            $('#close_batchs').click(function () {
                $('#batch_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_batchdata').show();
                clearvalues();
            });
            get_batchqualitytesting_details();
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
        function get_Batchs() {
            var data = { 'op': 'get_batch_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillBatchs(msg);
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
        function fillBatchs(msg) {
            var data = document.getElementById('slct_Source_Name');
            var length = data.options.length;
            document.getElementById('slct_Source_Name').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Batch";
            opt.value = "Select Batch";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].departmentid == "5" || msg[i].departmentid == "11") {
                    if (msg[i].batchtype != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].batchtype;
                        option.value = msg[i].batchid;
                        data.appendChild(option);
                    }
                }
            }
        }
        function save_batch_wise_qualitytesting_click() {
            var date = document.getElementById('txt_date').value;
            var qco = document.getElementById('txt_qco').value;
            var Remarks = document.getElementById('txt_Remarks').value;
            var Chemist = document.getElementById('txt_Chemist').value;
            var SourceID = document.getElementById('slct_Source_Name').value;
            var btnvalue = document.getElementById('save_batchdetails').value;
            var sample = document.getElementById('txt_sample').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
            if (date == "") {
                alert("Please Select Date");
                return false;
            }
            if (SourceID == "" || SourceID == "Select Batch") {
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
            var MilkqualityDetailsarray = [];
            $('#tbl_milk_details> tbody > tr').each(function () {
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
                    MilkqualityDetailsarray.push({'Qtykg': Qtykg, 'Qtyltr': Qtyltr, 'fat': fat, 'snf': snf, 'temp': temp,
                        'acidity': acidity, 'clr': clr, 'cob': cob, 'ot': ot, 'hs': hs, 'phosps': phosps, 'alcohol': alcohol, 'neutralizers': neutralizers, 'mbrt': MBRT
                    });
                }
            });
            if (MilkqualityDetailsarray.length == "0") {
                alert("Please enter fat,snf details");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_batch_wise_qualitytesting_click', 'qco': qco, 'date': date, 'remarks': Remarks, 'chemist': Chemist, 'sourceid': SourceID, 'sample': sample, 'sno': sno, 'btnvalue': btnvalue, 'MilkqualityDetailsarray': MilkqualityDetailsarray
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        $('#batch_fillform').css('display', 'none');
                        $('#showlogs').css('display', 'block');
                        $('#div_batchdata').show();
                        clearvalues();
                        Getmilkdetails();
                        get_Batchs();
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
            var names = 10;
            var results = '<div    style="overflow:auto;"><table id="tbl_curd_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Qty(Kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th><th scope="col">TEMP</th><th scope="col">Acidity</th><th scope="col">COB</th><th scope="col">OT</th><th scope="col">HS</th><th scope="col">Phosps</th><th scope="col">Alcohol%</th><th scope="col">MBRT</th><th scope="col">Neutralizers</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < 1; i++) {
                results += '<td data-title="Sno" class="2">' + i + '</td>';
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
            $("#div_batchdetails").html(results);
        }

        function get_batchqualitytesting_details() {
            var data = { 'op': 'get_batchqualitytesting_details' };
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
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Date</th></th><th scope="col">Qty(kgs)</th><th scope="col">fat</th><th scope="col">snf</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td class="11">' + msg[i].doe + '</td>';
                results += '<td data-title="Capacity" class="10">' + msg[i].OutwordQuantitykgs + '</td>';
                results += '<td data-title="Capacity" class="8">' + msg[i].fat + '</td>';
                results += '<td data-title="Capacity" class="9">' + msg[i].snf + '</td>';
                results += '<td style="display:none" class="4">' + msg[i].sectionid + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].transno + '</td>';
            }
            results += '</table></div>';
            $("#div_batchdata").html(results);
        }
        function getme(thisid) {
          
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
           Batch Wise Qualtity Testing <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Batch Wise Qualtity Testing</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Batch Wise Qualtity Testing Entry
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addBatch" type="button" name="submit" value='Add Qualtity Testing' class="btn btn-success" />
                </div>
                <div id="div_batchdata">
                </div>
                <div id='batch_fillform' style="display: none;">
                <div style="padding-left:200px;">
                    <table align="center">
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                   Date time</label>
                            </td>
                            <td>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Sample No</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input id="txt_sample" type="text" class="form-control" name="vendorcode" placeholder="Sample No">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Batch Name</label>
                            </td>
                            <td>
                                <select id="slct_Source_Name" class="form-control" onchange="Onchage_Source_Name();">
                                </select>
                            </td>
                             <td style="width: 6px;">
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
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Details</h3>
                        </div>
                        <div id="div_batchdetails">
                        </div>
                    </div>
                    <div style="text-align: center;">
                    <div style="padding-left:200px;">
                        <table align="center">
                            <tr>
                                <td style="height: 40px;">
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
                        <input id='save_batchdetails' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_batch_wise_qualitytesting_click()" />
                        <input id='close_batchs' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearvalues()" />
                        <input id='btnPrint' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />
                    </div>
                </div>
            </div>
            </div>
    </section>
</asp:Content>

