using GAT_Produkcja.db.Helpers.Kontrahent;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.CustomValidationAttributes
{
    public class NipValidator : ValidationAttribute
    {
        private KontrahentNipValidationHelper nipValidationHelper;

        public NipValidator()
        {
            nipValidationHelper = new KontrahentNipValidationHelper();
        }
        protected override ValidationResult IsValid(object nipPrzedWalidacja, ValidationContext validationContext)
        {
            string property = validationContext.DisplayName;
            string nip = nipPrzedWalidacja.ToString();
            string prefiksKraju = nipValidationHelper.WyseparujPrefiksKraju(nip);
            string nipCyfry = nipValidationHelper.WyseparujCyfry(nip);

            if (prefiksKraju != string.Empty && prefiksKraju.Length != 2)
            {
                return new ValidationResult("Prefiks kraju błędny", new List<string> { property });
            }

            if (nipCyfry?.Length != 10)
            {
                return new ValidationResult("Błędny numer NIP. NIP zawiera 10 cyfr", new List<string> { property });
            }

            return null;
        }
    }
}
