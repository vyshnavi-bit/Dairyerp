using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

public partial class kuppamproductionreport : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
    MySqlCommand mycmd;
    VehicleDBMgr vdmsale;
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
        btn_Generate_curdsection();
    }
    protected void btn_Save_Production_Click(object sender, EventArgs e)
    {
        btn_save_curdsection();
    }
    void btn_Generate_curdsection()
    {
        try
        {
            DataTable Report = new DataTable();
            SalesDBManager SalesDB = new SalesDBManager();
            VehicleDBMgr vehicleDB = new VehicleDBMgr();
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
            //DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
            DateTime dtapril = new DateTime();
            DateTime dtmarch = new DateTime();
            int currentyear = fromdate.Year;
            int nextyear = fromdate.Year + 1;
            if (fromdate.Month > 3)
            {
                string apr = "4/1/" + currentyear;
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + nextyear;
                dtmarch = DateTime.Parse(march);
            }
            if (fromdate.Month <= 3)
            {
                string apr = "4/1/" + (currentyear - 1);
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + (nextyear - 1);
                dtmarch = DateTime.Parse(march);
            }

            string fyear = "1718";
            string date = fromdate.ToString("dd/MM/yy");
            string[] str = date.Split('/');
            string dtdate = str[0].ToString();
            string month = str[1].ToString();
            string year = str[2].ToString();

            string dmarch = dtmarch.ToString("dd/MM/yy");
            string[] strm = dmarch.Split('/');
            string dtmarchdate = strm[0].ToString();
            string marchmonth = strm[1].ToString();
            string marchyear = strm[2].ToString();

            string dapril = dtapril.ToString("dd/MM/yy");
            string[] stra = dapril.Split('/');
            string dtaprildate = stra[0].ToString();
            string aprilmonth = stra[1].ToString();
            string aprilyear = stra[2].ToString();

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
            string refno = "" + branchcode + "/" + aprilyear + "" + marchyear + "/" + dtdate + "" + month + "";

            mycmd = new MySqlCommand("select sno,whcode from branchdata where whcode=@whcode");
            mycmd.Parameters.Add("@whcode", whcode);
            DataTable dtwhcode = vehicleDB.SelectQuery(mycmd).Tables[0];
            string salesbranchid = dtwhcode.Rows[0]["sno"].ToString();

            Report.Columns.Add("CreateDate");
            Report.Columns.Add("PostingDate");
            Report.Columns.Add("DocDate");
            Report.Columns.Add("ItemCode");
            Report.Columns.Add("ItemName");
            Report.Columns.Add("Production Quantity");
            Report.Columns.Add("WhsCode");
            Report.Columns.Add("Price");
            Report.Columns.Add("Account");
            Report.Columns.Add("Series");
            Report.Columns.Add("Ocrcode2");
            Report.Columns.Add("RefNo");
            Report.Columns.Add("Remarks");

            mycmd = new MySqlCommand("SELECT   SUM(ProductInfo.Qty) AS Qty, TripInfo.Sno, TripInfo.DCNo, ProductInfo.ProductName, ProductInfo.Categoryname, ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName,  TripInfo.DispType, TripInfo.DispMode, ProductInfo.Itemcode, ProductInfo.UnitPrice, ProductInfo.categorycode, TripInfo.whcode FROM  (SELECT        tripdata.Sno, tripdata.DCNo, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode, branchdata.whcode FROM   branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE  (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT  Categoryname, ProductName, Sno, Qty, Itemcode, UnitPrice, categorycode FROM  (SELECT  products_category.Categoryname, productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty, productsdata.Itemcode, productsdata.UnitPrice, products_category.categorycode FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno GROUP BY  ProductInfo.ProductName  ORDER BY ProductInfo.ProductName");
            mycmd.Parameters.Add("@branch", salesbranchid);
            mycmd.Parameters.Add("@d1", GetLowDate(fromdate).AddDays(-1));
            mycmd.Parameters.Add("@d2", GetHighDate(fromdate).AddDays(-1));
            DataTable dtsaledispatch = vehicleDB.SelectQuery(mycmd).Tables[0];

            // DataTable dispatchqty = vdmnr.SelectQuery(mycmd).Tables[0];
            //DataTable temp = new DataTable();
            //temp = dtsaledispatch.Copy();
            //DataView view = new DataView(dtsaledispatch);
            //DataTable distinctproducts = view.ToTable(true, "Itemcode", "ProductName");
            //DataTable AllProducts = new DataTable();
            //AllProducts.Columns.Add("Itemcode");
            //AllProducts.Columns.Add("ProductName");
            //foreach (DataRow drd in dtsaledispatch.Rows)
            //{
            //    DataRow[] data_exist = AllProducts.Select("Itemcode='" + drd["Itemcode"].ToString() + "'");
            //    if (data_exist.Length > 0)
            //    {
            //    }
            //    else
            //    {
            //        DataRow newrow = AllProducts.NewRow();
            //        newrow["Itemcode"] = drd["Itemcode"].ToString();
            //        newrow["ProductName"] = drd["ProductName"].ToString();
            //        AllProducts.Rows.Add(newrow);
            //    }
            //}

            if (dtsaledispatch.Rows.Count > 0)
            {
                foreach (DataRow dr in dtsaledispatch.Rows)
                {
                    string Categoryname = dr["Categoryname"].ToString();
                    if (Categoryname == "MILK")
                    {
                        double totaldispqtyqty = 0;
                        DataRow[] itemdr = dtsaledispatch.Select("Itemcode='" + dr["Itemcode"].ToString() + "'");
                        foreach (DataRow drdisp in itemdr)
                        {
                            double dispqty = 0;
                            double.TryParse(drdisp["Qty"].ToString(), out dispqty);
                            totaldispqtyqty += dispqty;
                        }
                        string qty = dr["Qty"].ToString();
                        double qtyd = Convert.ToDouble(qty);
                        if (qtyd > 0)
                        {
                            DataRow newrow = Report.NewRow();
                            //DateTime dtcpddate = Convert.ToDateTime(dr["fromdate"].ToString());
                            newrow["CreateDate"] = fromdate.ToString("MM/dd/yyyy");
                            newrow["PostingDate"] = fromdate.ToString("MM/dd/yyyy");
                            newrow["DocDate"] = fromdate.ToString("MM/dd/yyyy");
                            newrow["ItemName"] = dr["ProductName"].ToString();
                            newrow["ItemCode"] = dr["Itemcode"].ToString();
                            newrow["Production Quantity"] = totaldispqtyqty.ToString();
                            newrow["Remarks"] = "";
                            newrow["WhsCode"] = dr["whcode"].ToString();
                            newrow["Account"] = "5134004";
                            newrow["Price"] = dr["UnitPrice"].ToString();
                            newrow["Series"] = "19";
                            newrow["Ocrcode2"] = dr["categorycode"].ToString();
                            newrow["RefNo"] = refno;
                            Report.Rows.Add(newrow);
                        }
                    }
                }
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
            hidepanel.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
    void btn_save_curdsection()
    {
        try
        {
            SalesDBManager SalesDB = new SalesDBManager();
            DateTime CreateDate = sapDBmanager.GetTime(vdm.conn);
            sapDBmanager SAPvdm = new sapDBmanager();
            DateTime fromdate = DateTime.Now;
            int currentyear = (fromdate.Year);
            int NEXTYEAR = currentyear + 1;
            string CYER = currentyear.ToString();
            string NYEAR = NEXTYEAR.ToString();
            string fyear = "1718";
            // fyear = 
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
            DateTime ServerDateCurrentdate = sapDBmanager.GetTime(vdm.conn);
            string datetime = ServerDateCurrentdate.ToString("MM/dd/yyyy");
            string BranchID = Session["Branch_ID"].ToString();
            DataTable dt = (DataTable)Session["xportdata"];
            if (dt.Rows.Count > 0)
            {
                DateTime doe = DateTime.Now;
                int i = 1;

                foreach (DataRow dr in dt.Rows)
                {
                    string ocrcode2 = "";
                    string DocDate = dr["DocDate"].ToString();
                    DateTime dtdoc = Convert.ToDateTime(DocDate);
                    cmd = new SqlCommand("SELECT PostingDate,DocDate FROM EMROIGN WHERE DocDate BETWEEN @d1 and @d2 AND ReferenceNo=@rno AND ItemCode=@ItemCode AND ItemName=@ItemName and WhsCode=@WhsCode");
                    cmd.Parameters.Add("@d1", GetLowDate(dtdoc));
                    cmd.Parameters.Add("@d2", GetHighDate(dtdoc));
                    cmd.Parameters.Add("@rno", dr["RefNo"].ToString());
                    cmd.Parameters.Add("@ItemCode", dr["ItemCode"].ToString());
                    cmd.Parameters.Add("@ItemName", dr["ItemName"].ToString());
                    cmd.Parameters.Add("@WhsCode", dr["WhsCode"].ToString());
                    DataTable dtSAPproduction = SAPvdm.SelectQuery(cmd).Tables[0];
                    if (dtSAPproduction.Rows.Count > 0)
                    {
                        lblmsg.Text = "This date data already Saved";
                    }
                    else
                    {
                        cmd = new SqlCommand("Insert into EMROIGN (CreateDate,PostingDate,DocDate,ReferenceNo,ItemCode,ItemName,Quantity,WhsCode,Price,OcrCode,Remarks,B1Upload,Processed,Series,OcrCode2) values (@CreateDate,@PostingDate,@DocDate,@ReferenceNo,@ItemCode,@ItemName,@Quantity,@WhsCode,@Price,@OcrCode,@Remarks,@B1Upload,@Processed,@Series,@OcrCode2)");
                        string CDate = dr["CreateDate"].ToString();
                        string PostingDate = dr["PostingDate"].ToString();
                        cmd.Parameters.Add("@CreateDate", CDate);
                        cmd.Parameters.Add("@PostingDate", PostingDate);
                        cmd.Parameters.Add("@DocDate", DocDate);
                        cmd.Parameters.Add("@ReferenceNo", dr["RefNo"].ToString());
                        cmd.Parameters.Add("@ItemCode", dr["ItemCode"].ToString());
                        cmd.Parameters.Add("@ItemName", dr["ItemName"].ToString());
                        cmd.Parameters.Add("@Quantity", dr["Production Quantity"].ToString());
                        cmd.Parameters.Add("@WhsCode", dr["WhsCode"].ToString());
                        cmd.Parameters.Add("@Price", dr["Price"].ToString());
                        //string ledger = "0";
                        //cmd.Parameters.Add("@Account", ledger);
                        cmd.Parameters.Add("@OcrCode", dr["WhsCode"].ToString());
                        cmd.Parameters.Add("@Remarks", dr["Remarks"].ToString());
                        cmd.Parameters.Add("@Series", "19");
                        cmd.Parameters.Add("@OcrCode2", dr["Ocrcode2"].ToString());
                        string B1Upload = "N";
                        string Processed = "N";
                        cmd.Parameters.Add("@B1Upload", B1Upload);
                        cmd.Parameters.Add("@Processed", Processed);
                        SAPvdm.insert(cmd);
                    }
                }
            }
            DataTable dtempty = new DataTable();
            lblmsg.Text = "Successfully Saved";
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }
}