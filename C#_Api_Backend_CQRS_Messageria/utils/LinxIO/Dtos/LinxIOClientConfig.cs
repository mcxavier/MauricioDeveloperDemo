using LinxIO.Interfaces;

namespace LinxIO.Dtos
{
    public class LinxIOClientConfig : ILinxIOClientConfig
    {
        public string ApplicationId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}