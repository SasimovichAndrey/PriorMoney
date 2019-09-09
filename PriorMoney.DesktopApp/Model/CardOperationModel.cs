using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriorMoney.DesktopApp.ViewModel.Commands;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;

namespace PriorMoney.DesktopApp.Model
{
    public class CardOperationModel : INotifyPropertyChanged
    {

        #region Properties
        public Guid Id { get; set; }

        private string _userDefinedName;
        public string UserDefinedName
        {
            get { return _userDefinedName; }
            set
            {
                _userDefinedName = value;

                OnPropertyChanged(nameof(UserDefinedName));
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;

                OnPropertyChanged(nameof(Amount));
            }
        }

        private Currency _currency;
        public Currency Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;

                OnPropertyChanged(nameof(Currency));
            }
        }

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                _dateTime = value;

                OnPropertyChanged(nameof(DateTime));
            }
        }

        private HashSet<string> _categories;

        [HasDefaultValue]
        public HashSet<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;

                OnPropertyChanged(nameof(Categories));
            }
        }
        #endregion // Properties

        public event PropertyChangedEventHandler PropertyChanged;

        public CardOperationModel()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            DateTime = DateTime.Now;
            Categories = new HashSet<string>();
        }

        private void OnPropertyChanged(string propName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        internal void Clean()
        {
            var type = typeof(CardOperationModel);
            var props = type.GetProperties();

            foreach(var prop in props)
            {
                if(!prop.GetCustomAttributes(true).Any(a => a is HasDefaultValueAttribute))
                {
                    if (prop.GetType().IsValueType)
                        prop.SetValue(this, Activator.CreateInstance(prop.GetType()));
                    else
                        prop.SetValue(this, null);
                }
            }

            SetDefaultValues();
        }

        // TODO: move validation away from here
        public bool IsModelReadyForSaving()
        {
            if (string.IsNullOrWhiteSpace(UserDefinedName))
            {
                return false;
            }

            if(Amount == 0)
            {
                return false;
            }

            return true;
        }
    }

    internal class HasDefaultValueAttribute : Attribute
    {
    }
}
