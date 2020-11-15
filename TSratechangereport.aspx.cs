using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class TSratechangereport : System.Web.UI.Page
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
            Session["filename"] = "TS Rate Change Details";
            Session["title"] = "TS Rate Change Details";
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
            Report.Columns.Add("Vendor Name");
            Report.Columns.Add("Branch Name");
            Report.Columns.Add("EnteredBy");
            Report.Columns.Add("From Date");
            Report.Columns.Add("To Date");
            Report.Columns.Add("Prev_Rate");
            Report.Columns.Add("New_Rate");
            Report.Columns.Add("Rate_On");
            Report.Columns.Add("Calc_On");
            Report.Columns.Add("Approved By");
            Report.Columns.Add("Approve Date");
            Report.Columns.Add("Remarks");
            cmd = new SqlCommand("SELECT vendor_ts_rate_logs.sno, vendor_ts_rate_logs.vendor_sno, vendors.vendorname, branch_info.branchname, employee_erp.empname, employee_erp_1.empname as ApprovedName, vendor_ts_rate_logs.fromdate, vendor_ts_rate_logs.todate, vendor_ts_rate_logs.prev_rate, vendor_ts_rate_logs.new_rate, vendor_ts_rate_logs.doe,vendor_ts_rate_logs.branchid, vendor_ts_rate_logs.entry_by, vendor_ts_rate_logs.rate_on, vendor_ts_rate_logs.calc_on, vendor_ts_rate_logs.status, vendor_ts_rate_logs.approved_by, vendor_ts_rate_logs.Approve_date, vendor_ts_rate_logs.Remarks, vendor_ts_rate_logs.app_remarks FROM vendor_ts_rate_logs INNER JOIN vendors ON vendor_ts_rate_logs.vendor_sno = vendors.sno INNER JOIN branch_info ON vendor_ts_rate_logs.branchid = branch_info.sno INNER JOIN employee_erp ON vendor_ts_rate_logs.entry_by = employee_erp.sno INNER JOIN employee_erp AS employee_erp_1 ON vendor_ts_rate_logs.approved_by = employee_erp_1.sno  WHERE  (vendor_ts_rate_logs.doe BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            DataTable dtDispatch = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtDispatch.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Vendor Name"] = dr["vendorname"].ToString();
                    newrow["Branch Name"] = dr["branchname"].ToString();
                    newrow["EnteredBy"] = dr["empname"].ToString();
                    newrow["From Date"] = dr["fromdate"].ToString();
                    newrow["To Date"] = dr["todate"].ToString();
                    newrow["Prev_Rate"] = dr["prev_rate"].ToString();
                    newrow["New_Rate"] = dr["new_rate"].ToString();
                    newrow["Rate_On"] = dr["rate_on"].ToString();
                    newrow["Calc_On"] = dr["calc_on"].ToString();
                    newrow["Approved By"] = dr["ApprovedName"].ToString();
                    newrow["Approve Date"] = dr["Approve_date"].ToString();
                    newrow["Remarks"] = dr["remarks"].ToString();
                    Report.Rows.Add(newrow);
                  
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
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