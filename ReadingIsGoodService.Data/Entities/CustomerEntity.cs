using System.Collections.Generic;

namespace ReadingIsGoodService.Data.Entities
{
    public class CustomerEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }
}