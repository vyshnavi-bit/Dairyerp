using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.Services;
using System.Text;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Web.Security;

public partial class Operations : System.Web.UI.MasterPage
{
    string UserName = "";
    SalesDBManager vdm = new SalesDBManager();
    SqlCommand cmd;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Employ_Sno"] == "" || Session["Employ_Sno"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            UserName = Session["UserName"].ToString();
            if (!this.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    lblMessage.Text = UserName;
                    lblmyname.Text = UserName;
                    lblName.Text = UserName;
                    lblRole.Text = Session["leveltype"].ToString();
                }
            }
        }
    }
}
