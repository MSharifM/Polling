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
        #region Account

        Task<User> GetUserByName(string name);
        Task<User> LoginUser(LoginViewModel model);
        Task<ShowInformationForUsrePanelViewModel> GetUserInformationForUserPanel(string name);
        Task<EditAvatarViewModel> GetUserInformationForEditAvatar(string name);
        Task EditAvatar(EditAvatarViewModel model , string name);
        Task<bool> EditPassword(string name , ChangePasswordViewModel model);
        Task<int> GetUserGroup(string name);

        #endregion

        #region Polling

        Task<Tuple<List<ListPollsForShowToUserViewModel> , int>> GetPollsToShowForUser(int userGroup , int pageId = 1
            , string? filter = null , string getType = "all", string orderByType = "date", int take = 0);

        Task AddUserVote(int userId, int voteId , List<int> OptionsId);
        Task<Option> GetOptionById(int id);

        #endregion
    }
}
