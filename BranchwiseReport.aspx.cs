using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class BranchwiseReport : System.Web.UI.Page
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
                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.RegisterPostBackControl(this.Button1);
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
    DataTable Report1 = new DataTable();
    DataTable Report2 = new DataTable();
    DataTable Report3 = new DataTable();
    DataTable Report4 = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            grdcow.DataSource = null;
            grdcow.DataBind();
            grdReports.DataSource = null;
            grdReports.DataBind();
            Session["filename"] = "Branch Summery";
            Session["title"] = "Branch Summery Report";
            lblmsg.Text = "";
            DataTable Report = new DataTable();
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
            
            if (ddltype.SelectedValue == "All")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("Vendor Name");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("KG FAT RATE");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("OH");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");
                Report.Columns.Add("Per Ltr");
                if (ddlbranchType.SelectedValue == "All")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM    vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (vendor_subtable.milktype = @Milktype) AND (branchmapping.superbranch = @BranchID)");
                }
                if (ddlbranchType.SelectedValue == "Inter Branches")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM    vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (vendor_subtable.milktype = @Milktype) AND (branchmapping.superbranch = @BranchID) and (vendors.branchtype='Inter Branch')");
                }
                if (ddlbranchType.SelectedValue == "Other Branches")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM    vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (vendor_subtable.milktype = @Milktype) AND (branchmapping.superbranch = @BranchID) and (vendors.branchtype='Other Branch') ");
                }
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@Milktype", "Buffalo");
                DataTable dtVendor = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtVendor.Rows.Count > 0)
                {
                    double d_kgfattotal = 0;
                    double d_kgsnftotal = 0;
                    double d_kgstotal = 0;
                    double d_Ltrstotal = 0;
                    double d_TStotal = 0;
                    double d_mvaluetotal = 0;
                    double d_ohtotal = 0;
                    double d_snf9total = 0;
                    double d_milkvaluetotal = 0;
                    int i = 1;
                    foreach (DataRow drven in dtVendor.Rows)
                    {
                        cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid ");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "in");
                        cmd.Parameters.Add("@sectionid", drven["sno"].ToString());
                        cmd.Parameters.Add("@branchid", BranchID);
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
                            foreach (DataRow dr in dtDispatch.Rows)
                            {
                                DataRow newrow = Report.NewRow();
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
                                //Report.Rows.Add(newrow);
                            }
                            DataRow newvartical2 = Report.NewRow();
                            newvartical2["Sno"] = i++.ToString();
                            newvartical2["Vendor Name"] = drven["vendorname"].ToString();
                            newvartical2["KGS"] = kgstotal;
                            d_kgstotal += kgstotal;
                            newvartical2["LTRS"] = Ltrstotal;
                            d_Ltrstotal += Ltrstotal;
                            double fattotal = 0;
                            fattotal = (kgfattotal / kgstotal) * 100;
                            fattotal = Math.Round(fattotal, 2);
                            newvartical2["FAT"] = fattotal;
                            newvartical2["KG FAT"] = kgfattotal;
                            d_kgfattotal += kgfattotal;
                            double snftotal = 0;
                            snftotal = (kgsnftotal / kgstotal) * 100;
                            snftotal = Math.Round(snftotal, 2);
                            newvartical2["SNF"] = snftotal;
                            newvartical2["KG SNF"] = kgsnftotal;
                            d_kgsnftotal += kgsnftotal;
                            newvartical2["M VALUE"] = mvaluetotal;
                            d_mvaluetotal += mvaluetotal;
                            newvartical2["OH"] = ohtotal;
                            d_ohtotal += ohtotal;
                            snf9total = Math.Round(snf9total, 2);
                            newvartical2["SNF9"] = snf9total;
                            d_snf9total += snf9total;
                            newvartical2["MILK VALUE"] = milkvaluetotal;
                            d_milkvaluetotal += milkvaluetotal;
                            double kgfatratetotal = 0;
                            kgfatratetotal = mvaluetotal / kgfattotal;
                            kgfatratetotal = Math.Round(kgfatratetotal, 2);
                            newvartical2["KG FAT RATE"] = kgfatratetotal;
                            double perltr = 0;
                            perltr = milkvaluetotal / Ltrstotal;
                            perltr = Math.Round(perltr, 2);
                            newvartical2["Per Ltr"] = perltr;
                            Report.Rows.Add(newvartical2);
                        }

                    }
                    DataRow new1 = Report.NewRow();
                    new1["Vendor Name"] = "Total";
                    new1["KGS"] = d_kgstotal;
                    new1["LTRS"] = d_Ltrstotal;
                    double fat_total = 0;
                    fat_total = (d_kgfattotal / d_kgstotal) * 100;
                    fat_total = Math.Round(fat_total, 2);
                    new1["FAT"] = fat_total;
                    new1["KG FAT"] = d_kgfattotal;
                    double snf_total = 0;
                    snf_total = (d_kgsnftotal / d_kgstotal) * 100;
                    snf_total = Math.Round(snf_total, 2);
                    new1["SNF"] = snf_total;
                    new1["KG SNF"] = d_kgsnftotal;
                    new1["M VALUE"] = d_mvaluetotal;
                    new1["OH"] = d_ohtotal;
                    new1["SNF9"] = d_snf9total;
                    new1["MILK VALUE"] = d_milkvaluetotal;
                    double kgfatrate_total = 0;
                    kgfatrate_total = d_mvaluetotal / d_kgfattotal;
                    kgfatrate_total = Math.Round(kgfatrate_total, 2);
                    new1["KG FAT RATE"] = kgfatrate_total;
                    double per_ltr = 0;
                    per_ltr = d_milkvaluetotal / d_Ltrstotal;
                    per_ltr = Math.Round(per_ltr, 2);
                    new1["Per Ltr"] = per_ltr;
                    Report.Rows.Add(new1);
                    DataRow new2 = Report.NewRow();
                    new2["Vendor Name"] = "";
                    Report.Rows.Add(new2);
                    DataRow new3 = Report.NewRow();
                    new3["Vendor Name"] = "Cow";
                    Report.Rows.Add(new3);
                    Report1 = Report;
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                }
                DataTable dtReport = new DataTable();
                dtReport.Columns.Add("Sno");
                dtReport.Columns.Add("Vendor Name");
                dtReport.Columns.Add("KGS");
                dtReport.Columns.Add("LTRS");
                dtReport.Columns.Add("FAT");
                dtReport.Columns.Add("SNF");
                dtReport.Columns.Add("TS RATE");
                dtReport.Columns.Add("KG FAT");
                dtReport.Columns.Add("KG SNF");
                dtReport.Columns.Add("TS TOTAL");
                dtReport.Columns.Add("M VALUE");
                dtReport.Columns.Add("OH");
                dtReport.Columns.Add("SNF9");
                dtReport.Columns.Add("MILK VALUE");
                dtReport.Columns.Add("Per Ltr");
                if (ddlbranchType.SelectedValue == "All")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM    vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (vendor_subtable.milktype = @Milktype) AND (branchmapping.superbranch = @BranchID)");
                }
                if (ddlbranchType.SelectedValue == "Inter Branches")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM    vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (vendor_subtable.milktype = @Milktype) AND (branchmapping.superbranch = @BranchID) and (vendors.branchtype='Inter Branch')");
                }
                if (ddlbranchType.SelectedValue == "Other Branches")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM    vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (vendor_subtable.milktype = @Milktype) AND (branchmapping.superbranch = @BranchID) and (vendors.branchtype='Other Branch') ");
                }
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@Milktype", "cow");
                //cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dt_Vendor = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtVendor.Rows.Count > 0)
                {
                    pnlcow.Visible = true;
                    double d_kgfattotal = 0;
                    double d_kgsnftotal = 0;
                    double d_kgstotal = 0;
                    double d_Ltrstotal = 0;
                    double d_TStotal = 0;
                    double d_mvaluetotal = 0;
                    double d_ohtotal = 0;
                    double d_snf9total = 0;
                    double d_milkvaluetotal = 0;
                    int i = 1;
                    foreach (DataRow drven in dt_Vendor.Rows)
                    {
                        cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "in");
                        cmd.Parameters.Add("@branchid", BranchID);
                        cmd.Parameters.Add("@sectionid", drven["sno"].ToString());
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
                            foreach (DataRow dr in dtDispatch.Rows)
                            {
                                DataRow newrow = dtReport.NewRow();
                                double qty_kgs = 0;
                                double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
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
                                if (Rateon == "PerLtr" || Rateon == "PerKg")
                                {
                                    MValue = cost * qty_kgs;
                                }
                                else
                                {
                                    MValue = tstotal * cost * qty_ltr;
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

                            }
                            DataRow newvartical2 = dtReport.NewRow();
                            newvartical2["Sno"] = i++.ToString();
                            newvartical2["Vendor Name"] = drven["vendorname"].ToString();
                            newvartical2["KGS"] = kgstotal;
                            d_kgstotal += kgstotal;
                            newvartical2["LTRS"] = Ltrstotal;
                            d_Ltrstotal += Ltrstotal;
                            double fattotal = 0;
                            fattotal = (kgfattotal / kgstotal) * 100;
                            fattotal = Math.Round(fattotal, 2);
                            newvartical2["FAT"] = fattotal;
                            newvartical2["KG FAT"] = kgfattotal;
                            d_kgfattotal += kgfattotal;
                            double snftotal = 0;
                            snftotal = (kgsnftotal / kgstotal) * 100;
                            snftotal = Math.Round(snftotal, 2);
                            TStotal = snftotal + fattotal;
                            newvartical2["SNF"] = snftotal;
                            newvartical2["KG SNF"] = kgsnftotal;
                            d_kgsnftotal += kgsnftotal;
                            newvartical2["TS TOTAL"] = TStotal;
                            newvartical2["M VALUE"] = mvaluetotal;
                            d_mvaluetotal += mvaluetotal;
                            newvartical2["OH"] = ohtotal;
                            d_ohtotal += ohtotal;
                            newvartical2["SNF9"] = snf9total;
                            d_snf9total += snf9total;
                            newvartical2["MILK VALUE"] = milkvaluetotal;
                            d_milkvaluetotal += milkvaluetotal;
                            double ts = TStotal * Ltrstotal;
                            double tsratetotal = 0;
                            tsratetotal = (mvaluetotal / ts) * 100;
                            tsratetotal = Math.Round(tsratetotal, 2);
                            newvartical2["TS RATE"] = tsratetotal;
                            double perltr = 0;
                            perltr = milkvaluetotal / Ltrstotal;
                            perltr = Math.Round(perltr, 2);
                            newvartical2["Per Ltr"] = perltr;
                            dtReport.Rows.Add(newvartical2);
                        }
                    }
                    DataRow new1 = dtReport.NewRow();
                    new1["Vendor Name"] = "Total";
                    new1["KGS"] = d_kgstotal;
                    new1["LTRS"] = d_Ltrstotal;
                    double fat_total = 0;
                    fat_total = (d_kgfattotal / d_kgstotal) * 100;
                    fat_total = Math.Round(fat_total, 2);
                    new1["FAT"] = fat_total;
                    new1["KG FAT"] = d_kgfattotal;
                    double snf_total = 0;
                    snf_total = (d_kgsnftotal / d_kgstotal) * 100;
                    snf_total = Math.Round(snf_total, 2);
                    double TS_total = 0;
                    TS_total = snf_total + fat_total;
                    new1["SNF"] = snf_total;
                    new1["KG SNF"] = d_kgsnftotal;
                    new1["TS TOTAL"] = TS_total;
                    new1["M VALUE"] = d_mvaluetotal;
                    new1["OH"] = d_ohtotal;
                    new1["SNF9"] = d_snf9total;
                    new1["MILK VALUE"] = d_milkvaluetotal;
                    double B = TS_total * d_Ltrstotal;
                    double tsrate_total = 0;
                    tsrate_total = (d_mvaluetotal / B) * 100;
                    tsrate_total = Math.Round(tsrate_total, 2);
                    new1["TS RATE"] = tsrate_total;
                    double per_ltr = 0;
                    per_ltr = d_milkvaluetotal / d_Ltrstotal;
                    per_ltr = Math.Round(per_ltr, 2);
                    new1["Per Ltr"] = per_ltr;
                    dtReport.Rows.Add(new1);
                    grdcow.DataSource = dtReport;
                    grdcow.DataBind();
                    //merging first data table into second data table
                    Report1.Merge(dtReport);
                    Report1.AcceptChanges();
                    Session["xportdata"] = Report1;
                    hidepanel.Visible = true;
                }
            }
            if (ddltype.SelectedValue == "Buffalo")
            {

                Report.Columns.Add("Sno");
                Report.Columns.Add("Vendor Name");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("KG FAT RATE");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("OH");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");
                Report.Columns.Add("Per Ltr");
                if (ddlbranchType.SelectedValue == "All")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno WHERE  (vendor_subtable.milktype = @Milktype) AND (vendors.branchid = @BranchID)");
                }
                if (ddlbranchType.SelectedValue == "Inter Branches")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno WHERE  (vendor_subtable.milktype = @Milktype) AND (vendors.branchid = @BranchID) and (vendors.branchtype='Inter Branch')");
                }
                if (ddlbranchType.SelectedValue == "Other Branches")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno WHERE  (vendor_subtable.milktype = @Milktype) AND (vendors.branchid = @BranchID) and (vendors.branchtype='Other Branch') ");
                }
                //cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno WHERE  (vendor_subtable.milktype = @Milktype) AND (vendors.branchid = @BranchID)");
                cmd.Parameters.Add("@Milktype", "Buffalo");
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtVendor = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtVendor.Rows.Count > 0)
                {
                    double d_kgfattotal = 0;
                    double d_kgsnftotal = 0;
                    double d_kgstotal = 0;
                    double d_Ltrstotal = 0;
                    double d_TStotal = 0;
                    double d_mvaluetotal = 0;
                    double d_ohtotal = 0;
                    double d_snf9total = 0;
                    double d_milkvaluetotal = 0;
                    int i = 1;
                    foreach (DataRow drven in dtVendor.Rows)
                    {
                        cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid ");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "in");
                        cmd.Parameters.Add("@branchid", BranchID);
                        cmd.Parameters.Add("@sectionid", drven["sno"].ToString());
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
                            foreach (DataRow dr in dtDispatch.Rows)
                            {
                                DataRow newrow = Report.NewRow();
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
                                //Report.Rows.Add(newrow);
                            }
                            DataRow newvartical2 = Report.NewRow();
                            newvartical2["Sno"] = i++.ToString();
                            newvartical2["Vendor Name"] = drven["vendorname"].ToString();
                            newvartical2["KGS"] = kgstotal;
                            d_kgstotal += kgstotal;
                            newvartical2["LTRS"] = Ltrstotal;
                            d_Ltrstotal += Ltrstotal;
                            double fattotal = 0;
                            fattotal = (kgfattotal / kgstotal) * 100;
                            fattotal = Math.Round(fattotal, 2);
                            newvartical2["FAT"] = fattotal;
                            newvartical2["KG FAT"] = kgfattotal;
                            d_kgfattotal += kgfattotal;
                            double snftotal = 0;
                            snftotal = (kgsnftotal / kgstotal) * 100;
                            snftotal = Math.Round(snftotal, 2);
                            newvartical2["SNF"] = snftotal;
                            newvartical2["KG SNF"] = kgsnftotal;
                            d_kgsnftotal += kgsnftotal;
                            newvartical2["M VALUE"] = mvaluetotal;
                            d_mvaluetotal += mvaluetotal;
                            newvartical2["OH"] = ohtotal;
                            d_ohtotal += ohtotal;
                            snf9total = Math.Round(snf9total, 2);
                            newvartical2["SNF9"] = snf9total;
                            d_snf9total += snf9total;
                            newvartical2["MILK VALUE"] = milkvaluetotal;
                            d_milkvaluetotal += milkvaluetotal;
                            double kgfatratetotal = 0;
                            kgfatratetotal = mvaluetotal / kgfattotal;
                            kgfatratetotal = Math.Round(kgfatratetotal, 2);
                            newvartical2["KG FAT RATE"] = kgfatratetotal;
                            double perltr = 0;
                            perltr = milkvaluetotal / Ltrstotal;
                            perltr = Math.Round(perltr, 2);
                            newvartical2["Per Ltr"] = perltr;
                            Report.Rows.Add(newvartical2);
                        }

                    }
                    DataRow new1 = Report.NewRow();
                    new1["Vendor Name"] = "Total";
                    new1["KGS"] = d_kgstotal;
                    new1["LTRS"] = d_Ltrstotal;
                    double fat_total = 0;
                    fat_total = (d_kgfattotal / d_kgstotal) * 100;
                    fat_total = Math.Round(fat_total, 2);
                    new1["FAT"] = fat_total;
                    new1["KG FAT"] = d_kgfattotal;
                    double snf_total = 0;
                    snf_total = (d_kgsnftotal / d_kgstotal) * 100;
                    snf_total = Math.Round(snf_total, 2);
                    new1["SNF"] = snf_total;
                    new1["KG SNF"] = d_kgsnftotal;
                    new1["M VALUE"] = d_mvaluetotal;
                    new1["OH"] = d_ohtotal;
                    new1["SNF9"] = d_snf9total;
                    new1["MILK VALUE"] = d_milkvaluetotal;
                    double kgfatrate_total = 0;
                    kgfatrate_total = d_mvaluetotal / d_kgfattotal;
                    kgfatrate_total = Math.Round(kgfatrate_total, 2);
                    new1["KG FAT RATE"] = kgfatrate_total;
                    double per_ltr = 0;
                    per_ltr = d_milkvaluetotal / d_Ltrstotal;
                    per_ltr = Math.Round(per_ltr, 2);
                    new1["Per Ltr"] = per_ltr;
                    Report.Rows.Add(new1);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                    hidepanel.Visible = true;
                }
            }
            if (ddltype.SelectedValue == "Cow")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("Vendor Name");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("TS RATE");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("TS TOTAL");
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("OH");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");
                Report.Columns.Add("Per Ltr");
                if (ddlbranchType.SelectedValue == "All")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM    vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (vendor_subtable.milktype = @Milktype) AND (branchmapping.superbranch = @BranchID)");
                }
                if (ddlbranchType.SelectedValue == "Inter Branches")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM    vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (vendor_subtable.milktype = @Milktype) AND (branchmapping.superbranch = @BranchID) and (vendors.branchtype='Inter Branch')");
                }
                if (ddlbranchType.SelectedValue == "Other Branches")
                {
                    cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM    vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (vendor_subtable.milktype = @Milktype) AND (branchmapping.superbranch = @BranchID) and (vendors.branchtype='Other Branch') ");
                }
                cmd.Parameters.Add("@BranchID", BranchID);
                //cmd = new SqlCommand("SELECT vendors.sno, vendors.vendorname FROM vendors INNER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno WHERE  (vendor_subtable.milktype = @Milktype) AND (vendors.branchid = @BranchID)");
                cmd.Parameters.Add("@Milktype", "cow");
                //cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtVendor = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtVendor.Rows.Count > 0)
                {
                    double d_kgfattotal = 0;
                    double d_kgsnftotal = 0;
                    double d_kgstotal = 0;
                    double d_Ltrstotal = 0;
                    double d_TStotal = 0;
                    double d_mvaluetotal = 0;
                    double d_ohtotal = 0;
                    double d_snf9total = 0;
                    double d_milkvaluetotal = 0;
                    int i = 1;
                    foreach (DataRow drven in dtVendor.Rows)
                    {
                        cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid ");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "in");
                        cmd.Parameters.Add("@branchid", BranchID);
                        cmd.Parameters.Add("@sectionid", drven["sno"].ToString());
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
                            foreach (DataRow dr in dtDispatch.Rows)
                            {
                                DataRow newrow = Report.NewRow();
                                double qty_kgs = 0;
                                double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
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
                                if (Rateon == "PerLtr" || Rateon == "PerKg")
                                {
                                    MValue = cost * qty_kgs;
                                }
                                else
                                {
                                    MValue = tstotal * cost * qty_ltr;
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

                            }
                            DataRow newvartical2 = Report.NewRow();
                            newvartical2["Sno"] = i++.ToString();
                            newvartical2["Vendor Name"] = drven["vendorname"].ToString();
                            newvartical2["KGS"] = kgstotal;
                            d_kgstotal += kgstotal;
                            newvartical2["LTRS"] = Ltrstotal;
                            d_Ltrstotal += Ltrstotal;
                            double fattotal = 0;
                            fattotal = (kgfattotal / kgstotal) * 100;
                            fattotal = Math.Round(fattotal, 2);
                            newvartical2["FAT"] = fattotal;
                            newvartical2["KG FAT"] = kgfattotal;
                            d_kgfattotal += kgfattotal;
                            double snftotal = 0;
                            snftotal = (kgsnftotal / kgstotal) * 100;
                            snftotal = Math.Round(snftotal, 2);
                            TStotal = snftotal + fattotal;
                            newvartical2["SNF"] = snftotal;
                            newvartical2["KG SNF"] = kgsnftotal;
                            d_kgsnftotal += kgsnftotal;
                            newvartical2["TS TOTAL"] = TStotal;
                            newvartical2["M VALUE"] = mvaluetotal;
                            d_mvaluetotal += mvaluetotal;
                            newvartical2["OH"] = ohtotal;
                            d_ohtotal += ohtotal;
                            newvartical2["SNF9"] = snf9total;
                            d_snf9total += snf9total;
                            newvartical2["MILK VALUE"] = milkvaluetotal;
                            d_milkvaluetotal += milkvaluetotal;
                            double ts = TStotal * Ltrstotal;
                            double tsratetotal = 0;
                            tsratetotal = (mvaluetotal / ts) * 100;
                            tsratetotal = Math.Round(tsratetotal, 2);
                            newvartical2["TS RATE"] = tsratetotal;
                            double perltr = 0;
                            perltr = milkvaluetotal / Ltrstotal;
                            perltr = Math.Round(perltr, 2);
                            newvartical2["Per Ltr"] = perltr;
                            Report.Rows.Add(newvartical2);
                        }
                    }
                    DataRow new1 = Report.NewRow();
                    new1["Vendor Name"] = "Total";
                    new1["KGS"] = d_kgstotal;
                    new1["LTRS"] = d_Ltrstotal;
                    double fat_total = 0;
                    fat_total = (d_kgfattotal / d_kgstotal) * 100;
                    fat_total = Math.Round(fat_total, 2);
                    new1["FAT"] = fat_total;
                    new1["KG FAT"] = d_kgfattotal;
                    double snf_total = 0;
                    snf_total = (d_kgsnftotal / d_kgstotal) * 100;
                    snf_total = Math.Round(snf_total, 2);
                    double TS_total = 0;
                    TS_total = snf_total + fat_total;
                    new1["SNF"] = snf_total;
                    new1["KG SNF"] = d_kgsnftotal;
                    new1["TS TOTAL"] = TS_total;
                    new1["M VALUE"] = d_mvaluetotal;
                    new1["OH"] = d_ohtotal;
                    new1["SNF9"] = d_snf9total;
                    new1["MILK VALUE"] = d_milkvaluetotal;
                    double B = TS_total * d_Ltrstotal;
                    double tsrate_total = 0;
                    tsrate_total = (d_mvaluetotal / B) * 100;
                    tsrate_total = Math.Round(tsrate_total, 2);
                    new1["TS RATE"] = tsrate_total;
                    double per_ltr = 0;
                    per_ltr = d_milkvaluetotal / d_Ltrstotal;
                    per_ltr = Math.Round(per_ltr, 2);
                    new1["Per Ltr"] = per_ltr;
                    Report.Rows.Add(new1);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                    hidepanel.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btnExportExcel_Click(object sender, EventArgs e)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
         "attachment;filename=GridViewExport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        grdReports.DataSource = Report1;
        PrepareForExport(grdReports);
        PrepareForExport(grdcow);
        Table tb = new Table();
        TableRow tr1 = new TableRow();
        TableCell cell1 = new TableCell();
        cell1.Controls.Add(grdReports);
        tr1.Cells.Add(cell1);
        TableCell cell3 = new TableCell();
        cell3.Controls.Add(grdcow);
        TableCell cell2 = new TableCell();
        cell2.Text = "&nbsp;";

        TableRow tr2 = new TableRow();
        tr2.Cells.Add(cell2);
        TableRow tr3 = new TableRow();
        tr3.Cells.Add(cell3);
        tb.Rows.Add(tr1);
        tb.Rows.Add(tr2);
        tb.Rows.Add(tr3);

        tb.RenderControl(hw);
        //style to format numbers to string
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }
    protected void PrepareForExport(GridView Gridview)
    {
        Gridview.DataBind();
        //Change the Header Row back to white color
        Gridview.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //Apply style to Individual Cells
        for (int k = 0; k < Gridview.HeaderRow.Cells.Count; k++)
        {
            Gridview.HeaderRow.Cells[k].Style.Add("background-color", "green");
        }
        for (int i = 0; i < Gridview.Rows.Count; i++)
        {

            GridViewRow row = Gridview.Rows[i];
            //Change Color back to white
            row.BackColor = System.Drawing.Color.White;
            //Apply text style to each Row
            row.Attributes.Add("class", "textmode");
            //Apply style to Individual Cells of Alternating Row
            if (i % 2 != 0)
            {
                for (int j = 0; j < Gridview.Rows[i].Cells.Count; j++)
                {
                    row.Cells[j].Style.Add("background-color", "#C2D69B");
                }
            }
        }
    }
}