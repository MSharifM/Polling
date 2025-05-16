using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
                Groups = _db.Groups.Where(g => groupsId.Contains(g.GroupId)).ToList()
            };

            await _db.Votes.AddAsync(vote);
            await _db.SaveChangesAsync();
            return vote.VoteId;
        }

        public async Task<List<SelectListItem>> GetAllGroups()
        {
            return await _db.Groups.Select(g => new SelectListItem()
            {
                Text = g.Name,
                Value = g.GroupId.ToString()
            }).ToListAsync();
        }

        public async Task<Vote> GetVoteById(int voteId)
        {
            return await _db.Votes.Include(v => v.Groups).Include(v => v.Options).FirstOrDefaultAsync(g => g.VoteId == voteId);
        }
    }
}
