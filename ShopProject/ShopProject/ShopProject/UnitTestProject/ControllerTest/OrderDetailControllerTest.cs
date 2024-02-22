using AutoFixture;
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
    public class OrderDetailControllerTest
    {
        private Mock<IOrderDetailService> _orderdetailService;
        private Fixture _fixture;
        private OrderDetailController _controller;
        public OrderDetailControllerTest()
        {
            _orderdetailService = new Mock<IOrderDetailService>();
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_OrderDetail_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<OrderDetailAdminResponse> acclist = _fixture.CreateMany<OrderDetailAdminResponse>(1).ToList();
            Task<List<OrderDetailAdminResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderdetailService.Setup(re => re.GetOrderDetailAdminAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderDetailController(_orderdetailService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetOrderDetail(1);
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }


        [TestMethod]
        public async Task Get_OrderDetail_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Order Detail exception message");

            _orderdetailService.Setup(re => re.GetOrderDetailAdminAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderDetailController(_orderdetailService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.GetOrderDetail(1);
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
        public async Task Get_GetOrderDetailsById_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            OrderDetailResponse acclist = _fixture.Create<OrderDetailResponse>();
            Task<OrderDetailResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderdetailService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderDetailController(_orderdetailService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_GetOrderDetailsById_ReturnNotFound()
        {
            //SetUp cứng giá trị return của repository
            OrderDetailResponse acclist = null;
            Task<OrderDetailResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderdetailService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderDetailController(_orderdetailService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_GetOrderDetailsById_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("OrderDetailbyId exception message");

            _orderdetailService.Setup(re => re.GetByIdAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderDetailController(_orderdetailService.Object);

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
        public async Task Get_CreateOrderDetail_ReturnOk()
        {
            OrderDetailResponse orderDetailResponse = _fixture.Create<OrderDetailResponse>();

            int newOrderDetailId = orderDetailResponse.OrderDetailID; // Replace with the expected category ID.
            Task<OrderDetailResponse> taskview = Task.FromResult(orderDetailResponse);
            _orderdetailService.Setup(re => re.CreateOrderDetailAsync(orderDetailResponse)).ReturnsAsync(newOrderDetailId);
            _orderdetailService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).Returns(taskview);
            // Initialize the controller with the mocked service
            _controller = new OrderDetailController(_orderdetailService.Object);

            // Act
            var result = await _controller.CreateOrderDetail(orderDetailResponse);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }
        [TestMethod]
        public async Task Get_CreateOrderDetail_ThrowException()
        {
            // Create an exception of your choice, e.g., InvalidOperationException
            var exception = new InvalidOperationException("Create exception message");

            _orderdetailService.Setup(re => re.CreateOrderDetailAsync(It.IsAny<OrderDetailResponse>())).ThrowsAsync(exception);

            // Initialize the controller with the mocked service
            _controller = new OrderDetailController(_orderdetailService.Object);

            // Create a OrderDetailResponse object or initialize it with necessary data
            OrderDetailResponse OrderDetailResponse = new OrderDetailResponse
            {
                // Initialize with the required data.
            };

            // Act: Call the controller method with the OrderDetailResponse object
            var result = await _controller.CreateOrderDetail(OrderDetailResponse);

            // Assert: Check if the result is a BadRequestResult
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            var badRequestResult = (BadRequestResult)result;

            // Assert: Check the status code in the BadRequestResult
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }
        [TestMethod]
        public async Task Get_UpdateOrderDetail_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            OrderDetailResponse acclist = _fixture.Create<OrderDetailResponse>();
            Task<OrderDetailResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderdetailService.Setup(re => re.UpdateOrderDetailAsync(1, acclist)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderDetailController(_orderdetailService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.UpdateOrderDetail(1, acclist);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_UpdateOrderDetail_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Update cart exception message");
            OrderDetailResponse acclist = _fixture.Create<OrderDetailResponse>();
            _orderdetailService.Setup(re => re.UpdateOrderDetailAsync(1, acclist)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderDetailController(_orderdetailService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.UpdateOrderDetail(1, acclist);
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
        public async Task Get_DeleteOrderDetail_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            int acclist = 1;
            Task<int> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _orderdetailService.Setup(re => re.DeleteOrderDetailAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new OrderDetailController(_orderdetailService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.DeleteOrderDetail(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_DeleteOrderDetail_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Delete category exception message");

            _orderdetailService.Setup(re => re.DeleteOrderDetailAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new OrderDetailController(_orderdetailService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.DeleteOrderDetail(1);
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
