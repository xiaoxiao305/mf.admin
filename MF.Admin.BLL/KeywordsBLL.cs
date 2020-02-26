using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace MF.Admin.BLL
{
    public class ExtendKeywords
    {
        public int id { get; set; }
        public string name { get; set; }
        public int state { get; set; }
    }
    public class KeywordsBLL
    {
        public static List<ExtendKeywords> GetKeywordsList()
        {
            try
            {
                string json = MF.Admin.DAL.BaseDAL.LoadFile("gamekeywords.config");
                List<ExtendKeywords> list = MF.Common.Json.Json.DeserializeObject<List<ExtendKeywords>>(json);
                return list.OrderBy(s => s.id).ToList<ExtendKeywords>();
            }
            catch (Exception ex)
            {
                Base.WriteError("GetKeywordsList ex:", ex.Message);
                return null;
            }
        }
        public static int SetKeywords(int id, int state)
        {
            try
            {
                List<ExtendKeywords> list = GetKeywordsList();
                if (list == null || list.Count < 1)
                    return -1;
                ExtendKeywords model = list.First(x => x.id == id);
                if (model == null)
                    return -2;
                model.state = state;
                list.Remove(list.First(x => x.id == id));
                list.Add(model);
                string json = MF.Common.Json.Json.SerializeObject(list);
                File.WriteAllText("gamekeywords.config", json, Encoding.UTF8);
                return 1;
            }
            catch (Exception ex)
            {
                Base.WriteError("SetKeywords ex:", ex.Message);
                return 0;
            }
        }
    }
}
