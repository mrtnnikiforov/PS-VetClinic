using System.Collections.ObjectModel;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Entities;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class MedicalRecordViewModel : ViewModelBase
    {
        private readonly IRepository<MedicalRecord> _repository;

        private ObservableCollection<MedicalRecord> _records = new();
        public ObservableCollection<MedicalRecord> Records
        {
            get => _records;
            set => SetProperty(ref _records, value);
        }

        private MedicalRecord? _selectedRecord;
        public MedicalRecord? SelectedRecord
        {
            get => _selectedRecord;
            set => SetProperty(ref _selectedRecord, value);
        }

        private DateTime _date = DateTime.Today;
        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }

        private string _diagnosis = string.Empty;
        public string Diagnosis { get => _diagnosis; set => SetProperty(ref _diagnosis, value); }

        private string _treatment = string.Empty;
        public string Treatment { get => _treatment; set => SetProperty(ref _treatment, value); }

        private string _medications = string.Empty;
        public string Medications { get => _medications; set => SetProperty(ref _medications, value); }

        private decimal _cost;
        public decimal Cost { get => _cost; set => SetProperty(ref _cost, value); }

        private int _appointmentId;
        public int AppointmentId { get => _appointmentId; set => SetProperty(ref _appointmentId, value); }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public MedicalRecordViewModel(IRepository<MedicalRecord> repository)
        {
            _repository = repository;
            LoadCommand = new RelayCommand(_ => LoadData());
            AddCommand = new RelayCommand(_ => AddRecord());
            UpdateCommand = new RelayCommand(_ => UpdateRecord(), _ => SelectedRecord != null);
            DeleteCommand = new RelayCommand(_ => DeleteRecord(), _ => SelectedRecord != null);
            LoadData();
        }

        public void LoadData()
        {
            Records = new ObservableCollection<MedicalRecord>(_repository.GetAll());
        }

        private void AddRecord()
        {
            var record = new MedicalRecord
            {
                Date = Date,
                Diagnosis = Diagnosis,
                Treatment = Treatment,
                Medications = Medications,
                Cost = Cost,
                AppointmentId = AppointmentId
            };
            _repository.Add(record);
            LoadData();
            ClearForm();
        }

        private void UpdateRecord()
        {
            if (SelectedRecord == null) return;
            SelectedRecord.Date = Date;
            SelectedRecord.Diagnosis = Diagnosis;
            SelectedRecord.Treatment = Treatment;
            SelectedRecord.Medications = Medications;
            SelectedRecord.Cost = Cost;
            _repository.Update(SelectedRecord);
            LoadData();
        }

        private void DeleteRecord()
        {
            if (SelectedRecord == null) return;
            _repository.Delete(SelectedRecord.Id);
            LoadData();
            ClearForm();
        }

        private void ClearForm()
        {
            Date = DateTime.Today;
            Diagnosis = string.Empty;
            Treatment = string.Empty;
            Medications = string.Empty;
            Cost = 0;
            AppointmentId = 0;
        }
    }
}
