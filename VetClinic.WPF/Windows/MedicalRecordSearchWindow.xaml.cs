using System.Windows;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Windows
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
