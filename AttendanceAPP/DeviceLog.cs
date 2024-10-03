using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceAPP
{
    public class DeviceLog
    {
        public string serial_number { get; set; }
        public List<Log> log { get; set; }
    }
    public class Log
    { 
        public long employee_id { get; set; }
        public DateTime date_time { get; set; }
        public string device_name { get; set; }
        public string employee_name { get; set; }
        public int? AccessType { get; set; }
    }

    public class Response
    { 
        public string status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }
}
