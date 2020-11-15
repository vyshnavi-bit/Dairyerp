<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Collections.aspx.cs" Inherits="Collections" %>

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
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        });
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
        var PaymentType = "";
        function ddlPaymentTypeChange(Payment) {
            PaymentType = Payment.options[Payment.selectedIndex].text;
            if (PaymentType == "Cash") {
                $('.divChequeclass').css('display', 'none');
                $('.divChequeDateclass').css('display', 'none');
                $('.divBankclass').css('display', 'none');
            }
            if (PaymentType == "Bank Transfer") {
                $('.divChequeclass').css('display', 'none');
                $('.divChequeDateclass').css('display', 'none');
                $('.divBankclass').css('display', 'none');
            }
            if (PaymentType == "Cheque") {
                $('.divChequeclass').css('display', 'table-row');
                $('.divChequeDateclass').css('display', 'table-row');
                $('.divBankclass').css('display', 'table-row');
                document.getElementById("spnchequeno").innerHTML = "Cheque No";
                var input = document.getElementById("txtChequeNo");
                input.placeholder = "Enter Cheque No";
            }
            if (PaymentType == "DD") {
                $('.divChequeclass').css('display', 'table-row');
                $('.divChequeDateclass').css('display', 'table-row');
                $('.divBankclass').css('display', 'table-row');
                document.getElementById("spnchequeno").innerHTML = "DD No";
                var input = document.getElementById("txtChequeNo");
                input.placeholder = "Enter DD No";
            }
            if (PaymentType == "Journal Voucher") {
                $('.divChequeclass').css('display', 'table-row');
                document.getElementById("spnchequeno").innerHTML = "Voucher No";
                var input = document.getElementById("txtChequeNo");
                input.placeholder = "Enter Journal Voucher";
            }
        }
        function save_vendor_collection_click() {
            var vendorname = document.getElementById('ddlvendorname').value;
            var transtype = document.getElementById('ddltranstype').value;
            var paymenttype = document.getElementById('ddlPaymentType').value;
            var date = document.getElementById('txt_date').value;
            var chequeDate;
            var amount = document.getElementById('txt_amount').value;
            var remarks = document.getElementById('txtRemarks').value;
           
            if (vendorname == "" || vendorname=="Select Vendor Name") {
                alert("Enter vendor name");
                return false;
            }
            if (amount == "") {
                alert("Enter amount");
                return false;
            }
            if (remarks == "") {
                alert("Enter remarks");
                return false;
            }
            var txtChequeNo = "";
            var txtBankName = "";
            if (paymenttype == "Cheque") {
                txtChequeNo = document.getElementById('txtChequeNo').value;
                chequeDate = document.getElementById('dtchequedate').value;
                txtBankName = document.getElementById('txtBankName').value;
                if (txtChequeNo == "") {
                    alert("Enter Cheque No");
                    return false;
                }
                if (chequeDate == "") {
                    alert("Enter chequeDate");
                    return false;
                }
                if (txtBankName == "") {
                    alert("Enter Bank Name");
                    return false;
                }
            }
            if (paymenttype == "DD") {
                txtChequeNo = document.getElementById('txtChequeNo').value;
                if (txtChequeNo == "") {
                    alert("Enter DD No");
                    return false;
                }
            }
            if (paymenttype == "Journal Voucher") {
                txtChequeNo = document.getElementById('txtChequeNo').value;
                if (txtChequeNo == "") {
                    alert("Enter Journal Voucher");
                    return false;
                }
            }
            var data = { 'op': 'save_vendor_collection_click',  'vendorname': vendorname, 'transtype': transtype, 'date': date, 'paymenttype': paymenttype, 'amount': amount, 'remarks': remarks, 'txtChequeNo': txtChequeNo, 'chequeDate': chequeDate, 'txtBankName': txtBankName };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    forclearall();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall() {
            document.getElementById('ddlvendorname').selectedIndex = 0;
            document.getElementById('ddlPaymentType').selectedIndex = 0;
            document.getElementById('txt_amount').value = "";
            document.getElementById('txtChequeNo').value = "";
            document.getElementById('txtBankName').value = "";
            document.getElementById('dtchequedate').value = "";
            document.getElementById('txtRemarks').value = "";
            document.getElementById('btn_save').innerHTML = "Save";
        }
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
        function validate(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]|\./;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Collections<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Collections</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Collections Details
                </h3>
            </div>
            <div class="box-body" style="padding-left:230px;">
                <table cellpadding="1px" align="center" style="width: 60%;">
                    <tr>
                        <th colspan="2" align="center">
                        </th>
                    </tr>
                  <%--  <tr>
                        <td style="height: 40px;">
                            DCNo <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input type="text" id="txt_dcno" class="form-control" placeholder="Enter DC No" />
                        </td>
                    </tr>
                    <tr>
                            <td style="height: 50px;">
                                <label>
                                    DCDate</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input id="txt_dcdate" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Dc Date">
                            </td>
                        </tr>--%>

                      <tr>
                            <td style="height: 50px;">
                                <label>
                                    Datetime</label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                        </tr>
                    <tr>
                        <td style="height: 40px;">
                           <label> Vendor Name </label><span style="color: red;">*</span>
                        </td>
                        <td>
                            <select id="ddlvendorname" class="form-control">
                                <option selected disabled value="Select Vendor Name">Select Vendor Name</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 40px;">
                            <label>Transaction Type </label>
                        </td>
                        <td>
                            <select id="ddltranstype" class="form-control" >
                                <option>Collection</option>
                                <option>Payment</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 40px;">
                            <label> Payment Type </label>
                        </td>
                        <td>
                            <select id="ddlPaymentType" class="form-control" onchange="ddlPaymentTypeChange(this);">
                                <option>Cash</option>
                                <option>Cheque</option>
                                <option>DD</option>
                                <option>Bank Transfer</option>
                                <option>Journal Voucher</option>
                            </select>
                        </td>
                    </tr>
                    <tr class="divChequeclass" style="display: none;">
                        <td style="height: 40px;">
                            <span id="spnchequeno">Cheque No</span>
                        </td>
                        <td id="divCheque">
                            <input type="text" id="txtChequeNo" class="form-control" placeholder="Enter Cheque No" />
                        </td>
                    </tr>
                    <tr class="divBankclass" style="display: none;">
                        <td style="height: 40px;">
                            <span>Bank Name</span>
                        </td>
                        <td>
                            <input type="text" id="txtBankName" class="form-control" placeholder="Enter Bank Name" />
                        </td>
                    </tr>
                    <tr class="divChequeDateclass" style="display: none;">
                        <td style="height: 40px;">
                            <span> Date</span>
                        </td>
                        <td id="divchequedate">
                            <input type="text" name="journey_date" class="datepicker" tabindex="3" 
                                id="dtchequedate" placeholder="DD-MM-YYYY" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 40px;">
                           <label> Amount <span style="color: red;">*</span> </label>
                        </td>
                        <td>
                            <input type="text" maxlength="45" onkeypress="validate(event);" id="txt_amount" class="form-control" name="vendorcode"
                                placeholder="Enter Amount"><label id="lbl_name_error_msg" class="errormessage">* Please
                                    Enter Amount</label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 40px;">
                            <label>Remarks </label>
                        </td>
                        <td>
                            <textarea rows="5" cols="45" id="txtRemarks" class="form-control" maxlength="2000" placeholder="Enter Remarks"></textarea>
                        </td>
                    </tr>
                    <tr hidden>
                        <td>
                            <label id="lbl_sno">
                            </label>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td colspan="2" align="center" style="height: 40px;">
                            <input type="button" class="btn btn-success" name="submit" id="btn_save" value='Save'
                                onclick="save_vendor_collection_click()" />
                            <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close' />
                        </td>
                    </tr>--%>
                </table>
                <div  style="padding-left: 22%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="save_vendor_collection_click()"></span><span id="btn_save" onclick="save_vendor_collection_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="closedetails()"></span><span id='btn_close' onclick="closedetails()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
            </div>
        </div>
    </section>
</asp:Content>
