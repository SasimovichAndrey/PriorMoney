using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace PriorMoney.DesktopApp.ViewModel.ValidationRules
{
    public class UserDefinedNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value is BindingExpression)
            {
                BindingExpression binding = (BindingExpression)value;
                object dataItem = binding.DataItem;
                string propertyName = binding.ParentBinding.Path.Path;

                var propertyPath = propertyName.Split('.');

                for(int i = 0; i < propertyPath.Length; i++)
                {
                    value = dataItem.GetType().GetProperty(propertyPath[i]).GetValue(dataItem, null);
                    dataItem = value;
                }
            }
            

            var usrDefName = value as string;
            if (string.IsNullOrWhiteSpace(usrDefName))
            {
                return new ValidationResult(false, null);
            }

            return ValidationResult.ValidResult;
        }
    }
}
