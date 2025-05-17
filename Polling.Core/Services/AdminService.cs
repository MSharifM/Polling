using Microsoft.EntityFrameworkCore;
using Polling.Core.DTOs.Admin;
using Polling.Core.DTOs.User;
using Polling.Core.Sequrity;
using Polling.Core.Services.Interfaces;
using Polling.Datalayer.Context;
using Polling.Datalayer.Entities;
using Polling.Datalayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly PollingContext _db;

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
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
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
    }
}
