using FluentValidation.Results;
using MediumClone.Common.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Extensions
{
    public static class ValidationResultExtension
    {
        public static List<CustomValidationError> ConvertToCustomValidationError(this ValidationResult validationResult)
        {
            List<CustomValidationError> errors = new();

            foreach (var error in validationResult.Errors)
            {
                errors.Add(new()
                {
                    ErrorMessage = error.ErrorMessage,
                    PropertyName = error.PropertyName,
                });
            }

            return errors;
        }

    }
}
