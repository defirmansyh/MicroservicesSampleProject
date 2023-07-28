using AutoMapper;
using ECommerceATK.ViewException;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiVi.AuthService.DataAccess;
using TiVi.AuthService.Models.Request;
using TiVi.AuthService.Models.Response;
using TiVi.AuthService.Models.ViewModel;
using TiVi.AuthService.Services.Interfaces;
using TiVi.AuthService.Utilities;

namespace TiVi.AuthService.Services
{
    public class UserAccountService : BaseService, IUserAccountService
    {
        private readonly TiViContext _context;
        private readonly IMapper _mapper;
        public UserAccountService(TiViContext tiViContext, IMapper mapper)
        {
            _context = tiViContext;
            _mapper = mapper;
        }

        public void ChangePassword(ChangePasswordRequest passwordRequest)
        {
            var userAccount = _context.UserAccounts.FirstOrDefault(f => f.Username == ClaimIdentity.CurrentUserName && f.IsActive.Equals(true));
            if (userAccount != null)
            {
                userAccount.Password = EncryptionHelper.Encrypt(passwordRequest.new_password);
                SetAuditFieldsUpdate(userAccount);
                _context.SaveChanges();
            }
            else
            {
                throw new DataNotExistException();
            }
        }

        public UserAccountVModel GetUserAccount(string username)
        {
            var user = _context.UserAccounts.SingleOrDefault(x => x.Username == username && x.IsActive.Equals(true));
            return _mapper.Map<UserAccountVModel>(user);
        }

        public bool IsUserValid(string username, string password)
        {
            var encryptedPassword = EncryptionHelper.Encrypt(password);
            return _context.UserAccounts.Any(
                a => a.Username== username 
                && a.Password == encryptedPassword
                && a.IsActive.Equals(true));
        }

        public void Register(RegisterRequest registerRequest)
        {
            var isUsernameExist = _context.UserAccounts.Any(a => a.Username == registerRequest.Username);
            if (!isUsernameExist)
            {
                var userAccount = _mapper.Map<UserAccount>(registerRequest);
                userAccount.IsAdmin = false;
                userAccount.Password = EncryptionHelper.Encrypt(registerRequest.Password);
                SetAuditFieldsInsert(userAccount);
                _context.Add(userAccount);
                _context.SaveChanges();
            }
            else
            {
                throw new DataAlreadyExistException();
            }
        }

        public void DeleteUser(DeactivateAccountRequest accountRequest)
        {
            var encryptedPassword = EncryptionHelper.Encrypt(accountRequest.Password);
            var userAccount = _context.UserAccounts.SingleOrDefault(s => s.Username == accountRequest.Username 
            && s.Password == encryptedPassword);
            if (userAccount != null)
            {
                SetAuditFieldsDelete(userAccount);
                _context.SaveChanges();
            }
            else
            {
                throw new DataNotExistException();
            }
        }
    }
}
