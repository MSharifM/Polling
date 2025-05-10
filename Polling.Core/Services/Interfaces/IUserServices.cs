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
        Task<User> GetUserByName(string name);
        Task AddUser(User user);
        Task<User> LoginUser(LoginViewModel model);
        Task<ShowInformationForUsrePanelViewModel> GetUserInformationForUserPanel(string name);
        Task<EditAvatarViewModel> GetUserInformationForEditAvatar(string name);
        Task EditAvatar(EditAvatarViewModel model , string name);
        Task<bool> EditPassword(string name , ChangePasswordViewModel model);
    }
}
