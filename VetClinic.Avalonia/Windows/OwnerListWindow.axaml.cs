using Avalonia.Controls;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Windows
{
    public partial class OwnerListWindow : Window
    {
        public OwnerListViewModel CrudVm { get; }
        public GenericListViewModel ListVm { get; }

        public OwnerListWindow(MainViewModel mainVm)
        {
            CrudVm = new OwnerListViewModel(mainVm.OwnerRepository);
            ListVm = new GenericListViewModel(typeof(Owner));
            ListVm.SetItems(CrudVm.Owners);
            CrudVm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CrudVm.Owners))
                {
                    ListVm.SetItems(CrudVm.Owners);
                }
            };
            ListVm.ItemSelected += item =>
            {
                if (item is Owner owner)
                {
                    CrudVm.SelectedOwner = owner;
                    CrudVm.FirstName = owner.FirstName;
                    CrudVm.LastName = owner.LastName;
                    CrudVm.Phone = owner.Phone;
                    CrudVm.Email = owner.Email;
                    CrudVm.Address = owner.Address;
                }
            };

            InitializeComponent();
            DataContext = this;
        }
    }
}
