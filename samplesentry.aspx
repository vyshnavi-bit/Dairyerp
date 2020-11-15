<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="samplesentry.aspx.cs" Inherits="samplesentry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        input[type=number]::-webkit-inner-spin-button, input[type=number]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            get_sampleno_details();
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
        //Function for only no
        $(document).ready(function () {
            $("#txt_phoneno").keydown(function (event) {
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
        function save_edit_sampledetails() {
            var type = document.getElementById('slct_Department').value;
            if (type == "" || type == "Select") {
                alert("Please enter Sample Type");
                $("#slct_Department").focus();
                return false;
            }
            var celltype = document.getElementById('slctcelltype').value;
            if (celltype == "") {
                alert("Please enter celltype");
                $("#slctcelltype").focus();
                return false;
            }
            var dcno = document.getElementById('txtdcno').value;
            if (dcno == "") {
                alert("Please enter dcno");
                $("#txtdcno").focus();
                return false;
            }
            var vehicleno = document.getElementById('txtvehicleno').value;
            if (vehicleno == "") {
                alert("Please enter department name");
                $("#txtvehicleno").focus();
                return false;
            }
            var sampleno = document.getElementById('txtsmpno').value;
            if (sampleno == "") {
                alert("Please enter Sample No");
                $("#txtsmpno").focus();
                return false;
            }
            var sno = document.getElementById('lbl_sno').innerHTML;
            var btnval = document.getElementById('btn_save_batch').innerHTML;
            var data = { 'op': 'save_edit_sampledetails', 'type': type, 'celltype': celltype, 'dcno': dcno, 'vehicleno': vehicleno, 'sampleno': sampleno, 'btnval': btnval, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "ok") {
                            alert("Successfully Created");
                            get_sampleno_details();

                        }
                        else {
                            alert("Please enter valid details");
                        }
                    }
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall() {
            document.getElementById('txt_code').value = "";
            document.getElementById('txt_name').value = "";
            document.getElementById('btn_save').innerHTML = "Save";
        }
        function get_sampleno_details() {
            var data = { 'op': 'get_sampleno_details' };
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
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="text-align:center;font-weight: bold;">Sample Type</th><th scope="col" style="text-align:center;font-weight: bold;">Sample No</th><th scope="col">doe</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<th scope="row" class="1">' + msg[i].silo + '</th>';
                results += '<td data-title="brandstatus" class="41"><span class="glyphicon glyphicon-triangle-right" style="color: cadetblue;"></span> ' + msg[i].status + '</td>';
                results += '<td data-title="Code" class="2" style="display:none"  >' + msg[i].batch + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].Sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_batchdata").html(results);
        }
        function getme(thisid) {
            scrollTo(0, 0);
            var sno = $(thisid).parent().parent().children('.3').html();
            var code = $(thisid).parent().parent().children('.2').html();
            code = replaceHtmlEntites(code);
            var name = $(thisid).parent().parent().children('.1').html();
            name = replaceHtmlEntites(name);
            document.getElementById('txt_code').value = code;
            document.getElementById('txt_name').value = name;
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

        //-----------------------------------------------------------------------------------------
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
       
        function forclearall_emp() {
            document.getElementById('txt_Empname').value = "";
            document.getElementById('txt_UserName').value = "";
            document.getElementById('txt_passward').value = "";
            document.getElementById('slct_empDepart_Name').selectedIndex = 0;
            document.getElementById('btn_save_emp').innerHTML = "Save";
            document.getElementById('slct_branch').selectedIndex = 0;
            $("#lbl_code_error_msg").hide();
            $("#lbl_name_error_msg").hide();
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="content">
        <div class="box box-info">
          <div id="div_batch">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Sample Entry Details
                        </h3>
                    </div>
            <div class="box-body">
                <div id='fillform_batch'  align="center">
                    <table>
                        <tr>
                            <th>
                            </th>
                        </tr>
                        <tr>
                          
                            <td  style="height: 40px;">
                              <label>
                                    Sample Type<span style="color: red;">*</span></label>
                            </td>
                            <td>
                              
                                <select id="slct_Department" class="form-control">
                                   <option value="Select" selected>Select</option>
                                   <option value="Audit">Audit</option>
                                   <option value="Lab">Lab</option>
                                </select>
                            </td>  

                            <td  style="height: 40px;">
                              <label>
                                    Cell Type<span style="color: red;">*</span></label>
                            </td>
                            <td>
                              
                                <select id="slctcelltype" class="form-control">
                                   <option value="Fcell" selected>Fcell</option>
                                   <option value="Mcell">Mcell</option>
                                   <option value="BCell">BCell</option>
                                </select>
                            </td>
                     
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    DCNO
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txtdcno" class="form-control" name="vendorcode"
                                    placeholder="Enter DCNo">
                            </td>

                            <td style="height: 40px;">
                                <label>
                                    Vehicle No
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txtvehicleno" class="form-control" name="vendorcode"
                                    placeholder="Enter Vehicle No">
                            </td>


                        </tr>
                        <tr>
                            <td style="height: 40px;"> 
                                <label>
                                    Sample No
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txtsmpno" class="form-control" name="vendorcode"
                                    placeholder="Enter Sample No">
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                    </table>
                    <div  style="padding-left: 0%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save_batch1" onclick="save_edit_sampledetails()"></span><span id="btn_save_batch" onclick="save_edit_sampledetails()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close_batch1' onclick="closebatch()"></span><span id='btn_close_batch' onclick="closebatch()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                </div>
                <div id="div_batchdata">
                </div>
            </div>
        </div>
            </div>
    </div>
</asp:Content>

