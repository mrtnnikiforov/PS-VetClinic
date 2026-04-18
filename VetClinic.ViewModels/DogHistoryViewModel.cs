using System.Collections.ObjectModel;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class DogHistoryViewModel : ViewModelBase
    {
        private readonly IRepository<Dog> _dogRepository;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<MedicalRecord> _medicalRecordRepository;

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
                    LoadHistory();
                }
            }
        }

        private ObservableCollection<Appointment> _appointments = new();
        public ObservableCollection<Appointment> Appointments
        {
            get => _appointments;
            set => SetProperty(ref _appointments, value);
        }

        private ObservableCollection<MedicalRecord> _medicalRecords = new();
        public ObservableCollection<MedicalRecord> MedicalRecords
        {
            get => _medicalRecords;
            set => SetProperty(ref _medicalRecords, value);
        }

        public ICommand LoadCommand { get; }

        public DogHistoryViewModel(
            IRepository<Dog> dogRepository,
            IRepository<Appointment> appointmentRepository,
            IRepository<MedicalRecord> medicalRecordRepository)
        {
            _dogRepository = dogRepository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;

            LoadCommand = new RelayCommand(_ => LoadDogs());
            LoadDogs();
        }

        private void LoadDogs()
        {
            Dogs = new ObservableCollection<Dog>(_dogRepository.GetAll());
        }

        private void LoadHistory()
        {
            if (SelectedDog == null)
            {
                Appointments = new ObservableCollection<Appointment>();
                MedicalRecords = new ObservableCollection<MedicalRecord>();
                return;
            }

            var dogAppointments = _appointmentRepository.Query(a => a.DogId == SelectedDog.Id, "Veterinarian");
            Appointments = new ObservableCollection<Appointment>(
                dogAppointments.OrderByDescending(a => a.DateTime));

            var appointmentIds = dogAppointments.Select(a => a.Id).ToHashSet();
            var records = _medicalRecordRepository.Query(r => appointmentIds.Contains(r.AppointmentId));
            MedicalRecords = new ObservableCollection<MedicalRecord>(
                records.OrderByDescending(r => r.Date));
        }
    }
}
