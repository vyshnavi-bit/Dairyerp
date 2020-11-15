<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="CCQualityForm.aspx.cs" Inherits="CCQualityForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/JavaScript">
        $(function () {
            //showEmployeeImport();
            GetFixedrows();
            GetMilkdetailes();
            GetSilodetailes();
            Getremarks();
            $("#rejectdata").show();
        });
        function showEntrydetailes() {
            $("#rejectdata").show();
            $("#Reject_data").hide();
            $("#MIlkRoute").hide();
            $("#Silodetailes").hide();
            $("#Remarksdata").hide();
        }
        function shwRejectcans() {
            $("#rejectdata").hide();
            $("#Reject_data").show();
            $("#MIlkRoute").hide();
            $("#Silodetailes").hide();
            $("#Remarksdata").hide();
        }
        function ShowMilkscreen() {
            $("#rejectdata").hide();
            $("#Reject_data").hide();
            $("#MIlkRoute").show();
            $("#Silodetailes").hide();
            $("#Remarksdata").hide();
        }

        function ShowSilo() {
            $("#rejectdata").hide();
            $("#Reject_data").hide();
            $("#MIlkRoute").hide();
            $("#Silodetailes").show();
            $("#Remarksdata").hide();
        }

        function ShowSiloremarks() {
            $("#rejectdata").hide();
            $("#Reject_data").hide();
            $("#MIlkRoute").hide();
            $("#Silodetailes").hide();
            $("#Remarksdata").show();
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
        function GetFixedrows() {
            var results = '<div class="divcontainer" style="overflow:auto;"><table id="tbl_rejectd_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">CanNo</th><th scope="col">RouteName</th><th scope="col">Qty(Kgs)</th><th scope="col">Remarks</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 1; i < 11; i++) {
                results += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results += '<th scope="row" class="1"><input class="form-control"  type="text" placeholder="Can Number" name="CanNumber" id="txt_CanNo" value=" " style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Route Name" name="Route" value="" id="txt_RouteName" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text"  placeholder="QTY" name="Qty" value=""  id="txt_Qty" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Remarks"  id="txt_remarks" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
        }

        function GetMilkdetailes() {
            var results = '<div class="divcontainer" style="overflow:auto;"><table id="tbl_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">CanNo</th><th scope="col">RouteName</th><th scope="col">Qty(Kgs)</th><th scope="col">Remarks</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 1; i < 11; i++) {
                results += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results += '<th scope="row" class="1"><input class="form-control"  type="text" placeholder="Can Number" name="CanNumber" id="txtCanNo" value=" " style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="Route Name" name="Route" value="" id="txtQty" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="SNF" style="text-align:center;" class="4"><input class="form-control" type="text"  placeholder="QTY" name="Qty" value=""  id="txtRouteName" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder="Remarks"  id="txtremarks" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
            }
            results += '</table></div>';
            $("#div_MIlkdata").html(results);
        }

        function GetSilodetailes() {
            var Silonames = "Quantity,FAT %,CLR,SNF %,MBRT,H.S,TEMPERATUE,OT,ACIDITY,Age OF Milk";
            var names = Silonames.split(',');
            var results = '<div class="divcontainer" style="overflow:auto;"><table id="tbl_silo_milk_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Silo Milk Quality</th><th scope="col">Silo1</th><th scope="col">Silo2</th><th scope="col">Silo3</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < 10; i++) {
                results += '<td data-title="Sno" class="2">' + j++ + '</td>';
                results += '<th scope="row" class="1"><input class="form-control" disabled="disabled" type="text"  id="txt_silonames" placeholder="Silonames" name="Silonames" value="' + names[i] + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<th scope="row" class="1"><input class="form-control"  type="text" placeholder="" name="CanNumber" id="txt_silo1" value=" " style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Qty" style="text-align:center;" class="3"><input class="form-control" type="text" placeholder="" name="Route" value="" id="txt_silo2"  style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="CLR" style="text-align:center;" class="5"><input class="form-control" type="text" placeholder=""  id="txt_silo3" name="CLR" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td></tr>';
            }
            results += '</table></div>';
            $("#div_Silodata").html(results);
        }

        function Getremarks() {
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tabledetails">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Conclusion</th></tr></thead></tbody>';
            for (var i = 1; i < 15; i++) {
                results += '<tr><td scope="row" class="1" style="text-align:center;" id="txtsno">' + i + '</td>';
                results += '<td ><input id="txtConclusion" class="codecls"   placeholder= "Conclusion" style="width:920px;height:50px;" /></td>';
                results += '<td data-title="Minus"><span><img src="images/minus.png" onclick="removerow(this)" style="cursor:pointer"/></span></td>';
                results += '<td style="display:none" class="4">' + i + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_Remarks").html(results);
        }

        function save_CCQuantity_Silo_click() {
            var ChilingCenter = document.getElementById('txt_Chillingname').value;
            //            var Centerplace = document.getElementById('txt_Chillingplace').value;
            var StartingTime = document.getElementById('txtstartTime').value;
            var NoRoutes = document.getElementById('txt_NoRoutes').value;
            var LastArrival = document.getElementById('txt_LastArrival').value;
            var NoCans = document.getElementById('txt_NOCans').value;
            var RejectionQtymilk = document.getElementById('txt_RejAdultMilk').value;
            var Dumpendtime = document.getElementById('txt_DumpingTime').value;
            var TemperatureCan = document.getElementById('txt_TemperatuteCan').value;
            var TemperatureTub = document.getElementById('txt_TemperatuteTub').value;
            var MbrtRoute = document.getElementById('txt_MbrtRoute').value;
            var Chiledmilktemp = document.getElementById('txt_ChilledMilkTemp').value;
            var silombrt = document.getElementById('txt_SiloMLkMbrt').value;
            var Canlids = document.getElementById('txt_Cans&Lids').value;
            var ToplessVechicals = document.getElementById('txt_TopLessVechicals').value;
            var Recordmaintance = document.getElementById('txt_RecordMaintain').value;
            var plantcleaning = document.getElementById('txt_PlantHygeine').value;
            var Pipeline = document.getElementById('txt_Pipeline').value;
            var btnval = document.getElementById('btn_detailes').value;

//            var rejected_array = [];
//            $('#tbl_rejectd_milk_details> tbody > tr').each(function () {
//                //var txtsno = $(this).find('#txtSno').text();
//                var rejectcanno = $(this).find('#txt_CanNo').val();
//                var rejectRoutename = $(this).find('#txt_RouteName').val();
//                var rejectqty = $(this).find('#txt_Qty').val();
//                var rejectremarks = $(this).find('#txt_remarks').val();
//                //                var hdnproductsno = $(this).find('#hdnproductsno').val();
//                //                if (hdnproductsno == "" || hdnproductsno == "0") {
//                //                }
//                //                else {
//                rejected_array.push({ 'rejectcanno': rejectcanno, 'rejectRoutename': rejectRoutename, 'rejectqty': rejectqty, 'rejectremarks': rejectremarks
//                });
//                //}
//            });
//            var milk_array = [];
//            $('#tbl_milk_details> tbody > tr').each(function () {
//                //var txtsno = $(this).find('#txtSno').text();
//                var milkcanno = $(this).find('#txtCanNo').val();
//                var milkRoutename = $(this).find('#txtRouteName').val();
//                var milkqty = $(this).find('#txtQty').val();
//                var milkremarks = $(this).find('#txtremarks').val();
//                //                var hdnproductsno = $(this).find('#hdnproductsno').val();
//                //                if (hdnproductsno == "" || hdnproductsno == "0") {
//                //                }
//                //                else {
//                milk_array.push({ 'milkcanno': milkcanno, 'milkRoutename': milkRoutename, 'milkqty': milkqty, 'milkremarks': milkremarks
//                });
//                //}
//            });
//            var silo_array = [];
//            $('#tbl_silo_milk_details> tbody > tr').each(function () {
//                //var txtsno = $(this).find('#txtSno').text();
//                var silonames = $(this).find('#txt_silonames').val();
//                var silo1 = $(this).find('#txt_silo1').val();
//                var silo2 = $(this).find('#txt_silo2').val();
//                var silo3 = $(this).find('#txt_silo3').val();
//                //                var hdnproductsno = $(this).find('#hdnproductsno').val();
//                //                if (hdnproductsno == "" || hdnproductsno == "0") {
//                //                }
//                //                else {
//                silo_array.push({ 'silonames': silonames, 'silo1': silo1, 'silo2': silo2, 'silo3': silo3
//                });
//                //}
            //            });

            var Data = { 'op': 'save_CCQuantity_Silo_click', 'ChilingCenter': ChilingCenter, 'Centerplace': Centerplace, 'StartingTime': StartingTime, 'NoRoutes': NoRoutes, 'LastArrival': LastArrival, 'NoCans': NoCans, 'RejectionQtymilk': RejectionQtymilk, 'Dumpendtime': Dumpendtime, 'TemperatureCan': TemperatureCan, 'TemperatureTub': TemperatureTub, 'MbrtRoute': MbrtRoute, 'Chiledmilktemp': Chiledmilktemp, 'silombrt': silombrt, 'Canlids': Canlids, 'ToplessVechicals': ToplessVechicals, 'Recordmaintance': Recordmaintance, 'plantcleaning': plantcleaning, 'Pipeline': Pipeline, 'btnval': btnval };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    forclearall();
                }
            }
            var e = function (x, h, e) {
            };
              CallHandlerUsingJson(Data, s, e);
        }
        function clearccquantitymilk() {
            document.getElementById('txt_Chillingname').value = "";
            document.getElementById('txtstartTime').value = "";
            document.getElementById('txt_NoRoutes').value = "";
            document.getElementById('txt_LastArrival').value = "";
            document.getElementById('txt_NOCans').value = "";
            document.getElementById('txt_RejAdultMilk').value = "";
            document.getElementById('txt_DumpingTime').value = "";
            document.getElementById('txt_TemperatuteCan').value = "";
            document.getElementById('txt_TemperatuteTub').value = "";
            document.getElementById('txt_MbrtRoute').value = "";
            document.getElementById('txt_ChilledMilkTemp').value = "";
            document.getElementById('txt_SiloMLkMbrt').value = "";
            document.getElementById('txt_Cans&Lids').value = "";
            document.getElementById('txt_TopLessVechicals').value = "";
            document.getElementById('txt_RecordMaintain').value = "";
            document.getElementById('txt_PlantHygeine').value = "";
            document.getElementById('txt_Pipeline').value = "";
            document.getElementById('btn_detailes').value = "Save";

        }
        function save_CCQuantity_Rejectmilk_click() {
            var rejectcanno = document.getElementById('txt_CanNo').value;
            var rejectRoutename = document.getElementById('txt_RouteName').value;
            var rejectqty = document.getElementById('txt_Qty').value;
            var rejectremarks = document.getElementById('txt_remarks').value;
            var btnval = document.getElementById('btn_reject').value;

             var Data = { 'op': 'save_CCQuantity_Rejectmilk_click', 'rejectcanno': rejectcanno, 'rejectRoutename': rejectRoutename, 'rejectqty': rejectqty, 'rejectremarks': rejectremarks ,'btnval': btnval };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    forclearall();
                }
            }
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson(Data, s, e);
        }
        function clearreturnmilk() {
            document.getElementById('txt_CanNo').value = "";
            document.getElementById('txt_RouteName').value = "";
            document.getElementById('txt_Qty').value = "";
            document.getElementById('txt_remarks').value = "";
            document.getElementById('btn_reject').value = "Save";


        }
        
        function save_CCQuantity_milkSilo_click() {
            var milkcanno = document.getElementById('txtCanNo').value;
            var milkRoutename = document.getElementById('txtRouteName').value;
            var milkqty = document.getElementById('txtQty').value;
            var milkremarks = document.getElementById('txtremarks').value;
            var btnval = document.getElementById('btn_milk').value;

             var Data = { 'op': 'save_CCQuantity_milkSilo_click', 'milkcanno': milkcanno, 'milkRoutename': milkRoutename, 'milkqty': milkqty, 'milkremarks': milkremarks, 'btnval': btnval };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    forclearall();
                }
            }
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson(Data, s, e);
        }
        function clearmilksilo() {
            document.getElementById('txtCanNo').value = "";
            document.getElementById('txtRouteName').value = "";
            document.getElementById('txtQty').value = "";
            document.getElementById('txtremarks').value = "";
            document.getElementById('btn_milk').value = "Save";

        }
        function save_CCQuantity_Silomilk_click() {
            var silonames = document.getElementById('txt_silonames').value;
            var silo1 = document.getElementById('txt_silo1').value;
            var silo2 = document.getElementById('txt_silo2').value;
            var silo3 = document.getElementById('txt_silo3').value;
            var btnval = document.getElementById('btn_silo').value;
             var Data = { 'op': 'save_CCQuantity_Silomilk_click', 'silonames': silonames, 'silo1': silo1, 'silo2': silo2, 'silo3': silo3, 'btnval': btnval };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    forclearall();
                }
            }
            var e = function (x, h, e) {
            };
              CallHandlerUsingJson(Data, s, e);

          }
          function clearsilomilk() {
              document.getElementById('txt_silonames').value = "";
              document.getElementById('txt_silo1').value = "";
              document.getElementById('txt_silo2').value = "";
              document.getElementById('txt_silo3').value = "";
              document.getElementById('btn_silo').value = "Save";

          }


          function save_CCQuantity_remarks_click() {
              var sno = document.getElementById('txtsno').value;
              var Conclusion = document.getElementById('txtConclusion').value;
              var btnval = document.getElementById('save_milktremarks').value;
              var Data = { 'op': 'save_CCQuantity_remarks_click', 'Conclusion': Conclusion, 'sno': sno, 'btnval': btnval };
              var s = function (msg) {
                  if (msg) {
                      alert(msg);
                      forclearall();
                  }
              }
              var e = function (x, h, e) {
              };
              CallHandlerUsingJson(Data, s, e);
          }

          function clearvalues() {
              document.getElementById('txtsno').value = "";
              document.getElementById('txtConclusion').value = "";
              document.getElementById('save_milktremarks').value = "Save";
              var empty = [];
              var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tabledetails">';
              results += '<thead><tr><th scope="col">Sno</th><th scope="col">Conclusion</th></tr></thead></tbody>';
              for (var i = 0; i < empty.length; i++) {
              }
              results += '</table></div>';
              $("#div_Remarks").html(results);
          }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            CC Quality Form <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">CC Quality Form</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>CC Quality Form Details
                </h3>
            </div>
            <div class="box-body">
                <div>
                    <ul class="nav nav-tabs">
                        <li id="Li2" class="active"><a data-toggle="tab" href="#" onclick="showEntrydetailes()">
                            <i class="fa fa-user" aria-hidden="true"></i>&nbsp;&nbsp;Entry Details</a></li>
                        <li id="id_tab_Personal" class=""><a data-toggle="tab" href="#" onclick="shwRejectcans()">
                            <i class="fa fa-registered" aria-hidden="true"></i>&nbsp;&nbsp;Rejected Cans</a></li>
                        <li id="Li1" class=""><a data-toggle="tab" href="#" onclick="ShowMilkscreen()"><i
                            class="fa fa-file-text"></i>&nbsp;&nbsp;Milk screen display Aduttrarion</a></li>
                        <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="ShowSilo()">
                            <i class="fa fa-mobile" aria-hidden="true"></i>&nbsp;&nbsp;SiloDetailes</a></li>
                        <li id="Li3" class=""><a data-toggle="tab" href="#" onclick="ShowSiloremarks()"><i
                            class="fa fa-mobile" aria-hidden="true"></i>&nbsp;&nbsp;Remarks</a></li>
                    </ul>
                </div>
                <div id='rejectdata' style="display: none">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>CC Quality Form Details
                        </h3>
                    </div>
                    <div id='Entry_data'>
                        <table align="center">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px;">
                                    <label>
                                        Datetime</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input id="txt_date" class="form-control" type="datetime-local" name="vendorcode"
                                        placeholder="Enter Date" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px;">
                                    <label>
                                        Chiling Center Place
                                    </label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_Chillingname" class="form-control" name="vendorcode"
                                        placeholder="Enter  Chiling Center"><label id="lbl_code_error_msg" class="errormessage">*
                                            Please Enter Center place</label>
                                </td>
                                <td>
                                    <label>
                                        Dumping Starting Time
                                    </label>
                                </td>
                                <td>
                                    <input type="time" maxlength="45" id="txtstartTime" class="form-control" name="vendorcode"
                                        placeholder="Enter Dumping No">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        No.OF.Routes</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="height: 40px;">
                                    <input type="text" maxlength="45" id="txt_NoRoutes" class="form-control" name="vendorcode"
                                        placeholder="Enter No.OF.Routes">
                                </td>
                                <td style="height: 40px;">
                                    <label>
                                        Late Arrival Of Vehicle</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_LastArrival" class="form-control" name="vendorcode"
                                        placeholder="Enter Late Arrival Vehicle">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        NO.OF.Cans Reject</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_NOCans" class="form-control" name="vendorcode"
                                        placeholder="Enter No.Of Cans">
                                </td>
                                <td>
                                    <label>
                                        Rejection Qty.Of Adultrated Milk</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_RejAdultMilk" class="form-control" name="vendorcode"
                                        placeholder="Enter Rejection Qty">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Dumping Ending Time</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="Time" maxlength="45" id="txt_DumpingTime" class="form-control" name="vendorcode"
                                        placeholder="Enter  Ending Time">
                                </td>
                                <td>
                                    <label>
                                        Temperature Of Can Scrubber</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_TemperatuteCan" class="form-control" name="vendorcode"
                                        placeholder="Enter Temperature Can">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Temperature Of LID WasherTub</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_TemperatuteTub" class="form-control" name="vendorcode"
                                        placeholder="Enter Temperature Tub">
                                </td>
                                <td>
                                    <label>
                                        MBRT Less Than 1 HR Routes</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_MbrtRoute" class="form-control" name="vendorcode"
                                        placeholder="Enter MBRT Route">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Chilled Milk Temp.In Silo</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_ChilledMilkTemp" class="form-control" name="vendorcode"
                                        placeholder="Enter Chilled Milk">
                                </td>
                                <td>
                                    <label>
                                        Silo Milk MBRT</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_SiloMLkMbrt" class="form-control" name="vendorcode"
                                        placeholder="Enter Silo Milk">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Cans & Lids Clening</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_Cans&Lids" class="form-control" name="vendorcode"
                                        placeholder="Enter Cans Clening">
                                </td>
                                <td>
                                    <label>
                                        Top Less Vechicles</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_TopLessVechicals" class="form-control"
                                        name="vendorcode" placeholder="Enter Top Vechicles">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Records Maintainance</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_RecordMaintain" class="form-control" name="vendorcode"
                                        placeholder="Enter Records Maintainance">
                                </td>
                                <td>
                                    <label>
                                        PlantCleaning & Hygeine</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_PlantHygeine" class="form-control" name="vendorcode"
                                        placeholder="Enter PlantCleaning ">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Pipe Lines & Silo Cleaning</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_Pipeline" class="form-control" name="vendorcode"
                                        placeholder="Enter  Pipe Lines">
                                </td>
                                <td>
                                    <label>
                                        Silo Capacity</label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <input type="text" maxlength="45" id="txt_Silocapcity" class="form-control" name="vendorcode"
                                        placeholder="Enter Silo Capacity ">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <input id='btn_detailes' type="button" class="btn btn-success" name="submit" value='Save'
                            onclick="save_CCQuantity_Silo_click()" />
                        <input id='Button8' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearccquantitymilk()" />
                        <input id='Button9' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />
                    </div>
                </div>
                <div id='Reject_data' style="display: none">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>CC Quality Form Details
                        </h3>
                    </div>
                    <div id="div_vendordata">
                    </div>
                    <div>
                        <input id='btn_reject' type="button" class="btn btn-success" name="submit" value='Save'
                            onclick="save_CCQuantity_Rejectmilk_click()" />
                        <input id='Button11' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearreturnmilk()" />
                        <input id='Button12' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />
                    </div>
                </div>
                <div id='MIlkRoute' style="display: none">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>CC Quality Form Details
                        </h3>
                    </div>
                    <div id="div_MIlkdata">
                    </div>
                    <div>
                        <input id='btn_milk' type="button" class="btn btn-success" name="submit" value='Save'
                            onclick="save_CCQuantity_milkSilo_click()" />
                        <input id='Button5' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearmilksilo()" />
                        <input id='Button6' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />
                    </div>
                </div>
                <div id='Silodetailes' style="display: none">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>CC Quality Form Details
                        </h3>
                    </div>
                    <div id="div_Silodata">
                    </div>
                    <div>
                        <input id='btn_silo' type="button" class="btn btn-success" name="submit" value='Save'
                            onclick="save_CCQuantity_Silomilk_click()" />
                        <input id='Button2' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearsilomilk()" />
                        <input id='Button3' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />
                    </div>
                </div>
                <div id='Remarksdata' style="display: none">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>CC Quality Form Details
                        </h3>
                    </div>
                    <div id="div_Remarks">
                    </div>
                    <div>
                        <input id='save_milktremarks' type="button" class="btn btn-success" name="submit"
                            value='Save' onclick="save_CCQuantity_remarks_click()" />
                        <input id='close_vehmaster' type="button" class="btn btn-danger" name="Clear" value='Clear'
                            onclick="clearvalues()" />
                        <input id='btnPrint' type="button" class="btn btn-primary" name="Close" value='Print'
                            onclick="javascript:CallPrint('div_axilautofill');" />
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
