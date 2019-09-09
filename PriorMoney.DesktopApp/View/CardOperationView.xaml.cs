using PriorMoney.DesktopApp.Model;
using PriorMoney.DesktopApp.ViewModel.Commands;
using PriorMoney.DesktopApp.ViewModel.ValidationRules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PriorMoney.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for CardOperationView.xaml
    /// </summary>
    public partial class CardOperationView : UserControl, INotifyPropertyChanged
    {
        // debug handler
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        #region Dependency prop registration
        public static readonly DependencyProperty CardOperationProperty =
            DependencyProperty.Register("CardOperation", typeof(CardOperationModel), typeof(CardOperationView), new PropertyMetadata(SetCardOperationDep));

        private static void SetCardOperationDep(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (e.NewValue as CardOperationModel).PropertyChanged += (s, eargs) => 
            {
                (d as CardOperationView).SaveOperationCommand.FireCanExecuteChanged();
            } ;
        }

        public static readonly DependencyProperty AvailableCategoriesProperty =
            DependencyProperty.Register("AvailableCategories", typeof(List<string>), typeof(CardOperationView));
        #endregion // Dependency prop registration

        #region Private fields
        private bool _isBeingEdited;
        #endregion // Private fields

        #region Events
        public event OperationRemovedEventHandler OperationRemoved;
        public event OperationEditedEventHandler OperationSaved;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Command handlers
        public CommandHandler RemoveOperationCommand { get; set; }
        public CommandHandler EditOperationCommand { get; set; }
        public CommandHandler SaveOperationCommand { get; set; } 
        #endregion

        #region Model properties
        public bool IsAddCategoryButtonEnabled
        {
            get
            {
                return this.CategoriesComboBox.SelectedItem != null;
            }
        }

        public CardOperationModel CardOperation
        {
            get
            {
                return GetValue(CardOperationProperty) as CardOperationModel;
            }
            set
            {
                SetValue(CardOperationProperty, value);
            }
        }


        public List<string> AvailableCategories
        {
            get
            {
                return GetValue(AvailableCategoriesProperty) as List<string>;
            }
            set
            {
                SetValue(AvailableCategoriesProperty, value);
                OnPropertyChanged(nameof(AvailableCategories));
            }
        }
        public string SelectedCategoryToAdd { get; set; }

        public bool IsBeingEdited
        {
            get
            {
                return _isBeingEdited;
            }
            set
            {
                _isBeingEdited = value;
                OnPropertyChanged(nameof(IsBeingEdited));
            }
        }
        #endregion

        public CardOperationView()
        {
            InitializeComponent();

            this.RootLayout.DataContext = this;

            RemoveOperationCommand = new CommandHandler(
                o => true, 
                async o => 
                {
                    OperationRemoved(this, this.CardOperation);
                });

            EditOperationCommand = new CommandHandler(
                o => true,
                async o =>
                {
                    this.IsBeingEdited = true;
                });

            SaveOperationCommand = new CommandHandler(
                o =>
                {
                    return !this.userDefinedNameTextbox.GetBindingExpression(TextBox.TextProperty).HasValidationError;
                },
                async o =>
                {
                    this.IsBeingEdited = false;
                    if(OperationSaved != null)
                    {
                        OperationSaved(this, this.CardOperation);
                    }
                });

            this.CategoriesComboBox.SelectionChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(IsAddCategoryButtonEnabled));
            };
        }

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void AddSelectedCategory(object sender, RoutedEventArgs e)
        {
            var newCategoriesHashSet = new HashSet<string>();
            foreach(var str in this.CardOperation.Categories)
            {
                newCategoriesHashSet.Add(str);
            }
            newCategoriesHashSet.Add(this.SelectedCategoryToAdd);

            this.CardOperation.Categories = newCategoriesHashSet;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

    public delegate void OperationRemovedEventHandler(object sender, CardOperationModel cardOperation);
    public delegate void OperationEditedEventHandler(object sender, CardOperationModel cardOperation);
}
