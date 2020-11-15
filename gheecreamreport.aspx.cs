using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class gheecreamreport : System.Web.UI.Page
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
    DataTable gReport = new DataTable();
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        if (BranchID == "26" || BranchID == "115")
        {
            ghee_cream_details_wyra();
        }
        else
        {
            ghee_cream_details();
        }
    }
    private void ghee_cream_details()
    {
        try
        {
            lblmsg.Text = "";
            grdReports.DataSource = null;
            grdReports.DataBind();
            GridView1.DataSource = null;
            GridView1.DataBind();
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
            Report.Columns.Add("Sno");
            Report.Columns.Add("Cream Type");
            Report.Columns.Add("O/B");
            Report.Columns.Add("O/B FAT(%)");
            Report.Columns.Add("ReceivedQty");
            Report.Columns.Add("ReceivedFat(%)");
            Report.Columns.Add("Total Cream Qty");
            Report.Columns.Add("AVGFat");
            Report.Columns.Add("Date");
            double finalcreamqty = 0;
            double totalrecivedqty = 0;
            if (ddlcreamType.SelectedValue == "All")
            {
                cmd = new SqlCommand("SELECT  sno, creamtype, ob, obfat, recivedqty, recivedfat, avgfat, totalcreamqty, createdon, receivedfrom, receivedtype FROM plant_production_details WHERE (createdon BETWEEN @d1 AND @d2) AND (branchid = @BranchID) AND (creamtype = 'Cow') AND (deptid = '3') OR (createdon BETWEEN @d1 AND @d2) AND (branchid = @BranchID) AND (deptid='3') AND (creamtype = 'Buffalo') ");

            }
            if (ddlcreamType.SelectedValue == "Cow")
            {
                cmd = new SqlCommand("SELECT sno, creamtype, ob, obfat, recivedqty, recivedfat, avgfat,totalcreamqty, createdon, receivedfrom, receivedtype FROM plant_production_details WHERE (createdon BETWEEN @d1 AND @d2) AND (branchid = @BranchID) AND (creamtype='Cow') AND (deptid='3')");

            }
            if (ddlcreamType.SelectedValue == "Buffalo")
            {
                cmd = new SqlCommand("SELECT sno, creamtype, ob, obfat, recivedqty, recivedfat, avgfat, totalcreamqty, createdon, receivedfrom, receivedtype FROM plant_production_details WHERE (createdon BETWEEN @d1 AND @d2) AND (branchid = @BranchID) AND (creamtype='Buffalo') AND (deptid='3')");
            }
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtghee = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtghee.Rows.Count > 0)
            {
                string creamtype = dtghee.Rows[0]["creamtype"].ToString();
                if (creamtype != "")
                {
                    int i = 1;
                    foreach (DataRow dr in dtghee.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["createdon"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Date"] = date;
                        newrow["Cream Type"] = dr["creamtype"].ToString();
                        newrow["O/B"] = dr["ob"].ToString();
                        newrow["O/B FAT(%)"] = dr["obfat"].ToString();
                        newrow["ReceivedQty"] = dr["recivedqty"].ToString();
                        string recivedqty = dr["recivedqty"].ToString();
                        double recivedqtys = Convert.ToDouble(recivedqty);
                        totalrecivedqty += recivedqtys;
                        newrow["ReceivedFat(%)"] = dr["recivedfat"].ToString();
                        string angf = dr["avgfat"].ToString();
                        if (angf == "")
                        {
                            angf = "0";
                        }
                        double svgfat = Convert.ToDouble(angf);
                        svgfat = Math.Round(svgfat, 2);
                        newrow["AVGFat"] = svgfat;
                        string totq = dr["totalcreamqty"].ToString();
                        if (totq == "")
                        {
                            totq = "0";
                        }
                        double totalcreamqty = Convert.ToDouble(totq);
                        totalcreamqty = Math.Round(totalcreamqty, 2);
                        newrow["Total Cream Qty"] = totalcreamqty;
                        finalcreamqty += totalcreamqty;
                        Report.Rows.Add(newrow);
                    }
                    DataRow newrow1 = Report.NewRow();
                    newrow1["O/B FAT(%)"] = "Total";
                    newrow1["ReceivedQty"] = totalrecivedqty;
                    Report.Rows.Add(newrow1);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                    hidepanel.Visible = true;
                }
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
    private void ghee_cream_details_wyra()
    {
        try
        {
            lblmsg.Text = "";
            grdReports.DataSource = null;
            grdReports.DataBind();
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
            Report.Columns.Add("Sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("Received From");
            Report.Columns.Add("Cream Type");
            Report.Columns.Add("O/B");
            Report.Columns.Add("O/B FAT(%)");
            Report.Columns.Add("O/B KGFAT");
            Report.Columns.Add("O/B SNF(%)");
            Report.Columns.Add("O/B KGSNF");
            Report.Columns.Add("Received Qty");
            Report.Columns.Add("Received FAT(%)");
            Report.Columns.Add("Received KGFAT");
            Report.Columns.Add("Received SNF(%)");
            Report.Columns.Add("Received KGSNF");
            Report.Columns.Add("Total Cream Qty");
            Report.Columns.Add("AVG FAT");
            double finalcreamqty = 0;
            double totalrecivedqty = 0;
            if (ddlcreamType.SelectedValue == "All")
            {
                cmd = new SqlCommand("SELECT  sno, creamtype, ob, obfat, recivedqty, recivedfat, avgfat, totalcreamqty, createdon, receivedfrom, receivedtype,recivesnf, snf FROM plant_production_details WHERE (createdon BETWEEN @d1 AND @d2) AND (branchid = @BranchID) AND (creamtype = 'Cow') AND (deptid = '3') OR (createdon BETWEEN @d1 AND @d2) AND (branchid = @BranchID) AND (deptid='3') AND (creamtype = 'Buffalo') order by createdon");

            }
            if (ddlcreamType.SelectedValue == "Cow")
            {
                cmd = new SqlCommand("SELECT sno, creamtype, ob, obfat, recivedqty, recivedfat, avgfat,totalcreamqty, createdon, receivedfrom, receivedtype,recivesnf, snf FROM plant_production_details WHERE (createdon BETWEEN @d1 AND @d2) AND (branchid = @BranchID) AND (creamtype='Cow') AND (deptid='3') order by createdon");

            }
            if (ddlcreamType.SelectedValue == "Buffalo")
            {
                cmd = new SqlCommand("SELECT sno, creamtype, ob, obfat, recivedqty, recivedfat, avgfat, totalcreamqty, createdon, receivedfrom, receivedtype,recivesnf, snf FROM plant_production_details WHERE (createdon BETWEEN @d1 AND @d2) AND (branchid = @BranchID) AND (creamtype='Buffalo') AND (deptid='3') order by createdon ");
            }
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtghee = SalesDB.SelectQuery(cmd).Tables[0];
            if (dtghee.Rows.Count > 0)
            {
                string creamtype = dtghee.Rows[0]["creamtype"].ToString();
                if (creamtype != "")
                {
                    int i = 1;
                    foreach (DataRow dr in dtghee.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["createdon"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Date"] = date;
                        newrow["Cream Type"] = dr["creamtype"].ToString();
                        newrow["O/B"] = dr["ob"].ToString();
                        newrow["O/B FAT(%)"] = dr["obfat"].ToString();
                        string obfat = dr["obfat"].ToString();
                        if (obfat == "")
                        {
                            obfat = "0";
                        }
                        double obfats = Convert.ToDouble(obfat);
                        double obqty = Convert.ToDouble(dr["ob"].ToString());
                        newrow["O/B KGFAT"] = Math.Round((obfats * obqty) / 100, 2);
                        newrow["O/B SNF(%)"] = dr["snf"].ToString();
                        string obsnf = dr["snf"].ToString();
                        if (obsnf == "")
                        {
                            obsnf = "0";
                        }
                        double obsnfs = Convert.ToDouble(obsnf);
                        newrow["O/B KGSNF"] = Math.Round((obsnfs * obqty) / 100, 2);
                        newrow["Received Qty"] = dr["recivedqty"].ToString();
                        string recivedqty = dr["recivedqty"].ToString();
                        double recivedqtys = Convert.ToDouble(recivedqty);
                        totalrecivedqty += recivedqtys;
                        newrow["Received FAT(%)"] = dr["recivedfat"].ToString();
                        string recfat = dr["recivedfat"].ToString();
                        if (recfat == "")
                        {
                            recfat = "0";
                        }
                        double recfats = Convert.ToDouble(recfat);
                        newrow["Received KGFAT"] = Math.Round((recfats * recivedqtys) / 100, 2);
                        newrow["Received SNF(%)"] = dr["recivesnf"].ToString();
                        string recsnf = dr["snf"].ToString();
                        if (recsnf == "")
                        {
                            recsnf = "0";
                        }
                        double recsnfs = Convert.ToDouble(recsnf);
                        newrow["Received KGSNF"] = Math.Round((recsnfs * recivedqtys) / 100, 2);
                        string avgf = dr["avgfat"].ToString();
                        if (avgf == "")
                        {
                            avgf = "0";
                        }
                        double svgfat = Convert.ToDouble(avgf);
                        svgfat = Math.Round(svgfat, 2);
                        newrow["AVG FAT"] = svgfat;
                        string totq = dr["totalcreamqty"].ToString();
                        if (totq == "")
                        {
                            totq = "0";
                        }
                        double totalcreamqty = Convert.ToDouble(totq);
                        totalcreamqty = Math.Round(totalcreamqty, 2);
                        newrow["Total Cream Qty"] = totalcreamqty;
                        finalcreamqty += totalcreamqty;
                        string receivedtype = dr["receivedtype"].ToString();
                        string receivedfrom = dr["receivedfrom"].ToString();
                        if (receivedtype == "From KCC")
                        {
                            newrow["Received From"] = "Gowden";
                        }
                        else if (receivedtype == "Plant")
                        {
                            cmd = new SqlCommand("SELECT  sno, branchname, address, whcode, branchtype, tinno, cstno, mitno, branchcode FROM  branch_info WHERE  sno=@sno");
                            cmd.Parameters.Add("@sno", receivedfrom);
                            DataTable routes = vdm.SelectQuery(cmd).Tables[0];
                            if (routes.Rows.Count > 0)
                            {
                                string branchname = routes.Rows[0]["branchname"].ToString();
                                newrow["Received From"] = branchname.ToString();
                            }
                        }
                        else if (receivedtype == "Chilling Center")
                        {

                            cmd = new SqlCommand("SELECT  sno, vendorcode, vendorname FROM  vendors WHERE  (sno = @sno)");
                            cmd.Parameters.Add("@sno", receivedfrom);
                            DataTable routes = vdm.SelectQuery(cmd).Tables[0];
                            if (routes.Rows.Count > 0)
                            {
                                string vendorname = routes.Rows[0]["vendorname"].ToString();
                                newrow["Received From"] = vendorname.ToString();
                            }
                        }
                        Report.Rows.Add(newrow);
                    }
                    DataRow newrow1 = Report.NewRow();
                    newrow1["Received From"] = "Total";
                    newrow1["Received Qty"] = totalrecivedqty;
                    Report.Rows.Add(newrow1);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    hidepanel.Visible = true;
                }
            }
            else
            {
                grdReports.DataSource = null;
                grdReports.DataBind();
            }
            gReport.Columns.Add("Sno");
            gReport.Columns.Add("Date");
            gReport.Columns.Add("Product Name");
            gReport.Columns.Add("O/B");
            gReport.Columns.Add("Conversion Qty");
            gReport.Columns.Add("Conversion FAT(%)");
            gReport.Columns.Add("Conversion KGFAT");
            gReport.Columns.Add("Conversion SNF(%)");
            gReport.Columns.Add("Conversion KGSNF");
            gReport.Columns.Add("Production Qty");
            gReport.Columns.Add("Loss Qty");
            gReport.Columns.Add("Received Qty");
            if (ddlcreamType.SelectedValue == "All")
            {
                cmd = new SqlCommand("SELECT  gp.sno, gp.productid, gp.snf,gp.createdon, gp.type, gp.productionqty, gp.greciveqty, gp.createdon AS doe, gp.convertionquantity, gp.convertionfat, gp.lossqty, gp.remarks, gp.ob AS openingbalance, gp.cb AS closingbalance, gp.branchid,  pm.productname FROM  plant_production_details AS gp INNER JOIN productmaster AS pm ON pm.sno = gp.productid WHERE   (gp.deptid = 3) AND (gp.branchid = @branchid) AND (gp.createdon BETWEEN @d1 AND @d2) ORDER BY gp.createdon");

            }
            if (ddlcreamType.SelectedValue == "Cow")
            {
                cmd = new SqlCommand("SELECT  gp.sno, gp.productid, gp.snf, gp.createdon,gp.type, gp.productionqty, gp.greciveqty, gp.createdon AS doe, gp.convertionquantity, gp.convertionfat, gp.lossqty, gp.remarks, gp.ob AS openingbalance, gp.cb AS closingbalance, gp.branchid,  pm.productname FROM  plant_production_details AS gp INNER JOIN productmaster AS pm ON pm.sno = gp.productid WHERE   (gp.deptid = 3) AND (gp.branchid = @branchid) AND (gp.createdon BETWEEN @d1 AND @d2) AND (gp.productid = 10) ORDER BY gp.createdon");

            }
            if (ddlcreamType.SelectedValue == "Buffalo")
            {
                cmd = new SqlCommand("SELECT  gp.sno, gp.productid, gp.snf,gp.createdon, gp.type, gp.productionqty, gp.greciveqty, gp.createdon AS doe, gp.convertionquantity, gp.convertionfat, gp.lossqty, gp.remarks, gp.ob AS openingbalance, gp.cb AS closingbalance, gp.branchid,  pm.productname FROM  plant_production_details AS gp INNER JOIN productmaster AS pm ON pm.sno = gp.productid WHERE   (gp.deptid = 3) AND (gp.branchid = @branchid) AND (gp.createdon BETWEEN @d1 AND @d2) AND (gp.productid = 165) OR (gp.deptid = 3) AND (gp.branchid = @branchid) AND (gp.createdon BETWEEN @d1 AND @d2) AND (gp.productid = 166)  ORDER BY gp.createdon");
            }
            cmd.Parameters.Add("@d1", GetLowDate(fromdate));
            cmd.Parameters.Add("@d2", GetHighDate(todate));
            cmd.Parameters.Add("@BranchID", BranchID);
            DataTable dtgheecon = SalesDB.SelectQuery(cmd).Tables[0];
            double totconqty = 0;
            double totprodqty = 0;
            double totlosqty = 0;
            double totreceqty = 0;
            if (dtgheecon.Rows.Count > 0)
            {
                string productionqty = dtgheecon.Rows[0]["productionqty"].ToString();
                if (productionqty != "" || productionqty != "0")
                {
                    int i = 1;
                    foreach (DataRow dr in dtgheecon.Rows)
                    {
                        DataRow newrow = gReport.NewRow();
                        newrow["Sno"] = i++.ToString();
                        DateTime dtdoe = Convert.ToDateTime(dr["createdon"].ToString());
                        string date = dtdoe.ToString("dd/MM/yyyy");
                        newrow["Date"] = date;
                        newrow["Product Name"] = dr["productname"].ToString();
                        newrow["O/B"] = dr["openingbalance"].ToString();
                        newrow["Conversion Qty"] = dr["convertionquantity"].ToString();
                        string convertionquantity = dr["convertionquantity"].ToString();
                        if (convertionquantity == "")
                        {
                            convertionquantity = "0";
                        }
                        double convertionquantitys = Convert.ToDouble(convertionquantity);
                        totconqty += convertionquantitys;
                        newrow["Conversion FAT(%)"] = dr["convertionfat"].ToString();
                        string convertionfat = dr["convertionfat"].ToString();
                        if (convertionfat == "")
                        {
                            convertionfat = "0";
                        }
                        double convertionfats = Convert.ToDouble(convertionfat);
                        newrow["Conversion KGFAT"] = Math.Round((convertionfats * convertionquantitys) / 100, 2);
                        newrow["Conversion SNF(%)"] = dr["convertionfat"].ToString();
                        string convertionsnf = dr["snf"].ToString();
                        if (convertionsnf == "")
                        {
                            convertionsnf = "0";
                        }
                        double convertionsnfs = Convert.ToDouble(convertionsnf);
                        newrow["Conversion KGSNF"] = Math.Round((convertionsnfs * convertionquantitys) / 100, 2);
                        newrow["Production Qty"] = dr["productionqty"].ToString();
                        totprodqty += Convert.ToDouble(dr["productionqty"].ToString());
                        newrow["Loss Qty"] = dr["lossqty"].ToString();
                        totlosqty += Convert.ToDouble(dr["lossqty"].ToString());
                        newrow["Received Qty"] = dr["greciveqty"].ToString();
                        totreceqty += Convert.ToDouble(dr["greciveqty"].ToString());
                        gReport.Rows.Add(newrow);
                    }
                    DataRow newrow1 = gReport.NewRow();
                    newrow1["Product Name"] = "Total";
                    newrow1["Conversion Qty"] = totconqty;
                    newrow1["Production Qty"] = totprodqty;
                    newrow1["Loss Qty"] = totlosqty;
                    newrow1["Received Qty"] = totreceqty;
                    gReport.Rows.Add(newrow1);
                    GridView1.DataSource = gReport;
                    GridView1.DataBind();
                    hidepanel.Visible = true;
                }
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}