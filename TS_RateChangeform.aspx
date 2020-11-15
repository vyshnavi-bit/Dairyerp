<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="TS_RateChangeform.aspx.cs" Inherits="TS_RateChangeform" %>

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
            var data = { 'op': 'Get_Vendor_TS_Rates', 'type': type };
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
        function GetVendorTSRates(msg) {
            var results = '<div    style="overflow:auto;"><table id="table_Vendor_TSRate_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            var seltype = document.getElementById('ddltype').value
//            if (seltype == "Buffalo" || seltype == "Others Buffalo") {
//                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Vendor Name</th><th scope="col">From Date</th><th scope="col">To Date</th><th scope="col">RateOn</th><th scope="col">CalcOn</th><th scope="col">Prev Rate</th><th scope="col">New Rate</th><th scope="col">Prev OH ON</th><th scope="col">Prev OH Cost</th><th scope="col">Prev STDSNF</th><th scope="col">Prev SNF+/-On</th><th scope="col">Prev SNFCost</th> <th scope="col">NEW OH ON</th><th scope="col">NEW OH Cost</th><th scope="col">NEW STD SNF</th><th scope="col">NEW SNF+/-On</th><th scope="col">NEW SNFCost</th><th scope="col">Remarks</th><th></th></tr></thead></tbody>';
//            }
//            else {
//                
//                results += '<thead><tr><th scope="col">Sno</th><th scope="col">Vendor Name</th><th scope="col">From Date</th><th scope="col">To Date</th><th scope="col">RateOn</th><th scope="col">CalcOn</th><th scope="col">Prev Rate</th><th scope="col">New Rate</th><th scope="col">Remarks</th><th></th></tr></thead></tbody>';
//            }
            results += '<thead><tr ><th scope="col">Sno</th><th scope="col">Vendor Name</th><th scope="col">From Date</th><th scope="col">To Date</th><th scope="col">RateOn</th><th scope="col">CalcOn</th><th scope="col">Prev Rate</th><th scope="col">New Rate</th><th scope="col">Prev OH ON</th><th scope="col">Prev OH Cost</th><th scope="col">Prev STDSNF</th><th scope="col">Prev SNF+/-On</th><th scope="col">Prev SNFCost</th> <th scope="col">Prev STD FAT</th><th>Prev FAT +/- On</th><th>Prev FAT +/- Cost</th><th scope="col">NEW OH ON</th><th scope="col">NEW OH Cost</th><th scope="col">NEW STD SNF</th><th scope="col">NEW SNF+/-On</th><th scope="col">NEW SNFCost</th><th scope="col">New STD FAT</th><th>New FAT +/- On</th><th>New FAT +/- Cost</th><th scope="col">Remarks</th><th></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
//                if (msg[i].type == "Buffalo" || seltype == "Others Buffalo") {
                    results += '<tr>';
                    results += '<th scope="row" class="1"><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                    results += '<th scope="row" class="1"><span id="txt_Vendorname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].vendorname + '</span></th>';
                    results += '<td class="5"><input id="txt_fromdate" class="fromdatecls" type="date" name="vendorcode"placeholder="Enter Date"></td>';
                    results += '<td class="5"><input id="txt_todate" class="todatecls" type="date" name="vendorcode"placeholder="Enter Date"></td>';
                    results += '<td class="5"><span id="spn_rateon" style="font-size: 10px; color: Red;">' + msg[i].rate_on + '</span></td>';
                    results += '<td class="5"><span id="spn_calon" style="font-size: 10px; color: Red;">' + msg[i].calc_on + '</span></td>';
                    results += '<td class="5"><input id="txt_PrevRate" class="form-control" type="number"  name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].presrate + '"></td>';
                    results += '<td class="5"><input id="txt_NewRate" class="form-control" type="number" onkeypress="return isFloat(event);"  name="vendorcode"placeholder="Enter Rate"></td>';
                    results += '<td class="5"><input class="form-control" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].overheadon + '"></td>';
                    results += '<td class="5"><input class="form-control" type="text" placeholder="Over Head Cost"  name="OverHeadCost" value="' + msg[i].overheadcost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    
                    results += '<td class="5"><input class="form-control" name="MSTDSNF" type="text" placeholder="-STD SNF" value="' + msg[i].mstdsnf + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" name="PSTDSNF" type="text" value="' + msg[i].pstdsnf + '" placeholder="+STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td class="5"><input class="form-control" type="text" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].snfpluson + '"></td>';
                    results += '<td class="5"><input class="form-control" type="text" placeholder="-SNF Cost" name="MSNFPlusCost" value="' + msg[i].msnfpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+SNF Cost" name="PSNFPlusCost" value="' + msg[i].psnfpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';

                    results += '<td data-title="STD FAT"><input class="form-control" name="MSTDFAT" type="text" placeholder="-STD FAT" value="' + msg[i].mstdfat + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input  class="form-control" value="' + msg[i].pstdfat + '" name="PSTDFAT" type="text" placeholder="+STD FAT" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="FAT +/- On"><input class="form-control" type="text" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].fatpluson + '"></td>';
                    results += '<td data-title="FAT - Cost"><input  class="form-control" type="text" placeholder="-FAT Cost" name="MFATPlusCost" value="' + msg[i].mfatpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+FAT Cost" name="PFATPlusCost" value="' + msg[i].pfatpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    
                    
                    results += '<td data-title="Over Head On"><select id="slct_von" class="form-control" name="OverHeadOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>';
                    results += '<td data-title="Over Head Cost"><input id="txt_ohcost" class="form-control" type="text" placeholder="Over Head Cost" onkeypress="return isFloat(event);" name="OverHeadCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';

                    results += '<td data-title="STD SNF"><input id="txt_mstdsnf" class="form-control" name="MSTDSNF" type="text" onkeypress="return isFloat(event);" placeholder="-STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_pstdsnf" class="form-control" name="PSTDSNF" onkeypress="return isFloat(event);" type="text" placeholder="+STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="SNF +/- On"><select id="slct_snfplus" class="form-control" name="SNFPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>';
                    results += '<td data-title="SNF - Cost"><input id="txt_msnfcost" class="form-control" type="text" placeholder="-SNF Cost" name="MSNFPlusCost" onkeypress="return isFloat(event);" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_psnfcost" class="form-control" type="text" placeholder="+SNF Cost" name="PSNFPlusCost" value="0" onkeypress="return isFloat(event);" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';

                    results += '<td data-title="STD FAT"><input id="txt_mstdfat" class="form-control" name="MSTDFAT" type="text" placeholder="-STD FAT" onkeypress="return isFloat(event);" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_pstdfat"  onkeypress="return isFloat(event);" class="form-control" value="0" name="PSTDFAT" type="text" placeholder="+STD FAT" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="FAT +/- On"><select id="slct_fatplus" class="form-control" name="FATPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>';
                    results += '<td data-title="FAT - Cost"><input id="txt_mfatcost" class="form-control" type="text" placeholder="-FAT Cost" name="MFATPlusCost" value="0"  onkeypress="return isFloat(event);" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_pfatcost" class="form-control" type="text" placeholder="+FAT Cost" name="PFATPlusCost" value="0"  onkeypress="return isFloat(event);" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td class="5"><input id="txt_Remarks" class="form-control" type="text" name="vendorcode"placeholder="Enter Remarks"></td>';
                    //results += '<td><input id="btn_poplate" type="button"  onclick="btnsave_vendor_ts_rate_logs_click(this)" name="submit" class="btn btn-success" value="For Approval" /></td>';
                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To For Approve!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 apprvcls"  onclick="btnsave_vendor_ts_rate_logs_click(this)"><span class="glyphicon glyphicon-hand-right" style="top: 0px !important;"></span></button></td>';
                    results += '<td style="display:none" class="8"><input id="hdn_vendorno" class="form-control" type="number" name="vendorcode" value="' + msg[i].vendorno + '"></td></tr>';
                //}
//                else {
//                    results += '<tr>';
//                    results += '<th scope="row" class="1"><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
//                    results += '<th scope="row" class="1"><span id="txt_Vendorname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].vendorname + '</span></th>';
//                    results += '<td class="5"><input id="txt_fromdate" class="fromdatecls" type="date" name="vendorcode"placeholder="Enter Date"></td>';
//                    results += '<td class="5"><input id="txt_todate" class="todatecls" type="date" name="vendorcode"placeholder="Enter Date"></td>';
//                    results += '<td class="5"><span id="spn_rateon" style="font-size: 10px; color: Red;">' + msg[i].rate_on + '</span></td>';
//                    results += '<td class="5"><span id="spn_calon" style="font-size: 10px; color: Red;">' + msg[i].calc_on + '</span></td>';
//                    results += '<td class="5"><input id="txt_PrevRate" class="form-control" type="number" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].presrate + '"></td>';
//                    results += '<td class="5"><input id="txt_NewRate" class="form-control" type="number" name="vendorcode"placeholder="Enter Rate"></td>';

//                    results += '<td class="5"><input class="form-control" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].overheadon + '"></td>';
//                    results += '<td class="5"><input class="form-control" type="text" placeholder="Over Head Cost" name="OverHeadCost" value="' + msg[i].overheadcost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
//                    results += '<td class="5"><input class="form-control" name="MSTDSNF" type="text" placeholder="-STD SNF" value="' + msg[i].mstdsnf + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" name="PSTDSNF" type="text" value="' + msg[i].pstdsnf + '" placeholder="+STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
//                    results += '<td class="5"><input class="form-control" type="text" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].snfpluson + '"></td>';
//                    results += '<td class="5"><input class="form-control" type="text" placeholder="-SNF Cost" name="MSNFPlusCost" value="' + msg[i].msnfpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+SNF Cost" name="PSNFPlusCost" value="' + msg[i].psnfpluscost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';

//                    results += '<td data-title="Over Head On"><select id="slct_von" class="form-control" name="OverHeadOn" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>';
//                    results += '<td data-title="Over Head Cost"><input id="txt_ohcost" class="form-control" type="text" placeholder="Over Head Cost" name="OverHeadCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';

//                    results += '<td data-title="STD FAT"><input class="form-control" name="MSTDFAT" type="text" placeholder="-STD FAT" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" value="0" name="PSTDFAT" type="text" placeholder="+STD FAT" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
//                    results += '<td data-title="FAT +/- On"><select class="form-control" name="FATPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>';
//                    results += '<td data-title="FAT - Cost"><input class="form-control" type="text" placeholder="-FAT Cost" name="MFATPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input class="form-control" type="text" placeholder="+FAT Cost" name="PFATPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';

//                    results += '<td data-title="STD SNF"><input id="txt_mstdsnf" class="form-control" name="MSTDSNF" type="text" placeholder="-STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_pstdsnf" class="form-control" name="PSTDSNF" type="text" placeholder="+STD SNF" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
//                    results += '<td data-title="SNF +/- On"><select id="slct_snfplus" class="form-control" name="SNFPlus" style="font-size:12px;padding: 0px 5px;height:30px;" ><option  value="-">-</option><option  value="Ltrs">Ltrs</option><option  value="Kgs">Kgs</option></select></td>';
//                    results += '<td data-title="SNF - Cost"><input id="txt_msnfcost" class="form-control" type="text" placeholder="-SNF Cost" name="MSNFPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/><input id="txt_psnfcost" class="form-control" type="text" placeholder="+SNF Cost" name="PSNFPlusCost" value="0" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
//                    results += '<td class="5"><input id="txt_Remarks" class="form-control" type="text" name="vendorcode"placeholder="Enter Remarks"></td>';
//                    results += '<td><input id="btn_poplate" type="button"  onclick="btnsave_vendor_ts_rate_logs_click(this)" name="submit" class="btn btn-success" value="For Approval" /></td>';
//                    results += '<td style="display:none" class="8"><input id="hdn_vendorno" class="form-control" type="number" name="vendorcode" value="' + msg[i].vendorno + '"></td></tr>';
//                }
            }
            results += '</table></div>';
            $("#divFillScreen").html(results);
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
            $('.fromdatecls').val(yyyy + '-' + mm + '-' + dd);
            $('.todatecls').val(yyyy + '-' + mm + '-' + dd);
        }

        function btnsave_vendor_ts_rate_logs_click(thisid) {
//            var rows = $("#table_Vendor_TSRate_details tr:gt(0)");
//            var Vendor_TSRate_details = new Array();
//            $(rows).each(function (i, obj) {
//                if ($(this).find('#txt_NewRate').val() != "") {
//                    Vendor_TSRate_details.push({ vendorno: $(this).find('#hdn_vendorno').val(), fromdate: $(this).find('#txt_fromdate').val(), todate: $(this).find('#txt_todate').val(), rate_on: $(this).find('#spn_rateon').text(), calc_on: $(this).find('#spn_calon').text(), presrate: $(this).find('#txt_PrevRate').val(), newrate: $(this).find('#txt_NewRate').val(), remarks: $(this).find('#txt_Remarks').val() });
//                }
//            });
//            if (Vendor_TSRate_details.length == 0) {
//                alert("Please enter new rates");
//                return false;
            //            }
            var mytype = document.getElementById('ddltype').value;
            var vendorno = $(thisid).closest("tr").find("#hdn_vendorno").val();
            var fromdate = $(thisid).closest("tr").find("#txt_fromdate").val();
            var todate = $(thisid).closest("tr").find("#txt_todate").val();
            var PrevRate = $(thisid).closest("tr").find("#txt_PrevRate").val();
            var NewRate = $(thisid).closest("tr").find("#txt_NewRate").val();
            var rateon = $(thisid).closest("tr").find("#spn_rateon").text();
            var calon = $(thisid).closest("tr").find("#spn_calon").text();
            var ohon = $(thisid).closest("tr").find("#slct_von").val();
            var ohcost = $(thisid).closest("tr").find("#txt_ohcost").val();
            var mstdsnf = $(thisid).closest("tr").find("#txt_mstdsnf").val();
            var pstdsnf = $(thisid).closest("tr").find("#txt_pstdsnf").val();
            var snfpuls = $(thisid).closest("tr").find("#slct_snfplus").val();
            var msnfcost = $(thisid).closest("tr").find("#txt_msnfcost").val();
            var psnfcost = $(thisid).closest("tr").find("#txt_psnfcost").val();


            var mstdfat = $(thisid).closest("tr").find("#txt_mstdfat").val();
            var pstdfat = $(thisid).closest("tr").find("#txt_pstdfat").val();
            var fatpuls = $(thisid).closest("tr").find("#slct_fatplus").val();
            var mfatcost = $(thisid).closest("tr").find("#txt_mfatcost").val();
            var pfatcost = $(thisid).closest("tr").find("#txt_pfatcost").val();

            var Remarks = $(thisid).closest("tr").find("#txt_Remarks").val();
            var confi = confirm("Do you want save vendor TS rates ?");
            if (confi) {
                var Data = { 'op': 'btnsave_vendor_ts_rate_logs_click', 'vendorno': vendorno, 'fromdate': fromdate, 'todate': todate, 'PrevRate': PrevRate, 'NewRate': NewRate, 'rateon': rateon, 'calon': calon, 'ohon': ohon, 'ohcost': ohcost, 'mstdsnf': mstdsnf, 'pstdsnf': pstdsnf, 'snfpuls': snfpuls, 'msnfcost': msnfcost, 'psnfcost': psnfcost, 'mstdfat': mstdfat, 'pstdfat': pstdfat, 'fatpuls': fatpuls, 'mfatcost': mfatcost, 'pfatcost': pfatcost, 'mytype': mytype, 'Remarks': Remarks };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
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
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
          <i class="fa fa-usd fa-2x" aria-hidden="true" style="font-size: inherit"></i>  Vendor TS Rates<small>Preview</small>
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
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vendor TS Rate Change Details
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
                                <option>Skim Milk</option>
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
