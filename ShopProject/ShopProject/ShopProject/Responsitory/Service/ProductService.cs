using AutoMapper;
using BussinessObject.Models;
using DataAccess.DB;
using Microsoft.EntityFrameworkCore;
using Responsitory.DTO;
using Responsitory.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.Service
{
    public class ProductService : IProductService
    {
        private readonly Db _dbContext;
        private readonly IMapper _mapper;
        public Product product { get; set; } = new Product();
        public Category category { get; set; } = new Category();
        public List<Product> products { get; set; }
        public List<ProductView> productViews { get; set; } = new List<ProductView>();

        public ProductService(Db dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> CreateProductAsync(ProductResponse productResponse)
        {
            if(productResponse == null)
            {
                throw new Exception("loi r");
            }
            int count = _dbContext.Products.ToList().Count;
            if (count == 0)
            {
                product.ProductID = 1;

            }
            else
            {
                count++;
                product.ProductID = count;
            }
            product.ProductName = productResponse.ProductName;
            product.ProductBatteryCapacity = productResponse.ProductBatteryCapacity;
            product.ProductChipset = productResponse.ProductChipset;
            product.ProductDetailDescription = productResponse.ProductDetailDescription;
            product.ProductImage = productResponse.ProductImage;
            product.ProductMainCamera = productResponse.ProductMainCamera;
            product.ProductPrice = productResponse.ProductPrice;
            product.ProductQuantity = productResponse.ProductQuantity;
            product.ProductSelfieCamera = productResponse.ProductSelfieCamera;
            product.ProductStorageExternal = productResponse.ProductStorageExternal;
            product.ProductStorageInternal = productResponse.ProductStorageInternal;
            product.CategoryID = productResponse.CategoryID;
            product.ProductStatus = "1";
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return product.ProductID;
        }

        public async Task DeleteProductAsync(int id)
        {
                
                Product detemp = _dbContext.Products.SingleOrDefault(p => p.ProductID == id);
                if (detemp != null)
                {
                    detemp.ProductStatus = "0";
                    _dbContext.Products.Update(detemp);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("loi r");
                }
           
        }

        public async Task<ProductView> GetByIdAsync(int id)
        {
            
            product = await _dbContext.Products.FindAsync(id);
            category = _dbContext.Categorys.Where(c => c.CategoryId == product.CategoryID).FirstOrDefault();

            ProductView pro = new ProductView()
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductBatteryCapacity = product.ProductBatteryCapacity,
                ProductChipset = product.ProductChipset,
                ProductDetailDescription = product.ProductDetailDescription,
                ProductImage = product.ProductImage,
                ProductMainCamera = product.ProductMainCamera,
                ProductPrice = product.ProductPrice,
                ProductQuantity = product.ProductQuantity,
                ProductSelfieCamera = product.ProductSelfieCamera,
                ProductStorageExternal = product.ProductStorageExternal,
                ProductStorageInternal = product.ProductStorageInternal,
                CategoryID = product.CategoryID,
                CategoryName = category.CategoryName,
            };
            return pro;
        }


        public async Task UpdateProductAsync(int id, ProductEdit productResponse)
        {
            if (productResponse == null)
            {
                throw new Exception("product null");
            }
            if (id == productResponse.ProductId)
            {
                Product or = _dbContext.Products.Where(x => x.ProductID == id).FirstOrDefault();
                or.ProductName = productResponse.ProductName;
                or.ProductBatteryCapacity = productResponse.ProductBatteryCapacity;
                or.ProductChipset = productResponse.ProductChipset;
                or.ProductDetailDescription = productResponse.ProductDetailDescription;
                or.ProductImage = productResponse.ProductImage;
                or.ProductMainCamera = productResponse.ProductMainCamera;
                or.ProductPrice = productResponse.ProductPrice;
                or.ProductQuantity = productResponse.ProductQuantity;
                or.ProductSelfieCamera = productResponse.ProductSelfieCamera;
                or.ProductStorageExternal = productResponse.ProductStorageExternal;
                or.ProductStorageInternal = productResponse.ProductStorageInternal;
                or.CategoryID = productResponse.CategoryID;
                or.ProductStatus = "1";
                _dbContext.Products.Update(or);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("idnotmatch");
            }
           
        }
        public async Task<List<ProductView>> SearchProductAsync(string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                products = _dbContext.Products.Where(p =>
                    p.ProductName.Contains(search) && p.ProductStatus == "1"
                ).ToList();
                foreach (var product in products)
                {
                    category = _dbContext.Categorys.Where(c => c.CategoryId == product.CategoryID).FirstOrDefault();
                    if (category != null)
                    {
                        ProductView pro = new ProductView()
                        {
                            ProductID = product.ProductID,
                            ProductName = product.ProductName,
                            ProductBatteryCapacity = product.ProductBatteryCapacity,
                            ProductChipset = product.ProductChipset,
                            ProductDetailDescription = product.ProductDetailDescription,
                            ProductImage = product.ProductImage,
                            ProductMainCamera = product.ProductMainCamera,
                            ProductPrice = product.ProductPrice,
                            ProductQuantity = product.ProductQuantity,
                            ProductSelfieCamera = product.ProductSelfieCamera,
                            ProductStorageExternal = product.ProductStorageExternal,
                            ProductStorageInternal = product.ProductStorageInternal,
                            CategoryID = product.CategoryID,
                            CategoryName = category.CategoryName,
                        };
                        productViews.Add(pro);
                    }
                }
            }
            else
            {

                throw new Exception("Rongclmn");
            }

            return productViews;
        }


        public async Task<List<ProductView>> GetProductAsync()
        {
            products = _dbContext.Products.Where(p => p.ProductStatus == "1").ToList();
            foreach (var product in products)
            {
                category = _dbContext.Categorys.Where(c => c.CategoryId == product.CategoryID).FirstOrDefault();
                ProductView pro = new ProductView()
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    ProductBatteryCapacity = product.ProductBatteryCapacity,
                    ProductChipset = product.ProductChipset,
                    ProductDetailDescription = product.ProductDetailDescription,
                    ProductImage = product.ProductImage,
                    ProductMainCamera = product.ProductMainCamera,
                    ProductPrice = product.ProductPrice,
                    ProductQuantity = product.ProductQuantity,
                    ProductSelfieCamera = product.ProductSelfieCamera,
                    ProductStorageExternal = product.ProductStorageExternal,
                    ProductStorageInternal = product.ProductStorageInternal,
                    CategoryID = product.CategoryID,
                    CategoryName = category.CategoryName,
                };
                productViews.Add(pro);
            }

            return productViews;
        }

        public async Task<CountProduct> CountPro()
        {
            int count = _dbContext.Products.Where(C => C.ProductStatus == "1").ToList().Count();
            CountProduct co = new CountProduct();
            co.count = count;
            return co;
        }

        public async Task<List<ProductView>> SearchProductByCateAsync(int id)
        {
            List<ProductView> productViews = new List<ProductView>();

            if (id != null)
            {
                products = _dbContext.Products.Where(p =>
                    p.CategoryID == id && p.ProductStatus == "1"
                ).ToList();

                foreach (var product in products)
                {
                    var category = _dbContext.Categorys.Where(c => c.CategoryId == product.CategoryID).FirstOrDefault();
                    if (category != null)
                    {
                        ProductView pro = new ProductView()
                        {
                            ProductID = product.ProductID,
                            ProductName = product.ProductName,
                            ProductBatteryCapacity = product.ProductBatteryCapacity,
                            ProductChipset = product.ProductChipset,
                            ProductDetailDescription = product.ProductDetailDescription,
                            ProductImage = product.ProductImage,
                            ProductMainCamera = product.ProductMainCamera,
                            ProductPrice = product.ProductPrice,
                            ProductQuantity = product.ProductQuantity,
                            ProductSelfieCamera = product.ProductSelfieCamera,
                            ProductStorageExternal = product.ProductStorageExternal,
                            ProductStorageInternal = product.ProductStorageInternal,
                            CategoryID = product.CategoryID,
                            CategoryName = category.CategoryName,
                        };
                        productViews.Add(pro);
                    }
                }
            }
            else
            {
                throw new Exception("RongCMNR");
            }

            return productViews;
        }

    }
}



