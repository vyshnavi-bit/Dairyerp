using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class coldroomstockreport : System.Web.UI.Page
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

    private void bindproducts()
    {
        cmd = new SqlCommand("SELECT sno, productname FROM productmaster where departmentid='2' AND branchid=@branchid");
        cmd.Parameters.Add("@branchid", BranchID);
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlproductType.DataSource = dttrips;
        ddlproductType.DataTextField = "productname";
        ddlproductType.DataValueField = "sno";
        ddlproductType.DataBind();
        ddlproductType.ClearSelection();
        ddlproductType.Items.Insert(0, new ListItem { Value = "0", Text = "--Select Product--", Selected = true });
        ddlproductType.SelectedValue = "0";
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
            Report.Columns.Add("Sno");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Opening Balance");
            Report.Columns.Add("Production Quantity");
            Report.Columns.Add("Dispatch Quantity");
            Report.Columns.Add("Cutting");
            Report.Columns.Add("Transfer");
            Report.Columns.Add("Closing Balance");
            Report.Columns.Add("Date");
            if (ddlproductType.SelectedValue != "0")
            {
                cmd = new SqlCommand("SELECT cpm.sno, cpm.productid, cpm.dispatchquantity, cpm.rootname, cpm.cutting, cpm.producttotalquantity, cpm.remarks, cpm.userid, cpm.branchid, cpm.doe,  pm.productname, bi.branchname, cpm.transfer, cpm.closingbalance FROM   coldroomstockdetails AS cpm INNER JOIN productmaster AS pm ON pm.sno = cpm.productid INNER JOIN branch_info AS bi ON bi.sno = cpm.branchid WHERE (cpm.doe BETWEEN @d1 AND @d2) AND (cpm.branchid = @branchid) AND (cpm.productid = @productid) order by cpm.doe ");
                cmd.Parameters.Add("@productid", ddlproductType.SelectedValue);
            }
            else
            {
                cmd = new SqlCommand("SELECT cpm.sno, cpm.productid, cpm.dispatchquantity, cpm.rootname, cpm.cutting, cpm.producttotalquantity, cpm.remarks, cpm.userid, cpm.branchid, cpm.doe,  pm.productname, bi.branchname, cpm.transfer, cpm.closingbalance FROM   coldroomstockdetails AS cpm INNER JOIN productmaster AS pm ON pm.sno = cpm.productid INNER JOIN branch_info AS bi ON bi.sno = cpm.branchid WHERE (cpm.doe BETWEEN @d1 AND @d2) AND (cpm.branchid = @branchid) order by cpm.doe ");
            }
            //DateTime dtfrom = GetLowDate(fromdate);
            //DateTime dtfromdate = dtfrom.AddHours(6);
            //DateTime dtto = GetLowDate(todate);
            //DateTime dttodate = dtto.AddHours(30);

            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtInward.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtInward.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["Date"] = date;
                    newrow["Product Name"] = dr["productname"].ToString();
                    string productid = dr["productid"].ToString();
                    newrow["Opening Balance"] = dr["producttotalquantity"].ToString();
                    cmd = new SqlCommand("SELECT  sno, batchid, siloid, qty_ltr, fat, snf, clr, received_film, consumption_film, return_film, branchid, entry_by, doe, productid, wastage_film, cuttingfilm, processingstatus, reciveddate, filmrate FROM  packing_entry WHERE (doe BETWEEN @d1 AND @d2) AND (productid = @productid) AND branchid=@branchid");
                    cmd.Parameters.Add("@productid", productid);
                    cmd.Parameters.Add("@d1", GetLowDate(dtdoe));
                    cmd.Parameters.Add("@d2", GetHighDate(dtdoe));
                    cmd.Parameters.Add("@branchid", BranchID);
                    DataTable dtproduction = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtproduction.Rows.Count > 0)
                    {
                        foreach (DataRow drr in dtproduction.Rows)
                        {
                            string production = drr["qty_ltr"].ToString();
                            if (production == "" || production == null)
                            {
                                newrow["Production Quantity"] = "0";
                            }
                            else
                            {
                                newrow["Production Quantity"] = production.ToString();
                            }
                        }
                    }
                    newrow["Dispatch Quantity"] = dr["dispatchquantity"].ToString();
                    newrow["Cutting"] = dr["cutting"].ToString();
                    newrow["Transfer"] = dr["transfer"].ToString();
                    newrow["Closing Balance"] = dr["closingbalance"].ToString();
                    Report.Rows.Add(newrow);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
                hidepanel.Visible = false;
                grdReports.DataSource = null;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
}