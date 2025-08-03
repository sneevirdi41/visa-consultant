using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text;
using visa_consulatant.Models;

namespace visa_consulatant.Services
{
    public interface IEmailService
    {
        Task SendContactInquiryNotificationAsync(ContactInquiry inquiry);
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendContactInquiryNotificationAsync(ContactInquiry inquiry)
        {
            try
            {
                var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
                if (string.IsNullOrEmpty(adminEmail))
                {
                    _logger.LogWarning("Admin email not configured. Skipping email notification.");
                    return;
                }

                var subject = $"New Contact Inquiry from {inquiry.Name}";
                var body = BuildContactInquiryEmailBody(inquiry);

                await SendEmailAsync(adminEmail, subject, body);
                _logger.LogInformation($"Contact inquiry notification sent to {adminEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send contact inquiry notification");
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
                var smtpPortStr = Environment.GetEnvironmentVariable("SMTP_PORT");
                var smtpPort = int.Parse(smtpPortStr ?? "587");
                var username = Environment.GetEnvironmentVariable("SMTP_USERNAME");
                var password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
                var fromEmail = Environment.GetEnvironmentVariable("FROM_EMAIL");
                var fromName = Environment.GetEnvironmentVariable("FROM_NAME") ?? "Visa Consultant";

                if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(username) || 
                    string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fromEmail))
                {
                    _logger.LogWarning("Email settings not properly configured. Skipping email send.");
                    return;
                }

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(fromName, fromEmail));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(username, password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to}");
                throw;
            }
        }

        private string BuildContactInquiryEmailBody(ContactInquiry inquiry)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html><body>");
            sb.AppendLine("<h2>New Contact Inquiry Received</h2>");
            sb.AppendLine("<table style='border-collapse: collapse; width: 100%;'>");
            sb.AppendLine("<tr><td style='padding: 8px; border: 1px solid #ddd; font-weight: bold;'>Name:</td><td style='padding: 8px; border: 1px solid #ddd;'>" + inquiry.Name + "</td></tr>");
            sb.AppendLine("<tr><td style='padding: 8px; border: 1px solid #ddd; font-weight: bold;'>Email:</td><td style='padding: 8px; border: 1px solid #ddd;'>" + inquiry.Email + "</td></tr>");
            sb.AppendLine("<tr><td style='padding: 8px; border: 1px solid #ddd; font-weight: bold;'>Phone:</td><td style='padding: 8px; border: 1px solid #ddd;'>" + inquiry.Phone + "</td></tr>");
            
            if (!string.IsNullOrEmpty(inquiry.Subject))
            {
                sb.AppendLine("<tr><td style='padding: 8px; border: 1px solid #ddd; font-weight: bold;'>Subject:</td><td style='padding: 8px; border: 1px solid #ddd;'>" + inquiry.Subject + "</td></tr>");
            }
            
            if (!string.IsNullOrEmpty(inquiry.VisaType))
            {
                sb.AppendLine("<tr><td style='padding: 8px; border: 1px solid #ddd; font-weight: bold;'>Visa Type:</td><td style='padding: 8px; border: 1px solid #ddd;'>" + inquiry.VisaType + "</td></tr>");
            }
            
            sb.AppendLine("<tr><td style='padding: 8px; border: 1px solid #ddd; font-weight: bold;'>Message:</td><td style='padding: 8px; border: 1px solid #ddd;'>" + inquiry.Message.Replace("\n", "<br>") + "</td></tr>");
            sb.AppendLine("<tr><td style='padding: 8px; border: 1px solid #ddd; font-weight: bold;'>Received:</td><td style='padding: 8px; border: 1px solid #ddd;'>" + inquiry.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss UTC") + "</td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("<br><p><strong>Please respond to this inquiry promptly.</strong></p>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }
    }
} 