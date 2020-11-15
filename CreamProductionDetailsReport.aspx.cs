using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class CreamProductionDetailsReport : System.Web.UI.Page
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
            Report.Columns.Add("Date");
            Report.Columns.Add("OB Cream");
            Report.Columns.Add("OB Cream Fat %");
            Report.Columns.Add("Production Cream");
            Report.Columns.Add("Production Cream Fat %");
            Report.Columns.Add("Total Cream");
            Report.Columns.Add("Total Cream Fat %");
            Report.Columns.Add("Batches Usage QTY");
            Report.Columns.Add("Batches Usage Fat %");
            Report.Columns.Add("Dispatch To Butter");
            Report.Columns.Add("Dispatch To Butter Fat %");
            Report.Columns.Add("Dispatch To Ghee");
            Report.Columns.Add("Dispatch To Ghee Fat %");
            Report.Columns.Add("Sales");
            Report.Columns.Add("Sales Fat %");
            Report.Columns.Add("Closing Cream");
            Report.Columns.Add("Closing Cream Fat %");
            var dated = new List<DateTime>();
            for (var dt = fromdate; dt <= todate; dt = dt.AddDays(1))
            {
                dated.Add(dt);
            }
            int i = 1;
            double totobtotalqty = 0;
            double totobkgfat = 0;
            double totprodtotalqty = 0;
            double totprodkgfat = 0;
            double tottotaltotalqty = 0;
            double tottotalkgfat = 0;
            double totbatchestotalqty = 0;
            double totbatcheskgfat = 0;
            double totbuttertotalqty = 0;
            double totbutterkgfat = 0;
            double totgheetotalqty = 0;
            double totgheekgfat = 0;
            double totsalestotalqty = 0;
            double totsaleskgfat = 0;
            double totclosingtotalqty = 0;
            double totclosingkgfat = 0;
            double clobqty = 0;
            double clobkgfat = 0;
            for (int j = 0; j < dated.Count; j++)
            {
                DateTime dat = Convert.ToDateTime(dated[j].ToString().Trim());
                double obtotalqty = 0;
                double obkgfat = 0;
                double prodtotalqty = 0;
                double prodkgfat = 0;
                double totaltotalqty = 0;
                double totalkgfat = 0;
                double batchestotalqty = 0;
                double batcheskgfat = 0;
                double buttertotalqty = 0;
                double butterkgfat = 0;
                double gheetotalqty = 0;
                double gheekgfat = 0;
                double salestotalqty = 0;
                double saleskgfat = 0;
                double closingtotalqty = 0;
                double closingkgfat = 0;
                //ob
                if (dat == fromdate)
                {
                    cmd = new SqlCommand("SELECT   fat, snf, productid, closeddate, branchid, qty_ltrs FROM   closing_details WHERE   (closeddate BETWEEN @d1 AND @d2) AND (branchid = @branchid) AND (type = 'cream')");
                    cmd.Parameters.Add("@d1", GetLowDate(dat).AddDays(-1));
                    cmd.Parameters.Add("@d2", GetHighDate(dat).AddDays(-1));
                    cmd.Parameters.Add("@branchid", BranchID);
                    DataTable dtcream = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtcream.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtcream.Rows)
                        {
                            string obs = dr["qty_ltrs"].ToString();
                            if (obs == "")
                            {
                                obs = "0";
                            }
                            double ob = Convert.ToDouble(obs);
                            obtotalqty += ob;
                            string obfats = dr["fat"].ToString();
                            if (obfats == "")
                            {
                                obfats = "0";
                            }
                            double obfat = Convert.ToDouble(obfats);
                            double obkgfats = (obfat * ob) / 100;
                            obkgfat += obkgfats;
                        }
                    }
                    else
                    {
                        double ob = 0;
                        obtotalqty += ob;
                        double obfat = 0;
                        double obkgfats = (obfat * ob) / 100;
                        obkgfat += obkgfats;
                    }
                }
                else
                {
                    double ob = clobqty;
                    obtotalqty += ob;
                    double obfat = clobkgfat;
                    double obkgfats = 0;
                    obkgfats = (obfat * ob) / 100;
                    obkgfat += obkgfats;
                    clobqty = 0;
                    clobkgfat = 0;
                }
                //prod
                cmd = new SqlCommand("SELECT   sno, productionqty, productionfat, openingbalance, openingfat FROM   creamsaparation_details WHERE   (branchid = @cbranchid) AND (doe BETWEEN @cd1 AND @cd2)");
                cmd.Parameters.Add("@cd1", GetLowDate(dat));
                cmd.Parameters.Add("@cd2", GetHighDate(dat));
                cmd.Parameters.Add("@cbranchid", BranchID);
                DataTable dtprod = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtprod.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtprod.Rows)
                    {
                        string productionqtys = dr["productionqty"].ToString();
                        if (productionqtys == "")
                        {
                            productionqtys = "0";
                        }
                        double productionqty = Convert.ToDouble(productionqtys);
                        prodtotalqty += productionqty;
                        string productionfats = dr["productionfat"].ToString();
                        if (productionfats == "")
                        {
                            productionfats = "0";
                        }
                        double productionfat = Convert.ToDouble(productionfats);
                        double productionkgfat = (productionfat * productionqty) / 100;
                        prodkgfat += productionkgfat;
                    }
                }
                //Batches
                cmd = new SqlCommand("SELECT   batchentryid, batchid, qty_kgs, qty_ltrs, fat, doe, type FROM  batchentrydetails WHERE  (type = 'Cream') AND (doe BETWEEN @bsd1 AND @bsd2) AND (branchid = @bsbranchid) ORDER BY doe");
                cmd.Parameters.Add("@bsd1", GetLowDate(dat));
                cmd.Parameters.Add("@bsd2", GetHighDate(dat));
                cmd.Parameters.Add("@bsbranchid", BranchID);
                DataTable dtbatch = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtbatch.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtbatch.Rows)
                    {
                        string qty_kgss = dr["qty_kgs"].ToString();
                        if (qty_kgss == "")
                        {
                            qty_kgss = "0";
                        }
                        double qty_kgs = Convert.ToDouble(qty_kgss);
                        batchestotalqty += qty_kgs;
                        string fats = dr["fat"].ToString();
                        if (fats == "")
                        {
                            fats = "0";
                        }
                        double fat = Convert.ToDouble(fats);
                        double kgfat = (fat * qty_kgs) / 100;
                        batcheskgfat += kgfat;
                    }
                }
                //Butter
                cmd = new SqlCommand("SELECT   sno, qty_kgs, qty_ltrs, date, fat FROM  silo_outward_transaction WHERE  (branchid = @bbranchid) AND (date BETWEEN @bd1 AND @bd2) AND (departmentid = '10') ORDER BY date");
                cmd.Parameters.Add("@bd1", GetLowDate(dat));
                cmd.Parameters.Add("@bd2", GetHighDate(dat));
                cmd.Parameters.Add("@bbranchid", BranchID);
                DataTable dtbutter = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtbutter.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtbutter.Rows)
                    {
                        string qty_kgss = dr["qty_kgs"].ToString();
                        if (qty_kgss == "")
                        {
                            qty_kgss = "0";
                        }
                        double qty_kgs = Convert.ToDouble(qty_kgss);
                        buttertotalqty += qty_kgs;
                        string fats = dr["fat"].ToString();
                        if (fats == "")
                        {
                            fats = "0";
                        }
                        double fat = Convert.ToDouble(fats);
                        double kgfat = (fat * qty_kgs) / 100;
                        butterkgfat += kgfat;
                    }
                }
                //Ghee
                cmd = new SqlCommand("SELECT  sno, qty_kgs, qty_ltrs, date, fat FROM    silo_outward_transaction WHERE   (branchid = @gbranchid) AND (date BETWEEN @gd1 AND @gd2) AND (departmentid = '3') ORDER BY date");
                cmd.Parameters.Add("@gd1", GetLowDate(dat));
                cmd.Parameters.Add("@gd2", GetHighDate(dat));
                cmd.Parameters.Add("@gbranchid", BranchID);
                DataTable dtghee = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtghee.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtghee.Rows)
                    {
                        string qty_kgss = dr["qty_kgs"].ToString();
                        if (qty_kgss == "")
                        {
                            qty_kgss = "0";
                        }
                        double qty_kgs = Convert.ToDouble(qty_kgss);
                        gheetotalqty += qty_kgs;
                        string fats = dr["fat"].ToString();
                        if (fats == "")
                        {
                            fats = "0";
                        }
                        double fat = Convert.ToDouble(fats);
                        double kgfat = (fat * qty_kgs) / 100;
                        gheekgfat += kgfat;
                    }
                }
                //Sales
                cmd = new SqlCommand("SELECT  siloid, departmentid, productid, branchid, qty_kgs, qty_ltrs, date, createdby, sno, fat, snf, clr FROM  silo_outward_transaction WHERE  (departmentid = 4) AND (productid IN (12, 38, 39)) AND (branchid = @dbranchid) AND (date BETWEEN @d1 AND @d2) ORDER BY date");
                cmd.Parameters.Add("@d1", GetLowDate(dat));
                cmd.Parameters.Add("@d2", GetHighDate(dat));
                cmd.Parameters.Add("@dbranchid", BranchID);
                DataTable dtsales = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtsales.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtsales.Rows)
                    {
                        string qty_kgss = dr["qty_kgs"].ToString();
                        if (qty_kgss == "")
                        {
                            qty_kgss = "0";
                        }
                        double qty_kgs = Convert.ToDouble(qty_kgss);
                        salestotalqty += qty_kgs;
                        string fats = dr["fat"].ToString();
                        if (fats == "")
                        {
                            fats = "0";
                        }
                        double fat = Convert.ToDouble(fats);
                        double kgfat = (fat * qty_kgs) / 100;
                        saleskgfat += kgfat;
                    }
                }
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = (i++).ToString();
                newrow["Date"] = dat.ToString("dd/MMM/yyyy");
                //ob
                //if (obtotalqty >= 0)
                //{
                //    newrow["OB Cream"] = obtotalqty.ToString();
                //    totobtotalqty += obtotalqty;
                //}
                //else
                //{
                //    newrow["OB Cream"] = "";
                //}
                //if (obkgfat > 0)
                //{
                //    newrow["OB Cream Fat %"] = Math.Round(((obkgfat * 100) / obtotalqty), 2).ToString();
                //    totobkgfat += obkgfat;
                //}
                //else if (obkgfat == 0)
                //{
                //    newrow["OB Cream Fat %"] = "0";
                //}
                //else
                //{
                //    newrow["OB Cream Fat %"] = "";
                //}
                if (obtotalqty > 0)
                {
                    newrow["OB Cream"] = Math.Round(obtotalqty, 2).ToString();
                    totobtotalqty += obtotalqty;
                }
                else
                {
                    newrow["OB Cream"] = "0";
                }
                if (obkgfat == 0)
                {
                    newrow["OB Cream Fat %"] = "0";
                }
                else if (obkgfat >= 0)
                {
                    newrow["OB Cream Fat %"] = Math.Round(((obkgfat * 100) / obtotalqty), 2).ToString();
                    totobkgfat += obkgfat;
                }
                //prod
                if (prodtotalqty > 0)
                {
                    newrow["Production Cream"] = prodtotalqty.ToString();
                    totprodtotalqty += prodtotalqty;
                }
                else
                {
                    newrow["Production Cream"] = "";
                }
                if (prodkgfat > 0)
                {
                    newrow["Production Cream Fat %"] = Math.Round(((prodkgfat * 100) / prodtotalqty), 2).ToString();
                    totprodkgfat += prodkgfat;
                }
                else
                {
                    newrow["Production Cream Fat %"] = "";
                }
                //total
                if ((obtotalqty + prodtotalqty) > 0)
                {
                    newrow["Total Cream"] = (obtotalqty + prodtotalqty).ToString();
                    newrow["Total Cream Fat %"] = Math.Round((((obkgfat + prodkgfat) * 100) / (obtotalqty + prodtotalqty)), 2).ToString();
                }
                else
                {
                    newrow["Total Cream"] = "";
                    newrow["Total Cream Fat %"] = "";
                }
                //batches
                if (batchestotalqty > 0)
                {
                    newrow["Batches Usage QTY"] = batchestotalqty.ToString();
                    totbatchestotalqty += batchestotalqty;
                }
                else
                {
                    newrow["Batches Usage QTY"] = "";
                }
                if (batcheskgfat > 0)
                {
                    newrow["Batches Usage Fat %"] = Math.Round(((batcheskgfat * 100) / batchestotalqty), 2).ToString();
                    totbatcheskgfat += batcheskgfat;
                }
                else
                {
                    newrow["Batches Usage Fat %"] = "";
                }
                //butter
                if (buttertotalqty > 0)
                {
                    newrow["Dispatch To Butter"] = buttertotalqty.ToString();
                    totbuttertotalqty += buttertotalqty;
                }
                else
                {
                    newrow["Dispatch To Butter"] = "";
                }
                if (butterkgfat > 0)
                {
                    newrow["Dispatch To Butter Fat %"] = Math.Round(((butterkgfat * 100) / buttertotalqty), 2).ToString();
                    totbutterkgfat += butterkgfat;
                }
                else
                {
                    newrow["Dispatch To Butter Fat %"] = "";
                }
                //ghee
                if (gheetotalqty > 0)
                {
                    newrow["Dispatch To Ghee"] = Math.Round(gheetotalqty, 2).ToString();
                    totgheetotalqty += gheetotalqty;
                }
                else
                {
                    newrow["Dispatch To Ghee"] = "";
                }
                if (gheekgfat > 0)
                {
                    newrow["Dispatch To Ghee Fat %"] = Math.Round(((gheekgfat * 100) / gheetotalqty), 2).ToString();
                    totgheekgfat += gheekgfat;
                }
                else
                {
                    newrow["Dispatch To Ghee Fat %"] = "";
                }
                //sales
                if (salestotalqty > 0)
                {
                    newrow["Sales"] = Math.Round(salestotalqty, 2).ToString();
                    totsalestotalqty += salestotalqty;
                }
                else
                {
                    newrow["Sales"] = "";
                }
                if (saleskgfat > 0)
                {
                    newrow["Sales Fat %"] = Math.Round(((saleskgfat * 100) / salestotalqty), 2).ToString();
                    totsaleskgfat += saleskgfat;
                }
                else
                {
                    newrow["Sales Fat %"] = "";
                }
                //closing
                double cb = obtotalqty + prodtotalqty - batchestotalqty - buttertotalqty - gheetotalqty - salestotalqty;
                if (cb > 0)
                {
                    clobqty = cb;
                    newrow["Closing Cream"] = Math.Round(cb, 2);
                }
                else
                {
                    newrow["Closing Cream"] = "0";
                }
                double cbkgfat = obkgfat + prodkgfat - batcheskgfat - butterkgfat - gheekgfat - saleskgfat;
                double cbfat = 0;
                if (cbkgfat == 0)
                {
                    cbfat = 0;
                }
                else if (cbkgfat > 0)
                {
                    cbfat = (cbkgfat * 100) / cb;
                }
                newrow["Closing Cream Fat %"] = Math.Round(cbfat, 2);
                clobkgfat = cbfat;
                if (cbkgfat == 0)
                {
                    newrow["Closing Cream Fat %"] = "0";
                }
                Report.Rows.Add(newrow);
                obtotalqty = 0;
                obkgfat = 0;
                prodtotalqty = 0;
                prodkgfat = 0;
                totaltotalqty = 0;
                totalkgfat = 0;
                batchestotalqty = 0;
                batcheskgfat = 0;
                buttertotalqty = 0;
                butterkgfat = 0;
                gheetotalqty = 0;
                gheekgfat = 0;
                salestotalqty = 0;
                saleskgfat = 0;

            }
            DataRow newvartical4 = Report.NewRow();
            newvartical4["Date"] = "Total";
            //ob
            newvartical4["OB Cream"] = "";
            newvartical4["OB Cream Fat %"] = "";
            //prod
            newvartical4["Production Cream"] = Math.Round(totprodtotalqty, 2).ToString();
            if (totprodkgfat > 0)
            {
                newvartical4["Production Cream Fat %"] = Math.Round(((totprodkgfat * 100) / totprodtotalqty), 2).ToString();
            }
            //total
            newvartical4["Total Cream"] = "";
            newvartical4["Total Cream Fat %"] = "";
            //batches
            newvartical4["Batches Usage QTY"] = Math.Round(totbatchestotalqty, 2).ToString();
            if (totbatcheskgfat > 0)
            {
                newvartical4["Batches Usage Fat %"] = Math.Round(((totbatcheskgfat * 100) / totbatchestotalqty), 2).ToString();
            }
            //butter
            newvartical4["Dispatch To Butter"] = Math.Round(totbuttertotalqty, 2).ToString();
            if (totbutterkgfat > 0)
            {
                newvartical4["Dispatch To Butter Fat %"] = Math.Round(((totbutterkgfat * 100) / totbuttertotalqty), 2).ToString();
            }
            //ghee
            newvartical4["Dispatch To Ghee"] = Math.Round(totgheetotalqty, 2).ToString();
            if (totgheetotalqty > 0)
            {
                newvartical4["Dispatch To Ghee Fat %"] = Math.Round(((totgheekgfat * 100) / totgheetotalqty), 2).ToString();
            }
            //sales
            newvartical4["Sales"] = Math.Round(totsalestotalqty, 2).ToString();
            if (totsaleskgfat > 0)
            {
                newvartical4["Sales Fat %"] = Math.Round(((totsaleskgfat * 100) / totsalestotalqty), 2).ToString();
            }
            //closing
            newvartical4["Closing Cream"] = "";
            newvartical4["Closing Cream Fat %"] = "";
            Report.Rows.Add(newvartical4);
            grdReports.DataSource = Report;
            grdReports.DataBind();
            hidepanel.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
}