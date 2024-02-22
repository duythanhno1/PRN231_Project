using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Responsitory.IService;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopProjectAPI.Controllers;
using Responsitory.Service;
using AutoMapper;
using BussinessObject.Models;
using Responsitory.DTO;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestProject.ControllerTest
{
    [TestClass]
    public class AccountControllerTest
    {
        private Mock<IEmailService> _emailService;
        private Mock<IAccountService> _accountService;
        private Fixture _fixture;
        private AccountController _controller;
        public AccountControllerTest()
        {
            _emailService = new Mock<IEmailService>();
            _accountService = new Mock<IAccountService>();
            _fixture = new Fixture();
        }
        [TestMethod]
        public async Task Get_Account_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<CusView> acclist = _fixture.CreateMany<CusView>(1).ToList();
            Task<List<CusView>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _accountService.Setup(re => re.GetAccountAsync()).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new AccountController(_accountService.Object, _emailService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetAccount();
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_Account_ReturnNull()
        {
            List<CusView> acclist = null;
            Task<List<CusView>> task = Task.FromResult(acclist);

            _accountService.Setup(re => re.GetAccountAsync()).Returns(task);
            _controller = new AccountController(_accountService.Object, _emailService.Object);
            var result = await _controller.GetAccount();
            var obj = result as ObjectResult;

            // Kiểm tra xem result có phải là BadRequestResult
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(400, obj.StatusCode);

        }
        [TestMethod]
        public async Task Get_Account_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Custom exception message");

            _accountService.Setup(re => re.GetAccountAsync()).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new AccountController(_accountService.Object, _emailService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.GetAccount();
            var obj = result as ObjectResult;
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
        public async Task Create_Account_Ok()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            Account cus = _fixture.Create<Account>();
            CustomerResponse cusRe = _fixture.Create<CustomerResponse>();
            CusView cusView = _fixture.Create<CusView>();
            Task<int> taskAcciD = Task.FromResult(cus.AccountID);
            Task<CusView> taskCusview = Task.FromResult(cusView);
            _accountService.Setup(re => re.CreateAccountAsync(It.IsAny<CustomerResponse>())).Returns(taskAcciD);
            _accountService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).Returns(taskCusview);

            // Khởi tạo Controller
            _controller = new AccountController(_accountService.Object, _emailService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.CreateAccount(cusRe);
            var obj = result as ObjectResult;

            Assert.AreEqual(200, obj.StatusCode);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task Create_Account_CreateNull_NotFound()
        {
            Account cus = _fixture.Create<Account>();
            CustomerResponse cusRe = null;
            CusView cusView = _fixture.Create<CusView>();
            Task<int> taskAcciD = Task.FromResult(cus.AccountID);
            Task<CusView> taskCusview = Task.FromResult(cusView);
            _accountService.Setup(re => re.CreateAccountAsync(It.IsAny<CustomerResponse>())).Returns(taskAcciD);
            _accountService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).Returns(taskCusview);

            // Khởi tạo Controller
            _controller = new AccountController(_accountService.Object, _emailService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.CreateAccount(cusRe);
            var obj = result as BadRequestResult;
            Assert.AreEqual(400, obj.StatusCode);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
        [TestMethod]
        public async Task Create_Account_CreateFailed_NotFound()
        {
            Account cus = _fixture.Create<Account>();
            CustomerResponse cusRe = _fixture.Create<CustomerResponse>();
            CusView cusView = null;
            Task<int> taskAcciD = Task.FromResult(cus.AccountID);
            Task<CusView> taskCusview = Task.FromResult(cusView);
            _accountService.Setup(re => re.CreateAccountAsync(It.IsAny<CustomerResponse>())).Returns(taskAcciD);
            _accountService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).Returns(taskCusview);

            // Khởi tạo Controller
            _controller = new AccountController(_accountService.Object, _emailService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.CreateAccount(cusRe);
            var obj = result as NotFoundResult;
            Assert.AreEqual(404, obj.StatusCode);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
