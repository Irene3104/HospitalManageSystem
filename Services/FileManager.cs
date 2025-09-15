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
        public static string DataDir => Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "Data");

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
                $"{p.Id},{p.Name},{p.Password},{p.Age},{p.Gender},{p.DoctorId},{p.Email},{p.Phone},{p.StreetNumber},{p.Street},{p.City},{p.State}"));

            File.WriteAllLines(DoctorsPath, doctors.Select(d =>
                $"{d.Id},{d.Name},{d.Password},{d.Specialty},{d.Email},{d.Phone},{d.StreetNumber},{d.Street},{d.City},{d.State}"));

            File.WriteAllLines(AdminsPath, admins.Select(a =>
                $"{a.Id},{a.Name},{a.Password},{a.Email},{a.Phone},{a.StreetNumber},{a.Street},{a.City},{a.State}"));

            // Persist appointments in simple format: id,patientId,doctorId,note
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
                // Id,Name,Password,Age,Gender,DoctorId,Email,Phone,StreetNumber,Street,City,State
                return new Patient(
                    name: t[1],
                    password: t[2],
                    age: int.Parse(t[3]),
                    gender: t[4],
                    doctorId: string.IsNullOrWhiteSpace(t[5]) ? (int?)null : int.Parse(t[5]),
                    email: t.Length > 6 ? t[6] : "",
                    phone: t.Length > 7 ? t[7] : "",
                    streetNumber: t.Length > 8 ? t[8] : "",
                    street: t.Length > 9 ? t[9] : "",
                    city: t.Length > 10 ? t[10] : "",
                    state: t.Length > 11 ? t[11] : "",
                    id: int.Parse(t[0]));
            });
        }


        private static List<Doctor> LoadDoctors()
        {
            return Load(DoctorsPath, line =>
            {
                var t = line.Split(',');
                // Order in doctors.txt:
                // Id,Name,Password,Specialty,Email,Phone,StreetNumber,Street,City,State
                return new Doctor(
                    name: t[1],
                    password: t[2],
                    specialty: t[3],
                    email: t.Length > 4 ? t[4] : "",
                    phone: t.Length > 5 ? t[5] : "",
                    streetNumber: t.Length > 6 ? t[6] : "",
                    street: t.Length > 7 ? t[7] : "",
                    city: t.Length > 8 ? t[8] : "",
                    state: t.Length > 9 ? t[9] : "",
                    id: int.Parse(t[0]));
            });
        }

        private static List<Admin> LoadAdmins()
        {
            return Load(AdminsPath, line =>
            {
                var t = line.Split(',');
                // Order in admins.txt:
                // Id,Name,Password,Email,Phone,StreetNumber,Street,City,State
                return new Admin(
                    name: t[1],
                    password: t[2],
                    email: t.Length > 3 ? t[3] : "",
                    phone: t.Length > 4 ? t[4] : "",
                    streetNumber: t.Length > 5 ? t[5] : "",
                    street: t.Length > 6 ? t[6] : "",
                    city: t.Length > 7 ? t[7] : "",
                    state: t.Length > 8 ? t[8] : "",
                    id: int.Parse(t[0]));
            });
        }
        private static List<Appointment> LoadAppointments()
        {
            return Load(AppointmentsPath, line =>
            {
                if (line.StartsWith("#")) return default; // ignore header

                var parts = line.Split(',');
                if (parts.Length < 4) return default; // invalid row

                string id = parts[0];
                int patientId = int.Parse(parts[1]);
                int doctorId = int.Parse(parts[2]);
                string note = string.Join(",", parts.Skip(3));

                return new Appointment(id, patientId, doctorId, note);
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
