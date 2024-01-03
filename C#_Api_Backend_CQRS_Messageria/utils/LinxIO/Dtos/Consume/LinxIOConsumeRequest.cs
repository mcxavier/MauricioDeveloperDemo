namespace LinxIO.Dtos
{
    public class LinxIOConsumeRequest
    {
        public System.Guid CompanyId { get; set; }
        public LinxIOTopics Topics { get; set; }
    }
}
