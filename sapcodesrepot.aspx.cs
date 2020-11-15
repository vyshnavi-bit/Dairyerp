using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class sapcodesrepot : System.Web.UI.Page
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
            Session["title"] = "Vendor SAP Codes Details Information";
            lblmsg.Text = "";
            cmd = new SqlCommand("SELECT vendorname, ccname, tallyoh, ledgertype, ledgertype1, mobno2, sapcode, salesledgertype, salestallyoh, salesledgercode, sapvendorcode, purchaseohcode, salesohcode, customername, sapcustomercode FROM   vendors");
            //cmd.Parameters.Add("@BranchID", BranchID);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}