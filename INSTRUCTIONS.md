# VetClinic — Setup & Run Instructions

## Prerequisites

### 1. Install .NET 10.0 SDK
- Download from https://dotnet.microsoft.com/download/dotnet/10.0
- Install the **SDK** (not just Runtime)
- Verify: open PowerShell and run `dotnet --version` — should show `10.0.x`

### 2. Install SQL Server LocalDB
- Open **Visual Studio Installer**
- Click **Modify** on your VS installation
- Go to **Individual Components** tab
- Search for **SQL Server Express LocalDB** and check it
- Click **Modify** to install
- Alternatively if you don't have VS: download "SQL Server Express" from Microsoft and select LocalDB during install

### 3. Install Visual Studio 2022 (if not already)
- Any edition (Community is free)
- Workloads needed: **.NET desktop development** (includes WPF)

---

## Clone and Build

### 4. Clone the repo
```powershell
cd C:\Users\YourName\Desktop
git clone https://github.com/mrtnnikiforov/PS-VetClinic.git
cd PS-VetClinic
```

### 5. Restore NuGet packages
```powershell
dotnet restore VetClinic.sln
```

### 6. Build the solution
```powershell
dotnet build VetClinic.sln
```
---

## Run the WPF App

### 7. Launch WPF
```powershell
dotnet run --project VetClinic.WPF
```

**What you should see:**
- Main window with dark header, "VetClinic", database switcher (ComboBox), and left navigation menu
- Click each button to open windows:
  - **Dogs** — list + CRUD form
  - **Owners** — list + CRUD form
  - **Veterinarians** — list + CRUD form
  - **Search Dogs** — generic search filter with text fields
  - **Search Owners** — same generic control, different entity
  - **Appointments** — list with Complete/Cancel/Delete buttons
  - **Schedule Appointment** — form with dog/vet dropdowns and date picker
  - **Medical Records** — list + CRUD form
  - **Dog History** — select a dog, see appointments + records
  - **Search Appointments** — generic search with enum dropdown for status
  - **Search Records** — generic search for medical records
- Switch the database ComboBox from **SQLite** to **SQL Server** — data reloads from the other DB

---

## Run the Avalonia App

### 8. Launch Avalonia
```powershell
dotnet run --project VetClinic.Avalonia
```

**What you should see:** The exact same UI and functionality as WPF, but running on the Avalonia framework. Same navigation, same windows, same data.

---

## Test Key Features

### 9. Test database switching
1. Open WPF app
2. Click **Dogs** — see the seeded data (Rex, Bella, Charlie, Luna)
3. Switch ComboBox to **SQL Server**
4. Click **Dogs** again — same seed data but from SQL Server LocalDB
5. Add a new dog in SQL Server
6. Switch back to SQLite — the new dog is NOT there

### 10. Test generic search filter
1. Click **Search Dogs**
2. Type "Rex" in the Name field → click Search → only Rex appears
3. Type "German" in the Breed field → click Search → filtered results
4. Click Clear → filters reset

### 11. Test generic list display
1. Click **Dogs** — see columns: Name, Breed, Date of Birth, Weight, Chip Number
2. Click a row — form on the right fills with that dog's data

### 12. Test appointment scheduling
1. Click **Schedule Appointment**
2. Select a dog (e.g., Rex), a vet, pick a date, type a reason
3. Click **Schedule Appointment**
4. Green status message appears
5. Click **Appointments** — new appointment shows in the list

### 13. Test dog history
1. Click **Dog History**
2. Select "Rex" from the dropdown
3. See all Rex's appointments in the top table
4. See all medical records in the bottom table

---

## Take Screenshots for Documentation

Take screenshots of each window for the `[Screenshot: ...]` placeholders in `Documentation.md`:

1. Main window (WPF)
2. Main window (Avalonia) — shows same UI, different framework
3. Dog list with data
4. Owner search with filters filled and results
5. Appointment scheduler with filled form
6. Dog history with selected dog
7. Database switcher before/after switching
8. Appointment search with enum ComboBox filter

---

## Convert Documentation to Word/PDF

### 14. Convert Markdown to Word

**Option A — Pandoc** (if installed):
```powershell
pandoc Documentation.md -o Documentation.docx
```

**Option B — Manual:**
1. Open `Documentation.md` in VS Code
2. Preview it (Ctrl+Shift+V)
3. Select all, copy, paste into Word
4. Fix formatting as needed
