using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class ComparativeStatementAllBranches : System.Web.UI.Page
{
    SqlCommand cmd;
    string BranchID = "";
    string mainhead = "";
    double grandtotal = 0;
    double grndtotalperltr = 0;
    SalesDBManager vdm;
    private string _seperator = "|";
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
                    //fillbranches();
                }
            }
        }
    }
    DataTable Report = new DataTable();
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
            string branchid = "";
            Session["filename"] = "Comparative Statement For All Branches";
            Session["title"] = "Comparative Statement For All Branches";
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbltodate.Text = todate.ToString("dd/MMM/yyyy");
            cmd = new SqlCommand("SELECT  branch_info.branchname, branch_info.sno, branchmapping.superbranch FROM   branch_info INNER JOIN branchmapping ON branch_info.sno = branchmapping.subbranch WHERE    (branchmapping.superbranch = @branchid) ORDER BY branch_info.sno");
            cmd.Parameters.Add("@branchid", BranchID);
            DataTable dtbranches = SalesDB.SelectQuery(cmd).Tables[0];
            cmd = new SqlCommand("SELECT   mainoverhead_details.mainoverhead, overheadmaster.oh_name, overheadmaster.sno FROM   mainoverhead_details INNER JOIN overheadmaster ON mainoverhead_details.sno = overheadmaster.mainaccount");
            DataTable dtheads = SalesDB.SelectQuery(cmd).Tables[0];
            cmd = new SqlCommand("SELECT  branch_overhead.tq_tanker, branch_overhead.tq_close, branch_overhead.oh_month AS OverHeadDate, branch_overhead.mainaccount AS OverHeadAccount,  overheadmaster.oh_name AS OverHeadName, Branch_oh_sub.amount AS Amount, branch_overhead.remarks AS Remarks, branch_overhead.total_sale,  Branch_oh_sub.tot_qty_id, branch_overhead.branch_id, branch_info.branchname, mainoverhead_details.mainoverhead FROM  branch_overhead INNER JOIN Branch_oh_sub ON branch_overhead.sno = Branch_oh_sub.branch_oh_sno INNER JOIN overheadmaster ON Branch_oh_sub.oh_id = overheadmaster.sno INNER JOIN branch_info ON branch_overhead.branch_id = branch_info.sno INNER JOIN mainoverhead_details ON branch_overhead.mainaccount = mainoverhead_details.sno WHERE (branch_overhead.oh_month BETWEEN @d1 AND @d2) ORDER BY branch_overhead.branch_id ");
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            DataTable timeSheetData = SalesDB.SelectQuery(cmd).Tables[0];

            // Creating a customized time sheet table for binding with data grid view
            var timeSheet = new DataTable("");

            timeSheet.Columns.Add("OverHeadAccount" + _seperator + "");
            timeSheet.Columns.Add("OverHeadName" + _seperator + "");

            foreach (DataRow item in dtbranches.Rows)
            {
                string columnName = item["branchname"].ToString().Trim();
                timeSheet.Columns.Add(columnName + _seperator + "Total Exp");
                timeSheet.Columns.Add(columnName + _seperator + "Per ltr Exp");
            }

            string branchname = "";
            double total = 0;
            foreach (DataRow row in dtheads.Rows)
            {
                var dataRow = timeSheet.NewRow();
                string ohname = row["oh_name"].ToString();
                string OverHeadAccount = row["mainoverhead"].ToString();
                //string branchnames = dtbranches.Rows[0]["branchname"].ToString();
                dataRow["OverHeadAccount" + _seperator + ""] = row["mainoverhead"].ToString();
                dataRow["OverHeadName" + _seperator + ""] = row["oh_name"].ToString();
                foreach (DataRow dra in timeSheetData.Select("OverHeadName='" + ohname + "' AND mainoverhead='" + OverHeadAccount + "'"))
                {
                    branchname = dra["branchname"].ToString();
                    //dataRow["OverHeadAccount" + _seperator + ""] = dra["mainoverhead"].ToString();
                    //dataRow["OverHeadName" + _seperator + ""] = dra["OverHeadName"].ToString();
                    double amount = 0;
                    double.TryParse(dra["Amount"].ToString(), out amount);
                    double total_qty = 0;
                    double total_sale = 0;
                    double.TryParse(dra["total_sale"].ToString(), out total_sale);
                    string Oh_qty_id = dra["tot_qty_id"].ToString();
                    if (Oh_qty_id == "1" || Oh_qty_id == "2" || Oh_qty_id == "3")
                    {
                        total_qty = total_sale;
                    }
                    double perltr = 0;
                    perltr = amount / total_qty;
                    perltr = Math.Round(perltr, 2);
                    dataRow[branchname + "|Total Exp"] = amount;
                    total += amount;
                    dataRow[branchname + "|Per ltr Exp"] = perltr;
                }
                timeSheet.Rows.Add(dataRow);
            }
            grdReports.DataSource = timeSheet;
            grdReports.DataBind();
            Session["xportdata"] = timeSheet;
            hidepanel.Visible = true;

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }
    protected void gvMenu_DataBinding(object sender, EventArgs e)
    {
        GridViewGroup First = new GridViewGroup(grdReports, null, "OverHeadAccount|");
    }

    protected void gvMenu_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int cellCount = e.Row.Cells.Count;
        
        for (int item = 3; item < cellCount; item = item + 2)
        {
            if (e.Row.Cells != null)
            {
                var cellText = e.Row.Cells[item].Text;
            }
        }
    }

    protected void gvMenu_RowCreated(object sender, GridViewRowEventArgs e)
    {
        
        if (e.Row.RowType == DataControlRowType.Header)
            CustomizeGridHeader((GridView)sender, e.Row, 2);
    }

    private void CustomizeGridHeader(GridView timeSheetGrid, GridViewRow gridRow, int headerLevels)
    {
        for (int item = 1; item <= headerLevels; item++)
        {
            
            GridViewRow gridviewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            IEnumerable<IGrouping<string, string>> gridHeaders = null;

            
            gridHeaders = gridRow.Cells.Cast<TableCell>()
                        .Select(cell => GetHeaderText(cell.Text, item))
                        .GroupBy(headerText => headerText);

            foreach (var header in gridHeaders)
            {
                TableHeaderCell cell = new TableHeaderCell();

                if (item == 2)
                {
                    cell.Text = header.Key.Substring(header.Key.LastIndexOf(_seperator) + 1);
                }
                else
                {
                    cell.Text = header.Key.ToString();
                    if (!cell.Text.Contains("OverHead"))
                    {
                        cell.ColumnSpan = 2;
                    }
                }
                gridviewRow.Cells.Add(cell);
            }
            
            timeSheetGrid.Controls[0].Controls.AddAt(gridRow.RowIndex, gridviewRow);
        }
       
        gridRow.Visible = false;
    }

    private string GetHeaderText(string headerText, int headerLevel)
    {
        if (headerLevel == 2)
        {
            return headerText;
        }
        return headerText.Substring(0, headerText.LastIndexOf(_seperator));
    }
}