<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="flowoperation.aspx.cs" Inherits="flowoperation" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
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
        $('#txtdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        $('#add_employee').click(function () {
            $('#div_addemp').css('display', 'block');
            $('#emp_showlogs').css('display', 'none');
            $('#div_empmaster_table').css('display', 'none');
            prev_pass = "";
            $('#tablerow_id').hide();
            $('#login_details').css('display', 'none');
        });
        $('#close_empdiv').click(function () {
            clearcontrols();
            $('#div_addemp').css('display', 'none');
            $('#emp_showlogs').css('display', 'block');
            $('#div_empmaster_table').css('display', 'block');
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

    function callhandlernojsonpost(d, s, e) {
        $.ajax({
            type: "POST",
            url: 'FleetManagementHandler.axd',
            data: d,
            processData: false,
            contentType: false,
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
    function save_flowdata_click() {
        var empnum = document.getElementById('txt_empnum').value;
        if (empnum == "") {
            $("#txt_empnum").focus();
            alert("Please Enter Employee Number ");
            return false;
        }
        var date = document.getElementById('txtdate').value;
        if (date == "") {
            $("#txt_empname").focus();
            alert("Please Enter Employee Name");
            return;
        }
        var dataURL = document.getElementById('main_img').src;
        var div_text = $('#yourBtn').text().trim();
        var blob = dataURItoBlob(dataURL);
        var data = { 'op': 'flow_details_save', 'empnum': empnum, 'date': date, 'canvasImage': blob };
        var s = function (msg) {
            if (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('btn_upload_profilepic').disabled = true;
                }
                else {
                    document.location = "Default.aspx";
                }
            }
            else {

            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callhandlernojsonpost(data, s, e);
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


    <script type="text/javascript">
       
        function ValidateAlpha(evt) {
            var keyCode = (evt.which) ? evt.which : evt.keyCode
            if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

                return false;
            return true;
        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
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
      
        function check_login(thisid) {
            var btnval = document.getElementById('save_employee').innerHTML;
            if (btnval == "Save") {
                
            }
        }

        function upload_profile_pic() {
            var dataURL = document.getElementById('main_img').src;
            var div_text = $('#yourBtn').text().trim();
            var blob = dataURItoBlob(dataURL);
            var Data = new FormData();
            Data.append("op", "emp_profile_pic_files_upload");
            Data.append("empsno", empsno);
            Data.append("empcode", empcode);
            Data.append("canvasImage", blob);
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('btn_upload_profilepic').disabled = true;
                }
                else {
                    document.location = "Default.aspx";
                }
            };
            var e = function (x, h, e) {
            };
            callHandler_nojson_post(Data, s, e);
        }
        
        function getflowmwterreadingdetails() {

        }
        //-------------> allow only required extention
        function hasExtension(fileName, exts) {
            return (new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$')).test(fileName);
        }

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#main_img,#img_1').attr('src', e.target.result).width(200).height(200);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        function getFile() {
            document.getElementById("file").click();
        }
        //----------------> convert base 64 to file
        function dataURItoBlob(dataURI) {
            // convert base64/URLEncoded data component to raw binary data held in a string
            var byteString;
            if (dataURI.split(',')[0].indexOf('base64') >= 0)
                byteString = atob(dataURI.split(',')[1]);
            else
                byteString = unescape(dataURI.split(',')[1]);
            // separate out the mime component
            var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
            // write the bytes of the string to a typed array
            var ia = new Uint8Array(byteString.length);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            return new Blob([ia], { type: 'image/jpeg' });
        }

        
        function getFile_doc() {
            document.getElementById("FileUpload1").click();
        }
       
        function readURL_doc(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.readAsDataURL(input.files[0]);
                document.getElementById("FileUpload_div").innerHTML = input.files[0].name;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Flow Meter Master
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Processing</a></li>
            <li><a href="#">Flow Meter</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-user-plus"></i>Flow Meter Details
                </h3>
            </div>
            <div class="box-body">
                <div class="maindiv">
                    <div id="emp_showlogs" style="text-align: center;">
                        <table>
                            <tr>
                          
                                <td>
                                <div class="input-group">
                                <div class="input-group-addon" style="border-color: #3c8dbc;background-color: #3c8dbc;border-radius: 4px;color: whitesmoke;">
                                <span class="glyphicon glyphicon-plus-sign"  ></span> <span id="add_employee" ">Add Employee</span>
                          </div>
                          </div>
                                    <%--<input id="add_employee" type="button" class="btn btn-primary" name="submit" value="Add Employee">--%>
                                </td>
                             
                            </tr>
                        </table>
                    </div>
                    <div id="div_addemp" style="display: none;">
                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="well panel panel-default" style="padding: 0px;">
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-sm-4" style="width: 100%;">
                                                    <div class="row">
                                                        <div class="col-xs-12 col-sm-3 text-center">
                                                            <div class="pictureArea1">
                                                                <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img"
                                                                    src="Images/Employeeimg.jpg" alt="your image" style="border-radius: 5px; width: 200px;
                                                                    height: 200px; border-radius: 50%;" />
                                                                <div class="photo-edit-admin">
                                                                    <a onclick="getFile();" class="photo-edit-icon-admin" href="/employee/emp-master/emp-photo?eid=3"
                                                                        title="Change Profile Picture" data-toggle="modal" data-target="#photoup"><i class="fa fa-pencil">
                                                                        </i></a>
                                                                </div>
                                                                <div id="yourBtn" class="img_btn" onclick="getFile();" style="margin-top: 5px; display: none;">
                                                                    Click to Choose Image
                                                                </div>
                                                                <div style="height: 0px; width: 0px; overflow: hidden;">
                                                                    <input id="file" type="file" name="files[]" onchange="readURL(this);">
                                                                </div>
                                                                <div>
                                                                    <input type="button" id="btn_upload_profilepic" class="btn btn-primary" onclick="upload_profile_pic();"
                                                                        style="margin-top: 5px;" value="Upload Pic">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!--/col-->
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="div_basic_details" style="display: block;">
                                <div class="box box-danger">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-4">
                                                <label class="control-label" for="txt_empname">
                                                    Flow Metter Reading</label>
                                                <span style="color: Red;font-weight: bold;">*</span>
                                                <input type="text" id="txt_empnum" class="form-control" value="" placeholder="Enter Employee Number">
                                            </div>
                                            <div class="col-sm-4">
                                                <label class="control-label" for="txt_empname">
                                                    Date</label>
                                                <span style="color: Red;font-weight: bold;">*</span>
                                                 <input id="txtdate" class="form-control" type="datetime-local" name="vendorcode" placeholder="Enter Date">   
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                
                            </div>
                            
                            <div id="btn_modify" style="width: 8%; text-align: center; display: block;margin-left: 42%;">
                            <table align="center">
                            <tr>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon" style="border-color: #3c8dbc;background-color: #3c8dbc;border-radius: 4px;height: 30px!important;color: whitesmoke;">
                                <span id="save_employee1"  class="glyphicon glyphicon-ok-sign" > </span> <span  id="save_employee" onclick="save_flowdata_click();">Save</span>
                             </div>
                             </div>
                            </td>
                            <td  style="padding-left: 2%;"> 
                            <div class="input-group">
                                <div class="input-group-addon" style="border-color:#D9534F;background-color: #D9534F;border-radius: 4px;color: whitesmoke;">
                                <span   class="glyphicon glyphicon-remove-sign"  ></span> <span  id="close_empdiv">Close</span>
                          </div>
                          </div>
                            </td>
                            </tr>
                            </table>
                            
                                 
                            </div>
                        </div>
                    </div>
                </div>
                <div id="div_empmaster_table" >
                    <table id="tbl_empmaster" class="table table-bordered table-hover dataTable no-footer">
                        <thead>
                            <tr style="background:#cbc6dd;">
                                <th scope="col">
                                    Sno
                                </th>
                                <th scope="col" >
                                   Flowmeter Reading
                                </th>
                                <th scope="col">
                                    Image
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </section>
</asp:Content>