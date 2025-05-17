using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.DTOs.Admin
{
    public class ListPollsForShowToAdmin
    {
        public int VoteId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public int CountParticipated { get; set; }
    }
}
