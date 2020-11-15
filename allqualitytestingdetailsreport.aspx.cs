using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class allqualitytestingdetailsreport : System.Web.UI.Page
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
                Report.Columns.Add("SNO");
                Report.Columns.Add("Silo Name");
                // Report.Columns.Add("Creamtype");
                Report.Columns.Add("Qty(kgs)");
                Report.Columns.Add("Qty(ltrs)");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("Date");
                cmd = new SqlCommand("SELECT sqt.sno, sqt.time, sqt.sampleno, sqt.siloid, sqt.qty_ltrs, sqt.qty_kgs, sqt.percentageon, sqt.snf, sqt.fat, sqt.clr, sqt.hs, sqt.alcohol, sqt.remarks, sqt.chemist, sqt.qco, sqt.temp, sqt.doe, sqt.branchid, sqt.createdby, sqt.cob1, sqt.phosps1, sqt.mbrt,  sqt.acidity, sqt.ot, sqt.neutralizers, sqt.createdon, sqt.status, sm.SiloName FROM  qualitytesting_details sqt INNER JOIN silomaster sm on sm.SiloId = sqt.siloid WHERE (sqt.createdon between @d1 and @d2) AND (sqt.branchid=@branchid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtsilo = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtsilo.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dtsilo.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["createdon"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Silo Name"] = dr["SiloName"].ToString();
                        newrow["Qty(kgs)"] = dr["qty_kgs"].ToString();
                        newrow["Qty(ltrs)"] = dr["qty_ltrs"].ToString();
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
            if (ddltestingtype.SelectedItem.Text == "Batch")
            {
                Report.Columns.Add("sno");
                Report.Columns.Add("Batch Name");
                Report.Columns.Add("Qty(kgs)");
                Report.Columns.Add("Qty(ltrs)");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("Date");
                cmd = new SqlCommand("SELECT bwq.sno, b.batch, bwq.time, bwq.sampleno, bwq.batchid, bwq.qty_ltrs, bwq.qty_kgs, bwq.percentageon, bwq.snf, bwq.fat, bwq.clr, bwq.hs, bwq.alcohol, bwq.remarks, bwq.chemist, bwq.qco, bwq.temp, bwq.doe, bwq.branchid, bwq.createdby, bwq.cob1, bwq.phosps1, bwq.mbrt,  bwq.acidity, bwq.ot, bwq.neutralizers, bwq.createdon, bwq.status FROM  qualitytesting_details bwq INNER JOIN batchmaster b on b.batchid= bwq.batchid WHERE (bwq.doe BETWEEN @d1 AND @d2) AND (bwq.branchid=@branchid) ORDER BY bwq.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtbatch = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtbatch.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dtbatch.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Batch Name"] = dr["batch"].ToString();
                        newrow["Qty(kgs)"] = dr["qty_kgs"].ToString();
                        newrow["Qty(ltrs)"] = dr["qty_ltrs"].ToString();
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
            if (ddltestingtype.SelectedItem.Text == "Curd Section")
            {
                Report.Columns.Add("SNO");
                Report.Columns.Add("Date");
                Report.Columns.Add("Product Name");
                Report.Columns.Add("Packet Size");
                Report.Columns.Add("Sample No");
                Report.Columns.Add("Use By Date");
                Report.Columns.Add("MRP");
                Report.Columns.Add("Temp");
                cmd = new SqlCommand("SELECT productmaster.productname,cqt.doe, cqt.createdon, cqt.productid, cqt.sampleno, cqt.mrp, cqt.packetsize,  cqt.temp, cqt.usebydate, cqt.sno FROM qualitytesting_details AS cqt INNER JOIN productmaster ON cqt.productid = productmaster.sno WHERE cqt.doe BETWEEN @d1 and @d2 AND  (cqt.branchid=@BranchID) AND (cqt.deptid='1') ORDER BY cqt.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtcurd = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtcurd.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dtcurd.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNO"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Date"] = date;
                        newrow["Product Name"] = dr["productname"].ToString();
                        newrow["Packet Size"] = dr["packetsize"].ToString();
                        newrow["Sample No"] = dr["sampleno"].ToString();
                        DateTime dtusebydate = Convert.ToDateTime(dr["usebydate"].ToString());
                        newrow["Use By Date"] = dtusebydate.ToString("dd/MM/yyyy");
                        newrow["MRP"] = dr["mrp"].ToString();
                        newrow["Temp"] = dr["temp"].ToString();
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
            if (ddltestingtype.SelectedItem.Text == "Ghee Section")
            {
                Report.Columns.Add("SNO");
                Report.Columns.Add("Product Name");
                Report.Columns.Add("Packet Size");
                Report.Columns.Add("Sample No");
                Report.Columns.Add("Use By Date");
                Report.Columns.Add("MRP");
                Report.Columns.Add("Temp");
                cmd = new SqlCommand("SELECT GQT.sno, productmaster.productname, GQT.productid, GQT.sampleno, GQT.createdon, GQT.packetsize, GQT.usebydate, GQT.mrp, GQT.temp, GQT.remarks FROM qualitytesting_details AS GQT INNER JOIN  productmaster ON GQT.productid = productmaster.sno Where GQT.createdon BETWEEN @d1 AND @d2 AND (GQT.branchid=@BranchID) AND (GQT.deptid='3')");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtghee = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtghee.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dtghee.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNO"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["createdon"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Product Name"] = dr["productname"].ToString();
                        newrow["Packet Size"] = dr["packetsize"].ToString();
                        newrow["Sample No"] = dr["sampleno"].ToString();
                        DateTime dtusebydate = Convert.ToDateTime(dr["usebydate"].ToString());
                        newrow["Use By Date"] = dtusebydate.ToString("dd/MM/yyyy");
                        newrow["MRP"] = dr["mrp"].ToString();
                        newrow["Temp"] = dr["temp"].ToString();
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
            if (ddltestingtype.SelectedItem.Text == "Cream")
            {
                Report.Columns.Add("SNO");
                Report.Columns.Add("Date");
                Report.Columns.Add("Crem Type");
                Report.Columns.Add("Qty(Kgs)");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("Temprature");
                Report.Columns.Add("Acidity");
                Report.Columns.Add("Remarks");
                cmd = new SqlCommand("SELECT sno,qty_kgs, qty_ltrs, fat, snf, temp, acidity, remarks, chemist, creamtype, createdby, createdon, doe, branchid FROM  qualitytesting_details WHERE  (creamtype = 'Cow') AND (createdon BETWEEN @d1 AND @d2) AND (branchid = @branchid) OR  (creamtype = 'Buffalo') AND (createdon BETWEEN @d1 AND @d2) AND (branchid = @branchid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dtcream = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtcream.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dtcream.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNO"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Date"] = date;
                        newrow["Crem Type"] = dr["creamtype"].ToString();
                        newrow["Qty(Kgs)"] = dr["qty_kgs"].ToString();
                        newrow["FAT"] = dr["fat"].ToString();
                        newrow["SNF"] = dr["snf"].ToString();
                        newrow["Temprature"] = dr["temp"].ToString();
                        newrow["Acidity"] = dr["acidity"].ToString();
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
                    grdReports.DataSource = null;
                    grdReports.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}