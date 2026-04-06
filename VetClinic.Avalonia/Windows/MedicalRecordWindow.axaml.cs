using Avalonia.Controls;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Windows
{
    public partial class MedicalRecordWindow : Window
    {
        public MedicalRecordViewModel CrudVm { get; }
        public GenericListViewModel ListVm { get; }

        public MedicalRecordWindow(MainViewModel mainVm)
        {
            CrudVm = new MedicalRecordViewModel(mainVm.MedicalRecordRepository);
            ListVm = new GenericListViewModel(typeof(MedicalRecord));
            ListVm.SetItems(CrudVm.Records);
            CrudVm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CrudVm.Records))
                {
                    ListVm.SetItems(CrudVm.Records);
                }
            };
            ListVm.ItemSelected += item =>
            {
                if (item is MedicalRecord rec)
                {
                    CrudVm.SelectedRecord = rec;
                    CrudVm.Date = rec.Date;
                    CrudVm.Diagnosis = rec.Diagnosis;
                    CrudVm.Treatment = rec.Treatment;
                    CrudVm.Medications = rec.Medications;
                    CrudVm.Cost = rec.Cost;
                    CrudVm.AppointmentId = rec.AppointmentId;
                }
            };

            InitializeComponent();
            DataContext = this;
        }
    }
}
