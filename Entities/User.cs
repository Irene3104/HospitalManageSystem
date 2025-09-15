namespace DotnetHospital.Entities
{
    // Abstract base class for all system users
    public abstract class User
    {
        public int Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string StreetNumber { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;

        protected User(string name, string password, string email = "", string phone = "", 
                      string streetNumber = "", string street = "", string city = "", string state = "", int? id = null)
        {
            // Generate unique ID if not provided
            Id = id ?? Services.IdGenerator.NewId();
            Name = name;
            Password = password;
            Email = email;
            Phone = phone;
            StreetNumber = streetNumber;
            Street = street;
            City = city;
            State = state;
        }

        // Get formatted address
        public string GetFormattedAddress()
        {
            if (string.IsNullOrWhiteSpace(StreetNumber) && string.IsNullOrWhiteSpace(Street) && 
                string.IsNullOrWhiteSpace(City) && string.IsNullOrWhiteSpace(State))
                return "Not provided";
            
            return $"{StreetNumber} {Street}, {City}, {State}".Trim(' ', ',');
        }

        // Virtual method for summary output (can be overridden)
        public virtual string Summary()
        {
            return $"{Id} | {Name}";
        }

        public override string ToString()
        {
            return Summary();
        }
    }
}
