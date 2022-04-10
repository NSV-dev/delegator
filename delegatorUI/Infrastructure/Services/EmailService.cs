using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.Infrastructure.Services
{
    public static class EmailService
    {
        public async static void SendEmail(string toEmail, string toUserName,
            string fromUserName, string taskTitle, 
            string taskReport, string taskDuration,
            List<OpenFileDialog> attachments)
        {
            SmtpClient client = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "delegatorteam@gmail.com",
                    Password = "***"
                }
            };

            MailMessage msg = new()
            {
                From = new MailAddress("delegatorteam@gmail.com", "delegator Team"),
                Subject = $"Отчет о выполнении {taskTitle}",
                Body = $" {fromUserName} выпонил {taskTitle} за {taskDuration} часов\n\nТекст отчета:\n{taskReport}"
            };
            msg.To.Add(new MailAddress(toEmail, toUserName));
            attachments.ForEach(a => msg.Attachments.Add(new Attachment(a.FileName)));

            await client.SendMailAsync(msg);
            client.Dispose();
        }
    }
}
