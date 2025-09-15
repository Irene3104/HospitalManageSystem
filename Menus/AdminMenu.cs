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
            
            ConsoleUI.DisplayDoctorTable(doctors);
            ConsoleUI.Pause();
        }
        
        private static void CheckDoctorDetails(List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Doctor Details");
            Console.WriteLine();
            
            while (true)
            {
                int doctorId = ConsoleUI.GetValidIdInput("Please enter the ID of the doctor whose details you are checking");
                if (doctorId == -1) return; // User chose to go back

                var doctor = doctors.FirstOrDefault(d => d.Id == doctorId);
                if (doctor == null) 
                { 
                    ConsoleUI.Error($"Doctor with ID {doctorId} not found."); 
                    continue; 
                }

                ConsoleUI.Log($"Details for {doctor.DisplayName}", ConsoleColor.Green);
                Console.WriteLine();

                ConsoleUI.DisplayDoctorTable(new[] { doctor });
                ConsoleUI.Pause();
                return;
            }
        }
        
        private static void ShowAllPatients(List<Patient> patients, List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "All Patients");
            ConsoleUI.Log("All patients registered to the DOTNET Hospital Management System", ConsoleColor.Green);
            Console.WriteLine();
            
            ConsoleUI.DisplayPatientDoctorTable(patients, doctors);
            ConsoleUI.Pause();
        }
        
        private static void CheckPatientDetails(List<Patient> patients, List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Patient Details");
            Console.WriteLine();
            
            while (true)
            {
                int patientId = ConsoleUI.GetValidIdInput("Please enter the ID of the patient whose details you are checking");
                if (patientId == -1) return; // User chose to go back

                var patient = patients.FirstOrDefault(p => p.Id == patientId);
                if (patient == null) 
                { 
                    ConsoleUI.Error($"Patient with ID {patientId} not found."); 
                    continue; 
                }

                ConsoleUI.Log($"Details for {patient.Name}", ConsoleColor.Green);
                Console.WriteLine();
                
                ConsoleUI.DisplayPatientDoctorTable(new[] { patient }, doctors);
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
                // Get required information
                var firstName = ConsoleUI.GetRequiredTextInput("First Name: ", "First name");
                var lastName = ConsoleUI.GetRequiredTextInput("Last Name: ", "Last name");
                var specialty = ConsoleUI.GetRequiredTextInput("Specialty: ", "Specialty");
                
                // Get optional information
                var email = ConsoleUI.GetOptionalTextInput("Email: ");
                var phone = ConsoleUI.GetOptionalTextInput("Phone: ");
                var streetNumber = ConsoleUI.GetOptionalTextInput("Street Number: ");
                var street = ConsoleUI.GetOptionalTextInput("Street: ");
                var city = ConsoleUI.GetOptionalTextInput("City: ");
                var state = ConsoleUI.GetOptionalTextInput("State: ");
                
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
                // Get required information
                var firstName = ConsoleUI.GetRequiredTextInput("First Name: ", "First name");
                var lastName = ConsoleUI.GetRequiredTextInput("Last Name: ", "Last name");
                var gender = ConsoleUI.GetRequiredTextInput("Gender (M/F): ", "Gender");
                
                // Get and validate age
                int age;
                while (true)
                {
                    var ageInput = ConsoleUI.GetRequiredTextInput("Age: ", "Age");
                    if (int.TryParse(ageInput, out age))
                    {
                        break;
                    }
                    ConsoleUI.Error("Invalid age format. Please enter a number.");
                }
                
                // Get optional information
                var email = ConsoleUI.GetOptionalTextInput("Email: ");
                var phone = ConsoleUI.GetOptionalTextInput("Phone: ");
                var streetNumber = ConsoleUI.GetOptionalTextInput("Street Number: ");
                var street = ConsoleUI.GetOptionalTextInput("Street: ");
                var city = ConsoleUI.GetOptionalTextInput("City: ");
                var state = ConsoleUI.GetOptionalTextInput("State: ");
                
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
