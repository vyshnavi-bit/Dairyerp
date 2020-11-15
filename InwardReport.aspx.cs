using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class InwardReport : System.Web.UI.Page
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
            //cmd = new SqlCommand("SELECT milktransactions.dcno AS DCNo,  milktransactions.inwardno as InwardNo, milktransactions.vehicleno ,processingdept.deptname AS DeptName, milktransactions.transtype as Type, milktransactions.qty_ltr AS QtyLtr, milktransactions.qty_kgs AS QtyKgs,  milktransactions.percentageon AS PercentageOn, milktransactions.snf AS SNF, milktransactions.fat AS FAT, milktransactions.clr AS CLR, milktransactions.cob AS COB, milktransactions.hs AS HS, milktransactions.phosps as Phosps, milktransactions.alcohol as Alcohol, milktransactions.temp AS Temp, processingdept_1.deptname AS SectionName, milktransactions.remarks, milktransactions.qco, milktransactions.chemist FROM milktransactions INNER JOIN processingdept ON milktransactions.transid = processingdept.sno INNER JOIN processingdept processingdept_1 ON milktransactions.sectionid = processingdept_1.sno WHERE (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) ");
            cmd = new SqlCommand("SELECT CONVERT(VARCHAR(10), milktransactions.doe, 103) AS Date, milktransactions.dcno AS DCNo, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno AS VehicleNo,milktransactions.qty_kgs AS KGS, milktransactions.qty_ltr AS LTR, milktransactions.snf AS SNF, milktransactions.fat AS FAT, milktransactions.clr AS CLR, milktransactions.cob1 AS COB, milktransactions.hs AS HS, milktransactions.phosps1 AS Phosps, milktransactions.alcohol AS Alcohol, milktransactions.temp AS Temp, milktransactions.acidity AS Acidity, milktransactions.mbrt AS MBRT,vendors.vendorname as VendorName,  milktransactions.qco AS QCO, milktransactions.chemist AS Chemist FROM milktransactions INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransactions.branchid = @branchid)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@transtype", "in");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtInward.Rows.Count > 0)
            {
                grdReports.DataSource = dtInward;
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