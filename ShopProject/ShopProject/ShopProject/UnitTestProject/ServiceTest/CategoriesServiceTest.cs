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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.ServiceTest
{
    [TestClass]
    public class CategoriesServiceTest
    {
        private DbContextOptions<Db> options;
        private Db inMemoryDb;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private Mock<CategoryService> _caserviceMock;


        public CategoriesServiceTest()
        {
            options = new DbContextOptionsBuilder<Db>().Options;
            inMemoryDb = new Db(options);
            _mapperMock = new Mock<IMapper>();
            _fixture = new Fixture();
            _caserviceMock = new Mock<CategoryService>(inMemoryDb, _mapperMock.Object);
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
        public async Task CreateCategoriesAsync_FirstCreate()
        {
            CategoryResponse catomerResponse = new CategoryResponse()
            {
                CategoryId = 11,
                CategoryName = "Test",
            };
            var response = _caserviceMock.Object.CreateCategoryAsync(catomerResponse).Result;
            Assert.AreEqual(response, 11);
            ClearDatabate();
        }


        [TestMethod]
        public async Task UdateCategories_Ex()
        {
            try
            {
                Category acc = new Category()
                {
                    CategoryId = 2,
                    CategoryName = "Test",
                    CategoryStatus = "1",
                };
                inMemoryDb.Categorys.Add(acc);
                inMemoryDb.SaveChanges();
                CategoryResponse act = new CategoryResponse()
                {
                    CategoryId = 9388,
                    CategoryName = "Test1",

                };

                _caserviceMock.Object.UpdateCategoryAsync(act.CategoryId, act).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("XXXXX", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task UdateCategories_Valid()
        {

            Category acc = new Category()
            {
                CategoryId = 111,
                CategoryName = "Test",
                CategoryStatus = "1",
            };
            inMemoryDb.Categorys.Add(acc);
            inMemoryDb.SaveChanges();
            CategoryResponse act = new CategoryResponse()
            {
                CategoryId = 111,
                CategoryName = "Test1",
            };
            _caserviceMock.Object.UpdateCategoryAsync(act.CategoryId, act).GetAwaiter().GetResult();
            Assert.IsTrue(acc.CategoryId == act.CategoryId);
            ClearDatabate();
        }
        [TestMethod]
        public async Task GetCategoryAsync_Valid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _caserviceMock = new Mock<CategoryService>(inMemoryDb, mapper);
            Category acc = new Category()
            {
                CategoryId = 111,
                CategoryName = "Test",
                CategoryStatus = "1",
            };
            inMemoryDb.Categorys.Add(acc);
            inMemoryDb.SaveChanges();
            List<CategoryResponse> list = new List<CategoryResponse>();
            CategoryResponse cus = new CategoryResponse()
            {
                CategoryId = 111,
                CategoryName = "Test",
            };
            list.Add(cus);
            var response = _caserviceMock.Object.GetCategoryAsync().GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }

        [TestMethod]
        public async Task GetAsync_ValidEx()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryResponse>();
                });
                IMapper mapper = config.CreateMapper();
                _caserviceMock = new Mock<CategoryService>(inMemoryDb, mapper);
                List<CategoryResponse> list = new List<CategoryResponse>();
                CategoryResponse cus = new CategoryResponse()
                {
                    CategoryId = 11,
                    CategoryName = "Test",
                };
                list.Add(cus);
                var response = _caserviceMock.Object.GetCategoryAsync().GetAwaiter().GetResult();

                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("allcate", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task SearchCategory_Async()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _caserviceMock = new Mock<CategoryService>(inMemoryDb, mapper);

            Category categoryItem1 = new Category()
            {
                CategoryId = 1,
                CategoryName = "Example Product",
                CategoryStatus = "1"
            };
            inMemoryDb.Categorys.Add(categoryItem1);
            inMemoryDb.SaveChanges();

            string CategoryName = "Example Product";
            List<CategoryResponse> list = new List<CategoryResponse>();
            CategoryResponse cus = new CategoryResponse()
            {
                CategoryId = 1,
                CategoryName = "Example Product"

            };
            list.Add(cus);
            var response = _caserviceMock.Object.SearchCategoryAsync(CategoryName).GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task SearchCategoryAsync_Null()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Cart, CategoryService>();
                });
                IMapper mapper = config.CreateMapper();
                _caserviceMock = new Mock<CategoryService>(inMemoryDb, mapper);

                Category cartItem1 = new Category()
                {
                    CategoryId = 11,
                    CategoryName = "Text",
                    CategoryStatus = "1"
                };
                inMemoryDb.Categorys.Add(cartItem1);
                inMemoryDb.SaveChanges();

                string ProductName = "dsds";
                List<CartResponse> list = null;

                var response = _caserviceMock.Object.SearchCategoryAsync(ProductName).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("searchnull", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task SearchCategoryAsync_InputNull()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryResponse>();
                });
                IMapper mapper = config.CreateMapper();
                _caserviceMock = new Mock<CategoryService>(inMemoryDb, mapper);
                string ProductName = "";
                List<CategoryResponse> list = null;

                var response = _caserviceMock.Object.SearchCategoryAsync(ProductName).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("searchnull", ex.Message);
                ClearDatabate();
            }

        }
        [TestMethod]
        public async Task GetCategoryByIdAsync_Valid()
        {
            Category categoryItem = new Category()
            {
                CategoryId = 11,
                CategoryName = "Text",
                CategoryStatus = "1"

            };
            inMemoryDb.Categorys.Add(categoryItem);
            inMemoryDb.SaveChanges();
            _caserviceMock.Object.GetByIdAsync(categoryItem.CategoryId).GetAwaiter().GetResult();
            Assert.IsTrue(categoryItem.CategoryId == categoryItem.CategoryId);
            ClearDatabate();
        }

    }
}
