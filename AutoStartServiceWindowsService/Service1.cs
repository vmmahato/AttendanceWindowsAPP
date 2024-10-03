using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AutoStartServiceWindowsService
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
            StartService();
            SetTimer();
        }

        protected override void OnStop()
        {
        }
        private void SetTimer()
        {
                double timervalue;
                timervalue = Convert.ToDouble(ConfigurationManager.AppSettings["Timer"]);

            timer = new System.Timers.Timer(timervalue);
            // Hook up the Elapsed event for the timer.
            timer.Elapsed += OnElapsedTime;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            StartService();
        }
        protected void StartService()
        {
            string ServiceName = "TimeManagementLogFetcher";
            ServiceController sc = new ServiceController();
            sc.ServiceName = ServiceName;

           // Console.WriteLine("The {0} service status is currently set to {1}", ServiceName, sc.Status.ToString());

            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                // Start the service if the current status is stopped.
              //  Console.WriteLine("Starting the {0} service ...", ServiceName);
                try
                {
                    // Start the service, and wait until its status is "Running".
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);

                    // Display the current service status.
                   // Console.WriteLine("The {0} service status is now set to {1}.", ServiceName, sc.Status.ToString());
                }
                catch (InvalidOperationException e)
                {
                    //Console.WriteLine("Could not start the {0} service.", ServiceName);
                    //Console.WriteLine(e.Message);
                }
            }
            else
            {
               // Console.WriteLine("Service {0} already running.", ServiceName);
            }
        }
    }
}
