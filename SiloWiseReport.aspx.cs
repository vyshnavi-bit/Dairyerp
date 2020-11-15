using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class SiloWiseReport : System.Web.UI.Page
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
                    dtp_ToDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                    bindsilos();
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

    private void bindsilos()
    {
        SalesDBManager SalesDB = new SalesDBManager();
        cmd = new SqlCommand("SELECT SiloId, SiloName FROM silomaster where branchid=@branchid");
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlfromsilo.DataSource = dttrips;
        ddlfromsilo.DataTextField = "SiloName";
        ddlfromsilo.DataValueField = "SiloId";
        ddlfromsilo.DataBind();
        ddlfromsilo.ClearSelection();
        ddlfromsilo.Items.Insert(0, new ListItem { Value = "0", Text = "--Select Silo--", Selected = true });
        ddlfromsilo.SelectedValue = "0";
      
    }
    DataTable Report = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        get_daywise_details();
        //silo_wise_details();
    }
    private void silo_wise_details()
    {
        try
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
            datestrig = dtp_ToDate.Text.Split(' ');
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
            Report.Columns.Add("Sno");
            Report.Columns.Add("SILO NAME");
            Report.Columns.Add("KGS").DataType = typeof(Double);
            Report.Columns.Add("LTRS").DataType = typeof(Double);
            Report.Columns.Add("FAT");
            Report.Columns.Add("SNF");
            Report.Columns.Add("CLR");
            Report.Columns.Add("LTR FAT");
            Report.Columns.Add("LTR SNF");
            cmd = new SqlCommand("select * from silomaster");
            DataTable dtsilos = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtsilos.Rows.Count > 0)
            {
                if (ddlfromsilo.SelectedItem.Value != "0")
                {
                    cmd = new SqlCommand("SELECT silomaster.SiloName AS FromSILO, intra_silo_transactions.doe, intra_silo_transactions.qty_ltrs, intra_silo_transactions.qty_kgs, intra_silo_transactions.fat,intra_silo_transactions.snf, intra_silo_transactions.clr, silomaster_1.SiloName AS ToSILO FROM intra_silo_transactions INNER JOIN silomaster ON intra_silo_transactions.fromsiloid = silomaster.SiloId INNER JOIN silomaster AS silomaster_1 ON intra_silo_transactions.tosiloid = silomaster_1.SiloId WHERE (intra_silo_transactions.branchid = @BranchID)  AND (intra_silo_transactions.fromsiloid=@siloid) AND (intra_silo_transactions.doe BETWEEN @d1 AND @d2)");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@BranchID", BranchID);
                    cmd.Parameters.Add("@siloid", ddlfromsilo.SelectedItem.Value);
                    DataTable dtIntra = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtIntra.Rows.Count > 0)
                    {
                        DataRow newvartical4 = Report.NewRow();
                        newvartical4["SILO NAME"] = "Intra IN SILO";
                        Report.Rows.Add(newvartical4);

                        int i = 1;
                        double kgfattotal = 0;
                        double kgsnftotal = 0;
                        double kgstotal = 0;
                        double Ltrstotal = 0;
                        foreach (DataRow dr in dtIntra.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                            string date = dtdoe.ToString("dd/MM/yyyy");
                            newrow["SILO NAME"] = dr["FromSILO"].ToString();
                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                            newrow["KGS"] = Kgs;
                            kgstotal += Kgs;
                            double ltrs = 0;
                            double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                            Ltrstotal += ltrs;
                            newrow["LTRS"] = ltrs;
                            double fat = 0;
                            double.TryParse(dr["fat"].ToString(), out fat);
                            newrow["FAT"] = fat;
                            double snf = 0;
                            double.TryParse(dr["snf"].ToString(), out snf);
                            newrow["SNF"] = snf;
                            newrow["CLR"] = dr["clr"].ToString();
                            double ltrfat = 0;
                            ltrfat = ltrs * fat;
                            ltrfat = Math.Round(ltrfat / 100, 2);
                            newrow["LTR FAT"] = ltrfat;
                            kgfattotal += ltrfat;
                            double ltrsnf = 0;
                            ltrsnf = ltrs * snf;
                            ltrsnf = Math.Round(ltrsnf / 100, 2);
                            newrow["LTR SNF"] = ltrsnf;
                            kgsnftotal += ltrsnf;
                            Report.Rows.Add(newrow);
                        }
                    }
                    cmd = new SqlCommand("SELECT silo_inward_transaction.sno,silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs, silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName FROM silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId WHERE (silo_inward_transaction.branchid = @branchID) AND (silo_inward_transaction.date BETWEEN @d1 AND @d2) and (silo_inward_transaction.siloid=@siloid)");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@BranchID", BranchID);
                    cmd.Parameters.Add("@siloid", ddlfromsilo.SelectedItem.Value);
                    DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtInward.Rows.Count > 0)
                    {
                        DataRow newvartical5 = Report.NewRow();
                        newvartical5["SILO NAME"] = "Inward";
                        Report.Rows.Add(newvartical5);
                        int i = 1;
                        double kgfattotal = 0;
                        double kgsnftotal = 0;
                        double kgstotal = 0;
                        double Ltrstotal = 0;
                        foreach (DataRow dr in dtInward.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            DateTime dtdoe = Convert.ToDateTime(dr["date"].ToString());
                            string date = dtdoe.ToString("dd/MM/yyyy");

                            double Kgs = 0;
                            double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                            newrow["KGS"] = Kgs;
                            kgstotal += Kgs;
                            double ltrs = 0;
                            double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                            Ltrstotal += ltrs;
                            newrow["LTRS"] = ltrs;
                            double fat = 0;
                            double.TryParse(dr["fat"].ToString(), out fat);
                            newrow["FAT"] = fat;
                            double snf = 0;
                            double.TryParse(dr["snf"].ToString(), out snf);
                            newrow["SNF"] = snf;
                            newrow["CLR"] = dr["clr"].ToString();
                            newrow["SILO NAME"] = dr["SiloName"].ToString();
                            double ltrfat = 0;
                            ltrfat = ltrs * fat;
                            ltrfat = Math.Round(ltrfat / 100, 2);
                            newrow["LTR FAT"] = ltrfat;
                            kgfattotal += ltrfat;
                            double ltrsnf = 0;
                            ltrsnf = ltrs * snf;
                            ltrsnf = Math.Round(ltrsnf / 100, 2);
                            newrow["LTR SNF"] = ltrsnf;
                            kgsnftotal += ltrsnf;
                            Report.Rows.Add(newrow);
                        }

                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        hidepanel.Visible = true;
                    }
                }
                else
                {
                    foreach (DataRow drsilo in dtsilos.Rows)
                    {
                        cmd = new SqlCommand("SELECT silomaster.SiloName AS FromSILO, intra_silo_transactions.doe, intra_silo_transactions.qty_ltrs, intra_silo_transactions.qty_kgs, intra_silo_transactions.fat,intra_silo_transactions.snf, intra_silo_transactions.clr, silomaster_1.SiloName AS ToSILO FROM intra_silo_transactions INNER JOIN silomaster ON intra_silo_transactions.fromsiloid = silomaster.SiloId INNER JOIN silomaster AS silomaster_1 ON intra_silo_transactions.tosiloid = silomaster_1.SiloId WHERE (intra_silo_transactions.branchid = @BranchID) AND (intra_silo_transactions.doe BETWEEN @d1 AND @d2) AND (intra_silo_transactions.fromsiloid=@siloid)");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@BranchID", BranchID);
                        cmd.Parameters.Add("@siloid", drsilo["SiloId"].ToString());
                        DataTable dtIntra = SalesDB.SelectQuery(cmd).Tables[0];
                        if (dtIntra.Rows.Count > 0)
                        {
                            DataRow newvartical4 = Report.NewRow();
                            newvartical4["SILO NAME"] = "Intra IN SILO";
                            Report.Rows.Add(newvartical4);

                            int i = 1;
                            double kgfattotal = 0;
                            double kgsnftotal = 0;
                            double kgstotal = 0;
                            double Ltrstotal = 0;
                            foreach (DataRow dr in dtIntra.Rows)
                            {
                                DataRow newrow = Report.NewRow();
                                newrow["Sno"] = i++.ToString();
                                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                                string date = dtdoe.ToString("dd/MM/yyyy");
                                newrow["SILO NAME"] = dr["FromSILO"].ToString();
                                double Kgs = 0;
                                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                                newrow["KGS"] = Kgs;
                                kgstotal += Kgs;
                                double ltrs = 0;
                                double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                                Ltrstotal += ltrs;
                                newrow["LTRS"] = ltrs;
                                double fat = 0;
                                double.TryParse(dr["fat"].ToString(), out fat);
                                newrow["FAT"] = fat;
                                double snf = 0;
                                double.TryParse(dr["snf"].ToString(), out snf);
                                newrow["SNF"] = snf;
                                newrow["CLR"] = dr["clr"].ToString();
                                double ltrfat = 0;
                                ltrfat = ltrs * fat;
                                ltrfat = Math.Round(ltrfat / 100, 2);
                                newrow["LTR FAT"] = ltrfat;
                                kgfattotal += ltrfat;
                                double ltrsnf = 0;
                                ltrsnf = ltrs * snf;
                                ltrsnf = Math.Round(ltrsnf / 100, 2);
                                newrow["LTR SNF"] = ltrsnf;
                                kgsnftotal += ltrsnf;
                                Report.Rows.Add(newrow);
                            }
                        }
                        cmd = new SqlCommand("SELECT silo_inward_transaction.sno,silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs, silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName FROM silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId WHERE (silo_inward_transaction.branchid = @branchID) AND (silo_inward_transaction.date BETWEEN @d1 AND @d2) and (silo_inward_transaction.siloid=@siloid)");
                        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                        cmd.Parameters.Add("@d2", GetHighDate(todate));
                        cmd.Parameters.Add("@BranchID", BranchID);
                        cmd.Parameters.Add("@siloid", drsilo["SiloId"].ToString());
                        DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
                        if (dtInward.Rows.Count > 0)
                        {
                            DataRow newvartical5 = Report.NewRow();
                            newvartical5["SILO NAME"] = "Inward";
                            Report.Rows.Add(newvartical5);
                            int i = 1;
                            double kgfattotal = 0;
                            double kgsnftotal = 0;
                            double kgstotal = 0;
                            double Ltrstotal = 0;
                            foreach (DataRow dr in dtInward.Rows)
                            {
                                DataRow newrow = Report.NewRow();
                                newrow["Sno"] = i++.ToString();
                                DateTime dtdoe = Convert.ToDateTime(dr["date"].ToString());
                                string date = dtdoe.ToString("dd/MM/yyyy");

                                double Kgs = 0;
                                double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                                newrow["KGS"] = Kgs;
                                kgstotal += Kgs;
                                double ltrs = 0;
                                double.TryParse(dr["qty_ltrs"].ToString(), out ltrs);
                                Ltrstotal += ltrs;
                                newrow["LTRS"] = ltrs;
                                double fat = 0;
                                double.TryParse(dr["fat"].ToString(), out fat);
                                newrow["FAT"] = fat;
                                double snf = 0;
                                double.TryParse(dr["snf"].ToString(), out snf);
                                newrow["SNF"] = snf;
                                newrow["CLR"] = dr["clr"].ToString();
                                newrow["SILO NAME"] = dr["SiloName"].ToString();
                                double ltrfat = 0;
                                ltrfat = ltrs * fat;
                                ltrfat = Math.Round(ltrfat / 100, 2);
                                newrow["LTR FAT"] = ltrfat;
                                kgfattotal += ltrfat;
                                double ltrsnf = 0;
                                ltrsnf = ltrs * snf;
                                ltrsnf = Math.Round(ltrsnf / 100, 2);
                                newrow["LTR SNF"] = ltrsnf;
                                kgsnftotal += ltrsnf;
                                Report.Rows.Add(newrow);
                            }
                        }
                    }

                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                hidepanel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
   
    private void get_daywise_details()
    {
        try
        {
            DataTable Final = new DataTable();
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
            datestrig = dtp_ToDate.Text.Split(' ');
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
            int Reportsno=1;
            int TempgTotalindex = 0;
            int gTotalindex = 0;
            Report.Columns.Add("Sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("Silo Inward");
            Report.Columns.Add("Intra In Silo");
            Report.Columns.Add("Intra Out Silo");
            Report.Columns.Add("Silo OutWard");
            cmd = new SqlCommand("select * from silomaster");
            DataTable dtsilos = SalesDB.SelectQuery(cmd).Tables[0];

            if (dtsilos.Rows.Count > 0)
            {
                double siloinward = 0;
                double intrasilo = 0;
                double PerDayinwardsilo = 0;
                double PerDayintrasilo = 0;
                double PerDayoutsilo = 0;
                double silooutward = 0;
                double closing = 0;
                double intraoutsilo= 0;
                double PerDayintraoutsilo = 0;
                if (ddlfromsilo.SelectedItem.Value != "0")
                {
                    var dates = new List<DateTime>();
                    for (var dt = fromdate; dt <= todate; dt = dt.AddDays(1))
                    {
                        dates.Add(dt);
                    }
                    int i = 0;
                    for (int j = 0; j < dates.Count; j++)
                    { 
                        DateTime dat = Convert.ToDateTime(dates[j].ToString().Trim());
                        DateTime ddate = dat;
                        if(ddate == dat)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = Reportsno++;
                            newrow["Date"] = dat.ToString("MM/dd/yyyy");
                            Report.Rows.Add(newrow);
                        }
                        cmd = new SqlCommand("SELECT   CONVERT(varchar, silo_inward_transaction.date, 101) AS ddate, silo_inward_transaction.qty_kgs AS qty_kgs, silo_inward_transaction.qty_ltrs AS qty_ltrs FROM  silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId WHERE  (silo_inward_transaction.branchid = @branchID) AND (silo_inward_transaction.date BETWEEN @d1 AND @d2) AND (silo_inward_transaction.siloid = @siloid) ");
                        cmd.Parameters.Add("@d1", GetLowDate(dat));
                        cmd.Parameters.Add("@d2", GetHighDate(dat));
                        cmd.Parameters.Add("@BranchID", BranchID);
                        cmd.Parameters.Add("@siloid", ddlfromsilo.SelectedItem.Value);
                        DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
                        if (dtInward.Rows.Count > 0)
                        {
                            int rowcount = 0;
                            string d = dat.ToString("MM/dd/yyyy");
                            DataRow[] result = Report.Select("Date = '" + d + "' ");// Report.Select("SELECT Date=" + d.ToString().Trim() + " ");
                            double qty1 = 0.0;
                            foreach (DataRow row in result)
                            {
                                try
                                {
                                    PerDayinwardsilo = 0;
                                    rowcount = Convert.ToInt32(row["Sno"].ToString());
                                    rowcount = rowcount - 1;
                                    foreach (DataRow dr in dtInward.Rows)
                                    {
                                        try
                                        {
                                            qty1 = Convert.ToDouble(dr["qty_ltrs"].ToString());
                                            Report.Rows[rowcount]["Silo Inward"] = dr["qty_ltrs"].ToString();
                                            PerDayinwardsilo += qty1;
                                            siloinward += qty1;
                                            rowcount++;
                                        }
                                        catch (Exception ex)
                                        {
                                            DataRow newrow = Report.NewRow();
                                            newrow["Sno"] = Reportsno++;
                                            newrow["Silo Inward"] = qty1.ToString();
                                            PerDayinwardsilo += qty1;
                                            siloinward += qty1;
                                            Report.Rows.Add(newrow);
                                            rowcount++;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                break;
                            }
                        }
                        cmd = new SqlCommand("SELECT  intra_silo_transactions.qty_ltrs AS qty_ltrs, intra_silo_transactions.qty_kgs AS qty_kgs, CONVERT(varchar, intra_silo_transactions.doe, 101) AS ddate FROM  intra_silo_transactions INNER JOIN silomaster ON intra_silo_transactions.fromsiloid = silomaster.SiloId WHERE   (intra_silo_transactions.branchid = @BranchID) AND (intra_silo_transactions.tosiloid = @siloid) AND (intra_silo_transactions.doe BETWEEN @d1 AND @d2) ");
                        cmd.Parameters.Add("@d1", GetLowDate(dat));
                        cmd.Parameters.Add("@d2", GetHighDate(dat));
                        cmd.Parameters.Add("@BranchID", BranchID);
                        cmd.Parameters.Add("@siloid", ddlfromsilo.SelectedItem.Value);
                        DataTable dtIntra = SalesDB.SelectQuery(cmd).Tables[0];
                        if (dtIntra.Rows.Count > 0)
                        {
                            int rowcount = 0;
                            string d = dat.ToString("MM/dd/yyyy");
                            DataRow[] result = Report.Select("Date = '" + d + "' ");// Report.Select("SELECT Date=" + d.ToString().Trim() + " ");
                            double qty1 = 0.0;
                            foreach (DataRow row in result)
                            {
                                try
                                {
                                    PerDayintrasilo = 0;
                                    rowcount = Convert.ToInt32(row["Sno"].ToString());
                                    rowcount = rowcount - 1;
                                    foreach (DataRow dr in dtIntra.Rows)
                                    {
                                        try
                                        {                                            
                                            qty1 = Convert.ToDouble(dr["qty_ltrs"].ToString());
                                            Report.Rows[rowcount]["Intra In Silo"] = dr["qty_ltrs"].ToString();
                                            PerDayintrasilo += qty1;
                                            intrasilo += qty1;
                                            rowcount++;
                                        }
                                        catch (Exception ex)
                                        {
                                            DataRow newrow = Report.NewRow();
                                            newrow["Sno"] = Reportsno++;
                                            newrow["Intra In Silo"] = qty1.ToString();
                                            PerDayintrasilo += qty1;
                                            intrasilo += qty1;
                                            Report.Rows.Add(newrow);
                                            rowcount++;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {                                    
                                }
                                break;
                            }
                        }
                        cmd = new SqlCommand("SELECT  intra_silo_transactions.qty_ltrs AS qty_ltrs, intra_silo_transactions.qty_kgs AS qty_kgs, CONVERT(varchar, intra_silo_transactions.doe, 101) AS ddate FROM  intra_silo_transactions INNER JOIN silomaster ON intra_silo_transactions.fromsiloid = silomaster.SiloId WHERE   (intra_silo_transactions.branchid = @BranchID) AND (intra_silo_transactions.fromsiloid = @siloid) AND (intra_silo_transactions.doe BETWEEN @d1 AND @d2) ");
                        cmd.Parameters.Add("@d1", GetLowDate(dat));
                        cmd.Parameters.Add("@d2", GetHighDate(dat));
                        cmd.Parameters.Add("@BranchID", BranchID);
                        cmd.Parameters.Add("@siloid", ddlfromsilo.SelectedItem.Value);
                        DataTable dtIntraOut = SalesDB.SelectQuery(cmd).Tables[0];
                        if (dtIntraOut.Rows.Count > 0)
                        {
                            int rowcount = 0;
                            string d = dat.ToString("MM/dd/yyyy");
                            DataRow[] result = Report.Select("Date = '" + d + "' ");// Report.Select("SELECT Date=" + d.ToString().Trim() + " ");
                            double qty1 = 0.0;
                            foreach (DataRow row in result)
                            {
                                try
                                {
                                    PerDayintraoutsilo = 0;
                                    rowcount = Convert.ToInt32(row["Sno"].ToString());
                                    rowcount = rowcount - 1;
                                    foreach (DataRow dr in dtIntraOut.Rows)
                                    {
                                        try
                                        {
                                            qty1 = Convert.ToDouble(dr["qty_ltrs"].ToString());
                                            Report.Rows[rowcount]["Intra Out Silo"] = dr["qty_ltrs"].ToString();
                                            PerDayintraoutsilo += qty1;
                                            intraoutsilo += qty1;
                                            rowcount++;
                                        }
                                        catch (Exception ex)
                                        {
                                            DataRow newrow = Report.NewRow();
                                            newrow["Sno"] = Reportsno++;
                                            newrow["Intra Out Silo"] = qty1.ToString();
                                            PerDayintraoutsilo += qty1;
                                            intraoutsilo += qty1;
                                            Report.Rows.Add(newrow);
                                            rowcount++;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                break;
                            }
                        }
                        cmd = new SqlCommand("SELECT qty_kgs AS qty_kgs, qty_ltrs AS qty_ltrs, CONVERT(varchar, date, 101) AS ddate FROM   silo_outward_transaction WHERE   (siloid = @siloid) AND (date BETWEEN @d1 AND @d2) AND (branchid = @branchid) ");
                        cmd.Parameters.Add("@d1", GetLowDate(dat));
                        cmd.Parameters.Add("@d2", GetHighDate(dat));
                        cmd.Parameters.Add("@BranchID", BranchID);
                        cmd.Parameters.Add("@siloid", ddlfromsilo.SelectedItem.Value);
                        DataTable dtoutward = SalesDB.SelectQuery(cmd).Tables[0];
                        if (dtoutward.Rows.Count > 0)
                        {
                            int rowcount = 0;
                            string d = dat.ToString("MM/dd/yyyy");
                            DataRow[] result = Report.Select("Date = '" + d + "' ");// Report.Select("SELECT Date=" + d.ToString().Trim() + " ");
                            double qty1 = 0.0;
                            foreach (DataRow row in result)
                            {
                                try
                                {
                                    PerDayoutsilo = 0;
                                    rowcount = Convert.ToInt32(row["Sno"].ToString());
                                    rowcount = rowcount - 1;
                                    foreach (DataRow dr in dtoutward.Rows)
                                    {
                                        try
                                        {
                                            qty1 = Convert.ToDouble(dr["qty_ltrs"].ToString());
                                            Report.Rows[rowcount]["Silo OutWard"] = dr["qty_ltrs"].ToString();
                                            PerDayoutsilo += qty1;
                                            silooutward += qty1;
                                            rowcount++;
                                        }
                                        catch (Exception ex)
                                        {
                                            DataRow newrow = Report.NewRow();
                                            newrow["Sno"] = Reportsno++;
                                            newrow["Silo OutWard"] = qty1.ToString();
                                            PerDayoutsilo += qty1;
                                            silooutward += qty1;
                                            Report.Rows.Add(newrow);
                                            rowcount++;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                DataRow newrow1 = Report.NewRow();
                                newrow1["Sno"] = Reportsno++;
                                newrow1["Date"] = "Total";
                                newrow1["Silo Inward"] = PerDayinwardsilo.ToString();
                                newrow1["Intra In Silo"] = PerDayintrasilo.ToString();
                                newrow1["Intra Out Silo"] = PerDayintraoutsilo.ToString();
                                newrow1["Silo OutWard"] = PerDayoutsilo.ToString();
                                Report.Rows.Add(newrow1);
                                PerDayinwardsilo = 0;
                                PerDayintrasilo = 0;
                                PerDayintraoutsilo = 0;
                                PerDayoutsilo = 0;
                                break;
                            }
                        }
                        else
                        {
                            DataRow newrow1 = Report.NewRow();
                            newrow1["Sno"] = Reportsno++;
                            newrow1["Date"] = "Total";
                            newrow1["Silo Inward"] = PerDayinwardsilo.ToString();
                            newrow1["Intra In Silo"] = PerDayintrasilo.ToString();
                            newrow1["Intra Out Silo"] = PerDayintraoutsilo.ToString();
                            newrow1["Silo OutWard"] = PerDayoutsilo.ToString();
                            Report.Rows.Add(newrow1);
                            PerDayinwardsilo = 0;
                            PerDayintrasilo = 0;
                            PerDayintraoutsilo = 0;
                            PerDayoutsilo = 0;
                        }
                    }
                }
                DataRow newvartical4 = Report.NewRow();
                newvartical4["Date"] = "Grand Total";
                newvartical4["Silo Inward"] = siloinward;
                newvartical4["Intra In Silo"] = intrasilo;
                newvartical4["Intra Out Silo"] = intraoutsilo;
                newvartical4["Silo OutWard"] = silooutward;
                Report.Rows.Add(newvartical4);

                grdReports.DataSource = Report;
                grdReports.DataBind();
                hidepanel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "Grand Total")
            {
                e.Row.BackColor = System.Drawing.Color.Green;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }
}