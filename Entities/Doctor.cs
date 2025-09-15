namespace DotnetHospital.Entities
{
    // Doctor inherits from User
    public sealed class Doctor : User
    {
        public string Specialty { get; set; }

        // Display helper to ensure a unified "Dr. <Name>" label without double prefixing
        public string DisplayName
        {
            get
            {
                var n = Name ?? string.Empty;
                return n.TrimStart().StartsWith("Dr.", System.StringComparison.OrdinalIgnoreCase)
                    ? n
                    : $"Dr. {n}";
            }
        }

        public Doctor(string name, string password, string specialty, 
                     string email = "", string phone = "", string streetNumber = "", string street = "", 
                     string city = "", string state = "", int? id = null)
            : base(name, password, email, phone, streetNumber, street, city, state, id)
        {
            Specialty = specialty;
        }

        // Constructor for creating new doctors with auto-generated ID and password
        public static Doctor CreateNew(string name, string specialty, 
                                     string email = "", string phone = "", string streetNumber = "", string street = "", 
                                     string city = "", string state = "", 
                                     System.Collections.Generic.IEnumerable<Doctor> existingDoctors = null)
        {
            var newId = Services.IdGenerator.NewDoctorId(existingDoctors);
            var password = Services.IdGenerator.GeneratePassword(newId);
            return new Doctor(name, password, specialty, email, phone, streetNumber, street, city, state, newId);
        }

        // Override: add doctor-specific information
        public override string Summary()
        {
            return $"{Id} | Dr. {Name} | {Specialty}";
        }
    }
}
