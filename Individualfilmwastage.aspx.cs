using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Individualfilmwastage : System.Web.UI.Page
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
        ddlslct_Batch.Items.Insert(0, new ListItem { Value = "0", Text = "--Select--", Selected = true });
        ddlslct_Batch.SelectedValue = "0";
    }
    private void bindproducts()
    {
        string branchid = Session["Branch_ID"].ToString();

        string batch = ddlslct_Batch.SelectedItem.Value;
        if (branchid == "1" || branchid == "22" || branchid == "26")
        {
            cmd = new SqlCommand("SELECT productmaster.sno, productmaster.batchid, productmaster.productname, batchmaster.batch FROM productmaster INNER JOIN batchmaster ON productmaster.batchid = batchmaster.batchid WHERE (productmaster.branchid = @BranchID) and (productmaster.batchid=@batchid)");
        }
        else
        {
            cmd = new SqlCommand("SELECT productmaster.sno, productmaster.batchid, productmaster.productname, batchmaster.batch FROM productmaster INNER JOIN batchmaster ON productmaster.batchid = batchmaster.batchid WHERE (productmaster.branchid = @BranchID) and (productmaster.batchid=@batchid)");
        }
        cmd.Parameters.Add("@batchid", batch);
        cmd.Parameters.Add("@BranchID", branchid);
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
            int branch = Convert.ToInt32(ddlslct_Batch.SelectedItem.Value);
            int product = Convert.ToInt32(ddlslct_product.SelectedItem.Value);
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            string Type = Session["BranchType"].ToString();
            Report.Columns.Add("sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("Batch Name");
            Report.Columns.Add("Product Name");
           // Report.Columns.Add("Silo Name");
            Report.Columns.Add("Qty Ltr");
            Report.Columns.Add("Received Film").DataType = typeof(Double);
            Report.Columns.Add("Consumption Film").DataType = typeof(Double);
            Report.Columns.Add("Return Film");
            Report.Columns.Add("Wastage Film").DataType = typeof(Double);
            Report.Columns.Add("Cutting Film");
            Report.Columns.Add("Film Wastage(%)");

            cmd = new SqlCommand("SELECT batchmaster.batch, packing_entry.qty_ltr, packing_entry.fat, packing_entry.snf, packing_entry.clr, packing_entry.received_film, packing_entry.consumption_film, packing_entry.return_film, packing_entry.doe,packing_entry.wastage_film, packing_entry.cuttingfilm, productmaster.productname FROM packing_entry INNER JOIN batchmaster ON packing_entry.batchid = batchmaster.batchid INNER JOIN productmaster ON packing_entry.productid = productmaster.sno WHERE packing_entry.batchid =@branch AND packing_entry.productid =@product and packing_entry.branchid = @BranchID AND packing_entry.doe BETWEEN @dt1 and @dt2 ORDER BY packing_entry.doe DESC");
            cmd.Parameters.Add("@branch", branch);
            cmd.Parameters.Add("@product", product);
            cmd.Parameters.Add("@dt1", GetLowDate(fromdate));
            cmd.Parameters.Add("@dt2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtPacking.Rows.Count > 0)
            {
                int i = 1;
                double wastagetotal = 0;
                double cuttingtotal = 0;
                double consumptiontotal = 0;
                double receivedtotal = 0;
                double returnfilmtotal = 0;
                double totalwastage = 0;
                double totalcuttingwastage = 0;
                double overallwastage = 0;
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
                    newrow["Received Film"] = dr["received_film"].ToString();
                    newrow["Consumption Film"] = dr["consumption_film"].ToString();
                    newrow["Return Film"] = dr["return_film"].ToString();
                    newrow["Wastage Film"] = dr["wastage_film"].ToString();
                    newrow["Cutting Film"] = dr["cuttingfilm"].ToString();

                    double wastage = 0;
                    double.TryParse(dr["wastage_film"].ToString(), out wastage);
                    wastagetotal += wastage;

                    double cutting = 0;
                    double.TryParse(dr["cuttingfilm"].ToString(), out cutting);
                   cuttingtotal += cutting;

                    double consumption = 0;
                    double.TryParse(dr["consumption_film"].ToString(), out consumption);
                    consumptiontotal += consumption;
                 

                    double RecivedFilm = 0;
                    double.TryParse(dr["received_film"].ToString(), out RecivedFilm);
                    receivedtotal += RecivedFilm;
                   

                    double ReturnFilm = 0;
                    double.TryParse(dr["return_film"].ToString(), out ReturnFilm);
                    returnfilmtotal += ReturnFilm;
                    Report.Rows.Add(newrow);

                }
                DataRow newvartical2 = Report.NewRow();
                newvartical2["Product Name"] = "Total";
                newvartical2["Received Film"] = receivedtotal;
                newvartical2["Consumption Film"] = consumptiontotal;
                newvartical2["Wastage Film"] = wastagetotal;
                newvartical2["Cutting Film"] = cuttingtotal;
                newvartical2["Return Film"] = returnfilmtotal;                
                Report.Rows.Add(newvartical2);

                DataRow newvartical3 = Report.NewRow();
                totalwastage = (wastagetotal / consumptiontotal) * 100;
                totalwastage = Math.Round(totalwastage, 2);
                newvartical3["Cutting Film"] = "Total Wastage Film(%)";
                newvartical3["Film Wastage(%)"] = totalwastage;
                Report.Rows.Add(newvartical3);

                DataRow newvartical4 = Report.NewRow();
                totalcuttingwastage = (cuttingtotal / consumptiontotal) * 100;
                totalcuttingwastage = Math.Round(totalcuttingwastage, 2);
                newvartical4["Cutting Film"] = "Total Cutting Film(%)";
                newvartical4["Film Wastage(%)"] = totalcuttingwastage;
                Report.Rows.Add(newvartical4);

                DataRow newvartical5 = Report.NewRow();
                overallwastage = ((wastagetotal + cuttingtotal) / consumptiontotal) * 100;
                overallwastage = Math.Round(overallwastage, 2);
                newvartical5["Cutting Film"] = "Over All Film Wastage(%)";
                newvartical5["Film Wastage(%)"] = overallwastage;
                Report.Rows.Add(newvartical5);


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