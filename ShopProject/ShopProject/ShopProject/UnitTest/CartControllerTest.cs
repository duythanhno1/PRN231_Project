using AutoFixture;
using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
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

namespace UnitTest
{
    [TestClass]
    public class CartControllerTest
    {
        private Mock<ICartService> _cartService;
        private Fixture _fixture;
        private CartsController _cartsController;

        public CartControllerTest() {
            _fixture = new Fixture();
            _cartService = new Mock<ICartService>();
        }

        //[TestMethod]
        //public async Task Get_Cart_ReturnOk()
        //{
        //    List<CustomCartResponse> cartList = _fixture.CreateMany<CustomCartResponse>(0).ToList();
        //    Task<List<CustomCartResponse>> task = Task.FromResult(cartList);
        //    _cartService.Setup(sev => sev.GetCartsAsync(1)).Returns(task);

        //    _cartsController = new CartsController(_cartService.Object);
        //    var result = await _cartsController.GetCart(1);
        //    Console.WriteLine(result);
        //    var obj = result as ObjectResult;
        //    Console.WriteLine(obj);
        //    Assert.AreEqual(200,obj.StatusCode);



        //}
        [TestMethod]
        public async Task GetCartsAsync_ReturnsCustomCartResponses()
        {
            // Arrange
            int userId = 1;
            var mockCartService = new Mock<ICartService>();
            var customCartResponses = new List<CustomCartResponse>
            {
                // Add some CustomCartResponse objects here for testing
                new CustomCartResponse
                {
                    CartId = 1,
                    FullName = "User1",
                    Phone = "1234567890",
                    Address = "123 Main St",
                    ProductName = "Product1",
                    CartQuantity = 2,
                    ProductImage = "product1.jpg",
                    Price = 19.99
                    // Add other properties as needed
                },
                new CustomCartResponse
                {
                    CartId = 2,
                    FullName = "User2",
                    Phone = "9876543210",
                    Address = "456 Elm St",
                    ProductName = "Product2",
                    CartQuantity = 3,
                    ProductImage = "product2.jpg",
                    Price = 29.99
                    // Add other properties as needed
                },
            };

            mockCartService.Setup(service => service.GetCartsAsync(userId))
                .ReturnsAsync(customCartResponses);

            var cartController = new CartsController(mockCartService.Object);

            // Act
            var result = await cartController.GetCart(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<CustomCartResponse>));
        }
    }
}
