namespace DotnetHospital.Entities
{
    // Patient inherits from User
    public sealed class Patient : User
    {
        public int? DoctorId { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }  // NEW

        public Patient(string name, string password, int age, string gender, int? doctorId = null, int? id = null)
            : base(name, password, id)
        {
            Age = age;
            Gender = gender;                // NEW
            DoctorId = doctorId;
        }

        // Override: add patient-specific information
        public override string Summary()
        {
            return $"{Id} | {Name} | Age:{Age} | Gender:{Gender} | Doctor:{DoctorId?.ToString() ?? "-"}";
        }
    }
}
