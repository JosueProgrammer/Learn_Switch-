namespace Domain.Endpoint.Entities
{
    public class User:BaseEntity
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
