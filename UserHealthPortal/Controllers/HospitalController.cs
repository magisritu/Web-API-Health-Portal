using BusinessLibrary;
using HealthCareLog;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserHealthPortal.Controllers
{
    public class HospitalController : Controller
    {
        BusinessLibrary.UserDetailsBusiness UDBusiness = new BusinessLibrary.UserDetailsBusiness();
        public ActionResult Index()
        {
            try
            {
                if (Session["userModel"] != null)
                {

                    UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                    ModelState.Clear();
                    Session["userID"] = udModel.userId;
                    return View(UDBusiness.GetHospitalList(udModel.userId));
                }
                else
                {
                    return RedirectToAction("MainPage/Login");
                }
            }
            catch(Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
                return RedirectToAction("MainPage/Login");
            }
            
        }

        //public ActionResult ShowHospital()
        //{
        //    if (Session["userModel"] != null)
        //      {
        //        UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
        //        ModelState.Clear();
        //        Session["userID"] = udModel.userId;
        //        return View(UDBusiness.GetHospitalList(udModel.userId));
        //    }
        //    else
        //    {
        //        return RedirectToAction("MainPage/Login");
        //    }
        //}

        public ActionResult Create()
        {
            return View();
        }

        
        public ActionResult UpdateHospital(HospitalForTableModel UDHospital)
        {
            try
            {
                UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                UDBusiness.UpdateHospitalDetails(UDHospital);
                if (UDHospital.PrimaryMark.Equals("Yes"))
                {
                    UDBusiness.UpdatePrimaryMark(udModel.userId, UDHospital.HospitalID);
                }
                return JavaScript("location.reload(true)");
            }
            catch(Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
                return JavaScript("location.reload(true)");
            }
                
        }

        [HttpPost]
        public ActionResult NewHospitalDetails(string txtHospitalID,string txtHospitalName,string txtAddress,string txtEmail,string txtContact1,string txtContact2,string ddlPrimaryMark)
        {
            try
            {
                UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                HospitalDetailsModel HDModel = new HospitalDetailsModel();
                HDModel.hospitalID = Int32.Parse(txtHospitalID);
                HDModel.hospitalName = txtHospitalName;
                HDModel.address = txtAddress;
                HDModel.emailID = txtEmail;
                HDModel.contact1 = Int64.Parse(txtContact1);
                HDModel.contact2 = Int64.Parse(txtContact2);
                HDModel.primaryMark = ddlPrimaryMark;
                HDModel.userID = udModel.userId;

                UDBusiness.SetHospitalDetails(HDModel);
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
            return RedirectToAction("Index");
        }
        
    }
}