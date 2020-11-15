using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class diffreport : System.Web.UI.Page
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
            Session["filename"] = "Batch Entry";
            Session["title"] = "Batch Entry Details";
            DateTime dtfrom = GetLowDate(fromdate);
            DateTime dtfromdate = dtfrom.AddHours(6);
            DateTime dtto = GetLowDate(todate);
            DateTime dttodate = dtto.AddHours(30);
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            
            Report.Columns.Add("Sno");
            Report.Columns.Add("DATE");
            Report.Columns.Add("CC Name");
            Report.Columns.Add("Dispatch Quantity_Kgs");
            Report.Columns.Add("Inword Quantity_Kgs");
            Report.Columns.Add("Diff Quantity(kgs)");
            Report.Columns.Add("Dispatch FAT");
            Report.Columns.Add("Inword FAT");
            Report.Columns.Add("Diff FAT");
            Report.Columns.Add("Dispatch SNF");
            Report.Columns.Add("Inword SNF");
            Report.Columns.Add("Diff SNF");
            cmd = new SqlCommand("SELECT despatch_entry.sno, vendors.vendorname, despatch_entry.doe, despatch_sub.fat, despatch_sub.cellname, despatch_sub.qty_ltr, despatch_sub.qty_kgs, despatch_sub.snf, milktransactions.qty_ltr AS inwardqtyltr,  milktransactions.qty_kgs AS inwardqtykgs, milktransactions.cellno, milktransactions.fat AS inwardfat, milktransactions.snf AS inwardsnf FROM   despatch_entry INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno INNER JOIN milktransactions ON despatch_entry.sno = milktransactions.dcno INNER JOIN vendors on despatch_entry.cc_id=vendors.sno WHERE (despatch_entry.doe between @d1 and @d2) ORDER BY despatch_entry.doe");
            cmd.Parameters.Add("@d1", dtfromdate);
            cmd.Parameters.Add("@d2", dttodate);
            DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtInward.Rows.Count > 0)
            {

                int i = 1;
                double qty_kgs = 0;
                double dispatchtotalqtykgs = 0;
                double FAT = 0;
                double SNF = 0;
                double inword_qty_kgs = 0;
                double inwordtotalqtykgs = 0;
                double inword_FAT = 0;
                //grand total
                double inword_snf = 0;
                double totalfat = 0;
                double totalsnf = 0;
                double totalinfat = 0;
                double totalinsnf = 0;
                DateTime dt = DateTime.Now;
                string prevdate = string.Empty;
                foreach (DataRow dr in dtInward.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++.ToString();
                    string sno = dr["sno"].ToString();

                    if (prevdate == sno)
                    {
                        string celltype = dr["cellname"].ToString();
                         string cellno = dr["cellno"].ToString();
                         if (celltype == cellno)
                         {
                             DateTime doe = Convert.ToDateTime(dr["doe"].ToString());
                             string dtt = doe.ToString("dd/MMM/yyyy");
                             newrow["DATE"] = dtt;
                             newrow["CC Name"] = dr["vendorname"].ToString();

                             double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                             dispatchtotalqtykgs += qty_kgs;
                             newrow["Dispatch Quantity_Kgs"] = qty_kgs;

                             double.TryParse(dr["fat"].ToString(), out FAT);
                             totalfat += FAT;
                             FAT = Math.Round(FAT, 2);
                             newrow["Dispatch FAT"] = FAT;

                             double.TryParse(dr["snf"].ToString(), out SNF);
                             newrow["Dispatch SNF"] = SNF;
                             totalsnf += SNF;

                             double.TryParse(dr["inwardqtykgs"].ToString(), out inword_qty_kgs);
                             inwordtotalqtykgs += inword_qty_kgs;
                             newrow["Inword Quantity_Kgs"] = inword_qty_kgs;

                             double.TryParse(dr["inwardfat"].ToString(), out inword_FAT);
                             newrow["Inword FAT"] = dr["inwardfat"].ToString();
                             totalinfat += inword_FAT;

                             double.TryParse(dr["inwardsnf"].ToString(), out inword_snf);
                             newrow["Inword SNF"] = dr["inwardsnf"].ToString();
                             totalinsnf += inword_snf;

                             Report.Rows.Add(newrow);
                         }
                        // prevdate = currentdate;
                    }
                    else
                    {
                        if (dispatchtotalqtykgs > 0)
                        {
                            DataRow newvartical2 = Report.NewRow();
                            newvartical2["CC Name"] = "Total";
                            //newvartical2["Batch Opening qty"] = bopeningtotal;
                            newvartical2["Dispatch Quantity_Kgs"] = dispatchtotalqtykgs;
                            // newvartical2["CV"] = kgstotal + bopeningtotal;
                            newvartical2["Inword Quantity_Kgs"] = inwordtotalqtykgs;
                            double total = 0;
                            total = (dispatchtotalqtykgs - inwordtotalqtykgs);
                            newvartical2["Diff Quantity(kgs)"] = total;

                            newvartical2["Dispatch FAT"] = totalfat;
                            newvartical2["Dispatch SNF"] = totalsnf;
                            newvartical2["Inword FAT"] = totalinfat;
                            newvartical2["Inword SNF"] = totalinsnf;

                            double snftotal = totalsnf - totalinsnf;
                            newvartical2["Diff SNF"] = Math.Round(snftotal, 2);
                            double fattotal = totalfat - totalinfat;
                            newvartical2["Diff FAT"] = Math.Round(fattotal, 2);
                            Report.Rows.Add(newvartical2);
                            totalsnf = 0;
                            totalinsnf = 0;
                            totalfat = 0;
                            totalinfat = 0;
                            inwordtotalqtykgs = 0;
                            dispatchtotalqtykgs = 0;
                        }
                        prevdate = sno;
                        string celltype = dr["cellname"].ToString();
                         string cellno = dr["cellno"].ToString();
                         if (celltype == cellno)
                         {
                             DateTime doe = Convert.ToDateTime(dr["doe"].ToString());
                             string dtt = doe.ToString("dd/MMM/yyyy");
                             newrow["DATE"] = dtt;
                             newrow["Sno"] = dr["sno"].ToString();
                             newrow["CC Name"] = dr["vendorname"].ToString();
                             double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                             dispatchtotalqtykgs += qty_kgs;
                             newrow["Dispatch Quantity_Kgs"] = dispatchtotalqtykgs;

                             double.TryParse(dr["fat"].ToString(), out FAT);
                             totalfat += FAT;
                             FAT = Math.Round(FAT, 2);
                             newrow["Dispatch FAT"] = FAT;

                             double.TryParse(dr["snf"].ToString(), out SNF);
                             newrow["Dispatch SNF"] = SNF;
                             totalsnf += SNF;

                             double.TryParse(dr["inwardqtykgs"].ToString(), out inword_qty_kgs);
                             inwordtotalqtykgs += inword_qty_kgs;
                             newrow["Inword Quantity_Kgs"] = inwordtotalqtykgs;

                             double.TryParse(dr["inwardfat"].ToString(), out inword_FAT);
                             newrow["Inword FAT"] = dr["inwardfat"].ToString();
                             totalinfat += inword_FAT;

                             double.TryParse(dr["inwardsnf"].ToString(), out inword_snf);
                             newrow["Inword SNF"] = dr["inwardsnf"].ToString();
                             totalinsnf += inword_snf;
                             Report.Rows.Add(newrow);
                         }
                    }

                }
                //DataRow New1 = Report.NewRow();
                //New1["To Silo"] = "SMP";
                //New1["KGS"] = TOTsmp;
                //New1["LTR SNF"] = smptotal;
                //Report.Rows.Add(New1);

                DataRow newvartical1 = Report.NewRow();
                newvartical1["CC Name"] = "Total";
                //newvartical2["Batch Opening qty"] = bopeningtotal;
                newvartical1["Dispatch Quantity_Kgs"] = dispatchtotalqtykgs;
                // newvartical2["CV"] = kgstotal + bopeningtotal;
                newvartical1["Inword Quantity_Kgs"] = inwordtotalqtykgs;
                double ttotal = 0;
                ttotal = (dispatchtotalqtykgs - inwordtotalqtykgs);
                newvartical1["Diff Quantity(kgs)"] = ttotal;

                newvartical1["Dispatch FAT"] = totalfat;
                newvartical1["Dispatch SNF"] = totalsnf;
                newvartical1["Inword FAT"] = totalinfat;
                newvartical1["Inword SNF"] = totalinsnf;

                double tsnftotal = totalsnf - totalinsnf;
                newvartical1["Diff SNF"] = Math.Round(tsnftotal, 2);
                double tfattotal = totalfat - totalinfat;
                newvartical1["Diff FAT"] = Math.Round(tfattotal, 2);
                Report.Rows.Add(newvartical1);
               
                //DataRow newvartical3 = Report.NewRow();
                //newvartical3["Type"] = "Grand Total";
                //newvartical3["KGS"] = gkgstotal;
                //newvartical3["LTRS"] = gLtrstotal;
                //double gfattotal = 0;
                //gfattotal = (gkgfattotal / gLtrstotal) * 100;
                //gfattotal = Math.Round(gfattotal, 2);
                //newvartical3["FAT"] = gfattotal;
                //double gsnftotal = 0;
                //double gsmpkgsnftotal = 0;
                //gsmpkgsnftotal = gsmptotal + gkgsnftotal;
                //gsnftotal = (gsmpkgsnftotal / gLtrstotal) * 100;
                //gsnftotal = Math.Round(gsnftotal, 2);
                //newvartical3["SNF"] = gsnftotal;
                //newvartical3["LTR FAT"] = gkgfattotal;
                //newvartical3["LTR SNF"] = gsmpkgsnftotal;
                //newvartical3["SMP"] = gsmptotal;
                //Report.Rows.Add(newvartical3);
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
            //if (e.Row.Cells[4].Text == "Grand Total")
            //{
            //    e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
            //    e.Row.Font.Size = FontUnit.Large;
            //    e.Row.Font.Bold = true;
            //}
        }
    }

}