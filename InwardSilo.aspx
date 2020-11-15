<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="InwardSilo.aspx.cs" Inherits="SiloTransactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            get_Silos();
            $('#btn_addDept').click(function () {
                $('#Inwardsilo_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_Deptdata').hide();
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

            $('#btn_close').click(function () {
                $('#Inwardsilo_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                Clearvalues();
            });
            get_myInward_silo_transaction();
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
            get_myInward_silo_transaction();
        });
        function addinwardsilo() {
            $('#Inwardsilo_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            get_Silos();
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
        function Clearvalues() {
            document.getElementById('txt_date').value = "";
            document.getElementById('txt_dcno').value = "";
            document.getElementById('slct_cell').selectedIndex = 0;
            document.getElementById('slct_Source_Name').selectedIndex = 0;
            document.getElementById('slct_vtype').selectedIndex = 0;
            document.getElementById('txt_qtykgs').value = "";
            document.getElementById('txt_qtyltrs').value = "";
            document.getElementById('txt_fat').value = "";
            document.getElementById('txt_snf').value = "";
            document.getElementById('txt_clr').value = "";
            document.getElementById('save_inwordsilo').innerHTML = "Save";
            transsno = 0;
            $('#Inwardsilo_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_Deptdata').show();
            $('#tdother').css('display', 'none');
            $('#tddcno').css('display', 'none');
            $('#tdvendorname').css('display', 'none');
        }
        function save_inword_silotransaction_click() {
            var date = document.getElementById('txt_date').value;
            if (date == "") {
                alert("Please Enter date");
                $("#txt_date").focus();
                return false;
            }
            var dcno = document.getElementById('txt_dcno').value;
            var vendorname = document.getElementById('slct_vendorname').value;
            var otherparty = document.getElementById('txt_otherparty').value;
            var Cell = document.getElementById('slct_cell').value;
            var Siloname = document.getElementById('slct_Source_Name').value;
            if (Siloname == "") {
                alert("Please Select Silo name");
                $("#slct_Source_Name").focus();
                return false;
            }
            var Qtykgs = document.getElementById('txt_qtykgs').value;
            var Qtyltrs = document.getElementById('txt_qtyltrs').value;
            if (Qtyltrs == "") {
                alert("Please Select Qty ltrs");
                $("#txt_qtyltrs").focus();
                return false;
            }
            var fat = document.getElementById('txt_fat').value;
            var snf = document.getElementById('txt_snf').value;
            var clr = document.getElementById('txt_clr').value;
            // var sno = document.getElementById('lbl_sno').innerHTML;
            var btnval = document.getElementById('save_inwordsilo').innerHTML;
            var Cellname = document.getElementById('slct_cell').selectedIndex;
            var mysilo = document.getElementById('slct_Source_Name').selectedIndex;
            
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_inword_silotransaction_click', 'vendorname': vendorname, 'date': date, 'dcno': dcno, 'Cell': Cell, 'Siloname': Siloname, 'fat': fat, 'snf': snf, 'clr': clr,
                    'Qtykgs': Qtykgs, 'Qtyltrs': Qtyltrs, 'otherparty': otherparty, 'btnval': btnval, 'transsno': transsno
                };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_myInward_silo_transaction();
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

        function get_myInward_silo_transaction() {
            var getdatadate = document.getElementById('txt_getdatadate').value;
            var data = { 'op': 'get_myInward_silo_transaction', 'getdatadate': getdatadate };
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
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Silo Name</th><th scope="col" style="font-weight: bold;">DCNo</th><th scope="col" style="font-weight: bold;">CellName</th><th scope="col" style="font-weight: bold;">Quantity(kgs)</th><th scope="col" style="font-weight: bold;">Quantity(ltrs)</th><th scope="col" style="font-weight: bold;">FAT</th><th scope="col" style="font-weight: bold;">SNF</th><th scope="col" style="font-weight: bold;">CLR</th><th scope="col" style="font-weight: bold;">Party Name</th><th scope="col" style="font-weight: bold;">DOE</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1"  style="display:none;">' + msg[i].branchname + '</td>';
                results += '<td data-title="Code" class="2">' + msg[i].SiloName + '</td>';
                results += '<td data-title="Code" class="3">' + msg[i].DCNo + '</td>';
                results += '<td data-title="Code" class="4">' + msg[i].CellName + '</td>';
                results += '<td data-title="Code" class="5">' + msg[i].OutwordQuantitykgs + '</td>';
                results += '<td data-title="Code" class="6">' + msg[i].OutwordQuantityltrs + '</td>';
                results += '<td data-title="Code" class="7">' + msg[i].fat + '</td>';
                results += '<td data-title="Code" class="8">' + msg[i].snf + '</td>';
                results += '<td data-title="Code" class="9">' + msg[i].clr + '</td>';
                results += '<td data-title="Code" style="display:none;" class="10">' + msg[i].transno + '</td>';
                results += '<td data-title="Code"  class="12">' + msg[i].otherpartyname + '</td>';
                results += '<td data-title="Code" class="13">' + msg[i].doe + '</td>';
                results += '<td data-title="Code" style="display:none;" class="20">' + msg[i].datetime + '</td>';
                results += '<td data-title="Code" style="display:none;" class="11">' + msg[i].SiloId + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        var transsno = 0;
        function getme(thisid) {
            transsno = $(thisid).parent().parent().children('.10').html();
            var SiloName = $(thisid).parent().parent().children('.2').html();
            var DCNo = $(thisid).parent().parent().children('.3').html();
            var CellName = $(thisid).parent().parent().children('.4').html();
            var OutwordQuantitykgs = $(thisid).parent().parent().children('.5').html();
            var OutwordQuantityltrs = $(thisid).parent().parent().children('.6').html();
            var fat = $(thisid).parent().parent().children('.7').html();
            var snf = $(thisid).parent().parent().children('.8').html();
            var clr = $(thisid).parent().parent().children('.9').html();
            var SiloId = $(thisid).parent().parent().children('.11').html();
            var otherpartyname = $(thisid).parent().parent().children('.12').html();
            var doe = $(thisid).parent().parent().children('.13').html();
            var datetime = $(thisid).parent().parent().children('.20').html();
            $('#tdother').css('display', 'block');
            document.getElementById('txt_date').value = datetime
            document.getElementById('txt_dcno').value = DCNo
            document.getElementById('slct_cell').value = CellName;
            document.getElementById('slct_Source_Name').value = SiloId;
            document.getElementById('txt_qtykgs').value = OutwordQuantitykgs;
            document.getElementById('txt_qtyltrs').value = OutwordQuantityltrs;
            document.getElementById('txt_fat').value = fat;
            document.getElementById('txt_snf').value = snf;
            document.getElementById('txt_otherparty').value = otherpartyname;
            document.getElementById('slct_vtype').value = "Others";

            document.getElementById('txt_clr').value = clr;
            document.getElementById('save_inwordsilo').innerHTML = "Modify";
            $('#Inwardsilo_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
        }
        function kgsChange(qtyid) {
            var qtykg = 0;
            qtykg = parseFloat(qtyid.value).toFixed(3);
            var clr = document.getElementById('txt_clr').value;
            if (clr == "") {
                clr = 30;
            }
            clr = parseFloat(clr).toFixed(3);
            var qtykgsltr = 0;
            var modclr = (clr / 1000) + 1;
            modclr = parseFloat(modclr).toFixed(5);
            qtykgsltr = qtykg / modclr;
            qtykgsltr = parseFloat(qtykgsltr).toFixed(0);
            document.getElementById('txt_qtyltrs').value = qtykgsltr;
        }
        function LtrsChange(qtyid) {
            var siloid = document.getElementById('slct_Source_Name').value;
            if (siloid == "Select Silo") {
                alert("Please Select Silo");
                document.getElementById('txt_qtyltrs').value = "";
                return;
            }
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

                var fat = 0;
                fat = document.getElementById('txt_fat').value;
                fat = parseFloat(fat).toFixed(3);
                var snfvalue = (fat * 0.21) + (clr / 4 + 0.36);
                document.getElementById('txt_snf').value = parseFloat(snfvalue).toFixed(2);

            }
        }

        function getsiloqty() {
            var siloname = document.getElementById('slct_Source_Name').value;
            var data = { 'op': 'get_siloqty_details', 'siloname': siloname };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            document.getElementById('txt_siloqty').value = msg[i].quantity;
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
        function ddlvehiclechange() {
            var vtype = document.getElementById('slct_vtype').value;
            if (vtype == "Tanker") {
                $('#tddcno').css('display', 'block');
                $('#tdother').css('display', 'none');
                $('#tdvendorname').css('display', 'block');
                get_Vendor_details();
            }
            else {
                $('#tdother').css('display', 'block');
                $('#tddcno').css('display', 'none');
                $('#tdvendorname').css('display', 'none');
            }
        }
        function get_Vendor_details() {
            var data = { 'op': 'get_Vendor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var Vendor = msg[0].vendordetails;
                        fillvendors(Vendor);
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
        function fillvendors(msg) {
            var data = document.getElementById('slct_vendorname');
            var length = data.options.length;
            document.getElementById('slct_vendorname').options.length = null;
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

        //        var silocapacity;
        //        function checkcapacity() {
        //            var siloid = document.getElementById('slct_Source_Name').value;
        //            if (siloid == "Select Silo") {
        //                alert("Please Select Silo");
        //                document.getElementById('txt_qtyltrs').value = "";
        //                return;
        //            }
        //            var data = { 'op': 'get_Silo_capacity', 'siloid': siloid };
        //            var s = function (msg) {
        //                if (msg) {
        //                    if (msg.length > 0) {

        //                        fillcapacity(msg);
        //                    }
        //                    else {
        //                    }
        //                }
        //                else {
        //                }
        //            };
        //            var e = function (x, h, e) {
        //            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        //            callHandler(data, s, e);
        //        }
        //        function fillcapacity(msg) {
        //            for (var i = 0; i < msg.length; i++) {
        //                silocapacity = msg[i].Capacity;
        //                var qtycltrs = parseInt(document.getElementById('txt_qtyltrs').value);
        //                var caty = parseInt(silocapacity);
        //                if (qtycltrs > caty) {
        //                    alert("entry quantity must be less than the silo capacity");
        //                    document.getElementById('txt_qtyltrs').value = "";
        //                    return;
        //                }
        //            }
        //        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
     <h1>
            <i class="fa fa-television" aria-hidden="true"></i>SILO Inward<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Inward Silo Operations</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Inward SILO Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs">
                    <table>
                    <tr>
                    <td>
                          </td>
                        <td>
                            <label>
                                Date </label>
                        </td>
                        <td style="width: 5px;">
                          </td>
                        <td>
                            <input id="txt_getdatadate" class="form-control" type="date" name="vendorcode" />
                        </td>
                         <td style="width: 5px;">
                          </td>
                        <td>
                       <%-- <input id="txt_getgenerate" type="button" name="submit" value='Generate'
                        class="btn btn-primary" onclick="get_myInward_silo_transaction();"/>--%>
                         <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-refresh" onclick="get_myInward_silo_transaction()"></span> <span onclick="get_myInward_silo_transaction()">Generate</span>
                          </div>
                          </div>
                        </td>
                       <%-- <td style="width: 400px;">
                          </td>
                        <td>
                         <input id="btn_addDept" type="button" name="submit" value='Add Inward SILO' class="btn btn-success" />
                        </td>--%>
                        <td style="padding-left: 660px;"> 
                            <td>
                            </td>
                            <td>

                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addinwardsilo()"></span> <span onclick="addinwardsilo()">Add Inward</span>
                          </div>
                          </div>
                            </td>
                    </tr>
                </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='Inwardsilo_fillform' style="display: none;width:100%;" align="center">
                    <table >
                        <tr>
                            <td>
                                <label>
                                    Datetime<span style="color: red;">*</span></label>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                                <label id="lbl_date" class="errormessage">
                                    * Please Enter date</label>
                            </td>
                            </tr>
                            <tr>
                            <td>
                                <label>
                                    Vehicletype<span style="color: red;">*</span></label>
                                <select id="slct_vtype" class="form-control" onchange="ddlvehiclechange()">
                                    <option selected disabled value="Select Vehicle No">Select Vehicle</option>
                                    <option>Tanker</option>
                                    <option>Others</option>
                                </select>
                                <label id="Label1" class="errormessage">
                                    * Please select cell</label>
                            </td>
                            <td style="width:10px;"></td>
                            <td id='tddcno' style="display: none;">
                                <label>
                                    DC Ref No<span style="color: red;">*</span><span style="color: red;">*</span></label>
                                <input id="txt_dcno" type="text" class="form-control" name="vendorcode" placeholder="DC Ref No">
                                <label id="lbl_dcno" class="errormessage">
                                    * Please Enter dc no</label>
                            </td>
                            <td id='tdother' style="display: none;">
                                <label>
                                    Other Party Name<span style="color: red;">*</span><span style="color: red;">*</span></label>
                                <input id="txt_otherparty" type="text" class="form-control" name="vendorcode" placeholder="Enter vendor Name">
                                <label id="Label2" class="errormessage">
                                    * Please Enter Other Party Name</label>
                            </td>
                        </tr>
                        <tr>
                            <td id='tdvendorname' style="display: none;">
                                <label>
                                    Vendor Name<span style="color: red;">*</span><span style="color: red;">*</span></label>
                                <select id="slct_vendorname" class="form-control" name="vendorcode" >
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Cells<span style="color: red;">*</span></label>
                                <select id="slct_cell" class="form-control">
                                    <option selected disabled value="Select Vehicle No">Select Cell</option>
                                    <option>F Cell</option>
                                    <option>M Cell</option>
                                    <option>B Cell</option>
                                </select>
                                <label id="lbl_cell" class="errormessage">
                                    * Please select cell</label>
                            </td>
                            <td></td>
                            <td>
                                <label>
                                    SILO Name<span style="color: red;">*</span></label>
                                <select id="slct_Source_Name" class="form-control" onchange="getsiloqty()">
                                    <option selected disabled value="Select SILO No">Select SILO Name</option>
                                </select>
                                <label id="lbl_silo" class="errormessage">
                                    * Please select Silo</label>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                            <label>
                                    SILO Quantity<span style="color: red;">*</span></label>
                            <input id="txt_siloqty" type="text" class="form-control" name="vendorcode" readonly placeholder="Enter siloqty" readonly></td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Qty(Kgs)</label>
                                <input id="txt_qtykgs" type="text" class="form-control" name="vendorcode" readonly placeholder="Enter Qty in Kgs" onkeyup="kgsChange(this)" readonly>
                                <label id="lbl_Qtykgs" class="errormessage">
                                    * Please enter Quantity</label>
                            </td>
                            <td></td>
                            <td>
                                <label>
                                    Qty(ltrs)<span style="color: red;">*</span></label>
                                <input id="txt_qtyltrs" type="text" class="form-control" name="vendorcode" placeholder="Enter Qty in ltrs" onkeyup="LtrsChange(this)">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    FAT<span style="color: red;">*</span></label>
                                <input id="txt_fat" type="text" class="form-control" name="vendorcode" placeholder="Enter FAT">
                            </td>
                            <td></td>
                            <td>
                                <label>
                                    SNF</label>
                                <input id="txt_snf" type="text" class="form-control" name="vendorcode" placeholder="SNF" readonly/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    CLR<span style="color: red;">*</span></label>
                                <input id="txt_clr" type="text" class="form-control" name="vendorcode" placeholder="Enter CLR"
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
                        </table>
                        <%--<div>
                        <table>
                        <tr>
                            <td>
                                <input id='save_inwordsilo' type="button" class="btn btn-success" name="submit" value='Save'
                                    onclick="save_inword_silotransaction_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="Clearvalues()" />
                                <input id='btnPrint' type="button" class="btn btn-primary" name="Print" value='Print'
                                    onclick="javascript:CallPrint('div_axilautofill');" />
                            </td>
                        </tr>
                    </table>
                    </div>--%>
                    <div>
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="save_inwordsilo1" onclick="save_inword_silotransaction_click()"></span><span id="save_inwordsilo" onclick="save_inword_silotransaction_click()">Save</span>
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
                    <br />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
