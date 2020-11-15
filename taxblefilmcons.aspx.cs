using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class taxblefilmcons : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
    PODbmanager pdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            BranchID = Session["Branch_ID"].ToString();
            vdm = new SalesDBManager();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    DateTime dt = DateTime.Now.AddDays(-1);
                    dtp_FromDate.Text = dt.ToString("dd-MM-yyyy");
                    dtp_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                }
            }
        }
    }
    private DateTime GetLowDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        DT = dt;
        Hour = -dt.Hour;
        Min = -dt.Minute;
        Sec = -dt.Second;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;
    }
    private DateTime GetHighDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        Hour = 23 - dt.Hour;
        Min = 59 - dt.Minute;
        Sec = 59 - dt.Second;
        DT = dt;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;
    }
    DataTable Report = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            SalesDBManager SalesDB = new SalesDBManager();
            PODbmanager PoDB = new PODbmanager();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] datestrig = dtp_FromDate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = dtp_Todate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            string Type = Session["BranchType"].ToString();
            Report.Columns.Add("sno");
            Report.Columns.Add("ProductId");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Received Film");
            Report.Columns.Add("Consumption Film");
            Report.Columns.Add("Return Film");
            Report.Columns.Add("Wastage Film");
            Report.Columns.Add("Cutting Film");
            Report.Columns.Add("PACKED");
            Report.Columns.Add("Qtyltrs");
            Report.Columns.Add("PER KG YIELD");
            Report.Columns.Add("RATE PER KG");
            Report.Columns.Add("TAX %");
            Report.Columns.Add("VALUE OF FILM");
            Report.Columns.Add("TAXABLE VALUE");
            Report.Columns.Add("PER PACKET RATE");
            Report.Columns.Add("RATE PER LTR");
            //packing section
            cmd = new SqlCommand("SELECT   SUM(packing_entry.qty_ltr) AS qty_ltr, SUM(packing_entry.received_film) AS received_film, SUM(packing_entry.cuttingfilm) AS cuttingfilm, SUM(packing_entry.consumption_film) AS consumption_film, SUM(packing_entry.return_film) AS return_film, SUM(packing_entry.wastage_film) AS wastage_film, productmaster.productname, packing_entry.productid, productmaster.ml, productmaster.filimrate, packing_entry.filmrate, batchmaster.batch, batchproducts_mapping.storesproductid FROM   packing_entry INNER JOIN batchmaster ON packing_entry.batchid = batchmaster.batchid INNER JOIN productmaster ON packing_entry.productid = productmaster.sno INNER JOIN batchproducts_mapping ON productmaster.sno = batchproducts_mapping.sno WHERE   (packing_entry.branchid = @BranchID) AND (packing_entry.doe BETWEEN @d1 AND @d2) AND (packing_entry.section = 'packing') AND (packing_entry.received_film <> 0) GROUP BY productmaster.productname, productmaster.ml, productmaster.filimrate, batchmaster.batch, packing_entry.productid, packing_entry.filmrate, batchproducts_mapping.storesproductid ORDER BY packing_entry.productid");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
            int i = 1;
            double wastagetotal = 0;
            double cuttingtotal = 0;
            double consumptiontotal = 0;
            double receivedtotal = 0;
            double returnfilmtotal = 0;
            double total100 = 0;
            double total200 = 0;
            double total250 = 0;
            double total500 = 0;
            double total1000 = 0;
            double totalmilkltrs = 0;
            double totalqty = 0;
            double avgmilkrate = 0;
            double totalfilmrate = 0;
            double totalrate = 0;
            double rateperkg = 0;
            double totalwastage = 0;
            double totalcuttingwastage = 0;
            double overallwastage = 0;
            if (dtPacking.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPacking.Rows)
                {
                    //film rate taken from stores
                    string storesproductid = dr["storesproductid"].ToString();
                    cmd = new SqlCommand("SELECT    productmoniter.productid, productmoniter.qty, productmoniter.price, productmoniter.branchid, productmoniter.minstock, productmoniter.maxstock, productmaster.IGST FROM    productmoniter INNER JOIN productmaster ON productmoniter.productid = productmaster.productid WHERE    (productmoniter.branchid = @pbranchid) AND (productmoniter.productid = @pproductid)");
                    cmd.Parameters.Add("@pbranchid", "2");
                    cmd.Parameters.Add("@pproductid", storesproductid);
                    DataTable dtfilmcost = PoDB.SelectQuery(cmd).Tables[0];
                    foreach (DataRow drf in dtfilmcost.Rows)
                    {
                        string mlltr = dr["ml"].ToString();
                        if (mlltr != "")
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            newrow["ProductId"] = dr["productid"].ToString();
                            newrow["Product Name"] = dr["productname"].ToString();
                            newrow["Received Film"] = dr["received_film"].ToString();
                            newrow["Consumption Film"] = dr["consumption_film"].ToString();
                            newrow["Return Film"] = dr["return_film"].ToString();
                            newrow["Wastage Film"] = dr["wastage_film"].ToString();
                            newrow["Cutting Film"] = dr["cuttingfilm"].ToString();
                            double qty = Convert.ToDouble(dr["qty_ltr"].ToString());
                            totalqty += qty;
                            newrow["Qtyltrs"] = qty.ToString();
                            double consumption = Convert.ToDouble(dr["consumption_film"].ToString());
                            double wastage = Convert.ToDouble(dr["wastage_film"].ToString());
                            string rate = drf["price"].ToString();
                            string tax = drf["IGST"].ToString();
                            if (tax == "")
                            {
                                tax = "0";
                            }
                            newrow["TAX %"] = tax.ToString();
                            double taxpercent = Convert.ToDouble(tax);
                            if (rate == "")
                            {
                                rateperkg = 0;
                                newrow["RATE PER KG"] = "0";
                            }
                            else
                            {
                                rateperkg = Convert.ToDouble(rate);
                                newrow["RATE PER KG"] = rateperkg;
                            }
                            totalrate += rateperkg;

                            int int_mlltr = 0;
                            int.TryParse(mlltr, out int_mlltr);
                            int noofqty = 0;
                            noofqty = 1000 / int_mlltr;
                            double packets = noofqty * qty;

                            double packed = noofqty * qty;  //packets
                            double sumfilm = consumption + wastage;
                            double packedyeld = packed / sumfilm;
                            packedyeld = Math.Round(packedyeld, 2);
                            newrow["PACKED"] = packed.ToString();
                            newrow["PER KG YIELD"] = packedyeld.ToString();
                            newrow["VALUE OF FILM"] = sumfilm * rateperkg;
                            double valueoffilm = sumfilm * rateperkg;
                            double taxvalue = (valueoffilm * taxpercent) / 100;  //tax value
                            newrow["TAXABLE VALUE"] = Math.Round(taxvalue, 2).ToString();
                            double valueoffilms = taxvalue + valueoffilm;
                            double perpktrate = valueoffilms / packed;
                            perpktrate = Math.Round(perpktrate, 2);
                            newrow["PER PACKET RATE"] = perpktrate;
                            double rateperltr = perpktrate * noofqty;  //rateper ltr
                            newrow["RATE PER LTR"] = rateperltr;
                            total200 += qty;

                            double twastage = 0;
                            double.TryParse(dr["wastage_film"].ToString(), out twastage);
                            wastagetotal += twastage;

                            double cutting = 0;
                            double.TryParse(dr["cuttingfilm"].ToString(), out cutting);
                            cuttingtotal += cutting;

                            double tconsumption = 0;
                            double.TryParse(dr["consumption_film"].ToString(), out tconsumption);
                            consumptiontotal += tconsumption;

                            double RecivedFilm = 0;
                            double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                            receivedtotal += RecivedFilm;

                            double ReturnFilm = 0;
                            double.TryParse(dr["return_film"].ToString(), out ReturnFilm);
                            returnfilmtotal += ReturnFilm;
                            Report.Rows.Add(newrow);
                        }
                    }
                }
            }
            //curd section taken film usage
            cmd = new SqlCommand("SELECT  cpm.productid, SUM(cpm.received_film) AS received_film, SUM(cpm.consumption_film) AS consumption_film, SUM(cpm.wastage_film) AS wastage_film, SUM(cpm.production) AS production,  SUM(cpm.approveproduction) AS approveproduction, pm.productname, pm.ml, pm.filimrate, batchproducts_mapping.storesproductid FROM  packing_entry AS cpm INNER JOIN productmaster AS pm ON pm.sno = cpm.productid INNER JOIN batchproducts_mapping ON pm.sno = batchproducts_mapping.productid WHERE  (cpm.branchid = @branchid) AND (cpm.doe BETWEEN @d1 AND @d2) AND (cpm.section = 'curd') AND (cpm.received_film > 0) GROUP BY pm.productname, cpm.productid, pm.ml, pm.filimrate, batchproducts_mapping.storesproductid");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtcurd = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtcurd.Rows.Count > 0)
            {
                foreach (DataRow dr in dtcurd.Rows)
                {
                    //film rate taken from stores
                    string storesproductid = dr["storesproductid"].ToString();
                    cmd = new SqlCommand("SELECT    productmoniter.productid, productmoniter.qty, productmoniter.price, productmoniter.branchid, productmoniter.minstock, productmoniter.maxstock, productmaster.IGST FROM    productmoniter INNER JOIN productmaster ON productmoniter.productid = productmaster.productid WHERE    (productmoniter.branchid = @pbranchid) AND (productmoniter.productid = @pproductid)");
                    cmd.Parameters.Add("@pbranchid", "2");
                    cmd.Parameters.Add("@pproductid", storesproductid);
                    DataTable dtfilmcost = PoDB.SelectQuery(cmd).Tables[0];
                    foreach (DataRow drf in dtfilmcost.Rows)
                    {
                        //production quantity taken from curd section production details
                        string prdid = dr["productid"].ToString();
                        cmd = new SqlCommand("SELECT   productid, SUM(productionqty) AS productionqty, SUM(qty_kgs) AS qty_kgs, branchid FROM   plant_production_details WHERE   (doe BETWEEN @d1 AND @d2) AND (branchid = @branchid) AND (productid <> '') AND (productionqty IS NOT NULL) AND (productid = @prodid) GROUP BY productid, branchid");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@branchid", BranchID);
                        cmd.Parameters.Add("@prodid", prdid);
                        DataTable dtcurdqty = SalesDB.SelectQuery(cmd).Tables[0];
                        if (dtcurdqty.Rows.Count > 0)
                        {
                            string mlltr = dr["ml"].ToString();
                            if (mlltr != "")
                            {
                                foreach (DataRow drr in dtcurdqty.Rows)
                                {
                                    DataRow newrow = Report.NewRow();
                                    newrow["Sno"] = i++.ToString();
                                    newrow["ProductId"] = dr["productid"].ToString();
                                    newrow["Product Name"] = dr["productname"].ToString();
                                    newrow["Received Film"] = dr["received_film"].ToString();

                                    newrow["Consumption Film"] = dr["consumption_film"].ToString();
                                    newrow["Return Film"] = "0";
                                    newrow["Wastage Film"] = dr["wastage_film"].ToString();
                                    newrow["Cutting Film"] = "0";
                                    double qty = Convert.ToDouble(drr["productionqty"].ToString());
                                    totalqty += qty;
                                    newrow["Qtyltrs"] = qty.ToString();
                                    double consumption = Convert.ToDouble(dr["consumption_film"].ToString());
                                    double wastage = Convert.ToDouble(dr["wastage_film"].ToString());
                                    string rate = drf["price"].ToString();
                                    string tax = drf["IGST"].ToString();
                                    if (tax == "")
                                    {
                                        tax = "0";
                                    }
                                    newrow["TAX %"] = tax.ToString();
                                    double taxpercent = Convert.ToDouble(tax);
                                    if (rate == "")
                                    {
                                        rateperkg = 0;
                                        newrow["RATE PER KG"] = "0";
                                    }
                                    else
                                    {
                                        rateperkg = Convert.ToDouble(rate);
                                        newrow["RATE PER KG"] = rateperkg;
                                    }
                                    totalrate += rateperkg;

                                    int int_mlltr = 0;
                                    int.TryParse(mlltr, out int_mlltr);
                                    int noofqty = 0;
                                    noofqty = 1000 / int_mlltr;
                                    double packets = noofqty * qty;

                                    double packed = noofqty * qty;  //packets
                                    double sumfilm = consumption + wastage;
                                    double packedyeld = packed / sumfilm;
                                    packedyeld = Math.Round(packedyeld, 2);
                                    newrow["PACKED"] = packed.ToString();
                                    newrow["PER KG YIELD"] = packedyeld.ToString();
                                    newrow["VALUE OF FILM"] = sumfilm * rateperkg;
                                    double valueoffilm = sumfilm * rateperkg;
                                    double taxvalue = (valueoffilm * taxpercent) / 100;  //tax value
                                    newrow["TAXABLE VALUE"] = Math.Round(taxvalue, 2).ToString();
                                    double valueoffilms = taxvalue + valueoffilm;
                                    double perpktrate = valueoffilms / packed;
                                    perpktrate = Math.Round(perpktrate, 2);
                                    newrow["PER PACKET RATE"] = perpktrate;
                                    double rateperltr = perpktrate * noofqty;  //rateper ltr
                                    newrow["RATE PER LTR"] = rateperltr;
                                    total200 += qty;

                                    double twastage = 0;
                                    double.TryParse(dr["wastage_film"].ToString(), out twastage);
                                    wastagetotal += twastage;

                                    double cutting = 0;
                                    double.TryParse("0", out cutting);
                                    cuttingtotal += cutting;

                                    double tconsumption = 0;
                                    double.TryParse(dr["consumption_film"].ToString(), out tconsumption);
                                    consumptiontotal += tconsumption;

                                    double RecivedFilm = 0;
                                    double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                                    receivedtotal += RecivedFilm;

                                    double ReturnFilm = 0;
                                    double.TryParse("0", out ReturnFilm);
                                    returnfilmtotal += ReturnFilm;
                                    Report.Rows.Add(newrow);
                                }
                            }
                        }
                    }
                }
            }
            int count = Report.Rows.Count;
            avgmilkrate = totalrate / count;
            totalmilkltrs = total200;
            DataRow newvartical2 = Report.NewRow();
            receivedtotal = Math.Round(receivedtotal, 2);
            consumptiontotal = Math.Round(consumptiontotal, 2);
            wastagetotal = Math.Round(wastagetotal, 2);
            cuttingtotal = Math.Round(cuttingtotal, 2);
            returnfilmtotal = Math.Round(returnfilmtotal, 2);
            totalmilkltrs = Math.Round(totalmilkltrs, 2);
            avgmilkrate = Math.Round(avgmilkrate, 2);
            newvartical2["Product Name"] = "Total";
            newvartical2["Qtyltrs"] = totalqty;
            newvartical2["Received Film"] = receivedtotal;
            newvartical2["Consumption Film"] = consumptiontotal;
            newvartical2["Wastage Film"] = wastagetotal;
            newvartical2["Cutting Film"] = cuttingtotal;
            newvartical2["Return Film"] = returnfilmtotal;
            newvartical2["PACKED"] = totalmilkltrs;
            newvartical2["RATE PER KG"] = avgmilkrate;
            Report.Rows.Add(newvartical2);

            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
            Session["finalize"] = Report;
            hidepanel.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }


}