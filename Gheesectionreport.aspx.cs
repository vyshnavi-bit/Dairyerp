using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Gheesectionreport : System.Web.UI.Page
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
                    bindproducts();
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
    private void bindproducts()
    {
        cmd = new SqlCommand("SELECT sno, productname FROM productmaster where departmentid='3' ");
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlproduct.DataSource = dttrips;
        ddlproduct.DataTextField = "productname";
        ddlproduct.DataValueField = "sno";
        ddlproduct.DataBind();
        ddlproduct.ClearSelection();
        ddlproduct.Items.Insert(0, new ListItem { Value = "0", Text = "--Select Product--", Selected = true });
        ddlproduct.SelectedValue = "0";
    }
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        if (BranchID == "26" || BranchID == "115")
        {
            gheesales_wyra();
        }
        else
        {
            btn_Generate_Click();
        }
    }
    protected void btn_Generate_Click()
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
            string Type = Session["BranchType"].ToString();
            Report.Columns.Add("sno");
            Report.Columns.Add("Product Name");
            // Report.Columns.Add("Creamtype");
            Report.Columns.Add("Conversion Qty");
            Report.Columns.Add("Conversion Fat");
            Report.Columns.Add("O/B");
            Report.Columns.Add("Production Qty");
            Report.Columns.Add("Date");
            if (ddlproduct.SelectedValue != "0")
            {
                cmd = new SqlCommand("SELECT gp.sno, pm.productname, gp.productid, gp.creamtype, gp.convertionquantity, gp.convertionfat, gp.productionqty, gp.ob, gp.cb, gp.createdon FROM plant_production_details gp inner join productmaster pm on pm.sno=gp.productid WHERE (gp.createdon BETWEEN @d1 AND @d2) AND (gp.productid=@productid) AND (gp.branchid=@BranchID) AND (gp.deptid='3')");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@productid", ddlproduct.SelectedValue);
                cmd.Parameters.Add("@BranchID", BranchID);
            }
            else
            {
                cmd = new SqlCommand("SELECT gp.sno, pm.productname, gp.productid, gp.creamtype, gp.convertionquantity, gp.convertionfat, gp.productionqty, gp.ob, gp.cb, gp.createdon FROM plant_production_details gp inner join productmaster pm on pm.sno=gp.productid  WHERE (gp.createdon BETWEEN @d1 AND @d2) AND (gp.branchid=@BranchID) AND (gp.deptid='3') ORDER BY gp.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
            }
            double totalproductionqty = 0;
            DataTable dtghee = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtghee.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtghee.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    double productionqtys = 0;
                    double.TryParse(dr["productionqty"].ToString(), out productionqtys);
                    if (productionqtys > 0)
                    {
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["createdon"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Product Name"] = dr["productname"].ToString();
                        newrow["Conversion Qty"] = dr["convertionquantity"].ToString();
                        newrow["Conversion Fat"] = dr["convertionfat"].ToString();
                        double productionqty = 0;
                        double.TryParse(dr["productionqty"].ToString(), out productionqty);
                        productionqty = Math.Round(productionqty, 2);
                        newrow["Production Qty"] = productionqty;
                        totalproductionqty += productionqty;
                        double openingbalance = 0;
                        double.TryParse(dr["ob"].ToString(), out openingbalance);
                        openingbalance = Math.Round(openingbalance, 2);
                        newrow["O/B"] = openingbalance;
                        //double sales = 0;
                        //double.TryParse(dr["sales"].ToString(), out sales);
                        //sales = Math.Round(sales, 2);
                        //newrow["Sales"] = sales;
                        double closingbalance = 0;
                        double.TryParse(dr["cb"].ToString(), out closingbalance);
                        closingbalance = Math.Round(closingbalance, 2);
                        //newrow["C/B"] = closingbalance;
                        newrow["Date"] = date;
                        Report.Rows.Add(newrow);
                    }
                }
                DataRow newrow1 = Report.NewRow();
                newrow1["O/B"] = "TOTAL";
                newrow1["Production Qty"] = totalproductionqty;
                Report.Rows.Add(newrow1);

                DataRow newrow2 = Report.NewRow();
                newrow2["Product Name"] = "Ghee Sales Details";
                Report.Rows.Add(newrow2);

                grdReports.DataSource = Report;
                grdReports.DataBind();
                if (BranchID == "26" || BranchID == "115")
                {
                    gheesales_wyra();
                }
                else
                {
                    gheesales();
                }
                Session["xportdata"] = Report;
                hidepanel.Visible = true;
            }
            else
            {
                if (BranchID == "26" || BranchID == "115")
                {
                    gheesales_wyra();
                }
                else
                {
                    gheesales();
                }
                grdReports.DataSource = null;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "Ghee Sales Details")
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

    DataTable dtReport = new DataTable();
    private void gheesales()
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
            string Type = Session["BranchType"].ToString();
            dtReport.Columns.Add("sno");
            dtReport.Columns.Add("Product Name");
            // Report.Columns.Add("Creamtype");
            dtReport.Columns.Add("salesquantity");
            dtReport.Columns.Add("Date");
            if (ddlproduct.SelectedValue != "0")
            {
                cmd = new SqlCommand("SELECT gs.sno, gs.productid, pm.productname, gs.salesquantity, gs.branchid, gs.doe FROM gheesales gs inner join productmaster pm on pm.sno=gs.productid WHERE (gs.doe BETWEEN @d1 AND @d2) AND (gs.productid=@productid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@productid", ddlproduct.SelectedValue);
            }
            else
            {
                cmd = new SqlCommand("SELECT gs.sno, gs.productid, pm.productname, gs.salesquantity, gs.branchid, gs.doe FROM gheesales gs inner join productmaster pm on pm.sno=gs.productid WHERE (gs.doe BETWEEN @d1 AND @d2) Order by gs.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
            }
            double salesqty = 0;
            DataTable dtgheesales = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtgheesales.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtgheesales.Rows)
                {
                    DataRow newrow = dtReport.NewRow();
                    newrow["Sno"] = i++.ToString();
                    double sqty = Convert.ToDouble(dr["salesquantity"]);
                    if (sqty > 0)
                    {
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Product Name"] = dr["productname"].ToString();
                        newrow["salesquantity"] = dr["salesquantity"].ToString();
                        salesqty += sqty;
                        newrow["Date"] = date;
                        dtReport.Rows.Add(newrow);
                    }
                }
                DataRow newrow1 = dtReport.NewRow();
                newrow1["Product Name"] = "TOTAL";
                newrow1["salesquantity"] = salesqty;
                dtReport.Rows.Add(newrow1);
                grdsales.DataSource = dtReport;
                grdsales.DataBind();
                Session["xportdata"] = dtReport;
                hidepanel.Visible = true;
            }
            else
            {
                // lblmsg.Text = "No data were found";
                grdsales.DataSource = null;
                grdsales.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }


    private void gheesales_wyra()
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
            string Type = Session["BranchType"].ToString();
            dtReport.Columns.Add("Date");
            dtReport.Columns.Add("Sno");
            dtReport.Columns.Add("Product Name");
            dtReport.Columns.Add("Opening Bal");
            dtReport.Columns.Add("From KCC");
            dtReport.Columns.Add("Market Return");
            dtReport.Columns.Add("Other Return");
            dtReport.Columns.Add("Sales Qty");
            dtReport.Columns.Add("Cutting");
            dtReport.Columns.Add("Closing Bal");

            //cmd = new SqlCommand("SELECT   sno, qty_kgs, productid, closeddate, branchid FROM    closing_details WHERE  (type = 'ghee')");
            //cmd.Parameters.Add("@d1", GetLowDate(fromdate).AddDays(-1));
            //cmd.Parameters.Add("@d2", GetHighDate(fromdate).AddDays(-1));
            //DataTable dtob = SalesDB.SelectQuery(cmd).Tables[0];


            cmd = new SqlCommand("SELECT  sno, productid, salesquantity, branchid, userid, remarks, doe, fromkcc, marcketreturn, oterreturn, cutting FROM   gheesales ");
            DataTable dtgheesaless = SalesDB.SelectQuery(cmd).Tables[0];

            cmd = new SqlCommand("SELECT   sno, productname, ml FROM  productmaster");
            DataTable dtprod = SalesDB.SelectQuery(cmd).Tables[0];

            DataTable dtgheesales = new DataTable();
            if (ddlproduct.SelectedValue != "0")
            {
                cmd = new SqlCommand("SELECT gs.sno, gs.productid, pm.productname, gs.salesquantity, gs.branchid, gs.doe,gs.fromkcc, gs.marcketreturn, gs.oterreturn, gs.cutting, pm.ml FROM gheesales gs inner join productmaster pm on pm.sno=gs.productid WHERE (gs.doe BETWEEN @d1 AND @d2) AND (gs.productid=@productid) AND gs.branchid=@BranchID");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@productid", ddlproduct.SelectedValue);
                cmd.Parameters.Add("@BranchID", BranchID);
            }
            else
            {
                cmd = new SqlCommand("SELECT gs.sno, gs.productid, pm.productname, gs.salesquantity, gs.branchid, gs.doe,gs.fromkcc, gs.marcketreturn, gs.oterreturn, gs.cutting, pm.ml FROM gheesales gs inner join productmaster pm on pm.sno=gs.productid WHERE (gs.doe BETWEEN @d1 AND @d2) Order by gs.doe");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
            }
            double salesqty = 0;
            double frmkccqty = 0;
            double mrkretunqty = 0;
            double oterrtnqty = 0;
            double cuttingqty = 0;
            string cbob = "0";
            string prvdate = "";
            double obbalqty = 0;
            double closingqty = 0;

            double totsalesqty = 0;
            double totfrmkccqty = 0;
            double totmrkretunqty = 0;
            double tototerrtnqty = 0;
            double totcuttingqty = 0;
            dtgheesales = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtgheesaless.Rows.Count > 0)
            {
                if (ddlproduct.SelectedValue == "0")
                {
                    var gheesaled = from sales in dtgheesaless.AsEnumerable()
                                    join prod in dtprod.AsEnumerable()
                                      on sales.Field<int>("productid") equals
                                      prod.Field<int>("sno")
                                    where (sales.Field<int>("branchid") == Convert.ToInt32(BranchID)
                                                      && sales.Field<DateTime>("doe") >= GetLowDate(fromdate)
                                                      && sales.Field<DateTime>("doe") <= GetHighDate(todate))
                                    select new
                                    {
                                        productid = sales.Field<int>("productid"),
                                        salesquantity = sales.Field<double>("salesquantity"),
                                        doe = sales.Field<DateTime>("doe"),
                                        fromkcc = sales.Field<double>("fromkcc"),
                                        marcketreturn = sales.Field<double>("marcketreturn"),
                                        oterreturn = sales.Field<double>("oterreturn"),
                                        cutting = sales.Field<double>("cutting"),
                                        productname = prod.Field<string>("productname"),
                                        ml = prod.Field<double>("ml")
                                    };

                    int i = 1;
                    foreach (var obj in gheesaled)
                    {
                        DataRow newrow = dtReport.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(obj.doe.ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        if (date == prvdate)
                        {
                            string openingbal = "0";
                            string productid = obj.productid.ToString();
                            string pcksize = obj.cutting.ToString();
                            //if (GetLowDate(fromdate) == GetLowDate(dtdoe))
                            //{
                            cmd = new SqlCommand("SELECT   sno, qty_kgs, productid, closeddate, branchid FROM    closing_details WHERE  (type = 'ghee') and productid=@productid and branchid=@branchid and closeddate between @d1 and @d2");
                            cmd.Parameters.Add("@d1", GetLowDate(dtdoe).AddDays(-1));
                            cmd.Parameters.Add("@d2", GetHighDate(dtdoe).AddDays(-1));
                            cmd.Parameters.Add("@productid",productid);
                            cmd.Parameters.Add("@branchid", BranchID);
                            DataTable dtbl = SalesDB.SelectQuery(cmd).Tables[0];
                            if (dtbl.Rows.Count > 0)
                            {
                                openingbal = dtbl.Rows[0]["qty_kgs"].ToString();
                            }
                            newrow["Opening Bal"] = openingbal;
                            obbalqty += Convert.ToDouble(openingbal);
                            //}
                            //else
                            //{
                            //openingbal = cbob;
                            //obbalqty += Convert.ToDouble(openingbal);
                            //cbob = "";
                            //}
                            newrow["Sno"] = i++.ToString();
                            newrow["Product Name"] = obj.productname.ToString();
                            newrow["From KCC"] = obj.fromkcc.ToString();
                            double fromkcc = 0;
                            if (obj.fromkcc.ToString() == "" || obj.fromkcc.ToString() == null || obj.fromkcc.ToString() == "null")
                            {
                                fromkcc = 0;
                            }
                            else
                            {
                                fromkcc = Convert.ToDouble(obj.fromkcc.ToString());
                                frmkccqty += fromkcc;
                                totfrmkccqty += fromkcc;
                            }
                            newrow["Market Return"] = obj.marcketreturn.ToString();
                            double marcketreturn = 0;
                            if (obj.marcketreturn.ToString() == "" || obj.marcketreturn.ToString() == null || obj.marcketreturn.ToString() == "null")
                            {
                                marcketreturn = 0;
                            }
                            else
                            {
                                marcketreturn = Convert.ToDouble(obj.marcketreturn.ToString());
                                mrkretunqty += marcketreturn;
                                totmrkretunqty += marcketreturn;
                            }
                            newrow["Other Return"] = obj.oterreturn.ToString();
                            double oterreturn = 0;
                            if (obj.oterreturn.ToString() == "" || obj.oterreturn.ToString() == null || obj.oterreturn.ToString() == "null")
                            {
                                oterreturn = 0;
                            }
                            else
                            {
                                oterreturn = Convert.ToDouble(obj.oterreturn.ToString());
                                oterrtnqty += oterreturn;
                                tototerrtnqty += oterreturn;
                            }
                            newrow["Sales Qty"] = obj.salesquantity.ToString();
                            double salesquantity = 0;
                            if (obj.salesquantity.ToString() == "" || obj.salesquantity.ToString() == null || obj.salesquantity.ToString() == "null")
                            {
                                salesquantity = 0;
                            }
                            else
                            {
                                salesquantity = Convert.ToDouble(obj.salesquantity);
                                salesqty += salesquantity;
                                totsalesqty += salesquantity;
                            }
                            newrow["Cutting"] = obj.cutting.ToString();
                            double cutting = 0;
                            if (obj.cutting.ToString() == "" || obj.cutting.ToString() == null || obj.cutting.ToString() == "null")
                            {
                                cutting = 0;
                            }
                            else
                            {
                                cutting = Convert.ToDouble(obj.cutting.ToString());
                                cuttingqty += cutting;
                                totcuttingqty += cutting;
                            }
                            double add = Convert.ToDouble(openingbal) + fromkcc + marcketreturn + oterreturn;
                            double sub = salesquantity + cutting;
                            newrow["Closing Bal"] = Math.Round((add - sub), 4);
                            cbob = (add - sub).ToString();
                            closingqty += (add - sub);
                            dtReport.Rows.Add(newrow);
                        }
                        else
                        {
                            if (frmkccqty > 0 || mrkretunqty > 0 || oterrtnqty > 0 || salesqty > 0 || cuttingqty > 0 || obbalqty > 0 || obbalqty > 0)
                            {
                                DataRow newrow1 = dtReport.NewRow();
                                newrow1["Product Name"] = "Total";
                                newrow1["From KCC"] = Math.Round(frmkccqty, 4).ToString();
                                newrow1["Market Return"] = Math.Round(mrkretunqty, 4).ToString();
                                newrow1["Other Return"] = Math.Round(oterrtnqty, 4).ToString();
                                newrow1["Sales Qty"] = Math.Round(salesqty, 4).ToString();
                                newrow1["Cutting"] = Math.Round(cuttingqty, 4).ToString();
                                newrow1["Opening Bal"] = Math.Round(obbalqty, 4).ToString();
                                newrow1["Closing Bal"] = Math.Round(closingqty, 4).ToString();
                                dtReport.Rows.Add(newrow1);
                                salesqty = 0;
                                frmkccqty = 0;
                                mrkretunqty = 0;
                                oterrtnqty = 0;
                                cuttingqty = 0;
                                obbalqty = 0;
                                closingqty = 0;
                            }
                            string openingbal = "0";
                            string productid = obj.productid.ToString();
                            string pcksize = obj.cutting.ToString();
                            //if (GetLowDate(fromdate) == GetLowDate(dtdoe))
                            //{
                            cmd = new SqlCommand("SELECT   sno, qty_kgs, productid, closeddate, branchid FROM    closing_details WHERE  (type = 'ghee') and productid=@productid and branchid=@branchid and closeddate between @d1 and @d2");
                            cmd.Parameters.Add("@d1", GetLowDate(dtdoe).AddDays(-1));
                            cmd.Parameters.Add("@d2", GetHighDate(dtdoe).AddDays(-1));
                            cmd.Parameters.Add("@productid", productid);
                            cmd.Parameters.Add("@branchid", BranchID);
                            DataTable dtbl = SalesDB.SelectQuery(cmd).Tables[0];
                            if (dtbl.Rows.Count > 0)
                            {
                                openingbal = dtbl.Rows[0]["qty_kgs"].ToString();
                            }
                            newrow["Opening Bal"] = openingbal;
                            obbalqty += Convert.ToDouble(openingbal);
                            //}
                            //else
                            //{
                            //    openingbal = cbob;
                            //    obbalqty += Convert.ToDouble(openingbal);
                            //    cbob = "";
                            //}
                            newrow["Date"] = date;
                            newrow["Sno"] = i++.ToString();
                            newrow["Product Name"] = obj.productname.ToString();
                            newrow["From KCC"] = obj.fromkcc.ToString();
                            double fromkcc = 0;
                            if (obj.fromkcc.ToString() == "" || obj.fromkcc.ToString() == null || obj.fromkcc.ToString() == "null")
                            {
                                fromkcc = 0;
                            }
                            else
                            {
                                fromkcc = Convert.ToDouble(obj.fromkcc.ToString());
                                frmkccqty += fromkcc;
                                totfrmkccqty += fromkcc;
                            }
                            newrow["Market Return"] = obj.marcketreturn.ToString();
                            double marcketreturn = 0;
                            if (obj.marcketreturn.ToString() == "" || obj.marcketreturn.ToString() == null || obj.marcketreturn.ToString() == "null")
                            {
                                marcketreturn = 0;
                            }
                            else
                            {
                                marcketreturn = Convert.ToDouble(obj.marcketreturn.ToString());
                                mrkretunqty += marcketreturn;
                                totmrkretunqty += marcketreturn;
                            }
                            newrow["Other Return"] = obj.oterreturn.ToString();
                            double oterreturn = 0;
                            if (obj.oterreturn.ToString() == "" || obj.oterreturn.ToString() == null || obj.oterreturn.ToString() == "null")
                            {
                                oterreturn = 0;
                            }
                            else
                            {
                                oterreturn = Convert.ToDouble(obj.oterreturn.ToString());
                                oterrtnqty += oterreturn;
                                tototerrtnqty += oterreturn;
                            }
                            newrow["Sales Qty"] = obj.salesquantity.ToString();
                            double salesquantity = 0;
                            if (obj.salesquantity.ToString() == "" || obj.salesquantity.ToString() == null || obj.salesquantity.ToString() == "null")
                            {
                                salesquantity = 0;
                            }
                            else
                            {
                                salesquantity = Convert.ToDouble(obj.salesquantity);
                                salesqty += salesquantity;
                                totsalesqty += salesquantity;
                            }
                            newrow["Cutting"] = obj.cutting.ToString();
                            double cutting = 0;
                            if (obj.cutting.ToString() == "" || obj.cutting.ToString() == null || obj.cutting.ToString() == "null")
                            {
                                cutting = 0;
                            }
                            else
                            {
                                cutting = Convert.ToDouble(obj.cutting.ToString());
                                cuttingqty += cutting;
                                totcuttingqty += cutting;
                            }
                            double add = Convert.ToDouble(openingbal) + fromkcc + marcketreturn + oterreturn;
                            double sub = salesquantity + cutting;
                            newrow["Closing Bal"] = Math.Round((add - sub), 4);
                            cbob = (add - sub).ToString();
                            closingqty += (add - sub);
                            dtReport.Rows.Add(newrow);
                            prvdate = date;
                        }
                    }
                }
                else
                {
                    var gheesaled = from sales in dtgheesaless.AsEnumerable()
                                    join prod in dtprod.AsEnumerable()
                                      on sales.Field<int>("productid") equals
                                      prod.Field<int>("sno")
                                    where (sales.Field<int>("branchid") == Convert.ToInt32(BranchID)
                                                      && sales.Field<int>("productid") == Convert.ToInt32(ddlproduct.SelectedValue)
                                                      && sales.Field<DateTime>("doe") >= GetLowDate(fromdate)
                                                      && sales.Field<DateTime>("doe") <= GetHighDate(todate))
                                    select new
                                    {
                                        productid = sales.Field<int>("productid"),
                                        salesquantity = sales.Field<double>("salesquantity"),
                                        doe = sales.Field<DateTime>("doe"),
                                        fromkcc = sales.Field<double>("fromkcc"),
                                        marcketreturn = sales.Field<double>("marcketreturn"),
                                        oterreturn = sales.Field<double>("oterreturn"),
                                        cutting = sales.Field<double>("cutting"),
                                        productname = prod.Field<string>("productname"),
                                        ml = prod.Field<double>("ml")
                                    };
                    int i = 1;
                    foreach (var obj in gheesaled)
                    {
                        DataRow newrow = dtReport.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(obj.doe.ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        if (date == prvdate)
                        {
                            string openingbal = "0";
                            string productid = obj.productid.ToString();
                            string pcksize = obj.cutting.ToString();
                            //if (GetLowDate(fromdate) == GetLowDate(dtdoe))
                            //{
                            cmd = new SqlCommand("SELECT   sno, qty_kgs, productid, closeddate, branchid FROM    closing_details WHERE  (type = 'ghee') and productid=@productid and branchid=@branchid and closeddate between @d1 and @d2");
                            cmd.Parameters.Add("@d1", GetLowDate(dtdoe).AddDays(-1));
                            cmd.Parameters.Add("@d2", GetHighDate(dtdoe).AddDays(-1));
                            cmd.Parameters.Add("@productid", productid);
                            cmd.Parameters.Add("@branchid", BranchID);
                            DataTable dtbl = SalesDB.SelectQuery(cmd).Tables[0];
                            if (dtbl.Rows.Count > 0)
                            {
                                openingbal = dtbl.Rows[0]["qty_kgs"].ToString();
                            }
                            newrow["Opening Bal"] = openingbal;
                            obbalqty += Convert.ToDouble(openingbal);
                            //}
                            //else
                            //{
                            //    openingbal = cbob;
                            //    obbalqty += Convert.ToDouble(openingbal);
                            //    cbob = "";
                            //}
                            newrow["Sno"] = i++.ToString();
                            newrow["Product Name"] = obj.productname.ToString();
                            newrow["From KCC"] = obj.fromkcc.ToString();
                            double fromkcc = 0;
                            if (obj.fromkcc.ToString() == "" || obj.fromkcc.ToString() == null || obj.fromkcc.ToString() == "null")
                            {
                                fromkcc = 0;
                            }
                            else
                            {
                                fromkcc = Convert.ToDouble(obj.fromkcc.ToString());
                                frmkccqty += fromkcc;
                                totfrmkccqty += fromkcc;
                            }
                            newrow["Market Return"] = obj.marcketreturn.ToString();
                            double marcketreturn = 0;
                            if (obj.marcketreturn.ToString() == "" || obj.marcketreturn.ToString() == null || obj.marcketreturn.ToString() == "null")
                            {
                                marcketreturn = 0;
                            }
                            else
                            {
                                marcketreturn = Convert.ToDouble(obj.marcketreturn.ToString());
                                mrkretunqty += marcketreturn;
                                totmrkretunqty += marcketreturn;
                            }
                            newrow["Other Return"] = obj.oterreturn.ToString();
                            double oterreturn = 0;
                            if (obj.oterreturn.ToString() == "" || obj.oterreturn.ToString() == null || obj.oterreturn.ToString() == "null")
                            {
                                oterreturn = 0;
                            }
                            else
                            {
                                oterreturn = Convert.ToDouble(obj.oterreturn.ToString());
                                oterrtnqty += oterreturn;
                                tototerrtnqty += oterreturn;
                            }
                            newrow["Sales Qty"] = obj.salesquantity.ToString();
                            double salesquantity = 0;
                            if (obj.salesquantity.ToString() == "" || obj.salesquantity.ToString() == null || obj.salesquantity.ToString() == "null")
                            {
                                salesquantity = 0;
                            }
                            else
                            {
                                salesquantity = Convert.ToDouble(obj.salesquantity);
                                salesqty += salesquantity;
                                totsalesqty += salesquantity;
                            }
                            newrow["Cutting"] = obj.cutting.ToString();
                            double cutting = 0;
                            if (obj.cutting.ToString() == "" || obj.cutting.ToString() == null || obj.cutting.ToString() == "null")
                            {
                                cutting = 0;
                            }
                            else
                            {
                                cutting = Convert.ToDouble(obj.cutting.ToString());
                                cuttingqty += cutting;
                                totcuttingqty += cutting;
                            }
                            double add = Convert.ToDouble(openingbal) + fromkcc + marcketreturn + oterreturn;
                            double sub = salesquantity + cutting;
                            newrow["Closing Bal"] = Math.Round((add - sub), 4);
                            cbob = (add - sub).ToString();
                            closingqty += (add - sub);
                            dtReport.Rows.Add(newrow);
                        }
                        else
                        {
                            if (frmkccqty > 0 || mrkretunqty > 0 || oterrtnqty > 0 || salesqty > 0 || cuttingqty > 0 || obbalqty > 0 || obbalqty > 0)
                            {
                                DataRow newrow1 = dtReport.NewRow();
                                newrow1["Product Name"] = "Total";
                                newrow1["From KCC"] = Math.Round(frmkccqty, 4).ToString();
                                newrow1["Market Return"] = Math.Round(mrkretunqty, 4).ToString();
                                newrow1["Other Return"] = Math.Round(oterrtnqty, 4).ToString();
                                newrow1["Sales Qty"] = Math.Round(salesqty, 4).ToString();
                                newrow1["Cutting"] = Math.Round(cuttingqty, 4).ToString();
                                newrow1["Opening Bal"] = Math.Round(obbalqty, 4).ToString();
                                newrow1["Closing Bal"] = Math.Round(closingqty, 4).ToString();
                                dtReport.Rows.Add(newrow1);
                                salesqty = 0;
                                frmkccqty = 0;
                                mrkretunqty = 0;
                                oterrtnqty = 0;
                                cuttingqty = 0;
                                obbalqty = 0;
                                closingqty = 0;
                            }
                            string openingbal = "0";
                            string productid = obj.productid.ToString();
                            string pcksize = obj.cutting.ToString();
                            //if (GetLowDate(fromdate) == GetLowDate(dtdoe))
                            //{
                            cmd = new SqlCommand("SELECT   sno, qty_kgs, productid, closeddate, branchid FROM    closing_details WHERE  (type = 'ghee') and productid=@productid and branchid=@branchid and closeddate between @d1 and @d2");
                            cmd.Parameters.Add("@d1", GetLowDate(dtdoe).AddDays(-1));
                            cmd.Parameters.Add("@d2", GetHighDate(dtdoe).AddDays(-1));
                            cmd.Parameters.Add("@productid", productid);
                            cmd.Parameters.Add("@branchid", BranchID);
                            DataTable dtbl = SalesDB.SelectQuery(cmd).Tables[0];
                            if (dtbl.Rows.Count > 0)
                            {
                                openingbal = dtbl.Rows[0]["qty_kgs"].ToString();
                            }
                            newrow["Opening Bal"] = openingbal;
                            obbalqty += Convert.ToDouble(openingbal);
                            //}
                            //else
                            //{
                            //    openingbal = cbob;
                            //    obbalqty += Convert.ToDouble(openingbal);
                            //    cbob = "";
                            //}
                            newrow["Date"] = date;
                            newrow["Sno"] = i++.ToString();
                            newrow["Product Name"] = obj.productname.ToString();
                            newrow["From KCC"] = obj.fromkcc.ToString();
                            double fromkcc = 0;
                            if (obj.fromkcc.ToString() == "" || obj.fromkcc.ToString() == null || obj.fromkcc.ToString() == "null")
                            {
                                fromkcc = 0;
                            }
                            else
                            {
                                fromkcc = Convert.ToDouble(obj.fromkcc.ToString());
                                frmkccqty += fromkcc;
                                totfrmkccqty += fromkcc;
                            }
                            newrow["Market Return"] = obj.marcketreturn.ToString();
                            double marcketreturn = 0;
                            if (obj.marcketreturn.ToString() == "" || obj.marcketreturn.ToString() == null || obj.marcketreturn.ToString() == "null")
                            {
                                marcketreturn = 0;
                            }
                            else
                            {
                                marcketreturn = Convert.ToDouble(obj.marcketreturn.ToString());
                                mrkretunqty += marcketreturn;
                                totmrkretunqty += marcketreturn;
                            }
                            newrow["Other Return"] = obj.oterreturn.ToString();
                            double oterreturn = 0;
                            if (obj.oterreturn.ToString() == "" || obj.oterreturn.ToString() == null || obj.oterreturn.ToString() == "null")
                            {
                                oterreturn = 0;
                            }
                            else
                            {
                                oterreturn = Convert.ToDouble(obj.oterreturn.ToString());
                                oterrtnqty += oterreturn;
                                tototerrtnqty += oterreturn;
                            }
                            newrow["Sales Qty"] = obj.salesquantity.ToString();
                            double salesquantity = 0;
                            if (obj.salesquantity.ToString() == "" || obj.salesquantity.ToString() == null || obj.salesquantity.ToString() == "null")
                            {
                                salesquantity = 0;
                            }
                            else
                            {
                                salesquantity = Convert.ToDouble(obj.salesquantity);
                                salesqty += salesquantity;
                                totsalesqty += salesquantity;
                            }
                            newrow["Cutting"] = obj.cutting.ToString();
                            double cutting = 0;
                            if (obj.cutting.ToString() == "" || obj.cutting.ToString() == null || obj.cutting.ToString() == "null")
                            {
                                cutting = 0;
                            }
                            else
                            {
                                cutting = Convert.ToDouble(obj.cutting.ToString());
                                cuttingqty += cutting;
                                totcuttingqty += cutting;
                            }
                            double add = Convert.ToDouble(openingbal) + fromkcc + marcketreturn + oterreturn;
                            double sub = salesquantity + cutting;
                            newrow["Closing Bal"] = Math.Round((add - sub), 4);
                            cbob = (add - sub).ToString();
                            closingqty += (add - sub);
                            dtReport.Rows.Add(newrow);
                            prvdate = date;
                        }
                    }
                }
                //foreach (DataRow dr in dtgheesales.Rows)
                //{
                //    DataRow newrow = dtReport.NewRow();
                //    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                //    string date = dtdoe.ToString("dd/MM/yyyy");
                //    if (date == prvdate)
                //    {
                //        string openingbal = "0";
                //        string productid = dr["productid"].ToString();
                //        string pcksize = dr["ml"].ToString();
                //        if (GetLowDate(fromdate) == GetLowDate(dtdoe))
                //        {
                //            var results = from myRow in dtob.AsEnumerable()
                //                          where (myRow.Field<int>("productid") == Convert.ToInt32(productid)
                //                                    && myRow.Field<DateTime>("closeddate") >= GetLowDate(fromdate).AddDays(-1)
                //                                    && myRow.Field<DateTime>("closeddate") <= GetHighDate(fromdate).AddDays(-1))
                //                          select myRow;
                //            DataTable dtbl = results.CopyToDataTable<DataRow>();
                //            openingbal = dtbl.Rows[0]["qty_kgs"].ToString();
                //            newrow["Opening Bal"] = openingbal;
                //            obbalqty += Convert.ToDouble(openingbal);
                //        }
                //        else
                //        {
                //            openingbal = cbob;
                //            obbalqty += Convert.ToDouble(openingbal);
                //            cbob = "";
                //        }
                //        newrow["Sno"] = i++.ToString();
                //        newrow["Product Name"] = dr["productname"].ToString();
                //        newrow["From KCC"] = dr["fromkcc"].ToString();
                //        double fromkcc = 0;
                //        if (dr["fromkcc"].ToString() == "" || dr["fromkcc"].ToString() == null || dr["fromkcc"].ToString() == "null")
                //        {
                //            fromkcc = 0;
                //        }
                //        else
                //        {
                //            fromkcc = Convert.ToDouble(dr["fromkcc"].ToString());
                //            frmkccqty += fromkcc;
                //            totfrmkccqty += fromkcc;
                //        }
                //        newrow["Market Return"] = dr["marcketreturn"].ToString();
                //        double marcketreturn = 0;
                //        if (dr["marcketreturn"].ToString() == "" || dr["marcketreturn"].ToString() == null || dr["marcketreturn"].ToString() == "null")
                //        {
                //            marcketreturn = 0;
                //        }
                //        else
                //        {
                //            marcketreturn = Convert.ToDouble(dr["marcketreturn"].ToString());
                //            mrkretunqty += marcketreturn;
                //            totmrkretunqty += marcketreturn;
                //        }
                //        newrow["Other Return"] = dr["oterreturn"].ToString();
                //        double oterreturn = 0;
                //        if (dr["oterreturn"].ToString() == "" || dr["oterreturn"].ToString() == null || dr["oterreturn"].ToString() == "null")
                //        {
                //            oterreturn = 0;
                //        }
                //        else
                //        {
                //            oterreturn = Convert.ToDouble(dr["oterreturn"].ToString());
                //            oterrtnqty += oterreturn;
                //            tototerrtnqty += oterreturn;
                //        }
                //        newrow["Sales Qty"] = dr["salesquantity"].ToString();
                //        double salesquantity = 0;
                //        if (dr["salesquantity"].ToString() == "" || dr["salesquantity"].ToString() == null || dr["salesquantity"].ToString() == "null")
                //        {
                //            salesquantity = 0;
                //        }
                //        else
                //        {
                //            salesquantity = Convert.ToDouble(dr["salesquantity"]);
                //            salesqty += salesquantity;
                //            totsalesqty += salesquantity;
                //        }
                //        newrow["Cutting"] = dr["cutting"].ToString();
                //        double cutting = 0;
                //        if (dr["cutting"].ToString() == "" || dr["cutting"].ToString() == null || dr["cutting"].ToString() == "null")
                //        {
                //            cutting = 0;
                //        }
                //        else
                //        {
                //            cutting = Convert.ToDouble(dr["cutting"].ToString());
                //            cuttingqty += cutting;
                //            totcuttingqty += cutting;
                //        }
                //        double add = Convert.ToDouble(openingbal) + fromkcc + marcketreturn + oterreturn;
                //        double sub = salesquantity + cutting;
                //        newrow["Closing Bal"] = Math.Round((add - sub), 4);
                //        cbob = (add - sub).ToString();
                //        closingqty += (add - sub);
                //        dtReport.Rows.Add(newrow);
                //    }
                //    else
                //    {
                //        if (frmkccqty > 0 || mrkretunqty > 0 || oterrtnqty > 0 || salesqty > 0 || cuttingqty > 0 || obbalqty > 0 || obbalqty > 0)
                //        {
                //            DataRow newrow1 = dtReport.NewRow();
                //            newrow1["Product Name"] = "Total";
                //            newrow1["From KCC"] = Math.Round(frmkccqty, 4).ToString();
                //            newrow1["Market Return"] = Math.Round(mrkretunqty, 4).ToString();
                //            newrow1["Other Return"] = Math.Round(oterrtnqty, 4).ToString();
                //            newrow1["Sales Qty"] = Math.Round(salesqty, 4).ToString();
                //            newrow1["Cutting"] = Math.Round(cuttingqty, 4).ToString();
                //            newrow1["Opening Bal"] = Math.Round(obbalqty, 4).ToString();
                //            newrow1["Closing Bal"] = Math.Round(closingqty, 4).ToString();
                //            dtReport.Rows.Add(newrow1);
                //            salesqty = 0;
                //            frmkccqty = 0;
                //            mrkretunqty = 0;
                //            oterrtnqty = 0;
                //            cuttingqty = 0;
                //            obbalqty = 0;
                //            closingqty = 0;
                //        }
                //        string openingbal = "0";
                //        string productid = dr["productid"].ToString();
                //        string pcksize = dr["ml"].ToString();
                //        if (GetLowDate(fromdate) == GetLowDate(dtdoe))
                //        {
                //            var results = from myRow in dtob.AsEnumerable()
                //                          where (myRow.Field<int>("productid") == Convert.ToInt32(productid)
                //                                    && myRow.Field<DateTime>("closeddate") >= GetLowDate(fromdate).AddDays(-1)
                //                                    && myRow.Field<DateTime>("closeddate") <= GetHighDate(fromdate).AddDays(-1))
                //                          select myRow;
                //            DataTable dtbl = results.CopyToDataTable<DataRow>();
                //            openingbal = dtbl.Rows[0]["qty_kgs"].ToString();
                //            newrow["Opening Bal"] = openingbal;
                //            obbalqty += Convert.ToDouble(openingbal);
                //        }
                //        else
                //        {
                //            openingbal = cbob;
                //            obbalqty += Convert.ToDouble(openingbal);
                //            cbob = "";
                //        }
                //        newrow["Date"] = date;
                //        newrow["Sno"] = i++.ToString();
                //        newrow["Product Name"] = dr["productname"].ToString();
                //        newrow["From KCC"] = dr["fromkcc"].ToString();
                //        double fromkcc = 0;
                //        if (dr["fromkcc"].ToString() == "" || dr["fromkcc"].ToString() == null || dr["fromkcc"].ToString() == "null")
                //        {
                //            fromkcc = 0;
                //        }
                //        else
                //        {
                //            fromkcc = Convert.ToDouble(dr["fromkcc"].ToString());
                //            frmkccqty += fromkcc;
                //            totfrmkccqty += fromkcc;
                //        }
                //        newrow["Market Return"] = dr["marcketreturn"].ToString();
                //        double marcketreturn = 0;
                //        if (dr["marcketreturn"].ToString() == "" || dr["marcketreturn"].ToString() == null || dr["marcketreturn"].ToString() == "null")
                //        {
                //            marcketreturn = 0;
                //        }
                //        else
                //        {
                //            marcketreturn = Convert.ToDouble(dr["marcketreturn"].ToString());
                //            mrkretunqty += marcketreturn;
                //            totmrkretunqty += marcketreturn;
                //        }
                //        newrow["Other Return"] = dr["oterreturn"].ToString();
                //        double oterreturn = 0;
                //        if (dr["oterreturn"].ToString() == "" || dr["oterreturn"].ToString() == null || dr["oterreturn"].ToString() == "null")
                //        {
                //            oterreturn = 0;
                //        }
                //        else
                //        {
                //            oterreturn = Convert.ToDouble(dr["oterreturn"].ToString());
                //            oterrtnqty += oterreturn;
                //            tototerrtnqty += oterreturn;
                //        }
                //        newrow["Sales Qty"] = dr["salesquantity"].ToString();
                //        double salesquantity = 0;
                //        if (dr["salesquantity"].ToString() == "" || dr["salesquantity"].ToString() == null || dr["salesquantity"].ToString() == "null")
                //        {
                //            salesquantity = 0;
                //        }
                //        else
                //        {
                //            salesquantity = Convert.ToDouble(dr["salesquantity"]);
                //            salesqty += salesquantity;
                //            totsalesqty += salesquantity;
                //        }
                //        newrow["Cutting"] = dr["cutting"].ToString();
                //        double cutting = 0;
                //        if (dr["cutting"].ToString() == "" || dr["cutting"].ToString() == null || dr["cutting"].ToString() == "null")
                //        {
                //            cutting = 0;
                //        }
                //        else
                //        {
                //            cutting = Convert.ToDouble(dr["cutting"].ToString());
                //            cuttingqty += cutting;
                //            totcuttingqty += cutting;
                //        }
                //        double add = Convert.ToDouble(openingbal) + fromkcc + marcketreturn + oterreturn;
                //        double sub = salesquantity + cutting;
                //        newrow["Closing Bal"] = Math.Round((add - sub), 4);
                //        cbob = (add - sub).ToString();
                //        closingqty += (add - sub);
                //        prvdate = date;
                //        dtReport.Rows.Add(newrow);
                //    }
                //}
                DataRow newrow2 = dtReport.NewRow();
                newrow2["Product Name"] = "Total";
                newrow2["Opening Bal"] = Math.Round(obbalqty, 4).ToString();
                newrow2["From KCC"] = Math.Round(frmkccqty, 4).ToString();
                newrow2["Market Return"] = Math.Round(mrkretunqty, 4).ToString();
                newrow2["Other Return"] = Math.Round(oterrtnqty, 4).ToString();
                newrow2["Sales Qty"] = Math.Round(salesqty, 4).ToString();
                newrow2["Cutting"] = Math.Round(cuttingqty, 4).ToString();
                newrow2["Closing Bal"] = Math.Round(closingqty, 4).ToString();
                dtReport.Rows.Add(newrow2);

                DataRow newrow3 = dtReport.NewRow();
                newrow3["Product Name"] = "Grand Total";
                newrow3["From KCC"] = Math.Round(totfrmkccqty, 4).ToString();
                newrow3["Market Return"] = Math.Round(totmrkretunqty, 4).ToString();
                newrow3["Other Return"] = Math.Round(tototerrtnqty, 4).ToString();
                newrow3["Sales Qty"] = Math.Round(totsalesqty, 4).ToString();
                newrow3["Cutting"] = Math.Round(totcuttingqty, 4).ToString();
                dtReport.Rows.Add(newrow3);

                grdsales.DataSource = dtReport;
                grdsales.DataBind();
                Session["xportdata"] = dtReport;
                hidepanel.Visible = true;
            }
            else
            {
                // lblmsg.Text = "No data were found";
                grdsales.DataSource = null;
                grdsales.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void grdReports_RowDataBounds(object sender, GridViewRowEventArgs e)
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