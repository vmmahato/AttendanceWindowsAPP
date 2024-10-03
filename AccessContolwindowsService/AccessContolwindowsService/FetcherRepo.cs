using AccessContolwindowsService.DeviceRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace AccessContolwindowsService
{
    public class FetcherRepo
    {
        static string IsConfigPath = string.Empty;
        static string SqliteDB = string.Empty;

        static private Zkclient zkclient = new Zkclient();
        static private List<DeviceList> failurelst = new List<DeviceList>();
        public FetcherRepo()
            {
            IsConfigPath = ConfigurationManager.AppSettings["IsConfigPath"];
            SqliteDB = ConfigurationManager.AppSettings["SqliteDB"];
            if (IsConfigPath == "1")
            {
                string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
                SqliteDB = "URI = file:" + Path.GetFullPath(Path.Combine(BasePath, @"..\Database\ZkTecoDb.db"));
            }
            FetchLog();
            }
        public static void FetchLog()
        {
            var accessPointModel = GetDeviceList();
            foreach (var item in accessPointModel)
            {
                //Console.WriteLine($"Started To Fetch {item.SerialNumber}" + " " + item.IPAddress + ":" + item.port);
                // StaticVariables.Log("...........STATUS..........OF..... " + item.IPAddress + item.port);

                if (TelnetCommand(item.IPAddress, (int)item.port))
                    if (!zkclient.GetUserLog(item))

                    //failurelst.Add(item);
                    Console.Clear();
                    StaticVariables.Log("..................END..................");
            }
            //if (failurelst.Count > 0)
            //{
            //    foreach (var item in failurelst)
            //    {
            //        StaticVariables.Log("RE-TRY TO FETCH FAILURE DEVICE :" + item.SerialNumber + "," + item.port + "," + item.IPAddress);
            //        if (TelnetCommand(item.IPAddress, (int)item.port))
            //            zkclient.GetUserLog(item);
            //        StaticVariables.Log("-----------------END-------------");
            //        //Console.Clear();
            //       // FetchLog();
            //    }

            //}
            // FetchLog();
        }
        
        public static DataTable selectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();
            //string SqliteDB = @"URI=file:C:\AttendanceService\attendance.db";
            try
            {
                using (var connection = new SQLiteConnection(SqliteDB))
                {
                    SQLiteCommand cmd;
                    connection.Open();  //Initiate connection to the db
                    cmd = connection.CreateCommand();
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

        public static List<DeviceList> GetDeviceList()
        {

            List<DeviceList> List = new List<DeviceList>();
            try
            {
                string qry = @"
                            SELECT *
                            FROM device where IsActive='1' and IsAccessControl='1' order by 1 asc";
                var Data = selectQuery(qry);
                foreach (DataRow row in Data.Rows)
                {
                    DeviceList device = new DeviceList();
                    device.IPAddress = row["IPAddress"].ToString();
                    device.SerialNumber = row["SerialNumber"].ToString();
                    device.port = (int)row["PORT"];
                    device.DeviceName = row["DeviceName"].ToString();
                    device.LastPunchDate = row["LastPunchDate"].ToString();
                    device.Deviceuuid = row["Deviceuuid"].ToString();
                    device.AccessControl = row["AccessControl"].ToString();
                    device.IsAccessControl = row["IsAccessControl"].ToString();
                    device.IsActive = row["IsActive"].ToString();
                    device.IsAttendance = row["IsAttendance"].ToString();
                    List.Add(device);
                }
                return List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static bool TelnetCommand(string ip, int portNumber)
        {
            bool status = false;
            TcpClient tcpClient = null;
            try
            {
                IPAddress iPAddress = IPAddress.Parse(ip);
                tcpClient = new TcpClient();
                tcpClient.Connect(iPAddress, portNumber);
                if (tcpClient.Connected)
                    StaticVariables.Log("IP :{0}:{1} IS RESPONSING................ "+ iPAddress +","+ portNumber);
                tcpClient.Close();
                status = true;
            }
            catch (SocketException exp)
            {
                status = false;
                StaticVariables.Log("Socket Exception :{0}"+","+ exp.Message);
            }
            finally
            {
                tcpClient.Close();
               // status = true;
            }
            return status;
        }
    }
}
