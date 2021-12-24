using Microsoft.EntityFrameworkCore;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly ReadingIsGoodDbContext _dbContext;

        public CustomerRepository(ReadingIsGoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateCustomer(CustomerModel customer)
        {
            //todo: move mapping to mapper
            var entity = new CustomerEntity
            {
                Name = customer.Name,
                Email = customer.Email
            };

            entity.UpdatedDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.UtcNow;

            _dbContext.Customers.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            var customers = await _dbContext.Customers.ToListAsync();

            //todo: move mapping to mapper
            return customers.Select(c => new CustomerModel
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Orders = c.Orders.Select(r => new OrderModel
                {
                    Status = r.Status,
                    Items = r.Items.Select(i => new OrderItemModel
                    {
                        Product = new ProductModel { Name = i.Product.Name, Price = i.Product.Price },
                        Quantity = i.Count
                    }).ToList(),
                    CreatedDate = r.CreatedDate
                }).ToList()
            });
        }
    }
}