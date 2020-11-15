using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class weighingdiffreport : System.Web.UI.Page
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
        bindgrid();
    }

    protected void gvMenu_DataBinding(object sender, EventArgs e)
    {
        try
        {
            //GridViewGroup First = new GridViewGroup(grdReports, null, "TicketNo");
            //GridViewGroup Second = new GridViewGroup(grdReports, First, "Truck No");
            //GridViewGroup thired = new GridViewGroup(grdReports, Second, "Vehicle Type");
            //GridViewGroup four = new GridViewGroup(grdReports, thired, "Name");
            //GridViewGroup five = new GridViewGroup(grdReports, four, "Gross Weight");
            //GridViewGroup six = new GridViewGroup(grdReports, five, "Tare Weight");
            //GridViewGroup seveen = new GridViewGroup(grdReports, six, "Net Weight");
            //GridViewGroup eight = new GridViewGroup(grdReports, seveen, "Amount");


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void bindgrid()
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
            Report.Columns.Add("Route Name");
            Report.Columns.Add("Vehicle No");
            Report.Columns.Add("CC Weight");
            Report.Columns.Add("WB Weight");
            Report.Columns.Add("Difference Weight");
            Report.Columns.Add("Difference fat");
            Report.Columns.Add("Difference snf");
            Report.Columns.Add("Weight Date");
            Report.Columns.Add("lAB Date");
            cmd = new SqlCommand("SELECT weighingdiffdetails.vehicleno, vendors.vendorname, weighingdiffdetails.ccweight, weighingdiffdetails.weight, weighingdiffdetails.diffweight, weighingdiffdetails.transactionno, weighingdiffdetails.doe from weighingdiffdetails INNER JOIN despatch_entry on despatch_entry.sno = weighingdiffdetails.transactionno INNER JOIN vendors on vendors.sno = despatch_entry.cc_id WHERE (weighingdiffdetails.doe BETWEEN @d1 AND @d2) ");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            DataTable dtweight = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtweight.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtweight.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Route Name"] = dr["vendorname"].ToString();
                    string date = dr["doe"].ToString();
                    newrow["Vehicle No"] = dr["vehicleno"].ToString();
                    newrow["CC Weight"] = dr["ccweight"].ToString();
                    newrow["WB Weight"] = dr["weight"].ToString();
                    newrow["Difference Weight"] = dr["diffweight"].ToString();
                    newrow["Weight Date"] = date;
                    Report.Rows.Add(newrow);
                }
            }
            grdReports.DataSource = Report;
            grdReports.DataSource = Report;
            grdReports.DataBind();
            hidepanel.Visible = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}