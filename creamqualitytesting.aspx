<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="creamqualitytesting.aspx.cs" Inherits="creamqualitytesting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(function () {
        $('#btn_addCream').click(function () {
            $('#cream_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_creamdetailsdata').hide();
            Getcreamdetails();
            clearvalues();
            get_cream_qualitytesting_details();
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

        $('#close_creamtest').click(function () {
            $('#cream_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_creamdetailsdata').show();
        });
        clearvalues();
        get_cream_qualitytesting_details();
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
    function Getcreamdetails() {
        var names = 10;
        var results = '<div    style="overflow:auto;"><table id="tbl_cream_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Qty(Kgs)</th><th scope="col">FAT</th><th scope="col">SNF</th><th scope="col">TEMPRATURE</th><th scope="col">ACIDITY</th></tr></thead></tbody>';
        var j = 1;
        for (var i = 0; i < 1; i++) {
            results += '<td data-title="Sno" class="2">' + (i + 1) + '</td>';
            results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="number" placeholder="Qty(kgs)" name="Qtykg" id="txt_kg" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="fat" style="text-align:center;" class="3"><input class="form-control" type="number" placeholder="FAT" name="FAT" id="txt_fat" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="snf" style="text-align:center;" class="3"><input class="form-control" type="number" placeholder="SNF" name="SNF" id="txt_snf" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="temprature" style="text-align:center;" class="3"><input class="form-control" type="number" placeholder="TEMPRATURE" name="TEMPRATURE" id="txt_temprature" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="acidity" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="ACIDITY" name="ACIDITY" id="txt_acidity" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
        }
        results += '</table></div>';
        $("#div_creamdata").html(results);
    }
    function save_cream_qualitytesting_click() {
        var date = document.getElementById('txt_date').value;
        var creamtype = document.getElementById('slct_creamtype').value;
        var remarks = document.getElementById('txt_Remarks').value;
        var chemist = document.getElementById('txt_Chemist').value;
        var qco = document.getElementById('txt_qco').value;
        var sno = document.getElementById('lbl_sno').value;
        var btnvalue = document.getElementById('save_creamtest').value;
        var rows = $("#tbl_cream_details tr:gt(0)");
        var creamdetails = new Array();
        $(rows).each(function (i, obj) {
            if ($(this).find('#txt_fat').val() != "") {
                creamdetails.push({ Qtykg: $(this).find('#txt_kg').val(), fat: $(this).find('#txt_fat').val(), snf: $(this).find('#txt_snf').val(), temprature: $(this).find('#txt_temprature').val(), acidity: $(this).find('#txt_acidity').val() });
            }
        });
        if (creamdetails.length == 0) {
            alert("Please Enter FAT and SNF balance");
            return false;
        }
        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var data = { 'op': 'save_cream_qualitytesting_click', 'creamdetails': creamdetails, 'date': date, 'creamtype': creamtype, 'remarks': remarks, 'chemist': chemist, 'qco': qco, 'btnvalue': btnvalue, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        clearvalues();
                        $('#cream_fillform').css('display', 'none');
                        $('#showlogs').css('display', 'block');
                        $('#div_creamdetailsdata').show();
                        get_cream_qualitytesting_details();
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
    function clearvalues() {
        document.getElementById('txt_date').value = "";
        document.getElementById('slct_creamtype').selectedIndex = "";
        document.getElementById('txt_Remarks').value = "";
        document.getElementById('txt_Chemist').value = "";
        document.getElementById('txt_qco').value = "";
        document.getElementById('save_creamtest').value = "Save";
    }
    function get_cream_qualitytesting_details() {
        var data = { 'op': 'get_cream_qualitytesting_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    filldetails(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function filldetails(msg) {
        var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">Cream Type</th></th><th scope="col">Qty(kgs)</th><th scope="col">Fat</th><th scope="col">Snf</th><th scope="col">Date</th></tr></thead></tbody>';
        for (var i = 0; i < msg.length; i++) {
            results += '<tr>';
            results += '<td   class="10" >' + msg[i].creamtype + '</td>';
            results += '<td   class="10">' + msg[i].qtykgs + '</td>';
            results += '<td  class="8">' + msg[i].fat + '</td>';
            results += '<td   class="9">' + msg[i].snf + '</td>';
            results += '<td  class="4">' + msg[i].doe + '</td>';
            results += '<td style="display:none" class="12">' + msg[i].sno + '</td>';
        }
        results += '</table></div>';
        $("#div_creamdetailsdata").html(results);
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <section class="content-header">
        <h1>
           Cream Quality Testing <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Cream Quality Testing</a></li>
        </ol>
    </section>
    <section class="content">
    <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Cream Quality Testing Entry
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addCream" type="button" name="submit" value='Add Quality Testing' class="btn btn-success" />
                </div>
                <div id="div_creamdetailsdata">
                </div>
                <div id='cream_fillform' style="display: none;">
                <div style="padding-left:300px;">
                    <table align="center">
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                   Date</label>
                            </td>
                            <td>
                                <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Crem Type</label>
                            </td>
                            <td>
                                <select id="slct_creamtype" class="form-control">
                                <option selected value disabled>Select Cream Type</option>
                                <option>Cow</option>
                                <option>Buffalo</option>
                                </select>
                            </td>
                             <td style="width: 6px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                    </div>
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Details</h3>
                        </div>
                        <div id="div_creamdata">
                        </div>
                    </div>
                    <div style="text-align: center;">
                    <div style="padding-left:200px;">
                        <table align="center">
                            <tr>
                                <td style="height: 40px;">
                                    <label>
                                        Remarks</label>
                                </td>
                                <td colspan="6">
                                    <textarea rows="3" cols="45" id="txt_Remarks" class="form-control" maxlength="200"
                                        placeholder="Enter Remarks"></textarea>
                                    <%--   <textarea id="txt_Remarks" class="form-control" rows="5" cols="45" placeholder="Remarks" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Chemist</label>
                                </td>
                                <td>
                                    <input id="txt_Chemist" type="text" class="form-control" name="vendorcode" placeholder="Chemist" />
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td>
                                    <label>
                                        QCO</label>
                                </td>
                                <td>
                                    <input id="txt_qco" type="text" class="form-control" name="vendorcode" placeholder="QCO" />
                                </td>
                            </tr>
                            <tr hidden>
                                <td>
                                    <label id="lbl_sno">
                                    </label>
                                </td>
                            </tr>
                           
                        </table>
                        </div>
                        <input id='save_creamtest' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_cream_qualitytesting_click()" />
                        <input id='close_creamtest' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearvalues()" />
                        <br />
                    </div>
                </div>
            </div>
            </div>
    </section>
</asp:Content>

