using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWindowsService
{
   public class StaticVariables
    {
        static string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
        static string LogPath= Path.GetFullPath(Path.Combine(BasePath, @"Logs\"));
        //ConfigurationManager.AppSettings["Log"];
        static string LogFileName;
        static string SourceFile;
        static string EnableLog = ConfigurationManager.AppSettings["EnableLog"].ToString();
        // public static Logger _logger = LogManager.GetCurrentClassLogger();
        //public static void Log(string message)
        //{
        //    try
        //    {
        //        string _message = $"{message} {Environment.NewLine}";
        //        //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logFile.txt", _message);
        //        File.AppendAllText(System.Configuration.ConfigurationManager.AppSettings["Log"] + "logFile.txt", _message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _ = ex;
        //        throw;
        //    }
        //   }

        public static void Log(string str)
        {
            byte[] bytes = new byte[10240];
            if (EnableLog == "1")
            {
                try
                {
                    DateTime CurrTime = DateTime.Now;
                    string StrFile = CurrTime.ToString("MM/dd/yyyy").Replace(" ", "");
                    StrFile = StrFile.Replace(":", "");
                    StrFile = StrFile.Replace("/", "");
                    SourceFile = StrFile + ".txt";
                    LogFileName = LogPath + "\\" + SourceFile;

                    string strTime = CurrTime.ToString("MM/dd/yyyy HH:mm:ss");
                    str = strTime + "-" + str;
                    if (File.Exists(LogFileName) == false)
                    {
                        FileStream fs;
                        fs = File.Create(LogFileName);
                        fs.Close();
                    }
                    else
                    {
                        string strText = File.ReadAllText(LogFileName).ToString();
                        str = strText + Environment.NewLine + str;
                    }

                    File.WriteAllText(LogFileName, str, Encoding.Unicode);
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
