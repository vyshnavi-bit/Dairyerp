using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


  public  class DbconnectionManager
    {
        public string connectionstring = string.Empty;
        DataSet ds = new DataSet();
        SqlDataAdapter dr = new SqlDataAdapter();
        SqlConnection con;

        public DbconnectionManager()
        {
            
        }
     
      
        public SqlConnection GetConnection()
        {
            con = new SqlConnection(getconnectionstring());
            if(con.State == ConnectionState.Closed)            
            con.Open();       
            return con; 
        }
        public DataSet GetDataset(string cmdstr)
        {
            
                SqlConnection con = GetConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdstr, con);
                da.Fill(ds);
                return ds;            
        }

        public DataTable GetDatatable(string cmdstr)
        {
           
                SqlConnection con = GetConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmdstr, con);
                da.Fill(dt);
                return dt;
            
        }

        public SqlDataReader GetDatareader(string cmdstr)
        {
            
                SqlConnection con = GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                cmd.Connection = con;
                cmd.CommandText = cmdstr;
                dr = cmd.ExecuteReader();
                return dr;
            
        }
        public void ExecuteNonquorey(string cmdstr)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = cmdstr;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
        }
        public int ExecuteScalarint(string cmdstr)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = cmdstr;
                cmd.Connection = con;
                int max = Convert.ToInt32(cmd.ExecuteScalar());
                return max;
            }
        }
       public string ExecuteScalarstr(string cmdstr)
       {
           using (SqlConnection con = GetConnection())
           {
               string retstr;
               retstr = string.Empty;
               SqlCommand cmd = new SqlCommand();
               cmd.CommandText = cmdstr;
               cmd.Connection = con;
               object value = cmd.ExecuteScalar();
               if(value != null)
                   retstr = value.ToString();
               return retstr;
           }
       }
        private string getconnectionstring()
        {
            //string str = "Data Source=223.196.32.28;Initial Catalog=procurement;user id=sa;password=Vyshnavi123";
           // string str = "@ SERVER=49.50.65.160;DATABASE=HRMS;UID=sa;PASSWORD=WHHWYE%23@^#%;";
           // string str = "Data Source=49.50.65.160;Initial Catalog=HRMS;user id=sa;password=WHHWYE%23@^#%;";
            //string str = "Data Source=223.196.32.28;Initial Catalog=procurement;user id=sa;password=Vyshnavi123";
            string str = "Data Source=182.18.162.51;Initial Catalog=HRMS;user id=sa;password=Vyshnavi@123;";
            
            return str;
        }
        public void closeconnection()
        {
            con.Close();
        }


        //procurement
        public SqlConnection GetConnection1()
        {
            con = new SqlConnection(getconnectionstring1());
            if (con.State == ConnectionState.Closed)
                con.Open();
            return con;
        }
        private string getconnectionstring1()
        {//192.168.0.55
            string str = "Data Source=223.196.32.30;Initial Catalog=AMPS;user id=sa;password=sap@123;";           
            return str;
        }

      //PUrChaceorder
        public SqlConnection GetConnection2()
        {
            con = new SqlConnection(getconnectionstring2());
            if (con.State == ConnectionState.Closed)
                con.Open();
            return con;
        }
        private string getconnectionstring2()
        {//192.168.0.55
           // string str = "Data Source=49.50.65.160;Initial Catalog=purchaseandstores;user id=sa;password=WHHWYE%23@^#%;";

            string str = "Data Source=182.18.162.51;Initial Catalog=HRMS;user id=sa;password=Vyshnavi@123;";
            return str;
        }
    }

