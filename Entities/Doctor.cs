namespace DotnetHospital.Entities
{
    // Doctor inherits from User
    public sealed class Doctor : User
    {
        public string Specialty { get; set; }

        public Doctor(string name, string password, string specialty, int? id = null)
            : base(name, password, id)
        {
            Specialty = specialty;
        }

        // Override: add doctor-specific information
        public override string Summary()
        {
            return $"{Id} | Dr. {Name} | {Specialty}";
        }
    }
}
