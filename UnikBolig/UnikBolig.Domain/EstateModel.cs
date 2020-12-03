using System;
namespace UnikBolig.Models
{
    public class EstateModel : IModel
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid RulesetID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
    }
}
