using System;
using UnikBolig.Models;

namespace UnikBolig.API.Requests
{
    public class UserDetailsRequest
    {
        public string Token { get; set; }
        public UserDetailModel Details { get; set; }
    }
}
