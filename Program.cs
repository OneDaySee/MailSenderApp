using System;
using System.Collections.Generic;
using System.Text.Json;
using MailSenderApp.Helpers;
using MailSenderApp.Models;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hangi yöntemi kullanmak istersiniz?");
        Console.WriteLine("\n\n1. Manuel Giriş - SMTP ayarlarını ve mail bilgilerini kendiniz manuel olarak girerek mail gönderme işlemini başlatabilirsiniz. - Eğer SMTP ayarlarından ve bilgilerinizin doğruluğundan eminseniz '1. Manuel Giriş' seçeneğini seçin.");
        Console.WriteLine("\n\n2. Kombinasyon Denemesi - SMTP ayarlarından bazılarını doğru bilmediğinizde, olası ayar kombinasyonlarını deneyerek doğru ayarı bulmamıza izin verin. - Eğer SMTP ayarları hakkında emin değilseniz ve doğru kombinasyonu bulmak istiyorsanız '2. Kombinasyon Denemesi' seçeneğini seçin. (Yanlış kombinasyonda 5 saniye beklenecek.)");
        Console.Write("\n\nSeçiminizi yapın (1/2): ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            ManualInput();
        }
        else if (choice == "2")
        {
            CombinationTesting();
        }
        else
        {
            Console.WriteLine("Geçersiz seçim.");
        }
    }

    static void ManualInput()
    {
        Console.WriteLine("SMTP Ayarlarını Giriniz:");

        Console.Write("SMTP Server: ");
        string smtpServer = Console.ReadLine();

        Console.Write("SMTP Port: ");
        int smtpPort = int.Parse(Console.ReadLine());

        Console.Write("SMTP Kullanıcı Adı: ");
        string smtpUsername = Console.ReadLine();

        Console.Write("SMTP Şifresi: ");
        string smtpPassword = Console.ReadLine();

        Console.Write("Gönderen İsmi: ");
        string senderName = Console.ReadLine();

        Console.Write("SSL Kullanılsın mı? (true/false): ");
        bool enableSsl = bool.Parse(Console.ReadLine());

        Console.Write("Varsayılan Kimlik Doğrulama Kullanılsın mı? (true/false): ");
        bool useDefaultCredentials = bool.Parse(Console.ReadLine());

        var mailSettings = new MailSettings
        {
            SmtpServer = smtpServer,
            SmtpPort = smtpPort,
            SmtpUsername = smtpUsername,
            SmtpPassword = smtpPassword,
            SenderName = senderName,
            EnableSsl = enableSsl,
            UseDefaultCredentials = useDefaultCredentials
        };

        Console.WriteLine("\nMail Bilgilerini Giriniz:");

        Console.Write("Alıcı Email Adresi: ");
        string toMail = Console.ReadLine();

        Console.Write("Mail Konusu: ");
        string subject = Console.ReadLine();

        Console.Write("Mail İçeriği: ");
        string body = Console.ReadLine();

        var mailSendDto = new MailSendDto
        {
            ToMailAddress = toMail,
            Subject = subject,
            Body = body
        };

        Console.WriteLine("\nMail Gönderiliyor...");
        string result = MailHelper.MailSend(mailSendDto, mailSettings);
        Console.WriteLine(result);
    }

    static void CombinationTesting()
    {
        Console.Write("SMTP Server: ");
        string smtpServer = Console.ReadLine();

        Console.Write("SMTP Kullanıcı Adı: ");
        string smtpUsername = Console.ReadLine();

        Console.Write("SMTP Şifresi: ");
        string smtpPassword = Console.ReadLine();

        Console.Write("Gönderen İsmi: ");
        string senderName = Console.ReadLine();

        Console.Write("Alıcı Email Adresi: ");
        string toMail = Console.ReadLine();

        Console.Write("Mail Konusu: ");
        string subject = Console.ReadLine();

        Console.Write("Mail İçeriği: ");
        string body = Console.ReadLine();

        Console.Write("SMTP Port'u kesin biliyormusunuz? (evet/hayır): ");
        string portAnswer = Console.ReadLine();

        List<int> ports = new List<int>();
        if (portAnswer.ToLower() == "evet")
        {
            Console.Write("SMTP Port: ");
            ports.Add(int.Parse(Console.ReadLine()));
        }
        else
        {
            ports.AddRange(new List<int> { 465, 587 });
        }

        Console.Write("SSL'i kesin biliyormusunuz? (evet/hayır): ");
        string sslAnswer = Console.ReadLine();

        List<bool> sslOptions = new List<bool>();
        if (sslAnswer.ToLower() == "evet")
        {
            Console.Write("SSL Kullanılsın mı? (true/false): ");
            sslOptions.Add(bool.Parse(Console.ReadLine()));
        }
        else
        {
            sslOptions.AddRange(new List<bool> { true, false });
        }

        Console.Write("UseDefaultCredentials kesin biliyormusunuz? (evet/hayır): ");
        string credentialsAnswer = Console.ReadLine();

        List<bool> credentialsOptions = new List<bool>();
        if (credentialsAnswer.ToLower() == "evet")
        {
            Console.Write("UseDefaultCredentials (true/false): ");
            credentialsOptions.Add(bool.Parse(Console.ReadLine()));
        }
        else
        {
            credentialsOptions.AddRange(new List<bool> { true, false });
        }

        var mailSendDto = new MailSendDto
        {
            ToMailAddress = toMail,
            Subject = subject,
            Body = body
        };

        foreach (var port in ports)
        {
            foreach (var ssl in sslOptions)
            {
                foreach (var useDefaultCredentials in credentialsOptions)
                {
                    Console.WriteLine($"Deneme yapılıyor: Port={port}, SSL={ssl}, UseDefaultCredentials={useDefaultCredentials}");
                    var mailSettings = new MailSettings
                    {
                        SmtpServer = smtpServer,
                        SmtpPort = port,
                        SmtpUsername = smtpUsername,
                        SmtpPassword = smtpPassword,
                        SenderName = senderName,
                        EnableSsl = ssl,
                        UseDefaultCredentials = useDefaultCredentials
                    };

                    string result = MailHelper.MailSend(mailSendDto, mailSettings);

                    if (result.Contains("Mail başarıyla gönderildi"))
                    {
                        Console.WriteLine("Çalışan kombinasyon bulundu:");
                        var workingSettings = new
                        {
                            SmtpServer = smtpServer,
                            SmtpPort = port,
                            EnableSsl = ssl,
                            UseDefaultCredentials = useDefaultCredentials,
                            SmtpUsername = smtpUsername,
                            SenderName = senderName
                        };

                        string jsonResult = JsonSerializer.Serialize(workingSettings, new JsonSerializerOptions { WriteIndented = true });
                        Console.WriteLine(jsonResult);
                        return;
                    }
                    else
                    {
                        Console.WriteLine(result);
                    }
                }
            }
        }

        Console.WriteLine("Uygun bir kombinasyon bulunamadı.");
    }
}
