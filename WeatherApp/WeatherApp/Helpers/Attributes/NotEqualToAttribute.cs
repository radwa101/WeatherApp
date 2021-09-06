using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeatherApp.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotEqualToAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} cannot be the same as {1}.";

        public string OtherProperty { get; private set; }

        public NotEqualToAttribute(string otherProperty) : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty);
                var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

                if (value.Equals(otherPropertyValue))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    //return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                            //new[] { validationContext.MemberName, OtherProperty });
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "notequalto"
            };

            clientValidationRule.ValidationParameters.Add("otherproperty", OtherProperty);

            return new[] { clientValidationRule };
        }
    }
}