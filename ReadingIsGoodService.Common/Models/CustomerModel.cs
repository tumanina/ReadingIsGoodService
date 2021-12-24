using System;
using System.Collections.Generic;

namespace ReadingIsGoodService.Common.Models
{
    public class CustomerModel : BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<OrderModel> Orders { get; set; }
    }
}