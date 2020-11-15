using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class curdpktqualityrpt : System.Web.UI.Page
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
                    DateTime dt = DateTime.Now.AddDays(-1);
                    dtp_FromDate.Text = dt.ToString("dd-MM-yyyy");
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
            string Type = Session["BranchType"].ToString();
            Report.Columns.Add("sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Packet Size");
            Report.Columns.Add("Sample No");
            Report.Columns.Add("Use By Date");
            Report.Columns.Add("MRP");
            Report.Columns.Add("Temp");
            cmd = new SqlCommand("SELECT productmaster.productname,  curdpkt_qualitytesting.doe, curdpkt_qualitytesting_subdetails.productid, curdpkt_qualitytesting.sample, curdpkt_qualitytesting_subdetails.mrp, curdpkt_qualitytesting_subdetails.packetsize,  curdpkt_qualitytesting_subdetails.temp, curdpkt_qualitytesting_subdetails.usebydate, curdpkt_qualitytesting.sno FROM curdpkt_qualitytesting INNER JOIN curdpkt_qualitytesting_subdetails ON curdpkt_qualitytesting.sno = curdpkt_qualitytesting_subdetails.cpktrefno INNER JOIN productmaster ON curdpkt_qualitytesting_subdetails.productid = productmaster.sno WHERE curdpkt_qualitytesting.doe BETWEEN @d1 and @d2 AND  (curdpkt_qualitytesting.branchid=@BranchID) ORDER BY curdpkt_qualitytesting.doe");            
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtgheeqlty = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtgheeqlty.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtgheeqlty.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                    string date = dtdoe.ToString("dd/MM/yyyy");
                    newrow["Date"] = date;
                    newrow["Product Name"] = dr["productname"].ToString();
                    newrow["Packet Size"] = dr["packetsize"].ToString();
                    newrow["Sample No"] = dr["sample"].ToString();
                    DateTime dtusebydate = Convert.ToDateTime(dr["usebydate"].ToString());
                    newrow["Use By Date"] = dtusebydate.ToString("dd/MM/yyyy");
                    newrow["MRP"] = dr["mrp"].ToString();
                    newrow["Temp"] = dr["temp"].ToString();
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
                grdReports.DataSource = null;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}