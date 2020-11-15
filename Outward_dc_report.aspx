<%@ Page Title="" Language="C#" MasterPageFile="~/Operations.master" AutoEventWireup="true" CodeFile="Outward_dc_report.aspx.cs" Inherits="Outward_dc_report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
    </script>
    <style type="text/css">
        .container
        {
            max-width: 100%;
        }
        th
        {
            text-align: center;
        }
        #config
        {
            overflow: auto;
            margin-bottom: 10px;
        }
        .config
        {
            float: left;
            width: 200px;
            height: 250px;
            border: 1px solid #000;
            margin-left: 10px;
        }
        .config .title
        {
            font-weight: bold;
            text-align: center;
        }
        .config .barcode2D, #miscCanvas
        {
            display: none;
        }
        #submit
        {
            clear: both;
        }
        #barcodeTarget, #canvasTarget
        {
            margin-top: 20px;
        }
    </style>
    <link href="http://www.jqueryscript.net/css/jquerysctipttop.css" rel="stylesheet"
        type="text/css">
    <script src="http://code.jquery.com/jquery-latest.min.js"></script>
    <script src="Barcode/jquery-barcode.js" type="text/javascript"></script>
    <script type="text/javascript">

        function generateBarcode(barname) {

            var Beforevalue = "SVDS/" + barname + "/";
            var value = Beforevalue + document.getElementById("<%= txt_refdcno.ClientID %>").value;
            var btype = $("input[name=btype]:checked").val();
            var renderer = $("input[name=renderer]:checked").val();

            var quietZone = false;
            if ($("#quietzone").is(':checked') || $("#quietzone").attr('checked')) {
                quietZone = true;
            }

            var settings = {
                output: renderer,
                bgColor: $("#bgColor").val(),
                color: $("#color").val(),
                barWidth: $("#barWidth").val(),
                barHeight: $("#barHeight").val(),
                moduleSize: $("#moduleSize").val(),
                posX: $("#posX").val(),
                posY: $("#posY").val(),
                addQuietZone: $("#quietZoneSize").val()
            };
            if ($("#rectangular").is(':checked') || $("#rectangular").attr('checked')) {
                value = { code: value, rect: true };
            }
            if (renderer == 'canvas') {
                clearCanvas();
                $("#barcodeTarget").hide();
                $("#canvasTarget").show().barcode(value, btype, settings);
            } else {
                $("#canvasTarget").hide();
                $("#barcodeTarget").html("").show().barcode(value, btype, settings);
            }
        }

        function showConfig1D() {
            $('.config .barcode1D').show();
            $('.config .barcode2D').hide();
        }

        function showConfig2D() {
            $('.config .barcode1D').hide();
            $('.config .barcode2D').show();
        }

        function clearCanvas() {
            var canvas = $('#canvasTarget').get(0);
            var ctx = canvas.getContext('2d');
            ctx.lineWidth = 1;
            ctx.lineCap = 'butt';
            ctx.fillStyle = '#FFFFFF';
            ctx.strokeStyle = '#000000';
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.strokeRect(0, 0, canvas.width, canvas.height);
        }

        $(function () {
            $('input[name=btype]').click(function () {
                if ($(this).attr('id') == 'datamatrix') showConfig2D(); else showConfig1D();
            });
            $('input[name=renderer]').click(function () {
                if ($(this).attr('id') == 'canvas') $('#miscCanvas').show(); else $('#miscCanvas').hide();
            });
            //        generateBarcode();
        });
  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div>
        <asp:UpdateProgress ID="updateProgress1" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                    right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                        Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div id="config" style="display: none;">
        <input type="radio" name="btype" id="code128" checked="checked" value="code128">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="content-header">
                <h1>
                    Despatch Report<small>Preview</small>
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
                    <li><a href="#">Despatch Report</a></li>
                </ol>
            </section>
            <div style="border: 1px solid #d5d5d5; margin-left: 6px; margin-top: 10px; margin-right: 5px;">
                <div>
                    <div runat="server" id="d">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblfromdate" runat="server">From Date : </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width:10px;">
                                </td>
                                <td>
                                    <asp:Label ID="lbltodate" runat="server">To Date : </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txttodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="todate_calendarextender" runat="server" Enabled="true"
                                        TargetControlID="txttodate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width:10px;">
                                </td>
                                <td>
                                    <asp:Button ID="btn_getdetails" Text="Get DC Details" runat="server" CssClass="btn btn-success"
                                        OnClick="btn_getdetails_Click" />
                                </td>
                            </tr>
                        </table>
                        <div id="divdcdata" align="center" style="height: 180px; width: 100%; text-align: center;
                            overflow: auto;">
                            <asp:GridView ID="Gridcdata" runat="server" ForeColor="White" Width="100%" GridLines="Both"
                                Font-Size="Smaller">
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" Font-Size="13px" HorizontalAlign="Center"
                                    ForeColor="Black" Font-Italic="False" Font-Names="Raavi" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                <AlternatingRowStyle HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                            </asp:GridView>
                            <asp:Label ID="lbldateValidation" runat="server" Font-Size="20px" ForeColor="Red"
                                Text=""></asp:Label>
                        </div>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <table id="tbltrip">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_tripid" runat="server">Transaction No : </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_refdcno" runat="server" CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <td style="width:10px;">
                                                    </td>
                                                <td>
                                                    <asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-success" OnClick="btnGenerate_Click"
                                                        Text="Generate" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br></br>
                        <asp:Panel ID="pnlHide" runat="server" Visible="false">
                            <div id="divPrint">
                                <div style="width: 100%;">
                                    <div align="center" style="font-weight: bold; text-decoration: underline;">
                                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="30px" ForeColor="#0252aa"
                                            Text="">DELIVERY CHALLAN</asp:Label>
                                        <br />
                                    </div>
                                    <div style="width: 13%; float: left;">
                                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi" width="90px" height="62px" />
                                    </div>
                                    <div style="float: right;">
                                        <div id="barcodeTarget" class="barcodeTarget">
                                        </div>
                                        <canvas id="canvasTarget" width="150" height="150">
                                        </canvas>
                                    </div>
                                    <div>
                                    </div>
                                    <div style="width: 100%;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 28%; padding-left: 10%;">
                                                    <span style="font-weight: bold;">Transaction No: </span>
                                                    <asp:Label ID="lblRefdcno" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="width: 22%; padding-left: 5%;">
                                                    <span style="font-weight: bold;">DC No: </span>
                                                    <asp:Label ID="lbldcno" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="width: 28%; padding-left: 10%;">
                                                    <span style="font-weight: bold;">Date:</span>
                                                    <asp:Label ID="lblassigndate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="width: 22%; padding-left: 5%;">
                                                    <span style="font-weight: bold;">Time:</span>
                                                    <asp:Label ID="lbldisptime" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 21%; padding-left: 10%;">
                                                <span style="font-weight: bold;">Vehicle No:</span>
                                                <asp:Label ID="lblvehicleno" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td style="width: 20%; padding-left: 3%;">
                                                <span style="font-weight: bold;">Distance:</span>
                                                <asp:Label ID="lblkms" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td style="width: 28%; padding-left: 1%;">
                                                <span style="font-weight: bold;">Reaching Time:</span>
                                                <asp:Label ID="lblexpected" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 62%; padding-left: 10%;">
                                                <span style="font-weight: bold;">From:</span>
                                                <asp:Label ID="lbl" runat="server" Text="" Font-Bold="true" Font-Size="16px"
                                                    ForeColor="Red">SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD</asp:Label>
                                            </td>
                                            <td style="width: 20%; padding-left: 0%;">
                                                <span style="font-weight: bold;">Tin No:</span>
                                                <asp:Label ID="lblTinNo" runat="server" Text="" Font-Bold="true" Font-Size="14px"
                                                    ForeColor="Red"></asp:Label>
                                            </td>
                                            <td style="width: 23%; padding-left: 2%;">
                                                <span style="font-weight: bold;">CST No:</span>
                                                <asp:Label ID="lblCSTNo" runat="server" Text="" Font-Bold="true" Font-Size="14px"
                                                    ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 63%; padding-left: 10%;">
                                                <span style="font-weight: bold;">To:</span>
                                                <asp:Label ID="lblpartyname" runat="server" Text="" Font-Bold="true" Font-Size="16px" ForeColor="Red"> </asp:Label>
                                            </td>
                                            <td style="width: 20%; padding-left: 0%;">
                                                <span style="font-weight: bold;">MIT No:</span>
                                                <asp:Label ID="lblMitno" runat="server" Text="" Font-Bold="true" Font-Size="14px"
                                                    ForeColor="Red"></asp:Label>
                                            </td>
                                              <td style="width: 23%; padding-left: 2%;">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:GridView ID="grdReports" runat="server" CellPadding="5" CellSpacing="5" CssClass="gridcls"
                                    ForeColor="White" GridLines="Both" Width="100%" Font-Size="Small">
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                    <HeaderStyle BackColor="#f4f4f4" Font-Bold="true" Font-Italic="False" Font-Names="Raavi"
                                        Font-Size="13px" ForeColor="Black" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                                <br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 33%;">
                                            <span style="font-weight: bold; font-size: 12px;">DISPATCHER SIGNATURE</span>
                                        </td>
                                        <td style="width: 33%;">
                                            <span style="font-weight: bold; font-size: 12px;">DRIVER SIGNATURE</span>
                                        </td>
                                        <td style="width: 33%;">
                                            <span style="font-weight: bold; font-size: 12px;">MANGER SIGNATURE</span>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <span>...................................................................................................................................................................................................................................................................................................................</span>
                                </div>
                                <div>
                                    Processing Plant
                                </div>
                                <table style="width: 100%;">
                                    <tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">loadeding Time:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                        <td rowspan="2" style="border: 2px solid gray;">
                                            Ref No:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Flow Meter Ltrs:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Temp:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label20" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Temp After Chiller:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Remarks:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label21" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="width: 50%;">
                                            <span style="font-weight: bold; font-size: 14px;">PROCESSING INCHARGE SIGNATURE</span>
                                        </td>
                                    </tr>
                                </table>
                                 <div>
                                    <span>...................................................................................................................................................................................................................................................................................................................</span>
                                </div>
                                <div>
                                    Sample
                                    <br />
                                </div>
                                <table style="width: 100%;">
                                    <td style="width: 30%; padding-left: 12%;">
                                        <span style="font-weight: bold;">Sample Time:</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                    </td>
                                     <td rowspan="3" colspan="2" >
                                            <asp:GridView ID="grdsample" runat="server" CellPadding="5" CellSpacing="5" CssClass="gridcls"
                                                ForeColor="White" GridLines="Both" Width="80%" Font-Size="Small">
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                                <HeaderStyle BackColor="#f4f4f4" Font-Bold="true" Font-Italic="False" Font-Names="Raavi"
                                                    Font-Size="13px" ForeColor="Black" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Sample Taken:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                        </td>
                                    </tr>
                                    <tr></tr>
                                    <tr></tr>
                                    <tr>
                                        <td style="padding-left: 12%;">
                                            <span style="font-weight: bold; font-size: 14px;">SIGNATURE</span>
                                        </td>
                                        <td style="width: 40%;">
                                            <span style="font-weight: bold; font-size: 14px;">SECURITY SIGNATURE</span>
                                        </td>
                                         <td  style="border: 2px solid gray;height:30px;">
                                        Ref No:
                                    </td>
                                    </tr>
                                </table>
                                <div>
                                    <span>...................................................................................................................................................................................................................................................................................................................</span>
                                </div>
                                <div>
                                    QC Lab
                                    <br />
                                </div>
                                <asp:GridView ID="grdempty" runat="server" CellPadding="5" CellSpacing="5" CssClass="gridcls"
                                    ForeColor="White" GridLines="Both" Width="100%" Font-Size="Small">
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                    <HeaderStyle BackColor="#f4f4f4" Font-Bold="true" Font-Italic="False" Font-Names="Raavi"
                                        Font-Size="13px" ForeColor="Black" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                                <br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Unloading Clearance By QC Time:</span>
                                            <br />
                                            <br />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                            <br />
                                        </td>
                                        <td rowspan="2" style="border: 2px solid gray;">
                                            Ref No:
                                        </td>
                                    </tr>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 12%;">
                                            <span style="font-weight: bold; font-size: 14px;">CHEMIST</span>
                                        </td>
                                        <td style="width: 50%;">
                                            <span style="font-weight: bold; font-size: 14px;">QCO</span>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <span>...................................................................................................................................................................................................................................................................................................................</span>
                                </div>
                                <div>
                                    Weigh Bridge
                                </div>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 30%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Wighment Time:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                        </td>
                                          <td rowspan="3" colspan="2" >
                                         <asp:GridView ID="grdweight" runat="server" CellPadding="5" CellSpacing="5" CssClass="gridcls"
                                    ForeColor="White" GridLines="Both" Width="80%" Font-Size="Small">
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                    <HeaderStyle BackColor="#f4f4f4" Font-Bold="true" Font-Italic="False" Font-Names="Raavi"
                                        Font-Size="13px" ForeColor="Black" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                        <td style="width: 30%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Gross Weight:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                      <tr>
                                        <td style="width: 30%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Tare Weight:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label24" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Remarks:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Action Taken:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                        </td>
                                         <td rowspan="2" style="border: 2px solid gray;">
                                            Ref No:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 12%;">
                                            <span style="font-weight: bold; font-size: 14px;">SIGNATURE</span>
                                        </td>
                                        <td >
                                            <span style="font-weight: bold; font-size: 14px;">CLEARED BY</span>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <span>...................................................................................................................................................................................................................................................................................................................</span>
                                </div>
                                <div>
                                    Security
                                </div>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 30%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Date:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                        </td>
                                        <td rowspan="2" style="border: 2px solid gray;">
                                            Ref No:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Remarks:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label23" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Arrival Time:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 12%;">
                                            <span style="font-weight: bold; font-size: 14px;">SECURITY SIGNATURE</span>
                                        </td>
                                        <td style="width: 50%;">
                                            <span style="font-weight: bold; font-size: 14px;">DRIVER SIGNATURE</span>
                                        </td>
                                    </tr>
                                </table>
                                
                                
                                
                              <%--  
                                <div>
                                    <span>...................................................................................................................................................................................................................................................................................................................</span>
                                </div>--%>
                                
                                <div>
                                    <span>...................................................................................................................................................................................................................................................................................................................</span>
                                </div>
                                <div>
                                    Accounts
                                    <br />
                                </div>
                                <table style="width: 100%;">
                                    <tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Rate On:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Calculation On:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Cost:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">SNF +/-:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Transport:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 38%; padding-left: 12%;">
                                            <span style="font-weight: bold;">Toatl Cost:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label19" runat="server" Text="" ForeColor="Red">......................................................</asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold; font-size: 14px;">RECEIVER'S SIGNATURE</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold; font-size: 14px;">ACCOUNTS</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold; font-size: 14px;">INCHARGE</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold; font-size: 14px;">MANAGER SIGNATURE</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <br />
                            <asp:Button ID="btnPrint" runat="Server" CssClass="btn btn-success" OnClientClick="javascript:CallPrint('divPrint');"
                                Text="Print" />
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblmsg" runat="server" Font-Size="20px" ForeColor="Red" Text=""></asp:Label>
                        </br> </br> </br> </br>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
