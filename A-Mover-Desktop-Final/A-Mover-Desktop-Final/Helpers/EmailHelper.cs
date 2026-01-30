using System.Net;
using System.Net.Mail;

public static class EmailHelper
{
    public static async Task SendAsync(
        string host, int port, bool enableSsl,
        string username, string password,
        string fromEmail, string fromName,
        string toEmail, string subject, string body)
    {
        using var client = new SmtpClient(host, port)
        {
            EnableSsl = enableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(username.Trim(), password.Trim())
        };

        // importante: em alguns casos ajuda
        client.Timeout = 100000;


        using var msg = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        msg.To.Add(toEmail);

        await client.SendMailAsync(msg);
    }
}
