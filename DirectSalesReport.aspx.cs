using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class DirectSalesReport : System.Web.UI.Page
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
    
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesDBManager SalesDB = new SalesDBManager();
        if (ddlBranch.SelectedValue == "All")
        {
            hideVehicles.Visible = false;
        }
        if (ddlBranch.SelectedValue == "Branch Wise")
        {
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE    (branchmapping.superbranch = @branchid)");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "vendorname";
            ddlbranches.DataValueField = "sno";
            ddlbranches.DataBind();
        }
        if (ddlBranch.SelectedValue == "Vehicle Wise")
        {
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT sno,vehicleno,capacity,noofqty FROM vehicle_master WHERE branchid=@branchid");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "vehicleno";
            ddlbranches.DataValueField = "vehicleno";
            ddlbranches.DataBind();
        }
    }
    DataTable Report = new DataTable();
    DataTable dtcow = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            grdcow.DataSource = null;
            grdcow.DataBind();
            grdReports.DataSource = null;
            grdReports.DataBind();
            Session["filename"] = "Tanker Direct Sales";
            Session["title"] = "Tanker Direct Sales Details";
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
            Report.Columns.Add("DATE");
            Report.Columns.Add("KGS");
            Report.Columns.Add("LTRS");
            Report.Columns.Add("FAT");
            Report.Columns.Add("SNF");
            Report.Columns.Add("CLR");
            Report.Columns.Add("KG FAT RATE");
            Report.Columns.Add("KG FAT");
            Report.Columns.Add("KG SNF");
            Report.Columns.Add("MILK VALUE");
            Report.Columns.Add("Dc No");
            Report.Columns.Add("FROM CC Name");
            Report.Columns.Add("TO CC Name");
            Report.Columns.Add("TANKER NO");
            if (ddlBranch.SelectedValue == "Branch Wise")
            {
                cmd = new SqlCommand("SELECT directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @ccid) AND directsale.milktype='Buffalo' ORDER BY directsale.doe ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
            }
            else
            {
                cmd = new SqlCommand("SELECT directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND directsale.milktype='Buffalo' ORDER BY directsale.doe ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
            }
            DataTable dtDispatch = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double Ltrstotal = 0;
                double TStotal = 0;
                double mvaluetotal = 0;
                double ohtotal = 0;
                double snf9total = 0;
                double milkvaluetotal = 0;
                int i = 1;
                foreach (DataRow dr in dtDispatch.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    newrow["FROM CC Name"] = dr["fromcc"].ToString();
                    newrow["TO CC Name"] = dr["tocc"].ToString();
                    newrow["TANKER NO"] = dr["vehicleno"].ToString();
                    newrow["KGS"] = dr["qty_kgs"].ToString();
                    newrow["Dc No"] = dr["dcno"].ToString();
                    double qty_ltr = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                    newrow["LTRS"] = dr["qty_ltr"].ToString();
                    double FAT = 0;
                    double.TryParse(dr["fat"].ToString(), out FAT);
                    FAT = Math.Round(FAT, 2);
                    newrow["FAT"] = FAT;
                    double SNF = 0;
                    double.TryParse(dr["snf"].ToString(), out SNF);
                    newrow["SNF"] = SNF;
                    newrow["CLR"] = dr["clr"].ToString();
                    string Rateon = dr["rate_on"].ToString();
                    double weight = 0;
                    double KGFAT = 0;
                    double KGSNF = 0;
                    double ltrs = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                    Ltrstotal += ltrs;
                    double Kgs = 0;
                    double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                    kgstotal += Kgs;
                    double tstotal = 0;
                    tstotal = FAT + SNF;
                    if (Rateon == "TS")
                    {

                        double TS = 0;
                        TS = FAT + SNF;
                        weight = (TS * Kgs) / 100;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                    else if (Rateon == "KGFAT")
                    {
                        weight = (FAT * Kgs) / 100;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                    else if (Rateon == "PerLtr" || Rateon == "PerKg")
                    {
                        string CalOn = dr["calc_on"].ToString();
                        if (CalOn == "Ltrs")
                        {
                            weight = ltrs;
                            KGFAT = (FAT * ltrs) / 100;
                            KGSNF = (SNF * ltrs) / 100;
                        }
                        else
                        {
                            weight = Kgs;
                            KGFAT = (FAT * Kgs) / 100;
                            KGSNF = (SNF * Kgs) / 100;
                        }
                    }
                    double cost = 0;
                    double.TryParse(dr["rate"].ToString(), out cost);
                    newrow["KG FAT RATE"] = cost;
                    KGFAT = Math.Round(KGFAT, 2);
                    newrow["KG FAT"] = KGFAT;
                    kgfattotal += KGFAT;
                    KGSNF = Math.Round(KGSNF, 2);
                    newrow["KG SNF"] = KGSNF;
                    kgsnftotal += KGSNF;
                    double MValue = 0;
                    MValue = KGFAT * cost;
                    //MValue = MValue / 100;
                    MValue = Math.Round(MValue, 2);
                    //newrow["M VALUE"] = MValue;
                    mvaluetotal += MValue;
                    double OHandMvalue = 0;
                    OHandMvalue = MValue;// +OHcost + DiffSNFCost;
                    //newrow["OH"] = OHcost;
                    //ohtotal += OHcost;
                    //DiffSNFCost = Math.Round(DiffSNFCost, 2);
                    //newrow["SNF9"] = DiffSNFCost;
                    //snf9total += DiffSNFCost;
                    OHandMvalue = Math.Round(OHandMvalue, 2);
                    newrow["MILK VALUE"] = OHandMvalue;
                    milkvaluetotal += OHandMvalue;
                    //newrow["DC NO"] = dr["dcno"].ToString();

                    Report.Rows.Add(newrow);
                }
                DataRow newvartical2 = Report.NewRow();
                newvartical2["DATE"] = "Total";
                newvartical2["KGS"] = kgstotal;
                newvartical2["LTRS"] = Ltrstotal;
                double fattotal = 0;
                fattotal = (kgfattotal / kgstotal) * 100;
                fattotal = Math.Round(fattotal, 2);
                newvartical2["FAT"] = fattotal;
                newvartical2["KG FAT"] = kgfattotal;
                double snftotal = 0;
                snftotal = (kgsnftotal / kgstotal) * 100;
                snftotal = Math.Round(snftotal, 2);
                newvartical2["SNF"] = snftotal;
                newvartical2["KG SNF"] = kgsnftotal;
                //newvartical2["M VALUE"] = mvaluetotal;
                //newvartical2["OH"] = ohtotal;
                //newvartical2["SNF9"] = snf9total;
                newvartical2["MILK VALUE"] = milkvaluetotal;
                double kgfatratetotal = 0;
                kgfatratetotal = mvaluetotal / kgfattotal;
                kgfatratetotal = Math.Round(kgfatratetotal, 2);
                newvartical2["KG FAT RATE"] = kgfatratetotal;
                Report.Rows.Add(newvartical2);
                DataRow New1 = Report.NewRow();
                New1["DATE"] = "Cow";
                Report.Rows.Add(New1);
                grdReports.DataSource = Report;
                grdReports.DataBind();
                
            }
           
            dtcow.Columns.Add("Sno");
            dtcow.Columns.Add("DATE");
            dtcow.Columns.Add("KGS");
            dtcow.Columns.Add("LTRS");
            dtcow.Columns.Add("FAT");
            dtcow.Columns.Add("SNF");
            dtcow.Columns.Add("CLR");
            dtcow.Columns.Add("KG FAT RATE");
            dtcow.Columns.Add("KG FAT");
            dtcow.Columns.Add("KG SNF");
            dtcow.Columns.Add("MILK VALUE");
            dtcow.Columns.Add("Dc No");
            dtcow.Columns.Add("FROM CC Name");
            dtcow.Columns.Add("TO CC Name");
            dtcow.Columns.Add("TANKER NO");
            if (ddlBranch.SelectedValue == "Branch Wise")
            {
                cmd = new SqlCommand("SELECT directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @ccid) AND directsale.milktype='Cow' ORDER BY directsale.doe ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@ccid", ddlbranches.SelectedValue);
            }
            else
            {
                cmd = new SqlCommand("SELECT directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND directsale.milktype='Cow' ORDER BY directsale.doe ASC");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
            }
            DataTable dtcowDispatch = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtcowDispatch.Rows.Count > 0)
            {
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double Ltrstotal = 0;
                double TStotal = 0;
                double mvaluetotal = 0;
                double ohtotal = 0;
                double snf9total = 0;
                double milkvaluetotal = 0;
                int i = 1;
                foreach (DataRow dr in dtcowDispatch.Rows)
                {
                    DataRow newrow = dtcow.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    newrow["FROM CC Name"] = dr["fromcc"].ToString();
                    newrow["TO CC Name"] = dr["tocc"].ToString();
                    newrow["TANKER NO"] = dr["vehicleno"].ToString();
                    newrow["KGS"] = dr["qty_kgs"].ToString();
                    newrow["Dc No"] = dr["dcno"].ToString();

                    double qty_ltr = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                    newrow["LTRS"] = dr["qty_ltr"].ToString();
                    double FAT = 0;
                    double.TryParse(dr["fat"].ToString(), out FAT);
                    FAT = Math.Round(FAT, 2);
                    newrow["FAT"] = FAT;
                    double SNF = 0;
                    double.TryParse(dr["snf"].ToString(), out SNF);
                    newrow["SNF"] = SNF;
                    newrow["CLR"] = dr["clr"].ToString();
                    string Rateon = dr["rate_on"].ToString();
                    double weight = 0;
                    double KGFAT = 0;
                    double KGSNF = 0;
                    double ltrs = 0;
                    double.TryParse(dr["qty_ltr"].ToString(), out ltrs);
                    Ltrstotal += ltrs;
                    double Kgs = 0;
                    double.TryParse(dr["qty_kgs"].ToString(), out Kgs);
                    kgstotal += Kgs;
                    double tstotal = 0;
                    tstotal = FAT + SNF;
                    if (Rateon == "TS")
                    {
                        double TS = 0;
                        TS = FAT + SNF;
                        weight = (TS * Kgs) / 100;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                    else if (Rateon == "KGFAT")
                    {
                        weight = (FAT * Kgs) / 100;
                        KGFAT = (FAT * Kgs) / 100;
                        KGSNF = (SNF * Kgs) / 100;
                    }
                    else if (Rateon == "PerLtr" || Rateon == "PerKg")
                    {
                        string CalOn = dr["calc_on"].ToString();
                        if (CalOn == "Ltrs")
                        {
                            weight = ltrs;
                            KGFAT = (FAT * ltrs) / 100;
                            KGSNF = (SNF * ltrs) / 100;
                        }
                        else
                        {
                            weight = Kgs;
                            KGFAT = (FAT * Kgs) / 100;
                            KGSNF = (SNF * Kgs) / 100;
                        }
                    }
                    double cost = 0;
                    double.TryParse(dr["rate"].ToString(), out cost);
                    newrow["KG FAT RATE"] = cost;
                    KGFAT = Math.Round(KGFAT, 2);
                    newrow["KG FAT"] = KGFAT;
                    kgfattotal += KGFAT;
                    KGSNF = Math.Round(KGSNF, 2);
                    newrow["KG SNF"] = KGSNF;
                    kgsnftotal += KGSNF;
                    double MValue = 0;
                    MValue = KGFAT * cost;
                    //MValue = MValue / 100;
                    MValue = Math.Round(MValue, 2);
                    //newrow["M VALUE"] = MValue;
                    mvaluetotal += MValue;
                  
                    double OHandMvalue = 0;
                    OHandMvalue = MValue;// +OHcost + DiffSNFCost;
                    //newrow["OH"] = OHcost;
                    //ohtotal += OHcost;
                    //DiffSNFCost = Math.Round(DiffSNFCost, 2);
                    //newrow["SNF9"] = DiffSNFCost;
                    //snf9total += DiffSNFCost;
                    OHandMvalue = Math.Round(OHandMvalue, 2);
                    newrow["MILK VALUE"] = OHandMvalue;
                    milkvaluetotal += OHandMvalue;
                    //newrow["DC NO"] = dr["dcno"].ToString();
                    dtcow.Rows.Add(newrow);
                }
                DataRow newvartical2 = dtcow.NewRow();
                newvartical2["DATE"] = "Total";
                newvartical2["KGS"] = kgstotal;
                newvartical2["LTRS"] = Ltrstotal;
                double fattotal = 0;
                fattotal = (kgfattotal / kgstotal) * 100;
                fattotal = Math.Round(fattotal, 2);
                newvartical2["FAT"] = fattotal;
                newvartical2["KG FAT"] = kgfattotal;
                double snftotal = 0;
                snftotal = (kgsnftotal / kgstotal) * 100;
                snftotal = Math.Round(snftotal, 2);
                newvartical2["SNF"] = snftotal;
                newvartical2["KG SNF"] = kgsnftotal;
                newvartical2["MILK VALUE"] = milkvaluetotal;
                double kgfatratetotal = 0;
                kgfatratetotal = mvaluetotal / kgfattotal;
                kgfatratetotal = Math.Round(kgfatratetotal, 2);
                newvartical2["KG FAT RATE"] = kgfatratetotal;
                dtcow.Rows.Add(newvartical2);
                grdcow.DataSource = dtcow;
                grdcow.DataBind();
                Session["xportdata"] = dtcow;
                hidepanel.Visible = true;
            }
            else
            {
                pnlcow.Visible = true;
            }
            Session["xportdata"] = Report;
            hidepanel.Visible = true;
        }
        catch
        {
        }
    }
}