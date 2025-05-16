using System.ComponentModel.DataAnnotations;

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

        public int Count { get; set; }

        [Required]
        [MaxLength(300)]
        public string Text { get; set; }

        public bool IsDelete { get; set; } = false;

        #region Relations

        public virtual Vote Vote { get; set; }
        public virtual List<UserVote> UserVotes { get; set; }

        #endregion
    }
}
