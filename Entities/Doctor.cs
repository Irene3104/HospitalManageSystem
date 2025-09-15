namespace DotnetHospital.Entities
{
    // Doctor inherits from User
    public sealed class Doctor : User
    {
        public string Specialty { get; set; }

        public Doctor(string name, string password, string specialty, 
                     string email = "", string phone = "", string streetNumber = "", string street = "", 
                     string city = "", string state = "", int? id = null)
            : base(name, password, email, phone, streetNumber, street, city, state, id)
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
