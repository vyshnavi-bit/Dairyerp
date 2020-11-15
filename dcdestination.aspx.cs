using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class dcdestination : System.Web.UI.Page
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
                    //lbltinNo.Text = Session["TinNo"].ToString();
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
            Session["filename"] = "DC Report";
            Session["title"] = "DC Root Details";
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
            lbltoDate.Text = todate.ToString("dd/MMM/yyyy");
            Report.Columns.Add("Sno");
            Report.Columns.Add("DC NO");
            Report.Columns.Add("Source");
            Report.Columns.Add("Destination");
            Report.Columns.Add("VehicleNo");
            Report.Columns.Add("Desp Date");
            Report.Columns.Add("Desp Time");
            Report.Columns.Add("ModeType");
            Report.Columns.Add("Date");
            Report.Columns.Add("Time");
            if (BranchID == "1" || BranchID == "26")
            {
                cmd = new SqlCommand("SELECT despatch_entry.sno, despatch_entry.dc_no, vendors.vendorname, despatch_entry.doe, despatch_entry.vehciecleno, despatch_entry.desp_time,  despatch_entry.salestype  FROM  despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno WHERE (despatch_entry.doe BETWEEN @d1 AND @d2) AND (vendors.branchid = @branchid) order by despatch_entry.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
            }
            else
            {
                cmd = new SqlCommand("SELECT despatch_entry.sno, despatch_entry.dc_no, vendors.vendorname, despatch_entry.doe, despatch_entry.vehciecleno, despatch_entry.desp_time,  despatch_entry.salestype  FROM  despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno WHERE (despatch_entry.doe BETWEEN @d1 AND @d2) AND (despatch_entry.branchid = @branchid) order by despatch_entry.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
               
            }
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtDispatch = vdm.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtDispatch.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["DC NO"] = dr["dc_no"].ToString();
                    newrow["Source"] = dr["vendorname"].ToString();
                    
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
                    cmd = new SqlCommand("SELECT sno, transid, dcno, partydcno, entrydate, doe, entrydate, status FROM milktransactions WHERE (dcno=@dcno)");
                    cmd.Parameters.Add("@dcno", dr["sno"].ToString());
                    DataTable dtinword = vdm.SelectQuery(cmd).Tables[0];
                    if (dtinword.Rows.Count > 0)
                    {
                        foreach (DataRow drinward in dtinword.Rows)
                        {
                            DateTime dtdoe = Convert.ToDateTime(drinward["entrydate"].ToString());
                            string strinwardtime = dtdoe.ToString("dd/MMM/yyyy HH:mm");
                            string[] sinwardDateTime = strinwardtime.Split(' ');
                            string[] PlaninwardDateTime = strinwardtime.Split(' ');
                            newrow["Date"] = sinwardDateTime[0];
                            newrow["Time"] = PlaninwardDateTime[1];
                            newrow["ModeType"] = "Inward";
                            newrow["Destination"] = dr["salestype"].ToString();
                        }
                    }
                    cmd = new SqlCommand("SELECT directsale.sno, directsale.dcno, vendors.vendorname, directsale.doe FROM  directsale INNER JOIN vendors ON directsale.toccid = vendors.sno WHERE (dcno=@ddcno)");
                    cmd.Parameters.Add("@ddcno", dr["sno"].ToString());
                    DataTable dtdirectsale = vdm.SelectQuery(cmd).Tables[0];
                    if (dtdirectsale.Rows.Count > 0)
                    {
                        foreach (DataRow drdirect in dtdirectsale.Rows)
                        {
                            DateTime dtdoe = Convert.ToDateTime(drdirect["doe"].ToString());
                            string strdirecttime = dtdoe.ToString("dd/MMM/yyyy HH:mm");
                            string[] sdirectDateTime = strdirecttime.Split(' ');
                            string[] PlantdirectDateTime = strdirecttime.Split(' ');
                            newrow["Date"] = sdirectDateTime[0];
                            newrow["Time"] = PlantdirectDateTime[1];
                            newrow["ModeType"] = "Direct Sale";
                            newrow["Destination"] = drdirect["vendorname"].ToString();
                        }
                    }
                    Report.Rows.Add(newrow);
                }
                grdreport.DataSource = Report;
                grdreport.DataBind();
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