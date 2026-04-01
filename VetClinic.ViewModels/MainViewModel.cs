using System.Windows.Input;
using VetClinic.Common;
using VetClinic.DataLayer.Factories;
using VetClinic.DataLayer.Repositories;
using VetClinic.Model.Entities;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IDatabaseContextFactory _contextFactory;

        private string _currentDatabase = "SQLite";
        public string CurrentDatabase
        {
            get => _currentDatabase;
            set => SetProperty(ref _currentDatabase, value);
        }

        public IRepository<Dog> DogRepository { get; private set; }
        public IRepository<Owner> OwnerRepository { get; private set; }
        public IRepository<Veterinarian> VetRepository { get; private set; }
        public IRepository<Appointment> AppointmentRepository { get; private set; }
        public IRepository<MedicalRecord> MedicalRecordRepository { get; private set; }

        public ICommand SwitchDatabaseCommand { get; }

        public MainViewModel()
        {
            _contextFactory = new SqliteContextFactory();
            DogRepository = new GenericRepository<Dog>(_contextFactory);
            OwnerRepository = new GenericRepository<Owner>(_contextFactory);
            VetRepository = new GenericRepository<Veterinarian>(_contextFactory);
            AppointmentRepository = new GenericRepository<Appointment>(_contextFactory);
            MedicalRecordRepository = new GenericRepository<MedicalRecord>(_contextFactory);

            SwitchDatabaseCommand = new RelayCommand(param => SwitchDatabase(param?.ToString()));
        }

        private void SwitchDatabase(string? dbType)
        {
            if (string.IsNullOrEmpty(dbType)) return;

            if (dbType == "SQLite")
            {
                _contextFactory = new SqliteContextFactory();
                CurrentDatabase = "SQLite";
            }
            else if (dbType == "SQL Server")
            {
                _contextFactory = new SqlServerContextFactory();
                CurrentDatabase = "SQL Server";
            }

            DogRepository = new GenericRepository<Dog>(_contextFactory);
            OwnerRepository = new GenericRepository<Owner>(_contextFactory);
            VetRepository = new GenericRepository<Veterinarian>(_contextFactory);
            AppointmentRepository = new GenericRepository<Appointment>(_contextFactory);
            MedicalRecordRepository = new GenericRepository<MedicalRecord>(_contextFactory);

            OnPropertyChanged(nameof(DogRepository));
            OnPropertyChanged(nameof(OwnerRepository));
            OnPropertyChanged(nameof(VetRepository));
            OnPropertyChanged(nameof(AppointmentRepository));
            OnPropertyChanged(nameof(MedicalRecordRepository));
        }
    }
}
