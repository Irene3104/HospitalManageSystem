using System;
using System.Collections.Generic;
using System.Linq;
using DotnetHospital.Entities;

namespace DotnetHospital.Services
{
    // Service for generating unique IDs with specific rules
    public static class IdGenerator
    {
        private static readonly Random Rng = new Random();

        // Legacy method for backward compatibility (appointments, etc.)
        public static int NewId()
        {
            return Rng.Next(10000, 99999999); // always between 5 and 8 digits
        }

        // Generate Patient ID: 1000X format (10001, 10002, 10003, ...)
        public static int NewPatientId(IEnumerable<Patient> existingPatients)
        {
            var maxId = existingPatients?.Any() == true 
                ? existingPatients.Where(p => p.Id >= 10000 && p.Id < 20000).Max(p => p.Id)
                : 10000;
            return maxId + 1;
        }

        // Generate Doctor ID: 2000X format (20001, 20002, 20003, ...)
        public static int NewDoctorId(IEnumerable<Doctor> existingDoctors)
        {
            var maxId = existingDoctors?.Any() == true 
                ? existingDoctors.Where(d => d.Id >= 20000 && d.Id < 90000).Max(d => d.Id)
                : 20000;
            return maxId + 1;
        }

        // Generate Admin ID: 9000X format (90001, 90002, 90003, ...)
        public static int NewAdminId(IEnumerable<Admin> existingAdmins)
        {
            var maxId = existingAdmins?.Any() == true 
                ? existingAdmins.Where(a => a.Id >= 90000).Max(a => a.Id)
                : 90000;
            return maxId + 1;
        }

        // Generate password based on ID: "pw" + ID
        public static string GeneratePassword(int id)
        {
            return $"pw{id}";
        }

        // Generate Appointment ID: A0001, A0002, A0003, ... format
        public static string NewAppointmentId()
        {
            // For now, generate a random A000X format
            // In a real system, you'd want to track existing appointment IDs
            var random = new Random();
            var number = random.Next(1, 10000);
            return $"A{number:D4}";
        }
    }
}
