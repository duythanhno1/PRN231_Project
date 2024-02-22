using AutoMapper.Execution;
using BussinessObject.Models;
using Responsitory.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.IService
{
    public interface IAccountService
    {
        public Task<int> CreateAccountAsync(CustomerResponse CustomerResponse);
        public Task UpdateAccountAsync(int id, ChangePasswordResponse changePasswordResponse);
        public Task DeleteAccountAsync(int id);
        public Task<CusView> GetByIdAsync(int id);

        public Task<List<AccountResponse>> SearchAccountAsync(string search);
        public Task<List<CusView>> GetAccountAsync();
        public Task<string> ForgotPassword (string email);
        public Task<bool> CheckLog(string email, string pass);
        public Account GetMemberByMail(string mail);
        public Task AdminAccountAsync(int id);
        public Task VerifiedAccountAsync(int id);
        public Task<CountAccount> CountAcc();
    }
}
