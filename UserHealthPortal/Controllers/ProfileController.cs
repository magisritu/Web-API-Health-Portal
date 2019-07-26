using BusinessLibrary;
using HealthCareLog;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace UserHealthPortal.Controllers
{
    public class ProfileController : Controller
    {
        UserDetailsBusiness UDBusiness = new UserDetailsBusiness();
        // GET: Profile
        public ActionResult ProfilePage()
        {
            string email = Session["email"].ToString();

            return View();
        }
        [HttpPost]
        public ActionResult UpdateUserInformation(string txtFirstName,string txtLastName,string txtAddress,string txtContact)
        {
            UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
            int userID = udModel.userId;
            string email = udModel.email;
            string fName = txtFirstName;
            string lName = txtLastName;
            string address = txtAddress;
            Int64 contact = Int64.Parse(txtContact);
            UDBusiness.UpdateUserDetailsInCredential(userID,fName,lName);
            UDBusiness.UpdateAdditionalUserDetails(userID, address, contact);
            return RedirectToAction("UserDashBoard","MainPage");
        }
        public ActionResult UpdateUserPassword(string txtPassword,string txtNewPassword)
        {
            UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
            if (txtPassword.Equals(udModel.password))
            {
                UDBusiness.UpdatePassword(udModel.userId, txtNewPassword);
            }
            return RedirectToAction("UserDashBoard", "MainPage");
        }
        [HttpPost]
        public ActionResult UpdateUserProfile(HttpPostedFileBase file)
        {
            UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
            try
            {
                file.SaveAs("D://SamplePicture//" + file.FileName);
                String url = "D://SamplePicture/" + Path.GetFileName(file.FileName);
                UDBusiness.UpdateProfilePicture(udModel.userId, url);
                
            }
            catch (Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + message + "');</script>";
                Response.Write(script);
            }
            Session["imgURL"] = SetProfilePicture();
            return RedirectToAction("UserDashBoard", "MainPage");
        }

        public string SetProfilePicture()
        {
            ProfilePictureController profilePictureCobj = new ProfilePictureController();
            UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
            string completeURL = "";
            try
                {
                    string url = UDBusiness.GetProfilePicture(udModel.userId);
                    completeURL = "http://myportal.com" + "//ProfilePicture//ChangeProfile" + "?Imagepath=" + url + "";
            }
                catch (Exception ex)
                {
                    WriteLog write = new WriteLog();
                    string message = ex.Message;
                    write.WriteLogMessage(message);
                    string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                    string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                    Response.Write(script);
                }
            return completeURL;
        }
    }
}