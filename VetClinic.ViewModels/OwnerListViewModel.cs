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
            var owner = new Owner
            {
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                Email = Email,
                Address = Address
            };
            _repository.Add(owner);
            LoadData();
            ClearForm();
        }

        private void UpdateOwner()
        {
            if (SelectedOwner == null) return;
            SelectedOwner.FirstName = FirstName;
            SelectedOwner.LastName = LastName;
            SelectedOwner.Phone = Phone;
            SelectedOwner.Email = Email;
            SelectedOwner.Address = Address;
            _repository.Update(SelectedOwner);
            LoadData();
        }

        private void DeleteOwner()
        {
            if (SelectedOwner == null) return;
            _repository.Delete(SelectedOwner.Id);
            LoadData();
            ClearForm();
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
        }
    }
}
