<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="Gheepktwisequalitytesting.aspx.cs" Inherits="Gheepktwisequalitytesting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">
     $(function () {
         $('#btn_addghee').click(function () {
             $('#ghee_fillform').css('display', 'block');
             $('#showlogs').css('display', 'none');
             $('#div_gheedata').hide();
             clearvalues();
             get_gheeproduction_details();
//             Getgheedetails();
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

         $('#close_gheemaster').click(function () {
             $('#ghee_fillform').css('display', 'none');
             $('#showlogs').css('display', 'block');
             $('#div_gheedata').show();
             clearvalues();
         });
         get_gheeproductqualitytesting_details();
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
        $(document).ready(function ()
        {
            $("#txt_dcno,#txt_Inwardno").keydown(function (event)
            {
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
        function callHandler(d, s, e)
        {
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
        function save_ghee_wise_qualitytesting_click()
        {
            var date = document.getElementById('txt_date').value;
            var qco = document.getElementById('txt_qco').value;
            var Remarks = document.getElementById('txt_Remarks').value;
            var Chemist = document.getElementById('txt_Chemist').value;
//            var SourceID = document.getElementById('slct_Source_Name').value;
            var sampleno = document.getElementById('txtsample').value;
            var btnvalue = document.getElementById('save_gheetransactions').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
            
//            if (date == "") {
//                alert("Please Select Date");
//                return false;
//            }
//            if (SourceID == "" || SourceID == "Select Batch") {
//                alert("Select plant name");
//                return false;
//            }
//            if (Chemist == "") {
//                alert("Enter Chemist name");
//                return false;
//            }
//            if (qco == "") {
//                alert("Enter qco name");
//                return false;
//            }
            
            var gheequalityDetailsarray = [];
            $('#tbl_ghee_details> tbody > tr').each(function ()
            {
                var date = $(this).find('[name="DATE"]').val();
                var packsize = $(this).find('select[name="packsize"] :selected').val();
                var productid = $(this).find('[name="pid"]').val();
                var batchno = $(this).find('[name="BATCHNO"]').val();
                var usebydate = $(this).find('[name="USEBYDATE"]').val();
                var mrp = $(this).find('[name="MRP"]').val();
                var structure = $(this).find('[name="STRUCTURE"]').val();
                var ot = $(this).find('[name="OT"]').val();
                var temp = $(this).find('[name="TEMP"]').val(); 
                var acidity = $(this).find('[name="ACIDITY"]').val();
                var remarks = $(this).find('[name="REMARKS"]' ).val(); 
               
                if (mrp == "") {
                }
                else {
                    gheequalityDetailsarray.push({ 'date': date, 'productid': productid, 'packsize': packsize, 'batchno': batchno, 'usebydate': usebydate, 'mrp': mrp,
                        'structure': structure, 'ot': ot, 'temp': temp, 'acidity': acidity, 'remarks': remarks
                    });
                }
            });
            if (gheequalityDetailsarray.length == "0") {
                alert("Please enter packsize,batchno details");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_ghee_wise_qualitytesting_click', 'sampleno': sampleno, 'qco': qco, 'date': date, 'remarks': Remarks, 'chemist': Chemist, 'sno': sno, 'btnvalue': btnvalue, 'gheequalityDetailsarray': gheequalityDetailsarray
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        $('#ghee_fillform').css('display', 'none');
                        $('#showlogs').css('display', 'block');
                        $('#div_gheedata').show();
                        clearvalues();
                        get_gheeproductqualitytesting_details();
                    }
                    else {
                    }
                };
                var e = function (x, h, e)
                {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                CallHandlerUsingJson(data, s, e);
            }
        }
        function clearvalues()
        {
            document.getElementById('txt_date').value = "";
            document.getElementById('txt_qco').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('txt_Chemist').value = "";
//            document.getElementById('slct_Source_Name').selectedIndex = 0;
           // get_gheeproduct_details();
        }
        function CallHandlerUsingJson(d, s, e)
        {
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


        function get_gheeproduction_details() {
            var data = { 'op': 'get_gheeproduction_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        Getgheedetails(msg);
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
        function Getgheedetails(msg)
        {
            var names = 11;
            var results = '<div    style="overflow:auto;"><table id="tbl_ghee_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">PRODUCT NAME</th><th scope="col">PACK SIZE</th><th scope="col">BATCH NO</th><th scope="col">Manufacture Date</th><th scope="col">MRP</th><th scope="col">AppeaRance</th><th scope="col">OT</th><th scope="col">TEMP</th><th scope="col">FFA</th><th scope="col">REMARKS</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < msg.length; i++) {
                results += '<td data-title="Sno" class="0">' + i + '</td>';
                results += '<td data-title="Product" style="text-align:center;" class="1"><input class="form-control" type="text"  placeholder="Product Name" id="txtproductname" name="productname" value="' + msg[i].productname + '" style="font-size:12px;padding: 0px 5px;height:30px;"></td>';
                results += '<td data-title="packsize" style="text-align:center;" class="2"><select class="form-control" type="text" placeholder="Packet Size" name="packsize" id="pcktsize" value="" style="font-size:12px;padding: 0px 5px;height:30px;"><option  value="50 ML">Loose</option><option  value="50 ML">50 ML</option><option  value="100 ML">100 ML</option><option  value="180 ML">180 ML</option><option  value="200 ML">200 ML</option><option  value="500 ML">500 ML</option><option  value="1000 ML">1000 ML</option><option  value="1 KG">1 KG</option><option  value="5 KG TIN">5 KG TIN</option><option  value="15 KG TIN">15 KG TIN</option></select></td>';
                results += '<td data-title="BATCHNO" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Batchno" name="BATCHNO" id="txtfat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="USEBYDATE" style="text-align:center;" class="4"><input class="form-control" type="date" placeholder="Usebydate" name="USEBYDATE" value="" id="txtsnf" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="MRP" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Mrp" id="txtclr" name="MRP" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="STRUCTURE" style="text-align:center;" class="6 "><input class="form-control" type="text" placeholder="Structure" name="STRUCTURE" value="" id="txttemp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="OT" style="text-align:center;" class="7"><input class="form-control" type="text" placeholder="OT" name="OT"  id="txtacidity"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="TEMP" style="text-align:center;width:65px;" class="8"><input class="form-control" name="TEMP" placeholder="Temp" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ></td>';
                results += '<td data-title="ACIDITY" style="text-align:center;" class="9"><input class="form-control" type="text" placeholder="Enter FFA" name="ACIDITY" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="REMARKS" style="text-align:center;" class="10"><input class="form-control" name="REMARKS"  value="" placeholder="Remarks" style="width:100%; "font-size:12px;padding: 0px 5px;height:30px;" ></td>';
                results += '<td style="display:none" data-title="pid" style="text-align:center;" class="11"><input class="form-control" type="text" placeholder="pid" name="pid" id="txtpid" value="' + msg[i].productid + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
            }
            results += '</table></div>';
            $("#div_gheevendordata").html(results);
        }
        function getme(thisid) {

        }
        function get_gheeproductqualitytesting_details() {
            var data = { 'op': 'get_gheeproductqualitytesting_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillqualitydetails(msg);
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
        function fillqualitydetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Product Name</th><th scope="col">Packet Size</th><th scope="col">Sample No</th><th scope="col">Used By Date</th><th scope="col">Temp</th><th scope="col">MRP</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].packetsize + '</td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].sampleno + '</td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].usedbydate + '</td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].ptemp + '</td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].pmrp + '</td>';
                results += '<td style="display:none" class="2">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].sno + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_gheedata").html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<section class="content-header">
        <h1>
           GHEE Section Quality Testing <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">GHEE Section Quality Testing</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>GHEE Section Quality Testing Entry
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addghee" type="button" name="submit" value='Add Qualtity Testing' class="btn btn-success" />
                </div>
                <div id="div_gheedata">
                </div>
                <div id='ghee_fillform' style="display: none;">
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
                                    placeholder="Enter Date" />
                            </td>
                            <td>
                            
                            </td>
                            <td style="height: 40px;">
                                <label>
                                   Sampleno</label>
                            </td>
                            <td>
                                <input id="txtsample" class="form-control" type="text" name="vendorcode"
                                    placeholder="Enter Sample No" />
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                <label>
                                    Product Name</label>
                            </td>
                            <td>
                                <select id="slct_Source_Name" class="form-control">
                                </select>
                            </td>
                             <td style="width: 6px;">
                            </td>
                        </tr>--%>
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
                        <div id="div_gheevendordata">
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
                        <input id='save_gheetransactions' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_ghee_wise_qualitytesting_click()" />
                        <input id='close_gheemaster' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearvalues()" />
                        <input id='btnPrint' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />
                    </div>
                </div>
            </div>
    </section>

</asp:Content>

