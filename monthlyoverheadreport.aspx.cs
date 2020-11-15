using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class monthlyoverheadreport : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    string mainhead = "";
    double grandtotal = 0;
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
                    //filloverhead();
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
            Report.Columns.Add("Main Head");
            Report.Columns.Add("Over Head Name");
            Report.Columns.Add("Amount");
            Report.Columns.Add("Cost");
            cmd = new SqlCommand("SELECT  branch_overhead.tq_tanker, branch_overhead.tq_close,mainoverhead_details.sno, branch_overhead.oh_month AS OverHeadDate, branch_overhead.mainaccount AS OverHeadAccount,  overheadmaster.oh_name AS OverHeadName, Branch_oh_sub.amount AS Amount, branch_overhead.remarks AS Remarks, branch_overhead.total_sale,  Branch_oh_sub.tot_qty_id, mainoverhead_details.mainoverhead FROM branch_overhead INNER JOIN Branch_oh_sub ON branch_overhead.sno = Branch_oh_sub.branch_oh_sno INNER JOIN overheadmaster ON Branch_oh_sub.oh_id = overheadmaster.sno INNER JOIN mainoverhead_details ON branch_overhead.mainaccount = mainoverhead_details.sno WHERE  (branch_overhead.oh_month BETWEEN @d1 AND @d2) AND (branch_overhead.branch_id = @branchid)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dttable = SalesDB.SelectQuery(cmd).Tables[0];

            if (dttable.Rows.Count > 0)
            {

                string priviousohname = "";
                double totalamountoperating = 0;
                double totalperltr = 0;
                double totalsalevalue = 0;
                double oecost = 0;
                foreach (DataRow dr in dttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    string headname = dr["OverHeadAccount"].ToString();
                    if (headname == priviousohname)
                    {
                        newrow["Main Head"] = dr["mainoverhead"].ToString();
                        newrow["Over Head Name"] = dr["OverHeadName"].ToString();
                        double amount = 0;
                        double.TryParse(dr["Amount"].ToString(), out amount);
                        newrow["Amount"] = amount;
                        totalamountoperating += amount;
                        if (headname == "1" || headname == "5")
                        {
                            grandtotal += 0;
                        }
                        else
                        {
                            grandtotal += amount;
                        }
                        //grandtotal += totalamountoperating;
                        newrow["Amount"] = dr["Amount"].ToString();
                        double total_qty = 0;
                        string Oh_qty_id = dr["tot_qty_id"].ToString();
                        double total_sale = 0;
                        double.TryParse(dr["total_sale"].ToString(), out total_sale);
                        double tq_tanker = 0;
                        double tq_close = 0;
                        double.TryParse(dr["tq_tanker"].ToString(), out tq_tanker);
                        double.TryParse(dr["tq_close"].ToString(), out tq_close);
                        if (Oh_qty_id == "1")
                        {
                            total_qty = total_sale;
                        }
                        if (Oh_qty_id == "2")
                        {
                            total_qty = tq_tanker;
                        }
                        if (Oh_qty_id == "3")
                        {
                            total_qty = tq_close;
                        }
                        double perltr = 0;
                        perltr = amount / total_qty;
                        perltr = Math.Round(perltr, 2);
                        newrow["Cost"] = perltr;
                        totalperltr += perltr;
                        Report.Rows.Add(newrow);
                    }
                    else
                    {
                        if (totalamountoperating > 0)
                        {
                            DataRow newvartical2 = Report.NewRow();
                            newvartical2["Over Head Name"] = "Total";
                            newvartical2["Amount"] = totalamountoperating;
                            newvartical2["Cost"] = totalperltr;
                            Report.Rows.Add(newvartical2);
                            if (priviousohname == "Operating Expenses")
                            {
                                oecost = totalperltr;
                            }
                            totalamountoperating = 0;
                            totalperltr = 0;
                        }
                        priviousohname = headname;
                        newrow["Main Head"] = dr["mainoverhead"].ToString();
                        newrow["Over Head Name"] = dr["OverHeadName"].ToString();
                        double amountt = 0;
                        double.TryParse(dr["Amount"].ToString(), out amountt);
                        newrow["Amount"] = amountt;
                        totalamountoperating += amountt;
                        if (headname == "1" || headname == "5")
                        {
                            grandtotal += 0;
                        }
                        else
                        {
                            grandtotal += amountt;
                        }
                        newrow["Amount"] = dr["Amount"].ToString();
                        double tot_qty = 0;
                        string Ohh_qty_id = dr["tot_qty_id"].ToString();
                        double tot_sale = 0;
                        double.TryParse(dr["total_sale"].ToString(), out tot_sale);
                        totalsalevalue = tot_sale;
                        double tq_tankerr = 0;
                        double tq_closee = 0;
                        double.TryParse(dr["tq_tanker"].ToString(), out tq_tankerr);
                        double.TryParse(dr["tq_close"].ToString(), out tq_closee);
                        if (Ohh_qty_id == "1")
                        {
                            tot_qty = tot_sale;
                        }
                        if (Ohh_qty_id == "2")
                        {
                            tot_qty = tq_tankerr;
                        }
                        if (Ohh_qty_id == "3")
                        {
                            tot_qty = tq_closee;
                        }
                        double perltrs = 0;
                        perltrs = amountt / tot_qty;
                        perltrs = Math.Round(perltrs, 2);
                        newrow["Cost"] = perltrs;
                        totalperltr += perltrs;
                        Report.Rows.Add(newrow);
                    }
                }
                DataRow newvartical3 = Report.NewRow();
                newvartical3["Over Head Name"] = "Total";
                newvartical3["Amount"] = totalamountoperating;
                newvartical3["Cost"] = totalperltr;
                Report.Rows.Add(newvartical3);

                DataRow newvartical4 = Report.NewRow();
                newvartical4["Over Head Name"] = "Total Expenses";
                newvartical4["Amount"] = grandtotal;
                Report.Rows.Add(newvartical4);

                DataRow newvartical5 = Report.NewRow();
                newvartical5["Over Head Name"] = "Total sale";
                newvartical5["Amount"] = totalsalevalue;
                Report.Rows.Add(newvartical5);

                DataRow newvartical6 = Report.NewRow();
                newvartical6["Over Head Name"] = "COST PER LTR";
                double cpl = Math.Round(grandtotal / totalsalevalue, 2);
                newvartical6["Amount"] = (cpl + oecost);
                Report.Rows.Add(newvartical6);

                grdReports.DataSource = Report;
                grdReports.DataBind();
                hidepanel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }

    protected void gvMenu_DataBinding(object sender, EventArgs e)
    {
        string BranchID = Session["Branch_ID"].ToString();
        if (BranchID == "1")
        {
            GridViewGroup First = new GridViewGroup(grdReports, null, "Main Head");
        }
        else
        {
            GridViewGroup First = new GridViewGroup(grdReports, null, "Main Head");
        }
    }

    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells[1].Text == "Total")
        {
            e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
            e.Row.Font.Size = FontUnit.Large;
            e.Row.Font.Bold = true;
        }
        if (e.Row.Cells[1].Text == "Grand Total")
        {
            e.Row.BackColor = System.Drawing.Color.Bisque;
            e.Row.Font.Size = FontUnit.Large;
            e.Row.Font.Bold = true;
        }
        e.Row.Cells[3].BackColor = System.Drawing.Color.Bisque;
    }
}