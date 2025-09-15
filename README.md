# DOTNET Hospital Management System

A console-based hospital management system built with C# .NET Framework.

## 🚀 How to Run

### Prerequisites
- .NET Framework 4.7.2 or later
- Visual Studio 2019/2022

### Quick Start
1. Open `DotnetHospital.sln` in Visual Studio
2. Build the project (`Ctrl+Shift+B`)
3. Run the application (`F5` or `Ctrl+F5`)

### Login Credentials
Check the `Data/` folder for sample users:
- **Patients**: ID 10001, 10002, etc.
- **Doctors**: ID 20001, 20002, etc.
- **Admins**: ID 90001, 90002, etc.

## ✨ Features

### Patient
- View personal details
- Check assigned doctor
- View appointments
- Book new appointments

### Doctor
- View patient list
- Check appointment schedule
- Review patient details

### Admin
- List all doctors/patients
- Add new doctors/patients
- View detailed information

## 🛠️ Technical Features

- **OOP**: Inheritance, Polymorphism, Encapsulation
- **Anonymous Methods**: User authentication and data filtering
- **Extension Methods**: String utilities
- **File I/O**: CSV-based data persistence
- **Console UI**: Professional headers and menus

## 📁 Project Structure

```
DotnetHospital/
├── Entities/          # Data models (User, Patient, Doctor, Admin, Appointment)
├── Services/          # Business logic (ConsoleUI, FileManager, IdGenerator)
├── Menus/             # User interfaces (PatientMenu, DoctorMenu, AdminMenu)
├── Extensions/        # Utility extensions
├── Data/              # Data storage (CSV files)
└── Program.cs         # Application entry point
```

**DOTNET Hospital Management System** - Built with C# .NET Framework