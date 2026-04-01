using Avalonia.Controls;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Windows
{
    public partial class AppointmentSearchWindow : Window
    {
        public AppointmentSearchWindow(MainViewModel mainVm)
        {
            InitializeComponent();
            DataContext = new SearchFilterViewModel(typeof(Appointment), mainVm.AppointmentRepository);
        }
    }
}
