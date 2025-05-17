using Polling.Core.DTOs.Admin;
using Polling.Core.DTOs.User;
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
        Task <Tuple<List<ListUsersForAdminViewModel> , int>> GetUsers(int pageId = 1, string? filter = null, int take = 0);

    }
}
