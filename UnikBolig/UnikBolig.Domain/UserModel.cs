using System;
namespace UnikBolig.Models
{
    public class UserModel : IModel
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
    }
}
