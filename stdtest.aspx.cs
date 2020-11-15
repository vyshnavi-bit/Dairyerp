using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class stdtest : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
    string rd1 = string.Empty;
    int flag = 0;
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
                    fillbatches();
                    PopulateMonth();
                    PopulateYear();
                }
            }
        }
    }

    private void PopulateMonth()
    {
        ddlmonth.Items.Clear();
        ListItem lt = new ListItem();
        lt.Text = "MM";
        lt.Value = "0";
        ddlmonth.Items.Add(lt);
        for (int i = 1; i <= 12; i++)
        {
            lt = new ListItem();
            lt.Text = Convert.ToDateTime(i.ToString() + "/1/1900").ToString("MMMM");
            lt.Value = i.ToString();
            ddlmonth.Items.Add(lt);
        }
        ddlmonth.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
    }

    private void PopulateYear()
    {
        ddlyear.Items.Clear();
        ListItem lt = new ListItem();
        lt.Text = "YYYY";
        lt.Value = "0";
        ddlyear.Items.Add(lt);
        for (int i = DateTime.Now.Year; i >= 1950; i--)
        {
            lt = new ListItem();
            lt.Text = i.ToString();
            lt.Value = i.ToString();
            ddlyear.Items.Add(lt);
        }
        ddlyear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
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

    private void fillbatches()
    {
        cmd = new SqlCommand("SELECT batchid, batch, batchcode, branchid, createdby FROM batchmaster");
        cmd.Parameters.Add("@BranchID", BranchID);
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddl_mnthbranchname.DataSource = dttrips;
        ddl_mnthbranchname.DataTextField = "batch";
        ddl_mnthbranchname.DataValueField = "batchid";
        ddl_mnthbranchname.DataBind();
    }

    DataTable Report = new DataTable();
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesDBManager SalesDB = new SalesDBManager();
        if (ddlBatchType.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
        }
        if (ddlBatchType.SelectedValue == "Batch Wise")
        {
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT batchid, batch, batchcode, branchid, createdby FROM batchmaster");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbatches.DataSource = dttrips;
            ddlbatches.DataTextField = "batch";
            ddlbatches.DataValueField = "batchid";
            ddlbatches.DataBind();
        }
    }

    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            grdReports.DataSource = null;
            grdReports.DataBind();
            lblmsg.Text = "";
            string branchid = Session["Branch_ID"].ToString();
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
            Session["filename"] = "Batch Entry";
            Session["title"] = "Batch Entry Details";
            DateTime dtfrom = GetLowDate(fromdate);
            DateTime dtfromdate = dtfrom.AddHours(6);
            DateTime dtto = GetLowDate(todate);
            DateTime dttodate = dtto.AddHours(30);
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");

            DateTime dtfdate = GetLowDate(fromdate).AddDays(-1);
            DateTime dttdate = GetHighDate(todate).AddDays(-1);

            if (ddlBatchType.SelectedValue == "Batch Wise")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("Date");
                Report.Columns.Add("TIME");
                Report.Columns.Add("Batch Name");
                Report.Columns.Add("Source");
                //Report.Columns.Add("OB");
                Report.Columns.Add("KGS").DataType = typeof(Double);
                Report.Columns.Add("LTRS").DataType = typeof(Double);
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("SMP");
                Report.Columns.Add("Per Ltr Rate");
                Report.Columns.Add("Amount");
                Report.Columns.Add("stand");
                if (ddlBatchType.SelectedValue == "Batch Wise")
                {

                    lblbatchname.Text = "Batch Name   " + ddlbatches.SelectedItem.Text;
                    cmd = new SqlCommand("SELECT silomaster.SiloName AS FromSilo,processingdepartments.departmentid, silomaster.SiloId as FromSiloId, silomaster_1.SiloName AS ToSIlo, batchentrydetails.perltrrate,batchentrydetails.type, batchentrydetails.tosiloid, batchentrydetails.smp, batchentrydetails.qty_kgs, batchentrydetails.qty_ltrs, batchentrydetails.fat, batchentrydetails.snf, batchentrydetails.doe, batchentrydetails.type, batchentrydetails.clr, batchentrydetails.siloqty, vendors.vendorname, batchmaster.batch,processingdepartments.departmentname FROM silomaster AS silomaster_1 INNER JOIN batchentrydetails ON silomaster_1.SiloId = batchentrydetails.tosiloid INNER JOIN batchmaster ON batchentrydetails.batchid = batchmaster.batchid LEFT OUTER JOIN processingdepartments ON batchentrydetails.fromdeptid = processingdepartments.departmentid LEFT OUTER JOIN vendors ON batchentrydetails.fromccid = vendors.sno LEFT OUTER JOIN silomaster ON batchentrydetails.fromsiloid = silomaster.SiloId  WHERE (batchentrydetails.doe BETWEEN @d1 AND @d2)  AND (batchmaster.batchid=@BatchID) AND (batchentrydetails.branchid = @branchid) ORDER BY batchentrydetails.doe");
                    cmd.Parameters.Add("@BatchID", ddlbatches.SelectedValue);
                    cmd.Parameters.Add("@branchid", branchid);
                }
                else
                {
                    cmd = new SqlCommand("SELECT silomaster.SiloName AS FromSilo, silomaster.SiloId as FromSiloId,processingdepartments.departmentid, silomaster_1.SiloName AS ToSIlo, batchentrydetails.perltrrate, batchentrydetails.type, batchentrydetails.tosiloid, batchentrydetails.smp, batchentrydetails.qty_kgs, batchentrydetails.qty_ltrs,batchentrydetails.fat, batchentrydetails.snf, batchentrydetails.doe, batchentrydetails.type, batchentrydetails.clr, batchentrydetails.siloqty, vendors.vendorname, batchmaster.batch,processingdepartments.departmentname FROM silomaster AS silomaster_1 INNER JOIN batchentrydetails ON silomaster_1.SiloId = batchentrydetails.tosiloid INNER JOIN batchmaster ON batchentrydetails.batchid = batchmaster.batchid LEFT OUTER JOIN processingdepartments ON batchentrydetails.fromdeptid = processingdepartments.departmentid LEFT OUTER JOIN vendors ON batchentrydetails.fromccid = vendors.sno LEFT OUTER JOIN silomaster ON batchentrydetails.fromsiloid = silomaster.SiloId  WHERE (batchentrydetails.doe BETWEEN @d1 AND @d2) AND (batchentrydetails.branchid = @branchid) ORDER BY batchentrydetails.doe");
                    cmd.Parameters.Add("@branchid", branchid);
                    // cmd = new SqlCommand("SELECT SUM(batchentrydetails.qty_kgs) AS qtykgs, SUM(batchentrydetails.qty_ltrs) AS qtyltrs, SUM(batchentrydetails.fat) AS fat, SUM(batchentrydetails.snf) AS snf,  SUM(batchentrydetails.clr) AS clr, batchmaster.batch FROM batchentrydetails INNER JOIN   batchmaster ON batchmaster.batchid = batchentrydetails.batchid WHERE (batchentrydetails.doe BETWEEN @d1 AND @d2) GROUP BY batchmaster.batch");
                }

                cmd.Parameters.Add("@d1", dtfromdate);
                cmd.Parameters.Add("@d2", dttodate);
                DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];

                cmd = new SqlCommand("SELECT sno, dcno, cellname, siloid, qty_kgs, qty_ltrs, fat, snf, clr, date, enterby, branchid, otherpartyname, ccid, deptid FROM silo_inward_transaction WHERE (branchid = @branchid) and date between @d1 and @d2");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtsiloid = SalesDB.SelectQuery(cmd).Tables[0];

                cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.sectionid,milktransactions.transportvalue, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.kms,milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on FROM  milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE (milktransactions.doe BETWEEN @d1 AND @d2) and milktransactions.branchid=@branchid ORDER BY milktransactions.sno DESC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dtmilk = vdm.SelectQuery(cmd).Tables[0];

                cmd = new SqlCommand("SELECT   siloid, departmentid, qty_kgs, qty_ltrs, date FROM  silo_outward_transaction WHERE   (date BETWEEN @d1 AND @d2) AND (branchid = @branchid)");
                cmd.Parameters.Add("@d1", dtfdate);
                cmd.Parameters.Add("@d2", dttdate);
                cmd.Parameters.Add("branchid", branchid);
                DataTable dtreturnmilk = SalesDB.SelectQuery(cmd).Tables[0];

                if (dtInward.Rows.Count > 0)
                {
                    int i = 1;
                    int sj = 1;
                    double kgclosingsnftotal = 0;
                    double kgclosingfattotal = 0;
                    double bopeningtotal = 0;
                    double kgfattotal = 0;
                    double kgsnftotal = 0;
                    double kgstotal = 0;
                    double Ltrstotal = 0;
                    double smptotal = 0;
                    double TOTsmp = 0;
                    double closingvalue = 0;
                    //grand total
                    double gkgfattotal = 0;
                    double gkgsnftotal = 0;
                    double gkgstotal = 0;
                    double gboopening = 0;
                    double gLtrstotal = 0;
                    double gsmptotal = 0;
                    double gTOTsmp = 0;
                    double totalamount = 0;
                    string ob = "";
                    DateTime dt = DateTime.Now;
                    string prevdate = string.Empty;
                    string sessiondatetime = string.Empty;
                    string hiddenvalue = string.Empty;
                    foreach (DataRow dr in dtInward.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string time = dtdoe.ToString("HH:mm");
                        string currentdate = dtdoe.ToString();
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        //string siloid = dr["tosiloid"].ToString();
                        newrow["Date"] = date;
                        newrow["TIME"] = time;
                        DateTime sessiondate = GetLowDate(dtdoe).AddMinutes(1);
                        if (sessiondatetime != "")
                        {
                        }
                        else
                        {
                            sessiondatetime = sessiondate.ToString();
                        }
                        if (Convert.ToDateTime(currentdate) <= Convert.ToDateTime(sessiondatetime))
                        {
                            sj = 1;
                            string strPlantime = dtdoe.ToString("dd/MMM/yyyy HH:mm");
                            string[] PlanDateTime = strPlantime.Split(' ');

                            newrow["Batch Name"] = dr["batch"].ToString();
                            // newrow["To Silo"] = dr["ToSIlo"].ToString();
                            double bopening = 0;
                            bopeningtotal = bopening;
                            gboopening += bopening;
                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                            newrow["KGS"] = Kgs;
                            kgstotal += Kgs;
                            gkgstotal += Kgs;
                            double ltrs = 0;
                            double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                            Ltrstotal += ltrs;
                            gLtrstotal += ltrs;
                            newrow["LTRS"] = ltrs;
                            // newrow["CV"] = ltrs + bopening;
                            // closingvalue = ltrs + bopening;
                            double fat = 0;
                            double.TryParse(dr["fat"].ToString(), out fat);
                            newrow["FAT"] = fat;
                            double snfr = 0;
                            double.TryParse(dr["snf"].ToString(), out snfr);
                            newrow["SNF"] = snfr;
                            newrow["CLR"] = dr["clr"].ToString();
                            double ltrfatr = 0;
                            ltrfatr = Kgs * fat;
                            ltrfatr = Math.Round(ltrfatr / 100, 2);
                            newrow["KG FAT"] = ltrfatr;
                            kgfattotal += ltrfatr;
                            gkgfattotal += ltrfatr;
                            double ltrsnfs = 0;
                            ltrsnfs = Kgs * snfr;
                            ltrsnfs = Math.Round(ltrsnfs / 100, 2);
                            newrow["KG SNF"] = ltrsnfs;
                            double smpp = 0;
                            double.TryParse(dr["smp"].ToString(), out smpp);
                            TOTsmp += smpp;
                            gTOTsmp += smpp;
                            smpp = smpp * 0.97;
                            newrow["SMP"] = smpp;
                            smptotal += smpp;
                            gsmptotal += smpp;
                            kgsnftotal += ltrsnfs;
                            gkgsnftotal += ltrsnfs;
                            // Rate per liter Calucation
                            #region
                            string source = dr["FromSilo"].ToString();
                            if (source == "")
                            {
                                source = dr["vendorname"].ToString();
                                if (source == "")
                                {
                                    source = dr["departmentname"].ToString();
                                    double perltrp = 0;
                                    string type = dr["type"].ToString();
                                    string deptid = dr["departmentid"].ToString();
                                    #region
                                    if (type == "Return Milk" || type == "Mixed Milk" || type == "Cutting Milk")
                                    {
                                        double ltrcostsp = 0;
                                        double milkvaluetotalp = 0;
                                        double kgstotalp = 0;
                                        string siloid = "";
                                        foreach (DataRow drd in dtreturnmilk.Select("departmentid='" + deptid + "'"))
                                        {
                                            siloid = drd["siloid"].ToString();
                                            foreach (DataRow dra in dtsiloid.Select("siloid='" + siloid + "'"))
                                            {
                                                string dcno = dra["dcno"].ToString();
                                                string vendorid = dra["ccid"].ToString();
                                                if (vendorid == "")
                                                {
                                                    vendorid = "0";
                                                }
                                                foreach (DataRow drr in dtmilk.Select("dcno='" + dcno + "' AND sectionid='" + vendorid + "'"))
                                                {
                                                    if (dtmilk.Rows.Count > 0)
                                                    {
                                                        string milktype = drr["milktype"].ToString();
                                                        double kgfattotalp = 0;
                                                        double kgsnftotalp = 0;
                                                        double Ltrstotalp = 0;
                                                        double TStotalp = 0;
                                                        double mvaluetotalp = 0;
                                                        double ohtotalp = 0;
                                                        double snf9totalp = 0;
                                                        if (milktype == "Buffalo")
                                                        {
                                                            double qty_ltrp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                            double FAT = 0;
                                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                                            FAT = Math.Round(FAT, 2);
                                                            double SNF = 0;
                                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                                            string Rateon = drr["rate_on"].ToString();
                                                            double weight = 0;
                                                            double KGFAT = 0;
                                                            double KGSNF = 0;
                                                            double ltrsp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                            Ltrstotalp += ltrsp;
                                                            double Kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                            kgstotalp += Kgsp;
                                                            double tstotal = 0;
                                                            tstotal = FAT + SNF;
                                                            if (Rateon == "TS")
                                                            {
                                                                double TS = 0;
                                                                TS = FAT + SNF;
                                                                weight = (TS * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "KGFAT")
                                                            {
                                                                weight = (FAT * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                string CalOn = drr["calc_on"].ToString();
                                                                if (CalOn == "Ltrs")
                                                                {
                                                                    weight = ltrsp;
                                                                    KGFAT = (FAT * ltrsp) / 100;
                                                                    KGSNF = (SNF * ltrsp) / 100;
                                                                }
                                                                else
                                                                {
                                                                    weight = Kgsp;
                                                                    KGFAT = (FAT * Kgsp) / 100;
                                                                    KGSNF = (SNF * Kgsp) / 100;
                                                                }
                                                            }
                                                            double cost = 0;
                                                            double.TryParse(drr["cost"].ToString(), out cost);
                                                            KGFAT = Math.Round(KGFAT, 2);
                                                            kgfattotalp += KGFAT;
                                                            KGSNF = Math.Round(KGSNF, 2);
                                                            kgsnftotalp += KGSNF;
                                                            double MValue = 0;
                                                            MValue = KGFAT * cost;
                                                            MValue = Math.Round(MValue, 2);
                                                            mvaluetotalp += MValue;
                                                            string OverheadOn = drr["overheadon"].ToString();
                                                            double OHcost = 0;
                                                            double overheadcost = 0;
                                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                            if (OverheadOn == "Ltrs")
                                                            {
                                                                OHcost = overheadcost * ltrsp;
                                                            }
                                                            else
                                                            {
                                                                OHcost = overheadcost * Kgsp;
                                                            }
                                                            double MSnf = 0;
                                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                            double m_snfpluscost = 0;
                                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                            double DiffSNFCost = 0;
                                                            if (SNF < MSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                diffSNF = Math.Round(diffSNF, 2);
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                                }
                                                            }
                                                            double p_snfpluscost = 0;
                                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                            double PSnf = 0;
                                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                            if (SNF > PSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                                }
                                                            }
                                                            double OHandMvalue = 0;
                                                            OHandMvalue = MValue + OHcost + DiffSNFCost;
                                                            ohtotalp += OHcost;
                                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                            snf9totalp += DiffSNFCost;
                                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                                            milkvaluetotalp += OHandMvalue;
                                                            double ltrcostp = OHandMvalue / ltrsp;
                                                            ltrcostp = Math.Round(ltrcostp, 2);
                                                            ltrcostsp += ltrcostp;
                                                        }
                                                        else if (milktype == "Cow" || milktype == "Condensed")
                                                        {
                                                            double qty_kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out qty_kgsp);
                                                            double qty_ltrp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                            double FAT = 0;
                                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                                            FAT = Math.Round(FAT, 2);
                                                            double SNF = 0;
                                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                                            string Rateon = drr["rate_on"].ToString();
                                                            double weight = 0;
                                                            double KGFAT = 0;
                                                            double KGSNF = 0;
                                                            double ltrsp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                            Ltrstotalp += ltrsp;
                                                            double Kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                            kgstotalp += Kgsp;
                                                            double tstotal = 0;
                                                            tstotal = FAT + SNF;
                                                            if (Rateon == "TS")
                                                            {
                                                                double TS = 0;
                                                                TS = FAT + SNF;
                                                                weight = (TS * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "KGFAT")
                                                            {
                                                                weight = (FAT * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                string CalOn = drr["calc_on"].ToString();
                                                                if (CalOn == "Ltrs")
                                                                {
                                                                    weight = ltrsp;
                                                                    KGFAT = (FAT * ltrsp) / 100;
                                                                    KGSNF = (SNF * ltrsp) / 100;
                                                                }
                                                                else
                                                                {
                                                                    weight = Kgsp;
                                                                    KGFAT = (FAT * Kgsp) / 100;
                                                                    KGSNF = (SNF * Kgsp) / 100;
                                                                }
                                                            }
                                                            double cost = 0;
                                                            double.TryParse(drr["cost"].ToString(), out cost);
                                                            KGFAT = Math.Round(KGFAT, 2);
                                                            kgfattotalp += KGFAT;
                                                            KGSNF = Math.Round(KGSNF, 2);
                                                            kgsnftotalp += KGSNF;
                                                            double MValue = 0;
                                                            if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                MValue = cost * qty_kgsp;
                                                            }
                                                            else
                                                            {
                                                                MValue = tstotal * cost * qty_ltrp;
                                                                MValue = MValue / 100;
                                                            }
                                                            MValue = Math.Round(MValue, 2);
                                                            mvaluetotalp += MValue;
                                                            string OverheadOn = drr["overheadon"].ToString();
                                                            double OHcost = 0;
                                                            double overheadcost = 0;
                                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                            if (OverheadOn == "Ltrs")
                                                            {
                                                                OHcost = overheadcost * ltrsp;
                                                            }
                                                            else
                                                            {
                                                                OHcost = overheadcost * Kgsp;
                                                            }
                                                            double MSnf = 0;
                                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                            double m_snfpluscost = 0;
                                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                            double DiffSNFCost = 0;
                                                            if (SNF < MSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                diffSNF = Math.Round(diffSNF, 2);
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                                }
                                                            }
                                                            double p_snfpluscost = 0;
                                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                            double PSnf = 0;
                                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                            if (SNF > PSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                                }
                                                            }
                                                            double MFat = 0;
                                                            double.TryParse(drr["m_std_fat"].ToString(), out MFat);
                                                            double m_fatpluscost = 0;
                                                            double.TryParse(drr["m_fatpluscost"].ToString(), out m_fatpluscost);
                                                            double DiffFATCost = 0;
                                                            if (FAT < MFat)
                                                            {
                                                                string FATOn = drr["fatplus_on"].ToString();
                                                                double diffFAT = 0;
                                                                diffFAT = FAT - MFat;
                                                                diffFAT = Math.Round(diffFAT, 2);
                                                                if (FATOn == "Ltrs")
                                                                {
                                                                    DiffFATCost = diffFAT * ltrsp * m_fatpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffFATCost = diffFAT * Kgsp * m_fatpluscost * 10;
                                                                }
                                                            }
                                                            double p_fatpluscost = 0;
                                                            double.TryParse(drr["p_fatpluscost"].ToString(), out p_fatpluscost);
                                                            double PFat = 0;
                                                            double.TryParse(drr["p_std_fat"].ToString(), out PFat);
                                                            if (FAT > PFat)
                                                            {
                                                                string FATOn = drr["fatplus_on"].ToString();
                                                                double diffFAT = 0;
                                                                diffFAT = FAT - PFat;
                                                                if (FATOn == "Ltrs")
                                                                {
                                                                    DiffFATCost = diffFAT * ltrsp * p_fatpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffFATCost = diffFAT * Kgsp * p_fatpluscost * 10;
                                                                }
                                                            }
                                                            DiffFATCost = Math.Round(DiffFATCost, 2);
                                                            double OHandMvalue = 0;
                                                            OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                                                            ohtotalp += OHcost;
                                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                            snf9totalp += DiffSNFCost;
                                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                                            milkvaluetotalp += OHandMvalue;
                                                            double ltrcostp = OHandMvalue / ltrsp;
                                                            ltrcostp = Math.Round(ltrcostp, 2);
                                                            ltrcostsp += ltrcostp;
                                                        }
                                                    }
                                                }
                                            }
                                            double tkgs = kgstotalp;
                                            double ttotalmilkp = milkvaluetotalp;
                                            if (milkvaluetotalp == 0)
                                            {
                                                perltrp = 0;
                                            }
                                            else
                                            {
                                                perltrp = ttotalmilkp / tkgs;
                                            }
                                            perltrp = Math.Round(perltrp, 2);
                                        }
                                        double amt = perltrp * ltrs;
                                        totalamount += amt;
                                        newrow["Per Ltr Rate"] = perltrp;
                                        newrow["Amount"] = perltrp * ltrs;
                                    }
                                    #endregion
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    double ltrrate = 0;
                                    string perltr = dr["perltrrate"].ToString();
                                    if (perltr != "")
                                    {
                                        double.TryParse(perltr, out ltrrate);
                                    }
                                    else
                                    {

                                    }
                                    double amt = ltrrate * ltrs;
                                    totalamount += amt;
                                    newrow["Per Ltr Rate"] = ltrrate;
                                    newrow["Amount"] = ltrrate * ltrs;
                                }
                            }
                            else if (source != "")
                            {
                                string fromsiloid = dr["FromSiloId"].ToString();
                                double ltrcostsp = 0;
                                double milkvaluetotalp = 0;
                                double kgstotalp = 0;
                                double perltrp = 0;
                                if (dtsiloid.Rows.Count > 0)
                                {
                                    foreach (DataRow dra in dtsiloid.Select("siloid='" + fromsiloid + "'"))
                                    {
                                       
                                        string dcno = dra["dcno"].ToString();
                                        string vendorid = dra["ccid"].ToString();

                                        if (vendorid == "")
                                        {
                                            vendorid = "0";
                                        }
                                        foreach (DataRow drr in dtmilk.Select("dcno='" + dcno + "' AND sectionid='" + vendorid + "'"))
                                        {
                                            try
                                            {
                                                if (dtmilk.Rows.Count > 0)
                                                {
                                                    string milktype = drr["milktype"].ToString();
                                                    double kgfattotalp = 0;
                                                    double kgsnftotalp = 0;
                                                    double Ltrstotalp = 0;
                                                    double TStotalp = 0;
                                                    double mvaluetotalp = 0;
                                                    double ohtotalp = 0;
                                                    double snf9totalp = 0;
                                                    if (milktype == "Buffalo")
                                                    {

                                                        double qty_ltrp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                        double FAT = 0;
                                                        double.TryParse(drr["fat"].ToString(), out FAT);
                                                        FAT = Math.Round(FAT, 2);
                                                        double SNF = 0;
                                                        double.TryParse(drr["snf"].ToString(), out SNF);
                                                        string Rateon = drr["rate_on"].ToString();
                                                        double weight = 0;
                                                        double KGFAT = 0;
                                                        double KGSNF = 0;
                                                        double ltrsp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                        Ltrstotalp += ltrsp;
                                                        double Kgsp = 0;
                                                        double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                        kgstotalp += Kgsp;
                                                        double tstotal = 0;
                                                        tstotal = FAT + SNF;
                                                        if (Rateon == "TS")
                                                        {
                                                            double TS = 0;
                                                            TS = FAT + SNF;
                                                            weight = (TS * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "KGFAT")
                                                        {
                                                            weight = (FAT * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                        {
                                                            string CalOn = drr["calc_on"].ToString();
                                                            if (CalOn == "Ltrs")
                                                            {
                                                                weight = ltrsp;
                                                                KGFAT = (FAT * ltrsp) / 100;
                                                                KGSNF = (SNF * ltrsp) / 100;
                                                            }
                                                            else
                                                            {
                                                                weight = Kgsp;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                        }
                                                        double cost = 0;
                                                        double.TryParse(drr["cost"].ToString(), out cost);
                                                        KGFAT = Math.Round(KGFAT, 2);
                                                        kgfattotalp += KGFAT;
                                                        KGSNF = Math.Round(KGSNF, 2);
                                                        kgsnftotalp += KGSNF;
                                                        double MValue = 0;
                                                        MValue = KGFAT * cost;
                                                        MValue = Math.Round(MValue, 2);
                                                        mvaluetotalp += MValue;
                                                        string OverheadOn = drr["overheadon"].ToString();
                                                        double OHcost = 0;
                                                        double overheadcost = 0;
                                                        double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                        if (OverheadOn == "Ltrs")
                                                        {
                                                            OHcost = overheadcost * ltrsp;
                                                        }
                                                        else
                                                        {
                                                            OHcost = overheadcost * Kgsp;
                                                        }
                                                        double MSnf = 0;
                                                        double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                        double m_snfpluscost = 0;
                                                        double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                        double DiffSNFCost = 0;
                                                        if (SNF < MSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            diffSNF = Math.Round(diffSNF, 2);
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                            }
                                                        }
                                                        double p_snfpluscost = 0;
                                                        double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                        double PSnf = 0;
                                                        double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                        if (SNF > PSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                            }
                                                        }
                                                        double OHandMvalue = 0;
                                                        OHandMvalue = MValue + OHcost + DiffSNFCost;
                                                        ohtotalp += OHcost;
                                                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                        snf9totalp += DiffSNFCost;
                                                        OHandMvalue = Math.Round(OHandMvalue, 2);
                                                        milkvaluetotalp += OHandMvalue;
                                                        double ltrcostp = OHandMvalue / ltrsp;
                                                        ltrcostp = Math.Round(ltrcostp, 2);
                                                        ltrcostsp += ltrcostp;
                                                    }
                                                    else if (milktype == "Cow" || milktype == "Condensed")
                                                    {
                                                        double qty_kgsp = 0;
                                                        double.TryParse(drr["qty_kgs"].ToString(), out qty_kgsp);
                                                        double qty_ltrp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                        double FAT = 0;
                                                        double.TryParse(drr["fat"].ToString(), out FAT);
                                                        FAT = Math.Round(FAT, 2);
                                                        double SNF = 0;
                                                        double.TryParse(drr["snf"].ToString(), out SNF);
                                                        string Rateon = drr["rate_on"].ToString();
                                                        double weight = 0;
                                                        double KGFAT = 0;
                                                        double KGSNF = 0;
                                                        double ltrsp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                        Ltrstotalp += ltrsp;
                                                        double Kgsp = 0;
                                                        double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                        kgstotalp += Kgsp;
                                                        double tstotal = 0;
                                                        tstotal = FAT + SNF;
                                                        if (Rateon == "TS")
                                                        {
                                                            double TS = 0;
                                                            TS = FAT + SNF;
                                                            weight = (TS * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "KGFAT")
                                                        {
                                                            weight = (FAT * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                        {
                                                            string CalOn = drr["calc_on"].ToString();
                                                            if (CalOn == "Ltrs")
                                                            {
                                                                weight = ltrsp;
                                                                KGFAT = (FAT * ltrsp) / 100;
                                                                KGSNF = (SNF * ltrsp) / 100;
                                                            }
                                                            else
                                                            {
                                                                weight = Kgsp;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                        }
                                                        double cost = 0;
                                                        double.TryParse(drr["cost"].ToString(), out cost);
                                                        KGFAT = Math.Round(KGFAT, 2);
                                                        kgfattotalp += KGFAT;
                                                        KGSNF = Math.Round(KGSNF, 2);
                                                        kgsnftotalp += KGSNF;
                                                        double MValue = 0;
                                                        if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                        {
                                                            MValue = cost * qty_kgsp;
                                                        }
                                                        else
                                                        {
                                                            MValue = tstotal * cost * qty_ltrp;
                                                            MValue = MValue / 100;

                                                        }
                                                        MValue = Math.Round(MValue, 2);
                                                        mvaluetotalp += MValue;
                                                        string OverheadOn = drr["overheadon"].ToString();
                                                        double OHcost = 0;
                                                        double overheadcost = 0;
                                                        double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                        if (OverheadOn == "Ltrs")
                                                        {
                                                            OHcost = overheadcost * ltrsp;
                                                        }
                                                        else
                                                        {
                                                            OHcost = overheadcost * Kgsp;
                                                        }
                                                        double MSnf = 0;
                                                        double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                        double m_snfpluscost = 0;
                                                        double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                        double DiffSNFCost = 0;
                                                        if (SNF < MSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            diffSNF = Math.Round(diffSNF, 2);
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                            }
                                                        }
                                                        double p_snfpluscost = 0;
                                                        double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                        double PSnf = 0;
                                                        double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                        if (SNF > PSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                            }
                                                        }
                                                        double MFat = 0;
                                                        double.TryParse(drr["m_std_fat"].ToString(), out MFat);
                                                        double m_fatpluscost = 0;
                                                        double.TryParse(drr["m_fatpluscost"].ToString(), out m_fatpluscost);
                                                        double DiffFATCost = 0;
                                                        if (FAT < MFat)
                                                        {
                                                            string FATOn = drr["fatplus_on"].ToString();
                                                            double diffFAT = 0;
                                                            diffFAT = FAT - MFat;
                                                            diffFAT = Math.Round(diffFAT, 2);
                                                            if (FATOn == "Ltrs")
                                                            {
                                                                DiffFATCost = diffFAT * ltrsp * m_fatpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffFATCost = diffFAT * Kgsp * m_fatpluscost * 10;
                                                            }
                                                        }
                                                        double p_fatpluscost = 0;
                                                        double.TryParse(drr["p_fatpluscost"].ToString(), out p_fatpluscost);
                                                        double PFat = 0;
                                                        double.TryParse(drr["p_std_fat"].ToString(), out PFat);
                                                        if (FAT > PFat)
                                                        {
                                                            string FATOn = drr["fatplus_on"].ToString();
                                                            double diffFAT = 0;
                                                            diffFAT = FAT - PFat;
                                                            if (FATOn == "Ltrs")
                                                            {
                                                                DiffFATCost = diffFAT * ltrsp * p_fatpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffFATCost = diffFAT * Kgsp * p_fatpluscost * 10;
                                                            }
                                                        }
                                                        DiffFATCost = Math.Round(DiffFATCost, 2);
                                                        double OHandMvalue = 0;
                                                        OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                                                        ohtotalp += OHcost;
                                                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                        snf9totalp += DiffSNFCost;
                                                        OHandMvalue = Math.Round(OHandMvalue, 2);
                                                        milkvaluetotalp += OHandMvalue;
                                                        double ltrcostp = OHandMvalue / ltrsp;
                                                        ltrcostp = Math.Round(ltrcostp, 2);
                                                        ltrcostsp += ltrcostp;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    double tkgs = kgstotalp;
                                    double ttotalmilkp = milkvaluetotalp;
                                    if (milkvaluetotalp == 0)
                                    {
                                        perltrp = 0;
                                    }
                                    else
                                    {
                                        perltrp = ttotalmilkp / tkgs;
                                    }
                                    perltrp = Math.Round(perltrp, 2);
                                }
                                //double ltrrate = 0;
                                //string perltr = dr["perltrrate"].ToString();
                                //if (perltr != "")
                                //{
                                //    double.TryParse(perltr, out ltrrate);
                                //}
                                //else
                                //{

                                //}
                                double amt = perltrp * ltrs;
                                totalamount += amt;
                                newrow["Per Ltr Rate"] = perltrp;
                                newrow["Amount"] = perltrp * ltrs;
                            }
                            #endregion
                            // Rate Per liter Cluclation End
                            newrow["Source"] = source;
                            Report.Rows.Add(newrow);
                            // prevdate = currentdate;
                        }
                        else
                        {
                            if (kgstotal > 0)
                            {
                                DataRow newvartical2 = Report.NewRow();
                                newvartical2["Source"] = "Total";
                                //string val = hiddenvalue;
                                //double siloobvalue = Convert.ToDouble(val);
                                double fattotal = 0;
                                fattotal = (kgfattotal / kgstotal) * 100;
                                fattotal = Math.Round(fattotal, 2);
                                newvartical2["FAT"] = fattotal;
                                double snftotal = 0;
                                double smpkgsnftotal = 0;
                                smpkgsnftotal = smptotal + kgsnftotal;
                                snftotal = (smpkgsnftotal / kgstotal) * 100;
                                snftotal = Math.Round(snftotal, 2);
                                newvartical2["SNF"] = snftotal;
                                //newvartical2["LTR FAT"] = kgfattotal + kgclosingfattotal;
                                newvartical2["KG FAT"] = kgfattotal;
                                newvartical2["KG SNF"] = smpkgsnftotal;
                                newvartical2["SMP"] = smptotal;
                                //double opening = Convert.ToDouble(ob);
                                if (ddlBatchType.SelectedValue == "Batch Wise")
                                {
                                    // newvartical2["CV"] = closingvalue;
                                    newvartical2["LTRS"] = Ltrstotal;
                                }
                                else
                                {
                                    newvartical2["KGS"] = kgstotal;
                                    // newvartical2["CV"] = kgstotal + bopeningtotal;
                                    newvartical2["LTRS"] = Ltrstotal;
                                }
                                newvartical2["Amount"] = totalamount;
                                double stand = totalamount / Ltrstotal;
                                newvartical2["stand"] = Math.Round(stand, 2);
                                Report.Rows.Add(newvartical2);
                                kgfattotal = 0;
                                kgsnftotal = 0;
                                kgstotal = 0;
                                Ltrstotal = 0;
                                smptotal = 0;
                                TOTsmp = 0;
                                bopeningtotal = 0;
                                totalamount = 0;
                                hiddenvalue = string.Empty;
                                //ob = "";
                            }
                            sessiondate = sessiondate.AddDays(1);
                            sessiondate = sessiondate.AddHours(6);
                            sessiondatetime = sessiondate.ToString();
                            prevdate = currentdate;
                            //cmd = new SqlCommand("SELECT sno, siloid, branchid, batchid, doe, qty_ltrs, qty_kgs, fat, snf, entryby, closingdate FROM batchwiseclosing where (closingdate between @cd1 and @cd2) and (batchid=@bid)");
                            //cmd.Parameters.Add("@bid", ddlbatches.SelectedValue);
                            //cmd.Parameters.Add("@cd1", GetLowDate(dtdoe));
                            //cmd.Parameters.Add("@cd2", GetLowDate(dtdoe));
                            //DataTable dtclosing = SalesDB.SelectQuery(cmd).Tables[0];
                            //if (dtclosing.Rows.Count > 0)
                            //{
                            //    foreach (DataRow dcloser in dtclosing.Rows)
                            //    {
                            //       // ob = dcloser["qty_kgs"].ToString();
                            //        ob = "0";
                            //        newrow["OB"] = ob;
                            //    }
                            //}
                            string strPlantime = dtdoe.ToString("dd/MMM/yyyy HH:mm");
                            string[] PlanDateTime = strPlantime.Split(' ');
                            newrow["Date"] = PlanDateTime[0];
                            newrow["Batch Name"] = dr["batch"].ToString();
                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                            newrow["KGS"] = Kgs;
                            kgstotal += Kgs;
                            gkgstotal += Kgs;
                            double ltrs = 0;
                            double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                            Ltrstotal += ltrs;
                            gLtrstotal += ltrs;
                            newrow["LTRS"] = ltrs;
                            double fat = 0;
                            double.TryParse(dr["fat"].ToString(), out fat);
                            newrow["FAT"] = fat;
                            double snfr = 0;
                            double.TryParse(dr["snf"].ToString(), out snfr);
                            newrow["SNF"] = snfr;
                            newrow["CLR"] = dr["clr"].ToString();
                            double ltrfatr = 0;
                            ltrfatr = Kgs * fat;
                            ltrfatr = Math.Round(ltrfatr / 100, 2);
                            newrow["KG FAT"] = ltrfatr;
                            kgfattotal += ltrfatr;
                            gkgfattotal += ltrfatr;

                            double ltrsnfs = 0;
                            ltrsnfs = Kgs * snfr;
                            ltrsnfs = Math.Round(ltrsnfs / 100, 2);
                            newrow["KG SNF"] = ltrsnfs;
                            double smpp = 0;
                            double.TryParse(dr["smp"].ToString(), out smpp);
                            TOTsmp += smpp;
                            gTOTsmp += smpp;
                            smpp = smpp * 0.97;
                            newrow["SMP"] = smpp;
                            smptotal += smpp;
                            gsmptotal += smpp;
                            kgsnftotal += ltrsnfs;
                            gkgsnftotal += ltrsnfs;
                            // Rate per liter Calucation
                            #region
                            string source = dr["FromSilo"].ToString();
                            if (source == "")
                            {
                                source = dr["vendorname"].ToString();
                                if (source == "")
                                {
                                    source = dr["departmentname"].ToString();
                                    double perltrp = 0;
                                    string type = dr["type"].ToString();
                                    string deptid = dr["departmentid"].ToString();
                                    if (type == "Return Milk" || type == "Mixed Milk" || type == "Cutting Milk")
                                    {
                                        double ltrcostsp = 0;
                                        double milkvaluetotalp = 0;
                                        double kgstotalp = 0;
                                        string siloid = "";
                                        foreach (DataRow drd in dtreturnmilk.Select("departmentid='" + deptid + "'"))
                                        {
                                            siloid = drd["siloid"].ToString();
                                            foreach (DataRow dra in dtsiloid.Select("siloid='" + siloid + "'"))
                                            {
                                                string dcno = dra["dcno"].ToString();
                                                string vendorid = dra["ccid"].ToString();
                                                if (vendorid == "")
                                                {
                                                    vendorid = "0";
                                                }
                                                foreach (DataRow drr in dtmilk.Select("dcno='" + dcno + "' AND sectionid='" + vendorid + "'"))
                                                {
                                                    if (dtmilk.Rows.Count > 0)
                                                    {
                                                        string milktype = drr["milktype"].ToString();
                                                        double kgfattotalp = 0;
                                                        double kgsnftotalp = 0;
                                                        double Ltrstotalp = 0;
                                                        double TStotalp = 0;
                                                        double mvaluetotalp = 0;
                                                        double ohtotalp = 0;
                                                        double snf9totalp = 0;
                                                        if (milktype == "Buffalo")
                                                        {
                                                            double qty_ltrp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                            double FAT = 0;
                                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                                            FAT = Math.Round(FAT, 2);
                                                            double SNF = 0;
                                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                                            string Rateon = drr["rate_on"].ToString();
                                                            double weight = 0;
                                                            double KGFAT = 0;
                                                            double KGSNF = 0;
                                                            double ltrsp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                            Ltrstotalp += ltrsp;
                                                            double Kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                            kgstotalp += Kgsp;
                                                            double tstotal = 0;
                                                            tstotal = FAT + SNF;
                                                            if (Rateon == "TS")
                                                            {
                                                                double TS = 0;
                                                                TS = FAT + SNF;
                                                                weight = (TS * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "KGFAT")
                                                            {
                                                                weight = (FAT * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                string CalOn = drr["calc_on"].ToString();
                                                                if (CalOn == "Ltrs")
                                                                {
                                                                    weight = ltrsp;
                                                                    KGFAT = (FAT * ltrsp) / 100;
                                                                    KGSNF = (SNF * ltrsp) / 100;
                                                                }
                                                                else
                                                                {
                                                                    weight = Kgsp;
                                                                    KGFAT = (FAT * Kgsp) / 100;
                                                                    KGSNF = (SNF * Kgsp) / 100;
                                                                }
                                                            }
                                                            double cost = 0;
                                                            double.TryParse(drr["cost"].ToString(), out cost);
                                                            KGFAT = Math.Round(KGFAT, 2);
                                                            kgfattotalp += KGFAT;
                                                            KGSNF = Math.Round(KGSNF, 2);
                                                            kgsnftotalp += KGSNF;
                                                            double MValue = 0;
                                                            MValue = KGFAT * cost;
                                                            MValue = Math.Round(MValue, 2);
                                                            mvaluetotalp += MValue;
                                                            string OverheadOn = drr["overheadon"].ToString();
                                                            double OHcost = 0;
                                                            double overheadcost = 0;
                                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                            if (OverheadOn == "Ltrs")
                                                            {
                                                                OHcost = overheadcost * ltrsp;
                                                            }
                                                            else
                                                            {
                                                                OHcost = overheadcost * Kgsp;
                                                            }
                                                            double MSnf = 0;
                                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                            double m_snfpluscost = 0;
                                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                            double DiffSNFCost = 0;
                                                            if (SNF < MSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                diffSNF = Math.Round(diffSNF, 2);
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                                }
                                                            }
                                                            double p_snfpluscost = 0;
                                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                            double PSnf = 0;
                                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                            if (SNF > PSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                                }
                                                            }
                                                            double OHandMvalue = 0;
                                                            OHandMvalue = MValue + OHcost + DiffSNFCost;
                                                            ohtotalp += OHcost;
                                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                            snf9totalp += DiffSNFCost;
                                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                                            milkvaluetotalp += OHandMvalue;
                                                            double ltrcostp = OHandMvalue / ltrsp;
                                                            ltrcostp = Math.Round(ltrcostp, 2);
                                                            ltrcostsp += ltrcostp;
                                                        }
                                                        else if (milktype == "Cow" || milktype == "Condensed")
                                                        {
                                                            double qty_kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out qty_kgsp);
                                                            double qty_ltrp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                            double FAT = 0;
                                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                                            FAT = Math.Round(FAT, 2);
                                                            double SNF = 0;
                                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                                            string Rateon = drr["rate_on"].ToString();
                                                            double weight = 0;
                                                            double KGFAT = 0;
                                                            double KGSNF = 0;
                                                            double ltrsp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                            Ltrstotalp += ltrsp;
                                                            double Kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                            kgstotalp += Kgsp;
                                                            double tstotal = 0;
                                                            tstotal = FAT + SNF;
                                                            if (Rateon == "TS")
                                                            {
                                                                double TS = 0;
                                                                TS = FAT + SNF;
                                                                weight = (TS * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "KGFAT")
                                                            {
                                                                weight = (FAT * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                string CalOn = drr["calc_on"].ToString();
                                                                if (CalOn == "Ltrs")
                                                                {
                                                                    weight = ltrsp;
                                                                    KGFAT = (FAT * ltrsp) / 100;
                                                                    KGSNF = (SNF * ltrsp) / 100;
                                                                }
                                                                else
                                                                {
                                                                    weight = Kgsp;
                                                                    KGFAT = (FAT * Kgsp) / 100;
                                                                    KGSNF = (SNF * Kgsp) / 100;
                                                                }
                                                            }
                                                            double cost = 0;
                                                            double.TryParse(drr["cost"].ToString(), out cost);
                                                            KGFAT = Math.Round(KGFAT, 2);
                                                            kgfattotalp += KGFAT;
                                                            KGSNF = Math.Round(KGSNF, 2);
                                                            kgsnftotalp += KGSNF;
                                                            double MValue = 0;
                                                            if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                MValue = cost * qty_kgsp;
                                                            }
                                                            else
                                                            {
                                                                MValue = tstotal * cost * qty_ltrp;
                                                                MValue = MValue / 100;
                                                            }
                                                            MValue = Math.Round(MValue, 2);
                                                            mvaluetotalp += MValue;
                                                            string OverheadOn = drr["overheadon"].ToString();
                                                            double OHcost = 0;
                                                            double overheadcost = 0;
                                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                            if (OverheadOn == "Ltrs")
                                                            {
                                                                OHcost = overheadcost * ltrsp;
                                                            }
                                                            else
                                                            {
                                                                OHcost = overheadcost * Kgsp;
                                                            }
                                                            double MSnf = 0;
                                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                            double m_snfpluscost = 0;
                                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                            double DiffSNFCost = 0;
                                                            if (SNF < MSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                diffSNF = Math.Round(diffSNF, 2);
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                                }
                                                            }
                                                            double p_snfpluscost = 0;
                                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                            double PSnf = 0;
                                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                            if (SNF > PSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                                }
                                                            }
                                                            double MFat = 0;
                                                            double.TryParse(drr["m_std_fat"].ToString(), out MFat);
                                                            double m_fatpluscost = 0;
                                                            double.TryParse(drr["m_fatpluscost"].ToString(), out m_fatpluscost);
                                                            double DiffFATCost = 0;
                                                            if (FAT < MFat)
                                                            {
                                                                string FATOn = drr["fatplus_on"].ToString();
                                                                double diffFAT = 0;
                                                                diffFAT = FAT - MFat;
                                                                diffFAT = Math.Round(diffFAT, 2);
                                                                if (FATOn == "Ltrs")
                                                                {
                                                                    DiffFATCost = diffFAT * ltrsp * m_fatpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffFATCost = diffFAT * Kgsp * m_fatpluscost * 10;
                                                                }
                                                            }
                                                            double p_fatpluscost = 0;
                                                            double.TryParse(drr["p_fatpluscost"].ToString(), out p_fatpluscost);
                                                            double PFat = 0;
                                                            double.TryParse(drr["p_std_fat"].ToString(), out PFat);
                                                            if (FAT > PFat)
                                                            {
                                                                string FATOn = drr["fatplus_on"].ToString();
                                                                double diffFAT = 0;
                                                                diffFAT = FAT - PFat;
                                                                if (FATOn == "Ltrs")
                                                                {
                                                                    DiffFATCost = diffFAT * ltrsp * p_fatpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffFATCost = diffFAT * Kgsp * p_fatpluscost * 10;
                                                                }
                                                            }
                                                            DiffFATCost = Math.Round(DiffFATCost, 2);
                                                            double OHandMvalue = 0;
                                                            OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                                                            ohtotalp += OHcost;
                                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                            snf9totalp += DiffSNFCost;
                                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                                            milkvaluetotalp += OHandMvalue;
                                                            double ltrcostp = OHandMvalue / ltrsp;
                                                            ltrcostp = Math.Round(ltrcostp, 2);
                                                            ltrcostsp += ltrcostp;
                                                        }
                                                    }
                                                }
                                            }
                                            double tkgs = kgstotalp;
                                            double ttotalmilkp = milkvaluetotalp;
                                            if (milkvaluetotalp == 0)
                                            {
                                                perltrp = 0;
                                            }
                                            else
                                            {
                                                perltrp = ttotalmilkp / tkgs;
                                            }
                                            perltrp = Math.Round(perltrp, 2);
                                        }
                                        double amt = perltrp * ltrs;
                                        totalamount += amt;
                                        newrow["Per Ltr Rate"] = perltrp;
                                        newrow["Amount"] = perltrp * ltrs;
                                    }
                                }
                                else
                                {
                                    double ltrrate = 0;
                                    string perltr = dr["perltrrate"].ToString();
                                    if (perltr != "")
                                    {
                                        double.TryParse(perltr, out ltrrate);
                                    }
                                    else
                                    {

                                    }
                                    double amt = ltrrate * ltrs;
                                    totalamount += amt;
                                    newrow["Per Ltr Rate"] = ltrrate;
                                    newrow["Amount"] = ltrrate * ltrs;
                                }
                            }
                            else if (source != "")
                            {
                                string fromsiloid = dr["FromSiloId"].ToString();
                                double ltrcostsp = 0;
                                double milkvaluetotalp = 0;
                                double kgstotalp = 0;
                                double perltrp = 0;
                                if (dtsiloid.Rows.Count > 0)
                                {
                                    foreach (DataRow dra in dtsiloid.Select("siloid='" + fromsiloid + "'"))
                                    {
                                        string dcno = dra["dcno"].ToString();
                                        string vendorid = dra["ccid"].ToString();
                                        if (vendorid == "")
                                        {
                                            vendorid = "0";
                                        }

                                        foreach (DataRow drr in dtmilk.Select("dcno='" + dcno + "' AND sectionid='" + vendorid + "'"))
                                        {
                                            try
                                            {
                                                string milktype = drr["milktype"].ToString();
                                                double kgfattotalp = 0;
                                                double kgsnftotalp = 0;
                                                double Ltrstotalp = 0;
                                                double TStotalp = 0;
                                                double mvaluetotalp = 0;
                                                double ohtotalp = 0;
                                                double snf9totalp = 0;
                                                if (milktype == "Buffalo")
                                                {
                                                    double qty_ltrp = 0;
                                                    double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                    double FAT = 0;
                                                    double.TryParse(drr["fat"].ToString(), out FAT);
                                                    FAT = Math.Round(FAT, 2);
                                                    double SNF = 0;
                                                    double.TryParse(drr["snf"].ToString(), out SNF);
                                                    string Rateon = drr["rate_on"].ToString();
                                                    double weight = 0;
                                                    double KGFAT = 0;
                                                    double KGSNF = 0;
                                                    double ltrsp = 0;
                                                    double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                    Ltrstotalp += ltrsp;
                                                    double Kgsp = 0;
                                                    double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                    kgstotalp += Kgsp;
                                                    double tstotal = 0;
                                                    tstotal = FAT + SNF;
                                                    if (Rateon == "TS")
                                                    {
                                                        double TS = 0;
                                                        TS = FAT + SNF;
                                                        weight = (TS * Kgsp) / 100;
                                                        KGFAT = (FAT * Kgsp) / 100;
                                                        KGSNF = (SNF * Kgsp) / 100;
                                                    }
                                                    else if (Rateon == "KGFAT")
                                                    {
                                                        weight = (FAT * Kgsp) / 100;
                                                        KGFAT = (FAT * Kgsp) / 100;
                                                        KGSNF = (SNF * Kgsp) / 100;
                                                    }
                                                    else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                    {
                                                        string CalOn = drr["calc_on"].ToString();
                                                        if (CalOn == "Ltrs")
                                                        {
                                                            weight = ltrsp;
                                                            KGFAT = (FAT * ltrsp) / 100;
                                                            KGSNF = (SNF * ltrsp) / 100;
                                                        }
                                                        else
                                                        {
                                                            weight = Kgsp;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                    }
                                                    double cost = 0;
                                                    double.TryParse(drr["cost"].ToString(), out cost);
                                                    KGFAT = Math.Round(KGFAT, 2);
                                                    kgfattotalp += KGFAT;
                                                    KGSNF = Math.Round(KGSNF, 2);
                                                    kgsnftotalp += KGSNF;
                                                    double MValue = 0;
                                                    MValue = KGFAT * cost;
                                                    MValue = Math.Round(MValue, 2);
                                                    mvaluetotalp += MValue;
                                                    string OverheadOn = drr["overheadon"].ToString();
                                                    double OHcost = 0;
                                                    double overheadcost = 0;
                                                    double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                    if (OverheadOn == "Ltrs")
                                                    {
                                                        OHcost = overheadcost * ltrsp;
                                                    }
                                                    else
                                                    {
                                                        OHcost = overheadcost * Kgsp;
                                                    }
                                                    double MSnf = 0;
                                                    double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                    double m_snfpluscost = 0;
                                                    double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                    double DiffSNFCost = 0;
                                                    if (SNF < MSnf)
                                                    {
                                                        string SNFOn = drr["snfplus_on"].ToString();
                                                        double diffSNF = 0;
                                                        diffSNF = SNF - MSnf;
                                                        diffSNF = Math.Round(diffSNF, 2);
                                                        if (SNFOn == "Ltrs")
                                                        {
                                                            DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                        }
                                                        else
                                                        {
                                                            DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                        }
                                                    }
                                                    double p_snfpluscost = 0;
                                                    double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                    double PSnf = 0;
                                                    double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                    if (SNF > PSnf)
                                                    {
                                                        string SNFOn = drr["snfplus_on"].ToString();
                                                        double diffSNF = 0;
                                                        diffSNF = SNF - MSnf;
                                                        if (SNFOn == "Ltrs")
                                                        {
                                                            DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                        }
                                                        else
                                                        {
                                                            DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                        }
                                                    }
                                                    double OHandMvalue = 0;
                                                    OHandMvalue = MValue + OHcost + DiffSNFCost;
                                                    ohtotalp += OHcost;
                                                    DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                    snf9totalp += DiffSNFCost;
                                                    OHandMvalue = Math.Round(OHandMvalue, 2);
                                                    milkvaluetotalp += OHandMvalue;
                                                    double ltrcostp = OHandMvalue / ltrsp;
                                                    ltrcostp = Math.Round(ltrcostp, 2);
                                                    ltrcostsp += ltrcostp;
                                                }
                                                else if (milktype == "Cow" || milktype == "Condensed")
                                                {
                                                    double qty_kgsp = 0;
                                                    double.TryParse(drr["qty_kgs"].ToString(), out qty_kgsp);
                                                    double qty_ltrp = 0;
                                                    double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                    double FAT = 0;
                                                    double.TryParse(drr["fat"].ToString(), out FAT);
                                                    FAT = Math.Round(FAT, 2);
                                                    double SNF = 0;
                                                    double.TryParse(drr["snf"].ToString(), out SNF);
                                                    string Rateon = drr["rate_on"].ToString();
                                                    double weight = 0;
                                                    double KGFAT = 0;
                                                    double KGSNF = 0;
                                                    double ltrsp = 0;
                                                    double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                    Ltrstotalp += ltrsp;
                                                    double Kgsp = 0;
                                                    double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                    kgstotalp += Kgsp;
                                                    double tstotal = 0;
                                                    tstotal = FAT + SNF;
                                                    if (Rateon == "TS")
                                                    {
                                                        double TS = 0;
                                                        TS = FAT + SNF;
                                                        weight = (TS * Kgsp) / 100;
                                                        KGFAT = (FAT * Kgsp) / 100;
                                                        KGSNF = (SNF * Kgsp) / 100;
                                                    }
                                                    else if (Rateon == "KGFAT")
                                                    {
                                                        weight = (FAT * Kgsp) / 100;
                                                        KGFAT = (FAT * Kgsp) / 100;
                                                        KGSNF = (SNF * Kgsp) / 100;
                                                    }
                                                    else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                    {
                                                        string CalOn = drr["calc_on"].ToString();
                                                        if (CalOn == "Ltrs")
                                                        {
                                                            weight = ltrsp;
                                                            KGFAT = (FAT * ltrsp) / 100;
                                                            KGSNF = (SNF * ltrsp) / 100;
                                                        }
                                                        else
                                                        {
                                                            weight = Kgsp;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                    }
                                                    double cost = 0;
                                                    double.TryParse(drr["cost"].ToString(), out cost);
                                                    KGFAT = Math.Round(KGFAT, 2);
                                                    kgfattotalp += KGFAT;
                                                    KGSNF = Math.Round(KGSNF, 2);
                                                    kgsnftotalp += KGSNF;
                                                    double MValue = 0;
                                                    if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                    {
                                                        MValue = cost * qty_kgsp;
                                                    }
                                                    else
                                                    {
                                                        MValue = tstotal * cost * qty_ltrp;
                                                        MValue = MValue / 100;
                                                    }
                                                    MValue = Math.Round(MValue, 2);
                                                    mvaluetotalp += MValue;
                                                    string OverheadOn = drr["overheadon"].ToString();
                                                    double OHcost = 0;
                                                    double overheadcost = 0;
                                                    double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                    if (OverheadOn == "Ltrs")
                                                    {
                                                        OHcost = overheadcost * ltrsp;
                                                    }
                                                    else
                                                    {
                                                        OHcost = overheadcost * Kgsp;
                                                    }
                                                    double MSnf = 0;
                                                    double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                    double m_snfpluscost = 0;
                                                    double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                    double DiffSNFCost = 0;
                                                    if (SNF < MSnf)
                                                    {
                                                        string SNFOn = drr["snfplus_on"].ToString();
                                                        double diffSNF = 0;
                                                        diffSNF = SNF - MSnf;
                                                        diffSNF = Math.Round(diffSNF, 2);
                                                        if (SNFOn == "Ltrs")
                                                        {
                                                            DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                        }
                                                        else
                                                        {
                                                            DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                        }
                                                    }
                                                    double p_snfpluscost = 0;
                                                    double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                    double PSnf = 0;
                                                    double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                    if (SNF > PSnf)
                                                    {
                                                        string SNFOn = drr["snfplus_on"].ToString();
                                                        double diffSNF = 0;
                                                        diffSNF = SNF - MSnf;
                                                        if (SNFOn == "Ltrs")
                                                        {
                                                            DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                        }
                                                        else
                                                        {
                                                            DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                        }
                                                    }

                                                    double MFat = 0;
                                                    double.TryParse(drr["m_std_fat"].ToString(), out MFat);
                                                    double m_fatpluscost = 0;
                                                    double.TryParse(drr["m_fatpluscost"].ToString(), out m_fatpluscost);
                                                    double DiffFATCost = 0;
                                                    if (FAT < MFat)
                                                    {
                                                        string FATOn = drr["fatplus_on"].ToString();
                                                        double diffFAT = 0;
                                                        diffFAT = FAT - MFat;
                                                        diffFAT = Math.Round(diffFAT, 2);
                                                        if (FATOn == "Ltrs")
                                                        {
                                                            DiffFATCost = diffFAT * ltrsp * m_fatpluscost * 10;
                                                        }
                                                        else
                                                        {
                                                            DiffFATCost = diffFAT * Kgsp * m_fatpluscost * 10;
                                                        }
                                                    }
                                                    double p_fatpluscost = 0;
                                                    double.TryParse(drr["p_fatpluscost"].ToString(), out p_fatpluscost);
                                                    double PFat = 0;
                                                    double.TryParse(drr["p_std_fat"].ToString(), out PFat);
                                                    if (FAT > PFat)
                                                    {
                                                        string FATOn = drr["fatplus_on"].ToString();
                                                        double diffFAT = 0;
                                                        diffFAT = FAT - PFat;
                                                        if (FATOn == "Ltrs")
                                                        {
                                                            DiffFATCost = diffFAT * ltrsp * p_fatpluscost * 10;
                                                        }
                                                        else
                                                        {
                                                            DiffFATCost = diffFAT * Kgsp * p_fatpluscost * 10;
                                                        }
                                                    }
                                                    DiffFATCost = Math.Round(DiffFATCost, 2);
                                                    double OHandMvalue = 0;
                                                    OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                                                    ohtotalp += OHcost;
                                                    DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                    snf9totalp += DiffSNFCost;
                                                    OHandMvalue = Math.Round(OHandMvalue, 2);
                                                    milkvaluetotalp += OHandMvalue;
                                                    double ltrcostp = OHandMvalue / ltrsp;
                                                    ltrcostp = Math.Round(ltrcostp, 2);
                                                    ltrcostsp += ltrcostp;
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    double tkgs = kgstotalp;
                                    double ttotalmilkp = milkvaluetotalp;
                                    if (milkvaluetotalp == 0)
                                    {
                                        perltrp = 0;
                                    }
                                    else
                                    {
                                        perltrp = ttotalmilkp / tkgs;
                                    }
                                    perltrp = Math.Round(perltrp, 2);
                                }
                                //double ltrrate = 0;
                                //string perltr = dr["perltrrate"].ToString();
                                //if (perltr != "")
                                //{
                                //    double.TryParse(perltr, out ltrrate);
                                //}
                                //else
                                //{

                                //}
                                double amt = perltrp * ltrs;
                                totalamount += amt;
                                newrow["Per Ltr Rate"] = perltrp;
                                newrow["Amount"] = perltrp * ltrs;
                            }
                            #endregion
                            newrow["Source"] = source;
                            Report.Rows.Add(newrow);
                        }
                    }
                    //DataRow New1 = Report.NewRow();
                    //New1["To Silo"] = "SMP";
                    //New1["KGS"] = TOTsmp;
                    //New1["LTR SNF"] = smptotal;
                    //Report.Rows.Add(New1);

                    DataRow newvartical1 = Report.NewRow();
                    newvartical1["Source"] = "Total";
                    //newvartical1["KGS"] = kgstotal;
                    //double og = Convert.ToDouble(ob);
                    newvartical1["LTRS"] = Ltrstotal;
                    double mfattotal = 0;
                    mfattotal = (kgfattotal / kgstotal) * 100;
                    mfattotal = Math.Round(mfattotal, 2);
                    newvartical1["FAT"] = mfattotal;
                    double msnftotal = 0;
                    double msmpkgsnftotal = 0;
                    msmpkgsnftotal = smptotal + kgsnftotal;
                    msnftotal = (msmpkgsnftotal / kgstotal) * 100;
                    msnftotal = Math.Round(msnftotal, 2);
                    newvartical1["SNF"] = msnftotal;
                    newvartical1["KG FAT"] = kgfattotal;
                    newvartical1["KG SNF"] = msmpkgsnftotal;
                    newvartical1["SMP"] = smptotal;
                    // newvartical1["CV"] = closingvalue;

                    if (ddlBatchType.SelectedValue == "Batch Wise")
                    {
                        //  newvartical1["CV"] = closingvalue;
                    }
                    else
                    {
                        newvartical1["KGS"] = kgstotal;
                        newvartical1["LTRS"] = Ltrstotal;
                    }
                    newvartical1["Amount"] = totalamount;
                    double standa = totalamount / Ltrstotal;
                    newvartical1["stand"] = Math.Round(standa, 2);
                    Report.Rows.Add(newvartical1);

                    DataRow newvartical3 = Report.NewRow();
                    newvartical3["Source"] = "Grand Total";
                    newvartical3["KGS"] = gkgstotal;
                    newvartical3["LTRS"] = gLtrstotal;
                    double gfattotal = 0;
                    gfattotal = (gkgfattotal / gkgstotal) * 100;
                    gfattotal = Math.Round(gfattotal, 2);
                    newvartical3["FAT"] = gfattotal;
                    double gsnftotal = 0;
                    double gsmpkgsnftotal = 0;
                    gsmpkgsnftotal = gsmptotal + gkgsnftotal;
                    gsnftotal = (gsmpkgsnftotal / gkgstotal) * 100;
                    gsnftotal = Math.Round(gsnftotal, 2);
                    newvartical3["SNF"] = gsnftotal;
                    newvartical3["KG FAT"] = gkgfattotal;
                    newvartical3["KG SNF"] = gsmpkgsnftotal;
                    newvartical3["SMP"] = gsmptotal;
                    Report.Rows.Add(newvartical3);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                    hidepanel.Visible = true;
                }
            }
            else if (ddlBatchType.SelectedValue == "All")
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("Date");
                Report.Columns.Add("Batch Name");
                Report.Columns.Add("Source");
                //Report.Columns.Add("OB");
                Report.Columns.Add("KGS").DataType = typeof(Double);
                Report.Columns.Add("LTRS").DataType = typeof(Double);
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("CLR");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("SMP");
                Report.Columns.Add("Per Ltr Rate");
                Report.Columns.Add("Amount");
                Report.Columns.Add("stand");
                cmd = new SqlCommand("SELECT silomaster.SiloName AS FromSilo,silomaster.SiloId AS FromSiloId,processingdepartments.departmentid, silomaster_1.SiloName AS ToSIlo, batchentrydetails.perltrrate, batchentrydetails.type, batchentrydetails.tosiloid, batchentrydetails.smp, batchentrydetails.qty_kgs, batchentrydetails.qty_ltrs,batchentrydetails.fat, batchentrydetails.snf, CONVERT(VARCHAR(24),batchentrydetails.doe, 103) AS doe, batchentrydetails.type, batchentrydetails.clr, batchentrydetails.siloqty, vendors.vendorname, batchmaster.batch,processingdepartments.departmentname FROM silomaster AS silomaster_1 INNER JOIN batchentrydetails ON silomaster_1.SiloId = batchentrydetails.tosiloid INNER JOIN batchmaster ON batchentrydetails.batchid = batchmaster.batchid LEFT OUTER JOIN processingdepartments ON batchentrydetails.fromdeptid = processingdepartments.departmentid LEFT OUTER JOIN vendors ON batchentrydetails.fromccid = vendors.sno LEFT OUTER JOIN silomaster ON batchentrydetails.fromsiloid = silomaster.SiloId  WHERE (batchentrydetails.doe BETWEEN @d1 AND @d2) AND (batchentrydetails.branchid = @branchid) ORDER BY doe, batchmaster.batchid ");
                cmd.Parameters.Add("@branchid", branchid);
                cmd.Parameters.Add("@d1", dtfromdate);
                cmd.Parameters.Add("@d2", dttodate);
                DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];

                cmd = new SqlCommand("SELECT sno, dcno, cellname, siloid, qty_kgs, qty_ltrs, fat, snf, clr, date, enterby, branchid, otherpartyname, ccid, deptid FROM silo_inward_transaction WHERE (branchid = @branchid) and date between @d1 and @d2");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                DataTable dtsiloid = SalesDB.SelectQuery(cmd).Tables[0];

                cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.sectionid,milktransactions.transportvalue, milktransactions.inwardno AS InwardNo, milktransactions.partydcno, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost, milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.kms,milktransaction_logs.p_std_fat, milktransaction_logs.fatplus_on FROM  milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE (milktransactions.doe BETWEEN @d1 AND @d2) and milktransactions.branchid=@branchid ORDER BY milktransactions.sno DESC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dtmilk = vdm.SelectQuery(cmd).Tables[0];

                cmd = new SqlCommand("SELECT   siloid, departmentid, qty_kgs, qty_ltrs, date FROM  silo_outward_transaction WHERE   (date BETWEEN @d1 AND @d2) AND (branchid = @branchid)");
                cmd.Parameters.Add("@d1", dtfdate);
                cmd.Parameters.Add("@d2", dttdate);
                cmd.Parameters.Add("branchid", branchid);
                DataTable dtreturnmilk = SalesDB.SelectQuery(cmd).Tables[0];

                if (dtInward.Rows.Count > 0)
                {
                    int i = 1;
                    int sj = 1;
                    double kgclosingsnftotal = 0;
                    double kgclosingfattotal = 0;
                    double bopeningtotal = 0;
                    double kgfattotal = 0;
                    double kgsnftotal = 0;
                    double kgstotal = 0;
                    double Ltrstotal = 0;
                    double smptotal = 0;
                    double TOTsmp = 0;
                    double closingvalue = 0;
                    //grand total
                    double gkgfattotal = 0;
                    double gkgsnftotal = 0;
                    double gkgstotal = 0;
                    double gboopening = 0;
                    double gLtrstotal = 0;
                    double gsmptotal = 0;
                    double gTOTsmp = 0;
                    double totalamount = 0;
                    double kgclosingsnftotalb = 0;
                    double kgclosingfattotalb = 0;
                    double bopeningtotalb = 0;
                    double kgfattotalb = 0;
                    double kgsnftotalb = 0;
                    double kgstotalb = 0;
                    double Ltrstotalb = 0;
                    double smptotalb = 0;
                    double TOTsmpb = 0;
                    double closingvalueb = 0;
                    //grand total
                    double gkgfattotalb = 0;
                    double gkgsnftotalb = 0;
                    double gkgstotalb = 0;
                    double gboopeningb = 0;
                    double gLtrstotalb = 0;
                    double gsmptotalb = 0;
                    double gTOTsmpb = 0;
                    double totalamountb = 0;
                    string ob = "";
                    DateTime dt = DateTime.Now;
                    string prevdate = string.Empty;
                    string sessiondatetime = string.Empty;
                    string hiddenvalue = string.Empty;
                    string priviousbatchname = "";
                    foreach (DataRow dr in dtInward.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        //DateTime dtdoe = Convert.ToDate(dr["doe"].ToString());
                        //string currentdate = dtdoe.ToString();
                        string date = dr["doe"].ToString();
                        //string siloid = dr["tosiloid"].ToString();
                        newrow["Date"] = date;
                        string branchname = dr["batch"].ToString();
                        if (branchname == priviousbatchname)
                        {
                            double bopening = 0;
                            bopeningtotal = bopening;
                            gboopening += bopening;
                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                            newrow["KGS"] = Kgs;
                            kgstotal += Kgs;
                            gkgstotal += Kgs;
                            double ltrs = 0;
                            double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                            Ltrstotal += ltrs;
                            gLtrstotal += ltrs;
                            newrow["LTRS"] = ltrs;
                            // newrow["CV"] = ltrs + bopening;
                            // closingvalue = ltrs + bopening;
                            double fat = 0;
                            double.TryParse(dr["fat"].ToString(), out fat);
                            newrow["FAT"] = fat;
                            double snfr = 0;
                            double.TryParse(dr["snf"].ToString(), out snfr);
                            newrow["SNF"] = snfr;
                            newrow["CLR"] = dr["clr"].ToString();
                            double ltrfatr = 0;
                            ltrfatr = Kgs * fat;
                            ltrfatr = Math.Round(ltrfatr / 100, 2);
                            newrow["KG FAT"] = ltrfatr;
                            kgfattotal += ltrfatr;
                            gkgfattotal += ltrfatr;
                            double ltrsnfs = 0;
                            ltrsnfs = Kgs * snfr;
                            ltrsnfs = Math.Round(ltrsnfs / 100, 2);
                            newrow["KG SNF"] = ltrsnfs;
                            double smpp = 0;
                            double.TryParse(dr["smp"].ToString(), out smpp);
                            TOTsmp += smpp;
                            gTOTsmp += smpp;
                            smpp = smpp * 0.97;
                            newrow["SMP"] = smpp;
                            smptotal += smpp;
                            gsmptotal += smpp;
                            kgsnftotal += ltrsnfs;
                            gkgsnftotal += ltrsnfs;
                            // Rate per liter Calucation
                            string source = dr["FromSilo"].ToString();
                            if (source == "")
                            {
                                source = dr["vendorname"].ToString();
                                if (source == "")
                                {
                                    source = dr["departmentname"].ToString();
                                    double perltrp = 0;
                                    string type = dr["type"].ToString();
                                    string deptid = dr["departmentid"].ToString();
                                    if (type == "Return Milk" || type == "Mixed Milk" || type == "Cutting Milk")
                                    {
                                        double ltrcostsp = 0;
                                        double milkvaluetotalp = 0;
                                        double kgstotalp = 0;
                                        string siloid = "";
                                        foreach (DataRow drd in dtreturnmilk.Select("departmentid='" + deptid + "'"))
                                        {
                                            siloid = drd["siloid"].ToString();
                                            foreach (DataRow dra in dtsiloid.Select("siloid='" + siloid + "'"))
                                            {
                                                string dcno = dra["dcno"].ToString();
                                                string vendorid = dra["ccid"].ToString();
                                                if (vendorid == "")
                                                {
                                                    vendorid = "0";
                                                }
                                                foreach (DataRow drr in dtmilk.Select("dcno='" + dcno + "' AND sectionid='" + vendorid + "'"))
                                                {
                                                    if (dtmilk.Rows.Count > 0)
                                                    {
                                                        string milktype = drr["milktype"].ToString();
                                                        double kgfattotalp = 0;
                                                        double kgsnftotalp = 0;
                                                        double Ltrstotalp = 0;
                                                        double TStotalp = 0;
                                                        double mvaluetotalp = 0;
                                                        double ohtotalp = 0;
                                                        double snf9totalp = 0;
                                                        if (milktype == "Buffalo")
                                                        {
                                                            double qty_ltrp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                            double FAT = 0;
                                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                                            FAT = Math.Round(FAT, 2);
                                                            double SNF = 0;
                                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                                            string Rateon = drr["rate_on"].ToString();
                                                            double weight = 0;
                                                            double KGFAT = 0;
                                                            double KGSNF = 0;
                                                            double ltrsp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                            Ltrstotalp += ltrsp;
                                                            double Kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                            kgstotalp += Kgsp;
                                                            double tstotal = 0;
                                                            tstotal = FAT + SNF;
                                                            if (Rateon == "TS")
                                                            {
                                                                double TS = 0;
                                                                TS = FAT + SNF;
                                                                weight = (TS * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "KGFAT")
                                                            {
                                                                weight = (FAT * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                string CalOn = drr["calc_on"].ToString();
                                                                if (CalOn == "Ltrs")
                                                                {
                                                                    weight = ltrsp;
                                                                    KGFAT = (FAT * ltrsp) / 100;
                                                                    KGSNF = (SNF * ltrsp) / 100;
                                                                }
                                                                else
                                                                {
                                                                    weight = Kgsp;
                                                                    KGFAT = (FAT * Kgsp) / 100;
                                                                    KGSNF = (SNF * Kgsp) / 100;
                                                                }
                                                            }
                                                            double cost = 0;
                                                            double.TryParse(drr["cost"].ToString(), out cost);
                                                            KGFAT = Math.Round(KGFAT, 2);
                                                            kgfattotalp += KGFAT;
                                                            KGSNF = Math.Round(KGSNF, 2);
                                                            kgsnftotalp += KGSNF;
                                                            double MValue = 0;
                                                            MValue = KGFAT * cost;
                                                            MValue = Math.Round(MValue, 2);
                                                            mvaluetotalp += MValue;
                                                            string OverheadOn = drr["overheadon"].ToString();
                                                            double OHcost = 0;
                                                            double overheadcost = 0;
                                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                            if (OverheadOn == "Ltrs")
                                                            {
                                                                OHcost = overheadcost * ltrsp;
                                                            }
                                                            else
                                                            {
                                                                OHcost = overheadcost * Kgsp;
                                                            }
                                                            double MSnf = 0;
                                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                            double m_snfpluscost = 0;
                                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                            double DiffSNFCost = 0;
                                                            if (SNF < MSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                diffSNF = Math.Round(diffSNF, 2);
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                                }
                                                            }
                                                            double p_snfpluscost = 0;
                                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                            double PSnf = 0;
                                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                            if (SNF > PSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                                }
                                                            }
                                                            double OHandMvalue = 0;
                                                            OHandMvalue = MValue + OHcost + DiffSNFCost;
                                                            ohtotalp += OHcost;
                                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                            snf9totalp += DiffSNFCost;
                                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                                            milkvaluetotalp += OHandMvalue;
                                                            double ltrcostp = OHandMvalue / ltrsp;
                                                            ltrcostp = Math.Round(ltrcostp, 2);
                                                            ltrcostsp += ltrcostp;
                                                        }
                                                        else if (milktype == "Cow" || milktype == "Condensed")
                                                        {
                                                            double qty_kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out qty_kgsp);
                                                            double qty_ltrp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                            double FAT = 0;
                                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                                            FAT = Math.Round(FAT, 2);
                                                            double SNF = 0;
                                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                                            string Rateon = drr["rate_on"].ToString();
                                                            double weight = 0;
                                                            double KGFAT = 0;
                                                            double KGSNF = 0;
                                                            double ltrsp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                            Ltrstotalp += ltrsp;
                                                            double Kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                            kgstotalp += Kgsp;
                                                            double tstotal = 0;
                                                            tstotal = FAT + SNF;
                                                            if (Rateon == "TS")
                                                            {
                                                                double TS = 0;
                                                                TS = FAT + SNF;
                                                                weight = (TS * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "KGFAT")
                                                            {
                                                                weight = (FAT * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                string CalOn = drr["calc_on"].ToString();
                                                                if (CalOn == "Ltrs")
                                                                {
                                                                    weight = ltrsp;
                                                                    KGFAT = (FAT * ltrsp) / 100;
                                                                    KGSNF = (SNF * ltrsp) / 100;
                                                                }
                                                                else
                                                                {
                                                                    weight = Kgsp;
                                                                    KGFAT = (FAT * Kgsp) / 100;
                                                                    KGSNF = (SNF * Kgsp) / 100;
                                                                }
                                                            }
                                                            double cost = 0;
                                                            double.TryParse(drr["cost"].ToString(), out cost);
                                                            KGFAT = Math.Round(KGFAT, 2);
                                                            kgfattotalp += KGFAT;
                                                            KGSNF = Math.Round(KGSNF, 2);
                                                            kgsnftotalp += KGSNF;
                                                            double MValue = 0;
                                                            if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                MValue = cost * qty_kgsp;
                                                            }
                                                            else
                                                            {
                                                                MValue = tstotal * cost * qty_ltrp;
                                                                MValue = MValue / 100;
                                                            }
                                                            MValue = Math.Round(MValue, 2);
                                                            mvaluetotalp += MValue;
                                                            string OverheadOn = drr["overheadon"].ToString();
                                                            double OHcost = 0;
                                                            double overheadcost = 0;
                                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                            if (OverheadOn == "Ltrs")
                                                            {
                                                                OHcost = overheadcost * ltrsp;
                                                            }
                                                            else
                                                            {
                                                                OHcost = overheadcost * Kgsp;
                                                            }
                                                            double MSnf = 0;
                                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                            double m_snfpluscost = 0;
                                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                            double DiffSNFCost = 0;
                                                            if (SNF < MSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                diffSNF = Math.Round(diffSNF, 2);
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                                }
                                                            }
                                                            double p_snfpluscost = 0;
                                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                            double PSnf = 0;
                                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                            if (SNF > PSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                                }
                                                            }
                                                            double MFat = 0;
                                                            double.TryParse(drr["m_std_fat"].ToString(), out MFat);
                                                            double m_fatpluscost = 0;
                                                            double.TryParse(drr["m_fatpluscost"].ToString(), out m_fatpluscost);
                                                            double DiffFATCost = 0;
                                                            if (FAT < MFat)
                                                            {
                                                                string FATOn = drr["fatplus_on"].ToString();
                                                                double diffFAT = 0;
                                                                diffFAT = FAT - MFat;
                                                                diffFAT = Math.Round(diffFAT, 2);
                                                                if (FATOn == "Ltrs")
                                                                {
                                                                    DiffFATCost = diffFAT * ltrsp * m_fatpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffFATCost = diffFAT * Kgsp * m_fatpluscost * 10;
                                                                }
                                                            }
                                                            double p_fatpluscost = 0;
                                                            double.TryParse(drr["p_fatpluscost"].ToString(), out p_fatpluscost);
                                                            double PFat = 0;
                                                            double.TryParse(drr["p_std_fat"].ToString(), out PFat);
                                                            if (FAT > PFat)
                                                            {
                                                                string FATOn = drr["fatplus_on"].ToString();
                                                                double diffFAT = 0;
                                                                diffFAT = FAT - PFat;
                                                                if (FATOn == "Ltrs")
                                                                {
                                                                    DiffFATCost = diffFAT * ltrsp * p_fatpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffFATCost = diffFAT * Kgsp * p_fatpluscost * 10;
                                                                }
                                                            }
                                                            DiffFATCost = Math.Round(DiffFATCost, 2);
                                                            double OHandMvalue = 0;
                                                            OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                                                            ohtotalp += OHcost;
                                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                            snf9totalp += DiffSNFCost;
                                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                                            milkvaluetotalp += OHandMvalue;
                                                            double ltrcostp = OHandMvalue / ltrsp;
                                                            ltrcostp = Math.Round(ltrcostp, 2);
                                                            ltrcostsp += ltrcostp;
                                                        }
                                                    }
                                                }
                                            }
                                            double tkgs = kgstotalp;
                                            double ttotalmilkp = milkvaluetotalp;
                                            if (milkvaluetotalp == 0)
                                            {
                                                perltrp = 0;
                                            }
                                            else
                                            {
                                                perltrp = ttotalmilkp / tkgs;
                                            }
                                            perltrp = Math.Round(perltrp, 2);
                                        }
                                        double amt = perltrp * ltrs;
                                        totalamount += amt;
                                        newrow["Per Ltr Rate"] = perltrp;
                                        newrow["Amount"] = perltrp * ltrs;
                                    }
                                }
                                else
                                {
                                    double ltrrate = 0;
                                    string perltr = dr["perltrrate"].ToString();
                                    if (perltr != "")
                                    {
                                        double.TryParse(perltr, out ltrrate);
                                    }
                                    else
                                    {

                                    }
                                    double amt = ltrrate * ltrs;
                                    totalamount += amt;
                                    newrow["Per Ltr Rate"] = ltrrate;
                                    newrow["Amount"] = ltrrate * ltrs;
                                }
                            }
                            else if (source != "")
                            {
                                string fromsiloid = dr["FromSiloId"].ToString();
                                double ltrcostsp = 0;
                                double milkvaluetotalp = 0;
                                double kgstotalp = 0;
                                double perltrp = 0;
                                if (dtsiloid.Rows.Count > 0)
                                {
                                    foreach (DataRow dra in dtsiloid.Select("siloid='" + fromsiloid + "'"))
                                    {
                                        string dcno = dra["dcno"].ToString();
                                        string vendorid = dra["ccid"].ToString();
                                        if (vendorid == "")
                                        {
                                            vendorid = "0";
                                        }
                                        foreach (DataRow drr in dtmilk.Select("dcno='" + dcno + "' AND sectionid='" + vendorid + "'"))
                                        {
                                            try
                                            {
                                                if (dtmilk.Rows.Count > 0)
                                                {
                                                    string milktype = drr["milktype"].ToString();
                                                    double kgfattotalp = 0;
                                                    double kgsnftotalp = 0;
                                                    double Ltrstotalp = 0;
                                                    double TStotalp = 0;
                                                    double mvaluetotalp = 0;
                                                    double ohtotalp = 0;
                                                    double snf9totalp = 0;
                                                    if (milktype == "Buffalo")
                                                    {

                                                        double qty_ltrp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                        double FAT = 0;
                                                        double.TryParse(drr["fat"].ToString(), out FAT);
                                                        FAT = Math.Round(FAT, 2);
                                                        double SNF = 0;
                                                        double.TryParse(drr["snf"].ToString(), out SNF);
                                                        string Rateon = drr["rate_on"].ToString();
                                                        double weight = 0;
                                                        double KGFAT = 0;
                                                        double KGSNF = 0;
                                                        double ltrsp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                        Ltrstotalp += ltrsp;
                                                        double Kgsp = 0;
                                                        double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                        kgstotalp += Kgsp;
                                                        double tstotal = 0;
                                                        tstotal = FAT + SNF;
                                                        if (Rateon == "TS")
                                                        {

                                                            double TS = 0;
                                                            TS = FAT + SNF;
                                                            weight = (TS * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "KGFAT")
                                                        {
                                                            weight = (FAT * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                        {
                                                            string CalOn = drr["calc_on"].ToString();
                                                            if (CalOn == "Ltrs")
                                                            {
                                                                weight = ltrsp;
                                                                KGFAT = (FAT * ltrsp) / 100;
                                                                KGSNF = (SNF * ltrsp) / 100;
                                                            }
                                                            else
                                                            {
                                                                weight = Kgsp;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                        }
                                                        double cost = 0;
                                                        double.TryParse(drr["cost"].ToString(), out cost);
                                                        KGFAT = Math.Round(KGFAT, 2);
                                                        kgfattotalp += KGFAT;
                                                        KGSNF = Math.Round(KGSNF, 2);
                                                        kgsnftotalp += KGSNF;
                                                        double MValue = 0;
                                                        MValue = KGFAT * cost;
                                                        MValue = Math.Round(MValue, 2);
                                                        mvaluetotalp += MValue;
                                                        string OverheadOn = drr["overheadon"].ToString();
                                                        double OHcost = 0;
                                                        double overheadcost = 0;
                                                        double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                        if (OverheadOn == "Ltrs")
                                                        {
                                                            OHcost = overheadcost * ltrsp;
                                                        }
                                                        else
                                                        {
                                                            OHcost = overheadcost * Kgsp;
                                                        }
                                                        double MSnf = 0;
                                                        double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                        double m_snfpluscost = 0;
                                                        double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                        double DiffSNFCost = 0;
                                                        if (SNF < MSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            diffSNF = Math.Round(diffSNF, 2);
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                            }
                                                        }
                                                        double p_snfpluscost = 0;
                                                        double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                        double PSnf = 0;
                                                        double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                        if (SNF > PSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                            }
                                                        }

                                                        double OHandMvalue = 0;
                                                        OHandMvalue = MValue + OHcost + DiffSNFCost;
                                                        ohtotalp += OHcost;
                                                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                        snf9totalp += DiffSNFCost;
                                                        OHandMvalue = Math.Round(OHandMvalue, 2);
                                                        milkvaluetotalp += OHandMvalue;
                                                        double ltrcostp = OHandMvalue / ltrsp;
                                                        ltrcostp = Math.Round(ltrcostp, 2);
                                                        ltrcostsp += ltrcostp;
                                                    }
                                                    else if (milktype == "Cow" || milktype == "Condensed")
                                                    {
                                                        double qty_kgsp = 0;
                                                        double.TryParse(drr["qty_kgs"].ToString(), out qty_kgsp);
                                                        double qty_ltrp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                        double FAT = 0;
                                                        double.TryParse(drr["fat"].ToString(), out FAT);
                                                        FAT = Math.Round(FAT, 2);
                                                        double SNF = 0;
                                                        double.TryParse(drr["snf"].ToString(), out SNF);
                                                        string Rateon = drr["rate_on"].ToString();
                                                        double weight = 0;
                                                        double KGFAT = 0;
                                                        double KGSNF = 0;
                                                        double ltrsp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                        Ltrstotalp += ltrsp;
                                                        double Kgsp = 0;
                                                        double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                        kgstotalp += Kgsp;
                                                        double tstotal = 0;
                                                        tstotal = FAT + SNF;
                                                        if (Rateon == "TS")
                                                        {

                                                            double TS = 0;
                                                            TS = FAT + SNF;
                                                            weight = (TS * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "KGFAT")
                                                        {
                                                            weight = (FAT * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                        {
                                                            string CalOn = drr["calc_on"].ToString();
                                                            if (CalOn == "Ltrs")
                                                            {
                                                                weight = ltrsp;
                                                                KGFAT = (FAT * ltrsp) / 100;
                                                                KGSNF = (SNF * ltrsp) / 100;
                                                            }
                                                            else
                                                            {
                                                                weight = Kgsp;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                        }
                                                        double cost = 0;
                                                        double.TryParse(drr["cost"].ToString(), out cost);
                                                        KGFAT = Math.Round(KGFAT, 2);
                                                        kgfattotalp += KGFAT;
                                                        KGSNF = Math.Round(KGSNF, 2);
                                                        kgsnftotalp += KGSNF;
                                                        double MValue = 0;
                                                        if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                        {
                                                            MValue = cost * qty_kgsp;
                                                        }
                                                        else
                                                        {
                                                            MValue = tstotal * cost * qty_ltrp;
                                                            MValue = MValue / 100;

                                                        }
                                                        MValue = Math.Round(MValue, 2);
                                                        mvaluetotalp += MValue;
                                                        string OverheadOn = drr["overheadon"].ToString();
                                                        double OHcost = 0;
                                                        double overheadcost = 0;
                                                        double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                        if (OverheadOn == "Ltrs")
                                                        {
                                                            OHcost = overheadcost * ltrsp;
                                                        }
                                                        else
                                                        {
                                                            OHcost = overheadcost * Kgsp;
                                                        }
                                                        double MSnf = 0;
                                                        double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                        double m_snfpluscost = 0;
                                                        double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                        double DiffSNFCost = 0;
                                                        if (SNF < MSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            diffSNF = Math.Round(diffSNF, 2);
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                            }
                                                        }
                                                        double p_snfpluscost = 0;
                                                        double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                        double PSnf = 0;
                                                        double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                        if (SNF > PSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                            }
                                                        }

                                                        double MFat = 0;
                                                        double.TryParse(drr["m_std_fat"].ToString(), out MFat);
                                                        double m_fatpluscost = 0;
                                                        double.TryParse(drr["m_fatpluscost"].ToString(), out m_fatpluscost);
                                                        double DiffFATCost = 0;
                                                        if (FAT < MFat)
                                                        {
                                                            string FATOn = drr["fatplus_on"].ToString();
                                                            double diffFAT = 0;
                                                            diffFAT = FAT - MFat;
                                                            diffFAT = Math.Round(diffFAT, 2);
                                                            if (FATOn == "Ltrs")
                                                            {
                                                                DiffFATCost = diffFAT * ltrsp * m_fatpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffFATCost = diffFAT * Kgsp * m_fatpluscost * 10;
                                                            }
                                                        }
                                                        double p_fatpluscost = 0;
                                                        double.TryParse(drr["p_fatpluscost"].ToString(), out p_fatpluscost);
                                                        double PFat = 0;
                                                        double.TryParse(drr["p_std_fat"].ToString(), out PFat);
                                                        if (FAT > PFat)
                                                        {
                                                            string FATOn = drr["fatplus_on"].ToString();
                                                            double diffFAT = 0;
                                                            diffFAT = FAT - PFat;
                                                            if (FATOn == "Ltrs")
                                                            {
                                                                DiffFATCost = diffFAT * ltrsp * p_fatpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffFATCost = diffFAT * Kgsp * p_fatpluscost * 10;
                                                            }
                                                        }
                                                        DiffFATCost = Math.Round(DiffFATCost, 2);
                                                        double OHandMvalue = 0;
                                                        OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                                                        ohtotalp += OHcost;
                                                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                        snf9totalp += DiffSNFCost;
                                                        OHandMvalue = Math.Round(OHandMvalue, 2);
                                                        milkvaluetotalp += OHandMvalue;
                                                        double ltrcostp = OHandMvalue / ltrsp;
                                                        ltrcostp = Math.Round(ltrcostp, 2);
                                                        ltrcostsp += ltrcostp;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    double tkgs = kgstotalp;
                                    double ttotalmilkp = milkvaluetotalp;
                                    if (milkvaluetotalp == 0)
                                    {
                                        perltrp = 0;
                                    }
                                    else
                                    {
                                        perltrp = ttotalmilkp / tkgs;
                                    }
                                    perltrp = Math.Round(perltrp, 2);
                                }
                                double amt = perltrp * ltrs;
                                totalamount += amt;
                                newrow["Per Ltr Rate"] = perltrp;
                                newrow["Amount"] = perltrp * ltrs;
                            }
                            newrow["Source"] = source;
                            Report.Rows.Add(newrow);
                        }
                        else
                        {
                            if (kgfattotal > 0)
                            {
                                DataRow newvartical34 = Report.NewRow();
                                newvartical34["Source"] = "Total";
                                //string val = hiddenvalue;
                                //double siloobvalue = Convert.ToDouble(val);
                                double fattotal = 0;
                                fattotal = (kgfattotal / kgstotal) * 100;
                                fattotal = Math.Round(fattotal, 2);
                                newvartical34["FAT"] = fattotal;
                                double snftotal = 0;
                                double smpkgsnftotal = 0;
                                smpkgsnftotal = smptotal + kgsnftotal;
                                snftotal = (smpkgsnftotal / kgstotal) * 100;
                                snftotal = Math.Round(snftotal, 2);
                                newvartical34["SNF"] = snftotal;
                                //newvartical2["LTR FAT"] = kgfattotal + kgclosingfattotal;
                                newvartical34["KG FAT"] = kgfattotal;
                                newvartical34["KG SNF"] = smpkgsnftotal;
                                newvartical34["SMP"] = smptotal;
                                //double opening = Convert.ToDouble(ob);
                                if (ddlBatchType.SelectedValue == "Batch Wise")
                                {
                                    // newvartical2["CV"] = closingvalue;
                                    newvartical34["LTRS"] = Ltrstotal;
                                }
                                else
                                {
                                    newvartical34["KGS"] = kgstotal;
                                    // newvartical2["CV"] = kgstotal + bopeningtotal;
                                    newvartical34["LTRS"] = Ltrstotal;
                                }
                                newvartical34["Amount"] = totalamount;
                                double stand = totalamount / Ltrstotal;
                                newvartical34["stand"] = Math.Round(stand, 2);
                                Report.Rows.Add(newvartical34);
                                kgfattotal = 0;
                                kgsnftotal = 0;
                                kgstotal = 0;
                                Ltrstotal = 0;
                                smptotal = 0;
                                TOTsmp = 0;
                                bopeningtotal = 0;
                                totalamount = 0;
                                hiddenvalue = string.Empty;
                            }
                            priviousbatchname = branchname;
                            //string strPlantime = dtdoe.ToString("dd/MMM/yyyy HH:mm");
                            //string[] PlanDateTime = strPlantime.Split(' ');
                            newrow["Date"] = dr["doe"].ToString();
                            newrow["Batch Name"] = dr["batch"].ToString();
                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                            newrow["KGS"] = Kgs;
                            kgstotal += Kgs;
                            gkgstotal += Kgs;
                            double ltrs = 0;
                            double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                            Ltrstotal += ltrs;
                            gLtrstotal += ltrs;
                            newrow["LTRS"] = ltrs;
                            double fat = 0;
                            double.TryParse(dr["fat"].ToString(), out fat);
                            newrow["FAT"] = fat;
                            double snfr = 0;
                            double.TryParse(dr["snf"].ToString(), out snfr);
                            newrow["SNF"] = snfr;
                            newrow["CLR"] = dr["clr"].ToString();
                            double ltrfatr = 0;
                            ltrfatr = Kgs * fat;
                            ltrfatr = Math.Round(ltrfatr / 100, 2);
                            newrow["KG FAT"] = ltrfatr;
                            kgfattotal += ltrfatr;
                            gkgfattotal += ltrfatr;
                            double ltrsnfs = 0;
                            ltrsnfs = Kgs * snfr;
                            ltrsnfs = Math.Round(ltrsnfs / 100, 2);
                            newrow["KG SNF"] = ltrsnfs;
                            double smpp = 0;
                            double.TryParse(dr["smp"].ToString(), out smpp);
                            TOTsmp += smpp;
                            gTOTsmp += smpp;
                            smpp = smpp * 0.97;
                            newrow["SMP"] = smpp;
                            smptotal += smpp;
                            gsmptotal += smpp;
                            kgsnftotal += ltrsnfs;
                            gkgsnftotal += ltrsnfs;
                            // Rate per liter Calucation
                            string source = dr["FromSilo"].ToString();
                            if (source == "")
                            {
                                source = dr["vendorname"].ToString();
                                if (source == "")
                                {
                                    source = dr["departmentname"].ToString();
                                    double perltrp = 0;
                                    string type = dr["type"].ToString();
                                    string deptid = dr["departmentid"].ToString();
                                    if (type == "Return Milk" || type == "Mixed Milk" || type == "Cutting Milk")
                                    {
                                        double ltrcostsp = 0;
                                        double milkvaluetotalp = 0;
                                        double kgstotalp = 0;
                                        string siloid = "";
                                        foreach (DataRow drd in dtreturnmilk.Select("departmentid='" + deptid + "'"))
                                        {
                                            siloid = drd["siloid"].ToString();
                                            foreach (DataRow dra in dtsiloid.Select("siloid='" + siloid + "'"))
                                            {
                                                string dcno = dra["dcno"].ToString();
                                                string vendorid = dra["ccid"].ToString();
                                                if (vendorid == "")
                                                {
                                                    vendorid = "0";
                                                }
                                                foreach (DataRow drr in dtmilk.Select("dcno='" + dcno + "' AND sectionid='" + vendorid + "'"))
                                                {
                                                    if (dtmilk.Rows.Count > 0)
                                                    {
                                                        string milktype = drr["milktype"].ToString();
                                                        double kgfattotalp = 0;
                                                        double kgsnftotalp = 0;
                                                        double Ltrstotalp = 0;
                                                        double TStotalp = 0;
                                                        double mvaluetotalp = 0;
                                                        double ohtotalp = 0;
                                                        double snf9totalp = 0;
                                                        if (milktype == "Buffalo")
                                                        {
                                                            double qty_ltrp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                            double FAT = 0;
                                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                                            FAT = Math.Round(FAT, 2);
                                                            double SNF = 0;
                                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                                            string Rateon = drr["rate_on"].ToString();
                                                            double weight = 0;
                                                            double KGFAT = 0;
                                                            double KGSNF = 0;
                                                            double ltrsp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                            Ltrstotalp += ltrsp;
                                                            double Kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                            kgstotalp += Kgsp;
                                                            double tstotal = 0;
                                                            tstotal = FAT + SNF;
                                                            if (Rateon == "TS")
                                                            {
                                                                double TS = 0;
                                                                TS = FAT + SNF;
                                                                weight = (TS * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "KGFAT")
                                                            {
                                                                weight = (FAT * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                string CalOn = drr["calc_on"].ToString();
                                                                if (CalOn == "Ltrs")
                                                                {
                                                                    weight = ltrsp;
                                                                    KGFAT = (FAT * ltrsp) / 100;
                                                                    KGSNF = (SNF * ltrsp) / 100;
                                                                }
                                                                else
                                                                {
                                                                    weight = Kgsp;
                                                                    KGFAT = (FAT * Kgsp) / 100;
                                                                    KGSNF = (SNF * Kgsp) / 100;
                                                                }
                                                            }
                                                            double cost = 0;
                                                            double.TryParse(drr["cost"].ToString(), out cost);
                                                            KGFAT = Math.Round(KGFAT, 2);
                                                            kgfattotalp += KGFAT;
                                                            KGSNF = Math.Round(KGSNF, 2);
                                                            kgsnftotalp += KGSNF;
                                                            double MValue = 0;
                                                            MValue = KGFAT * cost;
                                                            MValue = Math.Round(MValue, 2);
                                                            mvaluetotalp += MValue;
                                                            string OverheadOn = drr["overheadon"].ToString();
                                                            double OHcost = 0;
                                                            double overheadcost = 0;
                                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                            if (OverheadOn == "Ltrs")
                                                            {
                                                                OHcost = overheadcost * ltrsp;
                                                            }
                                                            else
                                                            {
                                                                OHcost = overheadcost * Kgsp;
                                                            }
                                                            double MSnf = 0;
                                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                            double m_snfpluscost = 0;
                                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                            double DiffSNFCost = 0;
                                                            if (SNF < MSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                diffSNF = Math.Round(diffSNF, 2);
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                                }
                                                            }
                                                            double p_snfpluscost = 0;
                                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                            double PSnf = 0;
                                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                            if (SNF > PSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                                }
                                                            }
                                                            double OHandMvalue = 0;
                                                            OHandMvalue = MValue + OHcost + DiffSNFCost;
                                                            ohtotalp += OHcost;
                                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                            snf9totalp += DiffSNFCost;
                                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                                            milkvaluetotalp += OHandMvalue;
                                                            double ltrcostp = OHandMvalue / ltrsp;
                                                            ltrcostp = Math.Round(ltrcostp, 2);
                                                            ltrcostsp += ltrcostp;
                                                        }
                                                        else if (milktype == "Cow" || milktype == "Condensed")
                                                        {
                                                            double qty_kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out qty_kgsp);
                                                            double qty_ltrp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                            double FAT = 0;
                                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                                            FAT = Math.Round(FAT, 2);
                                                            double SNF = 0;
                                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                                            string Rateon = drr["rate_on"].ToString();
                                                            double weight = 0;
                                                            double KGFAT = 0;
                                                            double KGSNF = 0;
                                                            double ltrsp = 0;
                                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                            Ltrstotalp += ltrsp;
                                                            double Kgsp = 0;
                                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                            kgstotalp += Kgsp;
                                                            double tstotal = 0;
                                                            tstotal = FAT + SNF;
                                                            if (Rateon == "TS")
                                                            {
                                                                double TS = 0;
                                                                TS = FAT + SNF;
                                                                weight = (TS * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "KGFAT")
                                                            {
                                                                weight = (FAT * Kgsp) / 100;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                string CalOn = drr["calc_on"].ToString();
                                                                if (CalOn == "Ltrs")
                                                                {
                                                                    weight = ltrsp;
                                                                    KGFAT = (FAT * ltrsp) / 100;
                                                                    KGSNF = (SNF * ltrsp) / 100;
                                                                }
                                                                else
                                                                {
                                                                    weight = Kgsp;
                                                                    KGFAT = (FAT * Kgsp) / 100;
                                                                    KGSNF = (SNF * Kgsp) / 100;
                                                                }
                                                            }
                                                            double cost = 0;
                                                            double.TryParse(drr["cost"].ToString(), out cost);
                                                            KGFAT = Math.Round(KGFAT, 2);
                                                            kgfattotalp += KGFAT;
                                                            KGSNF = Math.Round(KGSNF, 2);
                                                            kgsnftotalp += KGSNF;
                                                            double MValue = 0;
                                                            if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                            {
                                                                MValue = cost * qty_kgsp;
                                                            }
                                                            else
                                                            {
                                                                MValue = tstotal * cost * qty_ltrp;
                                                                MValue = MValue / 100;
                                                            }
                                                            MValue = Math.Round(MValue, 2);
                                                            mvaluetotalp += MValue;
                                                            string OverheadOn = drr["overheadon"].ToString();
                                                            double OHcost = 0;
                                                            double overheadcost = 0;
                                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                            if (OverheadOn == "Ltrs")
                                                            {
                                                                OHcost = overheadcost * ltrsp;
                                                            }
                                                            else
                                                            {
                                                                OHcost = overheadcost * Kgsp;
                                                            }
                                                            double MSnf = 0;
                                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                            double m_snfpluscost = 0;
                                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                            double DiffSNFCost = 0;
                                                            if (SNF < MSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                diffSNF = Math.Round(diffSNF, 2);
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                                }
                                                            }
                                                            double p_snfpluscost = 0;
                                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                            double PSnf = 0;
                                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                            if (SNF > PSnf)
                                                            {
                                                                string SNFOn = drr["snfplus_on"].ToString();
                                                                double diffSNF = 0;
                                                                diffSNF = SNF - MSnf;
                                                                if (SNFOn == "Ltrs")
                                                                {
                                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                                }
                                                            }
                                                            double MFat = 0;
                                                            double.TryParse(drr["m_std_fat"].ToString(), out MFat);
                                                            double m_fatpluscost = 0;
                                                            double.TryParse(drr["m_fatpluscost"].ToString(), out m_fatpluscost);
                                                            double DiffFATCost = 0;
                                                            if (FAT < MFat)
                                                            {
                                                                string FATOn = drr["fatplus_on"].ToString();
                                                                double diffFAT = 0;
                                                                diffFAT = FAT - MFat;
                                                                diffFAT = Math.Round(diffFAT, 2);
                                                                if (FATOn == "Ltrs")
                                                                {
                                                                    DiffFATCost = diffFAT * ltrsp * m_fatpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffFATCost = diffFAT * Kgsp * m_fatpluscost * 10;
                                                                }
                                                            }
                                                            double p_fatpluscost = 0;
                                                            double.TryParse(drr["p_fatpluscost"].ToString(), out p_fatpluscost);
                                                            double PFat = 0;
                                                            double.TryParse(drr["p_std_fat"].ToString(), out PFat);
                                                            if (FAT > PFat)
                                                            {
                                                                string FATOn = drr["fatplus_on"].ToString();
                                                                double diffFAT = 0;
                                                                diffFAT = FAT - PFat;
                                                                if (FATOn == "Ltrs")
                                                                {
                                                                    DiffFATCost = diffFAT * ltrsp * p_fatpluscost * 10;
                                                                }
                                                                else
                                                                {
                                                                    DiffFATCost = diffFAT * Kgsp * p_fatpluscost * 10;
                                                                }
                                                            }
                                                            DiffFATCost = Math.Round(DiffFATCost, 2);
                                                            double OHandMvalue = 0;
                                                            OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                                                            ohtotalp += OHcost;
                                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                            snf9totalp += DiffSNFCost;
                                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                                            milkvaluetotalp += OHandMvalue;
                                                            double ltrcostp = OHandMvalue / ltrsp;
                                                            ltrcostp = Math.Round(ltrcostp, 2);
                                                            ltrcostsp += ltrcostp;
                                                        }
                                                    }
                                                }
                                            }
                                            double tkgs = kgstotalp;
                                            double ttotalmilkp = milkvaluetotalp;
                                            if (milkvaluetotalp == 0)
                                            {
                                                perltrp = 0;
                                            }
                                            else
                                            {
                                                perltrp = ttotalmilkp / tkgs;
                                            }
                                            perltrp = Math.Round(perltrp, 2);
                                        }
                                        double amt = perltrp * ltrs;
                                        totalamount += amt;
                                        newrow["Per Ltr Rate"] = perltrp;
                                        newrow["Amount"] = perltrp * ltrs;
                                    }
                                }
                                else
                                {
                                    double ltrrate = 0;
                                    string perltr = dr["perltrrate"].ToString();
                                    if (perltr != "")
                                    {
                                        double.TryParse(perltr, out ltrrate);
                                    }
                                    else
                                    {

                                    }
                                    double amt = ltrrate * ltrs;
                                    totalamount += amt;
                                    newrow["Per Ltr Rate"] = ltrrate;
                                    newrow["Amount"] = ltrrate * ltrs;
                                }
                            }
                            else if (source != "")
                            {
                                string fromsiloid = dr["FromSiloId"].ToString();
                                double ltrcostsp = 0;
                                double milkvaluetotalp = 0;
                                double kgstotalp = 0;
                                double perltrp = 0;
                                if (dtsiloid.Rows.Count > 0)
                                {
                                    foreach (DataRow dra in dtsiloid.Select("siloid='" + fromsiloid + "'"))
                                    {
                                        string dcno = dra["dcno"].ToString();
                                        string vendorid = dra["ccid"].ToString();
                                        if (vendorid == "")
                                        {
                                            vendorid = "0";
                                        }
                                        foreach (DataRow drr in dtmilk.Select("dcno='" + dcno + "' AND sectionid='" + vendorid + "'"))
                                        {
                                            try
                                            {
                                                if (dtmilk.Rows.Count > 0)
                                                {
                                                    string milktype = drr["milktype"].ToString();
                                                    double kgfattotalp = 0;
                                                    double kgsnftotalp = 0;
                                                    double Ltrstotalp = 0;
                                                    double TStotalp = 0;
                                                    double mvaluetotalp = 0;
                                                    double ohtotalp = 0;
                                                    double snf9totalp = 0;
                                                    if (milktype == "Buffalo")
                                                    {

                                                        double qty_ltrp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                        double FAT = 0;
                                                        double.TryParse(drr["fat"].ToString(), out FAT);
                                                        FAT = Math.Round(FAT, 2);
                                                        double SNF = 0;
                                                        double.TryParse(drr["snf"].ToString(), out SNF);
                                                        string Rateon = drr["rate_on"].ToString();
                                                        double weight = 0;
                                                        double KGFAT = 0;
                                                        double KGSNF = 0;
                                                        double ltrsp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                        Ltrstotalp += ltrsp;
                                                        double Kgsp = 0;
                                                        double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                        kgstotalp += Kgsp;
                                                        double tstotal = 0;
                                                        tstotal = FAT + SNF;
                                                        if (Rateon == "TS")
                                                        {

                                                            double TS = 0;
                                                            TS = FAT + SNF;
                                                            weight = (TS * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "KGFAT")
                                                        {
                                                            weight = (FAT * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                        {
                                                            string CalOn = drr["calc_on"].ToString();
                                                            if (CalOn == "Ltrs")
                                                            {
                                                                weight = ltrsp;
                                                                KGFAT = (FAT * ltrsp) / 100;
                                                                KGSNF = (SNF * ltrsp) / 100;
                                                            }
                                                            else
                                                            {
                                                                weight = Kgsp;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                        }
                                                        double cost = 0;
                                                        double.TryParse(drr["cost"].ToString(), out cost);
                                                        KGFAT = Math.Round(KGFAT, 2);
                                                        kgfattotalp += KGFAT;
                                                        KGSNF = Math.Round(KGSNF, 2);
                                                        kgsnftotalp += KGSNF;
                                                        double MValue = 0;
                                                        MValue = KGFAT * cost;
                                                        MValue = Math.Round(MValue, 2);
                                                        mvaluetotalp += MValue;
                                                        string OverheadOn = drr["overheadon"].ToString();
                                                        double OHcost = 0;
                                                        double overheadcost = 0;
                                                        double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                        if (OverheadOn == "Ltrs")
                                                        {
                                                            OHcost = overheadcost * ltrsp;
                                                        }
                                                        else
                                                        {
                                                            OHcost = overheadcost * Kgsp;
                                                        }
                                                        double MSnf = 0;
                                                        double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                        double m_snfpluscost = 0;
                                                        double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                        double DiffSNFCost = 0;
                                                        if (SNF < MSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            diffSNF = Math.Round(diffSNF, 2);
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                            }
                                                        }
                                                        double p_snfpluscost = 0;
                                                        double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                        double PSnf = 0;
                                                        double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                        if (SNF > PSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                            }
                                                        }

                                                        double OHandMvalue = 0;
                                                        OHandMvalue = MValue + OHcost + DiffSNFCost;
                                                        ohtotalp += OHcost;
                                                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                        snf9totalp += DiffSNFCost;
                                                        OHandMvalue = Math.Round(OHandMvalue, 2);
                                                        milkvaluetotalp += OHandMvalue;
                                                        double ltrcostp = OHandMvalue / ltrsp;
                                                        ltrcostp = Math.Round(ltrcostp, 2);
                                                        ltrcostsp += ltrcostp;
                                                    }
                                                    else if (milktype == "Cow" || milktype == "Condensed")
                                                    {
                                                        double qty_kgsp = 0;
                                                        double.TryParse(drr["qty_kgs"].ToString(), out qty_kgsp);
                                                        double qty_ltrp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                                        double FAT = 0;
                                                        double.TryParse(drr["fat"].ToString(), out FAT);
                                                        FAT = Math.Round(FAT, 2);
                                                        double SNF = 0;
                                                        double.TryParse(drr["snf"].ToString(), out SNF);
                                                        string Rateon = drr["rate_on"].ToString();
                                                        double weight = 0;
                                                        double KGFAT = 0;
                                                        double KGSNF = 0;
                                                        double ltrsp = 0;
                                                        double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                                        Ltrstotalp += ltrsp;
                                                        double Kgsp = 0;
                                                        double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                                        kgstotalp += Kgsp;
                                                        double tstotal = 0;
                                                        tstotal = FAT + SNF;
                                                        if (Rateon == "TS")
                                                        {

                                                            double TS = 0;
                                                            TS = FAT + SNF;
                                                            weight = (TS * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "KGFAT")
                                                        {
                                                            weight = (FAT * Kgsp) / 100;
                                                            KGFAT = (FAT * Kgsp) / 100;
                                                            KGSNF = (SNF * Kgsp) / 100;
                                                        }
                                                        else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                        {
                                                            string CalOn = drr["calc_on"].ToString();
                                                            if (CalOn == "Ltrs")
                                                            {
                                                                weight = ltrsp;
                                                                KGFAT = (FAT * ltrsp) / 100;
                                                                KGSNF = (SNF * ltrsp) / 100;
                                                            }
                                                            else
                                                            {
                                                                weight = Kgsp;
                                                                KGFAT = (FAT * Kgsp) / 100;
                                                                KGSNF = (SNF * Kgsp) / 100;
                                                            }
                                                        }
                                                        double cost = 0;
                                                        double.TryParse(drr["cost"].ToString(), out cost);
                                                        KGFAT = Math.Round(KGFAT, 2);
                                                        kgfattotalp += KGFAT;
                                                        KGSNF = Math.Round(KGSNF, 2);
                                                        kgsnftotalp += KGSNF;
                                                        double MValue = 0;
                                                        if (Rateon == "PerLtr" || Rateon == "PerKg")
                                                        {
                                                            MValue = cost * qty_kgsp;
                                                        }
                                                        else
                                                        {
                                                            MValue = tstotal * cost * qty_ltrp;
                                                            MValue = MValue / 100;

                                                        }
                                                        MValue = Math.Round(MValue, 2);
                                                        mvaluetotalp += MValue;
                                                        string OverheadOn = drr["overheadon"].ToString();
                                                        double OHcost = 0;
                                                        double overheadcost = 0;
                                                        double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                                        if (OverheadOn == "Ltrs")
                                                        {
                                                            OHcost = overheadcost * ltrsp;
                                                        }
                                                        else
                                                        {
                                                            OHcost = overheadcost * Kgsp;
                                                        }
                                                        double MSnf = 0;
                                                        double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                                        double m_snfpluscost = 0;
                                                        double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                                        double DiffSNFCost = 0;
                                                        if (SNF < MSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            diffSNF = Math.Round(diffSNF, 2);
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                            }
                                                        }
                                                        double p_snfpluscost = 0;
                                                        double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                                        double PSnf = 0;
                                                        double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                                        if (SNF > PSnf)
                                                        {
                                                            string SNFOn = drr["snfplus_on"].ToString();
                                                            double diffSNF = 0;
                                                            diffSNF = SNF - MSnf;
                                                            if (SNFOn == "Ltrs")
                                                            {
                                                                DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                            }
                                                        }

                                                        double MFat = 0;
                                                        double.TryParse(drr["m_std_fat"].ToString(), out MFat);
                                                        double m_fatpluscost = 0;
                                                        double.TryParse(drr["m_fatpluscost"].ToString(), out m_fatpluscost);
                                                        double DiffFATCost = 0;
                                                        if (FAT < MFat)
                                                        {
                                                            string FATOn = drr["fatplus_on"].ToString();
                                                            double diffFAT = 0;
                                                            diffFAT = FAT - MFat;
                                                            diffFAT = Math.Round(diffFAT, 2);
                                                            if (FATOn == "Ltrs")
                                                            {
                                                                DiffFATCost = diffFAT * ltrsp * m_fatpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffFATCost = diffFAT * Kgsp * m_fatpluscost * 10;
                                                            }
                                                        }
                                                        double p_fatpluscost = 0;
                                                        double.TryParse(drr["p_fatpluscost"].ToString(), out p_fatpluscost);
                                                        double PFat = 0;
                                                        double.TryParse(drr["p_std_fat"].ToString(), out PFat);
                                                        if (FAT > PFat)
                                                        {
                                                            string FATOn = drr["fatplus_on"].ToString();
                                                            double diffFAT = 0;
                                                            diffFAT = FAT - PFat;
                                                            if (FATOn == "Ltrs")
                                                            {
                                                                DiffFATCost = diffFAT * ltrsp * p_fatpluscost * 10;
                                                            }
                                                            else
                                                            {
                                                                DiffFATCost = diffFAT * Kgsp * p_fatpluscost * 10;
                                                            }
                                                        }
                                                        DiffFATCost = Math.Round(DiffFATCost, 2);
                                                        double OHandMvalue = 0;
                                                        OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                                                        ohtotalp += OHcost;
                                                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                                        snf9totalp += DiffSNFCost;
                                                        OHandMvalue = Math.Round(OHandMvalue, 2);
                                                        milkvaluetotalp += OHandMvalue;
                                                        double ltrcostp = OHandMvalue / ltrsp;
                                                        ltrcostp = Math.Round(ltrcostp, 2);
                                                        ltrcostsp += ltrcostp;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    double tkgs = kgstotalp;
                                    double ttotalmilkp = milkvaluetotalp;
                                    if (milkvaluetotalp == 0)
                                    {
                                        perltrp = 0;
                                    }
                                    else
                                    {
                                        perltrp = ttotalmilkp / tkgs;
                                    }
                                    perltrp = Math.Round(perltrp, 2);
                                }
                                double amt = perltrp * ltrs;
                                totalamount += amt;
                                newrow["Per Ltr Rate"] = perltrp;
                                newrow["Amount"] = perltrp * ltrs;
                            }
                            newrow["Source"] = source;
                            Report.Rows.Add(newrow);
                        }
                    }
                    //DataRow newvartical12 = Report.NewRow();
                    //newvartical12["Source"] = "Total";
                    ////newvartical1["KGS"] = kgstotal;
                    ////double og = Convert.ToDouble(ob);
                    //newvartical12["LTRS"] = Ltrstotal;
                    //double mfattotal = 0;
                    //mfattotal = (kgfattotal / Ltrstotal) * 100;
                    //mfattotal = Math.Round(mfattotal, 2);
                    //newvartical12["FAT"] = mfattotal;
                    //double msnftotal = 0;
                    //double msmpkgsnftotal = 0;
                    //msmpkgsnftotal = smptotal + kgsnftotal;
                    //msnftotal = (msmpkgsnftotal / Ltrstotal) * 100;
                    //msnftotal = Math.Round(msnftotal, 2);
                    //newvartical12["SNF"] = msnftotal;
                    //newvartical12["LTR FAT"] = kgfattotal;
                    //newvartical12["LTR SNF"] = msmpkgsnftotal;
                    //newvartical12["SMP"] = smptotal;
                    //// newvartical1["CV"] = closingvalue;

                    //if (ddlBatchType.SelectedValue == "Batch Wise")
                    //{
                    //    //  newvartical1["CV"] = closingvalue;
                    //}
                    //else
                    //{
                    //    newvartical12["KGS"] = kgstotal;
                    //    newvartical12["LTRS"] = Ltrstotal;
                    //}
                    //newvartical12["Amount"] = totalamount;
                    //double standa = totalamount / Ltrstotal;
                    //newvartical12["stand"] = Math.Round(standa, 2);
                    //Report.Rows.Add(newvartical12);
                    //DataRow New1 = Report.NewRow();
                    //New1["To Silo"] = "SMP";
                    //New1["KGS"] = TOTsmp;
                    //New1["LTR SNF"] = smptotal;
                    //Report.Rows.Add(New1);

                    DataRow newvartical1 = Report.NewRow();
                    newvartical1["Source"] = "Total";
                    //newvartical1["KGS"] = kgstotal;
                    //double og = Convert.ToDouble(ob);
                    newvartical1["LTRS"] = Ltrstotal;
                    double mfattotal = 0;
                    mfattotal = (kgfattotal / kgstotal) * 100;
                    mfattotal = Math.Round(mfattotal, 2);
                    newvartical1["FAT"] = mfattotal;
                    double msnftotal = 0;
                    double msmpkgsnftotal = 0;
                    msmpkgsnftotal = smptotal + kgsnftotal;
                    msnftotal = (msmpkgsnftotal / kgstotal) * 100;
                    msnftotal = Math.Round(msnftotal, 2);
                    newvartical1["SNF"] = msnftotal;
                    newvartical1["KG FAT"] = kgfattotal;
                    newvartical1["KG SNF"] = msmpkgsnftotal;
                    newvartical1["SMP"] = smptotal;
                    // newvartical1["CV"] = closingvalue;

                    if (ddlBatchType.SelectedValue == "Batch Wise")
                    {
                        //  newvartical1["CV"] = closingvalue;
                    }
                    else
                    {
                        newvartical1["KGS"] = kgstotal;
                        newvartical1["LTRS"] = Ltrstotal;
                    }
                    newvartical1["Amount"] = totalamount;
                    double standa = totalamount / Ltrstotal;
                    newvartical1["stand"] = Math.Round(standa, 2);
                    Report.Rows.Add(newvartical1);

                    DataRow newvartical3 = Report.NewRow();
                    newvartical3["Source"] = "Grand Total";
                    newvartical3["KGS"] = gkgstotal;
                    newvartical3["LTRS"] = gLtrstotal;
                    double gfattotal = 0;
                    gfattotal = (gkgfattotal / gkgstotal) * 100;
                    gfattotal = Math.Round(gfattotal, 2);
                    newvartical3["FAT"] = gfattotal;
                    double gsnftotal = 0;
                    double gsmpkgsnftotal = 0;
                    gsmpkgsnftotal = gsmptotal + gkgsnftotal;
                    gsnftotal = (gsmpkgsnftotal / gkgstotal) * 100;
                    gsnftotal = Math.Round(gsnftotal, 2);
                    newvartical3["SNF"] = gsnftotal;
                    gkgfattotal = Math.Round(gkgfattotal, 2);
                    newvartical3["KG FAT"] = gkgfattotal;
                    newvartical3["KG SNF"] = gsmpkgsnftotal;
                    newvartical3["SMP"] = gsmptotal;
                    Report.Rows.Add(newvartical3);
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
            hidepanel.Visible = false;
        }
    }
    protected void gvMenu_DataBinding(object sender, EventArgs e)
    {
        string BranchID = Session["Branch_ID"].ToString();
        if (BranchID == "1")
        {
            //GridViewGroup First = new GridViewGroup(grdReports, null, "Batch Name");
        }
        else
        {
            // GridViewGroup First = new GridViewGroup(grdReports, null, "Batch Name");
        }
    }
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ddlBatchType.SelectedValue == "All")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[3].Text == "Total")
                {
                    e.Row.BackColor = System.Drawing.Color.Aquamarine;
                    e.Row.Font.Size = FontUnit.Medium;
                    e.Row.Font.Bold = true;
                }
                if (e.Row.Cells[3].Text == "Grand Total")
                {
                    e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                    e.Row.Font.Size = FontUnit.Large;
                    e.Row.Font.Bold = true;
                }
                e.Row.Cells[12].BackColor = System.Drawing.Color.Bisque;
            }
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text == "Total")
                {
                    e.Row.BackColor = System.Drawing.Color.Aquamarine;
                    e.Row.Font.Size = FontUnit.Medium;
                    e.Row.Font.Bold = true;
                }
                if (e.Row.Cells[4].Text == "Grand Total")
                {
                    e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                    e.Row.Font.Size = FontUnit.Large;
                    e.Row.Font.Bold = true;
                }
                e.Row.Cells[13].BackColor = System.Drawing.Color.Bisque;
            }
        }
    }
    protected void lnkView_Click(object sender, EventArgs e)
    {

    }

    protected void btn_mnthGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            string branchid = Session["Branch_ID"].ToString();
            SalesDBManager SalesDB = new SalesDBManager();
            Session["filename"] = "Batch Entry";
            Session["title"] = "Batch Entry Details";
            lblFromDate.Text = ddlmonth.SelectedItem.Text;
            Report.Columns.Add("Sno");
            Report.Columns.Add("Batch Name");
            Report.Columns.Add("KGS").DataType = typeof(Double);
            Report.Columns.Add("LTRS").DataType = typeof(Double);
            Report.Columns.Add("Per Ltr Rate");
            Report.Columns.Add("Amount");
            Report.Columns.Add("stand");
            cmd = new SqlCommand("SELECT MONTH(batchentrydetails.doe) AS MonthName, YEAR(batchentrydetails.doe) AS YEAR, SUM(batchentrydetails.qty_kgs) AS qty_kgs, SUM(batchentrydetails.qty_ltrs) AS qty_ltrs, batchmaster.batch, batchentrydetails.perltrrate, batchentrydetails.batchid, batchentrydetails.fromsiloid FROM  batchentrydetails INNER JOIN batchmaster ON batchentrydetails.batchid = batchmaster.batchid WHERE (MONTH(batchentrydetails.doe) = @d1) AND (YEAR(batchentrydetails.doe) = @d2) AND (batchmaster.batchid = @BatchID) AND (batchentrydetails.branchid = @branchid) GROUP BY MONTH(batchentrydetails.doe), YEAR(batchentrydetails.doe), batchmaster.batch, batchentrydetails.perltrrate, batchentrydetails.batchid,batchentrydetails.fromsiloid ");
            cmd.Parameters.Add("@BatchID", ddl_mnthbranchname.SelectedValue);
            cmd.Parameters.Add("@branchid", branchid);
            cmd.Parameters.Add("@d1", ddlmonth.SelectedValue);
            cmd.Parameters.Add("@d2", ddlyear.SelectedValue);
            DataTable dtbatch = SalesDB.SelectQuery(cmd).Tables[0];

            cmd = new SqlCommand("SELECT   dcno, siloid, qty_kgs, MONTH(date) AS MonthName, YEAR(date) AS YEAR, ccid FROM silo_inward_transaction WHERE (branchid = @branchid) AND (MONTH(date) = @d1) AND (YEAR(date) = @d2)");
            cmd.Parameters.Add("@branchid", branchid);
            cmd.Parameters.Add("@d1", ddlmonth.SelectedValue);
            cmd.Parameters.Add("@d2", ddlyear.SelectedValue);
            DataTable dtsiloid = SalesDB.SelectQuery(cmd).Tables[0];

            cmd = new SqlCommand("SELECT  milktransactions.dcno, milktransactions.sectionid, milktransactions.transportvalue, milktransactions.inwardno AS InwardNo, milktransactions.partydcno,  milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon,  milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost,  milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.p_fatpluscost,  milktransaction_logs.m_std_snf, milktransaction_logs.m_std_fat, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on,  milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.m_fatpluscost, milktransaction_logs.transport_on,  milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname, vendors.kms, milktransaction_logs.p_std_fat,  milktransaction_logs.fatplus_on, MONTH(milktransactions.doe) AS MonthName, YEAR(milktransactions.doe) AS YEAR FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE (milktransactions.branchid = @branchid) AND (MONTH(milktransactions.doe) = @d1) AND (YEAR(milktransactions.doe) = @d2) ORDER BY milktransactions.sno DESC");
            cmd.Parameters.Add("@branchid", branchid);
            cmd.Parameters.Add("@d1", ddlmonth.SelectedValue);
            cmd.Parameters.Add("@d2", ddlyear.SelectedValue);
            DataTable dtmilk = vdm.SelectQuery(cmd).Tables[0];

            cmd = new SqlCommand("SELECT   siloid, departmentid, qty_kgs, qty_ltrs, date FROM  silo_outward_transaction WHERE (branchid = @branchid) AND (MONTH(date) = @d1) AND (YEAR(date) = @d2) order by date");
            cmd.Parameters.Add("@d1", ddlmonth.SelectedValue);
            cmd.Parameters.Add("@d2", ddlyear.SelectedValue);
            cmd.Parameters.Add("branchid", branchid);
            DataTable dtreturnmilk = SalesDB.SelectQuery(cmd).Tables[0];

            if (dtbatch.Rows.Count > 0)
            {
                int i = 1;
                int sj = 1;
                double kgstotal = 0;
                double Ltrstotal = 0;
                double gkgstotal = 0;
                double gLtrstotal = 0;
                double totalamount = 0;
                DateTime dt = DateTime.Now;
                foreach (DataRow dr in dtbatch.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++;
                    double Kgs = 0;
                    double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                    newrow["KGS"] = Kgs;
                    kgstotal += Kgs;
                    gkgstotal += Kgs;
                    double ltrs = 0;
                    double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                    Ltrstotal += ltrs;
                    gLtrstotal += ltrs;
                    newrow["LTRS"] = ltrs;
                    double ltrrate = 0;
                    string perltr = dr["perltrrate"].ToString();
                    if (perltr != "")
                    {
                        double.TryParse(perltr, out ltrrate);
                    }
                    else
                    {

                    }
                    newrow["Batch Name"] = dr["batch"].ToString();
                    string source = dr["fromsiloid"].ToString();
                    if (source == "" || source == "0")
                    {
                        double amt = ltrrate * ltrs;
                        totalamount += amt;
                        newrow["Amount"] = ltrrate * ltrs;
                        newrow["Per Ltr Rate"] = perltr;
                    }
                    //if (source == "")
                    //{
                    //    //source = dr["vendorname"].ToString();
                    //    if (source == "")
                    //    {
                    //        string deptid = "";
                    //        deptid = dr["fromdeptid"].ToString();

                    //        // Return Milk
                    //        // End
                    //    }
                    //    else
                    //    {
                    //        double amt = ltrrate * ltrs;
                    //        totalamount += amt;
                    //        newrow["Amount"] = ltrrate * ltrs;
                    //        newrow["Per Ltr Rate"] = perltr;
                    //    }
                    //}
                    else if (source != "")
                    {
                        string fromsiloid = dr["FromSiloId"].ToString();
                        double ltrcostsp = 0;
                        double milkvaluetotalp = 0;
                        double kgstotalp = 0;
                        double perltrp = 0;
                        if (dtsiloid.Rows.Count > 0)
                        {
                            foreach (DataRow dra in dtsiloid.Select("siloid='" + fromsiloid + "'"))
                            {
                                string dcno = dra["dcno"].ToString();
                                string vendorid = dra["ccid"].ToString();
                                if (vendorid == "")
                                {
                                    vendorid = "0";
                                }
                                foreach (DataRow drr in dtmilk.Select("dcno='" + dcno + "' AND sectionid='" + vendorid + "'"))
                                {
                                    try
                                    {
                                        string milktype = drr["milktype"].ToString();
                                        double kgfattotalp = 0;
                                        double kgsnftotalp = 0;
                                        double Ltrstotalp = 0;
                                        double mvaluetotalp = 0;
                                        double ohtotalp = 0;
                                        double snf9totalp = 0;
                                        if (milktype == "Buffalo")
                                        {
                                            double qty_ltrp = 0;
                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                            double FAT = 0;
                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                            FAT = Math.Round(FAT, 2);
                                            double SNF = 0;
                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                            string Rateon = drr["rate_on"].ToString();
                                            double weight = 0;
                                            double KGFAT = 0;
                                            double KGSNF = 0;
                                            double ltrsp = 0;
                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                            Ltrstotalp += ltrsp;
                                            double Kgsp = 0;
                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                            kgstotalp += Kgsp;
                                            double tstotal = 0;
                                            tstotal = FAT + SNF;
                                            if (Rateon == "TS")
                                            {

                                                double TS = 0;
                                                TS = FAT + SNF;
                                                weight = (TS * Kgsp) / 100;
                                                KGFAT = (FAT * Kgsp) / 100;
                                                KGSNF = (SNF * Kgsp) / 100;
                                            }
                                            else if (Rateon == "KGFAT")
                                            {
                                                weight = (FAT * Kgsp) / 100;
                                                KGFAT = (FAT * Kgsp) / 100;
                                                KGSNF = (SNF * Kgsp) / 100;
                                            }
                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                            {
                                                string CalOn = drr["calc_on"].ToString();
                                                if (CalOn == "Ltrs")
                                                {
                                                    weight = ltrsp;
                                                    KGFAT = (FAT * ltrsp) / 100;
                                                    KGSNF = (SNF * ltrsp) / 100;
                                                }
                                                else
                                                {
                                                    weight = Kgsp;
                                                    KGFAT = (FAT * Kgsp) / 100;
                                                    KGSNF = (SNF * Kgsp) / 100;
                                                }
                                            }
                                            double cost = 0;
                                            double.TryParse(drr["cost"].ToString(), out cost);
                                            KGFAT = Math.Round(KGFAT, 2);
                                            kgfattotalp += KGFAT;
                                            KGSNF = Math.Round(KGSNF, 2);
                                            kgsnftotalp += KGSNF;
                                            double MValue = 0;
                                            MValue = KGFAT * cost;
                                            MValue = Math.Round(MValue, 2);
                                            mvaluetotalp += MValue;
                                            string OverheadOn = drr["overheadon"].ToString();
                                            double OHcost = 0;
                                            double overheadcost = 0;
                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                            if (OverheadOn == "Ltrs")
                                            {
                                                OHcost = overheadcost * ltrsp;
                                            }
                                            else
                                            {
                                                OHcost = overheadcost * Kgsp;
                                            }
                                            double MSnf = 0;
                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                            double m_snfpluscost = 0;
                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                            double DiffSNFCost = 0;
                                            if (SNF < MSnf)
                                            {
                                                string SNFOn = drr["snfplus_on"].ToString();
                                                double diffSNF = 0;
                                                diffSNF = SNF - MSnf;
                                                diffSNF = Math.Round(diffSNF, 2);
                                                if (SNFOn == "Ltrs")
                                                {
                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                }
                                                else
                                                {
                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                }
                                            }
                                            double p_snfpluscost = 0;
                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                            double PSnf = 0;
                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                            if (SNF > PSnf)
                                            {
                                                string SNFOn = drr["snfplus_on"].ToString();
                                                double diffSNF = 0;
                                                diffSNF = SNF - MSnf;
                                                if (SNFOn == "Ltrs")
                                                {
                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                }
                                                else
                                                {
                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                }
                                            }

                                            double OHandMvalue = 0;
                                            OHandMvalue = MValue + OHcost + DiffSNFCost;
                                            ohtotalp += OHcost;
                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                            snf9totalp += DiffSNFCost;
                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                            milkvaluetotalp += OHandMvalue;
                                            double ltrcostp = OHandMvalue / ltrsp;
                                            ltrcostp = Math.Round(ltrcostp, 2);
                                            ltrcostsp += ltrcostp;
                                        }
                                        else if (milktype == "Cow" || milktype == "Condensed")
                                        {
                                            double qty_kgsp = 0;
                                            double.TryParse(drr["qty_kgs"].ToString(), out qty_kgsp);
                                            double qty_ltrp = 0;
                                            double.TryParse(drr["qty_ltr"].ToString(), out qty_ltrp);
                                            double FAT = 0;
                                            double.TryParse(drr["fat"].ToString(), out FAT);
                                            FAT = Math.Round(FAT, 2);
                                            double SNF = 0;
                                            double.TryParse(drr["snf"].ToString(), out SNF);
                                            string Rateon = drr["rate_on"].ToString();
                                            double weight = 0;
                                            double KGFAT = 0;
                                            double KGSNF = 0;
                                            double ltrsp = 0;
                                            double.TryParse(drr["qty_ltr"].ToString(), out ltrsp);
                                            Ltrstotalp += ltrsp;
                                            double Kgsp = 0;
                                            double.TryParse(drr["qty_kgs"].ToString(), out Kgsp);
                                            kgstotalp += Kgsp;
                                            double tstotal = 0;
                                            tstotal = FAT + SNF;
                                            if (Rateon == "TS")
                                            {
                                                double TS = 0;
                                                TS = FAT + SNF;
                                                weight = (TS * Kgsp) / 100;
                                                KGFAT = (FAT * Kgsp) / 100;
                                                KGSNF = (SNF * Kgsp) / 100;
                                            }
                                            else if (Rateon == "KGFAT")
                                            {
                                                weight = (FAT * Kgsp) / 100;
                                                KGFAT = (FAT * Kgsp) / 100;
                                                KGSNF = (SNF * Kgsp) / 100;
                                            }
                                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                                            {
                                                string CalOn = drr["calc_on"].ToString();
                                                if (CalOn == "Ltrs")
                                                {
                                                    weight = ltrsp;
                                                    KGFAT = (FAT * ltrsp) / 100;
                                                    KGSNF = (SNF * ltrsp) / 100;
                                                }
                                                else
                                                {
                                                    weight = Kgsp;
                                                    KGFAT = (FAT * Kgsp) / 100;
                                                    KGSNF = (SNF * Kgsp) / 100;
                                                }
                                            }
                                            double cost = 0;
                                            double.TryParse(drr["cost"].ToString(), out cost);
                                            KGFAT = Math.Round(KGFAT, 2);
                                            kgfattotalp += KGFAT;
                                            KGSNF = Math.Round(KGSNF, 2);
                                            kgsnftotalp += KGSNF;
                                            double MValue = 0;
                                            if (Rateon == "PerLtr" || Rateon == "PerKg")
                                            {
                                                MValue = cost * qty_kgsp;
                                            }
                                            else
                                            {
                                                MValue = tstotal * cost * qty_ltrp;
                                                MValue = MValue / 100;
                                            }
                                            MValue = Math.Round(MValue, 2);
                                            mvaluetotalp += MValue;
                                            string OverheadOn = drr["overheadon"].ToString();
                                            double OHcost = 0;
                                            double overheadcost = 0;
                                            double.TryParse(drr["overheadcost"].ToString(), out overheadcost);
                                            if (OverheadOn == "Ltrs")
                                            {
                                                OHcost = overheadcost * ltrsp;
                                            }
                                            else
                                            {
                                                OHcost = overheadcost * Kgsp;
                                            }
                                            double MSnf = 0;
                                            double.TryParse(drr["m_std_snf"].ToString(), out MSnf);
                                            double m_snfpluscost = 0;
                                            double.TryParse(drr["m_snfpluscost"].ToString(), out m_snfpluscost);
                                            double DiffSNFCost = 0;
                                            if (SNF < MSnf)
                                            {
                                                string SNFOn = drr["snfplus_on"].ToString();
                                                double diffSNF = 0;
                                                diffSNF = SNF - MSnf;
                                                diffSNF = Math.Round(diffSNF, 2);
                                                if (SNFOn == "Ltrs")
                                                {
                                                    DiffSNFCost = diffSNF * ltrsp * m_snfpluscost * 10;
                                                }
                                                else
                                                {
                                                    DiffSNFCost = diffSNF * Kgsp * m_snfpluscost * 10;
                                                }
                                            }
                                            double p_snfpluscost = 0;
                                            double.TryParse(drr["p_snfpluscost"].ToString(), out p_snfpluscost);
                                            double PSnf = 0;
                                            double.TryParse(drr["p_std_snf"].ToString(), out PSnf);
                                            if (SNF > PSnf)
                                            {
                                                string SNFOn = drr["snfplus_on"].ToString();
                                                double diffSNF = 0;
                                                diffSNF = SNF - MSnf;
                                                if (SNFOn == "Ltrs")
                                                {
                                                    DiffSNFCost = diffSNF * ltrsp * p_snfpluscost * 10;
                                                }
                                                else
                                                {
                                                    DiffSNFCost = diffSNF * Kgsp * p_snfpluscost * 10;
                                                }
                                            }
                                            double MFat = 0;
                                            double.TryParse(drr["m_std_fat"].ToString(), out MFat);
                                            double m_fatpluscost = 0;
                                            double.TryParse(drr["m_fatpluscost"].ToString(), out m_fatpluscost);
                                            double DiffFATCost = 0;
                                            if (FAT < MFat)
                                            {
                                                string FATOn = drr["fatplus_on"].ToString();
                                                double diffFAT = 0;
                                                diffFAT = FAT - MFat;
                                                diffFAT = Math.Round(diffFAT, 2);
                                                if (FATOn == "Ltrs")
                                                {
                                                    DiffFATCost = diffFAT * ltrsp * m_fatpluscost * 10;
                                                }
                                                else
                                                {
                                                    DiffFATCost = diffFAT * Kgsp * m_fatpluscost * 10;
                                                }
                                            }
                                            double p_fatpluscost = 0;
                                            double.TryParse(drr["p_fatpluscost"].ToString(), out p_fatpluscost);
                                            double PFat = 0;
                                            double.TryParse(drr["p_std_fat"].ToString(), out PFat);
                                            if (FAT > PFat)
                                            {
                                                string FATOn = drr["fatplus_on"].ToString();
                                                double diffFAT = 0;
                                                diffFAT = FAT - PFat;
                                                if (FATOn == "Ltrs")
                                                {
                                                    DiffFATCost = diffFAT * ltrsp * p_fatpluscost * 10;
                                                }
                                                else
                                                {
                                                    DiffFATCost = diffFAT * Kgsp * p_fatpluscost * 10;
                                                }
                                            }
                                            DiffFATCost = Math.Round(DiffFATCost, 2);
                                            double OHandMvalue = 0;
                                            OHandMvalue = MValue + OHcost + DiffSNFCost + DiffFATCost;
                                            ohtotalp += OHcost;
                                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                                            snf9totalp += DiffSNFCost;
                                            OHandMvalue = Math.Round(OHandMvalue, 2);
                                            milkvaluetotalp += OHandMvalue;
                                            double ltrcostp = OHandMvalue / ltrsp;
                                            ltrcostp = Math.Round(ltrcostp, 2);
                                            ltrcostsp += ltrcostp;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            double tkgs = kgstotalp;
                            double ttotalmilkp = milkvaluetotalp;
                            if (milkvaluetotalp == 0)
                            {
                                perltrp = 0;
                            }
                            else
                            {
                                perltrp = ttotalmilkp / tkgs;
                            }
                            perltrp = Math.Round(perltrp, 2);
                        }
                        double amt = perltrp * ltrs;
                        totalamount += amt;
                        newrow["Per Ltr Rate"] = perltrp;
                        newrow["Amount"] = perltrp * ltrs;
                    }
                    Report.Rows.Add(newrow);
                }
                DataRow newvartical1 = Report.NewRow();
                newvartical1["Batch Name"] = "Total";
                newvartical1["LTRS"] = Ltrstotal;
                newvartical1["Amount"] = totalamount;
                double standa = totalamount / Ltrstotal;
                newvartical1["stand"] = Math.Round(standa, 2);
                Report.Rows.Add(newvartical1);
                grdmnthrpt.DataSource = Report;
                grdmnthrpt.DataBind();
                Session["xportdata"] = Report;
                Session["finalize"] = Report;
                panalmnth.Visible = true;
                btnfinalize.Visible = true;
            }
            else
            {
                lblmmsg.Text = "No data were found";
                hidepanel.Visible = false;
                btnfinalize.Visible = false;
                grdmnthrpt.DataSource = null;
                grdmnthrpt.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }

    protected void grdmnthReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            e.Row.Cells[4].BackColor = System.Drawing.Color.Bisque;
        }
    }

    protected void btn_finalizeclick(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["finalize"];
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string stdrate = dr["stand"].ToString();
                if (stdrate != "")
                {
                    cmd = new SqlCommand("Update batchstdrates set stdrate=@rate where batchid=@bid and month=@mnth and year=@yr");
                    cmd.Parameters.Add("@mnth", ddlmonth.SelectedValue);
                    cmd.Parameters.Add("@yr", ddlyear.SelectedItem.Value);
                    cmd.Parameters.Add("@rate", stdrate);
                    cmd.Parameters.Add("@bid", ddl_mnthbranchname.SelectedValue);
                    if (vdm.Update(cmd) == 0)
                    {
                        cmd = new SqlCommand("Insert Into batchstdrates(batchid, stdrate, month, year) values (@batchid, @stdrate, @month, @year)");
                        cmd.Parameters.Add("@batchid", ddl_mnthbranchname.SelectedValue);
                        cmd.Parameters.Add("@stdrate", stdrate);
                        cmd.Parameters.Add("@month", ddlmonth.SelectedValue);
                        cmd.Parameters.Add("@year", ddlyear.SelectedItem.Value);
                        vdm.insert(cmd);
                    }
                }
            }
        }
    }
}