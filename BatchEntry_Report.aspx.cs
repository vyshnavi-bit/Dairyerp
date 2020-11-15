using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class BatchEntry_Report : System.Web.UI.Page
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
             cmd = new SqlCommand("SELECT batchid, batch, batchcode, branchid, createdby FROM batchmaster ");
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
            Report.Columns.Add("Sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("TIME");
            Report.Columns.Add("Batch Name");
            Report.Columns.Add("Batch Opening qty");
            Report.Columns.Add("To Silo");
            Report.Columns.Add("Source");
            Report.Columns.Add("Type");
            Report.Columns.Add("Time");
            Report.Columns.Add("KGS").DataType = typeof(Double);
            Report.Columns.Add("LTRS").DataType = typeof(Double);
            Report.Columns.Add("CV");
            Report.Columns.Add("FAT");
            Report.Columns.Add("SNF");
            Report.Columns.Add("CLR");
            Report.Columns.Add("LTR FAT");
            Report.Columns.Add("LTR SNF");
            Report.Columns.Add("SMP");
            if (ddlBatchType.SelectedValue == "Batch Wise")
            {
                lblbatchname.Text = "Batch Name   " + ddlbatches.SelectedItem.Text;
                cmd = new SqlCommand("SELECT silomaster.SiloName AS FromSilo,silomaster_1.SiloName AS ToSIlo,batchentrydetails.type, batchentrydetails.tosiloid, batchentrydetails.smp, batchentrydetails.qty_kgs, batchentrydetails.qty_ltrs, batchentrydetails.fat, batchentrydetails.snf, batchentrydetails.doe, batchentrydetails.type, batchentrydetails.clr, batchentrydetails.siloqty, vendors.vendorname, batchmaster.batch,processingdepartments.departmentname FROM silomaster AS silomaster_1 INNER JOIN batchentrydetails ON silomaster_1.SiloId = batchentrydetails.tosiloid INNER JOIN batchmaster ON batchentrydetails.batchid = batchmaster.batchid LEFT OUTER JOIN processingdepartments ON batchentrydetails.fromdeptid = processingdepartments.departmentid LEFT OUTER JOIN vendors ON batchentrydetails.fromccid = vendors.sno LEFT OUTER JOIN silomaster ON batchentrydetails.fromsiloid = silomaster.SiloId  WHERE (batchentrydetails.doe BETWEEN @d1 AND @d2)  AND (batchmaster.batchid=@BatchID) AND (batchentrydetails.branchid = @branchid) ORDER BY batchentrydetails.doe"); 
                cmd.Parameters.Add("@BatchID", ddlbatches.SelectedValue);
                cmd.Parameters.Add("@branchid", branchid);

            }
            else
            {
                cmd = new SqlCommand("SELECT silomaster.SiloName AS FromSilo, silomaster_1.SiloName AS ToSIlo,batchentrydetails.type, batchentrydetails.tosiloid, batchentrydetails.smp, batchentrydetails.qty_kgs, batchentrydetails.qty_ltrs,batchentrydetails.fat, batchentrydetails.snf, batchentrydetails.doe, batchentrydetails.type, batchentrydetails.clr, batchentrydetails.siloqty, vendors.vendorname, batchmaster.batch,processingdepartments.departmentname FROM silomaster AS silomaster_1 INNER JOIN batchentrydetails ON silomaster_1.SiloId = batchentrydetails.tosiloid INNER JOIN batchmaster ON batchentrydetails.batchid = batchmaster.batchid LEFT OUTER JOIN processingdepartments ON batchentrydetails.fromdeptid = processingdepartments.departmentid LEFT OUTER JOIN vendors ON batchentrydetails.fromccid = vendors.sno LEFT OUTER JOIN silomaster ON batchentrydetails.fromsiloid = silomaster.SiloId  WHERE (batchentrydetails.doe BETWEEN @d1 AND @d2) AND (batchentrydetails.branchid = @branchid) ORDER BY batchentrydetails.doe");
                cmd.Parameters.Add("@branchid", branchid);
                // cmd = new SqlCommand("SELECT SUM(batchentrydetails.qty_kgs) AS qtykgs, SUM(batchentrydetails.qty_ltrs) AS qtyltrs, SUM(batchentrydetails.fat) AS fat, SUM(batchentrydetails.snf) AS snf,  SUM(batchentrydetails.clr) AS clr, batchmaster.batch FROM batchentrydetails INNER JOIN   batchmaster ON batchmaster.batchid = batchentrydetails.batchid WHERE (batchentrydetails.doe BETWEEN @d1 AND @d2) GROUP BY batchmaster.batch");
            }
            
            cmd.Parameters.Add("@d1", dtfromdate);
            cmd.Parameters.Add("@d2", dttodate);
            DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];

           
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
                    string siloid = dr["tosiloid"].ToString();
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
                        newrow["Time"] = PlanDateTime[1];
                        newrow["Type"] = dr["type"].ToString();
                        string source = dr["FromSilo"].ToString();
                        if (source == "")
                        {
                            source = dr["vendorname"].ToString();
                            if (source == "")
                            {
                                source = dr["departmentname"].ToString();
                            }
                        }
                        newrow["Source"] = source;
                        newrow["Batch Name"] = dr["batch"].ToString();
                        newrow["To Silo"] = dr["ToSIlo"].ToString();
                        double bopening = 0;
                        //double.TryParse(dr["siloqty"].ToString(), out bopening);
                        newrow["Batch Opening qty"] = bopening;
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
                        newrow["CV"] = ltrs + bopening;
                        closingvalue = ltrs + bopening;
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
                        gkgfattotal += ltrfat;
                        double ltrsnf = 0;
                        ltrsnf = ltrs * snf;
                        ltrsnf = Math.Round(ltrsnf / 100, 2);
                        newrow["LTR SNF"] = ltrsnf;
                        double smp = 0;
                        double.TryParse(dr["smp"].ToString(), out smp);
                        TOTsmp += smp;
                        gTOTsmp += smp;
                        smp = smp * 0.97;
                        newrow["SMP"] = smp;
                        smptotal += smp;
                        gsmptotal += smp;
                        kgsnftotal += ltrsnf;
                        gkgsnftotal += ltrsnf;
                        Report.Rows.Add(newrow);
                       // prevdate = currentdate;
                    }
                    else
                    {
                        if (kgstotal > 0)
                        {
                            DataRow newvartical2 = Report.NewRow();
                            newvartical2["To Silo"] = "Total";
                            //newvartical2["Batch Opening qty"] = bopeningtotal;

                            string val = hiddenvalue;
                            double siloobvalue = Convert.ToDouble(val); 
                            double fattotal = 0;
                            fattotal = (kgfattotal / Ltrstotal) * 100;
                            fattotal = Math.Round(fattotal, 2);
                            newvartical2["FAT"] = fattotal;
                            double snftotal = 0;
                            double smpkgsnftotal = 0;
                            smpkgsnftotal = smptotal + kgsnftotal;
                            snftotal = (smpkgsnftotal / Ltrstotal) * 100;
                            snftotal = Math.Round(snftotal, 2);
                            newvartical2["SNF"] = snftotal;
                            //newvartical2["LTR FAT"] = kgfattotal + kgclosingfattotal;
                            newvartical2["LTR FAT"] = kgfattotal;
                            newvartical2["LTR SNF"] = smpkgsnftotal;
                            newvartical2["SMP"] = smptotal;
                            if (ddlBatchType.SelectedValue == "Batch Wise")
                            {
                               // newvartical2["CV"] = closingvalue;
                                newvartical2["LTRS"] = Ltrstotal + siloobvalue;
                            }
                            else
                            {
                                newvartical2["KGS"] = kgstotal;
                                // newvartical2["CV"] = kgstotal + bopeningtotal;
                                newvartical2["LTRS"] = Ltrstotal;
                            }
                            Report.Rows.Add(newvartical2);
                            kgfattotal = 0;
                            kgsnftotal = 0;
                            kgstotal = 0;
                            Ltrstotal = 0;
                            smptotal = 0;
                            TOTsmp = 0;
                            bopeningtotal = 0;
                            hiddenvalue = string.Empty;
                        }
                        sessiondate = sessiondate.AddDays(1);
                        sessiondate = sessiondate.AddHours(6);
                        sessiondatetime = sessiondate.ToString();
                        prevdate = currentdate;
                        if (sj <= 1)
                        {
                            double siloobqty;
                            double.TryParse(dr["siloqty"].ToString(), out siloobqty);
                            cmd = new SqlCommand("SELECT fat, snf from silowiseclosingdetails where siloid=@siloid and closingdate BETWEEN @date1 AND @date2");
                            cmd.Parameters.Add("@siloid", siloid);
                            cmd.Parameters.Add("@date1", GetLowDate(dtdoe));
                            cmd.Parameters.Add("@date2", GetHighDate(dtdoe));
                            DataTable dtsiloclosingfatsnf = SalesDB.SelectQuery(cmd).Tables[0];
                            if (dtsiloclosingfatsnf.Rows.Count > 0)
                            {
                                foreach (DataRow drsiloclosing in dtsiloclosingfatsnf.Rows)
                                {
                                    double siloclosingfat = 0;
                                    double.TryParse(drsiloclosing["fat"].ToString(), out siloclosingfat);
                                    double siloclosingsnf = 0;
                                    double.TryParse(drsiloclosing["snf"].ToString(), out siloclosingsnf);
                                    double ltrclosingfat = 0;
                                    ltrclosingfat = siloobqty * siloclosingfat;
                                    ltrclosingfat = Math.Round(ltrclosingfat / 100, 2);
                                    kgclosingfattotal += ltrclosingfat;
                                    double ltrclosingsnf = 0;
                                    ltrclosingsnf = siloobqty * siloclosingsnf;
                                    ltrclosingsnf = Math.Round(ltrclosingsnf / 100, 2);
                                    kgclosingsnftotal += ltrclosingsnf;
                                }
                                sj++;
                            }
                        }

                        //DateTime siloclosingdate =  GetLowDate(dtdoe);
                        //DateTime siloclosingdate1 = GetHighDate(dtdoe);
                        //cmd = new SqlCommand("SELECT  sno, siloid, qty_kgs, fat, snf, clr, closingdate, branchid, enteredby FROM  silowiseclosingdetails WHERE closingdate BETWEEN @scd1 AND @scd2");
                        //cmd.Parameters.Add("@scd1", siloclosingdate);
                        //cmd.Parameters.Add("@scd1", siloclosingdate);
                        //DataTable dtsiloclosing = SalesDB.SelectQuery(cmd).Tables[0];
                        double bopening = 0;
                        double.TryParse(dr["siloqty"].ToString(), out bopening);
                        //foreach (DataRow drclosing in dtsiloclosing.Select("siloid='" + dr["tosiloid"].ToString() + "'"))
                        //{
                            
                        //}
                        string strPlantime = dtdoe.ToString("dd/MMM/yyyy HH:mm");
                        string[] PlanDateTime = strPlantime.Split(' ');
                        newrow["Date"] = PlanDateTime[0];
                        newrow["Time"] = PlanDateTime[1];
                        newrow["Type"] = dr["type"].ToString();
                        string source = dr["FromSilo"].ToString();
                        if (source == "")
                        {
                            source = dr["vendorname"].ToString();
                            if (source == "")
                            {
                                source = dr["departmentname"].ToString();
                            }
                        }
                        newrow["Source"] = source;
                        newrow["Batch Name"] = dr["batch"].ToString();
                        newrow["To Silo"] = dr["ToSIlo"].ToString();
                       // double bopening = 0;
                       // double.TryParse(dr["siloqty"].ToString(), out bopening);
                        if (hiddenvalue == "")
                        {
                            hiddenvalue = bopening.ToString();
                        }
                        newrow["Batch Opening qty"] = bopening;
                        bopeningtotal += bopening;
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
                        newrow["CV"] = ltrs + bopening;
                        closingvalue = ltrs + bopening;
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
                        gkgfattotal += ltrfat;
                        double ltrsnf = 0;
                        ltrsnf = ltrs * snf;
                        ltrsnf = Math.Round(ltrsnf / 100, 2);
                        newrow["LTR SNF"] = ltrsnf;
                        double smp = 0;
                        double.TryParse(dr["smp"].ToString(), out smp);
                        TOTsmp += smp;
                        gTOTsmp += smp;
                        smp = smp * 0.97;
                        newrow["SMP"] = smp;
                        smptotal += smp;
                        gsmptotal += smp;
                        kgsnftotal += ltrsnf;
                        gkgsnftotal += ltrsnf;
                        Report.Rows.Add(newrow);
                    }
                    
                }
                //DataRow New1 = Report.NewRow();
                //New1["To Silo"] = "SMP";
                //New1["KGS"] = TOTsmp;
                //New1["LTR SNF"] = smptotal;
                //Report.Rows.Add(New1);

                DataRow newvartical1 = Report.NewRow();
                newvartical1["Type"] = "Total";
                //newvartical1["KGS"] = kgstotal;
                //newvartical1["LTRS"] = Ltrstotal;
                double mfattotal = 0;
                mfattotal = (kgfattotal / Ltrstotal) * 100;
                mfattotal = Math.Round(mfattotal, 2);
                newvartical1["FAT"] = mfattotal;
                double msnftotal = 0;
                double msmpkgsnftotal = 0;
                msmpkgsnftotal = smptotal + kgsnftotal;
                msnftotal = (msmpkgsnftotal / Ltrstotal) * 100;
                msnftotal = Math.Round(msnftotal, 2);
                newvartical1["SNF"] = msnftotal;
                newvartical1["LTR FAT"] = kgfattotal;
                newvartical1["LTR SNF"] = msmpkgsnftotal;
                newvartical1["SMP"] = smptotal;
                newvartical1["CV"] = closingvalue;

                if (ddlBatchType.SelectedValue == "Batch Wise")
                {
                    newvartical1["CV"] = closingvalue;
                }
                else
                {
                    newvartical1["KGS"] = kgstotal;
                    // newvartical2["CV"] = kgstotal + bopeningtotal;
                    newvartical1["LTRS"] = Ltrstotal;
                }
                Report.Rows.Add(newvartical1);

                DataRow newvartical3 = Report.NewRow();
                newvartical3["Type"] = "Grand Total";
                newvartical3["KGS"] = gkgstotal;
                newvartical3["LTRS"] = gLtrstotal;
                double gfattotal = 0;
                gfattotal = (gkgfattotal / gLtrstotal) * 100;
                gfattotal = Math.Round(gfattotal, 2);
                newvartical3["FAT"] = gfattotal;
                double gsnftotal = 0;
                double gsmpkgsnftotal = 0;
                gsmpkgsnftotal = gsmptotal + gkgsnftotal;
                gsnftotal = (gsmpkgsnftotal / gLtrstotal) * 100;
                gsnftotal = Math.Round(gsnftotal, 2);
                newvartical3["SNF"] = gsnftotal;
                newvartical3["LTR FAT"] = gkgfattotal;
                newvartical3["LTR SNF"] = gsmpkgsnftotal;
                newvartical3["SMP"] = gsmptotal;
                Report.Rows.Add(newvartical3);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }

    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[5].Text == "Total")
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
        }
    }

    protected void lnkView_Click(object sender, EventArgs e)
    {

    }
    
}