using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class cipreport : System.Web.UI.Page
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
            Report.Columns.Add("Sno");
            Report.Columns.Add("SiloName");
            Report.Columns.Add("Chemical Type");
            Report.Columns.Add("Temperature");
            Report.Columns.Add("Quantity");
            Report.Columns.Add("Start Time");
            Report.Columns.Add("Ending Time");
            Report.Columns.Add("Diff Time");
            Report.Columns.Add("Actual Strength");
            Report.Columns.Add("Date");
            cmd = new SqlCommand("Select cip.sno, sm.SiloName, cip.siloid, cip.chemicaltype, cip.temperature, cip.quantity, cip.starttime, cip.endingtime, cip.actualstreangth, cip.remarks, cip.userid, cip.branchid, cip.doe, bi.branchname from cipcleaningdetails cip INNER JOIN silomaster sm ON sm.SiloId = cip.siloid INNER JOIN branch_info bi on bi.sno = cip.branchid  WHERE (cip.doe BETWEEN @d1 AND @d2) and cip.branchid=@branchid");
            DateTime dtfrom = GetLowDate(fromdate);
            DateTime dtfromdate = dtfrom.AddHours(6);
            DateTime dtto = GetLowDate(todate);
            DateTime dttodate = dtto.AddHours(30);
            cmd.Parameters.Add("@d1", dtfromdate);
            cmd.Parameters.Add("@d2", dttodate);
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtInward.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtInward.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime doe = Convert.ToDateTime(dr["doe"].ToString());
                    string dte = doe.ToString("dd/MM/yyyy");
                    newrow["Date"] = dte;
                    DateTime dtdoe = Convert.ToDateTime(dr["starttime"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    string strPlantime = dtdoe.ToString("dd/MMM/yyyy HH:mm");
                    string[] PlanDateTime = strPlantime.Split(' ');
                    //newrow["Date"] = PlanDateTime[0];
                    newrow["Start Time"] = PlanDateTime[1];
                    DateTime time = Convert.ToDateTime(dr["endingtime"].ToString());
                    string Edate = time.ToString("dd/MM/yyyy");
                    string strendtime = time.ToString("dd/MMM/yyyy HH:mm");
                    string[] Planendtime = strendtime.Split(' ');
                   // newrow["Date"] = Planendtime[0];
                    newrow["Ending Time"] = Planendtime[1];
                    newrow["SiloName"] = dr["SiloName"].ToString();
                    newrow["Chemical Type"] = dr["chemicaltype"].ToString();
                    newrow["Temperature"] = dr["temperature"].ToString();
                    newrow["Quantity"] = dr["quantity"].ToString();
                   // newrow["Cutting"] = dr["cutting"].ToString();
                    newrow["Actual Strength"] = dr["actualstreangth"].ToString();
                    TimeSpan datediff = time - dtdoe;
                    newrow["Diff Time"] = datediff;
                    Report.Rows.Add(newrow);
                }
                //DataRow New1 = Report.NewRow();
                //New1["To Silo"] = "SMP";
                //New1["KGS"] = TOTsmp;
                //New1["LTR SNF"] = smptotal;
                //Report.Rows.Add(New1);
                //DataRow newvartical2 = Report.NewRow();
                //newvartical2["To Silo"] = "Total";
                //newvartical2["KGS"] = kgstotal;
                //newvartical2["LTRS"] = Ltrstotal;
                //double fattotal = 0;
                //fattotal = (kgfattotal / Ltrstotal) * 100;
                //fattotal = Math.Round(fattotal, 2);
                //newvartical2["FAT"] = fattotal;
                //double snftotal = 0;
                //double smpkgsnftotal = 0;
                //smpkgsnftotal = smptotal + kgsnftotal;
                //snftotal = (smpkgsnftotal / Ltrstotal) * 100;
                //snftotal = Math.Round(snftotal, 2);
                //newvartical2["SNF"] = snftotal;
                //newvartical2["LTR FAT"] = kgfattotal;
                //newvartical2["LTR SNF"] = smpkgsnftotal;
                ////newvartical2["SMP"] = smptotal;
                //Report.Rows.Add(newvartical2);
                grdReports.DataSource = Report;
                grdReports.DataBind();
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
                hidepanel.Visible = false;
                grdReports.DataSource = null;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
}