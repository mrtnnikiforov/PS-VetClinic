using System.Collections.ObjectModel;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class VetListViewModel : ViewModelBase
    {
        private readonly IRepository<Veterinarian> _repository;

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
            set => SetProperty(ref _selectedVet, value);
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
            Veterinarians = new ObservableCollection<Veterinarian>(_repository.GetAll());
        }

        private void AddVet()
        {
            var vet = new Veterinarian
            {
                FirstName = FirstName,
                LastName = LastName,
                Specialization = Specialization,
                LicenseNumber = LicenseNumber,
                Phone = Phone
            };
            _repository.Add(vet);
            LoadData();
            ClearForm();
        }

        private void UpdateVet()
        {
            if (SelectedVet == null) return;
            SelectedVet.FirstName = FirstName;
            SelectedVet.LastName = LastName;
            SelectedVet.Specialization = Specialization;
            SelectedVet.LicenseNumber = LicenseNumber;
            SelectedVet.Phone = Phone;
            _repository.Update(SelectedVet);
            LoadData();
        }

        private void DeleteVet()
        {
            if (SelectedVet == null) return;
            _repository.Delete(SelectedVet.Id);
            LoadData();
            ClearForm();
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Specialization = string.Empty;
            LicenseNumber = string.Empty;
            Phone = string.Empty;
        }
    }
}
