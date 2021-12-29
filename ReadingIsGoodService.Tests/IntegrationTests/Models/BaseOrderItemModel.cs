namespace ReadingIsGoodService.Tests.IntegrationTests.Models
{
    public abstract class BaseOrderItemModel
    {
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
