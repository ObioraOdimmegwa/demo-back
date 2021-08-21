using System;
using System.ComponentModel.DataAnnotations;

namespace Server
{
    public class ValidationToken
    {
        public string Token { get; set; }
        [Key]
        public string UserId { get; set; }
        public string Purpose { get; set; }
        public DateTime ExpireAt { get; set; }
    }

    public class TokenPurpose
    {
        public const string PasswordReset = "PasswordReset";
        public const string TwoFactorAuth = "2FactorAuth";
    }
}