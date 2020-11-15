using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class samplenowisediffrpt : System.Web.UI.Page
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
            grdReports.DataSource = null;
            grdReports.DataBind();
            Session["filename"] = "Sample Difference Report ";
            Session["title"] = "Sample Difference Details";
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
            Report.Columns.Add("DATE");
            Report.Columns.Add("SampleNo");
           
            Report.Columns.Add("Lab FAT");
            Report.Columns.Add("Audit FAT");
            Report.Columns.Add("Diff FAT");

            Report.Columns.Add("Lab SNF");
            Report.Columns.Add("Audit SNF");
            Report.Columns.Add("Diff SNF");

            cmd = new SqlCommand("SELECT DISTINCT dcno  FROM samplenodetails WHERE (doe BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            DataTable dtdcnos = SalesDB.SelectQuery(cmd).Tables[0];

            cmd = new SqlCommand("SELECT sno, type, celltype, dcno, vehicleno, sampleno, createdby, doe, branchid  FROM  samplenodetails  WHERE        (doe BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            DataTable dtsamples = SalesDB.SelectQuery(cmd).Tables[0];

            cmd = new SqlCommand("SELECT bwq.sno, b.batch, bwq.time, bwq.sampleno, bwq.batchid, bwq.qty_ltrs, bwq.qty_kgs, bwq.percentageon, bwq.snf, bwq.fat, bwq.clr, bwq.hs, bwq.alcohol, bwq.remarks, bwq.chemist, bwq.qco, bwq.temp, bwq.doe, bwq.branchid, bwq.createdby, bwq.cob1, bwq.phosps1, bwq.mbrt,  bwq.acidity, bwq.ot, bwq.neutralizers, bwq.createdon, bwq.status FROM  qualitytesting_details bwq INNER JOIN batchmaster b on b.batchid= bwq.batchid WHERE (bwq.doe BETWEEN @d1 AND @d2) AND (bwq.branchid=@branchid) AND (bwq.batchid='46') ORDER BY bwq.doe DESC");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtaudit = SalesDB.SelectQuery(cmd).Tables[0];

            cmd = new SqlCommand("SELECT bwq.sno, b.batch, bwq.time, bwq.sampleno, bwq.batchid, bwq.qty_ltrs, bwq.qty_kgs, bwq.percentageon, bwq.snf, bwq.fat, bwq.clr, bwq.hs, bwq.alcohol, bwq.remarks, bwq.chemist, bwq.qco, bwq.temp, bwq.doe, bwq.branchid, bwq.createdby, bwq.cob1, bwq.phosps1, bwq.mbrt,  bwq.acidity, bwq.ot, bwq.neutralizers, bwq.createdon, bwq.status FROM  qualitytesting_details bwq INNER JOIN batchmaster b on b.batchid= bwq.batchid WHERE (bwq.doe BETWEEN @d1 AND @d2) AND (bwq.branchid=@branchid) AND (bwq.batchid='45') ORDER BY bwq.doe DESC");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtlab = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtdcnos.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtdcnos.Rows)
                {
                    string dcno = dr["dcno"].ToString();
                    DataRow newrow = Report.NewRow();
                    foreach (DataRow drs in dtsamples.Select("dcno='" + dr["dcno"].ToString() + "'"))
                    {
                        string sampleno = drs["sampleno"].ToString();
                        foreach (DataRow drl in dtlab.Select("sampleno='" + sampleno + "'"))
                        {
                            newrow["Sno"] = dr["sno"].ToString();
                        }
                        foreach (DataRow dra in dtaudit.Select("sampleno='" + sampleno + "'"))
                        {

                        }
                    }

                    double labfat = 0;
                    double labsnf = 0;
                    double auditfat = 0;
                    double auditsnf = 0;
                  
                    
                    string sno = dr["sno"].ToString();
                   
                    
                    //double difffat = FAT - inword_FAT;
                    //double diffsnf = SNF - inword_snf;
                    //double diffqty = dispatchtotalqtykgs - inwordtotalqtykgs;
                    //newrow["Diff SNF"] = Math.Round(diffsnf, 2);
                    //newrow["Diff FAT"] = Math.Round(difffat, 2);
                    //newrow["Diff Quantity(kgs)"] = Math.Round(diffqty, 2);
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