using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLibrary;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DataAccessLibrary
{
    public class UserDetailsDataAccess
    {
        SqlConnection con;
        string cs = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
        public void SetUserData(UserDetailsModel UDModel)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spSetUserCredential", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("UserID", UDModel.userId);
            cmd.Parameters.AddWithValue("FirstName", UDModel.firstName);
            cmd.Parameters.AddWithValue("LastName", UDModel.lastName);
            cmd.Parameters.AddWithValue("EmailID", UDModel.email);
            cmd.Parameters.AddWithValue("Password", UDModel.password);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public SqlDataReader GetUserDetailsFromDB(Int32 userID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand command1 = new SqlCommand("select * from UserCredential where UserID=" + userID, con);
            SqlDataReader reader = command1.ExecuteReader();
            return reader;
        }
        public bool CheckCredential(string email, string password)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select EmailID,Password from UserCredential", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader[0].ToString().Equals(email) && reader[1].ToString().Equals(password))
                {
                    return true;
                }
            }
            return false;
        }

        public UserDetailsModel GetUserDetailsFromCredential(string email)
        {
            UserDetailsModel UDModel = new UserDetailsModel();
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("getUserDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("email", email);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                UDModel.userId = Int32.Parse(reader[0].ToString());
                UDModel.firstName = reader[1].ToString();
                UDModel.lastName = reader[2].ToString();
                UDModel.email = reader[3].ToString();
                UDModel.password = reader[4].ToString();
            }
            con.Close();
            return UDModel;
        }
        public void SetVerificationCode(string email, string status, string code)
        {
            string query = "insert into EmailVerification values('" + email + "','" + status + "','" + code + "')";
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void DeleteVerificationCode(string email)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spDeleteVerification", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("email", email);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public string RetriveVerificationKey(string email)
        {
            string code = "";
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spRetriveCode", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("email", email);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                code = reader[0].ToString();
            }
            return code;
        }
        public bool CheckEmailVerification(string email)
        {
            string status = "";
            bool result = false;
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spCheckEmailVerification", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("email", email);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                status = reader[0].ToString();
            }
            if(status.Equals("verified"))
            {
                result = true;
            }
            return result;
        }
        public void DeleteCredential(Int32 userID)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spDeleteCredential", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void UpdateEmailVerification(string email)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdateEmail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("email", email);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void AdditonUserDetails(Int32 userID)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spAdditionalUserDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void UpdateAdditonalDetails(Int32 userID, string address, Int64 phone)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdateAdditionalUserDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.Parameters.AddWithValue("address", address);
            cmd.Parameters.AddWithValue("phone", phone);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public SqlDataReader GetAdditionalUserDetails(Int32 userID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand command1 = new SqlCommand("select * from AdditionalUserDetails where UserID=" + userID, con);
            SqlDataReader reader = command1.ExecuteReader();
            return reader;
        }
        public void UpdateUserDetailsInCredential(Int32 userID, string firstName, string lastName)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdateUserCredential", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.Parameters.AddWithValue("firstName", firstName);
            cmd.Parameters.AddWithValue("lastName", lastName);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void UpdatePassword(Int32 userID, string password)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdatePAssword", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.Parameters.AddWithValue("password", password);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public DataTable FetchHospitalDetailsForGrid(Int32 userID)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("spSelectDataForHospitalGrid", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userId", userID);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            return dt;
        }
        public void SetHospitalDetails(HospitalDetailsModel HDModel)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spSetHospitalData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("hospitalID", HDModel.hospitalID);
            cmd.Parameters.AddWithValue("hospitalName", HDModel.hospitalName);
            cmd.Parameters.AddWithValue("address", HDModel.address);
            cmd.Parameters.AddWithValue("email", HDModel.emailID);
            cmd.Parameters.AddWithValue("contact1", HDModel.contact1);
            cmd.Parameters.AddWithValue("contact2", HDModel.contact2);
            cmd.Parameters.AddWithValue("primaryMark", HDModel.primaryMark);
            cmd.Parameters.AddWithValue("userID", HDModel.userID);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void DeleteHospitalDetails(Int32 hospitalID, Int32 userID)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from UserHospital where HospitalID=" + hospitalID + " and UserID=" + userID, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public bool CheckHospitalInGrid(Int32 hospitalID)
        {
            con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select HospitalID from UserHospital", con);
            SqlDataReader reader = cmd.ExecuteReader();
            int i = 0;
            bool result = false;
            while (reader.Read())
            {
                if (hospitalID == Int32.Parse(reader[i].ToString()))
                {
                    result = true;
                    break;
                }
            }
            con.Close();
            return result;
        }
        public SqlDataReader ShowHospitalDataOnModal(Int32 hospitalID, Int32 userID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spShowHospitalDataToModal", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("hospitalID", hospitalID);
            cmd.Parameters.AddWithValue("userID", userID);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        public void UpdateHospitalDetails(HospitalForTableModel UDHospital)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdateHospital", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("hospitalID", UDHospital.HospitalID);
            cmd.Parameters.AddWithValue("hospitalName", UDHospital.HospitalName);
            cmd.Parameters.AddWithValue("address",UDHospital.Address);
            cmd.Parameters.AddWithValue("email",UDHospital.EmailID);
            cmd.Parameters.AddWithValue("primaryContact",UDHospital.Contact1);
            cmd.Parameters.AddWithValue("seconadryContact",UDHospital.Contact2);
            cmd.Parameters.AddWithValue("primaryMark", UDHospital.PrimaryMark);
            cmd.Parameters.AddWithValue("userID", UDHospital.UserID);
            SqlDataReader reader = cmd.ExecuteReader();
            
        }
        public string GetProfilePicture(Int32 userID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select PictureURL from ProfilePicture where UserID=" + userID, con);
            string url = "";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                url = reader[0].ToString();
            }
            return url;
        }
        public void UpdateProfilePicture(Int32 userID, string url)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("UpdateProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.Parameters.AddWithValue("url", url);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void InsertProfilePicture(Int32 userID, string url)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spInsertProfileURL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.Parameters.AddWithValue("url", url);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public DataTable FetchDoctorDetailsForGrid(Int32 userID)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("spFetchDoctorDetailsFroGrid", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userId", userID);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            return dt;
        }
        public void DeleteDoctorDetails(Int32 DoctorID,Int32 userID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spDeleteDoctorDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("DoctorID", DoctorID);
            cmd.Parameters.AddWithValue("UserID", userID);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void SetDoctorDetails(DoctorDetailsModel DDModel)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spSetDoctorDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("doctorID", DDModel.doctorID);
            cmd.Parameters.AddWithValue("firstName", DDModel.firstName);
            cmd.Parameters.AddWithValue("lastName", DDModel.lastName);
            cmd.Parameters.AddWithValue("email", DDModel.email);
            cmd.Parameters.AddWithValue("relatedHospital", DDModel.relatedHospital);
            cmd.Parameters.AddWithValue("specialty", DDModel.specialty);
            cmd.Parameters.AddWithValue("address", DDModel.address);
            cmd.Parameters.AddWithValue("contactNumber1", DDModel.contactNumber1);
            cmd.Parameters.AddWithValue("contactNumber2", DDModel.contactNumber2);
            cmd.Parameters.AddWithValue("primaryDoctorMark", DDModel.primaryDoctorMark);
            cmd.Parameters.AddWithValue("userID", DDModel.userID);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public DataTable GetSpecialist()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("select * from Specialist", con);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            return dt;
        }
        public DataTable GetDoctor(Int32 userID)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            con = new SqlConnection(cs);
            //SqlCommand cmd = new SqlCommand("spRelatedHospital", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("userID", userID);
            SqlCommand cmd = new SqlCommand("select HospitalName from UserHospital where UserID=" + userID, con);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            return dt;
        }
        public void UpdateDoctorDetails(DoctorDetailsModel DDModel, Int32 userID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdateDoctorDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("doctorID", DDModel.doctorID);
            cmd.Parameters.AddWithValue("firstName", DDModel.firstName);
            cmd.Parameters.AddWithValue("lastName", DDModel.lastName);
            cmd.Parameters.AddWithValue("email", DDModel.email);
            cmd.Parameters.AddWithValue("specialty", DDModel.specialty);
            cmd.Parameters.AddWithValue("relatedHospital", DDModel.relatedHospital);
            cmd.Parameters.AddWithValue("address", DDModel.address);
            cmd.Parameters.AddWithValue("contact1", DDModel.contactNumber1);
            cmd.Parameters.AddWithValue("contact2", DDModel.contactNumber2);
            cmd.Parameters.AddWithValue("primaryMark", DDModel.primaryDoctorMark);
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public SqlDataReader GetDoctorDetails(Int32 doctorID, Int32 userID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from UserDoctor where DoctorID=" + doctorID + " and UserID=" + userID, con);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        public bool CheckDoctorInDB(Int32 doctorID, Int32 userID)
        {
            bool flagCheckDoctor = false;
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select DoctorID from UserDoctor where DoctorID=" + doctorID + " and UserID=" + userID, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader[0].ToString() == doctorID.ToString())
                {
                    flagCheckDoctor = true;
                }
            }
            return flagCheckDoctor;
        }
        public void UpdatePrimaryMark(Int32 userID, Int32 hospitalID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdatePrimaryMark", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.Parameters.AddWithValue("hospitalID", hospitalID);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void UpdateDoctorPrimaryMark(Int32 userID, Int32 doctorID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdateDoctorPrimaryMark", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userID", userID);
            cmd.Parameters.AddWithValue("doctorID", doctorID);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public SqlDataReader GetReportDetails(Int32 userID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from UserReport where UserID=" + userID, con);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        public SqlDataReader GetReportBasedOnDate(string fromDate, string toDate, Int32 userID)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spGetReportOnBasedOfDate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("fromDate", Convert.ToDateTime(fromDate));
            cmd.Parameters.AddWithValue("toDate", Convert.ToDateTime(toDate));
            cmd.Parameters.AddWithValue("userID", userID);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        public DataTable GetHospitalForDDL(Int32 userID)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("select HospitalName from UserHospital where UserID=" + userID, con);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            return dt;
        }
        public DataTable GetDoctorForDDL(Int32 userID)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("select FirstName from UserDoctor where UserID=" + userID, con);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            return dt;
        }
        public void UploadReportDetails(ReportModel reportModel)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUploadReportDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("reportID", reportModel.reportID);
            cmd.Parameters.AddWithValue("reportType", reportModel.reportType);
            cmd.Parameters.AddWithValue("hospital", reportModel.hospital);
            cmd.Parameters.AddWithValue("doctor", reportModel.doctor);
            cmd.Parameters.AddWithValue("date", Convert.ToDateTime(reportModel.date));
            cmd.Parameters.AddWithValue("upload", reportModel.upload);
            cmd.Parameters.AddWithValue("userID", reportModel.userID);
            cmd.ExecuteNonQuery();
        }
        public string GetFileURL(Int32 reportID, Int32 userID)
        {
            string url = "";
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select FileURL from UserReport where ReportID=" + reportID + " " + "and UserID=" + userID, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                url = reader[0].ToString();
            }
            return url;
        }
    }
}
