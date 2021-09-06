using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeatherApp.ViewModels
{
    public class UserDetails
    {
        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email id is required")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                            ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}