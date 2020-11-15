<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="gheequalitytestingapproval.aspx.cs" Inherits="gheequalitytestingapproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            get_gheeproductqualitytesting_details();
        });

        //Function for only no
        $(document).ready(function () {
            $("#txt_dcno,#txt_Inwardno").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
                // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
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
       
        function clearvalues() {
            document.getElementById('txt_date').value = "";
            document.getElementById('txt_qco').value = "";
            document.getElementById('txt_Remarks').value = "";
            document.getElementById('txt_Chemist').value = "";
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
        function get_gheeproductqualitytesting_details() {
            var data = { 'op': 'get_gheeproductqualitytesting_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillqualitydetails(msg);
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
        function fillqualitydetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Product Name</th><th scope="col">Packet Size</th><th scope="col">Sample No</th><th scope="col">Used By Date</th><th scope="col">Temp</th><th scope="col">MRP</th><th scope="col">OT</th><th scope="col">Acidity</th><th scope="col">Structure</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].productname + '</td>';
                results += '<td scope="row" class="2" style="text-align:center;">' + msg[i].packetsize + '</td>';
                results += '<td scope="row" class="3" style="text-align:center;">' + msg[i].sampleno + '</td>';
                results += '<td scope="row" class="4" style="text-align:center;">' + msg[i].usedbydate + '</td>';
                results += '<td scope="row" class="5" style="text-align:center;">' + msg[i].ptemp + '</td>';
                results += '<td scope="row" class="6" style="text-align:center;">' + msg[i].pmrp + '</td>';
                results += '<td scope="row" class="7" style="text-align:center;">' + msg[i].pot + '</td>';
                results += '<td scope="row" class="8" style="text-align:center;">' + msg[i].pacidity + '</td>';
                results += '<td scope="row" class="9" style="text-align:center;">' + msg[i].pstructure + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].productid + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].sno + '</td>';
                results += '<td><input id="btn_poplate" type="button"  onclick="approve(this)" name="submit" class="btn btn-success" value="Approve" /></td>';
                results += '<td><input id="btn_poplate" type="button"  onclick="reject(this)" name="submit" class="btn btn-success" value="Reject" /></td></tr>';
            }
            results += '</table></div>';
            $("#div_gheedata").html(results);
        }
        function approve(thisid) {
            var sno = $(thisid).parent().parent().children('.11').html();
            var productid = $(thisid).parent().parent().children('.10').html();
            var btnvalue = "Approve";
            var confi = confirm("Do you want to Aprove Transaction ?");
            if (confi) {
                var data = { 'op': 'Approve_ghee_packetwise_qualitytesting_click', 'sno': sno, 'productid': productid, 'btnvalue': btnvalue
                };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        get_gheeproductqualitytesting_details();
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
        function reject(thisid) {
            var sno = $(thisid).parent().parent().children('.11').html();
            var productid = $(thisid).parent().parent().children('.10').html();
            var btnvalue = "Reject";
            var confi = confirm("Do you want to Aprove Transaction ?");
            if (confi) {
                var data = { 'op': 'Approve_ghee_packetwise_qualitytesting_click', 'sno': sno, 'productid': productid, 'btnvalue': btnvalue
                };
                var s = function (msg) {
                    if (msg) {

                        alert(msg);
                        get_gheeproductqualitytesting_details();
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
           GHEE Section Qualtity Testing <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">GHEE Section Qualtity Testing</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>GHEE Section Qualtity Testing Entry
                </h3>
            </div>
            <div class="box-body">
                <div id="div_gheedata">
                </div>
            </div>
            </div>
    </section>
</asp:Content>

