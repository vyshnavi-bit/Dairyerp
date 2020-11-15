<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="Swabanalysis.aspx.cs" Inherits="Swabanalysis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(function () {
        $('#div_microbiology').css('display', 'block');
        $('#div_swabanalysis').css('display', 'none');
        get_microbiology_details();
        $('#btn_addmicro').click(function () {
            $('#fillform_micro').css('display', 'block');
            $('#showlogs_micro').css('display', 'none');
            $('#div_editmicrodata').hide();
            Getmicrodetails();
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
            $('#txt_mdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        });
        $('#btn_mclose').click(function () {
            $('#fillform_micro').css('display', 'none');
            $('#showlogs_micro').css('display', 'block');
            $('#div_editmicrodata').show();
            forclearallmicro();
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
            $('#txt_mdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            get_microbiology_details();
        });
        $('#btn_addswab').click(function () {
            $('#fillform').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_editswabdata').hide();
            Getswabdetails();
            get_swabanalysis_details();
        });
        $('#btn_close').click(function () {
            $('#fillform').css('display', 'none');
            $('#showlogs').css('display', 'block');
            $('#div_editswabdata').show();
            forclearall();
        });
    });
    function addswabdetails() {
        $('#fillform').css('display', 'block');
        $('#showlogs').css('display', 'none');
        $('#div_editswabdata').hide();
        Getswabdetails();
        get_swabanalysis_details();
    }
    function closemicrobilogy() {
        $('#fillform_micro').css('display', 'none');
        $('#showlogs_micro').css('display', 'block');
        $('#div_editmicrodata').show();
        forclearallmicro();
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
        $('#txt_mdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
        get_microbiology_details();
    }
    function addmicrobiologydetails() {
        $('#fillform_micro').css('display', 'block');
        $('#showlogs_micro').css('display', 'none');
        $('#div_editmicrodata').hide();
        Getmicrodetails();
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
        $('#txt_mdate').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
    }
    function showmicrobiology() {
        $('#div_microbiology').css('display', 'block');
        $('#div_swabanalysis').css('display', 'none');
    }
    function showswabanlysis() {
        $('#div_microbiology').css('display', 'none');
        $('#div_swabanalysis').css('display', 'block');
        get_swabanalysis_details();
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
    function Getmicrodetails() {
        var results = '<div style="overflow:auto;"><table id="tbl_micro_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Source</th><th scope="col" style="font-weight: 700;">Coliforms</th><th scope="col" style="font-weight: 700;">E.Coli/G</th><th scope="col" style="font-weight: 700;">Yeast AND Mould</th><th scope="col" style="font-weight: 700;">Tbc/G-10</th><th scope="col" style="font-weight: 700;">Tbc/G-10(2)</th><th scope="col" style="font-weight: 700;">Tbc/G-10(3)</th><th scope="col" style="font-weight: 700;">Tbc/G-10(4)</th><th scope="col" style="font-weight: 700;">Remarks</th></tr></thead></tbody>';
        for (var i = 1; i < 2; i++) {
            results += '<td data-title="Sno" id="txtsno" class="1">' + i + '</td>';
            results += '<td data-title="Source" style="text-align:center;" ><input  type="text" placeholder="Source" class="form-control" name="Source" id="txt_source"  style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="coliforms" style="text-align:center;" ><input type="text"  placeholder="Coliforms" id="txt_coliforms" name="coliforms"   style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="ecoli" style="text-align:center;" ><input class="form-control" type="text" placeholder="E.Coli/g"   name="ecoli" id="txt_ecolig"  style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="yeast" style="text-align:center;" ><input type="text" class="form-control" name="yeast" id="txt_yeastandmould" placeholder="Yeast and Mould"  style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ></td>';
            results += '<td data-title="tbcg1" style="text-align:center;" ><input type="text" class="form-control" name="tbcg1" id="txt_tbcg1" placeholder="Tbc/G-10" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ></td>';
            results += '<td data-title="tbcg2" style="text-align:center;" ><input type="text" class="form-control" name="tbcg2" id="txt_tbcg2" placeholder="Tbc/G-10(2)"  style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ></td>';
            results += '<td data-title="tbcg3" style="text-align:center;" ><input type="text" class="form-control" name="tbcg3" id="txt_tbcg3" placeholder="Tbc/G-10(3)"  style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ></td>';
            results += '<td data-title="tbcg4" style="text-align:center;" ><input type="text" class="form-control" name="tbcg4" id="txt_tbcg4" placeholder="Tbc/G-10(4)"  style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ></td>';
            results += '<td data-title="remarks" style="text-align:center;" ><input type="text" class="form-control" name="remarks" id="txt_remarks" placeholder="Remarks" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ></td>';
            results += '<td style="display:none" class="6">' + i + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_microdata").html(results);
    }

    var mDataTable;
    function add_mswab() {
        mDataTable = [];
        var source = 0;
        var coliforms = 0;
        var ecolig = 0;
        var yeastandmould = 0;
        var tbcg1 = 0;
        var tbcg2 = 0;
        var tbcg3 = 0;
        var tbcg4 = 0;
        var remarks = 0;
        var txtsno = 0;
        var sno = 0;
        var rows = $("#tbl_micro_details tr:gt(0)");
        var rowsno = 1;
        $(rows).each(function (i, obj) {
            if ($(this).find('#txt_source').val() != "") {
                txtsno = rowsno;
                source = $(this).find('#txt_source').val();
                coliforms = $(this).find('#txt_coliforms').val();
                ecolig = $(this).find('#txt_ecolig').val();
                yeastandmould = $(this).find('#txt_yeastandmould').val();

                tbcg1 = $(this).find('#txt_tbcg1').val();
                tbcg2 = $(this).find('#txt_tbcg2').val();
                tbcg3 = $(this).find('#txt_tbcg3').val();
                tbcg4 = $(this).find('#txt_tbcg4').val();
                remarks = $(this).find('#txt_remarks').val();

                sno = $(this).find('#txt_sub_sno').val();
                mDataTable.push({ Sno: txtsno, source: source, coliforms: coliforms, ecolig: ecolig, yeastandmould: yeastandmould, tbcg1: tbcg1, tbcg2: tbcg2, tbcg3: tbcg3, tbcg4: tbcg4, remarks: remarks });
                rowsno++;
            }
        });
        source = 0;
        coliforms = 0;
        ecolig = 0;
        yeastandmould = 0;
        tbcg1 = 0;
        tbcg2 = 0;
        remarks = 0;
        var Sno = parseInt(txtsno) + 1;
        mDataTable.push({ Sno: Sno, source: source, coliforms: coliforms, ecolig: ecolig, yeastandmould: yeastandmould, tbcg1: tbcg1, tbcg2: tbcg2, tbcg3: tbcg3, tbcg4: tbcg4, remarks: remarks });
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tbl_micro_details">';
        results += '<thead><tr><th scope="col" style="font-weight: 700;">Sno</th><th scope="col" style="font-weight: 700;">Source</th><th scope="col" style="font-weight: 700;">Coliforms</th><th scope="col" style="font-weight: 700;">E.Coli/G</th><th scope="col" style="font-weight: 700;">Yeast AND Mould</th><th scope="col" style="font-weight: 700;">Tbc/G-10</th><th scope="col" style="font-weight: 700;">Tbc/G-10(2)</th><th scope="col" style="font-weight: 700;">Tbc/G-10(3)</th><th scope="col" style="font-weight: 700;">Tbc/G-10(4)</th><th scope="col" style="font-weight: 700;">Remarks</th></tr></thead></tbody>';
        for (var i = 0; i < mDataTable.length; i++) {
            results += '<tr><td scope="row" class="1" style="text-align:center;" id="txtsno">' + mDataTable[i].Sno + '</td>';
            results += '<td ><input id="txt_source" type="text" class="form-control"  style="font-size:12px;padding: 0px 5px;height:30px;"   value="' + mDataTable[i].source + '"/></td>';
            results += '<td ><input id="txt_coliforms" type="text" class="form-control"  style="font-size:12px;padding: 0px 5px;height:30px;" value="' + mDataTable[i].coliforms + '"/></td>';
            results += '<td ><input id="txt_ecolig" type="text" class="form-control"  style="font-size:12px;padding: 0px 5px;height:30px;" value="' + mDataTable[i].ecolig + '"/></td>';
            results += '<td ><input id="txt_yeastandmould" type="text" class="form-control"  style="font-size:12px;padding: 0px 5px;height:30px;" value="' + mDataTable[i].yeastandmould + '"/></td>';

            results += '<td ><input id="txt_tbcg1" type="text" class="form-control"  style="font-size:12px;padding: 0px 5px;height:30px;" value="' + mDataTable[i].tbcg1 + '"/></td>';
            results += '<td ><input id="txt_tbcg2" type="text" class="form-control" style="font-size:12px;padding: 0px 5px;height:30px;" value="' + mDataTable[i].tbcg2 + '"/></td>';
            results += '<td ><input id="txt_tbcg3" type="text" class="form-control" style="font-size:12px;padding: 0px 5px;height:30px;" value="' + mDataTable[i].tbcg3 + '"/></td>';
            results += '<td ><input id="txt_tbcg4" type="text" class="form-control" style="font-size:12px;padding: 0px 5px;height:30px;" value="' + mDataTable[i].tbcg4 + '"/></td>';
            results += '<td ><input id="txt_remarks" type="text"  class="form-control"  style="font-size:12px;padding: 0px 5px;height:30px;" value="' + mDataTable[i].remarks + '"/></td>';

            results += '<td style="display:none" class="6">' + i + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_microdata").html(results);
    }
    function save_micro_details() {
        var date = document.getElementById('txt_mdate').value;
        var microbiologist = document.getElementById('txt_microbiologist').value;
        var qaic = document.getElementById('txt_qaic').value;
        var btnval = document.getElementById('btn_msave').innerHTML;
        var sno = document.getElementById('lbl_msno').value;
        var microdetailsarray = [];
        $('#tbl_micro_details> tbody > tr').each(function () {
            var txtsno = $(this).find('#txtsno').val();
            var source = $(this).find('#txt_source').val();
            var coliforms = $(this).find('#txt_coliforms').val();
            var ecolig = $(this).find('#txt_ecolig').val();
            var yeastandmould = $(this).find('#txt_yeastandmould').val();
            var tbcg1 = $(this).find('#txt_tbcg1').val();
            var tbcg2 = $(this).find('#txt_tbcg2').val();
            var tbcg3 = $(this).find('#txt_tbcg3').val();
            var tbcg4 = $(this).find('#txt_tbcg4').val();
            var remarks = $(this).find('#txt_remarks').val();
            if ($(this).find('#txt_source').val() != "") {
                microdetailsarray.push({ 'txtsno': txtsno, 'source': source, 'coliforms': coliforms, 'ecolig': ecolig, 'yeastandmould': yeastandmould, 'tbcg1': tbcg1, 'tbcg2': tbcg2, 'tbcg3': tbcg3, 'tbcg4': tbcg3, 'remarks': remarks });
            }
        });
        if (microdetailsarray.length == "0") {
            alert("Please enter Source");
            return false;
        }
        var confi = confirm("Do you want to Save Transaction ?");
        if (confi) {
            var Data = { 'op': 'save_micro_details', 'btnval': btnval, 'microbiologist': microbiologist, 'qaic': qaic, 'sno': sno, 'date': date, 'microdetailsarray': microdetailsarray };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    forclearallmicro();
                    get_microbiology_details();
                    $('#fillform_micro').css('display', 'none');
                    $('#showlogs_micro').css('display', 'block');
                    $('#div_editmicrodata').show();
                }
            }
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson(Data, s, e);
        }
    }
    function get_microbiology_details() {
        var data = { 'op': 'get_microbiology_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillmirodetails(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function fillmirodetails(msg) {
        var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Souce</th></th><th scope="col" style="font-weight: bold;">Coloforms</th><th scope="col" style="font-weight: bold;">Ecoli/G</th><th scope="col" style="font-weight: bold;">Yeast And Mould</th><th scope="col" style="font-weight: bold;">TBC/G-10(1)</th><th scope="col" style="font-weight: bold;">TBC/G-10(2)</th><th scope="col" style="font-weight: bold;">TBC/G-10(3)</th><th scope="col" style="font-weight: bold;">TBC/G-10(4)</th><th scope="col" style="font-weight: bold;">Date</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color:' + COLOR[l] + '">';
            results += '<td data-title="Capacity" class="11">' + msg[i].source + '</td>';
            results += '<td data-title="Capacity" class="2">' + msg[i].coliforms + '</td>';
            results += '<td data-title="Capacity" class="2">' + msg[i].ecolig + '</td>';
            results += '<td data-title="Capacity" class="3">' + msg[i].yeastandmould + '</td>';
            results += '<td data-title="Capacity" class="3">' + msg[i].tbcg1 + '</td>';
            results += '<td data-title="Capacity" class="10">' + msg[i].tbcg2 + '</td>';
            results += '<td data-title="Capacity" class="8">' + msg[i].tbcg3 + '</td>';
            results += '<td data-title="Capacity" class="9">' + msg[i].tbcg4 + '</td>';
            results += '<td data-title="Capacity" class="4">' + msg[i].date + '</td></tr>';
            l = l + 1;
            if (l == 4) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_editmicrodata").html(results);
    }
    function forclearallmicro() {
        document.getElementById('txt_microbiologist').value = "";
        document.getElementById('txt_qaic').value = "";
        document.getElementById('btn_msave').innerHTML = "Save";
        document.getElementById('lbl_msno').value = "";
        var empty = [];
        var results = '<div  style="overflow:auto;"><table id="tbl_swab_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">Sno</th><th scope="col" >Source</th><th scope="col">Date</th><th scope="col">Swab Delay</th><th scope="col">Remarks</th><th scope="col"></th></tr></thead></tbody>';
        for (var i = 0; i < empty.length; i++) {
        }
        results += '</table></div>';
        $("#div_microdata").html(results);
    }
    function Getswabdetails() {
        var results = '<div style="overflow:auto;"><table id="tbl_swab_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Source</th><th scope="col">Date</th><th scope="col">Swab Delay</th><th scope="col">Remarks</th></tr></thead></tbody>';
        for (var i = 1; i < 2; i++) {
            results += '<tr><td data-title="Sno" id="txtsno" class="1">' + i + '</td>';
            results += '<td data-title="Source" style="text-align:center;" ><input  type="text" placeholder="Source" class="form-control 2"  name="Source" id="txt_source1"  style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Date" style="text-align:center;" ><input type="date"  placeholder="Date" id="txt_sdate" name="Date" class="form-control 3"  style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Swab Delay" style="text-align:center;" ><input class="form-control" type="text" placeholder="Delay" class="form-control 4"  name="Delay" id="tct_sdelay"  style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
            results += '<td data-title="Remarks" style="text-align:center;" ><input type="text" class="form-control" name="Remarks" id="txt_remarks" class="form-control 5" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;" ></td>';
            results += '<td style="display:none" class="6">' + i + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_swabdata").html(results);
    }

    var DataTable;
    function add_swab() {
        DataTable = [];
        var source = 0;
        var date = 0;
        var swabdelay = 0;
        var remarks = 0;
        var txtsno = 0;
        var sno = 0;
        var rows = $("#tbl_swab_details tr:gt(0)");
        var rowsno = 1;
        $(rows).each(function (i, obj) {
            if ($(this).find('#txt_source1').val() != "") {
                txtsno = rowsno;
                source = $(this).find('#txt_source1').val();
                date = $(this).find('#txt_sdate').val();
                swabdelay = $(this).find('#tct_sdelay').val();
                remarks = $(this).find('#txt_remarks').val();
                sno = $(this).find('#txt_sub_sno').val();
                DataTable.push({ Sno: txtsno, source: source, date: date, swabdelay: swabdelay, remarks: remarks });
                rowsno++;
            }
        });
        source = 0;
        date = 0;
        swabdelay = 0;
        remarks = 0;
        var Sno = parseInt(txtsno) + 1;
        DataTable.push({ Sno: Sno, source: source, date: date, swabdelay: swabdelay, remarks: remarks });
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tbl_swab_details">';
        results += '<thead><tr><th scope="col">Sno</th><th scope="col">Source</th><th scope="col">Date</th><th scope="col">Swab Delay</th><th scope="col">Remarks</th></tr></thead></tbody>';
        for (var i = 0; i < DataTable.length; i++) {
            results += '<tr><td scope="row" class="1" style="text-align:center;" id="txtsno">' + DataTable[i].Sno + '</td>';
            results += '<td ><input id="txt_source1" type="text" class="clssource form-control"    value="' + DataTable[i].source + '"/></td>';
            results += '<td style="display:none;" class="2">' + DataTable[i].source + '</td>';
            results += '<td ><input id="txt_sdate" type="date" class="3 form-control"   value="' + DataTable[i].date + '"/></td>';
            results += '<td ><input id="tct_sdelay" type="text" class="4 form-control"   value="' + DataTable[i].swabdelay + '"/></td>';
            results += '<td ><input id="txt_remarks" type="text" class="5 form-control"   value="' + DataTable[i].remarks + '"/></td>';
            results += '<td data-title="Minus"><input id="btn_poplate" type="button"  onclick="removerow(this)" name="Edit" class="btn btn-primary" value="Remove" /></td>';
            results += '<td style="display:none" class="6">' + i + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_swabdata").html(results);
    }
    var swabdel = [];
    function removerow(thisid) {
        DataTable = [];
        var rows = $("#tbl_swab_details tr:gt(0)");
        $(rows).each(function (i, obj) {
            if ($(this).find('#txt_source1').val() != "") {
                txtsno = rowsno;
                source = $(this).find('#txt_source1').val();
                date = $(this).find('#txt_sdate').val();
                swabdelay = $(this).find('#tct_sdelay').val();
                remarks = $(this).find('#txt_remarks').val();
                sno = $(this).find('#txt_sub_sno').val();
                DataTable.push({ Sno: txtsno, source: source, date: date, swabdelay: swabdelay, remarks: remarks });
            }
        });
        var source_name = $(thisid).parent().parent().children('.2').html();
        var txtsno = 0;
        var rowsno = 1;
        swabdel = [];
        for (var i = 0; i < DataTable.length; i++) {
            if (source_name == DataTable[i].source) {
            }
            else {
                txtsno = rowsno;
                var source = DataTable[i].source;
                var date = DataTable[i].date;
                var swabdelay = DataTable[i].swabdelay;
                var remarks = DataTable[i].remarks;
                ///var sno = DataTable[i].sno;
                swabdel.push({ Sno: txtsno, source: source, date: date, swabdelay: swabdelay, remarks: remarks });
            }
        }
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tbl_swab_details">';
        results += '<thead><tr><th scope="col">Sno</th><th scope="col" >Source</th><th scope="col">Date</th><th scope="col">Swab Delay</th><th scope="col">Remarks</th><th scope="col"></th></tr></thead></tbody>';
        var j = 1;
        for (var i = 0; i < swabdel.length; i++) {
            results += '<tr><td scope="row" class="1" style="text-align:center;" id="txtsno">' + j + '</td>';
            results += '<td ><input id="txt_source1" type="text" class="clssource form-control"  value="' + swabdel[i].source + '"/></td>';
            results += '<td style="display:none;" class="2">' + swabdel[i].source + '</td>';
            results += '<td ><input id="txt_sdate" type="date" class="3 form-control" value="' + swabdel[i].date + '"/></td>';
            results += '<td ><input id="tct_sdelay" type="text" class="4 form-control"  value="' + swabdel[i].swabdelay + '"/></td>';
            results += '<td ><input id="txt_remarks" type="text" class="5 form-control"  value="' + swabdel[i].remarks + '"/></td>';
            results += '<td data-title="Minus"><input id="btn_poplate" type="button"  onclick="removerow(this)" name="Edit" class="btn btn-primary" value="Remove" /></td>';
            results += '<td style="display:none" class="6">' + i + '</td></tr>';
            j++;
        }
        results += '</table></div>';
        $("#div_swabdata").html(results);
    }
    function save_swab_details() {
        var btnval = document.getElementById('btn_save').innerHTML;
        var sno = document.getElementById('lbl_sno').value;
        var swabdetailsarray = [];
        $('#tbl_swab_details> tbody > tr').each(function () {
            var txtsno = $(this).find('#txtsno').val();
            var source = $(this).find('#txt_source1').val();
            var date = $(this).find('#txt_sdate').val();
            var swabdelay = $(this).find('#tct_sdelay').val();
            var remarks = $(this).find('#txt_remarks').val();
            if ($(this).find('#txt_source1').val() != "") {
                swabdetailsarray.push({ 'txtsno': txtsno, 'source': source, 'date': date, 'swabdelay': swabdelay, 'remarks': remarks });
            }
        });
        if (swabdetailsarray.length == "0") {
            alert("Please enter Source");
            return false;
        }
        var Data = { 'op': 'save_swab_details', 'btnval': btnval, 'sno': sno, 'swabdetailsarray': swabdetailsarray };
        var s = function (msg) {
            if (msg) {
                alert(msg);
                forclearall();
                $('#fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_editswabdata').show();
                get_swabanalysis_details();
            }
        }
        var e = function (x, h, e) {
        };
        CallHandlerUsingJson(Data, s, e);
    }
    function forclearall() {
        document.getElementById('btn_save').innerHTML= "Save";
        document.getElementById('lbl_sno').value = "";
        var empty = [];
        var results = '<div  style="overflow:auto;"><table id="tbl_swab_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">Sno</th><th scope="col" >Source</th><th scope="col">Date</th><th scope="col">Swab Delay</th><th scope="col">Remarks</th><th scope="col"></th></tr></thead></tbody>';
        for (var i = 0; i < empty.length; i++) {
        }
        results += '</table></div>';
    }

    function get_swabanalysis_details() {
        var data = { 'op': 'get_swabanalysis_details' };
        var s = function (msg) {
            if (msg) {
                if (msg.length > 0) {
                    fillswabdetails(msg);
                }
            }
            else {
            }
        };
        var e = function (x, h, e) {
        }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    function fillswabdetails(msg) {
        var results = '<div style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr><th scope="col">Souce</th></th><th scope="col">SwabDate</th><th scope="col">SwabDelay</th><th scope="col">Remarks</th><th scope="col">Date</th></tr></thead></tbody>';
        for (var i = 0; i < msg.length; i++) {
            results += '<tr><td data-title="Capacity" class="11">' + msg[i].source + '</td>';
            results += '<td data-title="Capacity" class="2">' + msg[i].swabdate + '</td>';
            results += '<td data-title="Capacity" class="2">' + msg[i].swabdelay + '</td>';
            results += '<td data-title="Capacity" class="3">' + msg[i].remarks + '</td>';
            results += '<td data-title="Capacity" class="3">' + msg[i].date + '</td></tr>';
        }
        results += '</table></div>';
        $("#div_editswabdata").html(results);
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            <i class="fa fa-files-o" aria-hidden="true"></i>Microbiology and Swab Analysis Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Microbiology and Swab Analysis Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showmicrobiology()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Microbiology</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showswabanlysis()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Swab Analysis</a></li>
                </ul>
            </div>
            <div id="div_microbiology">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Microbiology 
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs_micro" style="text-align: -webkit-right;">
                    <%--<input id="btn_addmicro" type="button" name="submit" value='Add Details' class="btn btn-success" />--%>
                <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addmicrobiologydetails()"></span> <span onclick="addmicrobiologydetails()">Add Details</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="div_editmicrodata">
                </div>
                <div id='fillform_micro' style="display: none;">
                <div style="padding-left:365px;">
                    <table align="center">
                     <tr>
                            <td style="height: 40px;">
                                <label>
                                   Date : </label>
                            </td>
                            <td>
                                <input id="txt_mdate" class="form-control" type="datetime-local" name="vendorcode"
                                    placeholder="Enter Date">
                            </td>
                        </tr>
                    </table>
                    </div>
                    <div>
                        <div class="box-body">
                            <div id="div_microdata">
                            </div>
                        </div>
                        <p id="mnewrow">
                            <input type="button" id="btn_addmicrodetails" onclick="add_mswab();" class="btn btn-default"
                                value="Insert row" /></p>
                    </div>
                    <label id="lbl_msno" style="display: none;">
                    </label>
                    <div id="">
                    </div>
                    <div  align="center">
                    <table>
                     <tr>
                            <td style="height: 40px;">
                                <label>
                                   Micro Biologist</label>
                            </td>
                            <td>
                                <input id="txt_microbiologist" class="form-control" type="text" name="vendorcode"
                                    placeholder="Enter Micro Biologist">
                            </td>
                            <td>
                            </td>
                            <td style="height: 40px;">
                                <label>
                                   QA I/C</label>
                            </td>
                            <td>
                                <input id="txt_qaic" class="form-control" type="text" name="vendorcode"
                                    placeholder="Enter QA I/C">
                            </td>
                        </tr>
                    </table>
                    <%--<table>
                        <tr>
                            <td>
                                <input type="button" class="btn btn-success" id="btn_msave" value="Save" onclick="save_micro_details();" />
                                <input type="button" class="btn btn-danger" id="btn_mclose" value="Close" />
                            </td>
                        </tr>
                    </table>--%>
                      <div  style="padding-left: 10%;padding-top: 5%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_msave1" onclick="save_micro_details()"></span><span id="btn_msave" onclick="save_micro_details()">Save</span>
                            </div>
                            </div>
                            </td>
                            <td style="width:10px;"></td>
                            <td>
                                <div class="input-group">
                                <div class="input-group-close">
                                <span class="glyphicon glyphicon-remove" id='btn_mclose1' onclick="closemicrobilogy()"></span><span id='btn_mclose' onclick="closemicrobilogy()">Close</span>
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

            <div id="div_swabanalysis">
            <div class="box info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Swab Analysis 
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" style="text-align: -webkit-right;">
                   <%-- <input id="btn_addswab" type="button" name="submit" value='Add SwabDetails' class="btn btn-success" />--%>
                   <table>
                        <tr>
                            <td>
                            <td>
                            </td>
                            <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="addswabdetails()"></span> <span onclick="addswabdetails()">Add SwabDetails</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                </div>
                <div id="div_editswabdata">
                </div>
                <div id='fillform' style="display: none;">
                    <div>
                        <div class="box-body">
                            <div id="div_swabdata">
                            </div>
                        </div>
                        <p id="newrow">
                            <input type="button" id="btn_addswabdetails" onclick="add_swab();" class="btn btn-default"
                                value="Insert row" /></p>
                    </div>
                    <label id="lbl_sno" style="display: none;">
                    </label>
                    <%--<div style="padding-left: 450px;">
                        <table align="center">
                            <tr>
                                <td>
                                    <input type="button" class="btn btn-success" id="btn_save" value="Save" onclick="save_swab_details();" />
                                    <input type="button" class="btn btn-danger" id="btn_close" value="Close" />
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                    <div  style="padding-left: 35%;">
                        <table>
                        <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="save_swab_details()"></span><span id="btn_save" onclick="save_swab_details()">Save</span>
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
      </div>
    </div>
  </section>
</asp:Content>

