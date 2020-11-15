using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class yeildreport : System.Web.UI.Page
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
            Report.Columns.Add("Date");
            Report.Columns.Add("Section");
            Report.Columns.Add("Milk Receive Qty");
            Report.Columns.Add("Milk Receive Fat");
            Report.Columns.Add("Total Cream Quantity");
            Report.Columns.Add("Skim Milk Qty");
            Report.Columns.Add("Skim Milk FAT");
            Report.Columns.Add("Total Skim Milk Production Fat");
            Report.Columns.Add("Production Qty");
            Report.Columns.Add("Production FAT");
            Report.Columns.Add("Total Cream Production Fat");
            Report.Columns.Add("Difference");
            Report.Columns.Add("Yeild");
            double totalyeild = 0;
            if(ddlsection.SelectedItem.Text=="All")
            {
                cmd = new SqlCommand("SELECT sno, milkreciveqty, milkrecivefat, creamqty, skimmilkqty, skimmilkfat, doe, createdby, createdon, branchid, section, butterfat, productionqty, productionfat,  totalcreamproductionfat, totalskimmilkfat FROM  creamsaparation_details WHERE (doe between @d1 and @d2) AND (branchid=@branchid) ORDER BY doe ");
            }
            else
            {
                cmd = new SqlCommand("SELECT sno, milkreciveqty, milkrecivefat, creamqty, skimmilkqty, skimmilkfat, doe, createdby, createdon, branchid, section, butterfat, productionqty, productionfat,  totalcreamproductionfat, totalskimmilkfat FROM  creamsaparation_details WHERE (doe between @d1 and @d2) AND (branchid=@branchid) AND (section=@section) ORDER BY doe");
                 cmd.Parameters.Add("@section", ddlsection.SelectedItem.Text);
            }
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetLowDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtyeild = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtyeild.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtyeild.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime doe = Convert.ToDateTime(dr["doe"].ToString());
                    string dte = doe.ToString("dd/MM/yyyy");
                    newrow["Date"] = dte;
                    newrow["Section"] = dr["section"].ToString();
                    newrow["Milk Receive Qty"] = dr["milkreciveqty"].ToString();
                    newrow["Milk Receive Fat"] = dr["milkrecivefat"].ToString();
                    newrow["Total Cream Quantity"] = dr["creamqty"].ToString();

                    newrow["Skim Milk Qty"] = dr["skimmilkqty"].ToString();
                    newrow["Skim Milk FAT"] = dr["skimmilkfat"].ToString();
                    newrow["Total Skim Milk Production Fat"] = dr["totalskimmilkfat"].ToString();

                    newrow["Production Qty"] = dr["productionqty"].ToString();
                    newrow["Production FAT"] = dr["productionfat"].ToString();
                    newrow["Total Cream Production Fat"] = dr["totalcreamproductionfat"].ToString();
                    
                    double tcqty = Convert.ToDouble(dr["creamqty"].ToString());
                    double creamproductionfat = Convert.ToDouble(dr["totalcreamproductionfat"].ToString());
                    double skimmilkfat = Convert.ToDouble(dr["totalskimmilkfat"].ToString());
                    double diff = tcqty - (creamproductionfat + skimmilkfat);

                    newrow["Difference"] = Math.Round(diff,2).ToString();
                    double mrqty = Convert.ToDouble(dr["milkreciveqty"].ToString());
                    double pqty = Convert.ToDouble(dr["productionqty"].ToString());
                    double yeield = pqty / mrqty;
                    yeield = yeield * 100;
                    totalyeild += yeield;
                    yeield = Math.Round(yeield,2);
                    newrow["Yeild"] = yeield;
                    Report.Rows.Add(newrow);
                }
                DataRow newrow1 = Report.NewRow();
                newrow1["Section"] = "Total";
                totalyeild = Math.Round(totalyeild,2);
                newrow1["Yeild"] = totalyeild;
                Report.Rows.Add(newrow1);
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
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[2].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
           
        }
    }
}