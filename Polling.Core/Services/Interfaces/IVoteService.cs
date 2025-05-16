using Microsoft.AspNetCore.Mvc.Rendering;
using Polling.Core.DTOs.Vote;
using Polling.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.Services.Interfaces
{
    public interface IVoteService
    {
        Task<List<SelectListItem>> GetAllGroups();
        Task<int> AddVote(CreateVoteViewModel model , List<int> groups);
        Task AddOptions(int voteId, List<CreateOptionViewModel> models);
        Task<Vote> GetVoteById(int voteId);
    }
}
