<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="personalinfo.aspx.cs" Inherits="personalinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function show_personal() {
            get_personal_details();
        });
        function closepersonaldetail() {
            $('#fillform_person').css('display', 'none');
            $('#showlogs_person').css('display', 'block');
            $('#div_persondata').show();
            forclearall_person();
            get_personal_details();
        }
        function addperonaldetails() {
            $('#fillform_person').css('display', 'block');
            $('#showlogs_person').css('display', 'none');
            $('#div_persondata').hide();
            forclearall_person();
        }
        function get_personal_details() {
            var data = { 'op': 'get_personal_detailstest' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        BindPersonaldetails(msg);
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
        function BindPersonaldetails(msg) {
            var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Name</th><th scope="col" style="font-weight: bold;">Mobile Number</th><th scope="col" style="font-weight: bold;">Email</th><th scope="col" style="font-weight: bold;">Message Type</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme_person(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" class="1" style="display:none">' + msg[i].name + '</td>';
                results += '<td data-title="brandstatus" class="41"><span class="fa fa-user" style="color: cadetblue;"></span> ' + msg[i].name + '</td>';
                results += '<td data-title="Capacity" style="display:none" class="2">' + msg[i].mobno + '</td>';
                results += '<td data-title="brandstatus" class="42"><span class="glyphicon glyphicon-phone-alt" style="color: cadetblue;"></span>' + msg[i].mobno + '</td>';
                results += '<td data-title="Capacity" style="display:none" class="3">' + msg[i].email + '</td>';
                results += '<td data-title="brandstatus" class="43"><span class="fa fa-envelope" style="color: cadetblue;"></span>' + msg[i].email + '</td>';
                results += '<td data-title="Capacity" class="4">' + msg[i].msgtype + '</td>';
                results += '<td style="display:none" class="5">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme_person(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_persondata").html(results);
        }
        function getme_person(thisid) {
            scrollTo(0, 0);
            var name = $(thisid).parent().parent().children('.1').html();
            var mobno = $(thisid).parent().parent().children('.2').html();
            var email = $(thisid).parent().parent().children('.3').html();
            var msgtype = $(thisid).parent().parent().children('.4').html();
            var id = $(thisid).parent().parent().children('.5').html();
            document.getElementById('txt_person_name').value = name;
            document.getElementById('txt_phoneno').value = mobno;
            document.getElementById('txt_email').value = email;
            document.getElementById('ddl_msgtype').value = msgtype;
            document.getElementById('lbl_sno_person').innerHTML = id;
            document.getElementById('btn_save_person').innerHTML = "Modify";
            $("#div_persondata").hide();
            $("#fillform_person").show();
            $('#showlogs_person').hide();
        }
        function forclearall_person() {
            document.getElementById('txt_person_name').value = "";
            document.getElementById('txt_phoneno').value = "";
            document.getElementById('txt_email').value = "";
            document.getElementById('ddl_msgtype').selectedIndex = 0;
            document.getElementById('btn_save_person').innerHTML = "Save";
        }
        function ValidateNos() {
            var phoneNo = document.getElementById('txt_phoneno');
            if (phoneNo.value == "" || phoneNo.value == null) {
                alert("Please enter your Mobile No.");
                $('#txt_phoneno').focus();
                return false;
            }
            if (phoneNo.value.length < 10 || phoneNo.value.length > 10) {
                alert("Mobile No. is not valid, Please Enter 10  Digit Mobile No.");
                $('#txt_phoneno').focus();
                return false;
            }
        }
        function validationemail() {
            var x = document.getElementById("txt_email").value;
            var atpos = x.indexOf("@");
            var dotpos = x.lastIndexOf(".");
            if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
                alert("Not a valid e-mail address")
            }
        }

        function save_personal_details() {
            var name = document.getElementById('txt_person_name').value;
            if (name == "") {
                alert("Please Enter name");
                $("#txt_person_name").focus();
                return false;
            }
            var phoneno = document.getElementById('txt_phoneno').value;
            if (phoneno == "") {
                alert("Please Enter Phone Number");
                $("#txt_phoneno").focus();
                return false;
            }
            if (phoneno.length < 10 || phoneno.length > 10) {
                alert("Mobile No. is not valid, Please Enter 10  Digit Mobile No.");
                return false;
            }
            var email = document.getElementById('txt_email').value;
            if (email == "") {
                alert("Please Enter Email");
                $("#txt_email").focus();
                return false;
            }
            var x = document.getElementById("txt_email").value;
            var atpos = x.indexOf("@");
            var dotpos = x.lastIndexOf(".");
            if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
                alert("Not a valid e-mail address")
            }
            var msgtype = document.getElementById('ddl_msgtype').value;
            if (msgtype == "") {
                alert("Please Select Message Type");
                $("#ddl_msgtype").focus();
                return false;
            }
            var id = document.getElementById('lbl_sno_person').innerHTML;
            var btnval = document.getElementById('btn_save_person').innerHTML;
            var data = { 'op': 'save_personal_details_clicktest', 'name': name, 'phoneno': phoneno, 'email': email, 'msgtype': msgtype, 'btnval': btnval, 'id': id };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        forclearall_person();
                        get_personal_details();
                        $('#div_persondata').show();
                        $('#fillform_person').css('display', 'none');
                        $('#showlogs_person').css('display', 'block');
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
         <div class="box box-info">
            <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Personal Details
                    </h3>
                </div>
            <div class="box-body">
              <div id="showlogs_person" style="text-align: -webkit-right;">
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addperonaldetails()"></span> <span onclick="addperonaldetails()">Add Details</span>
                          </div>
                          </div>
                            </td>
                         </tr>
                      </table>
                </div>
                <div id="div_persondata">
                </div>
                <div id='fillform_person' style="display: none;" align="center">
                    <table  style="width: 60%;">
                        <tr>
                            <th>
                            </th>
                        </tr>
                        <tr>
                            <td style="height:40px;">
                             <label>    Name </label> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_person_name" class="form-control" name="vendorcode"
                                    placeholder="Enter Name"><label id="Label2" class="errormessage">*
                                        Please Enter Name</label>
                            </td>
                        </tr>
                       <tr>
                            <td style="height:40px;">
                                 <label>  Mob No </label> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="10" id="txt_phoneno" class="form-control" name="vendorcode"
                                    placeholder="Enter Mobile Number" onkeypress="return isNumber(event);" onchange="return ValidateNos();" ><label id="lbl_mob_error_msg" class="errormessage">*
                                        Please Enter Mob No</label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:40px;">
                                  <label> Email </label> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_email" class="form-control" name="vendorcode"
                                    placeholder="Enter Email" onchange="validationemail();"><label id="lbl_email_error_msg" class="errormessage">*
                                        Please Enter Email</label>
                            </td>
                        </tr>
                         <tr>
                            <td style="height:40px;">
                           <label>     Message Type </label> <span style="color: red;">*</span>
                            </td>
                            <td>
                               <select id="ddl_msgtype"  class="form-control">
                               <option>Inward</option>
                               <option>Outward</option>
                               <option>Security Inward</option>
                               </select>
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno_person">
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
                                <span class="glyphicon glyphicon-ok" id="btn_save_person1" onclick="save_personal_details()"></span><span id="btn_save_person" onclick="save_personal_details()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close_person1' onclick="closepersonaldetail()"></span><span id='btn_close_person' onclick="closepersonaldetail()">Close</span>
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
