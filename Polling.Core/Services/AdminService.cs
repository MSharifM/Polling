using Microsoft.EntityFrameworkCore;
using Polling.Core.DTOs.Admin;
using Polling.Core.DTOs.User;
using Polling.Core.Sequrity;
using Polling.Core.Services.Interfaces;
using Polling.Datalayer.Context;
using Polling.Datalayer.Entities;

namespace Polling.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly PollingContext _db;

        #region User management

        public AdminService(PollingContext db)
        {
            _db = db;
        }

        public async Task AddUser(RegisterViewModel model)
        {
            var user = new User()
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                StudentCode = model.StudentCode,
                Password = PasswordHelper.EncodePasswordMd5(model.Password),
                GroupId = model.GroupId,
                RegisterDate = DateTime.Now,
                UserAvatar = "Default.jpg",
                IsActive = true,
                ActiveCode = Guid.NewGuid().ToString(),
                IsDelete = false,
                IsAdmin = model.IsAdmin
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await GetUserById(id);
            user.IsDelete = true;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task EditUser(EditUserViewModel model , int id)
        {
            var user = await GetUserById(id);

            user.FullName = model.FullName;
            user.Phone = model.Phone;
            user.StudentCode = model.StudentCode;
            user.IsAdmin = model.IsAdmin;
            user.GroupId = model.GroupId;
            
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public Task<User> GetUserById(int id)
        {
            return _db.Users
                .Include(u => u.Group)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<Tuple<List<ListUsersForAdminViewModel>, int>> GetUsers(int pageId = 1, string? filter = null, int take = 0)
        {
            if (take == 0)
                take = 8;

            IQueryable<User> result = _db.Users.Include(u => u.Group);

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.FullName.Contains(filter));
            }

            int skip = (pageId - 1) * take;

            int pageCount = result.Select(v => new ListUsersForAdminViewModel()
            {
                UserId = v.UserId,
            }).Count() / take;

            var query = await result.Select(v => new ListUsersForAdminViewModel()
            {
                UserId = v.UserId,
                Email = v.Email,
                Group = v.Group.Name,
                Name = v.FullName,
                Phone = v.Phone,
                StudentCode = v.StudentCode,
            }).Skip(skip).Take(take).ToListAsync();

            return Tuple.Create(query, pageCount);
        }

        public async Task<bool> IsAdmin(string userName)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.FullName == userName);
            if (user.IsAdmin == true)
                return true;

            return false;
        }

        #endregion

        #region Group management

        public async Task<Group> GetGroupById(int id)
        {
            return await _db.Groups.FindAsync(id);
        }

        public async Task AddGroup(string groupName)
        {
            var group = new Group()
            {
                Name = groupName,
            };
            await _db.Groups.AddAsync(group);
            await _db.SaveChangesAsync();
        }

        public async Task EditGroup(Group model)
        {
            Group group = await _db.Groups.FindAsync(model.GroupId);
            if (group != null)
            {
                group.Name = model.Name;
                _db.Groups.Update(group);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteGroup(int id)
        {
            var group = await GetGroupById(id);
            group.IsDelete = true;
            _db.Groups.Update(group);
            await _db.SaveChangesAsync();
        }

        #endregion
    }
}
