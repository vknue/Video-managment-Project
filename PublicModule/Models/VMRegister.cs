using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PublicModule.Models
{
    public class VMRegister
    {
        [DisplayName("Username")]
        public string Username { get; set; } = null;
        [DisplayName("E-mail")]
        public string Email { get; set; } = null;
        [DisplayName("Confirm e-mail")]
        [Compare("Email")]
        public string Email2 { get; set; } = null;
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null; 
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null;
        [DisplayName("Password")]
        public string Password { get; set; } = null;
        [DisplayName("Confirm Password")]
        [Compare("Password")]
        public string Password2 { get; set; }
        [DisplayName("Country of Residence")]
        public string CountryOfResidence { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
