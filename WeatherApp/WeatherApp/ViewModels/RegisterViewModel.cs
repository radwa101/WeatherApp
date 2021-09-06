using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Helpers.Attributes;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace WeatherApp.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "User name")]
        [Required(ErrorMessage = "You forgot to enter a username.")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 12 characters.")]
        [Remote("ValidateUserName", "Home", ErrorMessage = "Username is not available.")]
        public string UserName { get; set; }

        [Display(Name = "Email address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required (we promise not to spam you!).")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [NotEqualTo("UserName")]
        //[ValidatePasswordLength]
        public string Password { get; set; }

        [Required]
        [Range(18, 65, ErrorMessage = "Sorry, you must be between 18 and 65 to register.")]
        [RegularExpression(@"\d{1,3}", ErrorMessage = "Please enter a valid age.")]
        public string Age { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Promotional code?")]
        public bool HasPromotionalCode { get; set; }

        [Display(Name = "Promotional code")]
        [RequiredIf("HasPromotionalCode", Comparison.IsEqualTo, true)]
        public string PromotionalCode { get; set; }

        [CheckCountry(AllowCountry = "India,Pakistan,Nepal", ErrorMessage = ("Please choose a valid country eg.(India,Pakistan,Nepal)"))]
        public string Country { get; set; }
    }
}