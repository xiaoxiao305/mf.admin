using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MF.ConsoleApp
{
   public class Cache
    {
        /// <summary>
        /// {account:Users}
        /// </summary>
        public static Dictionary<string, Data.Users> CacheAccountList = new Dictionary<string, Data.Users>();
        /// <summary>
        /// {chargeid:Users}
        /// </summary>
        public static Dictionary<string, Data.Users> CacheChargeidList = new Dictionary<string, Data.Users>();
    }
}
