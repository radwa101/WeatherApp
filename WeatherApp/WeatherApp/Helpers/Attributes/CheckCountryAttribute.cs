using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeatherApp.Helpers.Attributes
{
    public sealed class CheckCountryAttribute : ValidationAttribute, IClientValidatable
    {
        public String AllowCountry { get; set; }
        protected override ValidationResult IsValid(object country, ValidationContext validationContext)
        {
            string[] myarr = AllowCountry.ToString().Split(',');
            if (myarr.Contains(country))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Please choose a valid country eg.(India,Pakistan,Nepal)");
            }
        }

        IEnumerable<ModelClientValidationRule> IClientValidatable.GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "checkcountry"
            };

            clientValidationRule.ValidationParameters.Add("allowcountry", AllowCountry);

            return new[] { clientValidationRule };
        }
    }

}