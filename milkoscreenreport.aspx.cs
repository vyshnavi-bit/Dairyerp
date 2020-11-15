using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class milkoscreenreport : System.Web.UI.Page
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
                    //tdvehicleno.Visible = false;
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
            Report.Columns.Add("Sno");
            Report.Columns.Add("Vehicle Type");
            Report.Columns.Add("Vehicle No");
            Report.Columns.Add("Cell Type");
            Report.Columns.Add("Milko Screen FAT");
            Report.Columns.Add("Milko Screen SNF");
            Report.Columns.Add("Milko Screen Protine");
            Report.Columns.Add("Manual FAT");
            Report.Columns.Add("Manual SNF");
            Report.Columns.Add("Route Name");
            Report.Columns.Add("Date");
            Report.Columns.Add("Transaction No");
            Report.Columns.Add("DC fat");
            Report.Columns.Add("DC snf");
            Report.Columns.Add("DC No");
           // Report.Columns.Add("Status");

            cmd = new SqlCommand("SELECT sno, vehicletype, vehicleno, celltype, branchid, doe, milkofat, milkosnf, milkoprotine, manualfat, manualsnf, manualprotine, entryby, routename, transactionno, dcfat,  dcsnf, dcno, status, remorks FROM  milkoscreendetails WHERE (doe BETWEEN @d1 AND @d2) AND (branchid=@branchid) ORDER BY sno desc");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtweight = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtweight.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtweight.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Vehicle Type"] = dr["vehicletype"].ToString();
                    newrow["Vehicle No"] = dr["vehicleno"].ToString();
                    newrow["Cell Type"] = dr["celltype"].ToString();
                    newrow["Milko Screen FAT"] = dr["milkofat"].ToString();
                    newrow["Milko Screen SNF"] = dr["milkosnf"].ToString();
                    newrow["Milko Screen Protine"] = dr["milkoprotine"].ToString();
                    newrow["Manual FAT"] = dr["manualfat"].ToString();
                    newrow["Manual SNF"] = dr["manualsnf"].ToString();
                    newrow["Route Name"] = dr["routename"].ToString();
                    newrow["Transaction No"] = dr["transactionno"].ToString();
                    newrow["DC fat"] = dr["dcfat"].ToString();
                    newrow["DC snf"] = dr["dcsnf"].ToString();
                    newrow["DC No"] = dr["dcno"].ToString();
                   // newrow["Status"] = dr["status"].ToString();
                    string date = dr["doe"].ToString();
                    newrow["Date"] = date;
                    Report.Rows.Add(newrow);

                }
                //DataRow newvartical = Report.NewRow();
                //newvartical["Weight Type"] = "Total";
                //double val = 0.0;
                //foreach (DataColumn dc in Report.Columns)
                //{
                //    if (dc.DataType == typeof(Double))
                //    {
                //        val = 0.0;
                //        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                //        newvartical[dc.ToString()] = val;
                //    }
                //}
               // Report.Rows.Add(newvartical);
                grdReports.DataSource = Report;
                grdReports.DataSource = Report;
                grdReports.DataBind();
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
                hidepanel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
}