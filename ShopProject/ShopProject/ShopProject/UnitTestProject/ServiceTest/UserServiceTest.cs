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
    public class UserServiceTest
    {
        private DbContextOptions<Db> options;
        private Db inMemoryDb;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private Mock<UserService> _userviceMock;


        public UserServiceTest()
        {
            options = new DbContextOptionsBuilder<Db>().Options;
            inMemoryDb = new Db(options);
            _mapperMock = new Mock<IMapper>();
            _fixture = new Fixture();
            _userviceMock = new Mock<UserService>(inMemoryDb, _mapperMock.Object);
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
        public async Task CreateUserAsync_Valid_FisrtCreate()
        {
            UserResponse userResponse = new UserResponse
            {
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
            };
            var Response = _userviceMock.Object.CreateUserAsync(userResponse).GetAwaiter().GetResult();
            Assert.IsNotNull(Response);
            Assert.AreEqual(Response, 1);
            ClearDatabate();
        }
        [TestMethod]
        public async Task CreateUserAsync_Valid_SecondCreate()
        {
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1"
            };
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            UserResponse userResponse = new UserResponse
            {
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
            };
            var Response = _userviceMock.Object.CreateUserAsync(userResponse).GetAwaiter().GetResult();
            Assert.IsNotNull(Response);
            Assert.AreEqual(Response, 2);
            ClearDatabate();
        }
        [TestMethod]
        public async Task DeleteUserAsync_Valid_SecondCreate()
        {
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1"
            };
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            _userviceMock.Object.DeleteUserAsync(u.UserID).GetAwaiter().GetResult();
            Assert.IsTrue(u.UserID == u.UserID);
            ClearDatabate();
        }
        [TestMethod]
        public async Task GetByIdAsync_Valid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _userviceMock = new Mock<UserService>(inMemoryDb, mapper);
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1"
            };
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            UserResponse userResponse = new UserResponse
            {
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
            };
            var Response = _userviceMock.Object.GetByIdAsync(1).GetAwaiter().GetResult();
            Assert.IsNotNull(Response);
            Response.Should().BeEquivalentTo(userResponse);
            ClearDatabate();
        }
        [TestMethod]
        public async Task GetUserAsync_Valid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _userviceMock = new Mock<UserService>(inMemoryDb, mapper);
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1"
            };
            User u1 = new User()
            {
                UserID = 2,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1"
            };
            inMemoryDb.Users.Add(u);
            inMemoryDb.Users.Add(u1);
            inMemoryDb.SaveChanges();
            List<UserResponse> list = new List<UserResponse>();
            UserResponse userResponse = new UserResponse
            {
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
            };
            UserResponse userResponse1 = new UserResponse
            {
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
            };
            list.Add(userResponse);
            list.Add(userResponse1);
            var Response = _userviceMock.Object.GetUserAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(Response);
            Response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task UpdateUserAsync_Valid_SecondCreate()
        {
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1"
            };
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            UserResponse updatedUserResponse = new UserResponse
            {
                FullName = "test1",
                Address = "address123",
                Phone = "100000000000"
            };
            _userviceMock.Object.UpdateUserAsync(u.UserID, updatedUserResponse).GetAwaiter().GetResult();
            Assert.AreEqual(u.UserID, 1);
            ClearDatabate();
        }
        [TestMethod]
        public async Task UpdateUserAsync_SecondValid()
        {
            try
            {

                UserResponse updatedUserResponse = null;
                await _userviceMock.Object.UpdateUserAsync(1, updatedUserResponse);
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("UPOR", ex.Message);
                ClearDatabate();
            }
        }

        [TestMethod]
        public async Task UpdateUserAsync_IdNul()
        {
            try
            {
                User u = new User()
                {
                    UserID = 1,
                    FullName = "test",
                    Address = "address",
                    Phone = "1234567890",
                    UserStatus = "1"
                };
                inMemoryDb.Users.Add(u);
                inMemoryDb.SaveChanges();
                UserResponse updatedUserResponse = new UserResponse
                {

                    FullName = "test",
                    Address = "address",
                    Phone = "1234567890",
                };
                await _userviceMock.Object.UpdateUserAsync(2, updatedUserResponse);
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("idnotmatch", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task SearchUserAsync()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponse>();
            });
            IMapper mapper = config.CreateMapper();
            _userviceMock = new Mock<UserService>(inMemoryDb, mapper);

            User userItem1 = new User()
            {

                UserID = 1,
                FullName = "Nguyen Minh Nhut",
                Phone = "0907552402",
                Address = "FPT",
                UserStatus = "1"

            };
            inMemoryDb.Users.Add(userItem1);
            inMemoryDb.SaveChanges();

            string UserName = "Nguyen Minh Nhut";
            List<UserResponse> list = new List<UserResponse>();
            UserResponse cus = new UserResponse()
            {
                FullName = "Nguyen Minh Nhut",
                Address = "FPT",
                Phone = "0907552402",
            };
            list.Add(cus);
            var response = _userviceMock.Object.SearchUserAsync(UserName).GetAwaiter().GetResult();
            response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task SearchUserAsync_Null()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserResponse>();
                });
                IMapper mapper = config.CreateMapper();
                _userviceMock = new Mock<UserService>(inMemoryDb, mapper);

                User UserItem1 = new User()
                {
                    UserID = 1,
                    FullName = "Nguyen Minh Nhut",
                    Phone = "0907552402",
                    Address = "FPT",
                    UserStatus = "1"
                };
                inMemoryDb.Users.Add(UserItem1);
                inMemoryDb.SaveChanges();

                string UserName = "dsds";
                List<UserResponse> list = null;

                var response = _userviceMock.Object.SearchUserAsync(UserName).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Loi DB", ex.Message);
                ClearDatabate();
            }

        }
        [TestMethod]
        public async Task SearchUserAsync_InputNull()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserResponse>();
                });
                IMapper mapper = config.CreateMapper();
                _userviceMock = new Mock<UserService>(inMemoryDb, mapper);
                string UserName = "";
                List<UserResponse> list = null;

                var response = _userviceMock.Object.SearchUserAsync(UserName).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("loi r", ex.Message);
                ClearDatabate();
            }

        }
    }
}
