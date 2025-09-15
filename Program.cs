using System;
using System.Linq;
using DotnetHospital.Entities;
using DotnetHospital.Services;
using DotnetHospital.Menus;

namespace DotnetHospital
{
    internal class Program
    {
        /// <summary>
        /// Entry point for .NET Framework console application
        /// Main application loop for hospital management system
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            // Initialize data from files
            Console.WriteLine("DataDir = " + FileManager.DataDir);

            var data = FileManager.LoadAll();
            var patients = data.Item1;
            var doctors = data.Item2;
            var admins = data.Item3;
            var appts = data.Item4;

            Console.WriteLine($"Counts → P:{patients.Count}, D:{doctors.Count}, A:{admins.Count}, Ap:{appts.Count}");

            // Main application loop
            while (true)
            {
                // Display login header
                ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Login");

                // Validate user ID input
                int id;
                while (true)
                {
                    Console.Write("ID: ");
                    var idRaw = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(idRaw))
                    {
                        ConsoleUI.Error("ID is required.");
                        continue; // Re-prompt for ID
                    }
                    if (!int.TryParse(idRaw, out id))
                    {
                        ConsoleUI.Error("Invalid ID format. Please enter digits only.");
                        continue;
                    }
                    break;
                }

                // Get password input (masked)
                Console.Write("Password: ");
                var pw = ConsoleUI.ReadPassword();

                // Combine all user types for authentication
                var allUsers = patients.Cast<User>()
                    .Concat(doctors.Cast<User>())
                    .Concat(admins.Cast<User>());

                // Anonymous method for user authentication
                User user = allUsers.ToList().Find(delegate(User u) { 
                    return u.Id == id && u.Password == pw; 
                });

                // Handle authentication result
                if (user == null)
                {
                    ConsoleUI.Error("Invalid credentials.");
                    ConsoleUI.Pause();
                    continue;
                }

                ConsoleUI.Log("Valid Credentials", ConsoleColor.Green);
                ConsoleUI.Pause();
                
                // Route to appropriate menu based on user type
                if (user is Patient patient)
                {
                    PatientMenu.ShowMenu(patient, patients, doctors, appts);
                }
                else if (user is Doctor doctor)
                {
                    DoctorMenu.ShowMenu(doctor, patients, doctors, appts);
                }
                else if (user is Admin admin)
                {
                    AdminMenu.ShowMenu(admin, patients, doctors, appts);
                }
                else
                {
                    ConsoleUI.Error("Unknown user type.");
                    ConsoleUI.Pause();
                }
            }
        }
    }
}
