using System.Windows;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Windows
{
    public partial class VetListWindow : Window
    {
        public VetListViewModel CrudVm { get; }
        public GenericListViewModel ListVm { get; }

        public VetListWindow(MainViewModel mainVm)
        {
            CrudVm = new VetListViewModel(mainVm.VetRepository);
            ListVm = new GenericListViewModel(typeof(Veterinarian));
            ListVm.SetItems(CrudVm.Veterinarians);
            CrudVm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CrudVm.Veterinarians))
                {
                    ListVm.SetItems(CrudVm.Veterinarians);
                }
            };
            ListVm.ItemSelected += item =>
            {
                if (item is Veterinarian vet)
                {
                    CrudVm.SelectedVet = vet;
                    CrudVm.FirstName = vet.FirstName;
                    CrudVm.LastName = vet.LastName;
                    CrudVm.Specialization = vet.Specialization;
                    CrudVm.LicenseNumber = vet.LicenseNumber;
                    CrudVm.Phone = vet.Phone;
                }
            };

            InitializeComponent();
            DataContext = this;
        }
    }
}
