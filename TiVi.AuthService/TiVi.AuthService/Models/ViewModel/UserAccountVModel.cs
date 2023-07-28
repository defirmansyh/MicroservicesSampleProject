namespace TiVi.AuthService.Models.ViewModel
{
    public class UserAccountVModel
    {
        public int UserAccountId { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        public bool IsAdmin { get; set; }
    }
}
