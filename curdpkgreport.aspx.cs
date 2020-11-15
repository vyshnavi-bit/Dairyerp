using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class curdpkgreport : System.Web.UI.Page
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
            if (ddlReportType.Text == "Day Wise")
            {
                if (BranchID == "26")
                {
                    lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
                    lbltodate.Text = todate.ToString("dd/MMM/yyyy");
                    string Type = Session["BranchType"].ToString();
                    Report.Columns.Add("Date");
                    Report.Columns.Add("Product Name");
                    Report.Columns.Add("Received Film");
                    Report.Columns.Add("Consumption Film");
                    Report.Columns.Add("Wastage Film");
                    Report.Columns.Add("Production Film");
                    Report.Columns.Add("Return Film");
                    Report.Columns.Add("Film Wastage(%)");

                    cmd = new SqlCommand("SELECT  cpm.sno, cpm.productid, cpm.ob, cpm.received_film,cpm.cb, cpm.total, cpm.consumption_film, cpm.remarks, cpm.wastage_film, cpm.production, cpm.approveproduction, cpm.entry_by, cpm.branchid, cpm.doe, pm.productname, bi.branchname, cpm.qty_ltr FROM   packing_entry AS cpm INNER JOIN productmaster AS pm ON pm.sno = cpm.productid INNER JOIN branch_info AS bi ON bi.sno = cpm.branchid WHERE  (cpm.branchid = @branchid) AND (cpm.doe BETWEEN @d1 AND @d2) AND (cpm.section = 'curd') ORDER BY cpm.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@branchid", BranchID);

                    DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtPacking.Rows.Count > 0)
                    {
                        int i = 1;
                        double wastagetotal = 0;
                        double cuttingtotal = 0;
                        double consumptiontotal = 0;
                        double receivedtotal = 0;
                        double returnfilmtotal = 0;
                        double totalwastage = 0;
                        double totalcuttingwastage = 0;
                        double overallwastage = 0;
                        double totalqtykgs = 0;
                        string prevdate = string.Empty;
                        foreach (DataRow dr in dtPacking.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            string Received = dr["received_film"].ToString();
                            string Consumption = dr["consumption_film"].ToString();
                            string Wastage = dr["wastage_film"].ToString();
                            string Production = dr["production"].ToString();
                            if (Received == "0" && Consumption == "0" && Wastage == "0" && Production == "0")
                            {
                            }
                            else
                            {
                                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                                string date = dtdoe.ToString("dd/MM/yyyy");
                                string currentdate = date;
                                if (currentdate == prevdate)
                                {
                                    newrow["DATE"] = date;
                                    newrow["Product Name"] = dr["productname"].ToString();
                                    newrow["Received Film"] = dr["received_film"].ToString();
                                    newrow["Consumption Film"] = dr["consumption_film"].ToString();
                                    newrow["Wastage Film"] = dr["wastage_film"].ToString();
                                    newrow["Production Film"] = dr["production"].ToString();
                                    double wastage = 0;
                                    double.TryParse(dr["wastage_film"].ToString(), out wastage);
                                    wastagetotal += wastage;
                                    double consumption = 0;
                                    double.TryParse(dr["consumption_film"].ToString(), out consumption);
                                    consumptiontotal += consumption;
                                    double RecivedFilm = 0;
                                    double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                                    receivedtotal += RecivedFilm;
                                    newrow["Return Film"] = Convert.ToDouble(RecivedFilm - consumption);
                                    Report.Rows.Add(newrow);
                                }
                                else
                                {
                                    if (receivedtotal > 0)
                                    {
                                        DataRow newvartical2 = Report.NewRow();
                                        newvartical2["Product Name"] = "Total";
                                        newvartical2["Received Film"] = receivedtotal;
                                        newvartical2["Consumption Film"] = consumptiontotal;
                                        newvartical2["Wastage Film"] = wastagetotal;
                                        Report.Rows.Add(newvartical2);
                                        DataRow newvartical3 = Report.NewRow();
                                        totalwastage = (wastagetotal / consumptiontotal) * 100;
                                        totalwastage = Math.Round(totalwastage, 2);
                                        newvartical3["Return Film"] = "Total Wastage Film(%)";
                                        newvartical3["Film Wastage(%)"] = totalwastage;
                                        Report.Rows.Add(newvartical3);
                                        DataRow newvartical5 = Report.NewRow();
                                        overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                                        overallwastage = Math.Round(overallwastage, 2);
                                        newvartical5["Return Film"] = "Over All Film Watsge(%)";
                                        newvartical5["Film Wastage(%)"] = overallwastage;
                                        Report.Rows.Add(newvartical5);

                                        wastagetotal = 0;
                                        cuttingtotal = 0;
                                        consumptiontotal = 0;
                                        receivedtotal = 0;
                                        returnfilmtotal = 0;
                                        totalwastage = 0;
                                        totalcuttingwastage = 0;
                                        overallwastage = 0;
                                    }
                                    prevdate = currentdate;
                                    newrow["DATE"] = date;
                                    newrow["Product Name"] = dr["productname"].ToString();
                                    newrow["Received Film"] = dr["received_film"].ToString();
                                    newrow["Consumption Film"] = dr["consumption_film"].ToString();
                                    newrow["Wastage Film"] = dr["wastage_film"].ToString();
                                    newrow["Production Film"] = dr["production"].ToString();
                                    double wastage = 0;
                                    double.TryParse(dr["wastage_film"].ToString(), out wastage);
                                    wastagetotal += wastage;
                                    double consumption = 0;
                                    double.TryParse(dr["consumption_film"].ToString(), out consumption);
                                    consumptiontotal += consumption;
                                    double RecivedFilm = 0;
                                    double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                                    receivedtotal += RecivedFilm;
                                    newrow["Return Film"] = Convert.ToDouble(RecivedFilm - consumption);
                                    Report.Rows.Add(newrow);
                                }
                            }
                        }
                        DataRow newvartical21 = Report.NewRow();
                        newvartical21["Product Name"] = "Total";
                        newvartical21["Received Film"] = receivedtotal;
                        newvartical21["Consumption Film"] = consumptiontotal;
                        newvartical21["Wastage Film"] = wastagetotal;
                        Report.Rows.Add(newvartical21);
                        DataRow newvartical31 = Report.NewRow();
                        totalwastage = (wastagetotal / consumptiontotal) * 100;
                        totalwastage = Math.Round(totalwastage, 2);
                        newvartical31["Return Film"] = "Total Wastage Film(%)";
                        newvartical31["Film Wastage(%)"] = totalwastage;
                        Report.Rows.Add(newvartical31);
                        DataRow newvartical51 = Report.NewRow();
                        overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                        overallwastage = Math.Round(overallwastage, 2);
                        newvartical51["Return Film"] = "Over All Film Watsge(%)";
                        newvartical51["Film Wastage(%)"] = overallwastage;
                        Report.Rows.Add(newvartical51);

                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        Session["xportdata"] = Report;
                        hidepanel.Visible = true;
                        panel_day.Visible = true;
                        panel_month.Visible = false;
                    }
                    else
                    {
                        grdReports.DataSource = null;
                        grdReports_mnth.DataSource = null;
                        grdReports.DataBind();
                        Session["xportdata"] = null;
                        hidepanel.Visible = true;
                        grdReports_mnth.Visible = false;
                        lblmsg.Text = "No data were found";
                    }
                }
                else
                {
                    lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
                    lbltodate.Text = todate.ToString("dd/MMM/yyyy");
                    string Type = Session["BranchType"].ToString();
                    Report.Columns.Add("Date");
                    Report.Columns.Add("Product Name");
                    Report.Columns.Add("Received Film");
                    Report.Columns.Add("Consumption Film");
                    Report.Columns.Add("Wastage Film");
                    Report.Columns.Add("Production Film");
                    Report.Columns.Add("Film Wastage(%)");

                    cmd = new SqlCommand("SELECT  cpm.sno, cpm.productid, cpm.ob, cpm.received_film,cpm.cb, cpm.total, cpm.consumption_film, cpm.remarks, cpm.wastage_film, cpm.production, cpm.approveproduction, cpm.entry_by, cpm.branchid, cpm.doe, pm.productname, bi.branchname, cpm.qty_ltr FROM   packing_entry AS cpm INNER JOIN productmaster AS pm ON pm.sno = cpm.productid INNER JOIN branch_info AS bi ON bi.sno = cpm.branchid WHERE  (cpm.branchid = @branchid) AND (cpm.doe BETWEEN @d1 AND @d2) AND (cpm.section = 'curd') ORDER BY cpm.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@branchid", BranchID);

                    DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtPacking.Rows.Count > 0)
                    {
                        int i = 1;
                        double wastagetotal = 0;
                        double cuttingtotal = 0;
                        double consumptiontotal = 0;
                        double receivedtotal = 0;
                        double returnfilmtotal = 0;
                        double totalwastage = 0;
                        double totalcuttingwastage = 0;
                        double overallwastage = 0;
                        double totalqtykgs = 0;
                        string prevdate = string.Empty;
                        foreach (DataRow dr in dtPacking.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            string Received = dr["received_film"].ToString();
                            string Consumption = dr["consumption_film"].ToString();
                            string Wastage = dr["wastage_film"].ToString();
                            string Production = dr["production"].ToString();
                            if (Received == "0" && Consumption == "0" && Wastage == "0" && Production == "0")
                            {
                            }
                            else
                            {
                                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                                string date = dtdoe.ToString("dd/MM/yyyy");
                                string currentdate = date;
                                if (currentdate == prevdate)
                                {
                                    newrow["DATE"] = date;
                                    newrow["Product Name"] = dr["productname"].ToString();
                                    newrow["Received Film"] = dr["received_film"].ToString();
                                    newrow["Consumption Film"] = dr["consumption_film"].ToString();
                                    newrow["Wastage Film"] = dr["wastage_film"].ToString();
                                    newrow["Production Film"] = dr["production"].ToString();
                                    double wastage = 0;
                                    double.TryParse(dr["wastage_film"].ToString(), out wastage);
                                    wastagetotal += wastage;
                                    double consumption = 0;
                                    double.TryParse(dr["consumption_film"].ToString(), out consumption);
                                    consumptiontotal += consumption;
                                    double RecivedFilm = 0;
                                    double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                                    receivedtotal += RecivedFilm;
                                    Report.Rows.Add(newrow);
                                }
                                else
                                {
                                    if (receivedtotal > 0)
                                    {
                                        DataRow newvartical2 = Report.NewRow();
                                        newvartical2["Product Name"] = "Total";
                                        newvartical2["Received Film"] = receivedtotal;
                                        newvartical2["Consumption Film"] = consumptiontotal;
                                        newvartical2["Wastage Film"] = wastagetotal;
                                        Report.Rows.Add(newvartical2);
                                        DataRow newvartical3 = Report.NewRow();
                                        totalwastage = (wastagetotal / consumptiontotal) * 100;
                                        totalwastage = Math.Round(totalwastage, 2);
                                        newvartical3["Production Film"] = "Total Wastage Film(%)";
                                        newvartical3["Film Wastage(%)"] = totalwastage;
                                        Report.Rows.Add(newvartical3);
                                        DataRow newvartical5 = Report.NewRow();
                                        overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                                        overallwastage = Math.Round(overallwastage, 2);
                                        newvartical5["Production Film"] = "Over All Film Watsge(%)";
                                        newvartical5["Film Wastage(%)"] = overallwastage;
                                        Report.Rows.Add(newvartical5);

                                        wastagetotal = 0;
                                        cuttingtotal = 0;
                                        consumptiontotal = 0;
                                        receivedtotal = 0;
                                        returnfilmtotal = 0;
                                        totalwastage = 0;
                                        totalcuttingwastage = 0;
                                        overallwastage = 0;
                                    }
                                    prevdate = currentdate;
                                    newrow["DATE"] = date;
                                    newrow["Product Name"] = dr["productname"].ToString();
                                    newrow["Received Film"] = dr["received_film"].ToString();
                                    newrow["Consumption Film"] = dr["consumption_film"].ToString();
                                    newrow["Wastage Film"] = dr["wastage_film"].ToString();
                                    newrow["Production Film"] = dr["production"].ToString();
                                    double wastage = 0;
                                    double.TryParse(dr["wastage_film"].ToString(), out wastage);
                                    wastagetotal += wastage;
                                    double consumption = 0;
                                    double.TryParse(dr["consumption_film"].ToString(), out consumption);
                                    consumptiontotal += consumption;
                                    double RecivedFilm = 0;
                                    double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                                    receivedtotal += RecivedFilm;
                                    Report.Rows.Add(newrow);
                                }
                            }
                        }
                        DataRow newvartical21 = Report.NewRow();
                        newvartical21["Product Name"] = "Total";
                        newvartical21["Received Film"] = receivedtotal;
                        newvartical21["Consumption Film"] = consumptiontotal;
                        newvartical21["Wastage Film"] = wastagetotal;
                        Report.Rows.Add(newvartical21);
                        DataRow newvartical31 = Report.NewRow();
                        totalwastage = (wastagetotal / consumptiontotal) * 100;
                        totalwastage = Math.Round(totalwastage, 2);
                        newvartical31["Production Film"] = "Total Wastage Film(%)";
                        newvartical31["Film Wastage(%)"] = totalwastage;
                        Report.Rows.Add(newvartical31);
                        DataRow newvartical51 = Report.NewRow();
                        overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                        overallwastage = Math.Round(overallwastage, 2);
                        newvartical51["Production Film"] = "Over All Film Watsge(%)";
                        newvartical51["Film Wastage(%)"] = overallwastage;
                        Report.Rows.Add(newvartical51);

                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        Session["xportdata"] = Report;
                        hidepanel.Visible = true;
                        panel_day.Visible = true;
                        panel_month.Visible = false;
                    }
                    else
                    {
                        grdReports.DataSource = null;
                        grdReports_mnth.DataSource = null;
                        grdReports.DataBind();
                        Session["xportdata"] = null;
                        hidepanel.Visible = true;
                        grdReports_mnth.Visible = false;
                        lblmsg.Text = "No data were found";
                    }
                }
            }
            else
            {
                lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
                lbltodate.Text = todate.ToString("dd/MMM/yyyy");
                string Type = Session["BranchType"].ToString();
                Report.Columns.Add("Product Name");
                Report.Columns.Add("Pid");
                Report.Columns.Add("Received Film");
                Report.Columns.Add("Consumption Film");
                Report.Columns.Add("Wastage Film");
                Report.Columns.Add("Production Film");
                Report.Columns.Add("Film Wastage(%)");
                cmd = new SqlCommand("SELECT   cpm.productid, SUM(cpm.received_film) AS received_film, SUM(cpm.consumption_film) AS consumption_film, SUM(cpm.wastage_film) AS wastage_film, SUM(cpm.production) AS production,  SUM(cpm.approveproduction) AS approveproduction, pm.productname FROM   packing_entry AS cpm INNER JOIN productmaster AS pm ON pm.sno = cpm.productid INNER JOIN branch_info AS bi ON bi.sno = cpm.branchid WHERE   (cpm.branchid = @branchid) AND (cpm.doe BETWEEN @d1 AND @d2) AND (cpm.section = 'curd') GROUP BY pm.productname, cpm.productid");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtPacking.Rows.Count > 0)
                {
                    int i = 1;
                    double wastagetotal = 0;
                    double cuttingtotal = 0;
                    double consumptiontotal = 0;
                    double receivedtotal = 0;
                    double returnfilmtotal = 0;
                    double totalwastage = 0;
                    double totalcuttingwastage = 0;
                    double overallwastage = 0;
                    double totalqtykgs = 0;
                    foreach (DataRow dr in dtPacking.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Product Name"] = dr["productname"].ToString();
                        newrow["Pid"] = dr["productid"].ToString();
                        double received_film = 0;
                        double.TryParse(dr["received_film"].ToString(), out received_film);
                        newrow["Received Film"] = Math.Round(received_film, 2);

                        double consumption_film = 0;
                        double.TryParse(dr["consumption_film"].ToString(), out consumption_film);
                        newrow["Consumption Film"] = Math.Round(consumption_film, 2);

                        double wastage_film = 0;
                        double.TryParse(dr["wastage_film"].ToString(), out wastage_film);
                        newrow["Wastage Film"] = Math.Round(wastage_film, 2);

                        double production = 0;
                        double.TryParse(dr["production"].ToString(), out production);
                        newrow["Production Film"] = Math.Round(production, 2);

                        double wastage = 0;
                        double.TryParse(dr["wastage_film"].ToString(), out wastage);
                        wastagetotal += wastage;
                        double consumption = 0;
                        double.TryParse(dr["consumption_film"].ToString(), out consumption);
                        consumptiontotal += consumption;
                        double RecivedFilm = 0;
                        double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                        receivedtotal += RecivedFilm;
                        Report.Rows.Add(newrow);
                    }
                    DataRow newvartical21 = Report.NewRow();
                    newvartical21["Product Name"] = "Total";
                    newvartical21["Received Film"] = receivedtotal;
                    newvartical21["Consumption Film"] = consumptiontotal;
                    newvartical21["Wastage Film"] = wastagetotal;
                    Report.Rows.Add(newvartical21);
                    DataRow newvartical31 = Report.NewRow();
                    totalwastage = (wastagetotal / consumptiontotal) * 100;
                    totalwastage = Math.Round(totalwastage, 2);
                    newvartical31["Production Film"] = "Total Wastage Film(%)";
                    newvartical31["Film Wastage(%)"] = totalwastage;
                    Report.Rows.Add(newvartical31);

                    DataRow newvartical51 = Report.NewRow();
                    overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                    overallwastage = Math.Round(overallwastage, 2);
                    newvartical51["Production Film"] = "Over All Film Watsge(%)";
                    newvartical51["Film Wastage(%)"] = overallwastage;
                    Report.Rows.Add(newvartical51);

                    grdReports.DataSource = null;
                    grdReports_mnth.DataSource = Report;
                    grdReports_mnth.DataBind();
                    Session["xportdata"] = Report;
                    hidepanel.Visible = true;
                    panel_day.Visible = false;
                    panel_month.Visible = true;
                }
                else
                {
                    grdReports.DataSource = null;
                    grdReports_mnth.DataSource = null;
                    grdReports.DataBind();
                    Session["xportdata"] = null;
                    hidepanel.Visible = true;
                    lblmsg.Text = "No data were found";
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void grdday_DataBinding(object sender, EventArgs e)
    {
        if (ddlReportType.Text == "Day Wise")
        {
            try
            {
                GridViewGroup First = new GridViewGroup(grdReports, null, "Date");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else
        {

        }
    }
    protected void grdday_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (ddlReportType.Text == "Day Wise")
            {
                if (e.Row.Cells[2].Text == "Total")
                {
                    e.Row.BackColor = System.Drawing.Color.Aquamarine;
                    e.Row.Font.Size = FontUnit.Large;
                    e.Row.Font.Bold = true;
                }
            }
            else
            {

            }
        }
    }
    protected void grdrecipts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataTable Report = new DataTable();
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdReports_mnth.Rows[rowIndex];
            string pid = row.Cells[2].Text;
            if (pid == "" || pid == null || pid == "&nbsp;")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen();", false);
            }
            else
            {
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
                Report.Columns.Add("Product Name");
                Report.Columns.Add("Received Film");
                Report.Columns.Add("Consumption Film");
                Report.Columns.Add("Wastage Film");
                Report.Columns.Add("Production Film");
                Report.Columns.Add("Film Wastage(%)");
                cmd = new SqlCommand("SELECT  cpm.sno, cpm.productid, cpm.ob, cpm.received_film, cpm.cb, cpm.total, cpm.consumption_film, cpm.remarks, cpm.wastage_film, cpm.production, cpm.approveproduction, cpm.entry_by, cpm.branchid, cpm.doe, pm.productname, bi.branchname, cpm.qty_ltr FROM   packing_entry AS cpm INNER JOIN productmaster AS pm ON pm.sno = cpm.productid INNER JOIN branch_info AS bi ON bi.sno = cpm.branchid  WHERE   (cpm.branchid = @branchid) AND (cpm.doe BETWEEN @d1 AND @d2) AND (cpm.section = 'curd') AND (cpm.productid = @pid) ORDER BY cpm.sno");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
                cmd.Parameters.Add("@pid", pid);
                DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtPacking.Rows.Count > 0)
                {
                    int i = 1;
                    double wastagetotal = 0;
                    double cuttingtotal = 0;
                    double consumptiontotal = 0;
                    double receivedtotal = 0;
                    double returnfilmtotal = 0;
                    double totalwastage = 0;
                    double totalcuttingwastage = 0;
                    double overallwastage = 0;
                    double totalqtykgs = 0;
                    foreach (DataRow dr in dtPacking.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Product Name"] = dr["productname"].ToString();
                        double received_film = 0;
                        double.TryParse(dr["received_film"].ToString(), out received_film);
                        newrow["Received Film"] = Math.Round(received_film, 2);

                        double consumption_film = 0;
                        double.TryParse(dr["consumption_film"].ToString(), out consumption_film);
                        newrow["Consumption Film"] = Math.Round(consumption_film, 2);

                        double wastage_film = 0;
                        double.TryParse(dr["wastage_film"].ToString(), out wastage_film);
                        newrow["Wastage Film"] = Math.Round(wastage_film, 2);

                        double production = 0;
                        double.TryParse(dr["production"].ToString(), out production);
                        newrow["Production Film"] = Math.Round(production, 2);

                        double wastage = 0;
                        double.TryParse(dr["wastage_film"].ToString(), out wastage);
                        wastagetotal += wastage;
                        double consumption = 0;
                        double.TryParse(dr["consumption_film"].ToString(), out consumption);
                        consumptiontotal += consumption;
                        double RecivedFilm = 0;
                        double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                        receivedtotal += RecivedFilm;
                        Report.Rows.Add(newrow);
                    }
                    DataRow newvartical21 = Report.NewRow();
                    newvartical21["Product Name"] = "Total";
                    newvartical21["Received Film"] = receivedtotal;
                    newvartical21["Consumption Film"] = consumptiontotal;
                    newvartical21["Wastage Film"] = wastagetotal;
                    Report.Rows.Add(newvartical21);
                    DataRow newvartical31 = Report.NewRow();
                    totalwastage = (wastagetotal / consumptiontotal) * 100;
                    totalwastage = Math.Round(totalwastage, 2);
                    newvartical31["Production Film"] = "Total Wastage Film(%)";
                    newvartical31["Film Wastage(%)"] = totalwastage;
                    Report.Rows.Add(newvartical31);

                    DataRow newvartical51 = Report.NewRow();
                    overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                    overallwastage = Math.Round(overallwastage, 2);
                    newvartical51["Production Film"] = "Over All Film Watsge(%)";
                    newvartical51["Film Wastage(%)"] = overallwastage;
                    Report.Rows.Add(newvartical51);

                    GrdProducts.DataSource = Report;
                    GrdProducts.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen();", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
}