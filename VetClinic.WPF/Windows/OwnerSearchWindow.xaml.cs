using System.Windows;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Windows
{
    public partial class OwnerSearchWindow : Window
    {
        public OwnerSearchWindow(MainViewModel mainVm)
        {
            InitializeComponent();
            DataContext = new SearchFilterViewModel(typeof(Owner), mainVm.OwnerRepository);
        }
    }
}
