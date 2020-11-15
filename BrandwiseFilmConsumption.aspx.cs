using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class BrandwiseFilmConsumption : System.Web.UI.Page
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
                    bindproducts();
                }
            }
        }
    }

   
    private void bindproducts()
    {
        cmd = new SqlCommand("SELECT productmaster.sno, productmaster.batchid, productmaster.productname, batchmaster.batch FROM productmaster INNER JOIN batchmaster ON productmaster.batchid = batchmaster.batchid where (productmaster.departmentid='2') AND (productmaster.branchid=@branchid)");
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dtproduct = vdm.SelectQuery(cmd).Tables[0];
        ddlslct_product.DataSource = dtproduct;
        ddlslct_product.DataTextField = "productname";
        ddlslct_product.DataValueField = "sno";
        ddlslct_product.DataBind();
        ddlslct_product.ClearSelection();
        ddlslct_product.Items.Insert(0, new ListItem { Value = "0", Text = "--Select--", Selected = true });
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
            string branchid = Session["Branch_ID"].ToString();
            int product = Convert.ToInt32(ddlslct_product.SelectedItem.Value);
            string Type = Session["BranchType"].ToString();
            Report.Columns.Add("sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("Batch Name");
            Report.Columns.Add("Product Name");
           // Report.Columns.Add("Silo Name");
            Report.Columns.Add("Qty Ltr");
            Report.Columns.Add("No of Packets").DataType = typeof(Double);
            if (product == 0)
            {
                cmd = new SqlCommand("SELECT  batchmaster.batch, packing_entry.qty_ltr, packing_entry.fat, packing_entry.snf, packing_entry.clr, packing_entry.received_film, packing_entry.doe, productmaster.ml,  productmaster.productname FROM  packing_entry INNER JOIN  batchmaster ON packing_entry.batchid = batchmaster.batchid INNER JOIN productmaster ON packing_entry.productid = productmaster.sno WHERE  (packing_entry.doe BETWEEN @dt1 AND @dt2) AND (packing_entry.branchid = @bid) ORDER BY packing_entry.doe DESC");
                cmd.Parameters.Add("@dt1", GetLowDate(fromdate));
                cmd.Parameters.Add("@dt2", GetHighDate(todate));
                cmd.Parameters.Add("@bid", branchid);
            }
            else
            {
                cmd = new SqlCommand("SELECT batchmaster.batch, packing_entry.qty_ltr, packing_entry.fat, packing_entry.snf, packing_entry.clr, packing_entry.received_film,  packing_entry.doe, productmaster.ml, productmaster.productname FROM packing_entry INNER JOIN batchmaster ON packing_entry.batchid = batchmaster.batchid  INNER JOIN productmaster ON packing_entry.productid = productmaster.sno WHERE packing_entry.productid =@d2 AND packing_entry.doe between @dt1 and @dt2 ORDER BY packing_entry.doe DESC");
                cmd.Parameters.Add("@d2", product);
                cmd.Parameters.Add("@dt1", GetLowDate(fromdate));
                cmd.Parameters.Add("@dt2", GetHighDate(todate));
                cmd.Parameters.Add("@BranchID", branchid);
            }
            DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtPacking.Rows.Count > 0)
            {
                int i = 1;
                double kgfattotal = 0;
                double kgsnftotal = 0;
                double kgstotal = 0;
                double Ltrstotal = 0;
                double noofpkt = 0;
                double packed = 0;
                foreach (DataRow dr in dtPacking.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["DATE"] = date;
                    newrow["Batch Name"] = dr["batch"].ToString();
                    newrow["Product Name"] = dr["productname"].ToString();
                   // newrow["Silo Name"] = dr["SiloName"].ToString();
                    newrow["Qty Ltr"] = dr["qty_ltr"].ToString();
                    
                   // newrow["Received Film"] = dr["received_film"].ToString();
                    string mlltr = dr["ml"].ToString();
                    double qty = Convert.ToDouble(dr["qty_ltr"].ToString());
                    if (mlltr == "100")
                    {
                         packed = 10 * qty;
                        newrow["No of Packets"] = packed;
                    }
                    if (mlltr == "200")
                    {
                         packed = 5 * qty;
                        newrow["No of Packets"] = packed;
                    }
                    if (mlltr == "250")
                    {
                         packed = 4 * qty;
                        newrow["No of Packets"] = packed;
                    }
                    if (mlltr == "500")
                    {
                         packed = 2 * qty;
                        newrow["No of Packets"] = packed;
                    }
                    if (mlltr == "1000")
                    {
                        packed = 1 * qty;
                        newrow["No of Packets"] = packed;
                    }
                    kgstotal += packed;
                    //newrow["Consumption Film"] = dr["consumption_film"].ToString();
                    //newrow["Return Film"] = dr["return_film"].ToString();
                    //newrow["Wastage Film"] = dr["wastage_film"].ToString();
                    Report.Rows.Add(newrow);
                }
               
                DataRow newvartical2 = Report.NewRow();
                newvartical2["Product Name"] = "Total Packets";
                newvartical2["No of Packets"] = kgstotal;
                Report.Rows.Add(newvartical2);
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
                hidepanel.Visible = true;
            }
            else
            {
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
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