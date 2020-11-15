<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="coldroomdetails.aspx.cs" Inherits="coldroomdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/utility.js" type="text/javascript"></script>
    <style type="text/css">
        input[type=number]::-webkit-inner-spin-button, input[type=number]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
<script type="text/javascript">
    $(function () {
        $('#div_coldroommaster').css('display', 'block');
        $('#div_curdcoldroom').css('display', 'none');
        forclearall_coldroom();
        get_coldroom_master_details();
        $('#btn_addRoommaster').click(function () {
            $('#coldroom_fillform').css('display', 'block');
            $('#coldmaster_showlogs').css('display', 'none');
            $('#div_coldroomdata').hide();
        });
        $('#btn_coldroomclose').click(function () {
            $('#coldroom_fillform').css('display', 'none');
            $('#coldmaster_showlogs').css('display', 'block');
            $('#div_coldroomdata').show();
            forclearall_coldroom();
            get_coldroom_master_details();
        });
        $('#btn_addcoldcurd').click(function () {
            $('#curdcold_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_curdcold').hide();
            //fillcoldroomdetails();
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
        });
        $('#btn_close').click(function () {
            $('#curdcold_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_curdcold').show();
        });
    });
    function showcoldroommaster() {
        $('#div_coldroommaster').css('display', 'block');
        $('#div_curdcoldroom').css('display', 'none');
        forclearall_coldroom();
        get_coldroom_master_details();
    }
    function showcurdcoldroom() {
        $('#div_coldroommaster').css('display', 'none');
        $('#div_curdcoldroom').css('display', 'block');
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
    function save_coldroom_master_details() {
        var type = document.getElementById('slct_coldroomtype').value;
        var coldroomname = document.getElementById('txt_coldroomname').value;
        var sno = document.getElementById('lbl_coldroomsno').value;
        var btnval = document.getElementById('btn_coldroomsave').value;
        if (type == "") {
            alert("Select Section Type");
            return false;
        }
        if (coldroomname == "") {
            alert("Enter Cold Room Name");
            return false;
        }
        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var data = { 'op': 'save_coldroom_master_details', 'type': type, 'coldroomname': coldroomname, 'sno': sno, 'btnval': btnval };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        get_coldroom_master_details();
                        forclearall_coldroom();
                        $('#btn_addRoommaster').show();
                        $('#div_coldroomdata').show();
                        $('#coldroom_fillform').css('display', 'none');
                        $('#coldmaster_showlogs').css('display', 'block');
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
    }
    function get_coldroom_master_details() {
        var data = { 'op': 'get_coldroom_master_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    coldroomdetails(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function coldroomdetails(msg) {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th>Department Name</th><th scope="col">Cold Room Type</th><th scope="col">Cold Room Name</th></tr></thead></tbody>';
        for (var i = 0; i < msg.length; i++) {
            results += '<tr><td><input id="btn_poplate" type="button"  onclick="getmecoldroom(this)" name="submit" class="btn btn-success" value="Choose" /></td>';
            results += '<td data-title="Code" class="1">' + msg[i].coldroomtype + '</td>';
            results += '<td data-title="Code" class="2">' + msg[i].coldroomname + '</td>';
            results += '<td style="display:none" class="3">' + msg[i].sno + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_coldroomdata").html(results);
    }
    function forclearall_coldroom() {
        document.getElementById('slct_coldroomtype').selectedIndex = 0;
        document.getElementById('txt_coldroomname').value = "";
        document.getElementById('btn_coldroomsave').value = "Save";
    }
    function getmecoldroom(thisid) {
        var coldroomtype = $(thisid).parent().parent().children('.1').html();
        var coldroomname = $(thisid).parent().parent().children('.2').html();
        var sno = $(thisid).parent().parent().children('.3').html();
        document.getElementById('slct_coldroomtype').value = coldroomtype;
        document.getElementById('txt_coldroomname').value = coldroomname;
        document.getElementById('lbl_coldroomsno').value = sno;
        document.getElementById('btn_coldroomsave').value = "Modify";
        $("#div_coldroomdata").hide();
        $("#coldroom_fillform").show();
        $('#coldmasrer_showlogs').hide();
        $('#btn_addRoommaster').hide();
    }
//    var coldroom = [];
//    function fillcoldroomdetails() {
//        var section = document.getElementById('txt_type').value;
//        var data = { 'op': 'get_coldroom_details', 'section': section };
//        var s = function (msg) {
//            if (msg) {
//                if (msg.length > 0) {
//                    //coldroom = msg[0].coldroomname;
//                    //for (var i = 0; i < msg.length; i++) {
//                    var results = '<div    style="overflow:auto;"><table id="tbl_coldroom_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
//                    results += '<thead><tr>';
//                    results += '<th scope="col">Sno</th>';
//                    results += '<th scope="col">Time</th>';
//                    for (var i = 0; i < msg.length; i++) {
//                        results += '<th scope="col" id="txt_coldroomname" value="' + msg[i].sno + '" class="coldroom" name="COLDROOMNAME">' + msg[i].coldroomname + '</th>';
//                    }
//                    results += '<th scope="col">Remarks</th>';

//                    results += '</tr></thead></tbody>';
//                    for (var i = 0; i < 24; i++) {
//                        var time = i + 1;
//                        results += '<tr><th style="text-align:center;" scope="row" id="txt_rowsno">' + i + '</th>';
//                        results += '<th style="text-align:center;" scope="row" class="TIME" name="TIME" id="txt_time">' + time + '</th>';
//                        for (var k = 0; k < msg.length; k++) {
//                            results += '<td><input type="text" id="txt_coldroomtemp"  class="COLDROOMTEMP"  name="COLDROOM" placeholder="Enter Temprature" style="width:100%; "font-size:12px;padding: 0px 5px;height:30px;"></td>';
//                            results += '<td style="display:none"><input type="text" class="hdncoldroomid" id="hdn_coldroomid" value="' + msg[k].sno + '" ></td>';
//                        }
//                        results += '<td data-title="REMARKS" style="text-align:center;" class="5"><input class="REMARKS" name="REMARKS" id="txt_remarks" placeholder="Remarks" style="width:100%; "font-size:12px;padding: 0px 5px;height:30px;" ></td>';
//                    }
//                    results += '<tr>';
//                    results += '</table></div>';

//                    $("#div_coldroomdetail").html(results);
//                }
//                else {
//                }
//            }
//        };
//        var e = function (x, h, e) {
//        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//        callHandler(data, s, e);
//        }

    var coldroom = [];
    function fillcoldroomdetails() {
        var section = document.getElementById('txt_type').value;
        var data = { 'op': 'get_coldroom_details', 'section': section };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    var results = '<div    style="overflow:auto;"><table id="tbl_coldroom_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                    results += '<thead><tr>';
                    results += '<th scope="col">Sno</th>';
                    results += '<th scope="col">Cold Room Name</th>';
                    for (var i = 1; i < 25; i++) {
                        results += '<th scope="col" id="txt_time" value="' + i + '" class="time" name="TIME">' + i + '</th>';
                    }
                    results += '<th scope="col">Remarks</th>';
                    results += '</tr></thead></tbody>';

                    for (var i = 0; i < msg.length; i++) {
                        results += '<tr><th style="text-align:center;" scope="row" id="txt_rowsno">' + (i + 1) + '</th>';
                        results += '<th><span style="text-align:center;" scope="row" class="coldroomname" name="COLDROOMNAME" id="txt_coldroomname">' + msg[i].coldroomname + '</span> </td>';
                        results += '<td style="display:none"><input type="text" class="hdncoldroomid" id="hdn_coldroomid" value="' + msg[i].sno + '" ></td>';
                        for (var k = 1; k < 25; k++) {
                            results += '<td><input  id="txt_temprature"  class="temprature" type="number"  name="TEMPATURE" placeholder="Enter Temprature" style="width:100%; "font-size:12px;padding: 0px 5px;height:30px;"></td>';
                        }
                        results += '<td data-title="REMARKS" style="text-align:center;"><input class="remarks" name="REMARKS" id="txt_remarks" placeholder="Remarks" style="width:100%; "font-size:12px;padding: 0px 5px;height:30px;" ></td>';
                    }
                    results += '<tr>';
                    results += '</table></div>';

                    $("#div_coldroomdetail").html(results);
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

    function save_curd_coldroom() {
        var date = document.getElementById('txt_date').value;
        var type = document.getElementById('txt_type').value;
        var btnval = document.getElementById('btnvalue').value;
        var sno = document.getElementById('lbl_sno').value;
        if (date == "") {
            alert("Select Date");
            return false;
        }
        if (type == "" || type == "Select Type") {
            alert("Select Type");
            return false;
        }
        var rows = $("#tbl_coldroom_details tr:gt(0)");
        var curdcoldroomDetailsarray = [];
        $(rows).each(function (i, obj) {
            curdcoldroomDetailsarray.push({ coldroomhiddenid: $(this).find('.hdncoldroomid').val(),
                temprature: $(this).find('.temprature').val(),
                remarks: $(this).find('.remarks').val(),
            });
        });
        if (curdcoldroomDetailsarray.length == 0) {
            alert("Please enter coldroom  details");
            return false;
        }
        var data = { 'op': 'save_curd_coldroom', 'date': date, 'type': type, 'btnval': btnval, 'sno': sno, 'curdcoldroomDetailsarray': curdcoldroomDetailsarray };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    alert(msg);
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

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            <i aria-hidden="true"></i>Cold Room Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Cold Room Details</a></li>
        </ol>
    </section>
    <section class="content">
     <div class="box box-info">
        <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showcoldroommaster()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Cold Room Master</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showcurdcoldroom()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Curd Cold Room</a></li>
                </ul>
            </div>
        <div id="div_coldroommaster">
                <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Department
               
                    </h3>
                </div>
            <div class="box-body">
                    <div id="coldmaster_showlogs" align="center">
                    <input id="btn_addRoommaster" type="button" name="submit" value='Add Cold Room' class="btn btn-success" />
                </div>
                <div id="div_coldroomdata">
                </div>
                <div id='coldroom_fillform' style="display: none; padding-left:223px;">
                    <table cellpadding="1px" align="center" style="width: 60%;">
                        <tr>
                            <th colspan="2" align="center">
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <label>Type </label> <span style="color: red;">*</span>
                            </td>
                            <td>
                                <select  id="slct_coldroomtype" class="form-control" >
                                <option selected value disabled >Select Type</option>
                                <option value="Curd Section">Curd Section</option>
                                <option value="Processing Section">Processing Section</option></select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Cold Room Name </label> <span style="color: red;">*</span>
                            </td>
                            <td style="height: 40px;">
                                <input type="text"  id="txt_coldroomname" class="form-control" name="vendorcode"
                                    placeholder="Enter Cold Room Name" />
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_coldroomsno">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input type="button" class="btn btn-success" name="submit" 
                                    id="btn_coldroomsave" value='Save' onclick="save_coldroom_master_details()" />
                                    <input id='btn_coldroomclose'
                                        type="button" class="btn btn-danger" name="Close" value='Close' />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            </div>
            </div>
        <div id="div_curdcoldroom">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Curd Cold Room Type
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addcoldcurd" type="button" name="submit" value='Add Cold Room Temp'
                        class="btn btn-success" />
                </div>
                <div id="div_curdcold">
                </div>
                <div id='curdcold_fillform' style="display: none;">
                    <div style="padding-left: 300px;">
                        <table align="center">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px;">
                                    <label>
                                        Date</label>
                                </td>
                                <td>
                                    <input id="txt_date" class="form-control" type="date" name="vendorcode" placeholder="Enter Date" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px;">
                                    <label>
                                        Type</label>
                                </td>
                                <td>
                                    <select id="txt_type"class="form-control" onchange="fillcoldroomdetails();">
                                        <option selected value disabled>Select Type</option>
                                        <option value="Curd Section">Curd</option>
                                        <option value="Processing Section">Process</option>
                                    </select>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <%-- <h3 class="box-title" id="lbldetals" style="display:none;">--%>
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Curd Cold Room Details</h3>
                        </div>
                        <div id="div_coldroomdetail">
                        </div>
                    </div>
                    <table>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                    </table>
                    <div style="text-align: center;">
                        <input id='btnvalue' type="button" class="btn btn-success" name="submit" value='Save'
                            onclick="save_curd_coldroom()" />
                        <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'/>
                    </div>
                </div>
            </div>
        </div>
        </div>
     </div>
    </section>
</asp:Content>

