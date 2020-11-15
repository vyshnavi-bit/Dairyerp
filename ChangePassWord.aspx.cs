using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
public partial class ChangePassWord : System.Web.UI.Page
{
    SqlCommand cmd;
   static string UserName = "";
   static SalesDBManager vdm;
   protected void Page_Load(object sender, EventArgs e)
   {
   }
    protected void btnSubmitt_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["Employ_Sno"] != null)
            {
                lblError.Text = "";
                UserName = Session["Employ_Sno"].ToString();
                vdm = new SalesDBManager();
                cmd = new SqlCommand("SELECT Passward FROM employee_erp WHERE Sno = @Sno");
                cmd.Parameters.Add("@Sno", UserName);
                DataTable dt = vdm.SelectQuery(cmd).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (txtNewPassWord.Text == txtConformPassWord.Text)
                    {
                        txtNewPassWord.Text = txtConformPassWord.Text;
                        cmd = new SqlCommand("Update employee_erp set Passward=@Password where Sno=@Sno ");
                        cmd.Parameters.Add("@Sno", UserName);
                        cmd.Parameters.Add("@Password", txtNewPassWord.Text.Trim());
                        vdm.Update(cmd);
                        lblMessage.Text = "Your Password has been Changed successfully";
                        Response.Redirect("Login.aspx", false);
                    }
                    else
                    {
                        lblError.Text = "Conform password not match";
                    }
                }
                else
                {
                    lblError.Text = "Entered username is incorrect";
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Password Changed Failed";
        }
    }
}