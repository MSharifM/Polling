using Polling.Core.DTOs.User;
using Polling.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.Services.Interfaces
{
    public interface IUserServices
    {
        Task AddUser(User user);
        Task<User> LoginUser(LoginViewModel model);
    }
}
