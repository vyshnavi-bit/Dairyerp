using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class deliverychallanreport : System.Web.UI.Page
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
                    txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                    //lbltinNo.Text = Session["TinNo"].ToString();
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
    protected void btn_getdetails_Click(object sender, EventArgs e)
    {
        try
        {
            lbldateValidation.Text = "";
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] datestrig = txtdate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            datestrig = txttodate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            if (BranchID == "1" || BranchID == "23")
            {
                //SELECT silo_outward_transaction.sno AS ref_no,processingdepartments.departmentname, silo_outward_transaction.date, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf, processingdepartments.departmentname FROM silo_outward_transaction INNER JOIN processingdepartm nts ON silo_outward_transaction.departmentid = processingdepartments.departmentid WHERE (silo_outward_transaction.date BETWEEN @d1 AND @d2) AND (silo_outward_transaction.departmentid = '3' OR silo_outward_transaction.departmentid = '10') AND (processingdepartments.branchid = @branchid)
                cmd = new SqlCommand("SELECT  silo_outward_transaction.sno, silo_outward_transaction.date, processingdepartments.departmentname, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf, silomaster.SiloName FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId WHERE (silo_outward_transaction.branchid = @BranchID) AND (silo_outward_transaction.date BETWEEN @d1 AND @d2) AND (processingdepartments.departmentname = 'Ghee' OR processingdepartments.departmentname = 'Butter Section')");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
            }
            else
            {

            }
            DataTable dtDispatch = vdm.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                Gridcdata.DataSource = dtDispatch;
                Gridcdata.DataBind();
            }
            else
            {
                lbldateValidation.Text = "No data were found";
            }
        }
        catch (Exception ex)
        {
            lbldateValidation.Text = ex.Message;

        }
    }
        DataTable Report = new DataTable();
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                lblmsg.Text = "";
                
                SalesDBManager SalesDB = new SalesDBManager();
                if (BranchID == "1" || BranchID == "26")
                {
                    //SELECT silo_outward_transaction.sno AS ref_no, silo_outward_transaction.date, processingdepartments.departmentname, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid WHERE (silo_outward_transaction.date BETWEEN @d1 AND @d2) AND (processingdepartments.branchid = @branchid) AND (silo_outward_transaction.sno = @ref_no)
                    //cmd = new SqlCommand("SELECT silo_outward_transaction.sno AS ref_no, silo_outward_transaction.date, processingdepartments.departmentname, silo_outward_transaction.qty_kgs, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat, silo_outward_transaction.snf FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid WHERE (processingdepartments.branchid = @branchid) AND (silo_outward_transaction.sno = @RefDcNo)");
                    cmd = new SqlCommand("SELECT silo_outward_transaction.sno AS ref_no, silo_outward_transaction.date, processingdepartments.departmentname as NameOfTheItem, silo_outward_transaction.qty_kgs as Quantity, silo_outward_transaction.qty_ltrs, silo_outward_transaction.fat as FAT, silo_outward_transaction.snf AS SNF, silomaster.SiloName FROM silo_outward_transaction INNER JOIN processingdepartments ON silo_outward_transaction.departmentid = processingdepartments.departmentid INNER JOIN silomaster ON silo_outward_transaction.siloid = silomaster.SiloId WHERE (processingdepartments.branchid = @branchid) AND (silo_outward_transaction.sno = @RefDcNo)");
                    cmd.Parameters.Add("@RefDcNo", txt_refdcno.Text);
                    cmd.Parameters.Add("@BranchID", BranchID);
                }
                else
                {

                }

                DataTable dtDispatch = vdm.SelectQuery(cmd).Tables[0];
                if (dtDispatch.Rows.Count > 0)
                {
                    DataView view1 = new DataView(dtDispatch);
                    DataTable dtlblValues = view1.ToTable(true, "ref_no", "date", "SiloName", "NameOfTheItem");
                    DataTable dtgrdValues = view1.ToTable(true, "NameOfTheItem", "Quantity", "FAT", "SNF");
                    if (dtlblValues.Rows.Count > 0)
                    {
                        lblRefdcno.Text = txt_refdcno.Text;
                        //string branchcode = dtlblValues.Rows[0]["branchcode"].ToString();
                        lblRefdcno.Text = dtlblValues.Rows[0]["ref_no"].ToString();
                        lblassigndate.Text = dtlblValues.Rows[0]["date"].ToString();
                        lblfrom.Text = "Processing Section";
                        lblto.Text = dtlblValues.Rows[0]["NameOfTheItem"].ToString();
                    }
                    if (dtgrdValues.Rows.Count > 0)
                    {
                        grdReports.DataSource = dtgrdValues;
                        grdReports.DataBind();
                    }
                    //string barname = dtlblValues.Rows[0]["branchcode"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "generateBarcode('');", true);
                    pnlHide.Visible = true;
                    
                }
                else
                {
                    lblmsg.Text = "No data were found";
                    pnlHide.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
                pnlHide.Visible = false;
            }
        }
    
}