using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Inward_Silo_Report : System.Web.UI.Page
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
            Report.Columns.Add("Sno");
            Report.Columns.Add("DATE");
            Report.Columns.Add("Vendor Name");
            //Report.Columns.Add("DCNo");
            Report.Columns.Add("SILO NAME");
            Report.Columns.Add("KGS").DataType = typeof(Double);
            Report.Columns.Add("LTRS").DataType = typeof(Double);
            Report.Columns.Add("FAT");
            Report.Columns.Add("SNF");
            Report.Columns.Add("CLR");
            Report.Columns.Add("LTR FAT");
            Report.Columns.Add("LTR SNF");
            //cmd = new SqlCommand("SELECT silo_inward_transaction.sno, v.vendorname, silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs,  silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName FROM  silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId INNER JOIN vendors AS v ON v.sno = silo_inward_transaction.ccid WHERE (silo_inward_transaction.branchid = @branchID) AND (silo_inward_transaction.date BETWEEN @d1 AND @d2) ORDER BY silo_inward_transaction.date");
            cmd = new SqlCommand("SELECT  silo_inward_transaction.sno, v.vendorname, silo_inward_transaction.date, silo_inward_transaction.dcno, silo_inward_transaction.qty_kgs,  silo_inward_transaction.qty_ltrs, silo_inward_transaction.fat, silo_inward_transaction.snf, silo_inward_transaction.clr, silomaster.SiloName,  processingdepartments.departmentname FROM  silo_inward_transaction INNER JOIN silomaster ON silo_inward_transaction.siloid = silomaster.SiloId LEFT OUTER JOIN vendors AS v ON v.sno = silo_inward_transaction.ccid LEFT OUTER JOIN processingdepartments ON silo_inward_transaction.deptid = processingdepartments.departmentid WHERE  (silo_inward_transaction.branchid = @branchID) AND (silo_inward_transaction.date BETWEEN @d1 AND @d2) ORDER BY silo_inward_transaction.date");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtInward.Rows.Count > 0)
            {
                int i = 1;
                double gkgfattotal = 0;
                double gkgsnftotal = 0;
                double gkgstotal = 0;
                double gLtrstotal = 0;
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double Ltrstotal = 0;
                DateTime dt = DateTime.Now;
                string prevdate = string.Empty;
                foreach (DataRow dr in dtInward.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["date"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    string currentdate = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    if (prevdate == currentdate)
                    {
                        string vendorname = dr["vendorname"].ToString();
                        if (vendorname == "")
                        {
                            vendorname = dr["departmentname"].ToString();
                            if (vendorname=="")
                            {
                                vendorname = "Others";
                            }
                        }
                        else
                        {
                            vendorname = dr["vendorname"].ToString();
                        }
                        newrow["Vendor Name"] = vendorname;
                        //newrow["DCNo"] = dr["dcno"].ToString();
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
                        gkgfattotal += ltrfat;
                        double ltrsnf = 0;
                        ltrsnf = ltrs * snf;
                        ltrsnf = Math.Round(ltrsnf / 100, 2);
                        newrow["LTR SNF"] = ltrsnf;
                        kgsnftotal += ltrsnf;
                        gkgsnftotal += ltrsnf;
                        Report.Rows.Add(newrow);
                    }
                    else
                    {
                        if (kgstotal > 0)
                        {
                            DataRow newvartical2 = Report.NewRow();
                            newvartical2["Vendor Name"] = "Total";
                            newvartical2["KGS"] = kgstotal;
                            newvartical2["LTRS"] = Ltrstotal;
                            newvartical2["LTR FAT"] = kgfattotal;
                            newvartical2["LTR SNF"] = kgsnftotal;
                            double tfattotal = 0;
                            tfattotal = (kgfattotal / Ltrstotal) * 100;
                            tfattotal = Math.Round(tfattotal, 2);
                            newvartical2["FAT"] = tfattotal;
                            double tsnftotal = 0;
                            tsnftotal = (kgsnftotal / Ltrstotal) * 100;
                            tsnftotal = Math.Round(tsnftotal, 2);
                            newvartical2["SNF"] = tsnftotal;
                            Report.Rows.Add(newvartical2);
                            kgfattotal = 0;
                            kgsnftotal = 0;
                            kgstotal = 0;
                            Ltrstotal = 0;
                        }
                        prevdate = currentdate;
                        string vname = dr["vendorname"].ToString();
                        if (vname == "")
                        {
                            vname = dr["departmentname"].ToString();
                            if (vname=="")
                            {
                                vname = "Others";
                            }
                        }
                        else
                        {
                            vname = dr["vendorname"].ToString();
                        }
                        newrow["Vendor Name"] = vname;
                        //newrow["DCNo"] = dr["dcno"].ToString();
                        double Kg = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out Kg);
                        newrow["KGS"] = Kg;
                        kgstotal += Kg;
                        gkgstotal += Kg;
                        double ltr = 0;
                        double.TryParse(dr["qty_ltrs"].ToString(), out ltr);
                        Ltrstotal += ltr;
                        gLtrstotal += ltr;
                        newrow["LTRS"] = ltr;
                        double sfat = 0;
                        double.TryParse(dr["fat"].ToString(), out sfat);
                        newrow["FAT"] = sfat;
                        double ssnf = 0;
                        double.TryParse(dr["snf"].ToString(), out ssnf);
                        newrow["SNF"] = ssnf;
                        newrow["CLR"] = dr["clr"].ToString();
                        newrow["SILO NAME"] = dr["SiloName"].ToString();
                        double sltrfat = 0;
                        sltrfat = ltr * sfat;
                        sltrfat = Math.Round(sltrfat / 100, 2);
                        newrow["LTR FAT"] = sltrfat;
                        kgfattotal += sltrfat;
                        gkgfattotal += sltrfat;
                        double sltrsnf = 0;
                        sltrsnf = ltr * ssnf;
                        sltrsnf = Math.Round(sltrsnf / 100, 2);
                        newrow["LTR SNF"] = sltrsnf;
                        kgsnftotal += sltrsnf;
                        gkgsnftotal += sltrsnf;
                        Report.Rows.Add(newrow);
                    }
                }
                DataRow newvartical1 = Report.NewRow();
                newvartical1["Vendor Name"] = "Total";
                newvartical1["KGS"] = kgstotal;
                newvartical1["LTRS"] = Ltrstotal;
                double fattotal = 0;
                fattotal = (kgfattotal / Ltrstotal) * 100;
                fattotal = Math.Round(fattotal, 2);
                newvartical1["FAT"] = fattotal;
                double snftotal = 0;
                snftotal = (kgsnftotal / Ltrstotal) * 100;
                snftotal = Math.Round(snftotal, 2);
                newvartical1["SNF"] = snftotal;
                newvartical1["LTR FAT"] = kgfattotal;
                newvartical1["LTR SNF"] = kgsnftotal;
                Report.Rows.Add(newvartical1);
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
                hidepanel.Visible = true;

                DataRow newvartical3 = Report.NewRow();
                newvartical3["Vendor Name"] = "Grand Total";
                newvartical3["KGS"] = gkgstotal;
                newvartical3["LTRS"] = gLtrstotal;
                double gfattotal = 0;
                gfattotal = (gkgfattotal / gLtrstotal) * 100;
                gfattotal = Math.Round(gfattotal, 2);
                newvartical3["FAT"] = gfattotal;
                double gsnftotal = 0;
                gsnftotal = (gkgsnftotal / gLtrstotal) * 100;
                gsnftotal = Math.Round(gsnftotal, 2);
                newvartical3["SNF"] = gsnftotal;
                newvartical3["LTR FAT"] = gkgfattotal;
                newvartical3["LTR SNF"] = gkgsnftotal;
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
            if (e.Row.Cells[2].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[2].Text == "Grand Total")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
    }

}