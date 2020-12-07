using System;
namespace UnikBolig.Models
{
    public class WaitingList
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid EstateID { get; set; }
    }
}
