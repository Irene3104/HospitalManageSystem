using System;
using System.Collections.Generic;
using System.Linq;
using DotnetHospital.Entities;
using DotnetHospital.Services;

namespace DotnetHospital.Menus
{
    /// <summary>
    /// Static class for doctor menu operations
    /// Handles all doctor-specific functionality and user interface
    /// </summary>
    public static class DoctorMenu
    {
        /// <summary>
        /// Displays and handles the main doctor menu
        /// </summary>
        /// <param name="currentDoctor">Currently logged in doctor</param>
        /// <param name="patients">List of all patients</param>
        /// <param name="doctors">List of all doctors</param>
        /// <param name="appointments">List of all appointments</param>
        public static void ShowMenu(Doctor currentDoctor, List<Patient> patients, 
                                  List<Doctor> doctors, List<Appointment> appointments)
        {
            while (true)
            {
                ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Doctor Menu");
                ConsoleUI.Log($"Welcome to DOTNET Hospital Management System {currentDoctor.Name}", ConsoleColor.Green);
                Console.WriteLine();
                
                string[] options = {
                    "List doctor details",
                    "List patients",
                    "List appointments", 
                    "Check particular patient",
                    "List appointments with patient",
                    "Logout",
                    "Exit"
                };
                
                ConsoleUI.DrawMenu("Please choose an option:", options, ConsoleColor.Green, includeZeroExit: false);
                
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ShowDoctorDetails(currentDoctor);
                        break;
                    case "2":
                        ShowPatients(currentDoctor, patients);
                        break;
                    case "3":
                        ShowAppointments(currentDoctor, appointments, patients);
                        break;
                    case "4":
                        CheckParticularPatient(patients, doctors);
                        break;
                    case "5":
                        ShowAppointmentsWithPatient(currentDoctor, appointments, patients);
                        break;
                    case "6":
                        return; // Return to login
                    case "7":
                        Environment.Exit(0);
                        break;
                    default:
                        ConsoleUI.Error("Invalid choice. Please try again.");
                        ConsoleUI.Pause();
                        break;
                }
            }
        }
        
        private static void ShowDoctorDetails(Doctor doctor)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "My Details");
            ConsoleUI.Log($"{doctor.DisplayName}'s Details", ConsoleColor.Green);
            Console.WriteLine();

            ConsoleUI.DisplayDoctorTable(new[] { doctor });
            ConsoleUI.Pause();
        }
        
        private static void ShowPatients(Doctor doctor, List<Patient> patients)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "My Patients");
            ConsoleUI.Log($"Patients assigned to {doctor.DisplayName}:", ConsoleColor.Green);
            Console.WriteLine();
            
            var assignedPatients = patients.Where(p => p.DoctorId == doctor.Id).ToList();
            
            if (assignedPatients.Count == 0)
            {
                ConsoleUI.Warning("No patients assigned to you.");
            }
            else
            {
                ConsoleUI.DisplayPatientDoctorTable(assignedPatients, new[] { doctor });
            }
            
            ConsoleUI.Pause();
        }
        
        private static void ShowAppointments(Doctor doctor, List<Appointment> appointments, List<Patient> patients)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "All Appointments");
            ConsoleUI.Log($"Appointments for {doctor.DisplayName}", ConsoleColor.Green);
            Console.WriteLine();
            
            // Anonymous method to find appointments for this doctor
            var doctorAppointments = appointments.FindAll(delegate(Appointment a) { 
                return a.DoctorId == doctor.Id; 
            });
            
            ConsoleUI.DisplayAppointmentTable(doctorAppointments, new[] { doctor }, patients);
            ConsoleUI.Pause();
        }
        
        private static void CheckParticularPatient(List<Patient> patients, List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Check Patient Details");
            Console.WriteLine();

            while (true)
            {
                int patientId = ConsoleUI.GetValidIdInput("Enter the ID of the patient to check");
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
        
        private static void ShowAppointmentsWithPatient(Doctor doctor, List<Appointment> appointments, List<Patient> patients)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Appointments With");
            Console.WriteLine();
            
            Patient patient;
            while (true)
            {
                int patientId = ConsoleUI.GetValidIdInput("Enter the ID of the patient you would like to view appointments for");
                if (patientId == -1) return; // User chose to go back
                
                patient = patients.FirstOrDefault(p => p.Id == patientId);
                if (patient == null) 
                { 
                    ConsoleUI.Error($"Patient with ID {patientId} not found."); 
                    continue; 
                }
                break;
            }
            
            // Anonymous method to find appointments for specific patient and doctor
            var patientAppointments = appointments.FindAll(delegate(Appointment a) { 
                return a.PatientId == patient.Id && a.DoctorId == doctor.Id; 
            });
            
            if (patientAppointments.Count == 0)
            {
                ConsoleUI.Warning($"No appointments found between {doctor.DisplayName} and {patient.Name}.");
            }
            else
            {
                ConsoleUI.DisplayAppointmentTable(patientAppointments, new[] { doctor }, new[] { patient });
            }
            
            ConsoleUI.Pause();
        }
    }
}
