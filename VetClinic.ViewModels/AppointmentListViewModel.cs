using System.Collections.ObjectModel;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.Model.Enums;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class AppointmentListViewModel : ViewModelBase
    {
        private readonly IRepository<Appointment> _repository;

        private ObservableCollection<Appointment> _appointments = new();
        public ObservableCollection<Appointment> Appointments
        {
            get => _appointments;
            set => SetProperty(ref _appointments, value);
        }

        private Appointment? _selectedAppointment;
        public Appointment? SelectedAppointment
        {
            get => _selectedAppointment;
            set => SetProperty(ref _selectedAppointment, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand CompleteCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }

        public AppointmentListViewModel(IRepository<Appointment> repository)
        {
            _repository = repository;
            LoadCommand = new RelayCommand(_ => LoadData());
            CompleteCommand = new RelayCommand(_ => CompleteAppointment(), _ => SelectedAppointment != null);
            CancelCommand = new RelayCommand(_ => CancelAppointment(), _ => SelectedAppointment != null);
            DeleteCommand = new RelayCommand(_ => DeleteAppointment(), _ => SelectedAppointment != null);
            LoadData();
        }

        public void LoadData()
        {
            Appointments = new ObservableCollection<Appointment>(_repository.GetAll());
        }

        private void CompleteAppointment()
        {
            if (SelectedAppointment == null) return;
            SelectedAppointment.Status = AppointmentStatus.Completed;
            _repository.Update(SelectedAppointment);
            LoadData();
        }

        private void CancelAppointment()
        {
            if (SelectedAppointment == null) return;
            SelectedAppointment.Status = AppointmentStatus.Cancelled;
            _repository.Update(SelectedAppointment);
            LoadData();
        }

        private void DeleteAppointment()
        {
            if (SelectedAppointment == null) return;
            _repository.Delete(SelectedAppointment.Id);
            LoadData();
        }
    }
}
