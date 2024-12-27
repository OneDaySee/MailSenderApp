using MailSenderApp.Models;
using System.Net;
using System.Net.Mail;

namespace MailSenderApp.Helpers
{
    public static class MailHelper
    {
        public static string MailSend(MailSendDto mailModel, MailSettings settings)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(settings.SmtpUsername, settings.SenderName),
                    Subject = mailModel.Subject,
                    Body = mailModel.Body,
                    IsBodyHtml = true
                };

                foreach (var recipient in mailModel.ToMailAddress.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(recipient))
                    {
                        message.To.Add(recipient);
                    }
                }

                using (SmtpClient client = new SmtpClient(settings.SmtpServer, settings.SmtpPort))
                {
                    client.EnableSsl = settings.EnableSsl;
                    client.UseDefaultCredentials = settings.UseDefaultCredentials;
                    client.Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword);

                    client.Timeout = 5000;

                    client.Send(message);
                }

                return "Mail başarıyla gönderildi!";
            }
            catch (SmtpException ex) when (ex.InnerException is TimeoutException || ex.Message.Contains("timed out"))
            {
                return "Mail gönderimi başarısız: Zaman aşımı. Kombinasyon denemesi için sonraki ayara geçiliyor.";
            }
            catch (Exception ex)
            {
                return $"Mail gönderimi başarısız: {ex.Message}";
            }
        }
    }
}
