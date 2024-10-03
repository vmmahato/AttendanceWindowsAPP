using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using AttendanceAPP;
using System.Configuration.Install;
using System.Reflection;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.ServiceProcess;
using System.Configuration;

namespace AttendanceAPP_CRUD
{
    public partial class Main : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataAdapter adapter;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int DeviceID;
        bool isDoubleClick = false;
        String connectString;
        string _exePath = string.Empty;
        string _exePath1 = string.Empty;
        string _exePath2 = string.Empty;
        string IsAccessControlActive = string.Empty;

        //Assembly.GetExecutingAssembly().Location;
        public Main()
        {
            InitializeComponent();
            //AdminRelauncher();
            //connectString = @"Data Source=" + Application.StartupPath + @"\Database\ZkTecoDb.db;version=3";
            //RegisterMe();
            //InstallMe();
            //InstallMeAccessControl();
            //GenerateDatabase();
            DeviceUpdate();
            //notification.Text = "0";
            //notification.BackColor = Color.Red;
        }
        private void AdminRelauncher()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase;

                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                    Process.GetCurrentProcess().Kill();
                }
                catch (Exception ex)
                {
                    throw ex;
                    Console.WriteLine("This program must be run as an administrator! \n\n" + ex.ToString());
                }
            }
        }

        private bool IsRunAsAdmin()
        {
            try
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(id);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool InstallMe()
        {
            try
            {
                ServiceController ctl = ServiceController.GetServices()
                                   .FirstOrDefault(s => s.ServiceName == "TimeManagementLogFetcher");
                if (ctl == null)
                {
                    _exePath = Application.StartupPath + @"\WindowsService\AWindowsService.exe";
                    ManagedInstallerClass.InstallHelper(
                        new string[] { _exePath });
                }   
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
            return true;
        }
        public bool InstallAutoStartService()
        {
            try
            {
                ServiceController ctl = ServiceController.GetServices()
                                   .FirstOrDefault(s => s.ServiceName == "AutoStartService");
                if (ctl == null)
                {
                    _exePath2 = Application.StartupPath + @"\AutoStartService\AutoStartServiceWindowsService.exe";
                    ManagedInstallerClass.InstallHelper(
                        new string[] { _exePath2 });
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
            return true;
        }
        public bool InstallMeAccessControl()
        {
            try
            {
                IsAccessControlActive = ConfigurationManager.AppSettings["IsAccessControlActive"];
                if (IsAccessControlActive=="1")
                {
                    ServiceController ctl = ServiceController.GetServices()
                                      .FirstOrDefault(s => s.ServiceName == "TimeManagementAccessControl");
                    if (ctl == null)
                    {
                        _exePath1 = Application.StartupPath + @"\AccessControlService\AccessContolwindowsService.exe";
                        ManagedInstallerClass.InstallHelper(
                            new string[] { _exePath1 });
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
            return true;
        }
        public bool RegisterMe()
        {
            try
            {
                #region Sqlite envaronment variable get set
                string DestinationSqliteFile = @"C:\Program Files (x86)\Sqlite3";
                string SourceSqlite3File = Application.StartupPath + @"\Sqlite3";
                if (!Directory.Exists(DestinationSqliteFile))
                {
                    DirectoryInfo di = Directory.CreateDirectory(DestinationSqliteFile);
                    foreach (var srcPath in Directory.GetFiles(SourceSqlite3File))
                    {
                        File.Copy(srcPath, srcPath.Replace(SourceSqlite3File, DestinationSqliteFile), true);
                    }
                }
               

                string EnvPath = System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine) ?? string.Empty;
                if (!string.IsNullOrEmpty(EnvPath) && !EnvPath.EndsWith(";"))
                    EnvPath = EnvPath + ';';
                int count = 0;
                string path = "";
                foreach (string test in (System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine) ?? string.Empty).Split(';'))
                {
                     path= test.Trim();
                    if (path== DestinationSqliteFile)
                    {
                        count = 1;
                    }
                }

                if (count==0)
                {
                    var name = "PATH";
                    var scope = EnvironmentVariableTarget.Machine; // or User
                    var oldValue = Environment.GetEnvironmentVariable(name, scope);
                    var newValue = oldValue + ";"+ DestinationSqliteFile;
                    Environment.SetEnvironmentVariable(name, newValue, scope);
                }
                #endregion

               // bool IsRegistered = IsAlreadyRegistered();
               // if (IsRegistered==false)
                //{
                    string ExistedFile = @"C:\Windows\SysWOW64\zkemkeeper.dll";
                    string destinationFile = @"C:\Windows\SysWOW64";
                    string sourceFile = Application.StartupPath + @"\Zkemkeeperdll";
                    if (!File.Exists(ExistedFile))
                    {
                        foreach (var srcPath in Directory.GetFiles(sourceFile))
                        {
                            File.Copy(srcPath, srcPath.Replace(sourceFile, destinationFile), true);
                        }
                    string commandToExecute = @"c:\Windows\SysWOW64\regsvr32.exe zkemkeeper.dll";
                    Process.Start(@"cmd", @"/c " + commandToExecute);
                }
                    
               // }
               
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
            return true;
        }

        private bool IsAlreadyRegistered()
        {
            using (var classesRootKey = Microsoft.Win32.RegistryKey.OpenBaseKey(
                   Microsoft.Win32.RegistryHive.ClassesRoot, Microsoft.Win32.RegistryView.Default))
            {
                const string clsid = "{00853A19-BD51-419B-9269-2DABE57EB61F}";

                var clsIdKey = classesRootKey.OpenSubKey(@"Wow6432Node\CLSID\" + clsid) ??
                                classesRootKey.OpenSubKey(@"CLSID\" + clsid);

                if (clsIdKey != null)
                {
                    clsIdKey.Dispose();
                    return true;
                }

                return false;
            }
        }

        private void Add(object sender, EventArgs e) {
           
            if (IPAddress.Text != "" || device_uuid.Text != "" || port.Text != "" || device_name.Text!="" || AccessCtrlUUID.Text!="")
            {
               
                try
                {
                    Zkclient zkadd = new Zkclient();
                    string SerialNumber = "";
                    if (IsManual.Checked==false)
                    {
                        SerialNumber = zkadd.GetDeviceSerialNumber(IPAddress.Text.ToString(), port.Text);
                        if (SerialNumber == "")
                        {
                            MessageBox.Show("Device is Inactive");
                            return;
                        }
                    }else
                    {
                        SerialNumber = txt_SerailNumber.Text.ToString();
                    }
                    
                    var IsDeviceExist = CheckDuplicateDevice(SerialNumber, IPAddress.Text.ToString(), port.Text);
                    if (IsDeviceExist.Rows.Count > 0)
                    {
                           MessageBox.Show("Device Already Added, Please check IPAddress,Port,SerialNumber.");
                        return;
                    }
                    var IsCheckScheduleExisted = CheckScheduleTime();
                    if (IsCheckScheduleExisted.Rows.Count <= 0)
                    {
                        conn = new SQLiteConnection(connectString);
                        cmd = new SQLiteCommand();
                        cmd.CommandText = @"INSERT into ServiceSchedule(RunTIme) VALUES (@RunTIme)";
                        cmd.Connection = conn;
                        cmd.Parameters.Add(new SQLiteParameter("@RunTIme", ScheduleTime.Text));
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    
                    conn = new SQLiteConnection(connectString);
                    cmd = new SQLiteCommand();
                    cmd.CommandText = @"INSERT into device(IPAddress,PORT,SerialNumber,Deviceuuid,DeviceName,AccessControl,IsActive,IsAccessControl,IsAttendance,LastpunchDate,CompanyName,EmailAddress,PhoneNumber) VALUES (@IPAddress, @PORT, @SerialNumber, @Deviceuuid, @DeviceName, @AccessControl,@IsActive,@IsAccessControl,@IsAttendance,@LastpunchDate,@CompanyName,@EmailAddress,@PhoneNumber)";
                    cmd.Connection = conn;
                    cmd.Parameters.Add(new SQLiteParameter("@IPAddress", IPAddress.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@PORT", port.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@SerialNumber", SerialNumber));
                    cmd.Parameters.Add(new SQLiteParameter("@Deviceuuid", device_uuid.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@DeviceName", device_name.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@AccessControl", AccessCtrlUUID.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@IsActive", IsActive.Checked));
                    cmd.Parameters.Add(new SQLiteParameter("@IsAccessControl", IsAccessControl.Checked));
                    cmd.Parameters.Add(new SQLiteParameter("@IsAttendance", IsAttendance.Checked));
                    cmd.Parameters.Add(new SQLiteParameter("@LastpunchDate", dateTimePicker1.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@CompanyName", Company_Name.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@EmailAddress", Email_Address.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@PhoneNumber", Phone_Number.Text));
                    conn.Open();

                    int i = cmd.ExecuteNonQuery();

                    #region notificaton
                    string count = zkadd.GetInactiveDeviceCount();
                    
                    notification.Show();
                    notification.Text = count;
                    notification.BackColor = Color.Red;

                    string activecount = zkadd.GetactiveDeviceCount();
                    ActiveNotification.Show();
                    ActiveNotification.Text = activecount;
                    ActiveNotification.BackColor = Color.Blue;
                    #endregion

                    if (i == 1)
                    {
                        MessageBox.Show("Successfully Created!");
                        device_name.Text = "";
                        IPAddress.Text = "";
                        device_uuid.Text = "";
                        port.Text = "";
                        AccessCtrlUUID.Text = "";
                        txt_SerailNumber.Text = "";
                        IsManual.Checked = true;
                        IsActive.Checked = false;
                        IsAccessControl.Checked = false;
                        IsAttendance.Checked = false;
                        Company_Name.Text = "";
                        Email_Address.Text = "";
                        Phone_Number.Text = "";

                        ReadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else {
                MessageBox.Show("Required Fields are Missing");
            }

        }

        private void GenerateDatabase() {
            String path = Application.StartupPath + @"\Database\ZkTecoDb.db";
            if (!File.Exists(path))
            {
                conn = new SQLiteConnection(connectString);
                conn.Open();
                string sql = "Create table device ( DeviceID INTEGER PRIMARY KEY AUTOINCREMENT, IPAddress TEXT, PORT INT, SerialNumber TEXT, Deviceuuid TEXT, DeviceName TEXT, AccessControl TEXT, LastPunchDate TEXT,IsActive TEXT,IsAccessControl TEXT,IsAttendance TEXT,CompanyName TEXT,EmailAddress TEXT,PhoneNumber TEXT,LastAccessDate TEXT)";
                cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void ReadData() 
        {
            try
            {
                conn = new SQLiteConnection(connectString);
                conn.Open();
                cmd = new SQLiteCommand();
                String sql = "SELECT  ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNO,DeviceID,IPAddress,PORT,SerialNumber,DeviceName,Deviceuuid,AccessControl,LastPunchDate,case when IsActive='1' then 'Active' else 'InActive' end IsActive,case when IsAccessControl='1' then 'YES' else 'NO' end IsAccessControl ,case when IsAttendance='1' then 'YES' else 'NO' end IsAttendance,CompanyName,EmailAddress,PhoneNumber  FROM device";
                adapter = new SQLiteDataAdapter(sql, conn);
                ds.Reset();
                adapter.Fill(ds);
                dt = ds.Tables[0];
                dataGridView1.DataSource = dt;
                conn.Close();
                dataGridView1.Columns[0].HeaderText = "SN";
                dataGridView1.Columns[2].HeaderText = "IP Address";
                dataGridView1.Columns[3].HeaderText = "Port No.";
                dataGridView1.Columns[4].HeaderText = "Serial Number";
                dataGridView1.Columns[5].HeaderText = "Device";
                dataGridView1.Columns[6].HeaderText = "Attendance";
                dataGridView1.Columns[7].HeaderText = "Access Control";
                dataGridView1.Columns[8].HeaderText = "Last PunchDate";
                dataGridView1.Columns[9].HeaderText = "Active Status";
                dataGridView1.Columns[10].HeaderText = "Access Control Status";
                dataGridView1.Columns[11].HeaderText = "Attendance Status";
                dataGridView1.Columns[12].HeaderText = "Company Name";
                dataGridView1.Columns[13].HeaderText = "Email Address";
                dataGridView1.Columns[14].HeaderText = "Phone Number";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetFilertedData
           (
           string IpAddress
           , string PortNo
           , string SerialNumber
           , string Attendance
           , string DeviceName
           , string AccessControl
           , string LastPunchDate
           , bool IsActive
           , bool IsAccessControl
           , bool IsAttendance
           , string CompanyName
           , string EmailAddress
           , string PhoneNumber
           )
        {
            try
            {
                conn = new SQLiteConnection(connectString);
                conn.Open();
                cmd = new SQLiteCommand();
                String sql = "SELECT  ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNO,DeviceID,IPAddress,PORT,SerialNumber,DeviceName,Deviceuuid,AccessControl,LastPunchDate,case when IsActive='1' then 'Active' else 'InActive' end IsActive,case when IsAccessControl='1' then 'YES' else 'NO' end IsAccessControl ,case when IsAttendance='1' then 'YES' else 'NO' end IsAttendance,CompanyName,EmailAddress,PhoneNumber  FROM device where 1=1 ";
                if (IpAddress != "")
                { 
                    sql = sql + "and IPAddress='" + IpAddress+"'";
                }
                if (PortNo != "")
                {
                    sql = sql + "and Port='" + PortNo + "'";
                }
                if (SerialNumber != "")
                {
                    sql = sql + "and LOWER(SerialNumber)='" + SerialNumber.ToLower() + "'";
                }
                if (Attendance != "")
                {
                    sql = sql + "and LOWER(Deviceuuid)='" + Attendance.ToLower() + "'";
                }
                if (DeviceName != "")
                {
                    sql = sql + "and LOWER(DeviceName)='" + DeviceName.ToLower() + "'";
                }
                if (AccessControl != "")
                {
                    sql = sql + "and LOWER(AccessControl)='" + AccessControl.ToLower() + "'";
                }
                string IsActiveStatus = "0";
                string IsAccessControlStatus = "0";
                string IsAttendanceStatus = "0";
                if (IsActive==true)
                {
                    IsActiveStatus = "1";
                }
                if (IsAccessControl == true)
                {
                    IsAccessControlStatus = "1";
                }
                if (IsAttendance == true)
                {
                    IsAttendanceStatus = "1";
                }
                sql = sql + "and IsActive='" + IsActiveStatus + "'";

                sql = sql + "and IsAccessControl='" + IsAccessControlStatus + "'";

                sql = sql + "and IsAttendance='" + IsAttendanceStatus + "'";

                if (CompanyName != "")
                {
                    sql = sql + "and LOWER(CompanyName)='" + CompanyName.ToLower() + "'";
                }
                if (EmailAddress != "")
                {
                    sql = sql + "and LOWER(EmailAddress)='" + EmailAddress.ToLower() + "'";
                }
                if (PhoneNumber != "")
                {
                    sql = sql + "and LOWER(PhoneNumber)='" + PhoneNumber.ToLower() + "'";
                }
                //if (LastPunchDate.ToString() != "")
                //{
                //    sql = sql + "and LastPunchDate<='" + LastPunchDate + "'";
                //}

                adapter = new SQLiteDataAdapter(sql, conn);
                ds.Reset();
                adapter.Fill(ds);
                dt = ds.Tables[0];
                dataGridView1.DataSource = dt;
                conn.Close();


                Zkclient zkping2 = new Zkclient();
                //dateTimePicker1.Visible = false;
                //LastFetchDateTime.Visible = false;
                #region notificaton
                string count = zkping2.GetInactiveDeviceCount();
                notification.Show();
                notification.Text = count;
                notification.BackColor = Color.Red;

                string activecount = zkping2.GetactiveDeviceCount();
                ActiveNotification.Show();
                ActiveNotification.Text = activecount;
                ActiveNotification.BackColor = Color.Blue;
                #endregion
                dataGridView1.Columns[0].HeaderText = "SN";
                dataGridView1.Columns[2].HeaderText = "IP Address";
                dataGridView1.Columns[3].HeaderText = "Port No.";
                dataGridView1.Columns[4].HeaderText = "Serial Number";
                dataGridView1.Columns[5].HeaderText = "Device";
                dataGridView1.Columns[6].HeaderText = "Attendance";
                dataGridView1.Columns[7].HeaderText = "Access Control";
                dataGridView1.Columns[8].HeaderText = "Last PunchDate";
                dataGridView1.Columns[9].HeaderText = "Active Status";
                dataGridView1.Columns[10].HeaderText = "Access Control Status";
                dataGridView1.Columns[11].HeaderText = "Attendance Status";
                dataGridView1.Columns[12].HeaderText = "Company Name";
                dataGridView1.Columns[13].HeaderText = "Email Address";
                dataGridView1.Columns[14].HeaderText = "Phone Number";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetInActiveFilertedData()
        {
            try
            {
                conn = new SQLiteConnection(connectString);
                conn.Open();
                cmd = new SQLiteCommand();
                String sql = "SELECT  ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNO,DeviceID,IPAddress,PORT,SerialNumber,DeviceName,Deviceuuid,AccessControl,LastPunchDate,case when IsActive='1' then 'Active' else 'InActive' end IsActive,case when IsAccessControl='1' then 'YES' else 'NO' end IsAccessControl ,case when IsAttendance='1' then 'YES' else 'NO' end IsAttendance,CompanyName,EmailAddress,PhoneNumber  FROM device where IsActive='0' ";
                

                adapter = new SQLiteDataAdapter(sql, conn);
                ds.Reset();
                adapter.Fill(ds);
                dt = ds.Tables[0];
                dataGridView1.DataSource = dt;
                conn.Close();
                dataGridView1.Columns[0].HeaderText = "SN";
                dataGridView1.Columns[2].HeaderText = "IP Address";
                dataGridView1.Columns[3].HeaderText = "Port No.";
                dataGridView1.Columns[4].HeaderText = "Serial Number";
                dataGridView1.Columns[5].HeaderText = "Device";
                dataGridView1.Columns[6].HeaderText = "Attendance";
                dataGridView1.Columns[7].HeaderText = "Access Control";
                dataGridView1.Columns[8].HeaderText = "Last PunchDate";
                dataGridView1.Columns[9].HeaderText = "Active Status";
                dataGridView1.Columns[10].HeaderText = "Access Control Status";
                dataGridView1.Columns[11].HeaderText = "Attendance Status";
                dataGridView1.Columns[12].HeaderText = "Company Name";
                dataGridView1.Columns[13].HeaderText = "Email Address";
                dataGridView1.Columns[14].HeaderText = "Phone Number";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetActiveFilertedData()
        {
            try
            {
                conn = new SQLiteConnection(connectString);
                conn.Open();
                cmd = new SQLiteCommand();
                String sql = "SELECT  ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNO,DeviceID,IPAddress,PORT,SerialNumber,DeviceName,Deviceuuid,AccessControl,LastPunchDate,case when IsActive='1' then 'Active' else 'InActive' end IsActive,case when IsAccessControl='1' then 'YES' else 'NO' end IsAccessControl ,case when IsAttendance='1' then 'YES' else 'NO' end IsAttendance,CompanyName,EmailAddress,PhoneNumber  FROM device where IsActive='1' ";


                adapter = new SQLiteDataAdapter(sql, conn);
                ds.Reset();
                adapter.Fill(ds);
                dt = ds.Tables[0];
                dataGridView1.DataSource = dt;
                conn.Close();
                dataGridView1.Columns[0].HeaderText = "SN";
                dataGridView1.Columns[2].HeaderText = "IP Address";
                dataGridView1.Columns[3].HeaderText = "Port No.";
                dataGridView1.Columns[4].HeaderText = "Serial Number";
                dataGridView1.Columns[5].HeaderText = "Device";
                dataGridView1.Columns[6].HeaderText = "Attendance";
                dataGridView1.Columns[7].HeaderText = "Access Control";
                dataGridView1.Columns[8].HeaderText = "Last PunchDate";
                dataGridView1.Columns[9].HeaderText = "Active Status";
                dataGridView1.Columns[10].HeaderText = "Access Control Status";
                dataGridView1.Columns[11].HeaderText = "Attendance Status";
                dataGridView1.Columns[12].HeaderText = "Company Name";
                dataGridView1.Columns[13].HeaderText = "Email Address";
                dataGridView1.Columns[14].HeaderText = "Phone Number";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Main_Load(object sender, EventArgs e)
        {
            txt_SerailNumber.Enabled = false;
            //dateTimePicker1.Visible = true;
            //LastFetchDateTime.Visible = true;
            device_uuid.Enabled = false;
            AccessCtrlUUID.Enabled = false;
            IsManual.Checked = true;
            ReadData();
        }

        private void Edit(object sender, DataGridViewCellEventArgs e) {
            DeviceID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            IPAddress.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            port.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txt_SerailNumber.Text= dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            device_name.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            device_uuid.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            AccessCtrlUUID.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            string LastPunchDate = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            string checkedvalue = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            string IsAccessControlEnable = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
            string IsAttendanceEnable = dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
            Company_Name.Text = dataGridView1.SelectedRows[0].Cells[12].Value.ToString();
            Email_Address.Text = dataGridView1.SelectedRows[0].Cells[13].Value.ToString();
            Phone_Number.Text = dataGridView1.SelectedRows[0].Cells[14].Value.ToString();
            NumericUpDown n = new NumericUpDown();
            string RunTime = GetServiceScheduleTime();
            int value = int.Parse(RunTime);
            ScheduleTime.Value = value;
            if (!String.IsNullOrEmpty(LastPunchDate))
            {
                dateTimePicker1.Text = LastPunchDate;
                //dateTimePicker1.MaxDate = Convert.ToDateTime(LastPunchDate);

                //dateTimePicker1.Visible = false;
                //LastFetchDateTime.Visible = false;

            }
            else
            {
                //dateTimePicker1.Visible = true;
                //LastFetchDateTime.Visible = true;

            }

            bool checkeddata = false;
            bool IsAccessControlData = false;
            bool IsAttendanceData = false;
            #region device status
                if (checkedvalue.ToLower()=="active")
                {
                    checkeddata = true;
                }else
                {
                    checkeddata = false;
                }
            #endregion

            #region Access Control status
                if (IsAccessControlEnable.ToLower() == "yes")
                {
                    IsAccessControlData = true;
                }
                else
                {
                    IsAccessControlData = false;
                }
            #endregion

            #region attendance status
                if (IsAttendanceEnable.ToLower() == "yes")
                {
                    IsAttendanceData = true;
                }
                else
                {
                    IsAttendanceData = false;
                }
            #endregion

            IsActive.Checked = checkeddata;
            IsAccessControl.Checked = IsAccessControlData;
            IsAttendance.Checked = IsAttendanceData;
            IsManual.Checked = true;
            isDoubleClick = true;
        }

        private void GetIdToDelete(object sender, DataGridViewCellEventArgs e) {
            DeviceID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            isDoubleClick = false;
            device_name.Text = "";
            IPAddress.Text = "";
            device_uuid.Text = "";
            port.Text = "";
            IsManual.Checked = true;
            IsActive.Checked = false;
            IsAccessControl.Checked = false;
            IsAttendance.Checked = false;
            txt_SerailNumber.Text = "";
            AccessCtrlUUID.Text = "";
            Company_Name.Text = "";
            Email_Address.Text = "";
            Phone_Number.Text = "";
        }


        private void Update(object sender, EventArgs e) {
            Zkclient zkupdate = new Zkclient();
            if (IPAddress.Text != "" || device_uuid.Text != "" || port.Text != "" || device_name.Text != "" || AccessCtrlUUID.Text != "")
            {
                string SerialNumber = "";
                if (IsManual.Checked == false)
                {
                    SerialNumber = zkupdate.GetDeviceSerialNumber(IPAddress.Text.ToString(), port.Text);
                    if (SerialNumber == "")
                    {
                        MessageBox.Show("Device is Inactive");
                        return;
                    }
                }
                else
                {
                    SerialNumber = txt_SerailNumber.Text.ToString();
                }
                //var IsDeviceExist = CheckDuplicateDeviceWhileUpdate(SerialNumber, IPAddress.Text.ToString(), port.Text, DeviceID);
                //if (IsDeviceExist.Rows.Count > 0)
                //{
                //    MessageBox.Show("Device Already Added, Please check IPAddress,Port,SerialNumber.");
                //    return;
                //}
                if (isDoubleClick)
                {
                    try
                    {
                        conn.Open();
                        cmd = new SQLiteCommand();
                        cmd.CommandText = @"UPDATE device set CompanyName=@CompanyName,EmailAddress=@EmailAddress,PhoneNumber=@PhoneNumber,IPAddress=@IPAddress, PORT=@PORT, SerialNumber=@SerialNumber,Deviceuuid=@Deviceuuid,DeviceName=@DeviceName,AccessControl=@AccessControl,IsActive=@IsActive,IsAccessControl=@IsAccessControl,IsAttendance=@IsAttendance,LastPunchDate=@LastPunchDate WHERE DeviceID='" + DeviceID + "'";
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@IPAddress", IPAddress.Text);
                        cmd.Parameters.AddWithValue("@PORT", port.Text);
                        cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                        cmd.Parameters.AddWithValue("@Deviceuuid", device_uuid.Text);
                        cmd.Parameters.AddWithValue("@DeviceName", device_name.Text);
                        cmd.Parameters.AddWithValue("@AccessControl", AccessCtrlUUID.Text);
                        cmd.Parameters.AddWithValue("@IsActive", IsActive.Checked);
                        cmd.Parameters.AddWithValue("@IsAccessControl", IsAccessControl.Checked);
                        cmd.Parameters.AddWithValue("@IsAttendance", IsAttendance.Checked);
                        cmd.Parameters.AddWithValue("@LastPunchDate", dateTimePicker1.Text);
                        cmd.Parameters.AddWithValue("@CompanyName", Company_Name.Text);
                        cmd.Parameters.AddWithValue("@EmailAddress", Email_Address.Text);
                        cmd.Parameters.AddWithValue("@PhoneNumber", Phone_Number.Text);
                        int i = cmd.ExecuteNonQuery();
                        #region notificaton
                        string count = zkupdate.GetInactiveDeviceCount();
                        notification.Show();
                        notification.Text = count;
                        notification.BackColor = Color.Red;

                        string activecount = zkupdate.GetactiveDeviceCount();
                        ActiveNotification.Show();
                        ActiveNotification.Text = activecount;
                        ActiveNotification.BackColor = Color.Blue;
                        #endregion
                        if (i == 1)
                        {
                            cmd.CommandText = @"UPDATE ServiceSchedule set RunTime=@RunTime";
                            cmd.Parameters.AddWithValue("@RunTime", ScheduleTime.Text);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Updated!");
                            IPAddress.Text = "";
                            device_uuid.Text = "";
                            port.Text = "";
                            device_name.Text = "";
                            AccessCtrlUUID.Text = "";
                            txt_SerailNumber.Text = "";
                            IsManual.Checked = true;
                            IsActive.Checked = false;
                            IsAccessControl.Checked = false;
                            IsAttendance.Checked = false;
                            ReadData();
                            DeviceID = 0;
                            dataGridView1.ClearSelection();
                            dataGridView1.CurrentCell = null;
                            isDoubleClick = false;
                            Company_Name.Text = "";
                            Email_Address.Text = "";
                            Phone_Number.Text = "";
                            //dateTimePicker1.Visible = true;
                            //LastFetchDateTime.Visible = true;
                        }

                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Required Fields are Missing");
            }
                
        }

        private void Delete(object sender, EventArgs e) {
            
            if (DeviceID <= 0)
            {
                MessageBox.Show("Please Select Data to delete.");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Do you to delete this record?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    //dateTimePicker1.Visible = true;
                    //LastFetchDateTime.Visible = true;
                    conn = new SQLiteConnection(connectString);
                    conn.Open();
                    cmd = new SQLiteCommand();
                    cmd.CommandText = @"DELETE FROM device WHERE DeviceID='" + DeviceID + "'";
                    cmd.Connection = conn;
                    int i = cmd.ExecuteNonQuery();

                    Zkclient zkping1 = new Zkclient();
                    //dateTimePicker1.Visible = false;
                    //LastFetchDateTime.Visible = false;
                    #region notificaton
                    string count = zkping1.GetInactiveDeviceCount();
                    notification.Show();
                    notification.Text = count;
                    notification.BackColor = Color.Red;

                    string activecount = zkping1.GetactiveDeviceCount();
                    ActiveNotification.Show();
                    ActiveNotification.Text = activecount;
                    ActiveNotification.BackColor = Color.Blue;
                    #endregion
                    if (i == 1)
                    {
                        MessageBox.Show("Successfully Deleted!");
                        DeviceID = 0;
                        dataGridView1.ClearSelection();
                        dataGridView1.CurrentCell = null;
                        ReadData();
                        dataGridView1.ClearSelection();
                        dataGridView1.CurrentCell = null;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }

        }

        private void Clear(object sender, EventArgs e)
        {
            DeviceID = 0;
            IPAddress.Text = "";
            device_uuid.Text = "";
            port.Text = "";
            AccessCtrlUUID.Text = "";
            txt_SerailNumber.Text = "";
            IsManual.Checked = true;
            IsActive.Checked = false;
            IsAccessControl.Checked = false;
            IsAttendance.Checked = false;
            device_name.Text = "";
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;
            isDoubleClick = false;
            //dateTimePicker1.Visible = true;
            //LastFetchDateTime.Visible = true;
            device_uuid.Enabled = false;
            AccessCtrlUUID.Enabled = false;
            dateTimePicker1.Text = DateTime.Now.ToString();
            Company_Name.Text = "";
            Email_Address.Text = "";
            Phone_Number.Text = "";
        }
        public DataTable CheckDuplicateDevice(string SerialNumber,string IPAddress,string Port)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();
            string SqliteDB = connectString;
            try
            {
                string query = @"
                    SELECT *
                    FROM device where SerialNumber=@SerialNumber OR (IPAddress=@IPAddress and Port=@Port)";
                using (var connection = new SQLiteConnection(SqliteDB))
                {
                    SQLiteCommand cmd;
                    connection.Open();  //Initiate connection to the db
                    cmd = connection.CreateCommand();
                    cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                    cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                    cmd.Parameters.AddWithValue("@Port", Port);
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

        public DataTable CheckScheduleTime()
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();
            string SqliteDB = connectString;
            try
            {
                string query = @"
                    SELECT *
                    FROM  ServiceSchedule ";
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

        public string GetServiceScheduleTime()
        {
            try
            {
                string RunTIme = "";
                var device = CheckScheduleTime();
                foreach (DataRow row in device.Rows)
                {
                    RunTIme = row["RunTIme"].ToString();
                    if (RunTIme=="")
                    {
                        RunTIme = "0";
                    }
                }  
                return RunTIme;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable CheckDuplicateDeviceWhileUpdate(string SerialNumber, string IPAddress, string Port,int DeviceID)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();
            string SqliteDB = connectString;
            try
            {
                string query = @"
                    SELECT *
                    FROM device where (SerialNumber=@SerialNumber and DeviceID!=@DeviceID) OR (IPAddress=@IPAddress and Port=@Port and DeviceID!=@DeviceID) ";
                using (var connection = new SQLiteConnection(SqliteDB))
                {
                    SQLiteCommand cmd;
                    connection.Open();  //Initiate connection to the db
                    cmd = connection.CreateCommand();
                    cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                    cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                    cmd.Parameters.AddWithValue("@Port", Port);
                    cmd.Parameters.AddWithValue("@DeviceID", DeviceID);
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void IPAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void device_name_TextChanged(object sender, EventArgs e)
        {

        }

        public void UpdateDeviceStatus()
        {
            try
            {
                AdminRelauncher();
                connectString = @"Data Source=" + Application.StartupPath + @"\Database\ZkTecoDb.db;version=3";
                RegisterMe();
                InstallMe();
                InstallMeAccessControl();
                InstallAutoStartService();
                GenerateDatabase();

                Zkclient zkping = new Zkclient();
                var accessPointModel = zkping.GetAllDeviceList();
                foreach (var item in accessPointModel)
                {
                    zkping.CheckDeviceStatusAND_Update(item.IPAddress, item.port.ToString());
                }
                string count = zkping.GetInactiveDeviceCount();
                notification.Show();
                notification.Text = count;
                notification.BackColor = Color.Red;

                string activecount = zkping.GetactiveDeviceCount();
                ActiveNotification.Show();
                ActiveNotification.Text = activecount;
                ActiveNotification.BackColor = Color.Blue;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void PingDevice()
        {
            Zkclient zkping = new Zkclient();
            //dateTimePicker1.Visible = false;
            //LastFetchDateTime.Visible = false;
            #region notificaton
            string count = zkping.GetInactiveDeviceCount();
            notification.Show();
            notification.Text = count;
            notification.BackColor = Color.Red;

            string activecount = zkping.GetactiveDeviceCount();
            ActiveNotification.Show();
            ActiveNotification.Text = activecount;
            ActiveNotification.BackColor = Color.Blue;
            #endregion
            if (IPAddress.Text != "" || port.Text != "")
            {
                bool status = FetcherRepo.TelnetCommand(IPAddress.Text.ToString(), Convert.ToInt32(port.Text));
                if (status == true)
                {
                    MessageBox.Show("Device is Active");
                    return;
                }
                else
                {
                    MessageBox.Show("Device is Inactive");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Required Field Missing");
                return;
            }

        }
        public void PING_Click(object sender, EventArgs e)
        {
            using (ProgressLoad progress = new ProgressLoad(PingDevice))
            {
                progress.ShowDialog(this);
            }
               
        }

        private void IsManual_CheckedChanged(object sender, EventArgs e)
        {
            if(IsManual.Checked==true)
            {
                txt_SerailNumber.Enabled =true;
            }
            else
            {
                txt_SerailNumber.Enabled = false;
            }
        }

        private void IsAttendance_CheckedChanged(object sender, EventArgs e)
        {
            if (IsAttendance.Checked == true)
            {
                device_uuid.Enabled = true;
            }
            else
            {
                device_uuid.Enabled = false;
            }
        }

        private void IsAccessControl_CheckedChanged(object sender, EventArgs e)
        {
            if (IsAccessControl.Checked == true)
            {
                AccessCtrlUUID.Enabled = true;
            }
            else
            {
                AccessCtrlUUID.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sourceFile = Application.StartupPath + @"\Database\ZkTecoDb.db";
            string destinationFile = @"C:\Users\user\Downloads\ZkTecoDb.db";
            try
            {
                File.Copy(sourceFile, destinationFile, true);
                MessageBox.Show("Data backup Successfully.");
            }
            catch (IOException iox)
            {
                MessageBox.Show("Somthing went wrong");
            }
        }

        private void ImportDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = ".db files | *.db"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                string sourceFile = dialog.FileName;
                string destinationFile = Application.StartupPath + @"\Database\ZkTecoDb.db";
               
               
                try
                {
                    DialogResult = MessageBox.Show("Are you sure to import new data? If yes older data will no more exists", "Conditional", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (DialogResult == DialogResult.Yes)
                    {
                        File.Copy(sourceFile, destinationFile, true);
                        MessageBox.Show("Database upload Successfully.");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Import Cancelled.");
                        return;
                    }
                }
                catch (IOException iox)
                {
                    throw iox;
                    MessageBox.Show("Somthing went wrong");
                }
            }

        }

            private void SEARCH_Click(object sender, EventArgs e)
            {
            GetFilertedData(IPAddress.Text, port.Text, txt_SerailNumber.Text,
                device_uuid.Text,device_name.Text,AccessCtrlUUID.Text
                ,dateTimePicker1.Text,IsActive.Checked
                ,IsAccessControl.Checked,IsAttendance.Checked,Company_Name.Text,Email_Address.Text,Phone_Number.Text);
            }

        private void ManualFetch_Click(object sender, EventArgs e)
        {
            Zkclient zkping = new Zkclient();
            zkping.FetchLog();
            MessageBox.Show("Log has successfully downloaded.");
        }

        private void notification_Click(object sender, EventArgs e)
        {
            GetInActiveFilertedData();
        }
        void DeviceUpdate()
        {
            using (ProgressLoad progress = new ProgressLoad(UpdateDeviceStatus))
            {
                progress.ShowDialog(this);
            }

        }

        private void DeviceHealthCheck_Click(object sender, EventArgs e)
        {
            //progressBar1.Show();
            //progressBar1.Visible = true;
            //UpdateDeviceStatus();
            //progressBar1.Visible = false;
            using (ProgressLoad progress=new ProgressLoad(UpdateDeviceStatus))
            {
                progress.ShowDialog(this);
            }
        }

        private void ActiveNotification_Click(object sender, EventArgs e)
        {
            GetActiveFilertedData();
        }
    }
}
