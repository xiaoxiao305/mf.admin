using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.ConsoleApp
{
   public class Log
    {
        private static string logPath = ConfigurationManager.AppSettings["logPath"];
        public static void WriteDebug(params object[] logs)
        {
            string path = string.Format("{0}Debug_{1}.txt", logPath, DateTime.Now.ToString("yyyy-MM-dd"));
            WriteContent(path, logs);
        }
        public static void WriteLog(params object[] logs)
        {
            string path = string.Format("{0}Log_{1}.txt", logPath, DateTime.Now.ToString("yyyy-MM-dd"));
            WriteContent(path, logs);
        }
        public static void WriteError(params object[] errs)
        {
            string path = string.Format("{0}Err_{1}.txt", logPath, DateTime.Now.ToString("yyyy-MM-dd"));
            WriteContent(path, errs);
        }
        public static void WriteContent(string path,params object[] pars)
        {
            try
            {
                string parStr = "";
                foreach (var par in pars)
                {
                    parStr += par;
                }
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        //sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff ") + " " + parStr + "\r\n");
                        sw.WriteLine(parStr );
                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    //sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff ") + " " + parStr + "\r\n");
                    sw.WriteLine(parStr);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
