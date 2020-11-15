using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class xmlexport : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
    string xmlstc;
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void btn_Procurement_click(object sender, EventArgs e)
    {
        string connetionString = null;
        SqlConnection cnn;
        connetionString = "DataSource=223.196.32.28;Initial Catalog=AMPS;User ID=sa;Password=vyshnavi123";
        cnn = new SqlConnection(connetionString);
        try
        {
            cnn.Open();
            DataTable table = new DataTable();

            string str = "SELECT SUM(NetAmount) AS NetAmount,SUM(TotAdditions) AS TotAdditions,SUM(TotDeductions) AS TotDeductions FROM paymentdata Where Plant_code=165 AND Frm_date='02-11-2016' GROUP BY Route_id";

            DataRow row = table.Rows[0];

            cnn.Close();
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnexport_click(object sender, EventArgs e)
    {
       
        try
        {
            DataTable table = new DataTable();
            string connetionString = null;
            SqlConnection cnn;
            int flag = 1;
            connetionString = "SERVER=192.168.0.36;DATABASE=AMPS;User ID=sa;Password=vyshnavi123";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cammand = new SqlCommand("SELECT Rout.Route_id,Rout.Route_name,F.GMrate,F.GSmltr,F.GSAmt,F.GSInsentAmt,F.GRoundoff,F.GNetAmount FROM (SELECT Gpay.*,Gpay1.* FROM  (SELECT  Route_id,Plant_code,SUM(Smkg) AS GSmkg,SUM(Smltr) AS GSmltr,SUM(SAmt) AS GSAmt,CAST((SUM(SAmt)/SUM(Smltr)) AS DECIMAL(18,2)) AS GMrate,SUM(SInsentAmt) AS GSInsentAmt,SUM(Scaramt) AS GScaramt,SUM(SSplBonus) AS GSSplBonus,SUM(SAmt)+SUM(TotAdditions) AS GTotAdditions,CAST(((SUM(SAmt)+SUM(SInsentAmt)+SUM(Scaramt)+SUM(SSplBonus))/SUM(Smltr)) AS DECIMAL(18,2)) AS GArate,SUM(ClaimAount) AS GClaimAount,SUM(Sinstamt) AS GSinstamt,SUM(Billadv) AS GBilladv,SUM(Ai) AS GAi,SUM(Feed) AS GFeed,SUM(can) AS Gcan,SUM(Recovery) AS GRecv,SUM(others) AS GOthr,SUM(Roundoff) AS GRoundoff,SUM(FLOOR(NetAmount)) AS GNetAmount,CAST(((SUM(Sfatkg)*100)/SUM(Smkg)) AS DECIMAL(18,2)) AS GAfat,CAST(((SUM(SSnfkg)*100)/SUM(Smkg)) AS DECIMAL(18,2)) AS GAsnf,SUM(Sfatkg) AS GSfatkg,SUM(SSnfkg) AS GSSnfkg,CAST(((SUM(SAmt)+SUM(SInsentAmt)+SUM(Scaramt)+SUM(SSplBonus))/SUM(Sfatkg)) AS  DECIMAL(18,2)) AS GkgFatrate  FROM Paymentdata Where Plant_code='165' AND Frm_date='02-11-2016' AND To_date='02-20-2016' GROUP BY Route_id,Plant_code) AS Gpay  LEFT JOIN (SELECT  Plant_code AS Pcode,SUM(Smkg) AS GSmkg1,SUM(Smltr) AS GSmltr1,SUM(SAmt) AS GSAmt1,CAST((SUM(SAmt)/SUM(Smltr)) AS DECIMAL(18,2)) AS GMrate1,SUM(SInsentAmt) AS GSInsentAmt1,SUM(Scaramt) AS GScaramt1,SUM(SSplBonus) AS GSSplBonus1,SUM(SAmt)+SUM(TotAdditions) AS GTotAdditions1,CAST(((SUM(SAmt)+SUM(SInsentAmt)+SUM(Scaramt)+SUM(SSplBonus))/SUM(Smltr)) AS DECIMAL(18,2)) AS GArate1,SUM(ClaimAount) AS GClaimAount1,SUM(Sinstamt) AS GSinstamt1,SUM(Billadv) AS GBilladv1,SUM(Ai) AS GAi1,SUM(Feed) AS GFeed1,SUM(can) AS Gcan1,SUM(Recovery) AS GRecv1,SUM(others) AS GOthr1,SUM(Roundoff) AS GRoundoff1,SUM(FLOOR(NetAmount)) AS GNetAmount1,CAST(((SUM(Sfatkg)*100)/SUM(Smkg)) AS DECIMAL(18,2)) AS GAfat1,CAST(((SUM(SSnfkg)*100)/SUM(Smkg)) AS DECIMAL(18,2)) AS GAsnf1,SUM(Sfatkg) AS GSfatkg1,SUM(SSnfkg) AS GSSnfkg1,CAST(((SUM(SAmt)+SUM(SInsentAmt)+SUM(Scaramt)+SUM(SSplBonus))/SUM(Sfatkg)) AS  DECIMAL(18,2)) AS GkgFatrate1  FROM Paymentdata Where Plant_code='165' AND Frm_date='02-11-2016' AND To_date='02-20-2016' GROUP BY Plant_code) AS Gpay1 ON Gpay.Plant_code=Gpay1.Pcode ) AS F  INNER JOIN  (SELECT DISTINCT(Route_id),Route_name FROM Paymentdata Where Plant_code='165' AND Frm_date='02-11-2016' AND To_date='02-20-2016' ) AS Rout ON F.Route_id=Rout.Route_id ORDER BY Route_id", cnn);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cammand;
            sda.Fill(table);
            cnn.Close();
            //  xmlstc = xmlstc + "<VOUCHER VCHTYPE=" + "\"" + "Receipt" + "\" ACTION=" + "\"" + "Create" + "\">";
            xmlstc = xmlstc + "<VOUCHER VCHTYPE=" + "\"" + "Receipt" + "\" ACTION=" + "\"" + "Create\">";
            xmlstc = "<ENVELOPE>";
            xmlstc = xmlstc + "<HEADER>";
            xmlstc = xmlstc + "<TALLYREQUEST>Import Data</TALLYREQUEST>";
            xmlstc = xmlstc + "</HEADER>";
            xmlstc = xmlstc + "<BODY>";
            xmlstc = xmlstc + "<IMPORTDATA>";
            xmlstc = xmlstc + "<REQUESTDESC>";
            xmlstc = xmlstc + "<REPORTNAME>All Masters</REPORTNAME>";
            xmlstc = xmlstc + "<STATICVARIABLES>";
            xmlstc = xmlstc + "<SVCURRENTCOMPANY>" + "SVD.P.LTD,C.S.PURAM-(2014-2015)" + "</SVCURRENTCOMPANY>";
            xmlstc = xmlstc + "</STATICVARIABLES>";
            xmlstc = xmlstc + "</REQUESTDESC>";

            xmlstc = xmlstc + "<REQUESTDATA>";

            foreach (DataRow dr in table.Rows)
            {

                if (flag == 1)
                {
                    flag++;
                    string strrate = dr["GMrate"].ToString();
                    string strgsmltr = dr["GSmltr"].ToString();
                    string strAmount = dr["GSAmt"].ToString();
                    string strloanamount = dr["GSInsentAmt"].ToString();
                    string roundoff = dr["GRoundoff"].ToString();
                    string strtotalamount = dr["GNetAmount"].ToString();

                    string date = "20160228";
                    string gid = "40cc2b84-29eb-49e4-b485-8e5ee5536f4e-00001933";
                    string NARRATION = "Being the purchase of raw milk from Vidavaluru route for the period  Feb 11-20, total qty in kgs, 12526/12161.17/6.17/7.88/772.78/529.62";
                    string PARTYNAME = "Vidavaluru Route Milk Bills";
                    string VOUCHERTYPENAME = "Purchase";
                    string REFERENCE = "VIDAVALURU";
                    string VOUCHERNUMBER = "187";
                    string PARTYLEDGERNAME = "Vidavaluru Route Milk Bills-Csp";
                    string BASICBASEPARTYNAME = "Vidavaluru Route Milk Bills";
                    string FBTPAYMENTTYPE = "Default";
                    string PERSISTEDVIEW = "Accounting Voucher View";
                    string BASICDATETIMEOFINVOICE = "20-Aug-2015 at 11:15";
                    string BASICDATETIMEOFREMOVAL = "20-Aug-2015 at 11:15";
                    string ALTERID = "8000";
                    string MASTERID = "6451";
                    string VOUCHERKEY = "182188217729064";
                    string ledgername = "Vidavaluru Route Milk Bills-Csp";
                    string stackitem = "Raw Milk";
                    string godownname = "Main Location";
                    string batchname = "Primary Batch";
                    string enterby = "lakshmi";
                    string No = "No";
                    string Yes = "Yes";
                    xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF='TallyUDF'>";
                    xmlstc = xmlstc + "<VOUCHER VCHTYPE=" + "\"" + "Purchase" + "\" ACTION=" + "\"" + "Create" + "\">";
                    xmlstc = xmlstc + "<OLDAUDITENTRYIDS.LIST TYPE='Number'>";
                    xmlstc = xmlstc + "<OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>";
                    xmlstc = xmlstc + "</OLDAUDITENTRYIDS.LIST>";
                    //Header Values
                    xmlstc = xmlstc + "<DATE>" + date + "</DATE>";
                    xmlstc = xmlstc + "<REFERENCEDATE>" + date + "</REFERENCEDATE>";
                    xmlstc = xmlstc + "<GUID>" + gid + "</GUID>";
                    xmlstc = xmlstc + "<NARRATION>" + NARRATION + "</NARRATION>";
                    xmlstc = xmlstc + "<PARTYNAME>" + PARTYNAME + "</PARTYNAME>";
                    xmlstc = xmlstc + "<VOUCHERTYPENAME>" + VOUCHERTYPENAME + "</VOUCHERTYPENAME>";
                    xmlstc = xmlstc + "<REFERENCE>" + REFERENCE + "</REFERENCE>";
                    xmlstc = xmlstc + "<VOUCHERNUMBER>" + VOUCHERNUMBER + "</VOUCHERNUMBER>";
                    xmlstc = xmlstc + "<PARTYLEDGERNAME>" + PARTYLEDGERNAME + "</PARTYLEDGERNAME>";
                    xmlstc = xmlstc + "<BASICBASEPARTYNAME>" + BASICBASEPARTYNAME + "</BASICBASEPARTYNAME>";
                    xmlstc = xmlstc + "<CSTFORMISSUETYPE/>";
                    xmlstc = xmlstc + "<CSTFORMRECVTYPE/>";
                    xmlstc = xmlstc + "<FBTPAYMENTTYPE>" + FBTPAYMENTTYPE + "</FBTPAYMENTTYPE>";
                    xmlstc = xmlstc + "<PERSISTEDVIEW>" + PERSISTEDVIEW + "</PERSISTEDVIEW>";
                    xmlstc = xmlstc + "<BASICDATETIMEOFINVOICE>" + BASICDATETIMEOFINVOICE + "</BASICDATETIMEOFINVOICE>";
                    xmlstc = xmlstc + "<BASICDATETIMEOFREMOVAL>" + BASICDATETIMEOFREMOVAL + "</BASICDATETIMEOFREMOVAL>";
                    xmlstc = xmlstc + "<VCHGSTCLASS/>";
                    xmlstc = xmlstc + "<ENTEREDBY>" + enterby + "</ENTEREDBY>";
                    xmlstc = xmlstc + "<DIFFACTUALQTY>" + No + "</DIFFACTUALQTY>";
                    xmlstc = xmlstc + "<ISMSTFROMSYNC>" + No + "</ISMSTFROMSYNC>";
                    xmlstc = xmlstc + "<ASORIGINAL>" + No + "</ASORIGINAL>";
                    xmlstc = xmlstc + "<AUDITED>" + No + "</AUDITED>";
                    xmlstc = xmlstc + "<FORJOBCOSTING>" + No + "</FORJOBCOSTING>";
                    xmlstc = xmlstc + "<ISOPTIONAL>" + No + "</ISOPTIONAL>";
                    xmlstc = xmlstc + "<EFFECTIVEDATE>" + date + "</EFFECTIVEDATE>";
                    xmlstc = xmlstc + "<USEFOREXCISE>" + No + "</USEFOREXCISE>";
                    xmlstc = xmlstc + "<ISFORJOBWORKIN>" + No + "</ISFORJOBWORKIN>";
                    xmlstc = xmlstc + "<ALLOWCONSUMPTION>" + No + "</ALLOWCONSUMPTION>";
                    xmlstc = xmlstc + "<USEFORINTEREST>" + No + "</USEFORINTEREST>";
                    xmlstc = xmlstc + "<USEFORGAINLOSS>" + No + "</USEFORGAINLOSS>";
                    xmlstc = xmlstc + "<USEFORGODOWNTRANSFER>" + No + "</USEFORGODOWNTRANSFER>";
                    xmlstc = xmlstc + "<USEFORCOMPOUND>" + No + "</USEFORCOMPOUND>";
                    xmlstc = xmlstc + "<USEFORSERVICETAX>" + No + "</USEFORSERVICETAX>";
                    xmlstc = xmlstc + "<ISEXCISEVOUCHER>" + No + "</ISEXCISEVOUCHER>";
                    xmlstc = xmlstc + "<EXCISETAXOVERRIDE>" + No + "</EXCISETAXOVERRIDE>";
                    xmlstc = xmlstc + "<USEFORTAXUNITTRANSFER>" + No + "</USEFORTAXUNITTRANSFER>";
                    xmlstc = xmlstc + "<EXCISEOPENING>" + No + "</EXCISEOPENING>";
                    xmlstc = xmlstc + "<USEFORFINALPRODUCTION>" + No + "</USEFORFINALPRODUCTION>";
                    xmlstc = xmlstc + "<ISTDSOVERRIDDEN>" + No + "</ISTDSOVERRIDDEN>";
                    xmlstc = xmlstc + "<ISTCSOVERRIDDEN>" + No + "</ISTCSOVERRIDDEN>";
                    xmlstc = xmlstc + "<ISTDSTCSCASHVCH>" + No + "</ISTDSTCSCASHVCH>";
                    xmlstc = xmlstc + "<INCLUDEADVPYMTVCH>" + No + "</INCLUDEADVPYMTVCH>";
                    xmlstc = xmlstc + "<ISSUBWORKSCONTRACT>" + No + "</ISSUBWORKSCONTRACT>";
                    xmlstc = xmlstc + "<ISVATOVERRIDDEN>" + No + "</ISVATOVERRIDDEN>";
                    xmlstc = xmlstc + "<IGNOREORIGVCHDATE>" + No + "</IGNOREORIGVCHDATE>";
                    xmlstc = xmlstc + "<ISSERVICETAXOVERRIDDEN>" + No + "</ISSERVICETAXOVERRIDDEN>";
                    xmlstc = xmlstc + "<ISISDVOUCHER>" + No + "</ISISDVOUCHER>";
                    xmlstc = xmlstc + "<ISEXCISEOVERRIDDEN>" + No + "</ISEXCISEOVERRIDDEN>";
                    xmlstc = xmlstc + "<ISEXCISESUPPLYVCH>" + No + "</ISEXCISESUPPLYVCH>";
                    xmlstc = xmlstc + "<ISVATPRINCIPALACCOUNT>" + No + "</ISVATPRINCIPALACCOUNT>";
                    xmlstc = xmlstc + "<ISSHIPPINGWITHINSTATE>" + No + "</ISSHIPPINGWITHINSTATE>";
                    xmlstc = xmlstc + "<ISCANCELLED>" + No + "</ISCANCELLED>";
                    xmlstc = xmlstc + "<HASCASHFLOW>" + No + "</HASCASHFLOW>";
                    xmlstc = xmlstc + "<ISPOSTDATED>" + No + "</ISPOSTDATED>";
                    xmlstc = xmlstc + "<USETRACKINGNUMBER>" + No + "</USETRACKINGNUMBER>";
                    xmlstc = xmlstc + "<ISINVOICE>" + No + "</ISINVOICE>";
                    xmlstc = xmlstc + "<MFGJOURNAL>" + No + "</MFGJOURNAL>";
                    xmlstc = xmlstc + "<HASDISCOUNTS>" + No + "</HASDISCOUNTS>";
                    xmlstc = xmlstc + "<ASPAYSLIP>" + No + "</ASPAYSLIP>";
                    xmlstc = xmlstc + "<ISCOSTCENTRE>" + No + "</ISCOSTCENTRE>";
                    xmlstc = xmlstc + "<ISSTXNONREALIZEDVCH>" + No + "</ISSTXNONREALIZEDVCH>";
                    xmlstc = xmlstc + "<ISEXCISEMANUFACTURERON>" + No + "</ISEXCISEMANUFACTURERON>";
                    xmlstc = xmlstc + "<ISBLANKCHEQUE>" + No + "</ISBLANKCHEQUE>";
                    xmlstc = xmlstc + "<ISVOID>" + No + "</ISVOID>";
                    xmlstc = xmlstc + "<ISONHOLD>" + No + "</ISONHOLD>";
                    xmlstc = xmlstc + "<ORDERLINESTATUS>" + No + "</ORDERLINESTATUS>";
                    xmlstc = xmlstc + "<VATISAGNSTCANCSALES>" + No + "</VATISAGNSTCANCSALES>";
                    xmlstc = xmlstc + "<VATISPURCEXEMPTED>" + No + "</VATISPURCEXEMPTED>";
                    xmlstc = xmlstc + "<ISVATRESTAXINVOICE>" + No + "</ISVATRESTAXINVOICE>";
                    xmlstc = xmlstc + "<ISDELETED>" + No + "</ISDELETED>";
                    xmlstc = xmlstc + "<CHANGEVCHMODE>" + No + "</CHANGEVCHMODE>";
                    xmlstc = xmlstc + "<ALTERID> " + ALTERID + "</ALTERID>";
                    xmlstc = xmlstc + "<MASTERID> " + MASTERID + "</MASTERID>";
                    xmlstc = xmlstc + "<VOUCHERKEY>" + VOUCHERKEY + "</VOUCHERKEY>";
                    xmlstc = xmlstc + "<EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>";
                    xmlstc = xmlstc + "<OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>";
                    xmlstc = xmlstc + "<ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>";
                    xmlstc = xmlstc + "<AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>";
                    xmlstc = xmlstc + "<DUTYHEADDETAILS.LIST>      </DUTYHEADDETAILS.LIST>";
                    xmlstc = xmlstc + "<SUPPLEMENTARYDUTYHEADDETAILS.LIST>      </SUPPLEMENTARYDUTYHEADDETAILS.LIST>";
                    xmlstc = xmlstc + "<INVOICEDELNOTES.LIST>      </INVOICEDELNOTES.LIST>";
                    xmlstc = xmlstc + "<INVOICEORDERLIST.LIST>      </INVOICEORDERLIST.LIST>";
                    xmlstc = xmlstc + "<INVOICEINDENTLIST.LIST>      </INVOICEINDENTLIST.LIST>";
                    xmlstc = xmlstc + "<ATTENDANCEENTRIES.LIST>      </ATTENDANCEENTRIES.LIST>";
                    xmlstc = xmlstc + "<ORIGINVOICEDETAILS.LIST>      </ORIGINVOICEDETAILS.LIST>";
                    xmlstc = xmlstc + "<INVOICEEXPORTLIST.LIST>      </INVOICEEXPORTLIST.LIST>";
                    //Credit Ledger
                    xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>";
                    xmlstc = xmlstc + "<OLDAUDITENTRYIDS.LIST TYPE='Number'>";
                    xmlstc = xmlstc + "<OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>";
                    xmlstc = xmlstc + "</OLDAUDITENTRYIDS.LIST>";
                    xmlstc = xmlstc + "<LEDGERNAME>" + ledgername + "</LEDGERNAME>";
                    xmlstc = xmlstc + "<GSTCLASS/>";
                    xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>" + No + "</ISDEEMEDPOSITIVE>";
                    xmlstc = xmlstc + "<LEDGERFROMITEM>" + No + "</LEDGERFROMITEM>";
                    xmlstc = xmlstc + "<REMOVEZEROENTRIES>" + No + "</REMOVEZEROENTRIES>";
                    xmlstc = xmlstc + "<ISPARTYLEDGER>" + Yes + "</ISPARTYLEDGER>";
                    xmlstc = xmlstc + "<ISLASTDEEMEDPOSITIVE>" + No + "</ISLASTDEEMEDPOSITIVE>";
                    xmlstc = xmlstc + "<AMOUNT>" + strAmount + "</AMOUNT>";
                    xmlstc = xmlstc + "<VATEXPAMOUNT>" + strAmount + "</VATEXPAMOUNT>";
                    xmlstc = xmlstc + "<SERVICETAXDETAILS.LIST>       </SERVICETAXDETAILS.LIST>";
                    xmlstc = xmlstc + "<BANKALLOCATIONS.LIST>       </BANKALLOCATIONS.LIST>";
                    xmlstc = xmlstc + "<BILLALLOCATIONS.LIST>       </BILLALLOCATIONS.LIST>";
                    xmlstc = xmlstc + "<INTERESTCOLLECTION.LIST>       </INTERESTCOLLECTION.LIST>";
                    xmlstc = xmlstc + "<OLDAUDITENTRIES.LIST>       </OLDAUDITENTRIES.LIST>";
                    xmlstc = xmlstc + "<ACCOUNTAUDITENTRIES.LIST>       </ACCOUNTAUDITENTRIES.LIST>";
                    xmlstc = xmlstc + "<AUDITENTRIES.LIST>       </AUDITENTRIES.LIST>";
                    xmlstc = xmlstc + "<INPUTCRALLOCS.LIST>       </INPUTCRALLOCS.LIST>";
                    if (stackitem == null)
                    {
                        //xmlstc = xmlstc + "<INVENTORYALLOCATIONS.LIST>       </INVENTORYALLOCATIONS.LIST>";
                    }
                    else
                    {
                        xmlstc = xmlstc + "<INVENTORYALLOCATIONS.LIST>";
                        xmlstc = xmlstc + "<STOCKITEMNAME>" + stackitem + "</STOCKITEMNAME>";
                        xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>" + Yes + "</ISDEEMEDPOSITIVE>";
                        xmlstc = xmlstc + "<ISLASTDEEMEDPOSITIVE>" + Yes + "</ISLASTDEEMEDPOSITIVE>";
                        xmlstc = xmlstc + "<ISAUTONEGATE>" + No + "</ISAUTONEGATE>";
                        xmlstc = xmlstc + "<ISCUSTOMSCLEARANCE>" + No + "</ISCUSTOMSCLEARANCE>";
                        xmlstc = xmlstc + "<ISTRACKCOMPONENT>" + No + "</ISTRACKCOMPONENT>";
                        xmlstc = xmlstc + "<ISTRACKPRODUCTION>" + No + "</ISTRACKPRODUCTION>";
                        xmlstc = xmlstc + "<ISPRIMARYITEM>" + No + "</ISPRIMARYITEM>";
                        xmlstc = xmlstc + "<ISSCRAP>" + No + "</ISSCRAP>";
                        xmlstc = xmlstc + "<RATE>" + strrate + "</RATE>";
                        xmlstc = xmlstc + "<AMOUNT>" + strAmount + "</AMOUNT>";
                        xmlstc = xmlstc + "<ACTUALQTY>" + strgsmltr + "</ACTUALQTY>";
                        xmlstc = xmlstc + "<BILLEDQTY>" + strgsmltr + "</BILLEDQTY>";
                        xmlstc = xmlstc + "<BATCHALLOCATIONS.LIST>";
                        xmlstc = xmlstc + "<GODOWNNAME>" + godownname + "</GODOWNNAME>";
                        xmlstc = xmlstc + "<BATCHNAME>" + batchname + "</BATCHNAME>";
                        xmlstc = xmlstc + "<INDENTNO/>";
                        xmlstc = xmlstc + "<ORDERNO/>";
                        xmlstc = xmlstc + "<TRACKINGNUMBER/>";
                        xmlstc = xmlstc + "<DYNAMICCSTISCLEARED>" + No + "</DYNAMICCSTISCLEARED>";
                        xmlstc = xmlstc + "<AMOUNT>" + strAmount + "</AMOUNT>";
                        xmlstc = xmlstc + "<ACTUALQTY>" + strgsmltr + "</ACTUALQTY>";
                        xmlstc = xmlstc + "<BILLEDQTY>" + strgsmltr + "</BILLEDQTY>";
                        xmlstc = xmlstc + "<ADDITIONALDETAILS.LIST>         </ADDITIONALDETAILS.LIST>";
                        xmlstc = xmlstc + "<VOUCHERCOMPONENTLIST.LIST>         </VOUCHERCOMPONENTLIST.LIST>";
                        xmlstc = xmlstc + "</BATCHALLOCATIONS.LIST>";
                        xmlstc = xmlstc + "<DUTYHEADDETAILS.LIST>        </DUTYHEADDETAILS.LIST>";
                        xmlstc = xmlstc + "<SUPPLEMENTARYDUTYHEADDETAILS.LIST>        </SUPPLEMENTARYDUTYHEADDETAILS.LIST>";
                        xmlstc = xmlstc + "<TAXOBJECTALLOCATIONS.LIST>        </TAXOBJECTALLOCATIONS.LIST>";
                        xmlstc = xmlstc + "<COSTTRACKALLOCATIONS.LIST>        </COSTTRACKALLOCATIONS.LIST>";
                        xmlstc = xmlstc + "<REFVOUCHERDETAILS.LIST>        </REFVOUCHERDETAILS.LIST>";
                        xmlstc = xmlstc + "<EXCISEALLOCATIONS.LIST>        </EXCISEALLOCATIONS.LIST>";
                        xmlstc = xmlstc + "<EXPENSEALLOCATIONS.LIST>        </EXPENSEALLOCATIONS.LIST>";
                        xmlstc = xmlstc + "</INVENTORYALLOCATIONS.LIST>";
                    }
                   
                    xmlstc = xmlstc + "<DUTYHEADDETAILS.LIST>       </DUTYHEADDETAILS.LIST>";
                    xmlstc = xmlstc + "<EXCISEDUTYHEADDETAILS.LIST>       </EXCISEDUTYHEADDETAILS.LIST>";
                    xmlstc = xmlstc + "<SUMMARYALLOCS.LIST>       </SUMMARYALLOCS.LIST>";
                    xmlstc = xmlstc + "<STPYMTDETAILS.LIST>       </STPYMTDETAILS.LIST>";
                    xmlstc = xmlstc + "<EXCISEPAYMENTALLOCATIONS.LIST>       </EXCISEPAYMENTALLOCATIONS.LIST>";
                    xmlstc = xmlstc + "<TAXBILLALLOCATIONS.LIST>       </TAXBILLALLOCATIONS.LIST>";
                    xmlstc = xmlstc + "<TAXOBJECTALLOCATIONS.LIST>       </TAXOBJECTALLOCATIONS.LIST>";
                    xmlstc = xmlstc + "<TDSEXPENSEALLOCATIONS.LIST>       </TDSEXPENSEALLOCATIONS.LIST>";
                    xmlstc = xmlstc + "<VATSTATUTORYDETAILS.LIST>       </VATSTATUTORYDETAILS.LIST>";
                    xmlstc = xmlstc + "<REFVOUCHERDETAILS.LIST>       </REFVOUCHERDETAILS.LIST>";
                    xmlstc = xmlstc + "<INVOICEWISEDETAILS.LIST>       </INVOICEWISEDETAILS.LIST>";
                    xmlstc = xmlstc + "<VATITCDETAILS.LIST>       </VATITCDETAILS.LIST>";
                    xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>";
                    xmlstc = xmlstc + "</VOUCHER>";
                    xmlstc = xmlstc + "</TALLYMESSAGE>";
                }
            }
            xmlstc = xmlstc + "</REQUESTDATA>";
            xmlstc = xmlstc + "</IMPORTDATA>";
            xmlstc = xmlstc + "</BODY>";
            xmlstc = xmlstc + "</ENVELOPE>";
            StringWriter sw = new StringWriter();
            sw.Write(xmlstc);
            string s = sw.ToString();
            string attachment = "attachment; filename=purchasetest.xml";
            Response.ClearContent();
            Response.ContentType = "application/xml";
            Response.AddHeader("content-disposition", attachment);
            Response.Write(s);
            Response.End();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}