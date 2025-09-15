namespace DotnetHospital.Entities
{
    /// <summary>
    /// Admin entity class inheriting from User
    /// Represents an administrator in the hospital management system
    /// </summary>
    public sealed class Admin : User
    {
        /// <summary>
        /// Constructor for Admin class
        /// </summary>
        /// <param name="name">Admin's full name</param>
        /// <param name="password">Admin's password</param>
        /// <param name="email">Admin's email address</param>
        /// <param name="phone">Admin's phone number</param>
        /// <param name="streetNumber">Street number</param>
        /// <param name="street">Street name</param>
        /// <param name="city">City name</param>
        /// <param name="state">State/Province name</param>
        /// <param name="id">Optional admin ID (auto-generated if not provided)</param>
        public Admin(string name, string password, 
                    string email = "", string phone = "", string streetNumber = "", string street = "", 
                    string city = "", string state = "", int? id = null)
            : base(name, password, email, phone, streetNumber, street, city, state, id) { }

        /// <summary>
        /// Static factory method for creating new admins with auto-generated ID and password
        /// </summary>
        /// <param name="name">Admin's full name</param>
        /// <param name="email">Admin's email address</param>
        /// <param name="phone">Admin's phone number</param>
        /// <param name="streetNumber">Street number</param>
        /// <param name="street">Street name</param>
        /// <param name="city">City name</param>
        /// <param name="state">State/Province name</param>
        /// <param name="existingAdmins">Collection of existing admins for ID generation</param>
        /// <returns>New Admin instance with auto-generated ID and password</returns>
        public static Admin CreateNew(string name, 
                                    string email = "", string phone = "", string streetNumber = "", string street = "", 
                                    string city = "", string state = "", 
                                    System.Collections.Generic.IEnumerable<Admin> existingAdmins = null)
        {
            var newId = Services.IdGenerator.NewAdminId(existingAdmins);
            var password = Services.IdGenerator.GeneratePassword(newId);
            return new Admin(name, password, email, phone, streetNumber, street, city, state, newId);
        }
    }
}
