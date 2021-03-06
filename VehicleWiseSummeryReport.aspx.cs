﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class VehicleWiseSummeryReport : System.Web.UI.Page
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
            cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM   vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE (branchmapping.superbranch = @branchid) ");
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

    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            Session["filename"] = "Tanker Inward";
            Session["title"] = "Tanker Inward Details";
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
            Report.Columns.Add("M VALUE");
            Report.Columns.Add("OH");
            Report.Columns.Add("SNF9");
            Report.Columns.Add("MILK VALUE");
            Report.Columns.Add("DC NO");
            if (ddlBranch.SelectedValue == "Branch Wise")
            {
            }
            else
            {
                Report.Columns.Add("CC Name");
            }
            if (ddlBranch.SelectedValue == "Vehicle Wise")
            {
            }
            else
            {
                Report.Columns.Add("TANKER NO");
            }
            if (ddlBranch.SelectedValue == "Vehicle Wise")
            {
                cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND (milktransactions.vehicleno=@vehicleno)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "in");
                cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
            }
            else if (ddlBranch.SelectedValue == "Branch Wise")
            {

                cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)  AND (milktransactions.sectionid=@sectionid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "in");
                cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
            }
            else
            {
                cmd = new SqlCommand("  SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno, milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost, milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport, vendors.vendorname FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno INNER JOIN vendors ON milktransactions.sectionid = vendors.sno WHERE  (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) AND (milktransaction_logs.milktype='Buffalo') AND (milktransactions.branchid=@branchid)");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@transtype", "in");
                cmd.Parameters.Add("@branchid", BranchID); 
            }
            DataTable dtBufello = SalesDB.SelectQuery(cmd).Tables[0];
            int i = 1;
            double kgfattotal = 0;
            double kgsnftotal = 0;
            double kgstotal = 0;
            double Ltrstotal = 0;
            double mvaluetotal = 0;
            double ohtotal = 0;
            double snf9total = 0;
            double milkvaluetotal = 0;
            foreach (DataRow dr in dtBufello.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = i++.ToString();
                DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                string date = dtdoe.ToString("dd/MM/yyyy");
                newrow["DATE"] = date;
                newrow["KGS"] = dr["qty_kgs"].ToString();

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
                double.TryParse(dr["cost"].ToString(), out cost);
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
                newrow["M VALUE"] = MValue;
                mvaluetotal += MValue;
                string OverheadOn = dr["overheadon"].ToString();
                double OHcost = 0;
                double overheadcost = 0;
                double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                if (OverheadOn == "Ltrs")
                {
                    OHcost = overheadcost * ltrs;
                }
                else
                {
                    OHcost = overheadcost * Kgs;
                }
                double MSnf = 0;
                double.TryParse(dr["m_std_snf"].ToString(), out MSnf);
                double m_snfpluscost = 0;
                double.TryParse(dr["m_snfpluscost"].ToString(), out m_snfpluscost);
                double DiffSNFCost = 0;
                if (SNF < MSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    diffSNF = Math.Round(diffSNF, 2);
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * m_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * m_snfpluscost * 10;
                    }
                }
                double p_snfpluscost = 0;
                double.TryParse(dr["p_snfpluscost"].ToString(), out p_snfpluscost);
                double PSnf = 0;
                double.TryParse(dr["p_std_snf"].ToString(), out PSnf);
                if (SNF > PSnf)
                {
                    string SNFOn = dr["snfplus_on"].ToString();
                    double diffSNF = 0;
                    diffSNF = SNF - MSnf;
                    if (SNFOn == "Ltrs")
                    {
                        DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                    }
                    else
                    {
                        DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                    }
                }

                double OHandMvalue = 0;
                OHandMvalue = MValue + OHcost + DiffSNFCost;
                newrow["OH"] = OHcost;
                ohtotal += OHcost;
                DiffSNFCost = Math.Round(DiffSNFCost, 2);
                newrow["SNF9"] = DiffSNFCost;
                snf9total += DiffSNFCost;
                OHandMvalue = Math.Round(OHandMvalue, 2);
                newrow["MILK VALUE"] = OHandMvalue;
                milkvaluetotal += OHandMvalue;
                newrow["DC NO"] = dr["dcno"].ToString();
                if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    lblName.Text = dr["vendorname"].ToString();
                }
                else
                {
                    newrow["CC Name"] = dr["vendorname"].ToString();
                }
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    lblName.Text = dr["vehicleno"].ToString();
                }
                else
                {
                    newrow["TANKER NO"] = dr["vehicleno"].ToString();
                }
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
            newvartical2["M VALUE"] = mvaluetotal;
            newvartical2["OH"] = ohtotal;
            newvartical2["SNF9"] = snf9total;
            newvartical2["MILK VALUE"] = milkvaluetotal;
            Report.Rows.Add(newvartical2);
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
}