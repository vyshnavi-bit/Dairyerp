using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.Services;
using System.Net.Mail;
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


public partial class Login : System.Web.UI.Page
{
    SalesDBManager vdm = new SalesDBManager();
    SqlCommand cmd;
    string ipaddress;
    AccessControldbmanger Accescontrol_db = new AccessControldbmanger();
    WebClient client = new WebClient();
    /// <summary>
    /// Note : ModuleId=1 for Sales
    ///        ModuleId=2 for Purchase and stores
    ///        ModuleId=3 for Production
    /// company codes SVDS=1
    ///                SVD=2
    ///                SVF=3
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // akbar 
        if (!IsPostBack)
        {
            try
            {
                login_click();
            }
            catch (Exception ex)
            {
                //lbl_validation.Text = ex.Message;
            }
        }
    }
    
    string username = "";
    string password = "";
    string entermsg = "";
    string sendmsg = "";
    string PhoneNo = "";
    protected void login_click(object sender, EventArgs e)
    {
        try
        {
            username = Usernme_txt.Text;
            password = Pass_pas.Text;
            lbl_username.Text = username;
            lbl_passwords.Text = password;
            DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
            //cmd = new SqlCommand("SELECT employee_erp.sno, employee_erp.leveltype,employee_erp.loginstatus, branch_info.venorid, employee_erp.empname, employee_erp.deptid, employee_erp.username, employee_erp.passward, employee_erp.emptype, employee_erp.branchid, branch_info.branchtype,branch_info.address,branch_info.branchname,branch_info.tinno FROM employee_erp INNER JOIN branch_info ON employee_erp.branchid = branch_info.sno WHERE  (employee_erp.username = @UN) AND (employee_erp.passward = @Pwd)");
            cmd = new SqlCommand("SELECT employee_erp.sno, employee_erp.leveltype,employee_erp.loginstatus, branch_info.venorid, branch_info.branchcode, employee_erp.empname, employee_erp.deptid, employee_erp.username, employee_erp.passward, employee_erp.emptype, employee_erp.branchid, branch_info.branchtype,branch_info.address,branch_info.branchname,branch_info.tinno, employee_erp.phoneno, employee_erp.otpstatus,employee_erp.empid FROM employee_erp INNER JOIN branch_info ON employee_erp.branchid = branch_info.sno WHERE  (employee_erp.username = @UN) AND (employee_erp.passward = @Pwd)");
            cmd.Parameters.Add("@Pwd", password);
            cmd.Parameters.Add("@UN", username);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string otpstatus = dt.Rows[0]["otpstatus"].ToString();
                if (otpstatus == "1")
                {
                    //session
                    string sno = dt.Rows[0]["sno"].ToString();
                    cmd = new SqlCommand("update employee_erp set loginstatus=@log where sno=@sno");
                    cmd.Parameters.Add("@log", "1");
                    cmd.Parameters.Add("@sno", sno);
                    vdm.Update(cmd);
                    Session["TitleName"] = dt.Rows[0]["branchname"].ToString(); // "SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD";
                    Session["TinNo"] = dt.Rows[0]["tinno"].ToString();
                    Session["DeptID"] = dt.Rows[0]["deptid"].ToString();
                    Session["Employ_Sno"] = dt.Rows[0]["sno"].ToString();
                    Session["Branch_ID"] = dt.Rows[0]["branchid"].ToString();
                    Session["Emp_Type"] = dt.Rows[0]["emptype"].ToString();
                    Session["Address"] = dt.Rows[0]["address"].ToString(); //"R.S.No:381/2,Punabaka village Post,Pellakuru Mandal,Nellore District -524129., ANDRAPRADESH (State).Phone: 9440622077, Fax: 044 – 26177799. ";// dt.Rows[0]["brnch_address"].ToString();
                    Session["BranchType"] = dt.Rows[0]["branchtype"].ToString();
                    Session["leveltype"] = dt.Rows[0]["leveltype"].ToString();
                    Session["UserName"] = dt.Rows[0]["empname"].ToString();
                    Session["VendorID"] = dt.Rows[0]["venorid"].ToString();
                    Session["loginflag"] = dt.Rows[0]["loginstatus"].ToString();
                    Session["branchcode"] = dt.Rows[0]["branchcode"].ToString();
                    Session["HRMS_EmpId"] = dt.Rows[0]["empid"].ToString();
                    string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                    //get ip address and device type
                    ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (ipaddress == "" || ipaddress == null)
                    {
                        ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                    }
                    HttpBrowserCapabilities browser = Request.Browser;
                    string devicetype = "";
                    string userAgent = Request.ServerVariables["HTTP_USER_AGENT"];
                    Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    string device_info = string.Empty;
                    if (OS.IsMatch(userAgent))
                    {
                        device_info = OS.Match(userAgent).Groups[0].Value;
                    }
                    if (device.IsMatch(userAgent.Substring(0, 4)))
                    {
                        device_info += device.Match(userAgent).Groups[0].Value;
                    }
                    if (!string.IsNullOrEmpty(device_info))
                    {
                        devicetype = device_info;
                        string[] words = devicetype.Split(')');
                        devicetype = words[0].ToString();
                    }
                    else
                    {
                        devicetype = "Desktop";
                    }
                    cmd = new SqlCommand("INSERT INTO logininfo(UserId, UserName, Logintime, IpAddress,devicetype,status) values (@userid, @UserName, @logintime, @ipaddress,@devicetype,@status)");
                    cmd.Parameters.Add("@userid", dt.Rows[0]["sno"].ToString());
                    cmd.Parameters.Add("@UserName", Session["UserName"]);
                    cmd.Parameters.Add("@logintime", ServerDateCurrentdate);
                    cmd.Parameters.Add("@ipaddress", ipaddress);
                    cmd.Parameters.Add("@devicetype", devicetype);
                    cmd.Parameters.Add("@status", "1");
                    vdm.insert(cmd);
                    //End
                    //otp
                    string Id = string.Empty;
                    string no =  dt.Rows[0]["phoneno"].ToString();
                    string empid = dt.Rows[0]["sno"].ToString();
                    string numbers = "1234567890";
                    string characters = numbers;
                    int length = 6;
                    string otp = string.Empty;
                    for (int i = 0; i < length; i++)
                    {
                        string character = string.Empty;
                        do
                        {
                            int index = new Random().Next(0, characters.Length);
                            character = characters.ToCharArray()[index].ToString();
                        } while (otp.IndexOf(character) != -1);
                        otp += character;
                    }
                    DateTime sdt = SalesDBManager.GetTime(vdm.conn);
                    int h = Convert.ToInt32(sdt.ToString("HH"));
                    int m = 0;
                    string otpexptime = string.Empty;
                    string sss = string.Empty;
                    string mm = string.Empty;
                    m = Convert.ToInt32(sdt.ToString("mm")) + 3;
                    int ss = Convert.ToInt32(sdt.ToString("ss"));
                    if (ss > 60)
                    {
                        ss = ss - 60;
                    }
                    if (ss < 10)
                    {
                        sss = "0" + m.ToString();
                    }
                    if (m > 60)
                    {
                        m = m - 60;
                    }
                    if (m < 10)
                    {
                        if (ss < 10)
                        {
                            mm = "0" + m.ToString();
                            otpexptime = h.ToString() + ":" + mm.ToString() + ":" + sss.ToString();
                        }
                        else
                        {
                            mm = m.ToString();
                            otpexptime = h.ToString() + ":" + mm.ToString() + ":" + ss.ToString();
                        }
                    }
                    else
                    {
                        if (ss < 10)
                        {
                            otpexptime = h.ToString() + ":" + m.ToString() + ":" + sss.ToString();
                        }
                        else
                        {
                            otpexptime = h.ToString() + ":" + m.ToString() + ":" + ss.ToString();
                        }

                    }
                    Otpupdate(no, otp, otpexptime, empid);
                    Id = Encrypt(no.Trim());
                    string hyperlink = "otp.aspx?Id=" + Id.Trim();
                    string message1 = "OTP for  " + empid + "  Login : " + otp + ". Valid till " + otpexptime + "  Do not share OTP for security reasons.";
                    string strUrl = "http://123.63.33.43/blank/sms/user/urlsms.php?username=vyshnavidairy&pass=vyshnavi@123&senderid=VYSAKG&dest_mobileno=" + no + "&message=" + message1 + "&response=Y";
                    //string strUrl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + no + "&source=VYSHRM&message=" + message1 + "";
                    WebRequest request = HttpWebRequest.Create(strUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream s = (Stream)response.GetResponseStream();
                    StreamReader readStream = new StreamReader(s);
                    string dataString = readStream.ReadToEnd();
                    response.Close();
                    s.Close();
                    readStream.Close();
                    string msg = hyperlink;
                    Response.Redirect("otp.aspx?Id=" + Id.Trim());
                }
                else
                {
                    fill_login_details();
                }

            }
            else
            {
                lbl_validation.Text = "Check Your Username and password";
            }
        }
        catch (Exception ex)
        {
            lbl_validation.Text = ex.Message;
        }
    }
    //otp
    protected void Otpupdate(string mno, string otp, string edate, string uniqid)
    {
        try
        {
            HRMSDBManager HRMSDM = new HRMSDBManager();
            string proid = "ERP";
            string frmmid = "Login";
            string userid = Context.Session["UserName"].ToString();
            int s = 1;
            cmd = new SqlCommand("INSERT INTO VysAuthentication(val1,val2,val3,val4,val5,val6,val7,val8,val9,val10) VALUES(@val1,@val2,@val3,@val4,@val5,@val6,@val7,@val8,@val9,@val10)");
            cmd.Parameters.Add("@val1", mno);
            cmd.Parameters.Add("@val2", otp);
            cmd.Parameters.Add("@val3", edate);
            cmd.Parameters.Add("@val4", proid);
            cmd.Parameters.Add("@val5", frmmid);
            cmd.Parameters.Add("@val6", userid);
            cmd.Parameters.Add("@val7", uniqid);
            cmd.Parameters.Add("@val8", "1");
            cmd.Parameters.Add("@val9", "1");
            cmd.Parameters.Add("@val10", s);
            HRMSDM.insert(cmd);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
    //otp
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "V99Y34S44H9N0A0V6I";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    void fill_login_details()
    {
        string sendmsgs = lbl_messsge.Text.ToString();
        string username = lbl_username.Text.ToString();
        string password = lbl_passwords.Text.ToString();
        DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
        cmd = new SqlCommand("SELECT employee_erp.sno, employee_erp.leveltype,employee_erp.loginstatus, branch_info.venorid, branch_info.branchcode, employee_erp.empname, employee_erp.deptid, employee_erp.username, employee_erp.passward, employee_erp.emptype, employee_erp.branchid, branch_info.branchtype,branch_info.address,branch_info.branchname,branch_info.tinno,employee_erp.empid FROM employee_erp INNER JOIN branch_info ON employee_erp.branchid = branch_info.sno WHERE  (employee_erp.username = @UN) AND (employee_erp.passward = @Pwd)");
        cmd.Parameters.Add("@Pwd", password);
        cmd.Parameters.Add("@UN", username);
        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
        if (dt.Rows.Count > 0)
        {
            string sno = dt.Rows[0]["sno"].ToString();
            cmd = new SqlCommand("update employee_erp set loginstatus=@log where sno=@sno");
            cmd.Parameters.Add("@log", "1");
            cmd.Parameters.Add("@sno", sno);
            vdm.Update(cmd);
            Session["TitleName"] = dt.Rows[0]["branchname"].ToString(); // "SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD";
            Session["TinNo"] = dt.Rows[0]["tinno"].ToString();
            Session["DeptID"] = dt.Rows[0]["deptid"].ToString();
            Session["Employ_Sno"] = dt.Rows[0]["sno"].ToString();
            Session["Branch_ID"] = dt.Rows[0]["branchid"].ToString();
            Session["Emp_Type"] = dt.Rows[0]["emptype"].ToString();
            Session["Address"] = dt.Rows[0]["address"].ToString(); //"R.S.No:381/2,Punabaka village Post,Pellakuru Mandal,Nellore District -524129., ANDRAPRADESH (State).Phone: 9440622077, Fax: 044 – 26177799. ";// dt.Rows[0]["brnch_address"].ToString();
            Session["BranchType"] = dt.Rows[0]["branchtype"].ToString();
            Session["leveltype"] = dt.Rows[0]["leveltype"].ToString();
            Session["UserName"] = dt.Rows[0]["empname"].ToString();
            Session["VendorID"] = dt.Rows[0]["venorid"].ToString();
            Session["loginflag"] = dt.Rows[0]["loginstatus"].ToString();
            Session["branchcode"] = dt.Rows[0]["branchcode"].ToString();
            Session["HRMS_EmpId"] = dt.Rows[0]["empid"].ToString();
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            //get ip address and device type
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
            {
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            HttpBrowserCapabilities browser = Request.Browser;
            string devicetype = "";
            string userAgent = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string device_info = string.Empty;
            if (OS.IsMatch(userAgent))
            {
                device_info = OS.Match(userAgent).Groups[0].Value;
            }
            if (device.IsMatch(userAgent.Substring(0, 4)))
            {
                device_info += device.Match(userAgent).Groups[0].Value;
            }
            if (!string.IsNullOrEmpty(device_info))
            {
                devicetype = device_info;
                string[] words = devicetype.Split(')');
                devicetype = words[0].ToString();
            }
            else
            {
                devicetype = "Desktop";
            }
            //End
            cmd = new SqlCommand("INSERT INTO logininfo(UserId, UserName, Logintime, IpAddress,devicetype,status) values (@userid, @UserName, @logintime, @ipaddress,@devicetype,@status)");
            cmd.Parameters.Add("@userid", dt.Rows[0]["sno"].ToString());
            cmd.Parameters.Add("@UserName", Session["UserName"]);
            cmd.Parameters.Add("@logintime", ServerDateCurrentdate);
            cmd.Parameters.Add("@ipaddress", ipaddress);
            cmd.Parameters.Add("@devicetype", devicetype);
            cmd.Parameters.Add("@status", "1");
            vdm.insert(cmd);
            cmd = new SqlCommand("SELECT  TOP (1) sno, UserId, status FROM logininfo WHERE (UserId = @UserId)  ORDER BY sno DESC");
            cmd.Parameters.Add("@UserId", dt.Rows[0]["sno"].ToString());
            DataTable dttatus = vdm.SelectQuery(cmd).Tables[0];
            Session["lgininfosno"] = dttatus.Rows[0]["sno"].ToString();
            Session["lgininfostatus"] = dttatus.Rows[0]["status"].ToString();
            // Cookies
            Response.Cookies["userInfo"]["Employ_Sno"] = Session["Employ_Sno"].ToString();
            Response.Cookies["userInfo"]["UserName"] = Session["UserName"].ToString();
            Response.Cookies["userInfo"]["Branch_ID"] = Session["Branch_ID"].ToString();
            Response.Cookies["userInfo"]["TitleName"] = Session["TitleName"].ToString();
            Response.Cookies["userInfo"]["TinNo"] = Session["TinNo"].ToString();
            Response.Cookies["userInfo"]["DeptID"] = Session["DeptID"].ToString();
            Response.Cookies["userInfo"]["Emp_Type"] = Session["Emp_Type"].ToString();
            Response.Cookies["userInfo"]["Address"] = Session["Address"].ToString();
            Response.Cookies["userInfo"]["BranchType"] = Session["BranchType"].ToString();
            Response.Cookies["userInfo"]["leveltype"] = Session["leveltype"].ToString();
            Response.Cookies["userInfo"]["VendorID"] = Session["VendorID"].ToString();
            Response.Cookies["userInfo"]["loginflag"] = Session["loginflag"].ToString();
            Response.Cookies["userInfo"]["lgininfosno"] = Session["lgininfosno"].ToString();
            Response.Cookies["userInfo"]["lgininfostatus"] = Session["lgininfostatus"].ToString();
            Response.Cookies["userInfo"]["lastVisit"] = DateTime.Now.ToString();
            Response.Cookies["userInfo"].Expires = DateTime.Now.AddDays(1);
            HttpCookie aCookie = new HttpCookie("userInfo");
            aCookie.Values["UserName"] = Session["UserName"].ToString();
            aCookie.Values["Employ_Sno"] = Session["Employ_Sno"].ToString();
            aCookie.Values["Branch_ID"] = Session["Branch_ID"].ToString();
            aCookie.Values["TitleName"] = Session["TitleName"].ToString();
            aCookie.Values["TinNo"] = Session["TinNo"].ToString();
            aCookie.Values["DeptID"] = Session["DeptID"].ToString();
            aCookie.Values["Emp_Type"] = Session["Emp_Type"].ToString();
            aCookie.Values["Address"] = Session["Address"].ToString();
            aCookie.Values["BranchType"] = Session["BranchType"].ToString();
            aCookie.Values["leveltype"] = Session["leveltype"].ToString();
            aCookie.Values["VendorID"] = Session["VendorID"].ToString();
            aCookie.Values["loginflag"] = Session["loginflag"].ToString();
            aCookie.Values["lgininfosno"] = Session["lgininfosno"].ToString();
            aCookie.Values["lgininfostatus"] = Session["lgininfostatus"].ToString();
            aCookie.Values["lastVisit"] = DateTime.Now.ToString();
            aCookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(aCookie);
            string branchtype = dt.Rows[0]["branchtype"].ToString();
            string leveltype = dt.Rows[0]["leveltype"].ToString();
            if (branchtype == "CC")
            {
                Response.Redirect("Despatch_Entry.aspx", false);
            }
            else
            {
                if (leveltype == "lab")
                {
                    Response.Redirect("DashBoard.aspx", false);
                }
                if (leveltype == "Security")
                {
                    Response.Redirect("Inward_dc_varify.aspx", false);
                }
                if (leveltype == "Processing")
                {
                    Response.Redirect("InwardSilo.aspx", false);
                }
                if (leveltype == "Operations")
                {
                    Response.Redirect("chartdashboard.aspx", false);
                }
                if (leveltype == "packing")
                {
                    Response.Redirect("Packingsection.aspx", false);
                }
                if (leveltype == "weighing")
                {
                    Response.Redirect("WeighingDayReport.aspx", false);
                }
                if (leveltype == "Admin")
                {
                    Response.Redirect("chartdashboard.aspx", false);
                }
                if (leveltype == "MAdmin")
                {
                    Response.Redirect("chartdashboard.aspx", false);
                }
                if (leveltype == "curdblock")
                {
                    Response.Redirect("chemicalconsumption.aspx", false);
                }
                if (leveltype == "Data")
                {
                    Response.Redirect("TankersInwardReport.aspx", false);
                }
                if (leveltype == "Reports")
                {
                    Response.Redirect("TankersInwardReport.aspx", false);
                }
                if (leveltype == "dailyreport")
                {
                    Response.Redirect("plantdailyreport.aspx", false);
                }
                if (leveltype == "ghee")
                {
                    Response.Redirect("GheeSection.aspx", false);
                }
                if (leveltype == "Butter")
                {
                    Response.Redirect("butterproductiondetails.aspx", false);
                }
                if (leveltype == "suboperations")
                {
                    Response.Redirect("SiloMonitor.aspx", false);
                }
                if (leveltype == "curdandlab")
                {
                    Response.Redirect("chemicalconsumption.aspx", false);
                }
            }
        }
    }
    protected void sessionsclick_click(object sender, EventArgs e)
    {
        try
        {
            this.AlertBox.Visible = false;
            string username = lbl_username.Text.ToString();
            string password = lbl_passwords.Text.ToString();
            DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
            cmd = new SqlCommand("update employee_erp set loginstatus=@log where username=@username and passward=@passward");
            cmd.Parameters.Add("@log", "0");
            cmd.Parameters.Add("@username", username);
            cmd.Parameters.Add("@passward", password);
            vdm.Update(cmd);
            cmd = new SqlCommand("SELECT sno FROM employee_erp where username=@username and passward=@passward");
            cmd.Parameters.Add("@username", username);
            cmd.Parameters.Add("@passward", password);
            DataTable dtEMP = vdm.SelectQuery(cmd).Tables[0];
            if (dtEMP.Rows.Count > 0)
            {
                string empid = dtEMP.Rows[0]["sno"].ToString();
                cmd = new SqlCommand("Select max(sno) as transno from logininfo where UserId=@userid");
                cmd.Parameters.Add("@userid", empid);
                DataTable dttime = vdm.SelectQuery(cmd).Tables[0];
                if (dttime.Rows.Count > 0)
                {
                    string transno = dttime.Rows[0]["transno"].ToString();
                    cmd = new SqlCommand("UPDATE logininfo set logouttime=@logouttime,status=@status where sno=@sno");
                    cmd.Parameters.Add("@logouttime", ServerDateCurrentdate);
                    cmd.Parameters.Add("@status","0");
                    cmd.Parameters.Add("@sno", transno);
                    vdm.Update(cmd);
                }
            }
            this.AlertBox.Visible = false;
            fill_login_details();
        }
        catch
        {
        }
    }
    protected void sessionsclick_Close(object sender, EventArgs e)
    {
        this.AlertBox.Visible = false;
        Response.Redirect("Login.aspx");
    }

    private void login_click()
    {
        try
        {
            Usernme_txt.Text = Request.QueryString["username"];
            Pass_pas.Text = Request.QueryString["pwd"];
            username = Usernme_txt.Text;
            password = Pass_pas.Text;
            lbl_username.Text = username;
            lbl_passwords.Text = password;
            DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
            //cmd = new SqlCommand("SELECT employee_erp.sno, employee_erp.leveltype,employee_erp.loginstatus, branch_info.venorid, employee_erp.empname, employee_erp.deptid, employee_erp.username, employee_erp.passward, employee_erp.emptype, employee_erp.branchid, branch_info.branchtype,branch_info.address,branch_info.branchname,branch_info.tinno FROM employee_erp INNER JOIN branch_info ON employee_erp.branchid = branch_info.sno WHERE  (employee_erp.username = @UN) AND (employee_erp.passward = @Pwd)");
            cmd = new SqlCommand("SELECT employee_erp.sno, employee_erp.leveltype,employee_erp.loginstatus, branch_info.venorid, branch_info.branchcode, employee_erp.empname, employee_erp.deptid, employee_erp.username, employee_erp.passward, employee_erp.emptype, employee_erp.branchid, branch_info.branchtype,branch_info.address,branch_info.branchname,branch_info.tinno, employee_erp.phoneno, employee_erp.otpstatus FROM employee_erp INNER JOIN branch_info ON employee_erp.branchid = branch_info.sno WHERE  (employee_erp.username = @UN) AND (employee_erp.passward = @Pwd)");
            cmd.Parameters.Add("@Pwd", password);
            cmd.Parameters.Add("@UN", username);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string otpstatus = dt.Rows[0]["otpstatus"].ToString();
                if (otpstatus == "1")
                {
                    //session
                    string sno = dt.Rows[0]["sno"].ToString();
                    cmd = new SqlCommand("update employee_erp set loginstatus=@log where sno=@sno");
                    cmd.Parameters.Add("@log", "1");
                    cmd.Parameters.Add("@sno", sno);
                    vdm.Update(cmd);
                    Session["TitleName"] = dt.Rows[0]["branchname"].ToString(); // "SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD";
                    Session["TinNo"] = dt.Rows[0]["tinno"].ToString();
                    Session["DeptID"] = dt.Rows[0]["deptid"].ToString();
                    Session["Employ_Sno"] = dt.Rows[0]["sno"].ToString();
                    Session["Branch_ID"] = dt.Rows[0]["branchid"].ToString();
                    Session["Emp_Type"] = dt.Rows[0]["emptype"].ToString();
                    Session["Address"] = dt.Rows[0]["address"].ToString(); //"R.S.No:381/2,Punabaka village Post,Pellakuru Mandal,Nellore District -524129., ANDRAPRADESH (State).Phone: 9440622077, Fax: 044 – 26177799. ";// dt.Rows[0]["brnch_address"].ToString();
                    Session["BranchType"] = dt.Rows[0]["branchtype"].ToString();
                    Session["leveltype"] = dt.Rows[0]["leveltype"].ToString();
                    Session["UserName"] = dt.Rows[0]["empname"].ToString();
                    Session["VendorID"] = dt.Rows[0]["venorid"].ToString();
                    Session["loginflag"] = dt.Rows[0]["loginstatus"].ToString();
                    Session["branchcode"] = dt.Rows[0]["branchcode"].ToString();
                    string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                    //get ip address and device type
                    ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (ipaddress == "" || ipaddress == null)
                    {
                        ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                    }
                    HttpBrowserCapabilities browser = Request.Browser;
                    string devicetype = "";
                    string userAgent = Request.ServerVariables["HTTP_USER_AGENT"];
                    Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    string device_info = string.Empty;
                    if (OS.IsMatch(userAgent))
                    {
                        device_info = OS.Match(userAgent).Groups[0].Value;
                    }
                    if (device.IsMatch(userAgent.Substring(0, 4)))
                    {
                        device_info += device.Match(userAgent).Groups[0].Value;
                    }
                    if (!string.IsNullOrEmpty(device_info))
                    {
                        devicetype = device_info;
                        string[] words = devicetype.Split(')');
                        devicetype = words[0].ToString();
                    }
                    else
                    {
                        devicetype = "Desktop";
                    }
                    cmd = new SqlCommand("INSERT INTO logininfo(UserId, UserName, Logintime, IpAddress,devicetype,status) values (@userid, @UserName, @logintime, @ipaddress,@devicetype,@status)");
                    cmd.Parameters.Add("@userid", dt.Rows[0]["sno"].ToString());
                    cmd.Parameters.Add("@UserName", Session["UserName"]);
                    cmd.Parameters.Add("@logintime", ServerDateCurrentdate);
                    cmd.Parameters.Add("@ipaddress", ipaddress);
                    cmd.Parameters.Add("@devicetype", devicetype);
                    cmd.Parameters.Add("@status", "1");
                    vdm.insert(cmd);
                    //End
                    //otp
                    string Id = string.Empty;
                    string no = dt.Rows[0]["phoneno"].ToString();
                    string empid = dt.Rows[0]["sno"].ToString();
                    string numbers = "1234567890";
                    string characters = numbers;
                    int length = 6;
                    string otp = string.Empty;
                    for (int i = 0; i < length; i++)
                    {
                        string character = string.Empty;
                        do
                        {
                            int index = new Random().Next(0, characters.Length);
                            character = characters.ToCharArray()[index].ToString();
                        } while (otp.IndexOf(character) != -1);
                        otp += character;
                    }
                    DateTime sdt = SalesDBManager.GetTime(vdm.conn);
                    int h = Convert.ToInt32(sdt.ToString("HH"));
                    int m = 0;
                    string otpexptime = string.Empty;
                    string sss = string.Empty;
                    string mm = string.Empty;
                    m = Convert.ToInt32(sdt.ToString("mm")) + 3;
                    int ss = Convert.ToInt32(sdt.ToString("ss"));
                    if (ss > 60)
                    {
                        ss = ss - 60;
                    }
                    if (ss < 10)
                    {
                        sss = "0" + m.ToString();
                    }
                    if (m > 60)
                    {
                        m = m - 60;
                    }
                    if (m < 10)
                    {
                        if (ss < 10)
                        {
                            mm = "0" + m.ToString();
                            otpexptime = h.ToString() + ":" + mm.ToString() + ":" + sss.ToString();
                        }
                        else
                        {
                            mm = m.ToString();
                            otpexptime = h.ToString() + ":" + mm.ToString() + ":" + ss.ToString();
                        }
                    }
                    else
                    {
                        if (ss < 10)
                        {
                            otpexptime = h.ToString() + ":" + m.ToString() + ":" + sss.ToString();
                        }
                        else
                        {
                            otpexptime = h.ToString() + ":" + m.ToString() + ":" + ss.ToString();
                        }

                    }
                    Otpupdate(no, otp, otpexptime, empid);
                    Id = Encrypt(no.Trim());
                    string hyperlink = "otp.aspx?Id=" + Id.Trim();
                    string message1 = "OTP for  " + empid + "  Login : " + otp + ". Valid till " + otpexptime + "  Do not share OTP for security reasons.";
                    string strUrl = "http://123.63.33.43/blank/sms/user/urlsms.php?username=vyshnavidairy&pass=vyshnavi@123&senderid=VYSAKG&dest_mobileno=" + no + "&message=" + message1 + "&response=Y";
                    //string strUrl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + no + "&source=VYSHRM&message=" + message1 + "";
                    WebRequest request = HttpWebRequest.Create(strUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream s = (Stream)response.GetResponseStream();
                    StreamReader readStream = new StreamReader(s);
                    string dataString = readStream.ReadToEnd();
                    response.Close();
                    s.Close();
                    readStream.Close();
                    string msg = hyperlink;
                    Response.Redirect("otp.aspx?Id=" + Id.Trim());
                }
                else
                {
                    fill_login_details();
                }

            }
            else
            {
                //lbl_validation.Text = "Check Your Username and password";
            }
        }
        catch (Exception ex)
        {
            lbl_validation.Text = ex.Message;
        }
    }
}