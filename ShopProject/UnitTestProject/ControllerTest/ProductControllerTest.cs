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

namespace UnitTestProject.ControllerTest
{
    [TestClass]
    public class ProductControllerTest
    {
        private Mock<IProductService> _productService;
        private Fixture _fixture;
        private ProductsController _controller;
        public ProductControllerTest()
        {
            _productService = new Mock<IProductService>();
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Product_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<ProductView> acclist = _fixture.CreateMany<ProductView>(1).ToList();
            Task<List<ProductView>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _productService.Setup(re => re.GetProductAsync()).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new ProductsController(_productService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetProducts();
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_Product_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Product exception message");

            _productService.Setup(re => re.GetProductAsync()).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.GetProducts();
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
        public async Task Get_GetProductById_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            ProductView acclist = _fixture.Create<ProductView>();
            Task<ProductView> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _productService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new ProductsController(_productService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_GetProductById_ReturnNotFound()
        {
            //SetUp cứng giá trị return của repository
            ProductView acclist = null;
            Task<ProductView> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _productService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new ProductsController(_productService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_GetProductById_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("product by Id exception message");

            _productService.Setup(re => re.GetByIdAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);
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
        public async Task Get_CreateProduct_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            Product pro = _fixture.Create<Product>();

            ProductResponse proRe = _fixture.Create<ProductResponse>();
            ProductView proView = _fixture.Create<ProductView>();
            Task<int> taskAcciD = Task.FromResult(pro.ProductID);
            Task<ProductView> taskCusview = Task.FromResult(proView);
            _productService.Setup(re => re.CreateProductAsync(It.IsAny<ProductResponse>())).Returns(taskAcciD);
            _productService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).Returns(taskCusview);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.CreateProduct(proRe);
            var obj = result as ObjectResult;

            Assert.AreEqual(200, obj.StatusCode);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task Get_CreateProduct_NotFound()
        {
            Product pro = _fixture.Create<Product>();
            ProductResponse proRe = _fixture.Create<ProductResponse>();
            ProductView proView = null;
            Task<int> taskAcciD = Task.FromResult(pro.ProductID);
            Task<ProductView> taskCusview = Task.FromResult(proView);
            _productService.Setup(re => re.CreateProductAsync(It.IsAny<ProductResponse>())).Returns(taskAcciD);
            _productService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).Returns(taskCusview);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.CreateProduct(proRe);
            var obj = result as NotFoundResult;
            Assert.AreEqual(404, obj.StatusCode);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Get_CreateProduct_ThrowException()
        {
            var exception = new InvalidOperationException("Create product exception message");
            Product pro = _fixture.Create<Product>();
            ProductResponse proRe = _fixture.Create<ProductResponse>();
            _productService.Setup(re => re.CreateProductAsync(proRe)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);
            // Thực hiện yêu cầu
            var result = await _controller.CreateProduct(proRe);
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
        public async Task Get_CreateProduct_GetIdThrowException()
        {
            var exception = new InvalidOperationException("Create product bad not found exception message");
            Product pro = _fixture.Create<Product>();

            ProductResponse proRe = _fixture.Create<ProductResponse>();
            ProductView proView = _fixture.Create<ProductView>();
            Task<int> taskAcciD = Task.FromResult(pro.ProductID);
            Task<ProductView> taskCusview = Task.FromResult(proView);
            _productService.Setup(re => re.CreateProductAsync(It.IsAny<ProductResponse>())).Returns(taskAcciD);
            _productService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(exception);
            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);
            // Thực hiện yêu cầu
            var result = await _controller.CreateProduct(proRe);
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
        public async Task Get_UpdateProduct_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            ProductEdit acclist = _fixture.Create<ProductEdit>();
            Task<ProductEdit> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _productService.Setup(re => re.UpdateProductAsync(1, acclist)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new ProductsController(_productService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.UpdateProduct(acclist);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_UpdateProduct_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Update cart exception message");
            ProductEdit acclist = _fixture.Create<ProductEdit>();
            _productService.Setup(re => re.UpdateProductAsync(acclist.ProductId, acclist)).Throws(exception);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.UpdateProduct(acclist);
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
        public async Task Get_DeleteProduct_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            int acclist = 1;
            Task<int> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _productService.Setup(re => re.DeleteProductAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new ProductsController(_productService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.DeleteProduct(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_DeleteProduct_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Delete product exception message");

            _productService.Setup(re => re.DeleteProductAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.DeleteProduct(1);
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
        public async Task Get_SearchProduct_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<ProductView> acclist = _fixture.CreateMany<ProductView>(1).ToList();
            Task<List<ProductView>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _productService.Setup(re => re.SearchProductAsync("oke")).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new ProductsController(_productService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.SearchProductAsync("oke");
            var obj = result as OkObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_SearchProduct_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("search product cart exception message");
            List<ProductView> acclist = _fixture.CreateMany<ProductView>(1).ToList();
            _productService.Setup(re => re.SearchProductAsync("oke")).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);

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
        [TestMethod]
        public async Task Get_SearchProductByCate_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<ProductView> acclist = _fixture.CreateMany<ProductView>().ToList();
            Task<List<ProductView>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _productService.Setup(re => re.SearchProductByCateAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new ProductsController(_productService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.SearchProductByCateAsync(1);
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_SearchProductByCate_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Search Product By Cate exception message");

            _productService.Setup(re => re.SearchProductByCateAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);
            // Thực hiện yêu cầu
            var result = await _controller.SearchProductByCateAsync(1);
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
        public async Task Get_CountProduct_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            CountProduct acclist = _fixture.Create<CountProduct>();
            Task<CountProduct> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _productService.Setup(re => re.CountPro()).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new ProductsController(_productService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.Count();
            var obj = result as OkObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_CountProduct_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Count cart exception message");

            _productService.Setup(re => re.CountPro()).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new ProductsController(_productService.Object);

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
    }
}
