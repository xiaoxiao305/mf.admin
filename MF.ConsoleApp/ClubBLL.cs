using System.Collections.Generic;
using System.Linq; 

namespace MF.ConsoleApp
{
    public class ClubBLL
    {
        private UserDAL userDal = new UserDAL();
        private ClubDAL dal = new ClubDAL();
        public List<ClubsModel> GetAllClubsList()
        {
            List<ClubsModel> list = new ClubDAL().GetAllClubsList();
            if (list == null || list.Count < 1) return null;
            //群主姓名
            string[] chargeids = list.Select(t => t.Founder).ToArray();
            userDal.QueryUserList(chargeids);
            //成员数量
            string[] clubids = list.Select(t => t.Id).ToArray();
            var members = PostHelper.Post<Dictionary<string, object>>("club_member", "members_count", clubids);
            //本周收益
            Dictionary<string, ClubDividends> dicDividends = dal.GetClubDividends(clubids);



            List<ClubsModel> newList = new List<ClubsModel>();
            Data.Users u = null;
            foreach (var item in list)
            {
                u = userDal.GetCacheUserByChargeIdFromCache(item.Founder);
                item.FounderName = u == null ? "" : u.Name;
                item.FounderIdentity = u == null ? "" : u.Identity;
                item.Mobile = u == null ? "" : u.Mobile;
                if (members != null && members.Count > 0 && members.ContainsKey(item.Id.ToString()))
                    item.Members_Count = int.Parse(members[item.Id.ToString()].ToString());
                item.dividends = null;
                if (dicDividends != null && dicDividends.Count > 0 && dicDividends.ContainsKey(item.Id.ToString()))
                {
                    item.dividends = dicDividends[item.Id.ToString()];
                    Log.WriteDebug("GetClubDividends222 dividends:", dicDividends[item.Id.ToString()].Week.ToString());
                }
                newList.Add(item);
            }
            return newList;
        }
    }
}
