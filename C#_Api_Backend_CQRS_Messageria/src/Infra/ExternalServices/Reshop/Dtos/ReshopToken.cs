using System;

namespace Infra.ExternalServices.Reshop.Dtos
{
    public class ReshopToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
        public int UId { get; set; }
        public int TenantId { get; set; }
        public string UserEmail { get; set; }
        public DateTime IssuedDateTime { get; set; }
        public DateTime ExpiresDateTime { get; set; }
        public Uri BaseAddress;
    }
}
