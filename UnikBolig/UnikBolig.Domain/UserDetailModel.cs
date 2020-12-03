using System;
namespace UnikBolig.Models
{
    public class UserDetailModel : IModel
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string About { get; set; }
        public Boolean Dog { get; set; }
        public Boolean Cat { get; set; }
        public Boolean Creep { get; set; }
        public Boolean Fish { get; set; }
    }
}
