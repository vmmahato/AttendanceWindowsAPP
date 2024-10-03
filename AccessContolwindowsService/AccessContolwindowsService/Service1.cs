using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AccessContolwindowsService
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            FetcherRepo FT = new FetcherRepo();
            SetTimer();
        }

        protected override void OnStop()
        {
        }
        private void SetTimer()
        {
            double intervaltime =double.Parse(GetServiceScheduleTime());
            intervaltime = intervaltime * 60000;
            double timervalue;
            if (intervaltime <= 0)
            {
                timervalue = Convert.ToDouble(ConfigurationManager.AppSettings["Timer"]);
            }else
            {
                timervalue = intervaltime;
            }
            
            timer = new System.Timers.Timer(timervalue);
            // Hook up the Elapsed event for the timer.
            timer.Elapsed += OnElapsedTime;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
           // Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",e.SignalTime);
            FetcherRepo FT = new FetcherRepo();
        }
        public DataTable CheckScheduleTime()
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();
            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string SqliteDB = "URI = file:" + Path.GetFullPath(Path.Combine(BasePath, @"..\Database\ZkTecoDb.db"));
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
                    if (RunTIme == "")
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
    }
}
