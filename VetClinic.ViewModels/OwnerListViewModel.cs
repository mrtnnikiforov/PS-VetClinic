using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class OwnerListViewModel : ViewModelBase
    {
        private readonly IRepository<Owner> _repository;
        private const string PhonePattern = @"^\+?[0-9]{7,15}$";
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        private ObservableCollection<Owner> _owners = new();
        public ObservableCollection<Owner> Owners
        {
            get => _owners;
            set => SetProperty(ref _owners, value);
        }

        private Owner? _selectedOwner;
        public Owner? SelectedOwner
        {
            get => _selectedOwner;
            set
            {
                if (SetProperty(ref _selectedOwner, value))
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

        private string _phone = string.Empty;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

        private string _email = string.Empty;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _address = string.Empty;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

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

        public OwnerListViewModel(IRepository<Owner> repository)
        {
            _repository = repository;
            LoadCommand = new RelayCommand(_ => LoadData());
            AddCommand = new RelayCommand(_ => AddOwner());
            UpdateCommand = new RelayCommand(_ => UpdateOwner(), _ => SelectedOwner != null);
            DeleteCommand = new RelayCommand(_ => DeleteOwner(), _ => SelectedOwner != null);
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                Owners = new ObservableCollection<Owner>(_repository.GetAll());
            }
            catch (Exception ex)
            {
                SetError($"Could not load owners: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void AddOwner()
        {
            ClearMessages();
            if (!ValidateOwnerInput(isUpdate: false)) return;
            var normalizedPhoneForStorage = NormalizePhoneForStorage(Phone);

            var owner = new Owner
            {
                FirstName = FirstName,
                LastName = LastName,
                Phone = normalizedPhoneForStorage,
                Email = Email,
                Address = Address
            };

            try
            {
                _repository.Add(owner);
                LoadData();
                ClearForm();
                SuccessMessage = "Owner added successfully.";
            }
            catch (Exception ex)
            {
                SetError($"Could not add owner: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void UpdateOwner()
        {
            ClearMessages();
            if (SelectedOwner == null) return;
            if (!ValidateOwnerInput(isUpdate: true)) return;
            var normalizedPhoneForStorage = NormalizePhoneForStorage(Phone);

            SelectedOwner.FirstName = FirstName;
            SelectedOwner.LastName = LastName;
            SelectedOwner.Phone = normalizedPhoneForStorage;
            SelectedOwner.Email = Email;
            SelectedOwner.Address = Address;

            try
            {
                _repository.Update(SelectedOwner);
                LoadData();
                SuccessMessage = "Owner updated successfully.";
            }
            catch (Exception ex)
            {
                SetError($"Could not update owner: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void DeleteOwner()
        {
            ClearMessages();
            if (SelectedOwner == null) return;

            try
            {
                _repository.Delete(SelectedOwner.Id);
                LoadData();
                ClearForm();
                SuccessMessage = "Owner deleted successfully.";
            }
            catch (Exception ex)
            {
                SetError($"Could not delete owner: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            SelectedOwner = null;
        }

        private void ClearMessages()
        {
            ClearError();
            SuccessMessage = string.Empty;
        }

        private bool ValidateOwnerInput(bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(FirstName) || FirstName.Trim().Length < 2)
            {
                SetError("First name is required and must be at least 2 characters.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName) || LastName.Trim().Length < 2)
            {
                SetError("Last name is required and must be at least 2 characters.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                SetError("Phone is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                SetError("Email is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                SetError("Address is required.");
                return false;
            }

            var normalizedPhone = Phone.Trim();
            var normalizedEmail = Email.Trim();
            var canonicalPhone = NormalizePhoneForComparison(normalizedPhone);

            if (!Regex.IsMatch(normalizedPhone, PhonePattern))
            {
                SetError("Phone format is invalid. Use digits with optional leading + (7 to 15 digits).");
                return false;
            }

            if (!Regex.IsMatch(normalizedEmail, EmailPattern, RegexOptions.IgnoreCase))
            {
                SetError("Email format is invalid.");
                return false;
            }

            try
            {
                var owners = _repository.GetAll();
                var excludeId = isUpdate ? SelectedOwner?.Id : null;

                var phoneExists = owners.Any(o =>
                    o.Id != excludeId &&
                    string.Equals(NormalizePhoneForComparison(o.Phone), canonicalPhone, StringComparison.OrdinalIgnoreCase));

                if (phoneExists)
                {
                    SetError("Phone number is already used by another owner.");
                    return false;
                }

                var emailExists = owners.Any(o =>
                    o.Id != excludeId &&
                    string.Equals(o.Email?.Trim(), normalizedEmail, StringComparison.OrdinalIgnoreCase));

                if (emailExists)
                {
                    SetError("Email is already used by another owner.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetError($"Could not validate owner data: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }

            return true;
        }

        private static string NormalizePhoneForComparison(string? rawPhone)
        {
            if (string.IsNullOrWhiteSpace(rawPhone))
            {
                return string.Empty;
            }

            // Keep digits only so +359 888-111-222 and +359888111222 compare equally.
            var digitsOnly = new string(rawPhone.Where(char.IsDigit).ToArray());

            // Treat local Bulgarian 0XXXXXXXXX as international 359XXXXXXXXX.
            if (digitsOnly.Length == 10 && digitsOnly.StartsWith("0", StringComparison.Ordinal))
            {
                return "359" + digitsOnly[1..];
            }

            // Treat 00359XXXXXXXXX and 359XXXXXXXXX as the same canonical value.
            if (digitsOnly.StartsWith("00359", StringComparison.Ordinal))
            {
                return digitsOnly[2..];
            }

            return digitsOnly;
        }

        private static string NormalizePhoneForStorage(string? rawPhone)
        {
            var canonicalDigits = NormalizePhoneForComparison(rawPhone);
            return string.IsNullOrEmpty(canonicalDigits) ? string.Empty : "+" + canonicalDigits;
        }
    }
}
