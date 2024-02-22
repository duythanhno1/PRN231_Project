using AutoMapper;
using AutoMapper.Execution;
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
    public class AccountService : IAccountService
    {
        private readonly Db _dbContext;
        private readonly IMapper _mapper;
        public User user { get; set; } = new User();
        public List<CusView> cusl { get; set; } = new List<CusView>();
        public Account cate { get; set; } = new Account();
        public List<Account> categories { get; set; }

        public AccountService(Db dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<int> CreateAccountAsync(CustomerResponse CustomerResponse)
        {
            int countuser = _dbContext.Users.ToList().Count();
            int count = _dbContext.Accounts.ToList().Count();
            var check = _dbContext.Accounts.Where(a => a.Email == CustomerResponse.Email).ToList();
            if (check.Count > 0)
            {
                throw new Exception("email");
            }
            if (CustomerResponse.PassWord == CustomerResponse.ReEnterPassword)
            {
                if (countuser == 0)
                {
                    user.UserID = 1;
                }
                else
                {
                    user.UserID = countuser + 1;
                }

                user.FullName = CustomerResponse.FullName;
                user.Phone = CustomerResponse.Phone;
                user.Address = CustomerResponse.Address;
                user.UserStatus = "1";

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                cate = new Account()
                {
                    UserID = user.UserID,
                    Email = CustomerResponse.Email,
                    Password = CustomerResponse.PassWord,
                    isVerified = false,
                    Admin = false,
                    AccountStatus = "1"
                };
                if (count == 0)
                {
                    cate.AccountID = 1;
                }
                else
                {
                    count++;
                    cate.AccountID = count;
                }
                _dbContext.Accounts.Add(cate);
                await _dbContext.SaveChangesAsync();
                return cate.AccountID;
            }
            return 99999999;
        }


        public async Task DeleteAccountAsync(int id)
        {
            Account detemp = _dbContext.Accounts.SingleOrDefault(c => c.AccountID == id);
            if (detemp == null)
            {
                throw new Exception("delete");
            }
            detemp.AccountStatus = "0";
            _dbContext.Accounts.Update(detemp);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CusView> GetByIdAsync(int id)
        {
            var Account = await _dbContext.Accounts.FindAsync(id);
            if (Account == null)
            {
                return null;
            }

            user = _dbContext.Users.Where(U => U.UserID == Account.UserID).FirstOrDefault();
            CusView cus = new CusView()
            {
                AccountID = Account.AccountID,
                Email = Account.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                Address = user.Address,
                Admin = Account.Admin
            };


            return cus;
        }

        public async Task<List<CusView>> GetAccountAsync()
        {
            var Accounts = await _dbContext.Accounts.Where(c => c.AccountStatus == "1").ToListAsync();
            foreach (var item in Accounts)
            {
                user = _dbContext.Users.Where(U => U.UserID == item.UserID).FirstOrDefault();
                CusView cus = new CusView()
                {
                    AccountID = item.AccountID,
                    Email = item.Email,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Address = user.Address,
                    Admin = item.Admin
                };
                cusl.Add(cus);
            }
            return cusl;
        }

        public async Task UpdateAccountAsync(int id, ChangePasswordResponse changePasswordResponse)
        {
            cate = _dbContext.Accounts.Where(u => u.AccountID == id).FirstOrDefault();
            if (changePasswordResponse.PresentPassword == cate.Password)
            {
                if (changePasswordResponse.NewPassword == changePasswordResponse.ReEnterNewPassword)
                {
                    cate.Password = changePasswordResponse.NewPassword;
                    _dbContext.Accounts.Update(cate);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("confirmpass");
                }
            }
            else
            {
                throw new Exception("oldpass");
            }
        }

        public async Task<bool> CheckLog(string email, string pass)
        {
            try
            {
                var check = _dbContext.Accounts.Where(s => s.Email.Equals(email)
                && s.Password.Equals(pass)).FirstOrDefault();
                if (check != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return false;
        }

        public Account GetMemberByMail(string mail)
        {
            Account acc = null;
            try
            {
                acc = _dbContext.Accounts.Where(x => x.Email == mail).FirstOrDefault();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return acc;
        }
        public List<Account> accounts { get; set; }
        public async Task<List<AccountResponse>> SearchAccountAsync(string search)
        {



            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                accounts = _dbContext.Accounts.Where(p =>
                    p.Email.Contains(search) && p.AccountStatus == "1"
                ).ToList();

            }

            return _mapper.Map<List<AccountResponse>>(accounts);
        }
        int length = 10;
        private static Random random = new Random();
        public async Task<string> ForgotPassword(string email)
        {
            string sourceString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder randomString = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(sourceString.Length);
                randomString.Append(sourceString[index]);
            }
            cate = _dbContext.Accounts.SingleOrDefault(a => a.Email == email);
            if (cate == null)
            {
                throw new Exception("EmailNotFound");
            }
            cate.Password = randomString.ToString();
            _dbContext.Accounts.Update(cate);
            await _dbContext.SaveChangesAsync();
            return cate.Password;
        }
        public async Task<CountAccount> CountAcc()
        {
            int count = _dbContext.Accounts.Where(C => C.AccountStatus == "1").ToList().Count();
            CountAccount co = new CountAccount();
            co.count = count;
            return co;
        }

        public async Task AdminAccountAsync(int id)
        {
            var account = _dbContext.Accounts.SingleOrDefault(c => c.AccountID == id);
            if (account != null && account.Admin == false)
            {
                account.Admin = true;
                _dbContext.Accounts.Update(account);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                account.Admin = false;
                _dbContext.Accounts.Update(account);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task VerifiedAccountAsync(int id)
        {
            var account = _dbContext.Accounts.SingleOrDefault(c => c.AccountID == id);
            if (account != null)
            {
                account.isVerified = true;
                _dbContext.Accounts.Update(account);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
