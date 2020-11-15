using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Tanker_forecastinfreport : System.Web.UI.Page
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
    DataTable dtReport = new DataTable();
    DataTable dtdiffreport = new DataTable();
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        SalesDBManager SalesDB = new SalesDBManager();
        if (ddlbranchType.SelectedValue == "All")
        {
            cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.vendorname,vendors.shortname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE (branchmapping.superbranch = @branchid)");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "shortname";
            ddlbranches.DataValueField = "sno";
            ddlbranches.DataBind();
        }
        if (ddlbranchType.SelectedValue == "Inter Branches")
        {
            cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode,vendors.shortname, vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE (branchmapping.superbranch = @branchid) and vendors.branchtype='Inter Branch'");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "shortname";
            ddlbranches.DataValueField = "sno";
            ddlbranches.DataBind();
        }
        if (ddlbranchType.SelectedValue == "Other Branches")
        {
            cmd = new SqlCommand("SELECT   vendors.sno, vendors.vendorcode, vendors.shortname,vendors.vendorname, vendors.email, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address FROM    vendors INNER JOIN branch_info ON vendors.sno = branch_info.venorid INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE  (branchmapping.superbranch = @branchid) and vendors.branchtype='Other Branch'");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlbranches.DataSource = dttrips;
            ddlbranches.DataTextField = "shortname";
            ddlbranches.DataValueField = "sno";
            ddlbranches.DataBind();
        }
    }
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        try
        {
            grdcow.DataSource = null;
            grdcow.DataBind();
            Session["filename"] = "Tanker Inward";
            Session["title"] = "Tanker Inward Details";
            lblmsg.Text = "";
            SalesDBManager SalesDB = new SalesDBManager();
            if (ddlReportType.SelectedValue == "Buffalo")
            {
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

                Report.Columns.Add("Total KMs");
                Report.Columns.Add("Rate per KM");
                Report.Columns.Add("Transport");
                Report.Columns.Add("Per Ltr");
                Report.Columns.Add("Per Ts");
                Report.Columns.Add("Total Value");

                Report.Columns.Add("Per ltr Cost");
                Report.Columns.Add("Destintion");
                Report.Columns.Add("TANKER NO");
            }
            else
            {
                Report.Columns.Add("Sno");
                Report.Columns.Add("DATE");
                Report.Columns.Add("KGS");
                Report.Columns.Add("LTRS");
                Report.Columns.Add("FAT");
                Report.Columns.Add("CLR");
                Report.Columns.Add("SNF");
                Report.Columns.Add("TS TOTAL");
                Report.Columns.Add("TS RATE");
                Report.Columns.Add("OH");
                Report.Columns.Add("M VALUE");
                Report.Columns.Add("SNF9");
                Report.Columns.Add("MILK VALUE");

                Report.Columns.Add("Total KMs");
                Report.Columns.Add("Rate per KM");
                Report.Columns.Add("Transport");
                Report.Columns.Add("Per Ltr");
                Report.Columns.Add("Per Ts");
                Report.Columns.Add("Total Value");

                Report.Columns.Add("KG FAT");
                Report.Columns.Add("KG SNF");
                Report.Columns.Add("Per ltr Cost");
                Report.Columns.Add("Total TS RATE");
                Report.Columns.Add("Per Solid RATE");

                Report.Columns.Add("Destintion");
                Report.Columns.Add("TANKER NO");
                dtReport.Columns.Add("Sno");
                dtReport.Columns.Add("DATE");
                dtReport.Columns.Add("KGS");
                dtReport.Columns.Add("LTRS");
                dtReport.Columns.Add("FAT");
                dtReport.Columns.Add("CLR");
                dtReport.Columns.Add("SNF");
                dtReport.Columns.Add("TS TOTAL");
                dtReport.Columns.Add("TS RATE");
                dtReport.Columns.Add("OH");
                dtReport.Columns.Add("M VALUE");
                dtReport.Columns.Add("SNF9");
                dtReport.Columns.Add("MILK VALUE");

                dtReport.Columns.Add("Total KMs");
                dtReport.Columns.Add("Rate per KM");
                dtReport.Columns.Add("Transport");
                dtReport.Columns.Add("Per Ltr");
                dtReport.Columns.Add("Per Ts");
                dtReport.Columns.Add("Total Value");

                dtReport.Columns.Add("KG FAT");
                dtReport.Columns.Add("KG SNF");
                dtReport.Columns.Add("Per ltr Cost");
                dtReport.Columns.Add("Total TS RATE");
                dtReport.Columns.Add("Per Solid RATE");

                dtReport.Columns.Add("Destintion");
                dtReport.Columns.Add("TANKER NO");
            }
            lblSource.Text = ddlbranches.SelectedItem.Text;
            cmd = new SqlCommand("SELECT vendors.sno,vendors.branchtype,vendors.vendortype, vendors.vendorcode, vendors.vendorname, vendors.email,vendors.kms,vendors.expectedtime, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address, vendor_subtable.sno AS vendor_sub_sno, vendor_subtable.milktype, vendor_subtable.rate_on, vendor_subtable.calc_on, vendor_subtable.cost, vendor_subtable.overheadon, vendor_subtable.overheadcost, vendor_subtable.m_std_snf, vendor_subtable.p_std_snf, vendor_subtable.snfplus_on, vendor_subtable.m_snfpluscost, vendor_subtable.p_snfpluscost, vendor_subtable.transport_on,vendor_subtable.transportcost,vendor_subtable.transport,vendor_subtable.fatplus_on,vendor_subtable.m_fatpluscost,vendor_subtable.p_fatpluscost,vendor_subtable.m_std_fat,vendor_subtable.p_std_fat FROM vendors LEFT OUTER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno where vendors.sno=@VendorID ");
            cmd.Parameters.Add("@VendorID", ddlbranches.SelectedValue);
            DataTable dtVendor = vdm.SelectQuery(cmd).Tables[0];
            if (dtVendor.Rows.Count > 0)
            {
                int i = 1;
                cmd = new SqlCommand("SELECT vendors.sno,vendors.branchtype,vendors.vendortype, vendors.vendorcode, vendors.vendorname, vendors.email,vendors.kms,vendors.expectedtime, vendors.mobno, vendors.panno, vendors.doe, vendors.branchid, vendors.address, vendor_subtable.sno AS vendor_sub_sno, vendor_subtable.milktype, vendor_subtable.rate_on, vendor_subtable.calc_on, vendor_subtable.cost, vendor_subtable.overheadon, vendor_subtable.overheadcost, vendor_subtable.m_std_snf, vendor_subtable.p_std_snf, vendor_subtable.snfplus_on, vendor_subtable.m_snfpluscost, vendor_subtable.p_snfpluscost, vendor_subtable.transport_on,vendor_subtable.transportcost,vendor_subtable.transport,vendor_subtable.fatplus_on,vendor_subtable.m_fatpluscost,vendor_subtable.p_fatpluscost,vendor_subtable.m_std_fat,vendor_subtable.p_std_fat FROM vendors LEFT OUTER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno where (vendors.salestype = 'plant')");
                DataTable dtOtherBranch = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow drBranch in dtOtherBranch.Rows)
                {
                    foreach (DataRow dr in dtVendor.Rows)
                    {
                        if (ddlReportType.SelectedValue == "Buffalo")
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                            string date = dtdoe.ToString("dd/MM/yyyy");
                            newrow["DATE"] = date;
                            newrow["KGS"] = txtqty.Text; //dr["qty_kgs"].ToString();

                            double qty_ltr = 0;
                            double.TryParse(txtqty.Text, out qty_ltr);
                            newrow["LTRS"] = qty_ltr.ToString();
                            double FAT = 0;
                            double.TryParse(dr["m_std_fat"].ToString(), out FAT);
                            FAT = Math.Round(FAT, 2);
                            newrow["FAT"] = FAT;
                            double SNF = 0;
                            double.TryParse(dr["m_std_snf"].ToString(), out SNF);
                            newrow["SNF"] = SNF;
                            newrow["CLR"] = 21;
                            string Rateon = dr["rate_on"].ToString();

                            double weight = 0;
                            double KGFAT = 0;
                            double KGSNF = 0;
                            double ltrs = 0;
                            double Kgs = 0;
                            double.TryParse(txtqty.Text, out Kgs);
                            double.TryParse(txtqty.Text, out ltrs);
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
                            KGSNF = Math.Round(KGSNF, 2);
                            newrow["KG SNF"] = KGSNF;
                            double MValue = 0;
                            MValue = KGFAT * cost;
                            //MValue = MValue / 100;
                            MValue = Math.Round(MValue, 2);
                            newrow["M VALUE"] = MValue;
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
                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                            newrow["SNF9"] = DiffSNFCost;
                            OHandMvalue = Math.Round(OHandMvalue, 2);
                            newrow["MILK VALUE"] = OHandMvalue;

                            ///////.................Transport.........//////////////////////////////
                            cmd = new SqlCommand("SELECT  sno, fromvendorid, tovendorid, distance, createdby, doe, branchid FROM vendordistencedetails WHERE (fromvendorid = @fromvendorid) AND (tovendorid = @tovendorid)");
                            cmd.Parameters.Add("@fromvendorid", 22);
                            cmd.Parameters.Add("@tovendorid", ddlbranches.SelectedValue);
                            DataTable dtdistancefirst = vdm.SelectQuery(cmd).Tables[0];
                            cmd = new SqlCommand("SELECT  sno, fromvendorid, tovendorid, distance, createdby, doe, branchid FROM vendordistencedetails WHERE (fromvendorid = @fromvendorid) AND (tovendorid = @tovendorid)");
                            cmd.Parameters.Add("@fromvendorid", ddlbranches.SelectedValue);
                            cmd.Parameters.Add("@tovendorid", drBranch["sno"].ToString());
                            DataTable dtdistance = vdm.SelectQuery(cmd).Tables[0];
                            cmd = new SqlCommand("SELECT  sno, fromvendorid, tovendorid, distance, createdby, doe, branchid FROM vendordistencedetails WHERE (fromvendorid = @fromvendorid) AND (tovendorid = @tovendorid)");
                            cmd.Parameters.Add("@fromvendorid", 22);
                            cmd.Parameters.Add("@tovendorid", drBranch["sno"].ToString());
                            DataTable dtdistancelast = vdm.SelectQuery(cmd).Tables[0];
                            double totalkms = 0;
                            double kms = 0;
                            double firstkms = 0;
                            double lastkms = 0;
                            if (dtdistancefirst.Rows.Count > 0)
                            {
                                double.TryParse(dtdistancefirst.Rows[0]["distance"].ToString(), out firstkms);
                            }
                            if (dtdistance.Rows.Count > 0)
                            {
                                double.TryParse(dtdistance.Rows[0]["distance"].ToString(), out kms);
                            }
                            if (dtdistance.Rows.Count > 0)
                            {
                                double.TryParse(dtdistancelast.Rows[0]["distance"].ToString(), out lastkms);
                            }
                            totalkms = firstkms + kms + lastkms;
                            newrow["Total KMs"] = totalkms;
                            double perkm = 0;
                            perkm = 27;
                            newrow["Rate per KM"] = perkm;
                            double Transport = 0;
                            Transport = totalkms * perkm;
                            Transport = Math.Round(Transport, 2);
                            newrow["Transport"] = Transport;
                            double perltrtransportcost = 0;
                            perltrtransportcost = Transport / qty_ltr;
                            perltrtransportcost = Math.Round(perltrtransportcost, 2);
                            newrow["Per Ltr"] = perltrtransportcost;
                            double PerTs = 0;
                            PerTs = perltrtransportcost / tstotal;
                            PerTs = Math.Round(PerTs, 2);
                            newrow["Per Ts"] = PerTs;
                            double totalvalue = 0;
                            totalvalue = OHandMvalue + Transport;
                            totalvalue = Math.Ceiling(totalvalue);
                            newrow["Total Value"] = totalvalue;

                            newrow["Destintion"] = drBranch["vendorname"].ToString();
                            //newrow["TANKER NO"] = dr["vehicleno"].ToString();
                            double ltrcost = totalvalue / ltrs;
                            ltrcost = Math.Round(ltrcost, 2);
                            newrow["Per ltr Cost"] = ltrcost;
                            Report.Rows.Add(newrow);
                        }
                        else
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                            string date = dtdoe.ToString("dd/MM/yyyy");
                            newrow["DATE"] = date;
                            newrow["Destintion"] = drBranch["vendorname"].ToString();
                            double qty_kgs = 0;
                            double.TryParse(txtqty.Text, out qty_kgs);
                            newrow["KGS"] = qty_kgs.ToString();

                            double qty_ltr = 0;
                            double.TryParse(txtqty.Text, out qty_ltr);
                            newrow["LTRS"] = qty_ltr.ToString();
                            double FAT = 0;
                            double.TryParse(dr["m_std_fat"].ToString(), out FAT);
                            FAT = Math.Round(FAT, 2);
                            newrow["FAT"] = FAT;
                            double SNF = 0;
                            double.TryParse(dr["m_std_snf"].ToString(), out SNF);
                            string temp = "21";
                            double snfvalue = SNF;
                            double clr =29;
                            //if (temp == "21")
                            //{
                            //    snfvalue = (FAT * 0.21) + (clr / 4 + 0.36);
                            //}
                            //if (temp == "27")
                            //{
                            //    snfvalue = (FAT * 0.27) + (clr / 4 + 0.36);
                            //}
                            //if (temp == "29")
                            //{
                            //    snfvalue = (FAT * 0.29) + (clr / 4 + 0.36);
                            //}
                            //snfvalue = Math.Round(snfvalue, 2);

                            //double.TryParse(dr["snf"].ToString(), out SNF);
                            newrow["SNF"] = snfvalue;
                            newrow["CLR"] = 29;
                            string Rateon = dr["rate_on"].ToString();


                            double weight = 0;
                            double KGFAT = 0;
                            double KGSNF = 0;
                            double ltrs = 0;
                            double.TryParse(txtqty.Text, out ltrs);
                            double Kgs = 0;
                            double.TryParse(txtqty.Text, out Kgs);
                            double tstotal = 0;
                            tstotal = FAT + snfvalue;
                            newrow["TS TOTAL"] = tstotal;
                            if (Rateon == "TS")
                            {

                                double TS = 0;
                                TS = FAT + snfvalue;
                                weight = (TS * Kgs) / 100;
                                KGFAT = (FAT * Kgs) / 100;
                                KGSNF = (snfvalue * Kgs) / 100;
                            }
                            else if (Rateon == "KGFAT")
                            {
                                weight = (FAT * Kgs) / 100;
                                KGFAT = (FAT * Kgs) / 100;
                                KGSNF = (snfvalue * Kgs) / 100;
                            }
                            else if (Rateon == "PerLtr" || Rateon == "PerKg")
                            {
                                string CalOn = dr["calc_on"].ToString();
                                if (CalOn == "Ltrs")
                                {
                                    weight = ltrs;
                                    KGFAT = (FAT * ltrs) / 100;
                                    KGSNF = (snfvalue * ltrs) / 100;
                                }
                                else
                                {
                                    weight = Kgs;
                                    KGFAT = (FAT * Kgs) / 100;
                                    KGSNF = (snfvalue * Kgs) / 100;
                                }
                            }
                            double cost = 0;
                            double.TryParse(dr["cost"].ToString(), out cost);
                            newrow["TS RATE"] = cost;

                            KGFAT = Math.Round(KGFAT, 2);
                            newrow["KG FAT"] = KGFAT;
                            KGSNF = Math.Round(KGSNF, 2);
                            newrow["KG SNF"] = KGSNF;
                            double MValue = 0;
                            if (Rateon == "PerLtr" || Rateon == "PerKg")
                            {
                                MValue = cost * qty_kgs;
                            }
                            else
                            {
                                MValue = tstotal * cost * qty_ltr;
                                MValue = MValue / 100;

                            }
                            MValue = Math.Round(MValue, 2);
                            newrow["M VALUE"] = MValue;
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
                            if (snfvalue < MSnf)
                            {
                                string SNFOn = dr["snfplus_on"].ToString();
                                double diffSNF = 0;
                                diffSNF = snfvalue - MSnf;
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
                            if (snfvalue > PSnf)
                            {
                                string SNFOn = dr["snfplus_on"].ToString();
                                double diffSNF = 0;
                                diffSNF = snfvalue - MSnf;
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
                            DiffSNFCost = Math.Round(DiffSNFCost, 2);
                            newrow["SNF9"] = DiffSNFCost;
                            OHandMvalue = Math.Round(OHandMvalue, 2);
                            newrow["MILK VALUE"] = OHandMvalue;


                            ///////.................Transport.........//////////////////////////////
                            cmd = new SqlCommand("SELECT  sno, fromvendorid, tovendorid, distance, createdby, doe, branchid FROM vendordistencedetails WHERE (fromvendorid = @fromvendorid) AND (tovendorid = @tovendorid)");
                            cmd.Parameters.Add("@fromvendorid", 22);
                            cmd.Parameters.Add("@tovendorid", ddlbranches.SelectedValue);
                            DataTable dtdistancefirst = vdm.SelectQuery(cmd).Tables[0];
                            cmd = new SqlCommand("SELECT  sno, fromvendorid, tovendorid, distance, createdby, doe, branchid FROM vendordistencedetails WHERE (fromvendorid = @fromvendorid) AND (tovendorid = @tovendorid)");
                            cmd.Parameters.Add("@fromvendorid", ddlbranches.SelectedValue);
                            cmd.Parameters.Add("@tovendorid", drBranch["sno"].ToString());
                            DataTable dtdistance = vdm.SelectQuery(cmd).Tables[0];
                            cmd = new SqlCommand("SELECT  sno, fromvendorid, tovendorid, distance, createdby, doe, branchid FROM vendordistencedetails WHERE (fromvendorid = @fromvendorid) AND (tovendorid = @tovendorid)");
                            cmd.Parameters.Add("@fromvendorid", 22);
                            cmd.Parameters.Add("@tovendorid", drBranch["sno"].ToString());
                            DataTable dtdistancelast = vdm.SelectQuery(cmd).Tables[0];
                            double totalkms = 0;
                            double kms = 0;
                            double firstkms = 0;
                            double lastkms = 0;
                            if (dtdistancefirst.Rows.Count > 0)
                            {
                                double.TryParse(dtdistancefirst.Rows[0]["distance"].ToString(), out firstkms);
                            }
                            if (dtdistance.Rows.Count > 0)
                            {
                                double.TryParse(dtdistance.Rows[0]["distance"].ToString(), out kms);
                            }
                            if (dtdistancelast.Rows.Count > 0)
                            {
                                double.TryParse(dtdistancelast.Rows[0]["distance"].ToString(), out lastkms);
                            }
                            totalkms = firstkms + kms + lastkms;
                            newrow["Total KMs"] = totalkms;
                            double perkm = 0;
                            perkm = 27;
                            newrow["Rate per KM"] = perkm;
                            double Transport = 0;
                            Transport = totalkms * perkm;
                            Transport = Math.Round(Transport, 2);
                            newrow["Transport"] = Transport;
                            double perltrtransportcost = 0;
                            perltrtransportcost = Transport / qty_ltr;
                            perltrtransportcost = Math.Round(perltrtransportcost, 2);
                            newrow["Per Ltr"] = perltrtransportcost;
                            double PerTs = 0;
                            PerTs = perltrtransportcost / tstotal;
                            PerTs = Math.Round(PerTs, 2);
                            newrow["Per Ts"] = PerTs;
                            double totalvalue = 0;
                            totalvalue = OHandMvalue + Transport;
                            totalvalue = Math.Ceiling(totalvalue);
                            newrow["Total Value"] = totalvalue;
                            double totaltsrate = 0;
                            totaltsrate = (totalvalue * 100) / (tstotal * qty_kgs);
                            totaltsrate = Math.Round(totaltsrate, 2);
                            newrow["Total TS RATE"] = totaltsrate;
                         
                            double ltrcost = totalvalue / ltrs;
                            ltrcost = Math.Round(ltrcost, 2);
                            newrow["Per ltr Cost"] = ltrcost;

                            double persolidrate = 0;
                            persolidrate = ltrcost / tstotal;
                            persolidrate = Math.Round(persolidrate, 2);
                            newrow["Per Solid RATE"] = persolidrate;
                            Report.Rows.Add(newrow);

                        }
                    }
                }
                //DataRow newrow1 = Report.NewRow();
                //newrow1["DATE"] = "";
                //Report.Rows.Add(newrow1);
                //DataRow newrow2 = Report.NewRow();
                //newrow2["DATE"] = "";
                //Report.Rows.Add(newrow2);
                //DataRow newrow3 = Report.NewRow();
                //newrow3["DATE"] = "";
                //Report.Rows.Add(newrow3);
                //DataRow newrow5 = Report.NewRow();
                //newrow5["DATE"] = "";
                //Report.Rows.Add(newrow5);
                //DataRow newrow4 = Report.NewRow();
                //newrow4["DATE"] = "Sales";
                //Report.Rows.Add(newrow4);
                grdcow.DataSource = Report;
                grdcow.DataBind();
                foreach (DataRow dr in dtOtherBranch.Rows)
                {
                    foreach (DataRow drBranch in dtVendor.Rows)
                    {
                        DataRow newrow = dtReport.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["DATE"] = date;
                        newrow["Destintion"] = dr["vendorname"].ToString();
                        double qty_kgs = 0;
                        double.TryParse(txtqty.Text, out qty_kgs);
                        newrow["KGS"] = qty_kgs.ToString();

                        double qty_ltr = 0;
                        double.TryParse(txtqty.Text, out qty_ltr);
                        newrow["LTRS"] = qty_ltr.ToString();
                        double FAT = 0;
                        double.TryParse(dr["m_std_fat"].ToString(), out FAT);
                        FAT = Math.Round(FAT, 2);
                        newrow["FAT"] = FAT;
                        double SNF = 0;
                        double.TryParse(dr["m_std_snf"].ToString(), out SNF);
                        string temp = "21";
                        double snfvalue = SNF;
                        double clr = 29;
                        //if (temp == "21")
                        //{
                        //    snfvalue = (FAT * 0.21) + (clr / 4 + 0.36);
                        //}
                        //if (temp == "27")
                        //{
                        //    snfvalue = (FAT * 0.27) + (clr / 4 + 0.36);
                        //}
                        //if (temp == "29")
                        //{
                        //    snfvalue = (FAT * 0.29) + (clr / 4 + 0.36);
                        //}
                        //snfvalue = Math.Round(snfvalue, 2);

                        //double.TryParse(dr["snf"].ToString(), out SNF);
                        newrow["SNF"] = snfvalue;
                        newrow["CLR"] = 29;
                        string Rateon = dr["rate_on"].ToString();


                        double weight = 0;
                        double KGFAT = 0;
                        double KGSNF = 0;
                        double ltrs = 0;
                        double.TryParse(txtqty.Text, out ltrs);
                        double Kgs = 0;
                        double.TryParse(txtqty.Text, out Kgs);
                        double tstotal = 0;
                        tstotal = FAT + snfvalue;
                        newrow["TS TOTAL"] = tstotal;
                        if (Rateon == "TS")
                        {

                            double TS = 0;
                            TS = FAT + snfvalue;
                            weight = (TS * Kgs) / 100;
                            KGFAT = (FAT * Kgs) / 100;
                            KGSNF = (snfvalue * Kgs) / 100;
                        }
                        else if (Rateon == "KGFAT")
                        {
                            weight = (FAT * Kgs) / 100;
                            KGFAT = (FAT * Kgs) / 100;
                            KGSNF = (snfvalue * Kgs) / 100;
                        }
                        else if (Rateon == "PerLtr" || Rateon == "PerKg")
                        {
                            string CalOn = dr["calc_on"].ToString();
                            if (CalOn == "Ltrs")
                            {
                                weight = ltrs;
                                KGFAT = (FAT * ltrs) / 100;
                                KGSNF = (snfvalue * ltrs) / 100;
                            }
                            else
                            {
                                weight = Kgs;
                                KGFAT = (FAT * Kgs) / 100;
                                KGSNF = (snfvalue * Kgs) / 100;
                            }
                        }
                        double cost = 0;
                        double.TryParse(dr["cost"].ToString(), out cost);
                        newrow["TS RATE"] = cost;

                        KGFAT = Math.Round(KGFAT, 2);
                        newrow["KG FAT"] = KGFAT;
                        KGSNF = Math.Round(KGSNF, 2);
                        newrow["KG SNF"] = KGSNF;
                        double MValue = 0;
                        if (Rateon == "PerLtr" || Rateon == "PerKg")
                        {
                            MValue = cost * qty_kgs;
                        }
                        else
                        {
                            MValue = tstotal * cost * qty_ltr;
                            MValue = MValue / 100;

                        }
                        MValue = Math.Round(MValue, 2);
                        newrow["M VALUE"] = MValue;
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
                        if (snfvalue < MSnf)
                        {
                            string SNFOn = dr["snfplus_on"].ToString();
                            double diffSNF = 0;
                            diffSNF = snfvalue - MSnf;
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
                        if (snfvalue > PSnf)
                        {
                            string SNFOn = dr["snfplus_on"].ToString();
                            double diffSNF = 0;
                            diffSNF = snfvalue - MSnf;
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
                        DiffSNFCost = Math.Round(DiffSNFCost, 2);
                        newrow["SNF9"] = DiffSNFCost;
                        OHandMvalue = Math.Round(OHandMvalue, 2);
                        newrow["MILK VALUE"] = OHandMvalue;


                        ///////.................Transport.........//////////////////////////////
                        cmd = new SqlCommand("SELECT  sno, fromvendorid, tovendorid, distance, createdby, doe, branchid FROM vendordistencedetails WHERE (fromvendorid = @fromvendorid) AND (tovendorid = @tovendorid)");
                        cmd.Parameters.Add("@fromvendorid", 22);
                        cmd.Parameters.Add("@tovendorid", ddlbranches.SelectedValue);
                        DataTable dtdistancefirst = vdm.SelectQuery(cmd).Tables[0];
                        cmd = new SqlCommand("SELECT  sno, fromvendorid, tovendorid, distance, createdby, doe, branchid FROM vendordistencedetails WHERE (fromvendorid = @fromvendorid) AND (tovendorid = @tovendorid)");
                        cmd.Parameters.Add("@fromvendorid", ddlbranches.SelectedValue);
                        cmd.Parameters.Add("@tovendorid", drBranch["sno"].ToString());
                        DataTable dtdistance = vdm.SelectQuery(cmd).Tables[0];
                        cmd = new SqlCommand("SELECT  sno, fromvendorid, tovendorid, distance, createdby, doe, branchid FROM vendordistencedetails WHERE (fromvendorid = @fromvendorid) AND (tovendorid = @tovendorid)");
                        cmd.Parameters.Add("@fromvendorid", 22);
                        cmd.Parameters.Add("@tovendorid", drBranch["sno"].ToString());
                        DataTable dtdistancelast = vdm.SelectQuery(cmd).Tables[0];
                        double totalkms = 0;
                        double kms = 0;
                        double firstkms = 0;
                        double lastkms = 0;
                        if (dtdistancefirst.Rows.Count > 0)
                        {
                            double.TryParse(dtdistancefirst.Rows[0]["distance"].ToString(), out firstkms);
                        }
                        if (dtdistance.Rows.Count > 0)
                        {
                            double.TryParse(dtdistance.Rows[0]["distance"].ToString(), out kms);
                        }
                        if (dtdistancelast.Rows.Count > 0)
                        {
                            double.TryParse(dtdistancelast.Rows[0]["distance"].ToString(), out lastkms);
                        }
                        totalkms = firstkms + kms + lastkms;
                        newrow["Total KMs"] = totalkms;
                        double perkm = 0;
                        perkm = 2;
                        newrow["Rate per KM"] = perkm;
                        double Transport = 0;
                        Transport = qty_kgs * perkm;
                        Transport = Math.Round(Transport, 2);
                        newrow["Transport"] = Transport;
                        double perltrtransportcost = 0;
                        perltrtransportcost = Transport / qty_ltr;
                        perltrtransportcost = Math.Round(perltrtransportcost, 2);
                        newrow["Per Ltr"] = perltrtransportcost;
                        double PerTs = 0;
                        PerTs = perltrtransportcost / tstotal;
                        PerTs = Math.Round(PerTs, 2);
                        newrow["Per Ts"] = PerTs;
                        double totalvalue = 0;
                        totalvalue = OHandMvalue ;
                        totalvalue = Math.Ceiling(totalvalue);
                        newrow["Total Value"] = totalvalue;
                        double totaltsrate = 0;
                        totaltsrate = (totalvalue * 100) / (tstotal * qty_kgs);
                        totaltsrate = Math.Round(totaltsrate, 2);
                        newrow["Total TS RATE"] = totaltsrate;

                        double ltrcost = totalvalue / ltrs;
                        ltrcost = Math.Round(ltrcost, 2);
                        newrow["Per ltr Cost"] = ltrcost;

                        double persolidrate = 0;
                        persolidrate = ltrcost / tstotal;
                        persolidrate = Math.Round(persolidrate, 2);
                        newrow["Per Solid RATE"] = persolidrate;
                        dtReport.Rows.Add(newrow);
                    }
                }
                grdReports.DataSource = dtReport;
                grdReports.DataBind();
                dtdiffreport.Columns.Add("Destination");
                dtdiffreport.Columns.Add("Purchase");
                dtdiffreport.Columns.Add("Sales");
                dtdiffreport.Columns.Add("Profit");
                dtdiffreport.Columns.Add("Transport");
                dtdiffreport.Columns.Add("Total Profit");
                foreach (DataRow drBranch in Report.Rows)
                {
                    foreach (DataRow dr in dtReport.Select("Destintion='" + drBranch["Destintion"].ToString() + "'"))
                    {
                        DataRow newrow = dtdiffreport.NewRow();
                        newrow["Destination"] = dr["Destintion"].ToString();
                        double purchasevalue = 0;
                        double.TryParse(drBranch["Total Value"].ToString(), out purchasevalue);
                        purchasevalue = Math.Round(purchasevalue, 2);
                        newrow["Purchase"] = purchasevalue.ToString();
                        double salesvalue = 0;
                        double.TryParse(dr["Total Value"].ToString(), out salesvalue);
                        salesvalue = Math.Round(salesvalue, 2);
                        newrow["Sales"] = salesvalue.ToString();
                        double profit = 0;
                        profit = salesvalue - purchasevalue;
                        profit = Math.Round(profit, 2);
                        newrow["Profit"] = profit.ToString();
                        double transport = 0;
                        double.TryParse(dr["Transport"].ToString(), out transport);
                        transport = Math.Round(transport, 2);
                        newrow["Transport"] = transport.ToString();
                        double totalprofit = 0;
                        totalprofit = profit + transport;
                        totalprofit = Math.Round(totalprofit, 2);
                        newrow["Total Profit"] = totalprofit.ToString();
                        dtdiffreport.Rows.Add(newrow);
                    }
                }

                grddiffreport.DataSource = dtdiffreport;
                grddiffreport.DataBind();
                Session["xportdata"] = Report;
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
                hidepanel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}