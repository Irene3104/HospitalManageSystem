using System;
using System.Linq;
using DotnetHospital.Entities;
using DotnetHospital.Services;

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

                Console.Write("ID: ");
                var idRaw = Console.ReadLine();

                Console.Write("Password: ");
                var pw = ConsoleUI.ReadPassword();

                int id;
                if (!int.TryParse(idRaw, out id))
                {
                    ConsoleUI.Error("Invalid ID format.");
                    ConsoleUI.Pause();
                    continue;
                }

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

                // TODO: 역할별 메뉴로 분기 (PatientMenu, DoctorMenu, AdminMenu)
                break;
            }
        }
    }
}
