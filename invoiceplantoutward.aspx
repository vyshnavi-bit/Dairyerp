<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="invoiceplantoutward.aspx.cs" Inherits="invoiceplantoutward" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .container
        {
            max-width: 100%;
        }
        th
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        function CallPrint(strid) {
            document.getElementById("tbl_po_print").style.borderCollapse = "collapse";
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
    </script>
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
            $('#txtdate').val(yyyy + '-' + mm + '-' + dd);
            $('#txttodate').val(yyyy + '-' + mm + '-' + dd);
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

        function btn_getdcdetails_Click() {
            var fromdate = document.getElementById('txtdate').value;
            var todate = document.getElementById('txttodate').value;
            if (fromdate == "") {
                alert("Please select from date");
                return false;
            }
            if (todate == "") {
                alert("Please select to date");
                return false;
            }
            var data = { 'op': 'btn_getdcdetails_out_Click', 'fromdate': fromdate, 'todate': todate };
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
            var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;"></th><th scope="col" style="font-weight: bold;">Transaction No</th><th scope="col" style="font-weight: bold;">Invoice No</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;">VendorName</th><th scope="col" style="font-weight: bold;">VehicleNo	</th><th scope="col" style="font-weight: bold;">Chemist</th><th scope="col" style="font-weight: bold;">Remarks</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<th><button id="btn_Print" type="button"   onclick="btn_getrefnowisedcdetails_Click(this);"  name="Edit" class="btn btn-primary" ><i class="fa fa-print"></i> Print</button></th>'
                results += '<td data-title="Code" class="1">' + msg[i].dcno + '</td>';
                results += '<td data-title="Code" class="2">' + msg[i].invoiceno + '</td>';
                results += '<td data-title="Code" class="3">' + msg[i].date + '</td>';
                results += '<td data-title="Code" class="4">' + msg[i].sectionid + '</td>';
                results += '<td data-title="Code" class="5">' + msg[i].vehicalno + '</td>';
                results += '<td data-title="Code" class="6">' + msg[i].Chemist + '</td>';
                results += '<td data-title="Code" class="7">' + msg[i].Remarks + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divdcdata").html(results);
        }

        function btn_getrefnowisedcdetails_Click(thisid) {
            var refno = "";
            var invoiceno = "";
            refno = $(thisid).parent().parent().children('.1').html();
            invoiceno = $(thisid).parent().parent().children('.2').html();
            var fromdate = document.getElementById('txtdate').value;
            var todate = document.getElementById('txttodate').value
            var data = { 'op': 'btn_getrefnowisedcdetails_out_Click', 'refno': refno, 'fromdate': fromdate, 'todate': todate, 'invoiceno': invoiceno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var main_details = msg[0].Dispathentrydetails;
                        var dc_sub_details = msg[0].Dispathsubdetails;
                        //document.getElementById('spnvendorname').innerHTML = main_details[0].desitnation;
                        document.getElementById('spntoaddress').innerHTML = main_details[0].toaddress;
                        document.getElementById('spntogstnno').innerHTML = main_details[0].togstnno;
                        document.getElementById('lbl_tosup_state').innerHTML = main_details[0].tostatename;
                        document.getElementById('spndcno').innerHTML = main_details[0].dcno;
                        document.getElementById('spnreferenceno').innerHTML = main_details[0].sno;
                        document.getElementById('spnvehicleno').innerHTML = main_details[0].vehicalno;
                        document.getElementById('spndcdate').innerHTML = main_details[0].date;
                        document.getElementById('spnAddress').innerHTML = main_details[0].address;
                        document.getElementById('spnstate').innerHTML = main_details[0].fromstate;
                        //                        if (main_details[0].fromstate == main_details[0].tostatename) {
                        //                            document.getElementById('spnheading').innerHTML = "STOCK TRANSFER";
                        //                        }
                        //                        else {
                        //                            document.getElementById('spnheading').innerHTML = "TAX INVOICE";
                        //                        }
                        var titlename = main_details[0].titlename;
                        var fromname = main_details[0].sectionid;
                        var dfromname = "";
                        if (fromname == "SVDS.P.LTD PUNABAKA PLANT") {
                            dfromname = titlename;
                        }
                        else {
                            dfromname = fromname;
                        }
                        document.getElementById('spnfromname').innerHTML = dfromname;
                        document.getElementById('spnfrmaddress').innerHTML = main_details[0].address;
                        document.getElementById('spnfrmgstin').innerHTML = main_details[0].gstinno;
                        document.getElementById('spnfrmstatename').innerHTML = main_details[0].fromstate;
                        document.getElementById('spnfrmstatecode').innerHTML = main_details[0].gststatecode;
                        document.getElementById('spndos').innerHTML = main_details[0].date;
                        document.getElementById('lbltovendorphoneno').innerHTML = main_details[0].mobno;
                        document.getElementById('lbltovendoremail').innerHTML = main_details[0].email;
                        document.getElementById('spn_tostatecode').innerHTML = main_details[0].togststatecode;
                        document.getElementById('spn_remarks').innerHTML = main_details[0].remarks;
                        document.getElementById('spn_dctime').innerHTML = main_details[0].expectedtime;
                        document.getElementById('lblcmp').innerHTML = "For &nbsp" + dfromname;
                        if (main_details[0].fromstate == main_details[0].tostatename) {
                            if ((main_details[0].frmcompanycode == main_details[0].tocompanycode)) {
                                document.getElementById('spnheading').innerHTML = "STOCK TRANSFER";
                                document.getElementById('spn_headercompanyname').innerHTML = titlename;
                                document.getElementById('spnvendorname').innerHTML = main_details[0].tovendorname;
                                document.getElementById('lbldcid').innerHTML = "DC No.:";
                                document.getElementById('lbldcdateid').innerHTML = "DC Date.:";
                                document.getElementById('lbldctimei').innerHTML = "DC Time.:";
                            }
                            else {
                                document.getElementById('spnheading').innerHTML = "TAX INVOICE/BILL OF SUPPLY";
                                document.getElementById('spn_headercompanyname').innerHTML = titlename;
                                document.getElementById('spnvendorname').innerHTML = main_details[0].tovendorname;
                                document.getElementById('lbldcid').innerHTML = "Invoice No.:";
                                document.getElementById('lbldcdateid').innerHTML = "Invoice Date.:";
                                document.getElementById('lbldctimei').innerHTML = "Invoice Time.:";
                                document.getElementById('spn_jurisdiction').innerHTML = main_details[0].jurisdiction;
                            }
                        }
                        else {
                            document.getElementById('spnheading').innerHTML = "TAX INVOICE/BILL OF SUPPLY";
                            document.getElementById('spn_headercompanyname').innerHTML = titlename;
                            document.getElementById('spnvendorname').innerHTML = main_details[0].tovendorname;
                            document.getElementById('lbldcid').innerHTML = "Invoice No.:";
                            document.getElementById('lbldcdateid').innerHTML = "Invoice Date.:";
                            document.getElementById('lbldctimei').innerHTML = "Invoice Time.:";
                            document.getElementById('spn_jurisdiction').innerHTML = main_details[0].jurisdiction;
                        }
                        filldcdetails(dc_sub_details);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldcdetails(msg) {
            var results = '<div><table id = "tbl_po_print" class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr style="background: antiquewhite;"><th value="#" colspan="1" style = "font-size: 12px;" rowspan="2">Sno</th><th value="Item Code" style = "font-size: 12px;" colspan="1" rowspan="2">Item Code</th><th style = "font-size: 12px;" value="Item Name" colspan="1" rowspan="2">Item Description</th><th style = "font-size: 12px;" value="HSN CODE" colspan="1" rowspan="2">HSN CODE</th><th value="UOM" style = "font-size: 12px;" colspan="1" rowspan="2">UOM</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="2">Qty</th><th value="KGFATTSRate" style = "font-size: 12px;" colspan="1" rowspan="2">KGFAT/ TSRate</th><th value="Rate/Item (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Rate</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Taxable Value</th><th value="OverHeadCost" style = "font-size: 12px;" colspan="1" rowspan="2">Over Head Cost</th><th value="DiffFATSNFCost" style = "font-size: 12px;" colspan="1" rowspan="2">DiffFAT/ SNFCost</th><th value="CGST" style = "font-size: 12px;" colspan="2" rowspan="1">SGST</th><th value="SGST" colspan="2" style = "font-size: 12px;" rowspan="1">CGST</th><th value="IGST" style = "font-size: 12px;" colspan="2" rowspan="1">IGST</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Total Amount</th></tr><tr style="background: antiquewhite;"><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th value="Amt (Rs.)" colspan="1" rowspan="1" style = "font-size: 12px;">Amt (Rs.)</th></tr></thead>';
            var j = 1;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var ohcost = 0;
            var difffatsnfcost = 0;
            var tot_qty = 0;
            var tot_taxablevalue = 0;
            var tot_sgstamount = 0;
            var tot_cgstamount = 0;
            var tot_igstamount = 0;
            var tot_totalamount = 0;
            var tot_ohcost = 0;
            var tot_difffatsnfcost = 0;
            var remarks = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><th scope="row" class="1" style = "font-size: 11px;">' + j + '</th>';
                //var rate = 100;
                var sgst_per = 0;
                var cgst_per = 0;
                var igst_per = 0;
                var gsttaxvalue = 0;
                var frmgstcode = msg[i].dfromgstcode;
                var togstcode = msg[i].dtogstcode;
                var fromcompanycode = msg[i].dfromcompanycode;
                var tocompanycode = msg[i].dtocompanycode;
                var milkvalue = msg[i].milkvalue;
                var milktype = msg[i].milktype;
                var taxval = 0;
                if (milktype == "Condensed") {
                    taxval = milkvalue;
                    if (frmgstcode == togstcode) {
                        if (fromcompanycode == tocompanycode) {
                            tot_amt = taxval;
                            sgst_per = 0;
                            cgst_per = 0;
                            igst_per = 0;
                            sgst_amt = (taxval * sgst_per) / 100 || 0;
                            cgst_amt = (taxval * cgst_per) / 100 || 0;
                            igst_amt = (taxval * igst_per) / 100 || 0;
                            gsttaxvalue = parseFloat(sgst_amt) + parseFloat(cgst_amt) + parseFloat(igst_amt);
                        }
                        else {
                            var igst = 18;
                            sgst_per = igst / 2;
                            cgst_per = igst / 2;
                            igst_per = 0;
                            sgst_amt = (taxval * sgst_per) / 100 || 0;
                            cgst_amt = (taxval * cgst_per) / 100 || 0;
                            igst_amt = (taxval * igst_per) / 100 || 0;
                            gsttaxvalue = parseFloat(sgst_amt) + parseFloat(cgst_amt) + parseFloat(igst_amt);
                        }
                    }
                    else {
                        var igst = 18;
                        sgst_per = 0;
                        cgst_per = 0;
                        igst_per = igst;
                        sgst_amt = (taxval * sgst_per) / 100 || 0;
                        cgst_amt = (taxval * cgst_per) / 100 || 0;
                        igst_amt = (taxval * igst_per) / 100 || 0;
                        gsttaxvalue = parseFloat(sgst_amt) + parseFloat(cgst_amt) + parseFloat(igst_amt);
                    }
                }
                else {
                    taxval = milkvalue;
                    sgst_per = 0;
                    cgst_per = 0;
                    igst_per = 0;
                    tot_amt = taxval;
                    sgst_amt = (taxval * sgst_per) / 100 || 0;
                    cgst_amt = (taxval * cgst_per) / 100 || 0;
                    igst_amt = (taxval * igst_per) / 100 || 0;
                    gsttaxvalue = parseFloat(sgst_amt) + parseFloat(cgst_amt) + parseFloat(igst_amt);
                }
                var totalamount = parseFloat(taxval) + parseFloat(gsttaxvalue);
                remarks = msg[i].remarks;
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].cellname + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].milktype + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].hsncode + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].calucationon + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].qtykgs + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].kgfattsvalue + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].costs + '</td>';
                //results += '<td style = "font-size: 11px;text-align:center;">0</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].MValue + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].ohcost + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + msg[i].difffatsnfcost + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + sgst_per + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + sgst_amt.toFixed(2) + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + cgst_per + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + cgst_amt.toFixed(2) + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + igst_per + '</td>';
                results += '<td style = "font-size: 11px;text-align:center;">' + igst_amt.toFixed(2) + '</td>';

                tot_qty += parseFloat(msg[i].qtykgs);
                tot_taxablevalue += parseFloat(msg[i].MValue);
                tot_sgstamount += parseFloat(sgst_amt);
                tot_cgstamount += parseFloat(cgst_amt);
                tot_igstamount += parseFloat(igst_amt);
                tot_totalamount += parseFloat(totalamount);
                tot_ohcost += parseFloat(msg[i].ohcost);
                tot_difffatsnfcost += parseFloat(msg[i].difffatsnfcost);

                results += '<td style = "font-size: 11px;">' + totalamount + '</td></tr>';
                j++;
            }
            var Total = "Total";
            results += '<tr>';
            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="5"><label>' + Total + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_qty).toFixed(2) + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="2"><label></label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_taxablevalue).toFixed(2) + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_ohcost).toFixed(2) + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_difffatsnfcost).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_sgstamount).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_cgstamount).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_igstamount).toFixed(2) + '</label></td>';
            results += '<td style="font-size: 12px;"><label>' + parseFloat(tot_totalamount).toFixed(2) + '</label></td></tr>';
            //            results += '<tr><td></td></tr>';
            //            results += '<tr >'
            //            results += '<td data-title="brandstatus" class="2"></td>';
            //            results += '<td scope="row" class="1" colspan="10" style="text-align:left;font-size: 11px;"><label>' + remarks + '</label></td>';
            //            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="6"><label></label></td>';
            //            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="1"><label></label></td></tr>';
            results += '</tr></table></div>';
            results += '</table></div>';
            $("#div_SUBitemdetails").html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Invoice Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">Invoice Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Invoice Report
                </h3>
            </div>
            <div class="box-body">
                <div runat="server" id="d">
                    <table>
                        <tr>
                            <td>
                                <label>
                                    From Date:</label>
                            </td>
                            <td>
                                <input type="date" id="txtdate" class="form-control" />
                            </td>
                            <td>
                                <label>
                                    To Date:</label>
                            </td>
                            <td>
                                <input type="date" id="txttodate" class="form-control" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <%--<input id="btn_save" type="button" class="btn btn-primary" name="submit" value='Get Details' onclick="btnPODetails_click()" />--%>
                                <div class="input-group">
                                    <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-flash"></span> <span id="btn_save" onclick="btn_getdcdetails_Click();">Get Details</span>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div id="divdcdata" style="height: 300px; overflow-y: scroll;">
                    </div>
                </div>

                <br />
                <br />

                <div>
                        <%--<table>
                            <tr>
                                <td>
                                    <div>
                                        <table id="tbltrip">
                                            <tr>
                                                <td>
                                                   <label> Transaction No</label>
                                                </td>
                                                <td>
                                                    <input type="text" id="txtrefno" class="form-control" />
                                                </td>
                                                <td style="width:5px;"></td>
                                                <td>
                                                    <div class="input-group">
                                    <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-flash"></span> <span id="Span2" onclick="btn_getrefnowisedcdetails_Click();">Get Details</span>
                                    </div>
                                </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>--%>
                        <br></br>
                        <br />
                    </div>
                
              <div id="divPrint">
                <div id="divrowcnt" style="border: 2px solid gray;">
                    <div style="width: 17%; float: right; padding-top:5px;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi" width="100px" height="72px" />
                        <br />
                    </div>
                    <div style="border: 1px solid gray; height:8%;">
                        <div style="font-family: Arial; font-size: 20px; font-weight: bold; color: Black;text-align: center;" id="spn_headercompanyname">
                            <span></span>
                            <br />
                        </div>
                        <div style="width:68%;text-align: center;padding-left: 8%;">
                        <span id="spnAddress" style="font-size: 11px;"></span>
                        </div>
                        <div style="width:68%;text-align: center;padding-left: 8%;">
                        <span id="spnstate" style="font-size: 12px;"></span>
                        </div>
                    </div>
                    <div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray;background-color: antiquewhite;">
                        <span id="spnheading" style="font-size: 16px; font-weight: bold;"></span>
                    </div>
                    <div style="width: 100%;">
                        <table style="width: 100%;">
                        <tr>
                                <td style="text-align:center; font-weight: bold;">
                                 <label style="font-size: 12px;">
                                        Bill From </label>
                                  </td>
                                  <td style="text-align:center;">
                                  </td>
                            </tr>
                        <tr>
                                <td style="width: 49%;  padding-left:2%; border:2px solid gray;">
                                <label style="font-size: 13px;"><b>
                                        Name :</b></label>
                                    <span style="font-weight:bold;font-size:12px;" id="spnfromname"></span>
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                        Address :</b></label>
                                    <span id="spnfrmaddress" style="font-size: 10px;"></span>
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                        GSTIN :</b></label>
                                    <span id="spnfrmgstin" style="font-size: 12px;"></span> 
                                    <br />
                                    <label style="font-size: 13px;"><b>State Name:</b></label>
                                    <span id="spnfrmstatename" style="font-size: 12px;"></span>
                                    &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp
                                    <label style="font-size: 13px;"><b>State Code:</b></label>
                                    <span id="spnfrmstatecode" style="font-size: 12px;"></span>
                                </td>
                                
                                <td style="width: 49%; border:2px solid gray;padding-left:2%;">
                                    <label id="lbldcid" style="font-size: 13px; font-weight:bold !important;"><b></b></label>
                                    <span id="spndcno" style="font-size: 12px;"></span>
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                        Ref NO :</b></label>
                                    <span id="spnreferenceno" style="font-size: 12px;"></span>
                                    <br />
                                    <label id="lbldcdateid" style="font-size: 13px;font-weight:bold !important;"><b></b></label>
                                    <span id="spndcdate" style="font-size: 12px;"></span>
                                    <br />
                                    <label id="lbldctimei" style="font-size: 13px;font-weight:bold !important;"><b></b></label>
                                    <span id="spn_dctime" style="font-size: 12px;"></span>
                                    <br />
                                    <label style="font-size: 13px; display:none;"><b>
                                        Reverse Charge(Y/N) :</b></label>
                                    <span id="spn_reversecharge" style="font-size: 12px;display:none;">N</span>
                                    
                                </td>
                                
                            </tr>
                             <tr>
                                <td style="text-align:center; font-weight: bold;">
                                 <label style="font-size: 12px;">
                                        Bill To </label>
                                  </td>
                                  <td style="text-align:center;">
                                  </td>
                            </tr>
                            <tr>
                                <td style="width: 49%;  padding-left:2%; border:2px solid gray;">
                                <label style="font-size: 13px;"><b>
                                        Name :</b></label>
                                    <span style="font-weight:bold;font-size:12px;" id="spnvendorname"></span>
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                        Address :</b></label>
                                    <span id="spntoaddress" style="font-size: 12px;"></span>
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                        GSTIN :</b></label>
                                    <span id="spntogstnno" style="font-size: 12px;"></span> 
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                        Telephone no :</b></label>
                                    <span id="lbltovendorphoneno" style="font-size: 12px;"></span>
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                        Email Id :</b></label>
                                    <span id="lbltovendoremail" style="font-size: 12px;"></span>
                                    <br />
                                    <label style="font-size: 13px;"><b>State Name :</b></label>
                                    <span id="lbl_tosup_state" style="font-size: 12px;"></span>
                                    &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp
                                    <label style="font-size: 13px;"><b>State Code:</b></label>
                                    <span id="spn_tostatecode" style="font-size: 12px;"></span>
                                </td>
                                <td style="width: 49%; border:2px solid gray;padding-left:2%;">
                                    <label style="font-size: 13px;"><b>
                                      Transport Mode :</b></label>
                                    <span id="Span9" style="font-size: 12px;">By Road</span>
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                        Vehicle No :</b></label>
                                    <span id="spnvehicleno" style="font-size: 12px;"></span>
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                        Date Of Dispatch :</b></label>
                                    <span id="spndos" style="font-size: 12px;"></span>
                                    <br />
                                    <label style="font-size: 13px;"><b>
                                       Place Of Dispatch :</b></label>
                                    <span id="spndispatchplace" style="font-size: 12px;"></span>
                                   
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div  style="border-bottom: 1px solid gray; border-top: 1px solid gray;text-align: center;">
                       <br />
                    </div>
                    <div id="div_SUBitemdetails">
                    </div>
                     <br />
                                <br />
                    <div style="text-align: -webkit-right;padding-right: 5%;">
                    <table>
                    <tr>
                    <label id="lblcmp" style="font-size: 12px;"><b> </b></label>
                    </tr>
                    <tr style="height:5%;">
                    <br />
                    <br />
                    </tr>
                    <tr>
                   
                    <label style="font-size: 13px;"><b>
                        AUTHORISED SIGNATURE</b></label>
                    <span id="Span3" style="font-size: 12px;"></span>
                                    
                    </tr>
                    </table>
                    </div>
                    <br />
                    <div>
                    <span style="font-weight: bold; font-size: 13px;">Remarks:</span>
                    <br />
                    <span style="font-size: 11px;" id="spn_remarks"></span>
                    </div>
                    <br />
                    <div>
                    <span style="font-weight: bold; font-size: 13px;">Declaration:</span>
                 <br />
                   <span style="font-size: 11px;">We declare that this invoice shows the actual price of the goods describe and that all particulars are true and correct.</span>
              <br />
                <span style="font-size: 11px;display:none;" id="spn_jurisdiction" ></span>
                <br />
                </div>
                </br>
                    </div>
                    </div>
                </div>
                <%--<input id="Button2" type="button" class="btn btn-primary" name="submit" style="display:none;" value='Print' onclick="javascript:CallPrint('divPrint');" />--%>
                 <div style="width: 20%;padding-left: 50%;padding-bottom: 2%;">
                <div class="input-group" id="Button2" style="padding-right: 90%;">
                    <div class="input-group-addon">
                        <span class="glyphicon glyphicon-print" onclick="javascript: CallPrint('divPrint');"></span> <span id="Span1" onclick="javascript: CallPrint('divPrint');">Print</span>
                    </div>
                </div>
                </div>
                <asp:Label ID="lblmsg" runat="server" Font-Size="20px" ForeColor="Red" Text=""></asp:Label>
            </div>
            </div>
    </section>
</asp:Content>
