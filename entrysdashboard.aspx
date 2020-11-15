<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="entrysdashboard.aspx.cs" Inherits="entrysdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
    .blink_me {
    -webkit-animation-name: blinker;
    -webkit-animation-duration: 2s;
    -webkit-animation-timing-function: linear;
    -webkit-animation-iteration-count: infinite;
    
    -moz-animation-name: blinker;
    -moz-animation-duration: 1s;
    -moz-animation-timing-function: linear;
    -moz-animation-iteration-count: infinite;
    
    animation-name: blinker;
    animation-duration: 1s;
    animation-timing-function: linear;
    animation-iteration-count: infinite;
    }
    @-moz-keyframes blinker {  
        0% { opacity: 1.0; }
        50% { opacity: 0.0; }
        100% { opacity: 1.0; }
    }

    @-webkit-keyframes blinker {  
        0% { opacity: 1.0; }
        50% { opacity: 0.0; }
        100% { opacity: 1.0; }
    }

    @keyframes blinker {  
        0% { opacity: 1.0; }
        50% { opacity: 0.0; }
        100% { opacity: 1.0; }
    }
</style>
<script type="text/javascript">
    $(function () {
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
        $('#txt_date').val(yyyy + '-' + mm + '-' + dd);
        get_entry_details();
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
    function get_entry_details() {
        var data = { 'op': 'get_entry_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    filldetails(msg);
                    blinkFont();
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
    function filldetails(msg) {
        var serverdatedate = document.getElementById('txt_date').value;
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col">Entries Type</th><th scope="col">Status</th><th scope="col">Last Update</th></tr></thead></tbody>';
        for (var i = 0; i < msg.length; i++) {
            var dates = msg[i].date;
            if (serverdatedate == dates) {
                results += '<tr>';
                results += '<td class="1">' + msg[i].type + '</td>';
                results += '<td class="2">True</td>';
                results += '<td class="3">' + msg[i].mdate + '</td>';
                results += '</tr>';
            }
            else {
                results += '<tr style="background-color: #58D5E3;" class="blink_me">';
                results += '<td class="1">' + msg[i].type + '</td>';
                results += '<td class="2">False</td>';
                results += '<td class="3">' + msg[i].mdate + '</td>';
                results += '</tr>'
            }
        }
        results += '</table></div>';
        $("#div_getdata").html(results);
    }
    function blinkFont() {
        $('.blinck').each(function (i, obj) {
            var qtyclass = $(this).text();
            if (qtyclass == "            " || qtyclass == "") {
                $(this).parent().css('background', 'aqua');
                $(this).parent().css('color', 'aqua');
            }  
        });
        setTimeout("setblinkFont()", 200)
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <section class="content-header">
        <h1>
           Dash Board <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#"> Dash Board</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Dash Board
                </h3>
            </div>
            <div class="box-body">
            <div style="text-align: -webkit-center;" hidden>
                <table>
                    <tr>
                        <td>
                            <input id="txt_date" class="form-control" type="date" onblur="get_entry_details();" />
                        </td>
                    </tr>
                </table>
            </div>
                <div id="div_getdata">
                </div>
            </div>
            </div>
    </section>
</asp:Content>

