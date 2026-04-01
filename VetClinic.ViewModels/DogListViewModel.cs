using System.Collections.ObjectModel;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class DogListViewModel : ViewModelBase
    {
        private readonly IRepository<Dog> _repository;

        private ObservableCollection<Dog> _dogs = new();
        public ObservableCollection<Dog> Dogs
        {
            get => _dogs;
            set => SetProperty(ref _dogs, value);
        }

        private Dog? _selectedDog;
        public Dog? SelectedDog
        {
            get => _selectedDog;
            set => SetProperty(ref _selectedDog, value);
        }

        private string _name = string.Empty;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private string _breed = string.Empty;
        public string Breed { get => _breed; set => SetProperty(ref _breed, value); }

        private DateTime _dateOfBirth = DateTime.Today;
        public DateTime DateOfBirth { get => _dateOfBirth; set => SetProperty(ref _dateOfBirth, value); }

        private double _weightKg;
        public double WeightKg { get => _weightKg; set => SetProperty(ref _weightKg, value); }

        private string _chipNumber = string.Empty;
        public string ChipNumber { get => _chipNumber; set => SetProperty(ref _chipNumber, value); }

        private int _ownerId;
        public int OwnerId { get => _ownerId; set => SetProperty(ref _ownerId, value); }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public DogListViewModel(IRepository<Dog> repository)
        {
            _repository = repository;
            LoadCommand = new RelayCommand(_ => LoadData());
            AddCommand = new RelayCommand(_ => AddDog());
            UpdateCommand = new RelayCommand(_ => UpdateDog(), _ => SelectedDog != null);
            DeleteCommand = new RelayCommand(_ => DeleteDog(), _ => SelectedDog != null);
            LoadData();
        }

        public void LoadData()
        {
            Dogs = new ObservableCollection<Dog>(_repository.GetAll());
        }

        private void AddDog()
        {
            var dog = new Dog
            {
                Name = Name,
                Breed = Breed,
                DateOfBirth = DateOfBirth,
                WeightKg = WeightKg,
                ChipNumber = ChipNumber,
                OwnerId = OwnerId
            };
            _repository.Add(dog);
            LoadData();
            ClearForm();
        }

        private void UpdateDog()
        {
            if (SelectedDog == null) return;
            SelectedDog.Name = Name;
            SelectedDog.Breed = Breed;
            SelectedDog.DateOfBirth = DateOfBirth;
            SelectedDog.WeightKg = WeightKg;
            SelectedDog.ChipNumber = ChipNumber;
            SelectedDog.OwnerId = OwnerId;
            _repository.Update(SelectedDog);
            LoadData();
        }

        private void DeleteDog()
        {
            if (SelectedDog == null) return;
            _repository.Delete(SelectedDog.Id);
            LoadData();
            ClearForm();
        }

        private void ClearForm()
        {
            Name = string.Empty;
            Breed = string.Empty;
            DateOfBirth = DateTime.Today;
            WeightKg = 0;
            ChipNumber = string.Empty;
            OwnerId = 0;
        }
    }
}
