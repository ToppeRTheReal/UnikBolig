using System;
using UnikBolig.Models;

namespace UnikBolig.Api.Requests
{
    public class UpdateEstateRequest
    {
        public Guid EstateID { get; set; }
        public EstateModel Estate { get; set; }
        public Guid UserID { get; set; }
        public string Token { get; set; }
    }
}
