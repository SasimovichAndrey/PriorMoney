using AutoMapper;
using PriorMoney.DesktopApp.Model;
using PriorMoney.DesktopApp.View;
using PriorMoney.DesktopApp.ViewModel.Commands;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PriorMoney.DesktopApp.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Private fields
        private IDbLogicManager _dbLogicManager;
        private readonly IMapper _mapper;
        private bool _isNewCardOperationBeingAdded;
        private List<string> _availableCategories;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        public ObservableCollection<CardOperationModel> CardOperations { get; set; } = new ObservableCollection<CardOperationModel>();

        public bool IsNewCardOperationBeingAdded {
            get
            {
                return _isNewCardOperationBeingAdded;
            }
            set
            {
                _isNewCardOperationBeingAdded = value;
                OnPropertyChanged(nameof(IsNewCardOperationBeingAdded));
            }
        }

        public List<string> AvailableCategories
        {
            get
            {
                return _availableCategories;
            }
            set
            {
                _availableCategories = value;
                OnPropertyChanged(nameof(AvailableCategories));
            }
        }

        public CardOperationModel NewCardOperation { get; set; }

        public string SelectedCategoryToAdd { get; set; }
        #endregion

        #region Command handlers
        public AddNewCardOperationCommand AddNewCardOperationCommand { get; set; }

        public SaveNewOperationCommand SaveNewOperationCommand { get; set; }

        public RemoveOperationCommand RemoveOperationCommand { get; private set; }

        public CommandHandler OpenImportDialog { get; set; }

        public CommandHandler AddCategoryToOperationCategoriesCommand { get; set; }
        #endregion // Command handlers

        public MainWindowViewModel(IDbLogicManager dbLogicManager, IMapper mapper)
        {
            _dbLogicManager = dbLogicManager;
            _mapper = mapper;

            NewCardOperation = new CardOperationModel();
            AddNewCardOperationCommand = new AddNewCardOperationCommand(this);
            SaveNewOperationCommand = new SaveNewOperationCommand(this);
            RemoveOperationCommand = new RemoveOperationCommand(this);
            AddCategoryToOperationCategoriesCommand = new CommandHandler(o => true, AddAddCategoryToOperationCategories);
            IsNewCardOperationBeingAdded = false;

            NewCardOperation.PropertyChanged += this.SaveNewOperationCommand.RaiseOperationModelChanged;
        }

        private async Task AddAddCategoryToOperationCategories(object arg)
        {

            var newCategoriesHashSet = new HashSet<string>();
            foreach (var str in this.NewCardOperation.Categories)
            {
                newCategoriesHashSet.Add(str);
            }
            newCategoriesHashSet.Add(this.SelectedCategoryToAdd);

            this.NewCardOperation.Categories = newCategoriesHashSet;
        }

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public async Task LoadData()
        {
            var operations = await _dbLogicManager.GetLastOperations(30);
            var categories = await _dbLogicManager.GetAllCategories();

            CardOperations.Clear();

            AddCardOperationsToModelCollection(operations);
            AddCategoriesToModelCollection(categories);
        }

        public async Task LoadAdditionalData()
        {
            var skip = CardOperations.Count;
            var take = 10;
            var operations = await _dbLogicManager.GetLastOperations(take, skip);

            AddCardOperationsToModelCollection(operations);
        }

        public async Task SaveCardOperation(CardOperationModel cardOperationModel)
        {
            var domainModel = _mapper.Map<CardOperation>(cardOperationModel);
            await _dbLogicManager.CreateOrUpdateOperations(new List<CardOperation> { domainModel });
        }

        public async Task RemoveCardOperation(CardOperationModel cardOperation)
        {
            await _dbLogicManager.RemoveCardOperationById(cardOperation.Id);
            CardOperations.Remove(cardOperation);
        }

        public async Task InitializeNewCardOperationForAdding()
        {
            NewCardOperation.Clean();
            IsNewCardOperationBeingAdded = true;

            await Task.CompletedTask;
        }

        public async Task SaveNewOperation()
        {
            var cardOperation = _mapper.Map<CardOperation>(NewCardOperation);
            var savedOperation = await _dbLogicManager.AddNewOperation(cardOperation);
            var savedOperationModel = _mapper.Map<CardOperationModel>(savedOperation);

            CardOperations.Insert(0, savedOperationModel);

            NewCardOperation.Clean();
            IsNewCardOperationBeingAdded = false;
        }

        private void AddCategoriesToModelCollection(List<string> categories)
        {
            AvailableCategories = categories;
        }

        private void AddCardOperationsToModelCollection(List<CardOperation> operations)
        {
            foreach(var op in operations)
            {
                var newCardOperationModel = new CardOperationModel();
                _mapper.Map<CardOperation, CardOperationModel>(op, newCardOperationModel);
                CardOperations.Add(newCardOperationModel);
            }
        }
    }
}
