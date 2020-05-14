using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web.Configuration;

namespace MF.Protocol
{
    public class Email
    {
        /// <summary>
        /// 校验邮箱格式是否正确
        /// </summary>
        /// <param name="mail">邮箱</param>
        /// <returns></returns>
        static bool checkMail(string mail)
        {
            var arr = mail.Split('@');
            if (arr.Length > 2 || arr.Length <= 1) return false;
            var domain = arr[1].Split('.');
            if (domain.Length <= 1 || domain.Length > 3) return false;
            return true;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toUser">收件人</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <returns></returns>
        public static bool Send(string toUser, string title, string body)
        {
            if (!checkMail(toUser)) return false;
            if (Base.IsDebug)
            {
                Base.WriteDebug("post to send email is ok.email:", toUser, ",msg:", body);
                return true;
            }
            try
            {
                var client = new SmtpClient
                {
                    Host = Base.mail.Smtp,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Base.mail.Account, Base.mail.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = Base.mail.Port
                };
                var mail = new MailMessage(Base.mail.From, toUser, title, body);
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Base.WriteError("发送邮件到", toUser, "异常：", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toUser">收件人</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="attachStream">附件</param>
        /// <param name="attachName">附件名称</param>
        /// <returns></returns>
        public static int Send(string toUser, string title, string body, Stream attachStream, string attachName)
        {
            int flag = 0;
            if (!checkMail(toUser)) return 10;

            try
            {
                
                var client = new SmtpClient
                {
                    Host = Base.mail.Smtp,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Base.mail.Account, Base.mail.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = Base.mail.Port
                };
                var mail = new MailMessage(Base.mail.From, toUser, title, body);
                mail.Attachments.Add(new Attachment(attachStream, attachName));
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                client.Send(mail);
                flag = 1;
            }
            catch (Exception ex)
            {
                flag = 11;
                Base.WriteError("发送邮件到", toUser, "失败 ,Title:", title, " 附件：", attachName, " Error:", ex.Message);
            }
            return flag;
        }
        /// <summary>
        /// 群发邮件
        /// </summary>
        /// <param name="toList">收件人</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <returns></returns>
        public static bool Send(string[] toList, string title, string body)
        {
            if (toList == null || toList.Length == 0)
                return false;
            var list = new List<string>();
            foreach (var m in toList)
            {
                if (checkMail(m)) list.Add(m);
            }
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Host = Base.mail.Smtp,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Base.mail.Account, Base.mail.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = Base.mail.Port
                };
                var mail = new MailMessage(Base.mail.From, list[0]);
                for (int i = 1; i < list.Count; i++)
                    mail.To.Add(list[i]);
                mail.Subject = title;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Base.WriteError("群发邮件失败 ,Title:", title, " Error:", ex.Message);
            }
            return false;
        }
    }
}
