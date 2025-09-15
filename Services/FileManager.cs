using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotnetHospital.Entities;

namespace DotnetHospital.Services
{
    // Service for loading and saving data to text files
    public static class FileManager
    {
        public static string DataDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        public static string PatientsPath => Path.Combine(DataDir, "patients.txt");
        public static string DoctorsPath => Path.Combine(DataDir, "doctors.txt");
        public static string AdminsPath => Path.Combine(DataDir, "admins.txt");
        public static string AppointmentsPath => Path.Combine(DataDir, "appointments.txt");

        // Load all data files at once
        public static Tuple<List<Patient>, List<Doctor>, List<Admin>, List<Appointment>> LoadAll()
        {
            Directory.CreateDirectory(DataDir);
            return new Tuple<List<Patient>, List<Doctor>, List<Admin>, List<Appointment>>(
                LoadPatients(), LoadDoctors(), LoadAdmins(), LoadAppointments());
        }

        // Save all entities back to files
        public static void SaveAll(
            IEnumerable<Patient> patients,
            IEnumerable<Doctor> doctors,
            IEnumerable<Admin> admins,
            IEnumerable<Appointment> appts)
        {
            Directory.CreateDirectory(DataDir);

            File.WriteAllLines(PatientsPath, patients.Select(p =>
                $"{p.Id},{p.Name},{p.Password},{p.Age},{p.Gender},{p.DoctorId}"));

            File.WriteAllLines(DoctorsPath, doctors.Select(d =>
                $"{d.Id},{d.Name},{d.Password},{d.Specialty}"));

            File.WriteAllLines(AdminsPath, admins.Select(a =>
                $"{a.Id},{a.Name},{a.Password}"));

            File.WriteAllLines(AppointmentsPath, appts.Select(a =>
                $"{a.Id},{a.PatientId},{a.DoctorId},{a.Note}"));
        }

        // Load methods for each entity type
        private static List<Patient> LoadPatients()
        {
            return Load(PatientsPath, line =>
            {
                var t = line.Split(',');
                // Order in patients.txt:
                // Id,Name,Password,Age,Gender,DoctorId
                return new Patient(
                    name: t[1],
                    password: t[2],
                    age: int.Parse(t[3]),
                    gender: t[4],                                          
                    doctorId: string.IsNullOrWhiteSpace(t[5]) ? (int?)null : int.Parse(t[5]),
                    id: int.Parse(t[0]));
            });
        }


        private static List<Doctor> LoadDoctors()
        {
            return Load(DoctorsPath, line =>
            {
                var t = line.Split(',');
                return new Doctor(
                    name: t[1],
                    password: t[2],
                    specialty: t[3],
                    id: int.Parse(t[0]));
            });
        }

        private static List<Admin> LoadAdmins()
        {
            return Load(AdminsPath, line =>
            {
                var t = line.Split(',');
                return new Admin(
                    name: t[1],
                    password: t[2],
                    id: int.Parse(t[0]));
            });
        }
        private static List<Appointment> LoadAppointments()
        {
            return Load(AppointmentsPath, line =>
            {
                if (line.StartsWith("#")) return default; // ignore header

                var t = line.Split(',');
                return new Appointment(
                    id: int.Parse(t[0]),
                    patientId: int.Parse(t[1]),
                    doctorId: int.Parse(t[2]),
                    date: DateTime.Parse(t[3]),
                    note: t[4]);
            }).Where(a => a != null).ToList();
        }

        // Generic loader with exception handling
        // Generic loader with exception handling
        private static List<T> Load<T>(string path, Func<string, T> map)
        {
            var list = new List<T>();
            if (!File.Exists(path)) return list;

            int lineNumber = 0;

            foreach (var raw in File.ReadAllLines(path, Encoding.UTF8))
            {
                lineNumber++;
                var line = raw?.Trim();
                if (string.IsNullOrWhiteSpace(line)) continue; // skip empty line
                if (line.StartsWith("#")) continue;            // skip header/comment line

                try
                {
                    list.Add(map(line));
                }
                catch (Exception ex)
                {
                    // Log invalid line to console
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[WARNING] Failed to parse line {lineNumber} in {Path.GetFileName(path)}");
                    Console.WriteLine($" → Content: \"{line}\"");
                    Console.WriteLine($" → Error: {ex.Message}");
                    Console.ResetColor();
                }
            }
            return list;
        }

    }
}
