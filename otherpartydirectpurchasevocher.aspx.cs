using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;


public partial class otherpartydirectpurchasevocher : System.Web.UI.Page
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
                    fillbranches();
                    // lblAddress.Text = Session["Address"].ToString();
                    // lblTitle.Text = Session["TitleName"].ToString();
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
    public void fillbranches()
    {
        SalesDBManager SalesDB = new SalesDBManager();
        cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (branchmapping.superbranch = @branchid) and vendors.branchtype='Other Branch'");
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlbranches.DataSource = dttrips;
        ddlbranches.DataTextField = "vendorname";
        ddlbranches.DataValueField = "sno";
        ddlbranches.DataBind();

    }
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            pnlcow.Visible = false;
            grdcow.DataSource = null;
            grdcow.DataBind();
            grdReports.DataSource = null;
            grdReports.DataBind();
            Session["filename"] = "Other Party Purchase Voucher";
            Session["title"] = "Other Party Purchase Voucher Details";
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
            //lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            // lbltodate.Text = todate.ToString("dd/MMM/yyyy");

            if (ddlReportType.SelectedValue == "Buffalo")
            {
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("INVOICE NO");
                Report.Columns.Add("INVOICE DATE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("LEDGER TYPE1");
                Report.Columns.Add("LEDGER AMOUNT1");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("Narration");
                cmd = new SqlCommand("SELECT directsales_purchase.dcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, directsales_purchase.doe, directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname, vendors.ledgertype, vendors.tallyoh, vendors.ccname FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.entrydate BETWEEN @d1 AND @d2) AND (directsales_purchase.entrytype = @transtype) AND (directsales_purchaselogs.milktype='Buffalo') AND (directsales_purchase.sectionid=@sectionid) ORDER BY doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "purchase");
                cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                cmd.Parameters.Add("@fbranchid", BranchID);
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
                    string olddcno = "";
                    string oldnarration = "";
                    int i = 1;
                    string fckg = "0";
                    string fcltrs = "0";
                    string bckg = "0";
                    string bcltrs = "0";
                    string mckg = "0";
                    string mcltrs = "0";
                    string fcfat = "0";
                    string bcfat = "0";
                    string mcfat = "0";
                    string fcsnf = "0";
                    string bcsnf = "0";
                    string mcsnf = "0";
                    string dcnaration = "";
                    foreach (DataRow dr in dtDispatch.Rows)
                    {
                        string newdcno1 = dr["partydcno"].ToString();
                        DataTable dtin = new DataTable();
                        DataRow[] drr = dtDispatch.Select("partydcno='" + newdcno1 + "'");
                        if (drr.Length > 0)
                        {
                            dtin = drr.CopyToDataTable();
                        }
                        if (dtin.Rows.Count > 0)
                        {
                            int d = 0;
                            int count = dtin.Rows.Count;
                            for (d = 0; d <= count; d++)
                            {
                                if (d == 0)
                                {
                                    fckg = dtin.Rows[0]["qty_kgs"].ToString();
                                    fcfat = dtin.Rows[0]["fat"].ToString();
                                    fcsnf = dtin.Rows[0]["snf"].ToString();
                                    fcltrs = dtin.Rows[0]["qty_ltr"].ToString();
                                }
                                if (d == 2)
                                {
                                    mckg = dtin.Rows[1]["qty_kgs"].ToString();
                                    mcfat = dtin.Rows[1]["fat"].ToString();
                                    mcsnf = dtin.Rows[1]["snf"].ToString();
                                    mcltrs = dtin.Rows[1]["qty_ltr"].ToString();
                                }
                                if (d == 3)
                                {
                                    bckg = dtin.Rows[2]["qty_kgs"].ToString();
                                    bcfat = dtin.Rows[2]["fat"].ToString();
                                    bcsnf = dtin.Rows[2]["snf"].ToString();
                                    bcltrs = dtin.Rows[2]["qty_ltr"].ToString();
                                }
                            }
                            dcnaration = "Being sales Of Milk from " + ddlbranches.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd-MMM-yyyy");
                        newrow["INVOICE DATE"] = date;
                        double qty_kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                        double qty_ltr = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                        newrow["QTY KG'S/LTRS"] = dr["qty_kgs"].ToString();
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        // newrow["CLR"] = dr["clr"].ToString();
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
                        // newrow["KG FAT RATE"] = cost;
                        KGFAT = Math.Round(KGFAT, 2);

                        // newrow["KG FAT"] = KGFAT;
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
                        // newrow["KG SNF"] = KGSNF;
                        kgsnftotal += KGSNF;
                        double MValue = 0;
                        MValue = KGFAT * cost;
                        //MValue = MValue / 100;
                        MValue = Math.Round(MValue, 2);
                        //newrow["M VALUE"] = MValue;
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
                        OHcost = Math.Round(OHcost, 0);
                        newrow["LEDGER AMOUNT1"] = OHcost;
                        ohtotal += OHcost;
                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                        snf9total += DiffSNFCost;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        string cn = "SRI VYSHNAVI DAIRY PVT LTD";
                        string ccname = dr["ccname"].ToString();
                        string vendername = dr["vendorname"].ToString();
                        string ledgrtype = dr["ledgertype"].ToString();
                        string tallyoh = dr["tallyoh"].ToString();
                        string ledgerccname = "Sri Vyshnavi Dairy Specialities Pvt Ltd.,";
                        milkvaluetotal += OHandMvalue;
                        if (ddlReportType.SelectedValue == "Buffalo")
                        {
                            newrow["ITEM NAME"] = "WHOLE MILK";
                        }
                        newrow["INVOICE NO"] = "1819/" + dr["partydcno"].ToString();
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        //string ccname = dr["vendorname"].ToString();
                        //newrow["ROUNDING OFF"] = "0";
                        newrow["LEDGER TYPE1"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 5);
                        newrow["RATE PER LTR"] = ltrcost;
                        double ledgeramount = 0;
                        ledgeramount = ltrcost * qty_ltr;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        //newrow["LEDGER AMOUNT"] = ltrcost * qty_ltr;
                        double roundvalue = Math.Round(OHandMvalue, 0);
                        newrow["Net VALUE"] = Math.Round(OHandMvalue, 0);
                        newrow["LEDGER AMOUNT"] = roundvalue - OHcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["partydcno"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " , " + " TS Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        Report.Rows.Add(newrow);
                        fckg = "0";
                        fcltrs = "0";
                        bckg = "0";
                        bcltrs = "0";
                        mckg = "0";
                        mcltrs = "0";
                        fcfat = "0";
                        bcfat = "0";
                        mcfat = "0";
                        fcsnf = "0";
                        bcsnf = "0";
                        mcsnf = "0";
                        dcnaration = "";
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
            else if (ddlReportType.SelectedValue == "Cow" || ddlReportType.SelectedValue == "Skim" || ddlReportType.SelectedValue == "Condensed")
            {
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("INVOICE NO");
                Report.Columns.Add("INVOICE DATE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("LEDGER TYPE1");
                Report.Columns.Add("LEDGER AMOUNT1");
                // Report.Columns.Add("ROUNDING OFF");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("Narration");
                cmd = new SqlCommand("  SELECT directsales_purchase.dcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, directsales_purchase.doe, directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost, directsales_purchaselogs.m_std_fat, directsales_purchaselogs.p_std_fat, directsales_purchaselogs.m_fatpluscost, directsales_purchaselogs.p_fatpluscost, directsales_purchaselogs.fatplus_on, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname, vendors.ledgertype, vendors.tallyoh, vendors.ccname  FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.entrydate BETWEEN @d1 AND @d2) AND (directsales_purchase.entrytype = @transtype) AND (directsales_purchaselogs.milktype='Cow') AND (directsales_purchase.sectionid=@sectionid) ORDER BY doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "purchase");
                cmd.Parameters.Add("@bwbranchid", BranchID);
                cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                DataTable dtDispatch = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtDispatch.Rows.Count > 0)
                {
                    int i = 1;
                    double kgfattotal = 0;
                    double kgsnftotal = 0;
                    double kgstotal = 0;
                    double Ltrstotal = 0;
                    double TStotal = 0;
                    double mvaluetotal = 0;
                    double ohtotal = 0;
                    double snf9total = 0;
                    double milkvaluetotal = 0;
                    string olddcno = "";
                    string oldnarration = "";
                    string fckg = "0";
                    string fcltrs = "0";
                    string bckg = "0";
                    string bcltrs = "0";
                    string mckg = "0";
                    string mcltrs = "0";
                    string fcfat = "0";
                    string bcfat = "0";
                    string mcfat = "0";
                    string fcsnf = "0";
                    string bcsnf = "0";
                    string mcsnf = "0";
                    string dcnaration = "";
                    foreach (DataRow dr in dtDispatch.Rows)
                    {
                        string newdcno1 = dr["dcno"].ToString();
                        DataTable dtin = new DataTable();
                        DataRow[] drr = dtDispatch.Select("dcno='" + newdcno1 + "'");
                        if (drr.Length > 0)
                        {
                            dtin = drr.CopyToDataTable();
                        }
                        if (dtin.Rows.Count > 0)
                        {
                            int d = 0;
                            int count = dtin.Rows.Count;
                            for (d = 0; d <= count; d++)
                            {
                                if (d == 0)
                                {
                                    fckg = dtin.Rows[0]["qty_kgs"].ToString();
                                    fcfat = dtin.Rows[0]["fat"].ToString();
                                    fcsnf = dtin.Rows[0]["snf"].ToString();
                                    fcltrs = dtin.Rows[0]["qty_ltr"].ToString();
                                }
                                if (d == 2)
                                {
                                    mckg = dtin.Rows[1]["qty_kgs"].ToString();
                                    mcfat = dtin.Rows[1]["fat"].ToString();
                                    mcsnf = dtin.Rows[1]["snf"].ToString();
                                    mcltrs = dtin.Rows[1]["qty_ltr"].ToString();
                                }
                                if (d == 3)
                                {
                                    bckg = dtin.Rows[2]["qty_kgs"].ToString();
                                    bcfat = dtin.Rows[2]["fat"].ToString();
                                    bcsnf = dtin.Rows[2]["snf"].ToString();
                                    bcltrs = dtin.Rows[2]["qty_ltr"].ToString();
                                }
                            }
                            dcnaration = "Being sales Of Milk from " + ddlbranches.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd-MMM-yyyy");
                        newrow["INVOICE DATE"] = date;
                        double qty_kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                        // newrow["KGS"] = dr["qty_kgs"].ToString();

                        double qty_ltr = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                        newrow["QTY KG'S/LTRS"] = dr["qty_ltr"].ToString();
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        // newrow["FAT"] = FAT;
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        // newrow["SNF"] = SNF;
                        // newrow["CLR"] = dr["clr"].ToString();
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
                        //newrow["TS TOTAL"] = tstotal;
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
                        //newrow["TS RATE"] = cost;

                        KGFAT = Math.Round(KGFAT, 2);
                        // newrow["KG FAT"] = KGFAT;
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
                        // newrow["KG SNF"] = KGSNF;
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
                        // newrow["M VALUE"] = MValue;
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

                        double OHandMvalue = 0;
                        OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                        // newrow["OH"] = OHcost;
                        ohtotal += OHcost;
                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                        // newrow["SNF9"] = DiffSNFCost;
                        snf9total += DiffSNFCost;
                        double roundvalue = Math.Round(OHandMvalue, 0);
                        newrow["NET VALUE"] = roundvalue;
                        double diffvalue = roundvalue - OHandMvalue;
                        // newrow["ROUNDING OFF"] = Math.Round(diffvalue, 2);
                        milkvaluetotal += OHandMvalue;

                        if (ddlReportType.SelectedValue == "Cow")
                        {
                            newrow["ITEM NAME"] = "COW MILK";
                        }
                        newrow["INVOICE NO"] = "1819/" + dr["dcno"].ToString();
                        string ledgerccname = "SVDS.P.LTD PUNABAKA PLANT";
                        string cn = "SVDS.P.LTD";
                        string plant = "Pbk";
                        string ccname = dr["ccname"].ToString();
                        string vendername = dr["vendorname"].ToString();
                        string ledgrtype = dr["ledgertype"].ToString();
                        string tallyoh = dr["tallyoh"].ToString();
                        //string ledgerccname = "Sri Vyshnavi Dairy Specialities Pvt Ltd.,";
                        newrow["LEDGER TYPE"] = ledgrtype;
                        newrow["CUSTOMER NAME"] = vendername;
                        newrow["LEDGER TYPE1"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 5);
                        newrow["RATE PER LTR"] = ltrcost;
                        double ledgeramount = 0;
                        ledgeramount = OHandMvalue - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        OHcost = Math.Round(OHcost, 0);
                        newrow["LEDGER AMOUNT1"] = OHcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["dcno"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " ," + " TS Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        newrow["LEDGER AMOUNT"] = roundvalue - OHcost;
                        Report.Rows.Add(newrow);
                        fckg = "0";
                        fcltrs = "0";
                        bckg = "0";
                        bcltrs = "0";
                        mckg = "0";
                        mcltrs = "0";
                        fcfat = "0";
                        bcfat = "0";
                        mcfat = "0";
                        fcsnf = "0";
                        bcsnf = "0";
                        mcsnf = "0";
                        dcnaration = "";
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
}