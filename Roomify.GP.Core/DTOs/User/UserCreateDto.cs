namespace Roomify.GP.Core.DTOs.User
{
    public class UserCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
    }
}
