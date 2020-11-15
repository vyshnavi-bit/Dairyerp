using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class tankerdiffreport : System.Web.UI.Page
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
            Session["filename"] = "Tanker Difference ";
            Session["title"] = "Tanker Difference Details";
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
            Report.Columns.Add("CC Name");
            Report.Columns.Add("Dispatch Quantity_Kgs");
            Report.Columns.Add("Inward Quantity_Kgs");
            Report.Columns.Add("Diff Quantity(kgs)");
            
            Report.Columns.Add("Dispatch FAT");
            Report.Columns.Add("Inward FAT");
            Report.Columns.Add("Diff FAT");

            Report.Columns.Add("Dispatch SNF");
            Report.Columns.Add("Inward SNF");
            Report.Columns.Add("Diff SNF");

            // cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf,milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) ORDER BY doe");               
            cmd = new SqlCommand("SELECT despatch_entry.sno from despatch_entry INNER JOIN branchmapping ON despatch_entry.branchid = branchmapping.subbranch WHERE (despatch_entry.doe BETWEEN @d1 AND @d2) AND (branchmapping.superbranch=@branchid)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtDispatch = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtDispatch.Rows)
                {
                    double inword_qty_kgs = 0;
                    double inword_qty_ltr = 0;
                    double inword_FAT = 0;
                    double inword_snf = 0;
                    double inwordtotalqtykgs = 0;
                    double inwordtotalqtyltrs = 0;
                    double dispatchtotalqtykgs = 0;
                    double dispatchtotalqtyltrs = 0;
                    double qty_kgs = 0;
                    double FAT = 0;
                    double SNF = 0;
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = dr["sno"].ToString();
                    string sno = dr["sno"].ToString();
                    cmd = new SqlCommand("SELECT vendors.vendorname, despatch_entry.sno, despatch_entry.doe, despatch_sub.fat, despatch_sub.snf, despatch_sub.qty_kgs, despatch_sub.qty_ltr FROM despatch_entry INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno inner join vendors ON vendors.sno = despatch_entry.cc_id WHERE despatch_entry.sno=@sno");
                    cmd.Parameters.Add("@sno", sno);
                    DataTable dtdispatch = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtdispatch.Rows.Count > 0)
                    {
                        foreach (DataRow drdispatch in dtdispatch.Rows)
                        {
                            DateTime dtdoe = Convert.ToDateTime(drdispatch["doe"].ToString());
                            string date = dtdoe.ToString("dd/MMM/yyyy");
                            newrow["DATE"] = date;
                            newrow["CC Name"] = drdispatch["vendorname"].ToString();

                            double.TryParse(drdispatch["qty_kgs"].ToString(), out qty_kgs);
                            dispatchtotalqtykgs += qty_kgs;
                            newrow["Dispatch Quantity_Kgs"] = dispatchtotalqtykgs;


                            double.TryParse(drdispatch["fat"].ToString(), out FAT);
                            FAT = Math.Round(FAT, 2);
                            newrow["Dispatch FAT"] = FAT;


                            double.TryParse(drdispatch["snf"].ToString(), out SNF);
                            newrow["Dispatch SNF"] = SNF;
                   
                        }
                    }
                    
                    cmd = new SqlCommand("SELECT milktransactions.qty_ltr AS inwordqtyltr, milktransactions.qty_kgs AS inwordqtykgs, milktransactions.snf AS inwordsnf, milktransactions.fat AS inwordfat FROM milktransactions WHERE dcno=@dcno");
                    cmd.Parameters.Add("@dcno", sno);
                    DataTable dtmilk = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtmilk.Rows.Count > 0)
                    {
                        foreach (DataRow drr in dtmilk.Rows)
                        {

                            double.TryParse(drr["inwordqtykgs"].ToString(), out inword_qty_kgs);
                            inwordtotalqtykgs += inword_qty_kgs;
                            newrow["Inward Quantity_Kgs"] = inwordtotalqtykgs;
                            double.TryParse(drr["inwordqtyltr"].ToString(), out inword_qty_ltr);
                            //newrow["Inword Quantity_LTRS"] = dr["inwordqtyltr"].ToString();
                            double.TryParse(drr["inwordfat"].ToString(), out inword_FAT);
                            newrow["Inward FAT"] = drr["inwordfat"].ToString();
                            double.TryParse(drr["inwordsnf"].ToString(), out inword_snf);
                            newrow["Inward SNF"] = drr["inwordsnf"].ToString();
                        }
                    }
                    double difffat = FAT - inword_FAT;
                    double diffsnf = SNF - inword_snf;
                    double diffqty = dispatchtotalqtykgs - inwordtotalqtykgs;
                    newrow["Diff SNF"] = Math.Round(diffsnf, 2);
                    newrow["Diff FAT"] = Math.Round(difffat, 2);
                    newrow["Diff Quantity(kgs)"] = Math.Round(diffqty, 2);
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