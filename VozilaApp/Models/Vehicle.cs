using System;

namespace SQLVehicles.Models
{
    public class Vehicle
    {
        public Guid ID { get; set; }
        public string Registration { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public Guid TypeOfVehicleID { get; set; }
        public Guid BrandOfVehicleID { get; set; }
        public Guid DriverID { get; set; }

        public Vehicle(Guid id, string registration, string name, string color, Guid typeOfVehicleID, Guid brandOfVehicleID, Guid driverID)
        {
            ID = id;
            Registration = registration;
            Name = name;
            Color = color;
            TypeOfVehicleID = typeOfVehicleID;
            BrandOfVehicleID = brandOfVehicleID;
            DriverID = driverID;
        }
    }
}
