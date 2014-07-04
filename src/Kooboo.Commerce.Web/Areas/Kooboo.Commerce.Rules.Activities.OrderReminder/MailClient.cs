using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Rules.Activities.OrderReminder
{
    public static class MailClient
    {
        public static void Send(MailInfo mail)
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/MailBox");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var path = Path.Combine(directory, SubjectToFileName(mail.Subject) + ".html");
            File.WriteAllText(path, mail.Body, Encoding.UTF8);
        }

        static string SubjectToFileName(string subject)
        {
            if (String.IsNullOrEmpty(subject))
            {
                return "No suject";
            }

            foreach (var ch in Path.GetInvalidFileNameChars())
            {
                subject = subject.Replace(ch, '_');
            }

            return subject;
        }
    }
}