using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class DespatchReport : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
    string BranchType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Branch_ID"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            BranchID = Session["Branch_ID"].ToString();
            BranchType = Session["BranchType"].ToString();
            vdm = new SalesDBManager();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    //lblAddress.Text = Session["Address"].ToString();
                    //lblTitle.Text = Session["TitleName"].ToString();
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
    //string strBuffer;
    //public delegate void InvokeDelegate();


    //private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
    //{
    //    strBuffer = SerialPort1.ReadExisting;
    //    txtUser.BeginInvoke(new InvokeDelegate(InvokeMethod));

    //}

    //public void InvokeMethod()
    //{
    //    double s = 0;

    //    if (double.TryParse(strBuffer, s))
    //        txtUser.Text = Strings.Format(s, "Standard");

    //}
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
            
            if (BranchType == "Plant")
            {
                cmd = new SqlCommand("SELECT despatch_entry.sno AS TransactionNo, despatch_entry.dc_no AS DCNo, CONVERT(VARCHAR(10), despatch_entry.doe, 103) AS Date, vendors.vendorname AS VendorName, despatch_entry.vehciecleno AS VehicleNo, despatch_entry.chemist AS Chemist,despatch_entry.remarks AS Remarks FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN branchmapping ON despatch_entry.branchid = branchmapping.subbranch WHERE (despatch_entry.doe BETWEEN @d1 AND @d2) AND (branchmapping.superbranch = @BranchID) AND (despatch_entry.trans_type=@Transtype)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@Transtype", "In");
            }
            else
            {
                cmd = new SqlCommand("SELECT despatch_entry.sno as TransactionNo, despatch_entry.dc_no as DCNo, CONVERT(VARCHAR(10), despatch_entry.doe, 103) AS Date, vendors.vendorname as VendorName, despatch_entry.vehciecleno as VehicleNo, despatch_entry.chemist as Chemist,despatch_entry.remarks as Remarks FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno WHERE  (despatch_entry.branchid = @BranchID) AND (despatch_entry.doe BETWEEN @d1 AND @d2) AND (despatch_entry.trans_type=@Transtype)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@Transtype", "In");
            }
            DataTable dtDispatch = vdm.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                Gridcdata.DataSource = dtDispatch;
                Gridcdata.DataBind();
            }
            else
            {
                lbldateValidation.Text = "No dc were found";
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
            if (BranchType == "Plant")
            {
                //cmd = new SqlCommand("SELECT despatch_sub.cellname AS CellName, despatch_sub.milktype AS MilkType, despatch_sub.fat AS FAT, despatch_sub.snf AS SNF, despatch_sub.qty_ltr AS QtyLtr, despatch_sub.qty_kgs AS QtyKgs, despatch_sub.percentageon AS PercentageOn, despatch_sub.clr AS CLR, despatch_sub.cob1 AS COB, despatch_sub.hs AS HS, despatch_sub.phosps1 AS Phosps, despatch_sub.mbrt AS MBRT, despatch_sub.alcohol AS Alcohol, despatch_sub.temp AS TEMP, vendors.vendorname, despatch_entry.doe, despatch_entry.chemist, despatch_entry.vehciecleno, despatch_entry.sno, despatch_entry.dc_no,vendors.kms,vendors.expectedtime FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno INNER JOIN branchmapping ON despatch_entry.branchid = branchmapping.subbranch WHERE  (despatch_entry.sno = @RefDcNo) AND (branchmapping.superbranch = @BranchID)");
                cmd = new SqlCommand("SELECT despatch_sub.cellname AS CellName,despatch_sub.sealno as SealNo, despatch_sub.milktype AS MilkType, despatch_sub.fat AS FAT,despatch_sub.ot as OT,    despatch_sub.snf AS SNF, despatch_sub.qty_ltr AS QtyLtr, despatch_sub.qty_kgs AS QtyKgs, despatch_sub.percentageon AS PercentageOn, despatch_sub.clr AS CLR, despatch_sub.cob1 AS COB, despatch_sub.hs AS HS, despatch_sub.phosps1 AS Phosps, despatch_sub.mbrt AS MBRT, despatch_sub.alcohol AS Alcohol, despatch_sub.temp AS TEMP, vendors.vendorname, despatch_entry.doe, despatch_entry.chemist, despatch_entry.vehciecleno, despatch_entry.sno, despatch_entry.dc_no, despatch_entry.salestype, vendors.kms, vendors.expectedtime, branch_info.tinno, branch_info.branchcode, branch_info.cstno, branch_info.mitno FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno INNER JOIN branchmapping ON despatch_entry.branchid = branchmapping.subbranch INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (despatch_entry.sno = @RefDcNo) AND (branchmapping.superbranch = @BranchID) AND (despatch_entry.trans_type=@Transtype)");
                cmd.Parameters.Add("@RefDcNo", txt_refdcno.Text);
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@Transtype", "In");
            }
            else
            {
                //cmd = new SqlCommand("SELECT despatch_sub.cellname as CellName, despatch_sub.milktype as MilkType, despatch_sub.fat as FAT,despatch_sub.snf as SNF, despatch_sub.qty_ltr as QtyLtr,despatch_sub.qty_kgs as QtyKgs, despatch_sub.percentageon as PercentageOn,despatch_sub.clr as CLR, despatch_sub.cob1 as COB, despatch_sub.hs as HS, despatch_sub.phosps1 as Phosps,despatch_sub.mbrt as MBRT, despatch_sub.alcohol as Alcohol, despatch_sub.temp as TEMP, vendors.vendorname, despatch_entry.doe, despatch_entry.chemist, despatch_entry.vehciecleno, despatch_entry.sno, despatch_entry.dc_no,vendors.kms,vendors.expectedtime FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno WHERE (despatch_entry.sno = @RefDcNo) AND (despatch_entry.branchid = @BranchID)");
                cmd = new SqlCommand("SELECT despatch_sub.cellname AS CellName,despatch_sub.sealno as SealNo, despatch_sub.milktype AS MilkType, despatch_sub.fat AS FAT,despatch_sub.ot as OT, despatch_sub.snf AS SNF, despatch_sub.qty_ltr AS QtyLtr, despatch_sub.qty_kgs AS QtyKgs, despatch_sub.percentageon AS PercentageOn, despatch_sub.clr AS CLR, despatch_sub.cob1 AS COB, despatch_sub.hs AS HS, despatch_sub.phosps1 AS Phosps, despatch_sub.mbrt AS MBRT, despatch_sub.alcohol AS Alcohol, despatch_sub.temp AS TEMP, vendors.vendorname, despatch_entry.doe, despatch_entry.chemist, despatch_entry.vehciecleno, despatch_entry.sno, despatch_entry.dc_no, despatch_entry.salestype, vendors.kms, vendors.expectedtime, branch_info.tinno, branch_info.cstno, branch_info.mitno, branch_info.branchcode FROM despatch_entry INNER JOIN vendors ON despatch_entry.cc_id = vendors.sno INNER JOIN despatch_sub ON despatch_entry.sno = despatch_sub.desp_refno INNER JOIN branch_info ON vendors.sno = branch_info.venorid WHERE  (despatch_entry.sno = @RefDcNo) AND (despatch_entry.branchid = @BranchID) AND (despatch_entry.trans_type=@Transtype)");
                cmd.Parameters.Add("@RefDcNo", txt_refdcno.Text);
                cmd.Parameters.Add("@BranchID", BranchID);
                cmd.Parameters.Add("@Transtype", "In");
            }
            DataTable dtDispatch = vdm.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                DataView view1 = new DataView(dtDispatch);
                DataTable dtlblValues = view1.ToTable(true, "sno", "dc_no", "vehciecleno", "doe", "chemist", "vendorname", "kms", "expectedtime", "tinno", "branchcode", "cstno", "mitno", "salestype");
                DataTable dtgrdValues = view1.ToTable(true, "CellName", "MilkType", "QtyKgs", "QtyLtr", "FAT", "SNF", "CLR", "COB","OT", "HS", "Phosps", "Alcohol", "TEMP", "MBRT","SealNo");
                Report.Columns.Add("Sno");
                Report.Columns.Add("Type");
                Report.Columns.Add("F Cell");
                Report.Columns.Add("M Cell");
                Report.Columns.Add("B Cell");
                if (dtlblValues.Rows.Count > 0)
                {
                    lblRefdcno.Text = txt_refdcno.Text;
                    string branchcode = dtlblValues.Rows[0]["branchcode"].ToString();
                    lbldcno.Text = branchcode+"/"+dtlblValues.Rows[0]["dc_no"].ToString();
                    lblvehicleno.Text = dtlblValues.Rows[0]["vehciecleno"].ToString();
                    lblpartyname.Text = dtlblValues.Rows[0]["vendorname"].ToString();
                    lblkms.Text = dtlblValues.Rows[0]["kms"].ToString();
                    lblTinNo.Text = dtlblValues.Rows[0]["tinno"].ToString();
                    lblCSTNo.Text = dtlblValues.Rows[0]["cstno"].ToString();
                    lblMitno.Text = dtlblValues.Rows[0]["mitno"].ToString();
                    lbldto.Text = dtlblValues.Rows[0]["salestype"].ToString();
                    string PlanTime = dtlblValues.Rows[0]["doe"].ToString();
                    DateTime dtPlantime = Convert.ToDateTime(PlanTime);
                    string time = dtPlantime.ToString("dd/MMM/yyyy");
                    string strPlantime = dtPlantime.ToString("dd/MMM/yyyy HH:mm");
                    string[] DateTime = strPlantime.Split(' ');
                    string[] PlanDateTime = strPlantime.Split(' ');
                    lblassigndate.Text = PlanDateTime[0];
                    lbldisptime.Text = PlanDateTime[1];
                    int Dcmin = 0;
                    int.TryParse(dtlblValues.Rows[0]["expectedtime"].ToString(), out Dcmin);
                    DateTime dtecpected = dtPlantime.AddMinutes(Dcmin);
                    lblexpected.Text = dtecpected.ToString("dd/MMM/yyyy HH:mm");
                }
                if (dtgrdValues.Rows.Count == 1)
                {
                    int i = 1;
                    for (i = 1; i <2; i++)
                    {
                        DataRow newr = dtgrdValues.NewRow();
                        newr["CellName"] = "M Cell";
                        dtgrdValues.Rows.Add(newr);
                    }
                    grdReports.DataSource = dtgrdValues;
                    grdReports.DataBind();
                }
                if (dtgrdValues.Rows.Count == 2)
                {
                    int i = 1;
                    for (i = 1; i < 2; i++)
                    {
                        DataRow newr = dtgrdValues.NewRow();
                        newr["CellName"] = "B Cell";
                        dtgrdValues.Rows.Add(newr);
                    }
                    grdReports.DataSource = dtgrdValues;
                    grdReports.DataBind();
                }
                if (dtgrdValues.Rows.Count == 3)
                {
                    grdReports.DataSource = dtgrdValues;
                    grdReports.DataBind();
                }
                string barname = dtlblValues.Rows[0]["branchcode"].ToString();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "generateBarcode('" + barname + "');", true);
                pnlHide.Visible = true;
                BindEmpty();
                BindSample();
                Bindweight();
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


    void Bindweight()
    {
        DataTable EmptyReport = new DataTable();
        EmptyReport.Columns.Add("CellName");
        EmptyReport.Columns.Add("MilkType");
        EmptyReport.Columns.Add("Qty(Kgs)");
        EmptyReport.Columns.Add("Time");
        int i = 0;
        for (i = 1; i < 4; i++)
        {
            if (i == 1)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["CellName"] = "F Cell";
                EmptyReport.Rows.Add(newr);
            }
            if (i == 2)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["CellName"] = "M Cell";
                EmptyReport.Rows.Add(newr);
            }
            if (i == 3)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["CellName"] = "B Cell";
                EmptyReport.Rows.Add(newr);
            }
        }
        grdweight.DataSource = EmptyReport;
        grdweight.DataBind();
    }
    void BindSample()
    {
        DataTable EmptyReport = new DataTable();
        EmptyReport.Columns.Add("CellName");
        EmptyReport.Columns.Add("MilkType");
        EmptyReport.Columns.Add("TEMP");
         int i = 0;
        for (i = 1; i < 4; i++)
        {
            if (i == 1)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["CellName"] = "F Cell";
                EmptyReport.Rows.Add(newr);
            }
            if (i == 2)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["CellName"] = "M Cell";
                EmptyReport.Rows.Add(newr);
            }
            if (i == 3)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["CellName"] = "B Cell";
                EmptyReport.Rows.Add(newr);
            }
        }
        grdsample.DataSource = EmptyReport;
        grdsample.DataBind();
    }
    void BindEmpty()
    {
        DataTable EmptyReport = new DataTable();
        EmptyReport.Columns.Add("CellName");
        EmptyReport.Columns.Add("MilkType");
        EmptyReport.Columns.Add("FAT");
        EmptyReport.Columns.Add("SNF");
        EmptyReport.Columns.Add("CLR");
        EmptyReport.Columns.Add("TEMP");
        EmptyReport.Columns.Add("Acidity");
        EmptyReport.Columns.Add("COB");
        EmptyReport.Columns.Add("OT");
        EmptyReport.Columns.Add("HS");
        EmptyReport.Columns.Add("Phosps");
        EmptyReport.Columns.Add("Alcohol");
        EmptyReport.Columns.Add("MBRT");
        EmptyReport.Columns.Add("Neutralizers");
        EmptyReport.Columns.Add("Remarks");
        int i = 0;
        for (i = 1; i < 4; i++)
        {
            if (i == 1)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["CellName"] = "F Cell";
                EmptyReport.Rows.Add(newr);
            }
            if (i == 2)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["CellName"] = "M Cell";
                EmptyReport.Rows.Add(newr);
            }
            if (i == 3)
            {
                DataRow newr = EmptyReport.NewRow();
                newr["CellName"] = "B Cell";
                EmptyReport.Rows.Add(newr);
            }
        }
        grdempty.DataSource = EmptyReport;
        grdempty.DataBind();
    }
}