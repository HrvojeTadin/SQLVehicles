using System;

namespace SQLVehicles.Models
{
    public class TypeOfVehicle
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public TypeOfVehicle(Guid id, string code, string name)
        {
            ID = id;
            Code = code;
            Name = name;
        }
    }
}
