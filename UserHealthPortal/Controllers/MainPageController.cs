using BusinessLibrary;
using HealthCareLog;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace UserHealthPortal.Controllers
{
    public class MainPageController : Controller
    {
        string currentEmail = "";
        UserDetailsModel UDModel = new UserDetailsModel();
        UserDetailsBusiness UDBusiness = new UserDetailsBusiness();
        bool flag;
        bool emailVerification;
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubmitLogin(string nameEmail,string namePassword,string check)
        {
            try
            {
                if(check == "on")
                {
                    //FormsAuthentication.SetAuthCookie(nameEmail,false);                    
                    //Response.Write("Cheecked");
                }
                ViewBag.Credential = "";
                flag = UDBusiness.CheckCredential(nameEmail, namePassword);
                emailVerification = UDBusiness.CheckEmailVerification(nameEmail);
                
                string hashCode = "h0sh@key";

                if (flag && emailVerification)
                {
                    
                    UDModel = UDBusiness.GetUserDetailsFromCredential(nameEmail);
                    Session["userModel"] = UDModel;
                    //Setting Cookies for API call
                    string token = UDModel.email + "~" + UDModel.password + "~" + UDModel.userId;
                    //Encryption using MD5
                    string encryption = "";
                    byte[] data = UTF8Encoding.UTF8.GetBytes(token);
                    using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                    {
                        byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashCode));
                        using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                        {
                            ICryptoTransform transform = tripDes.CreateEncryptor();
                            byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                            encryption = Convert.ToBase64String(results, 0, results.Length);
                        }
                    }

                    string encryptedToken = token;
                    HttpCookie loginIdCookie = new HttpCookie("LoggedUserToken");
                    loginIdCookie.Value = encryption;
                    Response.Cookies.Add(loginIdCookie);

                    return RedirectToAction("UserDashBoard");
                }
                else
                {
                    ViewBag.Credential = "Wrong Credential!";
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
                return View("Login");
            }
            
            
        }
        
        public ActionResult UserDashBoard()
        {
            if (Session["userModel"] != null)
            {
                try
                {
                    HttpCookie ck = Request.Cookies["LoggedUser"];
                    UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                    UserDetailsModel userModel = UDBusiness.GetUserDetailsFromCredential(udModel.email);
                    ViewBag.Session = userModel;
                    SqlDataReader reader = UDBusiness.GetAdditionalUserDetails(userModel.userId);
                    while (reader.Read())
                    {
                        Session["Address"] = reader[1].ToString();
                        Session["Contact"] = reader[2].ToString();
                    }
                    //ProfileController profile = new ProfileController();
                    //Session["imgURL"] = profile.SetProfilePicture();
                }
                catch(Exception ex)
                {
                    WriteLog write = new WriteLog();
                    string message = ex.Message;
                    write.WriteLogMessage(message);
                    string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                    string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                    Response.Write(script);
                }
                
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult SignUp(string nameUserId,string nameFirstName,string nameLastName,string namePassword,string nameConPassword,string nameEmail)
        {
            try
            {
                UDModel.userId = Int32.Parse(nameUserId);
                UDModel.firstName = nameFirstName;
                UDModel.lastName = nameLastName;
                UDModel.password = namePassword;
                UDModel.email = nameEmail;
                currentEmail = UDModel.email;
                Guid verificationKey = Guid.NewGuid();
                string code = verificationKey + "?" + "email=" + nameEmail;
                SendCodeToEmail(UDModel.firstName, code, UDModel.email);
                UDBusiness.SetVerificationKey(UDModel.email, "unverified", verificationKey.ToString());
                UDBusiness.SetUserData(UDModel);
                UDBusiness.AdditionalUserDetails(UDModel.userId);
                UDBusiness.InsertProfilePicture(UDModel.userId, "~/profile.jpg");
            }
            catch(Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
            }
            
            return View();          
        }
        protected void SendCodeToEmail(string fName, string code, string emailAddress)
        {
            try
            {
                var client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("b755faa9098bab", "45c75acaddcc09"),
                    EnableSsl = true
                };
                //string Body = "<a href=\"healthcare.com/Home/verify?verify=" + code + "&email=" + emailAddress + "\"> click here to verify</a>";
                string body = "Hello " + fName + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + string.Format("{0}://{1}/MainPage/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, code) + "'>Click here to activate your account.</a>";
                body += "<br /><br />Thanks";

                client.Send("magisriturajverma@gmail.com", emailAddress, "Verification mail", body);
            }
            catch(Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
            }            
        }
        public ActionResult Activation(string email)
        {
            ViewBag.Message = "Invalid Activation code.";
            string vCode = RouteData.Values["id"].ToString();
            if (RouteData.Values["id"] != null)
            {
                string DBCode = UDBusiness.RetriveVerificationKey(email);
                if(DBCode.Equals(vCode))
                {
                    UDBusiness.UpdateEmailVerification(email);
                    ViewBag.Message = "Email Activated";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        
        public ActionResult LogOut()
        {
            Session["userModel"] = null;
            if (Request.Cookies["LoggedUserToken"] != null)
            {
                Response.Cookies["LoggedUserToken"].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Login");
        }
    }
}