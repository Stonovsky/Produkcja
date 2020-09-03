using GalaSoft.MvvmLight;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntityValidation
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]

    // Jest to klasa, z której dziedziczą wszystkie klasy MetaData!!!
    // Zawiera w sobie całą - prostą - logikę dot. IDataErrorInfo
    public class ValidationBase : IDataErrorInfo
    {
        private bool isValid;

        #region Walidacja
        [NotMapped]
        public bool IsValid
        {
            get
            {
                IsValidated();
                return isValid;
            } 
            set => isValid = value;
        }
        //public bool IsValid { get => isValid = IsValidated(); set => isValid = value; }

        public string Error { get { return null; } } //Ta właściwość jest pomijana w WPF
        public Dictionary<string, string> Errors { get; set; }


        // Tutaj następuje przyporządkowanie błędów do Propertisów
        // Poniżej tworzymy Dictionary z błędami i przyporządkowanymi do nich Propertisami, a poniżej do każedego z Propertisów przypisujemy błąd, gdy występuje!
        public string this[string propertyName]
        {
            get
            {
                IsValidated();
                return Errors.ContainsKey(propertyName) ? Errors[propertyName] : string.Empty;
            }
        }

        private void IsValidated()
        {
            Validate();
            if (Errors.Count == 0)
                IsValid = true;
            else
                IsValid = false;
        }


        private void Validate()
        {
            this.Errors = new Dictionary<string, string>();

            // Poniżej pobieramy DataAnnotations oraz walidujemy
            //dla poszczególnych Propeties, które nie są zwalidowane pozytywnie 
            //-> ForEachujemy przez nie dołączająć błędy do Dictionary (propetiesName, ErrorMessage)

            var validationContext = new ValidationContext(this, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                this.Errors.Add(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        //public void Validate2()


        //{

        //    Errors = new Dictionary<string, string>();

        //    var types = this.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), false)
        //                  .OfType<MetadataTypeAttribute>()
        //                  .Select(mta => mta.MetadataClassType);


        //    foreach (Type t in types.Union(new Type[] { this.GetType() }))
        //    {
        //        foreach (var prop in t.GetProperties())
        //        {
        //            var validations = prop.GetCustomAttributes(true).OfType<ValidationAttribute>().ToArray();
        //            foreach (var val in validations)
        //            {
        //                var propVal = this[prop.Name];
        //                if (!val.IsValid(propVal))
        //                {
        //                    var errMsg = val.FormatErrorMessage(prop.Name);
        //                    Errors.Add(prop.Name, errMsg);
        //                    //if (ErrorsChanged != null)
        //                    //    ErrorsChanged(this, new DataErrorsChangedEventArgs(prop.Name));
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}
