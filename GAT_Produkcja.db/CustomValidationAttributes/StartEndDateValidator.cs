using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.CustomValidationAttributes
{
    public class StartEndDateValidator : ValidationAttribute
    {
        private readonly string endDate;
        public StartEndDateValidator(string endDateToCompare)
        {
            this.endDate = endDateToCompare;
        }

        protected override ValidationResult IsValid(object date, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(endDate);
            if (property == null)
            {
                return new ValidationResult(
                    string.Format("Unknown property: {0}", endDate)
                );
            }

            var endDateValue = property.GetValue(validationContext.ObjectInstance, null);
            var endDateToCompare = Convert.ToDateTime(endDateValue);

            // at this stage you have "value" and "otherValue" pointing
            // to the value of the property on which this attribute
            // is applied and the value of the other property respectively
            // => you could do some checks

            var startDate = Convert.ToDateTime(date);
            if (endDateValue != null
                && startDate > endDateToCompare)
            {
                // here we are verifying whether the 2 values are equal
                // but you could do any custom validation you like
                return new ValidationResult("Data zakończenia przed datą rozpoczęcia!", new List<string> { endDate });
            }
            return null;
        }
    }
}
