using System;
using System.Collections.Generic;
using System.Linq;
using DotnetHospital.Entities;
using DotnetHospital.Services;

namespace DotnetHospital.Menus
{
    public static class AdminMenu
    {
        public static void ShowMenu(Admin currentAdmin, List<Patient> patients, 
                                  List<Doctor> doctors, List<Appointment> appointments)
        {
            while (true)
            {
                ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Administrator Menu");
                ConsoleUI.Log($"Welcome to DOTNET Hospital Management System {currentAdmin.Name}", ConsoleColor.Green);
                Console.WriteLine();
                
                string[] options = {
                    "List all doctors",
                    "Check doctor details",
                    "List all patients",
                    "Check patient details", 
                    "Add doctor",
                    "Add patient",
                    "Logout",
                    "Exit"
                };
                
                ConsoleUI.DrawMenu("Please choose an option:", options);
                
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ShowAllDoctors(doctors);
                        break;
                    case "2":
                        CheckDoctorDetails(doctors);
                        break;
                    case "3":
                        ShowAllPatients(patients, doctors);
                        break;
                    case "4":
                        CheckPatientDetails(patients, doctors);
                        break;
                    case "5":
                        AddDoctor(doctors);
                        break;
                    case "6":
                        AddPatient(patients);
                        break;
                    case "7":
                        return; // Return to login
                    case "0":
                    case "8":
                        Environment.Exit(0);
                        break;
                    default:
                        ConsoleUI.Error("Invalid choice. Please try again.");
                        ConsoleUI.Pause();
                        break;
                }
            }
        }
        
        private static void ShowAllDoctors(List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "All Doctors");
            ConsoleUI.Log("All doctors registered to the DOTNET Hospital Management System", ConsoleColor.Green);
            Console.WriteLine();
            
            if (doctors.Count == 0)
            {
                ConsoleUI.Warning("No doctors found.");
            }
            else
            {
                Console.WriteLine("Name             | Email Address      | Phone        | Address");
                Console.WriteLine("-------------------------------------------------------------------");
                
                foreach (var doctor in doctors)
                {
                    Console.WriteLine($"{doctor.Name,-15} | {doctor.Email,-18} | {doctor.Phone,-11} | {doctor.GetFormattedAddress()}");
                }
            }
            
            ConsoleUI.Pause();
        }
        
        private static void CheckDoctorDetails(List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Doctor Details");
            Console.WriteLine();
            
            Console.Write("Please enter the ID of the doctor who's details you are checking. Or press n to return to menu: ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input) || input.ToLower() == "n")
            {
                return;
            }
            
            if (!int.TryParse(input, out int doctorId))
            {
                ConsoleUI.Error("Invalid doctor ID format.");
                ConsoleUI.Pause();
                return;
            }
            
            var doctor = doctors.FirstOrDefault(d => d.Id == doctorId);
            
            if (doctor == null)
            {
                ConsoleUI.Error($"Doctor with ID {doctorId} not found.");
                ConsoleUI.Pause();
                return;
            }
            
            ConsoleUI.Log($"Details for {doctor.Name}", ConsoleColor.Green);
            Console.WriteLine();
            
            Console.WriteLine("Name             | Email Address      | Phone        | Address");
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine($"{doctor.Name,-15} | {doctor.Email,-18} | {doctor.Phone,-11} | {doctor.GetFormattedAddress()}");
            
            ConsoleUI.Pause();
        }
        
        private static void ShowAllPatients(List<Patient> patients, List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "All Patients");
            ConsoleUI.Log("All patients registered to the DOTNET Hospital Management System", ConsoleColor.Green);
            Console.WriteLine();
            
            if (patients.Count == 0)
            {
                ConsoleUI.Warning("No patients found.");
            }
            else
            {
                Console.WriteLine("Patient          | Doctor         | Email Address      | Phone        | Address");
                Console.WriteLine("-------------------------------------------------------------------");
                
                foreach (var patient in patients)
                {
                    var doctor = doctors.FirstOrDefault(d => d.Id == patient.DoctorId);
                    var doctorName = doctor?.Name ?? "Not assigned";
                    Console.WriteLine($"{patient.Name,-15} | {doctorName,-13} | {patient.Email,-18} | {patient.Phone,-11} | {patient.GetFormattedAddress()}");
                }
            }
            
            ConsoleUI.Pause();
        }
        
        private static void CheckPatientDetails(List<Patient> patients, List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Patient Details");
            Console.WriteLine();
            
            Console.Write("Please enter the ID of the patient who's details you are checking. Or press n to return to menu: ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input) || input.ToLower() == "n")
            {
                return;
            }
            
            if (!int.TryParse(input, out int patientId))
            {
                ConsoleUI.Error("Invalid patient ID format.");
                ConsoleUI.Pause();
                return;
            }
            
            var patient = patients.FirstOrDefault(p => p.Id == patientId);
            
            if (patient == null)
            {
                ConsoleUI.Error($"Patient with ID {patientId} not found.");
                ConsoleUI.Pause();
                return;
            }
            
            var doctor = doctors.FirstOrDefault(d => d.Id == patient.DoctorId);
            var doctorName = doctor?.Name ?? "Not assigned";
            
            ConsoleUI.Log($"Details for {patient.Name}", ConsoleColor.Green);
            Console.WriteLine();
            
            Console.WriteLine("Patient          | Doctor         | Email Address      | Phone        | Address");
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine($"{patient.Name,-15} | {doctorName,-13} | {patient.Email,-18} | {patient.Phone,-11} | {patient.GetFormattedAddress()}");
            
            ConsoleUI.Pause();
        }
        
        private static void AddDoctor(List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Add Doctor");
            ConsoleUI.Log("Registering a new doctor with the DOTNET Hospital Management System", ConsoleColor.Green);
            Console.WriteLine();
            
            try
            {
                Console.Write("First Name: ");
                var firstName = Console.ReadLine();
                
                Console.Write("Last Name: ");
                var lastName = Console.ReadLine();
                
                Console.Write("Email: ");
                var email = Console.ReadLine();
                
                Console.Write("Phone: ");
                var phone = Console.ReadLine();
                
                Console.Write("Street Number: ");
                var streetNumber = Console.ReadLine();
                
                Console.Write("Street: ");
                var street = Console.ReadLine();
                
                Console.Write("City: ");
                var city = Console.ReadLine();
                
                Console.Write("State: ");
                var state = Console.ReadLine();
                
                Console.Write("Specialty: ");
                var specialty = Console.ReadLine();
                
                // Validate required fields
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || 
                    string.IsNullOrWhiteSpace(specialty))
                {
                    ConsoleUI.Error("First name, last name, and specialty are required.");
                    ConsoleUI.Pause();
                    return;
                }
                
                var fullName = $"{firstName} {lastName}";
                var newDoctor = new Doctor(fullName, "temp123", specialty, email, phone, streetNumber, street, city, state); // Default password
                
                doctors.Add(newDoctor);
                
                // Save to file
                var data = FileManager.LoadAll();
                FileManager.SaveAll(data.Item1, doctors, data.Item3, data.Item4);
                
                ConsoleUI.Log($"{fullName} added to the system!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ConsoleUI.Error($"Error adding doctor: {ex.Message}");
            }
            
            ConsoleUI.Pause();
        }
        
        private static void AddPatient(List<Patient> patients)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Add Patient");
            ConsoleUI.Log("Registering a new patient with the DOTNET Hospital Management System", ConsoleColor.Green);
            Console.WriteLine();
            
            try
            {
                Console.Write("First Name: ");
                var firstName = Console.ReadLine();
                
                Console.Write("Last Name: ");
                var lastName = Console.ReadLine();
                
                Console.Write("Email: ");
                var email = Console.ReadLine();
                
                Console.Write("Phone: ");
                var phone = Console.ReadLine();
                
                Console.Write("Street Number: ");
                var streetNumber = Console.ReadLine();
                
                Console.Write("Street: ");
                var street = Console.ReadLine();
                
                Console.Write("City: ");
                var city = Console.ReadLine();
                
                Console.Write("State: ");
                var state = Console.ReadLine();
                
                Console.Write("Age: ");
                var ageInput = Console.ReadLine();
                
                Console.Write("Gender (M/F): ");
                var gender = Console.ReadLine();
                
                // Validate required fields
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || 
                    string.IsNullOrWhiteSpace(ageInput) || string.IsNullOrWhiteSpace(gender))
                {
                    ConsoleUI.Error("First name, last name, age, and gender are required.");
                    ConsoleUI.Pause();
                    return;
                }
                
                if (!int.TryParse(ageInput, out int age))
                {
                    ConsoleUI.Error("Invalid age format.");
                    ConsoleUI.Pause();
                    return;
                }
                
                var fullName = $"{firstName} {lastName}";
                var newPatient = new Patient(fullName, "temp123", age, gender, null, email, phone, streetNumber, street, city, state); // Default password
                
                patients.Add(newPatient);
                
                // Save to file
                var data = FileManager.LoadAll();
                FileManager.SaveAll(patients, data.Item2, data.Item3, data.Item4);
                
                ConsoleUI.Log($"{fullName} added to the system!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ConsoleUI.Error($"Error adding patient: {ex.Message}");
            }
            
            ConsoleUI.Pause();
        }
    }
}
