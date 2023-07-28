using System.Diagnostics;
using System.Security.Claims;
using TiVi.AuthService.DataAccess;
using TiVi.AuthService.Utilities;

namespace TiVi.AuthService.Services
{
    public class BaseService
    {
        public int CurrentUserId
        {
            get
            {
                return ClaimIdentity.CurrentUserId;
            }
        }

        public string CurrentUserName
        {
            get
            {
                return ClaimIdentity.CurrentUserName;
            }
        }

        public string CurrentEmail
        {
            get
            {
                return ClaimIdentity.CurrentEmail;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return ClaimIdentity.IsAdmin;
            }
        }

        public string Password
        {
            get
            {
                return ClaimIdentity.Password;
            }
        }

        protected void SetAuditFieldsInsert(dynamic entity)
        {
            EntityHelper.SetAuditFieldsForInsert(entity, ClaimIdentity.CurrentUserName);
        }

        protected void SetAuditFieldsUpdate(dynamic entity)
        {
            EntityHelper.SetAuditFieldsForUpdate(entity, ClaimIdentity.CurrentUserName);
        }

        protected void SetAuditFieldsDelete(dynamic entity)
        {
            EntityHelper.SetAuditFieldsForDelete(entity, ClaimIdentity.CurrentUserName);
        }
    }
}
