using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessContolwindowsService
{
    public class AttendanceRepo
    {
        static string IsConfigPath = string.Empty;
        static string SqliteDB = string.Empty;
        public AttendanceRepo()
        {
            IsConfigPath = ConfigurationManager.AppSettings["IsConfigPath"];
            SqliteDB = ConfigurationManager.AppSettings["SqliteDB"];
            if (IsConfigPath == "1")
            {
                string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
                SqliteDB = "URI = file:" + Path.GetFullPath(Path.Combine(BasePath, @"..\Database\ZkTecoDb.db"));
            }
        }
       // string SqliteDB = ConfigurationManager.AppSettings["SqliteDB"];
        public DateTime GetLastDate(string deviceNumber)
        {
            try
            {
                DateTime LastPunch = DateTime.Now.AddDays(-365);
                string qry = @"
            SELECT *
            FROM device where SerialNumber=@SerialNumber ";
                var device = GetDeviceBySerialNumber(qry, deviceNumber);
                foreach (DataRow row in device.Rows)
                {
                    string dateString = row["LastAccessDate"].ToString();
                    if (String.IsNullOrEmpty(dateString))
                    {
                        dateString = LastPunch.ToString();
                    }
                    var datetime=Convert.ToDateTime(dateString).ToString();
                    LastPunch = Convert.ToDateTime(datetime);
                }
                return LastPunch;
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public void updatedata(Log log) 
        {
            var cmd = new SQLiteCommand();
            //string SqliteDB = @"URI=file:C:\AttendanceService\attendance.db";
            var connection = new SQLiteConnection(SqliteDB);
            connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "update device set LastAccessDate='" + log.date_time + "' where devicename='" + log.device_name + "'";
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public DataTable GetDeviceBySerialNumber(string query,string deviceNumber)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();
            // string SqliteDB = @"URI=file:C:\AttendanceService\attendance.db";
            try
            {
                using (var connection = new SQLiteConnection(SqliteDB))
                {
                    SQLiteCommand cmd;
                    connection.Open();  //Initiate connection to the db
                    cmd = connection.CreateCommand();
                    cmd.Parameters.AddWithValue("@SerialNumber", deviceNumber);
                    cmd.CommandText = query;  //set the passed query
                    ad = new SQLiteDataAdapter(cmd);
                    ad.Fill(dt); //fill the datasource
                }
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
            return dt;
        }
        public bool InsertAttendanceLogs(Log log)
        {
            try
            {
                using (var connection = new SQLiteConnection(SqliteDB))
                {
                    var cmd = new SQLiteCommand();
                    connection.Open();
                    cmd.Connection = connection;
                    cmd.CommandText = @"INSERT into AttendanceLogs(employee_id,date_time,device_name,employee_name,AccessType,CreateTs) VALUES (@employee_id, @date_time, @device_name, @employee_name,@AccessType,Datetime())";
                    cmd.Parameters.AddWithValue("@employee_id", log.employee_id);
                    cmd.Parameters.AddWithValue("@date_time", log.date_time);
                    cmd.Parameters.AddWithValue("@device_name", log.device_name);
                    cmd.Parameters.AddWithValue("@employee_name", log.employee_name);
                    cmd.Parameters.AddWithValue("@AccessType", log.AccessType);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
