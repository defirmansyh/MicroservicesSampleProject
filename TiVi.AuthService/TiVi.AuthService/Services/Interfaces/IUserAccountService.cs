using TiVi.AuthService.Models.Request;
using TiVi.AuthService.Models.Response;
using TiVi.AuthService.Models.ViewModel;

namespace TiVi.AuthService.Services.Interfaces
{
    public interface IUserAccountService
    {
        bool IsUserValid(string username, string password);
        UserAccountVModel GetUserAccount(string username);
        void Register(RegisterRequest registerRequest);
        void ChangePassword(ChangePasswordRequest passwordRequest);
        void DeleteUser(DeactivateAccountRequest accountRequest);
    }
}
