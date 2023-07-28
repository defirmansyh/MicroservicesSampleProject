namespace TiVi.AuthService.Models.Request
{
    public class AuthenticationRequest
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
