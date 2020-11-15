using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Filmconsumptionreport : System.Web.UI.Page
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
                    PopulateMonth();
                    PopulateYear();
                }
            }
        }
    }
    private void PopulateMonth()
    {
        ddlfrommonth.Items.Clear();
        ListItem lt = new ListItem();
        lt.Text = "MM";
        lt.Value = "0";
        ddlfrommonth.Items.Add(lt);
        ddltomonth.Items.Add(lt);
        for (int i = 1; i <= 12; i++)
        {
            lt = new ListItem();
            lt.Text = Convert.ToDateTime(i.ToString() + "/1/1900").ToString("MMMM");
            lt.Value = i.ToString();
            ddlfrommonth.Items.Add(lt);
            ddltomonth.Items.Add(lt);
        }
        ddlfrommonth.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
        ddltomonth.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
    }

    private void PopulateYear()
    {
        ddlyear.Items.Clear();
        ListItem lt = new ListItem();
        lt.Text = "YYYY";
        lt.Value = "0";
        ddlyear.Items.Add(lt);
        for (int i = DateTime.Now.Year; i >= 1950; i--)
        {
            lt = new ListItem();
            lt.Text = i.ToString();
            lt.Value = i.ToString();
            ddlyear.Items.Add(lt);
        }
        ddlyear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
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
            Report.Columns.Add("Date");
            Report.Columns.Add("sno");
            Report.Columns.Add("Batch Name");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Qty Ltr");
            Report.Columns.Add("Received Film");
            Report.Columns.Add("Consumption Film");
            Report.Columns.Add("Return Film");
            Report.Columns.Add("Wastage Film");
            Report.Columns.Add("Cutting Film");
            Report.Columns.Add("Film Wastage(%)");
            cmd = new SqlCommand("SELECT batchmaster.batch, packing_entry.qty_ltr, packing_entry.fat, packing_entry.snf, packing_entry.clr, packing_entry.received_film,packing_entry.cuttingfilm, packing_entry.consumption_film, packing_entry.return_film, packing_entry.doe,packing_entry.wastage_film, productmaster.productname FROM packing_entry INNER JOIN batchmaster ON packing_entry.batchid = batchmaster.batchid INNER JOIN productmaster ON packing_entry.productid = productmaster.sno WHERE (packing_entry.doe BETWEEN @d1 AND @d2) AND (packing_entry.branchid = @BranchID) ORDER BY packing_entry.doe");
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
                double totalwastage = 0;
                double totalcuttingwastage = 0;
                double overallwastage = 0;
                string prevdate = string.Empty;
                foreach (DataRow dr in dtPacking.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    string currentdate = date;
                    if (currentdate == prevdate)
                    {
                        newrow["DATE"] = date;
                        newrow["Batch Name"] = dr["batch"].ToString();
                        newrow["Product Name"] = dr["productname"].ToString();
                        // newrow["Silo Name"] = dr["SiloName"].ToString();
                        newrow["Qty Ltr"] = dr["qty_ltr"].ToString();
                        
                        newrow["Received Film"] = dr["received_film"].ToString();
                        newrow["Consumption Film"] = dr["consumption_film"].ToString();
                        newrow["Return Film"] = dr["return_film"].ToString();
                        newrow["Wastage Film"] = dr["wastage_film"].ToString();
                        newrow["Cutting Film"] = dr["cuttingfilm"].ToString();

                        double wastage = 0;
                        double.TryParse(dr["wastage_film"].ToString(), out wastage);
                        wastagetotal += wastage;

                        double cutting = 0;
                        double.TryParse(dr["cuttingfilm"].ToString(), out cutting);
                        cuttingtotal += cutting;

                        double consumption = 0;
                        double.TryParse(dr["consumption_film"].ToString(), out consumption);
                        consumptiontotal += consumption;


                        double RecivedFilm = 0;
                        double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                        receivedtotal += RecivedFilm;


                        double ReturnFilm = 0;
                        double.TryParse(dr["return_film"].ToString(), out ReturnFilm);
                        returnfilmtotal += ReturnFilm;

                        Report.Rows.Add(newrow);
                    }
                    else
                    {
                        if (receivedtotal > 0)
                        {
                            DataRow newvartical2 = Report.NewRow();
                            newvartical2["Product Name"] = "Total";
                            newvartical2["Received Film"] = Math.Round(receivedtotal, 2);
                            newvartical2["Consumption Film"] = Math.Round(consumptiontotal, 2);
                            newvartical2["Wastage Film"] = Math.Round(wastagetotal, 2);
                            newvartical2["Cutting Film"] = Math.Round(cuttingtotal, 2);
                            newvartical2["Return Film"] = Math.Round(returnfilmtotal, 2);
                            Report.Rows.Add(newvartical2);

                            DataRow newvartical3 = Report.NewRow();
                            totalwastage = (wastagetotal / consumptiontotal) * 100;
                            totalwastage = Math.Round(totalwastage, 2);
                            newvartical3["Cutting Film"] = "Total Wastage Film(%)";
                            newvartical3["Film Wastage(%)"] = totalwastage;
                            Report.Rows.Add(newvartical3);

                            DataRow newvartical4 = Report.NewRow();
                            totalcuttingwastage = (cuttingtotal / consumptiontotal) * 100;
                            totalcuttingwastage = Math.Round(totalcuttingwastage, 2);
                            newvartical4["Cutting Film"] = "Total Cutting Film(%)";
                            newvartical4["Film Wastage(%)"] = totalcuttingwastage;
                            Report.Rows.Add(newvartical4);

                            DataRow newvartical5 = Report.NewRow();
                            overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                            overallwastage = Math.Round(overallwastage, 2);
                            newvartical5["Cutting Film"] = "Over All Film Wastage(%)";
                            newvartical5["Film Wastage(%)"] = overallwastage;
                            Report.Rows.Add(newvartical5);
                            receivedtotal = 0;
                            consumptiontotal = 0;
                            wastagetotal = 0;
                            cuttingtotal = 0;
                            returnfilmtotal = 0;
                            totalwastage = 0;
                            totalcuttingwastage = 0;
                            overallwastage = 0;
                        }
                        prevdate = currentdate;
                        newrow["DATE"] = date;
                        newrow["Batch Name"] = dr["batch"].ToString();
                        newrow["Product Name"] = dr["productname"].ToString();
                        // newrow["Silo Name"] = dr["SiloName"].ToString();
                        newrow["Qty Ltr"] = dr["qty_ltr"].ToString();
                       
                        newrow["Received Film"] = dr["received_film"].ToString();
                        newrow["Consumption Film"] = dr["consumption_film"].ToString();
                        newrow["Return Film"] = dr["return_film"].ToString();
                        newrow["Wastage Film"] = dr["wastage_film"].ToString();
                        newrow["Cutting Film"] = dr["cuttingfilm"].ToString();

                        double wastage = 0;
                        double.TryParse(dr["wastage_film"].ToString(), out wastage);
                        wastagetotal += wastage;

                        double cutting = 0;
                        double.TryParse(dr["cuttingfilm"].ToString(), out cutting);
                        cuttingtotal += cutting;

                        double consumption = 0;
                        double.TryParse(dr["consumption_film"].ToString(), out consumption);
                        consumptiontotal += consumption;


                        double RecivedFilm = 0;
                        double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                        receivedtotal += RecivedFilm;


                        double ReturnFilm = 0;
                        double.TryParse(dr["return_film"].ToString(), out ReturnFilm);
                        returnfilmtotal += ReturnFilm;

                        Report.Rows.Add(newrow);
                    }
                }

                DataRow newvartical6 = Report.NewRow();
                newvartical6["Product Name"] = "Total";
                newvartical6["Received Film"] = Math.Round(receivedtotal, 2);
                newvartical6["Consumption Film"] = Math.Round(consumptiontotal, 2);
                newvartical6["Wastage Film"] = Math.Round(wastagetotal, 2);
                newvartical6["Cutting Film"] = Math.Round(cuttingtotal, 2);
                newvartical6["Return Film"] = Math.Round(returnfilmtotal, 2);
                Report.Rows.Add(newvartical6);

                DataRow newvartical7 = Report.NewRow();
                totalwastage = (wastagetotal / consumptiontotal) * 100;
                totalwastage = Math.Round(totalwastage, 2);
                newvartical7["Cutting Film"] = "Total Wastage Film(%)";
                newvartical7["Film Wastage(%)"] = totalwastage;
                Report.Rows.Add(newvartical7);

                DataRow newvartical8 = Report.NewRow();
                totalcuttingwastage = (cuttingtotal / consumptiontotal) * 100;
                totalcuttingwastage = Math.Round(totalcuttingwastage, 2);
                newvartical8["Cutting Film"] = "Total Cutting Film(%)";
                newvartical8["Film Wastage(%)"] = totalcuttingwastage;
                Report.Rows.Add(newvartical8);

                DataRow newvartical9 = Report.NewRow();
                overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                overallwastage = Math.Round(overallwastage, 2);
                newvartical9["Cutting Film"] = "Over All Film Wastage(%)";
                newvartical9["Film Wastage(%)"] = overallwastage;
                Report.Rows.Add(newvartical9);

                grdday.DataSource = Report;
                grdday.DataBind();
                Session["xportdata"] = Report;
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
                grdday.DataSource = null;
                grdday.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void grdday_DataBinding(object sender, EventArgs e)
    {
        try
        {
            GridViewGroup First = new GridViewGroup(grdday, null, "Date");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdday_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[3].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdmnthReports_DataBinding(object sender, EventArgs e)
    {
        try
        {
            GridViewGroup First = new GridViewGroup(grdmnthReports, null, "Month");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdmnthReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[2].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
        }
    }



    protected void btn_mnthGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            SalesDBManager SalesDB = new SalesDBManager();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            string Type = Session["BranchType"].ToString();
            Report.Columns.Add("Month");
            Report.Columns.Add("sno");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Qty Ltr");
            Report.Columns.Add("Received Film");
            Report.Columns.Add("Consumption Film");
            Report.Columns.Add("Return Film");
            Report.Columns.Add("Wastage Film");
            Report.Columns.Add("Cutting Film");
            Report.Columns.Add("Film Wastage(%)");
            cmd = new SqlCommand("SELECT MONTH(packing_entry.doe) AS MonthName, YEAR(packing_entry.doe) AS Year, SUM(packing_entry.received_film) AS received_film, SUM(packing_entry.consumption_film) AS consumption_film, SUM(packing_entry.return_film) AS return_film, SUM(packing_entry.wastage_film) AS wastage_film, SUM(packing_entry.cuttingfilm) AS cuttingfilm, SUM(packing_entry.qty_ltr) AS qty_ltr, productmaster.productname FROM  packing_entry INNER JOIN productmaster ON packing_entry.productid = productmaster.sno WHERE (MONTH(packing_entry.doe) BETWEEN @d1 AND @d2) AND YEAR(packing_entry.doe)=@year AND (packing_entry.branchid = @BranchID) GROUP BY MONTH(packing_entry.doe), YEAR(packing_entry.doe), productmaster.productname ORDER BY MONTH(packing_entry.doe)");
            // cmd = new SqlCommand("SELECT batchmaster.batch, packing_entry.qty_ltr, packing_entry.fat, packing_entry.snf, packing_entry.clr, packing_entry.received_film,packing_entry.cuttingfilm, packing_entry.consumption_film, packing_entry.return_film, packing_entry.doe,packing_entry.wastage_film, productmaster.productname FROM packing_entry INNER JOIN batchmaster ON packing_entry.batchid = batchmaster.batchid INNER JOIN productmaster ON packing_entry.productid = productmaster.sno WHERE (packing_entry.doe BETWEEN @d1 AND @d2) AND (packing_entry.branchid = @BranchID) ORDER BY packing_entry.doe");
            cmd.Parameters.Add("@d1", ddlfrommonth.SelectedItem.Value);
            cmd.Parameters.Add("@d2", ddltomonth.SelectedItem.Value);
            cmd.Parameters.Add("@year", ddlyear.SelectedItem.Value);
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
                double totalwastage = 0;
                double totalcuttingwastage = 0;
                double overallwastage = 0;
                string prevmonth = string.Empty;
                foreach (DataRow dr in dtPacking.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Month"] = dr["MonthName"].ToString();
                    string month = dr["MonthName"].ToString();
                    string currentmonth = month;
                    if (currentmonth == prevmonth)
                    {
                        newrow["Product Name"] = dr["productname"].ToString();
                        newrow["Qty Ltr"] = dr["qty_ltr"].ToString();
                        //newrow["FAT"] = dr["fat"].ToString();
                        //newrow["SNF"] = dr["snf"].ToString();
                        //newrow["CLR"] = dr["clr"].ToString();
                        newrow["Received Film"] = dr["received_film"].ToString();
                        newrow["Consumption Film"] = dr["consumption_film"].ToString();
                        newrow["Return Film"] = dr["return_film"].ToString();
                        newrow["Wastage Film"] = dr["wastage_film"].ToString();
                        newrow["Cutting Film"] = dr["cuttingfilm"].ToString();

                        double wastage = 0;
                        double.TryParse(dr["wastage_film"].ToString(), out wastage);
                        wastagetotal += wastage;

                        double cutting = 0;
                        double.TryParse(dr["cuttingfilm"].ToString(), out cutting);
                        cuttingtotal += cutting;

                        double consumption = 0;
                        double.TryParse(dr["consumption_film"].ToString(), out consumption);
                        consumptiontotal += consumption;


                        double RecivedFilm = 0;
                        double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                        receivedtotal += RecivedFilm;


                        double ReturnFilm = 0;
                        double.TryParse(dr["return_film"].ToString(), out ReturnFilm);
                        returnfilmtotal += ReturnFilm;

                        Report.Rows.Add(newrow);
                    }
                    else
                    {
                        if (receivedtotal > 0)
                        {
                            DataRow newvartical2 = Report.NewRow();
                            newvartical2["Product Name"] = "Total";
                            newvartical2["Received Film"] = Math.Round(receivedtotal, 2);
                            newvartical2["Consumption Film"] = Math.Round(consumptiontotal, 2);
                            newvartical2["Wastage Film"] = Math.Round(wastagetotal, 2);
                            newvartical2["Cutting Film"] = Math.Round(cuttingtotal, 2);
                            newvartical2["Return Film"] = Math.Round(returnfilmtotal, 2);
                            Report.Rows.Add(newvartical2);

                            DataRow newvartical3 = Report.NewRow();
                            totalwastage = (wastagetotal / consumptiontotal) * 100;
                            totalwastage = Math.Round(totalwastage, 2);
                            newvartical3["Cutting Film"] = "Total Wastage Film(%)";
                            newvartical3["Film Wastage(%)"] = totalwastage;
                            Report.Rows.Add(newvartical3);

                            DataRow newvartical4 = Report.NewRow();
                            totalcuttingwastage = (cuttingtotal / consumptiontotal) * 100;
                            totalcuttingwastage = Math.Round(totalcuttingwastage, 2);
                            newvartical4["Cutting Film"] = "Total Cutting Film(%)";
                            newvartical4["Film Wastage(%)"] = totalcuttingwastage;
                            Report.Rows.Add(newvartical4);

                            DataRow newvartical5 = Report.NewRow();
                            overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                            overallwastage = Math.Round(overallwastage, 2);
                            newvartical5["Cutting Film"] = "Over All Film Wastage(%)";
                            newvartical5["Film Wastage(%)"] = overallwastage;
                            Report.Rows.Add(newvartical5);
                            receivedtotal = 0;
                            consumptiontotal = 0;
                            wastagetotal = 0;
                            cuttingtotal = 0;
                            returnfilmtotal = 0;
                            overallwastage = 0;
                            totalcuttingwastage = 0;
                            totalwastage = 0;
                        }
                        prevmonth = currentmonth;
                        newrow["Product Name"] = dr["productname"].ToString();
                        newrow["Qty Ltr"] = dr["qty_ltr"].ToString();
                        //newrow["FAT"] = dr["fat"].ToString();
                        //newrow["SNF"] = dr["snf"].ToString();
                        //newrow["CLR"] = dr["clr"].ToString();
                        newrow["Received Film"] = dr["received_film"].ToString();
                        newrow["Consumption Film"] = dr["consumption_film"].ToString();
                        newrow["Return Film"] = dr["return_film"].ToString();
                        newrow["Wastage Film"] = dr["wastage_film"].ToString();
                        newrow["Cutting Film"] = dr["cuttingfilm"].ToString();
                        double wastage = 0;
                        double.TryParse(dr["wastage_film"].ToString(), out wastage);
                        wastagetotal += wastage;
                        double cutting = 0;
                        double.TryParse(dr["cuttingfilm"].ToString(), out cutting);
                        cuttingtotal += cutting;
                        double consumption = 0;
                        double.TryParse(dr["consumption_film"].ToString(), out consumption);
                        consumptiontotal += consumption;
                        double RecivedFilm = 0;
                        double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                        receivedtotal += RecivedFilm;
                        double ReturnFilm = 0;
                        double.TryParse(dr["return_film"].ToString(), out ReturnFilm);
                        returnfilmtotal += ReturnFilm;
                        Report.Rows.Add(newrow);
                    }
                }
                DataRow newvartical6 = Report.NewRow();
                newvartical6["Product Name"] = "Total";
                newvartical6["Received Film"] = Math.Round(receivedtotal, 2);
                newvartical6["Consumption Film"] = Math.Round(consumptiontotal, 2);
                newvartical6["Wastage Film"] = Math.Round(wastagetotal, 2);
                newvartical6["Cutting Film"] = Math.Round(cuttingtotal, 2);
                newvartical6["Return Film"] = Math.Round(returnfilmtotal, 2);
                Report.Rows.Add(newvartical6);

                DataRow newvartical7 = Report.NewRow();
                totalwastage = (wastagetotal / consumptiontotal) * 100;
                totalwastage = Math.Round(totalwastage, 2);
                newvartical7["Cutting Film"] = "Total Wastage Film(%)";
                newvartical7["Film Wastage(%)"] = totalwastage;
                Report.Rows.Add(newvartical7);

                DataRow newvartical8 = Report.NewRow();
                totalcuttingwastage = (cuttingtotal / consumptiontotal) * 100;
                totalcuttingwastage = Math.Round(totalcuttingwastage, 2);
                newvartical8["Cutting Film"] = "Total Cutting Film(%)";
                newvartical8["Film Wastage(%)"] = totalcuttingwastage;
                Report.Rows.Add(newvartical8);

                DataRow newvartical9 = Report.NewRow();
                overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                overallwastage = Math.Round(overallwastage, 2);
                newvartical9["Cutting Film"] = "Over All Film Wastage(%)";
                newvartical9["Film Wastage(%)"] = overallwastage;
                Report.Rows.Add(newvartical9);

                grdmnthReports.DataSource = Report;
                grdmnthReports.DataBind();
                Session["xportdata"] = Report;
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
                grdmnthReports.DataSource = null;
                grdmnthReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}