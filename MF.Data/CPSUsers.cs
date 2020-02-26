using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MF.Data
{
    /// <summary>
    /// CPSUsers:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CPSUsers
    {
        public CPSUsers()
        { }
        #region Model
        private int _id;
        private string _channel;
        private string _channel_name;
        private int _channel_num;
        private string _idnum;
        private string _device;
        private string _bank_name;
        private string _bank_addr;
        private string _bank_acc;
        private string _business_link;
        private string _telephone;
        private string _email;
        private string _qq;
        private decimal? _percent;
        private int _admin_id;
        private string _protocol;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 渠道码
        /// </summary>
        public string channel
        {
            set { _channel = value; }
            get { return _channel; }
        }
        /// <summary>
        /// 渠道中文名称
        /// </summary>
        public string channel_name
        {
            set { _channel_name = value; }
            get { return _channel_name; }
        }
        /// <summary>
        /// 渠道号
        /// </summary>
        public int channel_num
        {
            set { _channel_num = value; }
            get { return _channel_num; }
        }
        /// <summary>
        /// 合作商证件
        /// </summary>
        public string idnum
        {
            set { _idnum = value; }
            get { return _idnum; }
        }
        /// <summary>
        /// 硬件平台【web、pc、ard、ios】
        /// </summary>
        public string device
        {
            set { _device = value; }
            get { return _device; }
        }
        /// <summary>
        /// 开户行名称
        /// </summary>
        public string bank_name
        {
            set { _bank_name = value; }
            get { return _bank_name; }
        }
        /// <summary>
        /// 开户行地址
        /// </summary>
        public string bank_addr
        {
            set { _bank_addr = value; }
            get { return _bank_addr; }
        }
        /// <summary>
        /// 开户行账号
        /// </summary>
        public string bank_acc
        {
            set { _bank_acc = value; }
            get { return _bank_acc; }
        }
        /// <summary>
        /// 业务联系人
        /// </summary>
        public string business_link
        {
            set { _business_link = value; }
            get { return _business_link; }
        }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string telephone
        {
            set { _telephone = value; }
            get { return _telephone; }
        }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 联系人QQ
        /// </summary>
        public string qq
        {
            set { _qq = value; }
            get { return _qq; }
        }
        /// <summary>
        /// 分红比例
        /// </summary>
        public decimal? percent
        {
            set { _percent = value; }
            get { return _percent; }
        }
        /// <summary>
        /// 关联渠道登录账号id
        /// </summary>
        public int admin_id
        {
            set { _admin_id = value; }
            get { return _admin_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string protocol
        {
            set { _protocol = value; }
            get { return _protocol; }
        }
        #endregion Model
    }
    /// <summary>
    /// CPSUsers:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CPSUsersAdmin
    {
        public CPSUsersAdmin()
        { }
        #region Model
        private int _id;
        private string _channel;
        private string _channel_name;
        private string _idnum;
        private string _device;
        private string _bank_name;
        private string _bank_addr;
        private string _bank_acc;
        private string _business_link;
        private string _telephone;
        private string _email;
        private string _qq;
        private decimal? _percent;
        private int _admin_id;
        private string _protocol;
        private int _channel_num;

        private string _admin_account;
        private string _admin_password;
        private int _admin_flag;
        private string _admin_name;
        private string _admin_token;

        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 渠道码
        /// </summary>
        public string channel
        {
            set { _channel = value; }
            get { return _channel; }
        }
        /// <summary>
        /// 渠道中文名称
        /// </summary>
        public string channel_name
        {
            set { _channel_name = value; }
            get { return _channel_name; }
        }
        /// <summary>
        /// 合作商证件
        /// </summary>
        public string idnum
        {
            set { _idnum = value; }
            get { return _idnum; }
        }
        /// <summary>
        /// 硬件平台【web、pc、ard、ios】
        /// </summary>
        public string device
        {
            set { _device = value; }
            get { return _device; }
        }
        /// <summary>
        /// 开户行名称
        /// </summary>
        public string bank_name
        {
            set { _bank_name = value; }
            get { return _bank_name; }
        }
        /// <summary>
        /// 开户行地址
        /// </summary>
        public string bank_addr
        {
            set { _bank_addr = value; }
            get { return _bank_addr; }
        }
        /// <summary>
        /// 开户行账号
        /// </summary>
        public string bank_acc
        {
            set { _bank_acc = value; }
            get { return _bank_acc; }
        }
        /// <summary>
        /// 业务联系人
        /// </summary>
        public string business_link
        {
            set { _business_link = value; }
            get { return _business_link; }
        }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string telephone
        {
            set { _telephone = value; }
            get { return _telephone; }
        }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 联系人QQ
        /// </summary>
        public string qq
        {
            set { _qq = value; }
            get { return _qq; }
        }
        /// <summary>
        /// 分红比例
        /// </summary>
        public decimal? percent
        {
            set { _percent = value; }
            get { return _percent; }
        }
        /// <summary>
        /// 关联渠道登录账号id
        /// </summary>
        public int admin_id
        {
            set { _admin_id = value; }
            get { return _admin_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string protocol
        {
            set { _protocol = value; }
            get { return _protocol; }
        }
        /// <summary>
        /// 渠道号
        /// </summary>
        public int channel_num
        {
            set { _channel_num = value; }
            get { return _channel_num; }
        }


        public string admin_account
        {
            set { _admin_account = value; }
            get { return _admin_account; }
        }
        public string admin_password
        {
            set { _admin_password = value; }
            get { return _admin_password; }
        }
        public int admin_flag
        {
            set { _admin_flag = value; }
            get { return _admin_flag; }
        }
        public string admin_name
        {
            set { _admin_name = value; }
            get { return _admin_name; }
        }
        public string admin_token
        {
            set { _admin_token = value; }
            get { return _admin_token; }
        }
        #endregion Model
    }
}
