using HotelListingAPI.API.Models;
using HotelListingAPI.Service.Common.HomeService;
using System.Net;
using System.Net.Mail;

namespace HotelListingAPI.Service.HomeService
{
    public class HomeService : IHomeService
    {
        private readonly ILogger<HomeService> _logger;

        public HomeService(ILogger<HomeService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method used for sending the email to the address provided in case if someone has a question or want to report something.
        /// </summary>
        /// <param name="contactMeDto">Object that will contain all information needed for sending the mail, for example: Sender Email, Name and Message</param>
        /// <returns></returns>
        public async Task ContactMe(ContactMe contactMeDto)
        {
            _logger.LogInformation($"(Service) {nameof(ContactMe)}");

            var mail = new MailMessage();

            mail.To.Add(new MailAddress("my email"));
            mail.From = new MailAddress(contactMeDto.SenderEmail);
            mail.Subject = "Your Email Subject";
            mail.Body = string.Format("<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>", contactMeDto.SenderEmail, contactMeDto.SenderName, contactMeDto.Message);
            mail.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("my email", "my password");
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
