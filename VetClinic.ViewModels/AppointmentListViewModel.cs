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
            set
            {
                if (SetProperty(ref _selectedAppointment, value))
                {
                    (CompleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (CancelCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
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
            ClearError();
            if (SelectedAppointment == null) return;

            SelectedAppointment.Status = AppointmentStatus.Completed;
            try
            {
                _repository.Update(SelectedAppointment);
                LoadData();
            }
            catch (Exception ex)
            {
                SetError($"Could not complete appointment: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void CancelAppointment()
        {
            ClearError();
            if (SelectedAppointment == null) return;

            SelectedAppointment.Status = AppointmentStatus.Cancelled;
            try
            {
                _repository.Update(SelectedAppointment);
                LoadData();
            }
            catch (Exception ex)
            {
                SetError($"Could not cancel appointment: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void DeleteAppointment()
        {
            ClearError();
            if (SelectedAppointment == null) return;

            try
            {
                _repository.Delete(SelectedAppointment.Id);
                LoadData();
            }
            catch (Exception ex)
            {
                SetError($"Could not delete appointment: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
