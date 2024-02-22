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
    public class CategoriesControllerTest
    {
        private Mock<ICategoryService> _categoriesService;
        private Fixture _fixture;
        private CategoriesController _controller;

        public CategoriesControllerTest()
        {
            _categoriesService = new Mock<ICategoryService>();
            _fixture = new Fixture();
        }
        [TestMethod]
        public async Task Get_Categories_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            List<CategoryResponse> acclist = _fixture.CreateMany<CategoryResponse>(1).ToList();
            Task<List<CategoryResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _categoriesService.Setup(re => re.GetCategoryAsync()).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CategoriesController(_categoriesService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetCategory();
            var obj = result as ObjectResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_Categories_ReturnNotFound()
        {
            //SetUp cứng giá trị return của repository
            List<CategoryResponse> acclist = null;
            Task<List<CategoryResponse>> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _categoriesService.Setup(re => re.GetCategoryAsync()).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CategoriesController(_categoriesService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetCategory();
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_Categories_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Categories exception message");

            _categoriesService.Setup(re => re.GetCategoryAsync()).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CategoriesController(_categoriesService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.GetCategory();
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
        public async Task Get_GetCategoriesById_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            CategoryResponse acclist = _fixture.Create<CategoryResponse>();
            Task<CategoryResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _categoriesService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CategoriesController(_categoriesService.Object);
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
            CategoryResponse acclist = null;
            Task<CategoryResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _categoriesService.Setup(re => re.GetByIdAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CategoriesController(_categoriesService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.GetById(1);
            var obj = result as NotFoundResult;

            Assert.AreEqual(404, obj.StatusCode);
        }
        [TestMethod]
        public async Task Get_GetCategoriessById_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("CartbyId exception message");

            _categoriesService.Setup(re => re.GetByIdAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CategoriesController(_categoriesService.Object);

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
        public async Task Get_DeleteCategories_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            int acclist = 1;
            Task<int> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _categoriesService.Setup(re => re.DeleteCategoryAsync(1)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CategoriesController(_categoriesService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.DeleteCategory(1);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Get_DeleteCategories_ThrowException()
        {
            // Tạo một ngoại lệ tùy ý, ví dụ: InvalidOperationException
            var exception = new InvalidOperationException("Delete category exception message");

            _categoriesService.Setup(re => re.DeleteCategoryAsync(1)).ThrowsAsync(exception);

            // Khởi tạo Controller
            _controller = new CategoriesController(_categoriesService.Object);

            // Thực hiện yêu cầu
            var result = await _controller.DeleteCategory(1);
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
        public async Task Get_UpdateCategories_ReturnOk()
        {
            //SetUp cứng giá trị return của repository
            CategoryResponse acclist = _fixture.Create<CategoryResponse>();
            Task<CategoryResponse> task = Task.FromResult(acclist);
            //Phần này định nghĩa repository nó trả ra giá trị gì nghĩa là set giá trị cứng quăng vô controller
            _categoriesService.Setup(re => re.UpdateCategoryAsync(1, acclist)).Returns(task);


            //Dòng 40 này là bỏ giá trị mới set vô controller
            _controller = new CategoriesController(_categoriesService.Object);
            //Gọi hàm controller với  rồi nó trả mấy cái Ok BadRequest NotFound đồ tùy bên controller mình để là gì
            var result = await _controller.UpdateCategory(acclist);
            var obj = result as OkResult;


            //Kiểm tra kết quả
            //Rồi check thôi
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCategory_ExceptionMessage()
        {
            CategoryResponse categoryResponse = _fixture.Create<CategoryResponse>();
            var exceptionMessage = "Update failed";

            _categoriesService.Setup(re => re.UpdateCategoryAsync(categoryResponse.CategoryId, categoryResponse))
                .ThrowsAsync(new Exception(exceptionMessage));

            _controller = new CategoriesController(_categoriesService.Object);

            var result = await _controller.UpdateCategory(categoryResponse);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;

            Assert.AreEqual(400, badRequestResult.StatusCode);

            var exceptionFromController = badRequestResult.Value as string;
            Assert.IsNotNull(exceptionFromController);
            Assert.AreEqual(exceptionMessage, exceptionFromController);
        }

        [TestMethod]
        public async Task Get_CreateCategories_ReturnOk()
        {
            CategoryResponse categoryResponse = _fixture.Create<CategoryResponse>();

            int newCategoryId = categoryResponse.CategoryId; // Replace with the expected category ID.
            Task<CategoryResponse> taskview = Task.FromResult(categoryResponse);
            _categoriesService.Setup(re => re.CreateCategoryAsync(categoryResponse)).ReturnsAsync(newCategoryId);
            _categoriesService.Setup(re => re.GetByIdAsync(It.IsAny<int>())).Returns(taskview);
            // Initialize the controller with the mocked service
            _controller = new CategoriesController(_categoriesService.Object);

            // Act
            var result = await _controller.CreateCategory(categoryResponse);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }

        [TestMethod]
        public async Task Get_CreateCategories_ThrowException()
        {
            // Create an exception of your choice, e.g., InvalidOperationException
            var exception = new InvalidOperationException("Create exception message");

            _categoriesService.Setup(re => re.CreateCategoryAsync(It.IsAny<CategoryResponse>())).ThrowsAsync(exception);

            // Initialize the controller with the mocked service
            _controller = new CategoriesController(_categoriesService.Object);

            // Create a CategoryResponse object or initialize it with necessary data
            CategoryResponse categoryResponse = new CategoryResponse
            {
                // Initialize with the required data.
            };

            // Act: Call the controller method with the CategoryResponse object
            var result = await _controller.CreateCategory(categoryResponse);

            // Assert: Check if the result is a BadRequestResult
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            var badRequestResult = (BadRequestResult)result;

            // Assert: Check the status code in the BadRequestResult
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task SearchProductAsync_SuccessfulSearch_ReturnsOk()
        {
            // Arrange
            var search = "exampleSearchString";
            var categoryResponses = _fixture.CreateMany<CategoryResponse>(1).ToList();

            _categoriesService.Setup(service => service.SearchCategoryAsync(search))
                .ReturnsAsync(categoryResponses);

            // Initialize the controller with the mocked service
            _controller = new CategoriesController(_categoriesService.Object);

            // Act: Call the controller method that you want to test
            var result = await _controller.SearchProductAsync(search);

            // Assert: Check if the result is an OkObjectResult
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;

            Assert.AreEqual(200, okResult.StatusCode);

        }
        [TestMethod]
        public async Task SearchProduct_Return_ExceptionMessage()
        {
            var search = "exampleSearchString";
            var exceptionMessage = "Search failed";

            _categoriesService.Setup(service => service.SearchCategoryAsync(search))
                .ThrowsAsync(new Exception(exceptionMessage));

            _controller = new CategoriesController(_categoriesService.Object);

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
