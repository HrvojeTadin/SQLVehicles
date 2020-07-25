using System;

namespace SQLVehicles.Models
{
    public class BrandOfVehicle
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public BrandOfVehicle(Guid id, string code, string name)
        {
            ID = id;
            Code = code;
            Name = name;
        }
    }
}
