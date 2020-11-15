<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="packingmaterial.aspx.cs" Inherits="packingmaterial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btn_addDept').click(function () {
                $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
                fillproductdetails();
            });
            $('#btn_close').click(function () {
                $('#Inwardsilo_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                Clearvalues();
            });
            get_packing_material_details();
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
        function addmaterial() {
            $('#Inwardsilo_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            fillproductdetails();
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
        function fillproductdetails() {
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Product Name</th><th scope="col">Received</th><th scope="col">Consumption</th><th scope="col">Production</th><th scope="col">A.Production</th><th scope="col">Wastage</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].departmentid == "1" && msg[i].batchid != "13") {
                                results += '<tr>';
                                results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                results += '<td><input id="txt_reciveqty" class="form-control" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
                                results += '<td><input id="txt_concumption" class="form-control" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter Consumption qty"></td>';
                                results += '<td><input id="txt_Production" class="form-control" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter Production(kgs)"></td>';
                                results += '<td><input id="txt_aproduction" class="form-control" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter Approve Production(kgs)"></td>';
                                results += '<td><input id="txt_wastage" class="form-control" value="" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Wastage"></td>';
                                results += '<td style="display:none"><input id="txt_recivedqtymilk" class="form-control" value="0" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter Received Milk Qty"></td>';
                                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                            }
                        }
                        results += '</table></div>';
                        $("#divFillScreen").html(results);
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
        function get_product_details() {
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillchemicals(msg);
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
        function fillchemicals(msg) {
            var data = document.getElementById('slct_product');
            var length = data.options.length;
            document.getElementById('slct_product').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Product";
            opt.value = "Select Product";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].departmentid == "1") {
                    if (msg[i].productname != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].productname;
                        option.value = msg[i].productid;
                        data.appendChild(option);
                    }
                }
            }
        }
        function get_packing_material_details() {
            var data = { 'op': 'get_packing_material_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillpakingmaterials(msg);
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
        function fillpakingmaterials(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col">Product Name</th><th scope="col">O/B</th><th scope="col">Received</th><th scope="col">Total</th><th scope="col">consumption</th><th scope="col">CB</th><th scope="col">Date</th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1" style="display:none">' + msg[i].branchname + '</td>';
                results += '<td data-title="Capacity" class="2" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].ob + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].recived + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].total + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].consumption + '</td>';
                results += '<td  class="7" style="text-align:center;">' + msg[i].cb + '</td>';
                results += '<td  class="8" style="text-align:center;">' + msg[i].doe + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].production + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].aproveproduction + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].wastage + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="13">' + msg[i].remarks + '</td>';
                results += '<td style="display:none" class="15">' + msg[i].qty_ltr + '</td>';
                results += '<td style="display:none" class="14">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function getme(thisid) {
            var productname = $(thisid).parent().parent().children('.2').html();
            var ob = $(thisid).parent().parent().children('.3').html();
            var recived = $(thisid).parent().parent().children('.4').html();
            var total = $(thisid).parent().parent().children('.5').html();
            var consumption = $(thisid).parent().parent().children('.6').html();
            var cb = $(thisid).parent().parent().children('.7').html();
            var production = $(thisid).parent().parent().children('.9').html();
            var Approveproduction = $(thisid).parent().parent().children('.10').html();
            var wastage = $(thisid).parent().parent().children('.11').html();
            var productid = $(thisid).parent().parent().children('.12').html();
            var remarks = $(thisid).parent().parent().children('.13').html();
            var sno = $(thisid).parent().parent().children('.14').html();
            var qty_ltr = $(thisid).parent().parent().children('.15').html();
            var date = $(thisid).parent().parent().children('.8').html();

            var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Product Name</th><th scope="col">Received</th><th scope="col">Consumption</th><th scope="col">Production</th><th scope="col">A.Production</th><th scope="col">Wastage</th></tr></thead></tbody>';
            results += '<tr>';
            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + sno + '</span></th>';
            results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + productname + '</span></th>';
            results += '<td><input id="txt_reciveqty" class="form-control" onkeypress="return isFloat(event);" value="' + recived + '" type="text" name="vendorcode"placeholder="Enter Received qty"></td>';
            results += '<td><input id="txt_concumption" class="form-control" onkeypress="return isFloat(event);" value="' + consumption + '" type="text" name="vendorcode"placeholder="Enter Consumption qty"></td>';
            results += '<td><input id="txt_Production" class="form-control" onkeypress="return isFloat(event);" value="' + production + '" type="text" name="vendorcode"placeholder="Enter Production(kgs)"></td>';
            results += '<td><input id="txt_aproduction" class="form-control" onkeypress="return isFloat(event);" value="' + Approveproduction + '" type="text" name="vendorcode"placeholder="Enter Approve Production(kgs)"></td>';
            results += '<td><input id="txt_wastage" class="form-control" onkeypress="return isFloat(event);" value="' + wastage + '" type="text" name="vendorcode"placeholder="Wastage"></td>';
            results += '<td style="display:none"><input id="txt_recivedqtymilk" onkeypress="return isFloat(event);" class="form-control" value="' + qty_ltr + '" type="text" name="vendorcode"placeholder="Enter Received Milk Qty"></td>';
            results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="text" name="vendorcode" value="' + productid + '"></td></tr>';
            results += '</table></div>';
            $("#divFillScreen").html(results);
            document.getElementById('lbl_sno').value = sno;
            document.getElementById('txt_date').value = date;
            document.getElementById('save_batchdetails').innerHTML = "Modify";
            $("#div_Deptdata").hide();
            $("#Inwardsilo_fillform").show();
            $('#showlogs').hide();
        }
        function save_packing_material_click() {
            var remarks = document.getElementById('txt_Remarks').value;
            var date = document.getElementById('txt_date').value;
            var btnvalue = document.getElementById('save_batchdetails').innerHTML;
            var sno = document.getElementById('lbl_sno').value;
            //var pqty = document.getElementById('lbl_pqty').value;
            var rows = $("#table_shift_wise_details tr:gt(0)");
            var curd_packing_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_reciveqty').val() != "") {
                    curd_packing_details.push({ productid: $(this).find('#hdn_productid').val(), recive_qty: $(this).find('#txt_reciveqty').val(), consumption: $(this).find('#txt_concumption').val(), production: $(this).find('#txt_Production').val(), aproduction: $(this).find('#txt_aproduction').val(), wastage: $(this).find('#txt_wastage').val(), recivedqtymilk: $(this).find('#txt_recivedqtymilk').val() });
                }
            });
            if (curd_packing_details.length == 0) {
                alert("Please enter Receive quantity");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_packing_material_click', 'curd_packing_details':curd_packing_details, 'remarks': remarks,  'btnvalue': btnvalue, 'sno': sno, 'date': date };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_packing_material_details();
                            $('#Inwardsilo_fillform').css('display', 'none');
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
            document.getElementById('txt_ob').value = "";
            document.getElementById('txt_recived').value = "";
            document.getElementById('txt_total').value = "";
            document.getElementById('txt_consumption').value = "";
            document.getElementById('txt_closingbal').value = "";
            document.getElementById('txt_production').value = "";
            document.getElementById('txt_aproduction').value = "";
            document.getElementById('txt_wastage').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('save_batchdetails').innerHTML = "Save";
            $('#Inwardsilo_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_Deptdata').show();
        }
        function ctotal(qtyid) {
            if (qtyid.value == "") {
            }
            else {
                var ob = document.getElementById('txt_ob').value;
                if (ob == "") {
                    alert("Please enter O/B");
                    return false;
                }
                var recipts = qtyid.value;
                var sum = parseFloat(ob) + parseFloat(recipts);
                document.getElementById('txt_total').value = sum;
            }
        }
        function Consumptiontotal(cbid) {
            if (cbid.value == "") {
            }
            else {
                var ob = document.getElementById('txt_total').value;
                if (ob == "") {
                    alert("Please enter total");
                    return false;
                }
                var recipts = cbid.value;
                var sum = parseFloat(ob) - parseFloat(recipts);
                document.getElementById('txt_closingbal').value = sum;
            }
        }
        function productchange() {
            var productid = document.getElementById('slct_product').value;
            var data = { 'op': 'get_productqty_details', 'productid': productid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('txt_ob').value = msg[i].quantity;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Packing Material Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Curd</a></li>
            <li><a href="#">Packing Material Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Packing Material Details 
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                   <%-- <input id="btn_addDept" type="button" name="submit" value='Add Packing Material Details'
                        class="btn btn-success" />--%>
                        <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addmaterial()"></span> <span onclick="addmaterial()">Add Details</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='Inwardsilo_fillform' style="display: none;">
                <div style="text-align: -webkit-center;">
                    <table>
                        <tr>
                            <td>
                                <label>
                                    Date <span style="color: red;">*</span></label>
                            </td>
                            <td>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                        </tr>
                    </table>
                    </div>
                    <div id="divFillScreen">
                    </div>
                    <div style="text-align: -webkit-center;">
                    <table align="center">
                        <tr>
                            <td >
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
                    </div>
                    <%--<table align="center">
                        <tr>
                            <td>
                                <input id='save_batchdetails' type="button" class="btn btn-success" name="submit"
                                    value='Save' onclick="save_packing_material_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="Clearvalues()" />
                            </td>
                        </tr>
                    </table>--%>
                     <div  style="text-align: -webkit-center;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_batchdetails1" onclick="save_packing_material_click()"></span><span id="save_batchdetails" onclick="save_packing_material_click()">Save</span>
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
    </section>
</asp:Content>
