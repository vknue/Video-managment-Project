namespace IntegrationModule.VModels
{
    public class UserCreateRequest
    {
        //string username, string firstName, string lastName, string email, string password

        public string Username { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public string Email { get; set; } = null;
        public string Password { get; set; } = null;
        public string CountryOfResidence { get; set; }
        public string PhoneNumber { get; set; }
    }
}
