using MF.Admin.DAL;
using MF.Data.ExtendChannel;
using RedisHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Admin.BLL
{
    public class ChannelBLL
    {
        static RedisHelper redis = new RedisHelper(0);
        /// <summary>
        /// 统计时间内redis渠道汇总数据
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        /// <returns>返回时间内推广横向统计数据</returns>
        public static List<ExtendChannel> CountRedisData(DateTime time)
        {
            try
            {
                List<string> hash_keys = GetRedisKeyConst(time);
                if (hash_keys == null || hash_keys.Count < 1)
                    return null;
                Dictionary<string, List<ExtendChannelRecord>> each_key_dic = new Dictionary<string, List<ExtendChannelRecord>>();
                List<ExtendChannelRecord> value_list =new List<ExtendChannelRecord>();
                List<ExtendChannelRecord> value_list_temp =new List<ExtendChannelRecord> ();
                string[] hash_key_split;
                foreach (string hash_key in hash_keys)
                {
                    if (redis.KeyExists(hash_key))
                    {
                        value_list_temp =  redis.HashGetAll<ExtendChannelRecord>(hash_key);
                        if (value_list_temp != null && value_list_temp.Count > 0)
                        {
                            each_key_dic.Add(hash_key, value_list_temp);
                            foreach (ExtendChannelRecord value_list_model in value_list_temp)
                            {
                                hash_key_split = hash_key.Split('_');
                                value_list_model.Day = BaseDAL.ConvertDateToSpan(time, "d");
                                value_list_model.Channel = hash_key_split[1];
                                value_list_model.ChannelNum = hash_key_split[2];
                                value_list_model.Device = hash_key_split[3];
                                value_list.Add(value_list_model);
                            }
                        }
                    }
                } 
                //统计横向3端总和数量[PC,iOS,Android]
                Dictionary<string, ExtendChannel> list_dic = new Dictionary<string, ExtendChannel>();
                ExtendChannel ec = null;
                foreach (ExtendChannelRecord valmodel in value_list)
                {
                    string lastkey = string.Format("{0}_{1}_{2}", valmodel.Day, valmodel.Channel, valmodel.ChannelNum);
                    if (list_dic != null && list_dic.ContainsKey(lastkey))
                        ec = list_dic[lastkey];
                    else
                        ec = new ExtendChannel() { Day=valmodel.Day,Channel=valmodel.Channel,ChannelNum=valmodel.ChannelNum};
                    switch (valmodel.Device.ToUpper())
                    {
                        case "PC":
                            ec.PCLoad += valmodel.Load;
                            ec.PCDown += valmodel.Down;
                            ec.PCFirstActive += valmodel.FirstActive;
                            break;
                        case "IOS":
                            ec.iOSLoad += valmodel.Load;
                            ec.iOSDown += valmodel.Down;
                            ec.iOSFirstActive += valmodel.FirstActive;
                            break;
                        case "ANDROID":
                            ec.AndroidLoad += valmodel.Load;
                            ec.AndroidDown += valmodel.Down;
                            ec.AndroidFirstActive += valmodel.FirstActive;
                            break;
                    }
                    ec.LoadTime += valmodel.LoadTime;
                    ec.SecondDownTime += valmodel.SecondDownTime;
                    ec.Stay += valmodel.Stay;
                    ec.SecondDown += valmodel.SecondDown;
                    ec.Register += valmodel.Register;
                    ec.NetWifi += valmodel.NetWifi;
                    ec.NetMobileData += valmodel.NetMobileData;
                    if (ec.PCLoad + ec.PCDown + ec.PCFirstActive > 0)
                        ec.LoadTimeAvg = ec.LoadTime / (ec.PCLoad + ec.PCDown + ec.PCFirstActive);
                    if (ec.SecondDown > 0)
                        ec.SecondDownTimeAvg = ec.SecondDownTime / ec.SecondDown;
                    if (list_dic != null && list_dic.ContainsKey(lastkey))
                        list_dic[lastkey] = ec;
                    else
                        list_dic.Add(lastkey, ec);
                }
                return list_dic.Values.ToList<ExtendChannel>();
            }
            catch (Exception ex)
            {
               Base.WriteError("ChannelBLL CountRedisData ex:", ex.Message);
            }
            return null;
        }


        private static List<string> _redis_channel_keys;
        private static List<string> Redis_Channel_Keys
        {
            set
            {
                _redis_channel_keys = value;
            }
            get
            {
                return _redis_channel_keys;
            }
        }
        private static List<string> GetRedisKeyConst(DateTime time)
        {
            if (Redis_Channel_Keys == null || Redis_Channel_Keys.Count < 1)
            {
                string[] channels = Enum.GetNames(typeof(ChannelEnum));
                string[] channel_nums;
                string[] devices;
                foreach (string channel in channels)
                {
                    channel_nums = Enum.GetNames(typeof(ChannelNumEnum));
                    foreach (string channel_num in channel_nums)
                    {
                        devices = Enum.GetNames(typeof(DeviceEnum));
                        foreach (string device in devices)
                        {
                            if (Redis_Channel_Keys == null)
                                Redis_Channel_Keys = new List<string>();
                            string key = string.Format("{0}_{1}_{2}_{3}", time.ToString("yyyyMMdd"),
                                channel, channel_num, device);
                            Redis_Channel_Keys.Add(key.ToUpper());
                        }
                    }
                }
            }
            return Redis_Channel_Keys;
        }
    }
}
