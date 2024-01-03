namespace Core.QuerysCommands.Commands.Orders.ChangeOrderStatus
{
    public class ChangeOrderStatusResponse
    {
        public bool IsSuccess { get; }
        public int[] OrderIds { get; }
        public int NewStatus { get; }
        public ChangeOrderStatusResponse(bool isSuccess, int[] orderIds, int newStatus)
        {
            IsSuccess = isSuccess;
            OrderIds = orderIds;
            NewStatus = newStatus;
        }
    }
}