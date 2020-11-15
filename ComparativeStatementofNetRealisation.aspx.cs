using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class ComparativeStatementofNetRealisation : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    string mainhead = "";
    double grandtotal = 0;
    double grndtotalperltr = 0;
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
                    fillbranches();
                }
            }
        }
    }
    DataTable Report = new DataTable();

    void fillbranches()
    {
        cmd = new SqlCommand("SELECT sno, branchname, address, branchtype, tinno, cstno, mitno, branchcode FROM    branch_info");
        DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
        ddlbranchname.DataSource = dttrips;
        ddlbranchname.DataTextField = "branchname";
        ddlbranchname.DataValueField = "sno";
        ddlbranchname.DataBind();
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
            cmd = new SqlCommand("SELECT  branch_overhead.tq_tanker, branch_overhead.tq_close, branch_overhead.oh_month AS OverHeadDate, branch_overhead.mainaccount AS OverHeadAccount,  overheadmaster.oh_name AS OverHeadName, Branch_oh_sub.amount AS Amount, branch_overhead.remarks AS Remarks, branch_overhead.total_sale,  Branch_oh_sub.tot_qty_id, branch_overhead.branch_id, branch_info.branchname, mainoverhead_details.mainoverhead FROM branch_overhead INNER JOIN Branch_oh_sub ON branch_overhead.sno = Branch_oh_sub.branch_oh_sno INNER JOIN overheadmaster ON Branch_oh_sub.oh_id = overheadmaster.sno INNER JOIN branch_info ON branch_overhead.branch_id = branch_info.sno INNER JOIN mainoverhead_details ON branch_overhead.mainaccount = mainoverhead_details.sno WHERE  (branch_overhead.oh_month BETWEEN @d1 AND @d2) AND (branch_overhead.branch_id = @branchid)");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@branchid", ddlbranchname.SelectedValue);
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
                        grandtotal += amount;
                        newrow["Amount"] = dr["Amount"].ToString();
                        double total_qty = 0;
                        string Oh_qty_id = dr["tot_qty_id"].ToString();
                        double total_sale = 0;
                        double.TryParse(dr["total_sale"].ToString(), out total_sale);
                        double tq_tanker = 0;
                        double tq_close = 0;
                        double.TryParse(dr["tq_tanker"].ToString(), out tq_tanker);
                        double.TryParse(dr["tq_close"].ToString(), out tq_close);
                        if (Oh_qty_id == "1" || Oh_qty_id == "2" || Oh_qty_id == "3")
                        {
                            total_qty = total_sale;
                        }
                        double perltr = 0;
                        perltr = amount / total_qty;
                        perltr = Math.Round(perltr, 2);
                        newrow["Cost"] = perltr;
                        totalperltr += perltr;
                        grndtotalperltr += perltr;
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
                        grandtotal += amountt;
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
                        if (Ohh_qty_id == "1" || Ohh_qty_id == "2" || Ohh_qty_id == "3")
                        {
                            tot_qty = tot_sale;
                        }
                        double perltrs = 0;
                        perltrs = amountt / tot_qty;
                        perltrs = Math.Round(perltrs, 2);
                        newrow["Cost"] = perltrs;
                        totalperltr += perltrs;
                        grndtotalperltr += perltrs;
                        Report.Rows.Add(newrow);
                    }
                }
                DataRow newvartical3 = Report.NewRow();
                newvartical3["Over Head Name"] = "Total";
                newvartical3["Amount"] = totalamountoperating;
                newvartical3["Cost"] = totalperltr;
                Report.Rows.Add(newvartical3);

                DataRow newvartical4 = Report.NewRow();
                newvartical4["Over Head Name"] = "Grand Total";
                newvartical4["Amount"] = grandtotal;
                Report.Rows.Add(newvartical4);


                DataRow newvartical6 = Report.NewRow();
                newvartical6["Over Head Name"] = "Less Exp";
                newvartical6["Amount"] = grndtotalperltr;
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
        if (e.Row.Cells[1].Text == "Less Expensive")
        {
            e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
            e.Row.Font.Size = FontUnit.Large;
            e.Row.Font.Bold = true;
        }
        e.Row.Cells[3].BackColor = System.Drawing.Color.Bisque;
    }
}