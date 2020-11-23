using System;
namespace UnikBolig.Api.Response
{
    public class UserResponse : AbstractResponse
    {
        public string Token { get; set; }
        public Guid UserID { get; set; }
    }
}
