using System;
using System.ComponentModel.DataAnnotations;

namespace UnikBolig.Models
{
    public class TokenModel : IModel
    {
        public Guid ID { get; set; }
        public string Token { get; set; }
        public Guid UserID { get; set; }
        [Timestamp()]
        public byte[] RowVersion { get; set; }
    }
}
