using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class purchasedetails : System.Web.UI.Page
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
                    //fillbranches();
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
    DataTable jvreport = new DataTable();
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesDBManager SalesDB = new SalesDBManager();
        if (ddltype.SelectedValue == "Purchase Voucher")
        {
            hiddentype.Visible = true;
            hiddenbrances.Visible = true;
            hiddenwise.Visible = true;
            hideVehicles.Visible = false;
            grdReports.DataSource = null;
            grdReports.DataBind();
            grdjv.DataSource = null;
            grdjv.DataBind();
        }
        if (ddltype.SelectedValue == "Direct Purchase Voucher")
        {
            hiddentype.Visible = false;
            hiddenbrances.Visible = true;
            hiddenwise.Visible = true;
            hideVehicles.Visible = false;
            grdReports.DataSource = null;
            grdReports.DataBind();
            grdjv.DataSource = null;
            grdjv.DataBind();
        }
        if (ddltype.SelectedValue == "Other Party Direct Purchase Voucher")
        {
            hiddentype.Visible = false;
            hiddenbrances.Visible = false;
            hiddenwise.Visible = false;
            hideVehicles.Visible = true;
            grdReports.DataSource = null;
            grdReports.DataBind();
            fillbranches();
            grdjv.DataSource = null;
            grdjv.DataBind();
        }
    }
    protected void ddlBranchpurchase_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesDBManager SalesDB = new SalesDBManager();
        if (ddlBranchwisepurchase.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
        }
        if (ddlBranchwisepurchase.SelectedValue == "Branch Wise")
        {
            hideVehicles.Visible = true;
            if (ddlbranchTypepurchase.SelectedValue == "All")
            {
                cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE (branchmapping.superbranch = @branchid)");
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranchespurchase.DataSource = dttrips;
                ddlbranchespurchase.DataTextField = "vendorname";
                ddlbranchespurchase.DataValueField = "sno";
                ddlbranchespurchase.DataBind();
            }
            if (ddlbranchTypepurchase.SelectedValue == "Inter Branches")
            {
                cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE    (branchmapping.superbranch = @branchid) and vendors.branchtype='Inter Branch'");
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranchespurchase.DataSource = dttrips;
                ddlbranchespurchase.DataTextField = "vendorname";
                ddlbranchespurchase.DataValueField = "sno";
                ddlbranchespurchase.DataBind();
            }
            if (ddlbranchTypepurchase.SelectedValue == "Other Branches")
            {
                cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE    (branchmapping.superbranch = @branchid) and vendors.branchtype='Other Branch'");
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranchespurchase.DataSource = dttrips;
                ddlbranchespurchase.DataTextField = "vendorname";
                ddlbranchespurchase.DataValueField = "sno";
                ddlbranchespurchase.DataBind();
            }
        }
        if (ddlBranchwisepurchase.SelectedValue == "Vehicle Wise")
        {
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT sno,vehicleno,capacity,noofqty FROM vehicle_master where branchid=@vebranchid");
            cmd.Parameters.Add("@vebranchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranchespurchase.DataSource = dttrips;
            ddlbranchespurchase.DataTextField = "vehicleno";
            ddlbranchespurchase.DataValueField = "vehicleno";
            ddlbranchespurchase.DataBind();
        }
    }
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        if (ddltype.SelectedValue == "Purchase Voucher")
        {
            btn_Generate_Click_purchase();
        }
        if (ddltype.SelectedValue == "Direct Purchase Voucher")
        {
            btn_Generate_Click_direct();
        }
        if (ddltype.SelectedValue == "Other Party Direct Purchase Voucher")
        {
            btn_Generate_Click_other();
        }
    }
    protected void btn_Save_Purchase_Click(object sender, EventArgs e)
    {
        if (ddltype.SelectedValue == "Purchase Voucher")
        {
            btn_Save_Purchasevoucher_Click();
        }
        if (ddltype.SelectedValue == "Direct Purchase Voucher")
        {
            btn_Save_Purchasedirect_Click();
        }
        if (ddltype.SelectedValue == "Other Party Direct Purchase Voucher")
        {
            btn_Save_Purchaseotherparty_Click();
        }
    }
    void btn_Generate_Click_purchase()
    {
        try
        {
            pnlcow.Visible = false;
            grdcow.DataSource = null;
            grdcow.DataBind();
            grdReports.DataSource = null;
            grdReports.DataBind();
            Session["filename"] = "Purchase Voucher";
            Session["title"] = "Purchase Voucher Details";
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

            if (ddlReportType.SelectedValue == "Buffalo")
            {

                Report.Columns.Add("WH CODE");
                Report.Columns.Add("CUSTOMER CODE");
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("REFRENCE NO");
                Report.Columns.Add("TAX DATE");
                Report.Columns.Add("DOC DATE");
                Report.Columns.Add("DOC DUE DATE");
                Report.Columns.Add("LEDGER CODE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM CODE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("Diff Amount");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("FRIGHT TYPE");
                Report.Columns.Add("FRIGHT CODE");
                Report.Columns.Add("FRIGHT AMOUNT");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("DC NUMBER");
                Report.Columns.Add("Narration");
                Report.Columns.Add("salestype");

                jvreport.Columns.Add("JV No");
                jvreport.Columns.Add("JV Date");
                jvreport.Columns.Add("Wh Code");
                jvreport.Columns.Add("Ledger Code");
                jvreport.Columns.Add("Ledger Name");
                jvreport.Columns.Add("Amount");
                jvreport.Columns.Add("Narration");

                if (ddlBranchwisepurchase.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe,  milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat,  milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on,  milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf,  milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost,  milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost,  milktransaction_logs.transport, vendors.vendorname, vendors.purchasetype, vendors.tallyoh, vendors.ledgertype, branch_info.branchcode, milktransactions.cellno,  vendors.sapcustomercode, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.salesledgercode, vendors.sapcode,  vendors.ledgertype1, branch_info.whcode FROM  milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransactions.branchid = @abranchid) AND  (milktransactions.vehicleno = @vehicleno) ORDER BY milktransactions.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@vehicleno", ddlbranchespurchase.SelectedValue);
                    cmd.Parameters.Add("@abranchid", BranchID);
                }
                else if (ddlBranchwisepurchase.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe,  milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat,  milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on,  milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf,  milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost,  milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost,  milktransaction_logs.transport, vendors.vendorname, vendors.purchasetype, vendors.tallyoh, vendors.ledgertype, branch_info.branchcode, milktransactions.cellno,  vendors.sapcustomercode, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.sapvendorcode, vendors.salestallyoh,  vendors.salesledgercode, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1, branch_info.whcode FROM  milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransactions.branchid = @bbranchid) AND  (milktransaction_logs.milktype = 'Buffalo') AND (milktransactions.sectionid = @sectionid) ORDER BY milktransactions.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@sectionid", ddlbranchespurchase.SelectedValue);
                    cmd.Parameters.Add("@bbranchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("SELECT  milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe,  milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat,  milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on,  milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf,  milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost,  milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost,  milktransaction_logs.transport, vendors.vendorname, vendors.purchasetype, vendors.tallyoh, vendors.ledgertype, branch_info.branchcode, milktransactions.cellno,  vendors.sapcustomercode, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.sapvendorcode, vendors.salesledgercode,  vendors.salestallyoh, vendors.salesledgertype, vendors.ledgertype1, branch_info.whcode FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransactions.branchid = @cbranchid) AND  (milktransaction_logs.milktype = 'Buffalo') ORDER BY milktransactions.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@cbranchid", BranchID);
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
                            dcnaration = "Being PURCHASE Of Milk from " + ddlbranchespurchase.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DataRow jvnewrow = jvreport.NewRow();
                        if (ddlbranchTypepurchase.SelectedValue == "Inter Branches")
                        {
                            newrow["WH CODE"] = "SVDSPP02";
                        }
                        else
                        {
                            newrow["WH CODE"] = dr["whcode"].ToString();
                        }
                        newrow["DOC DATE"] = dr["doe"].ToString();
                        newrow["DOC DUE DATE"] = dr["doe"].ToString();
                        newrow["TAX DATE"] = dr["doe"].ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd-MMM-yyyy");
                        //newrow["INVOICE DATE"] = date;
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
                        string CLR = dr["clr"].ToString();
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
                        KGFAT = Math.Round(KGFAT, 2);
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
                        kgsnftotal += KGSNF;
                        double MValue = 0;
                        MValue = KGFAT * cost;
                        MValue = Math.Round(MValue, 2);
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
                        newrow["FRIGHT AMOUNT"] = OHcost;
                        ohtotal += OHcost;
                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                        snf9total += DiffSNFCost;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        string cn = "SRI VYSHNAVI DAIRY PVT LTD";
                        string vendername = dr["vendorname"].ToString();
                        if (ddlbranchTypepurchase.SelectedItem.Text == "Other Branches")
                        {
                            cn = vendername;
                            newrow["ITEM NAME"] = "WHOLE MILK Others";
                            newrow["ITEM CODE"] = "51410003";
                        }
                        else
                        {
                            cn = vendername;
                            newrow["ITEM NAME"] = "WHOLE MILK";
                            newrow["ITEM CODE"] = "11120003";
                        }
                        string ccname = "";
                        string tallyoh = dr["tallyoh"].ToString();
                        string ledgertype = dr["ledgertype"].ToString();
                        string celltype = dr["cellno"].ToString();
                        string branchcode = dr["branchcode"].ToString();
                        string invoiceno = dr["partydcno"].ToString();
                        string ledgerccname = "";
                        milkvaluetotal += OHandMvalue;
                        if (ddlBranchwisepurchase.SelectedValue == "Buffalo")
                        {
                        }
                        newrow["LEDGER CODE"] = dr["sapcode"].ToString();
                        newrow["LEDGER TYPE"] = ledgertype;
                        newrow["REFRENCE NO"] = invoiceno + "-" + celltype + "-" + branchcode;
                        ledgerccname = cn;
                        newrow["CUSTOMER CODE"] = dr["sapvendorcode"].ToString();
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        newrow["FRIGHT CODE"] = dr["purchaseohcode"].ToString();
                        newrow["FRIGHT TYPE"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        double rateltrcost = Math.Round(ltrcost, 2);
                        double diffrateperltr = ltrcost - rateltrcost;
                        double totaldiff = diffrateperltr * ltrs;
                        double netval = MValue + OHcost + DiffSNFCost;
                        newrow["Net VALUE"] = Math.Round(netval, 2);

                        double ledgeramount = 0;
                        ledgeramount = netval - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        newrow["LEDGER AMOUNT"] = ledgeramount;

                        double rateperltr = ledgeramount / qty_kgs;
                        double rateperltrround = Math.Round(rateperltr, 2);
                        double rateperltrdiff = rateperltrround - rateperltr;
                        double totalrateperltrdiff = rateperltrdiff * qty_kgs;
                        newrow["Diff Amount"] = Math.Round(totalrateperltrdiff, 2);
                        newrow["RATE PER LTR"] = rateperltrround;

                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["partydcno"].ToString();
                        newrow["FAT"] = FAT;
                        newrow["SNF"] = SNF;
                        newrow["CLR"] = CLR;
                        newrow["DC NUMBER"] = newdcno;
                        newrow["salestype"] = dr["purchasetype"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " , " + " KG fat Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        //jv format
                        double jvdiffammount = Math.Round(totaldiff, 2);
                        jvnewrow["JV No"] = "jv-" + invoiceno + "-" + celltype + "-" + branchcode;
                        DateTime jvDate = Convert.ToDateTime(dtdoe);
                        string jdates = jvDate.ToString("MM/dd/yyyy");
                        jvnewrow["JV Date"] = jdates;
                        jvnewrow["Wh Code"] = dr["whcode"].ToString();
                        jvnewrow["Ledger Code"] = dr["sapcode"].ToString();
                        jvnewrow["Ledger Name"] = ledgertype;
                        jvnewrow["Amount"] = jvdiffammount;
                        jvnewrow["Narration"] = dcnaration;
                        jvreport.Rows.Add(jvnewrow);
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
                    grdjv.DataSource = jvreport;
                    grdjv.DataBind();
                    Session["xportdata"] = Report;
                    hidepanel.Visible = true;
                }
                else
                {
                    lblmsg.Text = "No data were found";
                    hidepanel.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue == "Cow" || ddlReportType.SelectedValue == "Condensed")
            {
                Report.Columns.Add("WH CODE");
                Report.Columns.Add("CUSTOMER CODE");
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("REFRENCE NO");
                Report.Columns.Add("TAX DATE");
                Report.Columns.Add("DOC DATE");
                Report.Columns.Add("DOC DUE DATE");
                Report.Columns.Add("LEDGER CODE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM CODE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("Diff Amount");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("FRIGHT TYPE");
                Report.Columns.Add("FRIGHT CODE");
                Report.Columns.Add("FRIGHT AMOUNT");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("DC NUMBER");
                Report.Columns.Add("Narration");
                Report.Columns.Add("salestype");

                jvreport.Columns.Add("JV No");
                jvreport.Columns.Add("JV Date");
                jvreport.Columns.Add("Wh Code");
                jvreport.Columns.Add("Ledger Code");
                jvreport.Columns.Add("Ledger Name");
                jvreport.Columns.Add("Amount");
                jvreport.Columns.Add("Narration");


                if (ddlBranchwisepurchase.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT  milktransactions.dcno, milktransactions.cellno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno,  milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf,  milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on,  milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat,  milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost,  milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost,  milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, branch_info.branchcode, milktransactions.cellno AS Expr1,  branch_info.whcode, vendors.sapcustomercode, vendors.purchasetype, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.sapvendorcode,  vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1 FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransactions.vehicleno = @vehicleno) AND  (milktransactions.branchid = @dbranchid) ORDER BY milktransactions.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@vehicleno", ddlbranchespurchase.SelectedValue);
                    cmd.Parameters.Add("@dbranchid", BranchID);
                }
                else if (ddlBranchwisepurchase.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("SELECT  milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe,  milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat,  milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on,  milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat,  milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost,  milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost,  milktransaction_logs.transport, milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, vendors.vendorname, vendors.tallyoh, vendors.ledgertype,  branch_info.branchcode, milktransactions.cellno, vendors.sapcustomercode, vendors.purchasetype, vendors.customername, vendors.salesohcode, vendors.purchaseohcode,  vendors.sapvendorcode, vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1,  branch_info.whcode FROM  milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno LEFT OUTER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype = 'Cow') AND  (milktransactions.sectionid = @sectionid) AND (milktransactions.branchid = @ebranchid) ORDER BY milktransactions.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@ebranchid", BranchID);
                    cmd.Parameters.Add("@sectionid", ddlbranchespurchase.SelectedValue);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_fatpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.purchasetype, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.branchid=@fbranchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@fbranchid", BranchID);
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
                            dcnaration = "Being PURCHASE Of Milk from " + ddlbranchespurchase.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DataRow jvnewrow = jvreport.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        if (ddlbranchTypepurchase.SelectedValue == "Inter Branches")
                        {
                            newrow["WH CODE"] = "SVDSPP02";
                        }
                        else
                        {
                            newrow["WH CODE"] = dr["whcode"].ToString();
                        }
                        newrow["TAX DATE"] = dr["doe"].ToString();
                        newrow["DOC DATE"] = dr["doe"].ToString();
                        newrow["DOC DUE DATE"] = dr["doe"].ToString();
                        string date = dtdoe.ToString("dd-MMM-yyyy");
                        double qty_kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                        double qty_ltr = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                        newrow["QTY KG'S/LTRS"] = dr["qty_ltr"].ToString();
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        string CLR = dr["clr"].ToString();
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
                                weight = ltrs;
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
                        KGFAT = Math.Round(KGFAT, 2);
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
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
                        ohtotal += OHcost;
                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                        snf9total += DiffSNFCost;
                        double roundvalue = Math.Round(OHandMvalue, 0);
                        //  newrow["NET VALUE"] = roundvalue;
                        double diffvalue = roundvalue - OHandMvalue;
                        milkvaluetotal += OHandMvalue;
                        if (ddlBranchwisepurchase.SelectedValue == "Cow")
                        {

                        }
                        string plant = "Pbk";
                        string cn = "";
                        string vendername = dr["vendorname"].ToString();
                        if (ddlbranchTypepurchase.SelectedItem.Text == "Other Branches")
                        {
                            cn = vendername;
                            newrow["ITEM NAME"] = "COW MILK Others";
                            newrow["ITEM CODE"] = "51410002";
                        }
                        else
                        {
                            cn = vendername;
                            newrow["ITEM NAME"] = "COW MILK";
                            newrow["ITEM CODE"] = "11120002";
                        }
                        string ccname = "";
                        string tallyoh = dr["tallyoh"].ToString();
                        string ledgertype = dr["ledgertype"].ToString();
                        string celltype = dr["cellno"].ToString();
                        string branchcode = dr["branchcode"].ToString();
                        string invoiceno = dr["partydcno"].ToString();
                        if (vendername == "KOLAR-CDMPU LTD.")
                        {
                            ccname = "KOLAR-CDMPU LTD";
                            cn = "";
                        }
                        if (vendername == "Annamaiah Dairy")
                        {
                            ccname = "Annamaiah Dairy";
                            cn = "";
                            ledgertype = "Purchase of Cow Milk Others";
                        }
                        newrow["REFRENCE NO"] = invoiceno + "-" + celltype + "-" + branchcode;
                        newrow["LEDGER CODE"] = dr["sapcode"].ToString();
                        newrow["LEDGER TYPE"] = ledgertype;
                        string ledgerccname = cn + " " + ccname;
                        newrow["CUSTOMER CODE"] = dr["sapvendorcode"].ToString();
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        newrow["FRIGHT TYPE"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        double rateltrcost = Math.Round(ltrcost, 2);
                        double diffrateperltr = ltrcost - rateltrcost;
                        double totaldiff = diffrateperltr * ltrs;
                        newrow["Diff Amount"] = Math.Round(totaldiff, 2);


                        double netval = ltrs * rateltrcost;
                        newrow["NET VALUE"] = Math.Round(netval, 0);
                        netval = Math.Round(netval, 0);
                        double ledgeramount = 0;
                        ledgeramount = netval - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        OHcost = Math.Round(OHcost, 0);
                        newrow["FRIGHT AMOUNT"] = OHcost;

                        //double ledgeramount = 0;
                        //ledgeramount = OHandMvalue - OHcost;
                        //ledgeramount = Math.Round(ledgeramount, 0);
                        //OHcost = Math.Round(OHcost, 0);
                        newrow["FRIGHT CODE"] = dr["purchaseohcode"].ToString();
                        // newrow["FRIGHT AMOUNT"] = OHcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["partydcno"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " ," + " TS Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        newrow["LEDGER AMOUNT"] = ledgeramount;
                        double LAMT = ledgeramount;
                        newrow["RATE PER LTR"] = Math.Round(LAMT / qty_ltr, 2);
                        newrow["FAT"] = FAT;
                        newrow["SNF"] = SNF;
                        newrow["CLR"] = CLR;
                        newrow["DC NUMBER"] = newdcno;
                        newrow["salestype"] = dr["purchasetype"].ToString();

                        //jv format
                        double jvdiffammount = Math.Round(totaldiff, 2);
                        jvnewrow["JV No"] = "jv-" + invoiceno + "-" + celltype + "-" + branchcode;
                        DateTime jvDate = Convert.ToDateTime(dtdoe);
                        string jdates = jvDate.ToString("MM/dd/yyyy");
                        jvnewrow["JV Date"] = jdates;
                        jvnewrow["Wh Code"] = dr["whcode"].ToString();
                        jvnewrow["Ledger Code"] = dr["sapcode"].ToString();
                        jvnewrow["Ledger Name"] = ledgertype;
                        jvnewrow["Amount"] = jvdiffammount;
                        jvnewrow["Narration"] = dcnaration;
                        jvreport.Rows.Add(jvnewrow);
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
                    grdjv.DataSource = jvreport;
                    grdjv.DataBind();
                    Session["xportdata"] = Report;
                    hidepanel.Visible = true;
                }
                else
                {
                    lblmsg.Text = "No data were found";
                    hidepanel.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue == "Skim")
            {
                Report.Columns.Add("WH CODE");
                Report.Columns.Add("CUSTOMER CODE");
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("REFRENCE NO");
                Report.Columns.Add("TAX DATE");
                Report.Columns.Add("DOC DATE");
                Report.Columns.Add("DOC DUE DATE");
                Report.Columns.Add("LEDGER CODE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM CODE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("Diff Amount");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("FRIGHT TYPE");
                Report.Columns.Add("FRIGHT CODE");
                Report.Columns.Add("FRIGHT AMOUNT");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("DC NUMBER");
                Report.Columns.Add("Narration");
                Report.Columns.Add("salestype");

                jvreport.Columns.Add("JV No");
                jvreport.Columns.Add("JV Date");
                jvreport.Columns.Add("Wh Code");
                jvreport.Columns.Add("Ledger Code");
                jvreport.Columns.Add("Ledger Name");
                jvreport.Columns.Add("Amount");
                jvreport.Columns.Add("Narration");
                if (ddlBranchwisepurchase.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT  milktransactions.dcno, milktransactions.cellno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno,  milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf,  milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on,  milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat,  milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost,  milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost,  milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, branch_info.branchcode, milktransactions.cellno AS Expr1,  branch_info.whcode, vendors.sapcustomercode, vendors.purchasetype, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.sapvendorcode,  vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1 FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransactions.vehicleno = @vehicleno) AND  (milktransactions.branchid = @dbranchid) ORDER BY milktransactions.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@vehicleno", ddlbranchespurchase.SelectedValue);
                    cmd.Parameters.Add("@dbranchid", BranchID);
                }
                else if (ddlBranchwisepurchase.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("SELECT  milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe,  milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat,  milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on,  milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat,  milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost,  milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost,  milktransaction_logs.transport, milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, vendors.vendorname, vendors.tallyoh, vendors.ledgertype,  branch_info.branchcode, milktransactions.cellno, vendors.sapcustomercode, vendors.purchasetype, vendors.customername, vendors.salesohcode, vendors.purchaseohcode,  vendors.sapvendorcode, vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1,  branch_info.whcode FROM  milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno LEFT OUTER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype = 'Skim') AND  (milktransactions.sectionid = @sectionid) AND (milktransactions.branchid = @ebranchid) ORDER BY milktransactions.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@ebranchid", BranchID);
                    cmd.Parameters.Add("@sectionid", ddlbranchespurchase.SelectedValue);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_fatpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.purchasetype, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Skim') AND (milktransactions.branchid=@fbranchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@fbranchid", BranchID);
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
                            dcnaration = "Being PURCHASE Of Milk from " + ddlbranchespurchase.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DataRow jvnewrow = jvreport.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        if (ddlbranchTypepurchase.SelectedValue == "Inter Branches")
                        {
                            newrow["WH CODE"] = "SVDSPP02";
                        }
                        else
                        {
                            newrow["WH CODE"] = dr["whcode"].ToString();
                        }
                        newrow["TAX DATE"] = dr["doe"].ToString();
                        newrow["DOC DATE"] = dr["doe"].ToString();
                        newrow["DOC DUE DATE"] = dr["doe"].ToString();
                        string date = dtdoe.ToString("dd-MMM-yyyy");
                        double qty_kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                        double qty_ltr = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                        newrow["QTY KG'S/LTRS"] = dr["qty_ltr"].ToString();
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        string CLR = dr["clr"].ToString();
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
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
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
                        ohtotal += OHcost;
                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                        snf9total += DiffSNFCost;
                        double roundvalue = Math.Round(OHandMvalue, 0);
                        newrow["NET VALUE"] = roundvalue;
                        double diffvalue = roundvalue - OHandMvalue;
                        milkvaluetotal += OHandMvalue;
                        if (ddlBranchwisepurchase.SelectedValue == "Skim")
                        {

                        }
                        string plant = "Pbk";
                        string cn = "";
                        string vendername = dr["vendorname"].ToString();
                        if (ddlbranchTypepurchase.SelectedItem.Text == "Other Branches")
                        {
                            cn = vendername;
                            newrow["ITEM NAME"] = "SKIM MILK Others";
                            newrow["ITEM CODE"] = "";
                        }
                        else
                        {
                            cn = vendername;
                            newrow["ITEM NAME"] = "SKIM MILK";
                            newrow["ITEM CODE"] = "";
                        }
                        string ccname = "";
                        string tallyoh = dr["tallyoh"].ToString();
                        string ledgertype = dr["ledgertype"].ToString();
                        string celltype = dr["cellno"].ToString();
                        string branchcode = dr["branchcode"].ToString();
                        string invoiceno = dr["partydcno"].ToString();
                        if (vendername == "KOLAR-CDMPU LTD.")
                        {
                            ccname = "KOLAR-CDMPU LTD";
                            cn = "";
                        }
                        if (vendername == "Annamaiah Dairy")
                        {
                            ccname = "Annamaiah Dairy";
                            cn = "";
                            ledgertype = "Purchase of SKIM Milk Others";
                        }
                        newrow["REFRENCE NO"] = invoiceno + "-" + celltype + "-" + branchcode;
                        newrow["LEDGER CODE"] = dr["sapcode"].ToString();
                        newrow["LEDGER TYPE"] = ledgertype;
                        string ledgerccname = cn + " " + ccname;
                        newrow["CUSTOMER CODE"] = dr["sapvendorcode"].ToString();
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        newrow["FRIGHT TYPE"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        double roundltrcost = Math.Round(ltrcost, 2);
                        double diffrateperltr = ltrcost - roundltrcost;
                        double diffamount = diffrateperltr * ltrs;
                        newrow["Diff Amount"] = Math.Round(diffamount, 2);
                        newrow["RATE PER LTR"] = roundltrcost;
                        double ledgeramount = 0;
                        ledgeramount = OHandMvalue - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        OHcost = Math.Round(OHcost, 0);
                        newrow["FRIGHT CODE"] = dr["purchaseohcode"].ToString();
                        newrow["FRIGHT AMOUNT"] = OHcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["partydcno"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " ," + " TS Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        newrow["LEDGER AMOUNT"] = roundvalue - OHcost;
                        newrow["FAT"] = FAT;
                        newrow["SNF"] = SNF;
                        newrow["CLR"] = CLR;
                        newrow["DC NUMBER"] = newdcno;
                        newrow["salestype"] = dr["purchasetype"].ToString();

                        //jv format
                        double jvdiffammount = Math.Round(diffamount, 2);
                        jvnewrow["JV No"] = "jv-" + invoiceno + "-" + celltype + "-" + branchcode;
                        DateTime jvDate = Convert.ToDateTime(dtdoe);
                        string jdates = jvDate.ToString("MM/dd/yyyy");
                        jvnewrow["JV Date"] = jdates;
                        jvnewrow["Wh Code"] = dr["whcode"].ToString();
                        jvnewrow["Ledger Code"] = dr["sapcode"].ToString();
                        jvnewrow["Ledger Name"] = ledgertype;
                        jvnewrow["Amount"] = jvdiffammount;
                        jvnewrow["Narration"] = dcnaration;
                        jvreport.Rows.Add(jvnewrow);
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

    void btn_Save_Purchasevoucher_Click()
    {
        try
        {
            SalesDBManager SalesDB = new SalesDBManager();
            DateTime CreateDate = sapDBmanager.GetTime(vdm.conn);
            sapDBmanager SAPvdm = new sapDBmanager();
            DateTime fromdate = DateTime.Now;
            DateTime ServerDateCurrentdate = sapDBmanager.GetTime(vdm.conn);
            string datetime = ServerDateCurrentdate.ToString("MM/dd/yyyy");
            string BranchID = Session["Branch_ID"].ToString();
            DataTable dt = (DataTable)Session["xportdata"];
            if (dt.Rows.Count > 0)
            {
                DateTime doe = DateTime.Now;
                foreach (DataRow dr in dt.Rows)
                {
                    string DocDate = dr["DOC DATE"].ToString();
                    DateTime dDate = Convert.ToDateTime(DocDate);
                    cmd = new SqlCommand("SELECT CardCode, CardName FROM EMROPCH WHERE DocDate BETWEEN @d1 and @d2 AND ReferenceNo=@rno");
                    cmd.Parameters.Add("@d1", GetLowDate(dDate));
                    cmd.Parameters.Add("@d2", GetHighDate(dDate));
                    cmd.Parameters.Add("@rno", dr["REFRENCE NO"].ToString());
                    DataTable dtSAPpurchase = SAPvdm.SelectQuery(cmd).Tables[0];
                    if (dtSAPpurchase.Rows.Count > 0)
                    {
                        lblmsg.Text = "This date data already Saved";
                    }
                    else
                    {
                        cmd = new SqlCommand("Insert into EMROPCH (CreateDate, CardCode, CardName, TaxDate, DocDate, DocDueDate, ReferenceNo, GRNRefNo, ItemCode, Dscription, WhsCode, Quantity, Price, VAT_Percent, Quantity_Kgs, SNF, FAT, CLR, REMARKS, DOE, MILKTYPE, PARTYDCNO, ENTRYDATE, PURCHASETYPE, B1Upload, Processed, UnitQty, UnitCost, BRANCHID, OcrCode, OcrCode2, TaxCode, TaxAmount, Account, OverHeadAmount1) values (@CreateDate,@CardCode,@CardName,@TaxDate,@DocDate,@DocDueDate,@ReferenceNo, @GRNRefNo, @ItemCode, @Dscription, @WhsCode, @Quantity, @Price, @VAT_Percent, @Quantity_Kgs,@SNF,@FAT,@CLR,@REMARKS,@DOE,@MILKTYPE,@PARTYDCNO,@ENTRYDATE,@purchasetype,@B1Upload,@Processed,@UnitQty,@UnitCost,@BRANCHID, @OcrCode, @OcrCode2, @TaxCode, @TaxAmount, @Account,@OverHeadAmount1)");
                        cmd.Parameters.Add("@BRANCHID", BranchID);
                        cmd.Parameters.Add("@CreateDate", datetime);
                        string customercode = dr["CUSTOMER CODE"].ToString();
                        cmd.Parameters.Add("@CardCode", customercode);
                        string customername = dr["CUSTOMER NAME"].ToString();
                        cmd.Parameters.Add("@CardName", customername);

                        string TaxDate = dr["TAX DATE"].ToString();
                        DateTime TDate = Convert.ToDateTime(TaxDate);
                        TaxDate = TDate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@TaxDate", TaxDate);


                        DocDate = dDate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@DocDate", DocDate);
                        string DocDueDate = dr["DOC DUE DATE"].ToString();
                        DateTime docdate = Convert.ToDateTime(DocDueDate);
                        DocDueDate = docdate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@DocDueDate", DocDueDate);
                        string refno = dr["REFRENCE NO"].ToString();
                        cmd.Parameters.Add("@ReferenceNo", refno);
                        string ledgertype = dr["LEDGER TYPE"].ToString();
                        int ledgerlength = ledgertype.Length;
                        string whcode = dr["WH CODE"].ToString();
                        int whcodelength = ledgertype.Length;
                        cmd.Parameters.Add("@WhsCode", whcode);
                        cmd.Parameters.Add("@OcrCode", whcode);
                        string tTaxCode = "EXEMPT";
                        cmd.Parameters.Add("@TaxCode", tTaxCode);
                        string tTaxAmount = "0";
                        cmd.Parameters.Add("@TaxAmount", tTaxAmount);
                        string raeteperltr = dr["RATE PER LTR"].ToString();
                        cmd.Parameters.Add("@price", raeteperltr);
                        string VAT_Percent = "0";
                        cmd.Parameters.Add("@VAT_Percent", VAT_Percent);
                        string kgs = dr["QTY KG'S/LTRS"].ToString();
                        cmd.Parameters.Add("@UnitQty", kgs);
                        cmd.Parameters.Add("@Quantity", kgs);
                        string netvalue = dr["LEDGER AMOUNT"].ToString();
                        cmd.Parameters.Add("@UnitCost", netvalue);
                        cmd.Parameters.Add("@Quantity_Kgs", kgs);
                        string fat = dr["FAT"].ToString();
                        string snf = dr["SNF"].ToString();
                        string clr = dr["CLR"].ToString();
                        cmd.Parameters.Add("@SNF", snf);
                        cmd.Parameters.Add("@FAT", fat);
                        cmd.Parameters.Add("@CLR", clr);

                        string remarks = dr["Narration"].ToString();
                        cmd.Parameters.Add("@REMARKS", remarks);

                        cmd.Parameters.Add("@DOE", datetime);

                        string itemcode = dr["ITEM CODE"].ToString();
                        cmd.Parameters.Add("@ItemCode", itemcode);
                        string ocrcode2 = "";
                        if (ddlReportType.SelectedValue == "Buffalo")
                        {
                            ocrcode2 = "P0002";
                        }
                        else
                        {
                            ocrcode2 = "P0001";
                        }
                        cmd.Parameters.Add("@OcrCode2", ocrcode2);
                        string itemtype = dr["ITEM NAME"].ToString();
                        cmd.Parameters.Add("@MILKTYPE", itemtype);
                        cmd.Parameters.Add("@Dscription", itemtype);
                        string dcno = dr["DC NUMBER"].ToString();
                        cmd.Parameters.Add("@PARTYDCNO", dcno);
                        cmd.Parameters.Add("@ENTRYDATE", datetime);
                        string ledgertypes = dr["salestype"].ToString();
                        cmd.Parameters.Add("@purchasetype", ledgertypes);
                        string B1Upload = "N";
                        string Processed = "N";
                        cmd.Parameters.Add("@B1Upload", B1Upload);
                        cmd.Parameters.Add("@Processed", Processed);
                        string GRNRefNo = "NA";
                        cmd.Parameters.Add("@GRNRefNo", GRNRefNo);
                        string Account = dr["LEDGER CODE"].ToString();
                        cmd.Parameters.Add("@Account", Account);
                        string OverHeadAmount1 = dr["FRIGHT AMOUNT"].ToString();
                        cmd.Parameters.Add("@OverHeadAmount1", OverHeadAmount1);
                        SAPvdm.insert(cmd);
                        // 
                    }
                }
            }
            DataTable dtempty = new DataTable();
            if (lblmsg.Text == "")
            {
                lblmsg.Text = "Successfully Saved";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }

    DataTable dtdirectReport = new DataTable();
    DataTable dtdirectcow = new DataTable();

    private void btn_Generate_Click_direct()
    {
        try
        {
            pnlcow.Visible = false;
            grdcow.DataSource = null;
            grdcow.DataBind();
            grdReports.DataSource = null;
            grdReports.DataBind();
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

            if (ddlReportType.SelectedValue == "Buffalo")
            {
                Report.Columns.Add("WH CODE");
                Report.Columns.Add("CUSTOMER CODE");
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("REFRENCE NO");
                Report.Columns.Add("TAX DATE");
                Report.Columns.Add("DOC DATE");
                Report.Columns.Add("DOC DUE DATE");
                Report.Columns.Add("LEDGER CODE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM CODE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("Diff Amount");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("FRIGHT TYPE");
                Report.Columns.Add("FRIGHT CODE");
                Report.Columns.Add("FRIGHT AMOUNT");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("DC NUMBER");
                Report.Columns.Add("Narration");
                Report.Columns.Add("salestype");

                jvreport.Columns.Add("JV No");
                jvreport.Columns.Add("JV Date");
                jvreport.Columns.Add("Wh Code");
                jvreport.Columns.Add("Ledger Code");
                jvreport.Columns.Add("Ledger Name");
                jvreport.Columns.Add("Amount");
                jvreport.Columns.Add("Narration");

                if (ddlBranchwisepurchase.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost,despatch_entry.invoiceno, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost,  vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc,  vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.cell, directsale.veh_sno AS vehicleno, vendors.ledgertype1, vendors.purchasetype, vendors.sapcustomercode, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.sapvendorcode, vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, branch_info.whcode FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno = directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.veh_sno = @vehicleno) AND (directsale.branchid = @vbranchid) AND (directsale.milktype = 'Buffalo') ORDER BY directsale.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@vehicleno", ddlbranchespurchase.SelectedValue);
                    cmd.Parameters.Add("@vbranchid", BranchID);
                }
                else if (ddlBranchwisepurchase.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost,despatch_entry.invoiceno, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost,  vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc,  vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on,  directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.cell, directsale.veh_sno AS vehicleno,  branch_info.whcode, vendors.purchasetype, vendors.sapcustomercode, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.sapvendorcode,  vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1 FROM  directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno = directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.branchid = @bbranchid) AND (directsale.milktype = 'Buffalo') AND (directsale.fromccid = @sectionid) ORDER BY directsale.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranchespurchase.SelectedValue);
                    cmd.Parameters.Add("@bbranchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("SELECT   vendor_subtable.overheadon, vendor_subtable.overheadcost,despatch_entry.invoiceno, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost,  vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc,  vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on,  directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.cell, directsale.veh_sno AS vehicleno, vendors.purchasetype, vendors.sno, branch_info.whcode, vendors.sapcustomercode, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.sapvendorcode,  vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1 FROM  directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno = directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.branchid = @mbranchid) AND (directsale.milktype = 'Buffalo') AND (directsale.fromccid = @sectionid) ORDER BY directsale.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranchespurchase.SelectedValue);
                    cmd.Parameters.Add("@mbranchid", BranchID);
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
                        string newdcno1 = dr["dc_no"].ToString();
                        DataTable dtin = new DataTable();
                        DataRow[] drr = dtDispatch.Select("dc_no='" + newdcno1 + "'");
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
                            dcnaration = "Being PURCHASE Of Milk from " + ddlbranchespurchase.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DataRow jvnewrow = jvreport.NewRow();
                        if (ddlbranchTypepurchase.SelectedValue == "Inter Branches")
                        {
                            newrow["WH CODE"] = "SVDSPP02";
                        }
                        else
                        {
                            newrow["WH CODE"] = dr["whcode"].ToString();
                        }
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        newrow["TAX DATE"] = dr["doe"].ToString();
                        newrow["DOC DATE"] = dr["doe"].ToString();
                        newrow["DOC DUE DATE"] = dr["doe"].ToString();
                        string date = dtdoe.ToString("dd-MMM-yyyy");
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
                        string CLR = dr["clr"].ToString();
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
                        double.TryParse(dr["rate"].ToString(), out cost);
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
                        double OHcost = 0;
                        double overheadcost = 0;
                        double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                        string OverheadOn = dr["overheadon"].ToString();
                        if (OverheadOn == "Ltrs")
                        {
                            OHcost = overheadcost * ltrs;
                        }
                        else
                        {
                            OHcost = overheadcost * Kgs;
                        }
                        KGFAT = Math.Round(KGFAT, 2);
                        kgfattotal += KGFAT;
                        double MValue = 0;
                        MValue = KGFAT * cost;
                        MValue = Math.Round(MValue, 2);
                        mvaluetotal += MValue;
                        double OHandMvalue = 0;
                        OHandMvalue = MValue;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        double roundvalue = Math.Round(OHandMvalue, 0);
                        milkvaluetotal += OHandMvalue;
                        double ledgeramount = 0;
                        ledgeramount = OHandMvalue - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        OHcost = Math.Round(OHcost, 0);
                        newrow["FRIGHT AMOUNT"] = OHcost;
                        ohtotal += OHcost;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        double nval = OHandMvalue + OHcost + DiffSNFCost;
                        newrow["Net VALUE"] = Math.Round(nval);
                        double diffvalue = nval - OHcost;
                        newrow["LEDGER AMOUNT"] = Math.Round(diffvalue);
                        string cn = "SRI VYSHNAVI DAIRY PVT LTD";
                        string vendername = dr["fromcc"].ToString();
                        string ccname = "";
                        string tallyoh = dr["tallyoh"].ToString(); ;
                        string ledgertype = dr["ledgertype"].ToString();
                        string ledgerccname = "";
                        milkvaluetotal += OHandMvalue;
                        if (ddlbranchTypepurchase.SelectedValue == "Other Branches")
                        {
                            newrow["ITEM NAME"] = "WHOLE MILK Others";
                            newrow["ITEM CODE"] = "51410003";
                        }
                        else
                        {
                            newrow["ITEM NAME"] = "WHOLE MILK";
                            newrow["ITEM CODE"] = "11120003";
                        }
                        newrow["LEDGER CODE"] = dr["sapcode"].ToString();
                        newrow["LEDGER TYPE"] = ledgertype;
                        string shortname = dr["shortname"].ToString();
                        string dcno = dr["dc_no"].ToString();
                        string cell = dr["cell"].ToString();
                        newrow["REFRENCE NO"] = dcno + '-' + cell + '-' + shortname;
                        newrow["CUSTOMER CODE"] = dr["sapvendorcode"].ToString();
                        newrow["CUSTOMER NAME"] = vendername;
                        newrow["FRIGHT CODE"] = dr["purchaseohcode"].ToString();
                        newrow["FRIGHT TYPE"] = tallyoh;
                        double ltrcost = diffvalue / Kgs;
                        double rateltrcost = Math.Round(ltrcost, 2);
                        double diffrateperltr = ltrcost - rateltrcost;
                        double diffamount = diffrateperltr * Kgs;
                        newrow["Diff Amount"] = Math.Round(diffamount, 2);

                        newrow["RATE PER LTR"] = ltrcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["dc_no"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " , " + " KG fat Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        newrow["salestype"] = dr["purchasetype"].ToString();
                        newrow["DC NUMBER"] = newdcno;
                        newrow["FAT"] = FAT;
                        newrow["SNF"] = SNF;
                        newrow["CLR"] = CLR;
                        //jv format
                        double jvdiffammount = Math.Round(diffamount, 2);
                        jvnewrow["JV No"] = "jv-" + dcno + '-' + cell + '-' + shortname;
                        DateTime jvDate = Convert.ToDateTime(dtdoe);
                        string jdates = jvDate.ToString("MM/dd/yyyy");
                        jvnewrow["JV Date"] = jdates;
                        jvnewrow["Wh Code"] = dr["whcode"].ToString();
                        jvnewrow["Ledger Code"] = dr["sapcode"].ToString();
                        jvnewrow["Ledger Name"] = ledgertype;
                        jvnewrow["Amount"] = jvdiffammount;
                        jvnewrow["Narration"] = dcnaration;
                        jvreport.Rows.Add(jvnewrow);
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
                    grdjv.DataSource = jvreport;
                    grdjv.DataBind();
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
                Report.Columns.Add("WH CODE");
                Report.Columns.Add("CUSTOMER CODE");
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("REFRENCE NO");
                Report.Columns.Add("TAX DATE");
                Report.Columns.Add("DOC DATE");
                Report.Columns.Add("DOC DUE DATE");
                Report.Columns.Add("LEDGER CODE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM CODE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("Diff Amount");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("FRIGHT TYPE");
                Report.Columns.Add("FRIGHT CODE");
                Report.Columns.Add("FRIGHT AMOUNT");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("DC NUMBER");
                Report.Columns.Add("Narration");
                Report.Columns.Add("salestype");

                jvreport.Columns.Add("JV No");
                jvreport.Columns.Add("JV Date");
                jvreport.Columns.Add("Wh Code");
                jvreport.Columns.Add("Ledger Code");
                jvreport.Columns.Add("Ledger Name");
                jvreport.Columns.Add("Amount");
                jvreport.Columns.Add("Narration");
                if (ddlBranchwisepurchase.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT  vendor_subtable.overheadon, despatch_entry.invoiceno,vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost,  vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc,  vendors.tallyoh, vendors.purchasetype, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on,  directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.cell, directsale.veh_sno AS vehicleno,  branch_info.whcode, vendors.sapcustomercode, vendors.customername, vendors.purchaseohcode, vendors.salesohcode, vendors.sapvendorcode,  vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1 FROM  directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno = directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.branchid = @dbranchid) AND (directsale.milktype = 'Cow') AND (directsale.veh_sno = @vehicleno) ORDER BY directsale.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@vehicleno", ddlBranchwisepurchase.SelectedValue);
                    cmd.Parameters.Add("@dbranchid", BranchID);
                }
                else if (ddlBranchwisepurchase.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("SELECT   vendor_subtable.overheadon, despatch_entry.invoiceno,vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost,  vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc,  vendors.tallyoh, vendors.purchasetype, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on,  directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.cell, directsale.veh_sno AS vehicleno,  vendors.sno, branch_info.whcode, vendors.sapcustomercode, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.sapvendorcode,  vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1 FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno = directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.branchid = @bbranchid) AND (directsale.milktype = 'Cow') AND (directsale.fromccid = @sectionid) ORDER BY directsale.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranchespurchase.SelectedValue);
                    cmd.Parameters.Add("@bbranchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("SELECT  vendor_subtable.overheadon,despatch_entry.invoiceno, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost,  vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc,  vendors.tallyoh, vendors.purchasetype, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on,  directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.cell, directsale.veh_sno AS vehicleno,  vendors.sno, branch_info.whcode, vendors.sapcustomercode, vendors.customername, vendors.salesohcode, vendors.purchaseohcode, vendors.salesledgercode,  vendors.sapvendorcode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode, vendors.ledgertype1 FROM  directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno = directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.branchid = @ebranchid) AND (directsale.milktype = 'Cow') AND (directsale.fromccid = @sectionid) ORDER BY directsale.doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranchespurchase.SelectedValue);
                    cmd.Parameters.Add("@ebranchid", BranchID);
                }
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
                        string newdcno1 = dr["dc_no"].ToString();
                        DataTable dtin = new DataTable();
                        DataRow[] drr = dtDispatch.Select("dc_no='" + newdcno1 + "'");
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
                            dcnaration = "Being PURCHASE Of Milk from " + ddlbranchespurchase.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DataRow jvnewrow = jvreport.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        newrow["DOC DATE"] = dr["doe"].ToString();
                        newrow["DOC DUE DATE"] = dr["doe"].ToString();
                        newrow["TAX DATE"] = dr["doe"].ToString();
                        string date = dtdoe.ToString("dd-MMM-yyyy");
                        double qty_kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                        double qty_ltr = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                        newrow["QTY KG'S/LTRS"] = dr["qty_ltr"].ToString();
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        string CLR = dr["clr"].ToString();
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
                        double.TryParse(dr["rate"].ToString(), out cost);
                        double OHcost = 0;
                        double overheadcost = 0;
                        double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                        string OverheadOn = dr["overheadon"].ToString();
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
                        KGFAT = Math.Round(KGFAT, 2);
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
                        kgsnftotal += KGSNF;
                        double MValue = 0;
                        MValue = tstotal * qty_ltr * cost;
                        MValue = MValue / 100;
                        MValue = Math.Round(MValue, 2);
                        mvaluetotal += MValue;
                        double OHandMvalue = 0;
                        OHandMvalue = MValue;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        milkvaluetotal += OHandMvalue;
                        double OMILKVALUE = 0;
                        OMILKVALUE = MValue + OHcost + DiffSNFCost;
                        newrow["NET VALUE"] = OMILKVALUE;
                        double diffvalue = OMILKVALUE - OHandMvalue;
                        milkvaluetotal += OHandMvalue;
                        if (ddlbranchTypepurchase.SelectedValue == "Other Branches")
                        {
                            newrow["ITEM NAME"] = "COW MILK Others";
                            newrow["ITEM CODE"] = "51410002";
                        }
                        else
                        {
                            newrow["ITEM NAME"] = "COW MILK";
                            newrow["ITEM CODE"] = "11120002";
                        }
                        string shortname = dr["shortname"].ToString();
                        string dcno = dr["dc_no"].ToString();
                        string celltype = dr["cell"].ToString();
                        newrow["REFRENCE NO"] = dcno + '-' + celltype + '-' + shortname;
                        string cn = "SVDS.P.LTD";
                        string plant = "Pbk";
                        string vendername = dr["fromcc"].ToString();
                        string tallyoh = dr["tallyoh"].ToString(); ;
                        string ledgertype = dr["ledgertype"].ToString();
                        newrow["LEDGER CODE"] = dr["sapcode"].ToString();
                        newrow["LEDGER TYPE"] = ledgertype;
                        newrow["CUSTOMER CODE"] = dr["sapvendorcode"].ToString();
                        newrow["CUSTOMER NAME"] = vendername;
                        newrow["FRIGHT CODE"] = dr["purchaseohcode"].ToString();
                        newrow["FRIGHT TYPE"] = tallyoh;
                        double ledgeramt=OMILKVALUE - OHcost;
                        newrow["LEDGER AMOUNT"] = Math.Round(ledgeramt, 2);
                        double ltrcost = ledgeramt / Kgs;
                        double rateltrcost = Math.Round(ltrcost, 2);
                        double diffrateperltr = ltrcost - rateltrcost;
                        double diffamount = diffrateperltr * Kgs;
                        newrow["Diff Amount"] = Math.Round(diffamount, 2);
                        newrow["RATE PER LTR"] = rateltrcost;
                        double ledgeramount = 0;
                        ledgeramount = OHandMvalue - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        OHcost = Math.Round(OHcost, 0);
                        newrow["FRIGHT AMOUNT"] = OHcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["dc_no"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " ," + " TS Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        
                        newrow["salestype"] = dr["purchasetype"].ToString();
                        newrow["FAT"] = FAT;
                        newrow["SNF"] = SNF;
                        newrow["CLR"] = CLR;
                        newrow["DC NUMBER"] = newdcno;
                        //jv format
                        double jvdiffammount = Math.Round(diffamount, 2);
                        jvnewrow["JV No"] = "jv-" + dcno + '-' + celltype + '-' + shortname;
                        DateTime jvDate = Convert.ToDateTime(dtdoe);
                        string jdates = jvDate.ToString("MM/dd/yyyy");
                        jvnewrow["JV Date"] = jdates;
                        jvnewrow["Wh Code"] = dr["whcode"].ToString();
                        jvnewrow["Ledger Code"] = dr["sapcode"].ToString();
                        jvnewrow["Ledger Name"] = ledgertype;
                        jvnewrow["Amount"] = jvdiffammount;
                        jvnewrow["Narration"] = dcnaration;
                        jvreport.Rows.Add(jvnewrow);
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
                    grdjv.DataSource = jvreport;
                    grdjv.DataBind();
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
        catch
        {
        }
    }

    void btn_Save_Purchasedirect_Click()
    {
        try
        {
            SalesDBManager SalesDB = new SalesDBManager();
            DateTime CreateDate = sapDBmanager.GetTime(vdm.conn);
            sapDBmanager SAPvdm = new sapDBmanager();
            DateTime fromdate = DateTime.Now;
            DateTime ServerDateCurrentdate = sapDBmanager.GetTime(vdm.conn);
            string datetime = ServerDateCurrentdate.ToString("MM/dd/yyyy");
            string BranchID = Session["Branch_ID"].ToString();
            DataTable dt = (DataTable)Session["xportdata"];
            if (dt.Rows.Count > 0)
            {
                DateTime doe = DateTime.Now;
                foreach (DataRow dr in dt.Rows)
                {
                    string DocDate = dr["DOC DATE"].ToString();
                    DateTime dDate = Convert.ToDateTime(DocDate);
                    cmd = new SqlCommand("SELECT CardCode, CardName FROM EMROPCH WHERE DocDate BETWEEN @d1 and @d2 AND ReferenceNo=@rno");
                    cmd.Parameters.Add("@d1", GetLowDate(dDate));
                    cmd.Parameters.Add("@d2", GetHighDate(dDate));
                    cmd.Parameters.Add("@rno", dr["REFRENCE NO"].ToString());
                    DataTable dtSAPpurchase = SAPvdm.SelectQuery(cmd).Tables[0];
                    if (dtSAPpurchase.Rows.Count > 0)
                    {
                        lblmsg.Text = "This date data already Saved";
                    }
                    else
                    {
                        cmd = new SqlCommand("Insert into EMROPCH (CreateDate, CardCode, CardName, TaxDate, DocDate, DocDueDate, ReferenceNo, GRNRefNo, ItemCode, Dscription, WhsCode, Quantity, Price, VAT_Percent, Quantity_Kgs, SNF, FAT, CLR, REMARKS, DOE, MILKTYPE, PARTYDCNO, ENTRYDATE, PURCHASETYPE, B1Upload, Processed, UnitQty, UnitCost, BRANCHID, OcrCode, OcrCode2, TaxCode, TaxAmount,Account,OverHeadAmount1) values (@CreateDate,@CardCode,@CardName,@TaxDate,@DocDate,@DocDueDate,@ReferenceNo, @GRNRefNo, @ItemCode, @Dscription, @WhsCode, @Quantity, @Price, @VAT_Percent, @Quantity_Kgs,@SNF,@FAT,@CLR,@REMARKS,@DOE,@MILKTYPE,@PARTYDCNO,@ENTRYDATE,@purchasetype,@B1Upload,@Processed,@UnitQty,@UnitCost,@BRANCHID, @OcrCode, @OcrCode2, @TaxCode, @TaxAmount,@Account,@OverHeadAmount1)");
                        cmd.Parameters.Add("@BRANCHID", BranchID);
                        cmd.Parameters.Add("@CreateDate", datetime);
                        string customercode = dr["CUSTOMER CODE"].ToString();
                        cmd.Parameters.Add("@CardCode", customercode);
                        string customername = dr["CUSTOMER NAME"].ToString();
                        cmd.Parameters.Add("@CardName", customername);

                        string TaxDate = dr["TAX DATE"].ToString();
                        DateTime TDate = Convert.ToDateTime(TaxDate);
                        TaxDate = TDate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@TaxDate", TaxDate);

                        // string DocDate = dr["DOC DATE"].ToString();
                        // DateTime dDate = Convert.ToDateTime(DocDate);
                        DocDate = dDate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@DocDate", DocDate);

                        string DocDueDate = dr["DOC DUE DATE"].ToString();
                        DateTime docdate = Convert.ToDateTime(DocDueDate);
                        DocDueDate = docdate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@DocDueDate", DocDueDate);

                        string refno = dr["REFRENCE NO"].ToString();
                        cmd.Parameters.Add("@ReferenceNo", refno);

                        string ledgertype = dr["LEDGER TYPE"].ToString();
                        int ledgerlength = ledgertype.Length;
                        //  cmd.Parameters.Add("@Dscription", ledgertype);

                        string whcode = dr["WH CODE"].ToString();
                        int whcodelength = ledgertype.Length;
                        cmd.Parameters.Add("@WhsCode", whcode);

                        cmd.Parameters.Add("@OcrCode", whcode);

                        string tTaxCode = "EXEMPT";
                        cmd.Parameters.Add("@TaxCode", tTaxCode);
                        string tTaxAmount = "0";
                        cmd.Parameters.Add("@TaxAmount", tTaxAmount);

                        string raeteperltr = dr["RATE PER LTR"].ToString();
                        cmd.Parameters.Add("@price", raeteperltr);

                        string VAT_Percent = "0";
                        cmd.Parameters.Add("@VAT_Percent", VAT_Percent);

                        string kgs = dr["QTY KG'S/LTRS"].ToString();
                        cmd.Parameters.Add("@UnitQty", kgs);
                        cmd.Parameters.Add("@Quantity", kgs);

                        string netvalue = dr["LEDGER AMOUNT"].ToString();
                        cmd.Parameters.Add("@UnitCost", netvalue);

                        cmd.Parameters.Add("@Quantity_Kgs", kgs);

                        string fat = dr["FAT"].ToString();
                        string snf = dr["SNF"].ToString();
                        string clr = dr["CLR"].ToString();
                        cmd.Parameters.Add("@SNF", snf);
                        cmd.Parameters.Add("@FAT", fat);
                        cmd.Parameters.Add("@CLR", clr);

                        string remarks = dr["Narration"].ToString();
                        cmd.Parameters.Add("@REMARKS", remarks);

                        cmd.Parameters.Add("@DOE", datetime);

                        string itemcode = dr["ITEM CODE"].ToString();
                        cmd.Parameters.Add("@ItemCode", itemcode);
                        string ocrcode2 = "";
                        if (ddlReportType.SelectedValue == "Buffalo")
                        {
                            ocrcode2 = "P0002";
                        }
                        else
                        {
                            ocrcode2 = "P0001";
                        }
                        cmd.Parameters.Add("@OcrCode2", ocrcode2);
                        string itemtype = dr["ITEM NAME"].ToString();
                        cmd.Parameters.Add("@MILKTYPE", itemtype);
                        cmd.Parameters.Add("@Dscription", itemtype);

                        string dcno = dr["DC NUMBER"].ToString();
                        cmd.Parameters.Add("@PARTYDCNO", dcno);

                        cmd.Parameters.Add("@ENTRYDATE", datetime);

                        string ledgertypes = dr["salestype"].ToString();
                        cmd.Parameters.Add("@purchasetype", ledgertypes);

                        string B1Upload = "N";
                        string Processed = "N";
                        cmd.Parameters.Add("@B1Upload", B1Upload);
                        cmd.Parameters.Add("@Processed", Processed);
                        string GRNRefNo = "NA";
                        cmd.Parameters.Add("@GRNRefNo", GRNRefNo);

                        string Account = dr["LEDGER CODE"].ToString();
                        cmd.Parameters.Add("@Account", Account);

                        string famt = dr["FRIGHT AMOUNT"].ToString();
                        cmd.Parameters.Add("@OverHeadAmount1", famt);

                        SAPvdm.insert(cmd);
                    }

                    //cmd = new SqlCommand("Insert into EMROINV (cardcode,cardname,TaxDate, DocDate, DocDueDate,dscription,itemcode,quantity,price,whscode,vat_percent,taxamount,ReferenceNo,TaxCode,B1Upload,Processed,CreateDate,REMARKS,SALETYPE) values(@cardcode,@cardname,@TaxDate,@DocDate,@DocDueDate,@dscription,@itemcode,@quantity,@price,@whscode,@vat_percent,@taxamount,@ReferenceNo,@TaxCode,@B1Upload,@Processed,@CreateDate,@REMARKS,@SALETYPE)");
                }
            }
            DataTable dtempty = new DataTable();
            if (lblmsg.Text == "")
            {
                lblmsg.Text = "Successfully Saved";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }

    public void fillbranches()
    {
        SalesDBManager SalesDB = new SalesDBManager();
        cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors  where branchtype='Other Branch' AND branchid=@Obranchid");
        cmd.Parameters.Add("@Obranchid", BranchID);
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlbranchespurchase.DataSource = dttrips;
        ddlbranchespurchase.DataTextField = "vendorname";
        ddlbranchespurchase.DataValueField = "sno";
        ddlbranchespurchase.DataBind();
    }

    void btn_Generate_Click_other()
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
                Report.Columns.Add("WH CODE");
                Report.Columns.Add("CUSTOMER CODE");
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("REFRENCE NO");
                Report.Columns.Add("TAX DATE");
                Report.Columns.Add("DOC DATE");
                Report.Columns.Add("DOC DUE DATE");
                Report.Columns.Add("LEDGER CODE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM CODE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("Diff Amount");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("FRIGHT TYPE");
                Report.Columns.Add("FRIGHT CODE");
                Report.Columns.Add("FRIGHT AMOUNT");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("DC NUMBER");
                Report.Columns.Add("Narration");
                Report.Columns.Add("salestype");

                jvreport.Columns.Add("JV No");
                jvreport.Columns.Add("JV Date");
                jvreport.Columns.Add("Wh Code");
                jvreport.Columns.Add("Ledger Code");
                jvreport.Columns.Add("Ledger Name");
                jvreport.Columns.Add("Amount");
                jvreport.Columns.Add("Narration");

                cmd = new SqlCommand("SELECT  directsales_purchase.dcno,vendors.shortname, directsales_purchase.cellno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, directsales_purchase.doe,  directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf,  directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost,  directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf,  directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost,  directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname, vendors.ledgertype, vendors.purchasetype,  vendors.tallyoh, vendors.ccname, vendors.sno, branch_info.whcode,branch_info.branchcode, vendors.sapcustomercode, vendors.customername, vendors.purchaseohcode,  vendors.salesohcode, vendors.sapvendorcode, vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode,  vendors.ledgertype1 FROM  directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE (directsales_purchase.entrydate BETWEEN @d1 AND @d2) AND (directsales_purchase.entrytype = @transtype) AND (directsales_purchaselogs.milktype = 'Buffalo')  AND (directsales_purchase.sectionid = @sectionid) ORDER BY directsales_purchase.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "purchase");
                cmd.Parameters.Add("@sectionid", ddlbranchespurchase.SelectedValue);
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
                            dcnaration = "Being sales Of Milk from " + ddlbranchespurchase.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DataRow jvnewrow = jvreport.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        if (ddlbranchTypepurchase.SelectedValue == "Inter Branches")
                        {
                            newrow["WH CODE"] = "SVDSPP02";
                        }
                        else
                        {
                            newrow["WH CODE"] = dr["whcode"].ToString();
                        }
                        newrow["DOC DATE"] = dr["doe"].ToString();
                        newrow["DOC DUE DATE"] = dr["doe"].ToString();
                        newrow["TAX DATE"] = dr["doe"].ToString();

                        string date = dtdoe.ToString("dd-MMM-yyyy");
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
                        string CLR = dr["clr"].ToString();
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
                        KGFAT = Math.Round(KGFAT, 2);
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
                        kgsnftotal += KGSNF;
                        double MValue = 0;
                        MValue = KGFAT * cost;
                        MValue = Math.Round(MValue, 2);
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
                        newrow["FRIGHT AMOUNT"] = OHcost;
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
                            newrow["ITEM CODE"] = "11120003";
                        }

                        //invoiceno + "-" + celltype + "-" + branchcode;
                        string invoiceno = dr["dcno"].ToString();
                        string branchcode = dr["branchcode"].ToString();
                        string cellno = dr["cellno"].ToString();
                        newrow["REFRENCE NO"] = invoiceno + "-" + cellno + "-" + branchcode;
                        newrow["CUSTOMER CODE"] = dr["sapcustomercode"].ToString();
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        newrow["FRIGHT CODE"] = dr["purchaseohcode"].ToString();
                        newrow["FRIGHT TYPE"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        double rateperltrcost = Math.Round(ltrcost, 5);
                        double diffrateperltr = ltrcost - rateperltrcost;
                        double diffamount = diffrateperltr * ltrs;
                        newrow["Diff Amount"] = Math.Round(diffamount, 2);
                        newrow["RATE PER LTR"] = rateperltrcost;
                        double ledgeramount = 0;
                        ledgeramount = ltrcost * qty_ltr;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        double roundvalue = Math.Round(OHandMvalue, 0);
                        newrow["Net VALUE"] = Math.Round(OHandMvalue, 0);
                        newrow["LEDGER CODE"] = dr["sapcode"].ToString();
                        newrow["LEDGER TYPE"] = ledgrtype;
                        newrow["LEDGER AMOUNT"] = roundvalue - OHcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["partydcno"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " , " + " TS Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        newrow["salestype"] = dr["purchasetype"].ToString();
                        newrow["FAT"] = FAT;
                        newrow["SNF"] = SNF;
                        newrow["CLR"] = CLR;
                        newrow["DC NUMBER"] = newdcno;

                        //jv format
                        double jvdiffammount = Math.Round(diffamount, 2);
                        jvnewrow["JV No"] = "jv-" + dr["dcno"].ToString();
                        DateTime jvDate = Convert.ToDateTime(dtdoe);
                        string jdates = jvDate.ToString("MM/dd/yyyy");
                        jvnewrow["JV Date"] = jdates;
                        jvnewrow["Wh Code"] = dr["whcode"].ToString();
                        jvnewrow["Ledger Code"] = dr["sapcode"].ToString();
                        jvnewrow["Ledger Name"] = ledgrtype;
                        jvnewrow["Amount"] = jvdiffammount;
                        jvnewrow["Narration"] = dcnaration;
                        jvreport.Rows.Add(jvnewrow);
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
                Report.Columns.Add("WH CODE");
                Report.Columns.Add("CUSTOMER CODE");
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("REFRENCE NO");
                Report.Columns.Add("TAX DATE");
                Report.Columns.Add("DOC DATE");
                Report.Columns.Add("DOC DUE DATE");
                Report.Columns.Add("LEDGER CODE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM CODE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("Diff Amount");

                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("FRIGHT TYPE");
                Report.Columns.Add("FRIGHT CODE");
                Report.Columns.Add("FRIGHT AMOUNT");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("DC NUMBER");
                Report.Columns.Add("Narration");
                Report.Columns.Add("salestype");

                jvreport.Columns.Add("JV No");
                jvreport.Columns.Add("JV Date");
                jvreport.Columns.Add("Wh Code");
                jvreport.Columns.Add("Ledger Code");
                jvreport.Columns.Add("Ledger Name");
                jvreport.Columns.Add("Amount");
                jvreport.Columns.Add("Narration");
                cmd = new SqlCommand("SELECT   directsales_purchase.dcno,vendors.shortname,branch_info.branchcode, directsales_purchase.cellno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, directsales_purchase.doe,  directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf,  directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost,  directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf,  directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost,  directsales_purchaselogs.m_std_fat, directsales_purchaselogs.p_std_fat, directsales_purchaselogs.m_fatpluscost, directsales_purchaselogs.p_fatpluscost,  directsales_purchaselogs.fatplus_on, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport,  vendors.vendorname, vendors.ledgertype, vendors.tallyoh, vendors.ccname, vendors.sapcustomercode, vendors.customername, vendors.salesohcode,  vendors.purchaseohcode, vendors.sapvendorcode, vendors.purchasetype, vendors.salesledgercode, vendors.salestallyoh, vendors.salesledgertype, vendors.sapcode,  vendors.ledgertype1, vendors.sno, branch_info.whcode FROM  directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno LEFT OUTER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (directsales_purchase.entrytype = @transtype) AND (directsales_purchaselogs.milktype = 'Cow') AND (directsales_purchase.entrydate BETWEEN @d1 AND  @d2) AND (directsales_purchase.sectionid = @sectionid) ORDER BY directsales_purchase.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "purchase");
                cmd.Parameters.Add("@bwbranchid", BranchID);
                cmd.Parameters.Add("@sectionid", ddlbranchespurchase.SelectedValue);
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
                            dcnaration = "Being sales Of Milk from " + ddlbranchespurchase.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DataRow jvnewrow = jvreport.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        if (ddlbranchTypepurchase.SelectedValue == "Inter Branches")
                        {
                            newrow["WH CODE"] = "SVDSPP02";
                        }
                        else
                        {
                            newrow["WH CODE"] = dr["whcode"].ToString();
                        }
                        newrow["TAX DATE"] = dr["doe"].ToString();
                        newrow["DOC DATE"] = dr["doe"].ToString();
                        newrow["DOC DUE DATE"] = dr["doe"].ToString();
                        string date = dtdoe.ToString("dd-MMM-yyyy");
                        double qty_kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                        double qty_ltr = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                        newrow["QTY KG'S/LTRS"] = dr["qty_ltr"].ToString();
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        string CLR = dr["clr"].ToString();
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
                        KGFAT = Math.Round(KGFAT, 2);
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
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
                        ohtotal += OHcost;
                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                        snf9total += DiffSNFCost;
                        double roundvalue = Math.Round(OHandMvalue, 0);
                        newrow["NET VALUE"] = roundvalue;
                        double diffvalue = roundvalue - OHandMvalue;
                        milkvaluetotal += OHandMvalue;
                        if (ddlReportType.SelectedValue == "Cow")
                        {
                            newrow["ITEM NAME"] = "COW MILK";
                            newrow["ITEM CODE"] = "11120002";
                        }
                        //newrow["REFRENCE NO"] = dr["dcno"].ToString();
                        string invoiceno = dr["dcno"].ToString();
                        string branchcode = dr["branchcode"].ToString();
                        string cellno = dr["cellno"].ToString();
                        newrow["REFRENCE NO"] = invoiceno + "-" + cellno + "-" + branchcode;
                        string ledgerccname = "SVDS.P.LTD PUNABAKA PLANT";
                        string cn = "SVDS.P.LTD";
                        string plant = "Pbk";
                        string ccname = dr["ccname"].ToString();
                        string vendername = dr["vendorname"].ToString();
                        string ledgrtype = dr["ledgertype"].ToString();
                        string tallyoh = dr["tallyoh"].ToString();
                        newrow["LEDGER CODE"] = dr["sapcode"].ToString();
                        newrow["LEDGER TYPE"] = ledgrtype;
                        newrow["CUSTOMER CODE"] = dr["sapcustomercode"].ToString();
                        newrow["CUSTOMER NAME"] = vendername;
                        newrow["FRIGHT CODE"] = dr["purchaseohcode"].ToString();
                        newrow["FRIGHT TYPE"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        double roundltrcost = Math.Round(ltrcost, 2);
                        double diffrateperltr = ltrcost - roundltrcost;
                        double diffamount = diffrateperltr * ltrs;
                        newrow["Diff Amount"] = Math.Round(diffamount, 2);
                        newrow["RATE PER LTR"] = roundltrcost;
                        double ledgeramount = 0;
                        ledgeramount = OHandMvalue - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        OHcost = Math.Round(OHcost, 0);
                        newrow["FRIGHT AMOUNT"] = OHcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["dcno"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " ," + " TS Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        newrow["LEDGER AMOUNT"] = roundvalue - OHcost;
                        newrow["salestype"] = dr["purchasetype"].ToString();
                        newrow["FAT"] = FAT;
                        newrow["SNF"] = SNF;
                        newrow["CLR"] = CLR;
                        newrow["DC NUMBER"] = newdcno;

                        //jv format
                        double jvdiffammount = Math.Round(diffamount, 2);
                        jvnewrow["JV No"] = "jv-" + dr["dcno"].ToString();
                        DateTime jvDate = Convert.ToDateTime(dtdoe);
                        string jdates = jvDate.ToString("MM/dd/yyyy");
                        jvnewrow["JV Date"] = jdates;
                        jvnewrow["Wh Code"] = dr["whcode"].ToString();
                        jvnewrow["Ledger Code"] = dr["sapcode"].ToString();
                        jvnewrow["Ledger Name"] = ledgrtype;
                        jvnewrow["Amount"] = jvdiffammount;
                        jvnewrow["Narration"] = dcnaration;
                        jvreport.Rows.Add(jvnewrow);
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
                    grdjv.DataSource = jvreport;
                    grdjv.DataBind();
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

    void btn_Save_Purchaseotherparty_Click()
    {
        try
        {
            SalesDBManager SalesDB = new SalesDBManager();
            DateTime CreateDate = sapDBmanager.GetTime(vdm.conn);
            sapDBmanager SAPvdm = new sapDBmanager();
            DateTime fromdate = DateTime.Now;
            DateTime ServerDateCurrentdate = sapDBmanager.GetTime(vdm.conn);
            string datetime = ServerDateCurrentdate.ToString("MM/dd/yyyy");
            string BranchID = Session["Branch_ID"].ToString();
            DataTable dt = (DataTable)Session["xportdata"];
            if (dt.Rows.Count > 0)
            {
                DateTime doe = DateTime.Now;
                foreach (DataRow dr in dt.Rows)
                {
                    string DocDate = dr["DOC DATE"].ToString();
                    DateTime dDate = Convert.ToDateTime(DocDate);
                    cmd = new SqlCommand("SELECT CardCode, CardName FROM EMROPCH WHERE DocDate BETWEEN @d1 and @d2 AND ReferenceNo=@rno");
                    cmd.Parameters.Add("@d1", GetLowDate(dDate));
                    cmd.Parameters.Add("@d2", GetHighDate(dDate));
                    cmd.Parameters.Add("@rno", dr["REFRENCE NO"].ToString());
                    DataTable dtSAPpurchase = SAPvdm.SelectQuery(cmd).Tables[0];
                    if (dtSAPpurchase.Rows.Count > 0)
                    {
                        lblmsg.Text = "This date data already Saved";
                    }
                    else
                    {
                        cmd = new SqlCommand("Insert into EMROPCH (CreateDate, CardCode, CardName, TaxDate, DocDate, DocDueDate, ReferenceNo, GRNRefNo, ItemCode, Dscription, WhsCode, Quantity, Price, VAT_Percent, Quantity_Kgs, SNF, FAT, CLR, REMARKS, DOE, MILKTYPE, PARTYDCNO, ENTRYDATE, PURCHASETYPE, B1Upload, Processed, UnitQty, UnitCost, BRANCHID, OcrCode, OcrCode2, TaxCode, TaxAmount,Account,OverHeadAmount1) values (@CreateDate,@CardCode,@CardName,@TaxDate,@DocDate,@DocDueDate,@ReferenceNo, @GRNRefNo, @ItemCode, @Dscription, @WhsCode, @Quantity, @Price, @VAT_Percent, @Quantity_Kgs,@SNF,@FAT,@CLR,@REMARKS,@DOE,@MILKTYPE,@PARTYDCNO,@ENTRYDATE,@purchasetype,@B1Upload,@Processed,@UnitQty,@UnitCost,@BRANCHID, @OcrCode, @OcrCode2, @TaxCode, @TaxAmount,@Account,@OverHeadAmount1)");
                        cmd.Parameters.Add("@BRANCHID", BranchID);
                        cmd.Parameters.Add("@CreateDate", datetime);
                        string customercode = dr["CUSTOMER CODE"].ToString();
                        cmd.Parameters.Add("@CardCode", customercode);
                        string customername = dr["CUSTOMER NAME"].ToString();
                        cmd.Parameters.Add("@CardName", customername);

                        string TaxDate = dr["TAX DATE"].ToString();
                        DateTime TDate = Convert.ToDateTime(TaxDate);
                        TaxDate = TDate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@TaxDate", TaxDate);

                        // string DocDate = dr["DOC DATE"].ToString();
                        // DateTime dDate = Convert.ToDateTime(DocDate);
                        DocDate = dDate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@DocDate", DocDate);

                        string DocDueDate = dr["DOC DUE DATE"].ToString();
                        DateTime docdate = Convert.ToDateTime(DocDueDate);
                        DocDueDate = docdate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@DocDueDate", DocDueDate);

                        string refno = dr["REFRENCE NO"].ToString();
                        cmd.Parameters.Add("@ReferenceNo", refno);

                        string ledgertype = dr["LEDGER TYPE"].ToString();
                        int ledgerlength = ledgertype.Length;
                        //cmd.Parameters.Add("@Dscription", ledgertype);

                        string whcode = dr["WH CODE"].ToString();
                        int whcodelength = ledgertype.Length;
                        cmd.Parameters.Add("@WhsCode", whcode);

                        cmd.Parameters.Add("@OcrCode", whcode);

                        string tTaxCode = "EXEMPT";
                        cmd.Parameters.Add("@TaxCode", tTaxCode);
                        string tTaxAmount = "0";
                        cmd.Parameters.Add("@TaxAmount", tTaxAmount);

                        string raeteperltr = dr["RATE PER LTR"].ToString();
                        cmd.Parameters.Add("@price", raeteperltr);

                        string VAT_Percent = "0";
                        cmd.Parameters.Add("@VAT_Percent", VAT_Percent);

                        string kgs = dr["QTY KG'S/LTRS"].ToString();
                        cmd.Parameters.Add("@UnitQty", kgs);
                        cmd.Parameters.Add("@Quantity", kgs);

                        string netvalue = dr["LEDGER AMOUNT"].ToString();
                        cmd.Parameters.Add("@UnitCost", netvalue);

                        cmd.Parameters.Add("@Quantity_Kgs", kgs);

                        string fat = dr["FAT"].ToString();
                        string snf = dr["SNF"].ToString();
                        string clr = dr["CLR"].ToString();
                        cmd.Parameters.Add("@SNF", snf);
                        cmd.Parameters.Add("@FAT", fat);
                        cmd.Parameters.Add("@CLR", clr);

                        string remarks = dr["Narration"].ToString();
                        cmd.Parameters.Add("@REMARKS", remarks);

                        cmd.Parameters.Add("@DOE", datetime);

                        string itemcode = dr["ITEM CODE"].ToString();
                        cmd.Parameters.Add("@ItemCode", itemcode);
                        string ocrcode2 = "";
                        if (ddlReportType.SelectedValue == "Buffalo")
                        {
                            ocrcode2 = "P0002";
                        }
                        else
                        {
                            ocrcode2 = "P0001";
                        }
                        cmd.Parameters.Add("@OcrCode2", ocrcode2);
                        string itemtype = dr["ITEM NAME"].ToString();
                        cmd.Parameters.Add("@MILKTYPE", itemtype);
                        cmd.Parameters.Add("@Dscription", itemtype);

                        string dcno = dr["DC NUMBER"].ToString();
                        cmd.Parameters.Add("@PARTYDCNO", dcno);

                        cmd.Parameters.Add("@ENTRYDATE", datetime);

                        string ledgertypes = dr["salestype"].ToString();
                        cmd.Parameters.Add("@purchasetype", ledgertypes);

                        string B1Upload = "N";
                        string Processed = "N";
                        cmd.Parameters.Add("@B1Upload", B1Upload);
                        cmd.Parameters.Add("@Processed", Processed);
                        string GRNRefNo = "NA";
                        cmd.Parameters.Add("@GRNRefNo", GRNRefNo);

                        string Account = dr["LEDGER CODE"].ToString();
                        cmd.Parameters.Add("@Account", Account);

                        string famt = dr["FRIGHT AMOUNT"].ToString();
                        cmd.Parameters.Add("@OverHeadAmount1", famt);
                        SAPvdm.insert(cmd);
                    }
                }
            }
            DataTable dtempty = new DataTable();
            if (lblmsg.Text == "")
            {
                lblmsg.Text = "Successfully Saved";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }
}