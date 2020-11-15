<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="GetDirections.aspx.cs" Inherits="GetDirections" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <style>
        html, body, #map-canvas
        {
            height: 100%;
            margin: 0px;
            padding: 0px;
        }
        #control
        {
            background: #fff;
            padding: 5px;
            font-size: 14px;
            font-family: Arial;
            border: 1px solid #ccc;
            box-shadow: 0 2px 2px rgba(33, 33, 33, 0.4);
            display: none;
        }
           div#mapcontent
        {
            right: 0;
            bottom: 0;
            left: 0px;
            top: 100px;
            overflow: hidden;
            position: absolute;
        }
    </style>
    <script src="js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script type="text/javascript">
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

            google.maps.event.addListener(directionsDisplay, 'directions_changed', function () {
                computeTotalDistance(directionsDisplay.directions);
            });
            var control = document.getElementById('control');
            control.style.display = 'block';
            map.controls[google.maps.ControlPosition.TOP_CENTER].push(control);
            calcRoute();
        }

        function calcRoute() {
            var start = document.getElementById('start').value;
            var end = document.getElementById('end').value;
            var request = {
                origin: start,
                destination: end,
                travelMode: google.maps.DirectionsTravelMode.DRIVING
            };
            directionsService.route(request, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                }
            });
        }

        function computeTotalDistance(result) {
            var total = 0;
            var myroute = result.routes[0];
            for (var i = 0; i < myroute.legs.length; i++) {
                total += myroute.legs[i].distance.value;
            }
            total = total / 1000.
            document.getElementById('total').innerHTML = total + ' km';
        }

        google.maps.event.addDomListener(window, 'load', initialize);

    </script>
    <script type="text/javascript">
        $(function () {
            get_vendors();
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
        var Locationsdata = [];
        function fillvedors(data) {
            Locationsdata = data;
            var start = document.getElementById('start');
            var end = document.getElementById('end');
            for (var i = 0; i < data.length; i++) {
                var opt = document.createElement('option');
                opt.innerHTML = data[i].vendorname;
                opt.value = data[i].latitude + "," + data[i].longitude;
                start.appendChild(opt);
                var opt1 = document.createElement('option');
                opt1.innerHTML = data[i].vendorname;
                opt1.value = data[i].latitude + "," + data[i].longitude;
                end.appendChild(opt1);
            }
            var key = 'Locations';
            var temp = location.search.match(new RegExp(key + "=(.*?)($|\&)", "i"));
            if (temp === null || temp === typeof undefined) {
            }
            else {
                var locations = temp[1].split('@');
                document.getElementById('start').value = locations[0];
                document.getElementById('end').value = locations[1];
                calcRoute();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="control">
        <strong>Start:</strong>
        <select id="start"  onchange="calcRoute();" class="form-control" >
        </select>
        <strong>End:</strong>
        <select id="end" onchange="calcRoute();" class="form-control" >
        </select>
    </div>
    <div style="width: 100%;">
            <div id="mapcontent">
                <div id="googleMap" style="width: 70%; height: 100%; position: relative; background-color: rgb(229, 227, 223);">
                </div>
            </div>
        </div>
    <div id="directionsPanel" style="float: right; width: 30%; height: 100%; overflow-x: auto;">
        <p>
            Total Distance: <span id="total"></span>
        </p>
    </div>
</asp:Content>

