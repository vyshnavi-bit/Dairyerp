using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class LogOut : System.Web.UI.Page
{
    SalesDBManager vdm = new SalesDBManager();
    SqlCommand cmd;
    string ipaddress;
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
        //cmd = new SqlCommand("Select max(sno) as transno from logininfo where UserId=@userid");
        //cmd.Parameters.Add("@userid", Session["Employ_Sno"]);
        //cmd.Parameters.Add("@UserName", Session["UserName"]);
        if (Session["Employ_Sno"] != "" || Session["Employ_Sno"] != null || Session["Employ_Sno"] != "undefined")
        {
            string sno = Session["Employ_Sno"].ToString();
            cmd = new SqlCommand("update employee_erp set loginstatus=@log where sno=@sno");
            cmd.Parameters.Add("@log", "0");
            cmd.Parameters.Add("@sno", sno);
            vdm.Update(cmd);

            cmd = new SqlCommand("Select max(sno) as transno from logininfo where UserId=@userid");
            cmd.Parameters.Add("@userid", sno);
            DataTable dttime = vdm.SelectQuery(cmd).Tables[0];
            if (dttime.Rows.Count > 0)
            {
                string transno = dttime.Rows[0]["transno"].ToString();
                cmd = new SqlCommand("UPDATE logininfo set logouttime=@logouttime,status=@status where sno=@sno");
                cmd.Parameters.Add("@logouttime", ServerDateCurrentdate);
                cmd.Parameters.Add("@status", "0");
                cmd.Parameters.Add("@sno", transno);
                vdm.Update(cmd);
            }
        }
        ExpireAllCookies();
        Session.Clear();
        Session.RemoveAll();
        Session.Abandon();
        //window.localStorage.clear();
        //ClearCache();
        //clearchachelocalall();
        Response.Redirect("login.aspx");
    }
    private void ExpireAllCookies()
    {
        if (HttpContext.Current != null)
        {
            int cookieCount = HttpContext.Current.Request.Cookies.Count;
            for (var i = 0; i < cookieCount; i++)
            {
                var cookie = HttpContext.Current.Request.Cookies[i];
                if (cookie != null)
                {
                    var cookieName = cookie.Name;
                    var expiredCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                    HttpContext.Current.Response.Cookies.Add(expiredCookie); // overwrite it
                }
            }
            // clear cookies server side
            HttpContext.Current.Request.Cookies.Clear();
        }
    }
}