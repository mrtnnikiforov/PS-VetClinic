using System.Windows;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Windows
{
    public partial class DogSearchWindow : Window
    {
        public DogSearchWindow(MainViewModel mainVm)
        {
            InitializeComponent();
            DataContext = new SearchFilterViewModel(typeof(Dog), mainVm.DogRepository);
        }
    }
}
