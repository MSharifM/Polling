using System.ComponentModel.DataAnnotations;

namespace Polling.Datalayer.Entities
{
   public class Vote
    {
        public Vote()
        {
            Groups = new List<Group>();
            Options = new List<Option>();
            UserVotes = new List<UserVote>();
        }

        [Key]
        public int VoteId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }

        public bool IsDelete { get; set; } = false;

        public bool IsActive { get; set; }

        public bool AllowMultipleSelection { get; set; }

        #region Relations

        public virtual List<Group> Groups { get; set; }
        public virtual List<Option> Options { get; set; }
        public virtual List<UserVote> UserVotes { get; set; }

        #endregion
    }
}
