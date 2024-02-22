using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BussinessObject.Models;
using DataAccess.DB;
using Responsitory.DTO;
using Responsitory.IService;
namespace Responsitory.Service
{
    public class UserService : IUserService
    {
        public User user{ get; set; } = new User();
        private readonly Db _dbContext;
        private readonly IMapper _mapper;
        public User cate { get; set; }
        public List<User> categories { get; set; }
        public UserService(Db dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<int> CreateUserAsync(UserResponse UserResponse)
        {
            int count = _dbContext.Users.ToList().Count();
            if (count == 0)
            {
                user.UserID = 1;
            }
            else
            {
                count++;
                user.UserID = count;
            }

            user.FullName = UserResponse.FullName;
            user.Address = UserResponse.Address;
            user.Phone = UserResponse.Phone;
            user.UserStatus = "1";
            

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user.UserID;
        }

        public async Task DeleteUserAsync(int id)
        {
            User detemp = _dbContext.Users.SingleOrDefault(c => c.UserID == id);
            detemp.UserStatus = "0";
            _dbContext.Users.Update(detemp);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var User = await _dbContext.Users.FindAsync(id);
            return _mapper.Map<UserResponse>(User);
        }

        public async Task<List<UserResponse>> GetUserAsync()
        {
            var Users = _dbContext.Users.Where(c => c.UserStatus == "1");
            return _mapper.Map<List<UserResponse>>(Users);
        }
        public List<User> users { get; set; }
        public async Task<List<UserResponse>> SearchUserAsync(string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                users = _dbContext.Users.Where(p =>
                    p.FullName.Contains(search) && p.UserStatus == "1"
                ).ToList();
                if (users.Count == 0)
                {
                    throw new Exception("Loi DB");

                }
                return _mapper.Map<List<UserResponse>>(users);
            }
            else
            {
                throw new Exception("loi r");
            }
        }
        public async Task UpdateUserAsync(int id, UserResponse UserResponse)
        {

            if (UserResponse == null)
            {
                throw new Exception("UPOR");
            }
            var or = _dbContext.Users.Where(x => x.UserID == id).FirstOrDefault();
            if (null != or)
            {
                or.FullName = UserResponse.FullName;
                or.Address = UserResponse.Address;
                or.Phone = UserResponse.Phone;
                or.UserStatus = "1";
                _dbContext.Users.Update(or);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("idnotmatch");
            }
        }
    }
}
