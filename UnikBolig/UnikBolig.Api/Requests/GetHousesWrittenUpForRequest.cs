using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnikBolig.Api.Requests
{
    public class GetHousesWrittenUpForRequest
    {
        public Guid UserID { get; set; }
        public string Token { get; set; }
    }
}
