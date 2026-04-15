# Практически Проект по Програмни Среди

## Тема: Система за управление на ветеринарна клиника за кучета (VetClinic)

**Изготвили:** [Костадин Харизанов] Ф.Н.: [121223018],
[Мартин Никифоров] Ф.Н.: [121223034]

**Специалност:** КСИ
**Група:** 44

---

## 1. Въведение

Настоящият проект представлява система за управление на ветеринарна клиника за кучета — **VetClinic**. Приложението обслужва основните нужди на една ветеринарна практика: регистриране на кучета и техните собственици, управление на ветеринарни лекари, насрочване и проследяване на прегледи, както и водене на медицински досиета.

### Функционалности:

- **Управление на кучета** — добавяне, редактиране, изтриване и преглед на данни за пациентите (порода, тегло, чип номер, дата на раждане).
- **Управление на собственици** — CRUD операции за контактна информация на собствениците.
- **Управление на ветеринарни лекари** — данни за специализация, лицензионен номер и контакти.
- **Насрочване на прегледи** — създаване на прегледи с избор на куче, лекар, дата и причина. Смяна на статус (Насрочен, Завършен, Отменен).
- **Медицински записи** — диагнози, лечения, медикаменти и цена, свързани с конкретен преглед.
- **История на прегледите** — преглед на цялата медицинска история на конкретно куче.
- **Универсално търсене** — генерично търсене по полета за всяка таблица чрез динамично генерирани филтри.
- **Универсален списък** — генерично визуализиране и избор от списък с динамично конфигурируеми колони.

Системата работи с две различни View технологии (**WPF** и **AvaloniaUI**) и две различни бази данни (**SQLite** и **SQL Server LocalDB**), като ViewModel и бизнес логиката се преизползват без дублиране.

---

## 2. Представяне на View технологии и бази данни

### 2.1. WPF (Windows Presentation Foundation)

WPF е UI framework от Microsoft за създаване на desktop приложения под Windows. Използва XAML (Extensible Application Markup Language) за деклариране на интерфейси и поддържа мощна система за data binding, стилове и шаблони.

**Какво научихме при работа с WPF:**
- Data binding системата е изключително удобна — `{Binding PropertyName}` директно свързва свойствата на ViewModel с визуалните елементи.
- `DataTemplateSelector` позволява избор на различни визуални представяния на базата на типа данни — ключово за нашия генеричен SearchFilterControl.
- `ObservableCollection<T>` автоматично уведомява UI-а при промени в колекцията.
- `ICommand` интерфейсът (чрез нашия `RelayCommand`) позволява бутоните да извикват логика от ViewModel без code-behind.

### 2.2. AvaloniaUI

AvaloniaUI е cross-platform UI framework за .NET, който използва подобен на WPF XAML синтаксис (`.axaml` файлове). Основното му предимство е, че работи на Windows, macOS и Linux.

**Какво научихме при работа с Avalonia:**
- XAML синтаксисът е почти идентичен с WPF — `{Binding}`, `ItemsSource`, `DataContext` работят по същия начин.
- Разликата е в namespace декларацията: `xmlns="https://github.com/avaloniaui"` вместо WPF-ския.
- `DataTemplateSelector` не съществува в Avalonia — вместо него се използва `IDataTemplate` с метод `Match()`, или по-прости подходи като единен `DataTemplate` с `TextBox`.
- `DataGrid` идва като отделен пакет (`Avalonia.Controls.DataGrid`), не е вграден в ядрото.
- Файловете са `.axaml` вместо `.xaml`, но вътрешната структура е почти еднаква.

### 2.3. Разлики между WPF и AvaloniaUI

| Аспект | WPF | AvaloniaUI |
|--------|-----|-----------|
| Платформа | Само Windows | Windows, macOS, Linux |
| XAML файлове | `.xaml` | `.axaml` |
| DataTemplateSelector | Вграден | Няма — използва `IDataTemplate` |
| DataGrid | Вграден | Отделен NuGet пакет |
| Grid синтаксис | `<Grid.RowDefinitions>` дъщерни елементи | `RowDefinitions="Auto,*"` атрибут |
| Зрялост | 20+ години, стабилен | По-нов, по-малка общност |

### 2.4. SQLite

SQLite е вградена файлова база данни — не изисква сървър, базата е единичен `.db` файл. Идеална за development и леки приложения.

**Нашият опит:** Нулева конфигурация — само NuGet пакет и connection string с път до файл. `Database.EnsureCreated()` създава файла и таблиците автоматично.

### 2.5. SQL Server LocalDB

SQL Server LocalDB е олекотена версия на SQL Server, която идва с Visual Studio. Работи като истински SQL Server процес, но не изисква отделна инсталация.

**Нашият опит:** Connection string-ът е `Server=(localdb)\mssqllocaldb;Database=VetClinic;Trusted_Connection=True;`. Базата се създава автоматично чрез `EnsureCreated()`.

### 2.6. Разлики между SQLite и SQL Server

| Аспект | SQLite | SQL Server LocalDB |
|--------|--------|-------------------|
| Тип | Вградена файлова БД | Клиент-сървър |
| Инсталация | Не изисква | Идва с Visual Studio |
| Паралелен достъп | Ограничен | Пълен |
| Типове данни | Динамични (type affinity) | Строги |
| Функции | Ограничен набор | Пълен SQL Server набор |
| Подходяща за | Development, мобилни приложения | Enterprise приложения |

---

## 3. Работа със среда и библиотеки

Този раздел описва „невидимата" работа, която не се вижда в крайния продукт, но беше необходима за разработката.

### 3.1. Настройка на Entity Framework Core

Инсталирахме три EF Core NuGet пакета в `VetClinic.DataLayer`:
- `Microsoft.EntityFrameworkCore` (10.0.3) — ядрото на ORM-a
- `Microsoft.EntityFrameworkCore.Sqlite` (10.0.3) — SQLite provider
- `Microsoft.EntityFrameworkCore.SqlServer` (10.0.3) — SQL Server provider
- `Microsoft.EntityFrameworkCore.Tools` (10.0.3) — инструменти за миграции

Научихме, че EF Core абстрахира базата чрез `DbContext` — дефинираме `DbSet<T>` свойства и рамката генерира SQL-а автоматично. Разликата между двете бази е само в `OnConfiguring()` метода.

### 3.2. Настройка на Avalonia

За Avalonia добавихме следните NuGet пакети:
- `Avalonia` (11.2.7) — ядро
- `Avalonia.Desktop` (11.2.7) — desktop поддръжка
- `Avalonia.Themes.Fluent` (11.2.7) — визуална тема
- `Avalonia.Controls.DataGrid` (11.2.7) — DataGrid контрола

Avalonia изисква специален `Program.cs` с `AppBuilder.Configure<App>().UsePlatformDetect()` — различно от WPF, където `App.xaml` е достатъчен. Също така `App.axaml` използва `<FluentTheme />` вместо WPF стиловете.

### 3.3. Конфигурация на .NET 10.0

Проектът е базиран на .NET 10.0. Всички проекти използват `<ImplicitUsings>enable</ImplicitUsings>` и `<Nullable>enable</Nullable>`, следвайки шаблона от упражненията.

WPF проектът изисква `<TargetFramework>net10.0-windows</TargetFramework>` и `<UseWPF>true</UseWPF>`, докато останалите проекти (включително Avalonia) целят стандартен `net10.0`.

### 3.4. Solution структура и проектни референции

Конфигурирахме 6 проекта с ясна йерархия на зависимостите:
```
Model → (няма зависимости)
Common → Model
DataLayer → Model
ViewModels → Model, Common, DataLayer
WPF → ViewModels, Common, DataLayer
Avalonia → ViewModels, Common, DataLayer
```

Тази структура гарантира, че ViewModels проектът няма зависимост от нито един UI framework.

---

## 4. Описание на срещнати и преодолени трудности

### 4.1. Споделяне на ViewModel между WPF и Avalonia

**Проблем:** Първоначално опитахме да наследим `DependencyObject` в ViewModel класовете (както е в примера от упражненията `EasyMVVM/MainWindowVM.cs`). Това направи ViewModels зависими от WPF и невъзможни за използване в Avalonia.

**Решение:** Премахнахме `DependencyObject` и използвахме само `INotifyPropertyChanged` от `System.ComponentModel`, който е част от стандартната .NET библиотека и работи и с двата UI framework-а. Създадохме `ViewModelBase` абстрактен клас с `SetProperty<T>()` helper метод.

### 4.2. Липса на DataTemplateSelector в Avalonia

**Проблем:** WPF има `DataTemplateSelector`, който избира различни визуални шаблони на базата на данните. В нашия `SearchFilterControl` го използваме за показване на `TextBox` за текстови полета, `DatePicker` за дати и `ComboBox` за enum-и. Avalonia няма еквивалент.

**Решение:** За Avalonia версията на `SearchFilterControl` използвахме единен `DataTemplate` с `TextBox` контрола за всички типове полета. Потребителят въвежда стойностите текстово, а `SearchFilterViewModel` ги парсира според `PropertyType`. Това е компромис, но запазва генеричността.

### 4.3. Динамично изграждане на DataGrid колони

**Проблем:** И в WPF, и в Avalonia `DataGrid` не поддържа директно binding на `Columns` свойството — колоните не са dependency property. Не можехме да напишем `Columns="{Binding Columns}"` в XAML.

**Решение:** Решихме проблема чрез code-behind в `GenericListControl`. Прихващаме `DataContextChanged` и `CollectionChanged` събития, и програматично пресъздаваме `DataGridTextColumn` обекти от нашата `ColumnDefinition` колекция:

```csharp
private void RebuildColumns(ObservableCollection<ColumnDefinition> columns)
{
    ListDataGrid.Columns.Clear();
    foreach (var col in columns)
    {
        ListDataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = col.Header,
            Binding = new Binding(col.PropertyName)
        });
    }
}
```

### 4.4. Изграждане на динамични LINQ заявки чрез Expression Trees

**Проблем:** `SearchFilterViewModel` трябваше да генерира LINQ `Where` клаузи динамично, без да знае конкретния тип ентитет при компилация. Стандартните ламбда изрази не позволяват това.

**Решение:** Използвахме `System.Linq.Expressions.Expression` API за програматично изграждане на предикати. За string полета генерираме `Contains()` извикване, за числови — `Equal`, за дати — `GreaterThanOrEqual`, за enum-и — `Equal` с парснат enum. Комбинирахме отделните условия с `Expression.AndAlso()`.

### 4.5. Работа с два различни DbContext-а

**Проблем:** Трябваше бизнес логиката да работи и със SQLite, и със SQL Server, без дублиране на код.

**Решение:** Създадохме абстрактен `VetClinicContext`, който дефинира всички `DbSet<T>` свойства и relationship-ите в `OnModelCreating()`. Двете конкретни реализации (`SqliteVetContext`, `SqlServerVetContext`) само override-ват `OnConfiguring()`. `GenericRepository<T>` приема `IDatabaseContextFactory` и не знае коя база се използва.

---

## 5. Описание на реализацията

### 5.1. Архитектура

Проектът следва **MVVM (Model-View-ViewModel)** архитектурен шаблон с ясно разделение на отговорностите:

```
┌──────────────────────┐
│      View Layer      │  VetClinic.WPF / VetClinic.Avalonia
│  (XAML/AXAML + C#)   │  Визуализация, потребителски вход
├──────────────────────┤
│   ViewModel Layer    │  VetClinic.ViewModels
│  (C# класове)        │  Бизнес логика, команди, навигация
├──────────────────────┤
│    Model Layer       │  VetClinic.Model + VetClinic.DataLayer
│  (Entities + EF Core)│  Данни, достъп до БД, валидация
├──────────────────────┤
│   Common Layer       │  VetClinic.Common
│  (MVVM инфраструктура)│ ViewModelBase, RelayCommand, Reflection
└──────────────────────┘
```

**Model Layer** — отговаря за дефиницията на данните (ентитети, enum-и, атрибути), интерфейси за достъп до данни (`IRepository<T>`) и конкретните реализации на достъп до базата (`GenericRepository<T>`, `VetClinicContext`).

**ViewModel Layer** — съдържа цялата бизнес логика, CRUD операции, навигация и генерични компоненти за търсене и списъци. Не зависи от нито един UI framework.

**View Layer** — съдържа XAML/AXAML файлове и минимален code-behind (само за инстанциране на ViewModel и програматично изграждане на DataGrid колони).

### 5.2. Разпределение на типовете по слоеве

**VetClinic.Model (Model Layer):**
| Тип | Описание |
|-----|----------|
| `Dog`, `Owner`, `Veterinarian` | Основни ентитети |
| `Appointment`, `MedicalRecord` | Ентитети за прегледи и записи |
| `AppointmentStatus` | Enum за статус на преглед |
| `SearchableAttribute` | Маркира свойства за търсене |
| `DisplayableAttribute` | Маркира свойства за показване в списък |
| `IRepository<T>` | Интерфейс за достъп до данни |
| `IDatabaseContextFactory` | Интерфейс за създаване на DbContext |

**VetClinic.Common (Infrastructure):**
| Тип | Описание |
|-----|----------|
| `ViewModelBase` | Базов клас с INotifyPropertyChanged |
| `RelayCommand` | Реализация на ICommand |
| `FilterFieldInfo` | Описание на едно филтриращо поле |
| `ColumnDefinition` | Описание на една колона за списък |
| `ReflectionHelper` | Извличане на метаданни чрез reflection |

**VetClinic.DataLayer (Data Access):**
| Тип | Описание |
|-----|----------|
| `VetClinicContext` | Абстрактен DbContext с всички DbSet-ове |
| `SqliteVetContext` | SQLite конфигурация |
| `SqlServerVetContext` | SQL Server конфигурация |
| `SqliteContextFactory` | Фабрика за SQLite контекст |
| `SqlServerContextFactory` | Фабрика за SQL Server контекст |
| `GenericRepository<T>` | Генеричен repository за CRUD операции |

**VetClinic.ViewModels (ViewModel Layer):**
| Тип | Описание
|-----|---------
| `MainViewModel` | Навигация, превключване на БД |
| `DogListViewModel` | CRUD за кучета | 
| `OwnerListViewModel` | CRUD за собственици | 
| `VetListViewModel` | CRUD за ветеринари |
| `SearchFilterViewModel` | Генерично търсене |
| `GenericListViewModel` | Генеричен списък |
| `AppointmentListViewModel` | CRUD за прегледи | 
| `MedicalRecordViewModel` | CRUD за мед. записи | 
| `AppointmentSchedulerViewModel` | Насрочване на преглед |
| `DogHistoryViewModel` | История на прегледи 

### 5.3. Типове и функции

**ViewModelBase** — абстрактен клас, реализиращ `INotifyPropertyChanged`. Предоставя `SetProperty<T>()` метод, който автоматично нотифицира UI-а при промяна на свойство и `OnPropertyChanged()` за ръчно уведомяване.

**RelayCommand** — реализация на `ICommand`, приемаща `Action<object?>` за изпълнение и опционален `Func<object?, bool>` за `CanExecute`. Позволява XAML бутони да извикват ViewModel методи.

**GenericRepository\<T\>** — използва `IDatabaseContextFactory` за създаване на `VetClinicContext` при всяка операция. Предоставя `GetAll()`, `GetById()`, `Add()`, `Update()`, `Delete()` и `Query()` (с Expression предикат).

**SearchFilterViewModel** — приема `Type` и `object` repository чрез конструктора. Чрез reflection зарежда `[Searchable]` свойства и изгражда динамични LINQ заявки при търсене.

**GenericListViewModel** — приема `Type` и зарежда `[Displayable]` свойства като колони. Управлява `Items`, `SelectedItem` (пълен обект) и поддържа динамична смяна на колони.

---

## 6. Организация на преизползване на ViewModel

### Подход

Ключовото изискване е ViewModel класовете да се **преизползват** между WPF и AvaloniaUI, без да се създават отделни версии.

Това постигнахме чрез:

1. **Отделен проект `VetClinic.ViewModels`** — таргетира `net10.0` (не `net10.0-windows`). Няма никаква зависимост от WPF или Avalonia.

2. **Само стандартни .NET интерфейси:**
   - `INotifyPropertyChanged` от `System.ComponentModel`
   - `ICommand` от `System.Windows.Input` (част от .NET runtime, не е WPF-специфичен)
   - `ObservableCollection<T>` от `System.Collections.ObjectModel`

3. **И WPF, и Avalonia проектите референцират `VetClinic.ViewModels`** — същата компилирана DLL се зарежда от двете приложения.

### Пример за преизползване

`DogListViewModel` се инстанцира по идентичен начин и в двата UI проекта:

**WPF (`DogListWindow.xaml.cs`):**
```csharp
CrudVm = new DogListViewModel(mainVm.DogRepository);
```

**Avalonia (`DogListWindow.axaml.cs`):**
```csharp
CrudVm = new DogListViewModel(mainVm.DogRepository);
```

Кодът е **идентичен**. Разликата е само в XAML файловете (`.xaml` срещу `.axaml`), но binding синтаксисът е еднакъв: `{Binding CrudVm.Name}`.

---

## 7. Организация на преизползване на бази данни

### Подход

Бизнес логиката трябва да работи с двете бази без дублиране. Постигнахме това чрез **Factory Pattern + Generic Repository**:

```
IDatabaseContextFactory (интерфейс)
   ├── SqliteContextFactory     → създава SqliteVetContext
   └── SqlServerContextFactory  → създава SqlServerVetContext

VetClinicContext (абстрактен)
   ├── SqliteVetContext          → UseSqlite(...)
   └── SqlServerVetContext       → UseSqlServer(...)

GenericRepository<T> приема IDatabaseContextFactory
   → не знае коя база се използва
```

### Преминаване между бази в реално време

`MainViewModel` съдържа `SwitchDatabaseCommand`, който:
1. Създава нова фабрика (`SqliteContextFactory` или `SqlServerContextFactory`)
2. Пресъздава всички repository-та с новата фабрика
3. Уведомява UI-а чрез `OnPropertyChanged`

```csharp
private void SwitchDatabase(string? dbType)
{
    _contextFactory = dbType == "SQLite"
        ? new SqliteContextFactory()
        : new SqlServerContextFactory();

    DogRepository = new GenericRepository<Dog>(_contextFactory);
    OwnerRepository = new GenericRepository<Owner>(_contextFactory);
    // ... останалите repository-та
}
```

Нито един ViewModel освен `MainViewModel` не знае за конкретната база данни.

---

## 8. Организация на преизползване за допълнителни точки

### 8.1. Генеричен филтър за търсене

**Архитектура на три нива:**

**Ниво 1 — Атрибути (Model):** Свойствата на ентитетите се маркират с `[Searchable("Етикет")]`:
```csharp
public class Dog
{
    [Searchable("Name")]
    public string Name { get; set; }

    [Searchable("Breed")]
    public string Breed { get; set; }
}
```

**Ниво 2 — ViewModel (генеричен):** `SearchFilterViewModel` приема `Type` (не конкретен клас) и чрез `ReflectionHelper.GetSearchableFields()` зарежда полетата. При търсене изгражда `Expression<Func<T, bool>>` динамично.

**Ниво 3 — View (генеричен контрол):** `SearchFilterControl` визуализира `FilterFields` колекцията. Контролът **не знае** нищо за `Dog`, `Owner` или `Appointment` — работи изцяло с `FilterFieldInfo` метаданни.

**Демонстрация за 4 ентитета:**
- `DogSearchWindow` → `SearchFilterViewModel(typeof(Dog), dogRepo)`
- `OwnerSearchWindow` → `SearchFilterViewModel(typeof(Owner), ownerRepo)`
- `AppointmentSearchWindow` → `SearchFilterViewModel(typeof(Appointment), appointmentRepo)`
- `MedicalRecordSearchWindow` → `SearchFilterViewModel(typeof(MedicalRecord), recordRepo)`

Същият контрол, различни ентитети — **пълна преизползваемост**.

### 8.2. Генеричен списък с избор 

**Архитектура на три нива:**

**Ниво 1 — Атрибути (Model):** `[Displayable("Заглавие", ред)]` маркира кои свойства да се показват:
```csharp
public class Owner
{
    [Displayable("First Name", 1)]
    public string FirstName { get; set; }

    [Displayable("Last Name", 2)]
    public string LastName { get; set; }
}
```

**Ниво 2 — ViewModel:** `GenericListViewModel` управлява `Columns` (динамично конфигурируеми), `Items` и `SelectedItem` (връща **пълния обект**, не само показваните колони). `SetColumns()` метод позволява смяна на колоните в реално време.

**Ниво 3 — View:** `GenericListControl` програматично изгражда `DataGrid` колони от `ColumnDefinition` обекти. При `CollectionChanged` на `Columns` — пресъздава колоните.

**Демонстрация за 5 ентитета:**
- `DogListWindow`, `OwnerListWindow`, `VetListWindow`
- `AppointmentListWindow`, `MedicalRecordWindow`

---

## 9. Ключови кодови фрагменти

### 9.1. ViewModelBase — базов клас за MVVM

```csharp
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetProperty<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
```

Използва `[CallerMemberName]` за автоматично определяне на името на свойството — елиминира magic strings.

### 9.2. Динамично изграждане на LINQ Expression Tree

```csharp
if (field.PropertyType == typeof(string))
{
    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
    var constant = Expression.Constant(valueStr);
    condition = Expression.Call(property, containsMethod, constant);
}
else if (field.PropertyType.IsEnum)
{
    if (Enum.TryParse(field.PropertyType, valueStr, true, out var enumValue))
    {
        var constant = Expression.Constant(enumValue);
        condition = Expression.Equal(property, constant);
    }
}
```

Този код генерира различен LINQ предикат в зависимост от типа на полето — без да знае конкретния ентитет.

### 9.3. ReflectionHelper — извличане на метаданни

```csharp
public static List<FilterFieldInfo> GetSearchableFields(Type entityType)
{
    var fields = new List<FilterFieldInfo>();
    foreach (var prop in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
    {
        var attr = prop.GetCustomAttribute<SearchableAttribute>();
        if (attr != null)
        {
            fields.Add(new FilterFieldInfo
            {
                PropertyName = prop.Name,
                DisplayLabel = attr.DisplayName,
                PropertyType = prop.PropertyType
            });
        }
    }
    return fields;
}
```

Единна точка за извличане на метаданни — ViewModel-ите не правят reflection директно.

### 9.4. Абстрактен DbContext с наследяване

```csharp
public abstract class VetClinicContext : DbContext
{
    public DbSet<Dog> Dogs { get; set; }
    public DbSet<Owner> Owners { get; set; }
    // ...
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relationship-и и seed data — общи за двете бази
    }
}

public class SqliteVetContext : VetClinicContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={databasePath}");
    }
}
```

Relationship-ите и seed data са дефинирани **веднъж** в абстрактния клас. Конкретните реализации само конфигурират connection string-а.

---

## 10. Примери за използване на функционалността

### 10.1. Главен прозорец с навигация

[Screenshot: Главен прозорец на WPF приложението с навигационно меню и DB switcher]

[Screenshot: Главен прозорец на Avalonia приложението — идентичен интерфейс]

### 10.2. Списък с кучета (GenericListControl)

[Screenshot: DogListWindow с динамични колони (Name, Breed, Weight) и форма за CRUD]

### 10.3. Търсене на собственици (SearchFilterControl)

[Screenshot: OwnerSearchWindow с генерирани филтри (First Name, Last Name, Phone, Email) и резултати]

### 10.4. Насрочване на преглед

[Screenshot: AppointmentSchedulerWindow с избор на куче, ветеринар, дата и причина]

### 10.5. История на прегледи на куче

[Screenshot: DogHistoryWindow — избрано куче с таблици за прегледи и медицински записи]

### 10.6. Превключване между бази данни

[Screenshot: Преди и след превключване от SQLite на SQL Server — данните се зареждат от друга база]

### 10.7. Търсене на прегледи по статус (Enum филтър)

[Screenshot: AppointmentSearchWindow с ComboBox филтър за статус (Scheduled/Completed/Cancelled)]

---

## 11. Заключение

Проектът VetClinic демонстрира успешна реализация на MVVM архитектура с пълно разделение на отговорностите. Ключовите постижения са:

1. **Истинско преизползване на ViewModel** — един и същ компилиран DLL се зарежда от WPF и Avalonia без промени.

2. **Абстрахиран достъп до данни** — Factory + Repository шаблонът позволява превключване между SQLite и SQL Server с един клик, без промяна на бизнес логиката.

3. **Генерични UI компоненти** — `SearchFilterControl` и `GenericListControl` работят с произволни ентитети чрез reflection и custom атрибути. Демонстрирани за 4-5 различни таблици.

---

## 12. Използвана литература и източници

1. Microsoft, "Entity Framework Core Documentation", https://learn.microsoft.com/en-us/ef/core/ — основен източник за работа с EF Core, DbContext, миграции и provider модел.

2. Microsoft, "Windows Presentation Foundation (WPF) Documentation", https://learn.microsoft.com/en-us/dotnet/desktop/wpf/ — документация за WPF data binding, XAML, DataTemplateSelector, INotifyPropertyChanged.

3. AvaloniaUI, "Avalonia Documentation", https://docs.avaloniaui.net/ — документация за AvaloniaUI, разлики с WPF, DataGrid, IDataTemplate.

4. Microsoft, "MVVM Pattern", https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm — обяснение на Model-View-ViewModel архитектурния шаблон.

5. Microsoft, "INotifyPropertyChanged Interface", https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged — документация за интерфейса, използван за data binding нотификации.

6. Microsoft, "Expression Trees (C#)", https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/expression-trees/ — документация за Expression API, използвана за динамичните LINQ заявки в SearchFilterViewModel.

7. Microsoft, "SQLite EF Core Database Provider", https://learn.microsoft.com/en-us/ef/core/providers/sqlite/ — настройка и особености на SQLite provider за EF Core.

8. Microsoft, "SQL Server LocalDB", https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb — документация за LocalDB инсталация и употреба.

9. Anthropic, "Claude AI Assistant" — използван за консултации по архитектурни решения, генериране на код и решаване на технически проблеми. Claude предостави съвети за: Factory Pattern за абстрахиране на DbContext, Expression Trees за динамични LINQ заявки, организация на multi-project solution с MVVM.

10. NuGet Gallery, https://www.nuget.org/ — използван за намиране на версии на пакети (EF Core 10.0.3, Avalonia 11.2.7).
