using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class allsalesvochers : System.Web.UI.Page
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
            if (ddlbranchType.SelectedValue == "All")
            {
                cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors where branchid=@vbranchid");
                cmd.Parameters.Add("@vbranchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranches.DataSource = dttrips;
                ddlbranches.DataTextField = "vendorname";
                ddlbranches.DataValueField = "sno";
                ddlbranches.DataBind();
            }
            if (ddlbranchType.SelectedValue == "Inter Branches")
            {
                cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors where branchtype='Inter Branch' AND branchid=@Ibranchid ");
                cmd.Parameters.Add("@Ibranchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranches.DataSource = dttrips;
                ddlbranches.DataTextField = "vendorname";
                ddlbranches.DataValueField = "sno";
                ddlbranches.DataBind();
            }
            if (ddlbranchType.SelectedValue == "Other Branches")
            {
                cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors  where branchtype='Other Branch' AND branchid=@Obranchid");
                cmd.Parameters.Add("@Obranchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranches.DataSource = dttrips;
                ddlbranches.DataTextField = "vendorname";
                ddlbranches.DataValueField = "sno";
                ddlbranches.DataBind();
            }
        }
        if (ddlBranch.SelectedValue == "Vehicle Wise")
        {
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT sno,vehicleno,capacity,noofqty FROM vehicle_master where branchid=@vmbranchid");
            cmd.Parameters.Add("@vmbranchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "vehicleno";
            ddlbranches.DataValueField = "vehicleno";
            ddlbranches.DataBind();
        }
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
            Session["filename"] = "Sales Voucher";
            Session["title"] = "Sales Voucher Details";
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
                Report.Columns.Add("Wh CODE");
                Report.Columns.Add("CUSTOMER CODE");
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("INVOICE NO");
                Report.Columns.Add("INVOICE DATE");
                Report.Columns.Add("LEDGER CODE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM CODE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("OH CODE");
                Report.Columns.Add("LEDGER TYPE1");
                Report.Columns.Add("LEDGER AMOUNT1");
                //Report.Columns.Add("ROUNDING OFF");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("Narration");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost,branch_info.whcode, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, vendors.sapcustomercode, vendors.salesledgercode, vendors.salesohcode, vendors.customername, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND (milktransactions.vehicleno=@vehicleno) AND (milktransactions.branchid=@ebranchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@ebranchid", BranchID);
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    if (ddltankertype.SelectedValue == "in")
                    {
                        cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, branch_info.whcode,milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, vendors.sapcustomercode, vendors.salesledgercode, vendors.salesohcode, vendors.customername, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.sectionid=@sectionid) AND (milktransactions.branchid=@fbranchid) ORDER BY doe");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "in");
                        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                        cmd.Parameters.Add("@fbranchid", BranchID);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on,branch_info.whcode, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, vendors.sapcustomercode, vendors.salesledgercode, vendors.salesohcode, vendors.customername, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno LEFT OUTER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.sectionid=@sectionid) AND (milktransactions.branchid=@fbranchid) ORDER BY doe");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "Out");
                        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                        cmd.Parameters.Add("@fbranchid", BranchID);
                    }

                }
                else
                {
                    cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf,milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on,branch_info.whcode, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, vendors.sapcustomercode, vendors.salesledgercode, vendors.salesohcode, vendors.customername, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.branchid=@gbranchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@gbranchid", BranchID);

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
                        // newrow["SNF9"] = DiffSNFCost;
                        snf9total += DiffSNFCost;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        //newrow["MILK VALUE"] = "SVDS.P.LTD ARANI(Pbk)";
                        string cn = "SRI VYSHNAVI DAIRY PVT LTD";
                        string vendername = dr["vendorname"].ToString();
                        string ccname = dr["ccname"].ToString();
                        string tallyoh = dr["salestallyoh"].ToString();
                        string ledgertype = dr["salesledgertype"].ToString();
                        string ledgerccname = "";
                        string celltype = dr["cellno"].ToString();
                        string branchcode = dr["branchcode"].ToString();
                        string invoiceno = dr["partydcno"].ToString();
                        if (ddlbranchType.SelectedItem.Text == "Other Branches")
                        {
                            ledgerccname = vendername;
                            newrow["ITEM NAME"] = "WHOLE MILK Others";
                            ledgertype = dr["ledgertype"].ToString();
                        }
                        else
                        {
                            ledgerccname = dr["customername"].ToString();
                            newrow["ITEM NAME"] = "WHOLE MILK";
                        }
                        milkvaluetotal += OHandMvalue;
                        if (ddlReportType.SelectedValue == "Buffalo")
                        {

                        }
                        newrow["LEDGER TYPE"] = ledgertype;
                        newrow["INVOICE NO"] = invoiceno + "-" + celltype + "-" + branchcode;
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        string customercode = dr["sapcustomercode"].ToString();
                        string ledgercode = dr["salesledgercode"].ToString();
                        string ohcode = dr["salesohcode"].ToString();
                        newrow["CUSTOMER CODE"] = customercode;
                        newrow["LEDGER CODE"] = ledgercode;
                        newrow["OH CODE"] = ohcode;
                        //newrow["LEDGER TYPE"] = ledgertype;
                        newrow["LEDGER TYPE1"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 2);
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
                        newrow["Wh Code"]=dr["whcode"].ToString();
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
                Report.Columns.Add("Wh CODE");
                Report.Columns.Add("CUSTOMER CODE");
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("INVOICE NO");
                Report.Columns.Add("INVOICE DATE");
                Report.Columns.Add("LEDGER CODE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM CODE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("OH CODE");
                Report.Columns.Add("LEDGER TYPE1");
                Report.Columns.Add("LEDGER AMOUNT1");
                // Report.Columns.Add("ROUNDING OFF");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("Narration");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,branch_info.whcode, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype,  vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, vendors.sapcustomercode, vendors.salesledgercode, vendors.salesohcode, vendors.customername,  branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND ( milktransactions.vehicleno=@vehicleno) AND (milktransactions.branchid=@hbranchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@hbranchid", BranchID);
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {

                    if (ddltankertype.SelectedValue == "in")
                    {
                        cmd = new SqlCommand("  SELECT milktransactions.dcno,branch_info.whcode, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, vendors.salesledgertype, vendors.ccname, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salestallyoh, vendors.sapcustomercode, vendors.salesledgercode, vendors.salesohcode, vendors.customername, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.sectionid=@sectionid) AND (milktransactions.branchid=@bwbranchid) ORDER BY doe");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "in");
                        cmd.Parameters.Add("@bwbranchid", BranchID);
                        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT milktransactions.dcno, branch_info.whcode,milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, vendors.vendorname, vendors.salesledgertype, vendors.ccname, vendors.tallyoh, vendors.ledgertype, vendors.salestallyoh, vendors.sapcustomercode, vendors.salesledgercode, vendors.salesohcode, vendors.customername, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.sectionid=@sectionid)  ORDER BY doe");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "Out");
                        cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    }
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,branch_info.whcode, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_fatpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, vendors.sapcustomercode, vendors.salesledgercode, vendors.salesohcode, vendors.customername, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.branchid=@kbranchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@kbranchid", BranchID);
                    cmd.Parameters.Add("@transtype", "in");
                }
                //cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno,milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost,milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno WHERE (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)");

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
                        else
                        {
                            dcnaration = "Being sales Of Milk from " + ddlbranches.SelectedItem.Text + "," + "Dcno" + dr["dcno"] + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
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
                        OHandMvalue = MValue + OHcost + DiffFATCost + DiffSNFCost;
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
                        string cn = "SVDS.P.LTD";
                        string plant = "Pbk";

                       

                        string vendername = dr["vendorname"].ToString();
                        string ccname = dr["ccname"].ToString();
                        string tallyoh = dr["salestallyoh"].ToString();
                        string ledgertype = dr["salesledgertype"].ToString();
                        string celltype = dr["cellno"].ToString();
                        string branchcode = "";
                        string invoiceno = "";
                        if (ddltankertype.SelectedValue == "in")
                        {
                            branchcode = dr["branchcode"].ToString();
                            invoiceno = dr["partydcno"].ToString();
                        }
                        else
                        {
                            invoiceno = dr["dcno"].ToString();
                        }
                        string ledgerccname = "SVDS.P.LTD PUNABAKA PLANT";
                        if (ddlbranchType.SelectedItem.Text == "Other Branches")
                        {
                            ledgerccname = vendername;
                            newrow["ITEM NAME"] = "COW MILK Others";
                            ledgertype = dr["ledgertype"].ToString();
                            newrow["LEDGER TYPE"] = ledgertype;
                        }
                        else
                        {
                            ledgerccname = dr["customername"].ToString();
                            newrow["ITEM NAME"] = "COW MILK";
                        }
                        
                        newrow["LEDGER TYPE"] = ledgertype;
                        newrow["INVOICE NO"] = invoiceno + "-" + celltype + "-" + branchcode;
                        string customercode = dr["sapcustomercode"].ToString();
                        string ledgercode = dr["salesledgercode"].ToString();
                        string ohcode = dr["salesohcode"].ToString();
                        newrow["CUSTOMER CODE"] = customercode;
                        newrow["LEDGER CODE"] = ledgercode;
                        newrow["OH CODE"] = ohcode;
                        newrow["Wh Code"] = dr["whcode"].ToString();
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        newrow["LEDGER TYPE1"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 2);
                        newrow["RATE PER LTR"] = ltrcost;
                        double ledgeramount = 0;
                        ledgeramount = OHandMvalue - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);

                        OHcost = Math.Round(OHcost, 0);
                        newrow["LEDGER AMOUNT1"] = OHcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["partydcno"].ToString();
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