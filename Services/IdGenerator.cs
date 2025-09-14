using System;

namespace DotnetHospital.Services
{
    // Service for generating unique IDs (5~8 digit numbers)
    public static class IdGenerator
    {
        private static readonly Random Rng = new Random();

        public static int NewId()
        {
            return Rng.Next(10000, 99999999); // always between 5 and 8 digits
        }
    }
}
