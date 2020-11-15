using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Acknowledge : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    string BranchType = "";
    SalesDBManager vdm;
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
                    // fillvendors();
                }
            }
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillvendors();
    }
    void fillvendors()
    {
        if (BranchType == "Plant")
        {
            if (ddlbranchType.SelectedItem.Text == "All")
            {
                cmd = new SqlCommand("SELECT  vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address,  branchmapping.superbranch FROM   vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (branchmapping.superbranch = @branchid)");
            }
            else
            {
                cmd = new SqlCommand("SELECT  vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address, branchmapping.superbranch FROM  vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE (branchmapping.superbranch = @branchid) AND (vendors.branchtype = @branchtype)");
                cmd.Parameters.Add("@branchtype", ddlbranchType.SelectedItem.Text);
            }
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
    DataTable dtcow = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            hidepanel.Visible = true;
            pnlcow.Visible = false;
            grdcow.DataSource = Report;
            grdcow.DataBind();
            Session["filename"] = "Tanker Inward";
            Session["title"] = "Tanker Inward Details";
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
            string vendervalue = ddlbranches.SelectedValue;
            cmd = new SqlCommand("SELECT   sno, dcno, fromccid, toccid, doe  FROM  directsale WHERE   (fromccid = @fromccid) AND (doe BETWEEN @direct1 AND @direct2)");
            cmd.Parameters.Add("fromccid", ddlbranches.SelectedValue);
            cmd.Parameters.Add("@direct1", GetLowDate(fromdate));
            cmd.Parameters.Add("@direct2", GetHighDate(todate));
            DataTable dtdirectsale = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtdirectsale.Rows.Count > 0)
            {
                milktransactiondetails();
                directsale();
            }
            else
            {
                milktransactiondetails();
                directpurchasedetails();
                directtsalesdetails();
            }
        }
        catch
        {
        }
    }

    DataTable dtdirectReport = new DataTable();
    DataTable dtdirectcow = new DataTable();

    private void directsale()
    {
        try
        {
            Session["filename"] = "Tanker Direct Sales";
            Session["title"] = "Tanker Direct Sales Details";
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
            dtdirectReport.Columns.Add("Sno");
            dtdirectReport.Columns.Add("DATE");
            dtdirectReport.Columns.Add("KGS");
            dtdirectReport.Columns.Add("LTRS");
            dtdirectReport.Columns.Add("FAT");
            dtdirectReport.Columns.Add("SNF");
            dtdirectReport.Columns.Add("CLR");
            dtdirectReport.Columns.Add("KG FAT RATE");
            dtdirectReport.Columns.Add("KG FAT");
            dtdirectReport.Columns.Add("KG SNF");
            dtdirectReport.Columns.Add("M VALUE");
            dtdirectReport.Columns.Add("OH");
            dtdirectReport.Columns.Add("SNF9");
            dtdirectReport.Columns.Add("MILK VALUE");
            dtdirectReport.Columns.Add("Transaction No");
            dtdirectReport.Columns.Add("DC No");
            dtdirectReport.Columns.Add("FROM CC Name");
            dtdirectReport.Columns.Add("TO CC Name");
            dtdirectReport.Columns.Add("TANKER NO");

            if (BranchType == "Plant")
            {
                cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @ccid) AND (directsale.milktype='Buffalo') AND (directsale.branchid=@branchid) ORDER BY directsale.doe ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
                cmd.Parameters.Add("@branchid", BranchID);
            }
            else
            {
                cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @ccid) AND directsale.milktype='Buffalo' ORDER BY directsale.doe ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
            }
            DataTable dtDispatch = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double Ltrstotal = 0;
                double TStotal = 0;
                double mvaluetotal = 0;
                double ohtotal = 0;
                double snf9total = 0;
                double milkvaluetotal = 0;
                double totalmilkvaluetotal = 0;
                double kgfatratetotal = 0;
                double kgfatrate = 0;
                int i = 1;
                foreach (DataRow dr in dtDispatch.Rows)
                {
                    DataRow newrow = dtdirectReport.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    newrow["FROM CC Name"] = dr["fromcc"].ToString();
                    newrow["TO CC Name"] = dr["tocc"].ToString();
                    newrow["TANKER NO"] = dr["vehicleno"].ToString();
                    newrow["KGS"] = dr["qty_kgs"].ToString();
                    newrow["Transaction No"] = dr["dcno"].ToString();
                    newrow["DC No"] = dr["dc_no"].ToString();
                    double qty_ltr = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                    newrow["LTRS"] = dr["qty_ltr"].ToString();
                    double FAT = 0;
                    double.TryParse(dr["fat"].ToString(), out FAT);
                    FAT = Math.Round(FAT, 2);
                    newrow["FAT"] = FAT;
                    double SNF = 0;
                    double.TryParse(dr["snf"].ToString(), out SNF);
                    newrow["SNF"] = SNF;
                    newrow["CLR"] = dr["clr"].ToString();
                    string Rateon = dr["rate_on"].ToString();
                    double weight = 0;
                    double KGFAT = 0;
                    double KGSNF = 0;
                    double ltrs = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                    Ltrstotal += ltrs;
                    double Kgs = 0;
                    double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                    kgstotal += Kgs;
                    double tstotal = 0;
                    tstotal = FAT + SNF;
                    if (Rateon == "TS")
                    {

                        double TS = 0;
                        TS = FAT + SNF;
                        weight = (TS * Kgs) / 100;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                    else if (Rateon == "KGFAT")
                    {
                        weight = (FAT * Kgs) / 100;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                    else if (Rateon == "PerLtr" || Rateon == "PerKg")
                    {
                        string CalOn = dr["calc_on"].ToString();
                        if (CalOn == "Ltrs")
                        {
                            weight = ltrs;
                            KGFAT = (FAT * ltrs) / 100;
                            KGSNF = (SNF * ltrs) / 100;
                        }
                        else
                        {
                            weight = Kgs;
                            KGFAT = (FAT * Kgs) / 100;
                            KGSNF = (SNF * Kgs) / 100;
                        }
                    }

                    string OverheadOn = dr["overheadon"].ToString();
                    double OHcost = 0;
                    double overheadcost = 0;
                    double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                    if (OverheadOn == "Ltrs")
                    {
                        OHcost = overheadcost * ltrs;
                    }
                    else
                    {
                        OHcost = overheadcost * Kgs;
                    }

                    double MSnf = 0;
                    double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                    double m_snfpluscost = 0;
                    double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                    double DiffSNFCost = 0;
                    if (SNF < MSnf)
                    {
                        string SNFOn = dr["snfplus_on"].ToString();
                        double diffSNF = 0;
                        diffSNF = SNF - MSnf;
                        diffSNF = Math.Round(diffSNF, 2);
                        if (SNFOn == "Ltrs")
                        {
                            DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                        }
                        else
                        {
                            DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                        }
                    }
                    double p_snfpluscost = 0;
                    double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                    double PSnf = 0;
                    double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                    if (SNF > PSnf)
                    {
                        string SNFOn = dr["snfplus_on"].ToString();
                        double diffSNF = 0;
                        diffSNF = SNF - PSnf;
                        if (SNFOn == "Ltrs")
                        {
                            DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                        }
                        else
                        {
                            DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                        }
                    }

                    double cost = 0;
                    double.TryParse(dr["rate"].ToString(), out cost);
                    newrow["KG FAT RATE"] = cost;
                    kgfatratetotal += cost;
                    kgfatrate = cost;
                    KGFAT = Math.Round(KGFAT, 2);
                    newrow["KG FAT"] = KGFAT;
                    kgfattotal += KGFAT;
                    KGSNF = Math.Round(KGSNF, 2);
                    newrow["KG SNF"] = KGSNF;
                    kgsnftotal += KGSNF;
                    double MValue = 0;
                    MValue = KGFAT * cost;
                    //MValue = MValue / 100;
                    MValue = Math.Round(MValue, 2);
                    //newrow["M VALUE"] = MValue;
                    mvaluetotal += MValue;
                    double OHandMvalue = 0;
                    OHandMvalue = MValue;
                    OHandMvalue = Math.Round(OHandMvalue, 2);
                    newrow["M VALUE"] = OHandMvalue;
                    milkvaluetotal += OHandMvalue;

                    double OMILKVALUE = 0;
                    OMILKVALUE = MValue + OHcost + DiffSNFCost;
                    newrow["OH"] = OHcost;
                    ohtotal += OHcost;
                    DiffSNFCost = Math.Round(DiffSNFCost, 2);
                    newrow["SNF9"] = DiffSNFCost;
                    snf9total += DiffSNFCost;
                    OMILKVALUE = Math.Round(OMILKVALUE, 2);
                    newrow["MILK VALUE"] = OMILKVALUE;
                    totalmilkvaluetotal += OMILKVALUE;
                    dtdirectReport.Rows.Add(newrow);
                }
                DataRow newvartical2 = dtdirectReport.NewRow();
                newvartical2["DATE"] = "Total";
                newvartical2["KGS"] = kgstotal;
                newvartical2["LTRS"] = Ltrstotal;
                double fattotal = 0;
                fattotal = (kgfattotal / kgstotal) * 100;
                fattotal = Math.Round(fattotal, 2);
                newvartical2["FAT"] = fattotal;
                newvartical2["KG FAT"] = kgfattotal;
                double snftotal = 0;
                snftotal = (kgsnftotal / kgstotal) * 100;
                snftotal = Math.Round(snftotal, 2);
                newvartical2["SNF"] = snftotal;
                newvartical2["KG SNF"] = kgsnftotal;

                newvartical2["M VALUE"] = milkvaluetotal;
                newvartical2["OH"] = ohtotal;
                newvartical2["SNF9"] = snf9total;
                newvartical2["MILK VALUE"] = totalmilkvaluetotal;
                kgfatratetotal = Math.Round(kgfatratetotal, 2);
                newvartical2["KG FAT RATE"] = kgfatrate;
                dtdirectReport.Rows.Add(newvartical2);
                DataRow New1 = dtdirectReport.NewRow();
                New1["DATE"] = "Cow";
                dtdirectReport.Rows.Add(New1);
                Session["xportdata"] = dtdirectReport;
                grddirectbuf.DataSource = dtdirectReport;
                grddirectbuf.DataBind();
                pnldirectbuf.Visible = true;
                pnldirectcow.Visible = false;
            }
            dtdirectcow.Columns.Add("Sno");
            dtdirectcow.Columns.Add("DATE");
            dtdirectcow.Columns.Add("KGS");
            dtdirectcow.Columns.Add("LTRS");
            dtdirectcow.Columns.Add("FAT");
            dtdirectcow.Columns.Add("SNF");
            dtdirectcow.Columns.Add("CLR");
            dtdirectcow.Columns.Add("TS RATE");
            dtdirectcow.Columns.Add("KG FAT");
            dtdirectcow.Columns.Add("KG SNF");
            dtdirectcow.Columns.Add("M VALUE");
            dtdirectcow.Columns.Add("OH");
            dtdirectcow.Columns.Add("SNF9");
            dtdirectcow.Columns.Add("MILK VALUE");
            dtdirectcow.Columns.Add("Transaction No");
            dtdirectcow.Columns.Add("DC No");
            dtdirectcow.Columns.Add("FROM CC Name");
            dtdirectcow.Columns.Add("TO CC Name");
            dtdirectcow.Columns.Add("TANKER NO");
            if (BranchType == "Plant")
            {
                cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @ccid) AND (directsale.milktype='Cow') AND (directsale.branchid=@branchid) ORDER BY directsale.doe ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
                cmd.Parameters.Add("@branchid", BranchID);
            }
            else
            {
                cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @ccid) AND directsale.milktype='Cow' ORDER BY directsale.doe ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
            }
            DataTable dtcowDispatch = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtcowDispatch.Rows.Count > 0)
            {
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double Ltrstotal = 0;
                double TStotal = 0;
                double mvaluetotal = 0;
                double ohtotal = 0;
                double snf9total = 0;
                double milkvaluetotal = 0;
                double totalmilkvaluetotal = 0;
                double kgfatratetotal = 0;
                double kgfatrate = 0;
                int i = 1;
                foreach (DataRow dr in dtcowDispatch.Rows)
                {
                    DataRow newrow = dtdirectcow.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    newrow["FROM CC Name"] = dr["fromcc"].ToString();
                    newrow["TO CC Name"] = dr["tocc"].ToString();
                    newrow["TANKER NO"] = dr["vehicleno"].ToString();
                    newrow["KGS"] = dr["qty_kgs"].ToString();
                    newrow["Transaction No"] = dr["dcno"].ToString();
                    newrow["DC No"] = dr["dc_no"].ToString();
                    double qty_ltr = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                    newrow["LTRS"] = dr["qty_ltr"].ToString();
                    double FAT = 0;
                    double.TryParse(dr["fat"].ToString(), out FAT);
                    FAT = Math.Round(FAT, 2);
                    newrow["FAT"] = FAT;
                    double SNF = 0;
                    double.TryParse(dr["snf"].ToString(), out SNF);
                    newrow["SNF"] = SNF;
                    newrow["CLR"] = dr["clr"].ToString();
                    string Rateon = dr["rate_on"].ToString();


                    double weight = 0;
                    double KGFAT = 0;
                    double KGSNF = 0;
                    double ltrs = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                    Ltrstotal += ltrs;
                    double Kgs = 0;
                    double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                    kgstotal += Kgs;
                    double tstotal = 0;
                    tstotal = FAT + SNF;
                    if (Rateon == "TS")
                    {

                        double TS = 0;
                        TS = FAT + SNF;
                        weight = (TS * Kgs) / 100;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                    else if (Rateon == "KGFAT")
                    {
                        weight = (FAT * Kgs) / 100;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                    else if (Rateon == "PerLtr" || Rateon == "PerKg")
                    {
                        string CalOn = dr["calc_on"].ToString();
                        if (CalOn == "Ltrs")
                        {
                            weight = ltrs;
                            KGFAT = (FAT * ltrs) / 100;
                            KGSNF = (SNF * ltrs) / 100;
                        }
                        else
                        {
                            weight = Kgs;
                            KGFAT = (FAT * Kgs) / 100;
                            KGSNF = (SNF * Kgs) / 100;
                        }
                    }

                    string OverheadOn = dr["overheadon"].ToString();
                    double OHcost = 0;
                    double overheadcost = 0;
                    double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                    if (OverheadOn == "Ltrs")
                    {
                        OHcost = overheadcost * ltrs;
                    }
                    else
                    {
                        OHcost = overheadcost * Kgs;
                    }

                    double MSnf = 0;
                    double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                    double m_snfpluscost = 0;
                    double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                    double DiffSNFCost = 0;
                    if (SNF < MSnf)
                    {
                        string SNFOn = dr["snfplus_on"].ToString();
                        double diffSNF = 0;
                        diffSNF = SNF - MSnf;
                        diffSNF = Math.Round(diffSNF, 2);
                        if (SNFOn == "Ltrs")
                        {
                            DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                        }
                        else
                        {
                            DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                        }
                    }
                    double p_snfpluscost = 0;
                    double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                    double PSnf = 0;
                    double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                    if (SNF > PSnf)
                    {
                        string SNFOn = dr["snfplus_on"].ToString();
                        double diffSNF = 0;
                        diffSNF = SNF - PSnf;
                        if (SNFOn == "Ltrs")
                        {
                            DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                        }
                        else
                        {
                            DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                        }
                    }
                    double cost = 0;
                    double.TryParse(dr["rate"].ToString(), out cost);
                    newrow["TS RATE"] = cost;
                    kgfatratetotal += cost;
                    kgfatrate = cost;
                    KGFAT = Math.Round(KGFAT, 2);
                    newrow["KG FAT"] = KGFAT;
                    kgfattotal += KGFAT;
                    KGSNF = Math.Round(KGSNF, 2);
                    newrow["KG SNF"] = KGSNF;
                    kgsnftotal += KGSNF;
                    double MValue = 0;
                    MValue = tstotal * qty_ltr * cost;
                    MValue = MValue / 100;
                    MValue = Math.Round(MValue, 2);
                    //newrow["M VALUE"] = MValue;
                    mvaluetotal += MValue;

                    double OHandMvalue = 0;
                    OHandMvalue = MValue;
                    OHandMvalue = Math.Round(OHandMvalue, 2);
                    newrow["M VALUE"] = OHandMvalue;
                    milkvaluetotal += OHandMvalue;
                    //newrow["DC NO"] = dr["dcno"].ToString();
                    double OMILKVALUE = 0;
                    OMILKVALUE = MValue + OHcost + DiffSNFCost;
                    newrow["OH"] = OHcost;
                    ohtotal += OHcost;
                    DiffSNFCost = Math.Round(DiffSNFCost, 2);
                    newrow["SNF9"] = DiffSNFCost;
                    snf9total += DiffSNFCost;
                    OMILKVALUE = Math.Round(OMILKVALUE, 2);
                    newrow["MILK VALUE"] = OMILKVALUE;
                    totalmilkvaluetotal += OMILKVALUE;
                    dtdirectcow.Rows.Add(newrow);
                }
                DataRow newvartical2 = dtdirectcow.NewRow();
                newvartical2["DATE"] = "Total";
                newvartical2["KGS"] = kgstotal;
                newvartical2["LTRS"] = Ltrstotal;
                double fattotal = 0;
                fattotal = (kgfattotal / kgstotal) * 100;
                fattotal = Math.Round(fattotal, 2);
                newvartical2["FAT"] = fattotal;
                newvartical2["KG FAT"] = kgfattotal;
                double snftotal = 0;
                snftotal = (kgsnftotal / kgstotal) * 100;
                snftotal = Math.Round(snftotal, 2);
                newvartical2["SNF"] = snftotal;
                newvartical2["OH"] = ohtotal;
                newvartical2["SNF9"] = snf9total;
                newvartical2["KG SNF"] = kgsnftotal;
                newvartical2["M VALUE"] = milkvaluetotal;
                newvartical2["MILK VALUE"] = totalmilkvaluetotal;
                kgfatratetotal = mvaluetotal / kgfattotal;
                kgfatratetotal = Math.Round(kgfatratetotal, 2);
                newvartical2["TS RATE"] = Math.Round(kgfatrate);
                dtdirectcow.Rows.Add(newvartical2);
                grddirectcow.DataSource = dtdirectcow;
                grddirectcow.DataBind();
                Session["xportdata"] = dtdirectcow;
                pnldirectcow.Visible = true;
            }
            else
            {
                pnldirectcow.Visible = false;
            }
            // Session["xportdata"] = Report;
            //hidepanel.Visible = true;
        }
        catch
        {
        }
    }
    private void milktransactiondetails()
    {
        DataTable dtReport = new DataTable();
        pnldirectcow.Visible = false;
        pnldirectbuf.Visible = false;
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
        grdReports.DataSource = null;
        grdReports.DataBind();
        Report.Columns.Add("Sno");
        Report.Columns.Add("DATE");
        Report.Columns.Add("KGS").DataType = typeof(Double);
        Report.Columns.Add("LTRS").DataType = typeof(Double);
        Report.Columns.Add("FAT");
        Report.Columns.Add("SNF");
        Report.Columns.Add("CLR");
        Report.Columns.Add("KG FAT RATE");
        Report.Columns.Add("KG FAT").DataType = typeof(Double);
        Report.Columns.Add("KG SNF").DataType = typeof(Double);
        Report.Columns.Add("M VALUE").DataType = typeof(Double);
        Report.Columns.Add("OH").DataType = typeof(Double);
        Report.Columns.Add("SNF9").DataType = typeof(Double);
        Report.Columns.Add("MILK VALUE").DataType = typeof(Double);
        Report.Columns.Add("Transaction NO");
        Report.Columns.Add("DC NO");
        Report.Columns.Add("CC Name");
        Report.Columns.Add("TANKER NO");
        Report.Columns.Add("Destination");
        cmd = new SqlCommand("SELECT milktransactions.dcno, branch_info.branchname, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON branch_info.sno=milktransactions.branchid WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.sectionid=@sectionid) ORDER by milktransactions.doe");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@transtype", "in");
        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
        DataTable dtBufello = SalesDB.SelectQuery(cmd).Tables[0];
        int i = 1;
        if (dtBufello.Rows.Count > 0)
        {
            double kgfattotal = 0;
            double kgsnftotal = 0;
            double kgstotal = 0;
            double Ltrstotal = 0;
            double mvaluetotal = 0;
            double ohtotal = 0;
            double snf9total = 0;
            double milkvaluetotal = 0;
            foreach (DataRow dr in dtBufello.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = i++.ToString();
                newrow["Destination"] = dr["branchname"].ToString();
                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                string date = dtdoe.ToString("dd/MM/yyyy");
                newrow["DATE"] = date;
                newrow["KGS"] = dr["qty_kgs"].ToString();

                double qty_ltr = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                newrow["LTRS"] = dr["qty_ltr"].ToString();
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                FAT = Math.Round(FAT, 2);
                newrow["FAT"] = FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                newrow["SNF"] = SNF;
                newrow["CLR"] = dr["clr"].ToString();
                string Rateon = dr["rate_on"].ToString();


                double weight = 0;
                double KGFAT = 0;
                double KGSNF = 0;
                double ltrs = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                Ltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                kgstotal += Kgs;
                double tstotal = 0;
                tstotal = FAT + SNF;
                if (Rateon == "TS")
                {

                    double TS = 0;
                    TS = FAT + SNF;
                    weight = (TS * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "KGFAT")
                {
                    weight = (FAT * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "PerLtr" || Rateon == "PerKg")
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        weight = ltrs;
                        KGFAT = (FAT * ltrs) / 100;
                        KGSNF = (SNF * ltrs) / 100;
                    }
                    else
                    {
                        weight = Kgs;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                }
                double cost = 0;
                double.TryParse(dr["cost"].ToString(), out cost);
                newrow["KG FAT RATE"] = cost;
                KGFAT = Math.Round(KGFAT, 2);
                newrow["KG FAT"] = KGFAT;
                kgfattotal += KGFAT;
                KGSNF = Math.Round(KGSNF, 2);
                newrow["KG SNF"] = KGSNF;
                kgsnftotal += KGSNF;
                double MValue = 0;
                MValue = KGFAT * cost;
                //MValue = MValue / 100;
                MValue = Math.Round(MValue, 2);
                newrow["M VALUE"] = MValue;
                mvaluetotal += MValue;
                string OverheadOn = dr["overheadon"].ToString();
                double OHcost = 0;
                double overheadcost = 0;
                double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                if (OverheadOn == "Ltrs")
                {
                    OHcost = overheadcost * ltrs;
                }
                else
                {
                    OHcost = overheadcost * Kgs;
                }
                double MSnf = 0;
                double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                double m_snfpluscost = 0;
                double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                double DiffSNFCost = 0;
                if (SNF < MSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    diffSNF = Math.Round(diffSNF, 2);
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                    }
                }
                double p_snfpluscost = 0;
                double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                double PSnf = 0;
                double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                if (SNF > PSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - PSnf;
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                    }
                }

                double OHandMvalue = 0;
                OHandMvalue = MValue + OHcost + DiffSNFCost;
                newrow["OH"] = OHcost;
                ohtotal += OHcost;
                DiffSNFCost = Math.Round(DiffSNFCost, 2);
                newrow["SNF9"] = DiffSNFCost;
                snf9total += DiffSNFCost;
                OHandMvalue = Math.Round(OHandMvalue, 2);
                newrow["MILK VALUE"] = OHandMvalue;
                milkvaluetotal += OHandMvalue;
                newrow["Transaction NO"] = dr["dcno"].ToString();
                newrow["DC NO"] = dr["partydcno"].ToString();
                newrow["CC Name"] = dr["vendorname"].ToString();
                newrow["TANKER NO"] = dr["vehicleno"].ToString();
                Report.Rows.Add(newrow);
            }
            DataRow newvartical2 = Report.NewRow();
            newvartical2["DATE"] = "Total";
            newvartical2["KGS"] = kgstotal;
            newvartical2["LTRS"] = Ltrstotal;
            double fattotal = 0;
            fattotal = (kgfattotal / kgstotal) * 100;
            fattotal = Math.Round(fattotal, 2);
            newvartical2["FAT"] = fattotal;
            newvartical2["KG FAT"] = kgfattotal;
            double snftotal = 0;
            snftotal = (kgsnftotal / kgstotal) * 100;
            snftotal = Math.Round(snftotal, 2);
            newvartical2["SNF"] = snftotal;
            newvartical2["KG SNF"] = kgsnftotal;
            newvartical2["M VALUE"] = mvaluetotal;
            newvartical2["OH"] = ohtotal;
            newvartical2["SNF9"] = snf9total;
            newvartical2["MILK VALUE"] = milkvaluetotal;
            double kgfatratetotal = 0;
            kgfatratetotal = mvaluetotal / kgfattotal;
            kgfatratetotal = Math.Round(kgfatratetotal, 2);
            newvartical2["KG FAT RATE"] = kgfatratetotal;
            Report.Rows.Add(newvartical2);
            DataRow New1 = Report.NewRow();
            New1["DATE"] = "Cow";
            Report.Rows.Add(New1);
            Session["xportdata"] = Report;
            grdReports.DataSource = Report;
            grdReports.DataBind();

            //merge 
            Report.Rows.Add(dtReport);
        }
        dtReport.Columns.Add("Sno");
        dtReport.Columns.Add("DATE");
        dtReport.Columns.Add("KGS").DataType = typeof(Double);
        dtReport.Columns.Add("LTRS").DataType = typeof(Double);
        dtReport.Columns.Add("FAT");
        dtReport.Columns.Add("SNF");
        dtReport.Columns.Add("CLR");
        dtReport.Columns.Add("TS RATE");
        dtReport.Columns.Add("KG FAT").DataType = typeof(Double);
        dtReport.Columns.Add("KG SNF").DataType = typeof(Double);
        dtReport.Columns.Add("TS TOTAL");
        dtReport.Columns.Add("M VALUE").DataType = typeof(Double);
        dtReport.Columns.Add("OH").DataType = typeof(Double);
        dtReport.Columns.Add("FAT+/-").DataType = typeof(Double);
        dtReport.Columns.Add("SNF9").DataType = typeof(Double);
        dtReport.Columns.Add("MILK VALUE").DataType = typeof(Double);
        dtReport.Columns.Add("Transaction NO");
        dtReport.Columns.Add("DC NO");
        dtReport.Columns.Add("CC Name");
        dtReport.Columns.Add("TANKER NO");
        dtReport.Columns.Add("Destination");
        cmd = new SqlCommand("  SELECT milktransactions.dcno, branch_info.branchname, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.m_fatpluscost, milktransaction_logs.p_std_fat, milktransaction_logs.p_fatpluscost, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.fatplus_on, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON branch_info.sno = milktransactions.branchid WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.sectionid=@sectionid) ORDER by milktransactions.doe");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@transtype", "in");
        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
        DataTable dtcow = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtcow.Rows.Count > 0)
        {
            double kgfattotal = 0;
            double kgsnftotal = 0;
            double kgstotal = 0;
            double Ltrstotal = 0;
            double TStotal = 0;
            double mvaluetotal = 0;
            double ohtotal = 0;
            double snf9total = 0;
            double milkvaluetotal = 0;
            pnlcow.Visible = true;
            foreach (DataRow dr in dtcow.Rows)
            {
                DataRow newrow = dtReport.NewRow();
                newrow["Sno"] = i++.ToString();
                newrow["Destination"] = dr["branchname"].ToString();
                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                string date = dtdoe.ToString("dd/MM/yyyy");
                newrow["DATE"] = date;
                newrow["KGS"] = dr["qty_kgs"].ToString();
                double qty_ltr = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                newrow["LTRS"] = dr["qty_ltr"].ToString();
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                FAT = Math.Round(FAT, 2);
                newrow["FAT"] = FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                newrow["SNF"] = SNF;
                newrow["CLR"] = dr["clr"].ToString();
                string Rateon = dr["rate_on"].ToString();

                double weight = 0;
                double KGFAT = 0;
                double KGSNF = 0;
                double ltrs = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                Ltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                kgstotal += Kgs;
                double tstotal = 0;
                tstotal = FAT + SNF;
                newrow["TS TOTAL"] = tstotal;
                if (Rateon == "TS")
                {
                    double TS = 0;
                    TS = FAT + SNF;
                    weight = (TS * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "KGFAT")
                {
                    weight = (FAT * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "PerLtr" || Rateon == "PerKg")
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        weight = ltrs;
                        KGFAT = (FAT * ltrs) / 100;
                        KGSNF = (SNF * ltrs) / 100;
                    }
                    else
                    {
                        weight = Kgs;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                }
                double cost = 0;
                double.TryParse(dr["cost"].ToString(), out cost);
                newrow["TS RATE"] = cost;
                KGFAT = Math.Round(KGFAT, 2);
                newrow["KG FAT"] = KGFAT;
                kgfattotal += KGFAT;
                KGSNF = Math.Round(KGSNF, 2);
                newrow["KG SNF"] = KGSNF;
                kgsnftotal += KGSNF;
                double MValue = 0;
                if (Rateon == "PerLtr")
                {
                    MValue = cost * qty_ltr;
                }
                else if (Rateon == "PerKg")
                {
                    MValue = cost * Kgs;
                }
                else
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        MValue = tstotal * cost * ltrs;
                        MValue = MValue / 100;
                    }
                    else
                    {
                        MValue = tstotal * cost * Kgs;
                        MValue = MValue / 100;
                    }
                }
                MValue = Math.Round(MValue, 2);
                newrow["M VALUE"] = MValue;
                mvaluetotal += MValue;
                string OverheadOn = dr["overheadon"].ToString();
                double OHcost = 0;
                double overheadcost = 0;
                double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                if (OverheadOn == "Ltrs")
                {
                    OHcost = overheadcost * ltrs;
                }
                else
                {
                    OHcost = overheadcost * Kgs;
                }


                double MSnf = 0;
                double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                double m_snfpluscost = 0;
                double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                double DiffSNFCost = 0;
                if (SNF < MSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    diffSNF = Math.Round(diffSNF, 2);
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                    }
                }


                double p_snfpluscost = 0;
                double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                double PSnf = 0;
                double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                if (SNF > PSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - PSnf;
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                    }
                }

                double MFat = 0;
                double.TryParse(dr["m_std_fat"].ToString(), out MFat);
                double m_fatpluscost = 0;
                double.TryParse(dr["m_fatpluscost"].ToString(), out m_fatpluscost);
                double DiffFATCost = 0;
                if (FAT < MFat)
                {
                    string FATOn = dr["fatplus_on"].ToString();
                    double diffFAT = 0;
                    diffFAT = FAT - MFat;
                    diffFAT = Math.Round(diffFAT, 2);
                    if (FATOn == "Ltrs")
                    {
                        DiffFATCost = diffFAT * ltrs * m_fatpluscost * 10;
                    }
                    else
                    {
                        DiffFATCost = diffFAT * Kgs * m_fatpluscost * 10;
                    }
                }
                double p_fatpluscost = 0;
                double.TryParse(dr["p_fatpluscost"].ToString(), out p_fatpluscost);
                double PFat = 0;
                double.TryParse(dr["p_std_fat"].ToString(), out PFat);
                if (FAT > PFat)
                {
                    string FATOn = dr["fatplus_on"].ToString();
                    double diffFAT = 0;
                    diffFAT = FAT - PFat;
                    if (FATOn == "Ltrs")
                    {
                        DiffFATCost = diffFAT * ltrs * p_fatpluscost * 10;
                    }
                    else
                    {
                        DiffFATCost = diffFAT * Kgs * p_fatpluscost * 10;
                    }
                }
                DiffFATCost = Math.Round(DiffFATCost, 2);

                newrow["FAT+/-"] = DiffFATCost;

                double OHandMvalue = 0;
                OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                newrow["OH"] = OHcost;
                ohtotal += OHcost;
                DiffSNFCost = Math.Round(DiffSNFCost, 2);
                newrow["SNF9"] = DiffSNFCost;
                snf9total += DiffSNFCost;
                OHandMvalue = Math.Round(OHandMvalue, 2);
                newrow["MILK VALUE"] = OHandMvalue;
                milkvaluetotal += OHandMvalue;
                newrow["Transaction NO"] = dr["dcno"].ToString();
                newrow["DC NO"] = dr["partydcno"].ToString();
                newrow["CC Name"] = dr["vendorname"].ToString();
                newrow["TANKER NO"] = dr["vehicleno"].ToString();
                dtReport.Rows.Add(newrow);
            }
            DataRow newvartical2 = dtReport.NewRow();
            newvartical2["DATE"] = "Total";
            newvartical2["KGS"] = kgstotal;
            newvartical2["LTRS"] = Ltrstotal;
            double fattotal = 0;
            fattotal = (kgfattotal / kgstotal) * 100;
            fattotal = Math.Round(fattotal, 2);
            newvartical2["FAT"] = fattotal;
            newvartical2["KG FAT"] = kgfattotal;
            double snftotal = 0;
            snftotal = (kgsnftotal / kgstotal) * 100;
            snftotal = Math.Round(snftotal, 2);
            TStotal = snftotal + fattotal;
            newvartical2["SNF"] = snftotal;
            newvartical2["KG SNF"] = kgsnftotal;
            newvartical2["TS TOTAL"] = TStotal;
            newvartical2["M VALUE"] = mvaluetotal;
            newvartical2["OH"] = ohtotal;
            newvartical2["SNF9"] = snf9total;
            newvartical2["MILK VALUE"] = milkvaluetotal;
            double ts = TStotal * Ltrstotal;
            double tsratetotal = 0;
            tsratetotal = (mvaluetotal / ts) * 100;
            tsratetotal = Math.Round(tsratetotal);
            newvartical2["TS RATE"] = tsratetotal;
            dtReport.Rows.Add(newvartical2);
            grdcow.DataSource = dtReport;
            Session["xportdata"] = dtReport;
            grdcow.DataBind();
            pnlcow.Visible = true;
            // merge 
            Report.Rows.Add(dtReport);
        }
        DataTable dsReport = new DataTable();
        dsReport.Columns.Add("Sno");
        dsReport.Columns.Add("DATE");
        dsReport.Columns.Add("KGS").DataType = typeof(Double);
        dsReport.Columns.Add("LTRS").DataType = typeof(Double);
        dsReport.Columns.Add("FAT");
        dsReport.Columns.Add("SNF");
        dsReport.Columns.Add("CLR");
        dsReport.Columns.Add("TS RATE");
        dsReport.Columns.Add("KG FAT").DataType = typeof(Double);
        dsReport.Columns.Add("KG SNF").DataType = typeof(Double);
        dsReport.Columns.Add("TS TOTAL");
        dsReport.Columns.Add("M VALUE").DataType = typeof(Double);
        dsReport.Columns.Add("OH").DataType = typeof(Double);
        dsReport.Columns.Add("FAT+/-").DataType = typeof(Double);
        dsReport.Columns.Add("SNF9").DataType = typeof(Double);
        dsReport.Columns.Add("MILK VALUE").DataType = typeof(Double);
        dsReport.Columns.Add("Transaction NO");
        dsReport.Columns.Add("DC NO");
        dsReport.Columns.Add("CC Name");
        dsReport.Columns.Add("TANKER NO");
        dsReport.Columns.Add("Destination");
        cmd = new SqlCommand(" SELECT milktransactions.dcno, branch_info.branchname, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.m_fatpluscost, milktransaction_logs.p_std_fat, milktransaction_logs.p_fatpluscost, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.fatplus_on, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON branch_info.sno = milktransactions.branchid WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Skim') AND (milktransactions.sectionid=@sectionid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@transtype", "in");
        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
        DataTable dtskim = SalesDB.SelectQuery(cmd).Tables[0];

        if (dtskim.Rows.Count > 0)
        {
            double kgfattotal = 0;
            double kgsnftotal = 0;
            double kgstotal = 0;
            double Ltrstotal = 0;
            double TStotal = 0;
            double mvaluetotal = 0;
            double ohtotal = 0;
            double snf9total = 0;
            double milkvaluetotal = 0;
            pnlskim.Visible = true;
            foreach (DataRow dr in dtskim.Rows)
            {
                DataRow newrow = dsReport.NewRow();
                newrow["Sno"] = i++.ToString();
                newrow["Destination"] = dr["branchname"].ToString();
                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                string date = dtdoe.ToString("dd/MM/yyyy");
                newrow["DATE"] = date;
                newrow["KGS"] = dr["qty_kgs"].ToString();

                double qty_ltr = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                double qty_kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                newrow["LTRS"] = dr["qty_ltr"].ToString();
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                FAT = Math.Round(FAT, 2);
                newrow["FAT"] = FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                newrow["SNF"] = SNF;
                newrow["CLR"] = dr["clr"].ToString();
                string Rateon = dr["rate_on"].ToString();
                string CalOn = dr["calc_on"].ToString();

                double weight = 0;
                double KGFAT = 0;
                double KGSNF = 0;
                double ltrs = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                Ltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                kgstotal += Kgs;
                double tstotal = 0;
                tstotal = FAT + SNF;
                newrow["TS TOTAL"] = tstotal;
                if (Rateon == "TS")
                {
                    double TS = 0;
                    TS = FAT + SNF;
                    weight = (TS * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "KGFAT")
                {
                    weight = (FAT * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "PerLtr" || Rateon == "PerKg")
                {
                    if (CalOn == "Ltrs")
                    {
                        weight = Kgs;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                    else
                    {
                        weight = Kgs;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                }
                double cost = 0;
                double.TryParse(dr["cost"].ToString(), out cost);
                newrow["TS RATE"] = cost;
                KGFAT = Math.Round(KGFAT, 2);
                newrow["KG FAT"] = KGFAT;
                kgfattotal += KGFAT;
                KGSNF = Math.Round(KGSNF, 2);
                newrow["KG SNF"] = KGSNF;
                kgsnftotal += KGSNF;
                double MValue = 0;
                if (Rateon == "PerLtr" || Rateon == "PerKg")
                {
                    if (CalOn == "Ltrs")
                    {
                        MValue = cost * qty_ltr;
                    }
                    else
                    {
                        MValue = cost * qty_kgs;
                    }
                }
                else
                {
                    if (CalOn == "Ltrs")
                    {
                        MValue = tstotal * cost * qty_ltr;
                    }
                    else
                    {
                        MValue = tstotal * cost * qty_kgs;
                    }
                    MValue = MValue / 100;
                }
                MValue = Math.Round(MValue, 2);
                newrow["M VALUE"] = MValue;
                mvaluetotal += MValue;
                string OverheadOn = dr["overheadon"].ToString();
                double OHcost = 0;
                double overheadcost = 0;
                double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                if (OverheadOn == "Ltrs")
                {
                    OHcost = overheadcost * ltrs;
                }
                else
                {
                    OHcost = overheadcost * Kgs;
                }
                double MSnf = 0;
                double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                double m_snfpluscost = 0;
                double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                double DiffSNFCost = 0;
                if (SNF < MSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    diffSNF = Math.Round(diffSNF, 2);
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                    }
                }


                double p_snfpluscost = 0;
                double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                double PSnf = 0;
                double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                if (SNF > PSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - PSnf;
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                    }
                }

                double MFat = 0;
                double.TryParse(dr["m_std_fat"].ToString(), out MFat);
                double m_fatpluscost = 0;
                double.TryParse(dr["m_fatpluscost"].ToString(), out m_fatpluscost);
                double DiffFATCost = 0;
                if (FAT < MFat)
                {
                    string FATOn = dr["fatplus_on"].ToString();
                    double diffFAT = 0;
                    diffFAT = FAT - MFat;
                    diffFAT = Math.Round(diffFAT, 2);
                    if (FATOn == "Ltrs")
                    {
                        DiffFATCost = diffFAT * ltrs * m_fatpluscost * 10;
                    }
                    else
                    {
                        DiffFATCost = diffFAT * Kgs * m_fatpluscost * 10;
                    }
                }
                double p_fatpluscost = 0;
                double.TryParse(dr["p_fatpluscost"].ToString(), out p_fatpluscost);
                double PFat = 0;
                double.TryParse(dr["p_std_fat"].ToString(), out PFat);
                if (FAT > PFat)
                {
                    string FATOn = dr["fatplus_on"].ToString();
                    double diffFAT = 0;
                    diffFAT = FAT - PFat;
                    if (FATOn == "Ltrs")
                    {
                        DiffFATCost = diffFAT * ltrs * p_fatpluscost * 10;
                    }
                    else
                    {
                        DiffFATCost = diffFAT * Kgs * p_fatpluscost * 10;
                    }
                }
                DiffFATCost = Math.Round(DiffFATCost, 2);

                newrow["FAT+/-"] = DiffFATCost;

                double OHandMvalue = 0;
                OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                newrow["OH"] = OHcost;
                ohtotal += OHcost;
                DiffSNFCost = Math.Round(DiffSNFCost, 2);
                newrow["SNF9"] = DiffSNFCost;
                snf9total += DiffSNFCost;
                OHandMvalue = Math.Round(OHandMvalue, 2);
                newrow["MILK VALUE"] = OHandMvalue;
                milkvaluetotal += OHandMvalue;
                newrow["Transaction NO"] = dr["dcno"].ToString();
                newrow["DC NO"] = dr["partydcno"].ToString();
                newrow["CC Name"] = dr["vendorname"].ToString();
                newrow["TANKER NO"] = dr["vehicleno"].ToString();
                dsReport.Rows.Add(newrow);
            }
            DataRow newvartical3 = dsReport.NewRow();
            newvartical3["DATE"] = "Total";
            newvartical3["KGS"] = kgstotal;
            newvartical3["LTRS"] = Ltrstotal;
            double fattotal = 0;
            fattotal = (kgfattotal / kgstotal) * 100;
            fattotal = Math.Round(fattotal, 2);
            newvartical3["FAT"] = fattotal;
            newvartical3["KG FAT"] = kgfattotal;
            double snftotal = 0;
            snftotal = (kgsnftotal / kgstotal) * 100;
            snftotal = Math.Round(snftotal, 2);
            TStotal = snftotal + fattotal;
            newvartical3["SNF"] = snftotal;
            newvartical3["KG SNF"] = kgsnftotal;
            newvartical3["TS TOTAL"] = TStotal;
            newvartical3["M VALUE"] = mvaluetotal;
            newvartical3["OH"] = ohtotal;
            newvartical3["SNF9"] = snf9total;
            newvartical3["MILK VALUE"] = milkvaluetotal;
            double ts = TStotal * Ltrstotal;
            double tsratetotal = 0;
            tsratetotal = (mvaluetotal / ts) * 100;
            tsratetotal = Math.Round(tsratetotal);
            newvartical3["TS RATE"] = tsratetotal;
            dsReport.Rows.Add(newvartical3);
            Session["xportdata"] = dsReport;
            grdskim.DataSource = dsReport;
            grdskim.DataBind();
            pnlskim.Visible = true;
        }
        DataTable dcondReport = new DataTable();
        dcondReport.Columns.Add("Sno");
        dcondReport.Columns.Add("DATE");
        dcondReport.Columns.Add("KGS").DataType = typeof(Double);
        dcondReport.Columns.Add("LTRS").DataType = typeof(Double);
        dcondReport.Columns.Add("FAT");
        dcondReport.Columns.Add("SNF");
        dcondReport.Columns.Add("CLR");
        dcondReport.Columns.Add("TS RATE");
        dcondReport.Columns.Add("KG FAT").DataType = typeof(Double);
        dcondReport.Columns.Add("KG SNF").DataType = typeof(Double);
        dcondReport.Columns.Add("TS TOTAL");
        dcondReport.Columns.Add("M VALUE").DataType = typeof(Double);
        dcondReport.Columns.Add("OH").DataType = typeof(Double);
        dcondReport.Columns.Add("FAT+/-").DataType = typeof(Double);
        dcondReport.Columns.Add("SNF9").DataType = typeof(Double);
        dcondReport.Columns.Add("MILK VALUE").DataType = typeof(Double);
        dcondReport.Columns.Add("Transaction NO");
        dcondReport.Columns.Add("DC NO");
        dcondReport.Columns.Add("CC Name");
        dcondReport.Columns.Add("TANKER NO");
        dcondReport.Columns.Add("Destination");
        cmd = new SqlCommand("  SELECT milktransactions.dcno, branch_info.branchname, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.m_fatpluscost, milktransaction_logs.p_std_fat, milktransaction_logs.p_fatpluscost, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.fatplus_on, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON branch_info.sno = milktransactions.branchid WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Condensed') AND (milktransactions.sectionid=@sectionid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@transtype", "in");
        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
        DataTable dtcondenser = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtcondenser.Rows.Count > 0)
        {
            double kgfattotal = 0;
            double kgsnftotal = 0;
            double kgstotal = 0;
            double Ltrstotal = 0;
            double TStotal = 0;
            double mvaluetotal = 0;
            double ohtotal = 0;
            double snf9total = 0;
            double milkvaluetotal = 0;
            pnlcow.Visible = true;
            foreach (DataRow dr in dtcondenser.Rows)
            {
                DataRow newrow = dcondReport.NewRow();
                newrow["Sno"] = i++.ToString();
                newrow["Destination"] = dr["branchname"].ToString();
                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                string date = dtdoe.ToString("dd/MM/yyyy");
                newrow["DATE"] = date;
                newrow["KGS"] = dr["qty_kgs"].ToString();
                double qty_ltr = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                newrow["LTRS"] = dr["qty_ltr"].ToString();
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                FAT = Math.Round(FAT, 2);
                newrow["FAT"] = FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                newrow["SNF"] = SNF;
                newrow["CLR"] = dr["clr"].ToString();
                string Rateon = dr["rate_on"].ToString();

                double weight = 0;
                double KGFAT = 0;
                double KGSNF = 0;
                double ltrs = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                Ltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                kgstotal += Kgs;
                double tstotal = 0;
                tstotal = FAT + SNF;
                newrow["TS TOTAL"] = tstotal;
                if (Rateon == "TS")
                {
                    double TS = 0;
                    TS = FAT + SNF;
                    weight = (TS * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "KGFAT")
                {
                    weight = (FAT * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "PerLtr" || Rateon == "PerKg")
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        weight = ltrs;
                        KGFAT = (FAT * ltrs) / 100;
                        KGSNF = (SNF * ltrs) / 100;
                    }
                    else
                    {
                        weight = Kgs;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                }
                double cost = 0;
                double.TryParse(dr["cost"].ToString(), out cost);
                newrow["TS RATE"] = cost;
                KGFAT = Math.Round(KGFAT, 2);
                newrow["KG FAT"] = KGFAT;
                kgfattotal += KGFAT;
                KGSNF = Math.Round(KGSNF, 2);
                newrow["KG SNF"] = KGSNF;
                kgsnftotal += KGSNF;
                double MValue = 0;
                if (Rateon == "PerLtr")
                {
                    MValue = cost * qty_ltr;
                }
                else if (Rateon == "PerKg")
                {
                    MValue = cost * Kgs;
                }
                else
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        MValue = tstotal * cost * ltrs;
                        MValue = MValue / 100;
                    }
                    else
                    {
                        MValue = tstotal * cost * Kgs;
                        MValue = MValue / 100;
                    }
                }
                MValue = Math.Round(MValue, 2);
                newrow["M VALUE"] = MValue;
                mvaluetotal += MValue;
                string OverheadOn = dr["overheadon"].ToString();
                double OHcost = 0;
                double overheadcost = 0;
                double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                if (OverheadOn == "Ltrs")
                {
                    OHcost = overheadcost * ltrs;
                }
                else
                {
                    OHcost = overheadcost * Kgs;
                }


                double MSnf = 0;
                double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                double m_snfpluscost = 0;
                double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                double DiffSNFCost = 0;
                if (SNF < MSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    diffSNF = Math.Round(diffSNF, 2);
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                    }
                }


                double p_snfpluscost = 0;
                double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                double PSnf = 0;
                double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                if (SNF > PSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - PSnf;
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                    }
                }

                double MFat = 0;
                double.TryParse(dr["m_std_fat"].ToString(), out MFat);
                double m_fatpluscost = 0;
                double.TryParse(dr["m_fatpluscost"].ToString(), out m_fatpluscost);
                double DiffFATCost = 0;
                if (FAT < MFat)
                {
                    string FATOn = dr["fatplus_on"].ToString();
                    double diffFAT = 0;
                    diffFAT = FAT - MFat;
                    diffFAT = Math.Round(diffFAT, 2);
                    if (FATOn == "Ltrs")
                    {
                        DiffFATCost = diffFAT * ltrs * m_fatpluscost * 10;
                    }
                    else
                    {
                        DiffFATCost = diffFAT * Kgs * m_fatpluscost * 10;
                    }
                }
                double p_fatpluscost = 0;
                double.TryParse(dr["p_fatpluscost"].ToString(), out p_fatpluscost);
                double PFat = 0;
                double.TryParse(dr["p_std_fat"].ToString(), out PFat);
                if (FAT > PFat)
                {
                    string FATOn = dr["fatplus_on"].ToString();
                    double diffFAT = 0;
                    diffFAT = FAT - PFat;
                    if (FATOn == "Ltrs")
                    {
                        DiffFATCost = diffFAT * ltrs * p_fatpluscost * 10;
                    }
                    else
                    {
                        DiffFATCost = diffFAT * Kgs * p_fatpluscost * 10;
                    }
                }
                DiffFATCost = Math.Round(DiffFATCost, 2);
                newrow["FAT+/-"] = DiffFATCost;
                double OHandMvalue = 0;
                OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                newrow["OH"] = OHcost;
                ohtotal += OHcost;
                DiffSNFCost = Math.Round(DiffSNFCost, 2);
                newrow["SNF9"] = DiffSNFCost;
                snf9total += DiffSNFCost;
                OHandMvalue = Math.Round(OHandMvalue, 2);
                newrow["MILK VALUE"] = OHandMvalue;
                milkvaluetotal += OHandMvalue;
                newrow["Transaction NO"] = dr["dcno"].ToString();
                newrow["DC NO"] = dr["partydcno"].ToString();
                newrow["CC Name"] = dr["vendorname"].ToString();
                newrow["TANKER NO"] = dr["vehicleno"].ToString();
                dcondReport.Rows.Add(newrow);
            }
            DataRow newvartical2 = dcondReport.NewRow();
            newvartical2["DATE"] = "Total";
            newvartical2["KGS"] = kgstotal;
            newvartical2["LTRS"] = Ltrstotal;
            double fattotal = 0;
            fattotal = (kgfattotal / kgstotal) * 100;
            fattotal = Math.Round(fattotal, 2);
            newvartical2["FAT"] = fattotal;
            newvartical2["KG FAT"] = kgfattotal;
            double snftotal = 0;
            snftotal = (kgsnftotal / kgstotal) * 100;
            snftotal = Math.Round(snftotal, 2);
            TStotal = snftotal + fattotal;
            newvartical2["SNF"] = snftotal;
            newvartical2["KG SNF"] = kgsnftotal;
            newvartical2["TS TOTAL"] = TStotal;
            newvartical2["M VALUE"] = mvaluetotal;
            newvartical2["OH"] = ohtotal;
            newvartical2["SNF9"] = snf9total;
            newvartical2["MILK VALUE"] = milkvaluetotal;
            double ts = TStotal * Ltrstotal;
            double tsratetotal = 0;
            tsratetotal = (mvaluetotal / ts) * 100;
            tsratetotal = Math.Round(tsratetotal);
            newvartical2["TS RATE"] = tsratetotal;
            dcondReport.Rows.Add(newvartical2);
            Session["xportdata"] = dcondReport;
            grdcond.DataSource = dcondReport;
            grdcond.DataBind();
            plncond.Visible = true;
        }
        else
        {
            plncond.Visible = false;
        }
        // merge tables 
        DataTable dt_new = new DataTable();
        dt_new.Merge(Report);
        dt_new.Merge(dtReport);
        dt_new.Merge(dsReport);
        dt_new.Merge(dcondReport);
        dt_new.AcceptChanges();
        Session["xportdata"] = dt_new;
    }
    DataTable dtdirectpurchase = new DataTable();
    DataTable dtpcow = new DataTable();
    private void directpurchasedetails()
    {
        //pnldirectcow.Visible = false;
        //pnldirectbuf.Visible = false;
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
        grddpurchase.DataSource = null;
        grddpurchase.DataBind();
        dtdirectpurchase.Columns.Add("Sno");
        dtdirectpurchase.Columns.Add("DATE");
        dtdirectpurchase.Columns.Add("KGS").DataType = typeof(Double);
        dtdirectpurchase.Columns.Add("LTRS").DataType = typeof(Double);
        dtdirectpurchase.Columns.Add("FAT");
        dtdirectpurchase.Columns.Add("SNF");
        dtdirectpurchase.Columns.Add("CLR");
        dtdirectpurchase.Columns.Add("KG FAT RATE");
        dtdirectpurchase.Columns.Add("KG FAT").DataType = typeof(Double);
        dtdirectpurchase.Columns.Add("KG SNF").DataType = typeof(Double);
        dtdirectpurchase.Columns.Add("M VALUE").DataType = typeof(Double);
        dtdirectpurchase.Columns.Add("OH").DataType = typeof(Double);
        dtdirectpurchase.Columns.Add("FAT+/-").DataType = typeof(Double);
        dtdirectpurchase.Columns.Add("SNF9").DataType = typeof(Double);
        dtdirectpurchase.Columns.Add("MILK VALUE").DataType = typeof(Double);
        dtdirectpurchase.Columns.Add("Transaction NO");
        dtdirectpurchase.Columns.Add("DC NO");
        dtdirectpurchase.Columns.Add("CC Name");
        dtdirectpurchase.Columns.Add("TANKER NO");
        if (BranchType == "Plant")
        {
            cmd = new SqlCommand("  SELECT directsales_purchase.dcno,directsales_purchase.partydcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, directsales_purchase.doe, directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost, directsales_purchaselogs.fatplus_on, directsales_purchaselogs.m_fatpluscost, directsales_purchaselogs.p_fatpluscost, directsales_purchaselogs.m_std_fat, directsales_purchaselogs.p_std_fat, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.doe BETWEEN @d1 AND @d2) AND (directsales_purchase.transtype = @transtype) AND (directsales_purchaselogs.milktype='Buffalo') AND (directsales_purchase.sectionid=@sectionid) AND (directsales_purchase.branchid=@branchid)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@transtype", "in");
            cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
            cmd.Parameters.Add("@branchid", BranchID);
        }
        else
        {
            cmd = new SqlCommand("  SELECT directsales_purchase.dcno,directsales_purchase.partydcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, directsales_purchase.doe, directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost, directsales_purchaselogs.fatplus_on, directsales_purchaselogs.m_fatpluscost, directsales_purchaselogs.p_fatpluscost, directsales_purchaselogs.m_std_fat, directsales_purchaselogs.p_std_fat, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.doe BETWEEN @d1 AND @d2) AND (directsales_purchase.transtype = @transtype) AND (directsales_purchaselogs.milktype='Buffalo') AND (directsales_purchase.sectionid=@sectionid)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@transtype", "in");
            cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
        }
        DataTable dtBufello = SalesDB.SelectQuery(cmd).Tables[0];
        int i = 1;
        if (dtBufello.Rows.Count > 0)
        {
            double kgfattotal = 0;
            double kgsnftotal = 0;
            double kgstotal = 0;
            double Ltrstotal = 0;
            double mvaluetotal = 0;
            double ohtotal = 0;
            double snf9total = 0;
            double milkvaluetotal = 0;
            foreach (DataRow dr in dtBufello.Rows)
            {
                DataRow newrow = dtdirectpurchase.NewRow();
                newrow["Sno"] = i++.ToString();
                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                string date = dtdoe.ToString("dd/MM/yyyy");
                newrow["DATE"] = date;
                newrow["KGS"] = dr["qty_kgs"].ToString();

                double qty_ltr = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                newrow["LTRS"] = dr["qty_ltr"].ToString();
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                FAT = Math.Round(FAT, 2);
                newrow["FAT"] = FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                newrow["SNF"] = SNF;
                newrow["CLR"] = dr["clr"].ToString();
                string Rateon = dr["rate_on"].ToString();


                double weight = 0;
                double KGFAT = 0;
                double KGSNF = 0;
                double ltrs = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                Ltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                kgstotal += Kgs;
                double tstotal = 0;
                tstotal = FAT + SNF;
                if (Rateon == "TS")
                {

                    double TS = 0;
                    TS = FAT + SNF;
                    weight = (TS * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "KGFAT")
                {
                    weight = (FAT * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "PerLtr" || Rateon == "PerKg")
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        weight = ltrs;
                        KGFAT = (FAT * ltrs) / 100;
                        KGSNF = (SNF * ltrs) / 100;
                    }
                    else
                    {
                        weight = Kgs;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                }
                double cost = 0;
                double.TryParse(dr["cost"].ToString(), out cost);
                newrow["KG FAT RATE"] = cost;
                KGFAT = Math.Round(KGFAT, 2);
                newrow["KG FAT"] = KGFAT;
                kgfattotal += KGFAT;
                KGSNF = Math.Round(KGSNF, 2);
                newrow["KG SNF"] = KGSNF;
                kgsnftotal += KGSNF;
                double MValue = 0;
                MValue = KGFAT * cost;
                //MValue = MValue / 100;
                MValue = Math.Round(MValue, 2);
                newrow["M VALUE"] = MValue;
                mvaluetotal += MValue;
                string OverheadOn = dr["overheadon"].ToString();
                double OHcost = 0;
                double overheadcost = 0;
                double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                if (OverheadOn == "Ltrs")
                {
                    OHcost = overheadcost * ltrs;
                }
                else
                {
                    OHcost = overheadcost * Kgs;
                }

                double MSnf = 0;
                double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                double m_snfpluscost = 0;
                double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                double DiffSNFCost = 0;
                if (SNF < MSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    diffSNF = Math.Round(diffSNF, 2);
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                    }
                }
                double p_snfpluscost = 0;
                double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                double PSnf = 0;
                double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                if (SNF > PSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                    }
                }

                double OHandMvalue = 0;
                OHandMvalue = MValue + OHcost + DiffSNFCost;
                newrow["OH"] = OHcost;
                ohtotal += OHcost;
                DiffSNFCost = Math.Round(DiffSNFCost, 2);
                newrow["SNF9"] = DiffSNFCost;
                snf9total += DiffSNFCost;
                OHandMvalue = Math.Round(OHandMvalue, 2);
                newrow["MILK VALUE"] = OHandMvalue;
                milkvaluetotal += OHandMvalue;
                newrow["Transaction NO"] = dr["dcno"].ToString();
                newrow["DC NO"] = dr["partydcno"].ToString();
                newrow["CC Name"] = dr["vendorname"].ToString();
                newrow["TANKER NO"] = dr["vehicleno"].ToString();
                dtdirectpurchase.Rows.Add(newrow);
            }
            DataRow newvartical2 = dtdirectpurchase.NewRow();
            newvartical2["DATE"] = "Total";
            newvartical2["KGS"] = kgstotal;
            newvartical2["LTRS"] = Ltrstotal;
            double fattotal = 0;
            fattotal = (kgfattotal / kgstotal) * 100;
            fattotal = Math.Round(fattotal, 2);
            newvartical2["FAT"] = fattotal;
            newvartical2["KG FAT"] = kgfattotal;
            double snftotal = 0;
            snftotal = (kgsnftotal / kgstotal) * 100;
            snftotal = Math.Round(snftotal, 2);
            newvartical2["SNF"] = snftotal;
            newvartical2["KG SNF"] = kgsnftotal;
            newvartical2["M VALUE"] = mvaluetotal;
            newvartical2["OH"] = ohtotal;
            newvartical2["SNF9"] = snf9total;
            newvartical2["MILK VALUE"] = milkvaluetotal;
            double kgfatratetotal = 0;
            kgfatratetotal = mvaluetotal / kgfattotal;
            kgfatratetotal = Math.Round(kgfatratetotal, 2);
            newvartical2["KG FAT RATE"] = kgfatratetotal;
            dtdirectpurchase.Rows.Add(newvartical2);
            DataRow New1 = dtdirectpurchase.NewRow();
            New1["DATE"] = "Cow";
            dtdirectpurchase.Rows.Add(New1);
            Session["xportdata"] = dtdirectpurchase;
            grddpurchase.DataSource = dtdirectpurchase;
            grddpurchase.DataBind();
        }
        DataTable dtReport = new DataTable();
        dtReport.Columns.Add("Sno");
        dtReport.Columns.Add("DATE");
        dtReport.Columns.Add("KGS").DataType = typeof(Double);
        dtReport.Columns.Add("LTRS").DataType = typeof(Double);
        dtReport.Columns.Add("FAT");
        dtReport.Columns.Add("SNF");
        dtReport.Columns.Add("CLR");
        dtReport.Columns.Add("TS RATE");
        dtReport.Columns.Add("KG FAT").DataType = typeof(Double);
        dtReport.Columns.Add("KG SNF").DataType = typeof(Double);
        dtReport.Columns.Add("TS TOTAL");
        dtReport.Columns.Add("M VALUE").DataType = typeof(Double);
        dtReport.Columns.Add("OH").DataType = typeof(Double);
        dtReport.Columns.Add("FAT+/-").DataType = typeof(Double);
        dtReport.Columns.Add("SNF9").DataType = typeof(Double);
        dtReport.Columns.Add("MILK VALUE").DataType = typeof(Double);
        dtReport.Columns.Add("Transaction NO");
        dtReport.Columns.Add("DC NO");
        dtReport.Columns.Add("CC Name");
        dtReport.Columns.Add("TANKER NO");

        cmd = new SqlCommand("  SELECT directsales_purchase.dcno, directsales_purchase.partydcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, directsales_purchase.doe, directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost,directsales_purchaselogs.fatplus_on, directsales_purchaselogs.m_fatpluscost, directsales_purchaselogs.p_fatpluscost, directsales_purchaselogs.m_std_fat, directsales_purchaselogs.p_std_fat, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.doe BETWEEN @d1 AND @d2) AND (directsales_purchase.transtype = @transtype) AND (directsales_purchaselogs.milktype='Cow') AND (directsales_purchase.sectionid=@sectionid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@transtype", "in");
        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
        DataTable dtcow = SalesDB.SelectQuery(cmd).Tables[0];

        if (dtcow.Rows.Count > 0)
        {
            double kgfattotal = 0;
            double kgsnftotal = 0;
            double kgstotal = 0;
            double Ltrstotal = 0;
            double TStotal = 0;
            double mvaluetotal = 0;
            double ohtotal = 0;
            double snf9total = 0;
            double milkvaluetotal = 0;
            pnlcow.Visible = true;
            foreach (DataRow dr in dtcow.Rows)
            {
                DataRow newrow = dtReport.NewRow();
                newrow["Sno"] = i++.ToString();
                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                string date = dtdoe.ToString("dd/MM/yyyy");
                newrow["DATE"] = date;
                newrow["KGS"] = dr["qty_kgs"].ToString();

                double qty_ltr = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                newrow["LTRS"] = dr["qty_ltr"].ToString();
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                FAT = Math.Round(FAT, 2);
                newrow["FAT"] = FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                newrow["SNF"] = SNF;
                newrow["CLR"] = dr["clr"].ToString();
                string Rateon = dr["rate_on"].ToString();


                double weight = 0;
                double KGFAT = 0;
                double KGSNF = 0;
                double ltrs = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                Ltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                kgstotal += Kgs;
                double tstotal = 0;
                tstotal = FAT + SNF;
                newrow["TS TOTAL"] = tstotal;
                if (Rateon == "TS")
                {

                    double TS = 0;
                    TS = FAT + SNF;
                    weight = (TS * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "KGFAT")
                {
                    weight = (FAT * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "PerLtr" || Rateon == "PerKg")
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        weight = ltrs;
                        KGFAT = (FAT * ltrs) / 100;
                        KGSNF = (SNF * ltrs) / 100;
                    }
                    else
                    {
                        weight = Kgs;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                }
                double cost = 0;
                double.TryParse(dr["cost"].ToString(), out cost);
                newrow["TS RATE"] = cost;
                KGFAT = Math.Round(KGFAT, 2);
                newrow["KG FAT"] = KGFAT;
                kgfattotal += KGFAT;
                KGSNF = Math.Round(KGSNF, 2);
                newrow["KG SNF"] = KGSNF;
                kgsnftotal += KGSNF;
                double MValue = 0;
                if (Rateon == "PerLtr")
                {
                    MValue = cost * qty_ltr;
                }
                else if (Rateon == "PerKg")
                {
                    MValue = cost * Kgs;
                }
                else
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        MValue = tstotal * cost * ltrs;
                        MValue = MValue / 100;
                    }
                    else
                    {
                        MValue = tstotal * cost * Kgs;
                        MValue = MValue / 100;
                    }

                }
                newrow["M VALUE"] = MValue;
                mvaluetotal += MValue;
                string OverheadOn = dr["overheadon"].ToString();
                double OHcost = 0;
                double overheadcost = 0;
                double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                if (OverheadOn == "Ltrs")
                {
                    OHcost = overheadcost * ltrs;
                }
                else
                {
                    OHcost = overheadcost * Kgs;
                }
                double MSnf = 0;
                double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                double m_snfpluscost = 0;
                double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                double DiffSNFCost = 0;
                if (SNF < MSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    diffSNF = Math.Round(diffSNF, 2);
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                    }
                }
                double p_snfpluscost = 0;
                double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                double PSnf = 0;
                double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                if (SNF > PSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                    }
                }

                double MFat = 0;
                double.TryParse(dr["m_std_fat"].ToString(), out MFat);
                double m_fatpluscost = 0;
                double.TryParse(dr["m_fatpluscost"].ToString(), out m_fatpluscost);
                double DiffFATCost = 0;
                if (FAT < MFat)
                {
                    string FATOn = dr["fatplus_on"].ToString();
                    double diffFAT = 0;
                    diffFAT = FAT - MFat;
                    diffFAT = Math.Round(diffFAT, 2);
                    if (FATOn == "Ltrs")
                    {
                        DiffFATCost = diffFAT * ltrs * m_fatpluscost * 10;
                    }
                    else
                    {
                        DiffFATCost = diffFAT * Kgs * m_fatpluscost * 10;
                    }
                }
                double p_fatpluscost = 0;
                double.TryParse(dr["p_fatpluscost"].ToString(), out p_fatpluscost);
                double PFat = 0;
                double.TryParse(dr["p_std_fat"].ToString(), out PFat);
                if (FAT > PFat)
                {
                    string FATOn = dr["fatplus_on"].ToString();
                    double diffFAT = 0;
                    diffFAT = FAT - PFat;
                    if (FATOn == "Ltrs")
                    {
                        DiffFATCost = diffFAT * ltrs * p_fatpluscost * 10;
                    }
                    else
                    {
                        DiffFATCost = diffFAT * Kgs * p_fatpluscost * 10;
                    }
                }
                DiffFATCost = Math.Round(DiffFATCost, 2);

                newrow["FAT+/-"] = DiffFATCost;

                double OHandMvalue = 0;
                OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                newrow["OH"] = OHcost;
                ohtotal += OHcost;
                DiffSNFCost = Math.Round(DiffSNFCost, 2);
                newrow["SNF9"] = DiffSNFCost;
                snf9total += DiffSNFCost;
                OHandMvalue = Math.Round(OHandMvalue, 2);
                newrow["MILK VALUE"] = OHandMvalue;
                milkvaluetotal += OHandMvalue;
                newrow["Transaction NO"] = dr["dcno"].ToString();
                newrow["DC NO"] = dr["partydcno"].ToString();
                newrow["CC Name"] = dr["vendorname"].ToString();
                newrow["TANKER NO"] = dr["vehicleno"].ToString();
                dtReport.Rows.Add(newrow);
            }
            DataRow newvartical2 = dtReport.NewRow();
            newvartical2["DATE"] = "Total";
            newvartical2["KGS"] = kgstotal;
            newvartical2["LTRS"] = Ltrstotal;
            double fattotal = 0;
            fattotal = (kgfattotal / kgstotal) * 100;
            fattotal = Math.Round(fattotal, 2);
            newvartical2["FAT"] = fattotal;
            newvartical2["KG FAT"] = kgfattotal;
            double snftotal = 0;
            snftotal = (kgsnftotal / kgstotal) * 100;
            snftotal = Math.Round(snftotal, 2);
            TStotal = snftotal + fattotal;
            newvartical2["SNF"] = snftotal;
            newvartical2["KG SNF"] = kgsnftotal;
            newvartical2["TS TOTAL"] = TStotal;
            newvartical2["M VALUE"] = mvaluetotal;
            newvartical2["OH"] = ohtotal;
            newvartical2["SNF9"] = snf9total;
            newvartical2["MILK VALUE"] = milkvaluetotal;
            double ts = TStotal * Ltrstotal;
            double tsratetotal = 0;
            tsratetotal = (mvaluetotal / ts) * 100;
            tsratetotal = Math.Round(tsratetotal, 2);
            newvartical2["TS RATE"] = tsratetotal;
            dtReport.Rows.Add(newvartical2);
            Session["xportdata"] = dtReport;
            grddpurchase.DataSource = dtReport;
            grddpurchase.DataBind();
            pnldirectpurchage.Visible = true;
        }
        else
        {
            pnldirectpurchage.Visible = false;
        }
    }

    DataTable dtdirectsales = new DataTable();
    DataTable dtscow = new DataTable();
    private void directtsalesdetails()
    {
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
        grddsales.DataSource = null;
        grddsales.DataBind();
        dtdirectsales.Columns.Add("Sno");
        dtdirectsales.Columns.Add("DATE");
        dtdirectsales.Columns.Add("KGS").DataType = typeof(Double);
        dtdirectsales.Columns.Add("LTRS").DataType = typeof(Double);
        dtdirectsales.Columns.Add("FAT");
        dtdirectsales.Columns.Add("SNF");
        dtdirectsales.Columns.Add("CLR");
        dtdirectsales.Columns.Add("KG FAT RATE");
        dtdirectsales.Columns.Add("KG FAT").DataType = typeof(Double);
        dtdirectsales.Columns.Add("KG SNF").DataType = typeof(Double);
        dtdirectsales.Columns.Add("M VALUE").DataType = typeof(Double);
        dtdirectsales.Columns.Add("OH").DataType = typeof(Double);
        dtdirectsales.Columns.Add("SNF9").DataType = typeof(Double);
        dtdirectsales.Columns.Add("MILK VALUE").DataType = typeof(Double);
        dtdirectsales.Columns.Add("Transaction NO");
        dtdirectsales.Columns.Add("DC NO");
        dtdirectsales.Columns.Add("CC Name");
        dtdirectsales.Columns.Add("TANKER NO");

        cmd = new SqlCommand("  SELECT directsales_sales.dcno,directsales_sales.partydcno, directsales_sales.inwardno AS InwardNo, directsales_sales.vehicleno, directsales_sales.doe, directsales_sales.transtype, directsales_sales.qty_ltr, directsales_sales.qty_kgs, directsales_sales.percentageon, directsales_sales.snf, directsales_sales.fat, directsales_sales.clr, directsales_saleslogs.milktype, directsales_saleslogs.rate_on, directsales_saleslogs.cost, directsales_saleslogs.calc_on, directsales_saleslogs.overheadon, directsales_saleslogs.overheadcost, directsales_saleslogs.m_std_snf, directsales_saleslogs.p_std_snf, directsales_saleslogs.snfplus_on, directsales_saleslogs.m_snfpluscost, directsales_saleslogs.p_snfpluscost, directsales_saleslogs.transport_on, directsales_saleslogs.transportcost, directsales_saleslogs.transport, vendors.vendorname FROM directsales_sales INNER JOIN directsales_saleslogs ON directsales_sales.sno = directsales_saleslogs.salesrefno INNER JOIN vendors ON directsales_sales.sectionid = vendors.sno WHERE  (directsales_sales.doe BETWEEN @d1 AND @d2) AND (directsales_sales.transtype = @transtype) AND (directsales_saleslogs.milktype='Buffalo') AND (directsales_sales.sectionid=@sectionid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@transtype", "in");
        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
        DataTable dtBufello = SalesDB.SelectQuery(cmd).Tables[0];
        int i = 1;
        if (dtBufello.Rows.Count > 0)
        {
            double kgfattotal = 0;
            double kgsnftotal = 0;
            double kgstotal = 0;
            double Ltrstotal = 0;
            double mvaluetotal = 0;
            double ohtotal = 0;
            double snf9total = 0;
            double milkvaluetotal = 0;
            foreach (DataRow dr in dtBufello.Rows)
            {
                DataRow newrow = dtdirectsales.NewRow();
                newrow["Sno"] = i++.ToString();
                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                string date = dtdoe.ToString("dd/MM/yyyy");
                newrow["DATE"] = date;
                newrow["KGS"] = dr["qty_kgs"].ToString();

                double qty_ltr = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                newrow["LTRS"] = dr["qty_ltr"].ToString();
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                FAT = Math.Round(FAT, 2);
                newrow["FAT"] = FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                newrow["SNF"] = SNF;
                newrow["CLR"] = dr["clr"].ToString();
                string Rateon = dr["rate_on"].ToString();


                double weight = 0;
                double KGFAT = 0;
                double KGSNF = 0;
                double ltrs = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                Ltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                kgstotal += Kgs;
                double tstotal = 0;
                tstotal = FAT + SNF;
                if (Rateon == "TS")
                {

                    double TS = 0;
                    TS = FAT + SNF;
                    weight = (TS * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "KGFAT")
                {
                    weight = (FAT * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "PerLtr" || Rateon == "PerKg")
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        weight = ltrs;
                        KGFAT = (FAT * ltrs) / 100;
                        KGSNF = (SNF * ltrs) / 100;
                    }
                    else
                    {
                        weight = Kgs;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                }
                double cost = 0;
                double.TryParse(dr["cost"].ToString(), out cost);
                newrow["KG FAT RATE"] = cost;
                KGFAT = Math.Round(KGFAT, 2);
                newrow["KG FAT"] = KGFAT;
                kgfattotal += KGFAT;
                KGSNF = Math.Round(KGSNF, 2);
                newrow["KG SNF"] = KGSNF;
                kgsnftotal += KGSNF;
                double MValue = 0;
                MValue = KGFAT * cost;
                //MValue = MValue / 100;
                MValue = Math.Round(MValue, 2);
                newrow["M VALUE"] = MValue;
                mvaluetotal += MValue;
                string OverheadOn = dr["overheadon"].ToString();
                double OHcost = 0;
                double overheadcost = 0;
                double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                if (OverheadOn == "Ltrs")
                {
                    OHcost = overheadcost * ltrs;
                }
                else
                {
                    OHcost = overheadcost * Kgs;
                }
                double MSnf = 0;
                double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                double m_snfpluscost = 0;
                double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                double DiffSNFCost = 0;
                if (SNF < MSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    diffSNF = Math.Round(diffSNF, 2);
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                    }
                }
                double p_snfpluscost = 0;
                double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                double PSnf = 0;
                double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                if (SNF > PSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                    }
                }

                double OHandMvalue = 0;
                OHandMvalue = MValue + OHcost + DiffSNFCost;
                newrow["OH"] = OHcost;
                ohtotal += OHcost;
                DiffSNFCost = Math.Round(DiffSNFCost, 2);
                newrow["SNF9"] = DiffSNFCost;
                snf9total += DiffSNFCost;
                OHandMvalue = Math.Round(OHandMvalue, 2);
                newrow["MILK VALUE"] = OHandMvalue;
                milkvaluetotal += OHandMvalue;
                newrow["Transaction NO"] = dr["dcno"].ToString();
                newrow["DC NO"] = dr["partydcno"].ToString();
                newrow["CC Name"] = dr["vendorname"].ToString();
                newrow["TANKER NO"] = dr["vehicleno"].ToString();
                dtdirectsales.Rows.Add(newrow);
            }
            DataRow newvartical2 = dtdirectsales.NewRow();
            newvartical2["DATE"] = "Total";
            newvartical2["KGS"] = kgstotal;
            newvartical2["LTRS"] = Ltrstotal;
            double fattotal = 0;
            fattotal = (kgfattotal / kgstotal) * 100;
            fattotal = Math.Round(fattotal, 2);
            newvartical2["FAT"] = fattotal;
            newvartical2["KG FAT"] = kgfattotal;
            double snftotal = 0;
            snftotal = (kgsnftotal / kgstotal) * 100;
            snftotal = Math.Round(snftotal, 2);
            newvartical2["SNF"] = snftotal;
            newvartical2["KG SNF"] = kgsnftotal;
            newvartical2["M VALUE"] = mvaluetotal;
            newvartical2["OH"] = ohtotal;
            newvartical2["SNF9"] = snf9total;
            newvartical2["MILK VALUE"] = milkvaluetotal;
            double kgfatratetotal = 0;
            kgfatratetotal = mvaluetotal / kgfattotal;
            kgfatratetotal = Math.Round(kgfatratetotal, 2);
            newvartical2["KG FAT RATE"] = kgfatratetotal;
            dtdirectsales.Rows.Add(newvartical2);
            DataRow New1 = dtdirectsales.NewRow();
            New1["DATE"] = "Cow";
            dtdirectsales.Rows.Add(New1);
            Session["xportdata"] = dtdirectsales;
            grddsales.DataSource = dtdirectsales;
            grddsales.DataBind();
            pnldirectsale.Visible = true;
        }
        DataTable dtReport = new DataTable();
        dtReport.Columns.Add("Sno");
        dtReport.Columns.Add("DATE");
        dtReport.Columns.Add("KGS").DataType = typeof(Double);
        dtReport.Columns.Add("LTRS").DataType = typeof(Double);
        dtReport.Columns.Add("FAT");
        dtReport.Columns.Add("SNF");
        dtReport.Columns.Add("CLR");
        dtReport.Columns.Add("TS RATE");
        dtReport.Columns.Add("KG FAT").DataType = typeof(Double);
        dtReport.Columns.Add("KG SNF").DataType = typeof(Double);
        dtReport.Columns.Add("TS TOTAL");
        dtReport.Columns.Add("M VALUE").DataType = typeof(Double);
        dtReport.Columns.Add("OH").DataType = typeof(Double);
        dtReport.Columns.Add("SNF9").DataType = typeof(Double);
        dtReport.Columns.Add("MILK VALUE").DataType = typeof(Double);
        dtReport.Columns.Add("Transaction NO");
        dtReport.Columns.Add("DC NO");
        dtReport.Columns.Add("CC Name");
        dtReport.Columns.Add("TANKER NO");

        cmd = new SqlCommand("  SELECT directsales_sales.dcno, directsales_sales.partydcno, directsales_sales.inwardno AS InwardNo, directsales_sales.vehicleno, directsales_sales.doe, directsales_sales.transtype, directsales_sales.qty_ltr, directsales_sales.qty_kgs, directsales_sales.percentageon, directsales_sales.snf, directsales_sales.fat, directsales_sales.clr, directsales_saleslogs.milktype, directsales_saleslogs.rate_on, directsales_saleslogs.cost, directsales_saleslogs.calc_on, directsales_saleslogs.overheadon, directsales_saleslogs.overheadcost, directsales_saleslogs.m_std_snf, directsales_saleslogs.p_std_snf, directsales_saleslogs.snfplus_on, directsales_saleslogs.m_snfpluscost, directsales_saleslogs.p_snfpluscost, directsales_saleslogs.transport_on, directsales_saleslogs.transportcost, directsales_saleslogs.transport, vendors.vendorname FROM directsales_sales INNER JOIN directsales_saleslogs ON directsales_sales.sno = directsales_saleslogs.salesrefno INNER JOIN vendors ON directsales_sales.sectionid = vendors.sno WHERE  (directsales_sales.doe BETWEEN @d1 AND @d2) AND (directsales_sales.transtype = @transtype) AND (directsales_saleslogs.milktype='Cow') AND (directsales_sales.sectionid=@sectionid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@transtype", "in");
        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
        DataTable dtcow = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtcow.Rows.Count > 0)
        {
            double kgfattotal = 0;
            double kgsnftotal = 0;
            double kgstotal = 0;
            double Ltrstotal = 0;
            double TStotal = 0;
            double mvaluetotal = 0;
            double ohtotal = 0;
            double snf9total = 0;
            double milkvaluetotal = 0;
            pnlcow.Visible = true;
            foreach (DataRow dr in dtcow.Rows)
            {
                DataRow newrow = dtReport.NewRow();
                newrow["Sno"] = i++.ToString();
                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                string date = dtdoe.ToString("dd/MM/yyyy");
                newrow["DATE"] = date;
                newrow["KGS"] = dr["qty_kgs"].ToString();

                double qty_ltr = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                newrow["LTRS"] = dr["qty_ltr"].ToString();
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                FAT = Math.Round(FAT, 2);
                newrow["FAT"] = FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                newrow["SNF"] = SNF;
                newrow["CLR"] = dr["clr"].ToString();
                string Rateon = dr["rate_on"].ToString();


                double weight = 0;
                double KGFAT = 0;
                double KGSNF = 0;
                double ltrs = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                Ltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                kgstotal += Kgs;
                double tstotal = 0;
                tstotal = FAT + SNF;
                newrow["TS TOTAL"] = tstotal;
                if (Rateon == "TS")
                {

                    double TS = 0;
                    TS = FAT + SNF;
                    weight = (TS * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "KGFAT")
                {
                    weight = (FAT * Kgs) / 100;
                    KGFAT = (FAT * Kgs) / 100;
                    KGSNF = (SNF * Kgs) / 100;
                }
                else if (Rateon == "PerLtr" || Rateon == "PerKg")
                {
                    string CalOn = dr["calc_on"].ToString();
                    if (CalOn == "Ltrs")
                    {
                        weight = ltrs;
                        KGFAT = (FAT * ltrs) / 100;
                        KGSNF = (SNF * ltrs) / 100;
                    }
                    else
                    {
                        weight = Kgs;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                }
                double cost = 0;
                double.TryParse(dr["cost"].ToString(), out cost);
                newrow["TS RATE"] = cost;
                KGFAT = Math.Round(KGFAT, 2);
                newrow["KG FAT"] = KGFAT;
                kgfattotal += KGFAT;
                KGSNF = Math.Round(KGSNF, 2);
                newrow["KG SNF"] = KGSNF;
                kgsnftotal += KGSNF;
                double MValue = 0;
                MValue = tstotal * cost * qty_ltr;
                MValue = MValue / 100;
                MValue = Math.Round(MValue, 2);
                newrow["M VALUE"] = MValue;
                mvaluetotal += MValue;
                string OverheadOn = dr["overheadon"].ToString();
                double OHcost = 0;
                double overheadcost = 0;
                double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                if (OverheadOn == "Ltrs")
                {
                    OHcost = overheadcost * ltrs;
                }
                else
                {
                    OHcost = overheadcost * Kgs;
                }
                double MSnf = 0;
                double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                double m_snfpluscost = 0;
                double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                double DiffSNFCost = 0;
                if (SNF < MSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    diffSNF = Math.Round(diffSNF, 2);
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                    }
                }
                double p_snfpluscost = 0;
                double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                double PSnf = 0;
                double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                if (SNF > PSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                    }
                }

                double OHandMvalue = 0;
                OHandMvalue = MValue + OHcost + DiffSNFCost;
                newrow["OH"] = OHcost;
                ohtotal += OHcost;
                DiffSNFCost = Math.Round(DiffSNFCost, 2);
                newrow["SNF9"] = DiffSNFCost;
                snf9total += DiffSNFCost;
                OHandMvalue = Math.Round(OHandMvalue, 2);
                newrow["MILK VALUE"] = OHandMvalue;
                milkvaluetotal += OHandMvalue;
                newrow["Transaction NO"] = dr["dcno"].ToString();
                newrow["DC NO"] = dr["partydcno"].ToString();
                newrow["CC Name"] = dr["vendorname"].ToString();
                newrow["TANKER NO"] = dr["vehicleno"].ToString();
                dtReport.Rows.Add(newrow);
            }
            DataRow newvartical2 = dtReport.NewRow();
            newvartical2["DATE"] = "Total";
            newvartical2["KGS"] = kgstotal;
            newvartical2["LTRS"] = Ltrstotal;
            double fattotal = 0;
            fattotal = (kgfattotal / kgstotal) * 100;
            fattotal = Math.Round(fattotal, 2);
            newvartical2["FAT"] = fattotal;
            newvartical2["KG FAT"] = kgfattotal;
            double snftotal = 0;
            snftotal = (kgsnftotal / kgstotal) * 100;
            snftotal = Math.Round(snftotal, 2);
            TStotal = snftotal + fattotal;
            newvartical2["SNF"] = snftotal;
            newvartical2["KG SNF"] = kgsnftotal;
            newvartical2["TS TOTAL"] = TStotal;
            newvartical2["M VALUE"] = mvaluetotal;
            newvartical2["OH"] = ohtotal;
            newvartical2["SNF9"] = snf9total;
            newvartical2["MILK VALUE"] = milkvaluetotal;
            double ts = TStotal * Ltrstotal;
            double tsratetotal = 0;
            tsratetotal = (mvaluetotal / ts) * 100;
            tsratetotal = Math.Round(tsratetotal, 2);
            newvartical2["TS RATE"] = tsratetotal;
            dtReport.Rows.Add(newvartical2);
            Session["xportdata"] = dtReport;
            grddsales.DataSource = dtReport;
            grddsales.DataBind();
            pnldirectsale.Visible = true;
        }
        else
        {
            pnldirectsale.Visible = false;
        }
    }
}