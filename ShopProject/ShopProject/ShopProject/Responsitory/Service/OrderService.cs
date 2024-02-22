using AutoMapper;
using BussinessObject.Models;
using DataAccess.DB;
using Responsitory.DTO;
using Responsitory.IService;
using System;
using Microsoft.EntityFrameworkCore;
using Responsitory.DTO;
using Responsitory.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.Service
{
    public class OrderService : IOrderService
    {
        private readonly DataAccess.DB.Db _dbContext;
        private readonly IMapper _mapper;
        public User product { get; set; } = new User();
        public List<OrderDetail> orderdetail { get; set; } = new List<OrderDetail>();
        public Order order { get; set; } = new Order();
        public Product pro { get; set; } = new Product();
        public List<Order> orders { get; set; }
        public List<OrderAdminResponse> orderadminresponses { get; set; } = new List<OrderAdminResponse>();
        public OrderDetail ordersdetail { get; set; } = new OrderDetail(); 
        public List<Cart> cart { get; set; }
        public OrderService(DataAccess.DB.Db dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<int> CreateOrderAsync(int userid)
        {
            cart = _dbContext.Carts.Where(C=> C.UserID == userid && C.CartStatus == "1").ToList();
            int countDetail = _dbContext.OrderDetails.ToList().Count();
            int countCart = cart.Count;
            int count = _dbContext.Orders.ToList().Count();
            if(count == 0) {
                order.OrderID = 1;
            }
            else
            {
                count++;
                order.OrderID = count;
            }
                order.UserID = userid;
                order.OrderDate = DateTime.Now;
                order.OrderQuantity = countCart;
                order.ShipperDate = DateTime.Now.AddDays(5);
                order.OrderNote = "Waiting for confirm";
                order.OrderStatus = "1";
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            foreach (var item in cart)
            {
                pro = _dbContext.Products.Where(p=> p.ProductID == item.ProductID).FirstOrDefault();
                var ordersdetail = new OrderDetail();
                if (countDetail == 0)
                {
                    ordersdetail.OrderDetailID = 1;
                }
                else
                {
                    ordersdetail.OrderDetailID = countDetail + 1;
                }
                ordersdetail.OrderDetailTotalPrice = item.Price;
                ordersdetail.OrderDetailQuantity = item.CartQuantity;
                ordersdetail.OrderID = order.OrderID;
                ordersdetail.ProductID = item.ProductID;
                ordersdetail.UserID = userid;
                ordersdetail.OrderDetailStatus = "1";
                _dbContext.OrderDetails.Add(ordersdetail);
                item.CartStatus = "0";
                _dbContext.Carts.Update(item);
                countDetail++;
                pro.ProductQuantity = pro.ProductQuantity - 1;
                _dbContext.Products.Update(pro);
                await _dbContext.SaveChangesAsync();
            }
            return order.OrderID;
        }

        public async Task DeleteOrderAsync(int id)
        {
            Order detemp = _dbContext.Orders.SingleOrDefault(c => c.OrderID== id);
            detemp.OrderStatus = "0";
            _dbContext.Orders.Update(detemp);
            await _dbContext.SaveChangesAsync();
        }
        public async Task PlussOrderAsync(int id)
        {
            var order = _dbContext.Orders.SingleOrDefault(c => c.OrderID == id);

            if (order != null)
            {
                order.OrderQuantity = order.OrderQuantity + 1;
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task SubOrderAsync(int id)
        {
            var order = _dbContext.Orders.SingleOrDefault(c => c.OrderID == id);

            if (order != null)
            {
                order.OrderQuantity = order.OrderQuantity - 1;
                if (order.OrderQuantity <= 0)
                {
                    _dbContext.Orders.Remove(order);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    _dbContext.Orders.Update(order);
                    await _dbContext.SaveChangesAsync();
                }

            }
        }

        public async Task<OrderResponse> GetByIdAsync(int id)
        {
            order = await _dbContext.Orders.Where(c => c.OrderStatus == "1" && c.OrderID == id).FirstOrDefaultAsync();
            product = _dbContext.Users.Where(u => u.UserID == order.UserID).SingleOrDefault();
            OrderResponse orderResponse = new OrderResponse()
            {
                OrderID = order.OrderID,
                FullName = product.FullName,
                Address = product.Address,
                Phone = product.Phone,
                OrderDate = order.OrderDate,
                OrderQuantity = order.OrderQuantity,
                OrderNote = order.OrderNote,
                ShipperDate = order.ShipperDate
            };
            return orderResponse;
        }

        public async Task<List<OrderAdminResponse>> GetOrderAsync()
        {
            orders = await _dbContext.Orders.Where(c => c.OrderStatus == "1").ToListAsync();
            if (orders.Count == 0)
            {
                throw new Exception("nullOrder");
            }
            foreach (var order in orders) {
            product = _dbContext.Users.Where(u => u.UserID == order.UserID).SingleOrDefault();
                if (product == null)
                {
                    throw new Exception("nullProduct");
                }
                OrderAdminResponse orderAdminResponse = new OrderAdminResponse()
                {
                   OrderID = order.OrderID,
                   FullName = product.FullName,
                    Address = product.Address,
                    Phone = product.Phone,
                    OrderDate = order.OrderDate,
                   OrderQuantity = order.OrderQuantity,
                   OrderNote = order.OrderNote,
                   ShipperDate = order.ShipperDate
                };
                orderadminresponses.Add(orderAdminResponse);
            }
           
            return orderadminresponses;
        }
        public async Task UpdateOrderAsync(int id, OrderResponse orderResponse)
        {
            if (orderResponse == null)
            {
                throw new Exception("UPOR");
            }
            if (id == orderResponse.OrderID)
            {
                Order or = _dbContext.Orders.Where(x => x.OrderID == id).FirstOrDefault();
                or.OrderDate = orderResponse.OrderDate;
                or.OrderQuantity = orderResponse.OrderQuantity;
                or.ShipperDate = orderResponse.ShipperDate;
                or.OrderNote = orderResponse.OrderNote;
                _dbContext.Orders.Update(or);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("idnotmatch");
            }
        }
        public async Task<CountOrder> CountOr()
        {
            int count = _dbContext.Orders.Where(C => C.OrderStatus == "1").ToList().Count();
            CountOrder co = new CountOrder();
            co.count = count;
            return co;
        }

        public async Task<List<OrderResponse>> GetOrderHistoryAsync(int id)
        {
            var orders = await _dbContext.Orders.Where(c => c.UserID == id && c.OrderStatus == "1").ToListAsync();
            if (orders.Count == 0)
            {
                throw new Exception("error");
            }
            return _mapper.Map<List<OrderResponse>>(orders);
        }

        public async Task<OrderTotalPrice> TotalPrice()
        {
            double total = 0;
            OrderTotalPrice co = new OrderTotalPrice();
            orderdetail = await _dbContext.OrderDetails.Where(C => C.OrderDetailStatus == "1").ToListAsync();
            foreach (var item in orderdetail)
            {
                co.TotalPrice = co.TotalPrice + item.OrderDetailTotalPrice;
            }
            return co;
        }
        public async Task ConfirmOrdeAsync(int id)
        {
             order = await _dbContext.Orders.Where(c => c.OrderID == id && c.OrderStatus == "1").FirstOrDefaultAsync();
            if (order != null)
            {
                order.OrderNote = "Confirm";
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
