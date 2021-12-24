namespace ReadingIsGoodService.Common.Models
{
    public class OrderItemModel : BaseModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public OrderModel Order { get; set; }
        public ProductModel Product { get; set; }
    }
}