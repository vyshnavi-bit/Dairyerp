<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="curdpktwisequalitytesting.aspx.cs" Inherits="curdpktwisequalitytesting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btn_addDept').click(function () {
                $('#curpacket_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
                clearvalues();
                get_curdproduction_details();
                get_product_details();
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
                $('#curpacket_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                clearvalues();
            });
            get_curdproductqualitytesting_details();
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


        function get_product_details() {
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
//                        fillcurdproducts(msg);
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
//        function fillcurdproducts(msg) {
//            var data = document.getElementById('slct_Product_Name');
//            var length = data.options.length;
//            document.getElementById('slct_Product_Name').options.length = null;
//            var opt = document.createElement('option');
//            opt.innerHTML = "Select productname";
//            opt.value = "Select productname";
//            opt.setAttribute("selected", "selected");
//            opt.setAttribute("disabled", "disabled");
//            opt.setAttribute("class", "dispalynone");
//            data.appendChild(opt);
//            for (var i = 0; i < msg.length; i++) {
//                if (msg[i].departmentid == "1" && msg[i].batchid != "16" && msg[i].batchid != "13") {
//                    if (msg[i].batch != null) {
//                        var option = document.createElement('option');
//                        option.innerHTML = msg[i].productname;
//                        option.value = msg[i].productid;
//                        data.appendChild(option);
//                    }
//                }
//                else {
//                    if (msg[i].departmentid == "14" && msg[i].batchid == "16") {
//                        if (msg[i].batch != null) {
//                            var option = document.createElement('option');
//                            option.innerHTML = msg[i].productname;
//                            option.value = msg[i].productid;
//                            data.appendChild(option);
//                        }
//                    }
//                }
//            }
//        }
//            var ProductName = document.getElementById('slct_Product_Name').value;

        function save_curd_packetwise_qualitytesting_click() {
            var date = document.getElementById('txt_date').value;
            var qco = document.getElementById('txt_qco').value;
            var Remarks = document.getElementById('txt_Remarks').value;
            var Chemist = document.getElementById('txt_Chemist').value;
            var btnvalue = document.getElementById('btnvalue').value;
            var sample = document.getElementById('txt_sample').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
//            if (date == "") {
//                alert("Please Select Date");
//                return false;
//            }
//            if (ProductName == "") {
//                alert("Enter ProductName");
//                return false;
//            }
            var curdpktqualityarray = [];
            $('#tbl_curdpacket_details> tbody > tr').each(function () {
                var date = $(this).find('[name="Date"]').val();
                var packsize = $(this).find('select[name="packetsize"] :selected').val();
                var productid = $(this).find('[name="pid"]').val();
                var batchno = $(this).find('[name="BatchNo"]').val();
                var mrp = $(this).find('[name="Mrp"]').val();
                var structure = $(this).find('[name="Structure"]').val();
                var ot = $(this).find('[name="OT"]').val();
                var acidity = $(this).find('[name="Acidity"]').val();
                var temp = $(this).find('[name="TEMP"]').val();
                var usedbydate = $(this).find('[name="USEBYDATE"]').val();

                if (mrp == "") {
                }
                else {

                    curdpktqualityarray.push({ 'date': date, 'packetsize': packsize, 'productid': productid, 'batchno': batchno, 'mrp': mrp, 'structure': structure, 'ot': ot, 'acidity': acidity, 'temp': temp, 'usedbydate': usedbydate
                    });
                }
            });
            if (curdpktqualityarray.length == "0") {
                alert("Please enter packetsize");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_curd_packetwise_qualitytesting_click',  'qco': qco, 'Chemist': Chemist, 'date': date, 'sample': sample, 'remarks': Remarks, 'sno': sno, 'btnvalue': btnvalue, 'curdpktqualityarray': curdpktqualityarray
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        $('#curpacket_fillform').css('display', 'none');
                        $('#showlogs').css('display', 'block');
                        $('#div_Deptdata').show();
                        clearvalues();
                        get_curdproductqualitytesting_details();
                        //get_Batchs();
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
//            document.getElementById('slct_Product_Name').selectedIndex = 0;
            //document.getElementById('slct_milk_type').selectedIndex = 0;
            get_curdproduction_details();
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

        function get_curdproduction_details() {
            var data = { 'op': 'lab_curdproduction_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        Getcurddetails(msg);
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
        function Getcurddetails(msg) {
            var names = 10;
            var results = '<div    style="overflow:auto;"><table id="tbl_curdpacket_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Product Name</th><th scope="col">Pack Size</th><th scope="col">BatchNo</th><th scope="col">UsedByDate</th><th scope="col">Mrp</th><th scope="col">Structure</th><th scope="col">OT</th><th scope="col">Temp</th><th scope="col">Acidity% / Moisture</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < msg.length; i++) {
                results += '<td data-title="Sno" class="0">' + (i + 1) + '</td>';
                results += '<td data-title="Product" style="text-align:center;" class="1"><input class="form-control" type="text"  placeholder="PacketSize" id="txtproductname" readonly name="packetsize" value="' + msg[i].productname + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="PacketSize" style="text-align:center;" class="2"><select class="form-control" type="text"  placeholder="PacketSize" id="txtpacketsize" name="packetsize" value="' + msg[i].productid + '" style="font-size:12px;padding: 0px 5px;height:30px; width: 70px"><option  value="100 ML">100 ML</option><option  value="175 ML">175 ML</option><option  value="180 ML">180 ML</option><option  value="200 ML">200 ML</option><option  value="500 ML">500 ML</option><option  value="200 GM">200 GM</option><option  value="500 GM">500 GM</option><option  value="1 KG">1 KG</option><option  value="5 KG">5 KG</option><option  value="10 KG">10 KG</option><option  value="20 KG">20 KG</option><option  value="40 KG">40 KG</option><option  value="10 KG BUCKET">10 KG BUCKET</option><option  value="CURD MILK-AMARARAAJA">CURD MILK-AMARARAAJA</option><option  value="STD CURD BUCKETS">STD CURD BUCKETS</option></select></td>';
                results += '<td data-title="BatchNo" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="BatchNo" name="BatchNo" id="txtBatchNo" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="USEBYDATE" style="text-align:center;" class="4"><input class="form-control" type="date" placeholder="Usebydate" name="USEBYDATE" value="" id="txtusedbydate" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Mrp" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Mrp" name="Mrp" value="" id="txtMrp" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Structure" id="txstructure"  class="6"><input class="form-control" type="text" placeholder="Structure"   id="txtStructure" name="Structure" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="OT" style="text-align:center;" class="7"><input class="form-control" type="text" placeholder="OT" name="OT" value="" id="txtOT" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Temp" style="text-align:center;" class="8"><input class="form-control" type="text" placeholder="Temp" name="TEMP"  id="txtTemp"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Acidity" style="text-align:center;" class="9"><input class="form-control" type="text" placeholder="Acidity" name="Acidity"  id="txtAcidity"  value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td style="display:none" data-title="pid" style="text-align:center;" class="10"><input class="form-control" type="text" placeholder="pid" name="pid" id="txtpid" value="' + msg[i].productid + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
            }
            results += '</table></div>';
            $("#div_curdpacket").html(results);

        }
        function getme(thisid) {

        }
        function get_curdproductqualitytesting_details() {
            var data = { 'op': 'get_curd_packetwise_qualitytesting_details' };
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
                results += '<td scope="row" class="2" style="text-align:center;">' + msg[i].packetsize + '</td>';
                results += '<td scope="row" class="3" style="text-align:center;">' + msg[i].sampleno + '</td>';
                results += '<td scope="row" class="4" style="text-align:center;">' + msg[i].usedbydate + '</td>';
                results += '<td scope="row" class="5" style="text-align:center;">' + msg[i].ptemp + '</td>';
                results += '<td scope="row" class="6" style="text-align:center;">' + msg[i].pmrp + '</td>';
                results += '<td style="display:none" class="7">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="8">' + msg[i].sno + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
           CURD Section Qualtity Testing <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">CURD Section Qualtity Testing</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>CURD Section Qualtity Testing Entry
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addDept" type="button" name="submit" value='Add CurdPacket Details' class="btn btn-success" />
                </div>
                <div id="div_Deptdata">
                </div>
                
                <div id='curpacket_fillform' style="display: none;">
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
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <label>
                                    Sample No</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input id="txt_sample" type="text" class="form-control" name="vendorcode" placeholder="Sample No" />
                            </td>
                        </tr>
                            <%--<tr hidden>
                                <td>
                                    <label>
                                        Product Name</label>
                                </td>
                                <td>
                                    <select id="slct_Product_Name" class="form-control">
                                    </select>
                                </td>
                                    <td style="width: 6px;">
                                </td>
                            </tr>--%>
                            <tr>
                            <td>
                            </td>
                            <td>
                            <br/>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                    </div>
                    <div class="box box-danger">
                        <div class="box-header with-border">
                           <%-- <h3 class="box-title" id="lbldetals" style="display:none;">--%>
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Packing Details</h3>
                        </div>
                        <div id="div_curdpacket">
                        </div>
                    </div>
                    <div style="text-align: center;">
                    <div style="padding-left:200px;">
                        
                        <table align="center">
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
                            <tr hidden>
                                <td>
                                    <label id="lbl_sno">
                                    </label>
                                </td>
                            </tr>
                           
                        </table>
                        </div>
                        <input id='btnvalue' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_curd_packetwise_qualitytesting_click()" />
                        <input id='close_vehmaster' type="button" class="btn btn-danger" name="Clear" value='Clear'
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
