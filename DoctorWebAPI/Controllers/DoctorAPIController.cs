using BusinessLibrary;
using HealthCareLog;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using UserHealthPortal.Common;

namespace DoctorWebAPI.Controllers
{
    public class DoctorAPIController : ApiController
    {
        UserDetailsBusiness UDBusiness = new UserDetailsBusiness();
        string hashCode = "h0sh@key";
        public string[] UserDetails(string token)
        {
            string decryption = "";
            byte[] data = Convert.FromBase64String(token);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashCode));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    decryption = UTF8Encoding.UTF8.GetString(results);
                }
            }
            string[] encToken = decryption.Split('~');
            return encToken;
        }

        [BasicAuthentication]
        public List<DoctorForTable> Get()
        {
            string token = this.Request.Headers.GetValues("Token").Single();
            string[] userDetails = UserDetails(token);
            int userID = Convert.ToInt32(userDetails[2]);
            List<DoctorForTable> doctorList = new List<DoctorForTable>();
            try
            {
                doctorList = UDBusiness.GetDoctorList(userID);   
            }
            catch (Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
            }
            return doctorList;
        }

        [BasicAuthentication]
        public void Post(DoctorForTable doctorModel)
        {
            string token = this.Request.Headers.GetValues("Token").Single();
            string[] userDetails = UserDetails(token);
            int userID = Convert.ToInt32(userDetails[2]);
            try
            {
                
                DoctorDetailsModel docModel = new DoctorDetailsModel();
                docModel.doctorID = doctorModel.DoctorID;
                docModel.firstName = doctorModel.FirstName;
                docModel.lastName = doctorModel.LastName;
                docModel.email = doctorModel.EmailID;
                docModel.relatedHospital = doctorModel.RelatedHospital;
                docModel.specialty = doctorModel.Specialty;
                docModel.address = doctorModel.Address;
                docModel.contactNumber1 = doctorModel.ContactNumber1;
                docModel.contactNumber2 = doctorModel.ContactNumber2;
                docModel.primaryDoctorMark = doctorModel.PrimaryDoctorMark;
                docModel.userID = 101;
                //Checking Doctordetails InDataBase
                bool checkDoctorInDB = UDBusiness.CheckDoctorInDB(docModel.doctorID, docModel.userID);
                if (!checkDoctorInDB)
                {
                    //setting model to Doctor DB
                    UDBusiness.SetDoctorDetails(docModel);
                    //Updating Primary Mark
                    if (docModel.primaryDoctorMark.Equals("Yes"))
                    {
                        UDBusiness.UpdateDoctorPrimaryMark(docModel.userID, docModel.doctorID);
                    }
                }
                else
                {
                    //Update Doctor Details in DB
                    UDBusiness.UpdateDoctorDetails(docModel, docModel.userID);
                    if (docModel.primaryDoctorMark.Equals("Yes"))
                    {
                        UDBusiness.UpdateDoctorPrimaryMark(docModel.userID, docModel.doctorID);
                    }
                }
                //Authorization a =new Authorization
            }
            catch (Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
            }
            
        }

        [BasicAuthentication]
        public void Delete(string id)
        {
            string token = this.Request.Headers.GetValues("Token").Single();
            string[] userDetails = UserDetails(token);
            int userID = Convert.ToInt32(userDetails[2]);
            try
            { 
                int doctorID = Int32.Parse(id);
                UDBusiness.DeleteDoctorDetails(doctorID, userID);
            }
            catch (Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
            }

        }
    }
}