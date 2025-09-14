using System;
using DotnetHospital.Services; 
public sealed class Appointment
{
    public int Id { get; private set; }
    public int PatientId { get; private set; }
    public int DoctorId { get; private set; }
    public DateTime Date { get; private set; }   // NEW
    public string Note { get; private set; }

    // Constructor with explicit ID
    public Appointment(int id, int patientId, int doctorId, DateTime date, string note)
    {
        Id = id;
        PatientId = patientId;
        DoctorId = doctorId;
        Date = date;
        Note = note;
    }

    // Overloaded constructor (auto-generate ID)
    public Appointment(int patientId, int doctorId, DateTime date, string note)
        : this(IdGenerator.NewId(), patientId, doctorId, date, note) { }

    public override string ToString()
    {
        return $"{Id} | P:{PatientId} | D:{DoctorId} | {Date:yyyy-MM-dd HH:mm} | {Note}";
    }
}
