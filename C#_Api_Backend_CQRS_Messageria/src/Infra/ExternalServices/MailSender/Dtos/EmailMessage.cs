using System.Collections.Generic;
using System.Net.Mail;

namespace Infra.ExternalServices.MailSender.Dtos
{

    public class EmailMessage
    {

        public string To { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsHtml { get; set; }


        public IList<string> ReplyToList { get; set; }

        public IList<string> CCs { get; set; }

        public IList<string> BCCs { get; set; }

        public IList<Attachment> Attachments { get; set; }


        public EmailMessage()
        {
            this.ReplyToList = new List<string>();

            this.CCs = new List<string>();

            this.BCCs = new List<string>();

            this.Attachments = new List<Attachment>();
        }

    }

}