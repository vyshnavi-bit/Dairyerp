<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="silowiseclosing.aspx.cs" Inherits="silowiseclosing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <style type="text/css">
        input[type=text]::-webkit-inner-spin-button, input[type=text]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            get_Silos();
            get_batchs();
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
        function get_batchs() {
            var data = { 'op': 'get_batch_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbatchdetails(msg);
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
        function fillbatchdetails(msg) {
            $('.batchfillclsdetails').each(function () {
                var batch = $(this);
                batch[0].options.length = null;
                var opt = document.createElement('option');
                opt.innerHTML = "Select Batch";
                opt.value = "Select Batch";
                opt.setAttribute("selected", "selected");
                opt.setAttribute("disabled", "disabled");
                opt.setAttribute("class", "dispalynone");
                batch[0].appendChild(opt);
                for (var i = 0; i < msg.length; i++) {
                    if (msg[i].batchtype != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].batchtype;
                        option.value = msg[i].batchid;
                        batch[0].appendChild(option);
                    }
                }
            });
        }
        function get_Silos() {
            var data = { 'op': 'get_Silomonitor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        get_batchs();
                        var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Silo Name</th><th scope="col">Quantity</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">CLR</th><th scope="col">Batch Type</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr>';
                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].sno + '</span></th>';
                            results += '<th><span id="txt_SiloName" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].SiloName + '</span></th>';
                            results += '<td><input id="txt_qtykgs" class="form-control" value="' + msg[i].Quantity + '" onkeypress="return isFloat(event);" type="text" name="vendorcode"placeholder="Enter qty kgs"></td>';
                            results += '<td><input id="txt_fat" class="form-control" value="" type="text" onkeypress="return isFloat(event);" name="vendorcode"placeholder="Enter FAT"></td>';
                            results += '<td><input id="txt_snf" class="form-control" value="" type="text" onkeypress="return isFloat(event);" name="vendorcode"placeholder="Enter SNF"></td>';
                            results += '<td><input id="txt_clr" class="form-control" value="" type="text" onkeypress="return isFloat(event);" name="vendorcode"placeholder="Enter CLR"></td>';
                            results += '<td><select id="ddlbatchtype" class="batchfillclsdetails form-control" ></select></td>';
                            results += '<td style="display:none" class="8"><input id="hdn_siloid" class="form-control" type="text" name="vendorcode" value="' + msg[i].SiloId + '"></td></tr>';
                        }
                        results += '</table></div>';
                        $("#divFillScreen").html(results);
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

        function btnsave_batchclosing_click() {
            var rows = $("#table_shift_wise_details tr:gt(0)");
            var silo_wise_details = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_qtykgs').val() == "") {

                }
                else {
                    //                    if ($(this).find('#ddlbatchtype').val() == "" || $(this).find('#ddlbatchtype').val() == "Select Batch" || $(this).find('#ddlbatchtype').val() == null || $(this).find('#ddlbatchtype').val() == "null") {
                    //                        alert("Please Select Batch Type");
                    //                        return false;
                    //                    }
                    silo_wise_details.push({ siloid: $(this).find('#hdn_siloid').val(), qty_kgs: $(this).find('#txt_qtykgs').val(), fat: $(this).find('#txt_fat').val(), snf: $(this).find('#txt_snf').val(), clr: $(this).find('#txt_clr').val(), batchid: $(this).find('#ddlbatchtype').val() });
                }
            });
            if (silo_wise_details.length == 0) {
                alert("Please enter new quantity");
                return false;
            }
            var confi = confirm("Do you want to save  silo closing Details ?");
            if (confi) {
                var Data = { 'op': 'btnsave_siloclosing_click', 'silo_wise_details': silo_wise_details };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        get_Silos();
                        get_batchs();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                CallHandlerUsingJson(Data, s, e);
            }
        }
        function btn_cancel_click() {

        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
           <i class="fa fa-files-o" aria-hidden="true"></i> Silo Wise closing<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Silo Wise closing</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Silo Wise Closing Balance
                    Details
                </h3>
            </div>
            <div class="box-body">
                <div id="divFillScreen">
                </div>
               <div align="center">
                <table>
                    <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="btnsave_batchclosing_click()"></span><span id="btn_save" onclick="btnsave_batchclosing_click()">Save</span>
                          </div>
                       </div>
                    </td>
                </table>
            </div>
            </div>
        </div>
    </section>
</asp:Content>
