<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="transportvalue.aspx.cs" Inherits="transportvalue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
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
            $('#txt_fromdate').val(yyyy + '-' + mm + '-' + dd);
            $('#txt_todate').val(yyyy + '-' + mm + '-' + dd);
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

        function branch_typechange() {
            get_Vendor_details();
        }
        function get_Vendor_details() {
            var data = { 'op': 'get_Vendor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillvendors(msg);
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
        function fillvendors(msg) {
            var Vendor = msg[0].vendordetails;
            var branchtype = document.getElementById('slct_branchtype').value;
            var data = document.getElementById('slct_branchname');
            var length = data.options.length;
            document.getElementById('slct_branchname').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Branch";
            opt.value = "Select Branch";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < Vendor.length; i++) {
                if (Vendor[i].branchtype == branchtype) {
                    if (Vendor[i].vendorname != null) {
                        var option = document.createElement('option');
                        option.innerHTML = Vendor[i].vendorname;
                        option.value = Vendor[i].sno;
                        data.appendChild(option);
                    }
                }
            }
        }
        function get_vendor_milktransaction_details() {
            var fromdate = document.getElementById('txt_fromdate').value;
            var todate = document.getElementById('txt_todate').value;
            var transactiontype = document.getElementById('slct_transactontype').value;
            if (transactiontype == "" || transactiontype == "Select Transaction Type") {
                alert("Select Transaction Type");
                return false;
            }
            var data = { 'op': 'get_vendor_milktransaction_details', 'fromdate': fromdate, 'todate': todate, 'transactiontype': transactiontype };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $("#divFillScreen").show();
                        var results = '<div    style="overflow:auto;"><table id="table_milktransaction_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Vendor Name</th><th scope="col">Cell Type</th><th scope="col">Transaction No</th><th scope="col">Vehicle No</th><th scope="col">Qty(ltrs)</th><th scope="col">Total Milk(qty)</th><th scope="col">Transport Value</th><th scope="col">Per Ltr Rate</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr>';
                            results += '<th style="display:none;"><span id="span" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                            results += '<th><span id="txt_vendorname" style="font-size: 12px;  font-weight: bold; color: #0252aa;">' + msg[i].vendorname + '</span></th>';
                            results += '<th><span id="txt_cell" style="font-size: 12px;  font-weight: bold; color: #0252aa;">' + msg[i].cell + '</span></th>';
                            results += '<td><input id="txt_transactionno" class="form-control" value="' + msg[i].transactionno + '" type="number" name="vendorcode"placeholder="Enter transactionno"></td>';
                            results += '<td><input id="txt_vehicleno" class="form-control" value="' + msg[i].vehicleno + '"  name="vendorcode"placeholder="Enter vehicleno"></td>';
                            results += '<td><input id="txt_qtyltr" class="form-control" value="' + msg[i].qtyltrs + '"  name="vendorcode"placeholder="Enter qtyltrs"></td>';
                            results += '<td style="display:none;" ><input id="txt_kms" class="form-control" value="' + msg[i].kms + '" type="text" name="vendorcode"placeholder="Enter kms"></td>';
                            results += '<td><input id="txt_totleqtyltrs" class="form-control" value="' + msg[i].totalqty + '" type="text" name="vendorcode"placeholder="Enter Total Quantity"></td>';
                            results += '<td style="display:none;" ><input id="txt_totletransportvalue" class="form-control" value="" type="text"  name="vendorcode" placeholder="Enter Total transport value"></td>';
                            results += '<td><input id="txt_transportvalue" class="form-control" value="" type="number" onkeyup="Changevalue(this);" name="vendorcode" placeholder="Enter transport value"></td>';
                            results += '<td><input id="txt_perltrrate" class="form-control" value="" type="number" name="perliterrate"placeholder="Enter Per ltr Rate"></td>';
                            results += '<td><input id="btn_poplate" type="button"  onclick="save_transportvalue_click(this)" name="Save" class="btn btn-primary" value="Save" /></td>';
                            results += '<td style="display:none;"><input id="txt_vendorid" class="form-control" value="' + msg[i].vendorcode + '" type="number" name="vendorcode"></td>';
                            results += '</tr>';
                        }
                        results += '</table></div>';
                        $("#divFillScreen").html(results);
                    }
                    else {
                        $("#divFillScreen").hide();
                    }
                }
                else {
                    $("#divFillScreen").hide();
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Changevalue(qtyid) {
            if (qtyid.value != "") {
                var tranportvalue = 0;
                var qtyltr = 0;
                var transport = 0;
                var tvalue = 0;

                qtyltr = $(qtyid).closest("tr").find('#txt_qtyltr').val();
                tranportvalue = $(qtyid).closest("tr").find('#txt_transportvalue').val();
                transport = parseFloat(qtyid.value).toFixed(3);
                tvalue = tranportvalue / qtyltr;
                tvalue = tvalue.toFixed(2);
                $(qtyid).closest("tr").find('#txt_perltrrate').val(tvalue);
            }
        }
        function save_transportvalue_click() {
            //var branchname = document.getElementById('slct_branchname').value;
            var sno = document.getElementById('lbl_sno').value;
            var fromdate = document.getElementById('txt_fromdate').value;
            var todate = document.getElementById('txt_todate').value;
            var transactontype = document.getElementById('slct_transactontype').value;
            var btnvalue = document.getElementById('btn_save').value;
            var rows = $("#table_milktransaction_details tr:gt(0)");
            var transport_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_transportvalue').val() != "") {
                    transport_details.push({ vendorid: $(this).find('#txt_vendorid').val(),
                        vendorname: $(this).find('#txt_vendorname').text(),
                        celltype: $(this).find('#txt_cell').text(),
                        transactionno: $(this).find('#txt_transactionno').val(),
                        vehicleno: $(this).find('#txt_vehicleno').val(),
                        kms: $(this).find('#txt_kms').val(),
                        totalmilkqty: $(this).find('#txt_totleqtyltrs').val(),
                        totaltransportvalue: $(this).find('#txt_totletransportvalue').val(),
                        transportvalue: $(this).find('#txt_transportvalue').val(),
                        perltrrate: $(this).find('#txt_perltrrate').val(),
                        qtyltr: $(this).find('#txt_qtyltr').val(),
                        buttonval: $(this).find('#btn_poplate').val()
                    });
                }
            });
            if (transport_details.length == 0) {
                alert("Please Enter Transport Value");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_transportvalue_click', 'sno': sno, 'fromdate': fromdate, 'todate': todate, 'transactontype': transactontype, 'btnvalue': btnvalue, 'transport_details': transport_details };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_vendor_milktransaction_details();
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

        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Transport Entry<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Transport Entry</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Transport Entry Details
                </h3>
            </div>
            <div style="padding-left: 150PX;">
                <table>
                   <%-- <tr>
                        <td>
                            <label>
                                Branch Type<span style="color: red;">*</span></label>
                            <select id="slct_branchtype" class="form-control" onchange="branch_typechange();">
                                <option selected disabled value="Select SILO No">Select Branch Type</option>
                                <option value="Inter Branch">Inter Branches</option>
                                <option value="Other Branch">Other Branches</option>
                            </select>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <label>
                                Branch Name<span style="color: red;">*</span></label>
                            <select id="slct_branchname" class="form-control">
                            </select>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <label>
                                Transaction Type<span style="color: red;">*</span></label>
                            <select id="slct_transactontype" class="form-control">
                            <option selected disabled value="Select Transaction Type">Select Transaction Type</option>
                                <option>in</option>
                                <option>Out</option>
                            </select>
                        </td>
                         <td style="width: 10px;">
                        </td>
                        <td>
                            <label>
                                From Date <span style="color: red;">*</span></label>
                            <input id="txt_fromdate" class="form-control" type="date" name="vendorcode"
                                placeholder="Enter Date">
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <label>
                                To Date <span style="color: red;">*</span></label>
                            <input id="txt_todate" class="form-control" type="date" name="vendorcode"
                                placeholder="Enter Date">
                        </td>
                        <td style="padding-top: 4%;padding-left: 2%;">
                            <%--<input id='save_batchdetails' type="button" class="btn btn-success" name="submit"
                                value='Get Details' onclick="get_vendor_milktransaction_details()" />--%>
                                 <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="get_vendor_milktransaction_details()"></span> <span onclick="get_vendor_milktransaction_details()">Get Details</span>
                          </div>
                          </div>
                        </td>
                    </tr>
                    <tr hidden>
                        <label id="lbl_sno">
                        </label>
                    </tr>
                </table>
            </div>
            <div class="box-body">
                <div id="divFillScreen">
                </div>
            </div>
            <div class="box-body">
                <div id="div1" style="display:none;">
                <input id='btn_save' type="button" class="btn btn-success" name="submit"
                                value='Save' onclick="save_transportvalue_click()" />
                </div>
            </div>
            
        </div>
    </section>
</asp:Content>
