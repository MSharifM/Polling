using Polling.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.DTOs.Admin
{
    public class VoteDetailsViewModel
    {
        public int VoteId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public List<Group> Groups { get; set; }

        public List<OptionInfo> OptionInfos { get; set; }

        public List<UserVoteInfo> UserVotes { get; set; }
    }

    public class UserVoteInfo
    {
        public string UserFullName { get; set; }
    }

    public class OptionInfo
    {
        public string Text { get; set; }

        public int Count { get; set; }
    }

}
