using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.DTOs.Admin
{
    public class ListUsersForAdminViewModel
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string StudentCode { get; set; }

        public string Group { get; set; }
    }

    public class EditUserViewModel
    {
        public int GroupId { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string FullName { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [Phone(ErrorMessage = "شماره موبایل نامعتبر")]
        public string Phone { get; set; }

        [Display(Name = "شماره دانشجویی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(8, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(7, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "شماره دانشجویی باید فقط شامل عدد باشد.")]
        public string StudentCode { get; set; }

        [Display(Name ="ادمین")]
        public bool IsAdmin { get; set; }
    }
}
