using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

public partial class gainorlosereport : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
    MySqlCommand mycmd;
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
                    DateTime dt = DateTime.Now.AddDays(-1);
                    dtp_FromDate.Text = dt.ToString("dd-MM-yyyy HH:mm");
                    dtp_Todate.Text = dt.ToString("dd-MM-yyyy HH:mm");
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
    DataTable siloopenReport = new DataTable();
    DataTable despatch = new DataTable();
    DataTable curd = new DataTable();
    DataTable butter = new DataTable();
    DataTable condencer = new DataTable();
    DataTable tanker = new DataTable();
    DataTable receipts = new DataTable();
    DataTable buff = new DataTable();
    DataTable smp = new DataTable();
    DataTable returnmilk = new DataTable();
    DataTable creamproduction = new DataTable();
    DataTable stockreport = new DataTable();
    DataTable gainorloss = new DataTable();
    DataTable siloclosingReport = new DataTable();

    private void getdata()
    {
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
        lbltoDate.Text = todate.ToString("dd/MMM/yyyy");
        Session["filename"] = "Daily Report";
        Session["title"] = "Daily Report Details";
        lblmsg.Text = "";

        double siloopeningltrstotal = 0;
        double siloopeningkgstotal = 0;
        double siloopeningfattotal = 0;
        double siloopeningsnftotal = 0;
        double siloopeningclrtotal = 0;
        double siloopeningkgfattotal = 0;
        double siloopeningkgsnftotal = 0;

        double reciptltrstotal = 0;
        double reciptkgstotal = 0;
        double reciptfattotal = 0;
        double reciptsnftotal = 0;
        double reciptkgfattotal = 0;
        double reciptkgsnftotal = 0;

        double smpkgstotal = 0;
        double smpfattotal = 0;
        double smpsnftotal = 0;
        double smpkgfattotal = 0;
        double smpkgsnftotal = 0;

        double returnmilkltrstotal = 0;
        double returnmilkkgstotal = 0;
        double returnmilkfattotal = 0;
        double returnmilksnftotal = 0;
        double returnmilkkgfattotal = 0;
        double returnmilkkgsnftotal = 0;

        double totaldesp = 0;
        double despatchkgfattotal = 0;
        double despatchkgsnftotal = 0;
        double despatchfattotal = 0;
        double despatchsnftotal = 0;
        double despatchkgstotal = 0;
        double despatchclrtotal = 0;
        double despatchtotalltrs = 0;
        double despatchtotalkgs = 0;
        double despatchkgtotalfat = 0;
        double despatchkgtotalsnf = 0;

        double curdltrstotal = 0;
        double curdkgstotal = 0;
        double curdfattotal = 0;
        double curdsnftotal = 0;
        double curdkgfattotal = 0;
        double curdkgsnftotal = 0;

        double butterltrstotal = 0;
        double butterkgstotal = 0;
        double butterfattotal = 0;
        double buttersnftotal = 0;
        double butterkgfattotal = 0;
        double butterkgsnftotal = 0;
        double buttertotalkgsnf = 0;
        double buttertotalkgfat = 0;


        double condencerltrstotal = 0;
        double condencerkgstotal = 0;
        double condencerfattotal = 0;
        double condencersnftotal = 0;
        double condencerkgfattotal = 0;
        double condencerkgsnftotal = 0;


        double tankerltrstotal = 0;
        double tankerkgstotal = 0;
        double tankerfattotal = 0;
        double tankersnftotal = 0;
        double tankerkgfattotal = 0;
        double tankerkgsnftotal = 0;

        double siloclosingltrstotal = 0;
        double siloclosingkgstotal = 0;
        double siloclosingfattotal = 0;
        double siloclosingsnftotal = 0;
        double siloclosingclrtotal = 0;
        double siloclosingkgfattotal = 0;
        double siloclosingkgsnftotal = 0;

        double creamqtykgstotal = 0;
        double creamkgstotal = 0;
        double creamfattotal = 0;
        double creamsnftotal = 0;
        double creamclrtotal = 0;
        double creamkgfattotal = 0;
        double creamkgsnftotal = 0;
        double creamltrstotal = 0;

        double reciptcowltrstotal = 0;
        double reciptcowkgstotal = 0;
        double reciptcowfattotal = 0;
        double reciptcowsnftotal = 0;
        double reciptcowkgfattotal = 0;
        double reciptcowkgsnftotal = 0;
        double reciptbuffltrstotal = 0;
        string venderid = "";
        double reciptbuffkgstotal = 0;
        double reciptbufffattotal = 0;
        double reciptbuffsnftotal = 0;
        double reciptbuffkgfattotal = 0;
        double reciptbuffkgsnftotal = 0;
        double creamqtytotalkgs = 0;
        double creamtotalltrs = 0;
        double creamkgtotalfat = 0;
        double creamkgtotalsnf = 0;
        double tankertotalkgs = 0;
        double tankertotalltrs = 0;
        double tankerkgtotalfat = 0;
        double tankerkgtotalsnf = 0;
        double condencertotalltrs = 0;
        double condencertotalkgs = 0;
        double condencertotalkgfat = 0;
        double condencertotalkgsnf = 0;

        double curdtotalltrs = 0;
        double curdtotalkgs = 0;
        double curdtotalkgfat = 0;
        double curdtotalkgsnf = 0;

        double buttertotalltrs = 0;
        double buttertotalkgs = 0;

        double smpkgtotalfat = 0;
        double smpkgtotalsnf = 0;

        siloopenReport.Columns.Add("siloname");
        siloopenReport.Columns.Add("qty(ltr)");
        siloopenReport.Columns.Add("qty(kgs)");
        siloopenReport.Columns.Add("FAT");
        siloopenReport.Columns.Add("SNF");
        siloopenReport.Columns.Add("CLR");
        siloopenReport.Columns.Add("KG FAT");
        siloopenReport.Columns.Add("KG SNF");
        cmd = new SqlCommand("Select sm.SiloName, scd.qty_kgs, scd.fat, scd.snf, scd.clr, scd.closingdate from silowiseclosingdetails scd INNER JOIN silomaster sm on sm.SiloId=scd.siloid where (scd.closingdate BETWEEN @d1 AND @d2) AND (scd.branchid=@branchid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(fromdate));
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dtsiloopening = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtsiloopening.Rows.Count > 0)
        {
            siloopeningltrstotal = 0;
            siloopeningkgstotal = 0;
            siloopeningfattotal = 0;
            siloopeningsnftotal = 0;
            siloopeningclrtotal = 0;
            siloopeningkgfattotal = 0;
            siloopeningkgsnftotal = 0;
            foreach (DataRow dr in dtsiloopening.Rows)
            {
                DataRow newrow = siloopenReport.NewRow();
                newrow["siloname"] = dr["SiloName"].ToString();
                newrow["qty(ltr)"] = dr["qty_kgs"].ToString();
                newrow["FAT"] = dr["fat"].ToString();
                newrow["SNF"] = dr["snf"].ToString();
                newrow["CLR"] = dr["clr"].ToString();
                double ltrs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out ltrs);
                siloopeningltrstotal += ltrs;
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                siloopeningfattotal += FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                siloopeningsnftotal += SNF;
                double KGFAT = 0;
                double KGSNF = 0;
                double clr = 0;
                double.TryParse(dr["clr"].ToString(), out clr);
                siloopeningclrtotal += clr;
                double modclr = (clr / 1000) + 1;
                double qtyltrkgs = ltrs * modclr;
                newrow["qty(kgs)"] = qtyltrkgs;
                siloopeningkgstotal += qtyltrkgs;
                KGFAT = (FAT * qtyltrkgs) / 100;
                KGSNF = (SNF * qtyltrkgs) / 100;
                siloopeningkgfattotal += KGFAT;
                siloopeningkgsnftotal += KGSNF;
                newrow["KG FAT"] = Math.Round(KGFAT, 2);
                newrow["KG SNF"] = Math.Round(KGSNF, 2);
                siloopenReport.Rows.Add(newrow);
            }
            DataRow newvartical2 = siloopenReport.NewRow();
            newvartical2["siloname"] = "Total";
            newvartical2["qty(ltr)"] = siloopeningltrstotal;
            newvartical2["qty(kgs)"] = siloopeningkgstotal;
            newvartical2["KG FAT"] = Math.Round(siloopeningkgfattotal, 2);
            newvartical2["KG SNF"] = Math.Round(siloopeningkgsnftotal, 2);
            double kgfatt = siloopeningkgfattotal / siloopeningkgstotal;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            double kgsnft = siloopeningkgsnftotal / siloopeningkgstotal;
            newvartical2["SNF"] = Math.Round(kgsnft * 100, 2);
            siloopenReport.Rows.Add(newvartical2);
            DataRow newvartical3 = siloopenReport.NewRow();
            newvartical3["siloname"] = "Receipts";
            siloopenReport.Rows.Add(newvartical3);


            grdsiloopening.DataSource = siloopenReport;
            grdsiloopening.DataBind();
            Session["xportdata"] = siloopenReport;
            hidepanel.Visible = true;
        }
        else
        {
            DataRow newrow = siloopenReport.NewRow();
            newrow["siloname"] = "total";
            DataRow newvartical3 = siloopenReport.NewRow();
            newvartical3["siloname"] = "Receipts";
            siloopenReport.Rows.Add(newvartical3);

            DataRow newvartical4 = siloopenReport.NewRow();
            newvartical4["siloname"] = "Buffalo";
            siloopenReport.Rows.Add(newvartical4);

            grdsiloopening.DataSource = siloopenReport;
            grdsiloopening.DataBind();
            Session["xportdata"] = siloopenReport;
            hidepanel.Visible = true;
        }
        receipts.Columns.Add("Name");
        receipts.Columns.Add("qty(ltr)");
        receipts.Columns.Add("qty(kgs)");
        receipts.Columns.Add("FAT");
        receipts.Columns.Add("SNF");
        receipts.Columns.Add("KG FAT");
        receipts.Columns.Add("KG SNF");
        receipts.Columns.Add("ccid");
        buff.Columns.Add("Name");
        buff.Columns.Add("qty(ltr)");
        buff.Columns.Add("qty(kgs)");
        buff.Columns.Add("FAT");
        buff.Columns.Add("SNF");
        buff.Columns.Add("KG FAT");
        buff.Columns.Add("KG SNF");
        buff.Columns.Add("ccid");
        cmd = new SqlCommand("SELECT silo_inward_transaction.sno, silo_inward_transaction.ccid, v.vendorname, silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs, silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName FROM  silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId LEFT JOIN vendors v on v.sno = silo_inward_transaction.ccid LEFT JOIN processingdepartments ON processingdepartments.departmentid = silo_inward_transaction.deptid WHERE (silo_inward_transaction.date BETWEEN @date1 AND @date2) AND (silo_inward_transaction.branchid=@branchid)");
        cmd.Parameters.Add("@date1", GetLowDate(fromdate));
        cmd.Parameters.Add("@date2", GetHighDate(todate));
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dtrecipts = SalesDB.SelectQuery(cmd).Tables[0];
        //
        cmd = new SqlCommand("SELECT silo_inward_transaction.sno, silo_inward_transaction.ccid, v.vendorname, silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs, silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName, silo_inward_transaction.deptid FROM  silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId LEFT JOIN vendors v on v.sno = silo_inward_transaction.ccid LEFT JOIN processingdepartments ON processingdepartments.departmentid = silo_inward_transaction.deptid WHERE (silo_inward_transaction.date BETWEEN @date1 AND @date2) AND (silo_inward_transaction.branchid=@branchid) AND (silo_inward_transaction.ccid IS NULL)");
        cmd.Parameters.Add("@date1", GetLowDate(fromdate));
        cmd.Parameters.Add("@date2", GetHighDate(todate));
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dtccnullrecipts = SalesDB.SelectQuery(cmd).Tables[0];

        cmd = new SqlCommand("SELECT sno, vendorcode, vendorname, vendortype FROM vendors");
        DataTable dtvendors = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtvendors.Rows.Count > 0)
        {
            reciptltrstotal = 0;
            reciptkgstotal = 0;
            reciptfattotal = 0;
            reciptsnftotal = 0;
            reciptkgfattotal = 0;
            reciptkgsnftotal = 0;
            foreach (DataRow drr in dtvendors.Rows)
            {
                string VNAME = "";
                string milktype = "";
                double FAT = 0;
                double SNF = 0;
                double kgfat = 0;
                double kgsnf = 0;
                string ccid = drr["sno"].ToString();
                DataTable dtin = new DataTable();
                DataRow[] drv = dtrecipts.Select("ccid='" + ccid + "'");
                if (drv.Length > 0)
                {
                    dtin = drv.CopyToDataTable();
                }
                if (dtin.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtin.Rows)
                    {
                        VNAME = dr["vendorname"].ToString();
                        double ltrs = 0;
                        double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                        reciptltrstotal += ltrs;

                        double Kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                        reciptkgstotal += Kgs;

                        double.TryParse(dr["fat"].ToString(), out FAT);
                        reciptfattotal += FAT;
                        reciptbufffattotal += reciptfattotal;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        reciptsnftotal += SNF;
                        reciptbuffsnftotal += reciptsnftotal;
                        kgfat = Kgs * FAT;
                        kgfat = Math.Round(kgfat / 100, 2);
                        reciptkgfattotal += kgfat;
                        reciptbuffkgfattotal += kgfat;
                        kgsnf = Kgs * SNF;
                        kgsnf = Math.Round(kgsnf / 100, 2);
                        reciptkgsnftotal += kgsnf;
                        reciptbuffkgsnftotal += kgsnf;
                    }
                    DataRow newrow = receipts.NewRow();
                    newrow["Name"] = VNAME;
                    newrow["qty(ltr)"] = reciptltrstotal;
                    reciptbuffltrstotal += reciptltrstotal;
                    newrow["qty(kgs)"] = reciptkgstotal;
                    reciptbuffkgstotal += reciptkgstotal;
                    newrow["KG FAT"] = reciptkgfattotal;
                    newrow["KG SNF"] = reciptkgsnftotal;
                    double fatt = reciptkgfattotal / reciptkgstotal;
                    newrow["FAT"] = Math.Round(fatt * 100, 2);
                    double snft = reciptkgsnftotal / reciptkgstotal;
                    newrow["SNF"] = Math.Round(snft * 100, 2);
                    newrow["ccid"] = ccid;
                    venderid = ccid;
                    receipts.Rows.Add(newrow);
                    reciptkgsnftotal = 0;
                    reciptkgfattotal = 0;
                    reciptkgstotal = 0;
                    reciptltrstotal = 0;
                    venderid = "";
                }
            }
            if (dtccnullrecipts.Rows.Count > 0)
            {
                string VNAME = "";
                string milktype = "";
                double FAT = 0;
                double SNF = 0;
                double kgfat = 0;
                double kgsnf = 0;

                double oreciptltrstotal = 0;
                double oreciptkgstotal = 0;
                double oreciptfattotal = 0;
                double oreciptsnftotal = 0;
                double oreciptbuffkgsnftotal = 0;
                double oreciptbuffltrstotal = 0;
                double oreciptbuffkgstotal = 0;
                double oreciptbuffkgfattotal = 0;

                foreach (DataRow dr in dtccnullrecipts.Rows)
                {
                    string departmentid = dr["deptid"].ToString();
                    if (departmentid == "10")   // this place return milk only taken butter section only changes as per anand
                    {
                        VNAME = "Return Milk - Butter Section";
                        double ltrs = 0;
                        double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                        reciptltrstotal += ltrs;
                        oreciptltrstotal += ltrs;

                        double Kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                        reciptkgstotal += Kgs;
                        oreciptkgstotal += Kgs;

                        double.TryParse(dr["fat"].ToString(), out FAT);
                        reciptfattotal += FAT;
                        oreciptfattotal += FAT;

                        double.TryParse(dr["snf"].ToString(), out SNF);
                        reciptsnftotal += SNF;
                        oreciptsnftotal += SNF;

                        kgfat = Kgs * FAT;
                        kgfat = Math.Round(kgfat / 100, 2);
                        reciptbuffkgfattotal += kgfat;
                        oreciptbuffkgfattotal += kgfat;

                        kgsnf = Kgs * SNF;
                        kgsnf = Math.Round(kgsnf / 100, 2);
                        reciptbuffkgsnftotal += kgsnf;
                        oreciptbuffkgsnftotal += kgsnf;

                        reciptbuffltrstotal += ltrs;
                        oreciptbuffltrstotal += ltrs;

                        reciptbuffkgstotal += Kgs;
                        oreciptbuffkgstotal += Kgs;
                    }
                    else
                    {
                    }
                }
                DataRow newrow21 = receipts.NewRow();
                newrow21["Name"] = "Return Milk - Butter Section";
                newrow21["qty(ltr)"] = oreciptbuffltrstotal;
                newrow21["qty(kgs)"] = oreciptbuffkgstotal;
                newrow21["KG FAT"] = Math.Round(oreciptbuffkgfattotal, 2);
                newrow21["KG SNF"] = Math.Round(oreciptbuffkgsnftotal, 2);
                double okgfatt = oreciptbuffkgfattotal / oreciptbuffkgstotal;
                newrow21["FAT"] = Math.Round(okgfatt * 100, 2);
                double okgsnft = oreciptbuffkgsnftotal / oreciptbuffkgstotal;
                newrow21["SNF"] = Math.Round(okgsnft * 100, 2);
                newrow21["ccid"] = venderid;
                receipts.Rows.Add(newrow21);
            }
            DataRow newrow2 = receipts.NewRow();
            newrow2["Name"] = "Total";
            newrow2["qty(ltr)"] = reciptbuffltrstotal;
            newrow2["qty(kgs)"] = reciptbuffkgstotal;
            newrow2["KG FAT"] = Math.Round(reciptbuffkgfattotal, 2);
            newrow2["KG SNF"] = Math.Round(reciptbuffkgsnftotal, 2);
            double kgfatt = reciptbuffkgfattotal / reciptbuffkgstotal;
            newrow2["FAT"] = Math.Round(kgfatt * 100, 2);
            double kgsnft = reciptbuffkgsnftotal / reciptbuffkgstotal;
            newrow2["SNF"] = Math.Round(kgsnft * 100, 2);
            newrow2["ccid"] = venderid;
            receipts.Rows.Add(newrow2);
            DataRow newrow3 = receipts.NewRow();
            newrow3["Name"] = "SMP Details";
            receipts.Rows.Add(newrow3);
            grdrecipts.DataSource = receipts;
            grdrecipts.DataBind();
            Session["xportdata"] = receipts;
            pnlrecipts.Visible = true;
        }
        smp.Columns.Add("Name");
        smp.Columns.Add("qty(kgs)");
        smp.Columns.Add("FAT");
        smp.Columns.Add("SNF");
        smp.Columns.Add("KG FAT");
        smp.Columns.Add("KG SNF");
        // cmd = new SqlCommand("Select V.vendorname, ML.rate_on, ML.calc_on, mt.dcno, mt.qty_ltr, mt.qty_kgs, mt.snf, mt.fat, mt.clr, mt.percentageon, mt.vehicleno, mt.partydcno from milktransactions MT INNER JOIN vendors V on V.sno=mt.sectionid INNER JOIN milktransaction_logs ML ON mt.sno = ML.milktransaction_sno WHERE (mt.transtype='in') AND (mt.entrydate BETWEEN @date1 AND @date2)");
        cmd = new SqlCommand("SELECT qty_kgs, fat, snf, date, branchid, enterby FROM  smp_details WHERE  (date BETWEEN @smd1 AND @smd2) AND (branchid=@sbranchid)");
        cmd.Parameters.Add("@smd1", GetLowDate(fromdate));
        cmd.Parameters.Add("@smd2", GetHighDate(todate));
        cmd.Parameters.Add("@sbranchid", BranchID);
        DataTable dtsmp = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtsmp.Rows.Count > 0)
        {
            smpkgstotal = 0;
            smpfattotal = 0;
            smpsnftotal = 0;
            smpkgfattotal = 0;
            smpkgsnftotal = 0;

            double FAT = 0;
            double SNF = 0;
            double kgfat = 0;
            double kgsnf = 0;
            foreach (DataRow dr in dtsmp.Rows)
            {
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                smpkgstotal += Kgs;
                double.TryParse(dr["fat"].ToString(), out FAT);
                smpfattotal += FAT;
                double.TryParse(dr["snf"].ToString(), out SNF);
                smpsnftotal += SNF;
                kgfat = Kgs * FAT;
                kgfat = Math.Round(kgfat / 100, 2);
                smpkgfattotal += kgfat;
                smpkgtotalfat += kgfat;
                kgsnf = Kgs * SNF;
                kgsnf = Math.Round(kgsnf / 100, 2);
                smpkgsnftotal += kgsnf;
                smpkgtotalsnf += kgsnf;
            }
            DataRow newrow = smp.NewRow();
            newrow["Name"] = "SMP";
            newrow["qty(kgs)"] = smpkgstotal;
            newrow["KG FAT"] = smpkgfattotal;
            newrow["KG SNF"] = smpkgsnftotal;
            double fatt = smpkgfattotal / smpkgstotal;
            newrow["FAT"] = Math.Round(fatt * 100, 2);
            double snft = smpkgsnftotal / smpkgstotal;
            newrow["SNF"] = Math.Round(snft * 100, 2);
            smp.Rows.Add(newrow);
            smpkgfattotal = 0;
            smpkgsnftotal = 0;

            DataRow newvartical2 = smp.NewRow();
            newvartical2["Name"] = "Total";
            newvartical2["qty(kgs)"] = smpkgstotal;
            newvartical2["KG FAT"] = smpkgtotalfat;
            double kgfatt = smpkgtotalfat / smpkgstotal;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            newvartical2["KG SNF"] = smpkgtotalsnf;
            double kgSNFt = smpkgtotalsnf / smpkgstotal;
            newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);
            smp.Rows.Add(newvartical2);
            DataRow newvartical3 = smp.NewRow();
            newvartical3["Name"] = "Return Milk Details";
            smp.Rows.Add(newvartical3);
            grdsmp.DataSource = smp;
            grdsmp.DataBind();
            Session["xportdata"] = smp;
            pnlsmp.Visible = true;
        }

        returnmilk.Columns.Add("Department Name");
        returnmilk.Columns.Add("Silo Name");
        returnmilk.Columns.Add("qty(ltr)");
        returnmilk.Columns.Add("qty(kgs)");
        returnmilk.Columns.Add("FAT");
        returnmilk.Columns.Add("SNF");
        returnmilk.Columns.Add("KG FAT");
        returnmilk.Columns.Add("KG SNF");
        // cmd = new SqlCommand("Select V.vendorname, ML.rate_on, ML.calc_on, mt.dcno, mt.qty_ltr, mt.qty_kgs, mt.snf, mt.fat, mt.clr, mt.percentageon, mt.vehicleno, mt.partydcno from milktransactions MT INNER JOIN vendors V on V.sno=mt.sectionid INNER JOIN milktransaction_logs ML ON mt.sno = ML.milktransaction_sno WHERE (mt.transtype='in') AND (mt.entrydate BETWEEN @date1 AND @date2)");
        cmd = new SqlCommand("SELECT returnmilk_details.doe, processingdepartments.departmentname, silomaster.SiloName, returnmilk_details.quantity,returnmilk_details.fat,returnmilk_details.snf,returnmilk_details.clr,returnmilk_details.qty_ltr FROM returnmilk_details INNER JOIN processingdepartments ON returnmilk_details.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON returnmilk_details.siloid = silomaster.SiloId WHERE (returnmilk_details.doe BETWEEN @d1 AND @d2) AND (returnmilk_details.departmentid=4) AND (returnmilk_details.branchid=@rbranchid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@rbranchid", BranchID);
        DataTable dtreturnmilk = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtreturnmilk.Rows.Count > 0)
        {
            returnmilkltrstotal = 0;
            returnmilkkgstotal = 0;
            returnmilkfattotal = 0;
            returnmilksnftotal = 0;
            returnmilkkgfattotal = 0;
            returnmilkkgsnftotal = 0;
            foreach (DataRow dr in dtreturnmilk.Rows)
            {
                DataRow newrow = returnmilk.NewRow();
                newrow["Department Name"] = dr["departmentname"].ToString();
                newrow["Silo Name"] = dr["SiloName"].ToString();
                newrow["qty(ltr)"] = dr["qty_ltr"].ToString();
                newrow["qty(kgs)"] = dr["quantity"].ToString();
                double ltrs = 0;
                double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                returnmilkltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["quantity"].ToString(), out Kgs);
                returnmilkkgstotal += Kgs;
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                returnmilkfattotal += FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                returnmilksnftotal += SNF;
                double ltrfat = 0;
                ltrfat = Kgs * FAT;
                ltrfat = Math.Round(ltrfat / 100, 2);
                returnmilkkgfattotal += ltrfat;
                double ltrsnf = 0;
                ltrsnf = Kgs * SNF;
                ltrsnf = Math.Round(ltrsnf / 100, 2);
                returnmilkkgsnftotal += ltrsnf;
                newrow["FAT"] = dr["fat"].ToString();
                newrow["SNF"] = dr["snf"].ToString();
                newrow["KG FAT"] = ltrfat;
                newrow["KG SNF"] = ltrsnf;
                returnmilk.Rows.Add(newrow);
            }
            DataRow newvartical2 = returnmilk.NewRow();
            newvartical2["Department Name"] = "Total";
            newvartical2["qty(ltr)"] = Math.Round(returnmilkltrstotal, 2);
            newvartical2["qty(kgs)"] = Math.Round(returnmilkkgstotal, 2);
            returnmilk.Rows.Add(newvartical2);
            DataRow newvartical3 = returnmilk.NewRow();
            newvartical3["Department Name"] = "Total1";
            returnmilk.Rows.Add(newvartical3);
            grdreturnmilk.DataSource = returnmilk;
            grdreturnmilk.DataBind();
            Session["xportdata"] = returnmilk;
            pnlreturnmilk.Visible = true;
        }
        else
        {
            DataRow newrow = returnmilk.NewRow();
            newrow["Department Name"] = "total";
            DataRow newvartical3 = returnmilk.NewRow();
            newvartical3["Department Name"] = "Total1";
            returnmilk.Rows.Add(newvartical3);
            grdreturnmilk.DataSource = returnmilk;
            grdreturnmilk.DataBind();
            Session["xportdata"] = returnmilk;
            pnlreturnmilk.Visible = true;
        }

        curd.Columns.Add("Silo Name");
        curd.Columns.Add("Department Name");
        curd.Columns.Add("qty(ltr)");
        curd.Columns.Add("qty(kgs)");
        curd.Columns.Add("FAT");
        curd.Columns.Add("SNF");
        curd.Columns.Add("CLR");
        curd.Columns.Add("KG FAT");
        curd.Columns.Add("KG SNF");
        cmd = new SqlCommand("SELECT SiloId, SiloName From silomaster");
        DataTable dtcsilo = SalesDB.SelectQuery(cmd).Tables[0];
        cmd = new SqlCommand("SELECT silo_outward_transaction.date, silo_outward_transaction.siloid, silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf, silo_outward_transaction.clr FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId  WHERE (silo_outward_transaction.departmentid = '1') AND (silo_outward_transaction.date BETWEEN @cdate1 AND @cdate2) AND (silo_outward_transaction.branchid=@cubranchid) AND  (silo_outward_transaction.fat < 4.5)");
        cmd.Parameters.Add("@cdate1", GetLowDate(fromdate));
        cmd.Parameters.Add("@cdate2", GetHighDate(todate));
        cmd.Parameters.Add("@cubranchid", BranchID);
        DataTable dtcurd = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtcsilo.Rows.Count > 0)
        {
            curdltrstotal = 0;
            curdkgstotal = 0;
            curdfattotal = 0;
            curdsnftotal = 0;
            curdkgfattotal = 0;
            curdkgsnftotal = 0;
            foreach (DataRow drsilo in dtcsilo.Rows)
            {
                string siloid = drsilo["SiloId"].ToString();
                DataTable dtsiloin = new DataTable();
                DataRow[] drv = dtcurd.Select("siloid='" + siloid + "'");
                if (drv.Length > 0)
                {
                    dtsiloin = drv.CopyToDataTable();
                }
                if (dtsiloin.Rows.Count > 0)
                {
                    string Siloname = "";
                    string Departmentname = "";
                    double FAT = 0;
                    double SNF = 0;
                    double ltrfat = 0;
                    double ltrsnf = 0;
                    foreach (DataRow dr in dtsiloin.Rows)
                    {

                        Siloname = dr["SiloName"].ToString();
                        Departmentname = dr["departmentname"].ToString();
                        double ltrs = 0;
                        double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                        curdltrstotal += ltrs;
                        double Kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                        curdkgstotal += Kgs;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        curdfattotal += FAT;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        curdsnftotal += SNF;
                        ltrfat = Kgs * FAT;
                        ltrfat = Math.Round(ltrfat / 100, 2);
                        curdkgfattotal += ltrfat;
                        curdtotalkgfat += ltrfat;
                        ltrsnf = Kgs * SNF;
                        ltrsnf = Math.Round(ltrsnf / 100, 2);
                        curdkgsnftotal += ltrsnf;
                        curdtotalkgsnf += ltrsnf;
                    }
                    DataRow newrow = curd.NewRow();
                    newrow["Silo Name"] = Siloname;
                    newrow["Department Name"] = Departmentname;
                    newrow["qty(ltr)"] = curdltrstotal;
                    curdtotalltrs += curdltrstotal;
                    newrow["qty(kgs)"] = curdkgstotal;
                    curdtotalkgs += curdkgstotal;
                    newrow["KG FAT"] = curdkgfattotal;
                    newrow["KG SNF"] = curdkgsnftotal;
                    double fatt = curdkgfattotal / curdkgstotal;
                    newrow["FAT"] = Math.Round(fatt * 100, 2);
                    double snft = curdkgsnftotal / curdkgstotal;
                    newrow["SNF"] = Math.Round(snft * 100, 2);
                    curd.Rows.Add(newrow);
                    curdkgfattotal = 0;
                    curdkgsnftotal = 0;
                    curdltrstotal = 0;
                    curdkgstotal = 0;
                }
            }
            DataRow newvartical21 = curd.NewRow();
            newvartical21["Silo Name"] = "Bi-Products";
            curd.Rows.Add(newvartical21);
            cmd = new SqlCommand("SELECT silo_outward_transaction.date, silo_outward_transaction.siloid, silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf, silo_outward_transaction.clr FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId  WHERE (silo_outward_transaction.departmentid = '1') AND (silo_outward_transaction.date BETWEEN @cdate1 AND @cdate2) AND (silo_outward_transaction.branchid=@cubranchid) AND  (silo_outward_transaction.fat > 4.5)");
            cmd.Parameters.Add("@cdate1", GetLowDate(fromdate));
            cmd.Parameters.Add("@cdate2", GetHighDate(todate));
            cmd.Parameters.Add("@cubranchid", BranchID);
            DataTable dtcurdb = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtcurdb.Rows.Count > 0)
            {
                curdltrstotal = 0;
                curdkgstotal = 0;
                curdfattotal = 0;
                curdsnftotal = 0;
                curdkgfattotal = 0;
                curdkgsnftotal = 0;
                foreach (DataRow drsilo in dtcsilo.Rows)
                {
                    string siloid = drsilo["SiloId"].ToString();
                    DataTable dtsiloin = new DataTable();
                    DataRow[] drv = dtcurdb.Select("siloid='" + siloid + "'");
                    if (drv.Length > 0)
                    {
                        dtsiloin = drv.CopyToDataTable();
                    }
                    if (dtsiloin.Rows.Count > 0)
                    {
                        string Siloname = "";
                        string Departmentname = "";
                        double FAT = 0;
                        double SNF = 0;
                        double ltrfat = 0;
                        double ltrsnf = 0;
                        foreach (DataRow dr in dtsiloin.Rows)
                        {

                            Siloname = dr["SiloName"].ToString();
                            Departmentname = dr["departmentname"].ToString();
                            double ltrs = 0;
                            double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                            curdltrstotal += ltrs;
                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                            curdkgstotal += Kgs;
                            double.TryParse(dr["fat"].ToString(), out FAT);
                            curdfattotal += FAT;
                            double.TryParse(dr["snf"].ToString(), out SNF);
                            curdsnftotal += SNF;
                            ltrfat = Kgs * FAT;
                            ltrfat = Math.Round(ltrfat / 100, 2);
                            curdkgfattotal += ltrfat;
                            curdtotalkgfat += ltrfat;
                            ltrsnf = Kgs * SNF;
                            ltrsnf = Math.Round(ltrsnf / 100, 2);
                            curdkgsnftotal += ltrsnf;
                            curdtotalkgsnf += ltrsnf;
                        }
                        DataRow newrow = curd.NewRow();
                        newrow["Silo Name"] = Siloname;
                        newrow["Department Name"] = Departmentname;
                        newrow["qty(ltr)"] = curdltrstotal;
                        curdtotalltrs += curdltrstotal;
                        newrow["qty(kgs)"] = curdkgstotal;
                        curdtotalkgs += curdkgstotal;
                        newrow["KG FAT"] = curdkgfattotal;
                        newrow["KG SNF"] = curdkgsnftotal;
                        double fatt = curdkgfattotal / curdkgstotal;
                        newrow["FAT"] = Math.Round(fatt * 100, 2);
                        double snft = curdkgsnftotal / curdkgstotal;
                        newrow["SNF"] = Math.Round(snft * 100, 2);
                        curd.Rows.Add(newrow);
                        curdkgfattotal = 0;
                        curdkgsnftotal = 0;
                        curdltrstotal = 0;
                        curdkgstotal = 0;
                    }
                }
                DataRow newvartical2 = curd.NewRow();
                newvartical2["Silo Name"] = "Total";
                newvartical2["qty(ltr)"] = curdtotalltrs;
                newvartical2["qty(kgs)"] = curdtotalkgs;
                newvartical2["KG FAT"] = curdtotalkgfat;
                newvartical2["KG SNF"] = curdtotalkgsnf;
                double kgfatt = curdtotalkgfat / curdtotalkgs;
                newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
                double kgSNFt = curdtotalkgsnf / curdtotalkgs;
                newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);
                curd.Rows.Add(newvartical2);

                DataRow newvartical3 = curd.NewRow();
                //newvartical3["Silo Name"] = "Condensed Milk";
                newvartical3["Silo Name"] = "Dispatch Tankers";
                curd.Rows.Add(newvartical3);

                grdcurd.DataSource = curd;
                grdcurd.DataBind();
                Session["xportdata"] = curd;
                pnlcurd.Visible = true;
            }
        }
        else
        {
            DataRow newvartical3 = curd.NewRow();
            //newvartical3["Silo Name"] = "Condensed Milk";
            newvartical3["Silo Name"] = "Dispatch Tankers";
            curd.Rows.Add(newvartical3);
            grdcurd.DataSource = curd;
            grdcurd.DataBind();
            Session["xportdata"] = curd;
            pnlcurd.Visible = true;
        }

        despatch.Columns.Add("ProductName");
        despatch.Columns.Add("Qty(ltrs)");
        despatch.Columns.Add("Qty(kgs)");
        despatch.Columns.Add("FAT");
        despatch.Columns.Add("SNF");
        despatch.Columns.Add("KG FAT");
        despatch.Columns.Add("KG SNF");
        VehicleDBMgr vdmnr = new VehicleDBMgr();
        string ddates = "12/1/2017"; 
        DateTime dtdates = Convert.ToDateTime(ddates);
        if (dtdates < fromdate)
        {
            if (BranchID == "1")
            {
                cmd = new SqlCommand("SELECT  sno, productid, fat, snf, qty_kgs, total, doe, section FROM     plant_production_details WHERE   (section = 'sales') AND (doe BETWEEN @sd1 AND @sd2) AND (branchid = @sbranchid) order by productid");
                cmd.Parameters.Add("@sbranchid", BranchID);
                cmd.Parameters.Add("@sd1", GetLowDate(fromdate));
                cmd.Parameters.Add("@sd2", GetHighDate(todate));
                DataTable dtsales = SalesDB.SelectQuery(cmd).Tables[0];
                mycmd = new MySqlCommand("SELECT   sno, category_sno, SubCatName, Flag, userdata_sno, fat FROM   products_subcategory ");
                DataTable pdt = vdmnr.SelectQuery(mycmd).Tables[0];
                if (dtsales.Rows.Count > 0)
                {
                    foreach (DataRow drsale in pdt.Rows)
                    {
                        totaldesp = 0;
                        despatchkgfattotal = 0;
                        despatchkgsnftotal = 0;
                        despatchfattotal = 0;
                        despatchsnftotal = 0;
                        string productid = drsale["sno"].ToString();
                        DataTable dtpins = new DataTable();
                        DataRow[] drcvs = dtsales.Select("productid='" + productid + "'");
                        if (drcvs.Length > 0)
                        {
                            dtpins = drcvs.CopyToDataTable();
                        }
                        if (dtpins.Rows.Count > 0)
                        {
                            string productname = "";
                            double FAT = 0;
                            double SNF = 0;
                            foreach (DataRow drr in dtpins.Rows)
                            {
                                double qty = 0;
                                double.TryParse(drr["total"].ToString(), out qty);
                                totaldesp += qty;
                                despatchtotalltrs += qty;

                                double qtyltrkgs = 0;
                                double.TryParse(drr["qty_kgs"].ToString(), out qtyltrkgs);
                                despatchkgstotal += qtyltrkgs;
                                despatchtotalkgs += qtyltrkgs;

                                double.TryParse(drr["fat"].ToString(), out FAT);
                                double.TryParse(drr["snf"].ToString(), out SNF);

                                double KGFAT = qtyltrkgs * FAT;
                                double KGSNF = qtyltrkgs * SNF;
                                KGFAT = Math.Round(KGFAT / 100, 2);
                                despatchkgfattotal += KGFAT;
                                despatchkgtotalfat += KGFAT;
                                KGSNF = Math.Round(KGSNF / 100, 2);

                                despatchkgsnftotal += KGSNF;
                                despatchkgtotalsnf += KGSNF;

                                despatchfattotal += FAT;
                                despatchsnftotal += SNF;
                                double fatt = despatchkgfattotal / despatchkgstotal;
                                double snft = despatchkgsnftotal / despatchkgstotal;
                            }
                            DataRow newrow = despatch.NewRow();
                            newrow["ProductName"] = drsale["SubCatName"].ToString();
                            newrow["Qty(ltrs)"] = totaldesp;
                            newrow["Qty(kgs)"] = despatchkgstotal;
                            newrow["KG FAT"] = despatchkgfattotal;
                            newrow["KG SNF"] = despatchkgsnftotal;
                            double fatts = despatchkgfattotal / despatchkgstotal;
                            newrow["FAT"] = Math.Round(fatts * 100, 2);
                            double snfts = despatchkgsnftotal / despatchkgstotal;
                            newrow["SNF"] = Math.Round(snfts * 100, 2);
                            despatchkgstotal = 0;
                            despatchkgfattotal = 0;
                            despatchkgsnftotal = 0;
                            totaldesp = 0;
                            despatch.Rows.Add(newrow);
                        }
                    }
                    DataRow newvartical2 = despatch.NewRow();
                    newvartical2["ProductName"] = "Total";
                    despatchtotalltrs = Math.Round(despatchtotalltrs, 2);

                    newvartical2["Qty(ltrs)"] = Math.Round(despatchtotalltrs, 2);
                    newvartical2["Qty(kgs)"] = Math.Round(despatchtotalkgs, 2);

                    double kgfatt = despatchkgtotalfat / despatchtotalkgs;
                    newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);

                    double kgSNFt = despatchkgtotalsnf / despatchtotalkgs;
                    newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);

                    newvartical2["KG FAT"] = Math.Round(despatchkgtotalfat, 2);
                    newvartical2["KG SNF"] = Math.Round(despatchkgtotalsnf, 2);

                    despatch.Rows.Add(newvartical2);
                    DataRow newvartical3 = despatch.NewRow();
                    newvartical3["ProductName"] = "Curd Block";
                    despatch.Rows.Add(newvartical3);
                    grddespatch.DataSource = despatch;
                    grddespatch.DataBind();
                    Session["xportdata"] = despatch;
                    pnldespatch.Visible = true;
                }
            }
            else
            {
                // Kuppam Plant
                if (BranchID == "22")
                {
                    mycmd = new MySqlCommand("SELECT tripdata.Sno, tripdata.AssignDate, tripdata.Status, DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS IndentDate, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS despatchqty, productsdata.ProductName, productsdata.Inventorysno, products_category.Categoryname, products_subcategory.SubCatName FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN  productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN  (SELECT branch_sno, product_sno, Rank  FROM branchproducts WHERE (branch_sno = @branch)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @branch) AND (products_category.Categoryname = 'MILK') GROUP BY products_category.Categoryname, products_subcategory.SubCatName ORDER BY brnchprdt.Rank");
                    mycmd.Parameters.Add("@branch", "1801");
                    mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    mycmd.Parameters.Add("@d2", GetHighDate(todate));
                }
                // Wyra Plant
                else if (BranchID == "26")
                {
                    mycmd = new MySqlCommand("SELECT tripdata.Sno, tripdata.AssignDate, tripdata.Status, DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS IndentDate, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS despatchqty, productsdata.ProductName, productsdata.Inventorysno, products_category.Categoryname, products_subcategory.SubCatName FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN  productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN  (SELECT branch_sno, product_sno, Rank  FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @BranchID) AND (products_category.Categoryname = 'MILK') GROUP BY products_category.Categoryname, products_subcategory.SubCatName ORDER BY brnchprdt.Rank");
                    mycmd.Parameters.Add("@BranchID", "158");
                    mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    mycmd.Parameters.Add("@d2", GetHighDate(todate));
                }
                // Hyderabad Plant
                else if (BranchID == "115")
                {
                    mycmd = new MySqlCommand("SELECT tripdata.Sno, tripdata.AssignDate, tripdata.Status, DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS IndentDate, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS despatchqty, productsdata.ProductName, productsdata.Inventorysno, products_category.Categoryname, products_subcategory.SubCatName FROM            tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN  productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN  (SELECT        branch_sno, product_sno, Rank  FROM            branchproducts   WHERE        (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE        (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @BranchID) AND (products_category.Categoryname = 'MILK') GROUP BY products_category.Categoryname, products_subcategory.SubCatName ORDER BY brnchprdt.Rank");
                    mycmd.Parameters.Add("@BranchID", "4626");
                    mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    mycmd.Parameters.Add("@d2", GetHighDate(todate));
                }
                DataTable dtTotalDespatch_subcategorywise = vdmnr.SelectQuery(mycmd).Tables[0];
                if (dtTotalDespatch_subcategorywise.Rows.Count > 0)
                {
                    totaldesp = 0;
                    despatchkgfattotal = 0;
                    despatchkgsnftotal = 0;
                    despatchfattotal = 0;
                    despatchsnftotal = 0;
                    foreach (DataRow dr in dtTotalDespatch_subcategorywise.Rows)
                    {
                        double FAT = 0;
                        double SNF = 0;
                        double clr = 0;
                        DataRow newrow = despatch.NewRow();
                        newrow["ProductName"] = dr["SubCatName"].ToString();

                        string productname = dr["SubCatName"].ToString();
                        if (productname == "STD" || productname == "AP-SM" || productname == "SM")
                        {
                            FAT = 4.5;
                            SNF = 9.055;
                            clr = 32;
                        }
                        if (productname == "FCM" || productname == "GOLD")
                        {
                            FAT = 5.8;
                            SNF = 9.203;
                            clr = 31;
                        }
                        if (productname == "DTM")
                        {
                            FAT = 1.5;
                            SNF = 9.04;
                            clr = 33.5;
                        }
                        if (productname == "TM" || productname == "COW MILK" || productname == "TAAZA")
                        {
                            FAT = 3.0;
                            SNF = 9.24;
                            clr = 33.5;
                        }
                        if (productname == "NH")
                        {
                            FAT = 3.8;
                            SNF = 7.91;
                            clr = 29;
                        }
                        if (productname == "WholeMilk")
                        {
                            FAT = 3.8;
                            SNF = 7.91;
                            clr = 29;
                        }
                        if (productname == "GoldPlus")
                        {
                            FAT = 6.0;
                            SNF = 8.5;
                            clr = 29;
                        }
                        if (productname == "MahaGold" || productname == "SuperGold")
                        {
                            FAT = 6.2;
                            SNF = 9.0;
                            clr = 30;
                        }
                        double qty = 0;
                        double.TryParse(dr["despatchqty"].ToString(), out qty);
                        newrow["Qty(ltrs)"] = Math.Round(qty, 2);
                        totaldesp += qty;
                        despatchtotalltrs += qty;

                        despatchclrtotal += clr;
                        double modclr = (clr / 1000) + 1;
                        double qtyltrkgs = qty * modclr;
                        newrow["Qty(kgs)"] = Math.Round(qtyltrkgs, 2);

                        despatchkgstotal += qtyltrkgs;
                        despatchtotalkgs += qtyltrkgs;

                        double KGFAT = qtyltrkgs * FAT;
                        double KGSNF = qtyltrkgs * SNF;
                        KGFAT = Math.Round(KGFAT / 100, 2);
                        KGSNF = Math.Round(KGSNF / 100, 2);

                        despatchkgfattotal += KGFAT;

                        despatchkgtotalfat += KGFAT;

                        despatchkgsnftotal += KGSNF;

                        despatchkgtotalsnf += KGSNF;

                        despatchfattotal += FAT;
                        despatchsnftotal += SNF;

                        newrow["KG FAT"] = despatchkgfattotal;
                        newrow["KG SNF"] = despatchkgsnftotal;

                        double fatt = despatchkgfattotal / despatchkgstotal;
                        newrow["FAT"] = Math.Round(fatt * 100, 2);

                        double snft = despatchkgsnftotal / despatchkgstotal;
                        newrow["SNF"] = Math.Round(snft * 100, 2);
                        despatch.Rows.Add(newrow);
                        despatchkgstotal = 0;
                        despatchkgfattotal = 0;
                        despatchkgsnftotal = 0;
                        totaldesp = 0;
                    }
                    DataRow newvartical2 = despatch.NewRow();
                    newvartical2["ProductName"] = "Total";
                    despatchtotalltrs = Math.Round(despatchtotalltrs, 2);

                    newvartical2["Qty(ltrs)"] = Math.Round(despatchtotalltrs, 2);
                    newvartical2["Qty(kgs)"] = Math.Round(despatchtotalkgs, 2);

                    double kgfatt = despatchkgtotalfat / despatchtotalkgs;
                    newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);

                    double kgSNFt = despatchkgtotalsnf / despatchtotalkgs;
                    newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);

                    newvartical2["KG FAT"] = Math.Round(despatchkgtotalfat, 2);
                    newvartical2["KG SNF"] = Math.Round(despatchkgtotalsnf, 2);
                    despatch.Rows.Add(newvartical2);
                    DataRow newvartical3 = despatch.NewRow();
                    newvartical3["ProductName"] = "Curd Block";
                    despatch.Rows.Add(newvartical3);
                    grddespatch.DataSource = despatch;
                    grddespatch.DataBind();
                    Session["xportdata"] = despatch;
                    pnldespatch.Visible = true;
                }
            }
        }
        else
        {
            // Punabaka Plant 
            if (BranchID == "1")
            {
                mycmd = new MySqlCommand("SELECT tripdata.Sno, tripdata.AssignDate, tripdata.Status, DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS IndentDate, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS despatchqty, productsdata.ProductName, productsdata.Inventorysno, products_category.Categoryname, products_subcategory.SubCatName FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN  productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN  (SELECT branch_sno, product_sno, Rank  FROM branchproducts WHERE (branch_sno = @branch)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @branch) AND (products_category.Categoryname = 'MILK') GROUP BY products_category.Categoryname, products_subcategory.SubCatName ORDER BY brnchprdt.Rank");
                mycmd.Parameters.Add("@branch", 172);
                mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
                mycmd.Parameters.Add("@d2", GetHighDate(todate));
            }
            // Kuppam Plant
            else if (BranchID == "22")
            {
                mycmd = new MySqlCommand("SELECT tripdata.Sno, tripdata.AssignDate, tripdata.Status, DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS IndentDate, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS despatchqty, productsdata.ProductName, productsdata.Inventorysno, products_category.Categoryname, products_subcategory.SubCatName FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN  productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN  (SELECT branch_sno, product_sno, Rank  FROM branchproducts WHERE (branch_sno = @branch)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @branch) AND (products_category.Categoryname = 'MILK') GROUP BY products_category.Categoryname, products_subcategory.SubCatName ORDER BY brnchprdt.Rank");
                mycmd.Parameters.Add("@branch", 1801);
                mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
                mycmd.Parameters.Add("@d2", GetHighDate(todate));
            }
            // Wyra Plant
            else if (BranchID == "26")
            {
                mycmd = new MySqlCommand("SELECT tripdata.Sno, tripdata.AssignDate, tripdata.Status, DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS IndentDate, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS despatchqty, productsdata.ProductName, productsdata.Inventorysno, products_category.Categoryname, products_subcategory.SubCatName FROM            tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN  productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN  (SELECT        branch_sno, product_sno, Rank  FROM            branchproducts   WHERE        (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE        (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @BranchID) AND (products_category.Categoryname = 'MILK') GROUP BY products_category.Categoryname, products_subcategory.SubCatName ORDER BY brnchprdt.Rank");
                mycmd.Parameters.Add("@BranchID", 158);
                mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
                mycmd.Parameters.Add("@d2", GetHighDate(todate));
            }
            // Hyderabad Plant
            else if (BranchID == "115")
            {
                mycmd = new MySqlCommand("SELECT tripdata.Sno, tripdata.AssignDate, tripdata.Status, DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS IndentDate, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS despatchqty, productsdata.ProductName, productsdata.Inventorysno, products_category.Categoryname, products_subcategory.SubCatName FROM            tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN  productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN  (SELECT        branch_sno, product_sno, Rank  FROM            branchproducts   WHERE        (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE        (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @BranchID) AND (products_category.Categoryname = 'MILK') GROUP BY products_category.Categoryname, products_subcategory.SubCatName ORDER BY brnchprdt.Rank");
                mycmd.Parameters.Add("@BranchID", 4626);
                mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
                mycmd.Parameters.Add("@d2", GetHighDate(todate));
            }
            DataTable dtTotalDespatch_subcategorywise = vdmnr.SelectQuery(mycmd).Tables[0];
            if (dtTotalDespatch_subcategorywise.Rows.Count > 0)
            {
                totaldesp = 0;
                despatchkgfattotal = 0;
                despatchkgsnftotal = 0;
                despatchfattotal = 0;
                despatchsnftotal = 0;
                foreach (DataRow dr in dtTotalDespatch_subcategorywise.Rows)
                {
                    double FAT = 0;
                    double SNF = 0;
                    double clr = 0;
                    DataRow newrow = despatch.NewRow();
                    newrow["ProductName"] = dr["SubCatName"].ToString();

                    string productname = dr["SubCatName"].ToString();
                    if (productname == "STD" || productname == "AP-SM" || productname == "SM")
                    {
                        FAT = 4.5;
                        SNF = 9.055;
                        clr = 32;
                    }
                    if (productname == "FCM" || productname == "GOLD")
                    {
                        FAT = 5.8;
                        SNF = 9.203;
                        clr = 31;
                    }
                    if (productname == "DTM")
                    {
                        FAT = 1.5;
                        SNF = 9.04;
                        clr = 33.5;
                    }
                    if (productname == "TM" || productname == "COW MILK" || productname == "TAAZA")
                    {
                        FAT = 3.0;
                        SNF = 9.24;
                        clr = 33.5;
                    }
                    if (productname == "NH")
                    {
                        FAT = 3.8;
                        SNF = 7.91;
                        clr = 29;
                    }
                    if (productname == "WholeMilk")
                    {
                        FAT = 3.8;
                        SNF = 7.91;
                        clr = 29;
                    }
                    if (productname == "GoldPlus")
                    {
                        FAT = 6.0;
                        SNF = 8.5;
                        clr = 29;
                    }
                    if (productname == "MahaGold" || productname == "SuperGold")
                    {
                        FAT = 6.2;
                        SNF = 9.0;
                        clr = 30;
                    }
                    double qty = 0;
                    double.TryParse(dr["despatchqty"].ToString(), out qty);
                    newrow["Qty(ltrs)"] = Math.Round(qty, 2);
                    totaldesp += qty;
                    despatchtotalltrs += qty;

                    despatchclrtotal += clr;
                    double modclr = (clr / 1000) + 1;
                    double qtyltrkgs = qty * modclr;
                    newrow["Qty(kgs)"] = Math.Round(qtyltrkgs, 2);

                    despatchkgstotal += qtyltrkgs;
                    despatchtotalkgs += qtyltrkgs;

                    double KGFAT = qtyltrkgs * FAT;
                    double KGSNF = qtyltrkgs * SNF;
                    KGFAT = Math.Round(KGFAT / 100, 2);
                    KGSNF = Math.Round(KGSNF / 100, 2);

                    despatchkgfattotal += KGFAT;

                    despatchkgtotalfat += KGFAT;

                    despatchkgsnftotal += KGSNF;

                    despatchkgtotalsnf += KGSNF;

                    despatchfattotal += FAT;
                    despatchsnftotal += SNF;

                    newrow["KG FAT"] = despatchkgfattotal;
                    newrow["KG SNF"] = despatchkgsnftotal;

                    double fatt = despatchkgfattotal / despatchkgstotal;
                    newrow["FAT"] = Math.Round(fatt * 100, 2);

                    double snft = despatchkgsnftotal / despatchkgstotal;
                    newrow["SNF"] = Math.Round(snft * 100, 2);
                    despatch.Rows.Add(newrow);
                    despatchkgstotal = 0;
                    despatchkgfattotal = 0;
                    despatchkgsnftotal = 0;
                    totaldesp = 0;
                }
                DataRow newvartical2 = despatch.NewRow();
                newvartical2["ProductName"] = "Total";
                despatchtotalltrs = Math.Round(despatchtotalltrs, 2);

                newvartical2["Qty(ltrs)"] = Math.Round(despatchtotalltrs, 2);
                newvartical2["Qty(kgs)"] = Math.Round(despatchtotalkgs, 2);

                double kgfatt = despatchkgtotalfat / despatchtotalkgs;
                newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);

                double kgSNFt = despatchkgtotalsnf / despatchtotalkgs;
                newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);

                newvartical2["KG FAT"] = Math.Round(despatchkgtotalfat, 2);
                newvartical2["KG SNF"] = Math.Round(despatchkgtotalsnf, 2);

                despatch.Rows.Add(newvartical2);
                DataRow newvartical3 = despatch.NewRow();
                newvartical3["ProductName"] = "Curd Block";
                despatch.Rows.Add(newvartical3);
                grddespatch.DataSource = despatch;
                grddespatch.DataBind();
                Session["xportdata"] = despatch;
                pnldespatch.Visible = true;
            }
        }
        tanker.Columns.Add("Silo Name");
        tanker.Columns.Add("Department Name");
        tanker.Columns.Add("Batch");
        tanker.Columns.Add("qty(ltr)");
        tanker.Columns.Add("qty(kgs)");
        tanker.Columns.Add("FAT");
        tanker.Columns.Add("SNF");
        tanker.Columns.Add("CLR");
        tanker.Columns.Add("KG FAT");
        tanker.Columns.Add("KG SNF");
        cmd = new SqlCommand("SELECT SiloId, SiloName From silomaster");
        DataTable dtdsilo = SalesDB.SelectQuery(cmd).Tables[0];
        cmd = new SqlCommand("SELECT    batchid, batch, batchcode FROM    batchmaster");
         DataTable dtbatch = SalesDB.SelectQuery(cmd).Tables[0];
        cmd = new SqlCommand("SELECT   silo_outward_transaction.date, silo_outward_transaction.siloid, silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs,  silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf, silo_outward_transaction.clr, batchmaster.batch FROM   silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId INNER JOIN batchmaster ON silo_outward_transaction.productid = batchmaster.batchid WHERE    (silo_outward_transaction.departmentid = '4') AND (silo_outward_transaction.date BETWEEN @cdate1 AND @cdate2) AND (silo_outward_transaction.branchid = @tbranchid) ORDER BY silo_outward_transaction.productid");
        cmd.Parameters.Add("@cdate1", GetLowDate(fromdate));
        cmd.Parameters.Add("@cdate2", GetHighDate(todate));
        cmd.Parameters.Add("@tbranchid", BranchID);
        DataTable dttanker = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtdsilo.Rows.Count > 0)
        {
            tankerltrstotal = 0;
            tankerkgstotal = 0;
            tankerfattotal = 0;
            tankersnftotal = 0;
            tankerkgfattotal = 0;
            tankerkgsnftotal = 0;
            foreach (DataRow drdisp in dtdsilo.Rows)
            {

                string siloid = drdisp["siloid"].ToString();
                foreach (DataRow drb in dtbatch.Rows)
                {
                    string batchid = drb["batchid"].ToString();
                    DataTable dtdispatchtankerin = new DataTable();
                    //"dcno='" + dcno + "' AND sectionid='" + vendorid + "'"
                    DataRow[] drv = dttanker.Select("siloid='" + siloid + "' AND productid='" + batchid + "'");
                    if (drv.Length > 0)
                    {
                        dtdispatchtankerin = drv.CopyToDataTable();
                    }
                    if (dtdispatchtankerin.Rows.Count > 0)
                    {
                        string Siloname = "";
                        string Departmentname = "";
                        string batch = "";
                        double FAT = 0;
                        double SNF = 0;
                        double ltrfat = 0;
                        double ltrsnf = 0;
                        foreach (DataRow dr in dtdispatchtankerin.Rows)
                        {
                            Siloname = dr["SiloName"].ToString();
                            Departmentname = dr["departmentname"].ToString();
                            batch = dr["batch"].ToString();

                            double ltrs = 0;
                            double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                            tankerltrstotal += ltrs;
                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                            tankerkgstotal += Kgs;

                            double.TryParse(dr["fat"].ToString(), out FAT);
                            tankerfattotal += FAT;

                            double.TryParse(dr["snf"].ToString(), out SNF);
                            tankersnftotal += SNF;

                            ltrfat = Kgs * FAT;
                            ltrfat = Math.Round(ltrfat / 100, 0);
                            tankerkgfattotal += ltrfat;

                            ltrsnf = Kgs * SNF;
                            ltrsnf = Math.Round(ltrsnf / 100, 0);
                            tankerkgsnftotal += ltrsnf;
                        }
                        DataRow newrow = tanker.NewRow();
                        newrow["Silo Name"] = Siloname;
                        newrow["Department Name"] = Departmentname;
                        newrow["Batch"] = batch;
                        newrow["qty(ltr)"] = tankerltrstotal;
                        tankertotalltrs += tankerltrstotal;
                        newrow["qty(kgs)"] = tankerkgstotal;
                        tankertotalkgs += tankerkgstotal;
                        newrow["KG FAT"] = tankerkgfattotal;
                        tankerkgtotalfat += tankerkgfattotal;
                        newrow["KG SNF"] = tankerkgsnftotal;
                        tankerkgtotalsnf += tankerkgsnftotal;
                        double fatt = tankerkgfattotal / tankerkgstotal;
                        newrow["FAT"] = Math.Round(fatt * 100, 2);
                        double snft = tankerkgsnftotal / tankerkgstotal;
                        newrow["SNF"] = Math.Round(snft * 100, 2);
                        tanker.Rows.Add(newrow);
                        tankerkgstotal = 0;
                        tankerkgsnftotal = 0;
                        tankerkgfattotal = 0;
                        tankerltrstotal = 0;
                    }
                }
            }
            DataRow newvartical2 = tanker.NewRow();
            newvartical2["Silo Name"] = "Total";
            newvartical2["qty(ltr)"] = tankertotalltrs;
            newvartical2["qty(kgs)"] = tankertotalkgs;
            newvartical2["KG FAT"] = tankerkgtotalfat;
            newvartical2["KG SNF"] = tankerkgtotalsnf;
            double kgfatt = tankerkgtotalfat / tankertotalkgs;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            double kgsnft = tankerkgtotalsnf / tankertotalkgs;
            newvartical2["SNF"] = Math.Round(kgsnft * 100, 2);
            tanker.Rows.Add(newvartical2);
            DataRow newvartical3 = tanker.NewRow();
            newvartical3["Silo Name"] = "Closing Balance";
            tanker.Rows.Add(newvartical3);
            grdtankers.DataSource = tanker;
            grdtankers.DataBind();
            Session["xportdata"] = tanker;
            pnldispatchtankers.Visible = true;
        }
        else
        {
            DataRow new1 = tanker.NewRow();
            new1["Silo Name"] = "Total";
            tanker.Rows.Add(new1);
            DataRow new2 = tanker.NewRow();
            new2["Silo Name"] = "Closing Balance";
            tanker.Rows.Add(new2);
            grdtankers.DataSource = tanker;
            grdtankers.DataBind();
            Session["xportdata"] = tanker;
            pnldispatchtankers.Visible = true;

        }
        siloclosingReport.Columns.Add("siloname");
        siloclosingReport.Columns.Add("qty(ltr)");
        siloclosingReport.Columns.Add("qty(kgs)");
        siloclosingReport.Columns.Add("FAT");
        siloclosingReport.Columns.Add("SNF");
        siloclosingReport.Columns.Add("CLR");
        siloclosingReport.Columns.Add("KG FAT");
        siloclosingReport.Columns.Add("KG SNF");
        cmd = new SqlCommand("Select sm.SiloName, scd.qty_kgs, scd.fat, scd.snf, scd.clr, scd.closingdate from silowiseclosingdetails scd INNER JOIN silomaster sm on sm.SiloId=scd.siloid where (scd.closingdate BETWEEN @d1 AND @d2) AND (scd.branchid=@sbranchid)");
        cmd.Parameters.Add("@d1", GetLowDate(todate).AddDays(1));
        cmd.Parameters.Add("@d2", GetHighDate(todate).AddDays(1));
        cmd.Parameters.Add("@sbranchid", BranchID);
        DataTable dtsiloclosing = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtsiloclosing.Rows.Count > 0)
        {
            siloclosingltrstotal = 0;
            siloclosingkgstotal = 0;
            siloclosingfattotal = 0;
            siloclosingsnftotal = 0;
            siloclosingclrtotal = 0;
            siloclosingkgfattotal = 0;
            siloclosingkgsnftotal = 0;
            foreach (DataRow dr in dtsiloclosing.Rows)
            {
                DataRow newrow = siloclosingReport.NewRow();
                newrow["siloname"] = dr["SiloName"].ToString();
                newrow["qty(ltr)"] = dr["qty_kgs"].ToString();
                newrow["FAT"] = dr["fat"].ToString();
                newrow["SNF"] = dr["snf"].ToString();
                newrow["CLR"] = dr["clr"].ToString();
                double ltrs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out ltrs);
                siloclosingltrstotal += ltrs;
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                siloclosingfattotal += FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                siloclosingsnftotal += SNF;
                double clr = 0;
                double.TryParse(dr["clr"].ToString(), out clr);
                siloclosingclrtotal += clr;
                double modclr = (clr / 1000) + 1;
                double qtyltrkgs = ltrs * modclr;
                newrow["qty(kgs)"] = qtyltrkgs;
                siloclosingkgstotal += qtyltrkgs;
                double KGFAT = 0;
                double KGSNF = 0;
                KGFAT = (FAT * qtyltrkgs) / 100;
                KGSNF = (SNF * qtyltrkgs) / 100;
                siloclosingkgfattotal += KGFAT;
                siloclosingkgsnftotal += KGSNF;
                newrow["KG FAT"] = Math.Round(KGFAT, 2);
                newrow["KG SNF"] = Math.Round(KGSNF, 2);
                siloclosingReport.Rows.Add(newrow);
            }
            DataRow newvartical2 = siloclosingReport.NewRow();
            newvartical2["siloname"] = "Total";
            newvartical2["qty(ltr)"] = siloclosingltrstotal;
            newvartical2["qty(kgs)"] = siloclosingkgstotal;
            newvartical2["KG FAT"] = Math.Round(siloclosingkgfattotal, 2);
            newvartical2["KG SNF"] = Math.Round(siloclosingkgsnftotal, 2);

            double kgfatt = siloclosingkgfattotal / siloclosingkgstotal;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);

            double kgsnft = siloclosingkgsnftotal / siloclosingkgstotal;
            newvartical2["SNF"] = Math.Round(kgsnft * 100, 2);
            siloclosingReport.Rows.Add(newvartical2);

            DataRow newvartical3 = siloclosingReport.NewRow();
            newvartical3["siloname"] = "Cream Production";
            siloclosingReport.Rows.Add(newvartical3);


            grdsiloclosing.DataSource = siloclosingReport;
            grdsiloclosing.DataBind();
            Session["xportdata"] = siloclosingReport;
            pnlsiloclosing.Visible = true;
        }
        else
        {
            DataRow new2 = siloclosingReport.NewRow();
            new2["siloname"] = "Total";
            siloclosingReport.Rows.Add(new2);
            DataRow new3 = siloclosingReport.NewRow();
            new3["siloname"] = "Cream Production";
            siloclosingReport.Rows.Add(new3);
            grdsiloclosing.DataSource = siloclosingReport;
            grdsiloclosing.DataBind();
            Session["xportdata"] = siloclosingReport;
            pnlsiloclosing.Visible = true;
        }

        creamproduction.Columns.Add("Cream Type");
        creamproduction.Columns.Add("Silo Name");
        creamproduction.Columns.Add("Department Name");
        creamproduction.Columns.Add("Quantity(kgs)");
        creamproduction.Columns.Add("Quantity(ltrs)");
        creamproduction.Columns.Add("FAT");
        creamproduction.Columns.Add("SNF");
        creamproduction.Columns.Add("KG FAT");
        creamproduction.Columns.Add("KG SNF");
        creamproduction.Columns.Add("Date");
        DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
        cmd = new SqlCommand("SELECT SiloId, SiloName From silomaster");
        DataTable dtcrsilo = SalesDB.SelectQuery(cmd).Tables[0];
        cmd = new SqlCommand("SELECT silo_outward_transaction.date, silo_outward_transaction.siloid,  silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs,silo_outward_transaction.fat,silo_outward_transaction.snf,silo_outward_transaction.clr FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId  WHERE (silo_outward_transaction.departmentid = '3') AND (silo_outward_transaction.date BETWEEN @dt1 AND @dt2) AND (silo_outward_transaction.branchid=@crbranchid) ORDER BY silo_outward_transaction.date");
        //cmd = new SqlCommand("SELECT sno, creamtype, ob, obfat, recivedqty, recivedfat, totalcreamqty, avgfat, branchid, userid, doe FROM gheecreamdetails WHERE (doe BETWEEN @dt1 AND @dt2) ORDER BY sno DESC");
        cmd.Parameters.Add("@dt1", GetLowDate(fromdate));
        cmd.Parameters.Add("@dt2", GetHighDate(todate));
        cmd.Parameters.Add("@crbranchid", BranchID);
        DataTable dtcream = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtcrsilo.Rows.Count > 0)
        {
            creamqtykgstotal = 0;
            creamkgstotal = 0;
            creamfattotal = 0;
            creamsnftotal = 0;
            creamclrtotal = 0;
            creamkgfattotal = 0;
            creamkgsnftotal = 0;
            foreach (DataRow drcream in dtcrsilo.Rows)
            {
                string siloid = drcream["siloid"].ToString();
                DataTable dtcreamin = new DataTable();
                DataRow[] drcv = dtcream.Select("siloid='" + siloid + "'");
                if (drcv.Length > 0)
                {
                    dtcreamin = drcv.CopyToDataTable();
                }
                if (dtcreamin.Rows.Count > 0)
                {
                    string siloname = "";
                    string departmentname = "";
                    string date = "";
                    double FAT = 0;
                    double SNF = 0;
                    double KGFAT = 0;
                    double KGSNF = 0;
                    foreach (DataRow dr in dtcreamin.Rows)
                    {
                        siloname = dr["SiloName"].ToString();
                        departmentname = dr["departmentname"].ToString();
                        DateTime dtdatet = Convert.ToDateTime(dr["date"].ToString());
                        date = dtdatet.ToString("dd/MM/yyyy");
                        double qtykgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out qtykgs);
                        creamqtykgstotal += qtykgs;
                        double qtyltrs = 0;
                        double.TryParse(dr["qty_ltrs"].ToString(), out qtyltrs);
                        creamltrstotal += qtyltrs;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        creamfattotal += FAT;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        creamsnftotal += SNF;
                        KGFAT = (FAT * qtykgs) / 100;
                        KGSNF = (SNF * qtykgs) / 100;
                        creamkgfattotal += KGFAT;
                        creamkgsnftotal += KGSNF;

                    }
                    DataRow newrow = creamproduction.NewRow();
                    newrow["Cream Type"] = "Cow";
                    newrow["Silo Name"] = siloname;
                    newrow["Department Name"] = departmentname;
                    newrow["Date"] = date;
                    newrow["Quantity(kgs)"] = creamqtykgstotal;
                    creamqtytotalkgs += creamqtykgstotal;
                    newrow["Quantity(ltrs)"] = creamltrstotal;
                    creamtotalltrs += creamltrstotal;
                    newrow["KG FAT"] = creamkgfattotal;
                    creamkgtotalfat += creamkgfattotal;
                    newrow["KG SNF"] = creamkgsnftotal;
                    creamkgtotalsnf += creamkgsnftotal;
                    double fatt = creamkgfattotal / creamqtykgstotal;
                    newrow["FAT"] = Math.Round(fatt * 100, 2);
                    double snft = creamkgsnftotal / creamqtykgstotal;
                    newrow["SNF"] = Math.Round(snft * 100, 2);
                    creamproduction.Rows.Add(newrow);
                    creamkgfattotal = 0;
                    creamqtykgstotal = 0;
                    creamkgsnftotal = 0;
                    creamkgfattotal = 0;
                    creamltrstotal = 0;
                }
            }

            DataRow newvartical2 = creamproduction.NewRow();
            newvartical2["Cream Type"] = "Total";
            newvartical2["Quantity(kgs)"] = creamqtytotalkgs;
            newvartical2["Quantity(ltrs)"] = creamtotalltrs;
            newvartical2["KG FAT"] = Math.Round(creamkgtotalfat, 2);
            newvartical2["KG SNF"] = Math.Round(creamkgtotalsnf, 2);
            double kgfatt = creamkgtotalfat / creamqtytotalkgs;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            double kgsnft = creamkgtotalsnf / creamqtytotalkgs;
            newvartical2["SNF"] = Math.Round(kgsnft * 100, 2);
            creamproduction.Rows.Add(newvartical2);
            DataRow new3 = creamproduction.NewRow();
            new3["Cream Type"] = "Butter";
            creamproduction.Rows.Add(new3);
            grdcreamproduction.DataSource = creamproduction;
            grdcreamproduction.DataBind();
            Session["xportdata"] = creamproduction;
            pnlcream.Visible = true;
        }
        else
        {
            DataRow new3 = creamproduction.NewRow();
            new3["Cream Type"] = "Butter";
            creamproduction.Rows.Add(new3);
            grdcreamproduction.DataSource = creamproduction;
            grdcreamproduction.DataBind();
            Session["xportdata"] = creamproduction;
            pnlcream.Visible = true;
        }
        butter.Columns.Add("Silo Name");
        butter.Columns.Add("Department Name");
        butter.Columns.Add("qty(ltr)");
        butter.Columns.Add("qty(kgs)");
        butter.Columns.Add("FAT");
        butter.Columns.Add("SNF");
        butter.Columns.Add("CLR");
        butter.Columns.Add("KG FAT");
        butter.Columns.Add("KG SNF");
        cmd = new SqlCommand("SELECT SiloId, SiloName From silomaster");
        DataTable dtbuttersilo = SalesDB.SelectQuery(cmd).Tables[0];
        cmd = new SqlCommand("SELECT silo_outward_transaction.date, silo_outward_transaction.siloid, silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf, silo_outward_transaction.clr FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId  WHERE (silo_outward_transaction.departmentid = '10') AND (silo_outward_transaction.date BETWEEN @cdate1 AND @cdate2) AND (silo_outward_transaction.branchid=@cubranchid)");
        cmd.Parameters.Add("@cdate1", GetLowDate(fromdate));
        cmd.Parameters.Add("@cdate2", GetHighDate(todate));
        cmd.Parameters.Add("@cubranchid", BranchID);
        DataTable dtbutter = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtbuttersilo.Rows.Count > 0)
        {
            butterltrstotal = 0;
            butterkgstotal = 0;
            butterfattotal = 0;
            buttersnftotal = 0;
            butterkgfattotal = 0;
            butterkgsnftotal = 0;
            foreach (DataRow drsilo in dtbuttersilo.Rows)
            {
                string siloid = drsilo["SiloId"].ToString();
                DataTable dtsiloin = new DataTable();
                DataRow[] drv = dtbutter.Select("siloid='" + siloid + "'");
                if (drv.Length > 0)
                {
                    dtsiloin = drv.CopyToDataTable();
                }
                if (dtsiloin.Rows.Count > 0)
                {
                    string Siloname = "";
                    string Departmentname = "";
                    double FAT = 0;
                    double SNF = 0;
                    double ltrfat = 0;
                    double ltrsnf = 0;
                    foreach (DataRow dr in dtsiloin.Rows)
                    {

                        Siloname = dr["SiloName"].ToString();
                        Departmentname = dr["departmentname"].ToString();
                        double ltrs = 0;
                        double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                        butterltrstotal += ltrs;
                        double Kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                        butterkgstotal += Kgs;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        butterfattotal += FAT;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        buttersnftotal += SNF;
                        ltrfat = Kgs * FAT;
                        ltrfat = Math.Round(ltrfat / 100, 2);
                        butterkgfattotal += ltrfat;
                        buttertotalkgfat += ltrfat;
                        ltrsnf = Kgs * SNF;
                        ltrsnf = Math.Round(ltrsnf / 100, 2);
                        butterkgsnftotal += ltrsnf;
                        buttertotalkgsnf += ltrsnf;
                    }
                    DataRow newrow = butter.NewRow();
                    newrow["Silo Name"] = Siloname;
                    newrow["Department Name"] = Departmentname;
                    newrow["qty(ltr)"] = butterltrstotal;
                    buttertotalltrs += butterltrstotal;
                    newrow["qty(kgs)"] = butterkgstotal;
                    buttertotalkgs += butterkgstotal;
                    newrow["KG FAT"] = butterkgfattotal;
                    newrow["KG SNF"] = butterkgsnftotal;
                    double fatt = butterkgfattotal / butterkgstotal;
                    newrow["FAT"] = Math.Round(fatt * 100, 2);
                    double snft = butterkgsnftotal / butterkgstotal;
                    newrow["SNF"] = Math.Round(snft * 100, 2);
                    butter.Rows.Add(newrow);
                    butterkgfattotal = 0;
                    butterkgsnftotal = 0;
                    butterltrstotal = 0;
                    butterkgstotal = 0;
                }
            }

            DataRow newvartical2 = butter.NewRow();
            newvartical2["Silo Name"] = "Total";
            newvartical2["qty(ltr)"] = buttertotalltrs;
            newvartical2["qty(kgs)"] = buttertotalkgs;
            newvartical2["KG FAT"] = buttertotalkgfat;
            newvartical2["KG SNF"] = buttertotalkgsnf;
            double kgfatt = buttertotalkgfat / buttertotalkgs;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            double kgSNFt = buttertotalkgsnf / buttertotalkgs;
            newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);
            butter.Rows.Add(newvartical2);

            DataRow newvartical3 = butter.NewRow();
            newvartical3["Silo Name"] = "Total2";
            butter.Rows.Add(newvartical3);

            grdbutter.DataSource = butter;
            grdbutter.DataBind();
            Session["xportdata"] = butter;
            pnlbutter.Visible = true;
        }
        else
        {
            DataRow newvartical3 = butter.NewRow();
            newvartical3["Silo Name"] = "Total2";
            butter.Rows.Add(newvartical3);
            grdbutter.DataSource = butter;
            grdbutter.DataBind();
            Session["xportdata"] = butter;
            pnlbutter.Visible = true;
        }

        double totalltrs1 = siloopeningltrstotal + reciptbuffltrstotal + returnmilkltrstotal + smpkgstotal;
        double totalltrs2 = creamtotalltrs + tankertotalltrs + curdtotalltrs + despatchtotalltrs + condencertotalltrs + siloclosingltrstotal + buttertotalltrs;
        double glltrs = totalltrs2 - totalltrs1;

        double totalkgs1 = siloopeningkgstotal + reciptbuffkgstotal + returnmilkkgstotal + smpkgstotal;
        double totalkgs2 = despatchtotalkgs + tankertotalkgs + curdtotalkgs + siloclosingkgstotal + creamqtytotalkgs + condencertotalkgs + buttertotalkgs;
        double glkgss = totalkgs2 - totalkgs1;

        double totalkgfat1 = siloopeningkgfattotal + reciptkgfattotal + returnmilkkgfattotal + smpkgfattotal + reciptbuffkgfattotal + smpkgtotalfat;
        double totalkgfat2 = despatchkgfattotal + tankerkgfattotal + curdkgfattotal  + creamkgfattotal + condencerkgfattotal + butterkgfattotal + despatchkgtotalfat + curdtotalkgfat + tankerkgtotalfat + siloclosingkgfattotal + creamkgtotalfat + buttertotalkgfat;
        double glkgfat = totalkgfat2 - totalkgfat1;

        double totalkgsnf1 = siloopeningkgsnftotal + reciptkgsnftotal + returnmilkkgsnftotal + smpkgsnftotal + reciptbuffkgsnftotal + smpkgtotalsnf;
        double totalkgsnf2 = despatchkgsnftotal + tankerkgsnftotal + curdkgsnftotal  + creamkgsnftotal + condencerkgsnftotal + butterkgsnftotal + despatchkgtotalsnf + curdtotalkgsnf + tankerkgtotalsnf + siloclosingkgsnftotal + creamkgtotalsnf + buttertotalkgsnf;
        double glkgsnf = totalkgsnf2 - totalkgsnf1;

        gainorloss.Columns.Add("g");
        gainorloss.Columns.Add("Quantity(kgs)");
        gainorloss.Columns.Add("Quantity(ltrs)");
        gainorloss.Columns.Add("KG FAT");
        gainorloss.Columns.Add("KG SNF");
        DataRow dtnewrow = gainorloss.NewRow();
        dtnewrow["g"] = "Gain / Loss";
        dtnewrow["Quantity(kgs)"] = Math.Round(glkgss, 2);
        dtnewrow["Quantity(ltrs)"] = Math.Round(glltrs, 2);
        dtnewrow["KG FAT"] = Math.Round(glkgfat, 2);
        dtnewrow["KG SNF"] = Math.Round(glkgsnf, 2);
        gainorloss.Rows.Add(dtnewrow);
        grdgl.DataSource = gainorloss;
        grdgl.DataBind();
        Session["xportdata"] = gainorloss;
        pnlgl.Visible = true;
        DataTable total1 = new DataTable();
        total1.Columns.Add("g");
        total1.Columns.Add("Quantity(kgs)");
        total1.Columns.Add("Quantity(ltrs)");
        total1.Columns.Add("KG FAT");
        total1.Columns.Add("KG SNF");
        DataRow dttotal = total1.NewRow();
        dttotal["g"] = "Total";
        dttotal["Quantity(kgs)"] = Math.Round(totalkgs1, 2);
        dttotal["Quantity(ltrs)"] = Math.Round(totalltrs1, 2);
        dttotal["KG FAT"] = Math.Round(totalkgfat1, 2);
        dttotal["KG SNF"] = Math.Round(totalkgsnf1, 2);
        total1.Rows.Add(dttotal);

        DataRow dtnew2 = total1.NewRow();
        dtnew2["g"] = "Dispatch";
        total1.Rows.Add(dtnew2);
        grdtotal1.DataSource = total1;
        grdtotal1.DataBind();
        Session["xportdata"] = total1;
        pnltotal.Visible = true;


        DataTable total2 = new DataTable();
        total2.Columns.Add("g");
        total2.Columns.Add("Quantity(kgs)");
        total2.Columns.Add("Quantity(ltrs)");
        total2.Columns.Add("KG FAT");
        total2.Columns.Add("KG SNF");
        DataRow dttotal2 = total2.NewRow();
        dttotal2["g"] = "Total";
        dttotal2["Quantity(kgs)"] = Math.Round(totalkgs2, 2);
        dttotal2["Quantity(ltrs)"] = Math.Round(totalltrs2, 2);
        dttotal2["KG FAT"] = Math.Round(totalkgfat2, 2);
        dttotal2["KG SNF"] = Math.Round(totalkgsnf2, 2);
        total2.Rows.Add(dttotal2);

        DataRow dtnew3 = total2.NewRow();
        dtnew2["g"] = "Gain / Loss";
        total2.Rows.Add(dtnew3);
        grdtotal2.DataSource = total2;
        grdtotal2.DataBind();
        Session["xportdata"] = total2;
        pnltotal2.Visible = true;


        //condencer
        double outwardqtykgs = 0;
        double outwardqtyltrs = 0;
        double inwardqtykgs = 0;
        double inwardqtyltrs = 0;
        DateTime dt = fromdate;
        string month = dt.ToString("MM");
        string deptid = "9";
        DataTable dtcondencer = new DataTable();
        dtcondencer.Columns.Add("Dispatch Qty(ltrs)");
        dtcondencer.Columns.Add("Recive Qty(ltrs)");
        dtcondencer.Columns.Add("Evaparation Qty(ltrs)");
        //dtcondencer.Columns.Add("returnqtyltrs");
        //dtcondencer.Columns.Add("inwardqtykgs");
        //dtcondencer.Columns.Add("inwardqtyltrs");
        cmd = new SqlCommand("SELECT  SUM(qty_kgs) AS outwordqtykgs, SUM(qty_ltrs) AS outwordqtyltrs FROM   silo_outward_transaction WHERE   (departmentid = @codeptid) AND (branchid = @cobranchid) AND (date BETWEEN @cod1 AND @cod2)");
        cmd.Parameters.Add("@cod1", GetLowDate(fromdate));
        cmd.Parameters.Add("@cod2", GetHighDate(todate));
        cmd.Parameters.Add("@cobranchid", BranchID);
        cmd.Parameters.Add("@codeptid", deptid);
        DataTable dtoutwardcondencerdtails = SalesDB.SelectQuery(cmd).Tables[0];
        cmd = new SqlCommand("SELECT  SUM(qty_kgs) AS qtykgs,  SUM(qty_ltrs) AS qtyltrs FROM  silo_inward_transaction WHERE (deptid = @cideptid) AND (date BETWEEN @cid1 AND @cid2) AND (branchid = @cpdibranchid)");
        cmd.Parameters.Add("@cideptid", deptid);
        cmd.Parameters.Add("@cid1", GetLowDate(fromdate));
        cmd.Parameters.Add("@cid2", GetHighDate(todate));
        cmd.Parameters.Add("@cpdibranchid", BranchID);
        DataTable dtinwardcondencerdtails = SalesDB.SelectQuery(cmd).Tables[0];
        DataRow connewrow = dtcondencer.NewRow();
        if (dtoutwardcondencerdtails.Rows.Count > 0)
        {
            foreach (DataRow dr in dtoutwardcondencerdtails.Rows)
            {
                string qtykgs = dr["outwordqtykgs"].ToString();
                string qtyltrs = dr["outwordqtyltrs"].ToString();
                // connewrow["Outwardqtykgs"] = qtykgs;
                connewrow["Dispatch Qty(ltrs)"] = qtyltrs;
                if (qtyltrs != "")
                {
                    outwardqtyltrs = Convert.ToDouble(qtyltrs);
                }
            }
        }
        if (dtinwardcondencerdtails.Rows.Count > 0)
        {
            foreach (DataRow drr in dtinwardcondencerdtails.Rows)
            {
                string inqtykgs = drr["qtykgs"].ToString();
                string inqtyltrs = drr["qtyltrs"].ToString();
                //connewrow["inwardqtykgs"] = inqtykgs;
                connewrow["Recive Qty(ltrs)"] = inqtyltrs;
                if (inqtyltrs != "")
                {
                    inwardqtyltrs = Convert.ToDouble(inqtyltrs);
                }
            }
        }
        connewrow["Evaparation Qty(ltrs)"] = outwardqtyltrs - inwardqtyltrs;
        dtcondencer.Rows.Add(connewrow);
        grdcondencerm.DataSource = dtcondencer;
        grdcondencerm.DataBind();
        Session["xportdata"] = dtcondencer;
        pnlcondencerm.Visible = true;
    }


    protected void grdreturn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total1")
            {
                e.Row.Cells[0].Width = new Unit("320px");
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "CURD")
            {
                e.Row.Cells[0].Width = new Unit("320px");
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdsiloopening_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Receipts")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Buffalo")
            {
                e.Row.BackColor = System.Drawing.Color.LightBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdrecipts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "SMP Details")
            {
                e.Row.Cells[0].Width = new Unit("320px");
                e.Row.BackColor = System.Drawing.Color.LightBlue;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.Cells[0].Width = new Unit("320px");
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdbaffalow_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.Cells[0].Width = new Unit("320px");
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Grand Total")
            {
                e.Row.Cells[0].Width = new Unit("320px");
                e.Row.BackColor = System.Drawing.Color.LightGreen;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "SMP Details")
            {
                e.Row.Cells[0].Width = new Unit("320px");
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdsmp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.Cells[0].Width = new Unit("320px");
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Return Milk Details")
            {
                e.Row.Cells[0].Width = new Unit("320px");
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdcurd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            //
            if (e.Row.Cells[0].Text == "Dispatch Tankers")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdcondencer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            //
            if (e.Row.Cells[0].Text == "Dispatch Tankers")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdtankers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            //
            if (e.Row.Cells[0].Text == "Closing Balance")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdsiloclosing_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            //
            if (e.Row.Cells[0].Text == "Cream Production")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdcreamproduction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Butter")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdtotal2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Gain / Loss")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdtotal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Dispatch")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grddispatch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Curd Block")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdpl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Gain / Loss")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void grdbutter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[0].Text == "Total2")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        getdata();
    }

    protected void grdrecipts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataTable Report = new DataTable();
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdrecipts.Rows[rowIndex];
            string vendorid = row.Cells[8].Text;
            if (vendorid == "" || vendorid == null || vendorid == "&nbsp;")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen();", false);
            }
            else
            {
                Report.Columns.Add("Date");
                Report.Columns.Add("DC No");
                Report.Columns.Add("qty(kgs)");
                Report.Columns.Add("qty(ltr)");
                Report.Columns.Add("FAT");
                Report.Columns.Add("SNF");
                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
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
                //sql cmd in here
                cmd = new SqlCommand("SELECT silo_inward_transaction.sno, silo_inward_transaction.ccid, v.vendorname, silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs, silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName FROM  silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId LEFT JOIN vendors v on v.sno = silo_inward_transaction.ccid LEFT JOIN processingdepartments ON processingdepartments.departmentid = silo_inward_transaction.deptid WHERE (silo_inward_transaction.date BETWEEN @date1 AND @date2) AND (silo_inward_transaction.branchid=@branchid) AND silo_inward_transaction.ccid=@vendorid");
                cmd.Parameters.Add("@date1", GetLowDate(fromdate));
                cmd.Parameters.Add("@date2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
                cmd.Parameters.Add("@vendorid", vendorid);
                DataTable dtgrdrecipts = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtgrdrecipts.Rows.Count > 0)
                {
                    double reciptltrstotal = 0;
                    double reciptkgstotal = 0;
                    double reciptfattotal = 0;
                    double reciptsnftotal = 0;
                    double reciptkgfattotal = 0;
                    double reciptkgsnftotal = 0;
                    double reciptbuffltrstotal = 0;
                    double reciptbuffkgstotal = 0;
                    string VNAME = "";
                    string milktype = "";
                    double FAT = 0;
                    double SNF = 0;
                    double kgfat = 0;
                    double kgsnf = 0;
                    foreach (DataRow dr in dtgrdrecipts.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["date"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["DATE"] = date;
                        newrow["DC No"] = dr["dcno"].ToString();
                        VNAME = dr["vendorname"].ToString();
                        double ltrs = 0;
                        double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                        reciptltrstotal += ltrs;
                        double Kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                        reciptkgstotal += Kgs;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        reciptfattotal += FAT;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        reciptsnftotal += SNF;
                        kgfat = Kgs * FAT;
                        kgfat = Math.Round(kgfat / 100, 2);
                        reciptkgfattotal += kgfat;
                        kgsnf = Kgs * SNF;
                        kgsnf = Math.Round(kgsnf / 100, 2);
                        reciptkgsnftotal += kgsnf;
                        newrow["qty(ltr)"] = ltrs;
                        newrow["qty(kgs)"] = Kgs;
                        newrow["KG FAT"] = kgfat;
                        newrow["KG SNF"] = kgsnf;
                        newrow["FAT"] = FAT;
                        newrow["SNF"] = SNF;
                        Report.Rows.Add(newrow);
                    }
                    DataRow newrow2 = Report.NewRow();
                    //newrow2["Name"] = VNAME;
                    newrow2["qty(ltr)"] = reciptltrstotal;
                    reciptbuffltrstotal += reciptltrstotal;
                    newrow2["qty(kgs)"] = reciptkgstotal;
                    reciptbuffkgstotal += reciptkgstotal;
                    newrow2["KG FAT"] = reciptkgfattotal;
                    newrow2["KG SNF"] = reciptkgsnftotal;
                    double fatt = reciptkgfattotal / reciptkgstotal;
                    newrow2["FAT"] = Math.Round(fatt * 100, 2);
                    double snft = reciptkgsnftotal / reciptkgstotal;
                    newrow2["SNF"] = Math.Round(snft * 100, 2);
                    Report.Rows.Add(newrow2);
                    GrdProducts.DataSource = Report;
                    GrdProducts.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen();", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
}