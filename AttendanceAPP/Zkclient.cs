using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using zkemkeeper;

namespace AttendanceAPP
{
    public class Zkclient
    {
        private CZKEM cZKEM = null;
        private bool _isConnected = false;
        private int _iMachineNumber = 1;
        private bool _status = false;
        private AttendanceRepo attendanceRepo = null;
        private List<Log> logs = null;
        

        public Zkclient()
        {
            cZKEM = new CZKEM();
        }
        public void FetchLog()
        {
            var accessPointModel = GetDeviceList();
            foreach (var item in accessPointModel)
            {
                GetUserLog(item);
            }
        }

        public static DataTable selectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();
            //string SqliteDB = @"URI=file:C:\AttendanceService\attendance.db";
            string SqliteDB = @"Data Source=" + Application.StartupPath + @"\Database\ZkTecoDb.db;version=3";
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
                            FROM device where IsActive='1' order by 1 asc";
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

        //GET USER LOGS FROM TERMINAL
        public string GetDeviceSerialNumber(string IPAddress,string port)
        {
            string SerialNumber = string.Empty;
            try
            {
                _isConnected = cZKEM.Connect_Net(IPAddress, Convert.ToInt32(port));
                if (_isConnected)
                {
                    string IP = string.Empty;
                    cZKEM.GetDeviceIP(_iMachineNumber, ref IP);
                    cZKEM.GetSerialNumber(_iMachineNumber, out SerialNumber);
                    bool d=cZKEM.RegEvent(_iMachineNumber, 2);
                }
                 else
                {
                    SerialNumber = "";
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("something went wrong");
               
            }
            
            return SerialNumber;
        }

        public bool CheckDeviceStatus(string IPAddress, string port)
        {
            try
            {
                _isConnected = cZKEM.Connect_Net(IPAddress, Convert.ToInt32(port));
                if (_isConnected)
                {
                    _status = true;
                }
                else
                {
                    _status = false;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return _status;
        }

       
         public bool CheckDeviceStatusAND_Update(string IPAddress, string port)
        {
             AttendanceRepo attendanceupdate = new AttendanceRepo();
            try
            {
                _isConnected = FetcherRepo.TelnetCommand(IPAddress, Convert.ToInt32(port));
                if (_isConnected)
                {
                    attendanceupdate.UpdateDeviceStatus("1", IPAddress,port);
                    _status = true;
                }
                else
                {
                    attendanceupdate.UpdateDeviceStatus("0", IPAddress, port);
                    _status = false;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return _status;
        }


        public List<DeviceList> GetAllDeviceList()
        {

            List<DeviceList> List = new List<DeviceList>();
            try
            {
                string qry = @"
                            SELECT *
                            FROM device order by 1 asc";
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

        public string GetInactiveDeviceCount()
        {
            try
            {
                string InActiveCount = "";
                string qry = @"
                            SELECT count(*) InActiveCount
                            FROM device where IsActive='0' order by 1 asc";
                var Data = selectQuery(qry);
                foreach (DataRow row in Data.Rows)
                {
                     InActiveCount= row["InActiveCount"].ToString();
                }
                return InActiveCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string GetactiveDeviceCount()
        {
            try
            {
                string InActiveCount = "";
                string qry = @"
                            SELECT count(*) InActiveCount
                            FROM device where IsActive='1' order by 1 asc";
                var Data = selectQuery(qry);
                foreach (DataRow row in Data.Rows)
                {
                    InActiveCount = row["InActiveCount"].ToString();
                }
                return InActiveCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Ping Device
        public static bool PingTheDevice(string ipAdd)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ipAdd);
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                // Create a buffer of 32 bytes of data to be transmitted.  .Net Documentation
                string data = "Ping..!";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                    return true;
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ClearGLog(int machineNumber)
        {
            return cZKEM.ClearGLog(machineNumber);
        }
        public bool CheckDeviceStatus1(string IPAddress, string port)
        {
            _isConnected = cZKEM.Connect_Net(IPAddress, Convert.ToInt32(port));
            if (_isConnected)
            {
                _status = true;
            }
            else
            {
                _status = false;
            }
            return _status;
        }
        //GET USER LOGS FROM TERMINAL
        public void GetUserLog(DeviceList device)
        {
            try
            {

                //var data=cZKEM.ACUnlock(_iMachineNumber, 100);
                //StaticVariables.Log("CONNECTING.......");
                // Console.WriteLine($"Connecting a Device {device.SerialNumber}  {device.IPAddress}:{device.port}");
                // _isConnected = cZKEM.Connect_Net(device.IPAddress, (int)device.port);
                bool status = CheckDeviceStatus1(device.IPAddress, device.port.ToString());
                DateTime LastFetchDateTime = attendanceRepo.GetLastDate(device.SerialNumber);

                if (status == true)
                {
                    string idwEnrollNumber = "";
                    int idwVerifyMode = 0;
                    int idwInOutMode = 0;
                    int idwYear = 0;
                    int idwMonth = 0;
                    int idwDay = 0;
                    int idwHour = 0;
                    int idwMinute = 0;
                    int dwSecond = 0;
                    int dwWorkCode = 0;
                    string IP = string.Empty;
                    string SN = string.Empty;
                    var PostLog = new DeviceLog();


                    //Console.WriteLine($"{device.IPAddress} is Connected Successfully");
                    cZKEM.GetDeviceIP(_iMachineNumber, ref IP);
                    cZKEM.GetSerialNumber(_iMachineNumber, out SN);
                    cZKEM.RegEvent(_iMachineNumber, 65535);
                    //StaticVariables.Log(SN + " DEVICE SUCCESSFULLY CONNECTED!!!!");


                    //StaticVariables.Log("EVENT REGISTERED!!!!");
                    //StaticVariables.Log("DEVICE TO FETCH DATA.......");
                    //StaticVariables.Log("READING RECORDS FROM DEVICE......");

                    if (cZKEM.ReadGeneralLogData(_iMachineNumber))//read all the attendance records to the memory
                    {
                        //StaticVariables.Log("ADDING RECORDS TO LIST...!!!!");
                        while (cZKEM.SSR_GetGeneralLogData(_iMachineNumber, out idwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out dwSecond, ref dwWorkCode))//get records from the memory
                        {
                            var PunchDate = DateTime.Parse(idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + dwSecond);
                            if (PunchDate > LastFetchDateTime)
                            {
                                logs.Add(new Log
                                {
                                    employee_id = long.Parse(idwEnrollNumber),
                                    date_time = DateTime.Parse(idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + dwSecond),
                                    device_name = device.DeviceName,
                                    employee_name = "",
                                    AccessType = idwVerifyMode
                                });
                                PostLog = new DeviceLog
                                {
                                    serial_number = SN,
                                    log = logs
                                };

                                var Lg = new Log
                                {
                                    employee_id = long.Parse(idwEnrollNumber)
                                   ,
                                    date_time = DateTime.Parse(idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + dwSecond)
                                   ,
                                    device_name = device.DeviceName
                                   ,
                                    employee_name = ""
                                   ,
                                    AccessType = idwVerifyMode
                                };

                              //  attendanceRepo.InsertAttendanceLogs(Lg); //insert logs into table for backup

                                //#region Access control push to web
                                //if (device.IsAccessControl == "1")
                                //{
                                //    string responseStr1 = null;
                                //    string AccessControl = device.AccessControl;
                                //    string AccessControlURL = AccessControl;
                                //    int DeviceAccessType = 0;
                                //    if (idwVerifyMode == 1)
                                //    {
                                //        DeviceAccessType = 2;
                                //    }
                                //    else
                                //    {
                                //        DeviceAccessType = 1;
                                //    }
                                //    var data = new
                                //    {
                                //        serialNumber = SN,
                                //        accessType = DeviceAccessType
                                //    };

                                //    var Request = JsonConvert.SerializeObject(data);
                                //    using (var client = new HttpClient())
                                //    {
                                //        client.DefaultRequestHeaders.Add("Accept", "application/json");
                                //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                                //        using (var http_response = client.PostAsync(AccessControlURL, new StringContent(Request, Encoding.UTF8, "application/json")))
                                //        {
                                //            responseStr1 = http_response.Result.Content.ReadAsStringAsync().Result;
                                //        }
                                //    }
                                //    var JsonResponse1 = JsonConvert.DeserializeObject<Response>(responseStr1);
                                //    string Code1 = JsonResponse1.code.ToString();
                                //    //StaticVariables.Log("Request: " + data.ToString());
                                //    //StaticVariables.Log("Response: " + responseStr1.ToString());
                                //}
                                //#endregion
                            }

                        }

                        #region attendance log push to web
                        if (device.IsAttendance == "1")
                        {
                            if (PostLog.log != null)
                            {
                                var LastIndex = logs[logs.Count() - 1];//get last punch index
                                #region post to web
                                string responseStr = null;
                                string device_uuid = device.Deviceuuid;
                                string endpoint = device_uuid;
                                var JsonRequest = JsonConvert.SerializeObject(PostLog);

                                using (var client = new HttpClient())
                                {
                                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                                    Console.WriteLine($"Post Data to WEB API..!");
                                    using (var http_response = client.PostAsync(endpoint, new StringContent(JsonRequest, Encoding.UTF8, "application/json")))
                                    {
                                        responseStr = http_response.Result.Content.ReadAsStringAsync().Result;
                                    }
                                    var JsonResponse = JsonConvert.DeserializeObject<Response>(responseStr);
                                    string Code = JsonResponse.code.ToString();
                                    if (Code == "200")//success
                                    {
                                        attendanceRepo.updatedata(LastIndex);//update last punch og
                                    }
                                    Console.WriteLine($"Response from WEB API..!");
                                }

                                #endregion
                            }
                        }
                        #endregion
                        // ClearGLog(_iMachineNumber);//clrear device attendancelogs
                      //  logs.Clear();
                        _status = true;

                    }

                    else
                    {
                       // logs.Clear();
                        _status = true;
                        //Console.WriteLine("No New Record Found");
                        // StaticVariables.Log("NO NEW RECORD FOUND IN DEVICE ");
                    }
                }
                //else
                //{
                //    StaticVariables.Log("IPADDRESS {0} IS PINGING...."+","+ device.IPAddress);
                //    _isDeviceActive = PingTheDevice(device.IPAddress);
                //    if (_isDeviceActive)
                //    {
                //        StaticVariables.Log("IP {0} IS RESPONSING...ACTIVELY" +","+ device.IPAddress);
                //    }
                //    StaticVariables.Log("PLEASE RESTART YOUR DEVICE");
                //    logs.Clear();
                //    _status = false;
                //}
            }
            catch (Exception exp)
            {
               // StaticVariables.Log(string.Format("Device data not fetched {0}" + "," + exp.Message));
                _status = false;
            }
            finally
            {
                cZKEM.EnableDevice(_iMachineNumber, true);//enable the device
                                                          // StaticVariables.Log("ENABLING THE DEVICE ..!!!!");
                cZKEM.Disconnect();
                // StaticVariables.Log("DEVICE HAS BEEN DISCONNECTED");
               // logs.Clear();
            }
          //  logs.Clear();
            //   return _status;
        }

    }
}
