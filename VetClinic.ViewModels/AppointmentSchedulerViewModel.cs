using System.Collections.ObjectModel;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.Model.Enums;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class AppointmentSchedulerViewModel : ViewModelBase
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Dog> _dogRepository;
        private readonly IRepository<Veterinarian> _vetRepository;

        private ObservableCollection<Dog> _dogs = new();
        public ObservableCollection<Dog> Dogs
        {
            get => _dogs;
            set => SetProperty(ref _dogs, value);
        }

        private ObservableCollection<Veterinarian> _veterinarians = new();
        public ObservableCollection<Veterinarian> Veterinarians
        {
            get => _veterinarians;
            set => SetProperty(ref _veterinarians, value);
        }

        private Dog? _selectedDog;
        public Dog? SelectedDog
        {
            get => _selectedDog;
            set => SetProperty(ref _selectedDog, value);
        }

        private Veterinarian? _selectedVet;
        public Veterinarian? SelectedVet
        {
            get => _selectedVet;
            set => SetProperty(ref _selectedVet, value);
        }

        private DateTime _appointmentDate = DateTime.Today.AddDays(1);
        public DateTime AppointmentDate
        {
            get => _appointmentDate;
            set => SetProperty(ref _appointmentDate, value);
        }

        private string _reason = string.Empty;
        public string Reason { get => _reason; set => SetProperty(ref _reason, value); }

        private string _notes = string.Empty;
        public string Notes { get => _notes; set => SetProperty(ref _notes, value); }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand ScheduleCommand { get; }

        public AppointmentSchedulerViewModel(
            IRepository<Appointment> appointmentRepository,
            IRepository<Dog> dogRepository,
            IRepository<Veterinarian> vetRepository)
        {
            _appointmentRepository = appointmentRepository;
            _dogRepository = dogRepository;
            _vetRepository = vetRepository;

            ScheduleCommand = new RelayCommand(_ => ScheduleAppointment(),
                _ => SelectedDog != null && SelectedVet != null && !string.IsNullOrWhiteSpace(Reason));

            LoadData();
        }

        private void LoadData()
        {
            Dogs = new ObservableCollection<Dog>(_dogRepository.GetAll());
            Veterinarians = new ObservableCollection<Veterinarian>(_vetRepository.GetAll());
        }

        private void ScheduleAppointment()
        {
            if (SelectedDog == null || SelectedVet == null) return;

            var appointment = new Appointment
            {
                DateTime = AppointmentDate,
                Reason = Reason,
                Notes = Notes,
                Status = AppointmentStatus.Scheduled,
                DogId = SelectedDog.Id,
                VeterinarianId = SelectedVet.Id
            };

            _appointmentRepository.Add(appointment);
            StatusMessage = $"Appointment scheduled for {SelectedDog.Name} with {SelectedVet.FirstName} {SelectedVet.LastName} on {AppointmentDate:d}";

            Reason = string.Empty;
            Notes = string.Empty;
            AppointmentDate = DateTime.Today.AddDays(1);
        }
    }
}
