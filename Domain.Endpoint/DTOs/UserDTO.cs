namespace Domain.Endpoint.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SchoolName { get; set; }
        public string GradeLevel { get; set; }
        public string Gender { get; set; }
    }
}
