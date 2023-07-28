using System.ComponentModel.DataAnnotations;

namespace TiVi.AuthService.Models.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        public string? password { get; set; } = null!;
        [Required]
        public string? new_password { get; set; } = null!;
        [Required]
        public string? retype_password { get; set; } = null!;
    }
}
