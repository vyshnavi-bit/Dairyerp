using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class OutwardReport : System.Web.UI.Page
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
            cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE (branchmapping.superbranch = @branchid)");
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
            cmd = new SqlCommand("SELECT sno,vehicleno,capacity,noofqty FROM vehicle_master where branchid=@branchid");
            cmd.Parameters.Add("@branchid", BranchID);
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
            grdcow.DataSource = Report;
            grdcow.DataBind();
            pnlskim.Visible = false;
            grdskim.DataSource = null;
            grdskim.DataBind();
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

            if (ddlReportType.SelectedValue == "Buffalo")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("DATE");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("KG FAT RATE");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("OH");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");
                Report.Columns.Add("TRANSPORT");
                Report.Columns.Add("TOTAL MILK VALUE");
                Report.Columns.Add("DC NO");
                Report.Columns.Add("CC Name");
                Report.Columns.Add("TANKER NO");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND (milktransactions.vehicleno=@vehicleno) AND milktransactions.branchid=@branchid");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {

                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND milktransactions.branchid=@branchid");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@branchid", BranchID);
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
                    double grandtotalmilkvalue = 0;
                    int i = 1;
                    foreach (DataRow dr in dtDispatch.Rows)
                    {
                        DataRow newrow = Report.NewRow();
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
                        string transport_on = dr["transport_on"].ToString();
                        double transportcost = 0;
                        if (transport_on == "Ltrs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = ltrs * transport;
                        }
                        if (transport_on == "Kgs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = Kgs * transport;
                        }
                        newrow["TRANSPORT"] = transportcost;

                        milkvaluetotal += OHandMvalue;
                        double totmilkvalue = 0;
                        totmilkvalue = Math.Round(OHandMvalue + transportcost, 2);
                        newrow["TOTAL MILK VALUE"] = totmilkvalue;
                        grandtotalmilkvalue += totmilkvalue;

                        newrow["DC NO"] = dr["invoiceno"].ToString();
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
                    newvartical2["TOTAL MILK VALUE"] = grandtotalmilkvalue;
                    double kgfatratetotal = 0;
                    kgfatratetotal = mvaluetotal / kgfattotal;
                    kgfatratetotal = Math.Round(kgfatratetotal, 2);
                    newvartical2["KG FAT RATE"] = kgfatratetotal;
                    Report.Rows.Add(newvartical2);
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
            if (ddlReportType.SelectedValue == "Cow")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("DATE");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("TS RATE");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("TS TOTAL");
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("OH");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");
                Report.Columns.Add("TRANSPORT");
                Report.Columns.Add("TOTAL MILK VALUE");
                Report.Columns.Add("DC NO");
                Report.Columns.Add("CC Name");
                Report.Columns.Add("TANKER NO");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND ( milktransactions.vehicleno=@vehicleno) AND milktransactions.branchid=@branchid");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND milktransactions.branchid=@branchid");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@branchid", BranchID);
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
                    double grandtotalmilkvalue = 0;
                    foreach (DataRow dr in dtDispatch.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["DATE"] = date;
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
                        string transport_on = dr["transport_on"].ToString();
                        double transportcost = 0;
                        if (transport_on == "Ltrs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = ltrs * transport;
                        }
                        if (transport_on == "Kgs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = Kgs * transport;
                        }
                        newrow["TRANSPORT"] = Math.Round(transportcost, 2);
                        double totmilkvalue = 0;
                        totmilkvalue = Math.Round(OHandMvalue + transportcost, 2);
                        newrow["TOTAL MILK VALUE"] = totmilkvalue;
                        grandtotalmilkvalue += totmilkvalue;
                        newrow["DC NO"] = dr["invoiceno"].ToString();
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
                    TStotal = snftotal + fattotal;
                    newvartical2["SNF"] = snftotal;
                    newvartical2["KG SNF"] = kgsnftotal;
                    newvartical2["TS TOTAL"] = TStotal;
                    newvartical2["M VALUE"] = mvaluetotal;
                    newvartical2["OH"] = ohtotal;
                    newvartical2["SNF9"] = snf9total;
                    newvartical2["MILK VALUE"] = milkvaluetotal;
                    newvartical2["TOTAL MILK VALUE"] = grandtotalmilkvalue;
                    double ts = TStotal * Ltrstotal;
                    double tsratetotal = 0;
                    tsratetotal = (mvaluetotal / ts) * 100;
                    tsratetotal = Math.Round(tsratetotal, 2);
                    newvartical2["TS RATE"] = tsratetotal;
                    Report.Rows.Add(newvartical2);
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
            if (ddlReportType.SelectedValue == "Skim")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("DATE");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("RATE");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("TS TOTAL");
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("OH");
                Report.Columns.Add("FAT+/-");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");
                Report.Columns.Add("Total KMs");
                Report.Columns.Add("Transport Value");
                //Report.Columns.Add("Transaction NO");
                Report.Columns.Add("DC NO");
                Report.Columns.Add("Per ltr Cost");
                Report.Columns.Add("CC Name");
                Report.Columns.Add("TANKER NO");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.transportvalue, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.kms FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND ( milktransactions.vehicleno=@vehicleno) AND milktransactions.branchid=@branchid ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.transportvalue, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.kms FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Skim') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.transportvalue, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_fatpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport,  milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, vendors.vendorname, vendors.kms FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Skim') AND (milktransactions.branchid=@branchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                //cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno,milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost,milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno WHERE (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)");

                DataTable dtskim = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtskim.Rows.Count > 0)
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
                    foreach (DataRow dr in dtskim.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["DATE"] = date;
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
                        newrow["RATE"] = cost;

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
                        //newrow["Transaction No"] = dr["invoiceno"].ToString();
                        newrow["DC NO"] = dr["invoiceno"].ToString();

                        newrow["CC Name"] = dr["vendorname"].ToString();
                        newrow["TANKER NO"] = dr["vehicleno"].ToString();
                        double ltrcost = OHandMvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 2);
                        newrow["Per ltr Cost"] = ltrcost;

                        ///////.................Transport.........//////////////////////////////
                        double totalkms = 0;
                        double.TryParse(dr["kms"].ToString(), out totalkms);
                        newrow["Total KMs"] = totalkms;
                        double perkm = 0;
                        perkm = 27;
                        double transportvalue = 0;
                        double.TryParse(dr["transportvalue"].ToString(), out transportvalue);
                        newrow["Transport Value"] = Math.Round(transportvalue, 2);
                        ///////.................Transport end.........//////////////////////////////
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
                    //newvartical2["RATE"] = tsratetotal;
                    Report.Rows.Add(newvartical2);
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
            if (ddlReportType.SelectedValue == "Condensed")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("DATE");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("TS RATE");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("TS TOTAL");
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("OH");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");
                Report.Columns.Add("TRANSPORT");
                Report.Columns.Add("TOTAL MILK VALUE");
                Report.Columns.Add("DC NO");
                Report.Columns.Add("CC Name");
                Report.Columns.Add("TANKER NO");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND ( milktransactions.vehicleno=@vehicleno) AND milktransactions.branchid=@branchid");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Condensed') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Condensed') AND milktransactions.branchid=@branchid");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@branchid", BranchID);
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
                    double grandtotalmilkvalue = 0;
                    foreach (DataRow dr in dtDispatch.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["DATE"] = date;
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
                        string transport_on = dr["transport_on"].ToString();
                        double transportcost = 0;
                        if (transport_on == "Ltrs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = ltrs * transport;
                        }
                        if (transport_on == "Kgs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = Kgs * transport;
                        }
                        newrow["TRANSPORT"] = transportcost;
                        double totmilkvalue = 0;
                        totmilkvalue = Math.Round(OHandMvalue + transportcost, 2);
                        newrow["TOTAL MILK VALUE"] = totmilkvalue;
                        grandtotalmilkvalue += totmilkvalue;
                        newrow["DC NO"] = dr["invoiceno"].ToString();
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
                    TStotal = snftotal + fattotal;
                    newvartical2["SNF"] = snftotal;
                    newvartical2["KG SNF"] = kgsnftotal;
                    newvartical2["TS TOTAL"] = TStotal;
                    newvartical2["M VALUE"] = mvaluetotal;
                    newvartical2["OH"] = ohtotal;
                    newvartical2["SNF9"] = snf9total;
                    newvartical2["MILK VALUE"] = milkvaluetotal;
                    newvartical2["TOTAL MILK VALUE"] = grandtotalmilkvalue;
                    double ts = TStotal * Ltrstotal;
                    double tsratetotal = 0;
                    tsratetotal = (mvaluetotal / ts) * 100;
                    tsratetotal = Math.Round(tsratetotal, 2);
                    newvartical2["TS RATE"] = tsratetotal;
                    Report.Rows.Add(newvartical2);
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
            else if (ddlReportType.SelectedValue == "All")
            {
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
                Report.Columns.Add("TRANSPORT");
                Report.Columns.Add("TOTAL MILK VALUE");
                Report.Columns.Add("DC NO");
                Report.Columns.Add("CC Name");
                Report.Columns.Add("TANKER NO");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo')   AND (milktransactions.vehicleno=@vehicleno) AND milktransactions.branchid=@branchid ");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {

                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid ");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND milktransactions.branchid=@branchid ");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@branchid", BranchID);
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
                    double grandtotalmilkvalue = 0;

                    foreach (DataRow dr in dtBufello.Rows)
                    {
                        DataRow newrow = Report.NewRow();
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
                        string transport_on = dr["transport_on"].ToString();
                        double transportcost = 0;
                        if (transport_on == "Ltrs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = ltrs * transport;
                        }
                        if (transport_on == "Kgs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = Kgs * transport;
                        }
                        newrow["TRANSPORT"] = transportcost;
                        double totmilkvalue = 0;
                        totmilkvalue = Math.Round(OHandMvalue + transportcost, 2);
                        newrow["TOTAL MILK VALUE"] = totmilkvalue;
                        grandtotalmilkvalue += totmilkvalue;
                        newrow["DC NO"] = dr["invoiceno"].ToString();
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
                    newvartical2["TOTAL MILK VALUE"] = grandtotalmilkvalue;
                    double kgfatratetotal = 0;
                    kgfatratetotal = mvaluetotal / kgfattotal;
                    kgfatratetotal = Math.Round(kgfatratetotal, 2);
                    newvartical2["KG FAT RATE"] = kgfatratetotal;
                    Report.Rows.Add(newvartical2);
                    DataRow New1 = Report.NewRow();
                    New1["DATE"] = "Cow";
                    Report.Rows.Add(New1);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
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
                dtReport.Columns.Add("TRANSPORT");
                dtReport.Columns.Add("TOTAL MILK VALUE");
                dtReport.Columns.Add("DC NO");
                dtReport.Columns.Add("CC Name");
                dtReport.Columns.Add("TANKER NO");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND (milktransaction_logs.milktype='Cow')  AND ( milktransactions.vehicleno=@vehicleno) AND milktransactions.branchid=@branchid ");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid ");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND milktransactions.branchid=@branchid ");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@branchid", BranchID);
                }
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
                    double grandtotalmilkvalue = 0;
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
                        string CalOn = dr["calc_on"].ToString();
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
                        newrow["TS RATE"] = cost;
                        KGFAT = Math.Round(KGFAT, 2);
                        newrow["KG FAT"] = KGFAT;
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
                        newrow["KG SNF"] = KGSNF;
                        kgsnftotal += KGSNF;
                        double MValue = 0;
                        if (CalOn == "Ltrs")
                        {
                            MValue = tstotal * cost * qty_ltr;
                        }
                        else
                        {
                            MValue = tstotal * cost * Kgs;
                        }
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
                        string transport_on = dr["transport_on"].ToString();
                        double transportcost = 0;
                        if (transport_on == "Ltrs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = ltrs * transport;
                        }
                        if (transport_on == "Kgs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = Kgs * transport;
                        }
                        newrow["TRANSPORT"] = transportcost;
                        double totmilkvalue = 0;
                        totmilkvalue = Math.Round(OHandMvalue + transportcost, 2);
                        newrow["TOTAL MILK VALUE"] = totmilkvalue;
                        grandtotalmilkvalue += totmilkvalue;
                        newrow["DC NO"] = dr["invoiceno"].ToString();
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
                    newvartical2["TOTAL MILK VALUE"] = grandtotalmilkvalue;
                    double ts = TStotal * Ltrstotal;
                    double tsratetotal = 0;
                    tsratetotal = (mvaluetotal / ts) * 100;
                    tsratetotal = Math.Round(tsratetotal, 2);
                    newvartical2["TS RATE"] = tsratetotal;
                    dtReport.Rows.Add(newvartical2);
                    grdcow.DataSource = dtReport;
                    grdcow.DataBind();
                }
                DataTable dsReport = new DataTable();
                dsReport.Columns.Add("Sno");
                dsReport.Columns.Add("DATE");
                dsReport.Columns.Add("KGS");
                dsReport.Columns.Add("LTRS");
                dsReport.Columns.Add("FAT");
                dsReport.Columns.Add("SNF");
                dsReport.Columns.Add("CLR");
                dsReport.Columns.Add("RATE");
                dsReport.Columns.Add("KG FAT");
                dsReport.Columns.Add("KG SNF");
                dsReport.Columns.Add("TS TOTAL");
                dsReport.Columns.Add("M VALUE");
                dsReport.Columns.Add("OH");
                dsReport.Columns.Add("FAT+/-");
                dsReport.Columns.Add("SNF9");
                dsReport.Columns.Add("MILK VALUE");
                dsReport.Columns.Add("Total KMs");
                dsReport.Columns.Add("Transport Value");
                //dsReport.Columns.Add("Transaction NO");
                dsReport.Columns.Add("DC NO");
                dsReport.Columns.Add("Per ltr Cost");
                dsReport.Columns.Add("CC Name");
                dsReport.Columns.Add("TANKER NO");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.transportvalue, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.kms FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND ( milktransactions.vehicleno=@vehicleno) AND milktransactions.branchid=@branchid ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.transportvalue, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.kms FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Skim') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.transportvalue, milktransactions.partydcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_fatpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport,  milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on, vendors.vendorname, vendors.kms FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Skim') AND (milktransactions.branchid=@branchid) ORDER BY doe");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                //cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno,milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost,milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno WHERE (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)");
                DataTable dtskim = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtskim.Rows.Count > 0)
                {
                    int s = 1;
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
                        newrow["Sno"] = s++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["DATE"] = date;
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
                        newrow["RATE"] = cost;

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
                        //newrow["Transaction No"] = dr["invoiceno"].ToString();
                        newrow["DC NO"] = dr["invoiceno"].ToString();

                        newrow["CC Name"] = dr["vendorname"].ToString();
                        newrow["TANKER NO"] = dr["vehicleno"].ToString();
                        double ltrcost = OHandMvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 2);
                        newrow["Per ltr Cost"] = ltrcost;

                        ///////.................Transport.........//////////////////////////////
                        double totalkms = 0;
                        double.TryParse(dr["kms"].ToString(), out totalkms);
                        newrow["Total KMs"] = totalkms;
                        double perkm = 0;
                        perkm = 27;
                        double transportvalue = 0;
                        double.TryParse(dr["transportvalue"].ToString(), out transportvalue);
                        newrow["Transport Value"] = Math.Round(transportvalue, 2);
                        ///////.................Transport end.........//////////////////////////////
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
                    tsratetotal = Math.Round(tsratetotal, 2);
                    //newvartical2["RATE"] = tsratetotal;
                    dsReport.Rows.Add(newvartical3);
                    grdskim.DataSource = dsReport;
                    grdskim.DataBind();
                    //Session["xportdata"] = dsReport;
                }
                DataTable dcondReport = new DataTable();
                dcondReport.Columns.Add("Sno");
                dcondReport.Columns.Add("DATE");
                dcondReport.Columns.Add("KGS").DataType = typeof(Double);
                dcondReport.Columns.Add("LTRS");
                dcondReport.Columns.Add("FAT");
                dcondReport.Columns.Add("SNF");
                dcondReport.Columns.Add("CLR");
                dcondReport.Columns.Add("TS RATE");
                dcondReport.Columns.Add("KG FAT").DataType = typeof(Double);
                dcondReport.Columns.Add("KG SNF").DataType = typeof(Double);
                dcondReport.Columns.Add("TS TOTAL");
                dcondReport.Columns.Add("M VALUE").DataType = typeof(Double);
                dcondReport.Columns.Add("OH").DataType = typeof(Double);
                dcondReport.Columns.Add("SNF9").DataType = typeof(Double);
                dcondReport.Columns.Add("MILK VALUE").DataType = typeof(Double);
                dcondReport.Columns.Add("TRANSPORT");
                dcondReport.Columns.Add("TOTAL MILK VALUE");
                dcondReport.Columns.Add("DC NO");
                dcondReport.Columns.Add("CC Name");
                dcondReport.Columns.Add("TANKER NO");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND (milktransaction_logs.milktype='Condensed')  AND ( milktransactions.vehicleno=@vehicleno) AND milktransactions.branchid=@branchid ");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Condensed') AND (milktransactions.sectionid=@sectionid) AND milktransactions.branchid=@branchid ");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("  SELECT milktransactions.dcno,milktransactions.invoiceno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Condensed') AND milktransactions.branchid=@branchid ");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "Out");
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                DataTable dtcond = SalesDB.SelectQuery(cmd).Tables[0];


                if (dtcond.Rows.Count > 0)
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
                    double grandtotalmilkvalue = 0;
                    plncond.Visible = true;
                    foreach (DataRow dr in dtcond.Rows)
                    {
                        DataRow newrow = dcondReport.NewRow();
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
                        string CalOn = dr["calc_on"].ToString();
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
                        newrow["TS RATE"] = cost;
                        KGFAT = Math.Round(KGFAT, 2);
                        newrow["KG FAT"] = KGFAT;
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
                        newrow["KG SNF"] = KGSNF;
                        kgsnftotal += KGSNF;
                        double MValue = 0;
                        if (CalOn == "Ltrs")
                        {
                            MValue = tstotal * cost * qty_ltr;
                        }
                        else
                        {
                            MValue = tstotal * cost * Kgs;
                        }
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
                        string transport_on = dr["transport_on"].ToString();
                        double transportcost = 0;
                        if (transport_on == "Ltrs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = ltrs * transport;
                        }
                        if (transport_on == "Kgs")
                        {
                            double transport = 0;
                            double.TryParse(dr["transportcost"].ToString(), out transport);
                            transportcost = Kgs * transport;
                        }
                        newrow["TRANSPORT"] = transportcost;
                        double totmilkvalue = 0;
                        totmilkvalue = Math.Round(OHandMvalue + transportcost, 2);
                        newrow["TOTAL MILK VALUE"] = totmilkvalue;
                        grandtotalmilkvalue += totmilkvalue;
                        newrow["DC NO"] = dr["invoiceno"].ToString();
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
                    newvartical2["TOTAL MILK VALUE"] = grandtotalmilkvalue;
                    double ts = TStotal * Ltrstotal;
                    double tsratetotal = 0;
                    tsratetotal = (mvaluetotal / ts) * 100;
                    tsratetotal = Math.Round(tsratetotal, 2);
                    newvartical2["TS RATE"] = tsratetotal;
                    dcondReport.Rows.Add(newvartical2);
                    grdcond.DataSource = dcondReport;
                    grdcond.DataBind();
                }
                else
                {
                    plncond.Visible = true;
                }
                Session["xportdata"] = Report;
                hidepanel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
}