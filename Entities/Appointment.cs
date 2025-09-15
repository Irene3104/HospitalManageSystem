using System;
using DotnetHospital.Services;

namespace DotnetHospital.Entities
{
    public sealed class Appointment
{
    public string Id { get; private set; }
    public int PatientId { get; private set; }
    public int DoctorId { get; private set; }
    public string Note { get; private set; }

    // Constructor with explicit ID
    public Appointment(string id, int patientId, int doctorId, string note)
    {
        Id = id;
        PatientId = patientId;
        DoctorId = doctorId;
        Note = note;
    }

    // Overloaded constructor (auto-generate ID)
    public Appointment(int patientId, int doctorId, string note)
        : this(IdGenerator.NewAppointmentId(), patientId, doctorId, note) { }

    public override string ToString()
    {
        return $"{Id} | P:{PatientId} | D:{DoctorId} | {Note}";
    }
    }
}
