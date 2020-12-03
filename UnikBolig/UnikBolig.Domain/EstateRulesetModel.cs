using System;
namespace UnikBolig.Models
{
    public class EstateRulesetModel
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public Boolean Dog { get; set; }
        public Boolean Cat { get; set; }
        public Boolean Creep { get; set; }
        public Boolean Fish { get; set; }
    }
}
