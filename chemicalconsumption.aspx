<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="chemicalconsumption.aspx.cs" Inherits="chemicalconsumption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btn_addDept').click(function () {
                $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
                get_chemical_details();
            });
            $('#btn_close').click(function () {
                $('#Inwardsilo_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                Clearvalues();
            });
            get_chemicalconsumption_details();
            get_chemical_details();
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
        function addchamicalconsumpction() {
          $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
                get_chemical_details();
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
        function get_chemical_details() {
            var data = { 'op': 'get_chemical_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillchemicals(msg);
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
        function fillchemicals(msg) {
            var data = document.getElementById('slct_chemical');
            var length = data.options.length;
            document.getElementById('slct_chemical').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Chemical";
            opt.value = "Select Chemical";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].productname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].productname;
                    option.value = msg[i].productid;
                    data.appendChild(option);
                }
            }
        }

        function get_chemicalconsumption_details() {
            var data = { 'op': 'get_chemicalconsumption_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillchemicalconsumptions(msg);
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
        function fillchemicalconsumptions(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Branch Name</th><th scope="col" style="font-weight: bold;">Chemical Name</th><th scope="col" style="font-weight: bold;">O/B</th><th scope="col" style="font-weight: bold;">Receipts</th><th scope="col" style="font-weight: bold;">Total</th><th scope="col" style="font-weight: bold;">PHE</th><th scope="col" style="font-weight: bold;">CB</th><th scope="col" style="font-weight: bold;">Date</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
             var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].branchname + '</td>';
                results += '<td data-title="Capacity" class="2" style="text-align:center;">' + msg[i].chemicaltype + '</td>';
                results += '<td  class="3" style="text-align:center;">' + msg[i].ob + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].receipts + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].total + '</td>';
                results += '<td  class="6" style="text-align:center;">' + msg[i].phe + '</td>';
                results += '<td  class="7" style="text-align:center;">' + msg[i].cb + '</td>';
                results += '<td  class="8" style="text-align:center;">' + msg[i].doe + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].used + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].remarks + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].chemicalid + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                 l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }

        function getme(thisid) {
            var sno = $(thisid).parent().parent().children('.12').html();
            var chemicalid = $(thisid).parent().parent().children('.11').html();
            var remarks = $(thisid).parent().parent().children('.10').html();
            var used = $(thisid).parent().parent().children('.9').html();
            var cb = $(thisid).parent().parent().children('.7').html();
            var phe = $(thisid).parent().parent().children('.6').html();
            var total = $(thisid).parent().parent().children('.5').html();
            var receipts = $(thisid).parent().parent().children('.4').html();
            var ob = $(thisid).parent().parent().children('.3').html();
            document.getElementById('slct_chemical').value = chemicalid;
            document.getElementById('txt_ob').value = ob;
            document.getElementById('txt_receipts').value = receipts;
            document.getElementById('txt_total').value = total;
            document.getElementById('txt_phe').value = phe;
            document.getElementById('txt_cb').value = cb;
            document.getElementById('txt_use').value = used;
            document.getElementById('txt_Remarks').value = remarks;
            document.getElementById('lbl_sno').value = sno;
            document.getElementById('save_batchdetails').innerHTML = "Modify";
            $("#div_Deptdata").hide();
            $("#Inwardsilo_fillform").show();
            $('#showlogs').hide();
        }
        function save_curd_section_click() {
            var date = document.getElementById('txt_date').value;
            var chemicalid = document.getElementById('slct_chemical').value;
            var ob = document.getElementById('txt_ob').value;
            var receipts = document.getElementById('txt_receipts').value;
            var total = document.getElementById('txt_total').value;
            var phe = document.getElementById('txt_phe').value;
            var cb = document.getElementById('txt_cb').value;
            var used = document.getElementById('txt_use').value;
            var remarks = document.getElementById('txt_Remarks').value;
            var btnvalue = document.getElementById('save_batchdetails').innerHTML;
            var sno = document.getElementById('lbl_sno').value;
            if (chemicalid == "" || chemicalid == "Select Chemical") {
                alert("Please select Chemical Type");
                $("#slct_chemical").focus();
                return false;
            }
            if (ob == "") {
                alert("Enter O/B");
                $("#txt_ob").focus();
                return false;
            }
            if (receipts == "") {
                alert("Enter receipts");
                $("#txt_receipts").focus();
                return false;
            }
            if (total == "") {
                alert("Enter total");
                 $("#txt_total").focus();
                return false;
            }
            if (phe == "") {
                alert("Enter phe");
                $("#txt_phe").focus();
                return false;
            }
            if (cb == "") {
                alert("Enter cb");
                $("#txt_cb").focus();
                return false;
            }
          
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_curd_section_click', 'date': date, 'chemicalid': chemicalid, 'ob': ob, 'receipts': receipts, 'total': total, 'cb': cb, 'phe': phe, 'used':used, 'remarks':remarks, 'btnvalue': btnvalue, 'sno': sno, };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            Clearvalues();
                            get_chemicalconsumption_details();
                            $('#Inwardsilo_fillform').css('display', 'none');
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
                callHandler(data, s, e);
            }
        }
        function Clearvalues() {
            document.getElementById('slct_chemical').selectedIndex = 0;
            document.getElementById('txt_ob').value = "";
            document.getElementById('txt_receipts').value = "";
            document.getElementById('txt_total').value = "";
            document.getElementById('txt_phe').value = "";
            document.getElementById('txt_cb').value = "";
            document.getElementById('txt_use').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('save_batchdetails').innerHTML="Save";
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
        }

        function myvalue(qtyid) {
            if (qtyid.value == "") {
            }
            else {
                var total = 0;
                total = document.getElementById('txt_total').value;
                var phe = qtyid.value;
                var sum = parseFloat(total) - parseFloat(phe);
                document.getElementById('txt_cb').value = sum;
            }
        }
        function ctotal(qtyid) {
            if (qtyid.value == "") {
            }
            else {
                var ob = document.getElementById('txt_ob').value;
                if (ob == "") {
                    alert("Please enter O/B");
                    return false;
                }
                var recipts = qtyid.value;
                var sum = parseFloat(ob) + parseFloat(recipts);
                document.getElementById('txt_total').value = sum;
            }
        }
        function getobval(){
        var productid = document.getElementById('slct_chemical').value;
        var data = { 'op': 'get_productqty_details', 'productid':productid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                    for (var i = 0; i < msg.length; i++) {
                       document.getElementById('txt_ob').value = msg[i].quantity;
                       }
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Chemical Consumption<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Chemical Consumption</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Chemical Consumption Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style=" text-align: -webkit-right;">
                   <%-- <input id="btn_addDept" type="button" name="submit" value='Add Details'
                        class="btn btn-success" />--%>
                         <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addchamicalconsumpction()"></span> <span onclick="addchamicalconsumpction()">Add Details</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='Inwardsilo_fillform' style="display: none; padding-left:250px;">
                    <table align="center" style="width: 20%;">
                        <tr>
                            <td>
                            <label>
                                    Datetime</label>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                            
                        </tr>
                        <tr>
                        <td>
                                <label>
                                   Chemical Type<span style="color: red;">*</span></label>
                                <select id="slct_chemical" class="form-control" onchange="getobval()">
                                    <option selected disabled value="Select SILO No">Select Chemical</option>
                                </select>
                                <label id="Label3" class="errormessage">
                                    * Select Chemical Type</label>
                            </td>
                        </tr>
                    </table>
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    O/B<span style="color: red;">*</span></label>
                                 <input id="txt_ob" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter OB" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    Receipts<span style="color: red;">*</span></label>
                                <input id="txt_receipts" type="text" class="form-control" onkeypress="return isFloat(event);" name="vendorcode" placeholder="Enter Receipts" onkeyup="ctotal(this)">
                                <label id="lbl_receipts" class="errormessage">
                                    * Please enter receipts</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Total<span style="color: red;">*</span></label>
                                <input id="txt_total" type="text" class="form-control" name="vendorcode" placeholder="Enter Total" readonly>
                                <label id="lbl_total" class="errormessage">
                                    * Please enter Total</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                    PHE</label>
                                <input id="txt_phe" type="text" class="form-control" name="vendorcode" onkeypress="return isFloat(event);" placeholder="PHE" onkeyup="myvalue(this)"/>
                                <label id="lbl_phe" class="errormessage">
                                    * Please enter PHE</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    C/B<span style="color: red;">*</span></label>
                                <input id="txt_cb" type="text" class="form-control" name="vendorcode" placeholder="Enter C/B" readonly/>
                                <label id="lbl_cb" class="errormessage">
                                    * Please enter C/B</label>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td>
                                <label>
                                   Use<span style="color: red;">*</span></label>
                                <input id="txt_use" type="text" class="form-control" name="vendorcode" placeholder="Enter use" />
                                <label id="Label4" class="errormessage">
                                    * Please enter Use</label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td >
                                <label>
                                    Remarks</label>
                                <textarea rows="3" cols="45" id="txt_Remarks" class="form-control" maxlength="200"
                                        placeholder="Enter Remarks"></textarea>
                            </td>
                        </tr>
                         <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <%--<div style="padding-left: 165px;">
                    <table>
                        <tr>
                            <td>
                                <input id='save_batchdetails' type="button" class="btn btn-success" name="submit"
                                    value='Save' onclick="save_curd_section_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="Clearvalues()" />
                            </td>
                        </tr>
                    </table>
                    </div>--%>
                    <div  style="padding-left: 10%;padding-top: 0%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_batchdetails1" onclick="save_curd_section_click()"></span><span id="save_batchdetails" onclick="save_curd_section_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="Clearvalues()"></span><span id='btn_close' onclick="Clearvalues()">Close</span>
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

