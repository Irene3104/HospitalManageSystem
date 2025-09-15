namespace DotnetHospital.Entities
{
    // Admin inherits from User
    public sealed class Admin : User
    {
        public Admin(string name, string password, 
                    string email = "", string phone = "", string streetNumber = "", string street = "", 
                    string city = "", string state = "", int? id = null)
            : base(name, password, email, phone, streetNumber, street, city, state, id) { }

        // Constructor for creating new admins with auto-generated ID and password
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
