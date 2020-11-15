using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class butterproductionreport : System.Web.UI.Page
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
                    dtp_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    dtp_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
            Report.Columns.Add("Product Name");
            Report.Columns.Add("O/B");
            Report.Columns.Add("Conversion Qty");
            Report.Columns.Add("Conversion Fat");
            Report.Columns.Add("Production Qty");
            Report.Columns.Add("Total Qty");
            Report.Columns.Add("Date");
            cmd = new SqlCommand("SELECT bp.sno, pm.productname, bp.productid, bp.creamtype, bp.convertionquantity, bp.convertionfat, bp.productionqty, bp.ob, bp.sales, bp.cb, bp.createdon FROM plant_production_details bp inner join productmaster pm on pm.sno=bp.productid  WHERE (bp.createdon BETWEEN @d1 AND @d2) AND (pm.departmentid = '10') AND (bp.branchid = @branchid) ORDER BY bp.createdon");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            double totalproductionqty = 0;
            DataTable dtghee = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtghee.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtghee.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["createdon"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["Product Name"] = dr["productname"].ToString();
                    newrow["Conversion Qty"] = dr["convertionquantity"].ToString();
                    newrow["Conversion Fat"] = dr["convertionfat"].ToString();
                    double productionqty = 0;
                    double.TryParse(dr["productionqty"].ToString(), out productionqty);
                    productionqty = Math.Round(productionqty, 2);
                    newrow["Production Qty"] = productionqty;
                    totalproductionqty += productionqty;
                    double openingbalance = 0;
                    double.TryParse(dr["ob"].ToString(), out openingbalance);
                    openingbalance = Math.Round(openingbalance, 2);
                    newrow["O/B"] = openingbalance;
                    double sales = 0;
                    double.TryParse(dr["sales"].ToString(), out sales);
                    sales = Math.Round(sales, 2);
                    //newrow["Sales"] = sales;
                    double closingbalance = 0;
                    //double.TryParse(dr["closingbalance"].ToString(), out closingbalance);
                    //closingbalance = Math.Round(closingbalance, 2);
                    //newrow["C/B"] = closingbalance;
                    newrow["Date"] = date;
                    newrow["Total Qty"] = openingbalance + productionqty;
                    closingbalance = openingbalance + productionqty - sales;
                    //newrow["C/B"] = closingbalance;
                    Report.Rows.Add(newrow);
                }
                DataRow newrow1 = Report.NewRow();
                newrow1["O/B"] = "TOTAL";
                newrow1["Production Qty"] = totalproductionqty;
                Report.Rows.Add(newrow1);

                DataRow newrow2 = Report.NewRow();
                newrow2["Product Name"] = "Butter Sales Details";
                Report.Rows.Add(newrow2);

                grdReports.DataSource = Report;
                grdReports.DataBind();
                buttersales();
                Session["xportdata"] = Report;
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
            if (e.Row.Cells[1].Text == "Butter Sales Details")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[4].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    DataTable dtReport = new DataTable();
    private void buttersales()
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
            dtReport.Columns.Add("sno");
            dtReport.Columns.Add("Product Name");
            // Report.Columns.Add("Creamtype");
            dtReport.Columns.Add("salesquantity");
            dtReport.Columns.Add("dispatchtoghee");
            dtReport.Columns.Add("Date");
            cmd = new SqlCommand("SELECT bs.sno, bs.productid, pm.productname, bs.salesquantity, bs.dispatchtoghee, bs.branchid, bs.doe FROM buttersales bs inner join productmaster pm on pm.sno=bs.productid WHERE (bs.doe BETWEEN @d1 AND @d2) Order by bs.doe");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            double salesqty = 0;
            double despatchqty = 0;
            DataTable dtgheesales = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtgheesales.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtgheesales.Rows)
                {
                    DataRow newrow = dtReport.NewRow();
                    string salesquantity = dr["salesquantity"].ToString();
                    string dispatchtoghee = dr["dispatchtoghee"].ToString();
                    if (salesquantity != "0" || dispatchtoghee != "0")
                    {
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Product Name"] = dr["productname"].ToString();
                        newrow["salesquantity"] = dr["salesquantity"].ToString();
                        newrow["dispatchtoghee"] = dr["dispatchtoghee"].ToString();
                        double sqty = Convert.ToDouble(dr["salesquantity"]);
                        double despqty = Convert.ToDouble(dr["dispatchtoghee"]);
                        salesqty += sqty;
                        despatchqty += despqty;
                        newrow["Date"] = date;
                        dtReport.Rows.Add(newrow);
                    }
                }
                DataRow newrow1 = dtReport.NewRow();
                newrow1["Product Name"] = "TOTAL";
                newrow1["salesquantity"] = salesqty;
                newrow1["dispatchtoghee"] = despatchqty;
                dtReport.Rows.Add(newrow1);
                grdsales.DataSource = dtReport;
                grdsales.DataBind();
                Session["xportdata"] = dtReport;
                hidepanel.Visible = true;
            }
            else
            {
                // lblmsg.Text = "No data were found";
                grdsales.DataSource = null;
                grdsales.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}