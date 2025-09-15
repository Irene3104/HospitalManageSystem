namespace DotnetHospital.Entities
{
    /// <summary>
    /// Abstract base class for all system users (Patient, Doctor, Admin)
    /// Provides common properties and methods for user management
    /// </summary>
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

        /// <summary>
        /// Constructor for User base class
        /// </summary>
        /// <param name="name">User's full name</param>
        /// <param name="password">User's password</param>
        /// <param name="email">User's email address</param>
        /// <param name="phone">User's phone number</param>
        /// <param name="streetNumber">Street number</param>
        /// <param name="street">Street name</param>
        /// <param name="city">City name</param>
        /// <param name="state">State/Province name</param>
        /// <param name="id">Optional user ID (auto-generated if not provided)</param>
        protected User(string name, string password, string email = "", string phone = "", 
                      string streetNumber = "", string street = "", string city = "", string state = "", int? id = null)
        {
            // Auto-generate unique ID if not provided
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

        /// <summary>
        /// Formats the user's address into a readable string
        /// </summary>
        /// <returns>Formatted address string or "Not provided" if empty</returns>
        public string GetFormattedAddress()
        {
            if (string.IsNullOrWhiteSpace(StreetNumber) && string.IsNullOrWhiteSpace(Street) && 
                string.IsNullOrWhiteSpace(City) && string.IsNullOrWhiteSpace(State))
                return "Not provided";
            
            return $"{StreetNumber} {Street}, {City}, {State}".Trim(' ', ',');
        }

        /// <summary>
        /// Virtual method for summary output (can be overridden by derived classes)
        /// </summary>
        /// <returns>Basic user summary string</returns>
        public virtual string Summary()
        {
            return $"{Id} | {Name}";
        }

        /// <summary>
        /// Override ToString to return user summary
        /// </summary>
        /// <returns>User summary string</returns>
        public override string ToString()
        {
            return Summary();
        }
    }
}
