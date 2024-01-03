namespace Core.QuerysCommands.Commands.Orders.ChangeSeller
{
    public class ChangeSellerResponse
    {
        public bool IsSuccess { get; }
        public int OrderId { get; }
        public int? NewSellerId { get; }
        public ChangeSellerResponse(bool isSuccess, int orderId, int? newSellerId)
        {
            IsSuccess = isSuccess;
            OrderId = orderId;
            NewSellerId = newSellerId;
        }
    }
}