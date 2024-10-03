using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using zkemkeeper;

namespace AccessContolwindowsService.DeviceRepository
{
    public class Zkclient
    {
        private CZKEM cZKEM = null;
        private bool _isConnected = false;
        private bool _isDeviceActive = false;
        private int _iMachineNumber = 1;
        private bool _status = false;
        private AttendanceRepo attendanceRepo = null;
        private List<Log> logs = null;

        public Zkclient()
        {
            cZKEM = new CZKEM();
            attendanceRepo = new AttendanceRepo();
            logs = new List<Log>();
        }

        public bool ClearGLog(int machineNumber)
        {
            return cZKEM.ClearGLog(machineNumber);
        }
        public bool CheckDeviceStatus(string IPAddress, string port)
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
        public bool GetUserLog(DeviceList device)
        {
            try
            {

                //var data=cZKEM.ACUnlock(_iMachineNumber, 100);
                //StaticVariables.Log("CONNECTING.......");
                // Console.WriteLine($"Connecting a Device {device.SerialNumber}  {device.IPAddress}:{device.port}");
                // _isConnected = cZKEM.Connect_Net(device.IPAddress, (int)device.port);
                bool status=CheckDeviceStatus(device.IPAddress, device.port.ToString());
                DateTime LastFetchDateTime = attendanceRepo.GetLastDate(device.SerialNumber);
              
                if (status==true)
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
                    StaticVariables.Log(SN+" DEVICE SUCCESSFULLY CONNECTED!!!!");


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

                               // attendanceRepo.InsertAttendanceLogs(Lg); //insert logs into table for backup

                                #region Access control push to web
                                if (device.IsAccessControl == "1")
                                {
                                    string responseStr1 = null;
                                    string AccessControl = device.AccessControl;
                                    string AccessControlURL = AccessControl;
                                    int DeviceAccessType = 0;
                                    if (idwVerifyMode == 1)
                                    {
                                        DeviceAccessType = 2;
                                    }
                                    else
                                    {
                                        DeviceAccessType = 1;
                                    }
                                    var data = new
                                    {
                                        serialNumber = SN,
                                        accessType = DeviceAccessType
                                    };

                                    var Request = JsonConvert.SerializeObject(data);
                                    using (var client = new HttpClient())
                                    {
                                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                                        StaticVariables.Log($"Request from WEB API..!" + Request.ToString());
                                        using (var http_response = client.PostAsync(AccessControlURL, new StringContent(Request, Encoding.UTF8, "application/json")))
                                        {
                                            responseStr1 = http_response.Result.Content.ReadAsStringAsync().Result;
                                        }
                                    }
                                    var JsonResponse1 = JsonConvert.DeserializeObject<Response>(responseStr1);
                                    string Code1 = JsonResponse1.code.ToString();
                                   
                                    attendanceRepo.updatedata(Lg);//update last punch og

                                    StaticVariables.Log("Request: " + Request.ToString());
                                    StaticVariables.Log("Response: " + responseStr1.ToString());
                                }
                                #endregion
                            }

                        }
                       
                        if (device.IsAccessControl == "1" && device.IsAttendance != "1")
                        {
                           // ClearGLog(_iMachineNumber);//clear device attendancelogs
                        }
                        logs.Clear();
                        _status = true;

                    }
                    
                    else
                    {
                        logs.Clear();
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
                StaticVariables.Log(string.Format("Device data not fetched {0}"+","+ exp.Message));
                _status = false;
            }
            finally
            {
                cZKEM.EnableDevice(_iMachineNumber, true);//enable the device
               // StaticVariables.Log("ENABLING THE DEVICE ..!!!!");
                cZKEM.Disconnect();
               // StaticVariables.Log("DEVICE HAS BEEN DISCONNECTED");
                logs.Clear();
            }
            logs.Clear();
            return _status;
        }

        //IList convert to datatable
        private DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                //table.Columns.Add(prop.Name, prop.PropertyType);
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            StaticVariables.Log("WRITTING DATA TO DATATABLE");
            return table;
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
     
    }
}
