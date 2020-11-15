<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="packingmaster.aspx.cs" Inherits="packingmaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">
     $(function ()
     {
         get_filim_details();
         $('#btn_addpack').click(function ()
            {
                $('#package_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_package').hide();
            });

            $('#btn_close').click(function ()
            {
                $('#package_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_package').show();
            });
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
        function Clearpackages()
        {
            document.getElementById('txt_length').value = "";
            document.getElementById('txt_weight').value = "";
            document.getElementById('txt_width').value = "";
            document.getElementById('txt_microns').value = "";
            document.getElementById('lbl_sno').innerHTML = "";
            document.getElementById('save_pack').value = "save";
        }
        function save_packing_details()
        {
            var weight = document.getElementById('txt_weight').value;
            var length = document.getElementById('txt_length').value;
            var width = document.getElementById('txt_width').value;
            var microns = document.getElementById('txt_microns').value;
            var btnval = document.getElementById('save_pack').value;
            var sno = document.getElementById('lbl_sno').value;
            var confi = confirm("Do you want to Save Transaction ?");
            if (confi) {
                var data = { 'op': 'save_packing_details', 'weight': weight, 'length': length, 'width': width, 'microns': microns, 'btnval': btnval, 'sno': sno };
                var s = function (msg)
                {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_package_details();
                            Clearpackages();
                            $('#div_package').show();
                            $('#package_fillform').css('display', 'none');
                            $('#showlogs').css('display', 'block');
                           
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
        function get_filim_details()
        {
            var data = { 'op': 'get_filim_details' };
            var s = function (msg)
            {
                if (msg) {
                    if (msg.length > 0) {
                        fillpackdetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e)
            {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillpackdetails(msg)
        {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col">Weight</th><th scope="col">Length</th><th scope="col">Width</th><th scope="col">Microns</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<th scope="row" class="1" style="text-align:center;">' + msg[i].weight + '</th>';
                results += '<td data-title="code" class="2">' + msg[i].length + '</td>';
                results += '<td data-title="code" class="3">' + msg[i].width + '</td>';
                results += '<td data-title="code" class="4">' + msg[i].microns + '</td>';
                results += '<td style="display:none" class="6">' + msg[i].sno + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_package").html(results);
        }
        function getme(thisid) {
            var weight = $(thisid).parent().parent().children('.1').html();
            var length = $(thisid).parent().parent().children('.2').html();
            var width = $(thisid).parent().parent().children('.3').html();
            var microns = $(thisid).parent().parent().children('.4').html();
            var sno = $(thisid).parent().parent().children('.6').html();

            document.getElementById('txt_weight').value = weight;
            document.getElementById('txt_length').value = length;
            document.getElementById('txt_width').value = width;
            document.getElementById('txt_microns').value = microns;

            document.getElementById('save_pack').value = "Modify";
            document.getElementById('lbl_sno').value = sno;
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <section class="content-header">
        <h1>
          Packing master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Packing master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Packing master Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addpack" type="button" name="submit" value='Add Packings'
                        class="btn btn-success" />
                </div>
                <div id="div_package">
                </div>
                <div id='package_fillform' style="display: none; padding-left:235px;">
                    <table align="center" >
                        <tr>
                            <td>
                                <label>
                                    Weight<span style="color: red;">*</span></label>
                                <input id="txt_weight" class="form-control" type="text" name="vendorcode"
                                placeholder="Enter Weight">
                            </td>
                               <td  style="width: 3px;"></td>
                            <td>
                                <label>
                                   Length</label>
                                <input id="txt_length" type="text" class="form-control" name="vendorcode" placeholder="Enter Length">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Width<span style="color: red;">*</span></label>
                                <input id="txt_width" type="text" class="form-control" name="vendorcode" placeholder="Enter Width">
                            </td>
                            <td  style="width: 3px;"></td>
                            <td>
                                <label>
                                    Microns<span style="color: red;">*</span></label>
                                <input id="txt_microns" type="text" class="form-control" name="vendorcode" placeholder="Enter Microns">
                            </td>
                        </tr>
                        <tr hidden>
                            <td>
                                <label id="lbl_sno">
                                </label>
                            </td>
                        </tr>
                      <tr style="height:5px;">
                      </tr>
                        <tr>
                            <td>
                                <input id='save_pack' type="button" class="btn btn-success" name="submit"
                                    value='Save' onclick="save_packing_details()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Clear" value='Clear'
                                    onclick="Clearpackages()" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
        </div>
        <div id="div_packagedetails"></div>
    </section>


</asp:Content>

