using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class directsalevocher : System.Web.UI.Page
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
                    // lblAddress.Text = Session["Address"].ToString();
                    // lblTitle.Text = Session["TitleName"].ToString();
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
            if (ddlbranchType.SelectedValue == "All")
            {
                cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE (branchmapping.superbranch = @branchid)");
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranches.DataSource = dttrips;
                ddlbranches.DataTextField = "vendorname";
                ddlbranches.DataValueField = "sno";
                ddlbranches.DataBind();
            }
            if (ddlbranchType.SelectedValue == "Inter Branches")
            {
                cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE    (branchmapping.superbranch = @branchid) and vendors.branchtype='Inter Branch'");
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranches.DataSource = dttrips;
                ddlbranches.DataTextField = "vendorname";
                ddlbranches.DataValueField = "sno";
                ddlbranches.DataBind();
            }
            if (ddlbranchType.SelectedValue == "Other Branches")
            {
                cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE    (branchmapping.superbranch = @branchid) and vendors.branchtype='Other Branch'");
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
                ddlbranches.DataSource = dttrips;
                ddlbranches.DataTextField = "vendorname";
                ddlbranches.DataValueField = "sno";
                ddlbranches.DataBind();
            }
        }
        if (ddlBranch.SelectedValue == "Vehicle Wise")
        {
            hideVehicles.Visible = true;
            cmd = new SqlCommand("SELECT sno,vehicleno,capacity,noofqty FROM vehicle_master where branchid=@vebranchid");
            cmd.Parameters.Add("@vebranchid", BranchID);
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
            directsale();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }

    DataTable dtdirectReport = new DataTable();
    DataTable dtdirectcow = new DataTable();
    private void directsale()
    {
        try
        {
            pnlcow.Visible = false;
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

            if (ddlReportType.SelectedValue == "Buffalo")
            {
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("INVOICE NO");
                Report.Columns.Add("INVOICE DATE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("LEDGER TYPE1");
                Report.Columns.Add("LEDGER AMOUNT1");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("Narration");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT vendors.state AS fromstate, vendors.companycode AS fromcompcode, vendors_1.state AS tostate, vendors_1.companycode AS tocmpcode,vendor_subtable.overheadon,despatch_entry.invoiceno, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype,  vendors.shortname, directsale.cell, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.veh_sno = @vehicleno) AND (directsale.branchid=@vbranchid) AND directsale.milktype='Buffalo' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@vbranchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("SELECT vendors.state AS fromstate, vendors.companycode AS fromcompcode, vendors_1.state AS tostate, vendors_1.companycode AS tocmpcode,vendor_subtable.overheadon, despatch_entry.invoiceno,vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.cell, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @sectionid) AND (directsale.branchid=@bbranchid) AND directsale.milktype='Buffalo' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@bbranchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("SELECT vendors.state AS fromstate, vendors.companycode AS fromcompcode, vendors_1.state AS tostate, vendors_1.companycode AS tocmpcode,vendor_subtable.overheadon,despatch_entry.invoiceno, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.cell, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @sectionid) AND (directsale.branchid=@mbranchid) AND directsale.milktype='Buffalo' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@mbranchid", BranchID);
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
                    string olddcno = "";
                    string oldnarration = "";
                    int i = 1;
                    string fckg = "0";
                    string fcltrs = "0";
                    string bckg = "0";
                    string bcltrs = "0";
                    string mckg = "0";
                    string mcltrs = "0";
                    string fcfat = "0";
                    string bcfat = "0";
                    string mcfat = "0";
                    string fcsnf = "0";
                    string bcsnf = "0";
                    string mcsnf = "0";
                    string dcnaration = "";
                    foreach (DataRow dr in dtDispatch.Rows)
                    {
                        string newdcno1 = dr["dc_no"].ToString();
                        DataTable dtin = new DataTable();
                        DataRow[] drr = dtDispatch.Select("invoiceno='" + newdcno1 + "'");
                        if (drr.Length > 0)
                        {
                            dtin = drr.CopyToDataTable();
                        }
                        if (dtin.Rows.Count > 0)
                        {
                            int d = 0;
                            int count = dtin.Rows.Count;
                            for (d = 0; d <= count; d++)
                            {
                                if (d == 0)
                                {
                                    fckg = dtin.Rows[0]["qty_kgs"].ToString();
                                    fcfat = dtin.Rows[0]["fat"].ToString();
                                    fcsnf = dtin.Rows[0]["snf"].ToString();
                                    fcltrs = dtin.Rows[0]["qty_ltr"].ToString();
                                }
                                if (d == 2)
                                {
                                    mckg = dtin.Rows[1]["qty_kgs"].ToString();
                                    mcfat = dtin.Rows[1]["fat"].ToString();
                                    mcsnf = dtin.Rows[1]["snf"].ToString();
                                    mcltrs = dtin.Rows[1]["qty_ltr"].ToString();
                                }
                                if (d == 3)
                                {
                                    bckg = dtin.Rows[2]["qty_kgs"].ToString();
                                    bcfat = dtin.Rows[2]["fat"].ToString();
                                    bcsnf = dtin.Rows[2]["snf"].ToString();
                                    bcltrs = dtin.Rows[2]["qty_ltr"].ToString();
                                }
                            }
                            dcnaration = "Being Sale Of Milk from " + ddlbranches.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd-MMM-yyyy");
                        newrow["INVOICE DATE"] = date;
                        double qty_kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                        double qty_ltr = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                        newrow["QTY KG'S/LTRS"] = dr["qty_kgs"].ToString();
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        // newrow["CLR"] = dr["clr"].ToString();
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
                            diffSNF = SNF - PSnf;
                            if (SNFOn == "Ltrs")
                            {
                                DiffSNFCost = diffSNF * ltrs * p_snfpluscost * 10;
                            }
                            else
                            {
                                DiffSNFCost = diffSNF * Kgs * p_snfpluscost * 10;
                            }
                        }

                        double OHcost = 0;
                        double overheadcost = 0;
                        double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                        string OverheadOn = dr["overheadon"].ToString();
                        if (OverheadOn == "Ltrs")
                        {
                            OHcost = overheadcost * ltrs;
                        }
                        else
                        {
                            OHcost = overheadcost * Kgs;
                        }
                        //newrow["TS RATE"] = cost;
                        KGFAT = Math.Round(KGFAT, 2);
                        // newrow["KG FAT"] = KGFAT;
                        kgfattotal += KGFAT;
                        double MValue = 0;
                        MValue = KGFAT * cost;
                        //MValue = MValue / 100;
                        MValue = Math.Round(MValue, 2);
                        //newrow["M VALUE"] = MValue;
                        mvaluetotal += MValue;
                        double OHandMvalue = 0;
                        OHandMvalue = MValue;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        double roundvalue = Math.Round(OHandMvalue, 0);
                        milkvaluetotal += OHandMvalue;
                        double ledgeramount = 0;
                        ledgeramount = OHandMvalue - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        OHcost = Math.Round(OHcost, 0);
                        newrow["LEDGER AMOUNT1"] = OHcost;
                        double diffvalue = roundvalue - OHcost;
                        ohtotal += OHcost;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        double netvalue = OHandMvalue + OHcost + DiffSNFCost;
                        newrow["Net VALUE"] = Math.Round(netvalue, 0);
                        newrow["LEDGER AMOUNT"] = Math.Round((netvalue - OHcost), 0);
                        string cn = "SRI VYSHNAVI DAIRY PVT LTD";
                        string ledgerccname = "Sri Vyshnavi Dairy Specialities Pvt Ltd.,";
                        string vendername = dr["fromcc"].ToString();
                        string ledgertype = dr["ledgertype"].ToString();
                        string tallyoh = dr["tallyoh"].ToString();
                        milkvaluetotal += OHandMvalue;
                        if (ddlbranchType.SelectedValue == "Other Branches")
                        {
                            newrow["ITEM NAME"] = "WHOLE MILK Others";
                            ledgerccname = vendername;
                        }
                        else
                        {
                            newrow["ITEM NAME"] = "WHOLE MILK";
                            
                            if (vendername == "SRI VYSHNAVI DAIRY PVT LTD, GUDLURU")
                            {
                                tallyoh = "Over Head Gudluru";
                                ledgertype = "Sale of Milk-GUDLURU";
                                ledgerccname = "SRI VYSHNAVI FOODS PVT LTD.";
                                //ledgerccname = cn + "," + ccname;
                            }

                            if (vendername == "Sri Vyshnavi Dairy Pvt Ltd, C.S.Puram")
                            {
                                tallyoh = "Over Head C.S.Puram";
                                ledgertype = "Sale of Milk-C.S.Puram";
                                // ledgerccname = cn + "," + ccname;

                            }
                            if (vendername == "Sri Vyshnavi Dairy Pvt Ltd, Kondepi")
                            {
                                tallyoh = "Over Head Kondepi";
                                ledgertype = "Sale of Milk-Kdp";
                                //ledgerccname = cn + "," + ccname;
                            }
                            if (vendername == "Sri Vyshnavi Dairy Pvt Ltd., Kavali")
                            {
                                tallyoh = "Over Head Kavali";
                                ledgertype = "Sale of Milk-Kavali";

                            }
                            if (vendername == "SVDS.P.LTD KALIGIRI")
                            {
                                ledgerccname = "SVDS.P.LTD PUNABAKA PLANT";
                                tallyoh = "Over Head Kaligiri";
                                ledgertype = "Sale of buff Milk Inter Branches-Kaligir";
                            }
                            if (vendername == "SRI VYSHNAVI DAIRY PVT LTD-G.D.PADU")
                            {
                                tallyoh = "Over Head G.D.Padu";
                                ledgertype = "Sale of Milk-Gudipallipadu";
                            }
                        }
                        newrow["LEDGER TYPE"] = ledgertype;
                        string dcno = dr["dc_no"].ToString();
                        string celltype = dr["cell"].ToString();
                        string shortname = dr["shortname"].ToString();
                        //newrow["INVOICE NO"] = dcno + '-' + celltype + '-' + shortname;
                        string branchtype = ddlbranchType.SelectedValue;
                        //if (branchtype == "Inter Branches")
                        //{
                        //    newrow["INVOICE NO"] = "1819/" + dcno + "-" + celltype + "-" + shortname;
                        //}
                        //else
                        //{
                        //    newrow["INVOICE NO"] = "1819/" + dcno + "-" + celltype + "-" + shortname;
                        //}
                        DateTime dtapril = new DateTime();
                        DateTime dtmarch = new DateTime();
                        DateTime dt_st = Convert.ToDateTime(dr["doe"].ToString());
                        int currentyear = dt_st.Year;
                        int nextyear = dt_st.Year + 1;
                        int currntyearnum = 0;
                        int nextyearnum = 0;
                        if (dt_st.Month > 3)
                        {
                            string apr = "4/1/" + currentyear;
                            dtapril = DateTime.Parse(apr);
                            string march = "3/31/" + nextyear;
                            dtmarch = DateTime.Parse(march);
                            currntyearnum = currentyear;
                            nextyearnum = nextyear;
                        }
                        if (dt_st.Month <= 3)
                        {
                            string apr = "4/1/" + (currentyear - 1);
                            dtapril = DateTime.Parse(apr);
                            string march = "3/31/" + (nextyear - 1);
                            dtmarch = DateTime.Parse(march);
                            currntyearnum = currentyear - 1;
                            nextyearnum = nextyear - 1;
                        }
                        string newreceipt = "0";
                        long countdc = 0;
                        long.TryParse(dcno, out countdc);
                        if (countdc < 10)
                        {
                            newreceipt = "0000" + countdc;
                        }
                        if (countdc >= 10 && countdc <= 99)
                        {
                            newreceipt = "000" + countdc;
                        }
                        if (countdc >= 99 && countdc <= 999)
                        {
                            newreceipt = "00" + countdc;
                        }
                        if (countdc >= 999 && countdc <= 9999)
                        {
                            newreceipt = "0" + countdc;
                        }
                        if (countdc >= 9999)
                        {
                            newreceipt = "" + countdc;
                        }
                        //string companycode = dr["companycode"].ToString();
                        //newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "E/" + newreceipt + "/" + celltype;
                        string fromstate = dr["fromstate"].ToString();
                        string fromcompcode = dr["fromcompcode"].ToString();
                        string tostate = dr["tostate"].ToString();
                        string tocmpcode = dr["tocmpcode"].ToString();
                        if (fromcompcode == tocmpcode)
                        {
                            if (fromstate == tostate)
                            {
                                newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "ST/" + newreceipt + "/" + celltype;
                            }
                            else
                            {
                                DateTime cdate = Convert.ToDateTime(dr["doe"].ToString());
                                DateTime dadate = Convert.ToDateTime("07/01/2018");
                                if (cdate > dadate)
                                {
                                    newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + newreceipt + "/" + celltype;
                                }
                                else
                                {
                                    newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "E/" + newreceipt + "/" + celltype;
                                }
                            }
                        }
                        else
                        {
                            DateTime cdate = Convert.ToDateTime(dr["doe"].ToString());
                            DateTime dadate = Convert.ToDateTime("07/01/2018");
                            if (cdate > dadate)
                            {
                                newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + newreceipt + "/" + celltype;
                            }
                            else
                            {
                                newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "E/" + newreceipt + "/" + celltype;
                            }
                        }
                        newrow["CUSTOMER NAME"] = ledgerccname;
                        newrow["LEDGER TYPE1"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 5);
                        newrow["RATE PER LTR"] = ltrcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["invoiceno"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " , " + " KG fat Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        Report.Rows.Add(newrow);
                        fckg = "0";
                        fcltrs = "0";
                        bckg = "0";
                        bcltrs = "0";
                        mckg = "0";
                        mcltrs = "0";
                        fcfat = "0";
                        bcfat = "0";
                        mcfat = "0";
                        fcsnf = "0";
                        bcsnf = "0";
                        mcsnf = "0";
                        dcnaration = "";
                    }
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
            else if (ddlReportType.SelectedValue == "Cow" || ddlReportType.SelectedValue == "Skim" || ddlReportType.SelectedValue == "Condensed")
            {
                Report.Columns.Add("CUSTOMER NAME");
                Report.Columns.Add("INVOICE NO");
                Report.Columns.Add("INVOICE DATE");
                Report.Columns.Add("LEDGER TYPE");
                Report.Columns.Add("ITEM NAME");
                Report.Columns.Add("QTY KG'S/LTRS");
                Report.Columns.Add("RATE PER LTR");
                Report.Columns.Add("LEDGER AMOUNT");
                Report.Columns.Add("LEDGER TYPE1");
                Report.Columns.Add("LEDGER AMOUNT1");
                Report.Columns.Add("NET VALUE");
                Report.Columns.Add("Narration");
                if (ddlBranch.SelectedValue == "Vehicle Wise")
                {
                    cmd = new SqlCommand("SELECT vendors.state AS fromstate, vendors.companycode AS fromcompcode, vendors_1.state AS tostate, vendors_1.companycode AS tocmpcode,vendor_subtable.overheadon,despatch_entry.invoiceno, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc, directsale.cell, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.veh_sno = @vehicleno) AND (directsale.branchid=@dbranchid) AND directsale.milktype='Cow' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@transtype", "in");
                    cmd.Parameters.Add("@vehicleno", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@dbranchid", BranchID);
                }
                else if (ddlBranch.SelectedValue == "Branch Wise")
                {
                    cmd = new SqlCommand("SELECT vendors.state AS fromstate, vendors.companycode AS fromcompcode, vendors_1.state AS tostate, vendors_1.companycode AS tocmpcode,vendor_subtable.overheadon,despatch_entry.invoiceno, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc,  directsale.cell, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @sectionid) AND (directsale.branchid=@bbranchid) AND directsale.milktype='Cow' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@bbranchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("SELECT vendors.state AS fromstate, vendors.companycode AS fromcompcode, vendors_1.state AS tostate, vendors_1.companycode AS tocmpcode,vendor_subtable.overheadon,despatch_entry.invoiceno, vendor_subtable.overheadcost, despatch_entry.dc_no, vendor_subtable.m_std_snf, vendor_subtable.m_snfpluscost, vendor_subtable.snfplus_on, vendor_subtable.p_snfpluscost, vendor_subtable.p_std_snf, directsale.doe, directsale.dcno, vendors.vendorname AS fromcc, vendors.tallyoh, vendors.ledgertype, vendors.shortname, directsale.qty_kgs, directsale.qty_ltr, directsale.fat, directsale.snf, directsale.clr, directsale.transport_on, directsale.transport_cost, directsale.rate, directsale.rate_on, directsale.calc_on, vendors_1.vendorname AS tocc,  directsale.cell, directsale.veh_sno as vehicleno FROM directsale INNER JOIN vendors ON directsale.fromccid = vendors.sno INNER JOIN vendors AS vendors_1 ON directsale.toccid = vendors_1.sno INNER JOIN vendor_subtable ON vendor_subtable.vendor_refno=directsale.fromccid INNER JOIN despatch_entry ON despatch_entry.sno = directsale.dcno WHERE (directsale.doe BETWEEN @d1 AND @d2) AND (directsale.fromccid = @sectionid) AND (directsale.branchid=@ebranchid) AND directsale.milktype='Cow' ORDER BY directsale.doe ASC");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@sectionid", ddlbranches.SelectedValue);
                    cmd.Parameters.Add("@ebranchid", BranchID);
                }
                //cmd = new SqlCommand("SELECT milktransactions.dcno, milktransactions.inwardno AS InwardNo, milktransactions.vehicleno,milktransactions.doe, milktransactions.transtype, milktransactions.qty_ltr, milktransactions.qty_kgs, milktransactions.percentageon, milktransactions.snf, milktransactions.fat, milktransactions.clr, milktransaction_logs.milktype, milktransaction_logs.rate_on, milktransaction_logs.cost, milktransaction_logs.calc_on, milktransaction_logs.overheadon, milktransaction_logs.overheadcost, milktransaction_logs.m_std_snf, milktransaction_logs.p_std_snf, milktransaction_logs.snfplus_on, milktransaction_logs.m_snfpluscost,milktransaction_logs.p_snfpluscost, milktransaction_logs.transport_on, milktransaction_logs.transportcost, milktransaction_logs.transport FROM milktransactions INNER JOIN milktransaction_logs ON milktransactions.sno = milktransaction_logs.milktransaction_sno WHERE (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype)");

                DataTable dtDispatch = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtDispatch.Rows.Count > 0)
                {
                    int i = 1;
                    double kgfattotal = 0;
                    double kgsnftotal = 0;
                    double kgstotal = 0;
                    double Ltrstotal = 0;
                    double TStotal = 0;
                    double mvaluetotal = 0;
                    double ohtotal = 0;
                    double snf9total = 0;
                    double milkvaluetotal = 0;
                    string olddcno = "";
                    string oldnarration = "";
                    string fckg = "0";
                    string fcltrs = "0";
                    string bckg = "0";
                    string bcltrs = "0";
                    string mckg = "0";
                    string mcltrs = "0";
                    string fcfat = "0";
                    string bcfat = "0";
                    string mcfat = "0";
                    string fcsnf = "0";
                    string bcsnf = "0";
                    string mcsnf = "0";
                    string dcnaration = "";
                    foreach (DataRow dr in dtDispatch.Rows)
                    {
                        string newdcno1 = dr["dc_no"].ToString();
                        DataTable dtin = new DataTable();
                        DataRow[] drr = dtDispatch.Select("invoiceno='" + newdcno1 + "'");
                        if (drr.Length > 0)
                        {
                            dtin = drr.CopyToDataTable();
                        }
                        if (dtin.Rows.Count > 0)
                        {
                            int d = 0;
                            int count = dtin.Rows.Count;
                            for (d = 0; d <= count; d++)
                            {
                                if (d == 0)
                                {
                                    fckg = dtin.Rows[0]["qty_kgs"].ToString();
                                    fcfat = dtin.Rows[0]["fat"].ToString();
                                    fcsnf = dtin.Rows[0]["snf"].ToString();
                                    fcltrs = dtin.Rows[0]["qty_ltr"].ToString();
                                }
                                if (d == 2)
                                {
                                    mckg = dtin.Rows[1]["qty_kgs"].ToString();
                                    mcfat = dtin.Rows[1]["fat"].ToString();
                                    mcsnf = dtin.Rows[1]["snf"].ToString();
                                    mcltrs = dtin.Rows[1]["qty_ltr"].ToString();
                                }
                                if (d == 3)
                                {
                                    bckg = dtin.Rows[2]["qty_kgs"].ToString();
                                    bcfat = dtin.Rows[2]["fat"].ToString();
                                    bcsnf = dtin.Rows[2]["snf"].ToString();
                                    bcltrs = dtin.Rows[2]["qty_ltr"].ToString();
                                }
                            }
                            dcnaration = "Being Sale Of Milk from " + ddlbranches.SelectedItem.Text + "," + "Dcno" + newdcno1 + "," + " Total Quantity IN kg's " + fckg + "/" + fcltrs + "," + mckg + "/" + mcltrs + "," + bckg + "/" + bcltrs;
                        }
                        DataRow newrow = Report.NewRow();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd-MMM-yyyy");
                        newrow["INVOICE DATE"] = date;
                        double qty_kgs = 0;
                        double.TryParse(dr["qty_kgs"].ToString(), out qty_kgs);
                        // newrow["KGS"] = dr["qty_kgs"].ToString();

                        double qty_ltr = 0;
                        double.TryParse(dr["qty_ltr"].ToString(), out qty_ltr);
                        newrow["QTY KG'S/LTRS"] = dr["qty_ltr"].ToString();
                        double FAT = 0;
                        double.TryParse(dr["fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        // newrow["FAT"] = FAT;
                        double SNF = 0;
                        double.TryParse(dr["snf"].ToString(), out SNF);
                        // newrow["SNF"] = SNF;
                        // newrow["CLR"] = dr["clr"].ToString();
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


                        double OHcost = 0;
                        double overheadcost = 0;
                        double.TryParse(dr["overheadcost"].ToString(), out overheadcost);
                        string OverheadOn = dr["overheadon"].ToString();
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
                        KGFAT = Math.Round(KGFAT, 2);
                        kgfattotal += KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
                        kgsnftotal += KGSNF;
                        double MValue = 0;
                        MValue = tstotal * qty_ltr * cost;
                        MValue = MValue / 100;
                        MValue = Math.Round(MValue, 2);
                        mvaluetotal += MValue;
                        double OHandMvalue = 0;
                        OHandMvalue = MValue;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        milkvaluetotal += OHandMvalue;
                        //newrow["DC NO"] = dr["dcno"].ToString();
                        double OMILKVALUE = 0;
                        OMILKVALUE = MValue + OHcost + DiffSNFCost;
                        //newrow["TS RATE"] = cost;
                        newrow["NET VALUE"] = OMILKVALUE;
                        double diffvalue = OMILKVALUE - OHandMvalue;
                        // newrow["ROUNDING OFF"] = Math.Round(diffvalue, 2);
                        milkvaluetotal += OHandMvalue;
                        string ledgertype = dr["ledgertype"].ToString();
                        string customername = "SVDS.P.LTD PUNABAKA PLANT";
                        string vendername = dr["fromcc"].ToString();
                        string tallyoh = dr["tallyoh"].ToString();
                        if (ddlbranchType.SelectedValue == "Other Branches")
                        {
                            newrow["ITEM NAME"] = "COW MILK Others";
                            customername = vendername;
                        }
                        else
                        {
                            newrow["ITEM NAME"] = "COW MILK";
                            if (vendername == "SVDS.P.LTD WALAJA")
                            {
                                ledgertype = "Sale of Milk Inter Branches-WALAJA";
                                tallyoh = "Over Head-Walajah";
                            }
                            if (vendername == "SVDS.P.LTD BOMMA")
                            {
                                ledgertype = "Sale of Milk Inter Branches-BOMMA";
                                tallyoh = "Over Head Bomma";
                            }
                            if (vendername == "SVDS.P.LTD R.C.PURAM")
                            {
                                ledgertype = "Sale of Milk Inter Branches-R.C.PURAM";
                                tallyoh = "Over Head R.C.Puram";
                            }
                            if (vendername == "SVDS.P.LTD V.KOTA")
                            {
                                ledgertype = "Sale of Milk Inter Branches-V.KOTA";
                                tallyoh = "Over Head-V.Kota";
                            }

                            if (vendername == "SVDS.P.LTD KAVERIPATTINAM")
                            {
                                ledgertype = "Sale of Milk Inter Branches-KAVERIPATTINAM";
                                tallyoh = "Over Head-KVP";
                            }
                            if (vendername == "SVDS.P.LTD ARANI")
                            {
                                ledgertype = "Sale of Milk Inter Branches-ARANI";
                                tallyoh = "Over Head Arani";
                            }

                            if (vendername == "SVDS.P.LTD KALA.C.C")
                            {
                                ledgertype = "Sale of Milk Inter Branches-KALA.C.C";
                                tallyoh = "Over Head Kala C.C";
                            }
                            if (vendername == "SVDS.P.LTD TARIGONDA")
                            {
                                ledgertype = "Sale of Milk Inter Branches-TARIGONDA";
                                tallyoh = "Over Head-Tarigonda";
                            }
                        }
                        string dcno = dr["dc_no"].ToString();
                        string celltype  = dr["cell"].ToString();
                        string shortname = dr["shortname"].ToString();
                        //newrow["INVOICE NO"] = dcno + '-' + celltype + '-' + shortname;
                        //string branchtype = ddlbranchType.SelectedValue;
                        //if (branchtype == "Inter Branches")
                        //{
                        //    newrow["INVOICE NO"] = "1819/" + dcno + "-" + celltype + "-" + shortname;
                        //}
                        //else
                        //{
                        //    newrow["INVOICE NO"] = "1819/" + dcno + "-" + celltype + "-" + shortname;
                        //}
                        DateTime dtapril = new DateTime();
                        DateTime dtmarch = new DateTime();
                        DateTime dt_st = Convert.ToDateTime(dr["doe"].ToString());
                        int currentyear = dt_st.Year;
                        int nextyear = dt_st.Year + 1;
                        int currntyearnum = 0;
                        int nextyearnum = 0;
                        if (dt_st.Month > 3)
                        {
                            string apr = "4/1/" + currentyear;
                            dtapril = DateTime.Parse(apr);
                            string march = "3/31/" + nextyear;
                            dtmarch = DateTime.Parse(march);
                            currntyearnum = currentyear;
                            nextyearnum = nextyear;
                        }
                        if (dt_st.Month <= 3)
                        {
                            string apr = "4/1/" + (currentyear - 1);
                            dtapril = DateTime.Parse(apr);
                            string march = "3/31/" + (nextyear - 1);
                            dtmarch = DateTime.Parse(march);
                            currntyearnum = currentyear - 1;
                            nextyearnum = nextyear - 1;
                        }
                        string newreceipt = "0";
                        long countdc = 0;
                        long.TryParse(dcno, out countdc);
                        if (countdc < 10)
                        {
                            newreceipt = "0000" + countdc;
                        }
                        if (countdc >= 10 && countdc <= 99)
                        {
                            newreceipt = "000" + countdc;
                        }
                        if (countdc >= 99 && countdc <= 999)
                        {
                            newreceipt = "00" + countdc;
                        }
                        if (countdc >= 999 && countdc <= 9999)
                        {
                            newreceipt = "0" + countdc;
                        }
                        if (countdc >= 9999)
                        {
                            newreceipt = "" + countdc;
                        }
                        //string companycode = dr["companycode"].ToString();
                        //newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "E/" + newreceipt + "/" + celltype;
                        string fromstate = dr["fromstate"].ToString();
                        string fromcompcode = dr["fromcompcode"].ToString();
                        string tostate = dr["tostate"].ToString();
                        string tocmpcode = dr["tocmpcode"].ToString();
                        if (fromcompcode == tocmpcode)
                        {
                            if (fromstate == tostate)
                            {
                                newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "ST/" + newreceipt + "/" + celltype;
                            }
                            else
                            {
                                DateTime cdate = Convert.ToDateTime(dr["doe"].ToString());
                                DateTime dadate = Convert.ToDateTime("07/01/2018");
                                if (cdate > dadate)
                                {
                                    newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + newreceipt + "/" + celltype;
                                }
                                else
                                {
                                    newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "E/" + newreceipt + "/" + celltype;
                                }
                            }
                        }
                        else
                        {
                            DateTime cdate = Convert.ToDateTime(dr["doe"].ToString());
                            DateTime dadate = Convert.ToDateTime("07/01/2018");
                            if (cdate > dadate)
                            {
                                newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + newreceipt + "/" + celltype;
                            }
                            else
                            {
                                newrow["INVOICE NO"] = shortname + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "E/" + newreceipt + "/" + celltype;
                            }
                        }
                        string cn = "SVDS.P.LTD";
                        string plant = "Pbk";
                      //  string vendername = dr["fromcc"].ToString();
                        newrow["LEDGER TYPE"] = ledgertype;
                        newrow["CUSTOMER NAME"] = customername;
                        newrow["LEDGER TYPE1"] = tallyoh;
                        double ltrcost = OHandMvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 5);
                        newrow["RATE PER LTR"] = ltrcost;
                        double ledgeramount = 0;
                        ledgeramount = OHandMvalue - OHcost;
                        ledgeramount = Math.Round(ledgeramount, 0);
                        OHcost = Math.Round(OHcost, 0);
                        newrow["LEDGER AMOUNT1"] = OHcost;
                        string vehicleno = dr["vehicleno"].ToString();
                        string newdcno = dr["invoiceno"].ToString();
                        newrow["Narration"] = dcnaration + "," + " FAT " + FAT + "," + " SNF " + SNF + " ," + " TS Rate " + cost + ", OH " + OHcost + " Through tanker no " + vehicleno;
                        newrow["LEDGER AMOUNT"] = OMILKVALUE - OHcost;
                        Report.Rows.Add(newrow);
                        fckg = "0";
                        fcltrs = "0";
                        bckg = "0";
                        bcltrs = "0";
                        mckg = "0";
                        mcltrs = "0";
                        fcfat = "0";
                        bcfat = "0";
                        mcfat = "0";
                        fcsnf = "0";
                        bcsnf = "0";
                        mcsnf = "0";
                        dcnaration = "";
                    }
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
        }
        catch
        {
        }
    }
}