using System;
using UnikBolig.Models;

namespace UnikBolig.API.Requests
{
    public class CreateEstateRequest
    {
        public string Token { get; set; }
        public EstateModel Estate { get; set; }
    }
}
