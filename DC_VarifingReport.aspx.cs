using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class DC_VarifingReport : System.Web.UI.Page
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
            Session["filename"] = "Tanker Inward";
            Session["title"] = "Tanker Inward Details";
            lblmsg.Text = "";
            hidepanel.Visible = true;
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
            Report.Columns.Add("CC Name");
            Report.Columns.Add("VehicleNo");
            Report.Columns.Add("Desp Date");
            Report.Columns.Add("Desp Time");
            Report.Columns.Add("Inward Date");
            Report.Columns.Add("Inward Time");
            Report.Columns.Add("Weighing Date");
            Report.Columns.Add("Weighing Time");
            Report.Columns.Add("Lab Date");
            Report.Columns.Add("Lab Time");
            if (BranchID == "1")
            {
                cmd = new SqlCommand("SELECT vendors.vendorname, despatch_entry.sno, despatch_entry.dc_no, despatch_entry.vehciecleno, despatch_entry.doe, despatch_entry.desp_time FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN branchmapping ON despatch_entry.branchid = branchmapping.subbranch WHERE (despatch_entry.doe BETWEEN @d1 AND @d2) AND (branchmapping.superbranch = @branchid) order by despatch_entry.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
            }
            else
            {
                cmd = new SqlCommand("SELECT vendors.vendorname, despatch_entry.sno, despatch_entry.dc_no, despatch_entry.vehciecleno, despatch_entry.doe, despatch_entry.desp_time FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno WHERE (despatch_entry.doe BETWEEN @d1 AND @d2) AND (despatch_entry.branchid = @branchid) order by despatch_entry.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
            }
            DataTable dtDispatch = vdm.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtDispatch.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["CC Name"] = dr["vendorname"].ToString();
                    newrow["VehicleNo"] = dr["vehciecleno"].ToString();
                    if (dr["doe"].ToString() == "")
                    {
                    }
                    else
                    {
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string strPlantime = dtdoe.ToString("dd/MMM/yyyy HH:mm");
                        string[] sDateTime = strPlantime.Split(' ');
                        string[] PlanDateTime = strPlantime.Split(' ');
                        newrow["Desp Date"] = sDateTime[0];
                        newrow["Desp Time"] = PlanDateTime[1];
                    }
                    if (dr["desp_time"].ToString() == "")
                    {
                    }
                    else
                    {
                        DateTime dtinward = Convert.ToDateTime(dr["desp_time"].ToString());
                        string strinwardPlantime = dtinward.ToString("dd/MMM/yyyy HH:mm");
                        string[] inwardDateTime = strinwardPlantime.Split(' ');
                        string[] inwardPlanDateTime = strinwardPlantime.Split(' ');
                        newrow["Inward Date"] = inwardDateTime[0];
                        newrow["Inward Time"] = inwardPlanDateTime[1];
                    }
                    cmd = new SqlCommand("SELECT ticketno, weighttype, doe, entrydate FROM weighbridgedetails WHERE (dctransactionno = @dcno)");
                    cmd.Parameters.Add("@dcno", dr["sno"].ToString());
                    DataTable dtweigh = vdm.SelectQuery(cmd).Tables[0];
                    if (dtweigh.Rows.Count > 0)
                    {
                        string doe = dtweigh.Rows[0]["entrydate"].ToString();
                        string doe1 = doe.ToString();
                        if (doe1 == "")
                        {
                        }
                        else
                        {
                            DateTime dtinward = Convert.ToDateTime(doe1);
                            string strinwardPlantime = dtinward.ToString("dd/MMM/yyyy HH:mm");
                            string[] inwardDateTime = strinwardPlantime.Split(' ');
                            string[] inwardPlanDateTime = strinwardPlantime.Split(' ');
                            newrow["Weighing Date"] = inwardDateTime[0];
                            newrow["Weighing Time"] = inwardPlanDateTime[1];
                        }
                    }
                    cmd = new SqlCommand("SELECT sno, vehicletype, vehicleno, doe FROM  milkoscreendetails WHERE (transactionno = @dcno)");
                    cmd.Parameters.Add("@dcno", dr["sno"].ToString());
                    DataTable dtinwarddc = vdm.SelectQuery(cmd).Tables[0];
                    if (dtinwarddc.Rows.Count > 0)
                    {
                        string doe = dtinwarddc.Rows[0]["doe"].ToString();
                        if (doe == "")
                        {
                        }
                        else
                        {
                            DateTime dtinward = Convert.ToDateTime(doe);
                            string strinwardPlantime = dtinward.ToString("dd/MMM/yyyy HH:mm");
                            string[] inwardDateTime = strinwardPlantime.Split(' ');
                            string[] inwardPlanDateTime = strinwardPlantime.Split(' ');
                            newrow["Lab Date"] = inwardDateTime[0];
                            newrow["Lab Time"] = inwardPlanDateTime[1];
                        }
                    }
                    Report.Rows.Add(newrow);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
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