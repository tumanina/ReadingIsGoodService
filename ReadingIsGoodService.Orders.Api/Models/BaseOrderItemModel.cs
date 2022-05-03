namespace ReadingIsGoodService.OrdersApi.Models
{
    public abstract class BaseOrderItemModel
    {
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
