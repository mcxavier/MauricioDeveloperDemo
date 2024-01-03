using Core.SeedWork;

namespace Core.Models.Core.Ordering
{

    public class OrderStatus : Enumeration
    {
        public OrderStatus(int id, string name) : base(id, name) { }
        public static OrderStatus Submitted = new OrderStatus(1, "Pedido realizado");
        public static OrderStatus AwaitingValidation = new OrderStatus(2, "Aguardando validação");
        public static OrderStatus Paid = new OrderStatus(3, "Pedido pago");
        public static OrderStatus StockConfirmed = new OrderStatus(4, "Separação em estoque");
        public static OrderStatus Shipped = new OrderStatus(5, "Saída para entrega");
        public static OrderStatus Cancelled = new OrderStatus(6, "Cancelado");
        public static OrderStatus PaymentFailed = new OrderStatus(7, "Falha no pagamento");
        public static OrderStatus IntegrationSuccess = new OrderStatus(8, "Pedido Confirmado");
        public static OrderStatus IntegrationFailed = new OrderStatus(9, "Falha na Integração");
        public static OrderStatus Integrated = new OrderStatus(10, "Pedido Integrado");
        public static OrderStatus Completed = new OrderStatus(11, "Pedido finalizado");
        public static OrderStatus Invoiced = new OrderStatus(12, "Pedido faturado");
    }

    public enum OrderStatusEnum
    {
        Submitted = 1,
        AwaitingValidation = 2,
        Paid = 3,
        StockConfirmed = 4,
        Shipped = 5,
        Cancelled = 6,
        PaymentFailed = 7,
        Confirmed = 8,
        IntegrationFailed = 9,
        IntegrationSuccess = 10,
        Completed = 11,
        Invoiced = 12
    }
}