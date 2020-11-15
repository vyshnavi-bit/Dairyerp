<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="SendSMS.aspx.cs" Inherits="SendSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            Getvalues();
            GetPlantNames();
        });
        function GetPlantNames() {
            var data = { 'op': 'get_Vendor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        var Vendor = msg[0].vendordetails;
                        fillPalntdetails(Vendor);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillPalntdetails(msg) {
            var data = document.getElementById('cmb_vendortype');
            var length = data.options.length;
            document.getElementById('cmb_vendortype').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Plant Name";
            opt.value = "Select Plant Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].vendorname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].vendorname;
                    option.value = msg[i].VendorCode;
                    data.appendChild(option);
                }
            }
        }
        function Getvalues() {
            var cellnames = "FC,MC,BC";
            var names = cellnames.split(',');
            var results = '<div    style="overflow:auto;"><table id="tbl_milk_details" class="responsive-table">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Cell Name</th><th scope="col">Qty</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">TEMP</th><th scope="col">Acidity</th><th scope="col">CLR</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < names.length; i++) {
                results += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text" placeholder="CellName" name="CellName" value="' + names[i] + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Qty" name="Qty" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="FAT" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="FAT" name="FAT" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text" placeholder="SNF" name="SNF" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Temp" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="TEMP" name="TEMP" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Acidity" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Acidity" name="Acidity" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="CLR" style="text-align:center;" class="6"><input class="form-control" type="text" placeholder="CLR" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
        }
        function btn_send_ack_click() {

            var dispdate = document.getElementById('txtdate').value;
            var d = document.getElementById('cmb_vendortype');
            var plantname = d.options[d.selectedIndex].text;
            var dcno = document.getElementById('txt_dcno').value;
            var vendorid = document.getElementById('cmb_vendortype').value;
            var vehicleno = document.getElementById('txt_vehicleno').value;
            var mobno = document.getElementById('txt_mobno').value;
            if (plantname == "" || plantname == "Select Plant Name") {
                alert("Select Plant Name");
                return false;
            }
            if (dispdate == "") {
                alert("Enter disp date");
                return false;
            }
            if (dcno == "") {
                alert("Enter dc no");
                return false;
            }
            if (vehicleno == "") {
                alert("Enter vehicle no");
                return false;
            }
            if (mobno == "") {
                alert("Enter mob no");
                return false;
            }
            var Milk_array = [];
            $('#tbl_milk_details> tbody > tr').each(function () {
                var CellName = $(this).find('[name="CellName"]').val();
                var Qty = $(this).find('[name="Qty"]').val();
                var fat = $(this).find('[name="FAT"]').val();
                var snf = $(this).find('[name="SNF"]').val();
                var temp = $(this).find('[name="TEMP"]').val();
                var acidity = $(this).find('[name="Acidity"]').val();
                var clr = $(this).find('[name="CLR"]').val();
                Milk_array.push({ 'CellName': CellName, 'Qtyltr': Qty, 'fat': fat, 'snf': snf, 'temp': temp,
                    'acidity': acidity, 'clr': clr
                });
            });
            var Data = { 'op': 'Send_milk_details_click', 'dispdate': dispdate, 'mobno': mobno, 'dcno': dcno, 'vehicleno': vehicleno, 'plantname': plantname,
                'MilkfatDetailsMilkarray': Milk_array, 'vendorid': vendorid
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    btn_Clear_click();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(Data, s, e);
        }
        function btn_Clear_click() {
            document.getElementById('cmb_type').selectedIndex = 0;
            document.getElementById('cmb_vendortype').selectedIndex = 0;
            document.getElementById('txt_dcno').value = "";
            document.getElementById('txt_vehicleno').value = "";
            document.getElementById('txt_mobno').value = "";
            Getvalues();
        }
        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            d = encodeURIComponent(d);
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
        function CallHandlerUsingJson_POST(d, s, e) {
            d = JSON.stringify(d);
            d = encodeURIComponent(d);
            $.ajax({
                type: "POST",
                url: "FleetManagementHandler.axd",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Send SMS<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Send SMS</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Send SMS
                </h3>
            </div>
            <div class="box-body">
                <table cellpadding="1px" align="center">
                    <tr>
                        <th colspan="2" align="center">
                        </th>
                    </tr>
                    <tr>
                        <td>
                            Type
                        </td>
                        <td>
                            <input type="date" id="txtdate" class="form-control">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Plant Name
                        </td>
                        <td>
                            <select id="cmb_vendortype" class="form-control">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            DC No <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input type="text" maxlength="45" id="txt_dcno" class="form-control" name="vendorcode"
                                placeholder="Enter DC No"><label id="lbl_vencode_error_msg" class="errormessage">* Please
                                    Enter Vendor Code</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Vehicle No <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input type="text" maxlength="55" id="txt_vehicleno" class="form-control" name="vendorcode"
                                placeholder="Enter Vehicle No"><label id="lbl_vennme_error_msg" class="errormessage">*
                                    Please Enter Vendor Name</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Mob No <span style="color: red;">*</span>
                        </td>
                        <td>
                            <input type="number" maxlength="55" id="txt_mobno" class="form-control" name="vendorcode"
                                placeholder="Enter Mob No"><label id="Label1" class="errormessage">* Please Enter Mob
                                    No</label>
                        </td>
                    </tr>
                </table>
                  <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk Details</h3>
                        </div>
                <div id="div_vendordata">
                </div>
                </div>
                <table align="center">
                    <tr>
                        <td colspan="2">
                            <input id="btn_addlocation" type="button" class="btn btn-primary" value="Send SMS"
                                onclick="btn_send_ack_click()" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </section>
</asp:Content>
