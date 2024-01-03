using Core.SharedKernel;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Domains.Topics
{
    public class PublishApplicationFeedBack : IRequest<Response>
    {
        public FeedBackAdditionalInfo additionalInfo { get; set; } 
        public string applicationId { get; set; }
        public DateTime createdAt { get; set; }
        public string entityId { get; set; }
        public string message { get; set; }
        public string referenceMessageId { get; set; }
        public string topicId { get; set; }
        public FeedbackContent FeedbackContent { get; set; }
        public string Type { get; set; }  // SUCCESS: Indica que a mensagem foi processada com sucesso pelo sistema consumidor.
                                          // ERROR:  Mensagem foi processada e devido alguma errp/impedimento não pode ser concluída com sucesso.
    }

    public class FeedBackAdditionalInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FeedBackException exception { get; set; }
    }


    public class FeedBackException
    {
        public string description { get; set; }
        public string name { get; set; }
    }

    public enum FeedBackType
    {
        SUCCESS = 1,
        ERROR = 2
    }
}
