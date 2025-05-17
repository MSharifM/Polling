using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polling.Core.DTOs.User;
using Polling.Core.Genarator;
using Polling.Core.Sequrity;
using Polling.Core.Services.Interfaces;
using Polling.Datalayer.Context;
using Polling.Datalayer.Entities;

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

        public async Task<User> GetUserByName(string name)
        {
            return await _db.Users.SingleOrDefaultAsync(u => u.FullName == name);
        }

        public async Task<int> GetUserGroup(string name)
        {
            var user = await GetUserByName(name);
            return user.GroupId;   
        }

        
        public async Task<User> LoginUser(LoginViewModel model)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(model.Password);

            User user = await _db.Users.SingleOrDefaultAsync(u => u.StudentCode == model.StudentCode && u.Password == hashPassword);
            return user;
        }

        #endregion

        #region UserPanel

        public async Task<ShowInformationForUsrePanelViewModel> GetUserInformationForUserPanel(string name)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.FullName == name);
            if (user == null)
            {
                return null;
            }
            var result = new ShowInformationForUsrePanelViewModel()
            {
                Name = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                UserAvatar = user.UserAvatar
            };

            return result;
        }

        public async Task<EditAvatarViewModel> GetUserInformationForEditAvatar(string name)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.FullName == name);
            if (user == null)
            {
                return null;
            }
            var result = new EditAvatarViewModel()
            {
                Name = user.FullName,
                CreateDate = user.RegisterDate,
                StudentCode = user.StudentCode,
                AvatarName = user.UserAvatar
            };

            return result;
        }

        public async Task EditAvatar(EditAvatarViewModel model, string name)
        {
            if (model.Avatar == null)
                return;

            #region Save new iamge and delete old image

            string imagePath;
            if (model.AvatarName != "Default.jpg")
            {
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", model.AvatarName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }
            model.AvatarName = NameGenerator.GenerateUniqCode() + Path.GetExtension(model.Avatar.FileName);
            imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", model.AvatarName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                model.Avatar.CopyTo(stream);
            }

            #endregion

            var user = await GetUserByName(name);
            user.UserAvatar = model.AvatarName;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> EditPassword(string name, ChangePasswordViewModel model)
        {
            var user = await GetUserByName(name);
            if (PasswordHelper.EncodePasswordMd5(model.OldPassword) == user.Password)
            {
                user.Password = PasswordHelper.EncodePasswordMd5(model.NewPassword);
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }


        #endregion

        #region Polling

        public async Task<Tuple<List<ListPollsForShowToUserViewModel> , int>> GetPollsToShowForUser(int userGroup, int pageId = 1
            , string? filter = null, string getType = "all", string orderByType = "date", int take = 0)
        {
            if (take == 0)
                take = 8;

            IQueryable<Vote> result = _db.Votes;

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.Title.Contains(filter));
            }

            switch (getType)
            {
                case "all":
                    break;
                case "active":
                    result = result.Where(c => c.IsActive);
                    break;
                case "participated":
                    //ToDo check participate
                    break;
            }

            switch (orderByType)
            {
                case "date":
                    result = result.OrderByDescending(c => c.CreateDate);
                    break;
                case "enddate":
                    result = result.OrderByDescending(c => c.EndDate);
                    break;
            }

            result = result.Include(c => c.Groups).Where(v => v.Groups.Any(g => g.GroupId == userGroup));

            int skip = (pageId - 1) * take;

            int pageCount = result.Select(v => new ListPollsForShowToUserViewModel()
            {
                VoteId = v.VoteId
            }).Count() / take;

            var query = await result.Select(v => new ListPollsForShowToUserViewModel()
            {
                CreateDate = v.CreateDate,
                EndDate = v.EndDate,
                IsActive = v.IsActive,
                Text = v.Text,
                Title = v.Title,
                VoteId = v.VoteId
            }).Skip(skip).Take(take).ToListAsync();

            return Tuple.Create(query , pageCount);
        }

        public async Task AddUserVote(int userId, int voteId, List<int> OptionsId)
        {
            foreach (var item in OptionsId)
            {
                var userVote = new UserVote()
                {
                    VoteId = voteId,
                    UserId = userId,
                    OptionId = item
                };
                await _db.UsersVotes.AddAsync(userVote);
                var option = await GetOptionById(item);
                option.Count++;
                _db.Options.Update(option);
            }
                await _db.SaveChangesAsync();
        }

        public async Task<Option> GetOptionById(int id)
        {
            return await _db.Options.FindAsync(id);
        }


        #endregion
    }
}
