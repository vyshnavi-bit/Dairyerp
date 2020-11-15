using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class vendor_details_report : System.Web.UI.Page
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
                    Getvendordetails();
                }
            }
        }
    }
    void Getvendordetails()
    {
        try
        {
            Session["filename"] = "Vendor Details";
            Session["title"] = "Vendor Details Information";
            lblmsg.Text = "";
            cmd = new SqlCommand("SELECT vendors.vendorname, vendors.email, vendors.mobno, vendor_subtable.rate_on, vendor_subtable.calc_on, vendor_subtable.cost, vendor_subtable.overheadon AS OH, vendor_subtable.overheadcost AS OHCost,vendor_subtable.fatplus_on, Round(vendor_subtable.m_fatpluscost,2) as m_fatpluscost, round(vendor_subtable.p_fatpluscost,2) as p_fatpluscost,  vendor_subtable.m_std_fat, vendor_subtable.p_std_fat, vendor_subtable.m_std_snf, vendor_subtable.p_std_snf, vendor_subtable.snfplus_on, ROUND(vendor_subtable.m_snfpluscost, 2) AS m_snfpluscost, ROUND(vendor_subtable.p_snfpluscost, 2) AS p_snfpluscost, vendor_subtable.transport_on, vendor_subtable.transportcost FROM vendors LEFT OUTER JOIN vendor_subtable ON vendors.sno = vendor_subtable.vendor_refno WHERE (vendors.branchid = @BranchID)");
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtDispatch = vdm.SelectQuery(cmd).Tables[0];
            if (dtDispatch.Rows.Count > 0)
            {
                grdReports.DataSource = dtDispatch;
                grdReports.DataBind();
                Session["xportdata"] = dtDispatch;
                hidepanel.Visible = true;
            }
            else
            {
                hidepanel.Visible = false;
                lblmsg.Text = "No data were found";
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}