using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Core.DTOs.User
{
    public class LoginViewModel
    {
        [Display(Name = "شماره دانشجویی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(8, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(7, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "شماره دانشجویی باید فقط شامل عدد باشد.")]
        public string StudentCode { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل نامعتبر")]
        public string Email { get; set; }

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

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        [Compare("Password", ErrorMessage = "کلمه عبور  با تکرار آن برابر نیست")]
        public string RePassword { get; set; }

        public int GroupId { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [Phone(ErrorMessage = "شماره موبایل نامعتبر")]
        public string Phone { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string ActiveCode { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        [Compare("Password", ErrorMessage = "کلمه عبور  با تکرار آن برابر نیست")]
        public string RePassword { get; set; }
    }
}
