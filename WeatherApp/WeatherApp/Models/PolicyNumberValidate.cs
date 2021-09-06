using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WeatherApp.Models
{
    public class PolicyNumberValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            object timeTableId = type.GetProperty("Servicetax").GetValue(instance, null);

            string Message = string.Empty;
            if (value == null)
            {
                return new ValidationResult("Policy number is required");
            }

            string policynumber = value.ToString();

            if (new Regex(@"^1{1}").Match(policynumber).Success)
            {
                Message = "Policy number can't begin with 1";
                return new ValidationResult(Message);
            }
            if (new Regex(@"^2{1}").Match(policynumber).Success)
            {
                Message = "Policy number can't begin with 2";
                return new ValidationResult(Message);
            }
            if (new Regex(@"^3{1}").Match(policynumber).Success)
            {
                Message = "Policy number can't begin with 3";
                return new ValidationResult(Message);
            }
            return ValidationResult.Success;
        }
    }
}