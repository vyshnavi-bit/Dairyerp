using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Total_dc_report : System.Web.UI.Page
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
                    Getnames();
                }
            }
        }
    }
    void Getnames()
    {
        string Type = Session["BranchType"].ToString();
        if (Type == "CC")
        {
            pnlBranch.Visible = false;
        }
        else
        {
            pnlBranch.Visible = true;
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranch.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
            hdn_statename.Visible = false;
            hdn_statenames.Visible = false;
            hdn_companywise.Visible = false;
        }
        else if (ddlBranch.SelectedValue == "Branch Wise")
        {
            hideVehicles.Visible = true;
            hdn_statename.Visible = false;
            hdn_statenames.Visible = false;
            hdn_companywise.Visible = false;
            string Type = Session["BranchType"].ToString();
            if (Type == "CC")
            {
                cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorcode, vendors.vendorname FROM branch_info INNER JOIN vendors ON branch_info.venorid = vendors.sno WHERE  (branch_info.sno = @BranchID)");
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranches.DataSource = dttrips;
                ddlbranches.DataTextField = "vendorname";
                ddlbranches.DataValueField = "sno";
                ddlbranches.DataBind();
            }
            else
            {
                cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorcode, vendors.vendorname FROM     branch_info INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch INNER JOIN vendors ON branch_info.venorid = vendors.sno WHERE  (branchmapping.superbranch = @BranchID)");
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranches.DataSource = dttrips;
                ddlbranches.DataTextField = "vendorname";
                ddlbranches.DataValueField = "sno";
                ddlbranches.DataBind();
            }
        }
        else if (ddlBranch.SelectedValue == "State Wise")
        {
            hideVehicles.Visible = false;
            hdn_statename.Visible = true;
            hdn_statenames.Visible = false;
            hdn_companywise.Visible = false;
            string Type = Session["BranchType"].ToString();
            if (Type == "CC")
            {
                cmd = new SqlCommand("SELECT   sno, statename, createdby, createdon, statecode, ecode, gststatecode FROM   state_master");
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlstatename.DataSource = dttrips;
                ddlstatename.DataTextField = "statename";
                ddlstatename.DataValueField = "sno";
                ddlstatename.DataBind();
            }
            else
            {
                cmd = new SqlCommand("SELECT   sno, statename, createdby, createdon, statecode, ecode, gststatecode FROM   state_master");
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlstatename.DataSource = dttrips;
                ddlstatename.DataTextField = "statename";
                ddlstatename.DataValueField = "sno";
                ddlstatename.DataBind();
            }
        }
        else if (ddlBranch.SelectedValue == "State & Branch Wise")
        {
            hideVehicles.Visible = false;
            hdn_statename.Visible = false;
            hdn_statenames.Visible = true;
            hdn_companywise.Visible = false;
            string Type = Session["BranchType"].ToString();
            if (Type == "CC")
            {
                cmd = new SqlCommand("SELECT   sno, statename, createdby, createdon, statecode, ecode, gststatecode FROM   state_master");
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlststenamechange.DataSource = dttrips;
                ddlststenamechange.DataTextField = "statename";
                ddlststenamechange.DataValueField = "sno";
                ddlststenamechange.DataBind();
            }
            else
            {
                cmd = new SqlCommand("SELECT   sno, statename, createdby, createdon, statecode, ecode, gststatecode FROM   state_master");
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlststenamechange.DataSource = dttrips;
                ddlststenamechange.DataTextField = "statename";
                ddlststenamechange.DataValueField = "sno";
                ddlststenamechange.DataBind();
            }
        }
        else if (ddlBranch.SelectedValue == "Company Wise")
        {
            hideVehicles.Visible = false;
            hdn_statename.Visible = false;
            hdn_statenames.Visible = false;
            hdn_companywise.Visible = true;
        }
    }
    protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        hideVehicles.Visible = true;
        hdn_statename.Visible = false;
        hdn_statenames.Visible = true;
        cmd = new SqlCommand("SELECT  vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.state FROM   branch_info INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch INNER JOIN vendors ON branch_info.venorid = vendors.sno WHERE  (branchmapping.superbranch = @BranchID) AND (vendors.state = @state)");
        cmd.Parameters.Add("@BranchID", BranchID);
        cmd.Parameters.Add("@state", ddlststenamechange.SelectedValue);
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlbranches.DataSource = dttrips;
        ddlbranches.DataTextField = "vendorname";
        ddlbranches.DataValueField = "sno";
        ddlbranches.DataBind();
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
            Report.Columns.Add("VehicleNo");
            Report.Columns.Add("Date");
            Report.Columns.Add("RefDCNo");
            Report.Columns.Add("DCNo");
            Report.Columns.Add("Invoice No");
            Report.Columns.Add("DC Type");
            Report.Columns.Add("Cell");
            Report.Columns.Add("MilkType");
            Report.Columns.Add("FAT");
            Report.Columns.Add("SNF");
            Report.Columns.Add("KGS").DataType = typeof(Double);
            Report.Columns.Add("LTRS").DataType = typeof(Double);
            Report.Columns.Add("CLR");
            Report.Columns.Add("HS");
            Report.Columns.Add("Alcohol");
            Report.Columns.Add("TEMP");
            Report.Columns.Add("COB");
            Report.Columns.Add("Phosps");
            Report.Columns.Add("MBRT");
            if (Type == "CC" || ddlBranch.SelectedValue == "Branch Wise")
            {
                pnlVendor.Visible = true;
            }
            else
            {
                pnlVendor.Visible = false;
                Report.Columns.Add("VendorName");
            }
            Report.Columns.Add("Chemist");
            if (Type == "CC")
            {
                cmd = new SqlCommand("SELECT despatch_entry.vehciecleno AS VehicleNo,despatch_entry.dctype, despatch_entry.invoiceno, despatch_entry.doe AS Date, despatch_entry.sno AS RefDCNo, despatch_entry.dc_no AS DCNo, despatch_sub.cellname AS Cell, despatch_sub.milktype AS MilkType, despatch_sub.fat AS FAT, despatch_sub.snf AS SNF, despatch_sub.qty_ltr AS QTYLtr, despatch_sub.qty_kgs AS QtyKG, despatch_sub.percentageon AS PercentageOn, despatch_sub.clr AS CLR, despatch_sub.hs AS HS, despatch_sub.alcohol AS Alcohol, despatch_sub.temp AS TEMP, despatch_sub.cob1 AS COB, despatch_sub.phosps1 AS Phosps, despatch_sub.mbrt, vendors.vendorname AS VendorName, despatch_entry.chemist AS Chemist, despatch_entry.remarks AS Remarks,despatch_sub.HS FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno WHERE  (despatch_entry.doe BETWEEN @d1 AND @d2) AND (despatch_entry.cc_id = @BranchID)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", Session["VendorID"].ToString());
            }
            else
            {
                if (ddlBranch.SelectedValue == "All")
                {
                    cmd = new SqlCommand("SELECT despatch_entry.vehciecleno AS VehicleNo,despatch_entry.dctype, despatch_entry.invoiceno,despatch_entry.doe AS Date, despatch_entry.sno AS RefDCNo, despatch_entry.dc_no AS DCNo,  despatch_sub.cellname as Cell, despatch_sub.milktype as MilkType, despatch_sub.fat as FAT, despatch_sub.snf as SNF, despatch_sub.qty_ltr as QTYLtr, despatch_sub.qty_kgs as QtyKG, despatch_sub.percentageon as PercentageOn , despatch_sub.clr as CLR, despatch_sub.hs as HS, despatch_sub.alcohol as Alcohol, despatch_sub.temp AS TEMP, despatch_sub.cob1 as COB, despatch_sub.phosps1 as Phosps, despatch_sub.mbrt,vendors.vendorname AS VendorName, despatch_entry.chemist AS Chemist, despatch_entry.remarks AS Remarks,despatch_sub.HS FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN branchmapping ON despatch_entry.branchid = branchmapping.subbranch INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno WHERE (despatch_entry.doe BETWEEN @d1 AND @d2) AND (branchmapping.superbranch = @BranchID)");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@BranchID", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise" || ddlBranch.SelectedValue == "State & Branch Wise")
                {
                    cmd = new SqlCommand("SELECT despatch_entry.vehciecleno AS VehicleNo,despatch_entry.dctype, despatch_entry.invoiceno, despatch_entry.doe AS Date, despatch_entry.sno AS RefDCNo, despatch_entry.dc_no AS DCNo, despatch_sub.cellname AS Cell, despatch_sub.milktype AS MilkType, despatch_sub.fat AS FAT, despatch_sub.snf AS SNF, despatch_sub.qty_ltr AS QTYLtr, despatch_sub.qty_kgs AS QtyKG, despatch_sub.percentageon AS PercentageOn, despatch_sub.clr AS CLR, despatch_sub.hs AS HS, despatch_sub.alcohol AS Alcohol, despatch_sub.temp AS TEMP, despatch_sub.cob1 AS COB, despatch_sub.phosps1 AS Phosps, despatch_sub.mbrt, vendors.vendorname AS VendorName, despatch_entry.chemist AS Chemist, despatch_entry.remarks AS Remarks,despatch_sub.HS FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno WHERE  (despatch_entry.doe BETWEEN @d1 AND @d2) AND (despatch_entry.cc_id = @BranchID)");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@BranchID", ddlbranches.SelectedValue);
                }
                else if (ddlBranch.SelectedValue == "State Wise")
                {
                    cmd = new SqlCommand("SELECT despatch_entry.vehciecleno AS VehicleNo, despatch_entry.dctype, despatch_entry.invoiceno, despatch_entry.doe AS Date, despatch_entry.sno AS RefDCNo, despatch_entry.dc_no AS DCNo, despatch_sub.cellname AS Cell, despatch_sub.milktype AS MilkType, despatch_sub.fat AS FAT, despatch_sub.snf AS SNF, despatch_sub.qty_ltr AS QTYLtr, despatch_sub.qty_kgs AS QtyKG, despatch_sub.percentageon AS PercentageOn, despatch_sub.clr AS CLR, despatch_sub.hs AS HS, despatch_sub.alcohol AS Alcohol, despatch_sub.temp AS TEMP, despatch_sub.cob1 AS COB,  despatch_sub.phosps1 AS Phosps, despatch_sub.mbrt, vendors.vendorname AS VendorName, despatch_entry.chemist AS Chemist, despatch_entry.remarks AS Remarks, despatch_sub.hs AS Expr1,  vendors.state, branchmapping.superbranch FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno INNER JOIN branchmapping ON despatch_entry.branchid = branchmapping.subbranch WHERE   (despatch_entry.doe BETWEEN @d1 AND @d2) AND (vendors.state = @state) AND (branchmapping.superbranch = @BranchID) ORDER BY RefDCNo");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@state", ddlstatename.SelectedValue);
                    cmd.Parameters.Add("@BranchID", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Company Wise")
                {
                    cmd = new SqlCommand("SELECT despatch_entry.vehciecleno AS VehicleNo, despatch_entry.dctype, despatch_entry.invoiceno, despatch_entry.doe AS Date, despatch_entry.sno AS RefDCNo, despatch_entry.dc_no AS DCNo, despatch_sub.cellname AS Cell, despatch_sub.milktype AS MilkType, despatch_sub.fat AS FAT, despatch_sub.snf AS SNF, despatch_sub.qty_ltr AS QTYLtr, despatch_sub.qty_kgs AS QtyKG, despatch_sub.percentageon AS PercentageOn, despatch_sub.clr AS CLR, despatch_sub.hs AS HS, despatch_sub.alcohol AS Alcohol, despatch_sub.temp AS TEMP, despatch_sub.cob1 AS COB, despatch_sub.phosps1 AS Phosps, despatch_sub.mbrt, vendors.vendorname AS VendorName, despatch_entry.chemist AS Chemist, despatch_entry.remarks AS Remarks, despatch_sub.hs AS Expr1, vendors.state, branchmapping.superbranch FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno INNER JOIN branchmapping ON despatch_entry.branchid = branchmapping.subbranch WHERE (despatch_entry.doe BETWEEN @d1 AND @d2) AND (vendors.companycode = @companycode) AND (branchmapping.superbranch = @BranchId) ORDER BY RefDCNo");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@companycode", ddlcompanywise.SelectedValue);
                    cmd.Parameters.Add("@BranchID", BranchID);
                }
            }
            DataTable dtTotDc = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtTotDc.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTotDc.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["VehicleNo"] = dr["VehicleNo"].ToString();
                    newrow["Date"] = dr["Date"].ToString();
                    newrow["RefDCNo"] = dr["RefDCNo"].ToString();
                    newrow["DCNo"] = dr["DCNo"].ToString();
                    newrow["Invoice No"] = dr["invoiceno"].ToString();
                    string dctype = dr["dctype"].ToString();
                    if (dctype == "1")
                    {
                        dctype = "Stock Transfer";
                    }
                    else
                    {
                        dctype = "Invoice";
                    }
                    newrow["DC Type"] = dctype.ToString();
                    string cell = "";
                    if (dr["Cell"].ToString() == "F Cell")
                    {
                        cell = "F";
                    }
                    if (dr["Cell"].ToString() == "M Cell")
                    {
                        cell = "M";
                    }
                    if (dr["Cell"].ToString() == "B Cell")
                    {
                        cell = "B";
                    }
                    newrow["Cell"] = cell;
                    newrow["MilkType"] = dr["MilkType"].ToString();
                    newrow["FAT"] = dr["FAT"].ToString();
                    newrow["SNF"] = dr["SNF"].ToString();
                    double QtyKG = 0;
                    double.TryParse(dr["QtyKG"].ToString(), out QtyKG);
                    newrow["KGS"] = QtyKG;
                    double QTYLtr = 0;
                    double.TryParse(dr["QTYLtr"].ToString(), out QTYLtr);
                    newrow["LTRS"] = QTYLtr;
                    newrow["CLR"] = dr["CLR"].ToString();
                    newrow["HS"] = dr["HS"].ToString();
                    newrow["Alcohol"] = dr["Alcohol"].ToString();
                    newrow["TEMP"] = dr["TEMP"].ToString();
                    newrow["COB"] = dr["COB"].ToString();
                    newrow["Phosps"] = dr["Phosps"].ToString();
                    newrow["MBRT"] = dr["MBRT"].ToString();
                    if (Type == "CC" || ddlBranch.SelectedValue == "Branch Wise")
                    {
                        lblVendorName.Text = dr["VendorName"].ToString();
                    }
                    else
                    {
                        newrow["VendorName"] = dr["VendorName"].ToString();
                    }
                    newrow["Chemist"] = dr["Chemist"].ToString();
                    Report.Rows.Add(newrow);
                }
                DataRow newvartical2 = Report.NewRow();
                newvartical2["MilkType"] = "Total";
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        newvartical2[dc.ToString()] = val;
                    }
                }
                Report.Rows.Add(newvartical2);
                grdReports.DataSource = Report;
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