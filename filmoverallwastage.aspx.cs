using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class filmoverallwastage : System.Web.UI.Page
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
                    // dtp_FromDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    //  dtp_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                    lblAddress.Text = Session["Address"].ToString();
                    lblTitle.Text = Session["TitleName"].ToString();
                    bindbranches();
                }
            }
        }
    }

    private void bindbranches()
    {
        SalesDBManager SalesDB = new SalesDBManager();
        cmd = new SqlCommand("SELECT batchid, batch, batchcode FROM batchmaster");
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlslct_Batch.DataSource = dttrips;
        ddlslct_Batch.DataTextField = "batch";
        ddlslct_Batch.DataValueField = "batchid";
        ddlslct_Batch.DataBind();
        ddlslct_Batch.ClearSelection();
        ddlslct_Batch.Items.Insert(0, new ListItem { Value = "0", Text = "--Select Batch--", Selected = true });
        ddlslct_Batch.SelectedValue = "0";
    }
    private void bindproducts()
    {
        string branchid = Session["Branch_ID"].ToString();
        string batch = ddlslct_Batch.SelectedItem.Value;
        cmd = new SqlCommand("SELECT productmaster.sno, productmaster.batchid, productmaster.productname, batchmaster.batch FROM productmaster INNER JOIN batchmaster ON productmaster.batchid = batchmaster.batchid WHERE (productmaster.branchid = @BranchID) and (productmaster.batchid=@batchid)");
        cmd.Parameters.Add("@batchid", batch);
        cmd.Parameters.Add("@BranchID", branchid);
        DataTable dtproduct = vdm.SelectQuery(cmd).Tables[0];
        ddlslct_product.DataSource = dtproduct;
        ddlslct_product.DataTextField = "productname";
        ddlslct_product.DataValueField = "sno";
        ddlslct_product.DataBind();
        ddlslct_product.ClearSelection();
        ddlslct_product.Items.Insert(0, new ListItem { Value = "0", Text = "--Select Product--", Selected = true });
        ddlslct_product.SelectedValue = "0";
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindproducts();
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
        try
        {
            lblmsg.Text = "";
            SalesDBManager SalesDB = new SalesDBManager();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            int branch = Convert.ToInt32(ddlslct_Batch.SelectedItem.Value);
            int product = Convert.ToInt32(ddlslct_product.SelectedItem.Value);

            string Type = Session["BranchType"].ToString();
            Report.Columns.Add("sno");
           
            Report.Columns.Add("Batch Name");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Received Film");
            Report.Columns.Add("Consumption Film");
            Report.Columns.Add("Return Film");
            Report.Columns.Add("Wastage Film");
            Report.Columns.Add("Cutting Film");
            Report.Columns.Add("Total Wastage Film (%)");
            cmd = new SqlCommand("SELECT batchmaster.batch, sum(packing_entry.received_film) as receivedfilm, sum(packing_entry.consumption_film) as consumptionfilm, sum(packing_entry.return_film) as returnfilm, sum(packing_entry.wastage_film) as wastagefilm, sum(packing_entry.cuttingfilm) as cutting_film, productmaster.productname FROM packing_entry INNER JOIN batchmaster ON packing_entry.batchid = batchmaster.batchid INNER JOIN silomaster ON packing_entry.siloid = silomaster.SiloId INNER JOIN productmaster ON packing_entry.productid = productmaster.sno WHERE packing_entry.batchid =@d1 AND packing_entry.productid =@d2 and packing_entry.branchid = @BranchID group by batchmaster.batch, productmaster.productname");
            cmd.Parameters.Add("@d1", branch);
            cmd.Parameters.Add("@d2", product);
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtPacking.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtPacking.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Batch Name"] = dr["batch"].ToString();
                    newrow["Product Name"] = dr["productname"].ToString();
                    newrow["Received Film"] = dr["receivedfilm"].ToString();
                    newrow["Consumption Film"] = dr["consumptionfilm"].ToString();
                    newrow["Return Film"] = dr["returnfilm"].ToString();
                    newrow["Wastage Film"] = dr["wastagefilm"].ToString();
                    newrow["Cutting Film"] = dr["cutting_film"].ToString();

                    double cuttingfilm = Convert.ToDouble(dr["cutting_film"].ToString());
                    double cfilm = Convert.ToDouble(dr["consumptionfilm"].ToString());
                    double wfilm = Convert.ToDouble(dr["wastagefilm"].ToString());
                    double sumwastage = cuttingfilm + wfilm;
                    double totalfilm = (sumwastage / cfilm) * 100;
                    newrow["Total Wastage Film (%)"] = totalfilm;
                    Report.Rows.Add(newrow);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}