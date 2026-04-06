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
        private readonly IRepository<Owner> _ownerRepository;

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

        public DogListViewModel(IRepository<Dog> repository, IRepository<Owner> ownerRepository)
        {
            _repository = repository;
            _ownerRepository = ownerRepository;
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
            ClearError();
            if (!ValidateDogInput()) return;

            var dog = new Dog
            {
                Name = Name,
                Breed = Breed,
                DateOfBirth = DateOfBirth,
                WeightKg = WeightKg,
                ChipNumber = ChipNumber,
                OwnerId = OwnerId
            };

            try
            {
                _repository.Add(dog);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                SetError(ToUserMessage(ex, "Could not add dog"));
            }
        }

        private void UpdateDog()
        {
            ClearError();
            if (SelectedDog == null) return;
            if (!ValidateDogInput()) return;

            SelectedDog.Name = Name;
            SelectedDog.Breed = Breed;
            SelectedDog.DateOfBirth = DateOfBirth;
            SelectedDog.WeightKg = WeightKg;
            SelectedDog.ChipNumber = ChipNumber;
            SelectedDog.OwnerId = OwnerId;

            try
            {
                _repository.Update(SelectedDog);
                LoadData();
            }
            catch (Exception ex)
            {
                SetError(ToUserMessage(ex, "Could not update dog"));
            }
        }

        private void DeleteDog()
        {
            ClearError();
            if (SelectedDog == null) return;

            try
            {
                _repository.Delete(SelectedDog.Id);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                SetError(ToUserMessage(ex, "Could not delete dog"));
            }
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

        private bool ValidateDogInput()
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Trim().Length <= 2)
            {
                SetError("Name is required and must be longer than 2 characters.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Breed) || Breed.Trim().Length <= 2)
            {
                SetError("Breed is required and must be longer than 2 characters.");
                return false;
            }

            if (DateOfBirth.Date > DateTime.Today)
            {
                SetError("Date of birth cannot be in the future.");
                return false;
            }

            if (WeightKg <= 0)
            {
                SetError("Weight must be greater than 0.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(ChipNumber))
            {
                SetError("Chip Number is required.");
                return false;
            }

            if (OwnerId <= 0)
            {
                SetError("Owner ID must be a positive number.");
                return false;
            }

            var ownerIds = _ownerRepository.GetAll().Select(o => o.Id).OrderBy(id => id).ToList();
            if (!ownerIds.Contains(OwnerId))
            {
                var idsList = ownerIds.Count == 0 ? "none" : string.Join(", ", ownerIds);
                SetError($"Invalid Owner ID. Existing Owner IDs: {idsList}.");
                return false;
            }

            return true;
        }

        private static string ToUserMessage(Exception ex, string fallback)
        {
            var message = ex.InnerException?.Message ?? ex.Message;
            if (message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase))
            {
                return "Invalid Owner ID. Use an existing owner (for seed data: 1, 2, or 3).";
            }

            return $"{fallback}: {message}";
        }
    }
}
