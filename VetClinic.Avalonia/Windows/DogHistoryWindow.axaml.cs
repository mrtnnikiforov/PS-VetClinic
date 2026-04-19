using Avalonia.Controls;
using Avalonia.Data;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Windows
{
    public partial class DogHistoryWindow : Window
    {
        public DogHistoryWindow(MainViewModel mainVm)
        {
            InitializeComponent();
            DataContext = new DogHistoryViewModel(
                mainVm.DogRepository,
                mainVm.AppointmentRepository,
                mainVm.MedicalRecordRepository);

            BuildColumns(AppointmentsDataGrid, typeof(Appointment));
            BuildColumns(MedicalRecordsDataGrid, typeof(MedicalRecord));
        }

        private static void BuildColumns(DataGrid grid, Type entityType)
        {
            grid.Columns.Clear();
            foreach (var col in ReflectionHelper.GetDisplayableColumns(entityType))
            {
                grid.Columns.Add(new DataGridTextColumn
                {
                    Header = col.Header,
                    Binding = new Binding(col.PropertyName)
                });
            }
        }
    }
}
