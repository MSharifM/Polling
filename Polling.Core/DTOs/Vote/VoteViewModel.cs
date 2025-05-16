using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Polling.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.DTOs.Vote
{
    public class CreateVoteViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public bool IsActive { get; set; }

        public bool AllowMultipleSelection { get; set; }
    }

    public class CreateOptionViewModel
    {
        [MaxLength(300, ErrorMessage = "طول هر گزینه نمی تواند بیشتر از 300 کاراکتر باشد")]
        [Required]
        [Display(Name = "متن گزینه")]
        public string Text { get; set; }
    }
}
