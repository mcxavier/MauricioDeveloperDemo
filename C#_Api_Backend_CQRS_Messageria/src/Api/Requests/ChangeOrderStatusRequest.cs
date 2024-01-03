namespace Api.Requests
{
    public class ChangeOrderStatusRequest
    {
        public int[] OrderIds { get; set; }
        public int NewStatus { get; set; }
    }
}
