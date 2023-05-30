using Microsoft.Build.Framework;
using System.ComponentModel;

namespace PublicModule.Models
{
    public class VMLogIn
    {
        [Required]
        [DisplayName("username")]
        public string Username { get; set; }
        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Stay Signed-id")]
        public bool StaySignedIn { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
