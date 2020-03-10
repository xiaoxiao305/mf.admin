using MF.Protocol;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.ConsoleApp
{
    public class ClubDAL
    {
        public static string ClubsURI = ConfigurationManager.AppSettings["ClubsURI"];

        public Dictionary<string, ClubDividends> GetClubDividends(string[] clubids)
        {
            //总贡献
            var clubscores = PostHelper.Post<List<Dictionary<string, object>>>("club_dividends", "get", new Dictionary<string, object> { { "clubs", clubids } });
            if (clubscores == null || clubscores.Count < 1) return null;
            float week = 0;
            float lastweek = 0;
            Dictionary<string, ClubDividends> dic = new Dictionary<string, ClubDividends>();

            foreach (Dictionary<string, object> clubscore in clubscores)
            {
                week = 0;
                lastweek = 0;
                ClubDividends model = new ClubDividends();
                if (clubscore["club_id"] == null || string.IsNullOrEmpty(clubscore["club_id"].ToString())) { continue; }
                if (dic.ContainsKey(clubscore["club_id"].ToString())) continue;
                if (clubscore["play_income"] != null && !string.IsNullOrEmpty(clubscore["play_income"].ToString()))
                    //week += float.Parse(clubscore["play_income"].ToString());
                    week += ChangeDataToD(clubscore["play_income"].ToString());
                if (clubscore["room_card_income"] != null && !string.IsNullOrEmpty(clubscore["room_card_income"].ToString()))
                    //week += float.Parse(clubscore["room_card_income"].ToString());
                    week += ChangeDataToD(clubscore["room_card_income"].ToString());
                if (clubscore["last_week_play_income"] != null && !string.IsNullOrEmpty(clubscore["last_week_play_income"].ToString()))
                    //lastweek += float.Parse(clubscore["last_week_play_income"].ToString()); 
                    lastweek += ChangeDataToD(clubscore["last_week_play_income"].ToString());
                if (clubscore["last_week_room_card_income"] != null && !string.IsNullOrEmpty(clubscore["last_week_room_card_income"].ToString()))
                    //lastweek += float.Parse(clubscore["last_week_room_card_income"].ToString()); 
                    lastweek += ChangeDataToD(clubscore["last_week_room_card_income"].ToString());
                model.ClubId = clubscore["club_id"].ToString();
                model.Week = week;
                model.LastWeek = lastweek;
                dic.Add(model.ClubId, model);
            }
            return dic;
        }
        public static float ConvertNum(string numStr)
        {
            float result = -1;
            numStr = numStr.ToUpper();
            if (numStr.Contains("E"))
            {
                double b = double.Parse(numStr.Split('E')[0].ToString());//整数部分
                double c = double.Parse(numStr.Split('E')[1].ToString());//指数部分
                result = (float)(b * Math.Pow(10, c));
            }
            else
            {
                result = (float)double.Parse(numStr);
            }
            return result;
        }
        public static float ChangeDataToD(string numStr)
        {
            float dData = 0.0F;
            numStr = numStr.ToUpper();
            if (numStr.Contains("E"))
            {
                dData = float.Parse(numStr, System.Globalization.NumberStyles.Float);
            }
            else
                dData = float.Parse(numStr);
            return dData;
        }





        public List<ClubsModel> GetAllClubsList()
        {
            try
            {
                string param = "{\"module\":\"club\",\"func\":\"all\",\"args\":{}}";
                ClubsRes<List<ClubsModel>> res = PostHelper.PostClubServer<ClubsRes<List<ClubsModel>>>(ClubsURI, param);
                if (res == null) return null;
                return res.msg;
            }
            catch (Exception ex)
            {
                Log.WriteError("post get_all_club_list ex:", ex.Message);
            }
            return null;
        }
    }
}
