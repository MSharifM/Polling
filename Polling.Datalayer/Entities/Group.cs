using System.ComponentModel.DataAnnotations;

namespace Polling.Datalayer.Entities
{
    public class Group
    {
        public Group()
        {
            Users = new List<User>();
            Votes = new List<Vote>();
        }


        [Key]
        public int GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsDelete { get; set; } = false;

        #region Relations

        public virtual List<User> Users { get; set; }
        public virtual List<Vote> Votes { get; set; }

        #endregion
    }
}
