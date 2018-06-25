using Common;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayHistory.Client.Publisher
{
    public class PartitionOrRowKeyAttribute : ValidationAttribute
    {
        public PartitionOrRowKeyAttribute() : base("The field {0} must be a valid azure key.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var key = value as string;
            if (key == null)
            {
                return base.IsValid(value, validationContext);
            }

            if (StringUtils.IsValidPartitionOrRowKey(key))
            {
                return ValidationResult.Success;
            }

            if (validationContext == null)
            {
                return new ValidationResult($"\"{value}\" is invalid azure key.");
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                    new[] {validationContext.MemberName});
            }
        }
    }
}
