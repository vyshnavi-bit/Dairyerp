<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="curdcoldroom.aspx.cs" Inherits="curdcoldroom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btn_addcoldcurd').click(function () {
                $('#curdcold_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_curdcold').hide();
                clearvalues();
                subcurdcoldroomdetails();
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
            get_coldroom_details();
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
        function closedetails() {
            $('#curdcold_fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_curdcold').show();
        }
        function addcoldroomdetails() {
            $('#curdcold_fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_curdcold').hide();
            clearvalues();
            subcurdcoldroomdetails();
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
       
        function clearvalues() {
            document.getElementById('txt_type').value = "";
            document.getElementById('txtcoldroom1').value = "";
            document.getElementById('txtcoldroom2').value = "";
            document.getElementById('txtincubationroom').value = "";
            document.getElementById('txtremarks').value = "";
            subcurdcoldroomdetails();
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
        var BranchIDS = '<%=Session["Branch_ID"] %>';
        function subcurdcoldroomdetails() {
            if (BranchIDS == "1" || BranchIDS == "22") {
                $('#tr_type').show();
                var time = 0;
                var names = 6;
                var results = '<div    style="overflow:auto;"><table id="tbl_cold_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Time</th><th scope="col" style="font-weight: 700;">ColdRoom1</th><th scope="col" style="font-weight: 700;">ColdRoom2</th><th scope="col" style="font-weight: 700;">IncubationRoom / IBT</th><th scope="col" style="font-weight: 700;">Remarks</th></tr></thead></tbody>';
                var j = 1;
                for (var i = 0; i < 24; i++) {
                    time = i + 1;
                    results += '<td data-title="Sno" class="0">' + i + '</td>';
                    results += '<td data-title="TIME" style="text-align:center;width:30px;" class="1"><input class="form-control" id="txttime" name="time" value="' + time + '" style="font-size:12px;padding: 0px 5px;height:30px;"></td>';
                    results += '<td data-title="COLDROOM1" style="text-align:center;" class="2"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="Coldroom1" name="COLDROOM1" id="txtcoldroom1" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="COLDROOM2" style="text-align:center;" class="3"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="Coldroom2" name="COLDROOM2" id="txtcoldroom2" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="INCUBATIONROOM" style="text-align:center;" class="4"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="Incubation Room" name="INCUBATIONROOM" id="txtincubationroom" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                    results += '<td data-title="REMARKS" style="text-align:center;" class="5"><input class="form-control" name="REMARKS" id="txtremarks"  value="" placeholder="Remarks" style="width:100%; "font-size:12px;padding: 0px 5px;height:30px;" ></td></tr>';
                }
                results += '</table></div>';
                $("#div_curdpacket").html(results);
            }
            else {
                $('#tr_type').hide();
                var time = 0;
                var names = 6;
                var results = '<div style="overflow:auto;"><table id="tbl_cold_details_wyra" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Time</th><th scope="col" style="font-weight: 700;">ColdRoom1</th><th scope="col" style="font-weight: 700;">ColdRoom2</th><th scope="col" style="font-weight: 700;">ColdRoom3</th><th scope="col" style="font-weight: 700;">BlastRoom</th><th scope="col" style="font-weight: 700;">ContainerNo1</th><th scope="col" style="font-weight: 700;">ContainerNo2</th><th scope="col" style="font-weight: 700;">IBT</th><th scope="col" style="font-weight: 700;">IncubationRoom</th><th scope="col" style="font-weight: 700;">Remarks</th></tr></thead></tbody>';
                var j = 1;
                for (var i = 0; i < 24; i++) {
                    time = i + 1;
                    results += '<td data-title="Sno" class="0">' + i + '</td>';
                    results += '<td class="1"><input class="form-control" id="txttime" name="time" value="' + time + '" ></td>';
                    results += '<td class="2"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="Coldroom1" name="coldroom1" id="txt_coldroom1" value="" /></td>';
                    results += '<td class="3"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="Coldroom2" name="coldroom2" id="txt_coldroom2" value="" /></td>';
                    results += '<td class="4"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="Coldroom3" name="coldroom3" id="txt_coldroom3" value="" /></td>';
                    results += '<td class="5"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="BlastRoom" name="blastroom" id="txt_blastroom" value="" /></td>';
                    results += '<td class="6"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="ContainerNo1" name="containerno1" id="txt_containerno1" value="" /></td>';
                    results += '<td class="7"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="ContainerNo2" name="containerno2" id="txt_containerno2" value="" /></td>';
                    results += '<td class="8"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="IBT" name="ibt" id="txt_ibt" value="" /></td>';
                    results += '<td class="9"><input class="form-control" type="text" onkeypress="validate(event);" placeholder="IncubationRoom" name="incubationroom" id="txt_incubationroom" value="" /></td>';
                    results += '<td class="10"><input class="form-control" name="remarks" id="txtremarks"  value="" placeholder="Remarks" ></td></tr>';
                }
                results += '</table></div>';
                $("#div_curdpacket").html(results);
            }
        }
        function save_curd_coldroom() {
            var date = document.getElementById('txt_date').value;
            var type = document.getElementById('txt_type').value;
            var btnval = document.getElementById('btnvalue').innerHTML;
            var sno = document.getElementById('lbl_sno').value;
            var curdcoldroomDetailsarray = [];
            if (BranchIDS == "1" || BranchIDS == "22") {
                $('#tbl_cold_details> tbody > tr').each(function () {
                    var time = $(this).find('[name="time"]').val();
                    var coldroom1 = $(this).find('[name="COLDROOM1"]').val();
                    var coldroom2 = $(this).find('[name="COLDROOM2"]').val();
                    var incubationroom = $(this).find('[name="INCUBATIONROOM"]').val();
                    var remarks = $(this).find('[name="REMARKS"]').val();
                    if (coldroom1 == "") {
                    }
                    else {
                        curdcoldroomDetailsarray.push({ 'time': time, 'coldroom1': coldroom1, 'coldroom2': coldroom2,
                            'incubationroom': incubationroom, 'remarks': remarks
                        });
                    }
                });
            }
            else {
                $('#tbl_cold_details_wyra> tbody > tr').each(function () {
                    var time = $(this).find('[name="time"]').val();
                    var coldroom1 = $(this).find('[name="coldroom1"]').val();
                    var coldroom2 = $(this).find('[name="coldroom2"]').val();
                    var coldroom3 = $(this).find('[name="coldroom3"]').val();
                    var blastroom = $(this).find('[name="blastroom"]').val();
                    var containerno1 = $(this).find('[name="containerno1"]').val();
                    var containerno2 = $(this).find('[name="containerno2"]').val();
                    var ibt = $(this).find('[name="ibt"]').val();
                    var incubationroom = $(this).find('[name="incubationroom"]').val();
                    var remarks = $(this).find('[name="remarks"]').val();
                    if (coldroom1 == "") {
                    }
                    else {
                        curdcoldroomDetailsarray.push({ 'time': time, 'coldroom1': coldroom1, 'coldroom2': coldroom2,'coldroom3':coldroom3,'blastroom':blastroom,'containerno1':containerno1,
                            'containerno2': containerno2, 'ibt': ibt, 'incubationroom': incubationroom, 'remarks': remarks
                        });
                    }
                });
            }
            if (curdcoldroomDetailsarray.length == "0") {
                alert("Please enter coldroom,incubationroom details");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_curd_coldroom', 'date': date, 'type': type, 'sno': sno, 'btnvalue': btnval, 'curdcoldroomDetailsarray': curdcoldroomDetailsarray };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            $('#div_curdcold').show();
                            $('#curdcold_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                            get_coldroom_details();
                            clearvalues();

                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                CallHandlerUsingJson(data, s, e);
            }
        }
        function get_coldroom_details() {
            var data = { 'op': 'get_curdcoldroom_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcurddetails(msg);
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
        function fillcurddetails(msg) {
            var results = '<div  style="overflow:auto;"><table  class="table table-bordered table-hover dataTable no-footer">';
            if (BranchIDS == "1" || BranchIDS == "22") {
                results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Date</th><th scope="col">Time</th><th scope="col" style="font-weight: bold;">ColdRoom1</th><th scope="col" style="font-weight: bold;">ColdRoom2</th><th scope="col" style="font-weight: bold;">IncubationRoom / IBT</th><th scope="col" style="font-weight: bold;">Remarks</th></tr></thead></tbody>';
            }
            else {
                results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: 700;">Date</th><th scope="col" style="font-weight: 700;">Time</th><th scope="col" style="font-weight: 700;">ColdRoom1</th><th scope="col" style="font-weight: 700;">ColdRoom2</th><th scope="col" style="font-weight: 700;">ColdRoom3</th><th scope="col" style="font-weight: 700;">BlastRoom</th><th scope="col" style="font-weight: 700;">ContainerNo1</th><th scope="col" style="font-weight: 700;">ContainerNo2</th><th scope="col" style="font-weight: 700;">IBT</th><th scope="col" style="font-weight: 700;">IncubationRoom</th><th scope="col" style="font-weight: 700;">Remarks</th></tr></thead></tbody>';
            }
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<th class="1" >' + msg[i].doe + '</th>';
                results += '<th class="1" >' + msg[i].time + '</th>';
                if (BranchIDS == "1" || BranchIDS == "22") {
                    results += '<td class="2">' + msg[i].coldroom1 + '</td>';
                    results += '<td class="3">' + msg[i].coldroom2 + '</td>';
                    results += '<td class="2" style="display:none">' + msg[i].coldroom3 + '</td>';
                    results += '<td class="3" style="display:none">' + msg[i].coldroom4 + '</td>';
                    results += '<td class="2" style="display:none">' + msg[i].coldroom5 + '</td>';
                    results += '<td class="3" style="display:none">' + msg[i].coldroom6 + '</td>';
                    results += '<td class="3" style="display:none">' + msg[i].coldroom7 + '</td>';
                    results += '<td class="4">' + msg[i].incubationroom + '</td>';
                    results += '<td class="5">' + msg[i].remarks + '</td>';
                    results += '<td style="display:none" class="6">' + msg[i].csno + '</td>';
                }
                else {
                    results += '<td class="2">' + msg[i].coldroom1 + '</td>';
                    results += '<td class="">' + msg[i].coldroom2 + '</td>';
                    results += '<td class="2">' + msg[i].coldroom3 + '</td>';
                    results += '<td class="3">' + msg[i].coldroom4 + '</td>';
                    results += '<td class="2">' + msg[i].coldroom5 + '</td>';
                    results += '<td class="3">' + msg[i].coldroom6 + '</td>';
                    results += '<td class="3">' + msg[i].coldroom7 + '</td>';
                    results += '<td class="4">' + msg[i].incubationroom + '</td>';
                    results += '<td class="5">' + msg[i].remarks + '</td>';
                    results += '<td style="display:none" class="6">' + msg[i].csno + '</td>';
                }
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_curdcold").html(results);

        }
        function getme(thisid) {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_curdcold').hide();
            var date = $(thisid).parent().parent().children('.1').html();
            // var status = $(thisid).parent().parent().children('.7').html();
            var type = $(thisid).parent().parent().children('.2').html();
            document.getElementById('txtdate').value = date;
            document.getElementById('txttype').value = type;
            document.getElementById('btnvalue').innerHTML = "Modify";
            var table = document.getElementById("tabledetails");
            var results = '<div  style="overflow:auto;"><table ID="tabledetails" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col"></th><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;" >Date</th><th scope="col" style="font-weight: 700;">Type</th><th scope="col" style="font-weight: 700;">Time</th><th scope="col" style="font-weight: 700;">ColdRoom1</th><th scope="col" style="font-weight: 700;">ColdRoom2</th><th scope="col">IncubationRoom / IBT</th><th scope="col" style="font-weight: 700;">Remarks</th></tr></thead></tbody>';
            var k = 1;
            for (var i = 0; i < subcoldcurddetails.length; i++) {
                if (date == subcoldcurddetails[i].date) {
                    results += '<tr><td data-title="Sno" class="1">' + k + '</td>';
                    results += '<th data-title="From"><input id="txttime"  class="form-control"  name="time" value="' + subcoldcurddetails[i].time + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                    results += '<td data-title="From"><input class="form-control" id="txttype" name="type"  value="' + subcoldcurddetails[i].type + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                    results += '<td data-title="From"><input class="form-control"  id="txtcoldroom1" type="text "name="coldroom1" value="' + subcoldcurddetails[i].coldroom1 + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                    results += '<td data-title="From"><input class="form-control"  id="txtcoldroom2"  name="coldroom2" value="' + subcoldcurddetails[i].coldroom + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                    results += '<td data-title="From"><input class="form-control" id="txtremarks"  name="remarks" value="' + subcoldcurddetails[i].remarks + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                    results += '<td data-title="From"><input class="form-control" type="hidden"  id="lbl_sno"  name="nationalty" value="' + subcoldcurddetails[i].sno + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td></tr>';
                }
            }
            results += '</table></div>';
            $("#div_Griddata").html(results);
        }
        function clearvalues() {
            document.getElementById('txt_type').selectedIndex = 0;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Curd Cold Room <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Cold Room</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Cold Room 
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                         <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addcoldroomdetails()"></span> <span onclick="addcoldroomdetails()">Add Details</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
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
                            <tr id="tr_type">
                                <td style="height: 40px;">
                                    <label>
                                        Type</label>
                                </td>
                                <td>
                                    <select id="txt_type" class="form-control">
                                        <option>Curd</option>
                                        <option>Process</option>
                                    </select>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <%-- <h3 class="box-title" id="lbldetals" style="display:none;">--%>
                            <h3 class="box-title">
                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Cold Room Details</h3>
                        </div>
                        <div id="div_curdpacket">
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
                    <div  style="padding-left: 35%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btnvalue1" onclick="save_curd_coldroom()"></span><span id="btnvalue" onclick="save_curd_coldroom()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_close1' onclick="closedetails()"></span><span id='btn_close' onclick="closedetails()">Close</span>
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
