using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class filmconsumpctiontax : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    SalesDBManager vdm;
    PODbmanager pdm;
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
                    dtp_Todate.Text = DateTime.Now.ToString("dd-MM-yyyy");
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

        lblmsg.Text = "";
        SalesDBManager SalesDB = new SalesDBManager();
        PODbmanager PoDB = new PODbmanager();
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
        Report.Columns.Add("Received Film");
        if (ddltaxtype.SelectedValue == "Non Taxable")
        {
            cmd = new SqlCommand("SELECT   productmaster.sno,productmaster.productname, productmaster.igst, productmonitar.branchid, SUM(packing_entry.received_film) AS received_film, productmaster.batchid FROM  productmaster INNER JOIN productmonitar ON productmaster.sno = productmonitar.productid INNER JOIN packing_entry ON productmonitar.productid = packing_entry.productid WHERE  (productmonitar.branchid = @branchid) AND (productmaster.igst = 0) AND (packing_entry.doe BETWEEN @d1 AND @d2) GROUP BY productmaster.productname, productmaster.igst, productmonitar.branchid, productmaster.batchid,productmaster.sno ORDER BY productmaster.batchid");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
        }
        else
        {
            cmd = new SqlCommand("SELECT   productmaster.sno,productmaster.productname, productmaster.igst, productmonitar.branchid, SUM(packing_entry.received_film) AS received_film, productmaster.batchid FROM  productmaster INNER JOIN productmonitar ON productmaster.sno = productmonitar.productid INNER JOIN packing_entry ON productmonitar.productid = packing_entry.productid WHERE  (productmonitar.branchid = @branchid) AND (productmaster.igst > 0) AND (packing_entry.doe BETWEEN @d1 AND @d2) GROUP BY productmaster.productname, productmaster.igst, productmonitar.branchid, productmaster.batchid,productmaster.sno ORDER BY productmaster.batchid");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
        }
        DataTable dtPacking = SalesDB.SelectQuery(cmd).Tables[0];
        double totrecivedfilm = 0;
        if (dtPacking.Rows.Count > 0)
        {
            int i = 1;
            foreach (DataRow dr in dtPacking.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = i++.ToString();
                newrow["Product Name"] = dr["productname"].ToString();
                string pid = dr["sno"].ToString();
                double received_film = 0;
                if (pid == "133" || pid == "95" || pid == "132")
                {
                    newrow["Received Film"] = "0";
                    double.TryParse("0", out received_film);
                }
                else
                {
                    newrow["Received Film"] = dr["received_film"].ToString();
                    double.TryParse(dr["received_film"].ToString(), out received_film);
                }
                totrecivedfilm += received_film;
                Report.Rows.Add(newrow);
            }
            DataRow newvartical2 = Report.NewRow();
            newvartical2["Product Name"] = "Total";
            newvartical2["Received Film"] = Math.Round(totrecivedfilm, 2).ToString();
            Report.Rows.Add(newvartical2);
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
            hidepanel.Visible = true;
        }
        else
        {
            hidepanel.Visible = false;
            lblmsg.Text = "No data found";
        }
    }
}