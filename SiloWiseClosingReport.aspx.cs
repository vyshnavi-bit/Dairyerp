﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class SiloWiseClosingReport : System.Web.UI.Page
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
                 if (datestrig[0].Split('-').Length > 0)
                 {
                     string[] dates = datestrig[0].Split('-');
                     string[] times = datestrig[1].Split(':');
                     todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                 }
             }
             lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
             Report.Columns.Add("Sno");
             Report.Columns.Add("SILO NAME");
             Report.Columns.Add("Batch Name");
             Report.Columns.Add("QTY").DataType = typeof(Double);
             Report.Columns.Add("FAT");
             Report.Columns.Add("SNF");
             Report.Columns.Add("CLR");
             cmd = new SqlCommand("SELECT  silomaster.SiloName, silowiseclosingdetails.sno, silowiseclosingdetails.enteredby, silowiseclosingdetails.qty_kgs, silowiseclosingdetails.fat, silowiseclosingdetails.snf, silowiseclosingdetails.clr,  silowiseclosingdetails.closingdate, batchmaster.batch FROM     silowiseclosingdetails INNER JOIN silomaster ON silowiseclosingdetails.siloid = silomaster.SiloId LEFT OUTER JOIN batchmaster ON silowiseclosingdetails.batchid = batchmaster.batchid WHERE  (silowiseclosingdetails.closingdate BETWEEN @d1 AND @d2) AND (silowiseclosingdetails.branchid = @branchid)");
             cmd.Parameters.Add("@d1", GetLowDate(fromdate));
             cmd.Parameters.Add("@d2", GetHighDate(todate));
             cmd.Parameters.Add("@branchid", BranchID);
             DataTable dtsilos = SalesDB.SelectQuery(cmd).Tables[0];
             if (dtsilos.Rows.Count > 0)
             {
                 int i = 1;
                 foreach (DataRow dr in dtsilos.Rows)
                 {
                     DataRow newrow = Report.NewRow();
                     newrow["Sno"] = i++.ToString();
                     //DateTime dtdoe = Convert.ToDateTime(dr["doe"].ToString());
                     //string date = dtdoe.ToString("dd/MM/yyyy");
                     newrow["Batch Name"] = dr["batch"].ToString();
                     newrow["SILO NAME"] = dr["SiloName"].ToString();
                     newrow["QTY"] = dr["qty_kgs"].ToString();
                     newrow["FAT"] = dr["fat"].ToString();
                     newrow["SNF"] = dr["snf"].ToString();
                     newrow["CLR"] = dr["clr"].ToString();
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
             }
         }
         catch(Exception ex)
         {
             lblmsg.Text = ex.Message;
             hidepanel.Visible = false;
         }
     }
}