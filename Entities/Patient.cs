namespace DotnetHospital.Entities
{
    // Patient inherits from User
    public sealed class Patient : User
    {
        public int? DoctorId { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;  

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

        public Patient(string name, string password, int age, string gender, int? doctorId = null, 
                      string email = "", string phone = "", string streetNumber = "", string street = "", 
                      string city = "", string state = "", int? id = null)
            : base(name, password, email, phone, streetNumber, street, city, state, id)
        {
            Age = age;
            Gender = gender;
            DoctorId = doctorId;
        }

        // Constructor for creating new patients with auto-generated ID and password
        public static Patient CreateNew(string name, int age, string gender, 
                                      int? doctorId = null, string email = "", string phone = "", 
                                      string streetNumber = "", string street = "", string city = "", string state = "", 
                                      System.Collections.Generic.IEnumerable<Patient> existingPatients = null)
        {
            var newId = Services.IdGenerator.NewPatientId(existingPatients);
            var password = Services.IdGenerator.GeneratePassword(newId);
            return new Patient(name, password, age, gender, doctorId, email, phone, streetNumber, street, city, state, newId);
        }

        // Override: add patient-specific information
        public override string Summary()
        {
            return $"{Id} | {Name} | Age:{Age} | Gender:{GenderDisplay} | Doctor:{DoctorId?.ToString() ?? "-"}";
        }
    }
}
