using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntitiesValidation
{
    public class ValidationBaseINotifyDataErrorInfo : INotifyDataErrorInfo
    {
        public bool HasErrors { get {
                Validate();
                return errorsForName.Count > 0; } }

        public bool IsValid { get; private set; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private Dictionary<string, string> errorsForName = new Dictionary<string, string>();

        public IEnumerable GetErrors(string propertyName)
        {
            Validate();
            IsValidated();
            if (errorsForName.ContainsKey(propertyName))
                return new string[] { errorsForName[propertyName] };
            else
                return new string[0];
        }

        private bool IsValidated()
        {
            if (errorsForName.Count == 0)
            {
                IsValid = true;
                return true;
            }
            else
                IsValid = false;
                return false;
        }
        public void Validate()
        {
            errorsForName.Clear();
            var type = this.GetType();
            var types = this.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), false)
                                      .OfType<MetadataTypeAttribute>()
                                      .Select(mta => mta.MetadataClassType);

            foreach (Type t in types.Union(new Type[] { this.GetType() }))
            {
                foreach (var prop in t.GetProperties())
                {
                    var validationRules = prop.GetCustomAttributes(true).OfType<ValidationAttribute>().ToArray();
                    foreach (var rule in validationRules)
                    {
                        //var propVal = this[prop.Name];
                        if (rule.IsValid(prop.Name))
                        {
                            var errMsg = rule.FormatErrorMessage(prop.Name);
                            errorsForName.Add(prop.Name, errMsg);
                            if (ErrorsChanged != null)
                                ErrorsChanged(this, new DataErrorsChangedEventArgs(prop.Name));
                        }
                    }
                }
            }

        }
    }
}
