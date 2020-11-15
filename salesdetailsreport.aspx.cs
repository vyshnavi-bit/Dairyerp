using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class salesdetailsreport : System.Web.UI.Page
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
                    dtp_FromDatesale.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    dtp_Todatesale.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    //dtp_FromDatedirect.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    //dtp_Todatedirect.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    //dtp_FromDateother.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    //dtp_Todateother.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesDBManager SalesDB = new SalesDBManager();
        if (ddltype.SelectedValue == "Sales Voucher")
        {
            hiddentype.Visible = true;
            hiddenbrances.Visible = true;
            hiddenwise.Visible = true;
            hideVehicles.Visible = true;
            hideVehicles.Visible = false;
        }
        if (ddltype.SelectedValue == "Direct Sales Voucher")
        {
            hiddentype.Visible = false;
            hiddenbrances.Visible = true;
            hiddenwise.Visible = true;
            hideVehicles.Visible = true;
            hideVehicles.Visible = false;
        }
        if (ddltype.SelectedValue == "Other Party Direct Sales Voucher")
        {
            hiddentype.Visible = false;
            hiddenbrances.Visible = false;
            hiddenwise.Visible = false;
            hideVehicles.Visible = false;
            hideVehicles.Visible = true;
            fillbranches();
        }
    }
    protected void ddlBranchsale_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesDBManager SalesDB = new SalesDBManager();
        if (ddlBranchsale.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
        }
        if (ddlBranchsale.SelectedValue == "Branch Wise")
        {
            hideVehicles.Visible = true;
            if (ddlbranchTypesale.SelectedValue == "All")
            {
                cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors where branchid=@vbranchid");
                cmd.Parameters.Add("@vbranchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranchessale.DataSource = dttrips;
                ddlbranchessale.DataTextField = "vendorname";
                ddlbranchessale.DataValueField = "sno";
                ddlbranchessale.DataBind();
            }
            if (ddlbranchTypesale.SelectedValue == "Inter Branches")
            {
                cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors where branchtype='Inter Branch' AND branchid=@Ibranchid ");
                cmd.Parameters.Add("@Ibranchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranchessale.DataSource = dttrips;
                ddlbranchessale.DataTextField = "vendorname";
                ddlbranchessale.DataValueField = "sno";
                ddlbranchessale.DataBind();
            }
            if (ddlbranchTypesale.SelectedValue == "Other Branches")
            {
                cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors  where branchtype='Other Branch' AND branchid=@Obranchid");
                cmd.Parameters.Add("@Obranchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranchessale.DataSource = dttrips;
                ddlbranchessale.DataTextField = "vendorname";
                ddlbranchessale.DataValueField = "sno";
                ddlbranchessale.DataBind();
            }
        }
        if (ddlBranchsale.SelectedValue == "Vehicle Wise")
        {
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT sno,vehicleno,capacity,noofqty FROM vehicle_master where branchid=@vmbranchid");
            cmd.Parameters.Add("@vmbranchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranchessale.DataSource = dttrips;
            ddlbranchessale.DataTextField = "vehicleno";
            ddlbranchessale.DataValueField = "vehicleno";
            ddlbranchessale.DataBind();
        }
    }
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        if (ddltype.SelectedValue == "Sales Voucher")
        {
            btn_Generate_Click_sale();
        }
        if (ddltype.SelectedValue == "Direct Sales Voucher")
        {
            btn_Generate_Click_direct(); 
        }
        if (ddltype.SelectedValue == "Other Party Direct Sales Voucher")
        {
            btn_Generate_Click_other();
        }
    }
    void btn_Generate_Click_sale()
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
            string[] datestrig = dtp_FromDatesale.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = dtp_Todatesale.Text.Split(' ');
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

            if (ddlReportTypesale.SelectedValue == "Buffalo")
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
                //Report.Columns.Add("ROUNDING OFF");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("Narration");
                if (ddlBranchsale.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND (milktransactions.vehicleno=@vehicleno) AND (milktransactions.branchid=@ebranchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@ebranchid", BranchID);
                    cmd.Parameters.Add("@vehicleno", ddlbranchessale.SelectedValue);
                }
                else if (ddlBranchsale.SelectedValue == "Branch Wise")
                {
                    if (ddltankertypesale.SelectedValue == "in")
                    {
                        cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.sectionid=@sectionid) AND (milktransactions.branchid=@fbranchid) ORDER BY doe");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "in");
                        cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
                        cmd.Parameters.Add("@fbranchid", BranchID);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno LEFT OUTER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.sectionid=@sectionid) AND (milktransactions.branchid=@fbranchid) ORDER BY doe");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "Out");
                        cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
                        cmd.Parameters.Add("@fbranchid", BranchID);
                    }

                }
                else
                {
                    cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf,milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.branchid=@gbranchid) ORDER BY doe");
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
                            dcnaration = "Being sales Of Milk from " + ddlbranchessale.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
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
                        string ledgerccname = "Sri Vyshnavi Dairy Specialities Pvt Ltd.,";
                        string celltype = dr["cellno"].ToString();
                        string branchcode = dr["branchcode"].ToString();
                        string invoiceno = dr["partydcno"].ToString();
                        if (ddlbranchTypesale.SelectedItem.Text == "Other Branches")
                        {
                            ledgerccname = vendername;
                            newrow["ITEM NAME"] = "WHOLE MILK Others";
                            ledgertype = dr["ledgertype"].ToString();
                        }
                        else
                        {
                            ledgerccname = "Sri Vyshnavi Dairy Specialities Pvt Ltd.,";
                            newrow["ITEM NAME"] = "WHOLE MILK";
                        }
                        if (vendername == "SVDS.P.LTD KALIGIRI")
                        {
                            ledgerccname = "SVDS.P.LTD PUNABAKA PLANT";
                        }
                        milkvaluetotal += OHandMvalue;
                        if (ddlReportTypesale.SelectedValue == "Buffalo")
                        {

                        }
                        newrow["LEDGER TYPE"] = ledgertype;
                        newrow["INVOICE NO"] = invoiceno + "-" + celltype + "-" + branchcode;
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        //newrow["LEDGER TYPE"] = ledgertype;
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
            else if (ddlReportTypesale.SelectedValue == "Cow" || ddlReportTypesale.SelectedValue == "Skim" || ddlReportTypesale.SelectedValue == "Condensed")
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
                if (ddlBranchsale.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh,  branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND ( milktransactions.vehicleno=@vehicleno) AND (milktransactions.branchid=@hbranchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@hbranchid", BranchID);
                    cmd.Parameters.Add("@vehicleno", ddlbranchessale.SelectedValue);
                }
                else if (ddlBranchsale.SelectedValue == "Branch Wise")
                {

                    if (ddltankertypesale.SelectedValue == "in")
                    {
                        cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, vendors.salesledgertype, vendors.ccname, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salestallyoh,  branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.sectionid=@sectionid) AND (milktransactions.branchid=@bwbranchid) ORDER BY doe");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "in");
                        cmd.Parameters.Add("@bwbranchid", BranchID);
                        cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, vendors.vendorname, vendors.salesledgertype, vendors.ccname, vendors.tallyoh, vendors.ledgertype, vendors.salestallyoh, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.sectionid=@sectionid)  ORDER BY doe");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@transtype", "Out");
                        cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
                    }
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_fatpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype, vendors.salesledgertype, vendors.ccname, vendors.salestallyoh, branch_info.branchcode, milktransactions.cellno FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.branchid=@kbranchid) ORDER BY doe");
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
                            dcnaration = "Being sales Of Milk from " + ddlbranchessale.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        else
                        {
                            dcnaration = "Being sales Of Milk from " + ddlbranchessale.SelectedItem.Text + "," + "Dcno" + dr["dcno"] + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
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
                        if (ddltankertypesale.SelectedValue == "in")
                        {
                            branchcode = dr["branchcode"].ToString();
                            invoiceno = dr["partydcno"].ToString();
                        }
                        else
                        {
                            invoiceno = dr["dcno"].ToString();
                        }
                        string ledgerccname = "SVDS.P.LTD PUNABAKA PLANT";
                        if (ddlbranchTypesale.SelectedItem.Text == "Other Branches")
                        {
                            ledgerccname = vendername;
                            newrow["ITEM NAME"] = "COW MILK Others";
                            ledgertype = dr["ledgertype"].ToString();
                            newrow["LEDGER TYPE"] = ledgertype;
                        }
                        else
                        {
                            ledgerccname = "SVDS.P.LTD PUNABAKA PLANT";
                            newrow["ITEM NAME"] = "COW MILK";
                        }

                        newrow["LEDGER TYPE"] = ledgertype;
                        newrow["INVOICE NO"] = invoiceno + "-" + celltype + "-" + branchcode;
                        newrow["CUSTOMER NAME"] = ledgerccname;
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
    //protected void ddlBranchdirect_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    SalesDBManager SalesDB = new SalesDBManager();
    //    if (ddlBranchdirect.SelectedValue == "All")
    //    {
    //        hideVehiclesdirect.Visible = false;
    //    }
    //    if (ddlBranchdirect.SelectedValue == "Branch Wise")
    //    {
    //        hideVehiclesdirect.Visible = true;
    //        if (ddlbranchTypedirect.SelectedValue == "All")
    //        {
    //            cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors where (branchtype='Inter Branch') AND (branchid=@vbranchid)");
    //            cmd.Parameters.Add("@vbranchid", BranchID);
    //            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
    //            ddlbranchesdirect.DataSource = dttrips;
    //            ddlbranchesdirect.DataTextField = "vendorname";
    //            ddlbranchesdirect.DataValueField = "sno";
    //            ddlbranchesdirect.DataBind();
    //        }
    //        if (ddlbranchTypedirect.SelectedValue == "Inter Branches")
    //        {
    //            cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors where branchtype='Inter Branch' AND (branchid=@Ibranchid)");
    //            cmd.Parameters.Add("@Ibranchid", BranchID);
    //            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
    //            ddlbranchesdirect.DataSource = dttrips;
    //            ddlbranchesdirect.DataTextField = "vendorname";
    //            ddlbranchesdirect.DataValueField = "sno";
    //            ddlbranchesdirect.DataBind();
    //        }
    //        if (ddlbranchTypedirect.SelectedValue == "Other Branches")
    //        {
    //            cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors  where branchtype='Other Branch' AND (branchid=@Obranchid)");
    //            cmd.Parameters.Add("@Obranchid", BranchID);
    //            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
    //            ddlbranchesdirect.DataSource = dttrips;
    //            ddlbranchesdirect.DataTextField = "vendorname";
    //            ddlbranchesdirect.DataValueField = "sno";
    //            ddlbranchesdirect.DataBind();
    //        }
    //    }
    //    if (ddlBranchdirect.SelectedValue == "Vehicle Wise")
    //    {
    //        hideVehiclesdirect.Visible = true;
    //        cmd = new SqlCommand("SELECT sno,vehicleno,capacity,noofqty FROM vehicle_master where branchid=@vebranchid");
    //        cmd.Parameters.Add("@vebranchid", BranchID);
    //        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
    //        ddlbranchesdirect.DataSource = dttrips;
    //        ddlbranchesdirect.DataTextField = "vehicleno";
    //        ddlbranchesdirect.DataValueField = "vehicleno";
    //        ddlbranchesdirect.DataBind();
    //    }
    //}
    //void btn_Generate_Click_direct()
    //{
    //    try
    //    {
    //        directsale();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblmsg.Text = ex.Message;
    //        hidepanel.Visible = false;
    //    }
    //}
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
            string[] datestrig = dtp_FromDatesale.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = dtp_Todatesale.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }

            if (ddlReportTypesale.SelectedValue == "Buffalo")
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
                if (ddlBranchsale.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype,  vendors.shortname, directsale.cell, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.veh_sno = @vehicleno) AND (directsale.branchid=@vbranchid) AND directsale.milktype='Buffalo' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@vehicleno", ddlbranchessale.SelectedValue);
                    cmd.Parameters.Add("@vbranchid", BranchID);
                }
                else if (ddlBranchsale.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.cell, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @sectionid) AND (directsale.branchid=@bbranchid) AND directsale.milktype='Buffalo' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
                    cmd.Parameters.Add("@bbranchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.cell, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @sectionid) AND (directsale.branchid=@mbranchid) AND directsale.milktype='Buffalo' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
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
                            dcnaration = "Being Sale Of Milk from " + ddlbranchessale.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
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
                        //newrow["TS RATE"] = cost;
                        KGFAT = Math.Round(KGFAT, 2);
                        // newrow["KG FAT"] = KGFAT;
                        kgfattotal += KGFAT;
                        double MValue = 0;
                        MValue = KGFAT * cost;
                        //MValue = MValue / 100;
                        MValue = Math.Round(MValue, 2);
                        //newrow["M VALUE"] = MValue;
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
                        newrow["LEDGER AMOUNT1"] = OHcost;
                        double diffvalue = roundvalue - OHcost;
                        ohtotal += OHcost;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        double netvalue = OHandMvalue + OHcost + DiffSNFCost;
                        newrow["Net VALUE"] = Math.Round(netvalue, 0);
                        newrow["LEDGER AMOUNT"] = Math.Round((netvalue - OHcost), 0);
                        string cn = "SRI VYSHNAVI DAIRY PVT LTD";
                        string ledgerccname = "Sri Vyshnavi Dairy Specialities Pvt Ltd.,";
                        string vendername = dr["fromcc"].ToString();
                        string ledgertype = dr["ledgertype"].ToString();
                        string tallyoh = dr["tallyoh"].ToString();
                        milkvaluetotal += OHandMvalue;
                        if (ddlbranchTypesale.SelectedValue == "Other Branches")
                        {
                            newrow["ITEM NAME"] = "WHOLE MILK Others";
                            ledgerccname = vendername;
                        }
                        else
                        {
                            newrow["ITEM NAME"] = "WHOLE MILK";

                            if (vendername == "SRI VYSHNAVI DAIRY PVT LTD, GUDLURU")
                            {
                                tallyoh = "Over Head Gudluru";
                                ledgertype = "Sale of Milk-GUDLURU";
                                ledgerccname = "SRI VYSHNAVI FOODS PVT LTD.";
                                //ledgerccname = cn + "," + ccname;
                            }

                            if (vendername == "Sri Vyshnavi Dairy Pvt Ltd, C.S.Puram")
                            {
                                tallyoh = "Over Head C.S.Puram";
                                ledgertype = "Sale of Milk-C.S.Puram";
                                // ledgerccname = cn + "," + ccname;

                            }
                            if (vendername == "Sri Vyshnavi Dairy Pvt Ltd, Kondepi")
                            {
                                tallyoh = "Over Head Kondepi";
                                ledgertype = "Sale of Milk-Kdp";
                                //ledgerccname = cn + "," + ccname;
                            }
                            if (vendername == "Sri Vyshnavi Dairy Pvt Ltd., Kavali")
                            {
                                tallyoh = "Over Head Kavali";
                                ledgertype = "Sale of Milk-Kavali";

                            }
                            if (vendername == "SVDS.P.LTD KALIGIRI")
                            {
                                ledgerccname = "SVDS.P.LTD PUNABAKA PLANT";
                                tallyoh = "Over Head Kaligiri";
                                ledgertype = "Sale of buff Milk Inter Branches-Kaligir";
                            }
                            if (vendername == "SRI VYSHNAVI DAIRY PVT LTD-G.D.PADU")
                            {
                                tallyoh = "Over Head G.D.Padu";
                                ledgertype = "Sale of Milk-Gudipallipadu";
                            }
                        }
                        newrow["LEDGER TYPE"] = ledgertype;
                        string dcno = dr["dc_no"].ToString();
                        string celltype = dr["cell"].ToString();
                        string shortname = dr["shortname"].ToString();
                        newrow["INVOICE NO"] = dcno + '-' + celltype + '-' + shortname;
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        newrow["LEDGER TYPE1"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 5);
                        newrow["RATE PER LTR"] = ltrcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["dc_no"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " , " + " KG fat Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
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
            else if (ddlReportTypesale.SelectedValue == "Cow" || ddlReportTypesale.SelectedValue == "Skim" || ddlReportTypesale.SelectedValue == "Condensed")
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
                if (ddlBranchsale.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.cell, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.veh_sno = @vehicleno) AND (directsale.branchid=@dbranchid) AND directsale.milktype='Cow' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@vehicleno", ddlbranchessale.SelectedValue);
                    cmd.Parameters.Add("@dbranchid", BranchID);
                }
                else if (ddlBranchsale.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc,  directsale.cell, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @sectionid) AND (directsale.branchid=@bbranchid) AND directsale.milktype='Cow' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
                    cmd.Parameters.Add("@bbranchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("SELECT vendor_subtable.overheadon, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc,  directsale.cell, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @sectionid) AND (directsale.branchid=@ebranchid) AND directsale.milktype='Cow' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
                    cmd.Parameters.Add("@ebranchid", BranchID);
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
                            dcnaration = "Being Sale Of Milk from " + ddlbranchessale.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
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
                        //newrow["DC NO"] = dr["dcno"].ToString();
                        double OMILKVALUE = 0;
                        OMILKVALUE = MValue + OHcost + DiffSNFCost;
                        //newrow["TS RATE"] = cost;
                        newrow["NET VALUE"] = OMILKVALUE;
                        double diffvalue = OMILKVALUE - OHandMvalue;
                        // newrow["ROUNDING OFF"] = Math.Round(diffvalue, 2);
                        milkvaluetotal += OHandMvalue;
                        string ledgertype = dr["ledgertype"].ToString();
                        string customername = "SVDS.P.LTD PUNABAKA PLANT";
                        string vendername = dr["fromcc"].ToString();
                        string tallyoh = dr["tallyoh"].ToString();
                        if (ddlbranchTypesale.SelectedValue == "Other Branches")
                        {
                            newrow["ITEM NAME"] = "COW MILK Others";
                            customername = vendername;
                        }
                        else
                        {
                            newrow["ITEM NAME"] = "COW MILK";
                            if (vendername == "SVDS.P.LTD WALAJA")
                            {
                                ledgertype = "Sale of Milk Inter Branches-WALAJA";
                                tallyoh = "Over Head-Walajah";
                            }
                            if (vendername == "SVDS.P.LTD BOMMA")
                            {
                                ledgertype = "Sale of Milk Inter Branches-BOMMA";
                                tallyoh = "Over Head Bomma";
                            }
                            if (vendername == "SVDS.P.LTD R.C.PURAM")
                            {
                                ledgertype = "Sale of Milk Inter Branches-R.C.PURAM";
                                tallyoh = "Over Head R.C.Puram";
                            }
                            if (vendername == "SVDS.P.LTD V.KOTA")
                            {
                                ledgertype = "Sale of Milk Inter Branches-V.KOTA";
                                tallyoh = "Over Head-V.Kota";
                            }

                            if (vendername == "SVDS.P.LTD KAVERIPATTINAM")
                            {
                                ledgertype = "Sale of Milk Inter Branches-KAVERIPATTINAM";
                                tallyoh = "Over Head-KVP";
                            }
                            if (vendername == "SVDS.P.LTD ARANI")
                            {
                                ledgertype = "Sale of Milk Inter Branches-ARANI";
                                tallyoh = "Over Head Arani";
                            }

                            if (vendername == "SVDS.P.LTD KALA.C.C")
                            {
                                ledgertype = "Sale of Milk Inter Branches-KALA.C.C";
                                tallyoh = "Over Head Kala C.C";
                            }
                            if (vendername == "SVDS.P.LTD TARIGONDA")
                            {
                                ledgertype = "Sale of Milk Inter Branches-TARIGONDA";
                                tallyoh = "Over Head-Tarigonda";
                            }
                        }
                        string dcno = dr["dc_no"].ToString();
                        string celltype = dr["cell"].ToString();
                        string shortname = dr["shortname"].ToString();
                        newrow["INVOICE NO"] = dcno + '-' + celltype + '-' + shortname;
                        string cn = "SVDS.P.LTD";
                        string plant = "Pbk";
                        //  string vendername = dr["fromcc"].ToString();
                        newrow["LEDGER TYPE"] = ledgertype;

                        newrow["CUSTOMER NAME"] = customername;
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
                        string newdcno = dr["dc_no"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " ," + " TS Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        newrow["LEDGER AMOUNT"] = OMILKVALUE - OHcost;
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
        catch
        {
        }
    }
    public void fillbranches()
    {
        SalesDBManager SalesDB = new SalesDBManager();
        cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, email, mobno, panno, doe, branchid, address FROM vendors  where branchtype='Other Branch' AND branchid=@Obranchid");
        cmd.Parameters.Add("@Obranchid", BranchID);
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlbranchessale.DataSource = dttrips;
        ddlbranchessale.DataTextField = "vendorname";
        ddlbranchessale.DataValueField = "sno";
        ddlbranchessale.DataBind();
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
            Session["filename"] = "Other Party Sales Voucher";
            Session["title"] = "Other Party Sales Voucher Details";
            lblmsg.Text = "";
            SalesDBManager SalesDB = new SalesDBManager();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] datestrig = dtp_FromDatesale.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = dtp_Todatesale.Text.Split(' ');
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

            if (ddlReportTypesale.SelectedValue == "Buffalo")
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
                //Report.Columns.Add("ROUNDING OFF");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("Narration");
                cmd = new SqlCommand("SELECT directsales_purchase.dcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, directsales_purchase.doe, directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.doe BETWEEN @d1 AND @d2) AND (directsales_purchase.entrytype = @transtype) AND (directsales_purchaselogs.milktype='Buffalo') AND (directsales_purchase.sectionid=@sectionid) ORDER BY doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "sales");
                cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
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
                            dcnaration = "Being sales Of Milk from " + ddlbranchessale.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
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
                        string ledgrtype = dr["ledgertype"].ToString();
                        string tallyoh = dr["tallyoh"].ToString();
                        //string ledgerccname = "Sri Vyshnavi Dairy Specialities Pvt Ltd.,";
                        newrow["LEDGER TYPE"] = ledgrtype;
                        milkvaluetotal += OHandMvalue;
                        if (ddlReportTypesale.SelectedValue == "Buffalo")
                        {
                            newrow["ITEM NAME"] = "WHOLE MILK";
                        }
                        newrow["INVOICE NO"] = dr["partydcno"].ToString();
                        newrow["CUSTOMER NAME"] = vendername;
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
            else if (ddlReportTypesale.SelectedValue == "Cow" || ddlReportTypesale.SelectedValue == "Skim" || ddlReportTypesale.SelectedValue == "Condensed")
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
                cmd = new SqlCommand("  SELECT directsales_purchase.dcno, directsales_purchase.inwardno AS InwardNo, directsales_purchase.vehicleno, directsales_purchase.doe, directsales_purchase.transtype, directsales_purchase.qty_ltr, directsales_purchase.qty_kgs, directsales_purchase.percentageon, directsales_purchase.snf, directsales_purchase.fat, directsales_purchase.clr, directsales_purchaselogs.milktype, directsales_purchaselogs.rate_on, directsales_purchaselogs.cost, directsales_purchaselogs.calc_on, directsales_purchaselogs.overheadon, directsales_purchaselogs.overheadcost, directsales_purchaselogs.m_std_snf, directsales_purchaselogs.p_std_snf, directsales_purchaselogs.snfplus_on, directsales_purchaselogs.m_snfpluscost, directsales_purchaselogs.p_snfpluscost, directsales_purchaselogs.transport_on, directsales_purchaselogs.transportcost, directsales_purchaselogs.transport, vendors.vendorname, vendors.tallyoh, vendors.ledgertype FROM directsales_purchase INNER JOIN directsales_purchaselogs ON directsales_purchase.sno = directsales_purchaselogs.purchaserefno INNER JOIN vendors ON directsales_purchase.sectionid = vendors.sno WHERE  (directsales_purchase.doe BETWEEN @d1 AND @d2) AND (directsales_purchase.entrytype = @transtype) AND (directsales_purchaselogs.milktype='Cow') AND (directsales_purchase.sectionid=@sectionid) ORDER BY doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "sales");
                cmd.Parameters.Add("@bwbranchid", BranchID);
                cmd.Parameters.Add("@sectionid", ddlbranchessale.SelectedValue);
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
                            dcnaration = "Being sales Of Milk from " + ddlbranchessale.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
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

                        double OHandMvalue = 0;
                        OHandMvalue = MValue + OHcost + DiffSNFCost;
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
                        if (ddlReportTypesale.SelectedValue == "Cow")
                        {
                            newrow["ITEM NAME"] = "COW MILK";
                        }
                        newrow["INVOICE NO"] = dr["dcno"].ToString();

                        string cn = "SVDS.P.LTD";
                        string plant = "Pbk";
                        string vendername = dr["vendorname"].ToString();
                        string ledgertype = dr["ledgertype"].ToString();
                        string tallyoh = dr["tallyoh"].ToString();
                        newrow["LEDGER TYPE"] = ledgertype;
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