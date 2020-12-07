using System;
using UnikBolig.Models;

namespace UnikBolig.API.Requests
{
    public class UpdateUserTypeRequest
    {
        public Guid UserID { get; set; }
        public string UserToken { get; set; }
        public Guid AdminID { get; set; }
        public string AdminToken { get; set; }
        public string NewType { get; set; }
    }
}
