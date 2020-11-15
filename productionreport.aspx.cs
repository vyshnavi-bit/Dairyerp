using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class productionreport : System.Web.UI.Page
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
            Session["filename"] = "Production Details";
            Session["title"] = "Production Details";
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
            Report.Columns.Add("Date");
            Report.Columns.Add("Sno");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Qty KGS");
            Report.Columns.Add("FAT");
            Report.Columns.Add("SNF");
            Report.Columns.Add("KG FAT");
            Report.Columns.Add("KG SNF");
            Report.Columns.Add("Opening Balance");
            Report.Columns.Add("Prod");
            Report.Columns.Add("Total");
            Report.Columns.Add("Prod.pkt");
            Report.Columns.Add("Sales");
            if (BranchID == "26" || BranchID == "115")
            {
                Report.Columns.Add("Loss Qty");
                Report.Columns.Add("Damage Qty");
                Report.Columns.Add("Cutting Qty");
                Report.Columns.Add("Return Qty");
            }
            Report.Columns.Add("Closing Balance");
            Report.Columns.Add("Remarks");
            if (ddltype.SelectedItem.Text == "All")
            {
                cmd = new SqlCommand("Select cpd.sno, cpd.productid, cpd.fat, cpd.snf, cpd.qty_kgs, cpd.ob, cpd.productionqty, cpd.total, cpd.remarks, cpd.sales, cpd.createdby, cpd.branchid, cpd.createdon, pm.productname, bi.branchname,pm.ml, cpd.cuttingqty from plant_production_details cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.createdon BETWEEN @d1 AND @d2) AND (cpd.branchid=@branchid)  AND (cpd.deptid = '1') ORDER BY cpd.createdon");
            }
            if (ddltype.SelectedItem.Text == "PANEER")
            {
                cmd = new SqlCommand("Select cpd.sno, cpd.productid, cpd.fat, cpd.snf, cpd.qty_kgs, cpd.ob, cpd.productionqty, cpd.total, cpd.remarks, cpd.sales, cpd.createdby, cpd.branchid, cpd.createdon, pm.productname, bi.branchname,pm.ml, cpd.cuttingqty from plant_production_details cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.createdon BETWEEN @d1 AND @d2) AND (cpd.branchid=@branchid) AND (pm.batchid = '16') AND (cpd.deptid = '1') AND (pm.biproductsshortname = 'P') ORDER BY cpd.createdon");
            }
            if (ddltype.SelectedItem.Text == "KHOVA")
            {
                cmd = new SqlCommand("Select cpd.sno, cpd.productid, cpd.fat, cpd.snf, cpd.qty_kgs, cpd.ob, cpd.productionqty, cpd.total, cpd.remarks, cpd.sales, cpd.createdby, cpd.branchid, cpd.createdon, pm.productname, bi.branchname,pm.ml, cpd.cuttingqty from plant_production_details cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.createdon BETWEEN @d1 AND @d2) AND (cpd.branchid=@branchid) AND (pm.batchid = '16') AND (cpd.deptid = '1') AND (pm.biproductsshortname = 'K') ORDER BY cpd.createdon");
            }
            if (ddltype.SelectedItem.Text == "PANNER-SPL")
            {
                cmd = new SqlCommand("Select cpd.sno, cpd.productid, cpd.fat, cpd.snf, cpd.qty_kgs, cpd.ob, cpd.productionqty, cpd.total, cpd.remarks, cpd.sales, cpd.createdby, cpd.branchid, cpd.createdon, pm.productname, bi.branchname,pm.ml, cpd.cuttingqty from plant_production_details cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.createdon BETWEEN @d1 AND @d2) AND (cpd.branchid=@branchid) AND (pm.batchid = '16') AND (cpd.deptid = '1') AND (pm.biproductsshortname = 'PS') ORDER BY cpd.createdon");
            }
            if (ddltype.SelectedItem.Text == "CURD & BUTTER MILK")
            {
                cmd = new SqlCommand("Select cpd.sno, cpd.productid, cpd.fat, cpd.snf, cpd.qty_kgs, cpd.ob, cpd.productionqty, cpd.total, cpd.remarks, cpd.sales, cpd.createdby, cpd.branchid, cpd.createdon, pm.productname, bi.branchname,pm.ml, cpd.cuttingqty, cpd.returnqty, cpd.damageqty, cpd.lossqty from plant_production_details cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.createdon BETWEEN @d1 AND @d2) AND (cpd.branchid=@branchid) AND (pm.batchid != '16') AND (cpd.deptid = '1')  ORDER BY cpd.createdon, pm.productranking");
            }
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtDispatch = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double closingbalance = 0;
                double total = 0;
                double produtotal = 0;
                double sales = 0;
                int i = 1;
                string prevdate = "";
                foreach (DataRow dr in dtDispatch.Rows)
                {
                    string qtykgs = dr["qty_kgs"].ToString();
                    if (qtykgs != "")
                    {
                        DateTime dtdate = Convert.ToDateTime(dr["createdon"].ToString());
                        string newdate = dtdate.ToString("dd/MMM/yyyy");
                        if (newdate == prevdate)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            newrow["Date"] = dtdate.ToString("dd/MMM/yyyy");
                            newrow["Product Name"] = dr["productname"].ToString();
                            string qty = dr["qty_kgs"].ToString();
                            double qtykg = Convert.ToDouble(qty);
                            kgstotal += qtykg;
                            newrow["Qty KGS"] = qty.ToString();
                            double FAT = 0;
                            double.TryParse(dr["fat"].ToString(), out FAT);
                            FAT = Math.Round(FAT, 2);
                            newrow["FAT"] = FAT;
                            double SNF = 0;
                            double.TryParse(dr["snf"].ToString(), out SNF);
                            newrow["SNF"] = SNF;
                            double KGFAT = 0;
                            double KGSNF = 0;
                            double tstotal = 0;
                            tstotal = FAT + SNF;
                            KGFAT = (FAT * qtykg) / 100;
                            KGSNF = (SNF * qtykg) / 100;
                            KGFAT = Math.Round(KGFAT, 2);
                            newrow["KG FAT"] = KGFAT;
                            kgfattotal += KGFAT;
                            KGSNF = Math.Round(KGSNF, 2);
                            newrow["KG SNF"] = KGSNF;
                            kgsnftotal += KGSNF;
                            double ob = 0;
                            double.TryParse(dr["ob"].ToString(), out ob);
                            newrow["Opening Balance"] = Math.Round(ob, 2);
                            newrow["Prod"] = dr["productionqty"].ToString();
                            string pd = dr["productionqty"].ToString();
                            double Prod = Convert.ToDouble(pd);
                            produtotal += Prod;
                            newrow["Total"] = (Prod + ob).ToString();
                            tstotal = Prod + ob;
                            double ml = Convert.ToDouble(dr["ml"].ToString());
                            double perltrpck = (1000 / ml);
                            double prodqty = 0;
                            double.TryParse(dr["productionqty"].ToString(), out prodqty);
                            double pckqty = perltrpck * prodqty;
                            newrow["Prod.pkt"] = pckqty.ToString();
                            newrow["Sales"] = dr["sales"].ToString();
                            sales = Convert.ToDouble(dr["sales"].ToString());
                            string cutqty = dr["cuttingqty"].ToString();
                            double cuttingqty = 0;
                            if (cutqty != "")
                            {
                                cuttingqty = Convert.ToDouble(cutqty);
                            }
                            if (BranchID == "26" || BranchID == "115")
                            {
                                string returnqty = dr["returnqty"].ToString();
                                if(returnqty=="")
                                {
                                    returnqty = "0";
                                }
                                string damageqty = dr["damageqty"].ToString();
                                if (damageqty == "")
                                {
                                    damageqty = "0";
                                }
                                string lossqty = dr["lossqty"].ToString();
                                if (lossqty == "")
                                {
                                    lossqty = "0";
                                }
                                newrow["Loss Qty"] = lossqty.ToString();
                                newrow["Damage Qty"] = damageqty.ToString();
                                newrow["Cutting Qty"] = cuttingqty.ToString();
                                newrow["Return Qty"] = returnqty.ToString();
                                closingbalance = tstotal - sales - cuttingqty + Convert.ToDouble(returnqty) - Convert.ToDouble(lossqty) - Convert.ToDouble(damageqty);
                                newrow["Closing Balance"] = Math.Round(closingbalance, 2);
                            }
                            else
                            {
                                closingbalance = tstotal - sales;
                                newrow["Closing Balance"] = Math.Round(closingbalance, 2);
                            }
                            newrow["Remarks"] = dr["remarks"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        else
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            newrow["Date"] = dtdate.ToString("dd/MMM/yyyy");
                            newrow["Product Name"] = dr["productname"].ToString();
                            string qty = "0";
                            if (ddltype.SelectedItem.Text == "PANEER")
                            {
                                cmd = new SqlCommand("Select cpd.sno,cpd.recivedqty, cpd.productid, cpd.fat, cpd.snf, cpd.qty_kgs, cpd.ob, cpd.productionqty, cpd.total, cpd.remarks, cpd.sales, cpd.createdby, cpd.branchid, cpd.createdon, pm.productname, bi.branchname from plant_production_details cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.createdon BETWEEN @ds1 AND @ds2) AND (cpd.branchid=@branchids) AND (pm.batchid = '16') AND (cpd.deptid = '1') AND (cpd.productid = 93) ORDER BY cpd.createdon");
                                cmd.Parameters.Add("@ds1", GetLowDate(dtdate));
                                cmd.Parameters.Add("@ds2", GetHighDate(dtdate));
                                cmd.Parameters.Add("@branchids", BranchID);
                                DataTable dtget = SalesDB.SelectQuery(cmd).Tables[0];
                                if (dtget.Rows.Count > 0)
                                {
                                    qty = dtget.Rows[0]["recivedqty"].ToString();
                                }
                            }
                            else if (ddltype.SelectedItem.Text == "KHOVA")
                            {
                                cmd = new SqlCommand("Select cpd.sno,cpd.recivedqty, cpd.productid, cpd.fat, cpd.snf, cpd.qty_kgs, cpd.ob, cpd.productionqty, cpd.total, cpd.remarks, cpd.sales, cpd.createdby, cpd.branchid, cpd.createdon, pm.productname, bi.branchname from plant_production_details cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.createdon BETWEEN @ds1 AND @ds2) AND (cpd.branchid=@branchids) AND (pm.batchid = '16') AND (cpd.deptid = '1') AND (cpd.productid = 92) ORDER BY cpd.createdon");
                                cmd.Parameters.Add("@ds1", GetLowDate(dtdate));
                                cmd.Parameters.Add("@ds2", GetHighDate(dtdate));
                                cmd.Parameters.Add("@branchids", BranchID);
                                DataTable dtget = SalesDB.SelectQuery(cmd).Tables[0];
                                if (dtget.Rows.Count > 0)
                                {
                                    qty = dtget.Rows[0]["recivedqty"].ToString();
                                }
                            }
                            else if (ddltype.SelectedItem.Text == "PANNER-SPL")
                            {
                                cmd = new SqlCommand("Select cpd.sno,cpd.recivedqty, cpd.productid, cpd.fat, cpd.snf, cpd.qty_kgs, cpd.ob, cpd.productionqty, cpd.total, cpd.remarks, cpd.sales, cpd.createdby, cpd.branchid, cpd.createdon, pm.productname, bi.branchname from plant_production_details cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.createdon BETWEEN @ds1 AND @ds2) AND (cpd.branchid=@branchids) AND (pm.batchid = '16') AND (cpd.deptid = '1') AND (cpd.productid = 1211) ORDER BY cpd.createdon");
                                cmd.Parameters.Add("@ds1", GetLowDate(dtdate));
                                cmd.Parameters.Add("@ds2", GetHighDate(dtdate));
                                cmd.Parameters.Add("@branchids", BranchID);
                                DataTable dtget = SalesDB.SelectQuery(cmd).Tables[0];
                                if (dtget.Rows.Count > 0)
                                {
                                    qty = dtget.Rows[0]["recivedqty"].ToString();
                                }
                            }
                            else
                            {
                                qty = dr["qty_kgs"].ToString();
                            }
                            double qtykg = Convert.ToDouble(qty);
                            kgstotal += qtykg;
                            newrow["Qty KGS"] = qty.ToString();
                            double FAT = 0;
                            double.TryParse(dr["fat"].ToString(), out FAT);
                            FAT = Math.Round(FAT, 2);
                            newrow["FAT"] = FAT;
                            double SNF = 0;
                            double.TryParse(dr["snf"].ToString(), out SNF);
                            newrow["SNF"] = SNF;
                            double KGFAT = 0;
                            double KGSNF = 0;
                            double Kgs = 0;
                            double tstotal = 0;
                            tstotal = FAT + SNF;
                            KGFAT = (FAT * qtykg) / 100;
                            KGSNF = (SNF * qtykg) / 100;
                            KGFAT = Math.Round(KGFAT, 2);
                            newrow["KG FAT"] = KGFAT;
                            kgfattotal += KGFAT;
                            KGSNF = Math.Round(KGSNF, 2);
                            newrow["KG SNF"] = KGSNF;
                            kgsnftotal += KGSNF;
                            double ob = 0;
                            double.TryParse(dr["ob"].ToString(), out ob);
                            newrow["Opening Balance"] = Math.Round(ob, 2);
                            newrow["Prod"] = dr["productionqty"].ToString();
                            string pd = dr["productionqty"].ToString();
                            double Prod = Convert.ToDouble(pd);
                            produtotal += Prod;
                            newrow["Total"] = (Prod + ob).ToString();
                            tstotal = Prod + ob;
                            double ml = Convert.ToDouble(dr["ml"].ToString());
                            double perltrpck = (1000 / ml);
                            double prodqty = 0;
                            double.TryParse(dr["productionqty"].ToString(), out prodqty);
                            double pckqty = perltrpck * prodqty;
                            newrow["Prod.pkt"] = pckqty.ToString();
                            newrow["Sales"] = dr["sales"].ToString();
                            sales = Convert.ToDouble(dr["sales"].ToString());
                            string cutqty = dr["cuttingqty"].ToString();
                            double cuttingqty = 0;
                            if (cutqty != "")
                            {
                                cuttingqty = Convert.ToDouble(cutqty);
                            }
                            if (BranchID == "26" || BranchID == "115")
                            {
                                string returnqty = dr["returnqty"].ToString();
                                if (returnqty == "")
                                {
                                    returnqty = "0";
                                }
                                string damageqty = dr["damageqty"].ToString();
                                if (damageqty == "")
                                {
                                    damageqty = "0";
                                }
                                string lossqty = dr["lossqty"].ToString();
                                if (lossqty == "")
                                {
                                    lossqty = "0";
                                }
                                newrow["Loss Qty"] = lossqty.ToString();
                                newrow["Damage Qty"] = damageqty.ToString();
                                newrow["Cutting Qty"] = cuttingqty.ToString();
                                newrow["Return Qty"] = returnqty.ToString();
                                closingbalance = tstotal - sales - cuttingqty + Convert.ToDouble(returnqty) - Convert.ToDouble(lossqty) - Convert.ToDouble(damageqty);
                                newrow["Closing Balance"] = Math.Round(closingbalance, 2);
                            }
                            else
                            {
                                closingbalance = tstotal - sales;
                                newrow["Closing Balance"] = Math.Round(closingbalance, 2);
                            }
                            newrow["Remarks"] = dr["remarks"].ToString();
                            Report.Rows.Add(newrow);
                            prevdate = newdate;
                        }
                    }
                }
                DataRow newvartical2 = Report.NewRow();
                newvartical2["Product Name"] = "Total";
                newvartical2["Qty KGS"] = kgstotal;
                double fattotal = 0;
                fattotal = (kgfattotal / kgstotal) * 100;
                fattotal = Math.Round(fattotal, 2);
                if (fattotal > 0)
                {
                    newvartical2["FAT"] = fattotal;
                }
                else
                {
                    newvartical2["FAT"] = "0";
                }
                newvartical2["KG FAT"] = kgfattotal;
                double snftotal = 0;
                snftotal = (kgsnftotal / kgstotal) * 100;
                snftotal = Math.Round(snftotal, 2);
                if (snftotal > 0)
                {
                    newvartical2["SNF"] = snftotal;
                }
                else
                {
                    newvartical2["SNF"] = "0";
                }
                newvartical2["KG SNF"] = kgsnftotal;
                newvartical2["Prod"] = produtotal;
                Report.Rows.Add(newvartical2);
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
    protected void grdday_DataBinding(object sender, EventArgs e)
    {
        try
        {
            GridViewGroup First = new GridViewGroup(grdReports, null, "Date");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}