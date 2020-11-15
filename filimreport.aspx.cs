using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class filimreport : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
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
            Report.Columns.Add("VALUE OF FILM");
            Report.Columns.Add("PER PACKET RATE");
            Report.Columns.Add("RATE PER LTR");
            //Report.Columns.Add("productid")
           // cmd = new SqlCommand("SELECT batchmaster.batch, packing_entry.qty_ltr, packing_entry.fat, packing_entry.snf, packing_entry.clr, packing_entry.received_film, packing_entry.cuttingfilm, packing_entry.consumption_film, packing_entry.return_film, packing_entry.doe, packing_entry.wastage_film, productmaster.productname, productmaster.ml, productmaster.filimrate FROM packing_entry INNER JOIN batchmaster ON packing_entry.batchid = batchmaster.batchid INNER JOIN productmaster ON packing_entry.productid = productmaster.sno WHERE (packing_entry.doe BETWEEN @d1 AND @d2) AND (packing_entry.branchid = @BranchID) AND (packing_entry.received_film <> 0) ORDER BY packing_entry.doe");
            cmd = new SqlCommand("SELECT SUM(packing_entry.qty_ltr) AS qty_ltr, SUM(packing_entry.received_film) AS received_film, SUM(packing_entry.cuttingfilm) AS cuttingfilm, SUM(packing_entry.consumption_film) AS consumption_film, SUM(packing_entry.return_film) AS return_film, SUM(packing_entry.wastage_film) AS wastage_film, productmaster.productname, packing_entry.productid, productmaster.ml, productmaster.filimrate,packing_entry.filmrate, batchmaster.batch FROM  packing_entry INNER JOIN batchmaster ON packing_entry.batchid = batchmaster.batchid INNER JOIN productmaster ON packing_entry.productid = productmaster.sno WHERE (packing_entry.doe BETWEEN @d1 AND @d2) AND (packing_entry.branchid = @BranchID) AND (packing_entry.section='packing') AND (packing_entry.received_film <> 0) GROUP BY productmaster.productname, productmaster.ml, productmaster.filimrate, batchmaster.batch,  packing_entry.productid,packing_entry.filmrate order by  packing_entry.productid");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtPacking.Rows.Count > 0)
            {
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
                foreach (DataRow dr in dtPacking.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    //DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    //string date = dtdoe.ToString("dd/MM/yyyy");
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
                    string rate = dr["filimrate"].ToString();
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
                    string mlltr = dr["ml"].ToString();
                    //if (mlltr == "100")
                    //{
                    double milkltrs = Convert.ToDouble(mlltr);
                    double packetsperltr = 1000 / milkltrs;
                    double packed = Math.Round(packetsperltr * qty);
                    double pkdby = packed / 10;
                    double sumfilm = consumption + wastage;
                    double packedyeld = packed / sumfilm;
                    packedyeld = Math.Round(packedyeld, 2);
                    newrow["PACKED"] = packed.ToString();
                    newrow["PER KG YIELD"] = packedyeld.ToString();
                    newrow["VALUE OF FILM"] = sumfilm * rateperkg;
                    double valueoffilm = sumfilm * rateperkg;
                    double perpktrate = valueoffilm / packed;
                    perpktrate = Math.Round(perpktrate, 2);
                    newrow["PER PACKET RATE"] = perpktrate;
                    double rateperltr = perpktrate * packetsperltr;
                    newrow["RATE PER LTR"] = rateperltr;
                    total100 += qty;
                    //}
                    //if (mlltr=="100")
                    //{
                    //    double packed = 10 * qty;
                    //    double pkdby = packed / 10;
                    //    double sumfilm = consumption + wastage;
                    //    double packedyeld = packed / sumfilm;
                    //    packedyeld = Math.Round(packedyeld, 2);
                    //    newrow["PACKED"] = packed.ToString();
                    //    newrow["PER KG YIELD"] = packedyeld.ToString();
                    //    newrow["VALUE OF FILM"] = sumfilm * rateperkg;
                    //    double valueoffilm = sumfilm * rateperkg;
                    //    double perpktrate = valueoffilm / packed;
                    //    perpktrate = Math.Round(perpktrate, 2);
                    //    newrow["PER PACKET RATE"] = perpktrate;
                    //    double rateperltr = perpktrate * 10;
                    //    newrow["RATE PER LTR"] = rateperltr;
                    //    total100 += qty;
                    //}
                    //if (mlltr == "200")
                    //{
                    //    double packed = 5 * qty;
                    //    double sumfilm = consumption + wastage;
                    //    double packedyeld = packed / sumfilm;
                    //    packedyeld = Math.Round(packedyeld, 2);
                    //    newrow["PACKED"] = packed.ToString();
                    //    newrow["PER KG YIELD"] = packedyeld.ToString();
                    //    newrow["VALUE OF FILM"] = sumfilm * rateperkg;
                    //    double valueoffilm = sumfilm * rateperkg;
                    //    double perpktrate = valueoffilm / packed;
                    //    perpktrate = Math.Round(perpktrate, 2);
                    //    newrow["PER PACKET RATE"] = perpktrate;
                    //    double rateperltr = perpktrate * 5;
                    //    newrow["RATE PER LTR"] = rateperltr;
                    //    total200 += qty;
                    //}
                    //if (mlltr == "250")
                    //{
                    //    double packed = 4 * qty;
                    //    double sumfilm = consumption + wastage;
                    //    double packedyeld = packed / sumfilm;
                    //    packedyeld = Math.Round(packedyeld, 2);
                    //    newrow["PACKED"] = packed.ToString();
                    //    newrow["PER KG YIELD"] = packedyeld.ToString();
                    //    newrow["VALUE OF FILM"] = sumfilm * rateperkg;
                    //    double valueoffilm = sumfilm * rateperkg;
                    //    double perpktrate = valueoffilm / packed;
                    //    perpktrate = Math.Round(perpktrate, 2);
                    //    newrow["PER PACKET RATE"] = perpktrate;
                    //    double rateperltr = perpktrate * 4;
                    //    newrow["RATE PER LTR"] = rateperltr;
                    //    total250 += qty;
                    //}
                    //if (mlltr == "500")
                    //{
                    //    double packed = 2 * qty;
                    //    double sumfilm = consumption + wastage;
                    //    double packedyeld = packed / sumfilm;
                    //    packedyeld = Math.Round(packedyeld, 2);
                    //    newrow["PACKED"] = packed.ToString();
                    //    newrow["PER KG YIELD"] = packedyeld.ToString();
                    //    newrow["VALUE OF FILM"] = sumfilm * rateperkg;
                    //    double valueoffilm = sumfilm * rateperkg;
                    //    double perpktrate = valueoffilm / packed;
                    //    perpktrate = Math.Round(perpktrate, 2);
                    //    newrow["PER PACKET RATE"] = perpktrate;
                    //    double rateperltr = perpktrate * 2;
                    //    newrow["RATE PER LTR"] = rateperltr;
                    //    total500 += qty;
                    //}
                    //if (mlltr == "1000")
                    //{
                    //    double packed = 1 * qty;
                    //    double sumfilm = consumption + wastage;
                    //    double packedyeld = packed / sumfilm;
                    //    packedyeld = Math.Round(packedyeld, 2);
                    //    newrow["PACKED"] = packed.ToString();
                    //    newrow["PER KG YIELD"] = packedyeld.ToString();
                    //    newrow["VALUE OF FILM"] = sumfilm * rateperkg;
                    //    double valueoffilm = sumfilm * rateperkg;
                    //    double perpktrate = valueoffilm / packed;
                    //    perpktrate = Math.Round(perpktrate, 2);
                    //    newrow["PER PACKET RATE"] = perpktrate;
                    //    double rateperltr = perpktrate * 1;
                    //    newrow["RATE PER LTR"] = rateperltr;
                    //    total1000 += qty;
                    //}
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
                int count = Report.Rows.Count;
                avgmilkrate = totalrate / count;
                totalmilkltrs = total100 + total1000 + total200 + total250 + total500;
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
                //newvartical2["PACKED"] = totalmilkltrs;
                newvartical2["RATE PER KG"] = avgmilkrate;
                Report.Rows.Add(newvartical2);
                
                totalwastage = (wastagetotal / consumptiontotal) * 100;
                totalwastage = Math.Round(totalwastage, 2);
                lbltotalwastage.Text = totalwastage.ToString();

                totalcuttingwastage = (cuttingtotal / consumptiontotal) * 100;
                totalcuttingwastage = Math.Round(totalcuttingwastage, 2);
                lblcuttingfilim.Text = totalcuttingwastage.ToString();
               
                overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                overallwastage = Math.Round(overallwastage, 2);
                lbloverallfilim.Text = overallwastage.ToString();

                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
                Session["finalize"] = Report;
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
                grdReports.DataSource = null;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[2].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
            e.Row.Cells[13].BackColor = System.Drawing.Color.LightCyan;
            e.Row.Cells[14].BackColor = System.Drawing.Color.Bisque; // second col
        }
       
    }

    protected void btn_finalizeclick(object sender, EventArgs e)
    {
        DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
        DataTable dtdetails = (DataTable)Session["finalize"];
        if (dtdetails.Rows.Count > 0)
        {
            DateTime fromdate = DateTime.Now;
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

            string date = fromdate.ToString("dd/MM/yyyy");
            string[] data = date.Split('/');
            
            string month = data[1].ToString();
            string year = data[2].ToString();
            string mainbranch = Session["Branch_ID"].ToString();
            foreach (DataRow dr in dtdetails.Rows)
            {
                string productid = dr["ProductId"].ToString();
                string packingcharge = dr["RATE PER LTR"].ToString();
                cmd = new SqlCommand("Update packingcharges set rateperltr=@rateperltr where productid=@pid and month=@month and year=@yr and branchid=@bid");
                cmd.Parameters.Add("@month", month);
                cmd.Parameters.Add("@yr", year);
                cmd.Parameters.Add("@pid", productid);
                cmd.Parameters.Add("@rateperltr", packingcharge);
                cmd.Parameters.Add("@bid", mainbranch);
                if (vdm.Update(cmd) == 0)
                {
                    cmd = new SqlCommand("INSERT INTO packingcharges(productid, rateperltr, month, year, branchid, doe) values (@productid, @rate, @mnth, @year, @branchid, @doe)");
                    cmd.Parameters.Add("@mnth", month);
                    cmd.Parameters.Add("@year", year);
                    cmd.Parameters.Add("@productid", productid);
                    cmd.Parameters.Add("@rate", packingcharge);
                    cmd.Parameters.Add("@branchid", mainbranch);
                    cmd.Parameters.Add("@doe", ServerDateCurrentdate);
                    vdm.insert(cmd);
                }
            }
        }
    }
}