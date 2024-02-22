using AutoFixture;
using AutoMapper;
using BussinessObject.Models;
using DataAccess.DB;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Responsitory.DTO;
using Responsitory.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.ServiceTest
{
    [TestClass]
    public class OrderServiceTest
    {
        private DbContextOptions<Db> options;
        private Db inMemoryDb;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private Mock<OrderService> _orserviceMock;


        public OrderServiceTest()
        {
            options = new DbContextOptionsBuilder<Db>().Options;
            inMemoryDb = new Db(options);
            _mapperMock = new Mock<IMapper>();
            _fixture = new Fixture();
            _orserviceMock = new Mock<OrderService>(inMemoryDb, _mapperMock.Object);
        }
        public void ClearDatabate()
        {
            var query = "DELETE FROM Products";
            var query1 = "DELETE FROM Categorys";
            var query2 = "DELETE FROM Carts";
            var query3 = "DELETE FROM Accounts";
            var query4 = "DELETE FROM OrderDetails";
            var query5 = "DELETE FROM Orders";
            var query6 = "DELETE FROM Users";
            inMemoryDb.Database.ExecuteSqlRaw(query);
            inMemoryDb.Database.ExecuteSqlRaw(query1);
            inMemoryDb.Database.ExecuteSqlRaw(query2);
            inMemoryDb.Database.ExecuteSqlRaw(query3);
            inMemoryDb.Database.ExecuteSqlRaw(query4);
            inMemoryDb.Database.ExecuteSqlRaw(query5);
            inMemoryDb.Database.ExecuteSqlRaw(query6);
        }
        [TestMethod]
        public async Task CreateOrderAsync_Valid_FisrtCreate()
        {
            int userId = _fixture.Create<int>();
            Cart cart = new Cart()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 5,
                ProductImage = "product_image_url.jpg",
                Price = 10.99,
                CartStatus = "1"
            };
            Product product = new Product()
            {
                ProductID = 1,
                ProductName = "Example Product",
                ProductImage = "product.jpg",
                ProductPrice = 999.99,
                CategoryID = 2,
                ProductQuantity = 10,
                ProductDetailDescription = "This is a detailed product description.",
                ProductChipset = "Snapdragon 888",
                ProductStorageInternal = "256GB",
                ProductStorageExternal = "SD card slot available",
                ProductBatteryCapacity = 4000,
                ProductSelfieCamera = "16MP",
                ProductMainCamera = "48MP",
                ProductStatus = "1",
            };
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1",
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
            inMemoryDb.Carts.Add(cart);
            inMemoryDb.SaveChanges();
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            var response = _orserviceMock.Object.CreateOrderAsync(u.UserID).Result;
            Assert.AreEqual(response, 1);
            ClearDatabate();
        }
        [TestMethod]
        public async Task CreateOrderAsync_Valid_SecondCreate()
        {
            int userId = _fixture.Create<int>();
            Cart cart = new Cart()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 5,
                ProductImage = "product_image_url.jpg",
                Price = 10.99,
                CartStatus = "1"
            };
            Product product = new Product()
            {
                ProductID = 1,
                ProductName = "Example Product",
                ProductImage = "product.jpg",
                ProductPrice = 999.99,
                CategoryID = 2,
                ProductQuantity = 10,
                ProductDetailDescription = "This is a detailed product description.",
                ProductChipset = "Snapdragon 888",
                ProductStorageInternal = "256GB",
                ProductStorageExternal = "SD card slot available",
                ProductBatteryCapacity = 4000,
                ProductSelfieCamera = "16MP",
                ProductMainCamera = "48MP",
                ProductStatus = "1",
            };
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1",
            };
            Order order = new Order
            {
                OrderID = 1,
                UserID = 1,
                OrderDate = DateTime.Now,
                OrderQuantity = 5.0,
                ShipperDate = DateTime.Now.AddDays(5),
                OrderNote = "Example order note",
                OrderStatus = "1"
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
            inMemoryDb.Orders.Add(order);
            inMemoryDb.SaveChanges();
            inMemoryDb.Carts.Add(cart);
            inMemoryDb.SaveChanges();
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            var response = _orserviceMock.Object.CreateOrderAsync(u.UserID).Result;
            Assert.AreEqual(response, 2);
            ClearDatabate();
        }
        [TestMethod]
        public async Task CreateOrderAsync_Valid_SecondCreateDetail()
        {
            int userId = _fixture.Create<int>();
            Cart cart = new Cart()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 5,
                ProductImage = "product_image_url.jpg",
                Price = 10.99,
                CartStatus = "1"
            };
            Product product = new Product()
            {
                ProductID = 1,
                ProductName = "Example Product",
                ProductImage = "product.jpg",
                ProductPrice = 999.99,
                CategoryID = 2,
                ProductQuantity = 10,
                ProductDetailDescription = "This is a detailed product description.",
                ProductChipset = "Snapdragon 888",
                ProductStorageInternal = "256GB",
                ProductStorageExternal = "SD card slot available",
                ProductBatteryCapacity = 4000,
                ProductSelfieCamera = "16MP",
                ProductMainCamera = "48MP",
                ProductStatus = "1",
            };
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1",
            };
            Order order = new Order
            {
                OrderID = 1,
                UserID = 1,
                OrderDate = DateTime.Now,
                OrderQuantity = 5.0,
                ShipperDate = DateTime.Now.AddDays(5),
                OrderNote = "Example order note",
                OrderStatus = "1"
            };
            OrderDetail orderDetail = new OrderDetail
            {
                OrderDetailID = 1,
                OrderDetailTotalPrice = 100.0,
                OrderDetailQuantity = 5,
                OrderDetailStatus = "1",
                OrderID = 1,
                ProductID = 1,
                UserID = 1
            };
            inMemoryDb.OrderDetails.Add(orderDetail);
            inMemoryDb.SaveChanges();
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
            inMemoryDb.Orders.Add(order);
            inMemoryDb.SaveChanges();
            inMemoryDb.Carts.Add(cart);
            inMemoryDb.SaveChanges();
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            var response = _orserviceMock.Object.CreateOrderAsync(u.UserID).Result;
            Assert.AreEqual(response, 2);
            ClearDatabate();
        }
        [TestMethod]
        public async Task DeleteOrder_Valid()
        {
            try
            {
                Order order = new Order
                {
                    OrderID = 1,
                    UserID = 1,
                    OrderDate = DateTime.Now,
                    OrderQuantity = 5.0,
                    ShipperDate = DateTime.Now.AddDays(5),
                    OrderNote = "Example order note",
                    OrderStatus = "1"
                };
                inMemoryDb.Orders.Add(order);
                inMemoryDb.SaveChanges();
                _orserviceMock.Object.DeleteOrderAsync(order.OrderID).GetAwaiter().GetResult();
                Assert.IsTrue(order.OrderID == order.OrderID);
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task PlussOrderAsync_Valid()
        {
            Order order = new Order
            {
                OrderID = 1,
                UserID = 1,
                OrderDate = DateTime.Now,
                OrderQuantity = 5.0,
                ShipperDate = DateTime.Now.AddDays(5),
                OrderNote = "Example order note",
                OrderStatus = "1"
            };
            inMemoryDb.Orders.Add(order);
            inMemoryDb.SaveChanges();
            _orserviceMock.Object.PlussOrderAsync(order.OrderID).GetAwaiter().GetResult();
            Assert.IsTrue(order.OrderID == order.OrderID);
            ClearDatabate();
        }

        [TestMethod]
        public async Task SubOrderAsync_FirstValid()
        {
            Order order = new Order
            {
                OrderID = 1,
                UserID = 1,
                OrderDate = DateTime.Now,
                OrderQuantity = 5,
                ShipperDate = DateTime.Now.AddDays(5),
                OrderNote = "Example order note",
                OrderStatus = "1"
            };

            inMemoryDb.Orders.Add(order);
            inMemoryDb.SaveChanges();
            await _orserviceMock.Object.SubOrderAsync(order.OrderID);
            var updatedOrder = inMemoryDb.Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
            Assert.IsNotNull(updatedOrder);
            Assert.AreEqual(4, updatedOrder.OrderQuantity);
            ClearDatabate();
        }
        [TestMethod]
        public async Task SubOrderAsync_SecondValid()
        {
            Order order = new Order
            {
                OrderID = 1,
                UserID = 1,
                OrderDate = DateTime.Now,
                OrderQuantity = 1,
                ShipperDate = DateTime.Now.AddDays(5),
                OrderNote = "Example order note",
                OrderStatus = "1"
            };

            inMemoryDb.Orders.Add(order);
            inMemoryDb.SaveChanges();
            await _orserviceMock.Object.SubOrderAsync(order.OrderID);
            var updatedOrder = inMemoryDb.Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
            Assert.AreEqual(0, order.OrderQuantity);
            ClearDatabate();
        }
        [TestMethod]
        public async Task UpdateOrderAsync_FirstValid()
        {
            Order order = new Order
            {
                OrderID = 1,
                UserID = 1,
                OrderDate = DateTime.Now,
                OrderQuantity = 1,
                ShipperDate = DateTime.Now.AddDays(5),
                OrderNote = "Example order note",
                OrderStatus = "1"
            };
            inMemoryDb.Orders.Add(order);
            inMemoryDb.SaveChanges();
            OrderResponse updatedOrderResponse = new OrderResponse
            {
                OrderID = 1,
                OrderDate = DateTime.Now.AddDays(1),
                OrderQuantity = 5,
                OrderNote = "Updated order note",
                ShipperDate = DateTime.Now.AddDays(5)
            };
            _orserviceMock.Object.UpdateOrderAsync(order.OrderID, updatedOrderResponse).GetAwaiter().GetResult();
            Assert.AreEqual(order.OrderID, 1);
            ClearDatabate();
        }
        [TestMethod]
        public async Task UpdateOrderAsync_SecondValid()
        {
            try
            {

                OrderResponse updatedOrderResponse = null;
                await _orserviceMock.Object.UpdateOrderAsync(1, updatedOrderResponse);
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("UPOR", ex.Message);
                ClearDatabate();
            }
        }

        [TestMethod]
        public async Task UpdateOrderAsync_IdNul()
        {
            try
            {
                OrderResponse updatedOrderResponse = new OrderResponse
                {
                    OrderID = 2,
                    OrderDate = DateTime.Now.AddDays(1),
                    OrderQuantity = 5,
                    OrderNote = "Updated order note",
                    ShipperDate = DateTime.Now.AddDays(5)
                };
                await _orserviceMock.Object.UpdateOrderAsync(1, updatedOrderResponse);
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("idnotmatch", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task GetOrderByIdAsync_Valid()
        {
            Product product = new Product()
            {
                ProductID = 1,
                ProductName = "Example Product",
                ProductImage = "product.jpg",
                ProductPrice = 999.99,
                CategoryID = 2,
                ProductQuantity = 10,
                ProductDetailDescription = "This is a detailed product description.",
                ProductChipset = "Snapdragon 888",
                ProductStorageInternal = "256GB",
                ProductStorageExternal = "SD card slot available",
                ProductBatteryCapacity = 4000,
                ProductSelfieCamera = "16MP",
                ProductMainCamera = "48MP",
                ProductStatus = "1",
            };
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1",
            };
            Order order = new Order
            {
                OrderID = 1,
                UserID = 1,
                OrderDate = DateTime.Now,
                OrderQuantity = 1,
                ShipperDate = DateTime.Now.AddDays(5),
                OrderNote = "Example order note",
                OrderStatus = "1"
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
            inMemoryDb.Orders.Add(order);
            inMemoryDb.SaveChanges();
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            OrderResponse orderResponse = new OrderResponse()
            {
                OrderID = order.OrderID,
                FullName = u.FullName,
                Address = u.Address,
                Phone = u.Phone,
                OrderDate = order.OrderDate,
                OrderQuantity = order.OrderQuantity,
                OrderNote = order.OrderNote,
                ShipperDate = order.ShipperDate
            };
            var Result = _orserviceMock.Object.GetByIdAsync(order.OrderID).GetAwaiter().GetResult();
            Result.Should().BeEquivalentTo(orderResponse);
            ClearDatabate();
        }
        [TestMethod]
        public async Task GetOrderAsync_Valid()
        {
            int userId = 1;
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1",
            };
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            Order order1 = new Order
            {
                OrderID = 1,
                UserID = userId,
                OrderDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderQuantity = 2,
                ShipperDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderNote = "aaa",
                OrderStatus = "1"
            };
            Order order2 = new Order
            {
                OrderID = 2,
                UserID = userId,
                OrderDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderQuantity = 3,
                ShipperDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderNote = "aaa",
                OrderStatus = "1"
            };
            inMemoryDb.Orders.Add(order1);
            inMemoryDb.Orders.Add(order2);
            inMemoryDb.SaveChanges();
            List<OrderAdminResponse> list = new List<OrderAdminResponse>();
            OrderAdminResponse cus = new OrderAdminResponse()
            {
                OrderID = 2,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                OrderDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderQuantity = 3,
                ShipperDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderNote = "aaa",
            };
            OrderAdminResponse cus1 = new OrderAdminResponse()
            {
                OrderID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                OrderDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderQuantity = 2,
                ShipperDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderNote = "aaa",
            };
            list.Add(cus1);
            list.Add(cus);
            var result = _orserviceMock.Object.GetOrderAsync().Result;
            result.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task TotalPrice_Valid()
        {
            OrderDetail orderDetail1 = new OrderDetail
            {
                OrderDetailID = 1,
                OrderDetailTotalPrice = 50.0,
                OrderDetailStatus = "1",
            };

            OrderDetail orderDetail2 = new OrderDetail
            {
                OrderDetailID = 2,
                OrderDetailTotalPrice = 100.0,
                OrderDetailStatus = "1",
            };
            inMemoryDb.OrderDetails.Add(orderDetail1);
            inMemoryDb.OrderDetails.Add(orderDetail2);
            inMemoryDb.SaveChanges();

            var result = await _orserviceMock.Object.TotalPrice();
            Assert.IsNotNull(result);
            double expectedTotalPrice = orderDetail1.OrderDetailTotalPrice + orderDetail2.OrderDetailTotalPrice;
            Assert.AreEqual(expectedTotalPrice, result.TotalPrice);
            ClearDatabate();
        }
        [TestMethod]
        public async Task ConfirmOrderAsync_ValidId_ConfirmsOrder()
        {
            int orderId = 1;
            Order order = new Order
            {
                OrderID = orderId,
                UserID = 1,
                OrderDate = DateTime.Now,
                OrderQuantity = 1,
                ShipperDate = DateTime.Now.AddDays(5),
                OrderNote = "Example order note",
                OrderStatus = "1"
            };
            inMemoryDb.Orders.Add(order);
            inMemoryDb.SaveChanges();
            await _orserviceMock.Object.ConfirmOrdeAsync(orderId);
            Order confirmedOrder = inMemoryDb.Orders.FirstOrDefault(o => o.OrderID == orderId);
            Assert.IsNotNull(confirmedOrder);
            Assert.AreEqual("Confirm", confirmedOrder.OrderNote);
            ClearDatabate();
        }
        [TestMethod]
        public async Task GetOrderHistoryAsync_ValidId_ReturnsOrderHistory()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _orserviceMock = new Mock<OrderService>(inMemoryDb, mapper);
            int userId = 1;
            Order order1 = new Order
            {
                OrderID = 1,
                UserID = userId,
                OrderDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderQuantity = 2,
                ShipperDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderNote = "aaa",
                OrderStatus = "1"
            };
            Order order2 = new Order
            {
                OrderID = 2,
                UserID = userId,
                OrderDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderQuantity = 3,
                ShipperDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderNote = "aaa",
                OrderStatus = "1"
            };
            inMemoryDb.Orders.Add(order1);
            inMemoryDb.Orders.Add(order2);
            inMemoryDb.SaveChanges();
            List<OrderResponse> list = new List<OrderResponse>();
            OrderResponse cus = new OrderResponse()
            {
                OrderID = 2,
                UserID = userId,
                OrderDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderQuantity = 3,
                ShipperDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderNote = "aaa",
            };
            OrderResponse cus1 = new OrderResponse()
            {
                OrderID = 1,
                UserID = userId,
                OrderDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderQuantity = 2,
                ShipperDate = DateTime.ParseExact("11/2/2023 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                OrderNote = "aaa",
            };
            list.Add(cus1);
            list.Add(cus);
            var result = _orserviceMock.Object.GetOrderHistoryAsync(userId).Result;
            result.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
    }
}
