<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="filmratechange.aspx.cs" Inherits="filmratechange" %>

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
        $(function () {
            get_product_details();
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
        function get_product_details() {
            var data = { 'op': 'get_productfilimratechange_details' };
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
            var results = '<div    style="overflow:auto;"><table id="table_Vendor_TSRate_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Product Name</th><th scope="col">From Date</th><th scope="col">To Date</th><th scope="col">Prev Rate</th><th scope="col">New Rate</th><th scope="col">Remarks</th><th></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].departmentid == 2) {
                    results += '<tr>';
                    results += '<th scope="row" class="1"><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                    results += '<th scope="row" class="1"><span id="txt_Vendorname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].productname + '</span></th>';
                    results += '<td class="5"><input id="txt_fromdate" class="fromdatecls" type="date" name="vendorcode"placeholder="Enter Date"></td>';
                    results += '<td class="5"><input id="txt_todate" class="todatecls" type="date" name="vendorcode"placeholder="Enter Date"></td>';
                    results += '<td class="5"><input id="txt_PrevRate" class="form-control" type="number" name="vendorcode"placeholder="Enter Rate"  value="' + msg[i].filimrate + '"></td>';
                    results += '<td class="5"><input id="txt_NewRate" class="form-control" type="number" name="vendorcode"placeholder="Enter Rate"></td>';
                    results += '<td class="5"><input id="txt_Remarks" class="form-control" type="text" name="vendorcode"placeholder="Enter Remarks"></td>';
                    //results += '<td><input id="btn_poplate" type="button"  onclick="btnsave_filim_rate_click(this)" name="submit" class="btn btn-success" value="For Approval" /></td>';
                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To For Approval!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 apprvcls"  onclick="btnsave_filim_rate_click(this)"><span class="glyphicon glyphicon-hand-right" style="top: 0px !important;"></span></button></td>';
                    results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="number" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                }
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
        function btnsave_filim_rate_click(thisid) {
            var productid = $(thisid).closest("tr").find("#hdn_productid").val();
            var fromdate = $(thisid).closest("tr").find("#txt_fromdate").val();
            var todate = $(thisid).closest("tr").find("#txt_todate").val();
            var PrevRate = $(thisid).closest("tr").find("#txt_PrevRate").val();
            var NewRate = $(thisid).closest("tr").find("#txt_NewRate").val();
            var Remarks = $(thisid).closest("tr").find("#txt_Remarks").val();
            var confi = confirm("Do you want save Film rates ?");
            if (confi) {
                var Data = { 'op': 'btnsave_filim_rate_click', 'productid': productid, 'fromdate': fromdate, 'todate': todate, 'PrevRate': PrevRate, 'NewRate': NewRate, 'Remarks': Remarks };
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
        function btn_cancel_click() {
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Film Rates<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">FILM Rates</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i> Film Rate Change Details
                </h3>
            </div>
            <div class="box-body">
                <div id="divFillScreen">
                </div>
            </div>
        </div>
    </section>
</asp:Content>


