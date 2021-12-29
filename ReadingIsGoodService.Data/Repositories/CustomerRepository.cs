using Microsoft.EntityFrameworkCore;
using ReadingIsGoodService.Common.Enums;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Entities;
using ReadingIsGoodService.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    internal class CustomerRepository : BaseRepository<CustomerEntity>, ICustomerRepository
    {
        private readonly ReadingIsGoodDbContext _dbContext;

        public CustomerRepository(ReadingIsGoodDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override EntityType EntityType => EntityType.Customer;

        public override void AddEntity(CustomerEntity entity)
        {
            _dbContext.Customers.Add(entity);
        }

        public async Task<int> CreateCustomer(CustomerModel customer, int userId)
        {
            var entity = new CustomerEntity
            {
                Name = customer.Name,
                Email = customer.Email
            };

            return await Create(entity, userId);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            return (await _dbContext.Customers.ToListAsync()).Select(c => c.Map()).ToList();
        }
    }
}