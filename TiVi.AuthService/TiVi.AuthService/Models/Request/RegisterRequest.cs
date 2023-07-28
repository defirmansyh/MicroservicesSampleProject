namespace TiVi.AuthService.Models.Request
{
    public class RegisterRequest
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        //public bool IsAdmin { get; set; }
    }
}
