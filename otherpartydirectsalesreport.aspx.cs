using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class otherpartydirectsalesreport : System.Web.UI.Page
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

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesDBManager SalesDB = new SalesDBManager();
        if (ddlBranch.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
        }
        if (ddlBranch.SelectedValue == "Branch Wise")
        {
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE    (branchmapping.superbranch = @branchid)");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "vendorname";
            ddlbranches.DataValueField = "sno";
            ddlbranches.DataBind();
        }
        if (ddlBranch.SelectedValue == "Vehicle Wise")
        {
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT sno,vehicleno,capacity,noofqty FROM vehicle_master WHERE branchid=@branchid");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "vehicleno";
            ddlbranches.DataValueField = "vehicleno";
            ddlbranches.DataBind();
        }
    }

    DataTable Report = new DataTable();
    DataTable dtcow = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        directsale();
    }

    DataTable dtdirectReport = new DataTable();
    DataTable dtdirectcow = new DataTable();

    private void directsale()
    {
        try
        {
            Session["filename"] = "Tanker Other party Direct Sales";
            Session["title"] = "Tanker Other party Direct Sales Details";
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
            string salestype = ddlsaletype.SelectedItem.Text;
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
            dtdirectReport.Columns.Add("CC Name");
            dtdirectReport.Columns.Add("TANKER NO");
            if (ddlBranch.SelectedValue == "All")
            {
                cmd = new SqlCommand("SELECT dp.sno, dp.transid, dp.dcno, dp.transtype, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon, dp.snf, dp.fat, dp.clr, dp.hs, dp.alcohol, dp.remarks, dp.chemist, dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1, dp.phosps1, dp.mbrt, dp.acidity, dp.ot, dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno,  dpl.rate_on, dpl.calc_on, dpl.cost, dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport, dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on, dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname as fromcc FROM directsales_purchase AS dp INNER JOIN directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN vendors AS v ON dp.sectionid = v.sno WHERE (dp.entrydate BETWEEN @d1 and @d2)  AND (dp.milktype='Buffalo') AND (dp.entrytype=@saletype) ORDER BY dp.entrydate ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
                cmd.Parameters.Add("@saletype", salestype);
            }
            else
            {
                cmd = new SqlCommand("SELECT dp.sno, dp.transid, dp.dcno, dp.transtype, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon, dp.snf, dp.fat, dp.clr, dp.hs, dp.alcohol, dp.remarks, dp.chemist, dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1, dp.phosps1, dp.mbrt, dp.acidity, dp.ot, dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno,  dpl.rate_on, dpl.calc_on, dpl.cost, dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport, dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on, dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname as fromcc FROM directsales_purchase AS dp INNER JOIN directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN vendors AS v ON dp.sectionid = v.sno WHERE (dp.entrydate BETWEEN @d1 and @d2) AND (dp.sectionid = @ccid) AND (dp.milktype='Buffalo') AND (dp.entrytype=@saletype) ORDER BY dp.entrydate ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
                cmd.Parameters.Add("@saletype", salestype);
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
                int i = 1;
                foreach (DataRow dr in dtDispatch.Rows)
                {
                    DataRow newrow = dtdirectReport.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    newrow["CC Name"] = dr["fromcc"].ToString();
                    newrow["TANKER NO"] = dr["vehicleno"].ToString();
                    newrow["KGS"] = dr["qty_kgs"].ToString();
                    newrow["Transaction No"] = dr["dcno"].ToString();
                    newrow["DC No"] = dr["partydcno"].ToString();
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

                    double cost = 0;
                    double.TryParse(dr["cost"].ToString(), out cost);
                    newrow["KG FAT RATE"] = cost;
                    kgfatratetotal += cost;
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
                newvartical2["KG FAT RATE"] = kgfatratetotal;
                dtdirectReport.Rows.Add(newvartical2);
                DataRow New1 = dtdirectReport.NewRow();
                New1["DATE"] = "Cow";
                dtdirectReport.Rows.Add(New1);
                Session["xportdata"] = dtdirectReport;
                grdReports.DataSource = dtdirectReport;
                grdReports.DataBind();
                hidepanel.Visible = true;
                pnlcow.Visible = false;
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
            dtdirectcow.Columns.Add("FAT+/-");
            dtdirectcow.Columns.Add("SNF9");
            dtdirectcow.Columns.Add("MILK VALUE");
            dtdirectcow.Columns.Add("Transaction No");
            dtdirectcow.Columns.Add("DC No");
            dtdirectcow.Columns.Add("CC Name");
            dtdirectcow.Columns.Add("TANKER NO");
            if (ddlBranch.SelectedValue == "All")
            {
                cmd = new SqlCommand("SELECT dp.sno, dp.transid, dp.dcno, dp.transtype, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon, dp.snf, dp.fat, dp.clr, dp.hs, dp.alcohol, dp.remarks, dp.chemist, dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1, dp.phosps1, dp.mbrt, dp.acidity, dp.ot, dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno,  dpl.rate_on, dpl.calc_on, dpl.cost, dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport, dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on, dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname FROM directsales_purchase AS dp INNER JOIN directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN vendors AS v ON dp.sectionid = v.sno WHERE (dp.entrydate BETWEEN @d1 and @d2) AND (dp.milktype='Cow') AND (dp.entrytype=@saletypee) ORDER BY dp.entrydate ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
                cmd.Parameters.Add("@saletypee", salestype);
            }
            else
            {
                cmd = new SqlCommand("SELECT dp.sno, dp.transid, dp.dcno, dp.transtype, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon, dp.snf, dp.fat, dp.clr, dp.hs, dp.alcohol, dp.remarks, dp.chemist, dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1, dp.phosps1, dp.mbrt, dp.acidity, dp.ot, dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno,  dpl.rate_on, dpl.calc_on, dpl.cost, dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport, dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on, dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname FROM directsales_purchase AS dp INNER JOIN directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN vendors AS v ON dp.sectionid = v.sno WHERE (dp.entrydate BETWEEN @d1 and @d2) AND (dp.sectionid = @ccid) AND (dp.milktype='Cow') AND (dp.entrytype=@saletypee) ORDER BY dp.entrydate ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
                cmd.Parameters.Add("@saletypee", salestype);
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
                int i = 1;
                foreach (DataRow dr in dtcowDispatch.Rows)
                {
                    DataRow newrow = dtdirectcow.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    newrow["CC Name"] = dr["vendorname"].ToString();
                    newrow["TANKER NO"] = dr["vehicleno"].ToString();
                    newrow["KGS"] = dr["qty_kgs"].ToString();
                    newrow["Transaction No"] = dr["dcno"].ToString();
                    newrow["DC No"] = dr["partydcno"].ToString();
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
                    //MValue = tstotal * qty_ltr * cost;
                    //MValue = MValue / 100;
                    //MValue = Math.Round(MValue, 2);
                    //newrow["M VALUE"] = MValue;
                    mvaluetotal += MValue;
                    double OHandMvalue = 0;
                    OHandMvalue = MValue;
                    OHandMvalue = Math.Round(OHandMvalue, 2);
                    newrow["M VALUE"] = OHandMvalue;

                    milkvaluetotal += OHandMvalue;
                    //newrow["DC NO"] = dr["dcno"].ToString();
                    double OMILKVALUE = 0;
                    OMILKVALUE = MValue + OHcost + DiffSNFCost + DiffFATCost;
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
                double kgfatratetotal = 0;
                kgfatratetotal = mvaluetotal / kgfattotal;
                kgfatratetotal = Math.Round(kgfatratetotal, 2);
                newvartical2["TS RATE"] = kgfatratetotal;
                dtdirectcow.Rows.Add(newvartical2);
                grdcow.DataSource = dtdirectcow;
                grdcow.DataBind();
                Session["xportdata"] = dtdirectcow;
                pnlcow.Visible = true;
                hidepanel.Visible = true;
            }
            else
            {
                pnlcow.Visible = false;
            }

            // Session["xportdata"] = Report;
            //hidepanel.Visible = true;
        }
        catch
        {
        }
    }
}