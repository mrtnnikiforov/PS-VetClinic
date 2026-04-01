using System.Windows;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Windows
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
