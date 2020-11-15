<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="smprate.aspx.cs" Inherits="smprate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
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
            get_smp_details();
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

            document.getElementById('txt_qtykgs').value = "";

            document.getElementById('txt_fat').value = "";
            document.getElementById('txt_snf').value = "";
            document.getElementById('save_smpsilo').value = "Save";
            transsno = 0;
        }
        function save_smp_ratechange_click() {
            var fromdate = document.getElementById('txt_fromdate').value;
            var todate = document.getElementById('txt_todate').value;
            var priviewrate = document.getElementById('txt_prvrate').value;
            var newrate = document.getElementById('txt_newrate').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var btnval = document.getElementById('save_smpsilo').value;
            var flag = false;
            if (fromdate == "") {
                alert("please enter fromdate");
            }
            if (todate == "") {
                alert("please enter todate");
            }
            if (priviewrate == "") {
                alert("please enter prev rate");
            }
            if (newrate == "") {
                alert("please enter newrate");
            }
            if (flag) {
                return;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_smp_ratelogs_click', 'fromdate': fromdate, 'todate': todate, 'priviewrate': priviewrate, 'newrate': newrate, 'btnval': btnval, 'sno': sno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
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
            SMP Rate<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">SMP Rate</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>SMP Rate Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addDept" type="button" name="submit" value='Add SMP' class="btn btn-success" />
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='Inwardsilo_fillform' style="display: none;width:100%;" align="center">
                    <table >
                        <tr>
                            <td>
                                <label>
                                    From Date<span style="color: red;">*</span></label>
                                <input id="txt_fromdate" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                            <td style="width:10px;"></td>
                           <td>
                                 <label>
                                    To Date<span style="color: red;">*</span></label>
                                <input id="txt_todate" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                            </tr>
                        <tr>
                        <td>
                                <label>
                                  Prev Rate</label>
                                <input id="txt_prvrate" type="text" class="form-control" name="vendorcode"  placeholder="Enter Qty in Kgs">
                              
                            </td>
                            <td></td>
                            <td>
                                <label>
                                  New Rate</label>
                                <input id="txt_newrate" type="text" class="form-control" name="vendorcode"  placeholder="Enter Qty in Kgs">
                            </td>
                        </tr>
                        
                        </br>
                         <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <input id='save_smpsilo' type="button" class="btn btn-success" name="submit" value='Save'
                                    onclick="save_smp_ratechange_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="Clearvalues()" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
        </div>
    </section>
</asp:Content>

