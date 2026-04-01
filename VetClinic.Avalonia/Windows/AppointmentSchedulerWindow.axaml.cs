using Avalonia.Controls;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Windows
{
    public partial class AppointmentSchedulerWindow : Window
    {
        public AppointmentSchedulerWindow(MainViewModel mainVm)
        {
            InitializeComponent();
            DataContext = new AppointmentSchedulerViewModel(
                mainVm.AppointmentRepository,
                mainVm.DogRepository,
                mainVm.VetRepository);
        }
    }
}
