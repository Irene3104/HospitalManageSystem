namespace DotnetHospital.Entities
{
    /// <summary>
    /// Patient entity class inheriting from User
    /// Represents a patient in the hospital management system
    /// </summary>
    public sealed class Patient : User
    {
        public int? DoctorId { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;  

        /// <summary>
        /// Property to display gender in a user-friendly format
        /// </summary>
        public string GenderDisplay
        {
            get
            {
                var gender = Gender?.ToUpper();
                switch (gender)
                {
                    case "M":
                        return "Man";
                    case "W":
                        return "Woman";
                    default:
                        return Gender ?? "Unknown";
                }
            }
        }

        /// <summary>
        /// Constructor for Patient class
        /// </summary>
        /// <param name="name">Patient's full name</param>
        /// <param name="password">Patient's password</param>
        /// <param name="age">Patient's age</param>
        /// <param name="gender">Patient's gender (M/W)</param>
        /// <param name="doctorId">Assigned doctor's ID (optional)</param>
        /// <param name="email">Patient's email address</param>
        /// <param name="phone">Patient's phone number</param>
        /// <param name="streetNumber">Street number</param>
        /// <param name="street">Street name</param>
        /// <param name="city">City name</param>
        /// <param name="state">State/Province name</param>
        /// <param name="id">Optional patient ID (auto-generated if not provided)</param>
        public Patient(string name, string password, int age, string gender, int? doctorId = null, 
                      string email = "", string phone = "", string streetNumber = "", string street = "", 
                      string city = "", string state = "", int? id = null)
            : base(name, password, email, phone, streetNumber, street, city, state, id)
        {
            Age = age;
            Gender = gender;
            DoctorId = doctorId;
        }

        /// <summary>
        /// Static factory method for creating new patients with auto-generated ID and password
        /// </summary>
        /// <param name="name">Patient's full name</param>
        /// <param name="age">Patient's age</param>
        /// <param name="gender">Patient's gender</param>
        /// <param name="doctorId">Assigned doctor's ID (optional)</param>
        /// <param name="email">Patient's email address</param>
        /// <param name="phone">Patient's phone number</param>
        /// <param name="streetNumber">Street number</param>
        /// <param name="street">Street name</param>
        /// <param name="city">City name</param>
        /// <param name="state">State/Province name</param>
        /// <param name="existingPatients">Collection of existing patients for ID generation</param>
        /// <returns>New Patient instance with auto-generated ID and password</returns>
        public static Patient CreateNew(string name, int age, string gender, 
                                      int? doctorId = null, string email = "", string phone = "", 
                                      string streetNumber = "", string street = "", string city = "", string state = "", 
                                      System.Collections.Generic.IEnumerable<Patient> existingPatients = null)
        {
            var newId = Services.IdGenerator.NewPatientId(existingPatients);
            var password = Services.IdGenerator.GeneratePassword(newId);
            return new Patient(name, password, age, gender, doctorId, email, phone, streetNumber, street, city, state, newId);
        }

        /// <summary>
        /// Override Summary method to include patient-specific information
        /// </summary>
        /// <returns>Patient summary with ID, name, age, gender, and assigned doctor</returns>
        public override string Summary()
        {
            return $"{Id} | {Name} | Age:{Age} | Gender:{GenderDisplay} | Doctor:{DoctorId?.ToString() ?? "-"}";
        }
    }
}
