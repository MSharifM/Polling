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

        #endregion
    }
}
