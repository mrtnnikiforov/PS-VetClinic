using System.Collections.ObjectModel;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class OwnerListViewModel : ViewModelBase
    {
        private readonly IRepository<Owner> _repository;

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
            set => SetProperty(ref _selectedOwner, value);
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
            Owners = new ObservableCollection<Owner>(_repository.GetAll());
        }

        private void AddOwner()
        {
            ClearError();
            if (!ValidateOwnerInput()) return;

            var owner = new Owner
            {
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                Email = Email,
                Address = Address
            };

            try
            {
                _repository.Add(owner);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                SetError($"Could not add owner: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void UpdateOwner()
        {
            ClearError();
            if (SelectedOwner == null) return;
            if (!ValidateOwnerInput()) return;

            SelectedOwner.FirstName = FirstName;
            SelectedOwner.LastName = LastName;
            SelectedOwner.Phone = Phone;
            SelectedOwner.Email = Email;
            SelectedOwner.Address = Address;

            try
            {
                _repository.Update(SelectedOwner);
                LoadData();
            }
            catch (Exception ex)
            {
                SetError($"Could not update owner: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void DeleteOwner()
        {
            ClearError();
            if (SelectedOwner == null) return;

            try
            {
                _repository.Delete(SelectedOwner.Id);
                LoadData();
                ClearForm();
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
        }

        private bool ValidateOwnerInput()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                SetError("First name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                SetError("Last name is required.");
                return false;
            }

            return true;
        }
    }
}
