using System;
using DotnetHospital.Services;

namespace DotnetHospital.Entities
{
    /// <summary>
    /// Appointment entity class representing a medical appointment
    /// Links a patient with a doctor for a specific appointment
    /// </summary>
    public sealed class Appointment
    {
        public string Id { get; private set; }
        public int PatientId { get; private set; }
        public int DoctorId { get; private set; }
        public string Note { get; private set; }

        /// <summary>
        /// Constructor with explicit appointment ID
        /// </summary>
        /// <param name="id">Appointment ID</param>
        /// <param name="patientId">Patient's ID</param>
        /// <param name="doctorId">Doctor's ID</param>
        /// <param name="note">Appointment notes/description</param>
        public Appointment(string id, int patientId, int doctorId, string note)
        {
            Id = id;
            PatientId = patientId;
            DoctorId = doctorId;
            Note = note;
        }

        /// <summary>
        /// Overloaded constructor with auto-generated appointment ID
        /// </summary>
        /// <param name="patientId">Patient's ID</param>
        /// <param name="doctorId">Doctor's ID</param>
        /// <param name="note">Appointment notes/description</param>
        public Appointment(int patientId, int doctorId, string note)
            : this(IdGenerator.NewAppointmentId(), patientId, doctorId, note) { }

        /// <summary>
        /// Override ToString to return appointment summary
        /// </summary>
        /// <returns>Formatted appointment string with ID, patient ID, doctor ID, and notes</returns>
        public override string ToString()
        {
            return $"{Id} | P:{PatientId} | D:{DoctorId} | {Note}";
        }
    }
}
