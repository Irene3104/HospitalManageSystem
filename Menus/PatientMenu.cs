using System;
using System.Collections.Generic;
using System.Linq;
using DotnetHospital.Entities;
using DotnetHospital.Services;

namespace DotnetHospital.Menus
{
    public static class PatientMenu
    {
        public static void ShowMenu(Patient currentPatient, List<Patient> patients, 
                                  List<Doctor> doctors, List<Appointment> appointments)
        {
            while (true)
            {
                ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Patient Menu");
                ConsoleUI.Log($"Welcome to DOTNET Hospital Management System {currentPatient.Name}", ConsoleColor.Green);
                Console.WriteLine();
                
                string[] options = {
                    "List patient details",
                    "List my doctor details", 
                    "List all appointments",
                    "Book appointment",
                    "Logout",
                    "Exit"
                };
                
                ConsoleUI.DrawMenu("Please choose an option:", options, ConsoleColor.Green, includeZeroExit: false);
                
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ShowPatientDetails(currentPatient);
                        break;
                    case "2":
                        ShowMyDoctorDetails(currentPatient, doctors);
                        break;
                    case "3":
                        ShowAllAppointments(currentPatient, appointments, doctors);
                        break;
                    case "4":
                        BookAppointment(currentPatient, doctors, appointments);
                        break;
                    case "5":
                        return; // Return to login
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        ConsoleUI.Error("Invalid choice. Please try again.");
                        ConsoleUI.Pause();
                        break;
                }
            }
        }
        
        private static void ShowPatientDetails(Patient patient)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "My Details");
            ConsoleUI.Log($"{patient.Name}'s Details", ConsoleColor.Green);
            Console.WriteLine();
            
            Console.WriteLine($"Patient ID: {patient.Id}");
            Console.WriteLine($"Full Name: {patient.Name}");
            Console.WriteLine($"Age: {patient.Age}");
            Console.WriteLine($"Gender: {patient.GenderDisplay}");
            Console.WriteLine($"Address: {patient.GetFormattedAddress()}");
            Console.WriteLine($"Email: {patient.Email}");
            Console.WriteLine($"Phone: {patient.Phone}");
            
            ConsoleUI.Pause();
        }
        
        private static void ShowMyDoctorDetails(Patient patient, List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "My Doctor");
            
            if (patient.DoctorId == null)
            {
                ConsoleUI.Warning("You are not assigned to any doctor.");
            }
            else
            {
                var doctor = doctors.FirstOrDefault(d => d.Id == patient.DoctorId);
                if (doctor != null)
                {
                    ConsoleUI.Log("Your doctor:", ConsoleColor.Green);
                    Console.WriteLine();
                    // Unified table layout with dynamic last column width
                    int nameWidth = 20;
                    int emailWidth = 28;
                    int phoneWidth = 15;
                    int addressWidth = ConsoleUI.ComputeFlexibleWidth(nameWidth + emailWidth + phoneWidth, 3, 20, 60);

                    string header =
                        $"{"Name".PadRight(nameWidth)} | {"Email Address".PadRight(emailWidth)} | {"Phone".PadRight(phoneWidth)} | {"Address".PadRight(addressWidth)}";
                    Console.WriteLine(header);
                    Console.WriteLine(new string('-', header.Length));

                    var addr = ConsoleUI.Truncate(doctor.GetFormattedAddress(), addressWidth);
                    Console.WriteLine($"{doctor.DisplayName.PadRight(nameWidth)} | {doctor.Email.PadRight(emailWidth)} | {doctor.Phone.PadRight(phoneWidth)} | {addr}");
                }
                else
                {
                    ConsoleUI.Error("Your assigned doctor was not found.");
                }
            }
            
            ConsoleUI.Pause();
        }
        
        private static void ShowAllAppointments(Patient patient, List<Appointment> appointments, List<Doctor> doctors)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "My Appointments");
            ConsoleUI.Log($"Appointments for {patient.Name}", ConsoleColor.Green);
            Console.WriteLine();
            
            var patientAppointments = appointments.Where(a => a.PatientId == patient.Id).ToList();
            
            if (patientAppointments.Count == 0)
            {
                ConsoleUI.Warning("No appointments found.");
            }
            else
            {
                int doctorWidth = 20;
                int patientWidth = 20;
                int descWidth = ConsoleUI.ComputeFlexibleWidth(doctorWidth + patientWidth, 2, 15, 60);

                string header = $"{"Doctor".PadRight(doctorWidth)} | {"Patient".PadRight(patientWidth)} | {"Description".PadRight(descWidth)}";
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));
                
                foreach (var appointment in patientAppointments)
                {
                    var doctor = doctors.FirstOrDefault(d => d.Id == appointment.DoctorId);
                    var doctorName = doctor?.DisplayName ?? "Unknown Doctor";
                    var note = ConsoleUI.Truncate(appointment.Note ?? string.Empty, descWidth);
                    Console.WriteLine($"{doctorName.PadRight(doctorWidth)} | {patient.Name.PadRight(patientWidth)} | {note}");
                }
            }
            
            ConsoleUI.Pause();
        }
        
        private static void BookAppointment(Patient patient, List<Doctor> doctors, List<Appointment> appointments)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Book Appointment");
            
            Doctor selectedDoctor = null;
            
            if (patient.DoctorId == null)
            {
                ConsoleUI.Warning("You are not registered with any doctor! Please choose which doctor you would like to register with");
                Console.WriteLine();
                
                for (int i = 0; i < doctors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {doctors[i].DisplayName} | {doctors[i].Specialty}");
                }
                
                Console.Write("Please choose a doctor: ");
                var choice = Console.ReadLine();
                
                if (int.TryParse(choice, out int doctorIndex) && doctorIndex > 0 && doctorIndex <= doctors.Count)
                {
                    selectedDoctor = doctors[doctorIndex - 1];
                    patient.DoctorId = selectedDoctor.Id; // Register patient with doctor
                }
                else
                {
                    ConsoleUI.Error("Invalid doctor selection.");
                    ConsoleUI.Pause();
                    return;
                }
            }
            else
            {
                selectedDoctor = doctors.FirstOrDefault(d => d.Id == patient.DoctorId);
                if (selectedDoctor == null)
                {
                    ConsoleUI.Error("Your assigned doctor was not found.");
                    ConsoleUI.Pause();
                    return;
                }
            }
            
            ConsoleUI.Log($"You are booking a new appointment with {selectedDoctor.DisplayName}", ConsoleColor.Green);
            Console.Write("Description of the appointment: ");
            var description = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(description))
            {
                ConsoleUI.Error("Description cannot be empty.");
                ConsoleUI.Pause();
                return;
            }
            
            // Create new appointment
            var newAppointment = new Appointment(patient.Id, selectedDoctor.Id, DateTime.Now, description);
            appointments.Add(newAppointment);
            
            // Save to file
            var data = FileManager.LoadAll();
            FileManager.SaveAll(data.Item1, data.Item2, data.Item3, appointments);
            
            ConsoleUI.Log("The appointment has been booked successfully!", ConsoleColor.Green);
            ConsoleUI.Pause();
        }
    }
}
