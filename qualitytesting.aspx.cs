using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class qualitytesting : System.Web.UI.Page
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
            string Type = Session["BranchType"].ToString();
            if (ddltestingtype.SelectedItem.Text == "Silo")
            {
                Report.Columns.Add("sno");
                Report.Columns.Add("Silo Name");
                // Report.Columns.Add("Creamtype");
                Report.Columns.Add("Qty(kgs)");
                Report.Columns.Add("Qty(ltrs)");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("Date");
                cmd = new SqlCommand("SELECT sqt.sno, sqt.time, sqt.sample, sqt.siloid, sqt.qty_ltr, sqt.qty_kgs, sqt.percentageon, sqt.snf, sqt.fat, sqt.clr, sqt.hs, sqt.alcohol, sqt.remarks, sqt.chemist, sqt.qco, sqt.temp, sqt.doe, sqt.branchid, sqt.operatedby, sqt.cob1, sqt.phosps1, sqt.mbrt,  sqt.acidity, sqt.ot, sqt.neutralizers, sqt.entrydate, sqt.status, sm.SiloName FROM  silowise_qualitytesting sqt INNER JOIN silomaster sm on sm.SiloId = sqt.siloid WHERE (sqt.entrydate between @d1 and @d2) AND (sqt.branchid=@branchid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
            }
            else
            {
                Report.Columns.Add("sno");
                Report.Columns.Add("Batch Name");
                Report.Columns.Add("Qty(kgs)");
                Report.Columns.Add("Qty(ltrs)");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("Date");
                cmd = new SqlCommand("SELECT bwq.sno, b.batch, bwq.time, bwq.sample, bwq.batchid, bwq.qty_ltr, bwq.qty_kgs, bwq.percentageon, bwq.snf, bwq.fat, bwq.clr, bwq.hs, bwq.alcohol, bwq.remarks, bwq.chemist, bwq.qco, bwq.temp, bwq.doe, bwq.branchid, bwq.operatedby, bwq.cob1, bwq.phosps1, bwq.mbrt,  bwq.acidity, bwq.ot, bwq.neutralizers, bwq.entrydate, bwq.status FROM  batch_wise_qualitytesting bwq INNER JOIN batchmaster b on b.batchid= bwq.batchid WHERE (bwq.entrydate BETWEEN @d1 AND @d2) AND (bwq.branchid=@branchid) ORDER BY bwq.entrydate");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
            }
            double totalproductionqty = 0;
            DataTable dtghee = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtghee.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtghee.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    if (ddltestingtype.SelectedItem.Text == "Silo")
                    {
                        newrow["Silo Name"] = dr["SiloName"].ToString();
                    }
                    else
                    {
                        newrow["Batch Name"] = dr["batch"].ToString();
                    }
                    newrow["Qty(kgs)"] = dr["qty_kgs"].ToString();
                    newrow["Qty(ltrs)"] = dr["qty_ltr"].ToString();
                    newrow["FAT"] = dr["fat"].ToString();
                    newrow["SNF"] = dr["snf"].ToString();
                    newrow["Date"] = date;
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
                grdReports.DataSource = null;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "Ghee Sales Details")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            //if (e.Row.Cells[4].Text == "Grand Total")
            //{
            //    e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
            //    e.Row.Font.Size = FontUnit.Large;
            //    e.Row.Font.Bold = true;
            //}
        }
    }
}