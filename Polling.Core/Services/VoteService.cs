using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polling.Core.DTOs.Admin;
using Polling.Core.DTOs.User;
using Polling.Core.DTOs.Vote;
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
    public class VoteService : IVoteService
    {
        private PollingContext _db;

        public VoteService(PollingContext db)
        {
            _db = db;
        }

        public async Task AddOptions(int voteId, List<CreateOptionViewModel> models)
        {
            var vote = await _db.Votes.FindAsync(voteId);
            foreach (var item in models)
            {
                await _db.Options.AddAsync(new Option()
                {
                    Text = item.Text,
                    VoteId = voteId
                });
            }
            await _db.SaveChangesAsync();
        }

        public async Task<int> AddVote(CreateVoteViewModel model, List<int> groupsId)
        {
            var vote = new Vote()
            {
                Title = model.Title,
                Text = model.Text,
                AllowMultipleSelection = model.AllowMultipleSelection,
                IsActive = model.IsActive,
                Groups = _db.Groups.Where(g => groupsId.Any(i =>i == g.GroupId)).ToList()
            };

            await _db.Votes.AddAsync(vote);
            await _db.SaveChangesAsync();
            return vote.VoteId;
        }

        public async Task DeleteVote(int voteId)
        {
            var vote = await GetVoteById(voteId);
            vote.IsDelete = true;
            _db.Votes.Update(vote);
            await _db.SaveChangesAsync();
        }

        public async Task<List<SelectListItem>> GetAllGroups()
        {
            return await _db.Groups.Select(g => new SelectListItem()
            {
                Text = g.Name,
                Value = g.GroupId.ToString()
            }).ToListAsync();
        }

        public async Task<Tuple<List<ListPollsForShowToAdmin>, int>> GetPollsToShowForUser(int pageId = 1, string? filter = null, string getType = "all", string orderByType = "date", int take = 0)
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

            int skip = (pageId - 1) * take;

            int pageCount = result.Select(v => new ListPollsForShowToAdmin()
            {
                VoteId = v.VoteId
            }).Count() / take;

            var query = await result.Select(v => new ListPollsForShowToAdmin()
            {
                CreateDate = v.CreateDate,
                EndDate = v.EndDate,
                IsActive = v.IsActive,
                Text = v.Text,
                Title = v.Title,
                VoteId = v.VoteId
            }).Skip(skip).Take(take).ToListAsync();

            return Tuple.Create(query, pageCount);
        }

        public async Task<Vote> GetVoteById(int voteId)
        {
            return await _db.Votes.Include(v => v.Groups).Include(v => v.Options).FirstOrDefaultAsync(g => g.VoteId == voteId);
        }

        public async Task<VoteDetailsViewModel> GetVoteDetailsForAdmin(int voteId)
        {
            var result = await _db.Votes.Include(v => v.UserVotes).ThenInclude(u => u.User)
                .Where(v => v.VoteId == voteId).Select(v => new VoteDetailsViewModel()
            {
                VoteId = voteId,
                CreateDate = v.CreateDate,
                EndDate = v.EndDate,
                IsActive = v.IsActive,
                Text = v.Text,
                Title = v.Title,
                OptionInfos = v.Options.Select(o => new OptionInfo()
                {
                    Text = o.Text,
                    Count = o.Count,
                }).ToList(),
                Groups = v.Groups.Select(g => new Group()
                {
                    Name = g.Name,
                    GroupId = g.GroupId
                }).ToList(),
                UserVotes = v.UserVotes.Select(u => new UserVoteInfo()
                {
                    UserFullName = u.User.FullName
                }).ToList()
            }).FirstOrDefaultAsync();

            return result;
        }

    }
}
