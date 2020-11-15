<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="ohmaster.aspx.cs" Inherits="ohmaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript">
    $(function () {
        $("#div_oh").css("display", "block");
        $("#div_nameoh").css("display", "none");
        get_ohmaster_details();
        $('#btn_addohmain').click(function () {
            $('#showlogs_oh').hide();
            $('#div_ohdata').hide();
            $('#fill_oh').show();
            get_ohmaster_details();
        });
        $('#btn_close').click(function () {
            $('#showlogs_oh').show();
            $('#div_ohdata').show();
            $('#fill_oh').hide();
            forclearall();
        });

        $('#butn_ohname').click(function () {
            $('#show_logs_nameoh').hide();
            $('#div_nameohdata').hide();
            $('#fill_nameoh').show();
            get_ohmaster_details();
        });

        $('#btn_cancelohname').click(function () {
            $('#show_logs_nameoh').show();
            $('#div_nameohdata').show();
            $('#fill_nameoh').hide();
            get_ohmastername_details();
            cancelohnamedetails();
        });
    });
        function showohmain() {
            $("#div_oh").css("display", "block");
            $("#div_nameoh").css("display", "none");
            get_ohmaster_details();
        }
        function showohname() {
            $("#div_nameoh").css("display", "block");
            $("#div_oh").css("display", "none");
            get_ohmaster_details();
            get_ohmastername_details();
        }
        function addoh() {
            $('#showlogs_oh').hide();
            $('#div_ohdata').hide();
            $('#fill_oh').show();
            get_ohmaster_details();
        }
        function addohname() {
            $('#show_logs_nameoh').hide();
            $('#div_nameohdata').hide();
            $('#fill_nameoh').show();
            get_ohmaster_details();
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
        function save_ohmaster_details() {
            var mainoh = document.getElementById('txt_mainoh').value;
            if (mainoh == "") {
                alert("Enter Main Over Head ");
                $('#txt_mainoh').focus();
                return false;
            }
            var sno = document.getElementById('lbl_snooh').value;
            var btnval = document.getElementById('btn_save').innerHTML;
            var data = { 'op': 'save_ohmaster_details', 'mainoh': mainoh, 'btnval': btnval, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        get_ohmaster_details();
                        $('#showlogs_oh').show();
                        $('#div_ohdata').show();
                        $('#fill_oh').hide();
                        forclearall();
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
            document.getElementById('txt_mainoh').value = "";
            document.getElementById('btn_save').innerHTML = "Save";
            $('#showlogs_oh').show();
            $('#div_ohdata').show();
            $('#fill_oh').hide();
        }
        function get_ohmaster_details() {
            var data = { 'op': 'get_ohmaster_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillohdetails(msg);
                        fillohnamedetails(msg);
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
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col"  style="font-weight: bold;">OH Name</th><th scope="col"  style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Code" class="1" >' + msg[i].mainoh + '</td>';
                results += '<td style="display:none" class="2">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeoh(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_ohdata").html(results);
        }
        function getmeoh(thisid) {
            var mainoh = $(thisid).parent().parent().children('.1').html();
            var sno = $(thisid).parent().parent().children('.2').html();

            document.getElementById('txt_mainoh').value = mainoh;
            document.getElementById('lbl_snooh').value = sno;
            document.getElementById('btn_save').innerHTML = "Modify";
            $("#div_ohdata").hide();
            $("#fill_oh").show();
            $('#showlogs_oh').hide();
        }
        function fillohnamedetails(msg) {
            var data = document.getElementById('txt_mainoverhead');
            var length = data.options.length;
            document.getElementById('txt_mainoverhead').options.length = null;
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
        function save_nameoh_details() {
            var mainoverhead = document.getElementById('txt_mainoverhead').value;
            if (mainoverhead == "") {
                alert("Select Main Over Head");
                $('#txt_mainoverhead').focus();
                return false;
            }
            var status = document.getElementById('txt_status').value;
            var overheadname = document.getElementById('txt_overheadname').value; 
            if (overheadname == "") {
                alert("Enter Over Head Name");
                $('#txt_overheadname').focus();
                return false;
            }
            var sno = document.getElementById('lbl_snooh').value;
            var btnval = document.getElementById('bttn_nameohsave').innerHTML;
            var data = { 'op': 'save_nameoh_details', 'mainoverhead': mainoverhead, 'overheadname': overheadname, 'btnval': btnval, 'sno': sno, 'status': status };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        $('#div_nameohdata').show();
                        $('#fill_nameoh').css('display', 'none');
                        $('#show_logs_nameoh').css('display', 'block');
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
        function cancelohnamedetails() {
            document.getElementById('txt_mainoverhead').selectedIndex = 0;
            document.getElementById('txt_status').selectedIndex = 0;
            document.getElementById('txt_overheadname').value = "";
            document.getElementById('bttn_nameohsave').innerHTML = "Save";
            $('#show_logs_nameoh').show();
            $('#div_nameohdata').show();
            $('#fill_nameoh').hide();
            get_ohmastername_details();
        }
        function get_ohmastername_details() {
            var data = { 'op': 'get_ohmastername_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillohdetailsname(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillohdetailsname(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col"  style="font-weight: bold;">Over Head</th><th scope="col">Over Head Name</th><th scope="col">Status</th><th scope="col"  style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td  class="1" >' + msg[i].mainoh + '</td>';
                results += '<td  class="2" >' + msg[i].ohname + '</td>';
                results += '<td  class="3" >' + msg[i].status + '</td>';
                results += '<td style="display:none" class="4" >' + msg[i].mainohid + '</td>';
                results += '<td style="display:none" class="5">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeohname(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_nameohdata").html(results);
        }
        function getmeohname(thisid) {
            var mainoh = $(thisid).parent().parent().children('.1').html();
            var ohname = $(thisid).parent().parent().children('.2').html();
            var status = $(thisid).parent().parent().children('.3').html();
            var mainohid = $(thisid).parent().parent().children('.4').html();
            var sno = $(thisid).parent().parent().children('.5').html();

            document.getElementById('txt_mainoverhead').value = mainohid;
            document.getElementById('txt_overheadname').value = ohname;
            if (status == "True") {
                document.getElementById('txt_status').value = 1;
            }
            else if (status == "False") {
                document.getElementById('txt_status').value = 0;
            }
            document.getElementById('lbl_snonameoh').value = sno;
            document.getElementById('bttn_nameohsave').innerHTML = "Modify";
            $("#show_logs_nameoh").hide();
            $('#div_nameohdata').hide();
            $("#fill_nameoh").show();
        }


</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Over Head Master
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operation</a></li>
            <li><a href="#">Over Head Master</a></li>
        </ol>
    </section>
    <section>
        <section class="content">
            <div class="box box-info">
                <div>
                    <ul class="nav nav-tabs">
                        <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showohmain()">
                            <i class="fa fa-street-view"></i>&nbsp;&nbsp;Main Over Head</a></li>
                        <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showohname()">
                            <i class="fa fa-file-text"></i>&nbsp;&nbsp;Over Head Master</a></li>
                    </ul>
                </div>
                 <div id="div_oh" >
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Main Over Head
                        </h3>
                    </div>
                    <div class="box-body">
                        <div id="showlogs_oh" style="text-align: -webkit-right;">
                            <%--<input id="btn_addohmain" type="button" name="submit" value='Add OH' class="btn btn-primary" />--%>
                            <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addoh()"></span> <span onclick="addoh()">Add OH</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                        </div>
                        <div id="div_ohdata">
                        </div>
                        <div id='fill_oh' style="display: none;">
                            <table align="center" style="width: 60%;">
                                 <tr>
                                    <td style="height: 40px;">
                                     <label>  Over Head </label> <span style="color: red;">*</span>
                                    </td>
                                    <td>
                                      <input id="txt_mainoh" type="text"  class="form-control" placeholder="Enter Over Head" />
                                    </td>
                                </tr>
                                
                                <tr style="display: none;">
                                    <td>
                                        <label id="lbl_snooh">
                                        </label>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td colspan="6" align="center" style="height: 40px;">
                                        <input id="btn_save" type="button" class="btn btn-primary" name="submit" value='Save'
                                            onclick="save_ohmaster_details()" />
                                        <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close'
                                            onclick="canceldetails()" />
                                    </td>
                                </tr>--%>
                            </table>
                            <div  style="padding-left: 35%;">
                            <table>
                            <tr>
                            <td>
                                <div class="input-group">
                                    <div class="input-group-addon">
                                    <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="save_ohmaster_details()"></span><span id="btn_save" onclick="save_ohmaster_details()">Save</span>
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
                        </div>
                        </div>

                        <div id="div_nameoh" style="display: none;">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Over Head Master
                        </h3>
                    </div>
                    <div class="box-body">
                        <div id="show_logs_nameoh" style="text-align: -webkit-right;">
                           <%-- <input id="butn_ohname" type="button" name="submit" value='Add OH Name' class="btn btn-primary" />--%>
                            <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addohname()"></span> <span onclick="addohname()">Add OH Name</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                        </div>
                        <div id="div_nameohdata">
                        </div>
                        <div id='fill_nameoh' style="display: none;">
                         <table align="center" style="width: 60%;">
                                <tr>
                                </tr>
                                <tr>
                                    <td style="height: 40px;">
                                       <label>  Over Head </label> <span style="color: red;">*</span>
                                    </td>
                                    <td>
                                      <select type="text" id="txt_mainoverhead" class="form-control" >
                                      </select>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                   <label>  Over Head Name </label> <span style="color: red;">*</span> 
                                     </td>
                                   <td>
                                    <input type="text" class="form-control" id="txt_overheadname" placeholder="Enter Over Head Name"/>
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
                                <tr style="display: none;">
                                    <td>
                                        <label id="lbl_snonameoh">
                                        </label>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td colspan="2" align="center" style="height: 40px;">
                                        <input id="bttn_nameohsave" type="button" class="btn btn-primary" name="submit" value='Save'
                                            onclick="save_nameoh_details()" />
                                        <input id='btn_cancelohname' type="button" class="btn btn-danger" name="Close" value='Close'
                                            onclick="cancelohnamedetails()" />
                                    </td>
                                </tr>--%>
                            </table>
                            <div  style="padding-left: 35%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="bttn_nameohsave1" onclick="save_nameoh_details()"></span><span id="bttn_nameohsave" onclick="save_nameoh_details()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_cancelohname1' onclick="cancelohnamedetails()"></span><span id='btn_cancelohname' onclick="cancelohnamedetails()">Close</span>
                            </div>
                            </div>
                            </td>
                            </tr>
                            </table>
                        </div>
                       </div>
                    </div>
                </div>
              </div>
        </section>
    </section>
</asp:Content>
