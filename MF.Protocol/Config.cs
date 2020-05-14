namespace MF.Protocol
{
    public class Config
    {
        /// <summary>
        /// web网站http连接到服务器时需要的key
        /// </summary>
        public string WebServerKey { get; set; }
        /// <summary>
        /// web网站http连接到服务器时需要的url
        /// </summary>
        public string WebServerUrl { get; set; }
        /// <summary>
        /// 充值网站http连接到充值服务器时需要的key
        /// </summary>
        public string ChargeServerKey { get; set; }
        /// <summary>
        /// 充值网站http连接到充值服务器时需要的url
        /// </summary>
        public string ChargeServerUrl { get; set; }
        /// <summary>
        /// 是否为调试版本
        /// </summary>
        public bool IsDebug { get; set; }
    }
    internal class MailConfig
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
    }
}
