using System.Net.Mail;
using System.Net;

namespace GlobalSparkAcademy.Services.EmailSender
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEnrollmentConfirmationEmail(string toEmail, string fullName, string strandName, string? yearLevel, string? semester)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            using (var client = new SmtpClient(smtpServer, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username),
                    Subject = "Enrollment Confirmation",
                    Body = $@"
                <html>
                    <body>
                        <p>Dear {fullName},</p>
                        <p>Thank you for enrolling in the <strong>{strandName}</strong> program for Year Level: <strong>{yearLevel ?? "N/A"}</strong> and Semester: <strong>{semester ?? "N/A"}</strong>.</p>
                        <p>We are excited to have you with us!</p>
                        <img src='https://media.tenor.com/fakuspGm_0oAAAAM/maraming-salamat-gyl.gif' alt='Welcome GIF' style='width:300px;height:auto;'/>
                        <p>Best regards,<br/>Global Spark Academy</p>
                    </body>
                </html>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }

    }
}
