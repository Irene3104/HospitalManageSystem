namespace DotnetHospital.Entities
{
    // Abstract base class for all system users
    public abstract class User
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Password { get; set; }

        protected User(string name, string password, int? id = null)
        {
            // Generate unique ID if not provided
            Id = id ?? Services.IdGenerator.NewId();
            Name = name;
            Password = password;
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
