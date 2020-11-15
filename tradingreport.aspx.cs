using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class tradingreport : System.Web.UI.Page
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
    protected void ddlBranchsale_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesDBManager SalesDB = new SalesDBManager();
        if (ddlBranchsale.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
        }
        if (ddlBranchsale.SelectedValue == "Branch Wise")
        {
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
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT  directsales_purchase.sectionid, directsales_purchase.branchid, vendors.vendorname FROM directsales_purchase INNER JOIN  vendors ON directsales_purchase.sectionid = vendors.sno WHERE (directsales_purchase.doe BETWEEN @d1 AND @d2) AND (directsales_purchase.branchid = @bbranchid) AND (directsales_purchase.entrytype = 'sales') GROUP BY directsales_purchase.sectionid, directsales_purchase.branchid, vendors.vendorname");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@bbranchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranchessale.DataSource = dttrips;
            ddlbranchessale.DataTextField = "vendorname";
            ddlbranchessale.DataValueField = "sectionid";
            ddlbranchessale.DataBind();
        }
    }
    DataTable dtdirectcow = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            dtdirectcow.Columns.Add("SNO");
            dtdirectcow.Columns.Add("Date");
            dtdirectcow.Columns.Add("DC No");
            dtdirectcow.Columns.Add("TS RATE");
            dtdirectcow.Columns.Add("Purchase Vendor Name");
            dtdirectcow.Columns.Add("Purchase LTRS");
            dtdirectcow.Columns.Add("Purchase Per Ltr Cost");
            dtdirectcow.Columns.Add("Purchase Amount");
            dtdirectcow.Columns.Add("Purchase Transport");
            dtdirectcow.Columns.Add("Purchase Total Amount");
            dtdirectcow.Columns.Add("Sales Customer Name");
            dtdirectcow.Columns.Add("Sales LTRS");
            dtdirectcow.Columns.Add("Sales Per Ltr Cost");
            dtdirectcow.Columns.Add("Sales Amount");
            dtdirectcow.Columns.Add("Sales Transport");
            dtdirectcow.Columns.Add("Sales Total Amount");
            dtdirectcow.Columns.Add("Milk Difference");
            dtdirectcow.Columns.Add("Transport Difference");
            dtdirectcow.Columns.Add("Total Milk Difference");
            lblmsg.Text = "";
            Session["filename"] = "Trading Report";
            Session["title"] = "Trading Report";
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
            lblFromDate.Text = fromdate.ToString("dd/MM/yyyy");
            lbltodate.Text = todate.ToString("dd/MM/yyyy");
            cmd = new SqlCommand("SELECT  dp.sno, dp.transid, dp.dcno, dp.transtype, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon, dp.snf, dp.fat, dp.clr, dp.hs,dp.alcohol, dp.remarks, dp.chemist,  dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe,dp.transportvalue, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1,dp.phosps1, dp.mbrt, dp.acidity, dp.ot,  dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno, dpl.rate_on, dpl.calc_on, dpl.cost,dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport,dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on,  dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname FROM   directsales_purchase AS dp INNER JOIN  directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN  vendors AS v ON dp.sectionid = v.sno WHERE  (dp.entrytype = @saletypee) AND (dp.doe BETWEEN @d1 AND @d2) AND (dpl.milktype = 'Cow') and dp.branchid=@branchid ORDER BY dp.entrydate");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@saletypee", "purchase");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable tdpurchase = SalesDB.SelectQuery(cmd).Tables[0];
            //cmd = new SqlCommand("SELECT  dp.sno, dp.transid, dp.dcno, dp.transtype, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon, dp.snf, dp.fat, dp.clr, dp.hs,dp.alcohol, dp.remarks, dp.chemist,  dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1,dp.phosps1, dp.mbrt, dp.acidity, dp.ot,  dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno, dpl.rate_on, dpl.calc_on, dpl.cost,dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport,dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on,  dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname FROM   directsales_purchase AS dp INNER JOIN  directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN  vendors AS v ON dp.sectionid = v.sno WHERE  (dp.entrytype = @saletypee) AND (dp.entrydate BETWEEN @d1 AND @d2) ORDER BY dp.entrydate");
            //cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            //cmd.Parameters.Add("@d2", GetHighDate(todate));
            //cmd.Parameters.Add("@saletypee", "sales");
            //DataTable tdsales = SalesDB.SelectQuery(cmd).Tables[0];
            double purchasetotamt = 0;
            double purchasetransporttotamt = 0;
            double totpurchasetotamt = 0;
            double saletotamt = 0;
            double saletransporttotamt = 0;
            double totalsaletotamt = 0;
            double milkdifftot = 0;
            double transportdifftot = 0;
            double totmilkdifftot = 0;
            double totltrsp = 0;
            double totltrss = 0;
            if (tdpurchase.Rows.Count > 0)
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
                double transportvaluepurchase = 0;
                double transportsales = 0;
                double totalpurchase = 0;
                double totalsales = 0;
                double totalsale = 0;
                int i = 1;
                foreach (DataRow dr in tdpurchase.Rows)
                {

                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");

                    //newrow["TANKER NO"] = dr["vehicleno"].ToString();
                    //newrow["KGS"] = dr["qty_kgs"].ToString();
                    //newrow["Transaction No"] = dr["dcno"].ToString();

                    double qty_ltr = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);

                    double FAT = 0;
                    double.TryParse(dr["fat"].ToString(), out FAT);
                    FAT = Math.Round(FAT, 2);
                    //newrow["FAT"] = FAT;
                    double SNF = 0;
                    double.TryParse(dr["snf"].ToString(), out SNF);
                    //newrow["SNF"] = SNF;
                    //newrow["CLR"] = dr["clr"].ToString();
                    string Rateon = dr["rate_on"].ToString();
                    double weight = 0;
                    double KGFAT = 0;
                    double KGSNF = 0;
                    double ltrs = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out ltrs);

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
                    //newrow["FAT+/-"] = DiffFATCost;

                    double cost = 0;
                    double.TryParse(dr["cost"].ToString(), out cost);

                    KGFAT = Math.Round(KGFAT, 2);
                    //newrow["KG FAT"] = KGFAT;
                    kgfattotal += KGFAT;
                    KGSNF = Math.Round(KGSNF, 2);
                    //newrow["KG SNF"] = KGSNF;
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
                    //newrow["M VALUE"] = OHandMvalue;


                    //newrow["DC NO"] = dr["dcno"].ToString();
                    double OMILKVALUE = 0;
                    OMILKVALUE = MValue + OHcost + DiffSNFCost + DiffFATCost;
                    //newrow["OH"] = OHcost;
                    ohtotal += OHcost;
                    DiffSNFCost = Math.Round(DiffSNFCost, 2);
                    //newrow["SNF9"] = DiffSNFCost;
                    snf9total += DiffSNFCost;
                    double perltrcostp = OMILKVALUE / ltrs;
                    perltrcostp = Math.Round(perltrcostp, 2);


                    OMILKVALUE = Math.Round(OMILKVALUE, 2);

                    string dcno = dr["dcno"].ToString();
                    double transportvalue = 0;
                    double.TryParse(dr["transportvalue"].ToString(), out transportvalue);

                    double totmilkp = OMILKVALUE + transportvalue;

                    totmilkp = Math.Round(totmilkp, 2);
                    string cellno = dr["cellno"].ToString();

                    if (ddlBranchsale.SelectedValue == "Branch Wise")
                    {
                        cmd = new SqlCommand("SELECT  dp.sno, dp.transid, dp.dcno, dp.transtype, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon,dp.transportvalue, dp.snf, dp.fat, dp.clr, dp.hs,dp.alcohol, dp.remarks, dp.chemist,  dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1,dp.phosps1, dp.mbrt, dp.acidity, dp.ot,  dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno, dpl.rate_on, dpl.calc_on, dpl.cost,dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport,dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on,  dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname FROM   directsales_purchase AS dp INNER JOIN  directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN  vendors AS v ON dp.sectionid = v.sno WHERE  (dp.entrytype = @saletypee) AND (dp.doe BETWEEN @d1 AND @d2) AND (dp.dcno = @dcno) AND (dpl.milktype = 'Cow') and dp.branchid=@branchid AND (dp.sectionid = @csectionid)  AND (dp.cellno = @cellname) ORDER BY dp.entrydate");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@saletypee", "sales");
                        cmd.Parameters.Add("@dcno", dcno);
                        cmd.Parameters.Add("@branchid", BranchID);
                        cmd.Parameters.Add("@csectionid", ddlbranchessale.SelectedValue);
                        cmd.Parameters.Add("@cellname", cellno);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT  dp.sno, dp.transid, dp.dcno, dp.transtype, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon,dp.transportvalue, dp.snf, dp.fat, dp.clr, dp.hs,dp.alcohol, dp.remarks, dp.chemist,  dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1,dp.phosps1, dp.mbrt, dp.acidity, dp.ot,  dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno, dpl.rate_on, dpl.calc_on, dpl.cost,dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport,dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on,  dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname FROM   directsales_purchase AS dp INNER JOIN  directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN  vendors AS v ON dp.sectionid = v.sno WHERE  (dp.entrytype = @saletypee) AND (dp.doe BETWEEN @d1 AND @d2) AND (dp.dcno = @dcno) AND (dpl.milktype = 'Cow') and dp.branchid=@branchid AND (dp.cellno = @cellname)  ORDER BY dp.entrydate");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@saletypee", "sales");
                        cmd.Parameters.Add("@dcno", dcno);
                        cmd.Parameters.Add("@branchid", BranchID);
                        cmd.Parameters.Add("@cellname", cellno);
                    }
                    DataTable tdsales = SalesDB.SelectQuery(cmd).Tables[0];
                    double kgfattotals = 0;
                    double kgsnftotals = 0;
                    double kgstotals = 0;
                    double Ltrstotals = 0;
                    double TStotals = 0;
                    double mvaluetotals = 0;
                    double ohtotals = 0;
                    double snf9totals = 0;
                    double milkvaluetotals = 0;
                    double totalmilkvaluetotals = 0;
                    double kgfatratetotals = 0;
                    double OMILKVALUEs = 0;
                    double transportvalues = 0;
                    foreach (DataRow drs in tdsales.Rows)
                    {
                        DataRow newrow = dtdirectcow.NewRow();
                        newrow["Sno"] = i++.ToString();
                        newrow["DATE"] = date;
                        newrow["Purchase Vendor Name"] = dr["vendorname"].ToString();
                        newrow["DC No"] = dr["partydcno"].ToString();
                        newrow["Purchase LTRS"] = dr["qty_ltr"].ToString();
                        newrow["TS RATE"] = cost;
                        newrow["Purchase Per Ltr Cost"] = perltrcostp;
                        newrow["Purchase Amount"] = OMILKVALUE;
                        newrow["Purchase Transport"] = transportvalue;
                        newrow["Purchase Total Amount"] = totmilkp;
                        totltrsp += ltrs;
                        milkvaluetotal += OHandMvalue;
                        purchasetotamt += OMILKVALUE;
                        purchasetransporttotamt += transportvalue;
                        totpurchasetotamt += totmilkp;
                        //newrow["Sno"] = i++.ToString();
                        //DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        //string date = dtdoe.ToString("dd/MM/yyyy");
                        //newrow["DATE"] = date;
                        newrow["Sales Customer Name"] = drs["vendorname"].ToString();
                        //newrow["TANKER NO"] = dr["vehicleno"].ToString();
                        //newrow["KGS"] = dr["qty_kgs"].ToString();
                        //newrow["Transaction No"] = dr["dcno"].ToString();
                        //newrow["DC No"] = dr["partydcno"].ToString();
                        double qty_ltrs = 0;
                        double.TryParse(drs["qty_ltr"].ToString(), out qty_ltrs);
                        newrow["Sales LTRS"] = drs["qty_ltr"].ToString();
                        double FATs = 0;
                        double.TryParse(drs["fat"].ToString(), out FATs);
                        FAT = Math.Round(FAT, 2);
                        //newrow["FAT"] = FAT;
                        double SNFs = 0;
                        double.TryParse(drs["snf"].ToString(), out SNFs);
                        //newrow["SNF"] = SNF;
                        //newrow["CLR"] = dr["clr"].ToString();
                        string Rateons = drs["rate_on"].ToString();
                        double weights = 0;
                        double KGFATs = 0;
                        double KGSNFs = 0;
                        double ltrss = 0;
                        double.TryParse(drs["qty_ltr"].ToString(), out ltrss);
                        totltrss += ltrss;
                        double Kgss = 0;
                        double.TryParse(drs["qty_kgs"].ToString(), out Kgss);
                        kgstotal += Kgss;
                        double tstotals = 0;
                        tstotals = FATs + SNFs;
                        if (Rateons == "TS")
                        {
                            double TSs = 0;
                            TSs = FATs + SNFs;
                            weights = (TSs * Kgss) / 100;
                            KGFATs = (FATs * Kgss) / 100;
                            KGSNFs = (SNFs * Kgss) / 100;
                        }
                        else if (Rateons == "KGFAT")
                        {
                            weights = (FATs * Kgss) / 100;
                            KGFATs = (FATs * Kgss) / 100;
                            KGSNFs = (SNFs * Kgss) / 100;
                        }
                        else if (Rateons == "PerLtr" || Rateons == "PerKg")
                        {
                            string CalOns = drs["calc_on"].ToString();
                            if (CalOns == "Ltrs")
                            {
                                weights = ltrss;
                                KGFATs = (FATs * ltrss) / 100;
                                KGSNFs = (SNFs * ltrss) / 100;
                            }
                            else
                            {
                                weights = Kgss;
                                KGFATs = (FATs * Kgss) / 100;
                                KGSNFs = (SNFs * Kgss) / 100;
                            }
                        }

                        string OverheadOns = drs["overheadon"].ToString();
                        double OHcosts = 0;
                        double overheadcosts = 0;
                        double.TryParse(drs["overheadcost"].ToString(), out overheadcosts);
                        if (OverheadOns == "Ltrs")
                        {
                            OHcosts = overheadcosts * ltrss;
                        }
                        else
                        {
                            OHcosts = overheadcosts * Kgss;
                        }

                        double MSnfs = 0;
                        double.TryParse(drs["m_std_snf"].ToString(), out MSnfs);
                        double m_snfpluscosts = 0;
                        double.TryParse(drs["m_snfpluscost"].ToString(), out m_snfpluscosts);
                        double DiffSNFCosts = 0;
                        if (SNFs < MSnfs)
                        {
                            string SNFOns = drs["snfplus_on"].ToString();
                            double diffSNFs = 0;
                            diffSNFs = SNFs - MSnfs;
                            diffSNFs = Math.Round(diffSNFs, 2);
                            if (SNFOns == "Ltrs")
                            {
                                DiffSNFCosts = diffSNFs * ltrss * m_snfpluscosts * 10;
                            }
                            else
                            {
                                DiffSNFCosts = diffSNFs * Kgss * m_snfpluscosts * 10;
                            }
                        }
                        double p_snfpluscosts = 0;
                        double.TryParse(drs["p_snfpluscost"].ToString(), out p_snfpluscosts);
                        double PSnfs = 0;
                        double.TryParse(drs["p_std_snf"].ToString(), out PSnfs);
                        if (SNFs > PSnfs)
                        {
                            string SNFOns = drs["snfplus_on"].ToString();
                            double diffSNFs = 0;
                            diffSNFs = SNFs - MSnfs;
                            if (SNFOns == "Ltrs")
                            {
                                DiffSNFCosts = diffSNFs * ltrss * p_snfpluscosts * 10;
                            }
                            else
                            {
                                DiffSNFCosts = diffSNFs * Kgss * p_snfpluscosts * 10;
                            }
                        }

                        double MFats = 0;
                        double.TryParse(drs["m_std_fat"].ToString(), out MFats);
                        double m_fatpluscosts = 0;
                        double.TryParse(drs["m_fatpluscost"].ToString(), out m_fatpluscosts);
                        double DiffFATCosts = 0;
                        if (FATs < MFats)
                        {
                            string FATOns = drs["fatplus_on"].ToString();
                            double diffFATs = 0;
                            diffFATs = FAT - MFat;
                            diffFATs = Math.Round(diffFATs, 2);
                            if (FATOns == "Ltrs")
                            {
                                DiffFATCosts = diffFATs * ltrss * m_fatpluscosts * 10;
                            }
                            else
                            {
                                DiffFATCosts = diffFATs * Kgss * m_fatpluscosts * 10;
                            }
                        }
                        double p_fatpluscosts = 0;
                        double.TryParse(drs["p_fatpluscost"].ToString(), out p_fatpluscosts);
                        double PFats = 0;
                        double.TryParse(drs["p_std_fat"].ToString(), out PFats);
                        if (FATs > PFats)
                        {
                            string FATOns = drs["fatplus_on"].ToString();
                            double diffFATs = 0;
                            diffFATs = FATs - PFats;
                            if (FATOns == "Ltrs")
                            {
                                DiffFATCosts = diffFATs * ltrss * p_fatpluscosts * 10;
                            }
                            else
                            {
                                DiffFATCosts = diffFATs * Kgss * p_fatpluscosts * 10;
                            }
                        }
                        DiffFATCosts = Math.Round(DiffFATCosts, 2);
                        //newrow["FAT+/-"] = DiffFATCost;

                        double costs = 0;
                        double.TryParse(drs["cost"].ToString(), out costs);
                        //newrow["TS RATE"] = cost;
                        KGFATs = Math.Round(KGFATs, 2);
                        //newrow["KG FAT"] = KGFAT;
                        kgfattotals += KGFATs;
                        KGSNFs = Math.Round(KGSNFs, 2);
                        //newrow["KG SNF"] = KGSNF;
                        kgsnftotals += KGSNFs;
                        double MValues = 0;
                        if (Rateons == "PerLtr")
                        {
                            MValues = costs * qty_ltrs;
                        }
                        else if (Rateons == "PerKg")
                        {
                            MValues = costs * Kgss;
                        }
                        else
                        {
                            string CalOns = drs["calc_on"].ToString();
                            if (CalOns == "Ltrs")
                            {
                                MValues = tstotals * costs * ltrss;
                                MValues = MValues / 100;
                            }
                            else
                            {
                                MValues = tstotals * costs * Kgss;
                                MValues = MValues / 100;
                            }

                        }
                        //MValue = tstotal * qty_ltr * cost;
                        //MValue = MValue / 100;
                        //MValue = Math.Round(MValue, 2);
                        //newrow["M VALUE"] = MValue;
                        mvaluetotals += MValues;
                        double OHandMvalues = 0;
                        OHandMvalues = MValues;
                        OHandMvalues = Math.Round(OHandMvalues, 2);
                        //newrow["M VALUE"] = OHandMvalue;
                        milkvaluetotals += OHandMvalue;
                        //newrow["DC NO"] = dr["dcno"].ToString();
                        OMILKVALUEs = MValues + OHcosts + DiffSNFCosts + DiffFATCosts;
                        //newrow["OH"] = OHcost;
                        double perltrrates = 0;
                        perltrrates = OMILKVALUEs / ltrss;
                        perltrrates = Math.Round(perltrrates, 2);
                        newrow["Sales Per Ltr Cost"] = perltrrates;
                        ohtotals += OHcosts;
                        DiffSNFCosts = Math.Round(DiffSNFCosts, 2);
                        saletotamt += OMILKVALUEs;
                        snf9totals += DiffSNFCosts;
                        OMILKVALUEs = Math.Round(OMILKVALUEs, 2);
                        newrow["Sales Amount"] = OMILKVALUEs;

                        double.TryParse(drs["transportvalue"].ToString(), out transportvalues);
                        saletransporttotamt += transportvalues;
                        newrow["Sales Transport"] = transportvalues;

                        totalsale = transportvalues + OMILKVALUEs;
                        totalsaletotamt += totalsale;
                        totalsale = Math.Round(totalsale, 2);
                        newrow["Sales Total Amount"] = totalsale;

                        double differnce = 0;
                        differnce = OMILKVALUEs - OMILKVALUE;
                        milkdifftot += differnce;
                        differnce = Math.Round(differnce, 2);
                        newrow["Milk Difference"] = differnce;
                        double transportdiff = 0;
                        transportdiff = transportvalues - transportvalue;
                        transportdifftot += transportdiff;
                        transportdiff = Math.Round(transportdiff, 2);
                        newrow["Transport Difference"] = transportdiff;
                        double totalt = 0;
                        totalt = totalsale - totmilkp;
                        totmilkdifftot += totalt;
                        totalt = Math.Round(totalt, 2);
                        newrow["Total Milk Difference"] = totalt;
                        dtdirectcow.Rows.Add(newrow);
                    }
                    transportvaluepurchase = 0;
                    transportsales = 0;
                    totalpurchase = 0;
                    totalsales = 0;
                    totalsale = 0;
                }
                DataRow newvartical3 = dtdirectcow.NewRow();
                newvartical3["DATE"] = "Total";
                purchasetotamt = Math.Round(purchasetotamt, 2);
                newvartical3["Purchase Amount"] = purchasetotamt;
                purchasetransporttotamt = Math.Round(purchasetransporttotamt, 2);
                newvartical3["Purchase Transport"] = purchasetransporttotamt;
                totpurchasetotamt = Math.Round(totpurchasetotamt, 2);
                newvartical3["Purchase Total Amount"] = totpurchasetotamt;
                saletotamt = Math.Round(saletotamt, 2);
                newvartical3["Sales Amount"] = saletotamt;
                saletransporttotamt = Math.Round(saletransporttotamt, 2);
                newvartical3["Sales Transport"] = saletransporttotamt;
                totalsaletotamt = Math.Round(totalsaletotamt, 2);
                newvartical3["Sales Total Amount"] = totalsaletotamt;
                milkdifftot = Math.Round(milkdifftot, 2);
                newvartical3["Milk Difference"] = milkdifftot;
                transportdifftot = Math.Round(transportdifftot, 2);
                newvartical3["Transport Difference"] = transportdifftot;
                totmilkdifftot = Math.Round(totmilkdifftot, 2);
                newvartical3["Total Milk Difference"] = totmilkdifftot;
                newvartical3["Purchase LTRS"] = totltrsp;
                newvartical3["Sales LTRS"] = totltrss;
                double perltercotp = purchasetotamt / totltrsp;
                perltercotp = Math.Round(perltercotp, 2);
                newvartical3["Purchase Per Ltr Cost"] = perltercotp;
                double perltercots = saletotamt / totltrss;
                perltercots = Math.Round(perltercots, 2);
                newvartical3["Sales Per Ltr Cost"] = perltercots;
                dtdirectcow.Rows.Add(newvartical3);
                Session["xportdata"] = dtdirectcow;
                grdReports.DataSource = dtdirectcow;
                grdReports.DataBind();
                hidepanel.Visible = true;
                grdReports.Visible = true;
                pnlbuff.Visible = false;
            }
            DataTable dtdirectReport = new DataTable();

            dtdirectReport.Columns.Add("SNO");
            dtdirectReport.Columns.Add("Date");
            dtdirectReport.Columns.Add("DC No");
            dtdirectReport.Columns.Add("KG FAT RATE");
            dtdirectReport.Columns.Add("Purchase Vendor Name");
            dtdirectReport.Columns.Add("Purchase LTRS");
            dtdirectReport.Columns.Add("Purchase Per Ltr Cost");
            dtdirectReport.Columns.Add("Purchase Amount");
            dtdirectReport.Columns.Add("Purchase Transport");
            dtdirectReport.Columns.Add("Purchase Total Amount");
            dtdirectReport.Columns.Add("Sales Customer Name");
            dtdirectReport.Columns.Add("Sales LTRS");
            dtdirectReport.Columns.Add("Sales Per Ltr Cost");
            dtdirectReport.Columns.Add("Sales Amount");
            dtdirectReport.Columns.Add("Sales Transport");
            dtdirectReport.Columns.Add("Sales Total Amount");
            dtdirectReport.Columns.Add("Milk Difference");
            dtdirectReport.Columns.Add("Transport Difference");
            dtdirectReport.Columns.Add("Total Milk Difference");

            cmd = new SqlCommand("SELECT  dp.sno, dp.transid, dp.dcno,dp.transportvalue, dp.transtype, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon, dp.snf, dp.fat, dp.clr, dp.hs,dp.alcohol, dp.remarks, dp.chemist,  dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1,dp.phosps1, dp.mbrt, dp.acidity, dp.ot,  dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno, dpl.rate_on, dpl.calc_on, dpl.cost,dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport,dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on,  dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname FROM   directsales_purchase AS dp INNER JOIN  directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN  vendors AS v ON dp.sectionid = v.sno WHERE  (dp.entrytype = @saletypee) AND (dp.doe BETWEEN @d1 AND @d2) AND (dpl.milktype = 'Buffalo') and dp.branchid=@branchid ORDER BY dp.entrydate");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@saletypee", "purchase");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable tdpurchases = SalesDB.SelectQuery(cmd).Tables[0];
            double purchasetotamtb = 0;
            double purchasetransporttotamtb = 0;
            double totpurchasetotamtb = 0;

            double saletotamtb = 0;
            double saletransporttotamtb = 0;
            double totalsaletotamtb = 0;

            double milkdifftotb = 0;
            double transportdifftotb = 0;
            double totmilkdifftotb = 0;

            double Ltrstotalb = 0;
            if (tdpurchases.Rows.Count > 0)
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
                double transportvaluepurchase = 0;
                double transportsales = 0;
                double totalpurchase = 0;
                double totalsales = 0;
                double totalsale = 0;
                int i = 1;
                foreach (DataRow dr in tdpurchases.Rows)
                {

                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    //newrow["TANKER NO"] = dr["vehicleno"].ToString();
                    //newrow["KGS"] = dr["qty_kgs"].ToString();
                    //newrow["Transaction No"] = dr["dcno"].ToString();

                    double qty_ltr = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);

                    double FAT = 0;
                    double.TryParse(dr["fat"].ToString(), out FAT);
                    FAT = Math.Round(FAT, 2);
                    //newrow["FAT"] = FAT;
                    double SNF = 0;
                    double.TryParse(dr["snf"].ToString(), out SNF);
                    //newrow["SNF"] = SNF;
                    //newrow["CLR"] = dr["clr"].ToString();
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

                    kgfatratetotal += cost;
                    KGFAT = Math.Round(KGFAT, 2);
                    //newrow["KG FAT"] = KGFAT;
                    kgfattotal += KGFAT;
                    KGSNF = Math.Round(KGSNF, 2);
                    //newrow["KG SNF"] = KGSNF;
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
                    //newrow["M VALUE"] = OHandMvalue;
                    milkvaluetotal += OHandMvalue;
                    double OMILKVALUE = 0;
                    OMILKVALUE = MValue + OHcost + DiffSNFCost;
                    //newrow["OH"] = OHcost;
                    ohtotal += OHcost;
                    DiffSNFCost = Math.Round(DiffSNFCost, 2);
                    //newrow["SNF9"] = DiffSNFCost;
                    snf9total += DiffSNFCost;
                    purchasetotamtb += OMILKVALUE;
                    OMILKVALUE = Math.Round(OMILKVALUE, 2);

                    string dcno = dr["dcno"].ToString();

                    double transportvalue = 0;
                    double.TryParse(dr["transportvalue"].ToString(), out transportvalue);

                    purchasetransporttotamtb += transportvalue;
                    double totmilkp = OMILKVALUE + transportvalue;
                    totpurchasetotamtb += totmilkp;
                    totmilkp = Math.Round(totmilkp, 2);

                    double perltrratep = OMILKVALUE / ltrs;
                    perltrratep = Math.Round(perltrratep, 2);

                    cmd = new SqlCommand("SELECT  dp.sno, dp.transid, dp.dcno, dp.transtype,dp.transportvalue, dp.sectionid, dp.qty_ltr, dp.qty_kgs, dp.percentageon, dp.snf, dp.fat, dp.clr, dp.hs,dp.alcohol, dp.remarks, dp.chemist,  dp.qco, dp.inwardno, dp.vehicleno, dp.temp, dp.doe, dp.branchid, dp.operatedby, dp.cellno, dp.milktype, dp.cob1,dp.phosps1, dp.mbrt, dp.acidity, dp.ot,  dp.neutralizers, dp.partydcno, dp.entrydate, dp.status, dp.entrytype, dpl.purchaserefno, dpl.rate_on, dpl.calc_on, dpl.cost,dpl.overheadon, dpl.overheadcost, dpl.m_std_snf, dpl.p_std_snf, dpl.snfplus_on, dpl.p_snfpluscost, dpl.transport_on, dpl.transportcost, dpl.transport,dpl.entry_by, dpl.m_snfpluscost, dpl.fatplus_on,  dpl.m_fatpluscost, dpl.p_fatpluscost, dpl.m_std_fat, dpl.p_std_fat, v.vendorname FROM   directsales_purchase AS dp INNER JOIN  directsales_purchaselogs AS dpl ON dp.sno = dpl.purchaserefno INNER JOIN  vendors AS v ON dp.sectionid = v.sno WHERE  (dp.entrytype = @saletypee) AND (dp.doe BETWEEN @d1 AND @d2) AND (dp.dcno = @dcno) AND (dpl.milktype = 'Buffalo') and  dp.branchid=@branchid ORDER BY dp.entrydate");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@saletypee", "sales");
                    cmd.Parameters.Add("@dcno", dcno);
                    cmd.Parameters.Add("@branchid", BranchID);
                    DataTable dtbuffsaless = SalesDB.SelectQuery(cmd).Tables[0];
                    double kgfattotalb = 0;
                    double kgsnftotalb = 0;
                    double kgstotalb = 0;

                    double TStotalb = 0;
                    double mvaluetotalb = 0;
                    double ohtotalb = 0;
                    double snf9totalb = 0;
                    double milkvaluetotalb = 0;
                    double totalmilkvaluetotalb = 0;
                    double OMILKVALUEb = 0;
                    foreach (DataRow drb in dtbuffsaless.Rows)
                    {
                        DataRow newrow = dtdirectReport.NewRow();
                        newrow["Sno"] = i++.ToString();
                        newrow["DATE"] = date;
                        newrow["Purchase Vendor Name"] = dr["vendorname"].ToString();
                        newrow["DC No"] = dr["partydcno"].ToString();
                        newrow["Purchase LTRS"] = dr["qty_ltr"].ToString();
                        newrow["KG FAT RATE"] = cost;
                        newrow["Purchase Amount"] = OMILKVALUE;
                        newrow["Purchase Transport"] = transportvalue;
                        newrow["Purchase Total Amount"] = totmilkp;
                        newrow["Purchase Per Ltr Cost"] = perltrratep;
                        //DataRow newrow = dtdirectcow.NewRow();
                        //newrow["Sno"] = i++.ToString();
                        //DateTime dtdoeb = Convert.ToDateTime(dr["doe"].ToString());
                        //string dateb = dtdoeb.ToString("dd/MM/yyyy");
                        //newrow["DATE"] = date;
                        newrow["CC Name"] = drb["vendorname"].ToString();
                        ///newrow["TANKER NO"] = dr["vehicleno"].ToString();
                        //newrow["KGS"] = dr["qty_kgs"].ToString();
                        //newrow["Transaction No"] = dr["dcno"].ToString();
                        //newrow["DC No"] = dr["partydcno"].ToString();
                        double qty_ltrb = 0;
                        double.TryParse(drb["qty_ltr"].ToString(), out qty_ltrb);
                        newrow["Sales LTRS"] = dr["qty_ltr"].ToString();
                        double FATb = 0;
                        double.TryParse(drb["fat"].ToString(), out FATb);
                        FATb = Math.Round(FATb, 2);
                        //newrow["FAT"] = FAT;
                        double SNFb = 0;
                        double.TryParse(drb["snf"].ToString(), out SNFb);
                        //newrow["SNF"] = SNF;
                        //newrow["CLR"] = dr["clr"].ToString();
                        string Rateonb = drb["rate_on"].ToString();
                        double weightb = 0;
                        double KGFATb = 0;
                        double KGSNFb = 0;
                        double ltrsb = 0;
                        double.TryParse(drb["qty_ltr"].ToString(), out ltrsb);
                        Ltrstotalb += ltrsb;
                        double Kgsb = 0;
                        double.TryParse(drb["qty_kgs"].ToString(), out Kgsb);
                        kgstotalb += Kgsb;
                        double tstotalb = 0;
                        tstotalb = FATb + SNFb;
                        if (Rateonb == "TS")
                        {

                            double TSb = 0;
                            TSb = FATb + SNFb;
                            weightb = (TSb * Kgsb) / 100;
                            KGFATb = (FATb * Kgsb) / 100;
                            KGSNFb = (SNFb * Kgsb) / 100;
                        }
                        else if (Rateonb == "KGFAT")
                        {
                            weightb = (FATb * Kgsb) / 100;
                            KGFATb = (FATb * Kgsb) / 100;
                            KGSNFb = (SNFb * Kgsb) / 100;
                        }
                        else if (Rateonb == "PerLtr" || Rateonb == "PerKg")
                        {
                            string CalOnb = drb["calc_on"].ToString();
                            if (CalOnb == "Ltrs")
                            {
                                weightb = ltrsb;
                                KGFATb = (FATb * ltrsb) / 100;
                                KGSNFb = (SNFb * ltrsb) / 100;
                            }
                            else
                            {
                                weightb = Kgsb;
                                KGFATb = (FATb * Kgsb) / 100;
                                KGSNFb = (SNFb * Kgsb) / 100;
                            }
                        }

                        string OverheadOnb = drb["overheadon"].ToString();
                        double OHcostb = 0;
                        double overheadcostb = 0;
                        double.TryParse(drb["overheadcost"].ToString(), out overheadcostb);
                        if (OverheadOnb == "Ltrs")
                        {
                            OHcostb = overheadcostb * ltrsb;
                        }
                        else
                        {
                            OHcostb = overheadcostb * Kgsb;
                        }

                        double MSnfb = 0;
                        double.TryParse(dr["m_std_snf"].ToString(), out MSnfb);
                        double m_snfpluscostb = 0;
                        double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscostb);
                        double DiffSNFCostb = 0;
                        if (SNFb < MSnfb)
                        {
                            string SNFOnb = drb["snfplus_on"].ToString();
                            double diffSNFb = 0;
                            diffSNFb = SNFb - MSnfb;
                            diffSNFb = Math.Round(diffSNFb, 2);
                            if (SNFOnb == "Ltrs")
                            {
                                DiffSNFCostb = diffSNFb * ltrsb * m_snfpluscostb * 10;
                            }
                            else
                            {
                                DiffSNFCostb = diffSNFb * Kgsb * m_snfpluscostb * 10;
                            }
                        }
                        double p_snfpluscostb = 0;
                        double.TryParse(drb["p_snfpluscost"].ToString(), out p_snfpluscostb);
                        double PSnfb = 0;
                        double.TryParse(drb["p_std_snf"].ToString(), out PSnfb);
                        if (SNFb > PSnfb)
                        {
                            string SNFOnb = drb["snfplus_on"].ToString();
                            double diffSNFb = 0;
                            diffSNFb = SNFb - MSnfb;
                            if (SNFOnb == "Ltrs")
                            {
                                DiffSNFCostb = diffSNFb * ltrsb * p_snfpluscostb * 10;
                            }
                            else
                            {
                                DiffSNFCostb = diffSNFb * Kgsb * p_snfpluscostb * 10;
                            }
                        }

                        double MFatb = 0;
                        double.TryParse(drb["m_std_fat"].ToString(), out MFatb);
                        double m_fatpluscostb = 0;
                        double.TryParse(drb["m_fatpluscost"].ToString(), out m_fatpluscostb);
                        double DiffFATCostb = 0;
                        if (FATb < MFatb)
                        {
                            string FATOnb = drb["fatplus_on"].ToString();
                            double diffFATb = 0;
                            diffFATb = FATb - MFatb;
                            diffFATb = Math.Round(diffFATb, 2);
                            if (FATOnb == "Ltrs")
                            {
                                DiffFATCostb = diffFATb * ltrsb * m_fatpluscostb * 10;
                            }
                            else
                            {
                                DiffFATCostb = diffFATb * Kgsb * m_fatpluscostb * 10;
                            }
                        }
                        double p_fatpluscostb = 0;
                        double.TryParse(drb["p_fatpluscost"].ToString(), out p_fatpluscostb);
                        double PFatb = 0;
                        double.TryParse(drb["p_std_fat"].ToString(), out PFatb);
                        if (FATb > PFatb)
                        {
                            string FATOnb = drb["fatplus_on"].ToString();
                            double diffFATb = 0;
                            diffFATb = FATb - PFatb;
                            if (FATOnb == "Ltrs")
                            {
                                DiffFATCostb = diffFATb * ltrsb * p_fatpluscostb * 10;
                            }
                            else
                            {
                                DiffFATCostb = diffFATb * Kgsb * p_fatpluscostb * 10;
                            }
                        }
                        DiffFATCostb = Math.Round(DiffFATCostb, 2);
                        //newrow["FAT+/-"] = DiffFATCostb;

                        double costb = 0;
                        double.TryParse(drb["cost"].ToString(), out costb);
                        //newrow["TS RATE"] = cost;
                        KGFATb = Math.Round(KGFATb, 2);
                        //newrow["KG FAT"] = KGFATb;
                        kgfattotalb += KGFATb;
                        KGSNFb = Math.Round(KGSNFb, 2);
                        //newrow["KG SNF"] = KGSNFb;
                        kgsnftotalb += KGSNFb;
                        double MValueb = 0;
                        if (Rateonb == "PerLtr")
                        {
                            MValueb = costb * qty_ltrb;
                        }
                        else if (Rateonb == "PerKg")
                        {
                            MValueb = costb * Kgsb;
                        }
                        else
                        {
                            string CalOnb = drb["calc_on"].ToString();
                            if (CalOnb == "Ltrs")
                            {
                                MValueb = tstotalb * costb * ltrsb;
                                MValueb = MValueb / 100;
                            }
                            else
                            {
                                MValueb = tstotalb * costb * Kgsb;
                                MValueb = MValueb / 100;
                            }

                        }
                        //MValue = tstotal * qty_ltr * cost;
                        //MValue = MValue / 100;
                        //MValue = Math.Round(MValue, 2);
                        //newrow["M VALUE"] = MValue;
                        mvaluetotalb += MValueb;
                        double OHandMvalueb = 0;
                        OHandMvalueb = MValueb;
                        OHandMvalueb = Math.Round(OHandMvalueb, 2);
                        //newrow["M VALUE"] = OHandMvalue;

                        milkvaluetotalb += OHandMvalueb;
                        //newrow["DC NO"] = dr["dcno"].ToString();

                        OMILKVALUEb = MValueb + OHcostb + DiffSNFCostb + DiffFATCostb;
                        //newrow["OH"] = OHcost;
                        ohtotalb += OHcostb;
                        DiffSNFCostb = Math.Round(DiffSNFCostb, 2);
                        //newrow["SNF9"] = DiffSNFCost;
                        snf9totalb += DiffSNFCostb;
                        saletotamtb += OMILKVALUE;
                        OMILKVALUEb = Math.Round(OMILKVALUEb, 2);
                        newrow["Sales Amount"] = OMILKVALUEb;

                        double transportvalues = 0;
                        double.TryParse(drb["transportvalue"].ToString(), out transportvalues);
                        newrow["Sales Transport"] = transportvalues;
                        saletransporttotamtb += transportvalues;
                        totalsale = transportvalues + OMILKVALUEb;
                        totalsaletotamtb += totalsale;
                        totalsale = Math.Round(totalsale, 2);
                        newrow["Sales Total Amount"] = totalsale;
                        double perltrsales = 0;
                        perltrsales = OMILKVALUEb / ltrsb;
                        perltrsales = Math.Round(perltrsales, 2);
                        newrow["Sales Per Ltr Cost"] = perltrsales;
                        double differnceb = 0;
                        differnceb = OMILKVALUEb - OMILKVALUE;
                        milkdifftotb += differnceb;
                        differnceb = Math.Round(differnceb, 2);
                        newrow["Milk Difference"] = differnceb;

                        double transportdiff = 0;
                        transportdiff = transportsales - transportvaluepurchase;
                        transportdifftotb += transportdiff;
                        transportdiff = Math.Round(transportdiff, 2);
                        newrow["Transport Difference"] = transportdiff;

                        double totalt = 0;
                        totalt = totalsale - totmilkp;
                        totmilkdifftotb += totalt;
                        totalt = Math.Round(totalt, 2);
                        newrow["Total Milk Difference"] = totalt;
                        dtdirectReport.Rows.Add(newrow);
                    }
                    kgfatratetotal = 0;
                    transportvaluepurchase = 0;
                    transportsales = 0;
                    totalpurchase = 0;
                    totalsales = 0;
                    totalsale = 0;
                }
                DataRow newvartical2 = dtdirectReport.NewRow();
                newvartical2["DATE"] = "Total";
                purchasetotamtb = Math.Round(purchasetotamtb, 2);
                newvartical2["Purchase Amount"] = purchasetotamtb;
                purchasetransporttotamtb = Math.Round(purchasetransporttotamtb, 2);
                newvartical2["Purchase Transport"] = purchasetransporttotamtb;
                totpurchasetotamtb = Math.Round(totpurchasetotamtb, 2);
                newvartical2["Purchase Total Amount"] = totpurchasetotamtb;
                saletotamtb = Math.Round(saletotamtb, 2);
                newvartical2["Sales Amount"] = saletotamtb;
                saletransporttotamtb = Math.Round(saletransporttotamtb, 2);
                newvartical2["Sales Transport"] = saletransporttotamtb;
                totalsaletotamtb = Math.Round(totalsaletotamtb, 2);
                newvartical2["Sales Total Amount"] = totalsaletotamtb;
                milkdifftotb = Math.Round(milkdifftotb, 2);
                newvartical2["Milk Difference"] = milkdifftotb;
                transportdifftotb = Math.Round(transportdifftotb, 2);
                newvartical2["Transport Difference"] = transportdifftotb;
                totmilkdifftotb = Math.Round(totmilkdifftotb, 2);
                newvartical2["Total Milk Difference"] = totmilkdifftotb;
                newvartical2["Purchase LTRS"] = Ltrstotal;
                newvartical2["Sales LTRS"] = Ltrstotalb;
                double totperltrb = 0;
                totperltrb = purchasetotamtb / Ltrstotal;
                totperltrb = Math.Round(totperltrb, 2);
                newvartical2["Purchase Per Ltr Cost"] = totperltrb;
                double totperltrs = 0;
                totperltrs = totalsaletotamtb / Ltrstotalb;
                totperltrs = Math.Round(totperltrs, 2);
                newvartical2["Sales Per Ltr Cost"] = totperltrs;
                dtdirectReport.Rows.Add(newvartical2);

                Session["xportdata"] = dtdirectReport;
                grdbuff.DataSource = dtdirectReport;
                grdbuff.DataBind();
                hidepanel.Visible = true;
                grdbuff.Visible = true;
                pnlbuff.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = "No data were found";
            hidepanel.Visible = false;
        }
    }
    protected void grdcow_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }
}
