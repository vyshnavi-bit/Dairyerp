<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="overhead.aspx.cs" Inherits="overhead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(function () {
        get_overhead_click();
        $('#btn_addHead').click(function () {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            forclearall();
        });

        $('#btn_close').click(function () {
            $('#fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_Deptdata').show();
            forclearall();
            get_overhead_click();
        });
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
        function save_overhead_click() {
            var mainhead = document.getElementById('txt_mainhead').value;
            var overheadname = document.getElementById('txt_overheadname').value;
            var status = document.getElementById('txt_status').value;
            var sno = document.getElementById('lbl_sno').value;
            var btnval = document.getElementById('btn_save').value;
            if (mainhead == "") {
                alert("Select Main Head");
                return false;
            }
            if (overheadname == "") {
                alert("Enter Over Head Name");
                return false;
            }
            if (status == "") {
                alert("Select Status");
                return false;
            }
            var data = { 'op': 'save_overhead_click', 'mainhead': mainhead, 'overheadname': overheadname, 'btnval': btnval, 'sno': sno, 'status': status };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        forclearall();
                        get_overhead_click();
                        $('#div_Deptdata').show();
                        $('#fillform').css('display', 'none');
                        $('#showlogs').css('display', 'block');
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
        function forclearall() {
            document.getElementById('txt_mainhead').selectedIndex = 0;
            document.getElementById('txt_overheadname').value = "";
            document.getElementById('txt_status').selectedIndex = 0;
            document.getElementById('btn_save').value = "Save";
        }
        function get_overhead_click() {
            var data = { 'op': 'get_overhead_click' };
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
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Main Head</th><th scope="col">Head Name</th><th scope="col">Status</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-success" value="Edit" /></td>';
                results += '<td data-title="Code" class="1" >' + msg[i].mainhead1 + '</td>';
                results += '<td style="display:none" data-title="Code" class="2">' + msg[i].mainhead + '</td>';
                results += '<td data-title="Code" class="3">' + msg[i].overheadname + '</td>';
                results += '<td  class="4">' + msg[i].status + '</td>';
                results += '<td style="display:none" class="5">' + msg[i].sno + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function getme(thisid) {
            var mainhead1 = $(thisid).parent().parent().children('.1').html();
            var mainhead = $(thisid).parent().parent().children('.2').html();
            var overheadname = $(thisid).parent().parent().children('.3').html();
            var status = $(thisid).parent().parent().children('.4').html();
            var sno = $(thisid).parent().parent().children('.5').html();

            document.getElementById('txt_mainhead').value = mainhead;
            document.getElementById('txt_overheadname').value = overheadname;
            document.getElementById('txt_status').selectedIndex= status;
            document.getElementById('lbl_sno').value = sno;
            document.getElementById('btn_save').value = "Modify";
            $("#div_Deptdata").hide();
            $("#fillform").show();
            $('#showlogs').hide();
        }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            Over Head Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#"> Over Head Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i> Over Head Master
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addHead" type="button" name="submit" value='Add Head' class="btn btn-success" />
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='fillform' style="display: none; padding-left:223px;">
                    <table cellpadding="1px" align="center" style="width: 60%;">
                        <tr>
                            <td>
                                <label>
                                    Main Head</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="txt_mainhead" class="form-control">
                                    <option value="Operating Expenses">Operating Expenses</option>
                                    <option value="Administrative Expenses">Administrative Expenses</option>
                                    <option value="Employee Benefit Expenses">Employee Benefit Expenses</option>
                                    <option value="Rates and Taxes">Rates and Taxes</option>
                                    <option value="Financial Expenses">Financial Expenses</option>
                                    <option value="Selling and Distribution Expenses">Selling and Distribution Expenses</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Over Head Name</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_overheadname" class="form-control" type="text" placeholder="Over Head Name" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Status</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="txt_status" class="form-control" style="width: 100px;">
                                <option value="">Select</option>
                                    <option value="1">Active</option>
                                    <option value="0">InActive</option>
                                </select>
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="height:40px;">
                                <input type="button" class="btn btn-success" name="submit" 
                                    id="btn_save" value='Save' onclick="save_overhead_click()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </section>
</asp:Content>