using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Tankermilk_costreport : System.Web.UI.Page
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
   
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            grdcow.DataSource = null;
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
            if (ddlReportType.SelectedValue == "Cow")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("Vendor Name");
                Report.Columns.Add("DATE");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("CLR");
                Report.Columns.Add("SNF");
                Report.Columns.Add("TS TOTAL");
                Report.Columns.Add("TS RATE");
                Report.Columns.Add("OH");
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");

                Report.Columns.Add("Total KMs");
                Report.Columns.Add("Rate per KM");
                Report.Columns.Add("Transport");
                Report.Columns.Add("Per Ltr");
                Report.Columns.Add("Per Ts");
                Report.Columns.Add("Total Value");

                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("Per ltr Cost");
                Report.Columns.Add("Transaction NO");
                Report.Columns.Add("DC NO");
                Report.Columns.Add("TANKER NO");

                cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.partydcno,vendors.kms,vendors.temperature, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_fatpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Cow') AND (milktransactions.branchid=@branchid) ORDER BY doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "in");
                cmd.Parameters.Add("@branchid", BranchID);
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
                    foreach (DataRow dr in dtDispatch.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["DATE"] = date;
                        newrow["Vendor Name"] = dr["vendorname"].ToString();
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
                        //double SNF = 0;
                        string temp = dr["temperature"].ToString();
                        double snfvalue = 0;
                        double clr = 0;
                        double.TryParse(dr["clr"].ToString(), out clr);
                        if (temp == "21")
                        {
                            snfvalue = (FAT * 0.21) + (clr / 4 + 0.36);
                        }
                        if (temp == "27")
                        {
                            snfvalue = (FAT * 0.27) + (clr / 4 + 0.36);
                        }
                        if (temp == "29")
                        {
                            snfvalue = (FAT * 0.29) + (clr / 4 + 0.36);
                        }
                        snfvalue = Math.Round(snfvalue, 2);
                         
                        //double.TryParse(dr["snf"].ToString(), out SNF);
                        newrow["SNF"] = snfvalue;
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
                        tstotal = FAT + snfvalue;
                        newrow["TS TOTAL"] = tstotal;
                        if (Rateon == "TS")
                        {

                            double TS = 0;
                            TS = FAT + snfvalue;
                            weight = (TS * Kgs) / 100;
                            KGFAT = (FAT * Kgs) / 100;
                            KGSNF = (snfvalue * Kgs) / 100;
                        }
                        else if (Rateon == "KGFAT")
                        {
                            weight = (FAT * Kgs) / 100;
                            KGFAT = (FAT * Kgs) / 100;
                            KGSNF = (snfvalue * Kgs) / 100;
                        }
                        else if (Rateon == "PerLtr" || Rateon == "PerKg")
                        {
                            string CalOn = dr["calc_on"].ToString();
                            if (CalOn == "Ltrs")
                            {
                                weight = ltrs;
                                KGFAT = (FAT * ltrs) / 100;
                                KGSNF = (snfvalue * ltrs) / 100;
                            }
                            else
                            {
                                weight = Kgs;
                                KGFAT = (FAT * Kgs) / 100;
                                KGSNF = (snfvalue * Kgs) / 100;
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
                        if (snfvalue < MSnf)
                        {
                            string SNFOn = dr["snfplus_on"].ToString();
                            double diffSNF = 0;
                            diffSNF = snfvalue - MSnf;
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
                        if (snfvalue > PSnf)
                        {
                            string SNFOn = dr["snfplus_on"].ToString();
                            double diffSNF = 0;
                            diffSNF = snfvalue - MSnf;
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


                        ///////.................Transport.........//////////////////////////////
                        double totalkms = 0;
                        double.TryParse(dr["kms"].ToString(), out totalkms);
                        newrow["Total KMs"] = totalkms;
                        double perkm = 0;
                        perkm = 27;
                        newrow["Rate per KM"] = perkm;
                        double Transport = 0;
                        Transport = totalkms * perkm;
                        Transport = Math.Round(Transport, 2);
                        newrow["Transport"] = Transport;
                        double perltrtransportcost = 0;
                        perltrtransportcost = Transport / qty_ltr;
                        perltrtransportcost = Math.Round(perltrtransportcost, 2);
                        newrow["Per Ltr"] = perltrtransportcost;
                        double PerTs = 0;
                        PerTs = perltrtransportcost / tstotal;
                        PerTs = Math.Round(PerTs, 2);
                        newrow["Per Ts"] = PerTs;
                        double totalvalue = 0;
                        totalvalue = OHandMvalue + Transport;
                        totalvalue = Math.Ceiling(totalvalue);
                        newrow["Total Value"] = totalvalue;

                        newrow["Transaction No"] = dr["dcno"].ToString();
                        newrow["DC NO"] = dr["partydcno"].ToString();

                        newrow["TANKER NO"] = dr["vehicleno"].ToString();
                        double ltrcost = totalvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 2);
                        newrow["Per ltr Cost"] = ltrcost;
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
                    newvartical2["TS RATE"] = tsratetotal;
                    Report.Rows.Add(newvartical2);
                    grdcow.DataSource = Report;
                    grdcow.DataBind();
                    Session["xportdata"] = Report;
                    hidepanel.Visible = true;
                }
                else
                {
                    lblmsg.Text = "No data were found";
                    hidepanel.Visible = false;
                }
            }

            if (ddlReportType.SelectedValue == "Buffalo")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("Vendor Name");
                Report.Columns.Add("DATE");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("KG FAT RATE");
             
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("OH");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");

                Report.Columns.Add("Total KMs");
                Report.Columns.Add("Rate per KM");
                Report.Columns.Add("Transport");
                Report.Columns.Add("Per Ltr");
                Report.Columns.Add("Per Ts");
                Report.Columns.Add("Total Value");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("Per ltr Cost");
                Report.Columns.Add("Transaction No");
                Report.Columns.Add("DC No");
                Report.Columns.Add("TANKER NO");
                cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.partydcno,vendors.kms, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf,milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.entrydate BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.branchid = @branchid) ORDER BY doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "in");
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
                    int i = 1;
                    foreach (DataRow dr in dtDispatch.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        newrow["Vendor Name"] = dr["vendorname"].ToString();
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


                        ///////.................Transport.........//////////////////////////////
                        double totalkms = 0;
                        double.TryParse(dr["kms"].ToString(), out totalkms);

                        newrow["Total KMs"] = totalkms;
                        double perkm = 0;
                        perkm = 27;
                        newrow["Rate per KM"] = perkm;
                        double Transport = 0;
                        Transport = totalkms * perkm;
                        Transport = Math.Round(Transport, 2);
                        newrow["Transport"] = Transport;
                        double perltrtransportcost = 0;
                        perltrtransportcost = Transport / qty_ltr;
                        perltrtransportcost = Math.Round(perltrtransportcost, 2);
                        newrow["Per Ltr"] = perltrtransportcost;
                        double PerTs = 0;
                        PerTs = perltrtransportcost / tstotal;
                        PerTs = Math.Round(PerTs, 2);
                        newrow["Per Ts"] = PerTs;
                        double totalvalue = 0;
                        totalvalue = OHandMvalue + Transport;
                        totalvalue = Math.Ceiling(totalvalue);
                        newrow["Total Value"] = totalvalue;

                        newrow["Transaction No"] = dr["dcno"].ToString();
                        newrow["DC No"] = dr["partydcno"].ToString();
                        newrow["TANKER NO"] = dr["vehicleno"].ToString();
                        double ltrcost = totalvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 2);
                        newrow["Per ltr Cost"] = ltrcost;
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
                    grdcow.DataSource = Report;
                    grdcow.DataBind();
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
}