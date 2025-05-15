using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Datalayer.Entities
{
    public class Option
    {
        public Option()
        {
            UserVotes = new List<UserVote>();
        }

        [Key]
        public int OptionId { get; set; }

        public int VoteId { get; set; }

        [Required]
        public string Text { get; set; }

        #region Relations

        public virtual Vote Vote { get; set; }
        public virtual List<UserVote> UserVotes { get; set; }

        #endregion
    }
}
