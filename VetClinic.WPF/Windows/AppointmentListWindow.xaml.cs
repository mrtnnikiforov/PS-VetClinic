using System.Windows;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Windows
{
    public partial class AppointmentListWindow : Window
    {
        public AppointmentListViewModel CrudVm { get; }
        public GenericListViewModel ListVm { get; }

        public AppointmentListWindow(MainViewModel mainVm)
        {
            CrudVm = new AppointmentListViewModel(mainVm.AppointmentRepository);
            ListVm = new GenericListViewModel(typeof(Appointment));
            ListVm.SetItems(CrudVm.Appointments);
            ListVm.ItemSelected += item =>
            {
                if (item is Appointment appt)
                {
                    CrudVm.SelectedAppointment = appt;
                }
            };

            InitializeComponent();
            DataContext = this;
        }
    }
}
