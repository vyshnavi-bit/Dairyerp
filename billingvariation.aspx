<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="billingvariation.aspx.cs" Inherits="billingvariation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            get_Vendor_details();
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

        function get_tankeroutworddetails_click() {
            var vendername = document.getElementById('ddlvendorname').value;
            var fromdate = document.getElementById('txt_fromdate').value;
            var todate = document.getElementById('txt_todate').value;
            var data = { 'op': 'get_tankeroutworddetails_click', 'vendername': vendername, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filltankeroutworddetails(msg);
                    }
                    else {
                        $("#div_Deptdata").empty();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filltankeroutworddetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="text-align: center !important;">DC No</th><th scope="col">Vehicle No</th><th scope="col">Milk Type</th><th scope="col">Qty(ltrs)</th><th scope="col">Qty(kgs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">VariationQty</th><th scope="col">VariationFat</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td scope="row" class="1"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtdcno" readonly value="' + msg[i].dcno + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtvno" readonly value="' + msg[i].vehicleno + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtmilktype" readonly value="' + msg[i].milktype + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtqtyltrs" readonly value="' + msg[i].qtyltrs + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtqtykgs" readonly value="' + msg[i].qtykgs + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtfat" readonly value="' + msg[i].fat + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtsnf" readonly value="' + msg[i].snf + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';

                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtrateon" readonly value="' + msg[i].rateon + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtcalcon" readonly value="' + msg[i].calcon + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtoverheadon" readonly value="' + msg[i].overheadon + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtoverheadcost" readonly value="' + msg[i].overheadcost + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtmstdsnf" readonly value="' + msg[i].mstdsnf + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtpstdsnf" readonly value="' + msg[i].pstdsnf + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtsnfpluson" readonly value="' + msg[i].snfpluson + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtmsnfpluson" readonly value="' + msg[i].msnfpluson + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtpsnfpluson" readonly value="' + msg[i].psnfpluson + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txttransporton" readonly value="' + msg[i].transporton + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';

                results += '<td data-title="Code" class="2"><input class="form-control" type="text" placeholder="Qty(kgs)"  name="Qtykg" id="txtvariationqty" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" class="2"><input class="form-control" type="text" placeholder="Fat"  name="Qtykg" id="txtvariationfat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Code" style="display:none;" class="2"><input id="hdn_vendorno" class="form-control" type="number" name="vendorcode" value="' + msg[i].vendorno + '"></td>';
                results += '<td data-title="Code" class="3"><input id="btn_poplate" type="button"  onclick="btnsave_billingvariation_click(this)" name="submit" class="btn btn-success" value="Save" /></td></tr>';
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function btnsave_billingvariation_click(thisid) {
            var vendorno = $(thisid).closest("tr").find("#hdn_vendorno").val();
            var dcno = $(thisid).closest("tr").find("#txtdcno").val();
            var vehicleno = $(thisid).closest("tr").find("#txtvno").val();
            var milktype = $(thisid).closest("tr").find("#txtmilktype").val();
            var qtyltrs = $(thisid).closest("tr").find("#txtqtyltrs").val();
            var qtykgs = $(thisid).closest("tr").find("#txtqtykgs").val();
            var fat = $(thisid).closest("tr").find("#txtfat").val();
            var snf = $(thisid).closest("tr").find("#txtsnf").val();
            var variationqty = $(thisid).closest("tr").find("#txtvariationqty").val();
            var variationfat = $(thisid).closest("tr").find("#txtvariationfat").val();

            var rateon = $(thisid).closest("tr").find("#txtrateon").val();
            var calcon = $(thisid).closest("tr").find("#txtcalcon").val();
            var overheadon = $(thisid).closest("tr").find("#txtoverheadon").val();
            var overheadcost = $(thisid).closest("tr").find("#txtoverheadcost").val();
            var mstdsnf = $(thisid).closest("tr").find("#txtmstdsnf").val();
            var pstdsnf = $(thisid).closest("tr").find("#txtpstdsnf").val();
            var snfpluson = $(thisid).closest("tr").find("#txtsnfpluson").val();
            var msnfpluson = $(thisid).closest("tr").find("#txtmsnfpluson").val();
            var psnfpluson = $(thisid).closest("tr").find("#txtpsnfpluson").val();
            var transporton = $(thisid).closest("tr").find("#txttransporton").val();
            var confi = confirm("Do you want to EDIT  Grade Details ?");
            if (confi) {
                var Data = { 'op': 'btnsave_billingvariation_click', 'vendorno': vendorno, 'dcno': dcno, 'vehicleno': vehicleno, 'milktype': milktype, 'qtyltrs': qtyltrs, 'qtykgs': qtykgs, 'fat': fat, 'snf': snf, 'variationqty': variationqty, 'variationfat': variationfat, 'rateon': rateon, 'calcon': calcon, 'overheadon': overheadon, '': overheadcost, '': mstdsnf, '': pstdsnf };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
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

        function get_Vendor_details() {
            var data = { 'op': 'get_Vendor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var Vendor = msg[0].vendordetails;
                        fillSource(Vendor);
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

        function fillSource(msg) {
            var data = document.getElementById('ddlvendorname');
            var length = data.options.length;
            document.getElementById('ddlvendorname').options.length = null;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Billing Variation<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Billing</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Billing Variation Details
                </h3>
            </div>
            <div class="box-body" style="padding-left:230px;">
                <table cellpadding="1px" align="center" style="width: 60%;">
                      <tr>
                      <td>
                         <label>
                                    Vendor Name</label>
                            <select id="ddlvendorname" class="form-control">
                                <option selected disabled value="Select Vendor Name">Select Vendor Name</option>
                            </select>
                        </td>
                        <td style="width:10px;">  
                       
                        </td>
                        <td>
                            <label>
                                    FromDate</label>
                                <input id="txt_fromdate" class="form-control" type="date" name="vendorcode"
                                    placeholder="Enter From Date" />
                            </td>
                         </tr>
                        <tr>
                          
                            <td>
                            <label>
                                    ToDate</label>
                                <input id="txt_todate" class="form-control" type="date" name="vendorcode"
                                    placeholder="Enter To Date" />
                            </td>
                            <td></td>
                            <td>
                                 <input id='btnget' type="button" class="btn btn-success" name="submit"
                                    value='Get' onclick="get_tankeroutworddetails_click()" />
                            </td>
                        </tr>
                </table>
            </div>
             <div id="div_Deptdata">
                </div>
        </div>
    </section>
</asp:Content>
