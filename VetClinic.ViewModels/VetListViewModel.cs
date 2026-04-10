using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class VetListViewModel : ViewModelBase
    {
        private readonly IRepository<Veterinarian> _repository;
        private const string PhonePattern = @"^\+?[0-9]{7,15}$";

        private ObservableCollection<Veterinarian> _veterinarians = new();
        public ObservableCollection<Veterinarian> Veterinarians
        {
            get => _veterinarians;
            set => SetProperty(ref _veterinarians, value);
        }

        private Veterinarian? _selectedVet;
        public Veterinarian? SelectedVet
        {
            get => _selectedVet;
            set
            {
                if (SetProperty(ref _selectedVet, value))
                {
                    (UpdateCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _firstName = string.Empty;
        public string FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }

        private string _lastName = string.Empty;
        public string LastName { get => _lastName; set => SetProperty(ref _lastName, value); }

        private string _specialization = string.Empty;
        public string Specialization { get => _specialization; set => SetProperty(ref _specialization, value); }

        private string _licenseNumber = string.Empty;
        public string LicenseNumber { get => _licenseNumber; set => SetProperty(ref _licenseNumber, value); }

        private string _phone = string.Empty;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

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

        public VetListViewModel(IRepository<Veterinarian> repository)
        {
            _repository = repository;
            LoadCommand = new RelayCommand(_ => LoadData());
            AddCommand = new RelayCommand(_ => AddVet());
            UpdateCommand = new RelayCommand(_ => UpdateVet(), _ => SelectedVet != null);
            DeleteCommand = new RelayCommand(_ => DeleteVet(), _ => SelectedVet != null);
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                Veterinarians = new ObservableCollection<Veterinarian>(_repository.GetAll());
            }
            catch (Exception ex)
            {
                SetError($"Could not load veterinarians: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void AddVet()
        {
            ClearMessages();
            if (!ValidateVetInput(isUpdate: false)) return;
            var normalizedPhoneForStorage = NormalizePhoneForStorage(Phone);

            var vet = new Veterinarian
            {
                FirstName = FirstName,
                LastName = LastName,
                Specialization = Specialization,
                LicenseNumber = LicenseNumber,
                Phone = normalizedPhoneForStorage
            };

            try
            {
                _repository.Add(vet);
                LoadData();
                ClearForm();
                SuccessMessage = "Veterinarian added successfully.";
            }
            catch (Exception ex)
            {
                SetError($"Could not add veterinarian: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void UpdateVet()
        {
            ClearMessages();
            if (SelectedVet == null) return;
            if (!ValidateVetInput(isUpdate: true)) return;
            var normalizedPhoneForStorage = NormalizePhoneForStorage(Phone);

            SelectedVet.FirstName = FirstName;
            SelectedVet.LastName = LastName;
            SelectedVet.Specialization = Specialization;
            SelectedVet.LicenseNumber = LicenseNumber;
            SelectedVet.Phone = normalizedPhoneForStorage;

            try
            {
                _repository.Update(SelectedVet);
                LoadData();
                SuccessMessage = "Veterinarian updated successfully.";
            }
            catch (Exception ex)
            {
                SetError($"Could not update veterinarian: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void DeleteVet()
        {
            ClearMessages();
            if (SelectedVet == null) return;

            try
            {
                _repository.Delete(SelectedVet.Id);
                LoadData();
                ClearForm();
                SuccessMessage = "Veterinarian deleted successfully.";
            }
            catch (Exception ex)
            {
                SetError($"Could not delete veterinarian: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Specialization = string.Empty;
            LicenseNumber = string.Empty;
            Phone = string.Empty;
            SelectedVet = null;
        }

        private void ClearMessages()
        {
            ClearError();
            SuccessMessage = string.Empty;
        }

        private bool ValidateVetInput(bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(FirstName) || FirstName.Trim().Length < 2)
            {
                SetError("First name is required and must be at least 2 characters.");
                return false;
            }

            if (!IsLettersOnly(FirstName))
            {
                SetError("First name must contain letters only.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName) || LastName.Trim().Length < 2)
            {
                SetError("Last name is required and must be at least 2 characters.");
                return false;
            }

            if (!IsLettersOnly(LastName))
            {
                SetError("Last name must contain letters only.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Specialization))
            {
                SetError("Specialization is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(LicenseNumber))
            {
                SetError("License number is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                SetError("Phone is required.");
                return false;
            }

            var normalizedPhone = Phone.Trim();
            var canonicalPhone = NormalizePhoneForComparison(normalizedPhone);
            var normalizedLicense = LicenseNumber.Trim();

            if (!Regex.IsMatch(normalizedPhone, PhonePattern))
            {
                SetError("Phone format is invalid. Use digits with optional leading + (7 to 15 digits).");
                return false;
            }

            try
            {
                var vets = _repository.GetAll();
                var excludeId = isUpdate ? SelectedVet?.Id : null;

                var phoneExists = vets.Any(v =>
                    v.Id != excludeId &&
                    string.Equals(NormalizePhoneForComparison(v.Phone), canonicalPhone, StringComparison.OrdinalIgnoreCase));

                if (phoneExists)
                {
                    SetError("Phone number is already used by another veterinarian.");
                    return false;
                }

                var licenseExists = vets.Any(v =>
                    v.Id != excludeId &&
                    string.Equals(v.LicenseNumber?.Trim(), normalizedLicense, StringComparison.OrdinalIgnoreCase));

                if (licenseExists)
                {
                    SetError("License number is already used by another veterinarian.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetError($"Could not validate veterinarian data: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }

            return true;
        }

        private static bool IsLettersOnly(string value)
        {
            var trimmed = value.Trim();
            return trimmed.All(char.IsLetter);
        }

        private static string NormalizePhoneForComparison(string? rawPhone)
        {
            if (string.IsNullOrWhiteSpace(rawPhone))
            {
                return string.Empty;
            }

            if (rawPhone.Length == 10 && rawPhone.StartsWith("0", StringComparison.Ordinal))
            {
                return "359" + rawPhone[1..];
            }

            return rawPhone;
        }

        private static string NormalizePhoneForStorage(string? rawPhone)
        {
            var canonicalDigits = NormalizePhoneForComparison(rawPhone);
            return string.IsNullOrEmpty(canonicalDigits) ? string.Empty : "+" + canonicalDigits;
        }
    }
}
