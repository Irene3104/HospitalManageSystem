namespace DotnetHospital.Entities
{
    // Patient inherits from User
    public sealed class Patient : User
    {
        public int? DoctorId { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;  // NEW

        public string GenderDisplay
        {
            get
            {
                if (string.IsNullOrEmpty(Gender))
                    return "Unknown";

                switch (Gender.ToUpper())
                {
                    case "M":
                        return "Man";
                    case "W":
                        return "Woman";
                    default:
                        return Gender;
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

        // Override: add patient-specific information
        public override string Summary()
        {
            return $"{Id} | {Name} | Age:{Age} | Gender:{GenderDisplay} | Doctor:{DoctorId?.ToString() ?? "-"}";
        }
    }
}
