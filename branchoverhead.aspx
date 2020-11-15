<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="branchoverhead.aspx.cs" Inherits="branchoverhead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            get_branch_overhead_click();
            get_Branch_details();
            get_ohmaster_details();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
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
        function get_Branch_details() {
            var data = { 'op': 'get_Branch_details_click' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbranches(msg);
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
        function fillbranches(msg) {
            var data = document.getElementById('txt_branchoh');
            var length = data.options.length;
            document.getElementById('txt_branchoh').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Branch Name";
            opt.value = "";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].branchName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].branchName;
                    option.value = msg[i].Sno;
                    data.appendChild(option);
                }
            }
        }
        function get_ohmaster_details() {
            var data = { 'op': 'get_ohmaster_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillohdetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillohdetails(msg) {
            var data = document.getElementById('slct_mainhead');
            var length = data.options.length;
            document.getElementById('slct_mainhead').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Over Head";
            opt.value = "";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].mainoh != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].mainoh;
                    option.value = msg[i].sno;
                    data.appendChild(option);
                }
            }
        }
        function fillheadnamedetails() {
            var mainhead = document.getElementById('slct_mainhead').value;
            if (mainhead == "" || mainhead == "Select Over Head") {
                alert("Please Select Main Over Head");
                return false;
            }
            var data = { 'op': 'get_overhead_details', 'mainhead': mainhead };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $("#div_headname").show();
                        var results = '<div    style="overflow:auto;"><table id="table_ohname_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Over Head Name</th><th scope="col">Amount</th><th scope="col">QuantityType</th></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr>';
                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + (i+1) + '</span></th>';
                            results += '<th><span id="txt_headname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].overheadname + '</span></th>';
                            results += '<td><input id="txt_amount" class="form-control" value="" type="text"onkeypress="return isFloat(event);"placeholder="Enter Amount"></td>';
                            results += '<td><select id="txt_quantitytype" class="form-control" value="" type="text"><option value="1">Total Quantity</option><option value="2">TQ Including Tankers</option><option value="3">TQ Including Closing Stock</option></td>';
                            results += '<td style="display:none" class="7"><input id="hdnHeadSno" class="form-control" type="number" name="vendorcode" value="' + msg[i].sno + '"></td>';
                            results += '<td style="display:none" class="8"><input id="hdnsubSno" class="form-control" type="text" ></td></tr>';

                        }
                        results += '</table></div>';
                        $("#div_headname").html(results);
                    }
                    else {
                        $("#div_headname").hide();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function save_branch_overhead_click() {
            var branchoh = document.getElementById("txt_branchoh").value;
            if (branchoh == "") {
                alert("Please Select Branch");
                return false;
            }
            var date = document.getElementById("txt_date").value;
            var mainhead = document.getElementById("slct_mainhead").value;
            if (mainhead == "") {
                alert("Please Select Main Head");
                return false;
            }
            var totalsales = document.getElementById("txt_totalsales").value;
            if (totalsales == "") {
                alert("Please enter Total Sale");
                return false;
            }
            var remarks = document.getElementById('txt_remarks').value;
            var sno = document.getElementById("txtsno").value;
            var btnval = document.getElementById("btn_save").innerHTML;

            var totalqtyinctank = document.getElementById("txt_totalqtyinctank").value;
            if (totalqtyinctank == "") {
                alert("Please enter Total Quality Including Tanks");
                return false;
            }
            var totalqtyinclosingstock = document.getElementById("txt_totalqtyinclosingstock").value;
            if (totalqtyinclosingstock == "") {
                alert("Please enter Total Quantity Including Closing Stock");
                return false;
            }
            var rows = $("#table_ohname_details tr:gt(0)");
            var headnames = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txt_amount').val() == "" || $(this).find('#txt_amount').val() == "0") {
                }
                else {
                    headnames.push({ headsno: $(this).find('#hdnHeadSno').val(), hdnsubSno: $(this).find('#hdnsubSno').val(), headName: $(this).find('#txt_headname').text(), amount: $(this).find('#txt_amount').val(), quantitytype: $(this).find('#txt_quantitytype').val() });
                }
            });
            if (headnames.length == "") {
                alert("Please Enter Ammount");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_branch_overhead_click', 'branchoh': branchoh, 'date': date, 'headnames': headnames, 'mainhead': mainhead, 'totalsales': totalsales, 'totalqtyinctank': totalqtyinctank, 'totalqtyinclosingstock': totalqtyinclosingstock, 'remarks': remarks, 'sno': sno, 'btnval': btnval };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            forclearall();
                            get_branch_overhead_click();
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
        function forclearall() {
            document.getElementById('txt_branchoh').selectedIndex = 0;
            document.getElementById('slct_mainhead').selectedIndex=0;
            document.getElementById('txt_totalsales').value = "";
            document.getElementById('txt_remarks').value = "";
            document.getElementById('txt_totalqtyinctank').value = "";
            document.getElementById('txt_totalqtyinclosingstock').value = "";
            document.getElementById('btn_save').innerHTML = "Save";
            document.getElementById('txt_headname').value = "";
            document.getElementById('txt_amount').value = "";
            document.getElementById('txt_quantitytype').selectedIndex = 0;
            $("#div_headname").hide();
        }
        function get_branch_overhead_click() {
            var data = { 'op': 'get_branch_overhead_click' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbranchdetails(msg);
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
        function fillbranchdetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col">Branch Name</th><th scope="col">Date</th><th scope="col">MainHead</th><th scope="col">TotalSale</th><th scope="col">TotalQuantityIncludingTankers</th><th scope="col">TotalQuantityIncludingClosingStock</th><th scope="col">Remarks</th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td style="display:none" class="9">' + msg[i].branchoh + '</td>';
                results += '<td  class="10">' + msg[i].branchoh1 + '</td>';
                results += '<td  class="1">' + msg[i].date + '</td>';
                results += '<td  class="2" style="display:none">' + msg[i].mainhead + '</td>';
                results += '<td  class="3" style="display:none">' + msg[i].mainhead1 + '</td>';
                results += '<td class="10">' + msg[i].mainoverheadname + '</td>';
                results += '<td class="4">' + msg[i].totalsales + '</td>';
                results += '<td  class="7">' + msg[i].totalqtyinctank + '</td>';
                results += '<td  class="8">' + msg[i].totalqtyinclosingstock + '</td>';
                results += '<td  class="5">' + msg[i].remarks + '</td>';
                results += '<td style="display:none" class="6">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getcoln(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_data").html(results);
        }
        function getcoln(thisid) {
            scrollTo(0, 0);
            var date = $(thisid).parent().parent().children('.1').html();
            var mainhead = $(thisid).parent().parent().children('.2').html();
            var mainhead1 = $(thisid).parent().parent().children('.3').html();
            var totalsales = $(thisid).parent().parent().children('.4').html();
            var remarks = $(thisid).parent().parent().children('.5').html();
            var sno = $(thisid).parent().parent().children('.6').html();
            var totalqtyinctank = $(thisid).parent().parent().children('.7').html();
            var totalqtyinclosingstock = $(thisid).parent().parent().children('.8').html();
            var branchoh = $(thisid).parent().parent().children('.9').html();
            var branchoh1 = $(thisid).parent().parent().children('.10').html();
            
            document.getElementById('txt_date').value = date;
            document.getElementById('slct_mainhead').value = mainhead;
            document.getElementById('txt_totalsales').value = totalsales;
            document.getElementById('txt_totalqtyinctank').value = totalqtyinctank;
            document.getElementById('txt_totalqtyinclosingstock').value = totalqtyinclosingstock;
            document.getElementById('txt_remarks').value = remarks;
            document.getElementById('txtsno').value = sno;
            document.getElementById('txt_branchoh').value = branchoh;
            document.getElementById('btn_save').innerHTML = "Modify";
            get_subbranch_overhead_click(sno);
        }
        function get_subbranch_overhead_click(sno) {
            var data = { 'op': 'get_subbranch_overhead_click', 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $("#div_headname").show();
                        var results = '<div    style="overflow:auto;"><table id="table_ohname_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Over Head Name</th><th scope="col">Amount</th><th scope="col">QuantityType</th></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            results += '<tr>';
                            results += '<th><span id="Span1" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + (i + 1) + '</span></th>';
                            results += '<th><span id="txt_headname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].headName + '</span></th>';
                            results += '<td><input id="txt_amount" class="form-control"  type="text"  value="' + msg[i].amount + '"></td>';
                            results += '<td><select id="txt_quantitytype" class="form-control" type="text" value="' + msg[i].quantitytype + '"><option value="1">Total Quantity</option><option value="2">TQ Including Tankers</option><option value="3">TQ Including Closing Stock</option></td>';
                            results += '<td style="display:none" class="7"><input id="hdnHeadSno" class="form-control" type="text" value="' + msg[i].headid + '"></td>';
                            results += '<td style="display:none" class="8"><input id="hdnsubSno" class="form-control" type="text" value="' + msg[i].headsno + '" ></td></tr>';
                        }
                        results += '</table></div>';
                        $("#div_headname").html(results);
                    }
                    else {
                        $("#div_headname").hide();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
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
            Branch Over Head<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Branch Over Head</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Branch Over Head Details
                </h3>
            </div>
            <div class="box-body">
                <div id="div_Payment">
                </div>
                <div id='fillform'  style="padding-left:200px;">
                    <table>
                    <tr>
                            <td>
                                <label>
                                   Branch</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="txt_branchoh" class="form-control" type="text">
                                </select>
                            </td>
                        </tr>
                     <tr>
                            <td>
                                <label>
                                   Date</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_date" class="form-control" type="date"/>
                            </td>
                        </tr>
                           <tr>
                            <td>
                                <label>
                                    Main Head</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="slct_mainhead" class="form-control" onchange="fillheadnamedetails();">
                                </select>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <label>
                                   Total Sales</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_totalsales" class="form-control" type="text"onkeypress="return isFloat(event);" placeholder="Enter Total Sales" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                   TQ Including Tankers</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_totalqtyinctank" class="form-control" type="text"onkeypress="return isFloat(event);"placeholder="Enter Total Quantity Including Tankers" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                   TQ Including Closing Stock</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_totalqtyinclosingstock" class="form-control" type="text"onkeypress="return isFloat(event);"placeholder="Enter Total Quantity Including Closing Stock" />
                            </td>
                        </tr>
                        </table>
                        <table>
                        <tr>
                            <td colspan="4">
                                <div id="div_headname">
                                </div>
                            </td>
                        </tr>
                       </table>
                       <table>
                        <tr>
                            <td>
                                <label>
                                   Remarks</label>
                            </td>
                            <td style="height: 40px;padding-left:110px;">
                                <textarea id="txt_remarks" class="form-control" type="text" ></textarea>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                <label id="txtsno">
                                </label>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                       <%-- <tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input id="btn_save" type="button" class="btn btn-success" name="submit" value='Save'
                                    onclick="save_branch_overhead_click();" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Clear'
                                    onclick="forclearall()" />
                            </td>
                        </tr>--%>
                    </table>
                    <div  style="padding-left: 33%;padding-top: 2%;padding-bottom: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="save_branch_overhead_click()"></span><span id="btn_save" onclick="save_branch_overhead_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="forclearall()"></span><span id='btn_close' onclick="forclearall()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                 </div>
                <div id="div_data">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
