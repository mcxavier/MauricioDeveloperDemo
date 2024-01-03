using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Core.Repositories;
using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.MailSender.Configurations;
using Infra.ExternalServices.MailSender.Dtos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Utils;

namespace Infra.ExternalServices.MailSender
{
    
    public class EmailServices : IEmailServices
    {

        private readonly ILogger<EmailServices> _logger;
        private readonly SmartSalesIdentity _identity;
        private readonly ICompanyRepository _companyRepository;
        private ILinxIOSmtpConfig _config;

        public EmailServices(ILogger<EmailServices> logger,
                                      SmartSalesIdentity identity,
                                      ICompanyRepository companyRepository)
        {
            this._logger = logger;
            this._identity = identity;
            this._companyRepository = companyRepository;

            var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.SmtpConfig).Result;
            this._config = JsonConvert.DeserializeObject<ILinxIOSmtpConfig>(config.Value, JsonSettings.Settings);

        }

        public async Task<bool> SendEmailMessage(EmailMessage message)
        {
            
            this._logger.LogInformation("Sendding email {@mailSettings}\n{@mailMessage}", _config, message);
            try {
                var smtp = new SmtpClient {
                    EnableSsl = _config.Ssl,
                    Host      = _config.Host,
                    Port      = _config.Port,

                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(_config.Username, _config.Password),
                };
                
                using (var smtpMessage = new MailMessage(_config.Username, message.To)) {
                    smtpMessage.From         = new MailAddress(_config.Username);
                    smtpMessage.Subject      = message.Subject;
                    smtpMessage.Body         = message.Body;
                    smtpMessage.BodyEncoding = Encoding.UTF8;
                    smtpMessage.IsBodyHtml   = message.IsHtml;
                    
                    
                    if (message.Attachments.Any()) {
                        foreach (var messageAttachment in message.Attachments) {
                            smtpMessage.Attachments.Add(messageAttachment);
                        }
                    }
                    
                    if (message.ReplyToList.Any()) {
                        foreach (var repplier in message.ReplyToList) {
                            smtpMessage.ReplyToList.Add(repplier);
                        }
                    }
                    
                    if (message.CCs.Any()) {
                        foreach (var repplier in message.CCs) {
                            smtpMessage.CC.Add(repplier);
                        }
                    }
                    
                    if (message.BCCs.Any()) {
                        foreach (var repplier in message.BCCs) {
                            smtpMessage.Bcc.Add(repplier);
                        }
                    }

                    await Task.Run(() => smtp.Send(smtpMessage));
                }
                
                return true;
            } catch (Exception ex) {
                
                this._logger.LogError("Error on mail sender routine {@exception}", ex);

                throw;
            }

        }

    }

}