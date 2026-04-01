using System.Windows;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Windows
{
    public partial class DogListWindow : Window
    {
        public DogListViewModel CrudVm { get; }
        public GenericListViewModel ListVm { get; }

        public DogListWindow(MainViewModel mainVm)
        {
            CrudVm = new DogListViewModel(mainVm.DogRepository);
            ListVm = new GenericListViewModel(typeof(Dog));
            ListVm.SetItems(CrudVm.Dogs);
            ListVm.ItemSelected += item =>
            {
                if (item is Dog dog)
                {
                    CrudVm.SelectedDog = dog;
                    CrudVm.Name = dog.Name;
                    CrudVm.Breed = dog.Breed;
                    CrudVm.DateOfBirth = dog.DateOfBirth;
                    CrudVm.WeightKg = dog.WeightKg;
                    CrudVm.ChipNumber = dog.ChipNumber;
                    CrudVm.OwnerId = dog.OwnerId;
                }
            };

            InitializeComponent();
            DataContext = this;
        }
    }
}
