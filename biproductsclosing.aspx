<%@ Page Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="biproductsclosing.aspx.cs" Inherits="biproductsclosing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            fillgheeproductsales();
            get_biproductsdetails_click();
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
            $('#txt_sdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);

            $('#btnclear').click(function () {
                $('#div_getbpclose').show();
                $('#fill_details').show();
                $('#div_editbpclose').hide();
                fillgheeproductsales();
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
        function save_ghee_closing_click() {
        }
        function fillgheeproductsales() {
            var data = { 'op': 'get_product_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var results = '<div    style="overflow:auto;"><table id="table_sales_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr ><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Quantity</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].departmentid == "1" && msg[i].batchid == "16") {
                                results += '<tr>';
                                results += '<th><span id="Span1" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                results += '<td><input id="txt_sales" class="form-control" value="' + msg[i].quantity + '" type="number" name="vendorcode"placeholder="Enter qty"></td>';
                                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="number" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
                            }
                        }
                        results += '</table></div>';
                        $("#divsales").html(results);
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
        function save_biproduct_sales_click() {
            var remarks = document.getElementById('txt_sRemarks').value;
            var date = document.getElementById('txt_sdate').value;
            var btnvalue = document.getElementById('btnsales').innerHTML;
            var sno = document.getElementById('lbl_ssno').value;
            var rows = $("#table_sales_wise_details tr:gt(0)");
            var ghee_closing_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_dispatchqty').val() != "") {
                    ghee_closing_details.push({ productid: $(this).find('#hdn_productid').val(), quantity: $(this).find('#txt_sales').val(), sno: $(this).find('#spn_sno').text() });
                }
            });
            if (ghee_closing_details.length == 0) {
                alert("Please enter opening balance");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_biproducts_closing_click', 'ghee_closing_details': ghee_closing_details, 'date': date, 'remarks': remarks, 'btnvalue': btnvalue, 'sno': sno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_biproductsdetails_click();
                            $('#div_getbpclose').show();
                            $('#fill_details').show();
                            $('#div_editbpclose').hide();
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
        function salesclearvalues() {
            document.getElementById('btnsales').innerHTML = "Save";
            $('#div_getbpclose').show();
            $('#fill_details').show();
            $('#div_editbpclose').hide();
            fillgheeproductsales();
        }
        function get_biproductsdetails_click() {
            var data = { 'op': 'get_biproductsdetails_click' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbiprodtsdetails(msg);
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
        function fillbiprodtsdetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Product Name</th><th scope="col" style="font-weight: bold;">Quantity</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><input id="btn_poplate" type="button"  onclick="getmebpclose(this)" name="submit" class="btn btn-success" value="Edit" /></td>';
                results += '<td class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td class="2" style="text-align:center;">' + msg[i].quaninty + '</td>';
                results += '<td class="3" style="text-align:center;">' + msg[i].doe + '</td>';
                results += '<td style="display:none" class="4">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="5">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmebpclose(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_getbpclose").html(results);
        }
        function getmebpclose(thisid) {
            var productname = $(thisid).parent().parent().children('.1').html();
            var quaninty = $(thisid).parent().parent().children('.2').html();
            var doe = $(thisid).parent().parent().children('.3').html();
            var productid = $(thisid).parent().parent().children('.4').html();
            var sno = $(thisid).parent().parent().children('.5').html();

            document.getElementById('txt_sdate').value = doe;
            document.getElementById('btnsales').innerHTML = "Modify";

            var results = '<div    style="overflow:auto;"><table id="table_sales_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Product Name</th><th scope="col" style="font-weight: 700;">Quantity</th></tr></thead></tbody>';
            results += '<tr>';
            results += '<th><span id="spn_sno" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + sno + '</span></th>';
            results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: 700; color: #0252aa;">' + productname + '</span></th>';
            results += '<td><input id="txt_sales" class="form-control" value="' + quaninty + '" type="number" name="vendorcode"placeholder="Enter qty"></td>';
            results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="number" name="vendorcode" value="' + productid + '"></td></tr>';
            results += '</table></div>';
            $("#divsales").html(results);
            $('#div_getbpclose').hide();
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
           Curd Bi-Products Closing Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#">Curd Bi-Products Closing Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-body">
                <div id="divproductionsales">
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Curd Bi-Products Closing Details</h3>
                        </div>
                    </div>
                    <div id="fill_details">
                    <div style="padding-left: 40%;" >
                        <table>
                            <tr>
                                <label>
                                    Date<span style="color: red;">*</span></label>
                                <input id="txt_sdate" class="form-control" type="datetime-local" name="vendorcode"
                                    style="width: 200px;" placeholder="Enter Date">
                            </tr>
                        </table>
                    </div>
                    <div id="divsales">
                    </div>
                    <div style="padding-left: 36%;">
                        <table align="center">
                            <tr hidden>
                                <td>
                                    <label id="lbl_ssno">
                                    </label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Remarks</label>
                                    <textarea rows="3" cols="45" id="txt_sRemarks" class="form-control" maxlength="200"
                                        placeholder="Enter Remarks"></textarea>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    <input id='btnsales' type="button" class="btn btn-success" name="submit" value='Save'
                                        onclick="save_biproduct_sales_click()" />
                                    <input id='btnclear' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                        onclick="salesclearvalues()" />
                                </td>
                            </tr>--%>
                        </table>
                        <div  style="padding-left: 15%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btnsales1" onclick="save_biproduct_sales_click()"></span><span id="btnsales" onclick="save_biproduct_sales_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btnclear1' onclick="salesclearvalues()"></span><span id='btnclear' onclick="salesclearvalues()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                    </div>
                </div>
                <div id="div_getbpclose">
                </div>
            </div>
         </div>
    </section>
</asp:Content>
