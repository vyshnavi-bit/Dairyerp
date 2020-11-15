<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true"
    CodeFile="distancecalculation.aspx.cs" Inherits="distancecalculation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://maps.googleapis.com/maps/api/js?v=3.5"></script>
    <script type="text/javascript">
        $(function () {
            get_vendors();
            k = 0;
            google.maps.event.addDomListener(window, 'load', initialize);
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
        function get_vendors() {
            var data = { 'op': 'get_vendor_shortcodedetails' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillvedors(msg);
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
        function fillvedors(msg) {
            var data = document.getElementById('slct_location');
            var length = data.options.length;
            document.getElementById('slct_location').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select location";
            opt.value = "Select location";
            opt.setAttribute("selected", "selected");
            opt.setAttribute("disabled", "disabled");
            opt.setAttribute("class", "dispalynone");
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].vendorname != null) {
                    var option = document.createElement('option');
                    option.innerHTML = msg[i].vendorname;
                    option.value = msg[i].sno;
                    data.appendChild(option);

                }
            }
        }
        function getlatlongdetails() {
            var location = document.getElementById('slct_location').value;
            if (location == "" || location == "Select location") {
                alert("Please select location");
                return false;
            }
            var data = { 'op': 'get_vendor_latlongdetails', 'location': location };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].sno == location) {
                                document.getElementById('txt_lat1').value = msg[i].latitude;
                                document.getElementById('txt_long1').value = msg[i].longitude;
                            }
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
        var vendordetails = [];
        var savedetails = [];
        function get_distance() {
            var lat1 = document.getElementById('txt_lat1').value;
            var lon1 = document.getElementById('txt_long1').value;
            var lat2 = "";
            var lon2 = "";
            var location = document.getElementById('slct_location').value;
            if (location == "" || location == "Select location") {
                alert("Please select location");
                return false;
            }
            google.maps.event.addDomListener(window, 'load', initialize);
            var data = { 'op': 'get_vendor_distancedetails', 'lat1': lat1, 'lon1': lon1 };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        vendordetails = msg;
                        savedetails = msg;
                        k = 0;
                        var results = '<div    style="overflow:auto;"><table id="table_shift_wise_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                        results += '<thead><tr><th scope="col">location</th><th scope="col">Distance</th></tr></thead></tbody>';
                        for (var i = 0; i < msg.length; i++) {
                            if (msg[i].sno != location) {
                                lat2 = msg[i].latitude;
                                lon2 = msg[i].longitude;
                                var latlong1 = lat1 + "," + lon1;
                                var latlong2 = lat2 + "," + lon2;
                                var distance = "0";
                                calcRoute(latlong1, latlong2, msg[i].vendorname, msg[i].sno);
                                results += '<tr>';
                                results += '<th><span id="txt_vendorname" style="font-size: 12px; font-weight: bold; color: #0252aa;">' + msg[i].vendorname + '</span></th>';
                                results += '<td><input id="' + msg[i].sno + '" class="form-control" value="' + distance + '" type="text" name="distance" placeholder="Enter distance"></td>';
                                results += '<td style="display:none"><input id="hdn_vendordetails" class="form-control" value="' + location + '" type="number" name="vendorcode"></td>';
                                results += '<td style="display:none" class="8"><input id="hdn_vendorid" class="form-control" type="number" name="vendorcode" value="' + msg[i].sno + '"></td></tr>';
                            }
                        }
                        results += '</table></div>';
                        $("#divFillScreen").html(results);

                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function getcal() {
            for (var j = 0; j < totaldistancearray.length; j++) {
                var rows = $("#table_shift_wise_details tr:gt(0)");
                var distance_branch_wise_details = new Array();
                $(rows).each(function (i, obj) {
                    var vendor = $(this).find('#hdn_vendorid').val();
                    var vendorid = totaldistancearray[j].vendorid;
                    var distance = totaldistancearray[j].distance;
                    if (vendor == vendorid) {
                        $('#' + vendorid).val(distance);
                    }
                });
            }
        }
        function save_distance_section_click() {
            var rows = $("#table_shift_wise_details tr:gt(0)");
            var distance_branch_wise_details = new Array();
            $(rows).each(function (i, obj) {
            var distance =  $(this).find('[name="distance"]').val();
            var vendorid = $(this).find('#hdn_vendorid').val();
            var fromvendor = $(this).find('#hdn_vendordetails').val();
                if (distance != "") {
                    distance_branch_wise_details.push({ distance: distance, vendorname: vendorid, fromvendor: fromvendor });
                }
            });
            if (distance_branch_wise_details.length == 0) {
                alert("Please enter distance");
                return false;
            }
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_edit_vendor_distance_click', 'distance_branch_wise_details': distance_branch_wise_details };
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
        }
        function Clearvalues() {
            document.getElementById('slct_Batch').selectedIndex = 0;
            document.getElementById('slct_product').selectedIndex = 0;
            document.getElementById('ddlsource').selectedIndex = 0;
            document.getElementById('txt_qtyltrs').value = "";
            document.getElementById('txt_fat').value = "";
            document.getElementById('txt_snf').value = "";
            document.getElementById('txt_clr').value = "";
            document.getElementById('txt_receivedfilm').value = "";
            document.getElementById('txt_consumption').value = "";
            document.getElementById('txt_returnfilm').value = "";
            document.getElementById('txt_cuttingfilm').value = "";
            document.getElementById('txt_Wastage').value = "";
        }
        var rendererOptions = {
            draggable: true
        };
        var directionsDisplay = new google.maps.DirectionsRenderer(rendererOptions); ;
        var directionsService = new google.maps.DirectionsService();
        var map;

        var australia = new google.maps.LatLng(-25.274398, 133.775136);

        function initialize() {
            var mapOptions = {
                zoom: 7,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                center: australia
            };
            map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);
            directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById('directionsPanel'));


            //            var control = document.getElementById('control');
            //            control.style.display = 'block';
            //            map.controls[google.maps.ControlPosition.TOP_CENTER].push(control);
        }
        var k = 0;

        function calcRoute(latlong1, latlong2,vendorname,vendorid) {
            var start = latlong1;
            var end = latlong2;
            var request = {
                origin: start,
                destination: end,
                travelMode: google.maps.DirectionsTravelMode.DRIVING
            };
            directionsService.route(request, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                    var distance = response.routes[0].legs[0].distance.text;
                    computeTotalDistance(directionsDisplay.directions, vendorname, vendorid);
                }
            });
            google.maps.event.addListener(directionsDisplay, 'directions_changed', function () {

            });
        }
        var totaldistancearray = [];
        function computeTotalDistance(result, vendorname, vendorid) {
            var total = 0;
            var myroute = result.routes[0];
            for (var i = 0; i < myroute.legs.length; i++) {
                total += myroute.legs[i].distance.value;
            }
            total = total / 1000.
            total = parseInt(total);
            totaldistancearray.push({ 'distance': total, 'name': vendorname, 'vendorid': vendorid });
            document.getElementById('total').innerHTML = total + ' km';
            getcal();
        }
        google.maps.event.addDomListener(window, 'load', initialize);

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Distance Calculation <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Distance Calculation</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Distance Calculation Details
                </h3>
            </div>
            <div style="padding-left: 410PX;">
                <table>
                    <tr>
                        <td>
                            <label>
                                Location Type<span style="color: red;">*</span></label>
                            <select id="slct_location" class="form-control" onchange="getlatlongdetails();">
                            </select>
                        </td>
                        <td style="display: none;">
                            <input type="text" id="txt_lat1" class="form-control" name="vendorcode">
                        </td>
                        <td style="display: none;">
                            <input type="text" id="txt_long1" class="form-control" name="vendorcode">
                        </td>
                        <td>
                            <label>
                                <span>&nbsp&nbsp </span>
                            </label>
                            <input id='btnget' type="button" class="btn btn-success" name="submit" value='Calculate Distance'
                                onclick="get_distance();" />
                        </td>
                    </tr>
                    <tr hidden>
                        <label id="lbl_sno">
                        </label>
                    </tr>
                    <tr>
                        <td style="display: none;">
                            <input type="text" id="txt_total" class="form-control" name="vendorcode">
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%;">
                <div id="mapcontent" style="display: none;">
                    <div id="googleMap" style="width: 70%; height: 100%; position: relative; background-color: rgb(229, 227, 223);">
                    </div>
                </div>
            </div>
            <div id="directionsPanel" style="float: right; width: 30%; height: 100%; overflow-x: auto;display:none;">
                <p>
                    Total Distance: <span id="total"></span>
                </p>
            </div>
            <div class="box-body">
                <div id="divFillScreen">
                </div>
            </div>
            <div style="padding-left: 410PX;">
                <input id='save_batchdetails' type="button" class="btn btn-success" name="submit"
                    value='Save' onclick="save_distance_section_click()" />
            </div>
        </div>
    </section>
</asp:Content>
