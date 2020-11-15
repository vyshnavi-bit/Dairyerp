<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Approval_Vendor_TSRate.aspx.cs" Inherits="Approval_Vendor_TSRate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <style type="text/css">
        input[type=number]::-webkit-inner-spin-button, input[type=number]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
    <script type="text/javascript">
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
        function get_vendor_ts_rate_click() {
            var type = document.getElementById('ddltype').value
            if (type == "") {
                alert("Please select type");
                return false;
            }
            var data = { 'op': 'Get_Approval_Vendor_TS_Rates_Logs', 'type': type };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        GetVendorTSRates(msg);
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
        function GetVendorTSRates(msg) {
            var results = '<div  style="overflow:auto;"><table id="table_Vendor_TSRate_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            var seltype = document.getElementById('ddltype').value
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Vendor Name</th><th scope="col">From Date</th><th scope="col">To Date</th><th scope="col">RateOn</th><th scope="col">CalcOn</th><th scope="col">Prev Rate</th><th scope="col">New Rate</th><th scope="col">NEW OH ON</th><th scope="col">NEW OH Cost</th><th scope="col">NEW STD SNF</th><th scope="col">NEW SNF+/-On</th><th scope="col">NEW SNFCost</th><th scope="col">New STD FAT</th><th>New FAT +/- On</th><th>New FAT +/- Cost</th><th scope="col">Remarks</th><th></th></tr></thead></tbody>';
//            if (seltype == "Buffalo" || seltype == "Others Buffalo") {
//                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Vendor Name</th><th scope="col">From Date</th><th scope="col">To Date</th><th scope="col">RateOn</th><th scope="col">CalcOn</th><th scope="col">Prev Rate</th><th scope="col">New Rate</th><th scope="col">NEW OH ON</th><th scope="col">NEW OH Cost</th><th scope="col">NEW STD SNF</th><th scope="col">NEW SNF+/-On</th><th scope="col">NEW SNFCost</th><th scope="col">New STD FAT</th><th>New FAT +/- On</th><th>New FAT +/- Cost</th><th scope="col">Remarks</th><th></th></tr></thead></tbody>';
//            }
//            else {
//                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Vendor Name</th><th scope="col">From Date</th><th scope="col">To Date</th><th scope="col">RateOn</th><th scope="col">CalcOn</th><th scope="col">Prev Rate</th><th scope="col">New Rate</th><th scope="col">Remarks</th><th></th></tr></thead></tbody>';
//            }
            for (var i = 0; i < msg.length; i++) {
                //if (msg[i].type == "Buffalo" || msg[i].type == "Others Buffalo") {
                results += '<tr>';
                results += '<th scope="row" class="1"><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                results += '<th scope="row" class="1"><span id="txt_Vendorname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].vendorname + '</span></th>';
                results += '<td class="5"><input id="txt_fromdate" class="fromdatecls" type="date" name="vendorcode"placeholder="Enter Date" readonly value="' + msg[i].fromdate + '"></td>';
                results += '<td class="5"><input id="txt_todate" class="todatecls" type="date" name="vendorcode"placeholder="Enter Date" readonly value="' + msg[i].todate + '"></td>';
                results += '<td class="5"><span id="spn_rateon" style="font-size: 10px; color: Red;">' + msg[i].rate_on + '</span></td>';
                results += '<td class="5"><span id="spn_calon" style="font-size: 10px; color: Red;">' + msg[i].calc_on + '</span></td>';
                results += '<td class="5"><input id="txt_PrevRate" class="form-control" type="number" name="vendorcode"placeholder="Enter Rate" readonly  value="' + msg[i].presrate + '"></td>';
                results += '<td class="5"><input id="txt_NewRate" class="form-control" type="number" name="vendorcode"placeholder="Enter Rate" value="' + msg[i].newrate + '"></td>';

                results += '<td class="5"><input id="txt_ohon" class="form-control" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].overheadon + '"></td>';
                results += '<td class="5"><input id="txt_ohcost" class="form-control" type="text" placeholder="Over Head Cost" name="OverHeadCost" value="' + msg[i].overheadcost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td class="5"><input id="txt_mstdsnf" class="form-control" name="MSTDSNF" type="text" placeholder="-STD SNF" value="' + msg[i].mstdsnf + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_pstdsnf" class="form-control" name="PSTDSNF" type="text" value="' + msg[i].pstdsnf + '" placeholder="+STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td class="5"><input id="txt_snfpluson" class="form-control" type="text" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].snfpluson + '"></td>';
                results += '<td class="5"><input id="txt_msnfcost" class="form-control" type="text" placeholder="-SNF Cost" name="MSNFPlusCost" value="' + msg[i].msnfpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_psnfcost" class="form-control" type="text" placeholder="+SNF Cost" name="PSNFPlusCost" value="' + msg[i].psnfpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';


                results += '<td data-title="STD FAT"><input id="txt_mstdfat" class="form-control" name="MSTDFAT" type="text" placeholder="-STD FAT" value="' + msg[i].mstdfat + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_pstdfat" class="form-control" value="' + msg[i].pstdfat + '" name="PSTDFAT" type="text" placeholder="+STD FAT" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="FAT +/- On"><input id="txt_stdplus" class="form-control" type="text" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].fatpluson + '"></td>';
                results += '<td data-title="FAT - Cost"><input  id="txt_mfatcost" class="form-control" type="text" placeholder="-FAT Cost" name="MFATPlusCost" value="' + msg[i].mfatpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_pfatcost" class="form-control" type="text" placeholder="+FAT Cost" name="PFATPlusCost" value="' + msg[i].pfatpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';


                results += '<td class="5"><input id="txt_Remarks" class="form-control" type="text" name="vendorcode"placeholder="Enter Remarks" value="' + msg[i].remarks + '"></td>';
                //results += '<td><input id="btn_poplate" type="button" onclick="btnsave_approval_vendor_ts_rates_click(this)" name="submit" class="btn btn-success" value="For Approval" /></td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Approve!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 apprvcls"  onclick="btnsave_approval_vendor_ts_rates_click(this)"><span class="glyphicon glyphicon-thumbs-up" style="top: 0px !important;"></span></button></td>';
//                results += '<td><input id="btn_poplate" type="button"  onclick="sendotp(this)" name="submit" class="btn btn-success" value="OTP" /></td>';
//                results += '<td><input id="btn_poplate" type="button"  onclick="sendfp(this)" name="submit" class="btn btn-success" value="FP" /></td>';
               
                results += '<td style="display:none" class="8"><input id="hdn_vendorno" class="form-control" type="number" name="vendorcode" value="' + msg[i].vendorno + '"></td>';
                results += '<td style="display:none" class="8"><input id="hdn_refno" class="form-control" type="number" name="vendorcode" value="' + msg[i].refno + '"></td></tr>';
                //                }
                //                else {
                //                    results += '<tr>';
                //                    results += '<th scope="row" class="1"><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                //                    results += '<th scope="row" class="1"><span id="txt_Vendorname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].vendorname + '</span></th>';
                //                    results += '<td class="5"><input id="txt_fromdate" class="fromdatecls" type="date" name="vendorcode"placeholder="Enter Date" readonly value="' + msg[i].fromdate + '"></td>';
                //                    results += '<td class="5"><input id="txt_todate" class="todatecls" type="date" name="vendorcode"placeholder="Enter Date" readonly value="' + msg[i].todate + '"></td>';
                //                    results += '<td class="5"><span id="spn_rateon" style="font-size: 10px; color: Red;">' + msg[i].rate_on + '</span></td>';
                //                    results += '<td class="5"><span id="spn_calon" style="font-size: 10px; color: Red;">' + msg[i].calc_on + '</span></td>';
                //                    results += '<td class="5"><input id="txt_PrevRate" class="form-control" type="number" name="vendorcode"placeholder="Enter Rate" readonly  value="' + msg[i].presrate + '"></td>';
                //                    results += '<td class="5"><input id="txt_NewRate" class="form-control" type="number" name="vendorcode"placeholder="Enter Rate" value="' + msg[i].newrate + '"></td>';
                //                    results += '<td class="5"><input id="txt_Remarks" class="form-control" type="text" name="vendorcode"placeholder="Enter Remarks" value="' + msg[i].remarks + '"></td>';
                //                    results += '<td><input id="btn_poplate" type="button"  onclick="btnsave_approval_vendor_ts_rates_click(this)" name="submit" class="btn btn-success" value="For Approval" /></td>';
                //                    results += '<td style="display:none" class="8"><input id="hdn_vendorno" class="form-control" type="number" name="vendorcode" value="' + msg[i].vendorno + '"></td>';
                //                    results += '<td style="display:none" class="8"><input id="hdn_refno" class="form-control" type="number" name="vendorcode" value="' + msg[i].refno + '"></td></tr>';
                //                }
            }
            results += '</table></div>';
            $("#divFillScreen").html(results);
//            var today = new Date();
//            var dd = today.getDate();
//            var mm = today.getMonth() + 1; //January is 0!
//            var yyyy = today.getFullYear();
//            if (dd < 10) {
//                dd = '0' + dd
//            }
//            if (mm < 10) {
//                mm = '0' + mm
//            }
//            var hrs = today.getHours();
//            var mnts = today.getMinutes();
//            $('.fromdatecls').val(yyyy + '-' + mm + '-' + dd);
//            $('.todatecls').val(yyyy + '-' + mm + '-' + dd);

        }
        function btnsave_approval_vendor_ts_rates_click(thisid) {
//            var rows = $("#table_Vendor_TSRate_details tr:gt(0)");
//            var Vendor_TSRate_details = new Array();
//            $(rows).each(function (i, obj) {
//                if ($(this).find('#txt_NewRate').val() != "") {
//                    Vendor_TSRate_details.push({ vendorno: $(this).find('#hdn_vendorno').val(), fromdate: $(this).find('#txt_fromdate').val(), todate: $(this).find('#txt_todate').val(), rate_on: $(this).find('#spn_rateon').text(), calc_on: $(this).find('#spn_calon').text(), presrate: $(this).find('#txt_PrevRate').val(), newrate: $(this).find('#txt_NewRate').val(), remarks: $(this).find('#txt_Remarks').val(), refno: $(this).find('#hdn_refno').val() });
//                }
//            });
//            if (Vendor_TSRate_details.length == 0) {
//                alert("Please enter new rates");
//                return false;
            //            }
            var type = document.getElementById('ddltype').value;
            var vendorno = $(thisid).closest("tr").find("#hdn_vendorno").val();
            var fromdate = $(thisid).closest("tr").find("#txt_fromdate").val();
            var todate = $(thisid).closest("tr").find("#txt_todate").val();
            var PrevRate = $(thisid).closest("tr").find("#txt_PrevRate").val();
            var NewRate = $(thisid).closest("tr").find("#txt_NewRate").val();
            var ohon = $(thisid).closest("tr").find("#txt_ohon").val();
            var ohcost = $(thisid).closest("tr").find("#txt_ohcost").val();
            var mstdsnf = $(thisid).closest("tr").find("#txt_mstdsnf").val();
            var pstdsnf = $(thisid).closest("tr").find("#txt_pstdsnf").val();
            var snfpluson = $(thisid).closest("tr").find("#txt_snfpluson").val();
            var msnfcost = $(thisid).closest("tr").find("#txt_msnfcost").val();
            var psnfcost = $(thisid).closest("tr").find("#txt_psnfcost").val();


            var mstdfat = $(thisid).closest("tr").find("#txt_mstdfat").val();
            var pstdfat = $(thisid).closest("tr").find("#txt_pstdfat").val();
            var fatpuls = $(thisid).closest("tr").find("#txt_stdplus").val();
            var mfatcost = $(thisid).closest("tr").find("#txt_mfatcost").val();
            var pfatcost = $(thisid).closest("tr").find("#txt_pfatcost").val();


            var rateon = $(thisid).closest("tr").find("#spn_rateon").text();
            var calon = $(thisid).closest("tr").find("#spn_calon").text();
            var Remarks = $(thisid).closest("tr").find("#txt_Remarks").val();
            var refno = $(thisid).closest("tr").find("#hdn_refno").val();
            var confi = confirm("Do you want to EDIT  Grade Details ?");
            if (confi) {
                var Data = { 'op': 'btnsave_approval_vendor_ts_rates_click', 'type': type, 'vendorno': vendorno, 'fromdate': fromdate, 'todate': todate, 'PrevRate': PrevRate, 'rateon': rateon, 'ohon': ohon, 'ohcost': ohcost, 'mstdsnf': mstdsnf, 'pstdsnf': pstdsnf, 'snfpluson': snfpluson, 'msnfcost': msnfcost, 'psnfcost': psnfcost, 'mstdfat': mstdfat, 'pstdfat': pstdfat, 'fatpuls': fatpuls, 'mfatcost': mfatcost, 'pfatcost': pfatcost, 'calon': calon, 'Remarks': Remarks, 'NewRate': NewRate, 'refno': refno };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        get_vendor_ts_rate_click();
                        btn_cancel_click();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(Data, s, e);
            }
        }
        function btn_cancel_click() {
            //            $('#divFillScreen').removeTemplate();
            //            $('#divFillScreen').setTemplateURL('Vendor_TS_Ratemaster.htm');
            //            $('#divFillScreen').processTemplate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Vendor TS Rates<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vendor TS Rates</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Approval Vendor TS Rate Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <select id="ddltype" class="form-control">
                                <option selected disabled value="Select Type">Select Type</option>
                                <option>Buffalo</option>
                                <option>Cow</option>
                                <option>Others Buffalo</option>
                                <option>Others Cow</option>
                            </select>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <%--<td>
                            <input type="button" id="Button2" value="Get Details" onclick="get_vendor_ts_rate_click();"
                                class="btn btn-success" />
                        </td>--%>
                         <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="get_vendor_ts_rate_click()"></span> <span onclick="get_vendor_ts_rate_click()">Get Details</span>
                          </div>
                          </div>
                            </td>
                    </tr>
                </table>
                <div id="divFillScreen">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
