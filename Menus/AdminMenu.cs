using System;
using System.Collections.Generic;
using System.Linq;
using DotnetHospital.Entities;
using DotnetHospital.Services;

namespace DotnetHospital.Menus
{
    /// <summary>
    /// Static class for admin menu operations
    /// Handles all administrator-specific functionality and user interface
    /// </summary>
    public static class AdminMenu
    {
        /// <summary>
        /// Displays and handles the main administrator menu
        /// </summary>
        /// <param name="currentAdmin">Currently logged in administrator</param>
        /// <param name="patients">List of all patients</param>
        /// <param name="doctors">List of all doctors</param>
        /// <param name="appointments">List of all appointments</param>
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
                
                ConsoleUI.DrawMenu("Please choose an option:", options, ConsoleColor.Green, includeZeroExit: false);
                
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
                int nameWidth = 20;
                int emailWidth = 28;
                int phoneWidth = 15;
                int addressWidth = ConsoleUI.ComputeFlexibleWidth(nameWidth + emailWidth + phoneWidth, 3, 20, 60);

                string header = $"{"Name".PadRight(nameWidth)} | {"Email Address".PadRight(emailWidth)} | {"Phone".PadRight(phoneWidth)} | {"Address".PadRight(addressWidth)}";
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));

                foreach (var doctor in doctors)
                {
                    var addr = ConsoleUI.Truncate(doctor.GetFormattedAddress(), addressWidth);
                    Console.WriteLine($"{doctor.DisplayName.PadRight(nameWidth)} | {doctor.Email.PadRight(emailWidth)} | {doctor.Phone.PadRight(phoneWidth)} | {addr}");
                }
            }
            
            ConsoleUI.Pause();
        }
        
        private static void CheckDoctorDetails(List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Doctor Details");
            Console.WriteLine();
            
            while (true)
            {
                Console.Write("Please enter the ID of the doctor whose details you are checking ");
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("('b' to go back)"); Console.ResetColor(); Console.Write(": ");
                var input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) && input.Trim().Equals("b", StringComparison.OrdinalIgnoreCase)) return;
                if (string.IsNullOrWhiteSpace(input)) { ConsoleUI.Error("Doctor ID cannot be empty."); continue; }
                if (!int.TryParse(input, out int doctorId)) { ConsoleUI.Error("Invalid doctor ID format."); continue; }

                var doctor = doctors.FirstOrDefault(d => d.Id == doctorId);
                if (doctor == null) { ConsoleUI.Error($"Doctor with ID {doctorId} not found."); continue; }

                ConsoleUI.Log($"Details for {doctor.DisplayName}", ConsoleColor.Green);
                Console.WriteLine();

                int nameWidth = 20;
                int emailWidth = 28;
                int phoneWidth = 15;
                int addressWidth = ConsoleUI.ComputeFlexibleWidth(nameWidth + emailWidth + phoneWidth, 3, 20, 60);
                string header = $"{"Name".PadRight(nameWidth)} | {"Email Address".PadRight(emailWidth)} | {"Phone".PadRight(phoneWidth)} | {"Address".PadRight(addressWidth)}";
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));
                var addr = ConsoleUI.Truncate(doctor.GetFormattedAddress(), addressWidth);
                Console.WriteLine($"{doctor.DisplayName.PadRight(nameWidth)} | {doctor.Email.PadRight(emailWidth)} | {doctor.Phone.PadRight(phoneWidth)} | {addr}");
                ConsoleUI.Pause();
                return;
            }
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
                int patientWidth = 20;
                int doctorWidth = 20;
                int emailWidth = 28;
                int phoneWidth = 15;
                int addressWidth = ConsoleUI.ComputeFlexibleWidth(patientWidth + doctorWidth + emailWidth + phoneWidth, 4, 20, 60);

                string header = $"{"Patient".PadRight(patientWidth)} | {"Doctor".PadRight(doctorWidth)} | {"Email Address".PadRight(emailWidth)} | {"Phone".PadRight(phoneWidth)} | {"Address".PadRight(addressWidth)}";
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));
                
                foreach (var patient in patients)
                {
                    var doctor = doctors.FirstOrDefault(d => d.Id == patient.DoctorId);
                    var doctorName = doctor?.DisplayName ?? "Not assigned";
                    var addr = ConsoleUI.Truncate(patient.GetFormattedAddress(), addressWidth);
                    Console.WriteLine($"{patient.Name.PadRight(patientWidth)} | {doctorName.PadRight(doctorWidth)} | {patient.Email.PadRight(emailWidth)} | {patient.Phone.PadRight(phoneWidth)} | {addr}");
                }
            }
            
            ConsoleUI.Pause();
        }
        
        private static void CheckPatientDetails(List<Patient> patients, List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Patient Details");
            Console.WriteLine();
            
            while (true)
            {
                Console.Write("Please enter the ID of the patient whose details you are checking ");
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("('b' to go back)"); Console.ResetColor(); Console.Write(": ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && input.Trim().Equals("b", StringComparison.OrdinalIgnoreCase)) return;
                if (string.IsNullOrWhiteSpace(input)) { ConsoleUI.Error("Patient ID cannot be empty."); continue; }
                if (!int.TryParse(input, out int patientId)) { ConsoleUI.Error("Invalid patient ID format."); continue; }

                var patient = patients.FirstOrDefault(p => p.Id == patientId);
                if (patient == null) { ConsoleUI.Error($"Patient with ID {patientId} not found."); continue; }

                var doctor = doctors.FirstOrDefault(d => d.Id == patient.DoctorId);
                var doctorName = doctor?.DisplayName ?? "Not assigned";
                
                ConsoleUI.Log($"Details for {patient.Name}", ConsoleColor.Green);
                Console.WriteLine();
                int patientWidth2 = 20;
                int doctorWidth2 = 20;
                int emailWidth2 = 28;
                int phoneWidth2 = 15;
                int addressWidth2 = ConsoleUI.ComputeFlexibleWidth(patientWidth2 + doctorWidth2 + emailWidth2 + phoneWidth2, 4, 20, 60);
                string header2 = $"{"Patient".PadRight(patientWidth2)} | {"Doctor".PadRight(doctorWidth2)} | {"Email Address".PadRight(emailWidth2)} | {"Phone".PadRight(phoneWidth2)} | {"Address".PadRight(addressWidth2)}";
                Console.WriteLine(header2);
                Console.WriteLine(new string('-', header2.Length));
                var addr2 = ConsoleUI.Truncate(patient.GetFormattedAddress(), addressWidth2);
                Console.WriteLine($"{patient.Name.PadRight(patientWidth2)} | {doctorName.PadRight(doctorWidth2)} | {patient.Email.PadRight(emailWidth2)} | {patient.Phone.PadRight(phoneWidth2)} | {addr2}");
                ConsoleUI.Pause();
                return;
            }
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
                var newDoctor = Doctor.CreateNew(fullName, specialty, email, phone, streetNumber, street, city, state, doctors);
                
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
                var newPatient = Patient.CreateNew(fullName, age, gender, null, email, phone, streetNumber, street, city, state, patients);
                
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
