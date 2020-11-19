﻿using System;
namespace UnikBolig.Models
{
    public class UserModel
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public TokenModel Token { get; set; }
    }
}
