<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="OutwardSilo.aspx.cs" Inherits="OutwardSilo" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            get_Silos();
            get_SiloDepartments();
            get_Batchs();
            $('#btn_addDept').click(function () {
                $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();

            });

            $('#btn_close').click(function () {
                $('#Inwardsilo_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                forclearall();
            });
            get_outward_silo_transaction();

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
        $(function () {
            var today = new Date();
            var dd = today.getDate() - 1;
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
            $('#txt_getdatadate').val(yyyy + '-' + mm + '-' + dd);
            get_outward_silo_transaction();
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
        function Clearvalues() {
            //document.getElementById('txt_date').value = "";
            document.getElementById('slct_Department').selectedIndex = 0;
            document.getElementById('slct_Source_Name').selectedIndex = 0;
            document.getElementById('txt_qtykgs').value = "";
            document.getElementById('txt_qtyltrs').value = "";
            document.getElementById('save_Outwardtransactions').value = "Save";
            document.getElementById('txt_fat').value = "";
            document.getElementById('txt_snf').value = "";
            document.getElementById('txt_clr').value = "";
            transsno = 0;
            //  document.getElementById('txt_fat').value = "";
            //  document.getElementById('txt_snf').value = "";
            // $("#lbl_date").hide();
            $("#lbl_department").hide();
            $("#lbl_silo").hide();
            // $("#lbl_clr").hide();


        }

        function LtrsChange(qtyid) {
            var qtyltr = 0;
            qtyltr = parseFloat(qtyid.value).toFixed(3);
            var clr = document.getElementById('txt_clr').value;
            if (clr == "") {
                clr = 30;
            }
            clr = parseFloat(clr).toFixed(3);
            var qtyltrkgs = 0;
            var modclr = (clr / 1000) + 1;
            modclr = parseFloat(modclr).toFixed(5);
            qtyltrkgs = qtyltr * modclr;
            qtyltrkgs = parseFloat(qtyltrkgs).toFixed(0);
            document.getElementById('txt_qtykgs').value = qtyltrkgs;
        }

        function fatChange(qtyid) {
            var fat = 0;
            var clr = 0;
            var department = document.getElementById('slct_Department').value;
            if (department == "3" || department == "10") {
                fat = document.getElementById('txt_fat').value;
                fat = parseFloat(fat).toFixed(3);
                var snfvalue = (100-fat)/11;
                document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);
            }
        }
        function LRChange(qtyid) {
            if (qtyid.value == "") {
                var qty = 0;
                var qtykg = 0;
                qtykg = document.getElementById('txt_qtykgs').value;
                qtykg = parseFloat(qtykg).toFixed(3);
                var qtykgsltr = 0;
                var clr = 0;
                clr = parseFloat(qty).toFixed(3);
                var modclr = (clr / 1000) + 1;
                modclr = parseFloat(modclr).toFixed(3);
                qtykgsltr = qtykg / modclr;
                qtykgsltr = parseFloat(qtykgsltr).toFixed(2);
                document.getElementById('txt_qtyltrs').value = qtykgsltr;
                //  $(qtyid).closest("tr").find('#txtltr').val(qtykgsltr);
            }
            else {
                var qtykg = 0;
                qtykg = document.getElementById('txt_qtykgs').value;
                if (qtykg == "") {
                    qtykg = parseFloat(qtykg).toFixed(3);
                    var qtykgsltr = 0;
                    var clr = 0;
                    clr = parseFloat(qtyid.value).toFixed(3);
                    var modclr = (clr / 1000) + 1;
                    modclr = parseFloat(modclr).toFixed(5);
                    qtykgsltr = qtykg / modclr;
                    qtykgsltr = parseFloat(qtykgsltr).toFixed(0);
                    document.getElementById('txt_qtyltrs').value = qtykgsltr;
                }
                else {
                    var qtyltr = document.getElementById('txt_qtyltrs').value;
                    qtyltr = parseFloat(qtyltr).toFixed(3);
                    var qtyltrkgs = 0;
                    var clr = 0;
                    clr = parseFloat(qtyid.value).toFixed(3);
                    var modclr = (clr / 1000) + 1;
                    modclr = parseFloat(modclr).toFixed(5);
                    qtyltrkgs = qtyltr * modclr;
                    qtyltrkgs = parseFloat(qtyltrkgs).toFixed(0);
                    document.getElementById('txt_qtykgs').value = qtyltrkgs;
                }

                ////////For SNF ////////////////
                var department = document.getElementById('slct_Department').value;
                if (department == "3") {
                    fat = document.getElementById('txt_fat').value;
                    fat = parseFloat(fat).toFixed(3);
                    var snfvalue = (100 - fat) / 11;
                    document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);
                }
                else {
                    var fat = 0;
                    fat = document.getElementById('txt_fat').value;
                    fat = parseFloat(fat).toFixed(3);
                    var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
                    document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);
                }

            }
        }
        function get_Silos() {
            var data = { 'op': 'get_Silo_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillfSilos(msg);
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
        function fillfSilos(msg) {
            var data = document.getElementById('slct_Source_Name');
            var length = data.options.length;
            document.getElementById('slct_Source_Name').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Silo";
            opt.value = "Select Silo";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].SiloName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].SiloName;
                    option.value = msg[i].SiloId;
                    data.appendChild(option);
                }
            }
        }


        function get_SiloDepartments() {
            var data = { 'op': 'get_SiloDepartments_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldepartments(msg);
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
        function filldepartments(msg) {
            var data = document.getElementById('slct_Department');
            var length = data.options.length;
            document.getElementById('slct_Department').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Department";
            opt.value = "Select Department";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].DeportmentName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].DeportmentName;
                    option.value = msg[i].SiloDeportmentId;
                    data.appendChild(option);
                }
            }
        }

        function checkquantity() {
            var siloid = document.getElementById('slct_Source_Name').value;
            var data = { 'op': 'silo_Quantitycheck_transaction', 'siloid': siloid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        checkdetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function checkdetails(msg) {
            var siloquantity = "";
            var Qtykgs = document.getElementById('txt_qtykgs').value;
            for (var i = 0; i < msg.length; i++) {
                siloquantity = msg[i].OutwordQuantitykgs;
            }
            if (Qtykgs > siloquantity) {
                alert("Given quantity is more then the existing Quantity");
                document.getElementById('txt_qtykgs').value = "";
            }
            else {


            }
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
            var data = document.getElementById('slct_product');
            var length = data.options.length;
            document.getElementById('slct_product').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Batch";
            opt.value = "Select Batch";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].batchtype != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].batchtype;
                    option.value = msg[i].batchid;
                    data.appendChild(option);
                }
            }
        }

        function get_outward_silo_transaction() {
            var getdatadate = document.getElementById('txt_getdatadate').value;
            var data = { 'op': 'get_outward_silo_transaction', 'getdatadate': getdatadate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillosilodetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillosilodetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col" style="text-align: center !important;">Branch Name</th><th scope="col">Silo Name</th><th scope="col">Dept Name</th><th scope="col">Qty(kgs)</th><th scope="col">Qty(Ltrs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th><th scope="col">Date</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-success" value="Edit" /></td>';
                results += '<td scope="row" class="1">' + msg[i].branchname + '</td>';
                results += '<td data-title="Code" class="2">' + msg[i].SiloName + '</td>';
                results += '<td data-title="Code" class="3">' + msg[i].Deportment + '</td>';
                results += '<td data-title="Code" class="4">' + msg[i].OutwordQuantitykgs + '</td>';
                results += '<td data-title="Code"  class="8">' + msg[i].OutwordQuantityltrs + '</td>';
                results += '<td data-title="Code"  class="9">' + msg[i].fat + '</td>';
                results += '<td data-title="Code"  class="10">' + msg[i].snf + '</td>';
                results += '<td data-title="Code" class="11">' + msg[i].clr + '</td>';
                results += '<td data-title="Code" class="12">' + msg[i].doe + '</td>';
                results += '<td data-title="Code" style="display:none;" class="5">' + msg[i].departmentid + '</td>';
                results += '<td data-title="Code" style="display:none;" class="7">' + msg[i].SiloId + '</td>';
                results += '<td data-title="Code" style="display:none;" class="6">' + msg[i].transno + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        var transsno = 0;
        function getme(thisid) {
            transsno = $(thisid).parent().parent().children('.6').html();
            var branchname = $(thisid).parent().parent().children('.1').html();
            var SiloName = $(thisid).parent().parent().children('.2').html();
            var Deportment = $(thisid).parent().parent().children('.3').html();
            var OutwordQuantitykgs = $(thisid).parent().parent().children('.4').html();
            var departmentid = $(thisid).parent().parent().children('.5').html();
            var SiloId = $(thisid).parent().parent().children('.7').html();
            var OutwordQuantityltrs = $(thisid).parent().parent().children('.8').html();
            var fat = $(thisid).parent().parent().children('.9').html();
            var snf = $(thisid).parent().parent().children('.10').html();
            var clr = $(thisid).parent().parent().children('.11').html();

            document.getElementById('slct_Source_Name').value = SiloId;
            document.getElementById('slct_Department').value = departmentid;
            document.getElementById('txt_qtykgs').value = OutwordQuantitykgs;
            document.getElementById('txt_qtyltrs').value = OutwordQuantityltrs;
            document.getElementById('txt_fat').value = fat;
            document.getElementById('txt_snf').value = snf;
            document.getElementById('txt_clr').value = clr;
            document.getElementById('save_Outwardtransactions').value = "Modify";
            $('#Inwardsilo_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
        }
        function save_outword_silotransaction_click() {
            var date = document.getElementById('txt_date').value;
            var Siloname = document.getElementById('slct_Source_Name').value;
            var Department = document.getElementById('slct_Department').value;
            var product = document.getElementById('slct_product').value;
            var Qtykgs = document.getElementById('txt_qtykgs').value;
            var Qtyltrs = document.getElementById('txt_qtyltrs').value;
            var fat = document.getElementById('txt_fat').value;
            var snf = document.getElementById('txt_snf').value;
            var clr = document.getElementById('txt_clr').value;
            var btnval = document.getElementById('save_Outwardtransactions').value;
            var flag = false;

            if (Department == "Select Department") {
                $("#lbl_department").show();
                flag = true;
            }

            if (product == "Select product") {
                $("#lbl_product").show();
                flag = true;
            }

            if (Siloname == "Select Silo") {
                $("#lbl_silo").show();
                flag = true;
            }
            if (Qtykgs == "") {
                $("#lbl_qty").show();
                flag = true;
            }

            if (flag) {
                return;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_outword_silo_transaction_click', 'Department': Department, 'product': product, 'Siloname': Siloname, 'Qtykgs': Qtykgs, 'Qtyltrs': Qtyltrs, 'btnval': btnval, 'transsno': transsno, 'fat': fat, 'snf': snf, 'clr': clr, 'date': date };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_outward_silo_transaction();
                            Clearvalues();
                            $('#div_Deptdata').show();
                            $('#Inwardsilo_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
          <i class="fa fa-sign-out" aria-hidden="true"></i>  Outward SILO<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Outward SILO</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Outward SILO
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <%--<input id="btn_addDept" type="button" name="submit" value='Add OutWard SILO' class="btn btn-success" />--%>
                    <table>
                    <tr>
                    <td style="width: 200px;">
                          </td>
                        <td>
                            <label>
                                Date <span style="color: red;">*</span></label>
                        </td>
                        <td style="width: 5px;">
                          </td>
                        <td>
                            <input id="txt_getdatadate" class="form-control" type="date" name="vendorcode" />
                        </td>
                         <td style="width: 5px;">
                          </td>
                        <td>
                        <input id="txt_getgenerate" type="button" name="submit" value='Generate'
                        class="btn btn-primary" onclick="get_outward_silo_transaction();"/>
                        </td>
                        <td style="width: 400px;">
                          </td>
                        <td>
                         <input id="btn_addDept" type="button" name="submit" value='Add OutWard SILO' class="btn btn-success" />-
                        </td>
                    </tr>
                </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='Inwardsilo_fillform' style="display: none; padding-left:250px;">
                    <table align="center">
                    <tr>
                    <td colspan="2"><label>
                                    Date <span style="color: red;">*</span></label>
                                 
                                <input id="txt_date" class="form-control"  type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                        </td>  
                    </tr>
                        <tr>
                            <td>
                                <label>
                                    SILO Name<span style="color: red;">*</span></label>
                                <select id="slct_Source_Name" class="form-control">
                                    <option selected disabled value="Select SILO Name">Select SILO Name</option>
                                </select>
                                <label id="lbl_silo" class="errormessage">
                                    * Please select Silo</label>
                            </td>
                            <td style="width: 3px;">
                            <td>
                                <label>
                                    Department<span style="color: red;">*</span></label>
                                <select id="slct_Department" class="form-control">
                                    <option selected disabled value="Select Department">Select Department</option>
                                </select>
                                <label id="lbl_department" class="errormessage">
                                    * Please select Department</label>
                            </td>
                        </tr>
                        <tr>
                        <td>
                         <label>
                                    Batch<span style="color: red;">*</span></label>
                                <select id="slct_product" class="form-control">
                                    <option selected disabled value="Select Product">Select Batch</option>
                                </select>
                                <label id="lbl_product" class="errormessage">
                                    * Please select Batch</label>
                        </td>
                         <td style="width: 3px;">
                            <td>
                                <label>
                                    Qty(Kgs)</label>
                                <input id="txt_qtykgs" type="text" class="form-control" name="vendorcode" readonly
                                    placeholder="Enter Qty in Kgs" >
                                <label id="lbl_qty" class="errormessage">
                                    * Required Quantity</label>
                            </td>
                           
                           
                        </tr>
                        <tr>
                         <td>
                                <label>
                                    Qty(ltrs)<span style="color: red;">*</span></label>
                                <input id="txt_qtyltrs" type="text" class="form-control" name="vendorcode" onchange="LtrsChange(this)" placeholder="Enter Qty in ltrs">
                            </td>
                              <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                    FAT<span style="color: red;">*</span></label>
                                <input id="txt_fat" type="text" class="form-control" name="vendorcode" placeholder="FAT" onkeyup="fatChange(this)">
                                <label id="lbl_fat" class="errormessage">
                                    * Please enter fat</label>
                            </td>
                          
                            
                        </tr>
                        <tr>
                        <td>
                                <label>
                                    SNF</label>
                                <input id="txt_snf" type="text" class="form-control" name="vendorcode" placeholder="SNF" readonly/>
                                <label id="lbl_snf" class="errormessage">
                                    * Please enter snf</label>
                            </td>
                              <td style="width: 3px;">
                            </td>
                            <td>
                                <label>
                                    CLR<span style="color: red;">*</span></label>
                                <input id="txt_clr" type="text" class="form-control" name="vendorcode" placeholder="SNF"
                                    onkeyup="LRChange(this)" />
                                <label id="lbl_clr" class="errormessage">
                                    * Please enter CLR</label>
                            </td>
                          
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input id='save_Outwardtransactions' type="button" class="btn btn-success" name="submit"
                                    value='Save' onclick="save_outword_silotransaction_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close' />
                                <input id='btnPrint' type="button" class="btn btn-primary" name="Close" value='Print'
                                    onclick="javascript:CallPrint('div_axilautofill');" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
