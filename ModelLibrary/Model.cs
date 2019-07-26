using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class UserDetailsModel
    {
        public Int32 userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
    public class HospitalDetailsModel
    {
        public Int32 hospitalID { get; set; }
        public string hospitalName { get; set; }
        public string address { get; set; }
        public string emailID { get; set; }
        public Int64 contact1 { get; set; }
        public Int64 contact2 { get; set; }
        public string primaryMark { get; set; }
        public Int32 userID { get; set; }
    }

    public class HospitalForWebGrid
    {
        public Int32 HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string Address { get; set; }
        public string EmailID { get; set; }
        public Int64 Contact1 { get; set; }
        public Int64 Contact2 { get; set; }
        public string PrimaryMark { get; set; }

    }
    public class HospitalForTableModel
    {
        public Int32 HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string Address { get; set; }
        public string EmailID { get; set; }
        public Int64 Contact1 { get; set; }
        public Int64 Contact2 { get; set; }
        public string PrimaryMark { get; set; }
        public Int32 UserID { get; set; }
    }
    public class DoctorDetailsModel
    {
        public Int32 doctorID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string relatedHospital { get; set; }
        public string specialty { get; set; }
        public string address { get; set; }
        public Int64 contactNumber1 { get; set; }
        public Int64 contactNumber2 { get; set; }
        public string primaryDoctorMark { get; set; }
        public Int32 userID { get; set; }

    }

    public class DoctorForTable
    {
        public Int32 DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string RelatedHospital { get; set; }
        public string Specialty { get; set; }
        public string Address { get; set; }
        public Int64 ContactNumber1 { get; set; }
        public Int64 ContactNumber2 { get; set; }
        public string PrimaryDoctorMark { get; set; }

    }
    public class ReportModel
    {
        public Int32 reportID { get; set; }
        public string reportType { get; set; }
        public string hospital { get; set; }
        public string doctor { get; set; }
        public string date { get; set; }
        public string upload { get; set; }
        public Int32 userID { get; set; }
    }
}
