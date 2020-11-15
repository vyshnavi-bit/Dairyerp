using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class smppreparationreport : System.Web.UI.Page
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
                    txtfromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
            string[] datestrig = txtfromdate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = txttodate.Text.Split(' ');
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
            double totalob = 0;
            double totalrecive = 0;
            double totaltotal = 0;
            double totalconsumption = 0;
            double totalstocktransfor = 0;
            double totalcb = 0;

            Report.Columns.Add("sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("OB");
            Report.Columns.Add("Received Qty");
            Report.Columns.Add("Total");
            Report.Columns.Add("Consumption Qty");
            Report.Columns.Add("Stock Transfer");
            Report.Columns.Add("CB");
            //Report.Columns.Add("total");
            cmd = new SqlCommand("SELECT sno, qty_kgs as consumptionqty, ob, fat, snf, date, recivedqty, branchid, stocktransfor, enterby FROM  smp_details WHERE  (date BETWEEN @d1 AND @d2) AND (branchid = @BranchID) ORDER BY date");
            //cmd.Parameters.Add("@branchid", BranchID);
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);    
            DataTable dttotalsmp = SalesDB.SelectQuery(cmd).Tables[0];
            if (dttotalsmp.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dttotalsmp.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["date"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["Date"] = date;
                    newrow["OB"] = dr["ob"].ToString();
                    newrow["Received Qty"] = dr["recivedqty"].ToString();
                    newrow["Consumption Qty"] = dr["consumptionqty"].ToString();
                    newrow["Stock Transfer"] = dr["stocktransfor"].ToString();
                    double ob = Convert.ToDouble(dr["ob"].ToString());
                    totalob += ob;
                    double recived = Convert.ToDouble(dr["recivedqty"].ToString());
                    totalrecive += recived;
                    double consumption = Convert.ToDouble(dr["consumptionqty"].ToString());
                    totalconsumption += consumption;
                    double stocktransfor = Convert.ToDouble(dr["stocktransfor"].ToString());
                    totalstocktransfor += stocktransfor;
                    double total = ob + recived;
                    totaltotal += total;
                    newrow["Total"] = total.ToString();
                    double con = consumption + stocktransfor;
                    double cb = total - con;
                    totalcb += cb;
                    newrow["CB"] = cb;
                    //double total = Convert.ToDouble(dr["total"].ToString());
                    //newrow["total"] = total;
                    Report.Rows.Add(newrow);
                }
                
                DataRow newrow1 = Report.NewRow();
                newrow1["date"] = "Total";
                //newrow1["OB"] = totalob;
                newrow1["Received Qty"] = totalrecive;
                newrow1["Consumption Qty"] = totalconsumption;
                newrow1["Stock Transfer"] = totalstocktransfor;
                //newrow1["Total"] = totaltotal;
                //newrow1["CB"] = totalcb;
                Report.Rows.Add(newrow1);
                grdReports.DataSource = Report;
                grdReports.DataBind();
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


}