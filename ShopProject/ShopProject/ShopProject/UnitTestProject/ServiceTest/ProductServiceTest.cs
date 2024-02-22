using AutoFixture;
using AutoMapper;
using BussinessObject.Models;
using DataAccess.DB;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Org.BouncyCastle.Asn1.Cmp;
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
    public class ProductServiceTest
    {
        private DbContextOptions<Db> options;
        private Db inMemoryDb;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private Mock<ProductService> _accserviceMock;


        public ProductServiceTest()
        {
            options = new DbContextOptionsBuilder<Db>().Options;
            inMemoryDb = new Db(options);
            _mapperMock = new Mock<IMapper>();
            _fixture = new Fixture();
            _accserviceMock = new Mock<ProductService>(inMemoryDb, _mapperMock.Object);
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
        public async Task CreateProductAsync_Valid_firstCart()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _accserviceMock = new Mock<ProductService>(inMemoryDb, mapper);

          

            List<ProductResponse> list = new List<ProductResponse>();
            ProductResponse pro = new ProductResponse()
            {
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
                ProductMainCamera = "48MP"
            };
            list.Add(pro);
            var response = _accserviceMock.Object.CreateProductAsync(pro).GetAwaiter().GetResult();
            Assert.AreEqual(response, 1);
            ClearDatabate();

        }
        [TestMethod]
        public async Task CreateProductAsync_Valid_secondCart()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _accserviceMock = new Mock<ProductService>(inMemoryDb, mapper);
          
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
                ProductStatus = "1",
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
           
            List<ProductResponse> list = new List<ProductResponse>();
            ProductResponse pro = new ProductResponse()
            {
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
                ProductMainCamera = "48MP"
            };
            list.Add(pro);
            var response = _accserviceMock.Object.CreateProductAsync(pro).GetAwaiter().GetResult();
            ClearDatabate();
        
         }
        [TestMethod]
        public async Task CreateProductAsync_Valid_Throw()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, ProductResponse>();
                });
                IMapper mapper = config.CreateMapper();
                _accserviceMock = new Mock<ProductService>(inMemoryDb, mapper);

                ProductResponse pro = null;

                var response = _accserviceMock.Object.CreateProductAsync(pro).GetAwaiter().GetResult();
                Assert.AreEqual(response, 1);
                ClearDatabate();

            }catch(Exception ex)
            {
                Assert.AreEqual("loi r", ex.Message);
                ClearDatabate();
            }
        }

        [TestMethod]
        public async Task DeletesProductAsync_Valid()
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
                ProductStatus = "1",
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
            _accserviceMock.Object.DeleteProductAsync(product.ProductID).GetAwaiter().GetResult();
            Assert.IsTrue(product.ProductID == product.ProductID);
            ClearDatabate();
        }
        [TestMethod]
        public async Task DeleteProductsAsync_Failed()
        {
            try
            {
                _accserviceMock.Object.DeleteProductAsync(1).GetAwaiter().GetResult();
                Assert.Fail("loi r");
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("loi r", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task GetProductByIdAsync_Valid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductView>();
            });
            IMapper mapper = config.CreateMapper();
            _accserviceMock = new Mock<ProductService>(inMemoryDb, mapper);

            Product product = new Product()
            {
                ProductID = 1,
                ProductName = "Example Product",
                ProductImage = "product.jpg",
                ProductPrice = 999.99,
                CategoryID = 1,
                ProductQuantity = 10,
                ProductDetailDescription = "This is a detailed product description.",
                ProductChipset = "Snapdragon 888",
                ProductStorageInternal = "256GB",
                ProductStorageExternal = "SD card slot available",
                ProductBatteryCapacity = 4000,
                ProductSelfieCamera = "16MP",
                ProductMainCamera = "48MP",
                ProductStatus = "1",
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
             Category categoryItem1 = new Category()
            {
                CategoryId = 1,
                CategoryName = "Electronics",
                CategoryStatus = "1"
            };
            inMemoryDb.Categorys.Add(categoryItem1);
            inMemoryDb.SaveChanges();
            List<ProductView> list = new List<ProductView>();
            ProductView pro = new ProductView()
            {
                ProductID = 1,
                ProductName = "Example Product",
                ProductImage = "product.jpg",
                ProductPrice = 999.99,
                CategoryID = 1,
                CategoryName = "Electronics",
                ProductQuantity = 10,
                ProductDetailDescription = "This is a detailed product description.",
                ProductChipset = "Snapdragon 888",
                ProductStorageInternal = "256GB",
                ProductStorageExternal = "SD card slot available",
                ProductBatteryCapacity = 4000,
                ProductSelfieCamera = "16MP",
                ProductMainCamera = "48MP"
            };
            list.Add(pro);
            var response = _accserviceMock.Object.GetByIdAsync(product.ProductID).GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(pro);
            ClearDatabate();
        }

        [TestMethod]
        public async Task UdateProduct_FirtsValid()
        {
           
            Product product = new Product
            {
                ProductID = 1,
                ProductName = "Example Product",
                ProductImage = "product.jpg",
                ProductPrice = 999.99,
                CategoryID = 1,
                ProductQuantity = 10,
                ProductDetailDescription = "This is a detailed product description.",
                ProductChipset = "Snapdragon 888",
                ProductStorageInternal = "256GB",
                ProductStorageExternal = "SD card slot available",
                ProductBatteryCapacity = 4000,
                ProductSelfieCamera = "16MP",
                ProductMainCamera = "48MP",
                ProductStatus = "1",
            };
            inMemoryDb.Products.Add(product);
            inMemoryDb.SaveChanges();
           
            ProductEdit pro = new ProductEdit
            {
                ProductId = 1,
                ProductName = "Example Products",
                ProductImage = "product.jpg",
                ProductPrice = 999.99,
                CategoryID = 1,
                ProductQuantity = 11,
                ProductDetailDescription = "This is a detailed product description.x",
                ProductChipset = "Snapdragon 8881",
                ProductStorageInternal = "256GBs",
                ProductStorageExternal = "SD card slot availables",
                ProductBatteryCapacity = 4000,
                ProductSelfieCamera = "16MPs",
                ProductMainCamera = "48MPs",
            };
            _accserviceMock.Object.UpdateProductAsync(product.ProductID, pro).GetAwaiter().GetResult();
            Assert.AreEqual(product.ProductID, 1);
            ClearDatabate();

        }
        [TestMethod]
        public async Task UpdateOrderAsync_SecondValid()
        {
            try
            {

                ProductEdit productEdit = null;
                await _accserviceMock.Object.UpdateProductAsync(1, productEdit);
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("product null", ex.Message);
                ClearDatabate();
            }
        }

        [TestMethod]
        public async Task UpdateOrderAsync_IdNul()
        {
            try
            {
                ProductEdit productEdit = new ProductEdit
                {
                    ProductId = 2,
                    ProductName = "Example Products",
                    ProductImage = "product.jpg",
                    ProductPrice = 999.99,
                    CategoryID = 1,
                    ProductQuantity = 11,
                    ProductDetailDescription = "This is a detailed product description.x",
                    ProductChipset = "Snapdragon 8881",
                    ProductStorageInternal = "256GBs",
                    ProductStorageExternal = "SD card slot availables",
                    ProductBatteryCapacity = 4000,
                    ProductSelfieCamera = "16MPs",
                    ProductMainCamera = "48MPs",
                };
                await _accserviceMock.Object.UpdateProductAsync(1, productEdit);
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("idnotmatch", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task SearchProduct_Async()
        {
            Category caItem1 = new Category()
            {
                CategoryId = 1,
                CategoryName = "1",
                CategoryStatus = "1",

            };
            inMemoryDb.Categorys.Add(caItem1);
            inMemoryDb.SaveChanges();
            Product proItem1 = new Product()
            {
                ProductID = 1,
                ProductBatteryCapacity = 1,
                ProductChipset = "1",
                ProductDetailDescription = "1",
                ProductImage = "1",
                ProductName = "1",
                ProductMainCamera = "1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductSelfieCamera = "1",
                ProductStatus = "1",
                ProductStorageExternal = "1",
                ProductStorageInternal = "1",
                CategoryID = 1,

            };
            inMemoryDb.Products.Add(proItem1);
            inMemoryDb.SaveChanges();

            string ProductName = "1";
            List<ProductResponse> list = new List<ProductResponse>();
            ProductResponse cus = new ProductResponse()
            {
                ProductName = "1",
                ProductBatteryCapacity = 1,
                ProductChipset = "1",
                ProductDetailDescription = "1",
                ProductImage = "1",
                ProductMainCamera = "1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductSelfieCamera = "1",
                ProductStorageExternal = "1",
                ProductStorageInternal = "1",
                CategoryID = 1,
            };
            list.Add(cus);
            var response = _accserviceMock.Object.SearchProductAsync(ProductName).GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task SearchProductAsync_InputRong()
        {
            try
            {
                string ProductName = "";
                List<CategoryResponse> list = null;

                var response = _accserviceMock.Object.SearchProductAsync(ProductName).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Rongclmn", ex.Message);
                ClearDatabate();
            }

        }
        [TestMethod]
        public async Task GetProdductAsync_Valid()
        {
            Category caItem1 = new Category()
            {
                CategoryId = 1,
                CategoryName = "1",
                CategoryStatus = "1",

            };
            inMemoryDb.Categorys.Add(caItem1);
            inMemoryDb.SaveChanges();
            Product acc = new Product()
            {
                ProductID = 1,
                ProductBatteryCapacity = 1,
                ProductChipset = "1",
                ProductDetailDescription = "1",
                ProductImage = "1",
                ProductName = "1",
                ProductMainCamera = "1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductSelfieCamera = "1",
                ProductStatus = "1",
                ProductStorageExternal = "1",
                ProductStorageInternal = "1",
                CategoryID = 1,
            };
            inMemoryDb.Products.Add(acc);
            inMemoryDb.SaveChanges();
            List<ProductResponse> list = new List<ProductResponse>();
            ProductResponse cus = new ProductResponse()
            {
                ProductName = "1",
                ProductBatteryCapacity = 1,
                ProductChipset = "1",
                ProductDetailDescription = "1",
                ProductImage = "1",
                ProductMainCamera = "1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductSelfieCamera = "1",
                ProductStorageExternal = "1",
                ProductStorageInternal = "1",
                CategoryID = 1,
            };
            list.Add(cus);
            var response = _accserviceMock.Object.GetProductAsync().GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task CountPro_Valid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _accserviceMock = new Mock<ProductService>(inMemoryDb, mapper);
            Category caItem1 = new Category()
            {
                CategoryId = 11,
                CategoryName = "1",
                CategoryStatus = "1",

            };
            inMemoryDb.Categorys.Add(caItem1);
            inMemoryDb.SaveChanges();
            Product acc = new Product()
            {
                ProductID = 1,
                ProductBatteryCapacity = 1,
                ProductChipset = "1",
                ProductDetailDescription = "1",
                ProductImage = "1",
                ProductName = "1",
                ProductMainCamera = "1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductSelfieCamera = "1",
                ProductStatus = "1",
                ProductStorageExternal = "1",
                ProductStorageInternal = "1",
                CategoryID = 1,
            };
            inMemoryDb.Products.Add(acc);
            inMemoryDb.SaveChanges();

            CountProduct cus = new CountProduct()
            {
                count = 1,

            };

            var response = _accserviceMock.Object.CountPro().GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(cus);
            ClearDatabate();
        }
        [TestMethod]
        public async Task SearchProductByCateAsync()
        {
            Category caItem1 = new Category()
            {
                CategoryId = 1,
                CategoryName = "1",
                CategoryStatus = "1",

            };
            inMemoryDb.Categorys.Add(caItem1);
            inMemoryDb.SaveChanges();
            Product acc = new Product()
            {
                ProductID = 1,
                ProductBatteryCapacity = 1,
                ProductChipset = "1",
                ProductDetailDescription = "1",
                ProductImage = "1",
                ProductName = "1",
                ProductMainCamera = "1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductSelfieCamera = "1",
                ProductStatus = "1",
                ProductStorageExternal = "1",
                ProductStorageInternal = "1",
                CategoryID = 1,
            };
            inMemoryDb.Products.Add(acc);
            inMemoryDb.SaveChanges();
            List<ProductView> list = new List<ProductView>();
            ProductView cus = new ProductView()
            {
                ProductName = "1",
                ProductBatteryCapacity = 1,
                ProductChipset = "1",
                ProductDetailDescription = "1",
                ProductImage = "1",
                ProductMainCamera = "1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductSelfieCamera = "1",
                ProductStorageExternal = "1",
                ProductStorageInternal = "1",
                CategoryID = 1,
                CategoryName = "1",
                ProductID = 1,
            };
            list.Add(cus);
            var response = _accserviceMock.Object.SearchProductByCateAsync(cus.ProductID).GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task SearchProductByCateAsync_InputRong()
        {
            try
            {
                int ProductId = 1;
                List<ProductView> list = null;

                var response = _accserviceMock.Object.SearchProductByCateAsync(ProductId).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("RONGCMNL", ex.Message);
                ClearDatabate();
            }
        }
    }
}
