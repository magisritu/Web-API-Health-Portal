using BusinessLibrary;
using HealthCareLog;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace UserHealthPortal.Controllers
{
    public class ReportController : Controller
    {
        UserDetailsBusiness UDBusiness = new UserDetailsBusiness();
        public ActionResult ShowReport()
        {
            try
            {
                if (Session["userModel"] != null)
                {

                    UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                    ModelState.Clear();
                    Session["userID"] = udModel.userId;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "MainPage");
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
                return RedirectToAction("Login", "MainPage");
            }

        }

        public ActionResult GetReportDetails()
        {
            try
            {
                UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                List<ReportModel> reportList = new List<ReportModel>();
                reportList = UDBusiness.GetRepotDetailsOfUser(udModel.userId);
                //JavaScriptSerializer js = new JavaScriptSerializer();
                return Json(new { data = reportList }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
                return RedirectToAction("Login", "MainPage");
            }
            
        }

        public ActionResult GetReportByDate(string fromDate, string toDate)
        {
            try
            {
                UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                List<ReportModel> reportList = new List<ReportModel>();
                reportList = UDBusiness.GetReportBasedOnDate(fromDate, toDate, udModel.userId);

                return Json(new { data = reportList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
                return RedirectToAction("Report/ShowReport");
            }

        }

        //Dropdown List for Doctors used in Adding Report
        public ActionResult GetDDLDoctor()
        {
            List<String> doctorList = new List<string>();
            try
            {
                UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                DataTable dt = UDBusiness.GetDoctorForDDL(udModel.userId);
                foreach (DataRow row in dt.Rows)
                {
                    string item = row["FirstName"].ToString();
                    doctorList.Add(item);
                }
                return Json(doctorList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
                return RedirectToAction("Login", "MainPage");
            }
        }

        public ActionResult AddReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewReport(string txtReportID,string ddlReportType,string ddlPartialDoctor,string ddlPartialHospital,string txtCalender,HttpPostedFileBase fileUpload)
        {
            ReportModel reportModel = new ReportModel();
            reportModel.reportID = Int32.Parse(txtReportID);
            reportModel.reportType = ddlReportType;
            reportModel.doctor = ddlPartialDoctor;
            reportModel.hospital = ddlPartialHospital;
            reportModel.date = txtCalender;
            
            string folderPath = "D://Report/";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            fileUpload.SaveAs(folderPath + Path.GetFileName(fileUpload.FileName));
            String url = folderPath + Path.GetFileName(fileUpload.FileName);
            reportModel.upload = url;
            string userID = Session["userID"].ToString();
            reportModel.userID = Int32.Parse(userID);
            UDBusiness.UploadReportDetails(reportModel);


            return RedirectToAction("ShowReport", "Report");
        }
    }
}