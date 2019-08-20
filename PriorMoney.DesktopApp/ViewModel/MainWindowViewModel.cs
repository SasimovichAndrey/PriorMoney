using AutoMapper;
using PriorMoney.DesktopApp.Model;
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
        private IDbLogicManager _dbLogicManager;
        private readonly IMapper _mapper;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CardOperationModel> CardOperations { get; set; } = new ObservableCollection<CardOperationModel>();

        private bool _isNewCardOperationBeingAdded;
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

        private void OnPropertyChanged(string propName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public AddNewCardOperationCommand AddNewCardOperationCommand { get; set; }
        public SaveNewOperationCommand SaveNewOperationCommand{ get; set; }
        public RemoveOperationCommand RemoveOperationCommand { get; private set; }
        public ICommand TestCommand { get; set; }

        public CardOperationModel NewCardOperation { get; set; }

        public MainWindowViewModel(IDbLogicManager dbLogicManager, IMapper mapper)
        {
            _dbLogicManager = dbLogicManager;
            _mapper = mapper;

            NewCardOperation = new CardOperationModel();
            AddNewCardOperationCommand = new AddNewCardOperationCommand(this);
            SaveNewOperationCommand = new SaveNewOperationCommand(this);
            RemoveOperationCommand = new RemoveOperationCommand(this);

            IsNewCardOperationBeingAdded = false;
        }

        public async Task LoadData()
        {
            var operations = await _dbLogicManager.GetLastOperations(10);

            MapCardOperationsToModels(operations);
        }

        public async Task RemoveCardOperation(CardOperationModel cardOperation)
        {
            await _dbLogicManager.RemoveCardOperationById(cardOperation.Id);
            CardOperations.Remove(cardOperation);
        }

        public async Task AddNewCardOperation()
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

            CardOperations.Add(savedOperationModel);

            NewCardOperation.Clean();
            IsNewCardOperationBeingAdded = false;
        }

        private void MapCardOperationsToModels(List<CardOperation> operations)
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
