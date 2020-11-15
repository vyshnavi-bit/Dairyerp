using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class sapproduction : System.Web.UI.Page
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

            cmd = new SqlCommand("SELECT   sno, branchname, branchcode FROM branch_info WHERE  (sno = @branchid)");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtbranch = SalesDB.SelectQuery(cmd).Tables[0];
            string branchcode = "";
            foreach (DataRow dr in dtbranch.Rows)
            {
                branchcode = dr["branchcode"].ToString();
            }
            string refno = "" + branchcode + "/" + aprilyear + "" + marchyear + "/" + dtdate + "" + month + "";

            //curd section
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
            cmd = new SqlCommand("Select cpd.sno, cpd.productid, cpd.fat, cpd.snf, cpd.qty_kgs, cpd.ob, cpd.productionqty as production, cpd.total, cpd.remarks, cpd.sales, cpd.createdby, cpd.branchid, cpd.createdon AS doe, pm.productname, pm.productcode, pm.price, pm.categorycode, bi.branchname,bi.whcode from plant_production_details cpd INNER JOIN productmaster pm ON pm.sno = cpd.productid INNER JOIN branch_info bi on bi.sno = cpd.branchid  WHERE  (cpd.createdon BETWEEN @d1 AND @d2) AND (cpd.branchid=@branchid) AND (cpd.deptid='1') and (cpd.productid !='92') and (cpd.productid !='93')  ORDER BY cpd.doe");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(fromdate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtDispatch = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double closingbalance = 0;
                double total = 0;
                double sales = 0;
                int i = 1;
                foreach (DataRow dr in dtDispatch.Rows)
                {
                    string qty = dr["production"].ToString();
                    double qtyd = Convert.ToDouble(qty);
                    if (qtyd > 0)
                    {
                        string ItemCode = dr["productcode"].ToString();
                        if (ItemCode != "")
                        {
                            DataRow newrow = Report.NewRow();
                            DateTime dtcpddate = Convert.ToDateTime(dr["doe"].ToString());
                            newrow["CreateDate"] = dtcpddate.ToString("MM/dd/yyyy");
                            newrow["PostingDate"] = dtcpddate.ToString("MM/dd/yyyy");
                            newrow["DocDate"] = dtcpddate.ToString("MM/dd/yyyy");
                            newrow["ItemName"] = dr["productname"].ToString();
                            newrow["ItemCode"] = dr["productcode"].ToString();
                            newrow["Production Quantity"] = dr["production"].ToString();
                            newrow["Remarks"] = dr["remarks"].ToString();
                            newrow["WhsCode"] = dr["whcode"].ToString();
                            newrow["Account"] = "5134004";
                            newrow["Price"] = dr["price"].ToString();
                            newrow["Series"] = "19";
                            newrow["Ocrcode2"] = dr["categorycode"].ToString();
                            newrow["RefNo"] = refno;
                            Report.Rows.Add(newrow);
                        }
                    }
                }
            }

            //packing

            cmd = new SqlCommand("SELECT  batchmaster.batch, packing_entry.qty_ltr, packing_entry.fat, packing_entry.snf, packing_entry.clr, packing_entry.received_film, packing_entry.doe, productmaster.ml,  productmaster.productname, productmaster.categorycode, productmaster.price, productmaster.productcode, packing_entry.branchid, branch_info.whcode FROM  packing_entry INNER JOIN batchmaster ON packing_entry.batchid = batchmaster.batchid INNER JOIN productmaster ON packing_entry.productid = productmaster.sno INNER JOIN branch_info ON packing_entry.branchid = branch_info.sno WHERE  (packing_entry.doe BETWEEN @dt1 AND @dt2) AND (packing_entry.branchid = @branchid) ORDER BY packing_entry.doe DESC");
            cmd.Parameters.Add("@dt1", GetLowDate(fromdate));
            cmd.Parameters.Add("@dt2", GetHighDate(fromdate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtpacking = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtpacking.Rows.Count > 0)
            {
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double closingbalance = 0;
                double total = 0;
                double sales = 0;
                int i = 1;
                double packed = 0;
                foreach (DataRow dp in dtpacking.Rows)
                {
                    string qty = dp["qty_ltr"].ToString();
                    double qtyd = Convert.ToDouble(qty);
                    if (qtyd > 0)
                    {
                        string ItemCode = dp["productcode"].ToString();
                        if (ItemCode != "")
                        {
                            DataRow newrow = Report.NewRow();
                            DateTime dtpkgdate = Convert.ToDateTime(dp["doe"].ToString());
                            newrow["CreateDate"] = dtpkgdate.ToString("MM/dd/yyyy");
                            newrow["PostingDate"] = dtpkgdate.ToString("MM/dd/yyyy");
                            newrow["DocDate"] = dtpkgdate.ToString("MM/dd/yyyy");
                            newrow["ItemName"] = dp["productname"].ToString();
                            newrow["ItemCode"] = dp["productcode"].ToString();
                            newrow["Production Quantity"] = dp["qty_ltr"].ToString();
                            newrow["Remarks"] = "";
                            newrow["WhsCode"] = dp["whcode"].ToString();
                            newrow["Account"] = "5134004";
                            newrow["Price"] = dp["price"].ToString();
                            newrow["Series"] = "19";
                            newrow["Ocrcode2"] = dp["categorycode"].ToString();
                            newrow["RefNo"] = refno;
                            Report.Rows.Add(newrow);
                        }
                    }
                }
            }
            //ghee
            cmd = new SqlCommand("SELECT   gp.sno, pm.productname, pm.price, pm.categorycode, pm.productcode, gp.productid, gp.remarks, gp.creamtype, gp.convertionquantity, gp.convertionfat,  gp.productionqty, gp.ob as openingbalance, gp.sales, gp.cb as closingbalance, gp.createdon as doe, branch_info.whcode, gp.branchid FROM plant_production_details AS gp INNER JOIN productmaster AS pm ON pm.sno = gp.productid INNER JOIN branch_info ON gp.branchid = branch_info.sno WHERE (gp.createdon BETWEEN @d1 AND @d2) AND (gp.branchid = @BranchID) AND (gp.branchid = @branchid) AND gp.deptid='3' ORDER BY gp.doe");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(fromdate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtghee = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtghee.Rows.Count > 0)
            {
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double closingbalance = 0;
                double total = 0;
                double sales = 0;
                int i = 1;
                foreach (DataRow dr in dtghee.Rows)
                {
                    string qty = dr["productionqty"].ToString();
                    double qtyd = Convert.ToDouble(qty);
                    if (qtyd > 0)
                    {
                        string ItemCode = dr["productcode"].ToString();
                        if (ItemCode != "")
                        {
                            DataRow newrow = Report.NewRow();
                            DateTime dtgpdate = Convert.ToDateTime(dr["doe"].ToString());
                            newrow["CreateDate"] = dtgpdate.ToString("MM/dd/yyyy");
                            newrow["PostingDate"] = dtgpdate.ToString("MM/dd/yyyy");
                            newrow["DocDate"] = dtgpdate.ToString("MM/dd/yyyy");
                            newrow["ItemName"] = dr["productname"].ToString();
                            newrow["ItemCode"] = dr["productcode"].ToString();
                            newrow["Production Quantity"] = dr["productionqty"].ToString();
                            newrow["Remarks"] = dr["remarks"].ToString();
                            newrow["WhsCode"] = dr["whcode"].ToString();
                            newrow["Account"] = "5134004";
                            newrow["Price"] = dr["price"].ToString();
                            newrow["Series"] = "19";
                            newrow["Ocrcode2"] = dr["categorycode"].ToString();
                            newrow["RefNo"] = refno;
                            Report.Rows.Add(newrow);
                        }
                    }
                }
            }

            // butter
            cmd = new SqlCommand("SELECT   bp.sno, pm.productname, pm.categorycode, pm.price, pm.productcode, bp.productid, bp.remarks, bp.creamtype, bp.convertionquantity, bp.convertionfat,  bp.productionqty, bp.ob as openingbalance, bp.sales, bp.cb as closingbalance, bp.createdon as doe, branch_info.whcode, bp.branchid FROM  plant_production_details AS bp INNER JOIN productmaster AS pm ON pm.sno = bp.productid INNER JOIN branch_info ON bp.branchid = branch_info.sno WHERE  (bp.createdon BETWEEN @d1 AND @d2) AND (bp.branchid = @branchid) and bp.deptid='10' ORDER BY bp.doe");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(fromdate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtbutter = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtbutter.Rows.Count > 0)
            {
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double closingbalance = 0;
                double total = 0;
                double sales = 0;
                int i = 1;
                foreach (DataRow dr in dtbutter.Rows)
                {
                    string qty = dr["productionqty"].ToString();
                    double qtyd = Convert.ToDouble(qty);
                    if (qtyd > 0)
                    {
                        string ItemCode = dr["productcode"].ToString();
                        if (ItemCode != "")
                        {
                            DataRow newrow = Report.NewRow();
                            DateTime dtbpdate = Convert.ToDateTime(dr["doe"].ToString());
                            newrow["CreateDate"] = dtbpdate.ToString("MM/dd/yyyy");
                            newrow["PostingDate"] = dtbpdate.ToString("MM/dd/yyyy");
                            newrow["DocDate"] = dtbpdate.ToString("MM/dd/yyyy");
                            newrow["ItemName"] = dr["productname"].ToString();
                            newrow["ItemCode"] = dr["productcode"].ToString();
                            newrow["Production Quantity"] = dr["productionqty"].ToString();
                            newrow["Remarks"] = dr["remarks"].ToString();
                            newrow["WhsCode"] = dr["whcode"].ToString();
                            newrow["Account"] = "5134004";
                            newrow["Price"] = dr["price"].ToString();
                            newrow["Series"] = "19";
                            newrow["Ocrcode2"] = dr["categorycode"].ToString();
                            newrow["RefNo"] = refno;
                            Report.Rows.Add(newrow);
                        }
                    }
                }
            }

            //Other products
            cmd = new SqlCommand("SELECT  opd.sno, opd.createdon as doe, opd.branchid, opd.productid, opd.inward, opd.issue, opd.remarks, opd.createdby, opd.ob AS openingbalance, pm.productname, pm.sno AS Expr1,  pm.batchid, pm.branchid AS Expr2, pm.departmentid, pm.ml, pm.filimrate, pm.fat, pm.snf, pm.clr, pm.productcode, pm.price, pm.categorycode,  branch_info.whcode FROM  plant_production_details AS opd INNER JOIN productmaster AS pm ON opd.productid = pm.sno INNER JOIN branch_info ON opd.branchid = branch_info.sno WHERE   (opd.doe BETWEEN @d1 AND @d2) AND (opd.branchid = @branchid) AND (opd.deptid = 16) ORDER BY opd.createdon");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(fromdate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtotherproducts = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtotherproducts.Rows.Count > 0)
            {
                foreach (DataRow dr in dtotherproducts.Rows)
                {
                    string qty = dr["inward"].ToString();
                    double qtyd = Convert.ToDouble(qty);
                    if (qtyd > 0)
                    {
                        string ItemCode = dr["productcode"].ToString();
                        if (ItemCode != "")
                        {
                            DataRow newrow = Report.NewRow();
                            DateTime dtbpdate = Convert.ToDateTime(dr["doe"].ToString());
                            newrow["CreateDate"] = dtbpdate.ToString("MM/dd/yyyy");
                            newrow["PostingDate"] = dtbpdate.ToString("MM/dd/yyyy");
                            newrow["DocDate"] = dtbpdate.ToString("MM/dd/yyyy");
                            newrow["ItemName"] = dr["productname"].ToString();
                            newrow["ItemCode"] = dr["productcode"].ToString();
                            newrow["Production Quantity"] = dr["inward"].ToString();
                            newrow["Remarks"] = dr["remarks"].ToString();
                            newrow["WhsCode"] = dr["whcode"].ToString();
                            newrow["Account"] = "5134004";
                            newrow["Price"] = dr["price"].ToString();
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