using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class curdfilmcostperliter : System.Web.UI.Page
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

                    dtp_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
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
            Session["filename"] = "Curd Packing Report";
            Session["title"] = "Curd Packing Report Details";
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
            Report.Columns.Add("Sno");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Received Film");
            Report.Columns.Add("Consumption Film");
            Report.Columns.Add("Wastage Film");
            Report.Columns.Add("Prod. Qty");
            Report.Columns.Add("Prod. Packs");
            Report.Columns.Add("PER KG YIELD");
            Report.Columns.Add("Rate Per Kg");
            Report.Columns.Add("VALUE OF FILM");
            Report.Columns.Add("PER PACKET RATE");
            Report.Columns.Add("RATE PER LTR");

            cmd = new SqlCommand("SELECT cpm.productid, SUM(cpm.received_film) AS received_film, pm.ml, SUM(cpm.consumption_film) AS consumption_film, SUM(cpm.production) AS production, SUM(cpm.wastage_film) AS wastage_film, pm.productname,pm.filimrate FROM  packing_entry AS cpm INNER JOIN productmaster AS pm ON pm.sno = cpm.productid INNER JOIN branch_info AS bi ON bi.sno = cpm.branchid WHERE (cpm.branchid = @branchid) AND (cpm.doe BETWEEN @d1 AND @d2) AND (cpm.section = 'curd') AND (pm.groupname = 'film') GROUP BY cpm.productid, pm.productname , pm.ml, pm.filimrate");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];

            cmd = new SqlCommand("SELECT   SUM(productionqty) AS productionqty, productid FROM    plant_production_details WHERE  (createdon BETWEEN @d1 AND @d2) AND (branchid = @branchid) AND (deptid = 1) GROUP BY productid");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtprod = SalesDB.SelectQuery(cmd).Tables[0];

            if (dtPacking.Rows.Count > 0)
            {
                int i = 1;
                double totreceivedfilm = 0;
                double totconsumpctionfilm = 0;
                double totwastagefilm = 0;
                double totproduction = 0;
                foreach (DataRow dr in dtPacking.Rows)
                {
                    string productid = dr["productid"].ToString();
                    foreach (DataRow dra in dtprod.Select("productid='" + productid + "'"))
                    {
                        DataRow newrow = Report.NewRow();
                        string productionqty = dra["productionqty"].ToString();
                        if (Convert.ToDouble(productionqty) > 0)
                        {
                            newrow["Prod. Qty"] = productionqty.ToString();
                            totproduction += Convert.ToDouble(productionqty);
                            newrow["Sno"] = (i++).ToString();
                            newrow["Product Name"] = dr["productname"].ToString();
                            newrow["Received Film"] = dr["received_film"].ToString();
                            double received_film = Convert.ToDouble(dr["received_film"].ToString());
                            totreceivedfilm += received_film;
                            newrow["Consumption Film"] = dr["consumption_film"].ToString();
                            double consumption_film = Convert.ToDouble(dr["consumption_film"].ToString());
                            totconsumpctionfilm += consumption_film;
                            newrow["Wastage Film"] = dr["wastage_film"].ToString();
                            double wastage_film = Convert.ToDouble(dr["wastage_film"].ToString());
                            totwastagefilm += wastage_film;
                            string mlltr = dr["ml"].ToString();
                            double milkltrs = Convert.ToDouble(mlltr);
                            double packetsperltr = 1000 / milkltrs;
                            double packed = Math.Round(packetsperltr * Convert.ToDouble(productionqty));
                            newrow["Prod. Packs"] = packed.ToString();
                            double sumfilm = consumption_film + wastage_film;
                            double packedyeld = Math.Round(packed / sumfilm, 2);
                            newrow["PER KG YIELD"] = packedyeld.ToString();
                            newrow["Rate Per Kg"] = dr["filimrate"].ToString();
                            double rateperkg = Convert.ToDouble(dr["filimrate"].ToString());
                            newrow["VALUE OF FILM"] = sumfilm * rateperkg;
                            double valueoffilm = sumfilm * rateperkg;
                            double perpktrate = valueoffilm / packed;
                            perpktrate = Math.Round(perpktrate, 2);
                            newrow["PER PACKET RATE"] = perpktrate;
                            double rateperltr = perpktrate * 10;
                            newrow["RATE PER LTR"] = rateperltr;
                            Report.Rows.Add(newrow);
                        }
                    }
                }
                DataRow newvartical2 = Report.NewRow();
                totreceivedfilm = Math.Round(totreceivedfilm, 2);
                totconsumpctionfilm = Math.Round(totconsumpctionfilm, 2);
                totwastagefilm = Math.Round(totwastagefilm, 2);
                newvartical2["Received Film"] = totreceivedfilm.ToString();
                newvartical2["Consumption Film"] = totconsumpctionfilm.ToString();
                newvartical2["Wastage Film"] = totwastagefilm.ToString();
                newvartical2["Prod. Qty"] = Math.Round(totproduction, 2).ToString();
                Report.Rows.Add(newvartical2);

                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
                hidepanel.Visible = true;
                panel_day.Visible = true;
            }
            else
            {
                grdReports.DataSource = null;
                grdReports.DataBind();
                Session["xportdata"] = null;
                hidepanel.Visible = true;
                lblmsg.Text = "No data were found";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}