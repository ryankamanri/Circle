using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net; 
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using System.Threading;
 
namespace Kamanri.Self
{
    public static class EmailHelper 
    {
        private static MailMessage mailMessage;
        private static SmtpClient smtpClient;
        
        /// <summary>
        /// 多线程发送邮件
        /// </summary>
        /// <param name="emailto">目标地址</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        public static void SendThread(string emailto, string title, string body)
        {
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(ThreadMethod_EmailTo);
            Thread myThread = new Thread(ParStart);
            object o = title + "|" + body + "|" + emailto;
            myThread.Start(o);
        }
        private static void ThreadMethod_EmailTo(object ParObject)
        {
            try
            {
                string emailto = "974481066@qq.com";//邮箱地址
                string pwd = "olzfxggqzadlbajh";//邮箱密码
                string host = "smtp.qq.com";
                string[] str = ParObject.ToString().Split('|');
                string[] strArray = str[2].Split(',');
                foreach (string s in strArray)
                {
                   
                    mailMessage = new MailMessage();
                    mailMessage.To.Add(s);
                    mailMessage.From = new System.Net.Mail.MailAddress(emailto);
                    mailMessage.Subject = str[0];
                    mailMessage.Body = str[1];
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
 
                    smtpClient = new SmtpClient();
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, pwd);//设置发件人身份的票据  
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtpClient.Host = host;
                    smtpClient.Send(mailMessage);
                }
            }
            catch { }
        }
 
    }
}
