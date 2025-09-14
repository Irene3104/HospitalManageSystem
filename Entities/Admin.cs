namespace DotnetHospital.Entities
{
    // Admin inherits from User
    public sealed class Admin : User
    {
        public Admin(string name, string password, int? id = null)
            : base(name, password, id) { }
    }
}
