namespace DotnetHospital.Entities
{
    /// <summary>
    /// Doctor entity class inheriting from User
    /// Represents a doctor in the hospital management system
    /// </summary>
    public sealed class Doctor : User
    {
        public string Specialty { get; set; }

        /// <summary>
        /// Property to display doctor name with "Dr." prefix (avoids double prefixing)
        /// </summary>
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

        /// <summary>
        /// Constructor for Doctor class
        /// </summary>
        /// <param name="name">Doctor's full name</param>
        /// <param name="password">Doctor's password</param>
        /// <param name="specialty">Doctor's medical specialty</param>
        /// <param name="email">Doctor's email address</param>
        /// <param name="phone">Doctor's phone number</param>
        /// <param name="streetNumber">Street number</param>
        /// <param name="street">Street name</param>
        /// <param name="city">City name</param>
        /// <param name="state">State/Province name</param>
        /// <param name="id">Optional doctor ID (auto-generated if not provided)</param>
        public Doctor(string name, string password, string specialty, 
                     string email = "", string phone = "", string streetNumber = "", string street = "", 
                     string city = "", string state = "", int? id = null)
            : base(name, password, email, phone, streetNumber, street, city, state, id)
        {
            Specialty = specialty;
        }

        /// <summary>
        /// Static factory method for creating new doctors with auto-generated ID and password
        /// </summary>
        /// <param name="name">Doctor's full name</param>
        /// <param name="specialty">Doctor's medical specialty</param>
        /// <param name="email">Doctor's email address</param>
        /// <param name="phone">Doctor's phone number</param>
        /// <param name="streetNumber">Street number</param>
        /// <param name="street">Street name</param>
        /// <param name="city">City name</param>
        /// <param name="state">State/Province name</param>
        /// <param name="existingDoctors">Collection of existing doctors for ID generation</param>
        /// <returns>New Doctor instance with auto-generated ID and password</returns>
        public static Doctor CreateNew(string name, string specialty, 
                                     string email = "", string phone = "", string streetNumber = "", string street = "", 
                                     string city = "", string state = "", 
                                     System.Collections.Generic.IEnumerable<Doctor> existingDoctors = null)
        {
            var newId = Services.IdGenerator.NewDoctorId(existingDoctors);
            var password = Services.IdGenerator.GeneratePassword(newId);
            return new Doctor(name, password, specialty, email, phone, streetNumber, street, city, state, newId);
        }

        /// <summary>
        /// Override Summary method to include doctor-specific information
        /// </summary>
        /// <returns>Doctor summary with ID, name with Dr. prefix, and specialty</returns>
        public override string Summary()
        {
            return $"{Id} | Dr. {Name} | {Specialty}";
        }
    }
}
