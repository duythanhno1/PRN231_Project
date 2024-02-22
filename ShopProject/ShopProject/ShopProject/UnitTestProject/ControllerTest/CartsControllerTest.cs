using AutoFixture;
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

namespace UnitTestProject.ControllerTest
{
    [TestClass]
    public class CartsControllerTest
    {
        private Mock<ICartService> _cartService;
        private Fixture _fixture;
        private CartsController _controller;
        public CartsControllerTest()
        {
            _cartService = new Mock<ICartService>();
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Carts_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<CustomCartResponse> acclist = _fixture.CreateMany<CustomCartResponse>(1).ToList();
            Task<List<CustomCartResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.GetCartsAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetCart(1);
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_Carts_ReturnNotFound()
        {
            //SetUp cứng giá trị return của repository
            List<CustomCartResponse> acclist = null;
            Task<List<CustomCartResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.GetCartsAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetCart(1);
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_Cart_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("CartAll exception message");

            _cartService.Setup(re => re.GetCartsAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.GetCart(1);
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
        public async Task Get_GetCartsById_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            CartResponse acclist = _fixture.Create<CartResponse>();
            Task<CartResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.GetCartByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_GetCartsById_ReturnNotFound()
        {
            //SetUp cứng giá trị return của repository
            CartResponse acclist = null;
            Task<CartResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.GetCartByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_GetCartsById_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("CartbyId exception message");

            _cartService.Setup(re => re.GetCartByIdAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

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
        public async Task Get_CreateCarts_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            int acclist = 1;
            Task<int> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.CreateCartAsync(1, 1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.CreateCart(1, 1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_CreateCarts_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Create exception message");

            _cartService.Setup(re => re.CreateCartAsync(1, 1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.CreateCart(1, 1);
            var obj = result as BadRequestResult;
            // Kiểm tra xem result có phải là BadRequestObjectResult và có chứa ngoại lệ
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            var badRequestResult = (BadRequestResult)result;


            // Kiểm tra xem exceptionFromController có phải là ngoại lệ bạn mong đợi
            Assert.AreEqual(400, obj.StatusCode);


        }


        [TestMethod]
        public async Task Get_UpdateCarts_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            CartResponse acclist = _fixture.Create<CartResponse>();
            Task<CartResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.UpdateCartAsync(1, acclist)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.UpdateCart(1, acclist);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_UpdateCarts_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Update cart exception message");
            CartResponse acclist = _fixture.Create<CartResponse>();
            _cartService.Setup(re => re.UpdateCartAsync(1, acclist)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.UpdateCart(1, acclist);
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
        public async Task Get_DeleteCarts_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            int acclist = 1;
            Task<int> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.DeleteCartAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.DeleteCart(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_DeleteCarts_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Delete cart exception message");

            _cartService.Setup(re => re.DeleteCartAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.DeleteCart(1);
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
        public async Task Get_PlussCarts_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            int acclist = 1;
            Task<int> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.PlussCartAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.PlussCartAsync(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_PlussCarts_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Pluss cart exception message");

            _cartService.Setup(re => re.PlussCartAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.PlussCartAsync(1);
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
        public async Task Get_SubCarts_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            int acclist = 1;
            Task<int> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.SubCartAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.SubCartAsync(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_SubCarts_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Sub cart exception message");

            _cartService.Setup(re => re.SubCartAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.SubCartAsync(1);
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
        public async Task Get_TotalPrice_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            CartTotalPrice acclist = _fixture.Create<CartTotalPrice>();
            Task<CartTotalPrice> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.TotalPrice(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.TotalPrice(1);
            var obj = result as OkObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_TotalPrice_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Update cart exception message");
            CartTotalPrice acclist = _fixture.Create<CartTotalPrice>();
            _cartService.Setup(re => re.TotalPrice(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.TotalPrice(1);
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
        public async Task Get_CountCartQuan_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            CountCartQuan acclist = _fixture.Create<CountCartQuan>();
            Task<CountCartQuan> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.CountCart(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.CountCartQuan(1);
            var obj = result as OkObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_CountCartQuan_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("CounCartQuan cart exception message");
            CountCartQuan acclist = _fixture.Create<CountCartQuan>();
            _cartService.Setup(re => re.CountCart(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.CountCartQuan(1);
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
        public async Task Get_SearchProductCart_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<CartResponse> acclist = _fixture.CreateMany<CartResponse>(1).ToList();
            Task<List<CartResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _cartService.Setup(re => re.SearchCartAsync("oke")).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CartsController(_cartService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.SearchProductAsync("oke");
            var obj = result as OkObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_SearchProductCart_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("CounCartQuan cart exception message");
            List<CartResponse> acclist = _fixture.CreateMany<CartResponse>(1).ToList();
            _cartService.Setup(re => re.SearchCartAsync("oke")).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CartsController(_cartService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.SearchProductAsync("oke");
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
