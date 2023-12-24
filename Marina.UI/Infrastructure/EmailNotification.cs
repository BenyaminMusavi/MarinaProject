using Marina.UI.Infrastructure;
using System.Net.Mail;
using System.Net;

namespace Marina.UI.Infrastructure;

public class EmailNotification : INotification
{
    public void Send(string body, string email)
    {
        //using (MailMessage mail = new MailMessage())
        //{
        //    mail.From = new MailAddress("beni97d@@gmail.com");
        //    mail.To.Add(email);
        //    mail.Subject = "Subject";
        //    mail.Body = body;
        //    mail.IsBodyHtml = true;
        //    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
        //    {
        //        smtp.Credentials = new NetworkCredential("ItMarinaImport@gmail.com", "It_marina");
        //        smtp.EnableSsl = true;
        //        smtp.Send(mail);
        //    }
        //}


        var username = "itmarinaimport@gmail.com";
        var password = "kaob lruv eiwb isjd";
        var date = DateTime.Now.AddDays(-1);
        MailMessage mailMessage = new MailMessage();
        mailMessage.Subject = $"لیست نفراتی که در تاریخ {date} دیتایی ایمپورت نکرده اند ";
        mailMessage.To.Add(email);
        mailMessage.Body = body;
        mailMessage.From = new MailAddress(username);

        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        //smtpClient.Port = 587;
        smtpClient.Port = 587;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(username, password);

        try
        {
            smtpClient.Send(mailMessage);
            Console.WriteLine("Email Sent Successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }




    }
}


