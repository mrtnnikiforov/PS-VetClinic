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
            set
            {
                if (SetProperty(ref _selectedDog, value))
                {
                    (UpdateCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
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

        private string _successMessage = string.Empty;
        public string SuccessMessage
        {
            get => _successMessage;
            set
            {
                if (SetProperty(ref _successMessage, value))
                {
                    OnPropertyChanged(nameof(HasSuccessMessage));
                }
            }
        }

        public bool HasSuccessMessage => !string.IsNullOrWhiteSpace(SuccessMessage);

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
            try
            {
                Dogs = new ObservableCollection<Dog>(_repository.GetAll("Owner"));
            }
            catch (Exception ex)
            {
                SetError($"Could not load dogs: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void AddDog()
        {
            ClearMessages();
            if (!ValidateDogInput(isUpdate: false)) return;

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
                SuccessMessage = "Dog added successfully.";
            }
            catch (Exception ex)
            {
                SetError(ToUserMessage(ex, "Could not add dog"));
            }
        }

        private void UpdateDog()
        {
            ClearMessages();
            if (SelectedDog == null) return;
            if (!ValidateDogInput(isUpdate: true)) return;

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
                SuccessMessage = "Dog updated successfully.";
            }
            catch (Exception ex)
            {
                SetError(ToUserMessage(ex, "Could not update dog"));
            }
        }

        private void DeleteDog()
        {
            ClearMessages();
            if (SelectedDog == null) return;

            try
            {
                _repository.Delete(SelectedDog.Id);
                LoadData();
                ClearForm();
                SuccessMessage = "Dog deleted successfully.";
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
            SelectedDog = null;
        }

        private void ClearMessages()
        {
            ClearError();
            SuccessMessage = string.Empty;
        }

        private bool ValidateDogInput(bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Trim().Length <= 2)
            {
                SetError("Name is required and must be longer than 2 characters.");
                return false;
            }

            if (!IsLettersOnly(Name))
            {
                SetError("Name must contain letters only.");
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

            var normalizedChip = ChipNumber.Trim();
            if (!TryValidateUniqueChipNumber(normalizedChip, isUpdate))
            {
                return false;
            }

            if (OwnerId <= 0)
            {
                SetError("Owner ID must be a positive number.");
                return false;
            }

            List<int> ownerIds;
            try
            {
                ownerIds = _ownerRepository.GetAll().Select(o => o.Id).OrderBy(id => id).ToList();
            }
            catch (Exception ex)
            {
                SetError($"Could not validate Owner ID: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }

            if (!ownerIds.Contains(OwnerId))
            {
                var idsList = ownerIds.Count == 0 ? "none" : string.Join(", ", ownerIds);
                SetError($"Invalid Owner ID. Existing Owner IDs: {idsList}.");
                return false;
            }

            return true;
        }

        private static bool IsLettersOnly(string value)
        {
            var trimmed = value.Trim();
            return trimmed.All(char.IsLetter);
        }

        private bool TryValidateUniqueChipNumber(string normalizedChip, bool isUpdate)
        {
            try
            {
                var dogs = _repository.GetAll();
                var excludeDogId = isUpdate ? SelectedDog?.Id : null;
                var hasDuplicate = dogs.Any(d =>
                    d.Id != excludeDogId &&
                    string.Equals(d.ChipNumber?.Trim(), normalizedChip, StringComparison.OrdinalIgnoreCase));

                if (hasDuplicate)
                {
                    SetError("Chip Number already exists. Each dog must have a unique chip number.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                SetError($"Could not validate Chip Number: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        private static string ToUserMessage(Exception ex, string fallback)
        {
            var message = ex.InnerException?.Message ?? ex.Message;
            if (message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase))
            {
                return "Invalid Owner ID. Use an existing owner (for seed data: 1, 2, or 3).";
            }

            if (message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return "Chip Number already exists. Each dog must have a unique chip number.";
            }

            return $"{fallback}: {message}";
        }
    }
}
