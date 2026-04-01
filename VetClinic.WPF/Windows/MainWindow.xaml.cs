using System.Windows;
using System.Windows.Controls;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Windows
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainVm;

        public MainWindow()
        {
            InitializeComponent();
            _mainVm = new MainViewModel();
            DataContext = _mainVm;
        }

        private void DbComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DbComboBox.SelectedItem is ComboBoxItem item)
            {
                _mainVm.SwitchDatabaseCommand.Execute(item.Content?.ToString());
            }
        }

        private void OpenDogList_Click(object sender, RoutedEventArgs e)
        {
            var window = new DogListWindow(_mainVm);
            window.Show();
        }

        private void OpenOwnerList_Click(object sender, RoutedEventArgs e)
        {
            var window = new OwnerListWindow(_mainVm);
            window.Show();
        }

        private void OpenVetList_Click(object sender, RoutedEventArgs e)
        {
            var window = new VetListWindow(_mainVm);
            window.Show();
        }

        private void OpenDogSearch_Click(object sender, RoutedEventArgs e)
        {
            var window = new DogSearchWindow(_mainVm);
            window.Show();
        }

        private void OpenOwnerSearch_Click(object sender, RoutedEventArgs e)
        {
            var window = new OwnerSearchWindow(_mainVm);
            window.Show();
        }

        private void OpenAppointmentList_Click(object sender, RoutedEventArgs e)
        {
            var window = new AppointmentListWindow(_mainVm);
            window.Show();
        }

        private void OpenAppointmentScheduler_Click(object sender, RoutedEventArgs e)
        {
            var window = new AppointmentSchedulerWindow(_mainVm);
            window.Show();
        }

        private void OpenMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            var window = new MedicalRecordWindow(_mainVm);
            window.Show();
        }

        private void OpenDogHistory_Click(object sender, RoutedEventArgs e)
        {
            var window = new DogHistoryWindow(_mainVm);
            window.Show();
        }

        private void OpenAppointmentSearch_Click(object sender, RoutedEventArgs e)
        {
            var window = new AppointmentSearchWindow(_mainVm);
            window.Show();
        }

        private void OpenMedicalRecordSearch_Click(object sender, RoutedEventArgs e)
        {
            var window = new MedicalRecordSearchWindow(_mainVm);
            window.Show();
        }
    }
}
