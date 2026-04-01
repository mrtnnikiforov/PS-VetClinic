using Avalonia.Controls;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Windows
{
    public partial class DogHistoryWindow : Window
    {
        public DogHistoryWindow(MainViewModel mainVm)
        {
            InitializeComponent();
            DataContext = new DogHistoryViewModel(
                mainVm.DogRepository,
                mainVm.AppointmentRepository,
                mainVm.MedicalRecordRepository);
        }
    }
}
