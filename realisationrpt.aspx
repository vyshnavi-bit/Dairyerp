<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="realisationrpt.aspx.cs" Inherits="realisationrpt" %>

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
            $('#txtyear').val(yyyy);
            get_Batchs();
            get_Branch_details();
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
        function get_Branch_details() {
            var data = { 'op': 'get_Branch_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbranches(msg);
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
        function fillbranches(msg) {
            var data = document.getElementById('slct_branchname');
            var length = data.options.length;
            document.getElementById('slct_branchname').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Branch Name";
            opt.value = "";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].branchName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].branchName;
                    option.value = msg[i].Sno;
                    data.appendChild(option);
                }
            }
        }
        function get_pakingentry_details() {
            var data = { 'op': 'get_pakingentry_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillpakingentrydetails(msg);
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
        function fillpakingentrydetails(msg) {
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Batch Name</th><th scope="col">Product Name</th><th scope="col">Silo Name</th><th scope="col">Date</th><th scope="col">Quantity(ltrs)</th><th scope="col">Received Film</th><th scope="col">Cutting Film</th><th scope="col">Wastage Film</th><th scope="col">Return Film</th><th scope="col">Consumption Film</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].batch + '</td>';
                results += '<td data-title="Capacity" class="2" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].siloname + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].date + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].qtyltrs + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].receivedfilm + '</td>';
                results += '<td  class="7" style="text-align:center;">' + msg[i].cuttingfilm + '</td>';
                results += '<td  class="8" style="text-align:center;">' + msg[i].wastagefilm + '</td>';
                results += '<td  class="9" style="text-align:center;">' + msg[i].returnfilm + '</td>';
                results += '<td  class="10" style="text-align:center;">' + msg[i].consumptionfilm + '</td>';
                results += '<td style="display:none" class="17">' + msg[i].fat + '</td>';
                results += '<td style="display:none" class="16">' + msg[i].snf + '</td>';
                results += '<td style="display:none" class="15">' + msg[i].clr + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].batchid + '</td>';
                results += '<td style="display:none" class="13">' + msg[i].siloid + '</td>';
                results += '<td style="display:none" class="14">' + msg[i].sno + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }

        function getme(thisid) {

            var sno = $(thisid).parent().parent().children('.14').html();
            var siloid = $(thisid).parent().parent().children('.13').html();
            var batchid = $(thisid).parent().parent().children('.12').html();

            var productid = $(thisid).parent().parent().children('.11').html();

            var consumptionfilm = $(thisid).parent().parent().children('.10').html();
            var returnfilm = $(thisid).parent().parent().children('.9').html();
            var wastagefilm = $(thisid).parent().parent().children('.8').html();
            var cuttingfilm = $(thisid).parent().parent().children('.7').html();
            var receivedfilm = $(thisid).parent().parent().children('.6').html();
            var qtyltrs = $(thisid).parent().parent().children('.5').html();
            var date = $(thisid).parent().parent().children('.4').html();

            var fat = $(thisid).parent().parent().children('.17').html();
            var snf = $(thisid).parent().parent().children('.16').html();
            var clr = $(thisid).parent().parent().children('.15').html();

            document.getElementById('slct_Batch').value = batchid;
            document.getElementById('slct_product').value = productid;
            document.getElementById('ddlsource').value = siloid;

            document.getElementById('txt_qtyltrs').value = qtyltrs;
            document.getElementById('txt_fat').value = fat;
            document.getElementById('txt_snf').value = snf;

            document.getElementById('txt_clr').value = clr;
            document.getElementById('txt_consumption').value = consumptionfilm;
            document.getElementById('txt_receivedfilm').value = receivedfilm;

            document.getElementById('txt_returnfilm').value = returnfilm;
            document.getElementById('txt_Wastage').value = wastagefilm;
            document.getElementById('txt_cuttingfilm').value = cuttingfilm;

            document.getElementById('lbl_sno').value = sno;
            document.getElementById('save_batchdetails').value = "Modify";
            $("#div_Deptdata").hide();
            $("#realisation_fillform").show();
            $('#showlogs').hide();
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
        function fillproductdetails() {
            var batch = document.getElementById('slct_Batch').value;
            var month = document.getElementById('slct_mnth').value;
            var year = document.getElementById('txtyear').value;
            if (batch == "" || batch == "Select Batch") {
                alert("Please select batch Type");
                return false;
            }
            if (month == "" || month == "Select Month") {
                alert("Please select Month ");
                return false;
            }
            var data = { 'op': 'get_batch_productstdrate', 'batch': batch, 'month': month, 'year': year };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $("#divFillScreen").show();
                        var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Product Name</th><th scope="col">St.Rate</th><th scope="col">Packing Charge</th><th scope="col">Over Heads</th><th scope="col">Total</th><th scope="col">Realisation</th><th scope="col">Gain/Loss</th><th scope="col">Total Ltrs</th><th scope="col">Gain Amount</th><th scope="col">Loss Amount</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].packingcharges != "") {
                                results += '<tr>';
                                results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                                results += '<th><span id="txt_productname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].productname + '</span></th>';
                                results += '<td><input id="txt_strate" class="form-control" value="' + msg[i].stdrate + '" type="number" readonly name="vendorcode" placeholder="Enter ST.Rate"></td>';
                                results += '<td><input id="txt_packingrate" class="form-control" value="' + msg[i].packingcharges + '" type="number" readonly name="vendorcode" placeholder="Enter Packing Rate"></td>';
                                results += '<td><input id="txt_overheads" class="form-control" value="" type="number" name="vendorcode" onkeyup="overheadchange(this);" placeholder="Enter Over Heads"></td>';
                                results += '<td><input id="txt_total" class="form-control" value="" type="number" name="vendorcode" readonly placeholder="Enter Total"></td>';
                                results += '<td><input id="txt_Realisation" class="form-control" value="" type="number" name="vendorcode" onkeyup="realisationchange(this);" placeholder="Enter Realisation"></td>';
                                results += '<td><input id="txt_gainloss" class="form-control" value="" type="number" readonly name="vendorcode" placeholder="Enter gainloss"></td>';
                                results += '<td><input id="txt_totalltrs" class="form-control" value="" type="number" name="vendorcode" onkeyup="totalltrschange(this);" placeholder="Enter totalltrs"></td>';
                                results += '<td><input id="txt_gainamount" class="form-control" value="" type="number" readonly name="vendorcode" placeholder="Enter gainamount"></td>';
                                results += '<td><input id="txt_lossamount" class="form-control" value="" type="number" readonly name="vendorcode" placeholder="Enter lossamount"></td>';
                                results += '<td style="display:none" class="8"><input id="hdn_productid" class="form-control" type="number" name="vendorcode" value="' + msg[i].productid + '"></td></tr>';
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
        function overheadchange(qtyid) {
            var sum = 0;
            var packingrate = 0;
            var strate = $(qtyid).closest("tr").find('#txt_strate').val();
            strate = parseFloat(strate);
            packingrate = $(qtyid).closest("tr").find('#txt_packingrate').val();
            if (packingrate != "") {
                packingrate = parseFloat(packingrate);
            }
            else {
                packingrate = 0;
            }
            var overheadvalue = parseFloat(qtyid.value);
            sum = strate + packingrate;
            sum = sum + overheadvalue;
            $(qtyid).closest("tr").find('#txt_total').val(sum);
        }
        function realisationchange(thisid) {
            var gain = 0;
            var total = $(thisid).closest("tr").find('#txt_total').val();
            var realizationvalue = parseFloat(thisid.value);
            gain = realizationvalue - total;
            $(thisid).closest("tr").find('#txt_gainloss').val(gain);
        }
        function totalltrschange(thisid) {
            var gainamount = 0;
            var gainloss = $(thisid).closest("tr").find('#txt_gainloss').val();
            var realizationval = parseFloat(thisid.value);
            gainamount = gainloss * realizationval;
            if (gainamount > 0) {
                $(thisid).closest("tr").find('#txt_gainamount').val(gainamount);
                var ga = 0;
                $(thisid).closest("tr").find('#txt_lossamount').val(ga);
            }
            else {
                $(thisid).closest("tr").find('#txt_lossamount').val(gainamount);
                var ga = 0;
                $(thisid).closest("tr").find('#txt_gainamount').val(ga);
            }
        }
        function save_realizationdetails_click() {
            var branchname = document.getElementById('slct_branchname').value;
            if (branchname == "" || branchname == "Select Branch Name") {
                alert("Please Select Branch Name");
                return false;
            }
            var batch = document.getElementById('slct_Batch').value;
            var month = document.getElementById('slct_mnth').value;
            var year = document.getElementById('txtyear').value;
            var sno = document.getElementById('lbl_sno').value;
            var btnvalue = document.getElementById('save_realizationdetails').value;
            
            if (batch == "" || batch == "Select Batch") {
                alert("Please select batch Type");
                return false;
            }
            var rows = $("#table_shift_wise_details tr:gt(0)");
            var batch_wise_realizationdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_strate').val() != "") {
                    batch_wise_realizationdetails.push({ productid: $(this).find('#hdn_productid').val(), strate: $(this).find('#txt_strate').val(), packingrate: $(this).find('#txt_packingrate').val(), overheads: $(this).find('#txt_overheads').val(), total: $(this).find('#txt_total').val(), realisation: $(this).find('#txt_Realisation').val(), gainloss: $(this).find('#txt_gainloss').val(), totalltrs: $(this).find('#txt_totalltrs').val(), gainammount: $(this).find('#txt_gainamount').val(), lossammount: $(this).find('#txt_lossamount').val() });
                }
            });
            if (batch_wise_realizationdetails.length == 0) {
                alert("Please enter new quantity");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_realizationdetails_click', 'branchname': branchname,'sno':sno, 'month': month, 'year': year, 'batch': batch, 'sno': sno, 'btnvalue': btnvalue,
                    'batch_wise_realizationdetails': batch_wise_realizationdetails
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            //Clearvalues();
                            get_pakingentry_details();
                            $('#realisation_fillform').css('display', 'none');
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
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Realization Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Realization Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Realization Details 
                </h3>
            </div>
            <div style="padding-left: 300px;">
                <table>
                <tr>
                <td>
                    <label>
                        Branch Name <span style="color: red;">*</span></label>
                    <select type="text" class="form-control" id="slct_branchname" ></select>
                 </td>
                  <td style="width: 5px;">
                        </td>
                        <td>
                            <label>
                                Month <span style="color: red;">*</span></label>
                            <select id="slct_mnth" class="form-control">
                                <option selected disabled value="Select Month">Select Month</option>
                                <option value="1">Jan</option>
                                <option value="2">Feb</option>
                                <option value="3">Mar</option>
                                <option value="4">Apr</option>
                                <option value="5">May</option>
                                <option value="6">Jun</option>
                                <option value="7">Jul</option>
                                <option value="8">Aug</option>
                                <option value="9">Sep</option>
                                <option value="10">Oct</option>
                                <option value="11">Nov</option>
                                <option value="12">Dec</option>
                            </select>
                        </td>
                     </tr>
                      <tr>
                        <td>
                            <label>
                                Year <span style="color: red;">*</span></label>
                            <input type="text" class="form-control" id="txtyear" />
                        </td>
                     <td style="width: 5px;">
                        </td>
                        <td rowspan="3">
                            <label>
                                Batch Type<span style="color: red;">*</span></label>
                            <select id="slct_Batch" class="form-control" onchange="fillproductdetails();">
                                <option selected disabled value="Select SILO No">Select Batch Type</option>
                            </select>
                            <label id="lbl_silo" class="errormessage">
                                * Please Batch Type</label>
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
            <div style="padding-left: 410PX;">
                <input id='save_realizationdetails' type="button" class="btn btn-success" name="submit"
                    value='Finalize' onclick="save_realizationdetails_click()" />
            </div>
        </div>
    </section>
</asp:Content>
