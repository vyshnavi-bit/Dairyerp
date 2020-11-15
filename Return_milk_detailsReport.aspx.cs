using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Return_milk_detailsReport : System.Web.UI.Page
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
                    binddepartments();
                }
            }
        }
    }

    public void binddepartments()
    {
        SalesDBManager SalesDB = new SalesDBManager();
        cmd = new SqlCommand("SELECT departmentid, departmentname, quantity, branchid FROM processingdepartments ");
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlsection.DataSource = dttrips;
        ddlsection.DataTextField = "departmentname";
        ddlsection.DataValueField = "departmentid";
        ddlsection.DataBind();
        ddlsection.ClearSelection();
        ddlsection.Items.Insert(0, new ListItem { Value = "0", Text = "Select Section", Selected = true });
        ddlsection.SelectedValue = "0";
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
            Report.Columns.Add("DEPT NAME");
            Report.Columns.Add("SILO NAME");
            Report.Columns.Add("KGS").DataType = typeof(Double);
            Report.Columns.Add("LTRS").DataType = typeof(Double);
            Report.Columns.Add("FAT");
            Report.Columns.Add("SNF");
            Report.Columns.Add("CLR");
            Report.Columns.Add("LTR FAT");
            Report.Columns.Add("LTR SNF");
            if (ddlsection.SelectedItem.Value != "0")
            {
                cmd = new SqlCommand("SELECT returnmilk_details.doe, processingdepartments.departmentname, silomaster.SiloName, returnmilk_details.quantity,returnmilk_details.fat,returnmilk_details.snf,returnmilk_details.clr,returnmilk_details.qty_ltr FROM returnmilk_details INNER JOIN processingdepartments ON returnmilk_details.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON returnmilk_details.siloid = silomaster.SiloId WHERE (returnmilk_details.branchid = @BranchID) AND (returnmilk_details.doe BETWEEN @d1 AND @d2) AND (returnmilk_details.departmentid=@sectionid) ORDER BY returnmilk_details.doe");
                cmd.Parameters.Add("@sectionid", ddlsection.SelectedItem.Value);
            }
            else
            {
                cmd = new SqlCommand("SELECT returnmilk_details.doe, processingdepartments.departmentname, silomaster.SiloName, returnmilk_details.quantity,returnmilk_details.fat,returnmilk_details.snf,returnmilk_details.clr,returnmilk_details.qty_ltr FROM returnmilk_details INNER JOIN processingdepartments ON returnmilk_details.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON returnmilk_details.siloid = silomaster.SiloId WHERE (returnmilk_details.branchid = @BranchID) AND (returnmilk_details.doe BETWEEN @d1 AND @d2) ORDER BY returnmilk_details.doe");
            }
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
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    string currentdate = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    if (prevdate == currentdate)
                    {
                        newrow["DEPT NAME"] = dr["departmentname"].ToString();
                        newrow["SILO NAME"] = dr["SiloName"].ToString();
                        double Kgs = 0;
                        double.TryParse(dr["quantity"].ToString(), out Kgs);
                        newrow["KGS"] = Kgs;
                        kgstotal += Kgs;
                        gkgstotal += Kgs;
                        double ltrs = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
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
                            newvartical2["SILO NAME"] = "Total";
                            newvartical2["KGS"] = kgstotal;
                            newvartical2["LTRS"] = Ltrstotal;
                            double fattotal = 0;
                            fattotal = (kgfattotal / Ltrstotal) * 100;
                            fattotal = Math.Round(fattotal, 2);
                            newvartical2["FAT"] = fattotal;
                            double snftotal = 0;
                            snftotal = (kgsnftotal / Ltrstotal) * 100;
                            snftotal = Math.Round(snftotal, 2);
                            newvartical2["SNF"] = snftotal;
                            newvartical2["LTR FAT"] = kgfattotal;
                            newvartical2["LTR SNF"] = kgsnftotal;
                            Report.Rows.Add(newvartical2);
                            kgfattotal = 0;
                            kgsnftotal = 0;
                            kgstotal = 0;
                            Ltrstotal = 0;
                        }
                        prevdate = currentdate;
                        newrow["DEPT NAME"] = dr["departmentname"].ToString();
                        newrow["SILO NAME"] = dr["SiloName"].ToString();
                        double Kgs = 0;
                        double.TryParse(dr["quantity"].ToString(), out Kgs);
                        newrow["KGS"] = Kgs;
                        kgstotal += Kgs;
                        gkgstotal += Kgs;
                        double ltrs = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
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
                }
                DataRow newvartical3 = Report.NewRow();
                newvartical3["SILO NAME"] = "Total";
                newvartical3["KGS"] = kgstotal;
                newvartical3["LTRS"] = Ltrstotal;
                double tfattotal = 0;
                tfattotal = (kgfattotal / Ltrstotal) * 100;
                tfattotal = Math.Round(tfattotal, 2);
                newvartical3["FAT"] = tfattotal;
                double tsnftotal = 0;
                tsnftotal = (kgsnftotal / Ltrstotal) * 100;
                tsnftotal = Math.Round(tsnftotal, 2);
                newvartical3["SNF"] = tsnftotal;
                newvartical3["LTR FAT"] = kgfattotal;
                newvartical3["LTR SNF"] = kgsnftotal;
                Report.Rows.Add(newvartical3);

                DataRow newvartical4 = Report.NewRow();
                newvartical4["SILO NAME"] = "Grand Total";
                newvartical4["KGS"] = gkgstotal;
                newvartical4["LTRS"] = gLtrstotal;
                double gfattotal = 0;
                gfattotal = (gkgfattotal / gLtrstotal) * 100;
                gfattotal = Math.Round(gfattotal, 2);
                newvartical4["FAT"] = gfattotal;
                double gsnftotal = 0;
                gsnftotal = (gkgsnftotal / gLtrstotal) * 100;
                gsnftotal = Math.Round(gsnftotal, 2);
                newvartical4["SNF"] = gsnftotal;
                newvartical4["LTR FAT"] = gkgfattotal;
                newvartical4["LTR SNF"] = gkgsnftotal;
                Report.Rows.Add(newvartical4);
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
        }
    }
}