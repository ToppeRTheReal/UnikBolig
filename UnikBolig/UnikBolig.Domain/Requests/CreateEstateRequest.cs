using System;
namespace UnikBolig.Models.Requests
{
    public class CreateEstateRequest
    {
        public string Token { get; set; }
        public EstateModel Estate { get; set; }
    }
}
