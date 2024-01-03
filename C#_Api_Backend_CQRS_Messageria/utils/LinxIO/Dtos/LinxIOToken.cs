using System;

namespace LinxIO.Dtos
{
    public class LinxIOToken
    {
        public string Error { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}