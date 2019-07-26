using BusinessLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareLog
{
    public class WriteLog
    {
        UserDetailsBusiness UDBusiness = new UserDetailsBusiness();
        public void WriteLogMessage(string strMessage)
        {
            string strFileName = @"D:\LogFile\";
            DateTime dt = DateTime.Now;
            string dateTime = Convert.ToString(dt.Date);
            string[] dates = dateTime.Split(' ');
            string date = dates[0] + ".txt";
            var fileLocation = strFileName + UDBusiness.ChangeDateFormat(date);
            if (!File.Exists(fileLocation))
            {
                File.Create(fileLocation);
            }
            FileStream objFilestream = new FileStream(fileLocation, FileMode.Append, FileAccess.Write);
            StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
            strMessage = strMessage + "\t\t\t" + Convert.ToString(dt.TimeOfDay);
            objStreamWriter.WriteLine(strMessage);
            objStreamWriter.Close();
            objFilestream.Close();
        }
    }
}
