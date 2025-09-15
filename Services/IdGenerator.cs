using System;
using System.Collections.Generic;
using System.Linq;
using DotnetHospital.Entities;

namespace DotnetHospital.Services
{
    /// <summary>
    /// Service class for generating unique IDs with specific rules for different entity types
    /// Provides ID generation with predefined ranges for each user type
    /// </summary>
    public static class IdGenerator
    {
        private static readonly Random Rng = new Random();

        /// <summary>
        /// Legacy method for backward compatibility (appointments, etc.)
        /// Generates random ID between 10000 and 99999999
        /// </summary>
        /// <returns>Random integer ID</returns>
        public static int NewId()
        {
            return Rng.Next(10000, 99999999); // always between 5 and 8 digits
        }

        /// <summary>
        /// Generates Patient ID in 1000X format (10001, 10002, 10003, ...)
        /// </summary>
        /// <param name="existingPatients">Collection of existing patients for ID validation</param>
        /// <returns>New unique patient ID</returns>
        public static int NewPatientId(IEnumerable<Patient> existingPatients)
        {
            var maxId = existingPatients?.Any() == true 
                ? existingPatients.Where(p => p.Id >= 10000 && p.Id < 20000).Max(p => p.Id)
                : 10000;
            return maxId + 1;
        }

        /// <summary>
        /// Generates Doctor ID in 2000X format (20001, 20002, 20003, ...)
        /// </summary>
        /// <param name="existingDoctors">Collection of existing doctors for ID validation</param>
        /// <returns>New unique doctor ID</returns>
        public static int NewDoctorId(IEnumerable<Doctor> existingDoctors)
        {
            var maxId = existingDoctors?.Any() == true 
                ? existingDoctors.Where(d => d.Id >= 20000 && d.Id < 90000).Max(d => d.Id)
                : 20000;
            return maxId + 1;
        }

        /// <summary>
        /// Generates Admin ID in 9000X format (90001, 90002, 90003, ...)
        /// </summary>
        /// <param name="existingAdmins">Collection of existing admins for ID validation</param>
        /// <returns>New unique admin ID</returns>
        public static int NewAdminId(IEnumerable<Admin> existingAdmins)
        {
            var maxId = existingAdmins?.Any() == true 
                ? existingAdmins.Where(a => a.Id >= 90000).Max(a => a.Id)
                : 90000;
            return maxId + 1;
        }

        /// <summary>
        /// Generates password based on ID: "pw" + ID
        /// </summary>
        /// <param name="id">User ID to generate password for</param>
        /// <returns>Password string in format "pw{id}"</returns>
        public static string GeneratePassword(int id)
        {
            return $"pw{id}";
        }

        /// <summary>
        /// Generates Appointment ID in A0001, A0002, A0003, ... format
        /// </summary>
        /// <returns>New appointment ID string</returns>
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
