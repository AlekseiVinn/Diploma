using ScopeLap.DataBaseEngine;
using System.ComponentModel.DataAnnotations;

namespace ScopeLap.DbEngine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueMailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the DbContext from the validation context
            var dbContext = validationContext.GetService<ScopeLapDbContext>();

            // Get the current entity instance
            var currentEntity = validationContext.ObjectInstance as Account;

            // Check if the name already exists in the database
            var existingEntity = dbContext.Accounts.FirstOrDefault(b => b.Email == (string)value);

            if (existingEntity != null && existingEntity.Id != currentEntity.Id)
            {
                // Return a validation failure with the specified error message
                return new ValidationResult(ErrorMessage);
            }

            // Validation passed
            return ValidationResult.Success;
        }
    }

    //public class UniqueAccountAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        // Get the DbContext from the validation context
    //        var dbContext = validationContext.GetService<ScopeLapDbContext>();

    //        // Get the current entity instance
    //        var currentEntity = validationContext.ObjectInstance as Account;

    //        // Check if the name already exists in the database
    //        var existingEntity = dbContext.Accounts.FirstOrDefault(b => b.Login == (string)value);

    //        if (existingEntity != null && existingEntity.Id != currentEntity.Id)
    //        {
    //            // Return a validation failure with the specified error message
    //            return new ValidationResult(ErrorMessage);
    //        }

    //        // Validation passed
    //        return ValidationResult.Success;
    //    }
    //}
}