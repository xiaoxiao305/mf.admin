using MF.Common.Json;
using MF.Common.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace MF.Protocol
{
    public class Base
    {
        private static Config _config;
        private static MailConfig _mailconfig;
        internal static Config config
        {
            get
            {
                if (_config == null)
                    onLoadConfig();
                return _config;
            }
        }
        internal static MailConfig mail
        {
            get
            {
                if (_mailconfig == null)
                    onLoadMailConfig();
                return _mailconfig;
            }
        }
        public static bool IsDebug
        {
            get
            {
                return ((CompilationSection)ConfigurationManager.GetSection("system.web/compilation")).Debug;
            }
        }
        /// <summary>
        /// 1:ERROR 2:INFO 3:LOG 4:DEBUG 
        /// </summary>
        public static int logLv
        {
            get
            {
                if (ConfigurationManager.AppSettings["loglv"] != null)
                    return int.Parse(ConfigurationManager.AppSettings["loglv"]);
                else
                    return 0;
            }
        }
        static Base()
        {
        }
        static void onLoadMailConfig()
        {
            _mailconfig = new MailConfig();
            try
            {
                var cfg = LoadFile("mail.config");
                _mailconfig = Json.DeserializeObject<MailConfig>(cfg);
                _mailconfig.Password = AES.Decrypt(_mailconfig.Password);
                if (_mailconfig.Account == "" || _mailconfig.Port == 0)
                    WriteError("加载mail.config文件失败:", Json.SerializeObject(_mailconfig));
            }
            catch (Exception ex)
            {
                WriteError("加载mail.config文件异常：", ex.Message);
            }

        }
        static void onLoadConfig()
        {
            _config = new Config();
            try
            {
                _config.WebServerKey = ConfigurationManager.AppSettings["WEB_SECURITY"];
                _config.WebServerUrl = ConfigurationManager.AppSettings["WebURI"];
                _config.ChargeServerKey = ConfigurationManager.AppSettings["CHARGE_SECURITY"];
                _config.ChargeServerUrl = ConfigurationManager.AppSettings["ChargeURI"];
            }
            catch (Exception ex)
            {
                WriteError("获取web.config中server信息失败：", ex.Message);
            }
        }

        #region 日志
        private static string logPath = ConfigurationManager.AppSettings["logPath"];
        public static void WriteError(params object[] err)
        {
            if (logLv < 1)
                return;
            string path = string.Format("{0}Err_{1}.txt", logPath, DateTime.Now.ToString("yyyy-MM-dd"));
            WriteContent(path, err);
        } 
        public static void WriteInfo(string path, params object[] info)
        {
            if (logLv < 2)
                return;
            WriteContent(path, info);
        }
        public static void WriteLog(params object[] log)
        {
            if (logLv < 3)
                return;
            string path = string.Format("{0}Log_{1}.txt", logPath, DateTime.Now.ToString("yyyy-MM-dd"));
            WriteContent(path, log);
        }
        public static void WriteDebug(params object[] debug)
        {
            if (logLv < 4)
                return;
            string path = string.Format("{0}Debug_{1}.txt", logPath, DateTime.Now.ToString("yyyy-MM-dd"));
            WriteContent(path, debug);
        }
        private static void WriteCallback(IAsyncResult ar)
        {
            var fs = ar.AsyncState as FileStream;
            fs.Dispose();
        }
        public static void WriteContent(string path, params object[] parms)
        {
            try
            {
                string parmsstr = "";
                foreach (var par in parms)
                {
                    parmsstr += par;
                }
                FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 8, true);
                var buffer = Encoding.UTF8.GetBytes(DateTime.Now.ToString("HH:mm:ss.fff ") + " " + parmsstr + "\r\n");
                fs.BeginWrite(buffer, 0, buffer.Length, WriteCallback, fs);
                fs.Close();


                //admin---kf
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff ") + " " + parmsstr + "\r\n");
                        //sw.WriteLine(parmsstr);
                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff ") + " " + parmsstr + "\r\n");
                    //sw.WriteLine(parmsstr);
                }






            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 读取文件
        /// <summary>
        /// 读取指定的文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string filePath = ConfigurationManager.AppSettings["filePath"];
        public static string LoadFile(string filename)
        {
            try
            {
                filename = filePath + filename;
                if (!File.Exists(filename))
                {
                    Base.WriteError(filename + "文件不存在");
                    return "";
                }
                var fs = File.Open(filename, FileMode.Open);
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                var BOM = new byte[] { 0xEF, 0xBB, 0xBF };
                var index = 0;
                if (bytes.Length > BOM.Length)
                {
                    if (bytes[0] == BOM[0] && bytes[1] == BOM[1] && bytes[2] == BOM[2])
                        index = 3;
                }

                var text = System.Text.Encoding.UTF8.GetString(bytes, index, bytes.Length - index);
                fs.Close();
                return text;
            }
            catch (Exception ex)
            {
                WriteError("加载 ", filename, "文件失败：", ex.Message);
            }
            return null;
        }
        #endregion

        public static string[] GetAllKeys(IEnumerator collection, int l)
        {
            var keys = new string[l];
            int i = 0;
            while (collection.MoveNext())
            {
                keys[i] = collection.Current.ToString();
                i++;
            }
            return keys;
        }


        public static string ChineseToUncode(string chinese)
        {
            string outStr = "";
            try
            {
                if (!string.IsNullOrEmpty(chinese))
                {
                    for (int i = 0; i < chinese.Length; i++)
                    {
                        if (new Regex("^[\u4e00-\u9fa5]+$").IsMatch(chinese[i].ToString()))
                            outStr += "//u" + ((int)chinese[i]).ToString("x");
                        else
                            outStr += chinese[i];
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("ChinaToUncode ex:", ex.Message, ",chinese:", chinese);
            }
            return outStr;
        }
        public static string UncodeToChinese(string uncode)
        {
            string outStr = "";
            try
            {
                if (!string.IsNullOrEmpty(uncode))
                {
                    string[] strlist = uncode.Replace("//", "").Split('u');
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("UncodeToChinese ex:", ex.Message, ",uncode:", uncode);
            }
            return outStr;
        }

    }
}
