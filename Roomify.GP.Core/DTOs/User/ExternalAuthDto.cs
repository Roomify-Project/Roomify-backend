namespace Roomify.GP.Core.DTOs.User
{
    public class ExternalAuthDto
    {
        public string Provider { get; set; }  // Google
        public string IdToken { get; set; }   // Google Id Token
    }
}
