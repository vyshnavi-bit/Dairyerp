using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
public partial class CollectionReport : System.Web.UI.Page
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
                    bindbranches();
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
    private void bindbranches()
    {
        cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE (branchmapping.superbranch = @branchid)");
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlbranches.DataSource = dttrips;
        ddlbranches.DataTextField = "vendorname";
        ddlbranches.DataValueField = "sno";
        ddlbranches.DataBind();
    }
    double totalmilk;
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        report();
    }
    DataTable collection = new DataTable();
    DataTable Report = new DataTable();
    private void report()
    {
        try
        {
            lblmsg.Text = "";
            string milkopeningbal = string.Empty;
            string milkclosingbal = string.Empty;
            SalesDBManager SalesDB = new SalesDBManager();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string idcno = string.Empty;
            string inworddate = string.Empty;
            double totalpaidamount = 0;
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
            int vendorid = Convert.ToInt32(ddlbranches.SelectedItem.Value);
            Report.Columns.Add("Sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("Dc No");
            Report.Columns.Add("Dc Amount");
            Report.Columns.Add("Opp Bal");
            Report.Columns.Add("Total Amount");
            if (ddlcollectiontype.SelectedValue == "Collection")
            {
                Report.Columns.Add("Received Amount");
            }
            else
            {
                Report.Columns.Add("Paid Amount");
            }
            Report.Columns.Add("Balance");
            hidepanel.Visible = true;
            DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
            cmd = new SqlCommand("SELECT vendors.vendorname, collections.opp_bal, collections.amount, collections.transtype,CONVERT(VARCHAR(20),collections.doe, 101) AS doe,  collections.remarks, collections.clo_bal FROM   collections INNER JOIN vendors ON collections.vendorid = vendors.sno  WHERE (collections.vendorid=@vid) AND (collections.doe between @d1 and @d2) AND (collections.transtype='Payment') ");
            cmd.Parameters.Add("@vid", vendorid);
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", ServerDateCurrentdate);
            DataTable dtcollection = vdm.SelectQuery(cmd).Tables[0];
            double debitprice = 0;
            foreach (DataRow dr in dtcollection.Rows)
            {
                double amountDebited = 0;
                double.TryParse(dr["amount"].ToString(), out amountDebited);
                debitprice += amountDebited;
            }
            cmd = new SqlCommand("SELECT vendor_accounts.sno, vendor_accounts.amount, vendor_subtable.milktype FROM vendor_accounts INNER JOIN vendors ON vendor_accounts.vendorid = vendors.sno INNER JOIN  vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno WHERE (vendor_accounts.vendorid = @VendorID)");
            cmd.Parameters.Add("@VendorID", vendorid);
            DataTable dtvendor_presentopp = vdm.SelectQuery(cmd).Tables[0];
            double vendorpresentopp = 0;
            double.TryParse(dtvendor_presentopp.Rows[0]["Amount"].ToString(), out vendorpresentopp);
            string milktype = dtvendor_presentopp.Rows[0]["milktype"].ToString();

            if (ddlcollectiontype.SelectedValue == "Collection")
            {
                cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno,CONVERT(VARCHAR(20),milktransactions.doe, 101) AS doe , milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND (milktransactions.sectionid=@sectionid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", ServerDateCurrentdate);
                Session["filename"] = "Collection";
                Session["title"] = "Collection Details";
                cmd.Parameters.Add("@transtype", "Out");
            }
            if (ddlcollectiontype.SelectedValue == "Payment")
            {
                cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo,milktransaction_logs.m_std_fat,milktransaction_logs.fatplus_on,milktransaction_logs.p_std_fat,milktransaction_logs.m_fatpluscost,milktransaction_logs.p_fatpluscost, milktransaction_logs.p_std_snf, milktransactions.vehicleno,CONVERT(VARCHAR(20),milktransactions.doe, 101) AS doe , milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND (milktransactions.sectionid=@sectionid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", ServerDateCurrentdate);
                Session["filename"] = "Payment";
                Session["title"] = "Payment Details";
                cmd.Parameters.Add("@transtype", "In");
            }
            cmd.Parameters.Add("@sectionid", vendorid);
            DataTable dtcow = new DataTable();
            DataTable dttankersales = SalesDB.SelectQuery(cmd).Tables[0];
            if (ddlcollectiontype.SelectedValue == "Collection")
            {
                cmd = new SqlCommand("  SELECT directsales_purchase.dcno, directsales_purchase.partydcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, CONVERT(VARCHAR(20),directsales_purchase.doe, 101) AS doe, directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.doe BETWEEN @d1 AND @d2) AND (directsales_purchase.transtype = @transtype) AND (directsales_purchaselogs.milktype='Cow') AND (directsales_purchase.sectionid=@sectionid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "Out");
                cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                dtcow = SalesDB.SelectQuery(cmd).Tables[0];
            }
            if (ddlcollectiontype.SelectedValue == "Payment")
            {
                cmd = new SqlCommand("  SELECT directsales_purchase.dcno, directsales_purchase.partydcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, CONVERT(VARCHAR(20),directsales_purchase.doe, 101) AS doe, directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.doe BETWEEN @d1 AND @d2) AND (directsales_purchase.transtype = @transtype) AND (directsales_purchaselogs.milktype='Cow') AND (directsales_purchase.sectionid=@sectionid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "in");
                cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                dtcow = SalesDB.SelectQuery(cmd).Tables[0];
            }
            //cmd = new SqlCommand("  SELECT directsales_purchase.dcno,directsales_purchase.partydcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno,CONVERT(VARCHAR(20),directsales_purchase.doe, 101) AS doe , directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.doe BETWEEN @d1 AND @d2) AND (directsales_purchase.transtype = @transtype) AND (directsales_purchaselogs.milktype='Buffalo') AND (directsales_purchase.sectionid=@sectionid)");
            //cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            //cmd.Parameters.Add("@d2", GetHighDate(todate));
            //cmd.Parameters.Add("@transtype", "in");
            //cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
            //DataTable dtBufello = SalesDB.SelectQuery(cmd).Tables[0];
            DataTable dtAll = dttankersales.Copy();
            dtAll.Merge(dtcow);
            
            int i = 1;
            string closingbal = "";
            TimeSpan dateSpan = todate.Subtract(fromdate);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 1;
            double totdebitedamount = 0;
            double totsalesamount = 0;
            double oppcarry = 0;
            foreach (DataRow dr in dtAll.Rows)
            {
                string milktypes = dr["milktype"].ToString();
                {
                    if (milktypes == "Buffalo")
                    {
                        double qty_ltr = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        string Rateon = dr["rate_on"].ToString();
                        double weight = 0;
                        double KGFAT = 0;
                        double KGSNF = 0;
                        double ltrs = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                        double Kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
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
                        KGFAT = Math.Round(KGFAT, 2);
                        KGSNF = Math.Round(KGSNF, 2);
                        double MValue = 0;
                        MValue = KGFAT * cost;
                        //MValue = MValue / 100;
                        MValue = Math.Round(MValue, 2);
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
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        totsalesamount += OHandMvalue;
                    }
                    else
                    {
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        string Rateon = dr["rate_on"].ToString();
                        double weight = 0;
                        double KGFAT = 0;
                        double KGSNF = 0;
                        double ltrs = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                        double Kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                        double tstotal = 0;
                        tstotal = FAT + SNF;
                        tstotal = Math.Round(tstotal, 2);
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
                        KGFAT = Math.Round(KGFAT, 2);
                        KGSNF = Math.Round(KGSNF, 2);
                        double MValue = 0;
                        if (Rateon == "PerLtr")
                        {
                            MValue = cost * ltrs;
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
                            diffFAT = FAT - MFat;
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
                        OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        totsalesamount += OHandMvalue;
                        totsalesamount = Math.Ceiling(totsalesamount);
                    }
                }
            }
            //formula as per ERP 
            vendorpresentopp = (Math.Round(debitprice - totsalesamount)) + vendorpresentopp;
            lblopeningbalance.Text = vendorpresentopp.ToString();
            for (int j = 0; j < NoOfdays; j++)
            {
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = i;
                string dtcount = fromdate.AddDays(j).ToString();
                DateTime dtDOE = Convert.ToDateTime(dtcount);
                //string dtdate1 = branch["IndentDate"].ToString();
                string dtdate1 = dtDOE.AddDays(-1).ToString();
                DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                string ChangedTime1 = dtDOE1.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                string ChangedTime2 = dtDOE.AddDays(-1).ToString("dd MMM yy");
                newrow["Date"] = ChangedTime1;
                double totsale = 0;
                DataTable dtin = new DataTable();
                DataRow[] drr = dtAll.Select("doe='" + ChangedTime1 + "'");
                if (drr.Length > 0)
                {
                    dtin = drr.CopyToDataTable();
                }
                foreach (DataRow dr in dtin.Rows)
                {
                    string milktypes = dr["milktype"].ToString();
                    {
                        if (milktypes == "Buffalo")
                        {
                            double qty_ltr = 0;
                            double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                            double FAT = 0;
                            double.TryParse(dr["fat"].ToString(), out FAT);
                            FAT = Math.Round(FAT, 2);
                            double SNF = 0;
                            double.TryParse(dr["snf"].ToString(), out SNF);
                            string Rateon = dr["rate_on"].ToString();
                            double weight = 0;
                            double KGFAT = 0;
                            double KGSNF = 0;
                            double ltrs = 0;
                            double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
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
                            KGFAT = Math.Round(KGFAT, 2);
                            KGSNF = Math.Round(KGSNF, 2);
                            double MValue = 0;
                            MValue = KGFAT * cost;
                            //MValue = MValue / 100;
                            MValue = Math.Round(MValue, 2);
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
                            OHandMvalue = Math.Round(OHandMvalue, 2);
                            totsale += OHandMvalue;
                            totsale = Math.Ceiling(totsale);
                            newrow["Dc No"] = dr["dcno"].ToString();
                            newrow["Dc Amount"] = totsale;
                        }
                        else
                        {
                            DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                            string date = dtdoe.ToString("dd/MM/yyyy");
                            double FAT = 0;
                            double.TryParse(dr["fat"].ToString(), out FAT);
                            FAT = Math.Round(FAT, 2);
                            double SNF = 0;
                            double.TryParse(dr["snf"].ToString(), out SNF);
                            string Rateon = dr["rate_on"].ToString();
                            double weight = 0;
                            double KGFAT = 0;
                            double KGSNF = 0;
                            double ltrs = 0;
                            double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                            double tstotal = 0;
                            tstotal = FAT + SNF;
                            tstotal = Math.Round(tstotal, 2);
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
                            KGFAT = Math.Round(KGFAT, 2);
                            KGSNF = Math.Round(KGSNF, 2);
                            double MValue = 0;
                            if (Rateon == "PerLtr")
                            {
                                MValue = cost * ltrs;
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
                                diffFAT = FAT - MFat;
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
                            OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                            OHandMvalue = Math.Round(OHandMvalue, 2);
                            totsale += OHandMvalue;
                            totsale = Math.Ceiling(totsale);
                            newrow["Dc No"] = dr["dcno"].ToString();
                            newrow["Dc Amount"] = totsale;
                        }
                    }
                }
                double amtpaid = 0;
                DataTable dtamountcol = new DataTable();
                DataRow[] drramountcol = dtcollection.Select("doe='" + ChangedTime1 + "'");
                if (drramountcol.Length > 0)
                {
                    dtamountcol = drramountcol.CopyToDataTable();
                }
                double amount = 0;

                foreach (DataRow dr in dtamountcol.Rows)
                {
                    double.TryParse(dr["amount"].ToString(), out amount);
                    amtpaid += amount;
                }
                ///Get Opp Bal according to Erp Formula ..By Srinu
                double aopp = 0;
                if (i == 1)
                {
                    aopp = vendorpresentopp;
                }
                else
                {
                    aopp = vendorpresentopp;
                    if (totsale == 0)
                    {
                        aopp = oppcarry;
                    }
                }
                newrow["Opp Bal"] = Math.Ceiling(aopp);
                double totalamt = aopp + totsale;

                totalamt = Math.Ceiling(totalamt);
                newrow["Total Amount"] = totalamt;
                amtpaid = Math.Ceiling(amtpaid);
                if (ddlcollectiontype.SelectedValue == "Collection")
                {
                    newrow["Received Amount"] = amtpaid;
                }
                else
                {
                    newrow["Paid Amount"] = amtpaid;
                }
                totalpaidamount += amtpaid;
                double totalbalance = totalamt - amtpaid;
                totalbalance = Math.Ceiling(totalbalance);
                newrow["Balance"] = totalbalance;
                oppcarry = totalbalance;
                vendorpresentopp = totalbalance;
                Report.Rows.Add(newrow);
                i++;
                lblclosingbalance.Text = totalbalance.ToString();
            }
            DataRow newrow2 = Report.NewRow();
            newrow2["Date"] = "Total";
            newrow2["Dc Amount"] = totsalesamount;
            if (ddlcollectiontype.SelectedValue == "Collection")
            {
                newrow2["Received Amount"] = totalpaidamount;
            }
            else
            {
                newrow2["Paid Amount"] = totalpaidamount;
            }
            Report.Rows.Add(newrow2);
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}
   