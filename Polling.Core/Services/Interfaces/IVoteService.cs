using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Polling.Core.DTOs.Admin;
using Polling.Core.DTOs.Vote;
using Polling.Datalayer.Entities;

namespace Polling.Core.Services.Interfaces
{
    public interface IVoteService
    {
        Task<List<SelectListItem>> GetAllGroups();
        Task<int> AddVote(CreateVoteViewModel model , List<int> groups);
        Task AddOptions(int voteId, List<CreateOptionViewModel> models);
        Task<Vote> GetVoteById(int voteId);
        Task<Tuple<List<ListPollsForShowToAdmin>, int>> GetPollsToShowForUser(int pageId = 1, string? filter = null
            , string getType = "all", string orderByType = "date", int take = 0);
        Task DeleteVote(int voteId);
        Task<VoteDetailsViewModel> GetVoteDetailsForAdmin(int voteId);
    }
}
