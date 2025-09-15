using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
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
                    "Exit to login",
                    "Exit System"
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
                        ShowMyDoctorDetails(currentPatient, doctors, appointments);
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
        
        private static void ShowMyDoctorDetails(Patient patient, List<Doctor> doctors, List<Appointment> appointments)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "My Doctor");

            if (!patient.DoctorId.HasValue)
            {
                ConsoleUI.Warning("You are not assigned to any doctor.");
            }
            else
            {
                var d = doctors.FirstOrDefault(x => x.Id == patient.DoctorId.Value);
                if (d == null)
                {
                    ConsoleUI.Error("Your registered doctor was not found.");
                }
                else
                {
                    ConsoleUI.Log("Your doctor:", ConsoleColor.Green);
                    Console.WriteLine();
                    int nameWidth = 20;
                    int emailWidth = 28;
                    int phoneWidth = 15;
                    int addressWidth = ConsoleUI.ComputeFlexibleWidth(nameWidth + emailWidth + phoneWidth, 3, 20, 60);
                    string header = $"{"Name".PadRight(nameWidth)} | {"Email Address".PadRight(emailWidth)} | {"Phone".PadRight(phoneWidth)} | {"Address".PadRight(addressWidth)}";
                    Console.WriteLine(header);
                    Console.WriteLine(new string('-', header.Length));
                    var addr = ConsoleUI.Truncate(d.GetFormattedAddress(), addressWidth);
                    Console.WriteLine($"{d.Name.PadRight(nameWidth)} | {d.Email.PadRight(emailWidth)} | {d.Phone.PadRight(phoneWidth)} | {addr}");
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
                int dateWidth = 16;   // yyyy-MM-dd HH:mm

                int descWidth = ConsoleUI.ComputeFlexibleWidth(doctorWidth + patientWidth + dateWidth, 3, 15, 60);

                string header = $"{"Doctor".PadRight(doctorWidth)} | {"Patient".PadRight(patientWidth)} | {"Date/Time".PadRight(dateWidth)} | {"Description".PadRight(descWidth)}";
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));

                foreach (var appointment in patientAppointments.OrderBy(a => a.Date))
                {
                    var doctor = doctors.FirstOrDefault(d => d.Id == appointment.DoctorId);
                    var doctorName = doctor?.Name ?? "Unknown Doctor";
                    var shownNote = ConsoleUI.Truncate(appointment.Note ?? string.Empty, descWidth);
                    Console.WriteLine($"{doctorName.PadRight(doctorWidth)} | {patient.Name.PadRight(patientWidth)} | {appointment.Date:yyyy-MM-dd HH:mm} | {shownNote}");
                }
            }
            
            ConsoleUI.Pause();
        }
        
        private static void BookAppointment(Patient patient, List<Doctor> doctors, List<Appointment> appointments)
        {
            ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Book Appointment");

            // Simple spec: use current date/time for the appointment
            var apptDateTime = DateTime.Now;

            // 2) Ensure patient has a registered doctor. If not, register now.
            Doctor selectedDoctor = null;
            if (!patient.DoctorId.HasValue)
            {
                ConsoleUI.Warning("You are not registered with any doctor. Please choose one to register.");
                Console.WriteLine();

                int idxWidth = 4; // numbering width
                int nameWidth = 20;
                int specWidth = 18;
                int emailWidth = 28;
                int phoneWidth = 15;
                string header = $"{"#".PadRight(idxWidth)} {"Name".PadRight(nameWidth)} | {"Specialty".PadRight(specWidth)} | {"Email".PadRight(emailWidth)} | {"Phone".PadRight(phoneWidth)} | Address";
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));

                for (int i = 0; i < doctors.Count; i++)
                {
                    var d = doctors[i];
                    Console.WriteLine($"{(i + 1).ToString().PadRight(idxWidth)} {d.Name.PadRight(nameWidth)} | {d.Specialty.PadRight(specWidth)} | {d.Email.PadRight(emailWidth)} | {d.Phone.PadRight(phoneWidth)} | {d.GetFormattedAddress()}");
                }

                Console.Write("Please choose a doctor to register ");
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("('b' to go back)"); Console.ResetColor(); Console.Write(": ");
                var regChoice = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(regChoice) && regChoice.Trim().Equals("b", StringComparison.OrdinalIgnoreCase)) return;
                if (!(int.TryParse(regChoice, out int regIndex) && regIndex > 0 && regIndex <= doctors.Count))
                {
                    ConsoleUI.Error("Invalid doctor selection.");
                    ConsoleUI.Pause();
                    return;
                }
                selectedDoctor = doctors[regIndex - 1];
                patient.DoctorId = selectedDoctor.Id;

                // Persist registration immediately
                var dataForReg = FileManager.LoadAll();
                var patientsDataForReg = dataForReg.Item1;
                var d2 = patientsDataForReg.FirstOrDefault(p => p.Id == patient.Id);
                if (d2 != null) d2.DoctorId = patient.DoctorId;
                FileManager.SaveAll(patientsDataForReg, dataForReg.Item2, dataForReg.Item3, dataForReg.Item4);
            }
            else
            {
                selectedDoctor = doctors.FirstOrDefault(doc => doc.Id == patient.DoctorId.Value);
            }

            // If patient has no registered doctor yet, bind to the selected one
            if (patient.DoctorId == null || patient.DoctorId != selectedDoctor.Id)
            {
                patient.DoctorId = selectedDoctor.Id;
            }

            // 3) Get description
            ConsoleUI.Log($"You are booking a new appointment with {selectedDoctor.Name}", ConsoleColor.Green);
            string description;
            while (true)
            {
                Console.Write("Description of the appointment ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("('b' to go back)");
                Console.ResetColor();
                Console.Write(": ");
                description = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(description) && description.Trim().Equals("b", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(description))
                {
                    ConsoleUI.Error("Description cannot be empty.");
                    continue; // re-prompt instead of exiting
                }
                break;
            }

            // 4) Create and persist appointment with the registered doctor
            var newAppointment = new Appointment(patient.Id, selectedDoctor.Id, apptDateTime, description);
            appointments.Add(newAppointment);

            // Persist safely (load, append, save)
            var data = FileManager.LoadAll();
            var patientsData = data.Item1;
            var doctorsData = data.Item2;
            var adminsData = data.Item3;
            var apptsData = data.Item4;
            // Ensure patient's chosen doctor is saved to patients.txt
            var pInStore = patientsData.FirstOrDefault(p => p.Id == patient.Id);
            if (pInStore != null)
            {
                pInStore.DoctorId = patient.DoctorId;
            }

            apptsData.Add(newAppointment);
            FileManager.SaveAll(patientsData, doctorsData, adminsData, apptsData);

            ConsoleUI.Log("The appointment has been booked successfully!", ConsoleColor.Green);
            ConsoleUI.Pause();
        }
    }
}

