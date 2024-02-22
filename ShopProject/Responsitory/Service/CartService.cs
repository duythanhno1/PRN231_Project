using AutoMapper;
using BussinessObject.Models;
using DataAccess.DB;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cmp;
using Responsitory.DTO;
using Responsitory.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Responsitory.Service
{
    public class CartService : ICartService
    {
        private readonly Db _dbContext;
        private readonly IMapper _mapper;
        public Cart cart { get; set; } = new Cart();
        public List<Cart> carts { get; set; }
        public List<CustomCartResponse> customCartResponses { get; set; } = new List<CustomCartResponse> { };
        public List<Cart> cartcheck { get; set; }
        public Product product { get; set; } = new Product();
        public User user { get; set; }

        public CartService(Db dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> CreateCartAsync(int proid , int userid)
        {
            try
            {
                product = await _dbContext.Products.SingleOrDefaultAsync(c => c.ProductID == proid);
                user = await _dbContext.Users.SingleOrDefaultAsync(c => c.UserID == userid);
                int count = _dbContext.Carts.ToList().Count();
                if (count == 0)
                {
                    cart.CartId = 1;
                    cart.UserID = user.UserID;
                    cart.ProductID = product.ProductID;
                    cart.ProductName = product.ProductName;
                    cart.CartQuantity = 1;
                    cart.ProductImage = product.ProductImage;
                    cart.Price = product.ProductPrice * cart.CartQuantity;
                    cart.CartStatus = "1";
                }
                else
                {
                    count++;
                    cart.CartId = count;
                    cart.UserID = user.UserID;
                    cart.ProductID = product.ProductID;
                    cart.ProductName = product.ProductName;
                    cart.CartQuantity = 1;
                    cart.ProductImage = product.ProductImage;
                    cart.Price = product.ProductPrice * cart.CartQuantity;
                    cart.CartStatus = "1";
                }
                UpdateCart(cart);
                await _dbContext.SaveChangesAsync();
                return cart.CartId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteCartAsync(int id)
        {
            var cart = _dbContext.Carts.SingleOrDefault(c => c.CartId == id);

            if (cart != null)
            {
                cart.CartStatus="0";
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("delete");
            }    
        }
        public async Task PlussCartAsync(int id)
        {
            var cart = _dbContext.Carts.SingleOrDefault(c => c.CartId == id);

            if (cart != null)
            {
                int temp = cart.CartQuantity;
                double tempprice = cart.Price / temp;
                cart.CartQuantity = cart.CartQuantity + 1;
                cart.Price = tempprice * cart.CartQuantity;
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("plus");
            }
        }
        public async Task SubCartAsync(int id)
        {
            var cart = _dbContext.Carts.SingleOrDefault(c => c.CartId == id);

            if (cart != null)
            {
                int temp = cart.CartQuantity;
                double tempprice = cart.Price / temp;
                cart.CartQuantity = cart.CartQuantity - 1;
                cart.Price = tempprice * cart.CartQuantity;
                if (cart.CartQuantity <= 0)
                {
                    cart.CartStatus = "0";
                    _dbContext.Carts.Update(cart);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    _dbContext.Carts.Update(cart);
                    await _dbContext.SaveChangesAsync();
                }
              
            }
            else
            {
                throw new Exception("sub");
            }
        }
        public async Task<CartResponse> GetCartByIdAsync(int id)
        {
            var cart = await _dbContext.Carts.FindAsync(id);
            return _mapper.Map<CartResponse>(cart);
        }

        //public Cart CheckCart(Cart cart)
        //{
        //    var userCart = _dbContext.Carts
        //        .Where(c => c.UserID == cart.UserID && c.ProductID == cart.ProductID && c.CartStatus == "1")
        //        .FirstOrDefault();

        //    if (userCart != null)
        //    {
        //        // Người dùng đã mua sản phẩm này trước đó, cập nhật số lượng
        //        userCart.CartQuantity ++;
        //        _dbContext.Carts.Update(userCart);
        //    }
        //    else
        //    {
        //        _dbContext.Carts.Add(cart);
        //    }
        //    _dbContext.SaveChanges();

        //    return userCart;
        //}
        public Cart UpdateCart(Cart cart)
        {
            var userCart = _dbContext.Carts
                .Where(c => c.UserID == cart.UserID && c.ProductID == cart.ProductID && c.CartStatus == "1")
                .FirstOrDefault();

            if (userCart != null)
            {
                // Người dùng đã mua sản phẩm này trước đó, cập nhật số lượng
                int temp = userCart.CartQuantity;
                double tempprice = userCart.Price / temp;
                userCart.CartQuantity = userCart.CartQuantity + 1;
                userCart.Price = tempprice * userCart.CartQuantity;
                _dbContext.Carts.Update(userCart);
            }
            else
            {
                _dbContext.Carts.Add(cart);
            }
            _dbContext.SaveChanges();

            return userCart;
        }


        public async Task<List<CustomCartResponse>> GetCartsAsync(int id)
        {
            var carts = await _dbContext.Carts.Where(c => c.UserID == id && c.CartStatus == "1").ToListAsync();
            foreach(var cart in carts)
            {
                user = _dbContext.Users.Where(u => u.UserID == cart.UserID).FirstOrDefault();
                product = _dbContext.Products.Where(u => u.ProductID == cart.ProductID).FirstOrDefault();
                var cusCartResponse = new CustomCartResponse()
                {
                    CartId = cart.CartId,
                    FullName = user.FullName,
                    Address = user.Address,
                    Phone = user.Phone,
                    CartQuantity = cart.CartQuantity,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    Price = cart.Price
                };
                customCartResponses.Add(cusCartResponse);
            }
            return customCartResponses;
        }



        public async Task UpdateCartAsync(int id, CartResponse cartResponse)
        {
            var cart = await _dbContext.Carts.FindAsync(id);

            if (cart != null)
            {
                cart.ProductID = cartResponse.ProductID;
                cart.ProductName = cartResponse.ProductName;
                cart.CartQuantity = cartResponse.CartQuantity;
                cart.ProductImage = cartResponse.ProductImage;
                cart.Price = cartResponse.Price;
                

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<CartResponse>> SearchCartAsync(string search)
        {



            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                carts = _dbContext.Carts.Where(p =>
                    p.ProductName.Contains(search) && p.CartStatus == "1"
                ).ToList();
               
            }

            return _mapper.Map<List<CartResponse>>(carts);
        }
        public int CountCart()
        {
            int count = _dbContext.Carts.ToList().Count();

            return count;
        }

        public async Task<CartTotalPrice> TotalPrice(int id)
        {
            double total = 0;
            CartTotalPrice to = new CartTotalPrice();
            carts = await _dbContext.Carts.Where(C => C.CartStatus == "1" && C.UserID == id).ToListAsync();

            if (carts.Count == 0)
            {

                throw new Exception("loi r");

            }
            foreach (var item in carts)
            {
                to.TotalPrice = to.TotalPrice + item.Price;
            }
       
            return to;
        }

        public async Task<CountCartQuan> CountCart(int id)
        {
            double total = 0;
            CountCartQuan to = new CountCartQuan();
            carts = await _dbContext.Carts.Where(C => C.CartStatus == "1" && C.UserID == id).ToListAsync();
            if(carts.Count == 0 ){

                throw new Exception("loi r");
                    
            }
            foreach (var item in carts)
            {
                to.count = to.count + item.CartQuantity;
            }
            return to;
        }

        public List<Cart> CheckCart(List<Cart> carts)
        {
            throw new NotImplementedException();
        }
    }
}
