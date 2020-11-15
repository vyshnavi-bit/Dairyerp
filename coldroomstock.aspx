<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="coldroomstock.aspx.cs" Inherits="coldroomstock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btn_addDept').click(function () {
                $('#coldroom_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#diveditdata').css('display', 'none');
                $('#div_Deptdata').hide();
                //fillcoldroomdetails();
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
                $('#txt_date').val(yyyy + '-' + mm + '-' + dd);
                get_Batchs();
            });
            $('#btn_close').click(function () {
                $('#coldroom_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#diveditdata').css('display', 'none');
                $('#div_Deptdata').show();
                Clearvalues();
                get_coldroomstock_details();
            });
            $('#btn_ceclose').click(function () {
                $('#coldroom_fillform').css('display', 'none');
                $('#diveditdata').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                //Clearvalues();
                get_coldroomstock_details();
            });
            get_coldroomstock_details();
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
        $(function () {
            var today = new Date();
            var dd = today.getDate() - 1;
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
            $('#txt_getdatadate').val(yyyy + '-' + mm + '-' + dd);
            get_coldroomstock_details();
        });
        function addcoldroomtock() {
            $('#coldroom_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#diveditdata').css('display', 'none');
            $('#div_Deptdata').hide();
            //fillcoldroomdetails();
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
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd );
            get_Batchs();
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
            var data = document.getElementById('slct_Batch');
            var length = data.options.length;
            document.getElementById('slct_Batch').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Batch";
            opt.value = "Select Batch";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].departmentid == "5") {
                    if (msg[i].batchtype != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].batchtype;
                        option.value = msg[i].batchid;
                        data.appendChild(option);
                    }
                }
            }
        }
        function fillcoldroomdetails() {
            var date = document.getElementById('txt_date').value;
            var data = { 'op': 'get_Batch_Productstock_details', 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $("#divFillScreen").show();
                        var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Opening Balance</th><th scope="col" style="font-weight: 700;">Production(Quantity)</th><th scope="col" style="font-weight: 700;">Dispatch</th><th scope="col" style="font-weight: 700;">Cutting</th><th scope="col" style="font-weight: 700;">Transfer</th><th scope="col" style="font-weight: 700;">ClosingBalance</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].departmentid == "2") {
                                results += '<tr>';
                                results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + (i + 1) + '</span></th>';
                                results += '<td><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></td>';
                                results += '<td><input id="txt_openingbalance" class="form-control" value="' + msg[i].openingbalnce + '" type="number" name="opening"placeholder="Enter opening qty" readonly></td>';
                                results += '<td><input id="txt_productionquantity" class="form-control" value="' + msg[i].productionqty + '" type="number" name="production"placeholder="Enter production qty" readonly></td>';
                                results += '<td><input id="txt_dispatchqty" class="form-control" value="' + msg[i].dispatch + '" type="number" name="dispatch"placeholder="Enter dispatch qty" readonly></td>';
                                results += '<td><input id="txt_cutting" class="form-control" value="0" type="number" name="cutting" placeholder="Enter cutting"></td>';
                                results += '<td><input id="txt_transfer" class="form-control" value="0" type="number"  onkeyup="closingschange(this);" name="transfer" placeholder="Enter Transfer"></td>';
                                results += '<td><input id="txt_closingbalance" class="form-control" value="0" type="number"  name="closingbalance" placeholder="Enter Closing Balance" readonly></td>';
                                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="number" name="productid" value="' + msg[i].productid + '"></td></tr>';
                            }
                        }
                        results += '</table></div>';
                        $("#divFillScreen").html(results);
                    }
                    else {
                        $("#divFillScreen").hide();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function closingschange(qtyid) {
            var openingbalance = 0;
            var dispatchqty = 0;
            var cutting = 0;
            var transfer = 0;
            var closingbalance = 0;

            openingbalance = $(qtyid).closest("tr").find('#txt_openingbalance').val();
            openingbalance = parseFloat(openingbalance);
            dispatchqty = $(qtyid).closest("tr").find('#txt_dispatchqty').val();
            dispatchqty = parseFloat(dispatchqty);
            cutting = $(qtyid).closest("tr").find('#txt_cutting').val();
            cutting = parseFloat(cutting);
            transfer = $(qtyid).closest("tr").find('#txt_transfer').val();
            transfer = parseFloat(transfer);

            closingbalance = openingbalance - dispatchqty;
            closingbalance = closingbalance - cutting;
            closingbalance = closingbalance - transfer;
            $(qtyid).closest("tr").find('#txt_closingbalance').val(closingbalance);
        }
        function save_coldroomstock_click() {
            var btnvalue = document.getElementById('save_batchdetails').innerHTML;
            var remarks = document.getElementById('txt_Remarks').value;
            var sno = document.getElementById('lbl_sno').value;
            var date = document.getElementById('txt_date').value;
            if (date == "") {
                alert("Please Select Date");
                return false;
            }
            var rows = $("#table_shift_wise_details tr:gt(0)");
            var cold_room_stock_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_dispatchqty').val() != "") {
                    cold_room_stock_details.push({ productid: $(this).find('#hdn_productid').val(), disp_qty: $(this).find('#txt_dispatchqty').val(), cutting: $(this).find('#txt_cutting').val(), openingbalance: $(this).find('#txt_openingbalance').val(), transfer: $(this).find('#txt_transfer').val(), closingbalance: $(this).find('#txt_closingbalance').val() });
                }
            });
            if (cold_room_stock_details.length == 0) {
                alert("Please enter Dis Patch quantity");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_coldroomstock_click', 'cold_room_stock_details': cold_room_stock_details, 'remarks': remarks, 'btnvalue': btnvalue, 'sno': sno, 'date': date };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_coldroomstock_details();
                            $('#coldroom_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                            $('#div_Deptdata').show();
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
        function Clearvalues() {
            $('#coldroom_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#diveditdata').css('display', 'none');
            $('#div_Deptdata').show();
            Clearvalues();
            get_coldroomstock_details();
        }
        function productchange() {
            var productid = document.getElementById('slct_product').value;
            var data = { 'op': 'get_productqty_details', 'productid': productid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('.clsqty').value = msg[i].quantity;
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

        function get_coldroomstock_details() {
            var getdatadate = document.getElementById('txt_getdatadate').value;
            var data = { 'op': 'get_coldroomstock_details', 'getdatadate': getdatadate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcoldroomstocks(msg);
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
        function fillcoldroomstocks(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">Total Production</th><th scope="col" style="font-weight: bold;">Dispatch</th><th scope="col" style="font-weight: bold;">Cutting</th<th scope="col" style="font-weight: bold;">Transfer</th><th scope="col" style="font-weight: bold;">ClosingBalance</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td  class="2" style="text-align:center;">' + msg[i].openingbalance + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].dispatch + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].cutting + '</td>';
                results += '<td  class="9" style="text-align:center;">' + msg[i].transfer + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].closingbalance + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].doe + '</td>';
                results += '<td style="display:none" class="7">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="8">' + msg[i].sno + '</td>';
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
            var productname = $(thisid).parent().parent().children('.1').html();
            var openingbalance = $(thisid).parent().parent().children('.2').html();
            var dispatch = $(thisid).parent().parent().children('.3').html();
            var cutting = $(thisid).parent().parent().children('.4').html();
            var closingbalance = $(thisid).parent().parent().children('.5').html();
            var doe = $(thisid).parent().parent().children('.6').html();
            var productid = $(thisid).parent().parent().children('.7').html();
            var sno = $(thisid).parent().parent().children('.8').html();
            var transfer = $(thisid).parent().parent().children('.9').html();

            document.getElementById('hdn_cesno').value = sno;
            document.getElementById('hdn_ceoldcutting').value = cutting;
            document.getElementById('hdn_ceproductid').value = productid;
            document.getElementById('txt_cecuttingmilk').value = cutting;
            document.getElementById('txt_cedispatch').value = dispatch;
            document.getElementById('hdn_ceclosingbal').value = closingbalance;
            document.getElementById('txt_ceob').value = openingbalance;
            document.getElementById('txt_ceproductname').value = productname;
            document.getElementById('txt_cetranfer').value = transfer;
            document.getElementById('hdn_ceoldtransfer').value = transfer;
            document.getElementById('btn_cemodify').value = "Modify";

            $('#coldroom_fillform').css('display', 'none');
            $('#showlogs').css('display', 'none');
            $('#diveditdata').css('display', 'block');
            $('#div_Deptdata').hide();
        }

        function modify_cold_room_click() {
            var sno = document.getElementById('hdn_cesno').value;
            var oldcuttinmilk = document.getElementById('hdn_ceoldcutting').value;
            var productid = document.getElementById('hdn_ceproductid').value;
            var openingbalnace = document.getElementById('txt_ceob').value;
            var dispatch = document.getElementById('txt_cedispatch').value;
            var cutting = document.getElementById('txt_cecuttingmilk').value;
            var productname = document.getElementById('txt_ceproductname').value;
            var closingbalance = document.getElementById('hdn_ceclosingbal').value;
            var transfer = document.getElementById('txt_cetranfer').value;
            var oldtransfer = document.getElementById('hdn_ceoldtransfer').value;

            var btnvalue = document.getElementById('btn_cemodify').value;
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'modify_cold_room_click', 'btnvalue': btnvalue, 'productname': productname, 'cutting': cutting, 'dispatch': dispatch, 'openingbalnace': openingbalnace,
                    'productid': productid, 'oldcuttinmilk': oldcuttinmilk, 'sno': sno, 'closingbalance': closingbalance, 'transfer': transfer, 'oldtransfer': oldtransfer
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_coldroomstock_details();
                            $('#coldroom_fillform').css('display', 'none');
                            $('#diveditdata').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                            $('#div_Deptdata').show();
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            ColdRoom Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Curd</a></li>
            <li><a href="#">ColdRoom Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>ColdRoom Details
                </h3>
            </div>
            <div class="box-body">
            <div>
                <div id="showlogs">
                    <%--<input id="btn_addDept" type="button" name="submit" value='Add Production Details'
                        class="btn btn-success" />--%>

                        <table>
                    <tr>
                    <td>
                          </td>
                        <td>
                            <label>
                                Date <span style="color: red;">*</span></label>
                        </td>
                        <td style="width: 5px;">
                          </td>
                        <td>
                            <input id="txt_getdatadate" class="form-control" type="date" name="vendorcode" />
                        </td>
                         <td style="width: 5px;">
                          </td>
                        <td>
                        <%--<input id="txt_getgenerate" type="button" name="submit" value='Generate' class="btn btn-primary" onclick="get_coldroomstock_details();"/>--%>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="get_coldroomstock_details()"></span> <span onclick="get_coldroomstock_details()">Generate</span>
                          </div>
                          </div>
                            </td>
                        </td>
                        <%--<td style="width: 400px;">
                          </td>
                        <td>
                        <input id="btn_addDept" type="button" name="submit" value='Add Details'
                        class="btn btn-success" />
                        </td>--%>
                         <td>
                            <td style="padding-left: 654px;">
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addcoldroomtock()"></span> <span onclick="addcoldroomtock()">Add Details</span>
                          </div>
                          </div>
                            </td>
                    </tr>
                </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='coldroom_fillform' style="display: none;">
                <div style="padding-left: 35%;">
                <table>
                    <tr>
                        <td>
                            <label>
                                Date <span style="color: red;">*</span></label>
                                </td>
                                <td>
                            <input id="txt_date" class="form-control" type="date" name="date" />
                                </td>
                        <td style="width:10px">
                        </td>
                        <td>
                         <%--<input id='Button1' type="button" class="btn btn-success" name="submit"
                            value='Get' onclick="fillcoldroomdetails();" />--%>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="fillcoldroomdetails()"></span> <span onclick="fillcoldroomdetails()">Get</span>
                          </div>
                          </div>
                        </td>
                    </tr>
                    <tr hidden>
                        <td>
                            <label>
                                Batch Type<span style="color: red;">*</span></label>
                            <select id="slct_Batch" class="form-control" >
                                <option selected disabled value="Select SILO No">Select Batch Type</option>
                            </select>
                            <label id="lbl_silo" class="errormessage">
                                * Please Batch Type</label>
                        </td>
                    </tr>
                    <tr hidden>
                        <label id="Label1">
                        </label>
                    </tr>
                </table>
            </div>
                    <div id="divFillScreen">
                    </div>
                    <div style="padding-left: 40%;">
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Remarks</label>
                                <textarea rows="3" cols="45" id="txt_Remarks" class="form-control" maxlength="200"
                                    placeholder="Enter Remarks"></textarea>
                            </td>
                        </tr>
                        <tr hidden>
                            <label id="lbl_sno">
                            </label>
                        </tr>
                       <%-- <tr>
                            <td>
                                <input id='save_batchdetails' type="button" class="btn btn-success" name="submit"
                                    value='Save' onclick="save_coldroomstock_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="Clearvalues()" />
                            </td>
                        </tr>--%>
                    </table>
                     <div  style="padding-left: 10%;padding-top: 5%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_batchdetails1" onclick="save_coldroomstock_click()"></span><span id="save_batchdetails" onclick="save_coldroomstock_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="Clearvalues()"></span><span id='btn_close' onclick="Clearvalues()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div   class="box-body">
            <div id="diveditdata" style="display:none;align:center;padding-left: 100px;">
                </div>
            </div>
            </div>
        </div>
    </section>
</asp:Content>