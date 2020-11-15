using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for test
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class test : System.Web.Services.WebService {

    public test () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    [WebMethod]
    public void findContact(string eid)
    {
        //353572080915599,13.1116188882156,80.1831831047111
        string constr = "SERVER=49.50.65.160;DATABASE=HRMS;UID=sa;PASSWORD=WHHWYE%23@^#%; Max Pool Size=100000;";
        using (SqlConnection con = new SqlConnection(constr))
        {
            string details = eid;
            string[] words = details.Split(',');
            string imeino = words[0].ToString();
            string latitude = words[1].ToString();
            string longitude = words[2].ToString();
            DateTime dt = DateTime.Now;
            DataTable ds = new DataTable();
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd = new SqlCommand("UPDATE onlinetable SET imeino=@imeeino, latitude=@latitude, longitude=@longitude, doe=@dt where imeino=@imeeino");
            cmd.Parameters.AddWithValue("@imeeino", imeino);
            cmd.Parameters.AddWithValue("@latitude", latitude);
            cmd.Parameters.AddWithValue("@longitude", longitude);
            cmd.Parameters.AddWithValue("@dt", dt);
            cmd.Connection = con;
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                cmd = new SqlCommand("INSERT INTO onlinetable (imeino, latitude, longitude, doe, username) VALUES (@imeeeino, @latitude, @longitude, @dt, @uname)");
                cmd.Parameters.AddWithValue("@imeeeino", imeino);
                cmd.Parameters.AddWithValue("@latitude", latitude);
                cmd.Parameters.AddWithValue("@longitude", longitude);
                cmd.Parameters.AddWithValue("@dt", dt);
                cmd.Parameters.AddWithValue("@uname", "Vyshnavi");
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            //cmd = new SqlCommand("SELECT MAX(doe) AS DATE FROM androidlogs WHERE imeino = @imeino");
            //cmd.Parameters.AddWithValue("@imeino", imeino);
            //cmd.Connection = con;
            //DateTime datetime = new DateTime();
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds);
            //if (ds.Rows.Count > 0)
            //{
            //    datetime = Convert.ToDateTime(ds.Rows[0]["DATE"].ToString());
            //}
            //if (dt > datetime)
            //{
            using (cmd = new SqlCommand("INSERT INTO androidlogs (imeino, latitude, longitude, doe, username) VALUES (@imeino, @latitude, @longitude, @dt, @uname)"))
            {
                cmd.Parameters.AddWithValue("@imeino", imeino);
                cmd.Parameters.AddWithValue("@latitude", latitude);
                cmd.Parameters.AddWithValue("@longitude", longitude);
                cmd.Parameters.AddWithValue("@dt", dt);
                cmd.Parameters.AddWithValue("@uname", "Vyshnavi");
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
            }
           // }
        }
    }

    [WebMethod]
    public void reg(string eid)
    {
        string details = eid;
        string[] insertetails = details.Split(',');
        string name = insertetails[0].ToString();
        string email = insertetails[1].ToString();
        string username = insertetails[2].ToString();
        string password = insertetails[3].ToString();
        SqlConnection con = new SqlConnection("Data Source=223.196.32.28;Initial Catalog=Androidtest;User Id=sa;Password=Vyshnavi123;");
        con.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO registration(name,emailid,username,password) VALUES('" + name + "','" + email + "','" + username + "','" + password + "')", con);
        cmd.ExecuteNonQuery();
        con.Close();
    }

    [WebMethod]
    public void login(string eid)
    {
        string details = eid;
        string[] insertetails = details.Split(',');
        string username = insertetails[0].ToString();
        string password = insertetails[1].ToString();
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection("Data Source=223.196.32.28;Initial Catalog=Androidtest;User Id=sa;Password=Vyshnavi123;");
        con.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM registration WHERE username=@UNAME AND password=@PWD", con);
        cmd.Parameters.Add("@UNAME", username);
        cmd.Parameters.Add("@PWD", password);
        cmd.Connection = con;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
           
        }
        con.Close();
    }
}
