using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.Cookies["userInfo"] != null)
        //{
        //    Session["Employ_Sno"] = Request.Cookies["userInfo"]["Employ_Sno"];
        //    Session["UserName"] = Request.Cookies["userInfo"]["UserName"];
        //    Session["Branch_ID"] = Request.Cookies["userInfo"]["Branch_ID"];
        //    Session["TitleName"] = Request.Cookies["userInfo"]["TitleName"];
        //    Session["TinNo"] = Request.Cookies["userInfo"]["TinNo"];
        //    Session["DeptID"] = Request.Cookies["userInfo"]["DeptID"];
        //    Session["Emp_Type"] = Request.Cookies["userInfo"]["Emp_Type"];
        //    Session["Address"] = Request.Cookies["userInfo"]["Address"];
        //    Session["BranchType"] = Request.Cookies["userInfo"]["BranchType"];
        //    Session["leveltype"] = Request.Cookies["userInfo"]["leveltype"];
        //    Session["VendorID"] = Request.Cookies["userInfo"]["VendorID"];
        //    Session["loginflag"] = Request.Cookies["userInfo"]["loginflag"];
        //    Session["lgininfosno"] = Request.Cookies["userInfo"]["lgininfosno"];
        //    Session["lgininfostatus"] = Request.Cookies["userInfo"]["lgininfostatus"];
        //}
        //else
        //{
        //    Response.Redirect("Login.aspx");
        //}
        if (Session["Employ_Sno"] != null)
        {

        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
}