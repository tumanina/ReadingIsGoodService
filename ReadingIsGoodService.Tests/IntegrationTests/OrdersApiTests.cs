using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ReadingIsGoodService.Tests.IntegrationTests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Tests.IntegrationTests
{
    [TestClass]
    public class OrdersApiTests
    {
        private readonly string _url = "https://localhost:44329/api/v1";
        private readonly string _token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJ1c2VyaWQiOjU3OTN9.mXLWmKSxcuTX_GM5V6e7WeEDA-dofCsr0ZZldkd7v7k";

        [TestMethod]
        public async Task CreateOrder_WithNotExistedProduct_ReturnsError()
        {
            //todo: create and use here client package
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                var order = new CreateOrderModel { CustomerId = 3, OrderItems = new List<OrderItemModel> { new OrderItemModel { ProductId = 1, ProductQuantity = 1 } } };
                var body = new StringContent(JsonConvert.SerializeObject(order), UnicodeEncoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_url}/orders", body);

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<BaseApiDataModel<OrderDetailModel>>(content);

                    Assert.IsFalse(result.Success);
                    Assert.IsTrue(result.Errors.Any());
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public async Task GetCustomerOrders_NoOrders_ReturnsEmptyList()
        {
            //todo: create and use here client package
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response = await client.GetAsync($"{_url}/customers/5/orders");
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<BaseApiDataModel<IEnumerable<OrderDetailModel>>>(content);

                    Assert.IsTrue(result.Success);
                    Assert.IsFalse(result.Errors.Any());
                    Assert.AreEqual(result.Data.Count(), 0);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }
    }
}
