using System;
using System.Linq;
using DotnetHospital.Entities;
using DotnetHospital.Services;
using DotnetHospital.Menus;

namespace DotnetHospital
{
    internal class Program
    {
        // Entry point for .NET Framework console app
        static void Main(string[] args)
        {
            // --- quick diagnostics (you can comment these later) ---
            Console.WriteLine("DataDir = " + FileManager.DataDir);

            var data = FileManager.LoadAll();
            var patients = data.Item1;
            var doctors = data.Item2;
            var admins = data.Item3;
            var appts = data.Item4;

            Console.WriteLine($"Counts → P:{patients.Count}, D:{doctors.Count}, A:{admins.Count}, Ap:{appts.Count}");
            // -------------------------------------------------------

            while (true)
            {
                // Display header
                ConsoleUI.DrawHeader("DOTNET Hospital Management System", "Login");

                // Read and validate ID first; re-prompt if empty/invalid
                int id;
                while (true)
                {
                    Console.Write("ID: ");
                    var idRaw = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(idRaw))
                    {
                        ConsoleUI.Error("ID is required.");
                        continue; // ask again, don't ask for password yet
                    }
                    if (!int.TryParse(idRaw, out id))
                    {
                        ConsoleUI.Error("Invalid ID format. Please enter digits only.");
                        continue;
                    }
                    break;
                }

                Console.Write("Password: ");
                var pw = ConsoleUI.ReadPassword();

                // Find user across all roles
                var allUsers = patients.Cast<User>()
                    .Concat(doctors.Cast<User>())
                    .Concat(admins.Cast<User>());

                User user = allUsers.FirstOrDefault(u => u.Id == id && u.Password == pw);

                if (user == null)
                {
                    ConsoleUI.Error("Invalid credentials.");
                    ConsoleUI.Pause();
                    continue;
                }

                ConsoleUI.Log("Valid Credentials", ConsoleColor.Green);
                ConsoleUI.Pause();
                
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
