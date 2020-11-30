using System;
namespace UnikBolig.Models.Requests
{
    public class UserDetailsRequest
    {
        public string Token { get; set; }
        public UserDetailModel Details { get; set; }
    }
}
