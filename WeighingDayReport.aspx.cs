using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class WeighingDayReport : System.Web.UI.Page
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
                    tdvehicleno.Visible = false;
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
        //bindgrid();
        get_details();
    }

    private void bindvehicleno()
    {
        string vehicletype = ddlslct_vtype.SelectedItem.Text;
        SalesDBManager SalesDB = new SalesDBManager();
        try
        {
            cmd = new SqlCommand("SELECT distinct(vehicleno) FROM weighbridge where vehicletype = @vehicletype");
            cmd.Parameters.AddWithValue("@vehicletype", vehicletype);
            DataTable dttrips = vdm.SelectQuery(cmd).Tables[0];
            ddlvehicleno.DataSource = dttrips;
            ddlvehicleno.DataTextField = "vehicleno";
            //   ddl_vehicleno.DataValueField = "sno";
            ddlvehicleno.DataBind();
            ddlvehicleno.ClearSelection();
            ddlvehicleno.Items.Insert(0, new ListItem { Value = "0", Text = "All", Selected = true });
            ddlvehicleno.SelectedValue = "0";

            // dataGridView1.DataBindings();

        }
        catch (Exception ex)
        {

        }
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

    protected void ddlslct_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlslct_vtype.SelectedItem.Text == "All")
        {
            tdvehicleno.Visible = false;
        }
        else
        {
            tdvehicleno.Visible = true;
            bindvehicleno();
        }
    }

    //bindgrid();
    //      grdReports.PageIndex = e.NewPageIndex;
    //      grdReports.DataBind();

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
            Report.Columns.Add("TicketNo");
            Report.Columns.Add("Truck No");
            Report.Columns.Add("Vehicle Type");
            Report.Columns.Add("Name");
            Report.Columns.Add("Gross Weight");
            Report.Columns.Add("Tare Weight");
            Report.Columns.Add("Net Weight");
            Report.Columns.Add("Amount").DataType = typeof(Double);
            Report.Columns.Add("Weight Date");
            Report.Columns.Add("Route Name");
            Report.Columns.Add("Entryby");
            int vtype = Convert.ToInt32(ddlslct_vtype.SelectedItem.Value);
            //cmd = new SqlCommand("SELECT milktransactions.dcno AS DCNo,  milktransactions.inwardno as InwardNo, milktransactions.vehicleno ,processingdept.deptname AS DeptName, milktransactions.transtype as Type, milktransactions.qty_ltr AS QtyLtr, milktransactions.qty_kgs AS QtyKgs,  milktransactions.percentageon AS PercentageOn, milktransactions.snf AS SNF, milktransactions.fat AS FAT, milktransactions.clr AS CLR, milktransactions.cob AS COB, milktransactions.hs AS HS, milktransactions.phosps as Phosps, milktransactions.alcohol as Alcohol, milktransactions.temp AS Temp, processingdept_1.deptname AS SectionName, milktransactions.remarks, milktransactions.qco, milktransactions.chemist FROM milktransactions INNER JOIN processingdept ON milktransactions.transid = processingdept.sno INNER JOIN processingdept processingdept_1 ON milktransactions.sectionid = processingdept_1.sno WHERE (milktransactions.doe BETWEEN @d1 AND @d2) AND (milktransactions.transtype = @transtype) ");
            if (vtype == 1 || vtype == 0)
            {
                cmd = new SqlCommand("SELECT weighbridge.sno, weighbridge.vehicletype, weighbridgedetails.routedetails, weighbridge.entryby, weighbridge.vehicleno, weighbridge.transportername, weighbridgedetails.weighttype, weighbridgedetails.weight, weighbridgedetails.entrydate, weighbridgedetails.amount, weighbridge.doe FROM weighbridge INNER JOIN weighbridgedetails ON weighbridge.sno = weighbridgedetails.ticketno WHERE (weighbridge.doe BETWEEN @d1 AND @d2) AND (weighbridge.branchid=@branchid) ORDER BY weighbridge.sno desc");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
                DataTable dtweight = SalesDB.SelectQuery(cmd).Tables[0];
                if (dtweight.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dtweight.Rows)
                    {
                        int grossweight = 0;
                        int tareweight = 0;
                        int netweight = 0;
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        newrow["TicketNo"] = dr["sno"].ToString();
                        string date = dr["entrydate"].ToString();
                        newrow["Truck No"] = dr["vehicleno"].ToString();
                        newrow["Vehicle Type"] = dr["vehicletype"].ToString();
                        newrow["Name"] = dr["transportername"].ToString();
                        newrow["Weight Date"] = date;


                        // newrow["Weight Type"] = dr["weighttype"].ToString();

                        int ticketno = Convert.ToInt32(dr["sno"].ToString());
                        cmd = new SqlCommand("SELECT weighbridge.sno, weighbridge.vehicletype, weighbridgedetails.routedetails, weighbridge.entryby, weighbridge.vehicleno, weighbridge.transportername, weighbridgedetails.weighttype, weighbridgedetails.weight,weighbridgedetails.amount, weighbridge.doe FROM weighbridge INNER JOIN weighbridgedetails ON weighbridge.sno = weighbridgedetails.ticketno WHERE weighbridge.sno = @sno");
                        cmd.Parameters.Add("@sno", ticketno);
                        DataTable weighttype = SalesDB.SelectQuery(cmd).Tables[0];
                        if (weighttype.Rows.Count > 0)
                        {
                            int myno = Convert.ToInt32(dr["sno"].ToString());
                            foreach (DataRow drr in weighttype.Rows)
                            {
                                string wtype = drr["weighttype"].ToString();
                                if (wtype == "grossweight")
                                {
                                    grossweight = Convert.ToInt32(drr["weight"].ToString());
                                    newrow["Gross Weight"] = drr["weight"].ToString();
                                }
                                else
                                {
                                    tareweight = Convert.ToInt32(drr["weight"].ToString());
                                    newrow["Tare Weight"] = drr["weight"].ToString();
                                }

                                if (myno == Convert.ToInt32(drr["sno"].ToString()) && grossweight == 0 && tareweight != 0)
                                {
                                    netweight = tareweight;
                                    newrow["Net Weight"] = netweight;
                                }
                                if (myno == Convert.ToInt32(drr["sno"].ToString()) && grossweight != 0 && tareweight == 0)
                                {
                                    netweight = grossweight;
                                    newrow["Net Weight"] = netweight;
                                }
                                if (myno == Convert.ToInt32(drr["sno"].ToString()) && grossweight != 0 && tareweight != 0)
                                {
                                    netweight = 0;
                                    netweight = grossweight - tareweight;
                                    newrow["Net Weight"] = netweight;
                                }

                            }

                        }

                        newrow["Amount"] = dr["amount"].ToString();
                        newrow["Entryby"] = dr["entryby"].ToString();
                        newrow["Route Name"] = dr["routedetails"].ToString();
                        Report.Rows.Add(newrow);

                    }
                    DataRow newvartical = Report.NewRow();
                    newvartical["Net Weight"] = "Total";
                    double val = 0.0;
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            val = 0.0;
                            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                            newvartical[dc.ToString()] = val;
                        }
                    }
                    Report.Rows.Add(newvartical);
                    grdReports.DataSource = Report;
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    hidepanel.Visible = true;
                }
                else
                {
                    lblmsg.Text = "No data were found";
                    hidepanel.Visible = false;
                }

            }
            else
            {
                string vehicletype = ddlslct_vtype.SelectedItem.Text;
                //  int vno = Convert.ToInt32(ddlvehicleno.SelectedItem.Value);
                string vehicleno = ddlvehicleno.SelectedItem.Text;
                if (vehicleno == "All")
                {
                    cmd = new SqlCommand("SELECT weighbridge.sno, weighbridge.vehicletype,weighbridgedetails.entrydate, weighbridgedetails.routedetails, weighbridge.entryby, weighbridge.vehicleno, weighbridge.transportername, weighbridgedetails.weighttype, weighbridgedetails.weight,weighbridgedetails.amount, weighbridge.doe FROM weighbridge INNER JOIN weighbridgedetails ON weighbridge.sno = weighbridgedetails.ticketno WHERE (weighbridge.doe BETWEEN @d1 AND @d2) AND (weighbridge.vehicletype = @vehicletype) AND (weighbridge.branchid = @vbranchid) ORDER BY weighbridge.sno desc");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@vehicletype", vehicletype);
                    cmd.Parameters.Add("@vbranchid", BranchID);
                    DataTable dtweight = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtweight.Rows.Count > 0)
                    {
                        int i = 1;
                        foreach (DataRow dr in dtweight.Rows)
                        {
                            int grossweight = 0;
                            int tareweight = 0;
                            int netweight = 0;
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            newrow["TicketNo"] = dr["sno"].ToString();
                            string date = dr["entrydate"].ToString();
                            newrow["Truck No"] = dr["vehicleno"].ToString();
                            newrow["Vehicle Type"] = dr["vehicletype"].ToString();
                            newrow["Name"] = dr["transportername"].ToString();
                            newrow["Weight Date"] = date;
                            //  newrow["Weight Type"] = dr["weighttype"].ToString();
                            int ticketno = Convert.ToInt32(dr["sno"].ToString());
                            cmd = new SqlCommand("SELECT weighbridge.sno, weighbridge.vehicletype, weighbridgedetails.routedetails, weighbridge.entryby, weighbridge.vehicleno, weighbridge.transportername, weighbridgedetails.weighttype, weighbridgedetails.weight,weighbridgedetails.amount, weighbridge.doe FROM weighbridge INNER JOIN weighbridgedetails ON weighbridge.sno = weighbridgedetails.ticketno WHERE weighbridge.sno = @sno and weighbridge.branchid=@wbranchid");
                            cmd.Parameters.Add("@sno", ticketno);
                            cmd.Parameters.Add("@wbranchid", BranchID);
                            DataTable weighttype = SalesDB.SelectQuery(cmd).Tables[0];
                            if (weighttype.Rows.Count > 0)
                            {
                                int myno = Convert.ToInt32(dr["sno"].ToString());
                                foreach (DataRow drr in weighttype.Rows)
                                {
                                    string wtype = drr["weighttype"].ToString();
                                    if (wtype == "grossweight")
                                    {
                                        grossweight = Convert.ToInt32(drr["weight"].ToString());
                                        newrow["Gross Weight"] = drr["weight"].ToString();
                                    }
                                    else
                                    {
                                        tareweight = Convert.ToInt32(drr["weight"].ToString());
                                        newrow["Tare Weight"] = drr["weight"].ToString();
                                    }

                                    if (myno == Convert.ToInt32(drr["sno"].ToString()) && grossweight == 0 && tareweight != 0)
                                    {
                                        netweight = tareweight;
                                        newrow["Net Weight"] = netweight;
                                    }
                                    if (myno == Convert.ToInt32(drr["sno"].ToString()) && grossweight != 0 && tareweight == 0)
                                    {
                                        netweight = grossweight;
                                        newrow["Net Weight"] = netweight;
                                    }
                                    if (myno == Convert.ToInt32(drr["sno"].ToString()) && grossweight != 0 && tareweight != 0)
                                    {
                                        netweight = 0;
                                        netweight = grossweight - tareweight;
                                        newrow["Net Weight"] = netweight;
                                    }

                                }

                            }

                            newrow["Amount"] = dr["amount"].ToString();
                            newrow["Entryby"] = dr["entryby"].ToString();
                            newrow["Route Name"] = dr["routedetails"].ToString();
                            Report.Rows.Add(newrow);

                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Net Weight"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
                            }
                        }
                        Report.Rows.Add(newvartical);
                        grdReports.DataSource = Report;
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        hidepanel.Visible = true;
                    }
                    else
                    {
                        lblmsg.Text = "No data were found";
                        hidepanel.Visible = false;
                    }
                }
                else
                {
                    cmd = new SqlCommand("SELECT weighbridge.sno, weighbridge.vehicletype, weighbridgedetails.entrydate, weighbridgedetails.routedetails, weighbridge.entryby, weighbridge.vehicleno, weighbridge.transportername, weighbridgedetails.weighttype, weighbridgedetails.weight,weighbridgedetails.amount, weighbridge.doe FROM weighbridge INNER JOIN weighbridgedetails ON weighbridge.sno = weighbridgedetails.ticketno WHERE (weighbridge.doe BETWEEN @d1 AND @d2) AND (weighbridge.vehicletype = @vehicletype) AND (weighbridge.vehicleno = @vno) AND (weighbridge.branchid=@webranchid) ORDER BY weighbridge.sno desc");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@vehicletype", vehicletype);
                    cmd.Parameters.Add("@vno", vehicleno);
                    cmd.Parameters.Add("@webranchid", BranchID);
                    DataTable dtweight = SalesDB.SelectQuery(cmd).Tables[0];
                    if (dtweight.Rows.Count > 0)
                    {
                        int i = 1;
                        foreach (DataRow dr in dtweight.Rows)
                        {
                            int grossweight = 0;
                            int tareweight = 0;
                            int netweight = 0;
                            DataRow newrow = Report.NewRow();
                            newrow["Sno"] = i++.ToString();
                            newrow["TicketNo"] = dr["sno"].ToString();
                            string date = dr["entrydate"].ToString();
                            newrow["Truck No"] = dr["vehicleno"].ToString();
                            newrow["Vehicle Type"] = dr["vehicletype"].ToString();
                            newrow["Name"] = dr["transportername"].ToString();
                            newrow["Weight Date"] = date;
                            //  newrow["Weight Type"] = dr["weighttype"].ToString();
                            int ticketno = Convert.ToInt32(dr["sno"].ToString());
                            cmd = new SqlCommand("SELECT weighbridge.sno, weighbridge.vehicletype, weighbridgedetails.routedetails, weighbridge.entryby, weighbridge.vehicleno, weighbridge.transportername, weighbridgedetails.weighttype, weighbridgedetails.weight,weighbridgedetails.amount, weighbridge.doe FROM weighbridge INNER JOIN weighbridgedetails ON weighbridge.sno = weighbridgedetails.ticketno WHERE weighbridge.sno = @sno");
                            cmd.Parameters.Add("@sno", ticketno);
                            DataTable weighttype = SalesDB.SelectQuery(cmd).Tables[0];
                            if (weighttype.Rows.Count > 0)
                            {
                                int myno = Convert.ToInt32(dr["sno"].ToString());
                                foreach (DataRow drr in weighttype.Rows)
                                {
                                    string wtype = drr["weighttype"].ToString();
                                    if (wtype == "grossweight")
                                    {
                                        grossweight = Convert.ToInt32(drr["weight"].ToString());
                                        newrow["Gross Weight"] = drr["weight"].ToString();
                                    }
                                    else
                                    {
                                        tareweight = Convert.ToInt32(drr["weight"].ToString());
                                        newrow["Tare Weight"] = drr["weight"].ToString();
                                    }

                                    if (myno == Convert.ToInt32(drr["sno"].ToString()) && grossweight == 0 && tareweight != 0)
                                    {
                                        netweight = tareweight;
                                        newrow["Net Weight"] = netweight;
                                    }
                                    if (myno == Convert.ToInt32(drr["sno"].ToString()) && grossweight != 0 && tareweight == 0)
                                    {
                                        netweight = grossweight;
                                        newrow["Net Weight"] = netweight;
                                    }
                                    if (myno == Convert.ToInt32(drr["sno"].ToString()) && grossweight != 0 && tareweight != 0)
                                    {
                                        netweight = 0;
                                        netweight = grossweight - tareweight;
                                        newrow["Net Weight"] = netweight;
                                    }
                                }

                            }
                            //string wtype = dr["weighttype"].ToString();
                            //if (wtype == "grossweight")
                            //{
                            //    grossweight = Convert.ToInt32(dr["weight"].ToString());
                            //    newrow["Gross Weight"] = dr["weight"].ToString();
                            //}
                            //else
                            //{
                            //    tareweight = Convert.ToInt32(dr["weight"].ToString());
                            //    newrow["Tare Weight"] = dr["weight"].ToString();
                            //}
                            //if (grossweight != 0 && tareweight != 0)
                            //{
                            //    netweight = grossweight - tareweight;
                            //    newrow["Net Weight"] = netweight;
                            //}
                            //if (grossweight == 0 && tareweight != 0)
                            //{
                            //    netweight =  tareweight;
                            //    newrow["Net Weight"] = netweight;
                            //}
                            //if (grossweight != 0 && tareweight == 0)
                            //{
                            //    netweight = grossweight;
                            //    newrow["Net Weight"] = netweight;
                            //}
                            newrow["Amount"] = dr["amount"].ToString();
                            newrow["Entryby"] = dr["entryby"].ToString();
                            newrow["Route Name"] = dr["routedetails"].ToString();
                            Report.Rows.Add(newrow);

                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Net Weight"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
                            }
                        }
                        Report.Rows.Add(newvartical);
                        grdReports.DataSource = Report;
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        hidepanel.Visible = true;
                    }
                    else
                    {
                        lblmsg.Text = "No data were found";
                        hidepanel.Visible = false;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            hidepanel.Visible = false;
        }
    }

    private void get_details()
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
            Report.Columns.Add("TicketNo");
            Report.Columns.Add("Truck No");
            Report.Columns.Add("Vehicle Type");
            Report.Columns.Add("Name");
            Report.Columns.Add("Weight Type");
            Report.Columns.Add("Weight");
            Report.Columns.Add("Weight Date");
            Report.Columns.Add("Route Name");
            int vtype = Convert.ToInt32(ddlslct_vtype.SelectedItem.Value);
            if (vtype == 1 || vtype == 0)
            {
                cmd = new SqlCommand("SELECT wb.vehicletype, wb.vehicleno, wbsd.entrydate as doe, wb.branchid, wb.transportername, wb.transactionno, wbsd.dcno, wbsd.weighttype, wbsd.weight, wbsd.routedetails, wbsd.ticketno FROM   weighbridge AS wb INNER JOIN weighbridgedetails AS wbsd ON wb.sno = wbsd.ticketno WHERE  (wb.branchid = @branchid) AND (wb.doe BETWEEN @d1 AND @d2) ORDER BY wbsd.ticketno,wbsd.entrydate");
                cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                cmd.Parameters.Add("@d2", GetHighDate(todate));
                cmd.Parameters.Add("@branchid", BranchID);
            }
            else
            {
                string vehicletype = ddlslct_vtype.SelectedItem.Text;
                string vehicleno = ddlvehicleno.SelectedItem.Text;
                if (vehicleno == "All")
                {
                    cmd = new SqlCommand("SELECT  wb.sno, wb.vehicletype, wb.vehicleno, wbsd.entrydate as doe, wb.branchid, wb.transportername, wb.transactionno, wbsd.dcno, wbsd.weighttype, wbsd.weight, wbsd.routedetails, wbsd.ticketno FROM   weighbridge AS wb INNER JOIN weighbridgedetails AS wbsd ON wb.sno = wbsd.ticketno WHERE  (wb.branchid = @branchid) AND (wb.doe BETWEEN @d1 AND @d2) AND (wb.vehicletype = @vehicletype) ORDER BY wbsd.ticketno,wbsd.entrydate");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@vehicletype", vehicletype);
                    cmd.Parameters.Add("@branchid", BranchID);
                }
                else
                {
                    cmd = new SqlCommand("SELECT  wb.sno, wb.vehicletype, wb.vehicleno, wbsd.entrydate as doe, wb.branchid, wb.transportername, wb.transactionno, wbsd.dcno, wbsd.weighttype, wbsd.weight, wbsd.routedetails, wbsd.ticketno FROM   weighbridge AS wb INNER JOIN weighbridgedetails AS wbsd ON wb.sno = wbsd.ticketno WHERE  (wb.branchid = @branchid) AND (wb.doe BETWEEN @d1 AND @d2) AND (wb.vehicletype = @vehicletype) AND (wb.vehicleno = @vehicleno) ORDER BY wbsd.ticketno,wbsd.entrydate");
                    cmd.Parameters.Add("@d1", GetLowDate(fromdate));
                    cmd.Parameters.Add("@d2", GetHighDate(todate));
                    cmd.Parameters.Add("@branchid", BranchID);
                    cmd.Parameters.Add("@vehicletype", vehicletype);
                    cmd.Parameters.Add("@vehicleno", vehicleno);
                }
            }
            DataTable dtweight = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtweight.Rows.Count > 0)
            {
                int i = 1;
                string prvticketno = "";
                double totwt = 0;
                double grswt = 0;
                double trwt = 0;
                foreach (DataRow dr in dtweight.Rows)
                {
                    string ticketno = dr["ticketno"].ToString();
                    if (ticketno == prvticketno)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        //newrow["TicketNo"] = dr["ticketno"].ToString();
                        //newrow["Truck No"] = dr["vehicleno"].ToString();
                        //newrow["Vehicle Type"] = dr["vehicletype"].ToString();
                        //newrow["Name"] = dr["transportername"].ToString();
                        newrow["Weight Type"] = dr["weighttype"].ToString();
                        string weighttype = dr["weighttype"].ToString();
                        string weight = dr["weight"].ToString();
                        if (weighttype == "grossweight")
                        {
                            grswt += Convert.ToDouble(weight);
                        }
                        else
                        {
                            trwt += Convert.ToDouble(weight);
                        }
                        newrow["Weight"] = dr["weight"].ToString();
                        newrow["Weight Date"] = dr["doe"].ToString();
                        newrow["Route Name"] = dr["routedetails"].ToString();
                        Report.Rows.Add(newrow);
                    }
                    else
                    {
                        if (grswt > 0 || trwt > 0)
                        {
                            DataRow newrows = Report.NewRow();
                            newrows["Vehicle Type"] = "Total";
                            newrows["Weight Type"] = "Net Weight";
                            newrows["Weight"] = grswt - trwt;
                            Report.Rows.Add(newrows);
                            totwt = 0;
                            grswt = 0;
                            trwt = 0;
                        }
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        newrow["TicketNo"] = dr["ticketno"].ToString();
                        newrow["Truck No"] = dr["vehicleno"].ToString();
                        newrow["Vehicle Type"] = dr["vehicletype"].ToString();
                        newrow["Name"] = dr["transportername"].ToString();
                        newrow["Weight Type"] = dr["weighttype"].ToString();
                        newrow["Weight"] = dr["weight"].ToString();
                        newrow["Weight Date"] = dr["doe"].ToString();
                        newrow["Route Name"] = dr["routedetails"].ToString();
                        string weighttype = dr["weighttype"].ToString();
                        string weight = dr["weight"].ToString();
                        if (weighttype == "grossweight")
                        {
                            grswt += Convert.ToDouble(weight);
                        }
                        else
                        {
                            trwt += Convert.ToDouble(weight);
                        }
                        Report.Rows.Add(newrow);
                        prvticketno = dr["ticketno"].ToString();
                    }
                }
                if (grswt > 0 || trwt > 0)
                {
                    DataRow newrows = Report.NewRow();
                    newrows["Vehicle Type"] = "Total";
                    newrows["Weight Type"] = "Net Weight";
                    newrows["Weight"] = grswt - trwt;
                    Report.Rows.Add(newrows);
                    totwt = 0;
                    grswt = 0;
                    trwt = 0;
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                hidepanel.Visible = true;
            }
            else
            {
                lblmsg.Text = "No data were found";
                hidepanel.Visible = false;
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
            if (e.Row.Cells[3].Text == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Medium;
                e.Row.Font.Bold = true;
            }
        }
    }
}