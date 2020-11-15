using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

public partial class summaryreports : System.Web.UI.Page
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
                    DateTime dt = DateTime.Now.AddDays(-1);
                    dtp_FromDate.Text = dt.ToString("dd-MM-yyyy HH:mm");
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
    protected void btn_Generate_Click(object sender, EventArgs e)
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
            if (datestrig[0].Split('-').Length > 0)
            {
                string[] dates = datestrig[0].Split('-');
                string[] times = datestrig[1].Split(':');
                todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            }
        }
        lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
        lblmsg.Text = "";
        DataTable Report = new DataTable();
        Report.Columns.Add("TankerInward");
        Report.Columns.Add("Outward");
        Report.Columns.Add("SaleQuantity");
        Report.Columns.Add("TankerSale");
        Report.Columns.Add("TotalSale");
        Report.Columns.Add("RemainingMilk");
        Report.Columns.Add("ReturnMilk");
        Report.Columns.Add("Closing");
        cmd = new SqlCommand("SELECT  SUM(qty_ltr) AS qtyltrs, SUM(qty_kgs) AS qtykgs FROM  milktransactions WHERE (entrydate BETWEEN @d1 AND @d2) AND (branchid = @branchid) AND (transtype = 'in')");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@branchid", BranchID);
        double tankerquantityltrs = 0;
        double tankerquantitykgs = 0;
        DataTable dttankerin = SalesDB.SelectQuery(cmd).Tables[0];
        if (dttankerin.Rows.Count > 0)
        {
            foreach (DataRow dr in dttankerin.Rows)
            {
                double.TryParse(dr["qtyltrs"].ToString(), out tankerquantityltrs); //dr["qtyltr"].ToString();
                double.TryParse(dr["qtykgs"].ToString(), out tankerquantitykgs);  //tankerquantitykgs = dr["qtykg"].ToString();
            }
        }
        double tankeroutquantityltrs = 0;
        double tankeroutquantitykgs = 0;
        cmd = new SqlCommand("SELECT SUM(qty_kgs) AS qtykgs, SUM(qty_ltr) AS qtyltrs FROM directsale WHERE (doe BETWEEN @d1 AND @d2) AND (branchid = @branchid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dttankerout = SalesDB.SelectQuery(cmd).Tables[0];
        if (dttankerout.Rows.Count > 0)
        {
            foreach (DataRow dr in dttankerout.Rows)
            {
                double.TryParse(dr["qtyltrs"].ToString(), out tankeroutquantityltrs);  //siloinquantityltrs = dr["qtyltrs"].ToString();
                double.TryParse(dr["qtykgs"].ToString(), out tankeroutquantitykgs);  //siloinquantitykgs = dr["qtykgs"].ToString();
            }
        }
        double silooutquantityltrs = 0;
        double silooutquantitykgs = 0;
        cmd = new SqlCommand("SELECT  SUM(qty_kgs) AS qtykgs, SUM(qty_ltrs) AS qtyltrs FROM  silo_outward_transaction WHERE (date BETWEEN @d1 AND @d2) AND (branchid = @branchid)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dtsiloout = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtsiloout.Rows.Count > 0)
        {
            foreach (DataRow dr in dtsiloout.Rows)
            {
                double.TryParse(dr["qtyltrs"].ToString(), out silooutquantityltrs);  //siloinquantityltrs = dr["qtyltrs"].ToString();
                double.TryParse(dr["qtykgs"].ToString(), out silooutquantitykgs);  //siloinquantitykgs = dr["qtykgs"].ToString();
            }
        }
        cmd = new SqlCommand("SELECT  SUM(quantity) AS quantity FROM returnmilk_details WHERE  (doe BETWEEN @d1 AND @d2)");
        cmd.Parameters.Add("@d1", GetLowDate(fromdate));
        cmd.Parameters.Add("@d2", GetHighDate(todate));
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dtreturn = SalesDB.SelectQuery(cmd).Tables[0];
        double returnmilkquantity = 0;
        if (dtreturn.Rows.Count > 0)
        {
            foreach (DataRow dr in dtreturn.Rows)
            {
                double.TryParse(dr["quantity"].ToString(), out returnmilkquantity); //returnmilkquantity = dr["quantity"].ToString();
            }
        }
        VehicleDBMgr vdmnr = new VehicleDBMgr();
        MySqlCommand mycmd;
        if (BranchID == "1")
        {
            mycmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, products_subcategory.SubCatName, products_category.Categoryname, products_category.sno AS categorysno, products_subcategory.sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch) and (products_category.sno ='9') GROUP BY categorysno, products_subcategory.sno ORDER BY categorysno");
            mycmd.Parameters.Add("@branch", 172);
            mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
            mycmd.Parameters.Add("@d2", GetHighDate(fromdate));
        }
        else
        {
            mycmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, products_subcategory.SubCatName, products_category.Categoryname, products_category.sno AS categorysno, products_subcategory.sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch) and (products_category.sno ='9') GROUP BY categorysno, products_subcategory.sno ORDER BY categorysno");
            mycmd.Parameters.Add("@branch", 158);
            mycmd.Parameters.Add("@d1", GetLowDate(fromdate));
            mycmd.Parameters.Add("@d2", GetHighDate(fromdate));
        }
        double despatchkgstotal = 0;
        DataTable dtTotalDespatch_subcategorywise = vdmnr.SelectQuery(mycmd).Tables[0];
        //cmd = new SqlCommand("Select batch from batchmaster");
        //DataTable dtbatch = SalesDB.SelectQuery(cmd).Tables[0];
        if (dtTotalDespatch_subcategorywise.Rows.Count > 0)
        {
            double despatchclrtotal = 0;
            double totaldesp = 0;
            double despatchkgfattotal = 0;
            double despatchkgsnftotal = 0;
            double despatchfattotal = 0;
            double despatchsnftotal = 0;
            foreach (DataRow dr in dtTotalDespatch_subcategorywise.Rows)
            {
                double FAT = 0;
                double SNF = 0;
                double clr = 0;
                double qty = 0;
                double.TryParse(dr["Qty"].ToString(), out qty);
                totaldesp += qty;
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


                despatchclrtotal += clr;
                double modclr = (clr / 1000) + 1;
                double qtyltrkgs = qty * modclr;
                despatchkgstotal += qtyltrkgs;
                double KGFAT = qtyltrkgs * FAT;
                double KGSNF = qtyltrkgs * SNF;
                KGFAT = Math.Round(KGFAT / 100, 2);
                KGSNF = Math.Round(KGSNF / 100, 2);
                despatchkgfattotal += KGFAT;
                despatchkgsnftotal += KGSNF;
                despatchfattotal += FAT;
                despatchsnftotal += SNF;
            }
            totaldesp = Math.Round(totaldesp, 2);
            despatchkgstotal = Math.Round(despatchkgstotal, 2);
        }
        double remaingmilk = 0;
        double remaingmilk1 = 0;
        double remaingmilk2 = 0;
        double remaingmilk3 = 0;
        DataRow newrow = Report.NewRow();
        remaingmilk1 = silooutquantitykgs - (despatchkgstotal + tankeroutquantitykgs);
        remaingmilk3 = remaingmilk1 + returnmilkquantity;
        remaingmilk = remaingmilk3;
        newrow["TankerInward"] = tankerquantitykgs;
        newrow["Outward"] = silooutquantitykgs;
        newrow["SaleQuantity"] = despatchkgstotal;
        newrow["TankerSale"] = tankeroutquantitykgs;
        newrow["TotalSale"] = despatchkgstotal + tankeroutquantitykgs;
        newrow["RemainingMilk"] = remaingmilk1; 
        newrow["ReturnMilk"] = returnmilkquantity;
        newrow["Closing"] = remaingmilk;
        Report.Rows.Add(newrow);
        grdReports.DataSource = Report;
        grdReports.DataBind();
        Session["xportdata"] = Report;
        grdReports.Visible = true;
        hidepanel.Visible = true;
    }
}