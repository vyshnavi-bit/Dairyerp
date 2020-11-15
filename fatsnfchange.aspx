<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="fatsnfchange.aspx.cs" Inherits="fatsnfchange" %>

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
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd);
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
        function get_sales_despatch_details() {
            var date = document.getElementById('txt_date').value;
            var data = { 'op': 'get_sales_despatch_details', 'date': date };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $("#div_getdetails").css('display', 'block');
                        var results = '<div    style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Category Name</th><th scope="col" style="font-weight: bold;">Qty(Ltrs)</th><th scope="col" style="font-weight: bold;">Qty(Kgs)</th><th scope="col" style="font-weight: bold;">FAT</th><th scope="col" style="font-weight: bold;">SNF</th></tr></thead></tbody>';
                        var k = 1;
                        var l = 0;
                        var COLOR = ["", "", "", ""];
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr style="background-color:' + COLOR[l] + '">';
                            results += '<td   class="1"><input class="form-control" type="text" placeholder="SubCatName" name="SubCatName" id="txtsubcatname" readonly value="' + msg[i].SubCatName + '" /></td>';
                            results += '<td  class="2"><input class="form-control" type="text"  placeholder="Qty(Ltrs)" name="QtyLtrs" id="txtqtyltrs" value="' + msg[i].qtyltrs + '" /></td>';
                            results += '<td  class="6"><input class="form-control" type="text"  placeholder="Qty(Kgs)" name="QtyKgs" id="qtykgs" value="' + msg[i].qtykgs + '" /></td>';
                            results += '<td  class="3"><input class="form-control" type="text"  placeholder="FAT" name="FAT" id="txtfat" value="' + msg[i].fat + '" /></td>';
                            results += '<td  class="4"><input class="form-control" type="text"  placeholder="SNF" name="SNF" id="txtsnf" value="' + msg[i].snf + '" /></td>';
                            results += '<td  class="7" style="display:none;"><input class="form-control" type="text" placeholder="productid" name="productid" value="' + msg[i].productid + '" id="txtsalesproductid" /></td>';
                            results += '<td  class="7" style="display:none;"><input class="form-control" type="text" placeholder="SNO" name="SNO" value="' + msg[i].sno + '" id="txtsno" /></tr>';
                            l = l + 1;
                            if (l == 4) {
                                l = 0;
                            }
                        }
                        results += '</table></div>';
                        $("#div_getdetails").html(results);
                    }
                    else {
                        $("#div_getdetails").css('display', 'none');
                    }
                }
                else {
                    $("#div_getdetails").css('display', 'none');
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function LRChange(qtyid) {
            var fat = 0;
            fat = $(qtyid).closest("tr").find('#txtfat').val();
            fat = parseFloat(fat) || 0;
            var clr = 0;
            clr = $(qtyid).closest("tr").find('#txtclr').val();
            if (clr != "" || clr != "NaN" || clr != "undefined" || clr != undefined) {
                var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
                $(qtyid).closest("tr").find('#txtsnf').val(snfvalue.toFixed(2));
                var modclr = (clr / 1000) + 1;
                var qtyltr = $(qtyid).closest("tr").find('#txtqtyltrs').val();
                var qtyltrkgs = qtyltr * modclr;
                $(qtyid).closest("tr").find('#txtqtykgs').val(qtyltrkgs.toFixed(2));
            }
        }
        function save_sales_fatsnf_details() {
            var date = document.getElementById('txt_date').value;
            if (date == "") {
                alert("Select Date.");
                return false;
            }
            var btnvalue = document.getElementById('btn_save').innerHTML;
            var MilkfatDetailsMilkarray = [];
            $('#tbl_milk_details> tbody > tr').each(function () {
                var SubCatName = $(this).find('[name="SubCatName"]').val();
                var Qtyltr = $(this).find('[name="QtyLtrs"]').val();
                var Qtykg = $(this).find('[name="QtyKgs"]').val();
                var fat = $(this).find('[name="FAT"]').val();
                var snf = $(this).find('[name="SNF"]').val();
                var sno = $(this).find('[name="SNO"]').val();
                var productid = $(this).find('[name="productid"]').val();
                if (fat == "") {
                }
                else {
                    MilkfatDetailsMilkarray.push({ 'SubCatName': SubCatName, 'Qtyltr': Qtyltr, 'Qtykg': Qtykg, 'fat': fat, 'snf': snf, 'sno': sno,
                        'productid': productid, 'btnvalue': btnvalue
                    });
                }
            });
            if (MilkfatDetailsMilkarray.length == "0") {
                alert("Please enter quantity in  SNF, CLR details");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_sales_fatsnf_details', 'MilkfatDetailsMilkarray': MilkfatDetailsMilkarray, 'date': date, 'btnvalue': btnvalue
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        $('#div_getdetails').css('display', 'none');
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

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Fat And Snf Change<small></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#">Fat And Snf Change</a></li>
        </ol>
    </section>
    <section class="content">
         <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Fat And Snf Change</h3>
                        </div>
            <div class="box-body">
                <div id="divproductionsales">
                    <div style="padding-left: 40%;">
                        <table>
                            <tr>
                            <td>
                                <label>
                                    Date<span style="color: red;">*</span></label>
                                <input id="txt_date" class="form-control" type="date">
                                    </td>
                                    <td style="width:2%;"></td>
                                    <td>
                                     <label>
                                    &nbsp<span style="color: red;"></span></label>
                                    <table>
                                    <tr>
                                    <td>
                                    <td>
                                    </td>
                                    <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                        <span  class="glyphicon glyphicon-refresh" onclick="get_sales_despatch_details()"></span> <span onclick="get_sales_despatch_details()">Generate</span>
                                    </div>
                                    </div>
                            </td>
                     </tr>
                     </table>
                     </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div id="div_getdetails">
                    </div>
                     <div style="text-align: -webkit-center;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="save_sales_fatsnf_details()"></span><span id="btn_save" onclick="save_sales_fatsnf_details()">Modify</span>
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
