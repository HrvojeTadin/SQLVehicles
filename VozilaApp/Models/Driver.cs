using System;

namespace SQLVehicles.Models
{
    public class Driver
    {
        public Guid ID { get; set; }
        public string PIN { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public Driver(Guid id, string pin, string name, string lastName, DateTime birthDate)
        {
            ID = id;
            PIN = pin;
            Name = name;
            LastName = lastName;
            BirthDate = birthDate;
        }
    }
}
