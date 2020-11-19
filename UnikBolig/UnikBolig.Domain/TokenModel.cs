using System;
namespace UnikBolig.Models
{
    public class TokenModel
    {
        public Guid ID { get; set; }
        public string Token { get; set; }
        public Guid UserID { get; set; }
    }
}
