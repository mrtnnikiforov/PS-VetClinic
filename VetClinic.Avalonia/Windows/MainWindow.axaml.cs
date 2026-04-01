using Avalonia.Controls;
using Avalonia.Interactivity;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Windows
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainVm;

        public MainWindow()
        {
            InitializeComponent();
            _mainVm = new MainViewModel();
            DataContext = _mainVm;

            var dbCombo = this.FindControl<ComboBox>("DbComboBox");
            if (dbCombo != null)
            {
                dbCombo.SelectionChanged += (s, e) =>
                {
                    if (dbCombo.SelectedItem is ComboBoxItem item)
                    {
                        _mainVm.SwitchDatabaseCommand.Execute(item.Content?.ToString());
                    }
                };
            }
        }

        private void OpenDogList_Click(object? sender, RoutedEventArgs e)
        {
            new DogListWindow(_mainVm).Show();
        }

        private void OpenOwnerList_Click(object? sender, RoutedEventArgs e)
        {
            new OwnerListWindow(_mainVm).Show();
        }

        private void OpenVetList_Click(object? sender, RoutedEventArgs e)
        {
            new VetListWindow(_mainVm).Show();
        }

        private void OpenDogSearch_Click(object? sender, RoutedEventArgs e)
        {
            new DogSearchWindow(_mainVm).Show();
        }

        private void OpenOwnerSearch_Click(object? sender, RoutedEventArgs e)
        {
            new OwnerSearchWindow(_mainVm).Show();
        }

        private void OpenAppointmentList_Click(object? sender, RoutedEventArgs e)
        {
            new AppointmentListWindow(_mainVm).Show();
        }

        private void OpenAppointmentScheduler_Click(object? sender, RoutedEventArgs e)
        {
            new AppointmentSchedulerWindow(_mainVm).Show();
        }

        private void OpenMedicalRecord_Click(object? sender, RoutedEventArgs e)
        {
            new MedicalRecordWindow(_mainVm).Show();
        }

        private void OpenDogHistory_Click(object? sender, RoutedEventArgs e)
        {
            new DogHistoryWindow(_mainVm).Show();
        }

        private void OpenAppointmentSearch_Click(object? sender, RoutedEventArgs e)
        {
            new AppointmentSearchWindow(_mainVm).Show();
        }

        private void OpenMedicalRecordSearch_Click(object? sender, RoutedEventArgs e)
        {
            new MedicalRecordSearchWindow(_mainVm).Show();
        }
    }
}
