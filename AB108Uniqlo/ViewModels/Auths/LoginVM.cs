﻿using System.ComponentModel.DataAnnotations;

namespace AB108Uniqlo.ViewModels.Auths
{
    public class LoginVM
    {
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
