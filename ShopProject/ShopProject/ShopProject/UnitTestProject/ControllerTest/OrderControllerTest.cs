using AutoFixture;
using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Responsitory.DTO;
using Responsitory.IService;
using ShopProjectAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.ControllerTest
{
    [TestClass]
    public class OrderControllerTest
    {
        private Mock<IOrderService> _orderService;
        private Fixture _fixture;
        private OrderController _controller;
        public OrderControllerTest()
        {
            _orderService = new Mock<IOrderService>();
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Order_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<OrderAdminResponse> acclist = _fixture.CreateMany<OrderAdminResponse>(1).ToList();
            Task<List<OrderAdminResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.GetOrderAsync()).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetOrder();
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_Order_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Order exception message");

            _orderService.Setup(re => re.GetOrderAsync()).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.GetOrder();
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());

        }

        [TestMethod]
        public async Task Get_GetOrderById_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            OrderResponse acclist = _fixture.Create<OrderResponse>();
            Task<OrderResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_GetOrderById_ReturnNotFound()
        {
            //SetUp cứng giá trị return của repository
            OrderResponse acclist = null;
            Task<OrderResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_GetOrderById_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Order by Id exception message");

            _orderService.Setup(re => re.GetByIdAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.GetById(1);
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());

        }
        [TestMethod]
        public async Task Get_CreateOrder_ReturnOk()
        {
            OrderResponse orderResponse = _fixture.Create<OrderResponse>();
            Task<OrderResponse> OR = Task.FromResult(orderResponse);
            int newCategoryId = 1; // Replace with the expected category ID.
            _orderService.Setup(re => re.CreateOrderAsync(1)).ReturnsAsync(newCategoryId);
            _orderService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).Returns(OR);
            // Initialize the controller with the mocked service
            _controller = new OrderController(_orderService.Object);

            // Act
            var result = await _controller.CreateOrder(1);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }
        [TestMethod]
        public async Task Get_CreateOrder_ReturnNotFound()
        {
            // Arrange
            OrderResponse orderResponse = null;
            User or = _fixture.Create<User>();
            Task<OrderResponse> OR = Task.FromResult(orderResponse);
            int newCategoryId = 1; // Replace with the expected category ID.
            _orderService.Setup(re => re.CreateOrderAsync(or.UserID)).ReturnsAsync(newCategoryId);
            _orderService.Setup(re => re.GetByIdAsync(newCategoryId)).Returns(OR);
            // Initialize the controller with the mocked service
            _controller = new OrderController(_orderService.Object);

            // Act
            var result = await _controller.CreateOrder(or.UserID);
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_CreateOrder_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Order by Id exception message");
            int newCategoryId = 1;
            _orderService.Setup(re => re.CreateOrderAsync(newCategoryId)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.CreateOrder(1);
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());

        }

        [TestMethod]
        public async Task Get_DeleteOrder_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            int acclist = 1;
            Task<int> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.DeleteOrderAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.DeleteOrder(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_DeleteOrder_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Delete cart exception message");

            _orderService.Setup(re => re.DeleteOrderAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.DeleteOrder(1);
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());

        }
        [TestMethod]
        public async Task Get_UpdateOrder_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            OrderResponse acclist = _fixture.Create<OrderResponse>();
            Task<OrderResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.UpdateOrderAsync(1, acclist)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.UpdateOrder(1, acclist);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_UpdateOrder_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Update order exception message");
            OrderResponse acclist = _fixture.Create<OrderResponse>();
            _orderService.Setup(re => re.UpdateOrderAsync(1, acclist)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.UpdateOrder(1, acclist);
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());


        }
        [TestMethod]
        public async Task Get_PlussOrderAsync_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            OrderResponse acclist = _fixture.Create<OrderResponse>();
            Task<OrderResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.PlussOrderAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.PlussOrderAsync(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_PlussOrderAsync_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Pluss product exception message");
            OrderResponse acclist = _fixture.Create<OrderResponse>();
            _orderService.Setup(re => re.PlussOrderAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.PlussOrderAsync(1);
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());
        }
        [TestMethod]
        public async Task Get_SubOrderAsync_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            OrderResponse acclist = _fixture.Create<OrderResponse>();
            Task<OrderResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.SubOrderAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.SubOrderAsync(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_SubOrderAsync_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Sub product exception message");
            OrderResponse acclist = _fixture.Create<OrderResponse>();
            _orderService.Setup(re => re.SubOrderAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.SubOrderAsync(1);
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());
        }
        [TestMethod]
        public async Task Get_Count_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            CountOrder acclist = _fixture.Create<CountOrder>();
            Task<CountOrder> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.CountOr()).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.Count();
            var obj = result as OkObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_Count_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Count order exception message");
            CountOrder acclist = _fixture.Create<CountOrder>();
            _orderService.Setup(re => re.CountOr()).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.Count();
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());
        }
        [TestMethod]
        public async Task Get_Total_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            OrderTotalPrice acclist = _fixture.Create<OrderTotalPrice>();
            Task<OrderTotalPrice> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.TotalPrice()).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.Total();
            var obj = result as OkObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_Total_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Sub product exception message");
            OrderTotalPrice acclist = _fixture.Create<OrderTotalPrice>();
            _orderService.Setup(re => re.TotalPrice()).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.Total();
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());
        }
        [TestMethod]
        public async Task Get_GetOrderHistoryAsync_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<OrderResponse> acclist = _fixture.CreateMany<OrderResponse>(1).ToList();
            Task<List<OrderResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.GetOrderHistoryAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetOrderHistoryAsync(1);
            var obj = result as OkObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_GetOrderHistoryAsync_ReturnNotFound()
        {
            //SetUp cứng giá trị return của repository
            List<OrderResponse> acclist = null;
            Task<List<OrderResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.GetOrderHistoryAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetOrderHistoryAsync(1);
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_GetOrderHistoryAsync_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("OrderHistory exception message");

            _orderService.Setup(re => re.GetOrderHistoryAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.GetOrderHistoryAsync(1);
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());

        }
        [TestMethod]
        public async Task Get_ConfirmOrdeAsync_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            OrderResponse acclist = _fixture.Create<OrderResponse>();
            Task<OrderResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderService.Setup(re => re.ConfirmOrdeAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderController(_orderService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.ConfirmOrdeAsync(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_ConfirmOrdeAsync_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Sub product exception message");
            OrderResponse acclist = _fixture.Create<OrderResponse>();
            _orderService.Setup(re => re.ConfirmOrdeAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderController(_orderService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.ConfirmOrdeAsync(1);
            var obj = result as BadRequestObjectResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            var exceptionFromController = badRequestResult.Value;

            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exception.Message, exceptionFromController.ToString());
        }

    }
}
