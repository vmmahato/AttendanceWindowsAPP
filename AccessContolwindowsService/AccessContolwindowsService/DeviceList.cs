using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessContolwindowsService
{
    public class DeviceList
    {
        public int DeviceID { get; set; }
        public string IPAddress { get; set; }
        public Nullable<int> port { get; set; }
        public string SerialNumber { get; set; }
        public string Deviceuuid { get; set; }
        public string DeviceName { get; set; }
        public string AccessControl { get; set; }
        public string IsActive { get; set; }
        public string IsAccessControl { get; set; }
        public string IsAttendance { get; set; }
        public string LastPunchDate { get; set; }
    }
}
