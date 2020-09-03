using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.CustomValidationAttributes
{
    public class HudredPercentValidator : ValidationAttribute
    {
        public HudredPercentValidator()
        {
        }

        protected override ValidationResult IsValid(object przeslanaWartoscProcentowa, ValidationContext validationContext)
        {
            string nazwaProperty = validationContext.DisplayName;
            decimal procent = 0;

            var czyDecimal = Decimal.TryParse(przeslanaWartoscProcentowa.ToString(), out procent);

            if (!czyDecimal)
                return new ValidationResult("Błędny format", new [] { nazwaProperty });
                //return new ValidationResult("Błędny format", new List<string> { nazwaProperty });

            if (procent!=1)
                return new ValidationResult("Pole nie równe 100%", new[] { nazwaProperty });
                //return new ValidationResult("Pole nie równe 100%", new List<string> { nazwaProperty });

            return null;
        }
    }
}
