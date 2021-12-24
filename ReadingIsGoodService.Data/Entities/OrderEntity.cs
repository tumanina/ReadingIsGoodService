using ReadingIsGoodService.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public int CustomerId { get; set; }
        public OrderStatus Status { get; set; }

        public virtual CustomerEntity Customer { get; set; }
        public virtual ICollection<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();
    }
}