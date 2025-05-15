using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Datalayer.Entities
{
    public class UserVote
    {
        public UserVote()
        {

        }


        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int VoteId { get; set; }

        public int OptionId { get; set; }

        public DateTime VoteDate { get; set; } = DateTime.Now;

        #region Relations

        public virtual User User { get; set; }
        public virtual Vote Vote { get; set; }
        public virtual Option Option { get; set; }

        #endregion
    }

}
