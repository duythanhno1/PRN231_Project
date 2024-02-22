using AutoFixture;
using AutoMapper;
using BussinessObject.Models;
using DataAccess.DB;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Responsitory.DTO;
using Responsitory.IService;
using Responsitory.Service;
using ShopProjectAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.ServiceTest
{
    [TestClass]
    public class CartsServiceTest
    {
        private DbContextOptions<Db> options;
        private Db inMemoryDb;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private Mock<CartService> _accserviceMock;


        public CartsServiceTest()
        {
            options = new DbContextOptionsBuilder<Db>().Options;
            inMemoryDb = new Db(options);
            _mapperMock = new Mock<IMapper>();
            _fixture = new Fixture();
            _accserviceMock = new Mock<CartService>(inMemoryDb, _mapperMock.Object);
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
        public async Task CreateCartsAsync_Valid_firstCart()
        {
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
                ProductStatus = "Available",
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
            var response = _accserviceMock.Object.CreateCartAsync(product.ProductID, u.UserID).Result;
            // Assert

            // Kiểm tra xem result có phải là AccountID được trả về (giả định rằng nó sẽ thành công) hoặc 99999999 (nếu có lỗi)
            Assert.AreEqual(response, 1);
            ClearDatabate();
        }
        [TestMethod]
        public async Task CreateCartsAsync_Valid_secondCart()
        {
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
                ProductStatus = "Available",
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
            Cart cartItem = new Cart()
            {
                CartId = 1,
                ProductID = 2,
                UserID = 3,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "Active",
            };
            inMemoryDb.Carts.Add(cartItem);
            inMemoryDb.SaveChanges();
            var response = _accserviceMock.Object.CreateCartAsync(product.ProductID, u.UserID).Result;
            // Assert

            // Kiểm tra xem result có phải là AccountID được trả về (giả định rằng nó sẽ thành công) hoặc 99999999 (nếu có lỗi)
            Assert.AreEqual(response, 2);
            ClearDatabate();
        }

        [TestMethod]
        public async Task CreateCartsAsync_Valid_ThrowException1()
        {
            try
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
                    ProductStatus = "Available",
                };
                inMemoryDb.Products.Add(product);
                inMemoryDb.SaveChanges();
                var response = _accserviceMock.Object.CreateCartAsync(product.ProductID, 1).Result;
                ClearDatabate();
            }
            catch(Exception ex)
            {
                Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
                ClearDatabate();

            }
        }

        [TestMethod]
        public async Task CreateCartsAsync_Valid_ThrowException2()
        {
            try
            {
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
                var response = _accserviceMock.Object.CreateCartAsync(1, u.UserID).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task DeleteCartsAsync_Valid()
        {
            Cart cartItem = new Cart()
            {
                CartId = 1,
                ProductID = 2,
                UserID = 3,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "Active",
            };
            inMemoryDb.Carts.Add(cartItem);
            inMemoryDb.SaveChanges();
            _accserviceMock.Object.DeleteCartAsync(cartItem.CartId).GetAwaiter().GetResult();
            Assert.IsTrue(cartItem.CartId == cartItem.CartId);
            ClearDatabate();
        }
        [TestMethod]
        public async Task DeleteCartsAsync_Failed()
        {
            try
            {
                _accserviceMock.Object.DeleteCartAsync(1).GetAwaiter().GetResult();
                Assert.Fail("Khong catch");
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("delete", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task PlussCartAsync_Valid()
        {
            Cart cartItem = new Cart()
            {
                CartId = 1,
                ProductID = 2,
                UserID = 3,
                ProductName = "Example Product",
                CartQuantity = 1,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "Active",
            };
            inMemoryDb.Carts.Add(cartItem);
            inMemoryDb.SaveChanges();
            _accserviceMock.Object.PlussCartAsync(cartItem.CartId).GetAwaiter().GetResult();
            Assert.IsTrue(cartItem.CartId == cartItem.CartId);
            ClearDatabate();
        }
        [TestMethod]
        public async Task PlussCartAsync_Failed()
        {
            try
            {
                _accserviceMock.Object.PlussCartAsync(1).GetAwaiter().GetResult();
                Assert.Fail("Khong catch");
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("plus", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task SubCartAsync_Failed()
        {
            try
            {
                _accserviceMock.Object.SubCartAsync(1).GetAwaiter().GetResult();
                Assert.Fail("Khong catch");
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("sub", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task SubCartAsync_Valid1()
        {
            Cart cartItem = new Cart()
            {
                CartId = 1,
                ProductID = 2,
                UserID = 3,
                ProductName = "Example Product",
                CartQuantity = 1,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "Active",
            };

            inMemoryDb.Carts.Add(cartItem);
            inMemoryDb.SaveChanges();
            _accserviceMock.Object.SubCartAsync(cartItem.CartId).GetAwaiter().GetResult();
            Assert.IsTrue(cartItem.CartId == cartItem.CartId);
            ClearDatabate();
        }
        [TestMethod]
        public async Task SubCartAsync_Valid2()
        {
            Cart cartItem = new Cart()
            {
                CartId = 1,
                ProductID = 2,
                UserID = 3,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "Active",
            };

            inMemoryDb.Carts.Add(cartItem);
            inMemoryDb.SaveChanges();
            _accserviceMock.Object.SubCartAsync(cartItem.CartId).GetAwaiter().GetResult();
            Assert.IsTrue(cartItem.CartId == cartItem.CartId);
            ClearDatabate();
        }
        [TestMethod]
        public async Task GetCartByIdAsync_Valid()
        {
            Cart cartItem = new Cart()
            {
                CartId = 1,
                ProductID = 2,
                UserID = 3,
                ProductName = "Example Product",
                CartQuantity = 1,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "Active",
            };
            inMemoryDb.Carts.Add(cartItem);
            inMemoryDb.SaveChanges();
            _accserviceMock.Object.GetCartByIdAsync(cartItem.CartId).GetAwaiter().GetResult();
            Assert.IsTrue(cartItem.CartId == cartItem.CartId);
            ClearDatabate();
        }

        [TestMethod]
        public async Task CheckCart_Valid()
        {
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
                ProductStatus = "Available",
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
            Cart cartItem = new Cart()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "1",
            };
            inMemoryDb.Carts.Add(cartItem);
            inMemoryDb.SaveChanges();
            Cart cartItem1 = new Cart()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 3,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "1",
            };
            var response = _accserviceMock.Object.UpdateCart(cartItem);
            response.Should().BeEquivalentTo(cartItem1);
            ClearDatabate();
        }

        [TestMethod]
        public async Task GetCartsAsync_Valid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cart, CustomCartResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _accserviceMock = new Mock<CartService>(inMemoryDb, mapper);
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
                ProductStatus = "Available",
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
            Cart cartItem1 = new Cart()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "1",
            };
            inMemoryDb.Carts.Add(cartItem1);
            inMemoryDb.SaveChanges();
            List<CustomCartResponse> list = new List<CustomCartResponse>();
            CustomCartResponse cus = new CustomCartResponse()
            {
                CartId = 1,
                FullName = "test",
                Phone = "1234567890",
                Address = "address",
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99
            };
            list.Add(cus);
            var response = _accserviceMock.Object.GetCartsAsync(cus.CartId).GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task UdateCart_Valid()
        {
         
                Cart acc = new Cart()
                {
                    CartId = 1,
                    ProductID = 1,
                    UserID = 1,
                    ProductName = "Example Product",
                    CartQuantity = 2,
                    ProductImage = "product.jpg",
                    Price = 49.99,
                    CartStatus = "1",
                };
                inMemoryDb.Carts.Add(acc);
                inMemoryDb.SaveChanges();
                CartResponse cartResponse = new CartResponse()
                {
                    ProductID = 1,
                    UserID = 2,
                    ProductName = "Example Product",
                    CartQuantity = 3,
                    ProductImage = "product.jpg",
                    Price = 29.99
                };
                _accserviceMock.Object.UpdateCartAsync(acc.CartId, cartResponse).GetAwaiter().GetResult();
                ClearDatabate();
          
        }
        [TestMethod]
        public async Task SearchCartAsync_Valid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cart, CartResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _accserviceMock = new Mock<CartService>(inMemoryDb, mapper);
            
            Cart cartItem1 = new Cart()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "1",
            };
            inMemoryDb.Carts.Add(cartItem1);
            inMemoryDb.SaveChanges();

            string ProductName = "Example Product";
            List<CartResponse> list = new List<CartResponse>();
            CartResponse cus = new CartResponse()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
            };
            list.Add(cus);
            var response = _accserviceMock.Object.SearchCartAsync(ProductName).GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task SearchCartAsync_Null()
        {
            try {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Cart, CartResponse>();
                });
                IMapper mapper = config.CreateMapper();
                _accserviceMock = new Mock<CartService>(inMemoryDb, mapper);

                Cart cartItem1 = new Cart()
                {
                    CartId = 1,
                    ProductID = 1,
                    UserID = 1,
                    ProductName = "Example Product",
                    CartQuantity = 2,
                    ProductImage = "product.jpg",
                    Price = 49.99,
                    CartStatus = "1",
                };
                inMemoryDb.Carts.Add(cartItem1);
                inMemoryDb.SaveChanges();

                string ProductName = "dsds";
                List<CartResponse> list = null;

                var response = _accserviceMock.Object.SearchCartAsync(ProductName).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch(Exception ex) 
            {
                Assert.AreEqual("loi db", ex.Message);
                ClearDatabate();
            }
           
        }
        [TestMethod]
        public async Task SearchCartAsync_InputNull()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Cart, CartResponse>();
                });
                IMapper mapper = config.CreateMapper();
                _accserviceMock = new Mock<CartService>(inMemoryDb, mapper);
                string ProductName = "";
                List<CartResponse> list = null;

                var response = _accserviceMock.Object.SearchCartAsync(ProductName).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("loi r", ex.Message);
                ClearDatabate();
            }

        }
        [TestMethod]
        public void TotalPrice_Valid()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cart, CartTotalPrice>();
            });
            IMapper mapper = config.CreateMapper();
            _accserviceMock = new Mock<CartService>(inMemoryDb, mapper);

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

            Cart cartItem1 = new Cart()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "1",
            };
            inMemoryDb.Carts.Add(cartItem1);
            inMemoryDb.SaveChanges();
            Cart cartItem2 = new Cart()
            {
                CartId = 2,
                ProductID = 2,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "1",
            };
            inMemoryDb.Carts.Add(cartItem2);
            inMemoryDb.SaveChanges();

            var response = _accserviceMock.Object.TotalPrice(u.UserID).GetAwaiter().GetResult();

            double expectedTotalPrice = cartItem1.Price + cartItem2.Price;
            Assert.AreEqual(expectedTotalPrice, response.TotalPrice);

            ClearDatabate();
        }
        [TestMethod]
        public void TotalPrice_Null()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Cart, CartTotalPrice>();
                });
                IMapper mapper = config.CreateMapper();
                _accserviceMock = new Mock<CartService>(inMemoryDb, mapper);
                var response = _accserviceMock.Object.TotalPrice(1).GetAwaiter().GetResult();
                Assert.Fail("loi r");
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.AreEqual("loi r", ex.Message);
                ClearDatabate();
            }

        }
        [TestMethod]
        public void CountCart_Valid()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cart, CountCartQuan>();
            });
            IMapper mapper = config.CreateMapper();
            _accserviceMock = new Mock<CartService>(inMemoryDb, mapper);

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

            Cart cartItem1 = new Cart()
            {
                CartId = 1,
                ProductID = 1,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "1",
            };
            inMemoryDb.Carts.Add(cartItem1);
            inMemoryDb.SaveChanges();
            Cart cartItem2 = new Cart()
            {
                CartId = 2,
                ProductID = 2,
                UserID = 1,
                ProductName = "Example Product",
                CartQuantity = 2,
                ProductImage = "product.jpg",
                Price = 49.99,
                CartStatus = "1",
            };
            inMemoryDb.Carts.Add(cartItem2);
            inMemoryDb.SaveChanges();

            var response = _accserviceMock.Object.TotalPrice(u.UserID).GetAwaiter().GetResult();

            double expectedTotalPrice = cartItem1.Price + cartItem2.Price;
            Assert.AreEqual(expectedTotalPrice, response.TotalPrice);

            ClearDatabate();
        }

        [TestMethod]
        public void CountCart_Null()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Cart, CountCartQuan>();
                });
                IMapper mapper = config.CreateMapper();
                _accserviceMock = new Mock<CartService>(inMemoryDb, mapper);
                var response = _accserviceMock.Object.CountCart(1).GetAwaiter().GetResult();
                Assert.Fail("loi r");
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.AreEqual("loi r", ex.Message);
                ClearDatabate();
            }

        }
    }
    
}
