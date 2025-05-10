using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.DTOs.User
{
    public class ShowInformationForUsrePanelViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string UserAvatar { get; set; }
    }

    public class EditAvatarViewModel
    {
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string StudentCode { get; set; }
        public string AvatarName { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Avatar { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "لطفاً رمز عبور فعلی را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور فعلی")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "لطفاً رمز عبور جدید را وارد کنید")]
        [StringLength(100, ErrorMessage = "رمز عبور باید حداقل {2} و حداکثر {1} کاراکتر باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "لطفاً تکرار رمز عبور جدید را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور جدید")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور جدید با تکرار آن مطابقت ندارد")]
        public string ConfirmPassword { get; set; }
    }
}
