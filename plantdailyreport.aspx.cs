using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

public partial class plantdailyreport : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
    string leveltype = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            BranchID = Session["Branch_ID"].ToString();
            leveltype = Session["leveltype"].ToString();
            vdm = new SalesDBManager();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    if (leveltype == "MAdmin")
                    {
                        pln_branch.Visible = true;
                        fillbranches();
                    }
                    else
                    {
                        pln_branch.Visible = false;
                    }
                    DateTime dt = DateTime.Now.AddDays(-1);
                    dtp_FromDate.Text = dt.ToString("dd-MM-yyyy HH:mm");
                }
            }
        }
    }
    public void fillbranches()
    {
        SalesDBManager SalesDB = new SalesDBManager();
        cmd = new SqlCommand("SELECT  sno, branchname, address, branchtype, tinno, venorid, branchcode, cstno, mitno, whcode FROM  branch_info WHERE  (branchtype = 'Plant') ");
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlbranches.DataSource = dttrips;
        ddlbranches.DataTextField = "branchname";
        ddlbranches.DataValueField = "sno";
        ddlbranches.DataBind();
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
    DataTable BUTTER = new DataTable();
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
        if (leveltype == "MAdmin")
        {
            BranchID = ddlbranches.SelectedValue;
            lblTitle.Text = ddlbranches.SelectedItem.Text;
        }
        else
        {
            BranchID = Session["Branch_ID"].ToString();
            lblTitle.Text = Session["TitleName"].ToString();
            lblAddress.Text = Session["Address"].ToString();
        }
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
            if (datestrig[0].Split('-').Length > 0)
            {
                string[] dates = datestrig[0].Split('-');
                string[] times = datestrig[1].Split(':');
                todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            }
        }
        lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
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

        siloopenReport.Columns.Add("SiloName");
        siloopenReport.Columns.Add("BatchName");
        siloopenReport.Columns.Add("qty(ltr)");
        siloopenReport.Columns.Add("qty(kgs)");
        siloopenReport.Columns.Add("FAT");
        siloopenReport.Columns.Add("SNF");
        siloopenReport.Columns.Add("CLR");
        siloopenReport.Columns.Add("KG FAT");
        siloopenReport.Columns.Add("KG SNF");
        cmd = new SqlCommand("SELECT sm.SiloName, scd.qty_kgs, scd.fat, scd.snf, scd.clr, scd.closingdate, scd.batchid, bm.batch FROM   silowiseclosingdetails AS scd INNER JOIN silomaster AS sm ON sm.SiloId = scd.siloid LEFT OUTER JOIN batchmaster AS bm ON scd.batchid = bm.batchid WHERE  (scd.closingdate BETWEEN @d1 AND @d2) AND (scd.branchid = @branchid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
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
                string batch = "";
                batch = dr["batch"].ToString();
                string siloname = dr["SiloName"].ToString();
                newrow["SiloName"] = siloname;
                newrow["BatchName"] = batch;
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
            newvartical2["SiloName"] = "Total";
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

            DataRow newvartical4 = siloopenReport.NewRow();
            newvartical4["siloname"] = "Buffalo";
            siloopenReport.Rows.Add(newvartical4);

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

        buff.Columns.Add("Name");
        buff.Columns.Add("qty(ltr)");
        buff.Columns.Add("qty(kgs)");
        buff.Columns.Add("FAT");
        buff.Columns.Add("SNF");
        buff.Columns.Add("KG FAT");
        buff.Columns.Add("KG SNF");
        DateTime dtfromdate;
        DateTime dtto;
        DateTime dttodate;
        if (BranchID == "26")
        {
            dtfromdate = GetLowDate(fromdate).AddHours(8);
            dtto = GetLowDate(todate);
            dttodate = dtto.AddHours(32);
        }
        else
        {
            dtfromdate = GetLowDate(fromdate).AddHours(6);
            dtto = GetLowDate(todate);
            dttodate = dtto.AddHours(30);
        }
        // cmd = new SqlCommand("Select V.vendorname, ML.rate_on, ML.calc_on, mt.dcno, mt.qty_ltr, mt.qty_kgs, mt.snf, mt.fat, mt.clr, mt.percentageon, mt.vehicleno, mt.partydcno from milktransactions MT INNER JOIN vendors V on V.sno=mt.sectionid INNER JOIN milktransaction_logs ML ON mt.sno = ML.milktransaction_sno WHERE (mt.transtype='in') AND (mt.entrydate BETWEEN @date1 AND @date2)");
        // cmd = new SqlCommand("SELECT silo_inward_transaction.sno, v.vendorname, VS.milktype, silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs, silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName FROM silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId LEFT OUTER JOIN despatch_entry AS de ON de.sno = silo_inward_transaction.dcno LEFT OUTER JOIN vendors AS v ON de.cc_id = v.sno LEFT OUTER JOIN vendor_subtable VS ON VS.vendor_refno = v.sno WHERE  (silo_inward_transaction.date BETWEEN @date1 AND @date2)");
        // cmd = new SqlCommand("SELECT  silo_inward_transaction.sno, v.vendorname, silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs, silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName, vendor_subtable.milktype FROM  vendor_subtable LEFT OUTER JOIN vendors AS v ON vendor_subtable.vendor_refno = v.sno RIGHT OUTER JOIN silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId LEFT OUTER JOIN despatch_entry AS de ON de.sno = silo_inward_transaction.dcno ON v.sno = de.cc_id WHERE (silo_inward_transaction.date BETWEEN @date1 AND @date2)");
        cmd = new SqlCommand("SELECT  silo_inward_transaction.sno, silo_inward_transaction.ccid, v.vendorname, silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs, silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName, silo_inward_transaction.deptid FROM  silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId LEFT JOIN vendors v on v.sno = silo_inward_transaction.ccid LEFT JOIN processingdepartments ON processingdepartments.departmentid = silo_inward_transaction.deptid WHERE (silo_inward_transaction.date BETWEEN @date1 AND @date2) AND (silo_inward_transaction.branchid=@branchid) AND (silo_inward_transaction.ccid !='') OR (silo_inward_transaction.date BETWEEN @date1 AND @date2) AND (silo_inward_transaction.branchid = @branchid) AND (silo_inward_transaction.deptid = 6)");
        cmd.Parameters.Add("@date1", dtfromdate);
        cmd.Parameters.Add("@date2", dttodate);
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dtrecipts = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtrecipts.Rows.Count > 0)
        {
            double reciptcowltrstotal = 0;
            double reciptcowkgstotal = 0;
            double reciptcowfattotal = 0;
            double reciptcowsnftotal = 0;
            double reciptcowkgfattotal = 0;
            double reciptcowkgsnftotal = 0;
            double reciptbuffltrstotal = 0;
            double reciptbuffkgstotal = 0;
            double reciptbufffattotal = 0;
            double reciptbuffsnftotal = 0;
            double reciptbuffkgfattotal = 0;
            double reciptbuffkgsnftotal = 0;
            reciptltrstotal = 0;
            reciptkgstotal = 0;
            reciptfattotal = 0;
            reciptsnftotal = 0;
            reciptkgfattotal = 0;
            reciptkgsnftotal = 0;
            foreach (DataRow dr in dtrecipts.Rows)
            {
                string milktype = "";
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                if (FAT > 5.0)
                {
                    DataRow newrow = receipts.NewRow();
                    string VNAME = dr["vendorname"].ToString();
                    if (VNAME == "")
                    {
                        string id = dr["deptid"].ToString();
                        if (id == "6")
                        {
                            newrow["Name"] = "RCM Water";
                        }
                        //else
                        //{
                        //    newrow["Name"] = "Condensed Milk";
                        //}
                    }
                    else
                    {
                        newrow["Name"] = dr["vendorname"].ToString();
                    }
                    newrow["qty(ltr)"] = dr["qty_ltrs"].ToString();
                    newrow["qty(kgs)"] = dr["qty_kgs"].ToString();
                    double ltrs = 0;
                    double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                    reciptbuffltrstotal += ltrs;
                    reciptltrstotal += ltrs;
                    double Kgs = 0;
                    double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                    reciptkgstotal += Kgs;
                    reciptbuffkgstotal += Kgs;
                    FAT = 0;
                    double.TryParse(dr["fat"].ToString(), out FAT);
                    reciptfattotal += FAT;
                    reciptbufffattotal += FAT;
                    double SNF = 0;
                    double.TryParse(dr["snf"].ToString(), out SNF);
                    reciptsnftotal += SNF;
                    reciptbuffsnftotal += SNF;
                    double kgfat = 0;
                    kgfat = Kgs * FAT;
                    kgfat = Math.Round(kgfat / 100, 2);
                    reciptkgfattotal += kgfat;
                    reciptbuffkgfattotal += kgfat;
                    double kgsnf = 0;
                    kgsnf = Kgs * SNF;
                    kgsnf = Math.Round(kgsnf / 100, 2);
                    reciptkgsnftotal += kgsnf;
                    reciptbuffkgsnftotal += kgsnf;
                    newrow["FAT"] = dr["fat"].ToString();
                    newrow["SNF"] = dr["snf"].ToString();
                    newrow["KG FAT"] = kgfat;
                    newrow["KG SNF"] = kgsnf;
                    receipts.Rows.Add(newrow);
                }
                else
                {
                    DataRow bufnewrow = buff.NewRow();
                    string VNAME = dr["vendorname"].ToString();
                    if (VNAME == "")
                    {
                        string id = dr["deptid"].ToString();
                        if (id == "6")
                        {
                            bufnewrow["Name"] = "RCM Water";
                        }
                        //else
                        //{
                        //    bufnewrow["Name"] = "Condensed Milk";
                        //}
                        //bufnewrow["Name"] = "Condensed Milk";
                    }
                    else
                    {
                        bufnewrow["Name"] = dr["vendorname"].ToString();
                    }
                    bufnewrow["qty(ltr)"] = dr["qty_ltrs"].ToString();
                    bufnewrow["qty(kgs)"] = dr["qty_kgs"].ToString();
                    double ltrs = 0;
                    double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                    reciptltrstotal += ltrs;
                    reciptcowltrstotal += ltrs;
                    double Kgs = 0;
                    double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                    reciptkgstotal += Kgs;
                    reciptcowkgstotal += Kgs;
                    double.TryParse(dr["fat"].ToString(), out FAT);
                    reciptfattotal += FAT;
                    reciptcowfattotal += FAT;
                    double SNF = 0;
                    double.TryParse(dr["snf"].ToString(), out SNF);
                    reciptsnftotal += SNF;
                    reciptcowsnftotal += SNF;

                    double kgfat = 0;
                    kgfat = Kgs * FAT;
                    kgfat = Math.Round(kgfat / 100, 2);
                    reciptkgfattotal += kgfat;
                    reciptcowkgfattotal += kgfat;
                    double kgsnf = 0;
                    kgsnf = Kgs * SNF;
                    kgsnf = Math.Round(kgsnf / 100, 2);
                    reciptkgsnftotal += kgsnf;
                    reciptcowkgsnftotal += kgsnf;
                    bufnewrow["FAT"] = dr["fat"].ToString();
                    bufnewrow["SNF"] = dr["snf"].ToString();
                    bufnewrow["KG FAT"] = kgfat;
                    bufnewrow["KG SNF"] = kgsnf;
                    buff.Rows.Add(bufnewrow);
                }
            }
            DataRow recnewvartical2 = receipts.NewRow();
            recnewvartical2["Name"] = "Total";
            recnewvartical2["qty(ltr)"] = reciptbuffltrstotal;
            recnewvartical2["qty(kgs)"] = reciptbuffkgstotal;
            recnewvartical2["KG FAT"] = reciptbuffkgfattotal;
            double kgbuffatt = reciptbuffkgfattotal / reciptbuffkgstotal;
            recnewvartical2["FAT"] = Math.Round(kgbuffatt * 100, 2);
            recnewvartical2["KG SNF"] = reciptbuffkgsnftotal;
            double kgbufSNFt = reciptbuffkgsnftotal / reciptbuffkgstotal;
            recnewvartical2["SNF"] = Math.Round(kgbufSNFt * 100, 2);
            receipts.Rows.Add(recnewvartical2);
            DataRow recnewvartical3 = receipts.NewRow();
            recnewvartical3["Name"] = "Cow";
            receipts.Rows.Add(recnewvartical3);
            DataRow newvartical2 = buff.NewRow();
            newvartical2["Name"] = "Total";
            newvartical2["qty(ltr)"] = reciptcowltrstotal;
            newvartical2["qty(kgs)"] = reciptcowkgstotal;
            newvartical2["KG FAT"] = reciptcowkgfattotal;
            double kgfatt = reciptcowkgfattotal / reciptcowkgstotal;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            newvartical2["KG SNF"] = reciptcowkgsnftotal;
            double kgSNFt = reciptcowkgsnftotal / reciptcowkgstotal;
            newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);
            buff.Rows.Add(newvartical2);

            DataRow newvartical3 = buff.NewRow();
            newvartical3["Name"] = "Grand Total";
            newvartical3["qty(ltr)"] = reciptltrstotal;
            newvartical3["qty(kgs)"] = reciptkgstotal;
            newvartical3["KG FAT"] = reciptkgfattotal;
            double kgcowfatt = reciptkgfattotal / reciptkgstotal;
            newvartical3["FAT"] = Math.Round(kgcowfatt * 100, 2);
            newvartical3["KG SNF"] = reciptkgsnftotal;
            double kgcowSNFt = reciptkgsnftotal / reciptkgstotal;
            newvartical3["SNF"] = Math.Round(kgcowSNFt * 100, 2);
            buff.Rows.Add(newvartical3);

            DataRow newvartical4 = buff.NewRow();
            newvartical4["Name"] = "SMP Details";
            buff.Rows.Add(newvartical4);
            grdrecipts.DataSource = receipts;
            grdrecipts.DataBind();
            grdbaffalow.DataSource = buff;
            grdbaffalow.DataBind();
            Session["xportdata"] = receipts;
            pnlrecipts.Visible = true;

        }
        else
        {
            DataRow newrow = receipts.NewRow();
            newrow["Name"] = "total";
            DataRow newvartical3 = receipts.NewRow();
            newvartical3["Name"] = "SMP Details";
            receipts.Rows.Add(newvartical3);
            grdrecipts.DataSource = receipts;
            grdrecipts.DataBind();
            grdbaffalow.DataSource = buff;
            grdbaffalow.DataBind();
            Session["xportdata"] = receipts;
            pnlrecipts.Visible = true;
        }
        smp.Columns.Add("Name");
        smp.Columns.Add("qty(kgs)");
        smp.Columns.Add("FAT");
        smp.Columns.Add("SNF");
        smp.Columns.Add("KG FAT");
        smp.Columns.Add("KG SNF");
        //cmd = new SqlCommand("Select V.vendorname, ML.rate_on, ML.calc_on, mt.dcno, mt.qty_ltr, mt.qty_kgs, mt.snf, mt.fat, mt.clr, mt.percentageon, mt.vehicleno, mt.partydcno from milktransactions MT INNER JOIN vendors V on V.sno=mt.sectionid INNER JOIN milktransaction_logs ML ON mt.sno = ML.milktransaction_sno WHERE (mt.transtype='in') AND (mt.entrydate BETWEEN @date1 AND @date2)");
        if (BranchID == "1" || BranchID == "22")
        {
            cmd = new SqlCommand("SELECT  SUM(ISNULL(CAST(smp AS float), 0)) AS smp FROM  batchentrydetails WHERE  (doe BETWEEN @smd1 AND @smd2) AND (branchid=@sbranchid)");
        }
        else
        {
            cmd = new SqlCommand("SELECT qty_kgs, fat, snf, date, branchid, enterby FROM  smp_details WHERE  (date BETWEEN @smd1 AND @smd2) AND (branchid=@sbranchid)");
        }
        cmd.Parameters.Add("@smd1", dtfromdate);//
        cmd.Parameters.Add("@smd2", dttodate);
        cmd.Parameters.Add("@sbranchid", BranchID);
        DataTable dtsmp = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtsmp.Rows.Count > 0)
        {
            smpkgstotal = 0;
            smpfattotal = 0;
            smpsnftotal = 0;
            smpkgfattotal = 0;
            smpkgsnftotal = 0;
            foreach (DataRow dr in dtsmp.Rows)
            {
                DataRow newrow = smp.NewRow();
                newrow["Name"] = "SMP";
                if (BranchID == "26")
                {
                    newrow["qty(kgs)"] = dr["qty_kgs"].ToString();
                    double Kgs = 0;
                    double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                    smpkgstotal += Kgs;
                    double FAT = 0;
                    double.TryParse(dr["fat"].ToString(), out FAT);
                    smpfattotal += FAT;
                    double SNF = 0;
                    double.TryParse(dr["snf"].ToString(), out SNF);
                    smpsnftotal += SNF;
                    double kgfat = 0;
                    kgfat = Kgs * FAT;
                    kgfat = Math.Round(kgfat / 100, 2);
                    smpkgfattotal += kgfat;
                    double kgsnf = 0;
                    kgsnf = Kgs * SNF;
                    kgsnf = Math.Round(kgsnf / 100, 2);
                    smpkgsnftotal += kgsnf;
                    newrow["FAT"] = dr["fat"].ToString();
                    newrow["SNF"] = dr["snf"].ToString();
                    newrow["KG FAT"] = kgfat;
                    newrow["KG SNF"] = kgsnf;
                    smp.Rows.Add(newrow);
                }
                else
                {
                    newrow["qty(kgs)"] = dr["smp"].ToString();
                    double Kgs = 0;
                    double.TryParse(dr["smp"].ToString(), out Kgs);
                    smpkgstotal += Kgs;
                    double FAT = 0;
                    smpfattotal += FAT;
                    double SNF = 92;
                    smpsnftotal += SNF;
                    double kgfat = 0;
                    kgfat = Kgs * FAT;
                    kgfat = Math.Round(kgfat / 100, 2);
                    smpkgfattotal += kgfat;
                    double kgsnf = 0;
                    kgsnf = Kgs * SNF;
                    kgsnf = Math.Round(kgsnf / 100, 2);
                    smpkgsnftotal += kgsnf;
                    newrow["FAT"] = FAT;
                    newrow["SNF"] = SNF;
                    newrow["KG FAT"] = kgfat;
                    newrow["KG SNF"] = kgsnf;
                    smp.Rows.Add(newrow);
                }
            }
            DataRow newvartical2 = smp.NewRow();
            newvartical2["Name"] = "Total";
            newvartical2["qty(kgs)"] = smpkgstotal;
            newvartical2["KG FAT"] = smpkgfattotal;

            double kgfatt = smpkgfattotal / smpkgstotal;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);

            newvartical2["KG SNF"] = smpkgsnftotal;

            double kgSNFt = smpkgsnftotal / smpkgstotal;
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
        else
        {
            DataRow newrow = smp.NewRow();
            newrow["Name"] = "total";
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
        //cmd = new SqlCommand("SELECT returnmilk_details.doe, processingdepartments.departmentname, silomaster.SiloName, returnmilk_details.quantity,returnmilk_details.fat,returnmilk_details.snf,returnmilk_details.clr,returnmilk_details.qty_ltr FROM returnmilk_details INNER JOIN processingdepartments ON returnmilk_details.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON returnmilk_details.siloid = silomaster.SiloId WHERE (returnmilk_details.doe BETWEEN @d1 AND @d2) AND (returnmilk_details.departmentid=4) AND (returnmilk_details.branchid=@rbranchid)");
        //Change by sai instruction by shashagiri sir 24-04-2018 
        cmd = new SqlCommand("SELECT returnmilk_details.doe, processingdepartments.departmentname, silomaster.SiloName, returnmilk_details.quantity,returnmilk_details.fat,returnmilk_details.snf,returnmilk_details.clr,returnmilk_details.qty_ltr FROM returnmilk_details INNER JOIN processingdepartments ON returnmilk_details.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON returnmilk_details.siloid = silomaster.SiloId WHERE (returnmilk_details.doe BETWEEN @d1 AND @d2) AND (returnmilk_details.departmentid=4) AND (returnmilk_details.branchid=@rbranchid) OR (returnmilk_details.doe BETWEEN @d1 AND @d2) AND (returnmilk_details.departmentid = 1) AND (returnmilk_details.branchid = @rbranchid)");
        cmd.Parameters.Add("@d1", dtfromdate);//
        cmd.Parameters.Add("@d2", dttodate);
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
            grdreturnmilk.DataSource = "";
            grdreturnmilk.DataBind();
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
        despatch.Columns.Add("ProductName");
        despatch.Columns.Add("Qty(ltrs)");
        despatch.Columns.Add("Qty(kgs)");
        despatch.Columns.Add("FAT");
        despatch.Columns.Add("SNF");
        despatch.Columns.Add("KG FAT");
        despatch.Columns.Add("KG SNF");
        VehicleDBMgr vdmnr = new VehicleDBMgr();
        MySqlCommand mycmd = new MySqlCommand();
        //Check data saved or not

        //cmd = new SqlCommand("SELECT  sno, productid, fat, snf, qty_kgs, total, doe, section,status FROM plant_production_details WHERE   (section = 'sales') AND (doe BETWEEN @sd1 AND @sd2) AND (branchid = @sbranchid)");
        //cmd.Parameters.Add("@sbranchid", BranchID);
        //cmd.Parameters.Add("@sd1", GetLowDate(fromdate));
        //cmd.Parameters.Add("@sd2", GetHighDate(fromdate));
        //DataTable routes = vdm.SelectQuery(cmd).Tables[0];
        //if (routes.Rows.Count > 0)
        //{
        //    cmd = new SqlCommand("SELECT  sno, productid, fat, snf, qty_kgs, total, doe, section FROM     plant_production_details WHERE   (section = 'sales') AND (doe BETWEEN @sd1 AND @sd2) AND (branchid = @sbranchid)");
        //    cmd.Parameters.Add("@sbranchid", BranchID);
        //    cmd.Parameters.Add("@sd1", GetLowDate(fromdate));
        //    cmd.Parameters.Add("@sd2", GetHighDate(todate));
        //    DataTable dtsales = vdm.SelectQuery(cmd).Tables[0];
        //    if (dtsales.Rows.Count > 0)
        //    {
        //        totaldesp = 0;
        //        despatchkgfattotal = 0;
        //        despatchkgsnftotal = 0;
        //        despatchfattotal = 0;
        //        despatchsnftotal = 0;
        //        foreach (DataRow dr in dtsales.Rows)
        //        {
        //            double FAT = 0;
        //            double SNF = 0;
        //            double clr = 0;
        //            DataRow newrow = despatch.NewRow();
        //            string pid = dr["productid"].ToString();
        //            mycmd = new MySqlCommand("SELECT   sno, category_sno, SubCatName, Flag, userdata_sno, fat FROM   products_subcategory WHERE  (sno = @pid)");
        //            mycmd.Parameters.Add("@pid", pid);
        //            DataTable pdt = vdmnr.SelectQuery(mycmd).Tables[0];
        //            newrow["ProductName"] = pdt.Rows[0]["SubCatName"].ToString();
        //            double qty = 0;
        //            double.TryParse(dr["total"].ToString(), out qty);
        //            newrow["Qty(ltrs)"] = dr["total"].ToString();
        //            totaldesp += qty;
        //            double.TryParse(dr["fat"].ToString(), out FAT);
        //            double.TryParse(dr["snf"].ToString(), out SNF);
        //            double qtyltrkgs = 0;
        //            double.TryParse(dr["qty_kgs"].ToString(), out qtyltrkgs);
        //            newrow["Qty(kgs)"] = qtyltrkgs;
        //            despatchkgstotal += qtyltrkgs;
        //            double KGFAT = qtyltrkgs * FAT;
        //            double KGSNF = qtyltrkgs * SNF;
        //            KGFAT = Math.Round(KGFAT / 100, 2);
        //            KGSNF = Math.Round(KGSNF / 100, 2);
        //            despatchkgfattotal += KGFAT;
        //            despatchkgsnftotal += KGSNF;
        //            despatchfattotal += FAT;
        //            despatchsnftotal += SNF;
        //            newrow["FAT"] = FAT;
        //            newrow["SNF"] = SNF;
        //            newrow["KG FAT"] = KGFAT;
        //            newrow["KG SNF"] = KGSNF;
        //            despatch.Rows.Add(newrow);
        //        }
        //        DataRow newvartical2 = despatch.NewRow();
        //        newvartical2["ProductName"] = "Total";
        //        totaldesp = Math.Round(totaldesp, 2);
        //        newvartical2["Qty(ltrs)"] = Math.Round(totaldesp, 2);
        //        newvartical2["Qty(kgs)"] = Math.Round(despatchkgstotal, 2);
        //        double kgfatt = despatchkgfattotal / despatchkgstotal;
        //        double kgfatts = kgfatt * 100;
        //        newvartical2["FAT"] = Math.Round(kgfatts, 2);

        //        double kgSNFt = despatchkgsnftotal / despatchkgstotal;
        //        newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);

        //        newvartical2["KG FAT"] = Math.Round(despatchkgfattotal, 2);
        //        newvartical2["KG SNF"] = Math.Round(despatchkgsnftotal, 2);

        //        despatch.Rows.Add(newvartical2);
        //        DataRow newvartical3 = despatch.NewRow();
        //        newvartical3["ProductName"] = "Curd Block";
        //        despatch.Rows.Add(newvartical3);
        //        grddespatch.DataSource = despatch;
        //        grddespatch.DataBind();
        //        Session["xportdata"] = despatch;
        //        pnldespatch.Visible = true;
        //    }
        //}
        //else
        //{
            cmd = new SqlCommand("SELECT   sno, branchname, branchcode,whcode FROM branch_info WHERE  (sno = @branchid)");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtbranch = SalesDB.SelectQuery(cmd).Tables[0];
            string branchcode = "";
            string whcode = "";
            foreach (DataRow dr in dtbranch.Rows)
            {
                branchcode = dr["branchcode"].ToString();
                whcode = dr["whcode"].ToString();
            }
            mycmd = new MySqlCommand("select sno,whcode from branchdata where whcode=@whcode");
            mycmd.Parameters.Add("@whcode", whcode);
            DataTable dtwhcode = vdmnr.SelectQuery(mycmd).Tables[0];
            string salesbranchid = dtwhcode.Rows[0]["sno"].ToString();
            mycmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, products_subcategory.SubCatName, products_category.Categoryname, products_category.sno AS categorysno, products_subcategory.sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch) and (products_category.sno ='9') GROUP BY categorysno, products_subcategory.sno ORDER BY categorysno");
            mycmd.Parameters.Add("@branch", salesbranchid);
            mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
            mycmd.Parameters.Add("@d2", GetHighDate(todate));
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
                    string productname = dr["SubCatName"].ToString();
                    if (productname != "LOOSE KHM")
                    {
                        DataRow newrow = despatch.NewRow();
                        newrow["ProductName"] = dr["SubCatName"].ToString();
                        double qty = 0;
                        double.TryParse(dr["Qty"].ToString(), out qty);
                        newrow["Qty(ltrs)"] = dr["Qty"].ToString();
                        totaldesp += qty;
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
                            FAT = 4;
                            SNF = 8.47;
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
                        despatchclrtotal += clr;
                        double modclr = (clr / 1000) + 1;
                        double qtyltrkgs = qty * modclr;
                        newrow["Qty(kgs)"] = qtyltrkgs;
                        despatchkgstotal += qtyltrkgs;

                        string productid = dr["sno"].ToString();
                        cmd = new SqlCommand("SELECT  sno, productid, fat, snf, qty_kgs, total, doe, section FROM     plant_production_details WHERE   (section = 'sales') AND (doe BETWEEN @sd1 AND @sd2) AND (branchid = @sbranchid) AND (productid = @productid)");
                        cmd.Parameters.Add("@sbranchid", BranchID);
                        cmd.Parameters.Add("@sd1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@sd2", GetHighDate(todate));
                        cmd.Parameters.Add("@productid", productid);
                        DataTable dtsalesprod = vdm.SelectQuery(cmd).Tables[0];
                        //if data is saved display old transaction else show fixed fat and snf details
                        if (dtsalesprod.Rows.Count > 0)
                        {
                            FAT = Convert.ToDouble(dtsalesprod.Rows[0]["fat"].ToString());
                            SNF = Convert.ToDouble(dtsalesprod.Rows[0]["snf"].ToString());
                            double KGFAT = qtyltrkgs * FAT;
                            double KGSNF = qtyltrkgs * SNF;
                            KGFAT = Math.Round(KGFAT / 100, 2);
                            KGSNF = Math.Round(KGSNF / 100, 2);
                            despatchkgfattotal += KGFAT;
                            despatchkgsnftotal += KGSNF;
                            despatchfattotal += FAT;
                            despatchsnftotal += SNF;
                            newrow["FAT"] = FAT;
                            newrow["SNF"] = SNF;
                            newrow["KG FAT"] = KGFAT;
                            newrow["KG SNF"] = KGSNF;
                        }
                        else
                        {
                            double KGFAT = qtyltrkgs * FAT;
                            double KGSNF = qtyltrkgs * SNF;
                            KGFAT = Math.Round(KGFAT / 100, 2);
                            KGSNF = Math.Round(KGSNF / 100, 2);
                            despatchkgfattotal += KGFAT;
                            despatchkgsnftotal += KGSNF;
                            despatchfattotal += FAT;
                            despatchsnftotal += SNF;
                            newrow["FAT"] = FAT;
                            newrow["SNF"] = SNF;
                            newrow["KG FAT"] = KGFAT;
                            newrow["KG SNF"] = KGSNF;
                        }
                        despatch.Rows.Add(newrow);
                        
                        //Auto save the transactions

                        //DateTime datetimenow = SalesDBManager.GetTime(vdm.conn);
                        //cmd = new SqlCommand("insert into   plant_production_details (productid, fat, snf, qty_kgs, total, doe, section, createdby, createdon, branchid) values (@productid, @fat, @snf, @qty_kgs, @total, @doe, @section, @createdby, @createdon, @branchid)");
                        //cmd.Parameters.Add("@productid", productid);
                        //cmd.Parameters.Add("@fat", FAT);
                        //cmd.Parameters.Add("@snf", SNF);
                        //cmd.Parameters.Add("@qty_kgs", qtyltrkgs);
                        //cmd.Parameters.Add("@total", dr["Qty"].ToString()); //qty ltrs save total column
                        //cmd.Parameters.Add("@doe", Convert.ToDateTime(fromdate));
                        //cmd.Parameters.Add("@section", "sales");
                        //cmd.Parameters.Add("@createdby", Context.Session["Employ_Sno"].ToString());
                        //cmd.Parameters.Add("@createdon", datetimenow);
                        //cmd.Parameters.Add("@branchid", BranchID);
                        //vdm.insert(cmd);
                    }
                }

                DataRow newvartical2 = despatch.NewRow();
                newvartical2["ProductName"] = "Total";
                totaldesp = Math.Round(totaldesp, 2);
                newvartical2["Qty(ltrs)"] = Math.Round(totaldesp, 2);
                newvartical2["Qty(kgs)"] = Math.Round(despatchkgstotal, 2);
                double kgfatt = despatchkgfattotal / totaldesp;
                newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);

                double kgSNFt = despatchkgsnftotal / totaldesp;
                newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);

                newvartical2["KG FAT"] = Math.Round(despatchkgfattotal, 2);
                newvartical2["KG SNF"] = Math.Round(despatchkgsnftotal, 2);

                despatch.Rows.Add(newvartical2);
                DataRow newvartical3 = despatch.NewRow();
                newvartical3["ProductName"] = "Curd Block";
                despatch.Rows.Add(newvartical3);
                grddespatch.DataSource = despatch;
                grddespatch.DataBind();
                Session["xportdata"] = despatch;
                pnldespatch.Visible = true;
            }
            else
            {
                DataRow newvartical3 = despatch.NewRow();
                newvartical3["ProductName"] = "Curd Block";
                despatch.Rows.Add(newvartical3);
                grddespatch.DataSource = despatch;
                grddespatch.DataBind();
                Session["xportdata"] = despatch;
                pnldespatch.Visible = true;
            }
       // }
        curd.Columns.Add("Silo Name");
        curd.Columns.Add("Department Name");
        curd.Columns.Add("qty(ltr)");
        curd.Columns.Add("qty(kgs)");
        curd.Columns.Add("FAT");
        curd.Columns.Add("SNF");
        curd.Columns.Add("CLR");
        curd.Columns.Add("KG FAT");
        curd.Columns.Add("KG SNF");
        cmd = new SqlCommand("SELECT silo_outward_transaction.date,  silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf, silo_outward_transaction.clr FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId  WHERE (silo_outward_transaction.departmentid = '1') AND (silo_outward_transaction.date BETWEEN @cdate1 AND @cdate2) AND (silo_outward_transaction.branchid=@cubranchid)");
        cmd.Parameters.Add("@cdate1", dtfromdate);
        cmd.Parameters.Add("@cdate2", dttodate);
        cmd.Parameters.Add("@cubranchid", BranchID);
        DataTable dtcurd = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtcurd.Rows.Count > 0)
        {
            curdltrstotal = 0;
            curdkgstotal = 0;
            curdfattotal = 0;
            curdsnftotal = 0;
            curdkgfattotal = 0;
            curdkgsnftotal = 0;
            foreach (DataRow dr in dtcurd.Rows)
            {
                DataRow newrow = curd.NewRow();
                newrow["Silo Name"] = dr["SiloName"].ToString();
                newrow["Department Name"] = dr["departmentname"].ToString();
                newrow["qty(ltr)"] = dr["qty_ltrs"].ToString();
                newrow["qty(kgs)"] = dr["qty_kgs"].ToString();

                double ltrs = 0;
                double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                curdltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                curdkgstotal += Kgs;
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                curdfattotal += FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                curdsnftotal += SNF;

                double ltrfat = 0;
                ltrfat = Kgs * FAT;
                ltrfat = Math.Round(ltrfat / 100, 2);
                curdkgfattotal += ltrfat;

                double ltrsnf = 0;
                ltrsnf = Kgs * SNF;
                ltrsnf = Math.Round(ltrsnf / 100, 2);
                curdkgsnftotal += ltrsnf;

                newrow["FAT"] = dr["fat"].ToString();
                newrow["SNF"] = dr["snf"].ToString();
                newrow["KG FAT"] = ltrfat;
                newrow["KG SNF"] = ltrsnf;
                curd.Rows.Add(newrow);
            }
            DataRow newvartical2 = curd.NewRow();
            newvartical2["Silo Name"] = "Total";
            newvartical2["qty(ltr)"] = curdltrstotal;
            newvartical2["qty(kgs)"] = curdkgstotal;
            newvartical2["KG FAT"] = curdkgfattotal;
            newvartical2["KG SNF"] = curdkgsnftotal;

            double kgfatt = curdkgfattotal / curdkgstotal;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            double kgSNFt = curdkgsnftotal / curdkgstotal;
            newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);
            curd.Rows.Add(newvartical2);

            DataRow newvartical3 = curd.NewRow();
            newvartical3["Silo Name"] = "Dispatch Tankers";
            curd.Rows.Add(newvartical3);
            grdcurd.DataSource = curd;
            grdcurd.DataBind();
            Session["xportdata"] = curd;
            pnlcurd.Visible = true;
        }
        else
        {
            DataRow newvartical3 = curd.NewRow();
            newvartical3["Silo Name"] = "Dispatch Tankers";
            curd.Rows.Add(newvartical3);
            grdcurd.DataSource = curd;
            grdcurd.DataBind();
            Session["xportdata"] = curd;
            pnlcurd.Visible = true;
        }
        //condencer.Columns.Add("Silo Name");
        //condencer.Columns.Add("Department Name");
        //condencer.Columns.Add("qty(ltr)");
        //condencer.Columns.Add("qty(kgs)");
        //condencer.Columns.Add("FAT");
        //condencer.Columns.Add("SNF");
        //condencer.Columns.Add("CLR");
        //condencer.Columns.Add("KG FAT");
        //condencer.Columns.Add("KG SNF");
        //cmd = new SqlCommand("SELECT silo_outward_transaction.date,  silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs,silo_outward_transaction.fat,silo_outward_transaction.snf,silo_outward_transaction.clr FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId  WHERE (silo_outward_transaction.departmentid = '9') AND (silo_outward_transaction.date BETWEEN @cadate1 AND @cadate2) AND (silo_outward_transaction.branchid=@cubranchid)");
        //cmd.Parameters.Add("@cadate1", GetLowDate(fromdate));
        //cmd.Parameters.Add("@cadate2", GetHighDate(todate));
        //cmd.Parameters.Add("@cubranchid", BranchID);
        //DataTable dtcon = SalesDB.SelectQuery(cmd).Tables[0];
        //if (dtcon.Rows.Count > 0)
        //{
        //    condencerltrstotal = 0;
        //    condencerkgstotal = 0;
        //    condencerfattotal = 0;
        //    condencersnftotal = 0;
        //    condencerkgfattotal = 0;
        //    condencerkgsnftotal = 0;
        //    foreach (DataRow dr in dtcon.Rows)
        //    {
        //        DataRow newrow = condencer.NewRow();
        //        newrow["Silo Name"] = dr["SiloName"].ToString();
        //        newrow["Department Name"] = dr["departmentname"].ToString();
        //        newrow["qty(ltr)"] = dr["qty_ltrs"].ToString();
        //        newrow["qty(kgs)"] = dr["qty_kgs"].ToString();
        //        double ltrs = 0;
        //        double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
        //        condencerltrstotal += ltrs;
        //        double Kgs = 0;
        //        double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
        //        condencerkgstotal += Kgs;
        //        double FAT = 0;
        //        double.TryParse(dr["fat"].ToString(), out FAT);
        //        condencerfattotal += FAT;
        //        double SNF = 0;
        //        double.TryParse(dr["snf"].ToString(), out SNF);
        //        condencersnftotal += SNF;
        //        double ltrfat = 0;
        //        ltrfat = Kgs * FAT;
        //        ltrfat = Math.Round(ltrfat / 100, 2);
        //        condencerkgfattotal += ltrfat;
        //        double ltrsnf = 0;
        //        ltrsnf = Kgs * SNF;
        //        ltrsnf = Math.Round(ltrsnf / 100, 2);
        //        condencerkgsnftotal += ltrsnf;
        //        newrow["FAT"] = dr["fat"].ToString();
        //        newrow["SNF"] = dr["snf"].ToString();
        //        newrow["KG FAT"] = ltrfat;
        //        newrow["KG SNF"] = ltrsnf;
        //        condencer.Rows.Add(newrow);
        //    }
        //    DataRow newvartical2 = condencer.NewRow();
        //    newvartical2["Silo Name"] = "Total";
        //    newvartical2["qty(ltr)"] = condencerltrstotal;
        //    newvartical2["qty(kgs)"] = condencerkgstotal;
        //    newvartical2["KG FAT"] = condencerkgfattotal;
        //    newvartical2["KG SNF"] = condencerkgsnftotal;
        //    double kgfatt = condencerkgfattotal / condencerkgstotal;
        //    newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
        //    double kgSNFt = condencerkgsnftotal / condencerkgstotal;
        //    newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);
        //    condencer.Rows.Add(newvartical2);
        //    DataRow newvartical3 = condencer.NewRow();
        //    newvartical3["Silo Name"] = "Dispatch Tankers";
        //    condencer.Rows.Add(newvartical3);
        //    grdcondencer.DataSource = condencer;
        //    grdcondencer.DataBind();
        //    Session["xportdata"] = condencer;
        //    pnlcondencer.Visible = true;
        //}
        //else
        //{
        //    DataRow newvartical3 = condencer.NewRow();
        //    newvartical3["Silo Name"] = "Dispatch Tankers";
        //    condencer.Rows.Add(newvartical3);
        //    grdcondencer.DataSource = condencer;
        //    grdcondencer.DataBind();
        //    Session["xportdata"] = condencer;
        //    pnlcondencer.Visible = true;
        //}

        tanker.Columns.Add("Silo Name");
        tanker.Columns.Add("Department Name");
        tanker.Columns.Add("qty(ltr)");
        tanker.Columns.Add("qty(kgs)");
        tanker.Columns.Add("FAT");
        tanker.Columns.Add("SNF");
        tanker.Columns.Add("CLR");
        tanker.Columns.Add("KG FAT");
        tanker.Columns.Add("KG SNF");
        cmd = new SqlCommand("SELECT silo_outward_transaction.date,  silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs,silo_outward_transaction.fat,silo_outward_transaction.snf,silo_outward_transaction.clr FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId  WHERE (silo_outward_transaction.departmentid = '4') AND (silo_outward_transaction.date BETWEEN @cdate1 AND @cdate2) AND (silo_outward_transaction.branchid=@tbranchid)");
        cmd.Parameters.Add("@cdate1", dtfromdate);
        cmd.Parameters.Add("@cdate2", dttodate);
        cmd.Parameters.Add("@tbranchid", BranchID);
        DataTable dttanker = SalesDB.SelectQuery(cmd).Tables[0];
        if (dttanker.Rows.Count > 0)
        {
            tankerltrstotal = 0;
            tankerkgstotal = 0;
            tankerfattotal = 0;
            tankersnftotal = 0;
            tankerkgfattotal = 0;
            tankerkgsnftotal = 0;
            foreach (DataRow dr in dttanker.Rows)
            {
                DataRow newrow = tanker.NewRow();
                newrow["Silo Name"] = dr["SiloName"].ToString();
                newrow["Department Name"] = dr["departmentname"].ToString();
                newrow["qty(ltr)"] = dr["qty_ltrs"].ToString();
                newrow["qty(kgs)"] = dr["qty_kgs"].ToString();
                double ltrs = 0;
                double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                tankerltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                tankerkgstotal += Kgs;
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                tankerfattotal += FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                tankersnftotal += SNF;
                double ltrfat = 0;
                ltrfat = Kgs * FAT;
                ltrfat = Math.Round(ltrfat / 100, 0);
                tankerkgfattotal += ltrfat;
                double ltrsnf = 0;
                ltrsnf = Kgs * SNF;
                ltrsnf = Math.Round(ltrsnf / 100, 0);
                tankerkgsnftotal += ltrsnf;

                newrow["FAT"] = dr["fat"].ToString();
                newrow["SNF"] = dr["snf"].ToString();
                newrow["KG FAT"] = ltrfat;
                newrow["KG SNF"] = ltrsnf;
                tanker.Rows.Add(newrow);
            }
            DataRow newvartical2 = tanker.NewRow();
            newvartical2["Silo Name"] = "Total";
            newvartical2["qty(ltr)"] = tankerltrstotal;
            newvartical2["qty(kgs)"] = tankerkgstotal;
            newvartical2["KG FAT"] = tankerkgfattotal;
            newvartical2["KG SNF"] = tankerkgsnftotal;

            double kgfatt = tankerkgfattotal / tankerkgstotal;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            double kgsnft = tankerkgsnftotal / tankerkgstotal;
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

        siloclosingReport.Columns.Add("SiloName");
        siloclosingReport.Columns.Add("BatchName");
        siloclosingReport.Columns.Add("qty(ltr)");
        siloclosingReport.Columns.Add("qty(kgs)");
        siloclosingReport.Columns.Add("FAT");
        siloclosingReport.Columns.Add("SNF");
        siloclosingReport.Columns.Add("CLR");
        siloclosingReport.Columns.Add("KG FAT");
        siloclosingReport.Columns.Add("KG SNF");
        cmd = new SqlCommand("SELECT sm.SiloName, scd.qty_kgs, scd.fat, scd.snf, scd.clr, scd.closingdate, scd.batchid, bm.batch FROM   silowiseclosingdetails AS scd INNER JOIN silomaster AS sm ON sm.SiloId = scd.siloid LEFT OUTER JOIN batchmaster AS bm ON scd.batchid = bm.batchid WHERE  (scd.closingdate BETWEEN @d1 AND @d2) AND (scd.branchid = @sbranchid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate).AddDays(1));
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
                string batch = "";
                batch = dr["batch"].ToString();
                string siloname = dr["SiloName"].ToString();
                newrow["SiloName"] = siloname;
                newrow["BatchName"] = batch;
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
        cmd = new SqlCommand("SELECT silo_outward_transaction.date,  silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs,silo_outward_transaction.fat,silo_outward_transaction.snf,silo_outward_transaction.clr FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId  WHERE (silo_outward_transaction.departmentid = '3') AND (silo_outward_transaction.date BETWEEN @dt1 AND @dt2) AND (silo_outward_transaction.branchid=@crbranchid)");
        //cmd = new SqlCommand("SELECT sno, creamtype, ob, obfat, recivedqty, recivedfat, totalcreamqty, avgfat, branchid, userid, doe FROM gheecreamdetails WHERE (doe BETWEEN @dt1 AND @dt2) ORDER BY sno DESC");
        cmd.Parameters.Add("@dt1", dtfromdate);
        cmd.Parameters.Add("@dt2", dttodate);
        cmd.Parameters.Add("@crbranchid", BranchID);
        DataTable dtcream = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtcream.Rows.Count > 0)
        {
            creamqtykgstotal = 0;
            creamkgstotal = 0;
            creamfattotal = 0;
            creamsnftotal = 0;
            creamclrtotal = 0;
            creamkgfattotal = 0;
            creamkgsnftotal = 0;
            foreach (DataRow dr in dtcream.Rows)
            {
                DataRow newrow = creamproduction.NewRow();
                newrow["Cream Type"] = "Cow";
                newrow["Silo Name"] = dr["SiloName"].ToString();
                newrow["Department Name"] = dr["departmentname"].ToString();
                newrow["Quantity(kgs)"] = dr["qty_kgs"].ToString();
                newrow["Quantity(ltrs)"] = dr["qty_ltrs"].ToString();
                newrow["FAT"] = dr["fat"].ToString();
                newrow["SNF"] = dr["snf"].ToString();

                newrow["KG FAT"] = dr["fat"].ToString();
                newrow["KG SNF"] = dr["fat"].ToString();
                DateTime dtdatet = Convert.ToDateTime(dr["date"].ToString());
                newrow["Date"] = dtdatet.ToString("dd/MM/yyyy");
                double qtykgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out qtykgs);
                creamqtykgstotal += qtykgs;

                double qtyltrs = 0;
                double.TryParse(dr["qty_ltrs"].ToString(), out qtyltrs);
                creamltrstotal += qtyltrs;

                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                creamfattotal += FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                creamsnftotal += SNF;
                double KGFAT = 0;
                double KGSNF = 0;
                KGFAT = (FAT * qtykgs) / 100;
                KGSNF = (SNF * qtykgs) / 100;
                creamkgfattotal += KGFAT;
                creamkgsnftotal += KGSNF;
                newrow["KG FAT"] = KGFAT;
                newrow["KG SNF"] = KGSNF;
                creamproduction.Rows.Add(newrow);
            }
            DataRow newvartical2 = creamproduction.NewRow();
            newvartical2["Cream Type"] = "Total";
            newvartical2["Quantity(kgs)"] = creamqtykgstotal;
            newvartical2["Quantity(ltrs)"] = creamltrstotal;
            newvartical2["KG FAT"] = Math.Round(creamkgfattotal, 2);
            newvartical2["KG SNF"] = Math.Round(creamkgsnftotal, 2);
            double kgfatt = creamkgfattotal / creamqtykgstotal;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            double kgsnft = creamkgsnftotal / creamqtykgstotal;
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

        BUTTER.Columns.Add("Silo Name");
        BUTTER.Columns.Add("Department Name");
        BUTTER.Columns.Add("qty(ltr)");
        BUTTER.Columns.Add("qty(kgs)");
        BUTTER.Columns.Add("FAT");
        BUTTER.Columns.Add("SNF");
        BUTTER.Columns.Add("CLR");
        BUTTER.Columns.Add("KG FAT");
        BUTTER.Columns.Add("KG SNF");
        cmd = new SqlCommand("SELECT silo_outward_transaction.date,  silomaster.SiloName, processingdepartments.departmentname, silo_outward_transaction.productid, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf, silo_outward_transaction.clr FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId  WHERE (silo_outward_transaction.departmentid = '10') AND (silo_outward_transaction.date BETWEEN @budate1 AND @budate2) AND (silo_outward_transaction.branchid=@bubranchid)");
        cmd.Parameters.Add("@budate1", dtfromdate);
        cmd.Parameters.Add("@budate2", dttodate);
        cmd.Parameters.Add("@bubranchid", BranchID);
        DataTable dtbutter = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtbutter.Rows.Count > 0)
        {
            butterltrstotal = 0;
            butterkgstotal = 0;
            butterfattotal = 0;
            buttersnftotal = 0;
            butterkgfattotal = 0;
            butterkgsnftotal = 0;
            foreach (DataRow dr in dtbutter.Rows)
            {
                DataRow newrow = BUTTER.NewRow();
                newrow["Silo Name"] = dr["SiloName"].ToString();
                newrow["Department Name"] = dr["departmentname"].ToString();
                newrow["qty(ltr)"] = dr["qty_ltrs"].ToString();
                newrow["qty(kgs)"] = dr["qty_kgs"].ToString();

                double ltrs = 0;
                double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                butterltrstotal += ltrs;
                double Kgs = 0;
                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                butterkgstotal += Kgs;
                double FAT = 0;
                double.TryParse(dr["fat"].ToString(), out FAT);
                butterfattotal += FAT;
                double SNF = 0;
                double.TryParse(dr["snf"].ToString(), out SNF);
                buttersnftotal += SNF;

                double ltrfat = 0;
                ltrfat = Kgs * FAT;
                ltrfat = Math.Round(ltrfat / 100, 2);
                butterkgfattotal += ltrfat;

                double ltrsnf = 0;
                ltrsnf = Kgs * SNF;
                ltrsnf = Math.Round(ltrsnf / 100, 2);
                butterkgsnftotal += ltrsnf;

                newrow["FAT"] = dr["fat"].ToString();
                newrow["SNF"] = dr["snf"].ToString();
                newrow["KG FAT"] = ltrfat;
                newrow["KG SNF"] = ltrsnf;
                BUTTER.Rows.Add(newrow);
            }
            DataRow newvartical2 = BUTTER.NewRow();
            newvartical2["Silo Name"] = "Total";
            newvartical2["qty(ltr)"] = butterltrstotal;
            newvartical2["qty(kgs)"] = butterkgstotal;
            newvartical2["KG FAT"] = butterkgfattotal;
            newvartical2["KG SNF"] = butterkgsnftotal;

            double kgfatt = butterkgfattotal / butterkgstotal;
            newvartical2["FAT"] = Math.Round(kgfatt * 100, 2);
            double kgSNFt = butterkgsnftotal / butterkgstotal;
            newvartical2["SNF"] = Math.Round(kgSNFt * 100, 2);
            BUTTER.Rows.Add(newvartical2);

            DataRow newvartical3 = BUTTER.NewRow();
            newvartical3["Silo Name"] = "Total2";
            BUTTER.Rows.Add(newvartical3);
            grdbutter.DataSource = BUTTER;
            grdbutter.DataBind();
            Session["xportdata"] = BUTTER;
            pnlbutter.Visible = true;
        }
        else
        {
            DataRow newvartical3 = BUTTER.NewRow();
            newvartical3["Silo Name"] = "Total2";
            BUTTER.Rows.Add(newvartical3);
            grdbutter.DataSource = BUTTER;
            grdbutter.DataBind();
            Session["xportdata"] = BUTTER;
            pnlbutter.Visible = true;
        }

        double outwardqtykgs = 0;
        double outwardqtyltrs = 0;
        double inwardqtykgs = 0;
        double inwardqtyltrs = 0;
        DateTime dt = fromdate;
        string month = dt.ToString("MM");
        string deptid = "9";

        //condencer
        //DataTable dtcondencer = new DataTable();
        //dtcondencer.Columns.Add("Dispatch Qty(ltrs)");
        //dtcondencer.Columns.Add("Recive Qty(ltrs)");
        //dtcondencer.Columns.Add("Evaporation Qty(ltrs)");
        ////dtcondencer.Columns.Add("returnqtyltrs");
        ////dtcondencer.Columns.Add("inwardqtykgs");
        ////dtcondencer.Columns.Add("inwardqtyltrs");
        //cmd = new SqlCommand("SELECT SUM(qty_kgs) AS outwordqtykgs, SUM(qty_ltrs) AS outwordqtyltrs FROM  silo_outward_transaction WHERE  (departmentid = @deptid) AND (branchid = @branchid) AND (DATEPART(mm, date) = @date)");
        //cmd.Parameters.Add("@branchid", BranchID);
        //cmd.Parameters.Add("@deptid", deptid);
        //cmd.Parameters.Add("@date", month);
        //DataTable dtoutwardcondencerdtails = SalesDB.SelectQuery(cmd).Tables[0];
        //cmd = new SqlCommand("SELECT SUM(qty_kgs) AS qtykgs, SUM(qty_ltrs) AS qtyltrs FROM silo_inward_transaction WHERE (deptid = @cdeptid) AND (DATEPART(mm, date) = @cdate) AND (branchid = @cpdbranchid)");
        //cmd.Parameters.Add("@cdeptid", deptid);
        //cmd.Parameters.Add("@cdate", month);
        //cmd.Parameters.Add("@cpdbranchid", BranchID);
        //DataTable dtinwardcondencerdtails = SalesDB.SelectQuery(cmd).Tables[0];
        //DataRow connewrow = dtcondencer.NewRow();
        //if (dtoutwardcondencerdtails.Rows.Count > 0)
        //{
        //    foreach (DataRow dr in dtoutwardcondencerdtails.Rows)
        //    {
        //        string qtykgs = dr["outwordqtykgs"].ToString();
        //        string qtyltrs = dr["outwordqtyltrs"].ToString();
        //       // connewrow["Outwardqtykgs"] = qtykgs;
        //        connewrow["Dispatch Qty(ltrs)"] = qtyltrs;
        //        if (qtyltrs != "")
        //        {
        //            outwardqtyltrs = Convert.ToDouble(qtyltrs);
        //        }
        //    }
        //}
        //if (dtinwardcondencerdtails.Rows.Count > 0)
        //{
        //    foreach (DataRow drr in dtinwardcondencerdtails.Rows)
        //    {
        //        string inqtykgs = drr["qtykgs"].ToString();
        //        string inqtyltrs = drr["qtyltrs"].ToString();
        //        //connewrow["inwardqtykgs"] = inqtykgs;
        //        connewrow["Recive Qty(ltrs)"] = inqtyltrs;
        //        if (inqtyltrs != "")
        //        {
        //            inwardqtyltrs = Convert.ToDouble(inqtyltrs);
        //        }
        //    }
        //}
        //connewrow["Evaporation Qty(ltrs)"] = outwardqtyltrs - inwardqtyltrs;
        //dtcondencer.Rows.Add(connewrow);
        //grdcondencer.DataSource = dtcondencer;
        //grdcondencer.DataBind();
        //Session["xportdata"] = dtcondencer;
        //pnlcondencer.Visible = true;

        // condenser sai

        DataTable dtcondencer = new DataTable();
        dtcondencer.Columns.Add("Milk Type");
        dtcondencer.Columns.Add("Qty(ltrs)");
        dtcondencer.Columns.Add("FAT %");
        dtcondencer.Columns.Add("SNF %");
        dtcondencer.Columns.Add("LTR FAT %");
        dtcondencer.Columns.Add("LTR SNF %");

        cmd = new SqlCommand("SELECT   (qty_kgs) AS outwordqtykgs, (qty_ltrs) AS outwordqtyltrs, date, fat, snf, clr FROM  silo_outward_transaction WHERE  (departmentid = @deptid) AND (branchid = @branchid) AND (date BETWEEN @cd1 AND @cd2)");
        cmd.Parameters.Add("@branchid", BranchID);
        cmd.Parameters.Add("@deptid", deptid);
        cmd.Parameters.Add("@cd1", dtfromdate);
        cmd.Parameters.Add("@cd2", dttodate);
        DataTable dtcond = SalesDB.SelectQuery(cmd).Tables[0];

        cmd = new SqlCommand("SELECT  (qty_kgs) AS quantity, fat, snf, clr, (qty_ltrs) AS qty_ltr FROM  silo_inward_transaction WHERE (deptid = 9) AND (date BETWEEN @crd1 AND @crd2) AND (branchid = @branchid)");
        cmd.Parameters.Add("@branchid", BranchID);
        cmd.Parameters.Add("@crd1", dtfromdate);
        cmd.Parameters.Add("@crd2", dttodate);
        DataTable dtcondreturn = SalesDB.SelectQuery(cmd).Tables[0];
        double outwardqty = 0;
        double returnrecivedqty = 0;
        double outwardqtyltro = 0;
        double returnrecivedqtys = 0;
        double ltrfati = 0;
        double ltrsnfi = 0;
        double ltrfatr = 0;
        double ltrsnfr = 0;
        if (dtcond.Rows.Count > 0)
        {
            foreach (DataRow dr in dtcond.Rows)
            {
                DataRow connewrow = dtcondencer.NewRow();
                connewrow["Milk Type"] = "Skim Milk";
                string qtykgs = dr["outwordqtykgs"].ToString();
                string qtyltrs = dr["outwordqtyltrs"].ToString();
                connewrow["Qty(ltrs)"] = qtyltrs;
                if (qtyltrs != "")
                {
                    outwardqtyltrs = Convert.ToDouble(qtyltrs);
                    outwardqty = Convert.ToDouble(qtyltrs);
                    outwardqtyltro += outwardqtyltrs;
                }
                string fat = dr["fat"].ToString();
                double fats = Convert.ToDouble(fat);
                string snf = dr["snf"].ToString();
                double snfs = Convert.ToDouble(snf);
                connewrow["FAT %"] = fat;
                connewrow["SNF %"] = snf;
                double ltrfat = Math.Round((outwardqtyltrs * fats) / 100, 2);
                connewrow["LTR FAT %"] = ltrfat;
                ltrfati += ltrfat;
                double ltrsnf = Math.Round((outwardqtyltrs * snfs) / 100, 2);
                connewrow["LTR SNF %"] = ltrsnf;
                ltrsnfi += ltrsnf;
                dtcondencer.Rows.Add(connewrow);
            }
        }
        if (dtcondreturn.Rows.Count > 0)
        {
            foreach (DataRow dr in dtcondreturn.Rows)
            {
                DataRow connewrow1 = dtcondencer.NewRow();
                connewrow1["Milk Type"] = "Condenser Milk";
                string qtykgs = dr["quantity"].ToString();
                string qtyltrs = dr["qty_ltr"].ToString();
                connewrow1["Qty(ltrs)"] = qtyltrs;
                if (qtyltrs != "")
                {
                    outwardqtyltrs = Convert.ToDouble(qtyltrs);
                    returnrecivedqty = Convert.ToDouble(qtyltrs);
                    returnrecivedqtys += returnrecivedqty;
                }
                string fat = dr["fat"].ToString();
                double fatsr = Convert.ToDouble(fat);
                string snf = dr["snf"].ToString();
                double snfsr = Convert.ToDouble(snf);
                connewrow1["FAT %"] = fat;
                connewrow1["SNF %"] = snf;
                double ltrfat = Math.Round((outwardqtyltrs * fatsr) / 100, 2);
                connewrow1["LTR FAT %"] = ltrfat;
                ltrfatr += ltrfat;
                double ltrsnf = Math.Round((outwardqtyltrs * snfsr) / 100, 2);
                connewrow1["LTR SNF %"] = ltrsnf;
                ltrsnfr += ltrsnf;
                dtcondencer.Rows.Add(connewrow1);
            }
        }
        DataRow connewrow2 = dtcondencer.NewRow();
        connewrow2["Milk Type"] = "Difference";
        connewrow2["Qty(ltrs)"] = Math.Round(returnrecivedqtys - outwardqtyltro, 2);
        connewrow2["LTR FAT %"] = Math.Round(ltrfatr - ltrfati, 2);
        connewrow2["LTR SNF %"] = Math.Round(ltrsnfr - ltrsnfi, 2);
        dtcondencer.Rows.Add(connewrow2);
        DataRow connewrow3 = dtcondencer.NewRow();
        connewrow3["Milk Type"] = "Difference %";
        double tots = Math.Round((returnrecivedqty / outwardqtyltro) * 100, 2);
        string totss = tots.ToString();
        if (totss == "NaN" || totss == "infinity")
        {
            totss = "0";
        }
        connewrow3["Qty(ltrs)"] = totss;
        dtcondencer.Rows.Add(connewrow3);
        grdcondencer.DataSource = dtcondencer;
        grdcondencer.DataBind();
        Session["xportdata"] = dtcondencer;
        pnlcondencer.Visible = true;
        //end
        double stockqtykgscq = 0;
        double stockqtykgscb = 0;
        double stockqtykgsrq = 0;
        double stockqtykgsob = 0;
        stockreport.Columns.Add("Product Name");
        stockreport.Columns.Add("Opening Balance(qty_kgs)");
        stockreport.Columns.Add("No of Bags");
        stockreport.Columns.Add("Recived Quantity");
        stockreport.Columns.Add("Consumption Quantity");
        stockreport.Columns.Add("Stock Transfor Quantity");
        stockreport.Columns.Add("Sales");
        stockreport.Columns.Add("Closing Balance");

        cmd = new SqlCommand("SELECT (qty_kgs) AS consumptionqty, (recivedqty) as recivedqty, (ob) as openingbalance, stocktransfor FROM  smp_details where (date between @smpd1 and @smpd2) AND (branchid=@smpbranchid)");
        cmd.Parameters.Add("@smpd1", dtfromdate);
        cmd.Parameters.Add("@smpd2", dttodate);
        cmd.Parameters.Add("@smpbranchid", BranchID);
        DataTable dtsmpreport = SalesDB.SelectQuery(cmd).Tables[0];
        //////////////////.............Changed By Ravindra.////////////////////////
        //cmd = new SqlCommand("Select  (cpd.qty_kgs) AS recivedqty, (cpd.ob) AS openingbalance, (cpd.production) AS consumptionqty, cpd.total, cpd.sales,  pm.productname from curd_productiondetails cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.doe BETWEEN @d1 AND @d2) and (pm.batchid='16') and (pm.departmentid='1')");
        cmd = new SqlCommand("SELECT cpd.qty_kgs AS recivedqty, cpd.ob AS openingbalance, cpd.production AS consumptionqty, cpd.total, cpd.sales, pm.productname FROM curd_productiondetails AS cpd INNER JOIN productmaster AS pm ON pm.sno = cpd.productid INNER JOIN branch_info AS bi ON bi.sno = cpd.branchid WHERE (cpd.doe BETWEEN @d1 AND @d2) AND (pm.batchid = '16') AND (pm.departmentid = '1') AND (cpd.branchid=@cpdbranchid) GROUP BY cpd.qty_kgs, cpd.ob, cpd.production, cpd.total, cpd.sales, pm.productname");
        cmd.Parameters.Add("@d1", dtfromdate);
        cmd.Parameters.Add("@d2", dttodate);
        cmd.Parameters.Add("@cpdbranchid", BranchID);
        DataTable dtcurdproduction = SalesDB.SelectQuery(cmd).Tables[0];

        string gheeproductname = "";
        string gheeob = "";
        string gheerecive = "";
        string gheeconsumption = "";
        string gheesales = "";
        string gheeclosing = "";

        cmd = new SqlCommand("SELECT pm.productname, (gheeclosingdetails.quantity) as openingbalance FROM gheeclosingdetails inner join productmaster pm ON pm.sno = gheeclosingdetails.productid WHERE (gheeclosingdetails.doe BETWEEN @d1 AND @d2) AND (gheeclosingdetails.productid = '10') AND gheeclosingdetails.branchid=@gcdbranchid");
        cmd.Parameters.Add("@d1", dtfromdate);
        cmd.Parameters.Add("@d2", dttodate);
        cmd.Parameters.Add("@gcdbranchid", BranchID);
        DataTable dtgheeloose = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtgheeloose.Rows.Count > 0)
        {
            foreach (DataRow dr in dtgheeloose.Rows)
            {
                gheeproductname = dr["productname"].ToString();
                gheeob = dr["openingbalance"].ToString();
            }
        }

        cmd = new SqlCommand("SELECT SUM(productionqty) AS recivedqty FROM gheeproduction WHERE (doe BETWEEN @d1 AND @d2) AND (branchid=@gpbranchid)");
        cmd.Parameters.Add("@d1", dtfromdate);
        cmd.Parameters.Add("@d2", dttodate);
        cmd.Parameters.Add("@gpbranchid", BranchID);
        DataTable dtgheeproduction = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtgheeproduction.Rows.Count > 0)
        {
            foreach (DataRow dr in dtgheeproduction.Rows)
            {
                gheerecive = dr["recivedqty"].ToString();
            }
        }

        cmd = new SqlCommand("SELECT SUM(salesquantity) AS sales FROM  gheesales  WHERE   (doe BETWEEN @d1 AND @d2) AND (branchid=@gsbranchid)");
        cmd.Parameters.Add("@d1", dtfromdate);
        cmd.Parameters.Add("@d2", dttodate);
        cmd.Parameters.Add("@gsbranchid", BranchID);
        DataTable dtgheesales = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtgheesales.Rows.Count > 0)
        {
            foreach (DataRow dr in dtgheesales.Rows)
            {
                gheesales = dr["sales"].ToString();
            }
        }

        DataTable dtgheedetails = new DataTable();
        dtgheedetails.Columns.Add("productname");
        dtgheedetails.Columns.Add("openingbalance").DataType = typeof(Double);
        dtgheedetails.Columns.Add("recivedqty").DataType = typeof(Double);
        dtgheedetails.Columns.Add("consumptionqty").DataType = typeof(Double);
        dtgheedetails.Columns.Add("sales").DataType = typeof(Double);
        DataRow newghee = dtgheedetails.NewRow();
        if (gheeproductname != "")
        {
            newghee["productname"] = gheeproductname;
            newghee["openingbalance"] = gheeob;
            if (gheerecive == "")
            {
                gheerecive = "0";
            }
            newghee["recivedqty"] = gheerecive;
            newghee["consumptionqty"] = gheerecive;
            if (gheesales == "")
            {
                gheesales = "0";
            }
            newghee["sales"] = gheesales;
            //dtgheedetails.Rows.Add(newghee);
        }
        dtgheedetails.Rows.Add(newghee);
        DataTable dtAll1 = dtsmpreport.Copy();
        dtAll1.Merge(dtcurdproduction);

        DataTable dtall2 = dtAll1.Copy();
        dtall2.Merge(dtgheedetails);
        if (dtall2.Rows.Count > 0)
        {
            stockqtykgscq = 0;
            stockqtykgscb = 0;
            stockqtykgsrq = 0;
            stockqtykgsob = 0;

            foreach (DataRow dr in dtall2.Rows)
            {

                double saleqty = 0;
                double total = 0;
                double cb = 0;
                double obbal = 0;
                string ob = dr["openingbalance"].ToString();
                if (ob != "")
                {
                    obbal = Convert.ToDouble(ob);
                }
                if (obbal > 0)
                {
                    DataRow newrow = stockreport.NewRow();
                    string productname = dr["productname"].ToString();
                    string sales = dr["sales"].ToString();
                    if (sales == "")
                    {
                        saleqty = 0;
                    }
                    else
                    {
                        saleqty = Convert.ToDouble(sales);
                    }
                    if (productname == "")
                    {
                        newrow["Product Name"] = "SMP";
                    }
                    else
                    {
                        newrow["Product Name"] = productname;
                    }

                    double openingbalance = 0;
                    double.TryParse(dr["openingbalance"].ToString(), out openingbalance);
                    stockqtykgsob += openingbalance;

                    double recivedqty = 0;
                    double.TryParse(dr["recivedqty"].ToString(), out recivedqty);
                    stockqtykgsrq += recivedqty;
                    double stocktransfor = 0;
                    double.TryParse(dr["stocktransfor"].ToString(), out stocktransfor);

                    double consumptionqty = 0;
                    double.TryParse(dr["consumptionqty"].ToString(), out consumptionqty);
                    stockqtykgscq += consumptionqty;

                    if (productname == "")
                    {
                        total = openingbalance + recivedqty;
                        double consum = consumptionqty + stocktransfor;
                        cb = total - consum;
                    }
                    else
                    {
                        if (productname == "GHEE LOOSE")
                        {
                            total = openingbalance + recivedqty;
                            total = total - saleqty;
                            cb = total;
                        }
                        else
                        {
                            total = openingbalance + consumptionqty;
                            cb = total - saleqty;
                        }
                    }
                    openingbalance = Math.Round(openingbalance, 2);
                    newrow["Opening Balance(qty_kgs)"] = openingbalance;
                    if (productname == "")
                    {
                        newrow["No of Bags"] = openingbalance / 25;
                        newrow["Stock Transfor Quantity"] = stocktransfor;
                    }
                    newrow["Recived Quantity"] = recivedqty;
                    newrow["Consumption Quantity"] = consumptionqty;
                    newrow["Sales"] = sales;
                    newrow["Closing Balance"] = cb;
                    stockqtykgscb += cb;
                    stockreport.Rows.Add(newrow);
                }
            }

            //DataTable dtbp = dtbutterproduction.Copy();
            //dtbp.Merge(dtbuttersales);

            //
            //DataRow newvartical2 = stockreport.NewRow();
            //newvartical2["Product Name"] = "Total";
            //newvartical2["Opening Balance(qty_kgs)"] = Math.Round(stockqtykgsob, 2);
            //newvartical2["Recived Quantity"] = Math.Round(stockqtykgsrq, 2);
            //newvartical2["Consumption Quantity"] = Math.Round(stockqtykgscq, 2);
            //newvartical2["Closing Balance"] = Math.Round(stockqtykgscb, 2);
            //stockreport.Rows.Add(newvartical2);

            //grdstockreport.DataSource = stockreport;
            //grdstockreport.DataBind();
            //Session["xportdata"] = stockreport;
            //pnlstockreport.Visible = true;
        }
        else
        {
            DataRow new3 = stockreport.NewRow();
            new3["Product Name"] = "Total2";
            stockreport.Rows.Add(new3);
            //grdstockreport.DataSource = stockreport;
            //grdstockreport.DataBind();
            //Session["xportdata"] = stockreport;
            //pnlstockreport.Visible = true;
        }


        DataTable dtpbutter = new DataTable();
        dtpbutter.Columns.Add("Product Name");
        dtpbutter.Columns.Add("Opening Balance(qty_kgs)");
        dtpbutter.Columns.Add("Recive Cream Quantity");
        dtpbutter.Columns.Add("Recive Cream Fat");
        dtpbutter.Columns.Add("Production Quantity");
        dtpbutter.Columns.Add("Sales");
        dtpbutter.Columns.Add("Closing Balance");

        cmd = new SqlCommand("SELECT  ppd.sno, ppd.productid, ppd.ob AS openingbalance, ppd.convertionquantity, ppd.convertionfat, ppd.productionqty, ppd.sales, ppd.type, ppd.fat, ppd.snf, ppd.qty_kgs, ppd.total, ppd.remarks, ppd.createdby, ppd.createdon, ppd.branchid, ppd.cb AS closingbalance, pm.productname FROM  plant_production_details AS ppd INNER JOIN   productmaster AS pm ON ppd.productid = pm.sno WHERE  (ppd.createdon BETWEEN @bd1 AND @bd2) AND (ppd.branchid = @bsbranchid) AND (ppd.deptid = 10)");
        cmd.Parameters.Add("@bd1", dtfromdate);
        cmd.Parameters.Add("@bd2", dttodate);
        cmd.Parameters.Add("@bsbranchid", BranchID);
        DataTable dtbutterproduction = SalesDB.SelectQuery(cmd).Tables[0];

        cmd = new SqlCommand("SELECT SUM(salesquantity) AS sales FROM  buttersales  WHERE   (doe BETWEEN @sd1 AND @sd2) AND (branchid=@gbsbranchid)");
        cmd.Parameters.Add("@sd1", dtfromdate);
        cmd.Parameters.Add("@sd2", dttodate);
        cmd.Parameters.Add("@gbsbranchid", BranchID);
        DataTable dtbuttersales = SalesDB.SelectQuery(cmd).Tables[0];
        DataRow bnewvartical3 = dtpbutter.NewRow();
        double butterob = 0;
        double butterproduction = 0;
        double buttersale = 0;
        if (dtbutterproduction.Rows.Count > 0)
        {
            foreach (DataRow dr in dtbutterproduction.Rows)
            {
                string ob = dr["openingbalance"].ToString();
                if (ob != "")
                {
                    butterob = Convert.ToDouble(ob);
                }
                string convertionquantity = dr["convertionquantity"].ToString();
                string convertionfat = dr["convertionfat"].ToString();
                string productname = dr["productname"].ToString();
                string productionqty = dr["productionqty"].ToString();
                if (productionqty != "")
                {
                    butterproduction = Convert.ToDouble(productionqty);
                }
                bnewvartical3["Product Name"] = productname;
                bnewvartical3["Opening Balance(qty_kgs)"] = ob;
                bnewvartical3["Recive Cream Quantity"] = convertionquantity;
                bnewvartical3["Recive Cream Fat"] = convertionfat;
                bnewvartical3["Production Quantity"] = butterproduction;
            }
        }
        if (dtbuttersales.Rows.Count > 0)
        {
            foreach (DataRow dr in dtbuttersales.Rows)
            {
                string SALE = dr["sales"].ToString();
                bnewvartical3["Sales"] = SALE;
                if (SALE != "")
                {
                    buttersale = Convert.ToDouble(SALE);
                }
                else
                {
                    buttersale = 0;
                }
            }
            double closing = (butterob + butterproduction) - buttersale;
            bnewvartical3["Closing Balance"] = closing.ToString();
        }
        dtpbutter.Rows.Add(bnewvartical3);
        grdpbutter.DataSource = dtpbutter;
        grdpbutter.DataBind();
        pbutterpanel.Visible = true;

        double totalltrs1 = siloopeningltrstotal + reciptltrstotal + returnmilkltrstotal;
        double totalltrs2 = totaldesp + tankerltrstotal + curdltrstotal + siloclosingltrstotal + creamltrstotal + butterltrstotal;
        double glltrs = totalltrs2 - totalltrs1;

        double totalkgs1 = siloopeningkgstotal + reciptkgstotal + returnmilkkgstotal;
        double totalkgs2 = despatchkgstotal + tankerkgstotal + curdkgstotal + siloclosingkgstotal + creamqtykgstotal + butterkgstotal;
        double glkgss = totalkgs2 - totalkgs1;

        double totalkgfat1 = siloopeningkgfattotal + reciptkgfattotal + returnmilkkgfattotal + smpkgfattotal;
        double totalkgfat2 = despatchkgfattotal + tankerkgfattotal + curdkgfattotal + siloclosingkgfattotal + creamkgfattotal + butterkgfattotal;
        double glkgfat = totalkgfat2 - totalkgfat1;

        double totalkgsnf1 = siloopeningkgsnftotal + reciptkgsnftotal + returnmilkkgsnftotal + smpkgsnftotal;
        double totalkgsnf2 = despatchkgsnftotal + tankerkgsnftotal + curdkgsnftotal + siloclosingkgsnftotal + creamkgsnftotal + butterkgsnftotal;
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
            if (e.Row.Cells[0].Text == "Cow")
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

    protected void grdreturn_RowDataBound(object sender, GridViewRowEventArgs e)
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
            if (e.Row.Cells[0].Text == "Total1")
            {
                e.Row.Cells[0].Width = new Unit("320px");
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
            if (e.Row.Cells[0].Text == "Cream Production")
            {
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
            if (e.Row.Cells[0].Text == "Dispatch Qty(ltrs)")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
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
            if (e.Row.Cells[0].Text == "Closing Balance")
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

    protected void grdstockreport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
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

    protected void grdpbutter_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        getdata();
    }
}