using Microsoft.Win32;
using PriorMoney.DesktopApp.ViewModel.Commands;
using System;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PriorMoney.DesktopApp.Model;
using AutoMapper;
using PriorMoney.Storage.Interface;
using System.ComponentModel;

namespace PriorMoney.DesktopApp.ViewModel
{
    public class ImportWindowViewModel : INotifyPropertyChanged
    {
        private ICardOperationsLoader _cardOperationsLoader;
        private bool _dataImported;
        private List<string> _availableCategories;
        private readonly IMapper _mapper;
        private readonly IDbLogicManager _dbLogicManager;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CardOperationModel> CardOperations { get; set; } = new ObservableCollection<CardOperationModel>();
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
        public CommandHandler ImportHandler { get; private set; }

        public CommandHandler SaveImportedOperationsCommand { get; set; }
        public bool DataImported
        {
            get
            {
                return _dataImported;
            }
            set
            {
                _dataImported = value;
                OnPropertyChanged(nameof(DataImported));
            }
        }

        public ImportWindowViewModel(ICardOperationsLoader cardOperationsLoader, IMapper mapper, IDbLogicManager dbLogicManager)
        {
            ImportHandler = new CommandHandler((prm) => true, ImportOperationsHandler);
            SaveImportedOperationsCommand = new CommandHandler(
                o => true,
                this.SaveImportedOperationsHandler);
            _cardOperationsLoader = cardOperationsLoader;
            _mapper = mapper;
            _dbLogicManager = dbLogicManager;
            DataImported = false;
        }

        public async Task LoadData()
        {
            await LoadAvailableCategories();
        }

        private async Task LoadAvailableCategories()
        {
            var categories = await _dbLogicManager.GetAllCategories();
            AvailableCategories = categories;
        }

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private async Task SaveImportedOperationsHandler(object arg)
        {
            var operations = _mapper.Map<List<CardOperation>>(this.CardOperations);

            var importId = Guid.NewGuid();
            operations.ForEach(op => op.ImportId = importId);

            await _dbLogicManager.AddManyOperations(operations);

            this.CardOperations.Clear();
        }

        public async Task SaveCardOperation(CardOperationModel cardOperation)
        {
            //if (CardOperations.Contains(cardOperation))
            //{
            //    CardOperations.Remove(cardOperation);
            //}

            await Task.CompletedTask;
        }

        public Task RemoveCardOperation(CardOperationModel cardOperation)
        {
            if (CardOperations.Contains(cardOperation))
            {
                CardOperations.Remove(cardOperation);
            }

            return Task.CompletedTask;
        }

        private async Task ImportOperationsHandler(object obj)
        {
            var operations = await ImportCardOperations();
            UpdateViewModelAfterImport(operations);
        }

        private void UpdateViewModelAfterImport(List<CardOperation> operations)
        {
            CardOperations.Clear();

            foreach (var op in operations)
            {
                var newCardOperationModel = new CardOperationModel();
                _mapper.Map<CardOperation, CardOperationModel>(op, newCardOperationModel);
                newCardOperationModel.UserDefinedName = op.OriginalName;
                CardOperations.Add(newCardOperationModel);
            }

            DataImported = true;
        }

        private async Task<List<CardOperation>> ImportCardOperations()
        {
            return await _cardOperationsLoader.LoadAsync();
        }
    }
}
