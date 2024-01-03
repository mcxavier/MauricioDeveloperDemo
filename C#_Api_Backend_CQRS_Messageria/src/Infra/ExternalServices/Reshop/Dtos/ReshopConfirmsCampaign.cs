using System;

namespace Infra.ExternalServices.Reshop.Dtos
{
    public class ReshopConfirmsCampaign
    {
        public bool Resultado { get; set; }
        public string Mensagem { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public bool IsExeption { get; set; }
        public bool Offline { get; set; }
        public Object MessagesValidation { get; set; }
    }
}
