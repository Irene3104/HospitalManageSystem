using System;
using System.Collections.Generic;
using System.Linq;
using DotnetHospital.Entities;
using DotnetHospital.Services;

namespace DotnetHospital.Menus
{
    public static class DoctorMenu
    {
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

            int nameWidth = 20;
            int emailWidth = 28;
            int phoneWidth = 15;
            int addressWidth = ConsoleUI.ComputeFlexibleWidth(nameWidth + emailWidth + phoneWidth, 3, 20, 60);
            string header = $"{"Name".PadRight(nameWidth)} | {"Email Address".PadRight(emailWidth)} | {"Phone".PadRight(phoneWidth)} | {"Address".PadRight(addressWidth)}";
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));
            var addr = ConsoleUI.Truncate(doctor.GetFormattedAddress(), addressWidth);
            Console.WriteLine($"{doctor.Name.PadRight(nameWidth)} | {doctor.Email.PadRight(emailWidth)} | {doctor.Phone.PadRight(phoneWidth)} | {addr}");

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
                int patientWidth = 20;
                int doctorWidth = 20;
                int emailWidth = 28;
                int phoneWidth = 15;
                int addressWidth = ConsoleUI.ComputeFlexibleWidth(patientWidth + doctorWidth + emailWidth + phoneWidth, 5 - 1, 20, 60);
                string header = $"{"Patient".PadRight(patientWidth)} | {"Doctor".PadRight(doctorWidth)} | {"Email Address".PadRight(emailWidth)} | {"Phone".PadRight(phoneWidth)} | {"Address".PadRight(addressWidth)}";
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));
                
                foreach (var patient in assignedPatients)
                {
                    var addr = ConsoleUI.Truncate(patient.GetFormattedAddress(), addressWidth);
                    Console.WriteLine($"{patient.Name.PadRight(patientWidth)} | {doctor.DisplayName.PadRight(doctorWidth)} | {patient.Email.PadRight(emailWidth)} | {patient.Phone.PadRight(phoneWidth)} | {addr}");
                }
            }
            
            ConsoleUI.Pause();
        }
        
        private static void ShowAppointments(Doctor doctor, List<Appointment> appointments, List<Patient> patients)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "All Appointments");
            ConsoleUI.Log($"Appointments for {doctor.DisplayName}", ConsoleColor.Green);
            Console.WriteLine();
            
            var doctorAppointments = appointments.Where(a => a.DoctorId == doctor.Id).ToList();
            
            if (doctorAppointments.Count == 0)
            {
                ConsoleUI.Warning("No appointments found.");
            }
            else
            {
                int doctorWidth = 20;
                int patientWidth = 20;
                int descWidth = ConsoleUI.ComputeFlexibleWidth(doctorWidth + patientWidth, 3 - 1, 15, 60);
                string header = $"{"Doctor".PadRight(doctorWidth)} | {"Patient".PadRight(patientWidth)} | {"Description".PadRight(descWidth)}";
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));
                
                foreach (var appointment in doctorAppointments)
                {
                    var patient = patients.FirstOrDefault(p => p.Id == appointment.PatientId);
                    var patientName = patient?.Name ?? "Unknown Patient";
                    var note = ConsoleUI.Truncate(appointment.Note ?? string.Empty, descWidth);
                    Console.WriteLine($"{doctor.DisplayName.PadRight(doctorWidth)} | {patientName.PadRight(patientWidth)} | {note}");
                }
            }
            
            ConsoleUI.Pause();
        }
        
        private static void CheckParticularPatient(List<Patient> patients, List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Check Patient Details");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Enter the ID of the patient to check ");
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("('b' to go back)"); Console.ResetColor(); Console.Write(": ");
                var input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) && input.Trim().Equals("b", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleUI.Error("Patient ID cannot be empty.");
                    continue;
                }
                if (!int.TryParse(input, out int patientId))
                {
                    ConsoleUI.Error("Invalid patient ID format.");
                    continue;
                }

                var patient = patients.FirstOrDefault(p => p.Id == patientId);
                if (patient == null)
                {
                    ConsoleUI.Error($"Patient with ID {patientId} not found.");
                    continue;
                }

                ConsoleUI.Log($"Details for {patient.Name}", ConsoleColor.Green);
                Console.WriteLine();

                var doctor = doctors.FirstOrDefault(d => d.Id == patient.DoctorId);
                var doctorName = doctor?.Name ?? "Not assigned";

                int patientWidth2 = 20;
                int doctorWidth2 = 20;
                int emailWidth2 = 28;
                int phoneWidth2 = 15;
                int addressWidth2 = ConsoleUI.ComputeFlexibleWidth(patientWidth2 + doctorWidth2 + emailWidth2 + phoneWidth2, 5 - 1, 20, 60);
                string header2 = $"{"Patient".PadRight(patientWidth2)} | {"Doctor".PadRight(doctorWidth2)} | {"Email Address".PadRight(emailWidth2)} | {"Phone".PadRight(phoneWidth2)} | {"Address".PadRight(addressWidth2)}";
                Console.WriteLine(header2);
                Console.WriteLine(new string('-', header2.Length));
                var addr2 = ConsoleUI.Truncate(patient.GetFormattedAddress(), addressWidth2);
                Console.WriteLine($"{patient.Name.PadRight(patientWidth2)} | {doctorName.PadRight(doctorWidth2)} | {patient.Email.PadRight(emailWidth2)} | {patient.Phone.PadRight(phoneWidth2)} | {addr2}");

                ConsoleUI.Pause();
                return;
            }
        }
        
        private static void ShowAppointmentsWithPatient(Doctor doctor, List<Appointment> appointments, List<Patient> patients)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Appointments With");
            Console.WriteLine();
            
            int patientId;
            Patient patient;
            while (true)
            {
                Console.Write("Enter the ID of the patient you would like to view appointments for ");
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("('b' to go back)"); Console.ResetColor(); Console.Write(": ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && input.Trim().Equals("b", StringComparison.OrdinalIgnoreCase)) return;
                if (string.IsNullOrWhiteSpace(input)) { ConsoleUI.Error("Patient ID cannot be empty."); continue; }
                if (!int.TryParse(input, out patientId)) { ConsoleUI.Error("Invalid patient ID format."); continue; }
                patient = patients.FirstOrDefault(p => p.Id == patientId);
                if (patient == null) { ConsoleUI.Error($"Patient with ID {patientId} not found."); continue; }
                break;
            }
            
            var patientAppointments = appointments.Where(a => a.PatientId == patientId && a.DoctorId == doctor.Id).ToList();
            
            if (patientAppointments.Count == 0)
            {
                ConsoleUI.Warning($"No appointments found between {doctor.DisplayName} and {patient.Name}.");
            }
            else
            {
                int doctorWidth = 20;
                int patientWidth = 20;
                int descWidth = ConsoleUI.ComputeFlexibleWidth(doctorWidth + patientWidth, 3 - 1, 15, 60);
                string header = $"{"Doctor".PadRight(doctorWidth)} | {"Patient".PadRight(patientWidth)} | {"Description".PadRight(descWidth)}";
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));
                
                foreach (var appointment in patientAppointments)
                {
                    var note = ConsoleUI.Truncate(appointment.Note ?? string.Empty, descWidth);
                    Console.WriteLine($"{doctor.DisplayName.PadRight(doctorWidth)} | {patient.Name.PadRight(patientWidth)} | {note}");
                }
            }
            
            ConsoleUI.Pause();
        }
    }
}
