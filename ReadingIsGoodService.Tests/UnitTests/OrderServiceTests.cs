using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadingIsGoodService.Data.Repositories;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Common.Enums;
using ReadingIsGoodService.Orders.Logic;

namespace ReadingIsGoodService.Tests.UnitTests
{
    [TestClass]
    public class OrderServiceTests
    {
        private readonly int _customerId = 145;
        private readonly int _userId = 934;
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IProductRepository> _productRepositoryMock = new();

        [TestCleanup]
        public void TestCleanUp()
        {
            _orderRepositoryMock.Invocations.Clear();
            _productRepositoryMock.Invocations.Clear();
        }

        [TestMethod]
        public async Task GetCustomerOrders_OrdersExist_Correct()
        {
            _orderRepositoryMock.Setup(m => m.GetCustomerOrders(_customerId)).Returns(() => Task.FromResult<IEnumerable<OrderModel>>(new List<OrderModel> { new OrderModel { Id = 1 }, new OrderModel { Id = 5 }, new OrderModel { Id = 8 } }));

            var orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object);
            var result = await orderService.GetCustomerOrders(_customerId);

            Assert.AreEqual(result.Count(), 3);
            _orderRepositoryMock.Verify(m => m.GetCustomerOrders(_customerId), Times.Once());
        }

        [TestMethod]
        public async Task GetCustomerOrders_OrdersNotExist_Correct()
        {
            _orderRepositoryMock.Setup(m => m.GetCustomerOrders(_customerId)).Returns(() => Task.FromResult<IEnumerable<OrderModel>>(new List<OrderModel>()));

            var orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object);
            var result = await orderService.GetCustomerOrders(_customerId);

            Assert.AreEqual(result.Count(), 0);
            _orderRepositoryMock.Verify(m => m.GetCustomerOrders(_customerId), Times.Once());
        }

        [TestMethod]
        public async Task CreateOrder_StockIsEnough_DecrementedStock()
        {
            var item1 = new OrderItemModel { ProductId = 1, Quantity = 1 };
            var item2 = new OrderItemModel { ProductId = 4, Quantity = 2 };
            var orderId = 539;
            _orderRepositoryMock.Setup(m => m.CreateOrder(_customerId, It.IsAny<IEnumerable<OrderItemModel>>(), _userId)).Returns(() => Task.FromResult(orderId));
            _orderRepositoryMock.Setup(m => m.GetOrder(orderId)).Returns(() => Task.FromResult(new OrderModel()));
            _productRepositoryMock.Setup(m => m.GetProduct(item1.ProductId)).Returns(() => Task.FromResult(new ProductModel { StockQuantity = 10 }));
            _productRepositoryMock.Setup(m => m.GetProduct(item2.ProductId)).Returns(() => Task.FromResult(new ProductModel { StockQuantity = 20 }));
            _productRepositoryMock.Setup(m => m.StockQuantityDecrement(It.IsAny<int>(), It.IsAny<int>(), _userId));
            _orderRepositoryMock.Setup(m => m.UpdateStatus(It.IsAny<int>(), It.IsAny<OrderStatus>(), _userId));

            var orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object);
            var result = await orderService.CreateOrder(new OrderModel 
            { 
                CustomerId = _customerId, 
                Items = new List<OrderItemModel> { item1, item2 }
            }, _userId);

            Assert.IsTrue(result != null);
            _orderRepositoryMock.Verify(m => m.CreateOrder(_customerId, It.IsAny<IEnumerable<OrderItemModel>>(), _userId), Times.Once());
            _productRepositoryMock.Verify(m => m.GetProduct(item1.ProductId), Times.Once());
            _productRepositoryMock.Verify(m => m.GetProduct(item2.ProductId), Times.Once());
            _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item1.ProductId, item1.Quantity, _userId), Times.Once());
            _productRepositoryMock.Verify(m => m.StockQuantityIncrement(item1.ProductId, item1.Quantity, _userId), Times.Never());
            _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item2.ProductId, item2.Quantity, _userId), Times.Once());
            _productRepositoryMock.Verify(m => m.StockQuantityIncrement(item2.ProductId, item2.Quantity, _userId), Times.Never());
            _orderRepositoryMock.Verify(m => m.UpdateStatus(orderId, OrderStatus.Approved, _userId), Times.Once());
            _orderRepositoryMock.Verify(m => m.UpdateStatus(orderId, OrderStatus.Rejected, _userId), Times.Never());
        }

        [TestMethod]
        public async Task CreateOrder_FirstProductStockIsNotEnough_NoOrderCreatedNoStockUpdated()
        {
            var item1 = new OrderItemModel { ProductId = 1, Quantity = 10 };
            var item2 = new OrderItemModel { ProductId = 4, Quantity = 2 };
            var orderId = 539;
            _orderRepositoryMock.Setup(m => m.CreateOrder(_customerId, It.IsAny<IEnumerable<OrderItemModel>>(), _userId)).Returns(() => Task.FromResult(orderId));
            _orderRepositoryMock.Setup(m => m.GetOrder(orderId)).Returns(() => Task.FromResult(new OrderModel()));
            _productRepositoryMock.Setup(m => m.GetProduct(item1.ProductId)).Returns(() => Task.FromResult(new ProductModel { StockQuantity = 5 }));
            _productRepositoryMock.Setup(m => m.GetProduct(item2.ProductId)).Returns(() => Task.FromResult(new ProductModel { StockQuantity = 20 }));
            _productRepositoryMock.Setup(m => m.StockQuantityDecrement(It.IsAny<int>(), It.IsAny<int>(), _userId));
            _orderRepositoryMock.Setup(m => m.UpdateStatus(It.IsAny<int>(), It.IsAny<OrderStatus>(), _userId));

            var orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object);

            try
            {
                var result = await orderService.CreateOrder(new OrderModel
                {
                    CustomerId = _customerId,
                    Items = new List<OrderItemModel> { item1, item2 }
                }, _userId);

                Assert.Fail();
            }
            catch
            {
                _orderRepositoryMock.Verify(m => m.CreateOrder(_customerId, It.IsAny<IEnumerable<OrderItemModel>>(), _userId), Times.Never());
                _productRepositoryMock.Verify(m => m.GetProduct(item1.ProductId), Times.Once());
                _productRepositoryMock.Verify(m => m.GetProduct(item2.ProductId), Times.Never());
                _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item1.ProductId, item1.Quantity, _userId), Times.Never());
                _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item1.ProductId, item1.Quantity, _userId), Times.Never());
                _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item2.ProductId, item2.Quantity, _userId), Times.Never());
                _productRepositoryMock.Verify(m => m.StockQuantityIncrement(item2.ProductId, item2.Quantity, _userId), Times.Never());
                _orderRepositoryMock.Verify(m => m.UpdateStatus(orderId, OrderStatus.Approved, _userId), Times.Never());
                _orderRepositoryMock.Verify(m => m.UpdateStatus(orderId, OrderStatus.Rejected, _userId), Times.Never());
            }
        }

        [TestMethod]
        public async Task CreateOrder_SecondProductStockIsNotEnough_NoOrderCreatedNoStockUpdated()
        {
            var item1 = new OrderItemModel { ProductId = 1, Quantity = 1 };
            var item2 = new OrderItemModel { ProductId = 4, Quantity = 2 };
            var orderId = 539;
            _orderRepositoryMock.Setup(m => m.CreateOrder(_customerId, It.IsAny<IEnumerable<OrderItemModel>>(), _userId)).Returns(() => Task.FromResult(orderId));
            _orderRepositoryMock.Setup(m => m.GetOrder(orderId)).Returns(() => Task.FromResult(new OrderModel()));
            _productRepositoryMock.Setup(m => m.GetProduct(item1.ProductId)).Returns(() => Task.FromResult(new ProductModel { StockQuantity = 10 }));
            _productRepositoryMock.Setup(m => m.GetProduct(item2.ProductId)).Returns(() => Task.FromResult(new ProductModel { StockQuantity = 1 }));
            _productRepositoryMock.Setup(m => m.StockQuantityDecrement(It.IsAny<int>(), It.IsAny<int>(), _userId));
            _productRepositoryMock.Setup(m => m.StockQuantityIncrement(It.IsAny<int>(), It.IsAny<int>(), _userId));
            _orderRepositoryMock.Setup(m => m.UpdateStatus(It.IsAny<int>(), It.IsAny<OrderStatus>(), _userId));

            var orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object);
            try
            {
                var result = await orderService.CreateOrder(new OrderModel
                {
                    CustomerId = _customerId,
                    Items = new List<OrderItemModel> { item1, item2 }
                }, _userId);

                Assert.Fail();
            }
            catch
            {
                _orderRepositoryMock.Verify(m => m.CreateOrder(_customerId, It.IsAny<IEnumerable<OrderItemModel>>(), _userId), Times.Never());
                _productRepositoryMock.Verify(m => m.GetProduct(item1.ProductId), Times.Once());
                _productRepositoryMock.Verify(m => m.GetProduct(item2.ProductId), Times.Once());
                _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item1.ProductId, item1.Quantity, _userId), Times.Never());
                _productRepositoryMock.Verify(m => m.StockQuantityIncrement(item1.ProductId, item1.Quantity, _userId), Times.Never());
                _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item2.ProductId, item2.Quantity, _userId), Times.Never());
                _productRepositoryMock.Verify(m => m.StockQuantityIncrement(item2.ProductId, item2.Quantity, _userId), Times.Never());
                _orderRepositoryMock.Verify(m => m.UpdateStatus(orderId, OrderStatus.Approved, _userId), Times.Never());
                _orderRepositoryMock.Verify(m => m.UpdateStatus(orderId, OrderStatus.Rejected, _userId), Times.Never());
            }
        }

        [TestMethod]
        public async Task CreateOrder_UpdateStockFailed_IncrementedStock()
        {
            var item1 = new OrderItemModel { ProductId = 1, Quantity = 1 };
            var item2 = new OrderItemModel { ProductId = 4, Quantity = 2 };
            var orderId = 539;
            _orderRepositoryMock.Setup(m => m.CreateOrder(_customerId, It.IsAny<IEnumerable<OrderItemModel>>(), _userId)).Returns(() => Task.FromResult(orderId));
            _orderRepositoryMock.Setup(m => m.GetOrder(orderId)).Returns(() => Task.FromResult(new OrderModel()));
            _productRepositoryMock.Setup(m => m.GetProduct(item1.ProductId)).Returns(() => Task.FromResult(new ProductModel { StockQuantity = 10 }));
            _productRepositoryMock.Setup(m => m.GetProduct(item2.ProductId)).Returns(() => Task.FromResult(new ProductModel { StockQuantity = 20 }));
            _productRepositoryMock.Setup(m => m.StockQuantityDecrement(item1.ProductId, It.IsAny<int>(), _userId));
            _productRepositoryMock.Setup(m => m.StockQuantityDecrement(item2.ProductId, It.IsAny<int>(), _userId)).Throws(new Exception());
            _orderRepositoryMock.Setup(m => m.UpdateStatus(It.IsAny<int>(), It.IsAny<OrderStatus>(), _userId));

            var orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object);
            try
            {
                var result = await orderService.CreateOrder(new OrderModel
                {
                    CustomerId = _customerId,
                    Items = new List<OrderItemModel> { item1, item2 }
                }, _userId);

                Assert.Fail();
            }
            catch
            {
                _orderRepositoryMock.Verify(m => m.CreateOrder(_customerId, It.IsAny<IEnumerable<OrderItemModel>>(), _userId), Times.Once());
                _productRepositoryMock.Verify(m => m.GetProduct(item1.ProductId), Times.Once());
                _productRepositoryMock.Verify(m => m.GetProduct(item2.ProductId), Times.Once());
                _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item1.ProductId, item1.Quantity, _userId), Times.Once());
                _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item2.ProductId, item2.Quantity, _userId), Times.Once());
                _productRepositoryMock.Verify(m => m.StockQuantityDecrement(item1.ProductId, item1.Quantity, _userId), Times.Once());
                _productRepositoryMock.Verify(m => m.StockQuantityIncrement(item2.ProductId, item2.Quantity, _userId), Times.Never());
                _orderRepositoryMock.Verify(m => m.UpdateStatus(orderId, OrderStatus.Approved, _userId), Times.Never());
                _orderRepositoryMock.Verify(m => m.UpdateStatus(orderId, OrderStatus.Rejected, _userId), Times.Once());
            }
        }
    }
}
