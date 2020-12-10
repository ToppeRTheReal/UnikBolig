using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnikBolig.Models;

namespace UnikBolig.Api.Requests
{
    public class AddToWaitingListRequest
    {
        public WaitingList list { get; set; }
        public string Token { get; set; }
    }
}
