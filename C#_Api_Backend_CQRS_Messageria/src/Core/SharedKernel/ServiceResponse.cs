using System.Collections.Generic;

namespace Core.SharedKernel
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string AuthenticationUrl { get; set; }
        public string QRCode { get; set; }
        public string QRCodeUrl { get; set; }
        public T ResponseData { get; set; }
        public List<string> Errors { get; set; }
    }
}