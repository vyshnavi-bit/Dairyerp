<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="minimasters.aspx.cs" Inherits="closingdetails" %>

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
            $('#div_dept').css('display', 'block');
            $('#div_emp').css('display', 'none');
            $('#div_silo').css('display', 'none');
            $('#div_processing').css('display', 'none');
            $('#div_shift').css('display', 'none');
            $('#div_batch').css('display', 'none');
            $('#div_person').css('display', 'none');
            $('#div_vechile').css('display', 'none');
            $('#div_states').css('display', 'none');
            get_Dept_details();
            $('#btn_close').click(function () {
                $('#fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_Deptdata').show();
                forclearall();
            });
            //get_Dept_details();
        });
        function adddepartments() {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_Deptdata').hide();
            forclearall();
        }
        function closedept() {
            $('#fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_Deptdata').show();
            forclearall();
        }
        function show_department() {
            $('#div_dept').css('display', 'block');
            $('#div_emp').css('display', 'none');
            $('#div_silo').css('display', 'none');
            $('#div_processing').css('display', 'none');
            $('#div_shift').css('display', 'none');
            $('#div_batch').css('display', 'none');
            $('#div_person').css('display', 'none');
            $('#div_vechile').css('display', 'none');
            $('#div_states').css('display', 'none');
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
            get_Dept_details();
        }
        function show_employee() {
            $('#div_dept').css('display', 'none');
            $('#div_emp').css('display', 'block');
            $('#div_silo').css('display', 'none');
            $('#div_processing').css('display', 'none');
            $('#div_shift').css('display', 'none');
            $('#div_batch').css('display', 'none');
            $('#div_person').css('display', 'none');
            $('#div_vechile').css('display', 'none');
            $('#div_states').css('display', 'none');
            get_departments();
            get_branchs();
            $('#btn_close_emp').click(function () {
                $('#fillform_emp').css('display', 'none');
                $('#showlogs_emp').css('display', 'block');
                $('#div_empdata').show();
                forclearall_emp();
            });
            get_Emp_details();
        }
        function addemployess() {
            $('#fillform_emp').css('display', 'block');
            $('#showlogs_emp').css('display', 'none');
            $('#div_empdata').hide();
            get_departments();
            forclearall_emp();
        }
        function cloaseemployee() {
            $('#fillform_emp').css('display', 'none');
            $('#showlogs_emp').css('display', 'block');
            $('#div_empdata').show();
            forclearall_emp();
        }
        function show_silo() {
            $('#div_dept').css('display', 'none');
            $('#div_emp').css('display', 'none');
            $('#div_silo').css('display', 'block');
            $('#div_processing').css('display', 'none');
            $('#div_shift').css('display', 'none');
            $('#div_batch').css('display', 'none');
            $('#div_person').css('display', 'none');
            $('#div_vechile').css('display', 'none');
            $('#div_states').css('display', 'none');
            $('#btn_close_silo').click(function () {
                $('#fillform_silo').css('display', 'none');
                $('#showlogs_silo').css('display', 'block');
                $('#div_Silodata').show();
                forclearall_silo();
            });
            get_Silo_details();
        }
        function closesilos() {
            $('#fillform_silo').css('display', 'none');
            $('#showlogs_silo').css('display', 'block');
            $('#div_Silodata').show();
            forclearall_silo();
        }
        function addsilos() {
            $('#fillform_silo').css('display', 'block');
            $('#showlogs_silo').css('display', 'none');
            $('#div_Silodata').hide();
            forclearall_silo();
        }
        function show_processing() {
            $('#div_processing').css('display', 'block');
            $('#div_dept').css('display', 'none');
            $('#div_emp').css('display', 'none');
            $('#div_silo').css('display', 'none');
            $('#div_shift').css('display', 'none');
            $('#div_batch').css('display', 'none');
            $('#div_person').css('display', 'none');
            $('#div_vechile').css('display', 'none');
            $('#div_states').css('display', 'none');
            $('#btn_close_processing').click(function () {
                $('#fillform_processing').css('display', 'none');
                $('#showlogs_processing').css('display', 'block');
                $('#div_processingdata').show();
                forclearall_process();
            });
            get_processingdepartment_details();
        }
        function closesilodepartments() {
            $('#fillform_processing').css('display', 'none');
            $('#showlogs_processing').css('display', 'block');
            $('#div_processingdata').show();
            forclearall_process();
        }
        function addprocessdepartments() {
            $('#fillform_processing').css('display', 'block');
            $('#showlogs_processing').css('display', 'none');
            $('#div_processingdata').hide();
            forclearall_process();
        }
        function show_shift() {
            $('#div_dept').css('display', 'none');
            $('#div_emp').css('display', 'none');
            $('#div_silo').css('display', 'none');
            $('#div_processing').css('display', 'none');
            $('#div_shift').css('display', 'block');
            $('#div_batch').css('display', 'none');
            $('#div_person').css('display', 'none');
            $('#div_vechile').css('display', 'none');
            $('#div_states').css('display', 'none');
            $('#btn_close_shift').click(function () {
                $('#fillform_shift').css('display', 'none');
                $('#showlogs_shift').css('display', 'block');
                $('#div_shiftdata').show();
                forclearall_shift();
            });
            get_Shift_details();
        }
        function closeshift() {
            $('#fillform_shift').css('display', 'none');
            $('#showlogs_shift').css('display', 'block');
            $('#div_shiftdata').show();
            forclearall_shift();
        }
        function addsift() {
            $('#fillform_shift').css('display', 'block');
            $('#showlogs_shift').css('display', 'none');
            $('#div_shiftdata').hide();
            forclearall_shift();
        }
        function show_batch() {
            $('#div_dept').css('display', 'none');
            $('#div_emp').css('display', 'none');
            $('#div_silo').css('display', 'none');
            $('#div_processing').css('display', 'none');
            $('#div_shift').css('display', 'none');
            $('#div_batch').css('display', 'block');
            $('#div_person').css('display', 'none');
            $('#div_vechile').css('display', 'none');
            $('#div_states').css('display', 'none');
            $('#btn_close_batch').click(function () {
                $('#fillform_batch').css('display', 'none');
                $('#showlogs_batch').css('display', 'block');
                $('#div_batchdata').show();
                forclearall_batch();
            });
            get_batch_details();
            get_SiloDepartments();
        }
        function closebatch() {
            $('#fillform_batch').css('display', 'none');
            $('#showlogs_batch').css('display', 'block');
            $('#div_batchdata').show();
            forclearall_batch();
        }
        function addbatch() {
            $('#fillform_batch').css('display', 'block');
            $('#showlogs_batch').css('display', 'none');
            $('#div_batchdata').hide();
            forclearall_batch();
            get_SiloDepartments();
        }
        function show_personal() {
            $('#div_dept').css('display', 'none');
            $('#div_emp').css('display', 'none');
            $('#div_silo').css('display', 'none');
            $('#div_processing').css('display', 'none');
            $('#div_shift').css('display', 'none');
            $('#div_batch').css('display', 'none');
            $('#div_person').css('display', 'block');
            $('#div_vechile').css('display', 'none');
            $('#div_states').css('display', 'none');
            $('#btn_close_person').click(function () {
                $('#fillform_person').css('display', 'none');
                $('#showlogs_person').css('display', 'block');
                $('#div_persondata').show();
                forclearall_person();
                get_personal_details();
            });
            get_personal_details();
        }
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
            get_personal_details();
        }
        function show_vechile() {
            $('#div_dept').css('display', 'none');
            $('#div_emp').css('display', 'none');
            $('#div_silo').css('display', 'none');
            $('#div_processing').css('display', 'none');
            $('#div_shift').css('display', 'none');
            $('#div_batch').css('display', 'none');
            $('#div_person').css('display', 'none');
            $('#div_vechile').css('display', 'block');
            $('#div_states').css('display', 'none');
            $('#btn_close_vechile').click(function () {
                $('#fillform_vechile').css('display', 'none');
                $('#showlogs_vechile').css('display', 'block');
                $('#div_vechiledata').show();
                vehclecell();
                forclearall_vechile();
            });
            get_vechile_details();
        }
        function closevehicledetails() {
            $('#fillform_vechile').css('display', 'none');
            $('#showlogs_vechile').css('display', 'block');
            $('#div_vechiledata').show();
            vehclecell();
            forclearall_vechile();
        }
        function addvehicledetails() {
            $('#fillform_vechile').css('display', 'block');
            $('#showlogs_vechile').css('display', 'none');
            $('#div_vechiledata').hide();
            vehclecell();
            forclearall_vechile();
        }
        function show_statemaster() {
            $('#div_dept').css('display', 'none');
            $('#div_emp').css('display', 'none');
            $('#div_silo').css('display', 'none');
            $('#div_processing').css('display', 'none');
            $('#div_shift').css('display', 'none');
            $('#div_batch').css('display', 'none');
            $('#div_person').css('display', 'none');
            $('#div_vechile').css('display', 'none');
            $('#div_states').css('display', 'block');
            get_state_details();
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
        function for_save_edit_Dept() {
            var code = document.getElementById('txt_code').value;
            if (code == "") {
                alert("Please enter department code");
                $("#txt_code").focus();
                return false;
            }
            var name = document.getElementById('txt_name').value;
            if (name == "") {
                alert("Please enter department name");
                $("#txt_name").focus();
                return false;
            }
            var sno = document.getElementById('lbl_sno').innerHTML;
            var btnval = document.getElementById('btn_save').innerHTML;
            var data = { 'op': 'for_save_edit_Dept_click', 'code': code, 'name': name, 'btnval': btnval, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New Department Successfully Created");
                            forclearall();
                            get_Dept_details();
                            $('#div_Deptdata').show();
                            $('#fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                        }
                        else if (msg == "UPDATE") {
                            alert("  Department Successfully Modified");
                            forclearall();
                            get_Dept_details();
                            $('#div_Deptdata').show();
                            $('#fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                        }
                        else {
                            alert(msg);
                        }
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
            document.getElementById('txt_code').value = "";
            document.getElementById('txt_name').value = "";
            document.getElementById('btn_save').innerHTML = "Save";
        }
        function get_Dept_details() {
            var data = { 'op': 'get_Dept_details' };
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
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="text-align:center;font-weight: bold;">Department Name</th><th scope="col" style="text-align:center;font-weight: bold;">Department Code</th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<th scope="row" class="1" style="display:none" >' + msg[i].Name + '</th>';
                results += '<td data-title="brandstatus" class="41"><span class="glyphicon glyphicon-triangle-right" style="color: cadetblue;"></span> ' + msg[i].Name + '</td>';
                results += '<td data-title="Code" class="2" style="display:none"  >' + msg[i].Code + '</td>';
                results += '<td data-title="brandstatus" class="42"><span class="glyphicon glyphicon-barcode" style="color: cadetblue;"></span> ' + msg[i].Code + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].Sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
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

        function get_departments() {
            var data = { 'op': 'get_Dept_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillprocdepartments(msg);
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
        function fillprocdepartments(msg) {
            var data = document.getElementById('slct_empDepart_Name');
            var length = data.options.length;
            document.getElementById('slct_empDepart_Name').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Depart Name";
            opt.value = "Select Depart Name";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].Name != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].Name;
                    option.value = msg[i].Sno;
                    data.appendChild(option);
                }
            }
        }
        function get_branchs() {
            var data = { 'op': 'get_Branch_details' };
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
            var data = document.getElementById('slct_branch');
            var length = data.options.length;
            document.getElementById('slct_branch').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Branch Name";
            opt.value = "Select Branch Name";
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
        function for_save_edit_Employee() {
            var Empname = document.getElementById('txt_Empname').value;
            if (Empname == "") {
                alert("Please enter Employee Name");
                $("#txt_Empname").focus();
                return false;
            }
            var Deptsno = document.getElementById('slct_empDepart_Name').value;
            if (Deptsno == "" || Deptsno == "Select Depart Name") {
                alert("Please Select Department  Name");
                $("#slct_empDepart_Name").focus();
                return false;
            }
            var branchid = document.getElementById('slct_branch').value;
            if (branchid == "" || branchid == "Select Branch Name") {
                alert("Please Select Branch Name");
                $("#slct_branch").focus();
                return false;
            }
            var UserName = document.getElementById('txt_UserName').value;
            if (UserName == "") {
                alert("Please Enter User Name");
                $("#txt_UserName").focus();
                return false;
            }
            var passward = document.getElementById('txt_passward').value;
            if (passward == "") {
                alert("Please Enter Password");
                $("#txt_UserName").focus();
                return false;
            }
            var sno = document.getElementById('lbl_emp').innerHTML;
            var btnval = document.getElementById('btn_save_emp').innerHTML;
            var data = { 'op': 'for_save_edit_Employee_click', 'Empname': Empname, 'Deptsno': Deptsno, 'UserName': UserName, 'passward': passward, 'btnval': btnval, 'branchid': branchid, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("Employee Details Successfully Saved");
                            forclearall_emp();
                            get_Emp_details();
                            $('#div_empdata').show();
                            $('#fillform_emp').css('display', 'none');
                            $('#showlogs_emp').css('display', 'block');
                        }
                        else if (msg == "UPDATE") {
                            alert("Employee Details Successfully Modified");
                            forclearall_emp();
                            get_Emp_details();
                            $('#div_empdata').show();
                            $('#fillform_emp').css('display', 'none');
                            $('#showlogs_emp').css('display', 'block');
                        }
                        else {
                            alert(msg);
                        }
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
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

        function get_Emp_details() {
            var data = { 'op': 'get_Emp_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails_emp(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldetails_emp(msg) {
            var results = '<div  id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Branch Name</th><th scope="col" style="font-weight: bold;">Department Name</th></th><th scope="col" style="font-weight: bold;">Employee Name</th><th scope="col" style="font-weight: bold;">UserName</th><th scope="col" style="font-weight: bold;">Level Type</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme_emp(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td data-title="Code" style="display:none" class="7">' + msg[i].branchname + '</td>';
                results += '<td data-title="brandstatus" class="42"><span class="glyphicon glyphicon-list" style="color: cadetblue;"></span> ' + msg[i].branchname + '</td>';
                results += '<td data-title="Code"  class="10">' + msg[i].deptname + '</td>';
                results += '<th scope="row" class="1" style="display:none">' + msg[i].EmpName + '</th>';
                results += '<td data-title="brandstatus" class="43"><span class="fa fa-user-plus" style="color: cadetblue;"></span> ' + msg[i].EmpName + '</td>';
                results += '<td data-title="Code" style="display:none"  class="2">' + msg[i].UserName + '</td>';
                results += '<td data-title="brandstatus" class="44"><span class="fa fa-user" style="color: cadetblue;"></span> ' + msg[i].UserName + '</td>';
                results += '<td data-title="Code"  class="11">' + msg[i].leveltype + '</td>';
                results += '<td data-title="Code" style="display:none" class="3">' + msg[i].Passward + '</td>';
                results += '<td style="display:none" class="4">' + msg[i].Deptsno + '</td>';
                results += '<td style="display:none" class="8">' + msg[i].branchid + '</td>';
                results += '<td style="display:none" class="5">' + msg[i].Sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme_emp(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_empdata").html(results);
        }
        function getme_emp(thisid) {
            scrollTo(0, 0);
            var sno = $(thisid).parent().parent().children('.5').html();
            var Deptsno = $(thisid).parent().parent().children('.4').html();
            Deptsno = replaceHtmlEntites(Deptsno);
            var EmpName = $(thisid).parent().parent().children('.1').html();
            EmpName = replaceHtmlEntites(EmpName);
            var UserName = $(thisid).parent().parent().children('.2').html();
            UserName = replaceHtmlEntites(UserName);
            var Passward = $(thisid).parent().parent().children('.3').html();
            Passward = replaceHtmlEntites(Passward);
            var branchid = $(thisid).parent().parent().children('.8').html();
            document.getElementById('txt_Empname').value = EmpName;
            document.getElementById('txt_UserName').value = UserName;
            document.getElementById('txt_passward').value = Passward;
            document.getElementById('slct_branch').value = branchid;
            document.getElementById('slct_empDepart_Name').value = Deptsno;
            document.getElementById('lbl_emp').innerHTML = sno;
            document.getElementById('btn_save_emp').innerHTML = "Modify";
            $("#div_empdata").hide();
            $("#fillform_emp").show();
            $('#showlogs_emp').hide();
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

        //-----------------------------------------------------------------------------------------------

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
        function for_save_edit_Silo() {
            var silo = document.getElementById('txt_Siloname').value;
            if (silo == "") {
                alert("Please Enter silo name");
                $("#txt_Siloname").focus();
                return false;
            }
            var capacity = document.getElementById('txt_capacity').value;
            if (capacity == "") {
                alert("Please Enter Silo Capacity");
                $("#txt_capacity").focus();
                return false;
            }
            var SiloId = document.getElementById('lbl_silo').innerHTML;
            var btnval = document.getElementById('btn_save_silo').innerHTML;
            var data = { 'op': 'for_save_edit_Silo_click', 'silo': silo, 'Capacity': capacity, 'btnval': btnval, 'SiloId': SiloId };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New Silo Successfully Created");
                            forclearall_silo();
                            get_Silo_details();
                            $('#div_Silodata').show();
                            $('#fillform_silo').css('display', 'none');
                            $('#showlogs_silo').css('display', 'block');
                        }
                        else if (msg == "UPDATE") {
                            alert("Silo Successfully Modified");
                            forclearall_silo();
                            get_Silo_details();
                            $('#div_Silodata').show();
                            $('#fillform_silo').css('display', 'none');
                            $('#showlogs_silo').css('display', 'block');
                        }
                        else {
                            alert(msg);
                        }
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall_silo() {
            document.getElementById('txt_Siloname').value = "";
            document.getElementById('txt_capacity').value = "";
            document.getElementById('btn_save_silo').innerHTML = "Save";
            $("#lbl_silo_error_msg").hide();
            $("#lbl_silocap_error_msg").hide();
        }
        function get_Silo_details() {
            var data = { 'op': 'get_Silo_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails_silo(msg);
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
        function filldetails_silo(msg) {
            var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Silo Name</th><th scope="col" style="font-weight: bold;">Capacity</th><th scope="col" style="font-weight: bold;"style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme_silo(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<th scope="row" class="1" style="display:none">' + msg[i].SiloName + '</th>';
                results += '<td data-title="brandstatus" class="41"><span class="fa fa-circle-o" style="color: cadetblue;"></span> ' + msg[i].SiloName + '</td>';
                results += '<td data-title="Capacity" style="display:none" class="2">' + msg[i].Capacity + '</td>';
                results += '<td data-title="brandstatus" class="42"><span class="fa fa-balance-scale" style="color: cadetblue;"></span> ' + msg[i].Capacity + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].SiloId + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme_silo(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Silodata").html(results);
        }
        function getme_silo(thisid) {
            scrollTo(0, 0);
            var siloid = $(thisid).parent().parent().children('.3').html();
            var capacity = $(thisid).parent().parent().children('.2').html();
            capacity = replaceHtmlEntites(capacity);
            var name = $(thisid).parent().parent().children('.1').html();
            name = replaceHtmlEntites(name);
            var silo = name;
            var capacity = capacity;
            document.getElementById('txt_Siloname').value = silo;
            document.getElementById('txt_capacity').value = capacity;
            document.getElementById('lbl_silo').innerHTML = siloid;
            document.getElementById('btn_save_silo').innerHTML = "Modify";
            $("#div_Silodata").hide();
            $("#fillform_silo").show();
            $('#showlogs_silo').hide();
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
        function for_save_edit_Silo_processing() {
            var pdepartment = document.getElementById('txt_Deptname').value;
            if (pdepartment == "") {
                alert("Please Enter Department Name");
                $("#txt_Deptname").focus();
                return false;
            }
            var pdepartmentId = document.getElementById('lbl_processing').innerHTML;
            var btnval = document.getElementById('btn_save_processing').innerHTML;
            var data = { 'op': 'for_save_edit_pdepartment_click', 'pdepartment': pdepartment, 'btnval': btnval, 'pdepartmentId': pdepartmentId };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New processing department Successfully Created");
                            forclearall_process();
                            get_processingdepartment_details();
                            $('#div_processingdata').show();
                            $('#fillform_processing').css('display', 'none');
                            $('#showlogs_processing').css('display', 'block');
                        }
                        else if (msg == "UPDATE") {
                            alert("Processing department Successfully Modified");
                            forclearall_process();
                            get_processingdepartment_details();
                            $('#div_processingdata').show();
                            $('#fillform_processing').css('display', 'none');
                            $('#showlogs_processing').css('display', 'block');
                        }
                        else {
                            alert(msg);
                        }
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall_process() {
            document.getElementById('txt_Deptname').value = "";
            document.getElementById('btn_save_processing').innerHTML = "Save";
            $("#lbl_processing_error_msg").hide();
            $("#lbl_name_error_msg").hide();
        }

        function get_processingdepartment_details() {
            var data = { 'op': 'get_processingdepartment_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails_processing(msg);
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
        function filldetails_processing(msg) {
            var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Processing Department Name</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme_processing(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td  style="display:none" class="1" >' + msg[i].DeportmentName + '</td>';
                results += '<td data-title="brandstatus" class="40"><span class="" style="color: cadetblue;"></span> ' + msg[i].DeportmentName + '</td>';
                results += '<td style="display:none" class="2">' + msg[i].SiloDeportmentId + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme_processing(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_processingdata").html(results);
        }
        function getme_processing(thisid) {
            scrollTo(0, 0);
            var SiloDepartmentId = $(thisid).parent().parent().children('.2').html();
            var DepartmentName = $(thisid).parent().parent().children('.1').html();
            DepartmentName = replaceHtmlEntites(DepartmentName);
            document.getElementById('txt_Deptname').value = DepartmentName;
            document.getElementById('lbl_processing').innerHTML = SiloDepartmentId;
            document.getElementById('btn_save_processing').innerHTML = "Modify";
            $("#div_processingdata").hide();
            $("#fillform_processing").show();
            $('#showlogs_processing').hide();
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
        function validateEmail(email) {
            var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
            if (reg.test(email)) {
                return true;
            }
            else {
                return false;
            }
        }
        function for_save_edit_shift() {
            var Shifttype = document.getElementById('txt_Shifttype').value;
            if (Shifttype == "") {
                alert("Please Enter Shift Type");
                $("#txt_Shifttype").focus();
                return false;
            }
            var shifttiming = document.getElementById('txt_shifttimings').value;
            if (shifttiming == "") {
                alert("Please Enter Shift Timing");
                $("#txt_shifttimings").focus();
                return false;
            }
            var shiftid = document.getElementById('lbl_sno_shift').innerHTML;
            var btnval = document.getElementById('btn_save_shift').innerHTML;
            var data = { 'op': 'for_save_edit_Shift_click', 'Shifttype': Shifttype, 'shifttiming': shifttiming, 'btnval': btnval, 'shiftid': shiftid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New Shift Details Successfully Created");
                            forclearall_shift();
                            get_Shift_details();
                            $('#div_shiftdata').show();
                            $('#fillform_shift').css('display', 'none');
                            $('#showlogs_shift').css('display', 'block');
                        }
                        else if (msg == "UPDATE") {
                            alert("Shift Details Successfully Modified");
                            forclearall_shift();
                            get_Shift_details();
                            $('#div_shiftdata').show();
                            $('#fillform_shift').css('display', 'none');
                            $('#showlogs_shift').css('display', 'block');
                        }
                        else {
                            alert(msg);
                        }
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall_shift() {
            document.getElementById('txt_Shifttype').value = "";
            document.getElementById('txt_shifttimings').value = "";
            document.getElementById('btn_save_shift').innerHTML = "Save";
        }

        function get_Shift_details() {
            var data = { 'op': 'get_Shift_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails_shift(msg);
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
        function filldetails_shift(msg) {
            var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Shift Name</th><th scope="col" style="font-weight: bold;">Timings</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme_shift(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" style="display:none" class="1">' + msg[i].shiftname + '</td>';
                results += '<td data-title="brandstatus" class="41"><span class="glyphicon glyphicon-triangle-right" style="color: cadetblue;"></span> ' + msg[i].shiftname + '</td>';
                results += '<td data-title="Capacity"  style="display:none" class="2">' + msg[i].timings + '</td>';
                results += '<td data-title="brandstatus" class="42"><span class="fa fa-calendar-times-o" style="color: cadetblue;"></span> ' + msg[i].timings + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].shiftid + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls" onclick="getme_shift(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_shiftdata").html(results);
        }
        function getme_shift(thisid) {
            scrollTo(0, 0);
            var shiftid = $(thisid).parent().parent().children('.3').html();
            var timings = $(thisid).parent().parent().children('.2').html();
            timings = replaceHtmlEntites(timings);
            var shiftname = $(thisid).parent().parent().children('.1').html();
            shiftname = replaceHtmlEntites(shiftname);
            var silo = name;
            var capacity = capacity;
            document.getElementById('txt_Shifttype').value = shiftname;
            document.getElementById('txt_shifttimings').value = timings;
            document.getElementById('lbl_sno_shift').innerHTML = shiftid;
            document.getElementById('btn_save_shift').innerHTML = "Modify";
            $("#div_shiftdata").hide();
            $("#fillform_shift").show();
            $('#showlogs_shift').hide();
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
        //..........................................................................................

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
        function for_save_edit_batch() {
            var dept = document.getElementById('slct_Department').value;
            if (dept == "" || dept == "Select Department") {
                alert("Please Select Department");
                $("#slct_Department").focus();
                return false;
            }
            var batchcode = document.getElementById('txtbatchcode').value;
            if (batchcode == "") {
                alert("Please Enter Batch Code");
                $("#txtbatchcode").focus();
                return false;
            }
            var batch = document.getElementById('txtbatch').value;
            if (batch == "") {
                alert("Please Enter Batch");
                $("#txtbatch").focus();
                return false;
            }
            var batchid = document.getElementById('lbl_sno_batch').innerHTML;
            var btnval = document.getElementById('btn_save_batch').innerHTML;
            var data = { 'op': 'for_save_edit_batch_click', 'dept': dept, 'batch': batch, 'batchcode': batchcode, 'btnval': btnval, 'batchid': batchid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        if (msg == "OK") {
                            alert("New batch Details Successfully Created");
                            forclearall();
                            get_batch_details();
                            $('#div_batchdata').show();
                            $('#fillform_batch').css('display', 'none');
                            $('#showlogs_batch').css('display', 'block');
                        }
                        else if (msg == "UPDATE") {
                            alert("Batch Details Successfully Modified");
                            forclearall_batch();
                            get_batch_details();
                            $('#div_batchdata').show();
                            $('#fillform_batch').css('display', 'none');
                            $('#showlogs_batch').css('display', 'block');
                        }
                        else {
                            alert(msg);
                        }
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall_batch() {
            document.getElementById('txtbatch').value = "";
            document.getElementById('txtbatchcode').value = "";
            document.getElementById('btn_save_batch').innerHTML = "Save";
            $("#lbl_code_batch_msg").hide();
            $("#lbl_name_batch_msg").hide();
            get_batch_details();
        }
        function get_SiloDepartments() {
            var data = { 'op': 'get_SiloDepartments_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldepartments(msg);
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
        function filldepartments(msg) {
            var data = document.getElementById('slct_Department');
            var length = data.options.length;
            document.getElementById('slct_Department').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Department";
            opt.value = "Select Department";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].DeportmentName != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].DeportmentName;
                    option.value = msg[i].SiloDeportmentId;
                    data.appendChild(option);
                }
            }
        }
        function get_batch_details() {
            var data = { 'op': 'get_batch_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails_batch(msg);
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
        function filldetails_batch(msg) {
            var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Batch Name</th><th scope="col" style="font-weight: bold;">Code</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme_batch(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" style="display:none" class="1" >' + msg[i].batchtype + '</td>';
                results += '<td data-title="brandstatus" class="41"><span class="glyphicon glyphicon-arrow-right" style="color: cadetblue;"></span> ' + msg[i].batchtype + '</td>';
                results += '<td style="display:none" scope="row" class="4" style="text-align:center;">' + msg[i].departmentid + '</td>';
                results += '<td data-title="Capacity" style="display:none" class="2">' + msg[i].code + '</td>';
                results += '<td data-title="brandstatus" class="41"><span class="glyphicon glyphicon-barcode" style="color: cadetblue;"></span> ' + msg[i].code + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].batchid + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme_batch(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_batchdata").html(results);
        }
        function getme_batch(thisid) {
            scrollTo(0, 0);
            var shiftid = $(thisid).parent().parent().children('.3').html();
            var departmentid = $(thisid).parent().parent().children('.4').html();
            var timings = $(thisid).parent().parent().children('.2').html();
            timings = replaceHtmlEntites(timings);
            var shiftname = $(thisid).parent().parent().children('.1').html();
            shiftname = replaceHtmlEntites(shiftname);
            var silo = name;
            var capacity = capacity;
            document.getElementById('txtbatch').value = shiftname;
            document.getElementById('slct_Department').value = departmentid;
            document.getElementById('txtbatchcode').value = timings;
            document.getElementById('lbl_sno_batch').innerHTML = shiftid;
            document.getElementById('btn_save_batch').innerHTML = "Modify";
            $("#div_batchdata").hide();
            $("#fillform_batch").show();
            $('#showlogs_batch').hide();
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
        function get_personal_details() {
            var data = { 'op': 'get_personal_details' };
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
                results += '<td style="display:none" class="5">' + msg[i].id + '</td>';
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
            var data = { 'op': 'save_personal_details_click', 'name': name, 'phoneno': phoneno, 'email': email, 'msgtype': msgtype, 'btnval': btnval, 'id': id };
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
        //......................................................................................................
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
        function save_vehicle_master_click() {
            var vehicleno = document.getElementById('txt_vehicleno').value;
            if (vehicleno == "") {
                alert("Please Enter vehicle number");
                $("#txt_vehicleno").focus();
                return false;
            }
            var nocell = document.getElementById('txt_nocell').value;
            if (nocell == "") {
                alert("Please Enter Number Of Cells");
                $("#txt_nocell").focus();
                return false;
            }
            var capacity = document.getElementById('txt_veh_capacity').value;
            var frentcell = document.getElementById('frentcell').value;
            var middlecell = document.getElementById('middlecell').value;
            var backcell = document.getElementById('backcell').value;
            var kmperrate = document.getElementById('txtKRate').value;
            var make = document.getElementById('txtMake').value;
            var sno = document.getElementById('lbl_sno_vechile').innerHTML;
            var btnval = document.getElementById('btn_save_vechile').innerHTML;
            var data = { 'op': 'save_vehicle_master_click', 'vehicleno': vehicleno, 'nocell': nocell, 'capacity': capacity, 'frentcell': frentcell, 'middlecell': middlecell, 'backcell': backcell, 'kmperrate': kmperrate, 'make': make, 'btnval': btnval, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        forclearall_vechile();
                        get_vechile_details();
                        $('#div_vechiledata').show();
                        $('#fillform_vechile').css('display', 'none');
                        $('#showlogs_vechile').css('display', 'block');
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function forclearall_vechile() {
            document.getElementById('txt_vehicleno').value = "";
            document.getElementById('txt_nocell').value = "";
            document.getElementById('txt_veh_capacity').value = "";
            document.getElementById('frentcell').value = "";
            document.getElementById('middlecell').value = "";
            document.getElementById('backcell').value = "";
            document.getElementById('txtKRate').value = "";
            document.getElementById('txtMake').value = "";
            document.getElementById('btn_save_vechile').innerHTML = "Save";
            $("#lbl_code_vechile_msg").hide();
            $("#lbl_name_vechile_msg").hide();
        }
        function vehclecell() {
            var nocell = document.getElementById('txt_nocell').value;
            if (nocell == "1") {
                $('#trfcell').css('display', 'table-row');
                $('#trmcell').css('display', 'none');
                $('#trbcell').css('display', 'none');
            }
            if (nocell == "2") {
                $('#trfcell').css('display', 'table-row');
                $('#trmcell').css('display', 'none');
                $('#trbcell').css('display', 'table-row');
            }
            if (nocell == "3") {
                $('#trfcell').css('display', 'table-row');
                $('#trmcell').css('display', 'table-row');
                $('#trbcell').css('display', 'table-row');
            }
            if (nocell == "") {
                $('#trfcell').css('display', 'none');
                $('#trmcell').css('display', 'none');
                $('#trbcell').css('display', 'none');
            }
        }
        function get_vechile_details() {
            var data = { 'op': 'get_Vehicle_Master_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails_vechile(msg);
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
        function filldetails_vechile(msg) {
            var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Vehicle No</th><th scope="col" style="font-weight: bold;">Capacity</th><th scope="col" style="font-weight: bold;">No Of Cells</th><th scope="col" style="font-weight: bold;">F Cell</th><th scope="col" style="font-weight: bold;">M Cell</th><th scope="col" style="font-weight: bold;">B Cell</th><th scope="col" style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                //results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5"  onclick="getme_vechile(this)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                results += '<td scope="row" style="display:none" class="1" >' + msg[i].vehicleno + '</td>';
                results += '<td data-title="brandstatus" class="41"><span class="fa fa-truck" style="color: cadetblue;"></span> ' + msg[i].vehicleno + '</td>';
                results += '<td data-title="Code" class="2" style="display:none">' + msg[i].capacity + '</td>';
                results += '<td data-title="brandstatus" class="41"><span class="fa fa-balance-scale" style="color: cadetblue;"></span> ' + msg[i].capacity + '</td>';
                results += '<td  class="3">' + msg[i].noofqty + '</td>';
                results += '<td  scope="row" class="5">' + msg[i].frentcell + '</td>';
                results += '<td  data-title="Code" class="6">' + msg[i].middlecell + '</td>';
                results += '<td  class="7">' + msg[i].backcell + '</td>';
                results += '<td scope="row" style="display:none" class="8">' + msg[i].kmperrate + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].make + '</td>';
                results += '<td style="display:none" class="4">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme_vechile(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_vechiledata").html(results);
        }
        function getme_vechile(thisid) {
            scrollTo(0, 0);
            var sno = $(thisid).parent().parent().children('.4').html();
            var capacity = $(thisid).parent().parent().children('.2').html();
            capacity = replaceHtmlEntites(capacity);
            var noofqty = $(thisid).parent().parent().children('.3').html();
            noofqty = replaceHtmlEntites(noofqty);
            var vehicleno = $(thisid).parent().parent().children('.1').html();
            vehicleno = replaceHtmlEntites(vehicleno);
            var frentcell = $(thisid).parent().parent().children('.5').html();
            frentcell = replaceHtmlEntites(frentcell);
            var middlecell = $(thisid).parent().parent().children('.6').html();
            middlecell = replaceHtmlEntites(middlecell);
            var backcell = $(thisid).parent().parent().children('.7').html();
            backcell = replaceHtmlEntites(backcell);
            var kmperrate = $(thisid).parent().parent().children('.8').html();
            kmperrate = replaceHtmlEntites(kmperrate);
            var make = $(thisid).parent().parent().children('.9').html();
            make = replaceHtmlEntites(make);

            document.getElementById('txt_veh_capacity').value = capacity;
            document.getElementById('txt_nocell').value = noofqty;
            document.getElementById('txt_vehicleno').value = vehicleno;
            document.getElementById('frentcell').value = frentcell;
            document.getElementById('middlecell').value = middlecell;
            document.getElementById('backcell').value = backcell;
            document.getElementById('txtKRate').value = kmperrate;
            document.getElementById('txtMake').value = make;
            document.getElementById('lbl_sno_vechile').innerHTML = sno;
            document.getElementById('btn_save_vechile').innerHTML = "Modify";
            $("#div_vechiledata").hide();
            $("#fillform_vechile").show();
            $('#showlogs_vechile').hide();
            vehclecell();
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
        function addstatemaster() {
            $('#div_addstates').hide();
            $('#div_getstates').hide();
            $('#div_fillstates').show();
        }
        function closestate() {
            document.getElementById('txt_statecode').value = "";
            document.getElementById('txt_statename').value = "";
            document.getElementById('txt_ecode').value = "";
            document.getElementById('txt_gststatecode').value = "";
            document.getElementById('lbl_statesno').innerHTML = "";
            document.getElementById('btn_savestate').innerHTML = "Save";
            $('#div_addstates').show();
            $('#div_getstates').show();
            $('#div_fillstates').hide();
        }
        function for_save_edit_states() {
            var statecode = document.getElementById('txt_statecode').value;
            if (statecode == "") {
                alert("Please Enter State Code");
                $("#txt_Siloname").focus();
                return false;
            }
            var statename = document.getElementById('txt_statename').value;
            if (statename == "") {
                alert("Please Enter State Name");
                $("#txt_capacity").focus();
                return false;
            }
            var ecode = document.getElementById('txt_ecode').value;
            if (ecode == "") {
                alert("Please Enter E Code");
                $("#txt_ecode").focus();
                return false;
            }
            var gststatecode = document.getElementById('txt_gststatecode').value;
            if (gststatecode == "") {
                alert("Please Enter GST State Code");
                $("#txt_gststatecode").focus();
                return false;
            }
            var sno = document.getElementById('lbl_statesno').innerHTML;
            var btnval = document.getElementById('btn_savestate').innerHTML;
            var data = { 'op': 'for_save_edit_states', 'statecode': statecode, 'statename': statename, 'sno': sno, 'btnval': btnval, 'ecode': ecode, 'gststatecode': gststatecode };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        $('#div_addstates').show();
                        $('#div_getstates').show();
                        $('#div_fillstates').hide();
                        closestate();
                        get_state_details();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function get_state_details() {
            var data = { 'op': 'get_state_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillstatedetails(msg);
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
        function fillstatedetails(msg) {
            var results = '<div id="tblbranchdata"  style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">State Code</th><th scope="col" style="font-weight: bold;">State Name</th><th scope="col" style="font-weight: bold;">E Code</th><th scope="col" style="font-weight: bold;">GST State Code</th><th scope="col" style="font-weight: bold;"style="font-weight: bold;"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="brandstatus" class="1"><span class="" style="color: cadetblue;"></span><span id="1">' + msg[i].statecode + '</span></td>';
                results += '<td data-title="brandstatus" class="2"><span class="glyphicon glyphicon-map-marker" style="color: cadetblue;"></span><span id="2">' + msg[i].statename + '</span></td>';
                results += '<td style="display:none" class="3">' + msg[i].sno + '</td>';
                results += '<td  class="4">' + msg[i].ecode + '</td>';
                results += '<td  class="5">' + msg[i].gststatecode + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmestates(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_getstates").html(results);
        }
        function getmestates(thisid) {
            scrollTo(0, 0);
            var statename = $(thisid).parent().parent().find('#2').html();
            var statecode = $(thisid).parent().parent().find('#1').html();
            var sno = $(thisid).parent().parent().children('.3').html();
            var ecode = $(thisid).parent().parent().children('.4').html();
            var gststatecode = $(thisid).parent().parent().children('.5').html();
            document.getElementById('txt_statecode').value = statecode;
            document.getElementById('txt_statename').value = statename;
            document.getElementById('lbl_statesno').innerHTML = sno;
            document.getElementById('txt_ecode').value = ecode;
            document.getElementById('txt_gststatecode').value = gststatecode;
            document.getElementById('btn_savestate').innerHTML = "Modify";
            $('#div_addstates').hide();
            $('#div_getstates').hide();
            $('#div_fillstates').show();
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
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="content">
        <div class="box box-info">
            <div>
                <ul class="nav nav-tabs" style="font-weight: 500 !important;">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="show_department()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Departments</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="show_employee()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Employee</a></li>
                     <li id="Li1" class=""><a data-toggle="tab" href="#" onclick="show_silo()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Silo</a></li>
                     <li id="Li2" class=""><a data-toggle="tab" href="#" onclick="show_processing()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Processing Dept</a></li>
                     <li id="Li3" class=""><a data-toggle="tab" href="#" onclick="show_shift()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Shift</a></li>
                     <li id="Li4" class=""><a data-toggle="tab" href="#" onclick="show_batch()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Batch</a></li>
                    <li id="Li5" class=""><a data-toggle="tab" href="#" onclick="show_personal()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Personal details</a></li>
                    <li id="Li6" class=""><a data-toggle="tab" href="#" onclick="show_vechile()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Vehicle Master</a></li>
                    <li id="Li7" class=""><a data-toggle="tab" href="#" onclick="show_statemaster()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;State</a></li>
                </ul>
            </div>
            <div id="div_dept">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Department
                </h3>
            </div>
            <div class="box-body">
                 <div id="showlogs" style="text-align: -webkit-right;">
                    <%--<input id="btn_addDept" type="button" name="submit" value='Add Department' onclick="adddepartments();" class="btn btn-success" />--%>
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="adddepartments()"></span> <span onclick="adddepartments()">Add Department</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_Deptdata">
                </div>
                <div id='fillform' style="display: none; padding-left:223px;">
                    <table cellpadding="1px" align="center" style="width: 60%;">
                        <tr>
                            <th colspan="2" align="center">
                            </th>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                             <label>  Code </label> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_code" class="form-control" name="vendorcode"
                                    placeholder="Enter Dept Code">
                            </td>
                        </tr>
                        <tr>
                            <td>
                              <label>   Name </label> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txt_name" class="form-control" name="vendorcode"
                                    placeholder="Enter Dept Name">
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input type="button" class="btn btn-success" name="submit" 
                                    id="btn_save" value='Save' onclick="for_save_edit_Dept()" />
                                    <input id='btn_close'
                                        type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>--%>
                    </table>
                    <div  style="padding-left: 10%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="for_save_edit_Dept()"></span><span id="btn_save" onclick="for_save_edit_Dept()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="closedept()"></span><span id='btn_close' onclick="closedept()">Close</span>
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

            <div id="div_emp">
                <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-cog"></i> Employee Details
                    </h3>
                </div>
                <div class="box-body">
                    <div id="showlogs_emp" style="text-align: -webkit-right;">
                        <%--<input id="btn_addemp" type="button" name="submit" value='Add Employee' onclick="addemployess();" class="btn btn-success" />--%>
                        <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addemployess()"></span> <span onclick="addemployess()">Add Employee</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                    </div>
                    <div id="div_empdata">
                    </div>
                    <div id='fillform_emp' style="display: none; padding-left:223px;">
                        <table cellpadding="1px" align="center" style="width: 60%;">
                            <tr>
                                <th colspan="2" align="center">
                                </th>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                   <label> Employee Name </label><span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_Empname" class="form-control" name="vendorcode"
                                        placeholder="Enter Employee Name"><label id="lbl_Empname_error_msg" class="errormessage">*
                                            Please Enter Employee Name</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                   <label>  Department Name </label> <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <select id="slct_empDepart_Name" class="form-control">
                                        <option selected disabled value="Select Depart">Select Depart Name</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                   <label>  Branch Name </label> <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <select id="slct_branch" class="form-control">
                                        <option selected disabled value="Select Branch Name">Select Branch Name</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                    <label> User Name </label> <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_UserName" class="form-control" name="vendorcode"
                                        placeholder="Enter User Name">
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                    <label> Password </label><span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_passward" class="form-control" name="vendorcode"
                                        placeholder="Enter Password">
                                </td>
                            </tr>

                            <tr hidden>
                                <td>
                                    <label id="lbl_emp">
                                    </label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2" align="center" style="height:40px;">
                                    <input type="button" class="btn btn-success" name="submit" 
                                        id="btn_save_emp" value='Save' onclick="for_save_edit_Employee()" />
                                        <input id='btn_close_emp'
                                            type="button" class="btn btn-danger" name="Close" value='Close' />
                                </td>
                            </tr>--%>
                        </table>
                        <div  style="padding-left: 10%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save_emp1" onclick="for_save_edit_Employee()"></span><span id="btn_save_emp" onclick="for_save_edit_Employee()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close_emp1' onclick="cloaseemployee()"></span><span id='btn_close_emp' onclick="cloaseemployee()">Close</span>
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

            <div id="div_silo">
                <div class="box box-info">
                    <div class="box-header with-border">
                <h3 class="box-title"> 
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>ADD SILOS
                </h3>
            </div>
                    <div class="box-body">
                <div id="showlogs_silo" style="text-align: -webkit-right;">
                    <%--<input id="btn_addsilo" type="button" name="submit" value='Add Silos' onclick="addsilos();" class="btn btn-success" />--%>
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addsilos()"></span> <span onclick="addsilos()">Add Silo</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_Silodata">
                </div>
                <div id='fillform_silo' style="display: none;padding-left:223px;">
                    <div>
                        <table cellpadding="1px" align="center" style="width: 60%;">
                            <tr>
                                <th colspan="2" align="center">
                                </th>
                            </tr>
                            <tr>
                                <td style="height:50px;">
                                  <label>Silo Name </label><span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_Siloname" class="form-control" name="vendorcode"
                                        placeholder="Enter Silo Name">
                                </td>
                            </tr>
                            <tr>
                                <td>
                               <label>Silo Capacity</label> <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" onkeypress='validate(event)' id="txt_capacity" class="form-control" name="vendorcode"
                                        placeholder="Enter Silo Capacity">
                                </td>
                            </tr>
                            <tr hidden>
                                <td>
                                    <label id="lbl_silo">
                                    </label>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td colspan="2" align="center"  style="height:50px;">
                                    <input type="button" class="btn btn-success" name="submit" 
                                        id="btn_save_silo" value='Save' onclick="for_save_edit_Silo()" />
                                        
                                        <input id='btn_close_silo'
                                            type="button" class="btn btn-danger" name="Close" value='Close' />
                                </td>
                            </tr>--%>
                        </table>
                        <div  style="padding-left: 10%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save_silo1" onclick="for_save_edit_Silo()"></span><span id="btn_save_silo" onclick="for_save_edit_Silo()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close_silo1' onclick="closesilos()"></span><span id='btn_close_silo' onclick="closesilos()">Close</span>
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
            </div>

            <div id="div_processing">
             <div class="box box-info">
                <div class="box-header with-border">
                 <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Add Processing Department Details
                </h3>
            </div>
                <div class="box-body">
                <div id="showlogs_processing" style="text-align: -webkit-right;">
                    <%--<input id="btn_processing_add" type="button" name="submit" onclick="addprocessdepartments();" value='Add Department' class="btn btn-success" />--%>
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addprocessdepartments()"></span> <span onclick="addprocessdepartments()">Add Department</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_processingdata">
                </div>
                <div id='fillform_processing' style="display: none;" align="center">
                    <div>
                        <table cellpadding="1px" align="center" style="width: 60%;">
                            <tr style="height: 10px;">
                                <th colspan="2" align="center">
                                </th>
                            </tr>
                            <tr>
                                <td style="width: 181px;height:50px;">
                                   <label>Processing Department Name</label>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_Deptname" class="form-control" name="vendorcode"
                                        placeholder="Enter Department Name">
                                </td>
                            </tr>
                            <tr hidden>
                                <td colspan="2">
                                    <label id="lbl_processing">
                                    </label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2" align="center">
                                    <input type="button" class="btn btn-success" name="submit"
                                        id="btn_save_processing" value='Save' onclick="for_save_edit_Silo_processing()" />
                                        <input id='btn_close_processing'
                                            type="button" class="btn btn-danger" name="Close" value='Close' />
                                </td>
                            </tr>--%>
                        </table>
                        <div  style="padding-left: 0%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save_processing1" onclick="for_save_edit_Silo_processing()"></span><span id="btn_save_processing" onclick="for_save_edit_Silo_processing()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close_processing1' onclick="closesilodepartments()"></span><span id='btn_close_processing' onclick="closesilodepartments()">Close</span>
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
            </div>

            <div id="div_shift">
                 <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>ADD Shift Details
                        </h3>
                    </div>
                <div class="box-body">
                <div id="showlogs_shift" style="text-align: -webkit-right;">
                    <%--<input id="btn_shift" type="button" name="submit" onclick="addsift();" value='Add Shift' class="btn btn-success" />--%>
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addsift()"></span> <span onclick="addsift()">Add Shift</span>
                          </div>
                          </div>
                            </td>
                         </tr>
                      </table>
                </div>
                <div id="div_shiftdata">
                </div>
                <div id='fillform_shift' style="display: none;" align="center">
                    <div >
                        <table cellpadding="1px" align="center" style="width: 60%;">
                            <tr>
                                <th colspan="2" align="center">
                                </th>
                            </tr>
                            <tr>
                                <td style="height:50px;">
                                <label>     Shift Type </label> <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_Shifttype" class="form-control" name="vendorcode"
                                        placeholder="Enter Shift Name">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                 <label>    Shift Timings </label> <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_shifttimings" class="form-control" name="vendorcode"
                                        placeholder="Enter  Shift Timings">
                                </td>
                            </tr>
                            <tr hidden>
                                <td>
                                    <label id="lbl_sno_shift">
                                    </label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2" align="center" style="height:50px;">
                                    <input type="button" class="btn btn-success" name="submit" 
                                        id="btn_save_shift" value='Save' onclick="for_save_edit_shift()" /> <input id='btn_close_shift'
                                            type="button" class="btn btn-danger" name="Close" value='Close' />
                                </td>
                            </tr>--%>
                        </table>
                        <div  style="padding-left: 0%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save_shift1" onclick="for_save_edit_shift()"></span><span id="btn_save_shift" onclick="for_save_edit_shift()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close_shift1' onclick="closeshift()"></span><span id='btn_close_shift' onclick="closeshift()">Close</span>
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
            </div>

            <div id="div_batch">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Batch Details
                        </h3>
                    </div>
            <div class="box-body">
                <div id="showlogs_batch" style="text-align: -webkit-right;">
                    <%--<input id="btn_batch" type="button" name="submit" onclick="addbatch();" value='Add Batch' class="btn btn-success" />--%>
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addbatch()"></span> <span onclick="addbatch()">Add Batch</span>
                          </div>
                          </div>
                            </td>
                         </tr>
                      </table>
                </div>
                <div id="div_batchdata">
                </div>
                <div id='fillform_batch' style="display: none;" align="center">
                    <table>
                        <tr>
                            <th>
                            </th>
                        </tr>
                        <tr>
                          
                            <td  style="height: 40px;">
                              <label>
                                    Department<span style="color: red;">*</span></label>
                            </td>
                            <td>
                              
                                <select id="slct_Department" class="form-control">
                                   
                                </select>
                            </td>
                     
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    Batch Code
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txtbatchcode" class="form-control" name="vendorcode"
                                    placeholder="Enter Batch Code">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label>
                                    Batch Name
                                </label>
                                <span style="color: red;">*</span>
                            </td>
                            <td>
                                <input type="text" maxlength="45" id="txtbatch" class="form-control" name="vendorcode"
                                    placeholder="Enter Batch Name">
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno_batch">
                                </label>
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input type="button"  name="submit" class="btn btn-success"
                                    id="btn_save_batch" value='Save' onclick="for_save_edit_batch()" />  <input id='btn_close_batch'
                                        type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>--%>
                    </table>
                    <div  style="padding-left: 0%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save_batch1" onclick="for_save_edit_batch()"></span><span id="btn_save_batch" onclick="for_save_edit_batch()">Save</span>
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
            </div>
        </div>
            </div>

            <div id="div_person">
              <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Personal Details
                    </h3>
                </div>
            <div class="box-body">
              <div id="showlogs_person" style="text-align: -webkit-right;">
                    <%--<input id="btn_person" type="button" name="submit" value='Personal Details' onclick="addperonaldetails();" class="btn btn-success" />--%>
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
                       <%-- <tr>
                            <td colspan="2" align="center" style="height:40px;">
                                <input type="button" class="btn btn-success" name="submit" 
                                    id="btn_save_person" value='Save' onclick="save_personal_details()" />
                                    <input id='btn_close_person'
                                        type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>--%>
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
            </div>

            <div id="div_vechile">
                 <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>ADD Vehicle Details
                        </h3>
                    </div>
            <div class="box-body">
                <div id='vehmaster_fillform'>
                    <div id="showlogs_vechile" style="text-align: -webkit-right;">
                        <%--<input id="btn_vechileDept" type="button" name="submit" value='Add Vehicle' onclick="addvehicledetails();" class="btn btn-success" />--%>
                         <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addvehicledetails()"></span> <span onclick="addvehicledetails()">Add Vehicle</span>
                          </div>
                          </div>
                            </td>
                         </tr>
                      </table>
                    </div>
                    <div id="div_vechiledata">
                    </div>
                    <div id='fillform_vechile' style="display: none; padding-left:223px;">
                        <table cellpadding="1px" align="center" style="width: 60%;">
                            <tr>
                                <th colspan="2" align="center">
                                </th>
                            </tr>
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Vehicle No
                                    </label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_vehicleno" class="form-control" name="vendorcode"
                                        placeholder="Enter Vehicle No">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        No Of Cells
                                    </label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_nocell" class="form-control" name="vendorcode"
                                        placeholder="Enter No Of Cells" onkeypress="return isNumber(event);" onchange="vehclecell();">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trfcell" style="display:none;">
                                <td style="height: 50px;">
                                <label>
                                        Front Cell</label>
                                </td>
                                <td style="height: 40px;">
                                    <input id="frentcell" name="Other Browser" onkeypress="return isFloat(event);" class="form-control" type="text" placeholder="Enter F Cell capacity"  />
                                </td>
                            </tr>
                            <tr id="trmcell" style="display:none;">
                                <td style="height: 50px;">
                                <label>
                                        Middle Cell</label>
                                </td>
                                  
                                <td style="height: 40px;">
                                    <input id="middlecell" name="Other Browser" onkeypress="return isFloat(event);" class="form-control" type="text" placeholder="Enter M Cell capacity" />
                                </td>
                            </tr>
                            <tr id="trbcell" style="display:none;">
                                <td style="height: 50px;">
                                <label>
                                        Back Cell</label>
                                </td>
                                  
                                <td style="height: 40px;">
                                    <input id="backcell" name="Other Browser" onkeypress="return isFloat(event);" class="form-control" type="text" placeholder="Enter B Cell capacity"  />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Capacity</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" onkeypress="return isNumber(event);" id="txt_veh_capacity" class="form-control" name="vendorcode"
                                        placeholder="Enter capacity">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Per Km Rate</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txtKRate" onkeypress="return isNumber(event);" class="form-control" name="vendorcode"
                                        placeholder="Enter Per Km Rate">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Make</label><span style="color: red;"></span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txtMake" class="form-control" name="vendorcode"
                                        placeholder="Enter Make">
                                </td>
                            </tr>
                            <tr hidden>
                                <td>
                                    <label id="lbl_sno_vechile">
                                    </label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2" align="center" style="height: 50px;">
                                    <input type="button" class="btn btn-success" name="submit" id="btn_save_vechile" value='Save'
                                        onclick="save_vehicle_master_click()" />
                                    <input id='btn_close_vechile' type="button" class="btn btn-danger" name="Close" value='Close' />
                                </td>
                            </tr>--%>
                        </table>
                        <div  style="padding-left: 10%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save_vechile1" onclick="save_vehicle_master_click()"></span><span id="btn_save_vechile" onclick="save_vehicle_master_click()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close_vechile1' onclick="closevehicledetails()"></span><span id='btn_close_vechile' onclick="closevehicledetails()">Close</span>
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
         </div>

            <div id="div_states" style="display: none;">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>State Master
                </h3>
            </div>
            <div class="box-body">
                    <div id="div_addstates" style="text-align: -webkit-right;">
                    <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addstatemaster()"></span> <span onclick="addstatemaster()">Add State</span>
                            </div>
                            </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_getstates">
                </div>
                <div id='div_fillstates' style="display: none; padding-left:223px;">
                    <table cellpadding="1px" align="center" style="width: 60%;">
                        <tr>
                            <th colspan="2" align="center">
                            </th>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label> State Code </label> <span style="color: red;">*</span>
                                <input type="text" id="txt_statecode" class="form-control" 
                                    placeholder="Enter State Code">
                            </td>
                            <td style="padding-left:2%">
                            </td>
                            <td>
                                <label> State Name </label> <span style="color: red;">*</span>
                                <input type="text"  id="txt_statename" class="form-control"
                                    placeholder="Enter State Name">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 40px;">
                                <label> E Code </label> <span style="color: red;">*</span>
                                <input type="text" id="txt_ecode" class="form-control" 
                                    placeholder="Enter E Code">
                            </td>
                            <td style="padding-left:2%">
                            </td>
                            <td>
                                <label>GST State Code </label> <span style="color: red;">*</span>
                                <input type="text"  id="txt_gststatecode" class="form-control"
                                    placeholder="Enter GST State Code">
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_statesno">
                                </label>
                            </td>
                        </tr>
                    </table>
                    <div  style="padding-left: 21%;padding-top: 2%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_savestate1" onclick="for_save_edit_states()"></span><span id="btn_savestate" onclick="for_save_edit_states()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='Span3' onclick="closestate()"></span><span id='Span4' onclick="closestate()">Close</span>
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
    </div>
</asp:Content>

