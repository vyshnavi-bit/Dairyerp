<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="AddBranch.aspx.cs" Inherits="AddBranch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <link href="css/formstable.css" rel="stylesheet" type="text/css" />
    <link href="css/custom.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
        $(function () {
            get_Branch_details();
            $('#btn_addDept').click(function () {
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
            });
        });
        function addbranches() {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            forclearall();
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
        //Function for only no
        $(document).ready(function () {
            $("#txt_phoneno,#txt_servtax").keydown(function (event) {
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
        //------------>Prevent Backspace<--------------------//
        $(document).unbind('keydown').bind('keydown', function (event) {
            var doPrevent = false;
            if (event.keyCode === 8) {
                var d = event.srcElement || event.target;
                if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD'))
            || d.tagName.toUpperCase() === 'TEXTAREA') {
                    doPrevent = d.readOnly || d.disabled;
                } else {
                    doPrevent = true;
                }
            }

            if (doPrevent) {
                event.preventDefault();
            }
        });
        function validateEmail(email) {
            var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
            if (reg.test(email)) {
                return true;
            }
            else {
                return false;
            }
        }
        function save_branchdata_click() {
            var branchname = document.getElementById('txt_branchname').value;
            if (branchname == "") {
                alert("Please Enter branch name");
                $("#txt_branchname").focus();
                return false;
            }
            var address = document.getElementById('txt_Address').value;
            if (address == "") {
                alert("Please Enter address");
                $("#txt_Address").focus();
                return false;
            }
            var branchtype = document.getElementById('slct_branch_type').value;
            if (branchtype == "" || branchtype == "Select Branch Type") {
                alert("Please Select Branch Type");
                $("#slct_branch_type").focus();
                return false;
            }
            var tin = document.getElementById('txt_tin').value;
            var sno = document.getElementById('lbl_sno').innerHTML;
            var btnval = document.getElementById('btn_save').innerHTML;
            var branchcode = document.getElementById('txt_branchcode').value;
            var cstno = document.getElementById('txt_cstno').value;
            var mitno = document.getElementById('txt_mitno').value;
            var sapcodebranch = document.getElementById('txt_sapcodebranch').value;
            var data = { 'op': 'save_branchdata_click', 'branchcode': branchcode, 'cstno': cstno, 'mitno': mitno, 'branchname': branchname, 'address': address, 'branchtype': branchtype, 'tin': tin, 'btnval': btnval, 'sno': sno, 'sapcodebranch': sapcodebranch };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        forclearall();
                        get_Branch_details();
                        $('#div_Deptdata').show();
                        $('#fillform').css('display', 'none');
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
        function forclearall() {
            document.getElementById('txt_sapcodebranch').value = "";
            document.getElementById('txt_branchname').value = "";
            document.getElementById('txt_Address').value = "";
            document.getElementById('slct_branch_type').selectedIndex = 0;
            document.getElementById('txt_tin').value = "";
            document.getElementById('txt_branchcode').value = "";
            document.getElementById('txt_cstno').value = "";
            document.getElementById('txt_mitno').value = "";
            document.getElementById('btn_save').innerHTML = "Save";
        }
        function get_Branch_details() {
            var data = { 'op': 'get_Branch_details' };
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
            var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Branch Name</th><th scope="col" style="font-weight: bold;">BranchType</th><th scope="col" style="font-weight: bold;">Tin</th><th scope="col" style="font-weight: bold;">CST No</th><th scope="col" style="font-weight: bold;">MITNo</th><th scope="col" style="font-weight: bold;">SAPCode</th><th scope="col" style="font-weight: bold;">BranchCode</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<th scope="row" class="1" style="display:none">' + msg[i].branchName + '</th>';
                results += '<td data-title="brandstatus" class="20"><span class="glyphicon glyphicon-triangle-right" style="color: cadetblue;">&nbsp</span>' + msg[i].branchName + '</td>';
                results += '<td style="display:none" data-title="Code" class="3">' + msg[i].type + '</td>';
                results += '<td data-title="brandstatus" class="21"><span class="" style="color: cadetblue;"></span> ' + msg[i].type + '</td>';
                results += '<td  data-title="Code" class="4">' + msg[i].tin + '</td>';
                results += '<td class="5">' + msg[i].cstno + '</td>';
                results += '<td class="6">' + msg[i].mitno + '</td>';
                results += '<td  class="10" style="display:none">' + msg[i].sapcodebranch + '</td>';
                results += '<td data-title="brandstatus" class="23"><span class="" style="color: cadetblue;"></span> ' + msg[i].sapcodebranch + '</td>';
                results += '<td class="7" >' + msg[i].branchcode + '</td>';
                results += '<td data-title="Code" style="display:none" class="2">' + msg[i].address + '</td>';
                results += '<td style="display:none" class="8">' + msg[i].Sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function getme(thisid) {
            scrollTo(0, 0);
            var sno = $(thisid).parent().parent().children('.8').html();
            var branchName = $(thisid).parent().parent().children('.1').html();
            branchName = replaceHtmlEntites(branchName);
            var address = $(thisid).parent().parent().children('.2').html();
            address = replaceHtmlEntites(address);
            var type = $(thisid).parent().parent().children('.3').html();
            type = replaceHtmlEntites(type);
            var tin = $(thisid).parent().parent().children('.4').html();
            tin = replaceHtmlEntites(tin);

            var cstno = $(thisid).parent().parent().children('.5').html();
            cstno = replaceHtmlEntites(cstno);
            var mitno = $(thisid).parent().parent().children('.6').html();
            mitno = replaceHtmlEntites(mitno);
            var branchcode = $(thisid).parent().parent().children('.7').html();
            branchcode = replaceHtmlEntites(branchcode);
            var sapcodebranch = $(thisid).parent().parent().children('.10').html();
            sapcodebranch = replaceHtmlEntites(sapcodebranch);

            document.getElementById('txt_branchname').value = branchName;
            document.getElementById('txt_Address').value = address;
            document.getElementById('slct_branch_type').value = type;
            document.getElementById('txt_tin').value = tin;
            document.getElementById('txt_cstno').value = cstno;
            document.getElementById('txt_mitno').value = mitno;
            document.getElementById('txt_branchcode').value = branchcode;
            document.getElementById('txt_sapcodebranch').value = sapcodebranch;

            document.getElementById('lbl_sno').innerHTML = sno;
            document.getElementById('btn_save').innerHTML = "Modify";
            $("#div_Deptdata").hide();
            $("#fillform").show();
            $('#showlogs').hide();
        }
        var replaceHtmlEntites = (function () {
            var translate_re = /&(nbsp|amp|quot|lt|gt);/g;
            var translate = {
                "nbsp": " ",
                "amp": "&",
                "quot": "\"",
                "lt": "<",
                "gt": ">"
            };
            return function (s) {
                return (s.replace(translate_re, function (match, entity) {
                    return translate[entity];
                }));
            }
        })();
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Branch Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Branch Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Branch Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;" >
                   <%-- <input id="btn_addDept" type="button" name="submit" value='Add Branch' class="btn btn-success" />--%>
                   <table>
                        <tr>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addbranches()"></span> <span onclick="addbranches()">Add Branch</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='fillform' style="display: none; padding-left:30%;">
                    <table align="center">
                        <tr>
                            <th>
                            </th>
                        </tr>
                       
                        <tr>
                            <td>
                               <label> Branch Code </label> <span style="color: red;">*</span>
                                <input type="text" maxlength="45" id="txt_branchcode" class="form-control" name="vendorcode"
                                    placeholder="Enter Branch Code">
                            </td>
                            <td style="padding-left: 5%;">
                              <label>   Branch Name </label> <span style="color: red;">*</span>
                                <input type="text" maxlength="45" id="txt_branchname" class="form-control" name="vendorcode"
                                    placeholder="Enter Branch Name">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label> Address </label> <span style="color: red;">*</span>
                                <input type="text" id="txt_Address" class="form-control" name="vendorcode" placeholder="Enter Address">
                            </td>
                            <td style="padding-left: 5%;">
                               <label>  Branch Type </label> <span style="color: red;">*</span>
                                <select id="slct_branch_type" class="form-control">
                                    <option selected disabled value="Select Branch Type">Select Branch Type</option>
                                    <option>Plant</option>
                                    <option>CC</option>
                                    <option>Other</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td >
                              <label>   Tin </label> <span style="color: red;">*</span>
                                <input type="text" maxlength="45" id="txt_tin" class="form-control" name="vendorcode"
                                    placeholder="Enter Tin">
                            </td>
                            <td style="padding-left: 5%;">
                                <label> CST No </label> <span style="color: red;">*</span>
                                <input type="text" maxlength="45" id="txt_cstno" class="form-control" name="vendorcode"
                                    placeholder="Enter  CST No">
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <label>MIT No </label>  <span style="color: red;">*</span>
                                <input type="text" maxlength="45" id="txt_mitno" class="form-control" name="vendorcode"
                                    placeholder="Enter  MIT No">
                            </td>
                            <td style="padding-left: 5%;">
                               <label> SAP Code </label> <span style="color: red;">*</span>
                                <input type="text" maxlength="45" id="txt_sapcodebranch" class="form-control" name="sapcode"
                                    placeholder="Enter SAP Code">
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td colspan="2" align="center" style="height:40px;">
                                <input type="button"  name="submit" class="btn btn-success"
                                    id="btn_save" value='Save' onclick="save_branchdata_click()" />
                                    <input id='btn_close'
                                        type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>--%>
                    </table>
                     <div  style="padding-left: 10%;padding-top: 5%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="save_branchdata_click()"></span><span id="btn_save" onclick="save_branchdata_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="closebranches()"></span><span id='btn_close' onclick="closebranches()">Close</span>
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
