using Microsoft.EntityFrameworkCore;
using Polling.Core.DTOs.User;
using Polling.Core.Sequrity;
using Polling.Core.Services.Interfaces;
using Polling.Datalayer.Context;
using Polling.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.Services
{
    public class UserService : IUserServices
    {
        private PollingContext _db;

        public UserService(PollingContext context)
        {
            _db = context;
        }

        #region Account

        public async Task AddUser(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }
        
        public async Task<User> LoginUser(LoginViewModel model)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(model.Password);

            User user = await _db.Users.SingleOrDefaultAsync(u => u.StudentCode == model.StudentCode && u.Password == hashPassword);
            return user;
        }

        #endregion
    }
}
