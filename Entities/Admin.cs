namespace DotnetHospital.Entities
{
    // Admin inherits from User
    public sealed class Admin : User
    {
        public Admin(string name, string password, 
                    string email = "", string phone = "", string streetNumber = "", string street = "", 
                    string city = "", string state = "", int? id = null)
            : base(name, password, email, phone, streetNumber, street, city, state, id) { }
    }
}
