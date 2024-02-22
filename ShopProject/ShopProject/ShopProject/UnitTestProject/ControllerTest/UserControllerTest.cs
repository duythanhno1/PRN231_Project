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
    public class UserControllerTest
    {
        private Mock<IUserService> _userService;
        private Fixture _fixture;
        private UserController _controller;

        public UserControllerTest()
        {
            _userService = new Mock<IUserService>();
            _fixture = new Fixture();
        }
        [TestMethod]
        public async Task Get_User_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<UserResponse> acclist = _fixture.CreateMany<UserResponse>(1).ToList();
            Task<List<UserResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _userService.Setup(re => re.GetUserAsync()).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new UserController(_userService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetUser();
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_Categories_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("User exception message");

            _userService.Setup(re => re.GetUserAsync()).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new UserController(_userService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.GetUser();
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
        public async Task Get_UserById_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            UserResponse acclist = _fixture.Create<UserResponse>();
            Task<UserResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _userService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new UserController(_userService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_GetCategoriesById_ReturnNotFound()
        {
            //SetUp cứng giá trị return của repository
            UserResponse acclist = null;
            Task<UserResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _userService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new UserController(_userService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_GetUserById_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("UserbyId exception message");

            _userService.Setup(re => re.GetByIdAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new UserController(_userService.Object);

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
        public async Task CreateUser_ReturnsOk()
        {
            UserResponse userResponse = _fixture.Create<UserResponse>();

            Assert.IsNotNull(userResponse);

            _userService.Setup(re => re.CreateUserAsync(It.IsAny<UserResponse>())).ReturnsAsync(1);

            _userService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(userResponse);

            var _controller = new UserController(_userService.Object);

            var result = await _controller.CreateUser(userResponse);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);

            Assert.AreEqual(200, okResult.StatusCode);
        }
        [TestMethod]
        public async Task Get_CreateUser_ThrowException()
        {
            var exception = new InvalidOperationException("Create exception message");

            _userService.Setup(re => re.CreateUserAsync(It.IsAny<UserResponse>())).ThrowsAsync(exception);

            _controller = new UserController(_userService.Object);

            UserResponse UserResponse = new UserResponse
            {
            };

            var result = await _controller.CreateUser(UserResponse);

            // Assert: Check if the result is a BadRequestResult
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            var badRequestResult = (BadRequestResult)result;

            // Assert: Check the status code in the BadRequestResult
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }
        [TestMethod]
        public async Task Get_UpdateUser_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            UserResponse acclist = _fixture.Create<UserResponse>();
            Task<UserResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _userService.Setup(re => re.UpdateUserAsync(1, acclist)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new UserController(_userService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.UpdateUser(1, acclist);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task UpdateUser_ExceptionMessage()
        {
            UserResponse userResponse = _fixture.Create<UserResponse>();
            var exceptionMessage = "Update failed";

            _userService.Setup(re => re.UpdateUserAsync(1, userResponse))
                .ThrowsAsync(new Exception(exceptionMessage));

            _controller = new UserController(_userService.Object);

            var result = await _controller.UpdateUser(1, userResponse);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;

            Assert.AreEqual(400, badRequestResult.StatusCode);

            var exceptionFromController = badRequestResult.Value as string;
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exceptionMessage, exceptionFromController);
        }
        [TestMethod]
        public async Task Get_DeleteUser_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            int acclist = 1;
            Task<int> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _userService.Setup(re => re.DeleteUserAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new UserController(_userService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.DeleteUser(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_DeleteUser_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Delete category exception message");

            _userService.Setup(re => re.DeleteUserAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new UserController(_userService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.DeleteUser(1);
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
        public async Task SearchProductAsync_ReturnsOk()
        {
            // Arrange
            var search = "exampleSearchString";
            var userResponses = _fixture.CreateMany<UserResponse>(1).ToList();

            _userService.Setup(service => service.SearchUserAsync(search))
                .ReturnsAsync(userResponses);

            // Initialize the controller with the mocked service
            _controller = new UserController(_userService.Object);

            // Act: Call the controller method that you want to test
            var result = await _controller.SearchProductAsync(search);

            // Assert: Check if the result is an OkObjectResult
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;

            Assert.AreEqual(200, okResult.StatusCode);

        }
        [TestMethod]
        public async Task SearchUser_Return_ExceptionMessage()
        {
            var search = "exampleSearchString";
            var exceptionMessage = "Search failed";

            _userService.Setup(service => service.SearchUserAsync(search))
                .ThrowsAsync(new Exception(exceptionMessage));

            _controller = new UserController(_userService.Object);

            var result = await _controller.SearchProductAsync(search);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;

            Assert.AreEqual(400, badRequestResult.StatusCode);
            var exceptionFromController = badRequestResult.Value as string;
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exceptionMessage, exceptionFromController);
        }
    }
}

