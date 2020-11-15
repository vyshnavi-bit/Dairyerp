using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

public partial class importxl : System.Web.UI.Page
{
    SalesDBManager vdm;
    SqlCommand cmd;
    MySqlCommand mycmd;
    string userid = "";
    string mainbranch = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            string ConStr = "";
            //Extantion of the file upload control saving into ext because   
            //there are two types of extation .xls and .xlsx of Excel   
            string ext = Path.GetExtension(FileUploadToServer.FileName).ToLower();
            //getting the path of the file   
            string path = Server.MapPath("~/autocomplete/" + FileUploadToServer.FileName);
            //saving the file inside the MyFolder of the server  
            FileUploadToServer.SaveAs(path);
            lblmsg.Text = FileUploadToServer.FileName + "\'s Data showing into the GridView";
            //checking that extantion is .xls or .xlsx  
            if (ext.Trim() == ".xls")
            {
                //connection string for that file which extantion is .xls  
                ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (ext.Trim() == ".xlsx")
            {
                //connection string for that file which extantion is .xlsx  
                ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            //making query  
            OleDbConnection con = null;
            con = new OleDbConnection(ConStr);
            con.Close(); con.Open();
            DataTable dtquery = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //Get first sheet name
            string getExcelSheetName = dtquery.Rows[0]["Table_Name"].ToString();
            //string query = "SELECT * FROM [Total Deduction$]";
            //Providing connection  
            OleDbConnection conn = new OleDbConnection(ConStr);
            //checking that connection state is closed or not if closed the   
            //open the connection  
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //create command object  
            OleDbCommand cmd = new OleDbCommand(@"SELECT * FROM [" + getExcelSheetName + @"]", conn);
            // create a data adapter and get the data into dataadapter  
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //fill the Excel data to data set  
            da.Fill(dt);
            //set data source of the grid view  
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i][1] == DBNull.Value)
                    dt.Rows[i].Delete();
            }
            dt.AcceptChanges();
            grvExcelData.DataSource = dt;
            //binding the gridview  
            grvExcelData.DataBind();
            Session["dtImport"] = dt;
            //close the connection  
            conn.Close();
        }

        catch (Exception ex)
        {
            lblmsg.Text = ex.Message.ToString();

        }
    }
    DataTable Report = new DataTable();
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string BranchID = Session["Branch_ID"].ToString();
            DataTable dt = (DataTable)Session["dtImport"];
            FleetDBManager fdm = new FleetDBManager();
            vdm = new SalesDBManager();
            DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        string sno = dr["sno"].ToString();
                        string tripsheetno = dr["tripsheetno"].ToString();
                        string dcost = dr["DIESEL COST"].ToString();
                        mycmd = new MySqlCommand("UPDATE tripdata set DieselCost=@DieselCost where sno=@sno and tripsheetno=@tripsheetno");
                        mycmd.Parameters.Add("@DieselCost", dcost);
                        mycmd.Parameters.Add("@sno", sno);
                        mycmd.Parameters.Add("@tripsheetno", tripsheetno);
                        fdm.Update(mycmd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    //
                }
                lblmsg.Text = "uploded success";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblmsg.Text = ex.Message;
        }
    }
}