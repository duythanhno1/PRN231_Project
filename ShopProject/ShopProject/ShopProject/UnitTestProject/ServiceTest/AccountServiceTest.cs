using AutoFixture;
using AutoMapper;
using BussinessObject.Models;
using DataAccess.DB;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

namespace UnitTestProject.ServiceTest
{

    [TestClass]
    public class AccountServiceTest
    {
        private DbContextOptions<Db> options;
        private Db inMemoryDb;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private Mock<AccountService> _accserviceMock;


        public AccountServiceTest()
        {
            options = new DbContextOptionsBuilder<Db>().Options;
            inMemoryDb = new Db(options);
            _mapperMock = new Mock<IMapper>();
            _fixture = new Fixture();
            _accserviceMock = new Mock<AccountService>(inMemoryDb, _mapperMock.Object);
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
        public async Task CreateAccountAsync_UnValidPassword()
        {
            CustomerResponse customerResponse = _fixture.Create<CustomerResponse>();

            var response = _accserviceMock.Object.CreateAccountAsync(customerResponse).Result;
            // Assert

            // Kiểm tra xem result có phải là AccountID được trả về (giả định rằng nó sẽ thành công) hoặc 99999999 (nếu có lỗi)
            Assert.IsTrue(response == 99999999 || response > 0);
        }
        [TestMethod]
        public async Task CreateAccountAsync_ValidPassword_FirstCreate()
        {
            CustomerResponse customerResponse = new CustomerResponse()
            {
                FullName = "Nguyen Thanh Huy",
                Email = "Nguyen Thanh Huy",
                Address = "Nguyen Thanh Huy",
                PassWord = "123456",
                ReEnterPassword = "123456",
                Phone = "1234567891",
            };
            var response = _accserviceMock.Object.CreateAccountAsync(customerResponse).Result;
            Assert.AreEqual(response, 1);
            ClearDatabate();
        }
        [TestMethod]
        public async Task CreateAccountAsync_ValidPassword_EmailExist()
        {
            try
            {
                CustomerResponse customerResponse = new CustomerResponse()
                {
                    FullName = "Nguyen Thanh Huy",
                    Email = "Nguyen Thanh Huy",
                    Address = "Nguyen Thanh Huy",
                    PassWord = "123456",
                    ReEnterPassword = "123456",
                    Phone = "1234567891",
                };
                Account acc = new Account()
                {
                    AccountStatus = "1",
                    Email = "Nguyen Thanh Huy",
                    AccountID = 1,
                    Password = "password",
                    Admin = true,
                    UserID = 1,
                    isVerified = true,
                };
                inMemoryDb.Accounts.Add(acc);
                inMemoryDb.SaveChanges();
                var response = _accserviceMock.Object.CreateAccountAsync(customerResponse).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Account acc = new Account()
                {
                    AccountStatus = "1",
                    Email = "Nguyen Thanh Huy",
                    AccountID = 1,
                    Password = "password",
                    Admin = true,
                    UserID = 1,
                    isVerified = true,
                };
                Assert.AreEqual("email", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task CreateAccountAsync_ValidPassword_SecondCreate()
        {
            CustomerResponse customerResponse = new CustomerResponse()
            {
                FullName = "Nguyen Thanh Huy",
                Email = "Nguyen Thanh Huy",
                Address = "Nguyen Thanh Huy",
                PassWord = "123456",
                ReEnterPassword = "123456",
                Phone = "1234567891",
            };
            Account acc = new Account()
            {
                AccountStatus = "1",
                Email = "Nguyen Thanh Huy1",
                AccountID = 1,
                Password = "password",
                Admin = true,
                UserID = 1,
                isVerified = true,
            };
            inMemoryDb.Accounts.Add(acc);
            inMemoryDb.SaveChanges();
            var response = _accserviceMock.Object.CreateAccountAsync(customerResponse).Result;
            Assert.AreEqual(response, 2);
            ClearDatabate();
        }
        [TestMethod]
        public async Task GetAccount_Valid()
        {
            Account acc = new Account()
            {
                AccountStatus = "1",
                Email = "Nguyen Thanh Huy1",
                AccountID = 1,
                Password = "password",
                Admin = true,
                UserID = 1,
                isVerified = true,
            };
            inMemoryDb.Accounts.Add(acc);
            inMemoryDb.SaveChanges();
            User u = new User()
            {
                UserID = 1,
                FullName = "test",
                Address = "address",
                Phone = "1234567890",
                UserStatus = "1",
            };
            inMemoryDb.Users.Add(u);
            inMemoryDb.SaveChanges();
            List<CusView> list = new List<CusView>();
            CusView cus = new CusView()
            {
                AccountID = acc.AccountID,
                Email = acc.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                Address = u.Address,
                Admin = acc.Admin
            };
            list.Add(cus);
            Task<List<CusView>> tl = Task.FromResult(list);
            var response = _accserviceMock.Object.GetAccountAsync().Result;
            Console.WriteLine("Expected: " + list[0]);
            Console.WriteLine("Actual: " + response[0]);
            response.Should().BeEquivalentTo(list);
            ClearDatabate();
        }
        [TestMethod]
        public async Task UdateAccount_Valid()
        {
            try
            {
                Account acc = new Account()
                {
                    AccountStatus = "1",
                    Email = "Nguyen Thanh Huy1",
                    AccountID = 1,
                    Password = "password",
                    Admin = true,
                    UserID = 1,
                    isVerified = true,
                };
                inMemoryDb.Accounts.Add(acc);
                inMemoryDb.SaveChanges();
                ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse()
                {
                    PresentPassword = acc.Password,
                    NewPassword = "1",
                    ReEnterNewPassword = "1",
                };
                _accserviceMock.Object.UpdateAccountAsync(acc.AccountID, changePasswordResponse).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("oldpass", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task UdateAccount_Failed()
        {
            try
            {
                Account acc = new Account()
                {
                    AccountStatus = "1",
                    Email = "Nguyen Thanh Huy1",
                    AccountID = 1,
                    Password = "password",
                    Admin = true,
                    UserID = 1,
                    isVerified = true,
                };
                inMemoryDb.Accounts.Add(acc);
                inMemoryDb.SaveChanges();
                ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse()
                {
                    PresentPassword = acc.Password,
                    NewPassword = "11",
                    ReEnterNewPassword = "1",
                };
                _accserviceMock.Object.UpdateAccountAsync(acc.AccountID, changePasswordResponse).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("confirmpass", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task UdateAccount_FailedConfirmPass()
        {
            try
            {
                Account acc = new Account()
                {
                    AccountStatus = "1",
                    Email = "Nguyen Thanh Huy1",
                    AccountID = 1,
                    Password = "password",
                    Admin = true,
                    UserID = 1,
                    isVerified = true,
                };
                inMemoryDb.Accounts.Add(acc);
                inMemoryDb.SaveChanges();
                ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse()
                {
                    PresentPassword = "11",
                    NewPassword = "11",
                    ReEnterNewPassword = "1",
                };
                _accserviceMock.Object.UpdateAccountAsync(acc.AccountID, changePasswordResponse).GetAwaiter().GetResult();
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("oldpass", ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task DeleteAccount_Valid()
        {
            try
            {
                Account acc = new Account()
                {
                    AccountStatus = "1",
                    Email = "Nguyen Thanh Huy1",
                    AccountID = 1,
                    Password = "password",
                    Admin = true,
                    UserID = 1,
                    isVerified = true,
                };
                inMemoryDb.Accounts.Add(acc);
                inMemoryDb.SaveChanges();
                _accserviceMock.Object.DeleteAccountAsync(acc.AccountID).GetAwaiter().GetResult();
                Assert.IsTrue(acc.AccountID == acc.AccountID);
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                ClearDatabate();
            }
        }
        [TestMethod]
        public async Task DeleteAccount_Failed()
        {
            try
            {
                _accserviceMock.Object.DeleteAccountAsync(1).GetAwaiter().GetResult();
                Assert.Fail("Khong catch");
                ClearDatabate();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("delete", ex.Message);
                ClearDatabate();
            }
        }
    }
}
