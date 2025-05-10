using Microsoft.EntityFrameworkCore;
using Polling.Core.DTOs.User;
using Polling.Core.Genarator;
using Polling.Core.Sequrity;
using Polling.Core.Services.Interfaces;
using Polling.Datalayer.Context;
using Polling.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<bool> EditPassword(string name , ChangePasswordViewModel model)
        {
            var user = await GetUserByName(name);
            if(PasswordHelper.EncodePasswordMd5(model.OldPassword) == user.Password)
            {
                user.Password = PasswordHelper.EncodePasswordMd5(model.NewPassword);
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }


        #endregion
    }
}
