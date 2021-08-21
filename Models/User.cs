using Microsoft.AspNetCore.Identity;

namespace Server
{
    public class User : IdentityUser
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string RefCode { get; set; }
        public string DisplayName { get; set; }
        public string RecoveryPhrase { get; set; }
        public bool HasCompletedRegistration => !string.IsNullOrEmpty(this.DisplayName);
    }
}