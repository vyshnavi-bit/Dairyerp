using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class variationreport : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
    string BranchType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            BranchID = Session["Branch_ID"].ToString();
            BranchType = Session["BranchType"].ToString();
            vdm = new SalesDBManager();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    dtp_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    dtp_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                    fillvendors();
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

    void fillvendors()
    {
        if (BranchType == "Plant")
        {
            cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE    (branchmapping.superbranch = @branchid)");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "vendorname";
            ddlbranches.DataValueField = "sno";
            ddlbranches.DataBind();
        }
        else
        {
            cmd = new SqlCommand("SELECT vendors.vendorname, vendors.sno FROM branch_info INNER JOIN vendors ON branch_info.venorid = vendors.sno WHERE (branch_info.sno = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "vendorname";
            ddlbranches.DataValueField = "sno";
            ddlbranches.DataBind();
        }
    }

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
            Report.Columns.Add("Vendor Name");
            Report.Columns.Add("DCNo");
            Report.Columns.Add("Cell Type");
            Report.Columns.Add("DATE");
            Report.Columns.Add("Inward Qty(KGS)").DataType = typeof(Double);
            Report.Columns.Add("Inward Qty(LTRS)").DataType = typeof(Double);
            Report.Columns.Add("Dispatch Qty(KGS)");
            Report.Columns.Add("Dispatch Qty(LTRS)");
            Report.Columns.Add("Diff qty(kgs)");
            Report.Columns.Add("Diff qty(ltrs)");
            cmd = new SqlCommand("SELECT v.vendorname, milktransactions.dcno, milktransactions.partydcno, milktransactions.doe, milktransactions.entrydate, milktransactions.qty_ltr AS Inwardqtyltrs, milktransactions.qty_kgs AS Inwardqtykgs, ds.qty_ltr, ds.qty_kgs, milktransactions.cellno FROM milktransactions INNER JOIN despatch_entry AS DE ON DE.sno = milktransactions.dcno LEFT OUTER JOIN despatch_sub AS ds ON ds.desp_refno = milktransactions.dcno INNER JOIN vendors v on v.sno= milktransactions.sectionid WHERE (milktransactions.sectionid = @sectionid) AND (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.dcno = ds.desp_refno) AND (milktransactions.cellno = ds.cellname)");
           // cmd = new SqlCommand("SELECT DISTINCT v.vendorname, m.partydcno, m.doe, ds.qty_ltr, ds.qty_kgs, m.cellno, m.qty_ltr AS Inwardqtyltrs, m.qty_kgs AS Inwardqtykgs FROM despatch_entry AS de INNER JOIN despatch_sub AS ds ON ds.desp_refno = de.sno INNER JOIN milktransactions AS m ON m.partydcno = de.dc_no INNER JOIN vendors AS v ON v.sno = m.sectionid WHERE (m.entrydate BETWEEN @d1 AND @d2) and m.sectionid=@sectionid");
            //cmd = new SqlCommand("SELECT silo_inward_transaction.sno, silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs, silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName FROM silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId WHERE (silo_inward_transaction.branchid = @branchID) AND (silo_inward_transaction.date BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtInward.Rows.Count > 0)
            {
                int i = 1;
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double Ltrstotal = 0;
                foreach (DataRow dr in dtInward.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    newrow["Vendor Name"] = dr["vendorname"].ToString();
                    newrow["DCNo"] = dr["partydcno"].ToString();
                    newrow["Cell Type"] = dr["cellno"].ToString();
                    double Kgs = 0;
                    double.TryParse(dr["Inwardqtykgs"].ToString(), out Kgs);
                    newrow["Inward Qty(KGS)"] = Kgs;
                    kgstotal += Kgs;
                    double ltrs = 0;
                    double.TryParse(dr["Inwardqtyltrs"].ToString(), out ltrs);
                    Ltrstotal += ltrs;
                    newrow["Inward Qty(LTRS)"] = ltrs;

                    double dispatchKgs = 0;
                    double.TryParse(dr["qty_kgs"].ToString(), out dispatchKgs);
                    newrow["Dispatch Qty(KGS)"] = dispatchKgs;
                    kgstotal += dispatchKgs;
                    double dispatchltrs = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out dispatchltrs);
                    Ltrstotal += dispatchltrs;
                    newrow["Dispatch Qty(LTRS)"] = dispatchltrs;
                    double variationqtykgs = 0;
                    double variationqtyltrs = 0;
                    if (Kgs > dispatchKgs)
                    {
                        variationqtykgs = Kgs - dispatchKgs;
                    }
                    else
                    {
                        variationqtykgs = dispatchKgs - Kgs;
                    }
                    if (ltrs > dispatchltrs)
                    {
                        variationqtyltrs = ltrs - dispatchltrs;
                    }
                    else
                    {
                        variationqtyltrs = dispatchltrs - ltrs;
                    }
                    newrow["Diff qty(kgs)"] = variationqtykgs;

                    newrow["Diff qty(ltrs)"] = variationqtyltrs;

                    //double.TryParse(dr["fat"].ToString(), out fat);
                    //newrow["FAT"] = fat;
                    //double snf = 0;
                    //double.TryParse(dr["snf"].ToString(), out snf);
                    //newrow["SNF"] = snf;
                    //newrow["CLR"] = dr["clr"].ToString();
                    Report.Rows.Add(newrow);
                }
                //DataRow newvartical2 = Report.NewRow();
                //newvartical2["DATE"] = "Total";
                //newvartical2["KGS"] = kgstotal;
                //newvartical2["LTRS"] = Ltrstotal;
                //double fattotal = 0;
                //fattotal = (kgfattotal / Ltrstotal) * 100;
                //fattotal = Math.Round(fattotal, 2);
                //newvartical2["FAT"] = fattotal;
                //double snftotal = 0;
                //snftotal = (kgsnftotal / Ltrstotal) * 100;
                //snftotal = Math.Round(snftotal, 2);
                //newvartical2["SNF"] = snftotal;
                //newvartical2["LTR FAT"] = kgfattotal;
                //newvartical2["LTR SNF"] = kgsnftotal;
                //Report.Rows.Add(newvartical2);
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

    protected void gvMenu_DataBinding(object sender, EventArgs e)
    {
        try
        {
            //GridViewGroup First = new GridViewGroup(grdReports, null, "Vendor Name");
            //GridViewGroup Second = new GridViewGroup(grdReports, First, "DCNo");
            //GridViewGroup thired = new GridViewGroup(grdReports, Second, "Cell Type");
            //GridViewGroup four = new GridViewGroup(grdReports, thired, "DATE");
            //GridViewGroup five = new GridViewGroup(grdReports, four, "Inward Qty(KGS)");
            //GridViewGroup six = new GridViewGroup(grdReports, five, "Inward Qty(LTRS)");
            //GridViewGroup seveen = new GridViewGroup(grdReports, six, "Dispatch Qty(KGS)");
            //GridViewGroup eight = new GridViewGroup(grdReports, seveen, "Dispatch Qty(LTRS)");
            //GridViewGroup nine = new GridViewGroup(grdReports, eight, "Diff qty(kgs)");
            //GridViewGroup ten = new GridViewGroup(grdReports, nine, "Diff qty(ltrs)");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}