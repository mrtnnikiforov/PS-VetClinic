using Avalonia.Controls;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Windows
{
    public partial class MedicalRecordSearchWindow : Window
    {
        public MedicalRecordSearchWindow(MainViewModel mainVm)
        {
            InitializeComponent();
            DataContext = new SearchFilterViewModel(typeof(MedicalRecord), mainVm.MedicalRecordRepository);
        }
    }
}
