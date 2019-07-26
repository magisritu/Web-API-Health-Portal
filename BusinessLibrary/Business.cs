using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using ModelLibrary;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace BusinessLibrary
{
    public class UserDetailsBusiness
    {
        SqlConnection con;
        string cs = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
        UserDetailsDataAccess UDDataAccess = new UserDetailsDataAccess();
        public void SetUserData(UserDetailsModel UDModel)
        {
            UDDataAccess.SetUserData(UDModel);
        }
        public bool CheckCredential(string email, string password)
        {
            return UDDataAccess.CheckCredential(email, password);
        }
        public UserDetailsModel GetUserDetailsFromCredential(string email)
        {
            return UDDataAccess.GetUserDetailsFromCredential(email);
        }
        public void SetVerificationKey(string email, string status, string code)
        {
            UDDataAccess.SetVerificationCode(email, status, code);
        }
        public string RetriveVerificationKey(string email)
        {
            return UDDataAccess.RetriveVerificationKey(email);
        }
        public bool CheckEmailVerification(string email)
        {
            return UDDataAccess.CheckEmailVerification(email);
        }
        public void DeleteVerificationCode(string email)
        {
            UDDataAccess.DeleteVerificationCode(email);
        }
        public void DeleteCredential(Int32 userID)
        {
            UDDataAccess.DeleteCredential(userID);
        }
        public void UpdateEmailVerification(string email)
        {
            UDDataAccess.UpdateEmailVerification(email);
        }
        public SqlDataReader GetUserDetailsFromDB(Int32 userID)
        {
            return UDDataAccess.GetUserDetailsFromDB(userID);
        }
        public void AdditionalUserDetails(Int32 userID)
        {
            UDDataAccess.AdditonUserDetails(userID);
        }
        public void UpdateAdditionalUserDetails(Int32 userID, string address, Int64 phone)
        {
            UDDataAccess.UpdateAdditonalDetails(userID, address, phone);
        }
        public SqlDataReader GetAdditionalUserDetails(Int32 userID)
        {
            return UDDataAccess.GetAdditionalUserDetails(userID);
        }
        public void UpdateUserDetailsInCredential(Int32 userID, string firstName, string lastName)
        {
            UDDataAccess.UpdateUserDetailsInCredential(userID, firstName, lastName);
        }
        public void UpdatePassword(Int32 userID, string password)
        {
            UDDataAccess.UpdatePassword(userID, password);
        }
        public DataTable FetchHospitalDetailsForGrid(Int32 userID)
        {
            return UDDataAccess.FetchHospitalDetailsForGrid(userID);
        }
        public void SetHospitalDetails(HospitalDetailsModel HDModel)
        {
            UDDataAccess.SetHospitalDetails(HDModel);
        }
        public void DeleteHospitalDetails(Int32 hospitalID, Int32 userID)
        {
            UDDataAccess.DeleteHospitalDetails(hospitalID, userID);
        }
        public bool CheckHospitalInGrid(Int32 hospitalID)
        {
            return UDDataAccess.CheckHospitalInGrid(hospitalID);
        }
        public SqlDataReader ShowHospitalDataOnModal(Int32 hospitalID, Int32 userID)
        {
            return UDDataAccess.ShowHospitalDataOnModal(hospitalID, userID);
        }
        public void UpdateHospitalDetails(HospitalForTableModel UDHospital)
        {
            UDDataAccess.UpdateHospitalDetails(UDHospital);
        }
        public string GetProfilePicture(Int32 userID)
        {
            return UDDataAccess.GetProfilePicture(userID);
        }
        public void UpdateProfilePicture(Int32 userID, string url)
        {
            UDDataAccess.UpdateProfilePicture(userID, url);
        }
        public void InsertProfilePicture(Int32 userID, string url)
        {
            UDDataAccess.InsertProfilePicture(userID, url);
        }
        public DataTable FetchDoctorDetailsForGrid(Int32 userID)
        {
            return UDDataAccess.FetchDoctorDetailsForGrid(userID);
        }
        public void DeleteDoctorDetails(Int32 DoctorID,Int32 userID)
        {
            UDDataAccess.DeleteDoctorDetails(DoctorID,userID);
        }
        public void SetDoctorDetails(DoctorDetailsModel DDModel)
        {
            UDDataAccess.SetDoctorDetails(DDModel);
        }
        public DataTable GetSpecialist()
        {
            return UDDataAccess.GetSpecialist();
        }
        public DataTable GetDoctor(Int32 userID)
        {
            return UDDataAccess.GetDoctor(userID);
        }
        public void UpdateDoctorDetails(DoctorDetailsModel DDModel, Int32 userID)
        {
            UDDataAccess.UpdateDoctorDetails(DDModel, userID);
        }
        public bool CheckDoctorInDB(Int32 doctoeID, Int32 userID)
        {
            return UDDataAccess.CheckDoctorInDB(doctoeID, userID);
        }
        public SqlDataReader GetDoctorDetails(Int32 doctorID, Int32 userID)
        {
            return UDDataAccess.GetDoctorDetails(doctorID, userID);
        }
        public List<DoctorDetailsModel> GetAllDoctorList(string sortColumn)
        {
            List<DoctorDetailsModel> listDoctor = new List<DoctorDetailsModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "select DoctorID,FirstName,LastName,Address,ContactNumber1 from UserDoctor";
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    query += " order by " + sortColumn;
                }
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DoctorDetailsModel DDModel = new DoctorDetailsModel();
                    DDModel.doctorID = Int32.Parse(reader[0].ToString());
                    DDModel.firstName = reader[1].ToString();
                    DDModel.lastName = reader[2].ToString();
                    DDModel.address = reader[3].ToString();
                    DDModel.contactNumber1 = Int64.Parse(reader[4].ToString());
                    listDoctor.Add(DDModel);
                }

            }
            return listDoctor;
        }
        
        public List<HospitalForTableModel> GetHospitalList(Int32 userID)
        {
            List<HospitalForTableModel> listHospital = new List<HospitalForTableModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spSelectDataForHospitalGrid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("userID", userID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HospitalForTableModel HTableModel = new HospitalForTableModel();
                    HTableModel.HospitalID = Int32.Parse(reader[0].ToString());
                    HTableModel.HospitalName = reader[1].ToString();
                    HTableModel.Address = reader[2].ToString();
                    HTableModel.EmailID = reader[3].ToString();
                    HTableModel.Contact1 = Int64.Parse(reader[4].ToString());
                    HTableModel.Contact2 = Int64.Parse(reader[5].ToString());
                    HTableModel.PrimaryMark = reader[6].ToString();
                    listHospital.Add(HTableModel);
                }
            }
            return listHospital;
        }
        //Doctor Detail in List
        public List<DoctorForTable> GetDoctorList(Int32 userID)
        {
            List<DoctorForTable> listDoctor = new List<DoctorForTable>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetDoctorList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("userID", userID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DoctorForTable doctorModel = new DoctorForTable();
                    doctorModel.DoctorID = Int32.Parse(reader[0].ToString());
                    doctorModel.FirstName = reader[1].ToString();
                    doctorModel.LastName = reader[2].ToString();
                    doctorModel.EmailID = reader[3].ToString();
                    doctorModel.RelatedHospital = reader[4].ToString();
                    doctorModel.Specialty = reader[5].ToString();
                    doctorModel.Address = reader[6].ToString();
                    doctorModel.ContactNumber1 = Int64.Parse(reader[7].ToString());
                    doctorModel.ContactNumber2 = Int64.Parse(reader[8].ToString());
                    doctorModel.PrimaryDoctorMark = reader[9].ToString();
                    listDoctor.Add(doctorModel);
                }
            }
            return listDoctor;
        }

        //public SqlDataReader GetReportDetails(Int32 userID)
        public List<ReportModel> GetRepotDetailsOfUser(Int32 userID)
        {
            SqlDataReader reader = UDDataAccess.GetReportDetails(userID);
            List<ReportModel> listReport = new List<ReportModel>();
            while (reader.Read())
            {
                ReportModel reportModel = new ReportModel();
                reportModel.reportID = Int32.Parse(reader[0].ToString());
                reportModel.reportType = reader[1].ToString();
                reportModel.hospital = reader[2].ToString();
                reportModel.doctor = reader[3].ToString();
                reportModel.date = reader[4].ToString();
                //DateTime.ParseExact(reader[4].ToString(), "yyyy-MM-dd",
                //System.Globalization.CultureInfo.InvariantCulture);
                reportModel.upload = reader[5].ToString();
                listReport.Add(reportModel);

            }
            return listReport;
        }
        public List<ReportModel> GetReportBasedOnDate(string fromDate, string toDate, Int32 userID)
        {
            SqlDataReader reader = UDDataAccess.GetReportBasedOnDate(fromDate, toDate, userID);
            ReportModel reportModel = new ReportModel();
            List<ReportModel> listReport = new List<ReportModel>();
            while (reader.Read())
            {
                ReportModel reportModel1 = new ReportModel();
                reportModel1.reportID = Int32.Parse(reader[0].ToString());
                reportModel1.reportType = reader[1].ToString();
                reportModel1.hospital = reader[2].ToString();
                reportModel1.doctor = reader[3].ToString();
                reportModel1.date = reader[4].ToString();
                //DateTime.ParseExact(reader[4].ToString(), "yyyy-MM-dd",
                //                 System.Globalization.CultureInfo.InvariantCulture);
                reportModel1.upload = reader[5].ToString();
                listReport.Add(reportModel1);

            }
            return listReport;

        }
        public void UpdatePrimaryMark(Int32 userID, Int32 hospitalID)
        {
            UDDataAccess.UpdatePrimaryMark(userID, hospitalID);
        }
        public void UpdateDoctorPrimaryMark(Int32 userID, Int32 doctorID)
        {
            UDDataAccess.UpdateDoctorPrimaryMark(userID, doctorID);
        }
        public DataTable GetHospitalForDDL(Int32 userID)
        {
            return UDDataAccess.GetHospitalForDDL(userID);
        }
        public DataTable GetDoctorForDDL(Int32 userID)
        {
            return UDDataAccess.GetDoctorForDDL(userID);
        }
        public void UploadReportDetails(ReportModel reportModel)
        {
            UDDataAccess.UploadReportDetails(reportModel);
        }
        public string GetFileURL(Int32 reportID, Int32 userID)
        {
            return UDDataAccess.GetFileURL(reportID, userID);
        }
        public string IngnoreSpecialCharacter(string str)
        {
            string msg = "";
            foreach (char c in str)
            {
                if (c != '\'')
                {
                    msg = msg + c;
                }
            }
            return msg;
        }
        public string ChangeDateFormat(string date)
        {
            string msg = "";
            foreach (char c in date)
            {
                if (c == '/')
                {
                    msg = msg + '-';
                }
                else
                {
                    msg = msg + c;
                }
            }
            return msg;
        }

    }
}
