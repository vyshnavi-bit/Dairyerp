using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class sapreturnmilk : System.Web.UI.Page
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
        btn_Generate_returnmilk();
    }

    protected void btn_Save_Production_Click(object sender, EventArgs e)
    {
        btn_save_curdsection();
    }

    void btn_Generate_returnmilk()
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

            Report.Columns.Add("CreateDate");
            Report.Columns.Add("PostingDate");
            Report.Columns.Add("DocDate");
            Report.Columns.Add("ItemCode");
            Report.Columns.Add("ItemName");
            Report.Columns.Add("Production Quantity");
            Report.Columns.Add("WhsCode");
            Report.Columns.Add("Price");
            Report.Columns.Add("Account");
            Report.Columns.Add("Remarks");

            cmd = new SqlCommand("SELECT returnmilk_details.doe, processingdepartments.departmentname, silomaster.SiloName, returnmilk_details.quantity,returnmilk_details.fat,returnmilk_details.snf,returnmilk_details.clr,returnmilk_details.qty_ltr FROM returnmilk_details INNER JOIN processingdepartments ON returnmilk_details.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON returnmilk_details.siloid = silomaster.SiloId WHERE (returnmilk_details.branchid = @BranchID) AND (returnmilk_details.doe BETWEEN @d1 AND @d2)");
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
                    DataRow newrow = Report.NewRow();
                    DateTime dtdate = Convert.ToDateTime(dr["doe"].ToString());
                    newrow["CreateDate"] = dtdate.ToString("dd/MM/yyyy");
                    newrow["PostingDate"] = dtdate.ToString("dd/MM/yyyy");
                    newrow["DocDate"] = dtdate.ToString("dd/MM/yyyy");
                    newrow["ItemName"] = dr["productname"].ToString();
                    newrow["ItemCode"] = dr["productcode"].ToString();
                    newrow["Production Quantity"] = dr["production"].ToString();
                    newrow["Remarks"] = dr["remarks"].ToString();
                    newrow["WhsCode"] = "SVDSPP02";
                    newrow["Account"] = "5134004";
                    Report.Rows.Add(newrow);
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
            string fyear = CYER + '-' + NYEAR;
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
                cmd = new SqlCommand("SELECT * FROM EMROIGN WHERE CreateDate BETWEEN @d1 AND @d2");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(fromdate));
                DataTable dtdata = SAPvdm.SelectQuery(cmd).Tables[0];
                if (dtdata.Rows.Count < 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string date = dr["CreateDate"].ToString();
                        string[] str = date.Split('/');
                        string dtdate = str[0].ToString();
                        string month = str[1].ToString();
                        string year = str[2].ToString();
                        string refno = "SVDS/PBK/" + fyear + "/" + dtdate + "" + month + "/" + i++ + "";
                        cmd = new SqlCommand("Insert into EMROIGN (CreateDate,PostingDate,DocDate,ReferenceNo,ItemCode,ItemName,Quantity,WhsCode,Price,Account,OcrCode,Remarks,B1Upload,Processed) values (@CreateDate,@PostingDate,@DocDate,@ReferenceNo,@ItemCode,@ItemName,@Quantity,@WhsCode,@Price,@Account,@OcrCode,@Remarks,@B1Upload,@Processed)");
                        string CDate = dr["CreateDate"].ToString();
                        string PostingDate = dr["PostingDate"].ToString();
                        string DocDate = dr["DocDate"].ToString();
                        cmd.Parameters.Add("@CreateDate", CDate);
                        cmd.Parameters.Add("@PostingDate", PostingDate);
                        cmd.Parameters.Add("@DocDate", DocDate);
                        cmd.Parameters.Add("@ReferenceNo", refno);
                        cmd.Parameters.Add("@ItemCode", dr["ItemCode"].ToString());
                        cmd.Parameters.Add("@ItemName", dr["ItemName"].ToString());
                        cmd.Parameters.Add("@Quantity", dr["Production Quantity"].ToString());
                        cmd.Parameters.Add("@WhsCode", dr["WhsCode"].ToString());
                        cmd.Parameters.Add("@Price", "0");
                        string ledger = "5134004";
                        cmd.Parameters.Add("@Account", ledger);
                        cmd.Parameters.Add("@OcrCode", dr["WhsCode"].ToString());
                        cmd.Parameters.Add("@Remarks", dr["Remarks"].ToString());
                        string B1Upload = "N";
                        string Processed = "N";
                        cmd.Parameters.Add("@B1Upload", B1Upload);
                        cmd.Parameters.Add("@Processed", Processed);
                        SAPvdm.insert(cmd);
                    }
                    DataTable dtempty = new DataTable();
                    lblmsg.Text = "Successfully Saved";
                }
                else
                {
                    lblmsg.Text = "This Date Data Alredy Exist In Datbase Plese Check it";
                    grdReports.DataSource = null;
                    grdReports.DataBind();
                    Session["xportdata"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }
}