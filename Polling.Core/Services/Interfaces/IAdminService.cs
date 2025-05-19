using Polling.Core.DTOs.Admin;
using Polling.Core.DTOs.User;
using Polling.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.Services.Interfaces
{
   public interface IAdminService
    {
        Task AddUser(RegisterViewModel model);
        Task<User> GetUserById(int id);
        Task DeleteUser(int id);
        Task EditUser(EditUserViewModel model , int id);
        Task<Tuple<List<ListUsersForAdminViewModel> , int>> GetUsers(int pageId = 1, string? filter = null, int take = 0);


        Task<Group> GetGroupById(int id);
        Task AddGroup(string groupName);
        Task EditGroup(Group model);
        Task DeleteGroup(int id);
    }
}
